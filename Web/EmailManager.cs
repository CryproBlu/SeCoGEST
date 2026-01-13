using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using SeCoGEST.Helper;
using SeCoGEST.Infrastructure;
using System.IO;
using SeCoGes.Utilities;
using SeCoGEST.Entities;
using SeCoGEST.Logic;
using SeCoGEST.Web.Archivi;

namespace SeCoGEST.Web
{
    public static class EmailManager
    {
        #region Metodi Pubblici

        #region Generici

        private static string FirmaEmail()
        {
            string firma =  $"<strong>{ConfigurationKeys.DENOMINAZIONE_AZIENDA}</strong>{Environment.NewLine}";
            firma += ConfigurationKeys.DENOMINAZIONE_AZIENDA_ALTRO.Replace("\\n", Environment.NewLine);
            return firma;
        }

        /// <summary>
        /// Effettua l'invio di una email in base al valore dei parametri passati 
        /// </summary>
        /// <param name="mailSubject"></param>
        /// <param name="mailBody"></param>
        /// <param name="mittente"></param>
        /// <param name="destinatari"></param>
        /// <param name="priority"></param>
        /// <param name="deliveryNotificationOption"></param>
        /// <param name="attachments"></param>
        /// <remarks></remarks>
        public static void InviaEmail(string mailSubject, string mailBody, string mittente, string[] destinatari, MailPriority priority = MailPriority.Normal, DeliveryNotificationOptions? deliveryNotificationOption = null, System.IO.FileInfo[] attachments = null)
        {
            if (String.IsNullOrWhiteSpace(mittente))
            {
                throw new ArgumentNullException("mittente", "Non è stato indicato nessun mittente");
            }
                        
            if (destinatari == null)
            {
                destinatari = new string[]{};
            }

            IEnumerable<string> elencoDestinatari = destinatari.Where(x => !String.IsNullOrWhiteSpace(x));
            if (elencoDestinatari == null || elencoDestinatari.Count() <= 0)
            {
                throw new ArgumentNullException("elencoDestinatari", "Non è stato indicato nessun destinatario");
            }

            if (String.IsNullOrEmpty(mailSubject))
            {
                mailSubject = string.Empty;
            }

            if (String.IsNullOrEmpty(mailBody))
            {
                mailBody = string.Empty;
            }

            foreach (string destinatario in elencoDestinatari)
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        try
                        {
                            mailMessage.From = new MailAddress(mittente.Trim());
                            mailMessage.To.Add(new MailAddress(destinatario.Trim()));

                            mailMessage.Subject = String.Join(" - ", ConfigurationKeys.TITOLO_APPLICAZIONE, mailSubject);
                            mailMessage.Body = mailBody;
                            mailMessage.IsBodyHtml = true;
                            mailMessage.Priority = priority;

                            if (deliveryNotificationOption != null)
                            {
                                mailMessage.DeliveryNotificationOptions = deliveryNotificationOption.Value;
                            }

                            if (attachments != null && attachments.Count() > 0)
                            {
                                AggiungiAllegati(mailMessage, attachments);
                            }

                            smtp.Send(mailMessage);

                            mailMessage.Dispose();
                        }
                        catch (Exception) { }
                    }
                }
            }            
        }

        #endregion

        #region Specifici

        /// <summary>
        /// Effettua l'invio dei un'email tecnica di errore
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="lastError"></param>
        /// <param name="requestUrlString"></param>
        /// <param name="requestQueryString"></param>
        /// <remarks></remarks>
        public static void InviaEmailAvvisoErrore(string userName, Exception lastError, string requestUrlString, string requestQueryString)
        {
            if (String.IsNullOrEmpty(userName))
            {
                userName = string.Empty;
            }

            if (String.IsNullOrEmpty(requestUrlString))
            {
                requestUrlString = String.Empty;
            }

            if (String.IsNullOrEmpty(requestQueryString))
            {
                requestQueryString = String.Empty;
            }

            string lastErrorMessageString = String.Empty;
            if (lastError != null)
            {
                lastErrorMessageString = lastError.Message;
            }

            string lastErrorString = String.Empty;
            if (lastError != null)
            {
                lastErrorString = lastError.ToString();
            }

            string strBody = String.Empty;

            if (String.IsNullOrEmpty(requestUrlString) && String.IsNullOrEmpty(requestQueryString))
            {
                strBody = string.Format("Data/Ora: {1}{0}{0}Utente: {2}{0}{0}Client IP: {3}{0}{0}Errore: {0}<strong>{4}</strong>{0}{5}{0}", Helper.HtmlEnvironment.NewLine, DateTime.Now, userName, Helper.Web.GetClientIpAddress(), lastErrorMessageString, lastErrorString);
            }
            else
            {
                strBody = string.Format("Data/Ora: {1}{0}{0}Utente: {2}{0}{0}Client IP: {3}{0}{0}Errore: {0}<strong>{4}</strong>{0}{5}{0}{0}Request URL: {6}{0}{0}Request.QueryString: {7}", Helper.HtmlEnvironment.NewLine, DateTime.Now, userName, Helper.Web.GetClientIpAddress(), lastErrorMessageString, lastErrorString, requestUrlString, requestQueryString);
            }

            string[] destinatari = ConfigurationKeys.DESTINATARI_EMAIL_TECNICHE_APPLICAZIONE;

            InviaEmail("Errore di esecuzione", strBody, ConfigurationKeys.MITTENTE_EMAIL_TECNICHE_APPLICAZIONE, destinatari, MailPriority.High);
        }

        /// <summary>
        /// Effettua l'invio dell'email relativa alla chiusura di un intervento
        /// </summary>
        /// <param name="interventoChiuso"></param>
        /// <param name="userNameUtenteSalvataggio"></param>
        public static void InviaEmailChiusuraIntervento(Entities.Intervento interventoChiuso, string userNameUtenteSalvataggio)
        {
            if (interventoChiuso == null)
            {
                throw new ArgumentNullException("interventoChiuso", "Paramentro nullo");
            }

            if (String.IsNullOrEmpty(userNameUtenteSalvataggio))
            {
                throw new ArgumentNullException("userNameUtenteSalvataggio", "Paramentro nullo");
            }

            StringBuilder corpoMessaggio = new StringBuilder();
            corpoMessaggio.AppendLine(String.Format("<strong>Informazioni sull'intervento</strong>{0}", HtmlEnvironment.NewLine));
            corpoMessaggio.AppendLine(String.Format("<hr style=\"height:1px;\"/>{0}", HtmlEnvironment.NewLine));
            corpoMessaggio.AppendLine(String.Format("Chiusura Bollettino numero: {1} dell'azienda: {2}{0}", HtmlEnvironment.NewLine, interventoChiuso.Numero, ConfigurationKeys.DENOMINAZIONE_AZIENDA));
            corpoMessaggio.AppendLine(String.Format("Cliente: {1}{0}", HtmlEnvironment.NewLine, interventoChiuso.RagioneSociale));
            corpoMessaggio.AppendLine(String.Format("Salvato dall'utente: {1}{0}", HtmlEnvironment.NewLine, userNameUtenteSalvataggio));
            corpoMessaggio.AppendLine(String.Format("Data Intervento: {1:dd/MM/yyyy HH:mm}{0}", HtmlEnvironment.NewLine, interventoChiuso.DataPrevistaIntervento));
            corpoMessaggio.AppendLine(String.Format("Oggetto: {1}{0}", HtmlEnvironment.NewLine, interventoChiuso.Oggetto));
            corpoMessaggio.AppendLine(String.Format("Descrizione dell'intervento: {1}{0}{0}{0}", HtmlEnvironment.NewLine, interventoChiuso.Definizione));

            string tipologiaIntervento = string.Empty;
            if (interventoChiuso.IdTipologia.HasValue)
            {
                tipologiaIntervento = interventoChiuso.TipologiaIntervento.Nome;
            }
            corpoMessaggio.AppendLine(String.Format("Tipologia Intervento: {1}{0}", HtmlEnvironment.NewLine, tipologiaIntervento));
            //corpoMessaggio.AppendLine(String.Format("Tipologia Intervento: {1}{0}", HtmlEnvironment.NewLine, interventoChiuso.TipologiaString));


            corpoMessaggio.AppendLine(String.Format("<strong>Informazioni sugli operatori</strong>{0}", HtmlEnvironment.NewLine));
            corpoMessaggio.AppendLine(String.Format("<hr style=\"height:1px;\"/>{0}", HtmlEnvironment.NewLine));

            Logic.Intervento_Operatori llIntervento_Operatori = new Logic.Intervento_Operatori();
            IQueryable<Entities.Intervento_Operatore> elencoInterventoOperatori = llIntervento_Operatori.Read(interventoChiuso);

            if (elencoInterventoOperatori != null && elencoInterventoOperatori.Count() > 0)
            {
                corpoMessaggio.AppendLine("<table border=\"1\" cellspacing=\"0\" cellpadding=\"5\">");
                corpoMessaggio.AppendLine("<thead>");
                corpoMessaggio.AppendLine("<tr style=\"background-color:#DDD; font-weight:bold\">");
                corpoMessaggio.AppendLine("<td>Operatore</td>");
                corpoMessaggio.AppendLine("<td align=\"center\">Modalità Risoluzione</td>");
                corpoMessaggio.AppendLine("<td align=\"center\">Inizio Intervento</td>");
                corpoMessaggio.AppendLine("<td align=\"center\">Fine Intervento</td>");
                corpoMessaggio.AppendLine("<td align=\"center\">Durata Minuti</td>");
                corpoMessaggio.AppendLine("</tr>");
                corpoMessaggio.AppendLine("</thead>");
                corpoMessaggio.AppendLine("<tbody>");


                foreach(Entities.Intervento_Operatore intervento_operatore in elencoInterventoOperatori)
                {
                    corpoMessaggio.AppendLine("<tr>");
                    corpoMessaggio.AppendLine(String.Format("<td>{0}</td>", intervento_operatore.CognomeNomeOperatore));
                    corpoMessaggio.AppendLine(String.Format("<td align=\"center\">{0}</td>", intervento_operatore.DescrizioneModalitaRisoluzioneIntervento));
                    corpoMessaggio.AppendLine(String.Format("<td align=\"center\">{0:dd/MM/yyyy HH:mm}</td>", intervento_operatore.InizioIntervento));
                    corpoMessaggio.AppendLine(String.Format("<td align=\"center\">{0:dd/MM/yyyy HH:mm}</td>", intervento_operatore.FineIntervento));
                    corpoMessaggio.AppendLine(String.Format("<td align=\"center\">{0}</td>", intervento_operatore.DurataMinuti));
                    corpoMessaggio.AppendLine("</tr>");
                }

                corpoMessaggio.AppendLine("</tbody>");
                corpoMessaggio.AppendLine("</table>");
            }

            string urlIntervento = String.Format("{0}Interventi/Intervento.aspx?ID={1}", ConfigurationKeys.URL_APPLICAZIONE, interventoChiuso.ID);
            corpoMessaggio.AppendLine(String.Format("{0}<a href=\"{1}\">Visualizza l'Intervento</a>", HtmlEnvironment.NewLine, urlIntervento));

            string corpoMessaggioString = corpoMessaggio.ToString();
            string[] destinatari = ConfigurationKeys.DESTINATARI_EMAIL_CHIUSURA_INTERVENTO;
            string oggetto = String.Format("Invio automatizzato Email per chiusura Intervento Tecnico numero \"{0}\" dell'azienda \"{1}\"", interventoChiuso.Numero, ConfigurationKeys.DENOMINAZIONE_AZIENDA);

            InviaEmail(oggetto, corpoMessaggioString, ConfigurationKeys.MITTENTE_EMAIL_CHIUSURA_INTERVENTO, destinatari, MailPriority.Normal);
        }

        /// <summary>
        /// Effettua l'invio della email che contiene l'elenco degli interventi generati
        /// </summary>
        /// <param name="elencoInterventiGenerati"></param>
        /// <param name="elencoIdentificativiInterventiNonGenerati"></param>
        /// <param name="entityAccount"></param>
        public static void InviaEmailInterventiGenerati(List<FileInfo> elencoInterventiGenerati, List<Entities.EntityId<Entities.Intervento>> elencoIdentificativiInterventiNonGenerati, Entities.Account entityAccount)
        {
            if (elencoInterventiGenerati == null)
            {
                throw new ArgumentNullException("elencoInterventiGenerati", "Parametro nullo");
            }

            if (elencoIdentificativiInterventiNonGenerati == null)
            {
                throw new ArgumentNullException("elencoIdentificativiInterventiNonGenerati", "Parametro nullo");
            }

            if (entityAccount == null)
            {
                throw new ArgumentNullException("entityAccount", "Parametro nullo");
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("Gentile {0},{1}", entityAccount.Nominativo, Helper.Web.HTML_NEW_LINE));
            sb.AppendLine(String.Format("ecco l'elenco degli interventi selezionati.{0}{0}", Helper.Web.HTML_NEW_LINE));
            sb.AppendLine(String.Format("Di seguito troverai tutte le informazioni relative alla generazione degli interventi richiesti.{0}{0}", Helper.Web.HTML_NEW_LINE));

            sb.AppendLine(String.Format("<strong>Interventi generati (pdf in allegato)</strong>{0}", Helper.Web.HTML_NEW_LINE));

            foreach (FileInfo filegenerato in elencoInterventiGenerati)
            {
                sb.AppendLine(String.Format("{1}{0}", Helper.Web.HTML_NEW_LINE, filegenerato.Name));
            }

            if (elencoIdentificativiInterventiNonGenerati != null && elencoIdentificativiInterventiNonGenerati.Count > 0)
            {
                sb.AppendLine(String.Format("{0}<strong>Identificativi degli interventi non generati a causa di un errore</strong>{0}", Helper.Web.HTML_NEW_LINE));

                foreach (Entities.EntityId<Entities.Intervento> identificativo in elencoIdentificativiInterventiNonGenerati)
                {
                    sb.AppendLine(String.Format("{1}{0}", Helper.Web.HTML_NEW_LINE, identificativo.Value));
                }
            }

            sb.AppendLine(String.Format("{0}Cordiali saluti.{1}", Helper.Web.HTML_NEW_LINE, ConfigurationKeys.DENOMINAZIONE_AZIENDA_ALTRO.Replace("\\n", Helper.Web.HTML_NEW_LINE)));

            string corpoMessaggio = sb.ToString();

            InviaEmail("Elenco Interventi Richiesti", corpoMessaggio, Infrastructure.ConfigurationKeys.MITTENTE_EMAIL_CHIUSURA_INTERVENTO, new string[] { entityAccount.Email }, attachments: elencoInterventiGenerati.ToArray());
        }

        /// <summary>
        /// Effettua l'invio dell'email di assegnazione dell'intervento a tutti gli indirizzi email degli account associati agli operatori passati come parametro
        /// </summary>
        /// <param name="intervento"></param>
        /// <param name="elencoOperatoriIntervento"></param>
        public static void InviaEmailAssegnazioneIntervento(Entities.Intervento intervento, IEnumerable<Entities.Operatore> elencoOperatoriIntervento)
        {
            if (intervento == null)
            {
                throw new ArgumentNullException("intervento", "Parametro nullo");
            }
            
            // Nel caso in cui l'elenco degli operatori passato non sia valorizzato oppure non contenga alcun record,
            // viene annullata l'operazione di invio dell'email uscendo dalla funzione
            if (elencoOperatoriIntervento == null || elencoOperatoriIntervento.Count() <= 0) return;

            string urlIntervento = String.Format("{0}Interventi/Intervento.aspx?ID={1}", ConfigurationKeys.URL_APPLICAZIONE, intervento.ID);
            
            IEnumerable<Guid> elencoIdentificativiOperatoriAssegnati = elencoOperatoriIntervento.Select(x => x.ID).Distinct();

            Logic.Operatori llOperatori = new Logic.Operatori();

            foreach (Guid identificativoOperatore in elencoIdentificativiOperatoriAssegnati)
            {
                Entities.Operatore operatore = llOperatori.Find(new Entities.EntityId<Entities.Operatore>(identificativoOperatore));
                if (operatore != null)
                {
                    List<string> elencoEmail = llOperatori.GetValidEmailsListFromOperatore(operatore);
                    if (elencoEmail != null && elencoEmail.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(String.Format("Gentile {0},{1}", operatore.CognomeNome, Helper.Web.HTML_NEW_LINE));
                        sb.AppendLine(String.Format("questa email è stata generata in automatico.{0}", Helper.Web.HTML_NEW_LINE));
                        sb.AppendLine(String.Format("Clicca sul link sottostante per accedere alla pagina di dettaglio dell'intervento N.{1}{0}{0}", Helper.Web.HTML_NEW_LINE, intervento.Numero));

                        sb.AppendLine(String.Format("Ragione Sociale: {1}{0}", HtmlEnvironment.NewLine, intervento.RagioneSociale));
                        sb.AppendLine(String.Format("Referente Chiamata: {1}{0}", HtmlEnvironment.NewLine,intervento.ReferenteChiamata));
                        sb.AppendLine(String.Format("Oggetto: {1}{0}", HtmlEnvironment.NewLine, intervento.Oggetto));
                        sb.AppendLine(String.Format("Numero Telefono: {1}{0}{0}", HtmlEnvironment.NewLine, intervento.Telefono));

                        sb.AppendLine(String.Format("Link Al Gestionale: <a href=\"{1}\">Intervento N.{2} - {3}</a>{0}{0}", HtmlEnvironment.NewLine, urlIntervento, intervento.Numero, intervento.RagioneSociale));

                        if (intervento.Urgente.HasValue && intervento.Urgente.Value)
                        {
                            sb.AppendLine(String.Format("<span style=\"color:red;\"><strong>URGENTE</strong></span>{0}", HtmlEnvironment.NewLine));
                        }

                        List<string> elencoTipologieTicket = new List<string>();
                        if (intervento.Chiamata.HasValue && intervento.Chiamata.Value)
                        {
                            elencoTipologieTicket.Add("Chiamata");
                        }

                        if (intervento.Interno.HasValue && intervento.Interno.Value)
                        {
                            elencoTipologieTicket.Add("Intervento Interno");
                        }
                        else
                        {
                            elencoTipologieTicket.Add("Intervento Esterno");
                        }

                        //sb.AppendLine(String.Format("Ticket di tipo: {1}{0}{0}", HtmlEnvironment.NewLine, String.Join(" / ", elencoTipologieTicket)));

                        sb.AppendLine("Cordiali saluti." + HtmlEnvironment.NewLine);

                        string corpoMessaggio = sb.ToString();
                        string ogetto = String.Format("Assegnazione Intervento N.{0}", intervento.Numero);

                        foreach(string email in elencoEmail)
                        {                            
                            InviaEmail(ogetto, corpoMessaggio, ConfigurationKeys.MITTENTE_EMAIL_NOTIFICA_INTERVENTO, new string[] { email }, MailPriority.Normal);
                        }
                    }
                }
            }
            
        }

        public static void InviaEmailAssegnazioneAttivitaProgetto(Entities.Progetto_Attivita attivita, Entities.Operatore operatore, IEnumerable<string> ruoli)
        {
            if (attivita == null)
            {
                throw new ArgumentNullException("attivita", "Parametro nullo");
            }

            if (operatore == null)
            {
                throw new ArgumentNullException("operatore", "Parametro nullo");
            }

            Logic.Operatori llOperatori = new Logic.Operatori();
            List<string> elencoEmail = llOperatori.GetValidEmailsListFromOperatore(operatore);
            if (elencoEmail == null || elencoEmail.Count <= 0)
            {
                return;
            }

            List<string> ruoliDistinct = ruoli?
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList() ?? new List<string>();

            Entities.Progetto progetto = attivita.Progetto;
            if (progetto == null)
            {
                Logic.Progetti llProgetti = new Logic.Progetti();
                progetto = llProgetti.Find(new EntityId<Entities.Progetto>(attivita.IDProgetto));
            }

            string denominazioneProgetto = progetto != null ? progetto.DenominazioneCliente : string.Empty;
            string descrizioneAttivita = attivita.Descrizione ?? string.Empty;
            string urlAttivita = String.Format("{0}Progetti/DettagliProgetto.aspx?ID={1}&IDAttivita={2}", ConfigurationKeys.URL_APPLICAZIONE, attivita.IDProgetto, attivita.ID);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("Gentile {0},{1}", operatore.CognomeNome, HtmlEnvironment.NewLine));
            sb.AppendLine(String.Format("questa email è stata generata in automatico.{0}", HtmlEnvironment.NewLine));

            if (ruoliDistinct.Count > 0)
            {
                sb.AppendLine(String.Format("Sei stato indicato come {1} per l'attività di progetto.{0}", HtmlEnvironment.NewLine, String.Join(", ", ruoliDistinct)));
            }
            else
            {
                sb.AppendLine(String.Format("Sei stato indicato per l'attività di progetto.{0}", HtmlEnvironment.NewLine));
            }

            if (!string.IsNullOrWhiteSpace(denominazioneProgetto))
            {
                sb.AppendLine(String.Format("Progetto: {1}{0}", HtmlEnvironment.NewLine, denominazioneProgetto));
            }

            if (!string.IsNullOrWhiteSpace(descrizioneAttivita))
            {
                sb.AppendLine(String.Format("Attività: {1}{0}", HtmlEnvironment.NewLine, descrizioneAttivita));
            }

            sb.AppendLine(String.Format("{0}<a href=\"{1}\">Apri attività nel gestionale</a>{0}", HtmlEnvironment.NewLine, urlAttivita));
            sb.AppendLine(String.Format("{0}Cordiali saluti.{1}{2}", HtmlEnvironment.NewLine, HtmlEnvironment.NewLine, FirmaEmail()));

            string oggetto = "Assegnazione Attività Progetto";

            foreach (string email in elencoEmail)
            {
                InviaEmail(oggetto, sb.ToString(), ConfigurationKeys.MITTENTE_EMAIL_NOTIFICA_INTERVENTO, new string[] { email }, MailPriority.Normal);
            }
        }

        /// <summary>
        /// Effettua l'invio dell'email contenente tutti gli interventi, raggruppati per operatore, che non risultano chiusi e validati e che sono stati presi in carico
        /// </summary>
        /// <param name="raggruppamentoInterventiPerOperatore"></param>
        public static void InviaEmailInterventiAssegnatiNonChiusiNonValidati(List<IGrouping<Entities.Operatore, Guid>> raggruppamentoInterventiPerOperatore)
        {
            if (raggruppamentoInterventiPerOperatore == null || raggruppamentoInterventiPerOperatore.Count <= 0) return;
            
            Logic.Interventi llInterventi = new Logic.Interventi();
            Logic.Operatori llOperatori = new Logic.Operatori();

            foreach (IGrouping<Entities.Operatore, Guid> raggruppamento in raggruppamentoInterventiPerOperatore)
            {
                Entities.Operatore operatore = raggruppamento.Key;
                IEnumerable<Guid> elencoUnivociInterventi = raggruppamento.Distinct();

                if (elencoUnivociInterventi != null && elencoUnivociInterventi.Count() > 0)
                {
                    List<string> elencoEmail = llOperatori.GetValidEmailsListFromOperatore(operatore);
                    if (elencoEmail != null && elencoEmail.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(String.Format("Gentile {0},{1}", operatore.CognomeNome, Helper.Web.HTML_NEW_LINE));
                        sb.AppendLine(String.Format("questa email è stata generata in automatico per renderti al corrente dei bollettini a te assegnati ma non chiusi.{0}", Helper.Web.HTML_NEW_LINE));
                        sb.AppendLine(String.Format("Clicca sul link sottostante per accedere alla pagina di dettaglio del singolo intervento.{0}{0}", Helper.Web.HTML_NEW_LINE));

                        foreach (Guid idIntervento in elencoUnivociInterventi)
                        {
                            Entities.Intervento entityIntervento = llInterventi.Find(idIntervento);
                            if (entityIntervento != null)
                            {
                                string urlIntervento = String.Format("{0}Interventi/Intervento.aspx?ID={1}", Infrastructure.ConfigurationKeys.URL_APPLICAZIONE, idIntervento);
                                sb.AppendLine(String.Format("<a href=\"{1}\">Intervento N.{2} - {3}</a>{0}{0}", HtmlEnvironment.NewLine, urlIntervento, entityIntervento.Numero, entityIntervento.RagioneSociale));
                            }
                        }


                        //sb.AppendLine(String.Format("{0}Buona giornata.", Helper.Web.HTML_NEW_LINE));
                        sb.AppendLine(String.Format("{0}Cordiali saluti.{1}", Helper.Web.HTML_NEW_LINE, ConfigurationKeys.DENOMINAZIONE_AZIENDA_ALTRO.Replace("\\n", Helper.Web.HTML_NEW_LINE)));

                        string corpoMessaggio = sb.ToString();

                        foreach (string email in elencoEmail)
                        {                            
                            InviaEmail("Riepilogo Interventi Assegnati Da Chiudere", corpoMessaggio, ConfigurationKeys.MITTENTE_EMAIL_NOTIFICA_INTERVENTO, new string[] { email }, MailPriority.Normal);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Effettua l'invio dell'email contenente tutti gli interventi, raggruppati per operatore, che non risultano chiusi o validati e che non sono stati presi in carico
        /// </summary>
        /// <param name="raggruppamentoInterventiPerOperatore"></param>
        public static void InviaEmailInterventiAssegnatiNonPresiInCarico(List<IGrouping<Entities.Operatore, Guid>> raggruppamentoInterventiPerOperatore)
        {
            if (raggruppamentoInterventiPerOperatore == null || raggruppamentoInterventiPerOperatore.Count <= 0) return;

            Logic.Interventi llInterventi = new Logic.Interventi();
            Logic.Operatori llOperatori = new Logic.Operatori();

            foreach (IGrouping<Entities.Operatore, Guid> raggruppamento in raggruppamentoInterventiPerOperatore)
            {
                Entities.Operatore operatore = raggruppamento.Key;
                IEnumerable<Guid> elencoUnivociInterventi = raggruppamento.Distinct();

                if (elencoUnivociInterventi != null && elencoUnivociInterventi.Count() > 0)
                {
                    List<string> elencoEmail = llOperatori.GetValidEmailsListFromOperatore(operatore);
                    if (elencoEmail != null && elencoEmail.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(String.Format("Gentile {0},{1}", operatore.CognomeNome, Helper.Web.HTML_NEW_LINE));
                        sb.AppendLine(String.Format("questa email è stata generata in automatico per renderti al corrente dei bollettini a te assegnati ma non presi in carico.{0}", Helper.Web.HTML_NEW_LINE));
                        sb.AppendLine(String.Format("Clicca sul link sottostante per accedere alla pagina di dettaglio del singolo intervento.{0}{0}", Helper.Web.HTML_NEW_LINE));

                        foreach (Guid idIntervento in elencoUnivociInterventi)
                        {
                            Entities.Intervento entityIntervento = llInterventi.Find(idIntervento);
                            if (entityIntervento != null)
                            {
                                string urlIntervento = String.Format("{0}Interventi/Intervento.aspx?ID={1}", Infrastructure.ConfigurationKeys.URL_APPLICAZIONE, idIntervento);
                                sb.AppendLine(String.Format("<a href=\"{1}\">Intervento N.{2} - {3}</a>{0}{0}", HtmlEnvironment.NewLine, urlIntervento, entityIntervento.Numero, entityIntervento.RagioneSociale));
                            }
                        }

                        //sb.AppendLine(String.Format("{0}Buona giornata.", Helper.Web.HTML_NEW_LINE));
                        sb.AppendLine(String.Format("{0}Cordiali saluti.{1}", Helper.Web.HTML_NEW_LINE, ConfigurationKeys.DENOMINAZIONE_AZIENDA_ALTRO.Replace("\\n", Helper.Web.HTML_NEW_LINE)));

                        foreach (string email in elencoEmail)
                        {
                            string corpoMessaggio = sb.ToString();
                            InviaEmail("Riepilogo Interventi Assegnati Da Prendere In Carico", corpoMessaggio, ConfigurationKeys.MITTENTE_EMAIL_NOTIFICA_INTERVENTO, new string[] { email }, MailPriority.Normal);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Effettua l'invio dell'email di assegnazione dell'intervento a tutti gli indirizzi email degli account associati agli operatori passati come parametro
        /// </summary>
        /// <param name="accountCliente"></param>
        /// <param name="intervento"></param>
        public static void InviaEmailCreazioneInterventoDaParteDelCliente(Entities.Account accountCliente, Entities.Intervento intervento)
        {
            if (accountCliente == null)
            {
                throw new ArgumentNullException("accountCliente", "Parametro nullo");
            }

            if (intervento == null)
            {
                throw new ArgumentNullException("intervento", "Parametro nullo");
            }

            List<string> emails = Helper.EmailHelper.GetEmailsValidList(accountCliente.Email);
            if (emails != null && emails.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(String.Format("Gentile {0},{1}", accountCliente.Nominativo, Helper.Web.HTML_NEW_LINE));
                sb.AppendLine(String.Format("questa email è stata generata in automatico.{0}", Helper.Web.HTML_NEW_LINE));

                sb.AppendLine(String.Format("Clicca sul link sottostante per accedere alla pagina di dettaglio dell'intervento N.{1}{0}{0}", Helper.Web.HTML_NEW_LINE, intervento.Numero));

                sb.AppendLine(String.Format("Ragione Sociale: {1}{0}", HtmlEnvironment.NewLine, intervento.RagioneSociale));
                sb.AppendLine(String.Format("Referente Chiamata: {1}{0}", HtmlEnvironment.NewLine, intervento.ReferenteChiamata));
                sb.AppendLine(String.Format("Oggetto: {1}{0}", HtmlEnvironment.NewLine, intervento.Oggetto));
                sb.AppendLine(String.Format("Numero Telefono: {1}{0}{0}", HtmlEnvironment.NewLine, intervento.Telefono));

                string urlIntervento = String.Format("{0}Interventi/Ticket.aspx?ID={1}", ConfigurationKeys.URL_APPLICAZIONE, intervento.ID);
                sb.AppendLine(String.Format("Link Al Gestionale: <a href=\"{1}\">Intervento N.{2} - {3}</a>{0}{0}", HtmlEnvironment.NewLine, urlIntervento, intervento.Numero, intervento.RagioneSociale));

                if (intervento.Urgente.HasValue && intervento.Urgente.Value)
                {
                    sb.AppendLine(String.Format("<span style=\"color:red;\"><strong>URGENTE</strong></span>{0}", HtmlEnvironment.NewLine));
                }

                List<string> elencoTipologieTicket = new List<string>();
                if (intervento.Chiamata.HasValue && intervento.Chiamata.Value)
                {
                    elencoTipologieTicket.Add("Chiamata");
                }

                if (intervento.Interno.HasValue && intervento.Interno.Value)
                {
                    elencoTipologieTicket.Add("Intervento Interno");
                }
                else
                {
                    elencoTipologieTicket.Add("Intervento Esterno");
                }

                //sb.AppendLine(String.Format("Ticket di tipo: {1}{0}{0}", HtmlEnvironment.NewLine, String.Join(" / ", elencoTipologieTicket)));

                sb.AppendLine("Cordiali saluti." + HtmlEnvironment.NewLine + HtmlEnvironment.NewLine);
                AggiungiDefaultFirmaEmail(sb);

                string corpoMessaggio = sb.ToString();
                string oggetto = String.Format("Creazione del Ticket N.{0} - {1}", intervento.Numero, intervento.RagioneSociale);
                InviaEmail(oggetto, corpoMessaggio, ConfigurationKeys.MITTENTE_EMAIL_PER_INTERVENTO_APERTO_DAL_CLIENTE, emails.ToArray(), MailPriority.Normal);
            }
            else
            {
                if (String.IsNullOrEmpty(accountCliente.Email))
                {
                    throw new Exception(String.Format("All'account '{0}' non è stata associata nessuna email", accountCliente.Nominativo));
                }
                else
                {
                    throw new Exception(String.Format("All'account '{0}' non possiede nessuna email valida", accountCliente.Nominativo));
                }
            }
        }

        /// <summary>
        /// Effettua l'inivo dell'email per comunicare ai validatori dell'offerta un messaggio di richiesta di validazione.
        /// </summary>
        /// <param name="identificativoOfferta"></param>
        /// <returns></returns>
        public static bool InziaEmailRichiestaValidazioneOfferta(EntityId<Offerta> identificativoOfferta)
        {
            OfferteAccountValidatori ll = new OfferteAccountValidatori();
            IQueryable<Entities.OffertaAccountValidatore> query = ll.Read(identificativoOfferta);
            if (query.Any())
            {
                string domainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                string link = String.Format("{0}/Offerte/DettagliOfferta.aspx?ID={1}", domainName, identificativoOfferta.Value);

                foreach (Entities.OffertaAccountValidatore validatore in query)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(String.Format("Buongiorno {0},<br/>", validatore.Account.Nominativo));
                    sb.AppendLine(String.Format("questa email è stata generata e inviata dal {0} per comunicarle la richiesta di validazione dell'offerta n.{1}.<br/>Quest'ultima sarà accessibile mediante il link riportato di seguito.", ConfigurationKeys.TITOLO_APPLICAZIONE, validatore.Offerta.Numero));
                    sb.AppendLine("<br/><br/>");
                    sb.AppendLine(String.Format("<a href=\"{0}\" target=\"_blank\">Offerta N. {1}</a><br/><br/>", link, validatore.Offerta.Numero));
                    sb.AppendLine("Oppure copiare ed incollare il seguente link in un browser:<br/>");
                    sb.AppendLine(link);
                    sb.AppendLine("<br/><br/>");
                    sb.AppendLine("Questo messaggio è stato generato automaticamente dal sistema MUA. Non rispondere a questa e-mail poiché l'indirizzo non è controllato.");

                    string subject = String.Format("{0} - Richiesta validazione offerta n.{1}", ConfigurationKeys.TITOLO_APPLICAZIONE, validatore.Offerta.Numero); ;
                    InviaEmail(subject, sb.ToString(), ConfigurationKeys.MITTENTE_EMAIL_OFFERTA, new string[] { validatore.Account.Email }, MailPriority.High);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Invia una email all'utente di riferimento per la notifica di un nuovo messaggio chat
        /// </summary>
        /// <param name="intervento">Intervento relativo alla chat</param>
        /// <param name="operatoreCliente">Indica se il messaggio è scritto da un cliente</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void InviaEmailNuovaDiscussione(Entities.Intervento intervento, bool operatoreCliente)
        {
            if (intervento == null)
            {
                throw new ArgumentNullException("intervento", "Parametro nullo");
            }

            if (!intervento.Intervento_Discussiones.Any())
            {
                return;
            }


            string emailDaAvvisare = string.Empty;
            string urlIntervento = string.Empty;

            if (operatoreCliente)
            {
                // Se la nuova discussione è stata inserita da un cliente allora il messaggio va inviato all'indirizzo email indicato in configurazione
                if(intervento.ConfigurazioneTipologiaTicketCliente != null &&
                    intervento.ConfigurazioneTipologiaTicketCliente.Operatore != null &&
                    !string.IsNullOrEmpty(intervento.ConfigurazioneTipologiaTicketCliente.Operatore.EmailResponsabile))
                {
                    emailDaAvvisare = intervento.ConfigurazioneTipologiaTicketCliente.Operatore.EmailResponsabile;
                }
                //IEnumerable<Entities.Intervento_Discussione> elencoDiscussioni = intervento.Intervento_Discussiones.OrderBy(x => x.DataCommento);
                //Entities.Intervento_Discussione primaDiscussione = null;
                //primaDiscussione = elencoDiscussioni.FirstOrDefault();
                //emailDaAvvisare = primaDiscussione.Account.Email;
                urlIntervento = String.Format("{0}Interventi/Intervento.aspx?ID={1}", ConfigurationKeys.URL_APPLICAZIONE, intervento.ID);
            }
            else
            {
                // Se la nuova discussione è stata inserita da un operatore dell'assistenza allora il messaggio va inviato all'indirizzo email dell'utente cha ha creato l'intervento
                emailDaAvvisare = intervento.AccountRiferimento.Email;
                urlIntervento = String.Format("{0}Interventi/Ticket.aspx?ID={1}", ConfigurationKeys.URL_APPLICAZIONE, intervento.ID);
            }

            if(!string.IsNullOrEmpty(emailDaAvvisare))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(String.Format("Gentile utente,{1}c'è un nuovo messaggio da leggere relativo al ticket di intervento N.{0}{1}", intervento.Numero, Helper.Web.HTML_NEW_LINE));
                sb.AppendLine(String.Format("Clicca sul link sottostante per accedere al ticket di intervento{0}", Helper.Web.HTML_NEW_LINE));

                //string urlIntervento = String.Format("{0}Interventi/Intervento.aspx?ID={1}", ConfigurationKeys.URL_APPLICAZIONE, intervento.ID);
                sb.AppendLine(String.Format("{0}<a href=\"{1}\">Visualizza Ticket</a>", HtmlEnvironment.NewLine, urlIntervento));


                //sb.AppendLine(String.Format("Link intervento: <a href=\"{1}\">Intervento N.{2} - {3}</a>{0}{0}", HtmlEnvironment.NewLine, urlIntervento, intervento.Numero, intervento.RagioneSociale));

                if (intervento.Urgente.HasValue && intervento.Urgente.Value)
                {
                    sb.AppendLine(String.Format("<span style=\"color:red;\"><strong>URGENTE</strong></span>{0}", HtmlEnvironment.NewLine));
                }

                //sb.AppendLine("Cordiali saluti." + HtmlEnvironment.NewLine);

                string corpoMessaggio = sb.ToString();
                string oggetto = String.Format("Nuovo messaggio - Intervento N.{0}", intervento.Numero);

                //foreach (string email in elencoEmail)
                //{
                    InviaEmail(oggetto, corpoMessaggio, ConfigurationKeys.MITTENTE_EMAIL_NOTIFICA_INTERVENTO, new string[] { emailDaAvvisare }, MailPriority.Normal);
                //}
            }

        }


        ///// <summary>
        ///// Effettua l'invio dell'email di assegnazione dell'intervento a tutti gli indirizzi email degli account associati agli operatori passati come parametro
        ///// </summary>
        ///// <param name="accountCliente"></param>
        ///// <param name="intervento"></param>
        //public static void InviaCopiaEmailCreazioneInterventoDaParteDelClienteDestinatariSpecifici(Entities.Account accountCliente, Entities.Intervento intervento)
        //{
        //    if (accountCliente == null)
        //    {
        //        throw new ArgumentNullException("accountCliente", "Parametro nullo");
        //    }

        //    if (intervento == null)
        //    {
        //        throw new ArgumentNullException("intervento", "Parametro nullo");
        //    }

        //    string[] elencoUserName = Infrastructure.ConfigurationKeys.USERNAME_UTENTI_GESTIONALE_INVIO_EMAIL_PER_INTERVENTO_APERTO_DAL_CLIENTE;
        //    if (elencoUserName != null && elencoUserName.Length > 0)
        //    {
        //        Logic.Sicurezza.Accounts llAccount = new Logic.Sicurezza.Accounts();

        //        foreach(string username in elencoUserName)
        //        {
        //            Entities.Account accountGestionale = llAccount.Find(username);
        //            if (accountGestionale != null)
        //            {
        //                List<string> emails = Helper.EmailHelper.GetEmailsValidList(accountGestionale.Email);
        //                if (emails != null && emails.Count > 0)
        //                {
        //                    foreach (string email in emails)
        //                    {
        //                        StringBuilder sb = new StringBuilder();
        //                        sb.AppendLine(String.Format("Gentile {0},{1}", accountGestionale.Nominativo, Helper.Web.HTML_NEW_LINE));

        //                        sb.AppendLine(String.Format("questa email è stata generata in automatico dal software dal Gestionale per renderti al corrente che il cliente '{1}' ha effettuato la creazione dell'Intervento N.{2}.{0}",
        //                                                    Helper.Web.HTML_NEW_LINE,
        //                                                    accountCliente.Nominativo,
        //                                                    intervento.Numero));

        //                        sb.AppendLine(String.Format("Cliccando sul link sottostante, potrai accedere alla pagina di dettaglio dell'Intervento.{0}{0}", Helper.Web.HTML_NEW_LINE));

        //                        string urlIntervento = String.Format("{0}Interventi/Intervento.aspx?ID={1}", ConfigurationKeys.URL_APPLICAZIONE, intervento.ID);
        //                        sb.AppendLine(String.Format("<a href=\"{1}\">Intervento N.{2}</a>{0}{0}", HtmlEnvironment.NewLine, urlIntervento, intervento.Numero));
        //                        sb.AppendLine("Buona giornata.");

        //                        string corpoMessaggio = sb.ToString();
        //                        string ogetto = String.Format("Cliente '{0}': Creazione dell'Intervento N.{1}", accountCliente.Nominativo, intervento.Numero);
        //                        InviaEmail(ogetto, corpoMessaggio, ConfigurationKeys.MITTENTE_EMAIL_PER_INTERVENTO_APERTO_DAL_CLIENTE, emails.ToArray(), MailPriority.Normal);
        //                    }
        //                }                        
        //            }
        //        }                
        //    }
        //    else
        //    {
        //        throw new Exception("Non sono presenti degli utenti del gestionale a cui inviare l'email di copia per la creazione di un intervento da parte del cliente");
        //    }
        //}

        #endregion

        #region Creazione Messaggi

        /// <summary>
        /// Effettua la creazione del corpo dell'email da notificare, restituendolo mediante parametri out
        /// </summary>
        /// <param name="htmlBody"></param>
        /// <param name="plaintTextBody"></param>
        public static void CreaCorpoEmailInvioNotifica(NotificaDaInviareDTO notificaDaInviare, out string htmlBody, out string plaintTextBody)
        {
            if (notificaDaInviare == null) throw new ArgumentNullException("notificaDaInviare", "Parametro nullo");
            
            StringBuilder sbCorpoHTML = new StringBuilder();
            StringBuilder sbCorpoPlainText = new StringBuilder();

            sbCorpoHTML.AppendLine(String.Format("Gentile {0},{1}", notificaDaInviare.Nominativo, Helper.Web.HTML_NEW_LINE));
            sbCorpoHTML.AppendLine(String.Format("questa email è stata generata in automatico per renderti fornirti delle informazioni in merito alle notifiche in cui sei menzionato.{0}", Helper.Web.HTML_NEW_LINE));
            sbCorpoHTML.AppendLine(String.Format("Di seguito troverai tutte le tipologie di notifica con le relative informazioni.{0}{0}", Helper.Web.HTML_NEW_LINE));

            string htmlDaSostituire = sbCorpoHTML.ToString();
            sbCorpoPlainText.AppendLine(htmlDaSostituire.Replace(Helper.Web.HTML_NEW_LINE, Helper.Web.PLAIN_TEXT_NEW_LINE));

            if (notificaDaInviare.EsistonoNotificheInformazioniDaInviare)
            {
                IDictionary<TipologiaNotificaEnum, List<InformazioneNotiticaDaInviareDTO>> elencoNotifiche = notificaDaInviare.GetElencoNotificheInformazioni();
                foreach (KeyValuePair<TipologiaNotificaEnum, List<InformazioneNotiticaDaInviareDTO>> informazioniNotifiche in elencoNotifiche)
                {
                    sbCorpoHTML.AppendLine(String.Format("<strong>{0}</strong>{1}", informazioniNotifiche.Key.GetDescription(), Helper.Web.HTML_NEW_LINE));
                    sbCorpoPlainText.AppendLine(String.Format("{0}{1}", informazioniNotifiche.Key.GetDescription(), Helper.Web.PLAIN_TEXT_NEW_LINE));

                    if (informazioniNotifiche.Value.Count > 0)
                    {
                        foreach (InformazioneNotiticaDaInviareDTO informazioneNotifica in informazioniNotifiche.Value)
                        {
                            sbCorpoHTML.AppendLine(String.Format("{0}{1}", informazioneNotifica.InformazioneHTML, Helper.Web.HTML_NEW_LINE));
                            sbCorpoPlainText.AppendLine(String.Format("{0}{1}", informazioneNotifica.InformazionePlainText, Helper.Web.PLAIN_TEXT_NEW_LINE));
                        }

                        sbCorpoHTML.AppendLine(String.Format("{0}", Helper.Web.HTML_NEW_LINE));
                        sbCorpoPlainText.AppendLine(String.Format("{0}", Helper.Web.PLAIN_TEXT_NEW_LINE));
                    }
                    else
                    {
                        sbCorpoHTML.AppendLine(String.Format("Nessun dato associato{0}{0}", Helper.Web.HTML_NEW_LINE));
                        sbCorpoPlainText.AppendLine(String.Format("Nessun dato associato{0}{0}", Helper.Web.PLAIN_TEXT_NEW_LINE));
                    }
                }
            }

            //sbCorpoHTML.AppendLine(String.Format("{0}Buona giornata.", Helper.Web.HTML_NEW_LINE));
            sbCorpoHTML.AppendLine(String.Format("{0}Cordiali saluti.{1}", Helper.Web.HTML_NEW_LINE, ConfigurationKeys.DENOMINAZIONE_AZIENDA_ALTRO.Replace("\\n", Helper.Web.HTML_NEW_LINE)));

            //sbCorpoPlainText.AppendLine(String.Format("{0}Buona giornata.", Helper.Web.PLAIN_TEXT_NEW_LINE));
            sbCorpoPlainText.AppendLine(String.Format("{0}Cordiali saluti.{1}", Helper.Web.PLAIN_TEXT_NEW_LINE, ConfigurationKeys.DENOMINAZIONE_AZIENDA_ALTRO.Replace("\\n", Helper.Web.PLAIN_TEXT_NEW_LINE)));

            htmlBody = sbCorpoHTML.ToString();
            plaintTextBody = sbCorpoPlainText.ToString();
        }

        #endregion

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Aggiunge al messaggio passato come parametro, come mittenti, elenco dei destinatari passati come parametro
        /// </summary>
        /// <param name="mailMessage"></param>
        /// <param name="destinatari"></param>
        /// <remarks></remarks>

        private static void AggiungiDestinatari(MailMessage mailMessage, string[] destinatari)
        {
            if (mailMessage == null)
            {
                throw new ArgumentNullException("mailMessage", "Paramentro nullo");
            }

            if (destinatari == null || destinatari.Count() <= 0)
            {
                throw new Exception("Non è stato indicato nessun destinatario");
            }

            foreach (string destinatario in destinatari)
            {
                if (!String.IsNullOrWhiteSpace(destinatario))
                {
                    mailMessage.To.Add(destinatario);
                }                
            }
        }

        /// <summary>
        /// Aggiunge all'oggetto MailMessage, come allegati, i file passati come parametro
        /// </summary>
        /// <param name="mailMessage"></param>
        /// <param name="fileDaAllegare"></param>
        /// <remarks></remarks>

        private static void AggiungiAllegati(MailMessage mailMessage, System.IO.FileInfo[] fileDaAllegare)
        {
            if (mailMessage == null)
            {
                throw new ArgumentNullException("mailMessage", "paramentro nullo");
            }

            if (fileDaAllegare != null && fileDaAllegare.Count() > 0)
            {
                foreach (System.IO.FileInfo file in fileDaAllegare)
                {
                    if (file.Exists)
                    {
                        Attachment allegato = new Attachment(file.FullName);
                        mailMessage.Attachments.Add(allegato);
                    }
                }
            }
        }


        private static void AggiungiDefaultFirmaEmail(StringBuilder sb)
        {
            sb.AppendLine("Gruppo L&T." + HtmlEnvironment.NewLine);
            sb.AppendLine("Brescia: Tel. +39 030 2306877" + HtmlEnvironment.NewLine);
            sb.AppendLine("Roma: Tel. +39 06 56569307" + HtmlEnvironment.NewLine);
            sb.AppendLine("http://gruppolt.com");
        }


        #endregion
    }
}