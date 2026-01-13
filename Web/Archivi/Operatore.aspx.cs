using SeCoGes.Utilities;
using SeCoGEST.Entities;
using SeCoGEST.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SeCoGEST.Web.Archivi
{
    public partial class Operatore : System.Web.UI.Page
    {
        #region Properties

        /// <summary>
        /// Recupera o setta la property Enabled degli oggetti nell'usercontrol
        /// </summary>
        private bool Enabled
        {
            get
            {
                if (ViewState["Enabled"] == null)
                {
                    ViewState["Enabled"] = true;
                }

                return (bool)ViewState["Enabled"];
            }
            set
            {
                ViewState["Enabled"] = value;
                AbilitaCampi(value);
            }
        }

        /// <summary>
        /// Restituisce l'ID corrente
        /// </summary>
        protected EntityId<Entities.Operatore> currentID
        {
            get
            {
                if (Request.QueryString["ID"] != null)
                    return new EntityId<Entities.Operatore>(Request.QueryString["ID"]);
                else
                    return EntityId<Entities.Operatore>.Empty;
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante Nuovo della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_Nuovo
        {
            get
            {
                return RadToolBar1.FindItemByValue("Nuovo");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al separatore del pulsante Salva
        /// </summary>
        RadToolBarItem PulsanteToolbar_SeparatoreSalva
        {
            get
            {
                return RadToolBar1.FindItemByValue("SeparatoreSalva");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante Salva della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_Salva
        {
            get
            {
                return RadToolBar1.FindItemByValue("Salva");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante Aggiorna della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_Aggiorna
        {
            get
            {
                return RadToolBar1.FindItemByValue("Aggiorna");
            }
        }

        #endregion

        #region Intercettazione Eventi

        /// <summary>
        /// Metodo di gestione dell'evento Load della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CaricaAutorizzazioni();

            if (!Helper.Web.IsPostOrCallBack(this))
            {
                try
                {
                    SeCoGes.Logging.LogManager.AddLogAccessi(String.Format("Accesso alla pagina '{0}'.", Request.Url.AbsolutePath));

                    Logic.Operatori ll = new Logic.Operatori();
                    Entities.Operatore entityToShow = ll.Find(currentID);
                    
                    if (entityToShow != null)
                    {
                        ShowData(entityToShow);
                    }
                    else
                    {
                        ShowData(new Entities.Operatore());
                    }

                    ApplicaAutorizzazioni();
                }
                catch (Exception ex)
                {
                    SeCoGes.Logging.LogManager.AddLogErrori(ex);
                    MessageHelper.ShowErrorMessage(this, ex.Message);
                }
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento PreRender della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ApplicaAutorizzazioni();

            // Dopo tutto, eseguo il PreRender definito nella BaseWebUIPage che applica le eventuali restrizioni di accesso ai controlli della pagina
            //base.Page_PreRender(sender, e);
        }

        /// <summary>
        /// Metodo di gestione dell'evento ButtonClick relativo alla toolbar presente nella pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadToolBar1_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {
            try
            {
                RadToolBarButton clickedButton = (RadToolBarButton)e.Item;

                switch (clickedButton.CommandName)
                {
                    case "Salva":
                        SaveData();
                        break;

                    default:
                        break;
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        #region rgGriglia

        /// <summary>
        /// Metodo di gestione dell'evento ItemCommand della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGriglia_PreRender(object sender, EventArgs e)
        {
            TelerikRadGridHelper.ApplicaTraduzioneDaFileDiResource(rgGriglia);
        }
        
        /// <summary>
        /// Metodo di gestione dell'evento NeedDataSource della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGriglia_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Logic.AccountsOperatori llAccountsOperatori = new Logic.AccountsOperatori();
                //IQueryable<Entities.Account> elencoOperatori = llAccountsOperatori.ReadAccount(new EntityId<Entities.Operatore>(currentID));

                IQueryable<Entities.AccountOperatore> elencoAccountOperatori = llAccountsOperatori.Read().Where(x => x.IDOperatore == currentID.Value);
                rgGriglia.DataSource = elencoAccountOperatori;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento ItemCreated della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGriglia_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            //Traduce le voci del menu di filtro della griglia. Questa operazione deve essere fatta ad ogni post
            TelerikHelper.TraduciElementiGriglia(e);
        }

        /// <summary>
        /// Metodo di gestione dell'evento ItemDataBound della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGriglia_ItemDataBound(object sender, GridItemEventArgs e)
        {
            // Inserisce gli attributi necessari per la trasformazione del layout della griglia per dispositivi mobili
            TelerikRadGridHelper.ManageColumnContentOnMobileLayout(rgGriglia, e);
        }

        #endregion

        #endregion

        #region Metodi di gestione

        /// <summary>
        /// Mostra nell'interfaccia i dati dell'entity passata
        /// </summary>
        /// <param name="entity"></param>
        private void ShowData(Entities.Operatore entityToShow)
        {
            ApplicaRedirectPulsanteAggiorna();

            if (entityToShow == null)
            {
                throw new ArgumentNullException("Parametro nullo", "entityToShow");
            }
            
            if (!entityToShow.ID.Equals(Guid.Empty))
            {
                lblTitolo.Text = String.Format("Ruolo {0}", entityToShow.CognomeNome);                
            }
            else
            {
                lblTitolo.Text = "Nuovo Ruolo";
                entityToShow.Attivo = true;
                lrGrigliaAccountAssociati.Visible = false;
            }

            this.Title = lblTitolo.Text;

            rtbCognome.Text = entityToShow.Cognome;
            rtbNome.Text = entityToShow.Nome;
            rtbEmailResponsabile.Text = entityToShow.EmailResponsabile;
            chkArea.Checked = entityToShow.Area;
            chkDisabilitato.Checked = (!entityToShow.Attivo);
        }

        /// <summary>
        ///  Memorizza i dati presenti nei controlli dell'interfaccia
        /// </summary>
        /// <param name="reloadPageAfterSave"></param>
        private void SaveData(bool reloadPageAfterSave = true)
        {
            // Valida i dati nella pagina
            MessagesCollector erroriDiValidazione = ValidaDati();            
            if (erroriDiValidazione.HaveMessages)
            {
                // Se qualcosa non va bene mostro un avviso all'utente
                MessageHelper.ShowErrorMessage(this, erroriDiValidazione.ToString("<br />"));
                return;
            }

            // Definisco una variabile per memorizzare l'entità da salvare
            Entities.Operatore entityToSave = null;

            // Definisco una variabile che conterrà l'Codice dell'entity salvata. 
            // Se per qualche motivo l'entity non viene salvata allora entityId rimarrà String.Empty
            string entityId = String.Empty;
            Logic.Operatori ll = new Logic.Operatori();

            try
            {
                ll.StartTransaction();

                //Se currentID contiene un Codice allora cerco l'entity nel database
                if (currentID.HasValue)
                {
                    entityToSave = ll.Find(currentID);
                }

                // Definisco una variabile che indica se si deve compiere una azione di creazione di una nuova entity o di modifica
                bool nuovo = false;

                if (entityToSave == null)
                {
                    if (currentID.HasValue)
                    {
                        // Se CurrentId ha un valore ma entityToSave è null allora vuol dire che la pagina è stata aperta
                        // per modificare un entità che adesso non esiste più nella base dati.
                        // In questo caso avviso l'utente
                        throw new Exception(
                            "L'Operatore che si sta variando non esiste più in archivio."
                                + "\n\rOperazione annullata!");
                    }

                    // Al contrario, se entityToSave è nulla e CurrentId è vuota
                    // vuol dire che la pagina è stata aperta per la creazione di una nuova entità
                    nuovo = true;
                    entityToSave = new Entities.Operatore();

                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriDallaView(entityToSave);


                    //Creo nuova entity
                    ll.Create(entityToSave, false);
                }
                else
                {
                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriDallaView(entityToSave);
                }
                
                // Persisto le modifiche sulla base dati nella transazione
                ll.SubmitToDatabase();

                // Persisto le modifiche sulla base dati effettuando il commit delle modifiche apportate nella transazione
                ll.CommitTransaction();

                SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - {1} l'entity Operatore con Cognome '{2}' e Nome '{3}'.", Request.Url.AbsolutePath, ((nuovo) ? "Creato" : "Salvato"), entityToSave.Cognome, entityToSave.Nome));

                // Memorizzo l'Codice dell'entità
                entityId = entityToSave.ID.ToString();
            }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);

                ll.RollbackTransaction();

                // ...e mostro il messaggio d'errore all'utente
                MessageHelper.ShowErrorMessage(this, ex.Message);
            }
            finally
            {
                // Alla fine, se il salvataggio è andato a buon fine (entityId != Guid.Empty)
                // allora ricarico la pagina aprendola in modifica
                if (entityId != String.Empty && reloadPageAfterSave)
                {
                    Helper.Web.ReloadPage(this, entityId);
                }
            }
        }
        
        #endregion

        #region Funzioni Accessorie
        
        /// <summary>
        /// Abilita o disabilita i campi dell'interfaccia in base al parametro passato
        /// </summary>
        /// <param name="abilitaCampi"></param>
        private void AbilitaCampi(bool enabled)
        {
            PulsanteToolbar_SeparatoreSalva.Visible = enabled;
            PulsanteToolbar_Salva.Visible = enabled;

            rtbCognome.ReadOnly = !enabled;
            rtbNome.ReadOnly = !enabled;
            rtbEmailResponsabile.ReadOnly = !enabled;
            chkArea.Enabled = enabled;
            chkDisabilitato.Enabled = enabled;
        }

        /// <summary>
        /// Assegna l'indirizzo della pagina da aprire al pulsante Aggiorna.
        /// L'indirizzo è esattamente lo stesso della pagina aperta.
        /// </summary>
        private void ApplicaRedirectPulsanteAggiorna()
        {
            if (PulsanteToolbar_Aggiorna != null)
                ((RadToolBarButton)PulsanteToolbar_Aggiorna).NavigateUrl = Request.Url.ToString();
        }

        /// <summary>
        /// Legge i dati inseriti nell'interfaccia e li inserisce nell'entity passata
        /// </summary>
        /// <param name="entityToFill"></param>
        public void EstraiValoriDallaView(Entities.Operatore entityToFill)
        {
            if (entityToFill == null)
            {
                throw new ArgumentNullException("Parametro nullo", "entityToFill");
            }

            entityToFill.Cognome = rtbCognome.Text.ToTrimmedString();
            entityToFill.Nome = rtbNome.Text.ToTrimmedString();
            entityToFill.EmailResponsabile = rtbEmailResponsabile.Text.ToTrimmedString();
            entityToFill.Area = chkArea.Checked;
            entityToFill.Attivo = (!chkDisabilitato.Checked);
        }

        /// <summary>
        /// Restituisce un oggetto contenente gli eventuali errori di validazione dei dati
        /// </summary>
        /// <returns></returns>
        private MessagesCollector ValidaDati(Logic.Operatori llOperatori = null)
        {
            MessagesCollector messaggi = new MessagesCollector();

            // Errori dei validatori client
            Page.Validate();
            if (!Page.IsValid)
            {
                foreach (IValidator validatore in Page.Validators)
                {

                    if (!validatore.IsValid)
                    {
                        messaggi.Add(validatore.ErrorMessage);
                    }
                }
                if (messaggi.HaveMessages) return messaggi;
            }

            if (!String.IsNullOrEmpty(rtbEmailResponsabile.Text))
            {
                string valoreTextBoxEmail = rtbEmailResponsabile.Text.ToTrimmedString();
                string[] emailInserite = valoreTextBoxEmail.Split(new string[] { EmailHelper.SEPARATORE_EMAIL }, StringSplitOptions.RemoveEmptyEntries);

                List<string> elencoEmailValide = EmailHelper.GetEmailsValidList(valoreTextBoxEmail, EmailHelper.SEPARATORE_EMAIL);
                if (elencoEmailValide != null && 
                    elencoEmailValide.Count > 0 && 
                    elencoEmailValide.Count != emailInserite.Length)
                {
                    messaggi.Add("E' necessario verificare la validità delle email inserite in quanto non risultano tutte valide.");
                }
            }

            if (llOperatori == null)
            {
                llOperatori = new Logic.Operatori();
            }

            EntityString<Entities.Operatore> cognomeToFind = new EntityString<Entities.Operatore>(rtbCognome.Text.ToTrimmedString());
            EntityString<Entities.Operatore> nomeToFind = new EntityString<Entities.Operatore>(rtbNome.Text.ToTrimmedString());
            Entities.Operatore entityOperatore = llOperatori.Find(cognomeToFind, nomeToFind);

            if (entityOperatore != null && (!currentID.HasValue || currentID.Value != entityOperatore.Identifier.Value))
            {
                messaggi.Add(String.Format("L'operatore '{0}' è già persente nella fonte dati.", entityOperatore.CognomeNome));
            }

            return messaggi;
        }
        
        #endregion

        #region Gestione delle autorizzazioni

        //private Entities.ApplicazioneAccount utenteCollegato;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneAziende;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneDisattivazioneAzienda;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneAnniAccademici;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestionePeriodiAccademiciPerAnnoAccademico;

        /// <summary>
        /// Carica gli oggetti contenenti le informazioni di accesso ai dati ed alle funzionalità esposte dalla pagina
        /// </summary>
        private void CaricaAutorizzazioni()
        {
            //try
            //{
            //    InformazioniAccountAutenticato infoAccount = InformazioniAccountAutenticato.GetIstance();
            //    utenteCollegato = infoAccount.Account;

            //    Autorizzazioni_GestioneAziende = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneAziende);
            //    Autorizzazioni_GestioneDisattivazioneAzienda = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneDisattivazioneAzienda);
            //    Autorizzazioni_GestioneAnniAccademici = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneAnniAccademici);
            //    Autorizzazioni_GestionePeriodiAccademiciPerAnnoAccademico = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestionePeriodiAccademiciPerAnnoAccademico);
            //}
            //catch (Exception ex)
            //{
            //    SeCoGes.Logging.LogManager.AddLogErrori(ex);
            //    MessageHelper.ShowErrorMessage(this, ex);
            //}
        }

        /// <summary>
        /// Blocca o permette l'accesso ai dati ed applica agli oggetti dell'interfaccia lo stile in base alle regole di accesso dell'azienda corrente
        /// </summary>
        private void ApplicaAutorizzazioni()
        {
            //if (Autorizzazioni_GestioneAziende == null) throw new ArgumentNullException("Autorizzazioni_GestioneAziende");
            //if (Autorizzazioni_GestioneDisattivazioneAzienda == null) throw new ArgumentNullException("Autorizzazioni_GestioneDisattivazioneAzienda");


            //// Applicazione regole di accesso di VISIBILITA' 
            //if (!Autorizzazioni_GestioneAziende.Consenti_Visibilità)
            //{
            //    MessageHelper.RedirectToErrorPageWithMessage(ErrorMessage.ACCESSO_NEGATO_MESSAGE);
            //    return;
            //}
            //else if (!currentID.HasValue && !Autorizzazioni_GestioneAziende.Consenti_Inserimento) // Applicazione regole di accesso di INSERIMENTO
            //{
            //    MessageHelper.RedirectToErrorPageWithMessage(ErrorMessage.UTENTE_OPERAZIONE_NON_CONSENTITA_MESSAGE);
            //    return;
            //}

            //// Applicazione regole di accesso di INSERIMENTO
            //if (!Autorizzazioni_GestioneAziende.Consenti_Inserimento)
            //{
            //    PulsanteToolbar_Nuovo.Enabled = false;
            //}

            //// Applicazione regole accesso in MODIFICA ed accesso a FUNZIONALITA' di disattivazione dell'azienda
            //if (!Autorizzazioni_GestioneDisattivazioneAzienda.Consenti_Funzionalità)
            //{
            //    chkDisattivato.Enabled = false;
            //}

            //// Applicazione regole accesso in MODIFICA ed accesso a FUNZIONALITA'
            //if (currentID.HasValue && !Autorizzazioni_GestioneAziende.Consenti_Modifica)
            //{
            //    PulsanteToolbar_Salva.Enabled = false;
            //    this.Enabled = false;
            //}

            //// Vengono gestiti i permessi relativi alla gestione degli anni accademici
            //GestisciPermessiAnniAccademici();

            //// Applicazione regole accesso in SOLA LETTURA
            //if (utenteCollegato.SolaLettura.HasValue && utenteCollegato.SolaLettura.Value)
            //{
            //    this.Enabled = false;
            //}
        }

        #endregion

        protected void chkInviaNotifiche_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkInviaNotifiche = sender as CheckBox;
                string IDAccount = ((sender as CheckBox).NamingContainer as GridDataItem).GetDataKeyValue("IDAccount").ToString();
                string IDOperatore = ((sender as CheckBox).NamingContainer as GridDataItem).GetDataKeyValue("IDOperatore").ToString();
                if (!string.IsNullOrEmpty(IDAccount) && !string.IsNullOrEmpty(IDOperatore))
                {
                    Logic.AccountsOperatori llAccountsOperatori = new Logic.AccountsOperatori();
                    Entities.AccountOperatore ao = llAccountsOperatori.Find(new EntityId<Entities.Account>(IDAccount), new EntityId<Entities.Operatore>(IDOperatore));
                    if (ao != null)
                    {
                        ao.InviaNotifiche = chkInviaNotifiche.Checked;
                        llAccountsOperatori.SubmitToDatabase();
                    }
                }
            }
            catch { }
        }
    }
}