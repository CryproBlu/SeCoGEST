using Microsoft.Office.Interop.Word;
using SeCoGes.Utilities;
using SeCoGEST.Data;
using SeCoGEST.Entities;
using SeCoGEST.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeCoGEST.Logic
{
    public class MotoreInvioNotifiche
    {
        /// <summary>
        /// Invia le notifiche specificatamente richieste dall'utente con l'indicazione nell'intervento della data e del testo da notificare
        /// </summary>
        public void InviaNotificheSpecificatamenteRichieste()
        {
            Logic.Interventi LogiclInterventi = new Interventi(true);
            Logic.Intervento_Operatori LogiclInterventoOperatori = new Logic.Intervento_Operatori(LogiclInterventi);

            // 1) Rileva l'elenco degli interventi che hanno l'indicazione di notifica e non sono ancora stati notificati
            IQueryable<Entities.Intervento> elencoInterventi = LogiclInterventi.Read().Where(x => 
                x.DataNotifica.HasValue && 
                x.DataNotifica.Value <= DateTime.Now &&
                (!x.Notificata.HasValue || !x.Notificata.Value));

            // Nel caso in cui ci siano interventi con richiesta di notifica...
            if (elencoInterventi != null && elencoInterventi.Count() > 0)
            {
                // 2) Cicla per ogni Intervento rilevato...
                foreach (Entities.Intervento intervento in elencoInterventi)
                {
                    string emailSubject = "Promemoria Intervento";
                    string emailTemplate = "Gentile utente,{0}questo è un promemoria per l'intervento N.{1} {2}{0}{0}Promemoria richiesto per il: {3:dd/MM/yyyy HH.mm}{0}Testo del promemoria: {4}{0}{0}Visualizza intervento: {5}";
                    
                    string urlInterventoPlainText = String.Format("{0}Interventi/Intervento.aspx?ID={1}", Infrastructure.ConfigurationKeys.URL_APPLICAZIONE, intervento.ID);
                    //string infoInterventoPlainText = String.Format("Intervento N.{0} - {1}", intervento.Numero, intervento.RagioneSociale);
                    string urlInterventoHTML = String.Format("<a href=\"{0}\">Intervento N.{1} - {2}</a>", urlInterventoPlainText, intervento.Numero, intervento.RagioneSociale);

                    string testoNotifica = "Promemoria.";
                    if(!string.IsNullOrEmpty(intervento.TestoNotifica)) testoNotifica = intervento.TestoNotifica.Trim();

                    string emailPlainText = string.Format(emailTemplate, Environment.NewLine, intervento.Numero, intervento.RagioneSociale, intervento.DataNotifica, testoNotifica, urlInterventoPlainText);
                    string emailHTMLText = string.Format(emailTemplate, "<br/>", intervento.Numero, intervento.RagioneSociale, intervento.DataNotifica, testoNotifica, urlInterventoHTML);


                    //return Read().Where(x => x.IDOperatore == identifierOperatore.Value && x.InviaNotifiche.HasValue && x.InviaNotifiche.Value).Select(x => x.Account);

                    Logic.Operatori llOp = new Operatori(LogiclInterventoOperatori);
                    List<Entities.Operatore> operatoriIntervento = LogiclInterventoOperatori.Read(intervento).Where(i => !i.FineIntervento.HasValue).Select(i => i.Operatore).Distinct().ToList();
                    List<string> emailsOperatoriIntervento = new List<string>();
                    foreach(Entities.Operatore operatore in operatoriIntervento)
                    {
                        emailsOperatoriIntervento.AddRange(llOp.GetValidEmailsListFromOperatore(operatore));
                    }
                    emailsOperatoriIntervento = emailsOperatoriIntervento.Distinct().ToList();

                    //List<string> emailsOperatoriIntervento = LogiclInterventoOperatori.Read(intervento).Where(i => !i.FineIntervento.HasValue).SelectMany(i => i.Operatore.AccountOperatores.Select(a => a.Account.Email)).Distinct().ToList();
                    if (emailsOperatoriIntervento.Any())
                    {
                        // Se l'intervento è associato ad una configurazione ticket allora notifica anche alla mail dell'operatore di riferimento per la confugurazione
                        if (intervento.ConfigurazioneTipologiaTicketCliente != null &&
                            intervento.ConfigurazioneTipologiaTicketCliente.Operatore != null &&
                            !string.IsNullOrEmpty(intervento.ConfigurazioneTipologiaTicketCliente.Operatore.EmailResponsabile))
                        {
                            if(!emailsOperatoriIntervento.Contains(intervento.ConfigurazioneTipologiaTicketCliente.Operatore.EmailResponsabile)) emailsOperatoriIntervento.Add(intervento.ConfigurazioneTipologiaTicketCliente.Operatore.EmailResponsabile);
                        }

                        try
                        {
                            EmailHelper.InviaEmail(emailSubject, emailPlainText, emailHTMLText, Infrastructure.ConfigurationKeys.MITTENTE_EMAIL_NOTIFICA_INTERVENTO, emailsOperatoriIntervento.ToArray());
                            intervento.Notificata = true;
                            LogiclInterventi.SubmitToDatabase();
                        }
                        catch(Exception ex) {}
                    }
                }
            }
        }





        //private Logic.Interventi _llInterventi; 
        //private Logic.Interventi LogiclInterventi
        //{ get
        //    {
        //        if (_llInterventi == null) _llInterventi = new Interventi();
        //        return _llInterventi;
        //    } 
        //}


        /// <summary>
        /// Effettua l'analisi delle notifiche da inviare agli operatori degli interventi ai quali è associata una configurazione ticket con tempistiche SLA
        /// </summary>
        public void InviaNotifichePerTicketsConConfigurazione()
        {
            // Passaggi da eseguire:
            // 1) Rileva l'elenco degli interventi APERTI che sono ASSOCIATI ALMENO AD UNA ConfigurazioneTipologiaTicketCliente
            // 2) Cicla per ogni Intervento rilevato...
            // 3) Determina gli Operatori associati all'Intervento corrente ai quali andrebbe inviata la notifica
            // 4) Cicla per ogni Tipologia di NOTIFICA da inviare (dipende dal tipo di CaratteristicaInterventoEnum della ConfigurazioneTipologiaTicketCliente associata all'Intervento corrente)...
            // 5) Cicla per gli Operatori associati all'intervento...
            // 6) Se la notifica NON è già stata inviata per l'Operatore corrente relativamente alla Tipologia di Notifica corrente...
            // 7) Determina il Tempo Più Restrittivo Per il Tipo di Caratteristica corrente e se è ora di inviare la notifica (manca 1 ora)...
            // 4) Invia all'Operatore la mail specifica per la tipologia di notifica

            Logic.Interventi LogiclInterventi = new Interventi(true);
            Logic.Operatori llOperatori = new Logic.Operatori(LogiclInterventi);
            Logic.LogInvioNotifiche llLogInvioNotifiche = new LogInvioNotifiche(LogiclInterventi);

            // 1) Rileva l'elenco degli interventi APERTI che sono ASSOCIATI ALMENO AD UNA ConfigurazioneTipologiaTicketCliente
            IQueryable<Entities.Intervento> elencoInterventi = LogiclInterventi.Read(Entities.StatoInterventoEnum.Aperto, Entities.StatoInterventoEnum.InGestione, Entities.StatoInterventoEnum.Eseguito).Where(x => x.ConfigurazioneTipologiaTicketCliente != null);

            // Nel caso in cui ci siano interventi aperti associati a configurazioni tickets cliente...
            if (elencoInterventi != null && elencoInterventi.Count() > 0)
            {
                // 2) Cicla per ogni Intervento rilevato...
                Logic.RepartiUfficio llRU = new Logic.RepartiUfficio(LogiclInterventi);
                Logic.CaratteristicheTipologieIntervento llCTI = new Logic.CaratteristicheTipologieIntervento(LogiclInterventi);

                foreach (Entities.Intervento intervento in elencoInterventi)
                {
                    if (intervento.ConfigurazioneTipologiaTicketCliente != null)
                    {
                        // Informazioni sugli orari del Reparto di assistenza
                        Entities.RepartoUfficio repartoUfficio = llRU.Find(new Entities.EntityId<RepartoUfficio>(intervento.ConfigurazioneTipologiaTicketCliente.IdRepartoUfficio));

                        // Informazioni sugli SLA dell'intervento
                        IEnumerable<Entities.CaratteristicaTipologiaIntervento> caratteristicheIntervento = llCTI.Read(intervento.IDConfigurazioneTipologiaTicket.Value);

                        foreach (Entities.CaratteristicaTipologiaIntervento caratteristicaTipologiaIntervento in caratteristicheIntervento)
                        {
                            if (Enum.IsDefined(typeof(Entities.CaratteristicaInterventoEnum), caratteristicaTipologiaIntervento.IdCaratteristica))
                            {
                                Entities.CaratteristicaInterventoEnum caratteristicaInterventoCorrente = (Entities.CaratteristicaInterventoEnum)caratteristicaTipologiaIntervento.IdCaratteristica;

                                int oreNotifica = 1;
                                if(intervento.ConfigurazioneTipologiaTicketCliente != null &&
                                   intervento.ConfigurazioneTipologiaTicketCliente.TipologiaIntervento != null &&
                                   intervento.ConfigurazioneTipologiaTicketCliente.TipologiaIntervento.OreNotifica.HasValue &&
                                   intervento.ConfigurazioneTipologiaTicketCliente.TipologiaIntervento.OreNotifica.Value > 0)
                                {
                                    oreNotifica= intervento.ConfigurazioneTipologiaTicketCliente.TipologiaIntervento.OreNotifica.Value;
                                }

                                TimeSpan tempoLimiteDefinitoPerCaratteristica = GetTempoLimiteDefinitoPerCaratteristica(caratteristicaTipologiaIntervento, out string descri);

                                DateTime? dataLimite = null;
                                if (repartoUfficio != null)
                                {
                                    dataLimite = llRU.GetDataLimiteIntervento(repartoUfficio, tempoLimiteDefinitoPerCaratteristica, caratteristicaInterventoCorrente, intervento);
                                }
                                else
                                {
                                    if (tempoLimiteDefinitoPerCaratteristica.Ticks > 0)
                                    {
                                        dataLimite = DateTime.Now.Add(tempoLimiteDefinitoPerCaratteristica);
                                    }
                                }

                                if (dataLimite.HasValue)
                                {

                                    // 5) Cicla per gli Operatori associati all'intervento...
                                    // 6) Se la notifica NON è già stata inviata in precedenza per l'Operatore corrente relativamente alla Tipologia di Notifica corrente...
                                    Entities.CaratteristicaInterventoEnum caratteristicaIntervento = (Entities.CaratteristicaInterventoEnum)caratteristicaTipologiaIntervento.IdCaratteristica;
                                    List<Intervento_Operatore> operatoriInterventoDaNotificare = GetInterventiOperatoreDaNotificare(intervento, caratteristicaIntervento); //, out TipologiaNotificaEnum tipologiaNotifica);
                                    if (operatoriInterventoDaNotificare.Any())
                                    {
                                        foreach (Intervento_Operatore operatoreInterventoDaNotificare in operatoriInterventoDaNotificare)
                                        {
                                            switch (caratteristicaIntervento)
                                            {
                                                case CaratteristicaInterventoEnum.PresaInCaricoEntroMinuti:
                                                case CaratteristicaInterventoEnum.RispostaEntroMinuti:
                                                    // Le notifiche vanno inviate per diverse motivazioni:
                                                    // SLA SCADUTA - NON PRESO IN CARICO: inviata superato l'orario di scadenza.
                                                    if (DateTime.Now > dataLimite.Value)
                                                    {
                                                        GesisciInvioNotifica(operatoreInterventoDaNotificare, TipologiaNotificaEnum.InterventoScadutoNonPresoInCarico, dataLimite.Value, llOperatori, llLogInvioNotifiche);
                                                    }
                                                    else
                                                    {   
                                                        // SCADENZA SLA - NON PRESO IN CARICO: inviata non prima di un ora dall'orario di scadenza.
                                                        if (DateTime.Now > dataLimite.Value.AddHours(-oreNotifica))
                                                        {
                                                            GesisciInvioNotifica(operatoreInterventoDaNotificare, TipologiaNotificaEnum.InterventoApertoNonPresoInCarico, dataLimite.Value, llOperatori, llLogInvioNotifiche);
                                                        }
                                                    }

                                                    break;




                                                case CaratteristicaInterventoEnum.RipristinoEntroMinuti:
                                                case CaratteristicaInterventoEnum.RipristinoEntroMinutiDaPresaInCarico: // TODO CONTROLLARE CHE QUESTO CASE CI DEBBA ESSERE O SE DEVE ESSERE GESTITO IN UN CASE APPOSITO

                                                    // SLA SCADUTA - NON PRESO IN CARICO: inviata superato l'orario di scadenza.
                                                    if (DateTime.Now > dataLimite.Value)
                                                    {
                                                        GesisciInvioNotifica(operatoreInterventoDaNotificare, TipologiaNotificaEnum.InterventoScadutoPresoInCaricoNonChiuso, dataLimite.Value, llOperatori, llLogInvioNotifiche);
                                                    }
                                                    else
                                                    {
                                                        // SCADENZA SLA - NON PRESO IN CARICO: inviata non prima di un ora dall'orario di scadenza.
                                                        if (DateTime.Now > dataLimite.Value.AddHours(-oreNotifica))
                                                        {
                                                            GesisciInvioNotifica(operatoreInterventoDaNotificare, TipologiaNotificaEnum.InterventoPresoInCaricoNonChiuso, dataLimite.Value, llOperatori, llLogInvioNotifiche);
                                                        }
                                                    }
                                                    break;
                                            }

                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //private void GeneraEmailNotificaPerCaratteristicaIntervento(Intervento_Operatore operatoreInterventoDaNotificare, CaratteristicaTipologiaIntervento caratteristicaTipologiaIntervento, DateTime dataLimite, 
        //    out string emailSubject, out string emailPlainText, out string emailHtmlText)
        //{
        //    string emailTemplate = String.Empty;
        //    emailSubject = String.Empty;

        //    string urlIntervento = String.Format("{0}Interventi/Intervento.aspx?ID={1}", Infrastructure.ConfigurationKeys.URL_APPLICAZIONE, operatoreInterventoDaNotificare.IDIntervento);
        //    string infoInterventoPlainText = String.Format("Intervento N.{0} - {1}", operatoreInterventoDaNotificare.Intervento.Numero, operatoreInterventoDaNotificare.Intervento.RagioneSociale);
        //    string infoInterventoHTML = String.Format("<a href=\"{0}\">Intervento N.{1} - {2}</a>", urlIntervento, operatoreInterventoDaNotificare.Intervento.Numero, operatoreInterventoDaNotificare.Intervento.RagioneSociale);

        //    Entities.CaratteristicaInterventoEnum caratteristicaIntervento = (Entities.CaratteristicaInterventoEnum)caratteristicaTipologiaIntervento.IdCaratteristica;
        //    switch(caratteristicaIntervento)
        //    {
        //        case CaratteristicaInterventoEnum.PresaInCaricoEntroMinuti:
        //        case CaratteristicaInterventoEnum.RispostaEntroMinuti:
        //            emailSubject = "Intervento da prendere in carico";
        //            emailTemplate = "Gentile {1},{0}si notifica la presenza di un intervento da prendere in carico:{0}{0}{2}{0}{3}{0}Scadenza: {4}";
        //            break;


        //        case CaratteristicaInterventoEnum.RipristinoEntroMinuti:
        //            emailSubject = "Chiusura intervento richiesta";
        //            emailTemplate = "Gentile {1},{0}si notifica la presenza di un intervento da chiudere:{0}{0}{2}{0}{3}{0}Scadenza: {4}";
        //            break;
        //    }

        //    emailPlainText = string.Format(emailTemplate, Helper.Web.PLAIN_TEXT_NEW_LINE,
        //        operatoreInterventoDaNotificare.Operatore.CognomeNome,
        //        infoInterventoPlainText,
        //        operatoreInterventoDaNotificare.Intervento.Oggetto,
        //        dataLimite.ToString("g"));



        //    string infoDataLimiteHTML = $"<strong>{dataLimite:g}</strong>";
        //    if (dataLimite < DateTime.Now)
        //    {
        //        infoDataLimiteHTML = $"<span style='color: red;font-weight: bold;'>{dataLimite:g}</span>";
        //    }

        //    emailHtmlText = string.Format(emailTemplate, Helper.Web.HTML_NEW_LINE,
        //        operatoreInterventoDaNotificare.Operatore.CognomeNome,
        //        infoInterventoHTML,
        //        operatoreInterventoDaNotificare.Intervento.Oggetto,
        //        infoDataLimiteHTML);
        //}
        
        private void GesisciInvioNotifica(Intervento_Operatore operatoreInterventoDaNotificare, TipologiaNotificaEnum tipoNotifica, DateTime dataLimite, Logic.Operatori llOperatori, Logic.LogInvioNotifiche llLogInvioNotifiche)
        {
            string dataLimiteString = dataLimite.ToString();

            if (!llLogInvioNotifiche.IsInvioNotificaGiàEffettuato<Entities.Intervento_Operatore>(operatoreInterventoDaNotificare.Identifier, InfoOperazioneTabellaEnum.InterventoOperatore, tipoNotifica, dataLimiteString))
            {
                GeneraEmailNotificaPerCaratteristicaIntervento(operatoreInterventoDaNotificare, tipoNotifica, dataLimite,
                out string emailSubject, out string emailPlainText, out string emailHtmlText);

                List<string> recipientEmails = llOperatori.GetValidEmailsListFromOperatore(operatoreInterventoDaNotificare.Operatore);
                if(tipoNotifica == TipologiaNotificaEnum.InterventoScadutoNonPresoInCarico ||
                   tipoNotifica == TipologiaNotificaEnum.InterventoScadutoPresoInCaricoNonChiuso)
                {
                    if(operatoreInterventoDaNotificare.Intervento != null &&
                       operatoreInterventoDaNotificare.Intervento.ConfigurazioneTipologiaTicketCliente != null &&
                       operatoreInterventoDaNotificare.Intervento.ConfigurazioneTipologiaTicketCliente.Operatore != null)
                    {
                        List<string> emailOperatoreRiferimentoConfigurazioneIntervento = llOperatori.GetValidEmailsListFromOperatore(operatoreInterventoDaNotificare.Intervento.ConfigurazioneTipologiaTicketCliente.Operatore);
                        if(emailOperatoreRiferimentoConfigurazioneIntervento.Any())
                        {
                            recipientEmails.AddRange(emailOperatoreRiferimentoConfigurazioneIntervento);
                        }
                    }
                }

                if (recipientEmails.Count == 0) return;

                EmailHelper.InviaEmail(emailSubject, emailPlainText, emailHtmlText, Infrastructure.ConfigurationKeys.MITTENTE_EMAIL_NOTIFICA_INTERVENTO, recipientEmails.Distinct().ToArray());

                Entities.LogInvioNotifica entityLogNotifica = new LogInvioNotifica();
                entityLogNotifica.Data = DateTime.Now;
                entityLogNotifica.IDLegame = operatoreInterventoDaNotificare.ID;
                entityLogNotifica.IDTabellaLegameEnum = InfoOperazioneTabellaEnum.InterventoOperatore;
                entityLogNotifica.IDNotificaEnum = tipoNotifica;
                entityLogNotifica.Esito = true;
                entityLogNotifica.Note = dataLimiteString;

                llLogInvioNotifiche.Create(entityLogNotifica, true);
            }
        }

        private void GeneraEmailNotificaPerCaratteristicaIntervento(Intervento_Operatore operatoreInterventoDaNotificare, TipologiaNotificaEnum tipologiaNotifica, DateTime dataLimite,
            out string emailSubject, out string emailPlainText, out string emailHtmlText)
        {
            string emailTemplate = String.Empty;
            emailSubject = String.Empty;

            string urlIntervento = String.Format("{0}Interventi/Intervento.aspx?ID={1}", Infrastructure.ConfigurationKeys.URL_APPLICAZIONE, operatoreInterventoDaNotificare.IDIntervento);
            string infoInterventoPlainText = String.Format("Intervento N.{0} - {1}", operatoreInterventoDaNotificare.Intervento.Numero, operatoreInterventoDaNotificare.Intervento.RagioneSociale);
            string infoInterventoHTML = String.Format("<a href=\"{0}\">Intervento N.{1} - {2}</a>", urlIntervento, operatoreInterventoDaNotificare.Intervento.Numero, operatoreInterventoDaNotificare.Intervento.RagioneSociale);

            switch (tipologiaNotifica)
            {
                case TipologiaNotificaEnum.InterventoApertoNonPresoInCarico:
                    emailSubject = "Intervento da prendere in carico";
                    emailTemplate = "Gentile {1},{0}si notifica la presenza di un intervento da prendere in carico:{0}{0}{2}{0}{3}{0}Scadenza: {4}";
                    break;

                case TipologiaNotificaEnum.InterventoScadutoNonPresoInCarico:
                    emailSubject = "Intervento da prendere in carico scaduto";
                    emailTemplate = "Gentile {1},{0}si notifica la presenza di un intervento scaduto da prendere in carico:{0}{0}{2}{0}{3}{0}Scadenza: {4}";
                    break;



                case TipologiaNotificaEnum.InterventoPresoInCaricoNonChiuso:
                    emailSubject = "Intervento da chiudere";
                    emailTemplate = "Gentile {1},{0}si notifica la presenza di un intervento da chiudere:{0}{0}{2}{0}{3}{0}Scadenza: {4}";
                    break;

                case TipologiaNotificaEnum.InterventoScadutoPresoInCaricoNonChiuso:
                    emailSubject = "Intervento scaduto da chiudere";
                    emailTemplate = "Gentile {1},{0}si notifica la presenza di un intervento scaduto da chiudere:{0}{0}{2}{0}{3}{0}Scadenza: {4}";
                    break;
            }

            emailPlainText = string.Format(emailTemplate, Helper.Web.PLAIN_TEXT_NEW_LINE,
                operatoreInterventoDaNotificare.Operatore.CognomeNome,
                infoInterventoPlainText,
                operatoreInterventoDaNotificare.Intervento.Oggetto,
                dataLimite.ToString("g"));



            string infoDataLimiteHTML = $"<strong>{dataLimite:g}</strong>";
            if (dataLimite < DateTime.Now)
            {
                infoDataLimiteHTML = $"<span style='color: red;font-weight: bold;'>{dataLimite:g}</span>";
            }

            emailHtmlText = string.Format(emailTemplate, Helper.Web.HTML_NEW_LINE,
                operatoreInterventoDaNotificare.Operatore.CognomeNome,
                infoInterventoHTML,
                operatoreInterventoDaNotificare.Intervento.Oggetto,
                infoDataLimiteHTML);
        }

        /// <summary>
        /// Restituisce l'elenco degli Id di Intervento_Operatore relativi agli Operatori dell'Intervento ai quali non è stata mai inviata una notifica per la CaratteristicaInterventoEnum specificata
        /// </summary>
        /// <param name="intervento"></param>
        /// <param name="caratteristicaIntervento"></param>
        /// <returns></returns>
        private List<Intervento_Operatore> GetInterventiOperatoreDaNotificare(Intervento intervento, CaratteristicaInterventoEnum caratteristicaIntervento)
        {
            List<Intervento_Operatore> elencoIdOperatoriIntervento = new List<Intervento_Operatore>();
            List<Entities.Intervento_Operatore> operatoriIntervento = new Logic.Intervento_Operatori().Read(intervento).ToList();

            switch (caratteristicaIntervento)
            {
                case CaratteristicaInterventoEnum.PresaInCaricoEntroMinuti:
                case CaratteristicaInterventoEnum.RispostaEntroMinuti:
                    
                    // Per queste due caratteristiche si controlla la Presa in Carico dell'Operatore
                    //tipoNotifica = TipologiaNotificaEnum.InterventoApertoNonPresoInCarico;

                    foreach (Entities.Intervento_Operatore interventoOperatore in operatoriIntervento)
                    {
                        if(!interventoOperatore.PresaInCarico.HasValue || interventoOperatore.PresaInCarico.Value == false)
                        {
                            //if (!llLIN.IsInvioNotificaGiàEffettuato<Entities.Intervento_Operatore>(interventoOperatore.Identifier, InfoOperazioneTabellaEnum.InterventoOperatore, tipoNotifica.Value, dataLimite))
                            //{
                            elencoIdOperatoriIntervento.Add(interventoOperatore);
                            //}
                        }
                    }
                    break;






                case CaratteristicaInterventoEnum.RipristinoEntroMinuti:
                case CaratteristicaInterventoEnum.RipristinoEntroMinutiDaPresaInCarico:

                    // Per questa caratteristica si controlla l'indicazione della data/orario di Fine Intervento dell'Operatore
                    //tipoNotifica = TipologiaNotificaEnum.InterventoPresoInCaricoNonChiuso;

                    foreach (Entities.Intervento_Operatore interventoOperatore in operatoriIntervento)
                    {
                        if (!interventoOperatore.FineIntervento.HasValue)
                        {
                            //if (!llLIN.IsInvioNotificaGiàEffettuato<Entities.Intervento_Operatore>(interventoOperatore.Identifier, InfoOperazioneTabellaEnum.InterventoOperatore, tipoNotifica.Value, dataLimite))
                            //{
                            elencoIdOperatoriIntervento.Add(interventoOperatore);
                            //}
                        }
                    }
                    break;
            }


            //tipologiaNotifica = tipoNotifica.Value;
            return elencoIdOperatoriIntervento;
        }

        private TimeSpan GetTempoLimiteDefinitoPerCaratteristica(Entities.CaratteristicaTipologiaIntervento caratteristicaTipologiaIntervento, out string descrizioneSLAIntervento)
        {
            TimeSpan tempoLimiteDefinitoPerCaratteristica = new TimeSpan(0); 
            descrizioneSLAIntervento = string.Empty;

            if (!string.IsNullOrWhiteSpace(caratteristicaTipologiaIntervento.Parametri))
            {
                long giorni = 0;
                long ore = 0;
                long minuti = 0;
                if (long.TryParse(caratteristicaTipologiaIntervento.Parametri, out minuti))
                {
                    if (minuti >= 1440)
                    {
                        giorni = minuti / 1440;
                        minuti = minuti % 1440;
                    }
                    if (minuti >= 60)
                    {
                        ore = minuti / 60;
                        minuti = minuti % 60;
                    }

                    tempoLimiteDefinitoPerCaratteristica = new TimeSpan((int)giorni, (int)ore, (int)minuti, 0, 0);
                    string tempo = string.Format("{0:%d} giorni {0:%h} ore {0:%m} minuti", tempoLimiteDefinitoPerCaratteristica);
                    descrizioneSLAIntervento = $"{caratteristicaTipologiaIntervento.NomeCaratteristicaIntervento} - Tempo: {tempo}<br />";
                }
            }

            return tempoLimiteDefinitoPerCaratteristica;
        }




        ///// <summary>
        ///// Restituisce l'elenco degli operatori dell'intervento indicato che non hanno ancora indicato la data di chiusura della propria lavorazione
        ///// </summary>
        ///// <param name="intervento"></param>
        ///// <returns></returns>
        //private List<Intervento_Operatore> GetOperatoriInterventoAperti(Intervento intervento)
        //{
        //    return new Logic.Intervento_Operatori().Read(intervento).Where(i => !i.FineIntervento.HasValue).ToList();
        //}
    }
















    //public class MotoreInvioNotifiche2 : Base.LogicLayerBase
    //{
    //    #region Properties Private

    //    private Entities.ElencoNotificaDaInviare ElencoNotificaDaInviare;
    //    private List<Entities.LogInvioNotifica> ElencoLogInvioNotifica;

    //    #endregion

    //    #region Costruttori e DAL interno

    //    /// <summary>
    //    /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
    //    /// </summary>
    //    Logic.Intervento_Operatori llInterventiOperatori;
    //    Logic.Interventi llInterventi;
    //    Logic.Operatori llOperatori;
    //    Logic.LogInvioNotifiche llLogInvioNotifiche;

    //    /// <summary>
    //    /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
    //    /// </summary>
    //    public MotoreInvioNotifiche2()
    //        : base(false)
    //    {
    //        CreateDalAndLogic();
    //    }

    //    /// <summary>
    //    /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
    //    /// </summary>
    //    /// <param name="createStandaloneContext"></param>
    //    public MotoreInvioNotifiche2(bool createStandaloneContext)
    //        : base(createStandaloneContext)
    //    {
    //        CreateDalAndLogic();
    //    }

    //    /// <summary>
    //    /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
    //    /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
    //    /// </summary>
    //    /// <param name="logicLayer"></param>
    //    public MotoreInvioNotifiche2(Base.LogicLayerBase logicLayer)
    //        : base(logicLayer)
    //    {
    //        CreateDalAndLogic();
    //    }



    //    /// <summary>
    //    /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
    //    /// </summary>
    //    private void CreateDalAndLogic()
    //    {
    //        ElencoLogInvioNotifica = new List<Entities.LogInvioNotifica>();
    //        ElencoNotificaDaInviare = new ElencoNotificaDaInviare();

    //        llInterventiOperatori = new Intervento_Operatori(this);
    //        llInterventi = new Logic.Interventi(this);
    //        llOperatori = new Logic.Operatori(this);
    //        llLogInvioNotifiche = new LogInvioNotifiche(this);
    //    }

    //    #endregion

    //    #region Metodi Pubblici

    //    /// <summary>
    //    /// Effettua l'analisi delle notifiche da inviare per una determinata tipologia
    //    /// </summary>
    //    /// <param name="tipologiaNotifica"></param>
    //    public void AnalizzaNotificaDaInviare(Entities.TipologiaNotificaEnum tipologiaNotifica)
    //    {
    //        switch(tipologiaNotifica)
    //        {
    //            case Entities.TipologiaNotificaEnum.InterventoApertoNonPresoInCarico:
    //                AnalizzaNotificaInterventoApertoNonPresoInCarico();
    //                break;

    //            case Entities.TipologiaNotificaEnum.InterventoPresoInCaricoNonChiuso:
    //                AnalizzaNotificaInterventoPresoInCaricoNonChiuso();
    //                break;

    //            case Entities.TipologiaNotificaEnum.InterventoSenzaAlcunOperatoreAssegnato:
    //                AnalizzaNotificaInterventiSenzaOperatori();
    //                break;

    //            //case Entities.TipologiaNotificaEnum.InterventoDaNotificarePerSLA:
    //            //    AnalizzaNotificaOperatoriInterventiPerSLA();
    //            //    break;
    //        }
    //    }

    //    /// <summary>
    //    /// Effettua l'invio delle notifiche, tramite email
    //    /// </summary>
    //    public void InviaNotificheTramiteEmail(bool memorizzaInviiEffettuati, bool submitChanges)
    //    {
    //        if (ElencoNotificaDaInviare != null)
    //        {
    //            if (ElencoNotificaDaInviare.EsistonoNotifiche)
    //            {
    //                string oggetto = String.Join(" - ", Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE, "Elenco Notifiche Rilevate");
    //                List<NotificaDaInviareDTO> notifiche = ElencoNotificaDaInviare.GetNotifiche();
    //                EffettuaInvioDelleNotifiche(oggetto, notifiche);
    //            }

    //            if (ElencoNotificaDaInviare.EsistonoNotificheAiResponsabili)
    //            {
    //                string oggetto = String.Join(" - ", Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE, "Elenco Notifiche Rilevate");
    //                List<NotificaDaInviareAdEmailSpecificaDTO> notificheAiResponsabili = ElencoNotificaDaInviare.GetNotificheAiResponsabili();
    //                EffettuaInvioDelleNotificheAiResponsabili(oggetto, notificheAiResponsabili);
    //            }

    //            if (ElencoNotificaDaInviare.EsistonoNotificheInterventiSenzaOperatore)
    //            {
    //                string oggetto = String.Join(" - ", Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE, "Elenco Interventi Senza Operatore");
    //                List<NotificaDaInviareAdEmailSpecificaDTO> notificheInterventiSenzaOperatori = ElencoNotificaDaInviare.GetNotificheInterventiSenzaOperatore();
    //                EffettuaInvioDelleNotifichePerInterventiSenzaOperatori(oggetto, notificheInterventiSenzaOperatori);
    //            }

    //            if (ElencoNotificaDaInviare.EsistonoNotificheInterventiSLA)
    //            {
    //                string oggetto = String.Join(" - ", Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE, "Elenco Interventi in base a SLA");
    //                List<NotificaDaInviareAdEmailSpecificaDTO> notificheInterventiSLA = ElencoNotificaDaInviare.GetNotificheInterventiSLA();
    //                EffettuaInvioDelleNotifichePerInterventiSLA(oggetto, notificheInterventiSLA);
    //            }

    //        }

    //        if (memorizzaInviiEffettuati)
    //        {
    //            PersistiElencoLogInvioNotificaSeEsistenti(submitChanges);
    //        }
    //    }

    //    /// <summary>
    //    /// 
    //    /// Effettua la pulizia dell'elenco delle notifiche da inviare
    //    /// </summary>
    //    public void PulisciElencoNotificaDaInviare()
    //    {
    //        if (ElencoNotificaDaInviare != null && ElencoNotificaDaInviare.EsistonoNotifiche)
    //        {
    //            ElencoNotificaDaInviare.Clear();
    //        }
    //    }

    //    #endregion

    //    #region Funzioni per la Creazione Messaggi

    //    /// <summary>
    //    /// Effettua la creazione del corpo dell'email da notificare, restituendolo mediante parametri out
    //    /// </summary>
    //    /// <param name="htmlBody"></param>
    //    /// <param name="plaintTextBody"></param>
    //    public static void CreaCorpoEmailInvioNotifica(NotificaDaInviareDTO notificaDaInviare, out string htmlBody, out string plaintTextBody)
    //    {
    //        IDictionary<TipologiaNotificaEnum, List<InformazioneNotiticaDaInviareDTO>> elencoNotifiche = notificaDaInviare.GetElencoNotificheInformazioni();
    //        CreaCorpoEmailInvioNotificaGenerica(notificaDaInviare.Nominativo, elencoNotifiche, out htmlBody, out plaintTextBody);
    //    }

    //    /// <summary>
    //    /// Effettua la creazione del corpo dell'email da notificare ai responsabili, restituendolo mediante parametri out
    //    /// </summary>
    //    /// <param name="htmlBody"></param>
    //    /// <param name="plaintTextBody"></param>
    //    public static void CreaCorpoEmailInvioNotificaAiResponsabili(NotificaDaInviareAdEmailSpecificaDTO notificaDaInviare, out string htmlBody, out string plaintTextBody)
    //    {
    //        IDictionary<TipologiaNotificaEnum, List<InformazioneNotiticaDaInviareDTO>> elencoNotifiche = notificaDaInviare.GetElencoNotificheInformazioni();
    //        CreaCorpoEmailInvioNotificaGenerica(notificaDaInviare.Nominativo, elencoNotifiche, out htmlBody, out plaintTextBody);
    //    }

    //    /// <summary>
    //    /// Effettua la creazione del corpo dell'email da notificare per gli interventi senza operatori, restituendolo mediante parametri out
    //    /// </summary>
    //    /// <param name="htmlBody"></param>
    //    /// <param name="plaintTextBody"></param>
    //    public static void CreaCorpoEmailInvioNotificaInterventiSenzaOperatori(NotificaDaInviareAdEmailSpecificaDTO notificaDaInviare, out string htmlBody, out string plaintTextBody)
    //    {
    //        IDictionary<TipologiaNotificaEnum, List<InformazioneNotiticaDaInviareDTO>> elencoNotifiche = notificaDaInviare.GetElencoNotificheInformazioni();
    //        CreaCorpoEmailInvioNotificaGenerica(notificaDaInviare.Nominativo, elencoNotifiche, out htmlBody, out plaintTextBody);
    //    }

    //    /// <summary>
    //    /// Effettua la creazione del corpo dell'email da notificare per gli interventi SLA, restituendolo mediante parametri out
    //    /// </summary>
    //    /// <param name="htmlBody"></param>
    //    /// <param name="plaintTextBody"></param>
    //    public static void CreaCorpoEmailInvioNotificaInterventiSLA(NotificaDaInviareAdEmailSpecificaDTO notificaDaInviare, out string htmlBody, out string plaintTextBody)
    //    {
    //        IDictionary<TipologiaNotificaEnum, List<InformazioneNotiticaDaInviareDTO>> elencoNotifiche = notificaDaInviare.GetElencoNotificheInformazioni();
    //        CreaCorpoEmailInvioNotificaGenerica(notificaDaInviare.Nominativo, elencoNotifiche, out htmlBody, out plaintTextBody);
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="nominativo"></param>
    //    /// <param name="elencoNotifiche"></param>
    //    /// <param name="htmlBody"></param>
    //    /// <param name="plaintTextBody"></param>
    //    private static void CreaCorpoEmailInvioNotificaGenerica(string nominativo, IDictionary<TipologiaNotificaEnum, List<InformazioneNotiticaDaInviareDTO>> elencoNotifiche, out string htmlBody, out string plaintTextBody)
    //    {
    //        if (String.IsNullOrEmpty(nominativo)) nominativo = "Utente";

    //        StringBuilder sbCorpoHTML = new StringBuilder();
    //        StringBuilder sbCorpoPlainText = new StringBuilder();

    //        sbCorpoHTML.AppendLine(String.Format("Gentile {0},{1}", nominativo, Helper.Web.HTML_NEW_LINE));
    //        sbCorpoHTML.AppendLine(String.Format("questa email è stata generata in automatico dal software dal Gestionale per fornirti le informazioni relative a degli elementi a cui dovresti dare attenzione.{0}", Helper.Web.HTML_NEW_LINE));
    //        sbCorpoHTML.AppendLine(String.Format("Di seguito troverai tutte le tipologie di notifica con le relative informazioni.{0}{0}", Helper.Web.HTML_NEW_LINE));

    //        string htmlDaSostituire = sbCorpoHTML.ToString();
    //        sbCorpoPlainText.AppendLine(htmlDaSostituire.Replace(Helper.Web.HTML_NEW_LINE, Helper.Web.PLAIN_TEXT_NEW_LINE));

    //        if (elencoNotifiche != null && elencoNotifiche.Count > 0)
    //        {                
    //            foreach (KeyValuePair<TipologiaNotificaEnum, List<InformazioneNotiticaDaInviareDTO>> informazioniNotifiche in elencoNotifiche)
    //            {
    //                sbCorpoHTML.AppendLine(String.Format("<strong>{0}</strong>{1}", informazioniNotifiche.Key.GetDescription(), Helper.Web.HTML_NEW_LINE));
    //                sbCorpoPlainText.AppendLine(String.Format("{0}{1}", informazioniNotifiche.Key.GetDescription(), Helper.Web.PLAIN_TEXT_NEW_LINE));

    //                if (informazioniNotifiche.Value.Count > 0)
    //                {
    //                    foreach (InformazioneNotiticaDaInviareDTO informazioneNotifica in informazioniNotifiche.Value)
    //                    {
    //                        sbCorpoHTML.AppendLine(String.Format("{0}{1}", informazioneNotifica.InformazioneHTML, Helper.Web.HTML_NEW_LINE));
    //                        sbCorpoPlainText.AppendLine(String.Format("{0}{1}", informazioneNotifica.InformazionePlainText, Helper.Web.PLAIN_TEXT_NEW_LINE));
    //                    }

    //                    sbCorpoHTML.AppendLine(String.Format("{0}", Helper.Web.HTML_NEW_LINE));
    //                    sbCorpoPlainText.AppendLine(String.Format("{0}", Helper.Web.PLAIN_TEXT_NEW_LINE));
    //                }
    //                else
    //                {
    //                    sbCorpoHTML.AppendLine(String.Format("Nessun dato associato{0}{0}", Helper.Web.HTML_NEW_LINE));
    //                    sbCorpoPlainText.AppendLine(String.Format("Nessun dato associato{0}{0}", Helper.Web.PLAIN_TEXT_NEW_LINE));
    //                }
    //            }
    //        }

    //        sbCorpoHTML.AppendLine(String.Format("{0}Buona giornata.", Helper.Web.HTML_NEW_LINE));
    //        sbCorpoPlainText.AppendLine(String.Format("{0}Buona giornata.", Helper.Web.PLAIN_TEXT_NEW_LINE));

    //        htmlBody = sbCorpoHTML.ToString();
    //        plaintTextBody = sbCorpoPlainText.ToString();
    //    }

    //    #endregion

    //    #region Funzioni per l'invio dei messaggi

    //    /// <summary>
    //    /// Effettua il reale invio delle notifiche in base all'elenco dei dto passato come parametro
    //    /// </summary>
    //    /// <param name="oggetto"></param>
    //    /// <param name="notifiche"></param>
    //    private void EffettuaInvioDelleNotifiche(string oggetto, List<NotificaDaInviareDTO> notifiche)
    //    {
    //        if (notifiche != null && notifiche.Count > 0)
    //        {
    //            if (String.IsNullOrEmpty(oggetto))
    //            {
    //                oggetto = String.Join(" - ", Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE, "Elenco Notifiche Rilevate");
    //            }

    //            foreach (Entities.NotificaDaInviareDTO notificaDaInviare in notifiche)
    //            {
    //                Entities.Operatore operatore = llOperatori.Find(notificaDaInviare.IdentifierOperatore);
    //                if (operatore != null)
    //                {
    //                    // Viene recuperata la lista delle email valide, associate ad un operatore
    //                    List<string> elencoEmailPerOperatore = llOperatori.GetValidEmailsListFromOperatore(operatore);

    //                    // Nel caso in cui esista almento una email valdia ..
    //                    if (elencoEmailPerOperatore != null && elencoEmailPerOperatore.Count > 0)
    //                    {
    //                        string html = String.Empty;
    //                        string plainText = String.Empty;
    //                        string[] destinatari = elencoEmailPerOperatore.ToArray();

    //                        CreaCorpoEmailInvioNotifica(notificaDaInviare, out html, out plainText);

    //                        EmailHelper.InviaEmail(oggetto, plainText, html, Infrastructure.ConfigurationKeys.MITTENTE_EMAIL_NOTIFICA_INTERVENTO, destinatari);
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Effettua il reale invio delle notifiche in base all'elenco dei dto passato come parametro
    //    /// </summary>
    //    /// <param name="oggetto"></param>
    //    /// <param name="notifiche"></param>
    //    private void EffettuaInvioDelleNotificheAiResponsabili(string oggetto, List<NotificaDaInviareAdEmailSpecificaDTO> notificheAiResponsabili)
    //    {
    //        if (notificheAiResponsabili != null && notificheAiResponsabili.Count > 0)
    //        {
    //            if (String.IsNullOrEmpty(oggetto))
    //            {
    //                oggetto = String.Join(" - ", Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE, "Elenco Notifiche Rilevate");
    //            }

    //            foreach (Entities.NotificaDaInviareAdEmailSpecificaDTO notificaDaInviare in notificheAiResponsabili)
    //            {
    //                string html = String.Empty;
    //                string plainText = String.Empty;
    //                string[] destinatari = new string[] { notificaDaInviare.Email };

    //                CreaCorpoEmailInvioNotificaAiResponsabili(notificaDaInviare, out html, out plainText);

    //                EmailHelper.InviaEmail(oggetto, plainText, html, Infrastructure.ConfigurationKeys.MITTENTE_EMAIL_NOTIFICA_INTERVENTO, destinatari);
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Effettua il reale invio delle notifiche in base all'elenco dei dto passato come parametro
    //    /// </summary>
    //    /// <param name="oggetto"></param>
    //    /// <param name="notifiche"></param>
    //    private void EffettuaInvioDelleNotifichePerInterventiSenzaOperatori(string oggetto, List<NotificaDaInviareAdEmailSpecificaDTO> notificheInterventiSenzaOperatori)
    //    {
    //        if (notificheInterventiSenzaOperatori != null && notificheInterventiSenzaOperatori.Count > 0)
    //        {
    //            if (String.IsNullOrEmpty(oggetto))
    //            {
    //                oggetto = String.Join(" - ", Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE, "Elenco Interventi Senza Operatore");
    //            }

    //            foreach (Entities.NotificaDaInviareAdEmailSpecificaDTO notificaDaInviare in notificheInterventiSenzaOperatori)
    //            {
    //                string html = String.Empty;
    //                string plainText = String.Empty;
    //                string[] destinatari = new string[] { notificaDaInviare.Email };

    //                CreaCorpoEmailInvioNotificaInterventiSenzaOperatori(notificaDaInviare, out html, out plainText);

    //                EmailHelper.InviaEmail(oggetto, plainText, html, Infrastructure.ConfigurationKeys.MITTENTE_EMAIL_PER_NOTIFICA_INTERVENTI_SENZA_OPERATORI, destinatari);
    //            }
    //        }
    //    }


    //    /// <summary>
    //    /// Effettua il reale invio delle notifiche in base all'elenco dei dto passato come parametro
    //    /// </summary>
    //    /// <param name="oggetto"></param>
    //    /// <param name="notifiche"></param>
    //    private void EffettuaInvioDelleNotifichePerInterventiSLA(string oggetto, List<NotificaDaInviareAdEmailSpecificaDTO> notificheInterventiSLA)
    //    {
    //        if (notificheInterventiSLA != null && notificheInterventiSLA.Count > 0)
    //        {
    //            if (String.IsNullOrEmpty(oggetto))
    //            {
    //                oggetto = String.Join(" - ", Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE, "Elenco Interventi in base a SLA");
    //            }

    //            foreach (Entities.NotificaDaInviareAdEmailSpecificaDTO notificaDaInviare in notificheInterventiSLA)
    //            {
    //                string html = String.Empty;
    //                string plainText = String.Empty;
    //                string[] destinatari = new string[] { notificaDaInviare.Email };

    //                CreaCorpoEmailInvioNotificaInterventiSLA(notificaDaInviare, out html, out plainText);

    //                EmailHelper.InviaEmail(oggetto, plainText, html, Infrastructure.ConfigurationKeys.MITTENTE_EMAIL_NOTIFICA_INTERVENTO, destinatari);
    //            }
    //        }
    //    }
    //    #endregion

    //    #region Funzioni Di Analisi

    //    /// <summary>
    //    /// Effettua l'analisi delle notifiche da inviare per gli interventi non chiusi e non validati, non presi in carico dagli operatori
    //    /// </summary>
    //    private void AnalizzaNotificaInterventoApertoNonPresoInCarico()
    //    {
    //        TipologiaNotificaEnum tipologia = TipologiaNotificaEnum.InterventoApertoNonPresoInCarico;

    //        // Vengono recuperati tutti i record relativi ad intervento che aperti ma non presi incarico dagli operatori
    //        IEnumerable<Entities.Intervento_Operatore> elencoInterventiOperatoreNonPresiInCarico = llInterventiOperatori.GetInterventiNonChiusiNonValidatiNonPresiInCarico();

    //        // Nel caso in cui esistano dei record ..
    //        if (elencoInterventiOperatoreNonPresiInCarico != null && elencoInterventiOperatoreNonPresiInCarico.Count() > 0)
    //        {
    //            // Viene recuperato univocamente l'elenco degli identificativi degli interventi univoci
    //            IEnumerable<Guid> elencoIDInterventoUnivoci = elencoInterventiOperatoreNonPresiInCarico.Select(x => x.IDIntervento).Distinct();

    //            AggiungiGlobalmenteLogInvioNotificaInterventoApertoNonPresoInCarico(elencoIDInterventoUnivoci);

    //            // Viene recuperata l'elenco degli interventi non presi in carico effettuando un raggruppamento per l'id dell'operatore
    //            List<IGrouping<Entities.Operatore, Guid>> raggruppamentoInterventiRaggruppatePerOperatore =
    //                elencoInterventiOperatoreNonPresiInCarico.OrderBy(x => x.Intervento.Numero)
    //                                                         .GroupBy(key => key.Operatore, informazioneUnivocaDaRecuperare => informazioneUnivocaDaRecuperare.Intervento.ID)
    //                                                         .ToList();
    //            // Per ogni raggrupppamento ..
    //            foreach (IGrouping<Entities.Operatore, Guid> raggruppamento in raggruppamentoInterventiRaggruppatePerOperatore)
    //            {
    //                // Vengono recuperate le informazioni sull'operatore e sugli incarichi che sono stati assegnati a lui e che risultano non presi in carico
    //                Entities.Operatore operatore = raggruppamento.Key;
    //                IEnumerable<Guid> elencoUnivociInterventi = raggruppamento.Distinct();

    //                List<Entities.InformazioneNotiticaDaInviareDTO> elencoInformazioneNotiticaDaInviare = new List<InformazioneNotiticaDaInviareDTO>();
    //                List<Entities.InformazioneNotiticaDaInviareDTO> elencoInformazioneNotiticaDaInviareAiResponsabili = new List<InformazioneNotiticaDaInviareDTO>();

    //                // Viene recuperato l'elenco delle informazioni da inviare in base all'elenco degli interventi recuperati
    //                GetElencoInformazioniNotificheDaInviarePerIncarichiApertiNonPresiInCarico(elencoInterventiOperatoreNonPresiInCarico, operatore, elencoUnivociInterventi, out elencoInformazioneNotiticaDaInviare, out elencoInformazioneNotiticaDaInviareAiResponsabili);

    //                // Viene recuperata una notifica ed aggiunta all'elenco delle notifiche da inviare globalmente
    //                Entities.NotificaDaInviareDTO notificaDaInviare = new NotificaDaInviareDTO(operatore.Identifier, operatore.CognomeNome, tipologia, elencoInformazioneNotiticaDaInviare);
    //                ElencoNotificaDaInviare.Add(notificaDaInviare);

    //                if (elencoInformazioneNotiticaDaInviareAiResponsabili != null && elencoInformazioneNotiticaDaInviareAiResponsabili.Count > 0)
    //                {
    //                    List<string> elencoEmailValide = llOperatori.GetValidEmailsListResponsabileFromOperatore(operatore);
    //                    if (elencoEmailValide != null && elencoEmailValide.Count > 0)
    //                    {
    //                        foreach (string emailValida in elencoEmailValide)
    //                        {
    //                            // Viene recuperata una notifica ed aggiunta all'elenco delle notifiche da inviare globalmente
    //                            Entities.NotificaDaInviareAdEmailSpecificaDTO notificaDaInviareAiResponsabili = new NotificaDaInviareAdEmailSpecificaDTO(emailValida, "Responsabile", tipologia, elencoInformazioneNotiticaDaInviareAiResponsabili);
    //                            ElencoNotificaDaInviare.Add(notificaDaInviareAiResponsabili);
    //                        }
    //                    }
    //                }                    
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Effettua l'analisi delle notifiche da inviare per gli intervento non chiusi e non validati, che risultano non chiusi
    //    /// </summary>
    //    private void AnalizzaNotificaInterventoPresoInCaricoNonChiuso()
    //    {
    //        TipologiaNotificaEnum tipologia = TipologiaNotificaEnum.InterventoPresoInCaricoNonChiuso;

    //        // Vengono recuperati tutti i record relativi ad intervento che non risultano chiusi
    //        IEnumerable<Entities.Intervento_Operatore> elencoInterventiOperatoreNonChiusi = llInterventiOperatori.GetInterventiNonChiusiNonValidati();

    //        // Nel caso in cui esistano dei record ..
    //        if (elencoInterventiOperatoreNonChiusi != null && elencoInterventiOperatoreNonChiusi.Count() > 0)
    //        {
    //            // Viene recuperato univocamente l'elenco degli identificativi degli interventi univoci
    //            IEnumerable<Guid> elencoIDInterventoUnivoci = elencoInterventiOperatoreNonChiusi.Select(x => x.IDIntervento).Distinct();

    //            AggiungiGlobalmenteLogInvioNotificaInterventoPresoInCaricoNonChiuso(elencoIDInterventoUnivoci);

    //            // Viene recuperata l'elenco degli interventi presi in carico ma non chiusi effettuando un raggruppamento per l'id dell'operatore
    //            List<IGrouping<Entities.Operatore, Guid>> raggruppamentoInterventiRaggruppatePerOperatore =
    //                elencoInterventiOperatoreNonChiusi.OrderBy(x => x.Intervento.Numero)
    //                                                         .GroupBy(key => key.Operatore, informazioneUnivocaDaRecuperare => informazioneUnivocaDaRecuperare.Intervento.ID)
    //                                                         .ToList();

    //            // Per ogni raggrupppamento ..
    //            foreach (IGrouping<Entities.Operatore, Guid> raggruppamento in raggruppamentoInterventiRaggruppatePerOperatore)
    //            {
    //                // Vengono recuperate le informazioni sull'operatore e sugli incarichi che sono stati assegnati a lui e che risultano presi in carico ma mon chiusi
    //                Entities.Operatore operatore = raggruppamento.Key;
    //                IEnumerable<Guid> elencoUnivociInterventi = raggruppamento.Distinct();

    //                List<Entities.InformazioneNotiticaDaInviareDTO> elencoInformazioneNotiticaDaInviare = new List<InformazioneNotiticaDaInviareDTO>();
    //                List<Entities.InformazioneNotiticaDaInviareDTO> elencoInformazioneNotiticaDaInviareAiResponsabili = new List<InformazioneNotiticaDaInviareDTO>();

    //                // Viene recuperato l'elenco delle informazioni da inviare in base all'elenco degli interventi recuperati
    //                GetElencoInformazioniNotificheDaInviarePerInterventiPresiCaricoNonChiusi(elencoInterventiOperatoreNonChiusi, operatore, elencoUnivociInterventi, out elencoInformazioneNotiticaDaInviare, out elencoInformazioneNotiticaDaInviareAiResponsabili);

    //                // Viene recuperata una notifica ed aggiunta all'elenco delle notifiche da inviare globalmente
    //                Entities.NotificaDaInviareDTO notificaDaInviare = new NotificaDaInviareDTO(operatore.Identifier, operatore.CognomeNome, tipologia, elencoInformazioneNotiticaDaInviare);
    //                ElencoNotificaDaInviare.Add(notificaDaInviare);

    //                if (elencoInformazioneNotiticaDaInviareAiResponsabili != null && elencoInformazioneNotiticaDaInviareAiResponsabili.Count > 0)
    //                {
    //                    List<string> elencoEmailValide = llOperatori.GetValidEmailsListResponsabileFromOperatore(operatore);
    //                    if (elencoEmailValide != null && elencoEmailValide.Count > 0)
    //                    {
    //                        foreach (string emailValida in elencoEmailValide)
    //                        {
    //                            // Viene recuperata una notifica ed aggiunta all'elenco delle notifiche da inviare globalmente
    //                            Entities.NotificaDaInviareAdEmailSpecificaDTO notificaDaInviareAiResponsabili = new NotificaDaInviareAdEmailSpecificaDTO(emailValida, "Responsabile", tipologia, elencoInformazioneNotiticaDaInviareAiResponsabili);
    //                            ElencoNotificaDaInviare.Add(notificaDaInviareAiResponsabili);
    //                        }
    //                    }
    //                }                    
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Effettua l'analisi delle notifiche da inviare per gli interventi a cui non sono stati associati degli operatori
    //    /// </summary>
    //    private void AnalizzaNotificaInterventiSenzaOperatori()
    //    {
    //        TipologiaNotificaEnum tipologia = TipologiaNotificaEnum.InterventoSenzaAlcunOperatoreAssegnato;

    //        IEnumerable<Entities.Intervento> elencoInterventiSenzaOperatori = llInterventi.ReadSenzaOperatoriAssociati(Interventi.TipologiaLetturaInterventiCliente.Tutti);

    //        // Nel caso in cui esistano dei record ..
    //        if (elencoInterventiSenzaOperatori != null && elencoInterventiSenzaOperatori.Count() > 0)
    //        {
    //            // Viene recuperato univocamente l'elenco degli identificativi degli interventi univoci
    //            IEnumerable<Guid> elencoIDInterventoUnivoci = elencoInterventiSenzaOperatori.Select(x => x.ID).Distinct();

    //            AggiungiGlobalmenteLogInvioNotificaInterventiSenzaOperartori(elencoIDInterventoUnivoci);

    //            List<Entities.InformazioneNotiticaDaInviareDTO> elencoInformazioneNotiticaDaInviare = GetInformazioniNotificaDaInviarePerInterventoSenzaOperatore(elencoInterventiSenzaOperatori);

    //            if (Infrastructure.ConfigurationKeys.DESTINATARI_EMAIL_PER_NOTIFICA_INTERVENTI_SENZA_OPERATORI != null && Infrastructure.ConfigurationKeys.DESTINATARI_EMAIL_PER_NOTIFICA_INTERVENTI_SENZA_OPERATORI.Length > 0)
    //            {
    //                foreach (string email in Infrastructure.ConfigurationKeys.DESTINATARI_EMAIL_PER_NOTIFICA_INTERVENTI_SENZA_OPERATORI)
    //                {
    //                    // Viene recuperata una notifica ed aggiunta all'elenco delle notifiche da inviare globalmente
    //                    Entities.NotificaDaInviareAdEmailSpecificaDTO notificaDaInviareAiResponsabili = new NotificaDaInviareAdEmailSpecificaDTO(email, "Utente SE.CO.GES.", tipologia, elencoInformazioneNotiticaDaInviare);
    //                    ElencoNotificaDaInviare.AddNotificaInterventoSenzaOperatore(notificaDaInviareAiResponsabili);
    //                }
    //            }
    //        }
    //    }



    //    ///// <summary>
    //    ///// Effettua l'analisi delle notifiche da inviare agli operatori degli interventi ai quali è associata una configurazione ticket con tempistiche SLA
    //    ///// </summary>
    //    //private void AnalizzaNotificaOperatoriInterventiPerSLA() // VERSIONE PER INVIO DI UNA MAIL CON PIU' INTERVENTI
    //    //{
    //    //    //TipologiaNotificaEnum tipologia = TipologiaNotificaEnum.InterventoDaNotificareInBaseSLA;

    //    //    List<Entities.Intervento> elencoInterventi = llInterventi.ReadInterventiConSLAPerNotifiche(Interventi.TipologiaLetturaInterventi.SoloAperti);

    //    //    // Nel caso in cui ci siano interventi aperti associati a configurazioni tickets con SLA...
    //    //    if (elencoInterventi != null && elencoInterventi.Count() > 0)
    //    //    {
    //    //        Logic.RepartiUfficio llRU = new Logic.RepartiUfficio(llInterventi);
    //    //        Logic.CaratteristicheTipologieIntervento llCTI = new Logic.CaratteristicheTipologieIntervento(llInterventi);
    //    //        List<Entities.Intervento> elencoInterventoUnivoci = new List<Entities.Intervento>();
    //    //        List<Guid> elencoIDInterventoUnivoci = new List<Guid>();

    //    //        foreach (Entities.Intervento intervento in elencoInterventi)
    //    //        {
    //    //            if (intervento.ConfigurazioneTipologiaTicketCliente != null)
    //    //            {
    //    //                bool includiIntervento = false;

    //    //                //// Imposta l'eventuale flag di urgenza in base alla configurazione selezionata
    //    //                //if (intervento.ConfigurazioneTipologiaTicketCliente.CondizioneIntervento != null &&
    //    //                //    intervento.ConfigurazioneTipologiaTicketCliente.CondizioneIntervento.CondizioneInterventoEnumValue == CondizioneInterventoEnum.UrgenteBloccante)
    //    //                //{
    //    //                //    chkUrgente.Checked = true;
    //    //                //}
    //    //                //else
    //    //                //{
    //    //                //    chkUrgente.Checked = false;
    //    //                //}

    //    //                // Informazioni sugli orari del Reparto di assistenza
    //    //                Entities.RepartoUfficio repartoUfficio = llRU.Find(new Entities.EntityId<RepartoUfficio>(intervento.ConfigurazioneTipologiaTicketCliente.IdRepartoUfficio));
    //    //                //if (repartoUfficio != null)
    //    //                //{
    //    //                //    lblUfficioOrariReparto.Text = $"<strong>Orari apertura {repartoUfficio.Reparto}</strong><br />";
    //    //                //    foreach (Entities.OrarioRepartoUfficio orario in repartoUfficio.OrarioRepartoUfficios.OrderBy(o => o.Giorno).ThenBy(o => o.OrarioDalle))
    //    //                //    {
    //    //                //        string nomeGiorno = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.DayNames[orario.Giorno];
    //    //                //        string text = $"{nomeGiorno} dalle {orario.OrarioDalle} alle {orario.OrarioAlle}<br />";
    //    //                //        lblUfficioOrariReparto.Text += text;
    //    //                //    }
    //    //                //}


    //    //                // Informazioni sugli SLA dell'intervento
    //    //                IEnumerable<Entities.CaratteristicaTipologiaIntervento> caratteristicheIntervento = llCTI.Read(intervento.IDConfigurazioneTipologiaTicket.Value);
    //    //                //lblCaratteristicheTipologiaIntervento.Text = $"<strong>SLA Intervento</strong><br />";

    //    //                TimeSpan tempoPiùRestrittivo = new TimeSpan(0);

    //    //                foreach (Entities.CaratteristicaTipologiaIntervento archCar in caratteristicheIntervento)
    //    //                {
    //    //                    if (Enum.IsDefined(typeof(Entities.CaratteristicaInterventoEnum), archCar.IdCaratteristica))
    //    //                    {
    //    //                        Entities.CaratteristicaInterventoEnum ciEnum = (Entities.CaratteristicaInterventoEnum)archCar.IdCaratteristica;
    //    //                        switch (ciEnum)
    //    //                        {
    //    //                            case CaratteristicaInterventoEnum.PresaInCaricoEntroMinuti:
    //    //                            case CaratteristicaInterventoEnum.RispostaEntroMinuti:
    //    //                            case CaratteristicaInterventoEnum.RipristinoEntroMinuti:

    //    //                                includiIntervento = true;

    //    //                                string tempo = string.Empty;
    //    //                                if (!string.IsNullOrWhiteSpace(archCar.Parametri))
    //    //                                {
    //    //                                    long giorni = 0;
    //    //                                    long ore = 0;
    //    //                                    long minuti = 0;
    //    //                                    if (long.TryParse(archCar.Parametri, out minuti))
    //    //                                    {
    //    //                                        if (minuti >= 1440)
    //    //                                        {
    //    //                                            giorni = minuti / 1440;
    //    //                                            minuti = minuti % 1440;
    //    //                                            //ore = minuti % 1440;
    //    //                                            //minuti = minuti % 60;
    //    //                                        }
    //    //                                        if (minuti >= 60)
    //    //                                        {
    //    //                                            ore = minuti / 60;
    //    //                                            minuti = minuti % 60;
    //    //                                        }

    //    //                                        TimeSpan ts = new TimeSpan((int)giorni, (int)ore, (int)minuti, 0, 0);
    //    //                                        tempo = string.Format("{0:%d} giorni {0:%h} ore {0:%m} minuti", ts);
    //    //                                        if (tempoPiùRestrittivo.Ticks == 0)
    //    //                                        {
    //    //                                            tempoPiùRestrittivo = ts;
    //    //                                        }
    //    //                                        else
    //    //                                        {
    //    //                                            if (ts < tempoPiùRestrittivo) tempoPiùRestrittivo = ts;
    //    //                                        }
    //    //                                    }
    //    //                                }
    //    //                                intervento.DescrizioneSLAIntervento = $"{archCar.NomeCaratteristicaIntervento} - Tempo: {tempo}<br />";
    //    //                                //lblCaratteristicheTipologiaIntervento.Text += $"{archCar.NomeCaratteristicaIntervento} - Tempo: {tempo}<br />";
    //    //                                break;
    //    //                            default:
    //    //                                //intervento.DescrizioneSLAIntervento = $"{archCar.NomeCaratteristicaIntervento}<br />";
    //    //                                //lblCaratteristicheTipologiaIntervento.Text += $"{archCar.NomeCaratteristicaIntervento}<br />";
    //    //                                break;
    //    //                        }
    //    //                    }
    //    //                    //else
    //    //                    //{
    //    //                    //    intervento.DescrizioneSLAIntervento = $"{archCar.NomeCaratteristicaIntervento}<br />";
    //    //                    //    //lblCaratteristicheTipologiaIntervento.Text += $"{archCar.NomeCaratteristicaIntervento}<br />";
    //    //                    //}
    //    //                }

    //    //                DateTime? dataLimite = null;
    //    //                if (repartoUfficio != null)
    //    //                {
    //    //                    dataLimite = llRU.GetDataLimiteIntervento(repartoUfficio, tempoPiùRestrittivo, intervento.DataRedazione);
    //    //                    //lblDataLimite.Text = dataLimite.ToString("dddd dd MMMM yyyy alle HH:mm");
    //    //                }
    //    //                else
    //    //                {
    //    //                    //lblDataLimite.Text = tempoPiùRestrittivo.Ticks > 0 ? DateTime.Now.Add(tempoPiùRestrittivo).ToString("dddd dd MMMM yyyy alle HH:mm") : "";
    //    //                    if(tempoPiùRestrittivo.Ticks > 0)
    //    //                    {
    //    //                        dataLimite = DateTime.Now.Add(tempoPiùRestrittivo);
    //    //                    }
    //    //                }


    //    //                if(includiIntervento && dataLimite < DateTime.Now.AddHours(1))
    //    //                {
    //    //                    intervento.DescrizioneSLAIntervento += "Data Limite: " + dataLimite.Value.ToString("dddd dd MMMM yyyy alle HH:mm") + "<br/>";
    //    //                    elencoInterventoUnivoci.Add(intervento);
    //    //                    elencoIDInterventoUnivoci.Add(intervento.ID);
    //    //                }

    //    //            }

    //    //        }















    //    //        elencoIDInterventoUnivoci = elencoIDInterventoUnivoci.Distinct().ToList();

    //    //        // Viene recuperato univocamente l'elenco degli identificativi degli interventi univoci
    //    //        //IEnumerable<Guid> elencoIDInterventoUnivoci = elencoInterventi.Select(x => x.ID).Distinct();

    //    //        //AggiungiGlobalmenteLogInvioNotificaInterventiSenzaOperartori(elencoIDInterventoUnivoci);
    //    //        List<Entities.InformazioneNotiticaDaInviareDTO> elencoInformazioneNotiticaDaInviare = GetInformazioniNotificaDaInviarePerInterventoConSLA(elencoInterventoUnivoci);

    //    //        if (Infrastructure.ConfigurationKeys.DESTINATARI_EMAIL_PER_NOTIFICA_INTERVENTI_PER_SLA != null && Infrastructure.ConfigurationKeys.DESTINATARI_EMAIL_PER_NOTIFICA_INTERVENTI_PER_SLA.Length > 0)
    //    //        {
    //    //            foreach (string email in Infrastructure.ConfigurationKeys.DESTINATARI_EMAIL_PER_NOTIFICA_INTERVENTI_PER_SLA)
    //    //            {
    //    //                // Viene recuperata una notifica ed aggiunta all'elenco delle notifiche da inviare globalmente
    //    //                Entities.NotificaDaInviareAdEmailSpecificaDTO notificaDaInviareAiResponsabili = new NotificaDaInviareAdEmailSpecificaDTO(email, "Utente SE.CO.GES.", TipologiaNotificaEnum.InterventoDaNotificarePerSLA, elencoInformazioneNotiticaDaInviare);
    //    //                ElencoNotificaDaInviare.AddNotificaInterventoSLA(notificaDaInviareAiResponsabili);
    //    //            }
    //    //        }
    //    //    }
    //    //}

    //    #endregion

    //    #region Funzioni Creazione Informazioni Per Log Invio Notifiche

    //    /// <summary>
    //    /// Effettua l'aggiunta, globalmente, delle informazioni relative ai log di invio per gli interventi aperti ma non presi in carico
    //    /// </summary>
    //    /// <param name="elencoUnivociInterventi"></param>
    //    private void AggiungiGlobalmenteLogInvioNotificaInterventoApertoNonPresoInCarico(IEnumerable<Guid> elencoUnivociInterventi)
    //    {
    //        if (elencoUnivociInterventi != null && elencoUnivociInterventi.Count() > 0)
    //        {
    //            foreach (Guid idIntervento in elencoUnivociInterventi)
    //            {
    //                Entities.LogInvioNotifica entityLogNotifica = new LogInvioNotifica();
    //                entityLogNotifica.IDLegame = idIntervento;
    //                entityLogNotifica.IDTabellaLegameEnum = InfoOperazioneTabellaEnum.Intervento;
    //                entityLogNotifica.IDNotificaEnum = TipologiaNotificaEnum.InterventoApertoNonPresoInCarico;
    //                entityLogNotifica.Esito = true;
    //                entityLogNotifica.Note = null;

    //                ElencoLogInvioNotifica.Add(entityLogNotifica);
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Effettua l'aggiunta, globalmente, delle informazioni relative ai log di invio per gli interventi aperti ma non presi in carico
    //    /// </summary>
    //    /// <param name="elencoUnivociInterventi"></param>
    //    private void AggiungiGlobalmenteLogInvioNotificaInterventoPresoInCaricoNonChiuso(IEnumerable<Guid> elencoUnivociInterventi)
    //    {
    //        if (elencoUnivociInterventi != null && elencoUnivociInterventi.Count() > 0)
    //        {
    //            foreach (Guid idIntervento in elencoUnivociInterventi)
    //            {
    //                Entities.LogInvioNotifica entityLogNotifica = new LogInvioNotifica();
    //                entityLogNotifica.IDLegame = idIntervento;
    //                entityLogNotifica.IDTabellaLegameEnum = InfoOperazioneTabellaEnum.Intervento;
    //                entityLogNotifica.IDNotificaEnum = TipologiaNotificaEnum.InterventoPresoInCaricoNonChiuso;
    //                entityLogNotifica.Esito = true;
    //                entityLogNotifica.Note = null;

    //                ElencoLogInvioNotifica.Add(entityLogNotifica);
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Effettua l'aggiunta, globalmente, delle informazioni relative ai log di invio per gli interventi a cui non sono stati associati degli interventi
    //    /// </summary>
    //    /// <param name="elencoUnivociInterventi"></param>
    //    private void AggiungiGlobalmenteLogInvioNotificaInterventiSenzaOperartori(IEnumerable<Guid> elencoUnivociInterventi)
    //    {
    //        if (elencoUnivociInterventi != null && elencoUnivociInterventi.Count() > 0)
    //        {
    //            foreach (Guid idIntervento in elencoUnivociInterventi)
    //            {
    //                Entities.LogInvioNotifica entityLogNotifica = new LogInvioNotifica();
    //                entityLogNotifica.IDLegame = idIntervento;
    //                entityLogNotifica.IDTabellaLegameEnum = InfoOperazioneTabellaEnum.Intervento;
    //                entityLogNotifica.IDNotificaEnum = TipologiaNotificaEnum.InterventoSenzaAlcunOperatoreAssegnato;
    //                entityLogNotifica.Esito = true;
    //                entityLogNotifica.Note = null;

    //                ElencoLogInvioNotifica.Add(entityLogNotifica);
    //            }
    //        }
    //    }

    //    #endregion

    //    #region Funzioni Accessorie

    //    /// <summary>
    //    /// Restituisce l'elenco delle notifiche da inviare per gli incarichi che risultano non chiusi da parte di un operatore
    //    /// </summary>
    //    /// <param name="elencoInterventiOperatorePresiInCaricoNonChiusi"></param>
    //    /// <param name="operatore"></param>
    //    /// <param name="elencoUnivociInterventi"></param>
    //    /// <param name="elencoInformazioneNotiticaDaInviare"></param>
    //    /// <param name="elencoInformazioneNotiticaDaInviareAiResponsabili"></param>
    //    /// <returns></returns>
    //    private void GetElencoInformazioniNotificheDaInviarePerInterventiPresiCaricoNonChiusi(IEnumerable<Entities.Intervento_Operatore> elencoInterventiOperatorePresiInCaricoNonChiusi, Entities.Operatore operatore, IEnumerable<Guid> elencoUnivociInterventi, out List<Entities.InformazioneNotiticaDaInviareDTO> elencoInformazioneNotiticaDaInviare, out List<Entities.InformazioneNotiticaDaInviareDTO> elencoInformazioneNotiticaDaInviareAiResponsabili)
    //    {
    //        if (elencoInterventiOperatorePresiInCaricoNonChiusi == null) throw new ArgumentNullException("elencoInterventiOperatoreNonPresiInCarico", "Parametro nullo");
    //        if (operatore == null) throw new ArgumentNullException("operatore", "Parametro nullo");

    //        TipologiaNotificaEnum tipologiaNotifica = TipologiaNotificaEnum.InterventoPresoInCaricoNonChiuso;

    //        List<string> elencoEmailValide = llOperatori.GetValidEmailsListFromOperatore(operatore);
    //        bool emailNonPresenti = false;
    //        if (elencoEmailValide == null || elencoEmailValide.Count <= 0)
    //        {
    //            emailNonPresenti = true;
    //        }

    //        List<Entities.InformazioneNotiticaDaInviareDTO> elencoInformazioneNotiticaDaInviareTemp = new List<InformazioneNotiticaDaInviareDTO>();
    //        List<Entities.InformazioneNotiticaDaInviareDTO> elencoInformazioneNotiticaDaInviareAiResponsabiliTemp = new List<InformazioneNotiticaDaInviareDTO>();

    //        // Nel caso in cui esista almento un identificativo univoco ..
    //        if (elencoUnivociInterventi != null && elencoUnivociInterventi.Count() > 0)
    //        {
    //            // Per ogni identificativo presente nell'elenco ..
    //            foreach (Guid idIntervento in elencoUnivociInterventi)
    //            {
    //                if (emailNonPresenti)
    //                {
    //                    SettaEsitoNoteLogInvioNotifica(new EntityId<Intervento>(idIntervento), tipologiaNotifica, false, String.Format("Operatore '{0}' (ID '{1}'): Nessuna email associata", operatore.CognomeNome, operatore.ID));
    //                }

    //                // Viene recuperato il record relativo ad un intervento operatore necessario per recuperare l'intervento
    //                Entities.Intervento_Operatore entityInterventoOperatore = elencoInterventiOperatorePresiInCaricoNonChiusi.Where(x => x.IDIntervento == idIntervento && x.IDOperatore == operatore.ID).FirstOrDefault();

    //                // Nel caso in cui esista un record ed abbia la property relativa all'intervento valorizzata
    //                if (entityInterventoOperatore != null && entityInterventoOperatore.Intervento != null)
    //                {
    //                    // Viene recuperato l'intervento..
    //                    Entities.Intervento intervento = entityInterventoOperatore.Intervento;

    //                    // Vengono recuperate le informazioni necessarie ..
    //                    string urlIntervento = String.Format("{0}Interventi/Intervento.aspx?ID={1}", Infrastructure.ConfigurationKeys.URL_APPLICAZIONE, idIntervento);
    //                    string informazioneHTML = String.Format("<a href=\"{0}\">Intervento N.{1} - {2}</a>", urlIntervento, intervento.Numero, intervento.RagioneSociale);
    //                    string informazionPlainText = String.Format("Intervento N.{0} - {1}", intervento.Numero, intervento.RagioneSociale);

    //                    // Viene creato un dto che contiene le informazioni recuperate ed aggiunte ell'elenco delle informazioni da mandare
    //                    Entities.InformazioneNotiticaDaInviareDTO dtoInformazioneNotiticaDaInviare = new InformazioneNotiticaDaInviareDTO(informazioneHTML, informazionPlainText);
    //                    elencoInformazioneNotiticaDaInviareTemp.Add(dtoInformazioneNotiticaDaInviare);

    //                    int inviiEffettuati = llLogInvioNotifiche.GetCountInviiEffettuati<Entities.Intervento>(intervento.Identifier, tipologiaNotifica);
    //                    if (inviiEffettuati >= 1)
    //                    {
    //                        elencoInformazioneNotiticaDaInviareAiResponsabiliTemp.Add(dtoInformazioneNotiticaDaInviare);
    //                    }
    //                }
    //            }
    //        }

    //        elencoInformazioneNotiticaDaInviare = elencoInformazioneNotiticaDaInviareTemp;
    //        elencoInformazioneNotiticaDaInviareAiResponsabili = elencoInformazioneNotiticaDaInviareAiResponsabiliTemp;
    //    }

    //    /// <summary>
    //    /// Restituisce l'elenco delle notifiche da inviare per gli incarichi non presi in carico da parte di un operatore
    //    /// </summary>
    //    /// <param name="elencoInterventiOperatoreNonPresiInCarico"></param>
    //    /// <param name="operatore"></param>
    //    /// <param name="elencoUnivociInterventi"></param>
    //    /// <param name="elencoInformazioneNotiticaDaInviare"></param>
    //    /// <param name="elencoInformazioneNotiticaDaInviareAiResponsabili"></param>
    //    /// <returns></returns>
    //    private void GetElencoInformazioniNotificheDaInviarePerIncarichiApertiNonPresiInCarico(IEnumerable<Entities.Intervento_Operatore> elencoInterventiOperatoreNonPresiInCarico, Entities.Operatore operatore, IEnumerable<Guid> elencoUnivociInterventi, out List<Entities.InformazioneNotiticaDaInviareDTO> elencoInformazioneNotiticaDaInviare, out List<Entities.InformazioneNotiticaDaInviareDTO> elencoInformazioneNotiticaDaInviareAiResponsabili)
    //    {
    //        if (elencoInterventiOperatoreNonPresiInCarico == null) throw new ArgumentNullException("elencoInterventiOperatoreNonPresiInCarico", "Parametro nullo");
    //        if (operatore == null) throw new ArgumentNullException("operatore", "Parametro nullo");

    //        List<string> elencoEmailValide = llOperatori.GetValidEmailsListFromOperatore(operatore);
    //        bool emailNonPresenti = false;
    //        if (elencoEmailValide == null || elencoEmailValide.Count <= 0)
    //        {
    //            emailNonPresenti = true;
    //        }

    //        List<Entities.InformazioneNotiticaDaInviareDTO> elencoInformazioneNotiticaDaInviareTemp = new List<InformazioneNotiticaDaInviareDTO>();
    //        List<Entities.InformazioneNotiticaDaInviareDTO> elencoInformazioneNotiticaDaInviareAiResponsabiliTemp = new List<InformazioneNotiticaDaInviareDTO>();

    //        // Nel caso in cui esista almento un identificativo univoco ..
    //        if (elencoUnivociInterventi != null && elencoUnivociInterventi.Count() > 0)
    //        {
    //            TipologiaNotificaEnum tipologiaNotifica = TipologiaNotificaEnum.InterventoApertoNonPresoInCarico;

    //            // Per ogni identificativo presente nell'elenco ..
    //            foreach (Guid idIntervento in elencoUnivociInterventi)
    //            {
    //                if (emailNonPresenti)
    //                {
    //                    SettaEsitoNoteLogInvioNotifica(new EntityId<Intervento>(idIntervento), tipologiaNotifica, false, String.Format("Operatore '{0}' (ID '{1}'): Nessuna email associata", operatore.CognomeNome, operatore.ID));
    //                }

    //                // Viene recuperato il record relativo ad un intervento operatore necessario per recuperare l'intervento
    //                Entities.Intervento_Operatore entityInterventoOperatore = elencoInterventiOperatoreNonPresiInCarico.Where(x => x.IDIntervento == idIntervento && x.IDOperatore == operatore.ID).FirstOrDefault();

    //                // Nel caso in cui esista un record ed abbia la property relativa all'intervento valorizzata
    //                if (entityInterventoOperatore != null && entityInterventoOperatore.Intervento != null)
    //                {
    //                    // Viene recuperato l'intervento..
    //                    Entities.Intervento intervento = entityInterventoOperatore.Intervento;

    //                    // Vengono recuperate le informazioni necessarie ..
    //                    string urlIntervento = String.Format("{0}Interventi/Intervento.aspx?ID={1}", Infrastructure.ConfigurationKeys.URL_APPLICAZIONE, idIntervento);
    //                    string informazioneHTML = String.Format("<a href=\"{0}\">Intervento N.{1} - {2}</a>", urlIntervento, intervento.Numero, intervento.RagioneSociale);
    //                    string informazionPlainText = String.Format("Intervento N.{0} - {1}", intervento.Numero, intervento.RagioneSociale);

    //                    // Viene creato un dto che contiene le informazioni recuperate ed aggiunte ell'elenco delle informazioni da mandare
    //                    Entities.InformazioneNotiticaDaInviareDTO dtoInformazioneNotiticaDaInviare = new InformazioneNotiticaDaInviareDTO(informazioneHTML, informazionPlainText);
    //                    elencoInformazioneNotiticaDaInviareTemp.Add(dtoInformazioneNotiticaDaInviare);

    //                    int inviiEffettuati = llLogInvioNotifiche.GetCountInviiEffettuati<Entities.Intervento>(intervento.Identifier, tipologiaNotifica);
    //                    if (inviiEffettuati >= 1)
    //                    {
    //                        elencoInformazioneNotiticaDaInviareAiResponsabiliTemp.Add(dtoInformazioneNotiticaDaInviare);
    //                    }
    //                }
    //            }
    //        }

    //        // Viene restituito l'elenco delle informazioni delle notifiche da inviare
    //        elencoInformazioneNotiticaDaInviare = elencoInformazioneNotiticaDaInviareTemp;
    //        elencoInformazioneNotiticaDaInviareAiResponsabili = elencoInformazioneNotiticaDaInviareAiResponsabiliTemp;
    //    }

    //    /// <summary>
    //    /// Restituisce l'elenco delle informazioni da inviare per gli interventi senza operatori
    //    /// </summary>
    //    /// <param name="elencoInterventiSenzaOperatori"></param>
    //    /// <returns></returns>
    //    private List<Entities.InformazioneNotiticaDaInviareDTO> GetInformazioniNotificaDaInviarePerInterventoSenzaOperatore(IEnumerable<Entities.Intervento> elencoInterventiSenzaOperatori)
    //    {
    //        List<Entities.InformazioneNotiticaDaInviareDTO> elencoInformazioneNotiticaDaInviareTemp = new List<InformazioneNotiticaDaInviareDTO>();

    //        if (elencoInterventiSenzaOperatori != null && elencoInterventiSenzaOperatori.Count() > 0)
    //        {
    //            foreach (Entities.Intervento intervento in elencoInterventiSenzaOperatori)
    //            {
    //                // Vengono recuperate le informazioni necessarie ..
    //                string urlIntervento = String.Format("{0}Interventi/Intervento.aspx?ID={1}", Infrastructure.ConfigurationKeys.URL_APPLICAZIONE, intervento.ID);
    //                string informazioneHTML = String.Format("<a href=\"{0}\">Intervento N.{1} - {2}</a>", urlIntervento, intervento.Numero, intervento.RagioneSociale);
    //                string informazionPlainText = String.Format("Intervento N.{0} - {1}", intervento.Numero, intervento.RagioneSociale);

    //                // Viene creato un dto che contiene le informazioni recuperate ed aggiunte ell'elenco delle informazioni da mandare
    //                Entities.InformazioneNotiticaDaInviareDTO dtoInformazioneNotiticaDaInviare = new InformazioneNotiticaDaInviareDTO(informazioneHTML, informazionPlainText);
    //                elencoInformazioneNotiticaDaInviareTemp.Add(dtoInformazioneNotiticaDaInviare);
    //            }
    //        }

    //        return elencoInformazioneNotiticaDaInviareTemp;
    //    }

    //    /// <summary>
    //    /// Restituisce l'elenco delle informazioni da inviare per gli interventi senza operatori
    //    /// </summary>
    //    /// <param name="elencoInterventi"></param>
    //    /// <returns></returns>
    //    private List<Entities.InformazioneNotiticaDaInviareDTO> GetInformazioniNotificaDaInviarePerInterventoConSLA(IEnumerable<Entities.Intervento> elencoInterventi)
    //    {
    //        List<Entities.InformazioneNotiticaDaInviareDTO> elencoInformazioneNotiticaDaInviareTemp = new List<InformazioneNotiticaDaInviareDTO>();

    //        if (elencoInterventi != null && elencoInterventi.Count() > 0)
    //        {
    //            foreach (Entities.Intervento intervento in elencoInterventi)
    //            {
    //                // Vengono recuperate le informazioni necessarie ..
    //                string urlIntervento = String.Format("{0}Interventi/Intervento.aspx?ID={1}", Infrastructure.ConfigurationKeys.URL_APPLICAZIONE, intervento.ID);
    //                string informazioneHTML = String.Format("<a href=\"{0}\">Intervento N.{1} - {2}</a><br/>Data Redazione: {3}<br/>SLA: {4}", urlIntervento, intervento.Numero, intervento.RagioneSociale, intervento.DataRedazione, intervento.DescrizioneSLAIntervento);
    //                string informazionPlainText = String.Format("Intervento N.{0} - {1} - SLA: {2}", intervento.Numero, intervento.RagioneSociale, intervento.DescrizioneSLAIntervento);

    //                // Viene creato un dto che contiene le informazioni recuperate ed aggiunte ell'elenco delle informazioni da mandare
    //                Entities.InformazioneNotiticaDaInviareDTO dtoInformazioneNotiticaDaInviare = new InformazioneNotiticaDaInviareDTO(informazioneHTML, informazionPlainText);
    //                elencoInformazioneNotiticaDaInviareTemp.Add(dtoInformazioneNotiticaDaInviare);
    //            }
    //        }

    //        return elencoInformazioneNotiticaDaInviareTemp;
    //    }

    //    /// <summary>
    //    /// Effettua la persistenza dei log relativi all'invio delle notifiche
    //    /// </summary>
    //    /// <param name="submitChanges"></param>
    //    private void PersistiElencoLogInvioNotificaSeEsistenti(bool submitChanges)
    //    {
    //        if (ElencoLogInvioNotifica != null && ElencoLogInvioNotifica.Count > 0)
    //        {
    //            foreach(LogInvioNotifica entityLogInvioNotifica in ElencoLogInvioNotifica)
    //            {
    //                entityLogInvioNotifica.Data = DateTime.Now;
    //                llLogInvioNotifiche.Create(entityLogInvioNotifica, submitChanges);
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Effettua il settaggio del campo esito e note del log della notifica da memorizzare nel database
    //    /// </summary>
    //    /// <param name="identificativoIntervento"></param>
    //    /// <param name="tipologiaNotifica"></param>
    //    /// <param name="esito"></param>
    //    /// <param name="note"></param>
    //    private void SettaEsitoNoteLogInvioNotifica(EntityId<Intervento> identificativoIntervento, TipologiaNotificaEnum tipologiaNotifica, bool esito, string note)
    //    {
    //        LogInvioNotifica entityLogNotifica = ElencoLogInvioNotifica.Where(x => x.IDLegame == identificativoIntervento.Value && x.IDNotificaEnum == tipologiaNotifica).FirstOrDefault();
    //        if (entityLogNotifica != null)
    //        {
    //            entityLogNotifica.Esito = esito;
    //            entityLogNotifica.Note = note;
    //        }
    //    }

    //    #endregion
    //}
}
