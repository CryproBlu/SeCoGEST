using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using SeCoGEST.Infrastructure;
using SeCoGEST.Helper;
using SeCoGes.Utilities;
using Telerik.Web.UI;
using System.Web.UI.WebControls;

namespace SeCoGEST.Web.Sicurezza
{
    public partial class Account : System.Web.UI.Page
    {
        #region Costanti

        protected const string RAD_BUTTON_MODIFICA_PASSWORD_ONCLIENTCLICKED_JS_FUNCTION_NAME = "rbModificaPassword_OnClientClicked";
        private const string COGNOME_NOME_OPERATORE_GRID_OPERATORI_DA_ASSOCIARE_KEY = "CognomeNomeOperatore";

        #endregion

        #region Properties

        /// <summary>
        /// Restituisce l'id dell'entity correntemente gestita
        /// </summary>
        private Entities.EntityId<Entities.Account> CurrentId
        {
            get
            {
                if (Request.QueryString["ID"] != null)
                    return new Entities.EntityId<Entities.Account>(Request.QueryString["ID"]);
                else
                {
                    return Entities.EntityId<Entities.Account>.Empty;
                }
            }
        }

        /// <summary>
        /// Restituisce l'indice della Tab correntemente attiva
        /// </summary>
        private int CurrentTab
        {
            get
            {
                if (Request.QueryString["Tab"] != null)
                {
                    int tabIndex;
                    if (int.TryParse(Request.QueryString["Tab"], out tabIndex))
                    {
                        return tabIndex;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Imposta o restituisce un valore che indica se gli oggetti della pagina sono attivi o meno
        /// </summary>
        bool Enabled
        {
            get
            {
                if (ViewState["Enabled"] == null) Enabled = true;
                return (bool)ViewState["Enabled"];
            }
            set
            {
                ViewState["Enabled"] = value;

                // Attivo/Disattivo i controlli della view in base al valore della property Enabled
                rtbUserName.ReadOnly = !value;
                rtbNominativo.ReadOnly = !value;
                rtbEmail.ReadOnly = !value;
                chkAmministratore.Enabled = value;
                chkValidatore.Enabled = value;
                chkSolaLettura.Enabled = value;
                chkBloccaUtente.Enabled = value;

                rbModificaPassword.Visible = value;
                //rlbGruppi.Enabled = value;

                PulsanteToolbar_Salva.Enabled = value;
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
        /// Restituisce un riferimento al pulsante Torna all'elenco della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_TornaElenco
        {
            get
            {
                return RadToolBar1.FindItemByValue("TornaElenco");
            }
        }

        private bool IsAmministratore
        {
            get
            {
                if (ViewState["IsAmministratore"] == null) IsAmministratore = false;
                return (bool)ViewState["IsAmministratore"];
            }
            set
            {
                ViewState["IsAmministratore"] = value;
            }
        }

        private Unit ComboOperatoriGrigliaOperatoriDaAssociareWidth
        {
            get
            {
                return new Unit(500, UnitType.Pixel);
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
            try
            {
                // Esegue l'autenticazione e la registrazione delle autorizzazioni di accesso alle funzionalità dell pagina
                //if (!this.IsCallback) GestisciAutorizzazioniBase();
                CaricaAutorizzazioni();

                //Traduce le voci del menu di filtro della griglia. Questa operazione deve essere fatta ad ogni post
                TelerikHelper.TraduciMenuFiltro(rgGrigliaOperatoriAssociati.FilterMenu);
                
                //Se non è un postback oppure un callback...
                if (!Helper.Web.IsPostOrCallBack(this))
                {
                    RadPersistenceHelper.LoadState(this);
                    TelerikRadGridHelper.ManageExelExportSettings(rgGrigliaOperatoriAssociati, "Elenco_Operatori", true);

                    SeCoGes.Logging.LogManager.AddLogAccessi(String.Format("Accesso alla pagina '{0}'.", Request.Url.AbsolutePath));

                    rbModificaPassword.OnClientClicked = RAD_BUTTON_MODIFICA_PASSWORD_ONCLIENTCLICKED_JS_FUNCTION_NAME;

                    // Seleziona la Tab specificata nella querystring
                    rtsAccount.SelectedIndex = CurrentTab;
                    rmpAccount.SelectedIndex = rtsAccount.SelectedIndex;

                    Logic.Sicurezza.Accounts llT = new Logic.Sicurezza.Accounts();
                    Entities.Account entityToShow = llT.Find(CurrentId.Value);

                    if (entityToShow != null)
                    {
                        ShowData(entityToShow);
                    }
                    else
                    {
                        ShowData(new Entities.Account());
                        rtsAccount.Tabs[1].Visible = false;
                        //GestisciAutorizzazioniInserimento();
                    }
                }
                else
                {
                    // Registrazione degli script necessari ai filtri
                    TelerikRadGridHelper.InjectScriptToHiddenFilterItemIfEmpty(this, rgGrigliaOperatoriAssociati);
                }
            }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);
                MessageHelper.ShowErrorMessage(this, ex.Message);
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento PreRender della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            // Ad ogni post resetto la variabile che indica la modifica della password
            // Questo perchè gestita tramite tags html e codice javascript che vengono annullati ad ogni post
            RadAjaxManager ram = MasterPageHelper.GetRadAjaxManager(this);
            if (!this.Page.IsCallback || (ram != null && !ram.IsAjaxRequest)) hdPasswordModificata.Value = "0";

            ApplicaAutorizzazioni();

            // Dopo tutto, eseguo il PreRender definito nella BaseWebUIPage che applica le eventuali restrizioni di accesso ai controlli della pagina
            //base.Page_PreRender(sender, e);
        }

        /// <summary>
        /// Metodo di gestione dell'evento Click della toolbar presente nella pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadToolBar1_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
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

        protected void chkAmministratore_CheckedChanged(object sender, EventArgs e)
        {
            bool nuovo = CurrentId.Equals(Guid.Empty);
        }

        /// <summary>
        /// Metodo di gestione dell'evento relativo al combo del cliente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rcbCliente_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                string testoRicerca = (e.Text == null) ? String.Empty : e.Text.ToLower();
                RadComboBox combo = (RadComboBox)sender;
                combo.Items.Clear();

                //Carica tutti i Clienti accessibili dall'account corrente e che contengono il testo digitato dall'utente
                Logic.Metodo.AnagraficheClienti ll = new Logic.Metodo.AnagraficheClienti();
                IQueryable<Entities.AnagraficaClienti> queryBase = ll.Read();

                if (!string.IsNullOrWhiteSpace(testoRicerca))
                {
                    queryBase = queryBase.Where(x => x.DSCCONTO1.ToLower().Contains(testoRicerca) ||
                                                x.DSCCONTO2.ToLower().Contains(testoRicerca) ||
                                                x.CODCONTO.ToLower().Contains(testoRicerca));
                }

                int itemsPerRequest = (combo.ItemsPerRequest <= 0) ? 20 : combo.ItemsPerRequest;
                int itemOffset = e.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                int numTotaleUtenti = queryBase.Count();


                if (endOffset > numTotaleUtenti)
                    endOffset = numTotaleUtenti;

                IEnumerable<Entities.AnagraficaClienti> entities = queryBase.Skip(itemOffset).Take(itemsPerRequest);

                foreach (Entities.AnagraficaClienti entity in entities)
                {
                    RadComboBoxItem item = new RadComboBoxItem(entity.DSCCONTO1, entity.CODCONTO);
                    item.Attributes.Add("Ind", entity.INDIRIZZO);
                    item.Attributes.Add("CAP", entity.CAP);
                    item.Attributes.Add("Loc", entity.LOCALITA);
                    item.Attributes.Add("Prov", entity.PROVINCIA);
                    item.Attributes.Add("Tel", entity.TELEFONO);
                    item.DataItem = entity;
                    combo.Items.Add(item);
                }

                combo.DataBind();

                if (numTotaleUtenti > 0)
                {
                    e.Message = String.Format("Clienti (da <b>1</b> a <b>{0}</b> di <b>{1}</b>)", endOffset.ToString(), numTotaleUtenti.ToString());
                }
                else
                {
                    e.Message = "Nessuna corrispondenza";
                }
            }
            catch (Exception ex)
            {
                e.Message = ex.Message;
            }
        }

        protected void rcbIndirizzi_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                RadComboBox combo = (RadComboBox)sender;
                combo.Items.Clear();

                IEnumerable<Entities.AnagraficaDestinazioneMerce> queryBase;

                string codiceCliente = string.Empty;
                if (e.Context.ContainsKey("CodiceCliente") && e.Context["CodiceCliente"].ToString() != string.Empty)
                {
                    codiceCliente = e.Context["CodiceCliente"].ToString();
                    Logic.Metodo.AnagraficaDestinazioniMerce ll = new Logic.Metodo.AnagraficaDestinazioniMerce();
                    queryBase = ll.Read(codiceCliente, false);
                }
                else
                {
                    queryBase = new List<Entities.AnagraficaDestinazioneMerce>();
                }


                int itemsPerRequest = (combo.ItemsPerRequest <= 0) ? 20 : combo.ItemsPerRequest;
                int itemOffset = e.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                int numTotaleUtenti = queryBase.Count();


                if (endOffset > numTotaleUtenti)
                    endOffset = numTotaleUtenti;

                foreach (Entities.AnagraficaDestinazioneMerce entity in queryBase.Skip(itemOffset).Take(itemsPerRequest))
                {
                    RadComboBoxItem item = new RadComboBoxItem(entity.RagioneSociale, entity.CodiceDestinazione.ToString());
                    item.Attributes.Add("Ind", entity.Indirizzo);
                    item.Attributes.Add("CAP", entity.CAP);
                    item.Attributes.Add("Loc", entity.Localita);
                    item.Attributes.Add("Prov", entity.Provincia);
                    item.Attributes.Add("Tel", entity.Telefono);
                    item.DataItem = entity;
                    combo.Items.Add(item);
                }

                combo.DataBind();

                if (numTotaleUtenti > 0)
                {
                    e.Message = String.Format("Destinazioni (da <b>1</b> a <b>{0}</b> di <b>{1}</b>)", endOffset.ToString(), numTotaleUtenti.ToString());
                }
                else
                {
                    e.Message = "Nessuna destinazione";
                }
            }
            catch (Exception ex)
            {
                e.Message = ex.Message;
            }
        }

        #region Griglia Operatori Associati

        /// <summary>
        /// Metodo di gestione dell'evento ItemCommand della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGrigliaOperatoriAssociati_PreRender(object sender, EventArgs e)
        {
            TelerikRadGridHelper.ApplicaTraduzioneDaFileDiResource(rgGrigliaOperatoriAssociati);
        }

        /// <summary>
        /// Metodo di gestione dell'evento 'InsertCommand' della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGrigliaOperatoriAssociati_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            Logic.AccountsOperatori llAccountOperatori = new Logic.AccountsOperatori();
            llAccountOperatori.StartTransaction();

            try
            {
                if (e.Item is GridEditableItem)
                {
                    RadComboBox rcbOperatore = TelerikRadGridHelper.FindControl<RadComboBox>(e.Item, "rcbOperatore");
                    if (rcbOperatore == null) throw new Exception("Non è stato possibile recuperare il combo relativo agli operatori");
                    else if (rcbOperatore.CheckedItems == null || rcbOperatore.CheckedItems.Count <= 0) throw new Exception("Non è stato selezionato nessun operatore");

                    MessagesCollector errori = new MessagesCollector();

                    foreach (RadComboBoxItem checkedItem in rcbOperatore.CheckedItems)
                    {
                        try
                        {
                            Entities.EntityId<Entities.Operatore> identifierOperatore = new Entities.EntityId<Entities.Operatore>(checkedItem.Value);
                            if (!llAccountOperatori.EsisteRelazione(CurrentId, identifierOperatore))
                            {
                                llAccountOperatori.Create(CurrentId, identifierOperatore, true);
                            }
                        }
                        catch(Exception ex)
                        {
                            errori.Add(ex.Message);
                        }
                    }
                    
                    llAccountOperatori.CommitTransaction();

                    // Gli unici errori che potrebbero generarsi sono dovuti ad un errato formato dell'id dell'operatore 
                    // oppure all'esesistenza di una relazione tra l'account selezionato e l'operatore selezionato
                    if (errori.HaveMessages)
                    {
                        MessageHelper.ShowErrorMessage(this, errori.ToString(Helper.HtmlEnvironment.NewLine));
                    }
                }
            }
            catch (Exception ex)
            {
                llAccountOperatori.RollbackTransaction();
                e.Canceled = true;
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento DeleteCommand della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGrigliaOperatoriAssociati_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {

            Logic.AccountsOperatori llAccountsOperatori = new Logic.AccountsOperatori();

            try
            {
                Guid idAccount = (Guid)(((GridDataItem)(e.Item)).GetDataKeyValue("IDAccount"));
                Guid idOperatore = (Guid)(((GridDataItem)(e.Item)).GetDataKeyValue("IDOperatore"));

                if (!idAccount.IsNullOrEmpty() && !idOperatore.IsNullOrEmpty())
                {
                    Entities.EntityId<Entities.Account> identifierAccount = new Entities.EntityId<Entities.Account>(idAccount);
                    Entities.EntityId<Entities.Operatore> identifierOperatore = new Entities.EntityId<Entities.Operatore>(idOperatore);

                    Entities.AccountOperatore entityToDelete = llAccountsOperatori.Find(identifierAccount, identifierOperatore);
                    if (entityToDelete != null)
                    {
                        //Entities.ApplicazioneAccount utenteCollegato = InformazioniSessione.GetIstance().AccountCollegato;
                        //if (utenteCollegato == null)
                        //{
                        //    throw new Exception(ErrorMessage.UTENTE_SCONOSCIUTO_MESSAGE);
                        //}

                        string userNameAccount = entityToDelete.Account.UserName;
                        string cognomeNomeOperatore = entityToDelete.Operatore.CognomeNome;

                        llAccountsOperatori.Delete(entityToDelete, true);

                        SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - Rimosso l'entity AccountOperatore con Username dell'Account '{1}' ed Cognome e Nome dell'operatore '{2}'.", Request.Url.AbsolutePath, userNameAccount, cognomeNomeOperatore));
                    }
                }
            }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);
                MessageHelper.ShowErrorMessage(this, "Operazione di Eliminazione non riuscita, è stato riscontrato il seguente errore:<br />" + ex.Message);
                e.Canceled = true;
            }
            finally
            {
                rgGrigliaOperatoriAssociati.Rebind();
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento NeedDataSource della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGrigliaOperatoriAssociati_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (rgGrigliaOperatoriAssociati.Visible)
                {
                    Logic.AccountsOperatori llAccountsOperatori = new Logic.AccountsOperatori();
                    IQueryable<Entities.AccountOperatore> elencoAccountOperatore = llAccountsOperatori.Read(CurrentId);
                    rgGrigliaOperatoriAssociati.DataSource = elencoAccountOperatore;
                }                
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
        protected void rgGrigliaOperatoriAssociati_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                //Traduce le voci del menu di filtro della griglia. Questa operazione deve essere fatta ad ogni post
                TelerikHelper.TraduciElementiGriglia(e);

                if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
                {
                    GridEditFormItem filerItem = (GridEditFormItem)e.Item;
                    if (filerItem[COGNOME_NOME_OPERATORE_GRID_OPERATORI_DA_ASSOCIARE_KEY] != null)
                    {
                        RadComboBox rcbOperatore = filerItem[COGNOME_NOME_OPERATORE_GRID_OPERATORI_DA_ASSOCIARE_KEY].FindControl("rcbOperatore") as RadComboBox;
                        if (rcbOperatore != null)
                        {
                            Logic.Operatori llOperatori = new Logic.Operatori();

                            rcbOperatore.Width = ComboOperatoriGrigliaOperatoriDaAssociareWidth;
                            rcbOperatore.DataSource = llOperatori.ReadOperatoriAccountNonAssociati(CurrentId);
                            rcbOperatore.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento ItemDataBound della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGrigliaOperatoriAssociati_ItemDataBound(object sender, GridItemEventArgs e)
        {
            // Inserisce gli attributi necessari per la trasformazione del layout della griglia per dispositivi mobili
            TelerikRadGridHelper.ManageColumnContentOnMobileLayout(rgGrigliaOperatoriAssociati, e);
        }

        #endregion

        #endregion

        #region Metodi di gestione

        /// <summary>
        /// Mostra nell'interfaccia i dati dell'entity passata
        /// </summary>
        /// <param name="entity"></param>
        private void ShowData(Entities.Account entityToShow)
        {
            ApplicaRedirectPulsanteAggiorna();

            //In base al fatto che l'entity sia nuova o meno imposto anche il titolo della pagina
            bool nuovo = entityToShow.ID.Equals(Guid.Empty);
            if (!nuovo)
            {
                lblTitolo.Text = string.Format("Account: {0}", entityToShow.UserName);
                hfIsNuovoAccount.Value = "0";
            }
            else
            {
                lblTitolo.Text = "Nuovo Account";
                entityToShow.TipologiaEnum = Entities.TipologiaAccountEnum.SeCoGes;
                hfIsNuovoAccount.Value = "1";
                rblTipoAccountClienteStandard.Checked = true;
            }
            this.Title = lblTitolo.Text;



            // Mostro a video i dati presenti nell'entity

            rtbUserName.Text = entityToShow.UserName;
            rtbNominativo.Text = entityToShow.Nominativo;
            rtbEmail.Text = entityToShow.Email;
            chkAmministratore.Checked = (entityToShow.Amministratore.HasValue) ? entityToShow.Amministratore.Value : false;
            chkValidatore.Checked = entityToShow.ValidatoreOfferta;
            chkSolaLettura.Checked = (entityToShow.SolaLettura.HasValue) ? entityToShow.SolaLettura.Value : false;
            chkBloccaUtente.Checked = (entityToShow.Bloccato.HasValue) ? entityToShow.Bloccato.Value : false;

            IsAmministratore = chkAmministratore.Checked;

            if (entityToShow.ScadenzaPassword.Equals(DateTime.MinValue))
            {
                rdpDataScadenzaPassword.SelectedDate = null;
            }
            else
            {
                rdpDataScadenzaPassword.SelectedDate = entityToShow.ScadenzaPassword;
            }

            if (entityToShow.TipologiaEnum == Entities.TipologiaAccountEnum.SeCoGes)
            {
                rbtnStandard.Checked = true;
                rblTipoAccountClienteStandard.Checked = true;
            }
            else if (entityToShow.TipologiaEnum == Entities.TipologiaAccountEnum.ClienteStandard)
            {
                rbtnCliente.Checked = true;
                rblTipoAccountClienteStandard.Checked = true;
            }
            else if (entityToShow.TipologiaEnum == Entities.TipologiaAccountEnum.ClienteSupervisore)
            {
                rbtnCliente.Checked = true;
                rblTipoAccountClienteSupervisore.Checked = true;
            }
            else if (entityToShow.TipologiaEnum == Entities.TipologiaAccountEnum.ClienteAdmin)
            {
                rbtnCliente.Checked = true;
                rblTipoAccountClienteAdmin.Checked = true;
            }

            rcbCliente.Text = entityToShow.RagioneSociale;
            rcbCliente.SelectedValue =  entityToShow.CodiceCliente;

            rcbIndirizzi.Text = entityToShow.DestinazioneMerce;
            rcbIndirizzi.SelectedValue = entityToShow.IdDestinazione;
        }

        /// <summary>
        ///  Memorizza i dati presenti nei controlli dell'interfaccia
        /// </summary>
        private void SaveData()
        {
            //if (Autorizzazioni_GestioneAccounts == null) throw new ArgumentNullException("Autorizzazioni_GestioneAccounts");

            //if ((CurrentId.IsNullOrEmpty() && !Autorizzazioni_GestioneAccounts.Consenti_Inserimento) || (!CurrentId.IsNullOrEmpty() && !Autorizzazioni_GestioneAccounts.Consenti_Modifica))
            //{
            //    MessageHelper.ShowErrorMessage(this, "Diritti insufficienti per il completamento dell'operazione. Salvataggio Annullato!");
            //    return;
            //}
            //if (CurrentId.IsNullOrEmpty() && !Autorizzazioni_GestioneAccounts.Consenti_Inserimento)
            //{
            //    MessageHelper.ShowErrorMessage(this, "Diritti insufficienti per il completamento dell'operazione. Salvataggio Annullato!");
            //    return;
            //}
            //if (!CurrentId.IsNullOrEmpty() && !Autorizzazioni_GestioneAccounts.Consenti_Modifica)
            //{
            //    MessageHelper.ShowErrorMessage(this, "Diritti insufficienti per il completamento dell'operazione. Salvataggio Annullato!");
            //    return;
            //}

            //if (!(CurrentId.IsNullOrEmpty() && Autorizzazioni_GestioneAccounts.Consenti_Inserimento) 
            //    || !(!CurrentId.IsNullOrEmpty() && Autorizzazioni_GestioneAccounts.Consenti_Modifica))
            //{
            //    MessageHelper.ShowErrorMessage(this, "Diritti insufficienti per il completamento dell'operazione. Salvataggio Annullato!");
            //    return;
            //}

            // Valida i dati nella pagina
            MessagesCollector erroriDiValidazione = ValidaDati();
            if (erroriDiValidazione.HaveMessages)
            {
                // Se qualcosa non va bene mostro un avviso all'utente
                MessageHelper.ShowErrorMessage(this, erroriDiValidazione.ToString("<br />"));
                return;
            }

            // Definisco una variabile per memorizzare l'entità da salvare
            Entities.Account entityToSave = null;

            // Definisco una variabile che conterrà l'ID dell'entity salvata. 
            // Se per qualche motivo l'entity non viene salvata allora entityId rimarrà Guid.Empty
            Guid entityId = Guid.Empty;
            string entityUsername = string.Empty;
            bool nuovo = false;
            Logic.Sicurezza.Accounts llAccount = new Logic.Sicurezza.Accounts();

            try
            {
                llAccount.StartTransaction();

                //Se CurrentId contiene un ID allora cerco l'entity nel database
                if (CurrentId.HasValue)
                {
                    entityToSave = llAccount.Find(CurrentId.Value);
                }

                // Definisco una variabile che indica se si deve compiere una azione di creazione di una nuova entity o di modifica
                
                bool sincronizzaAziendeFraSoggettoEdAccount = false;

                if (entityToSave == null)
                {
                    if (CurrentId.HasValue)
                    {
                        // Se CurrentId ha un valore ma entityToSave è null allora vuol dire che la pagina è stata aperta
                        // per modificare un entità che adesso non esiste più nella base dati.
                        // In questo caso avviso l'utente
                        throw new Exception(
                            "L'Account che si sta variando non esiste più in archivio."
                                + "\n\rOperazione annullata!");
                    }

                    // Al contrario, se entityToSave è nulla e CurrentId è vuota
                    // vuol dire che la pagina è stata aperta per la creazione di una nuova entità
                    nuovo = true;
                    entityToSave = new Entities.Account();

                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriDallaView(entityToSave, true, out sincronizzaAziendeFraSoggettoEdAccount);

                    //Creo nuova entity
                    llAccount.Create(entityToSave, Logic.Sicurezza.CurrentSession.GetLoggedAccount(), false);
                }
                else
                {
                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriDallaView(entityToSave, false, out sincronizzaAziendeFraSoggettoEdAccount);
                }

                if (!nuovo && (entityToSave.TipologiaEnum == Entities.TipologiaAccountEnum.ClienteStandard || entityToSave.TipologiaEnum == Entities.TipologiaAccountEnum.ClienteAdmin || entityToSave.TipologiaEnum == Entities.TipologiaAccountEnum.ClienteSupervisore))
                {
                    Logic.AccountsOperatori llOperatori = new Logic.AccountsOperatori(llAccount);
                    IQueryable<Entities.AccountOperatore> elencoOperatoriDaRimuovere = llOperatori.Read(new Entities.EntityId<Entities.Account>(CurrentId));
                    if (elencoOperatoriDaRimuovere != null && elencoOperatoriDaRimuovere.Count() > 0)
                    {
                        llOperatori.Delete(elencoOperatoriDaRimuovere, false);
                    }
                }

                //if (sincronizzaAziendeFraSoggettoEdAccount)
                //{
                //    // Sincronizza tutte le informazioni relative alle entities AccademiaRuoloSoggetto e li applica come AccountAzienda per l'Account specificato ed il relativo Soggetto
                //    ll.SincronizzaAziendeFraSoggettoEdAccount(entityToSave);
                //}

                // Persisto le modifiche sulla base dati
                llAccount.SubmitToDatabase();

                SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - {1} l'entity Account con UserName '{2}'.", Request.Url.AbsolutePath, ((nuovo) ? "Creato" : "Salvato"), entityToSave.UserName));

                // Memorizzo l'ID dell'entità
                entityId = entityToSave.ID;
                entityUsername = entityToSave.UserName;

                llAccount.CommitTransaction();

                InformazioniAccountAutenticato.RimuoviInformazioniAccount(entityToSave.UserName);
            }
            catch (Exception ex)
            {
                llAccount.RollbackTransaction();

                SeCoGes.Logging.LogManager.AddLogErrori(ex);

                // ...e mostro il messaggio d'errore all'utente
                MessageHelper.ShowErrorMessage(this, ex.Message);
            }
            finally
            {
                if (nuovo)
                {
                    llAccount.InviaEmailCreazioneAccount(entityToSave, GetPasswordInChiaro());
                }

                // Alla fine, se il salvataggio è andato a buon fine (entityId != Guid.Empty)
                // allora ricarico la pagina aprendola in modifica
                if (entityId != Guid.Empty)
                {
                    //InformazioniAccountAutenticato.RimuoviInformazioniAccount(entityUsername);
                    Helper.Web.ReloadPageWithIdAndTab(this, entityId.ToString(), rtsAccount.SelectedIndex);
                    //Response.Redirect(string.Format("{0}?ID={1}&tab={2}", Request.AppRelativeCurrentExecutionFilePath.ToString(), entityId, rtsApparecchiatura.SelectedIndex), false);

                }
            }
        }

        #endregion

        #region Funzioni Accessorie


        /// <summary>
        /// Assegna l'indirizzo della pagina da aprire al pulsante Aggiorna.
        /// L'indirizzo è esattamente lo stesso della pagina aperta.
        /// </summary>
        private void ApplicaRedirectPulsanteAggiorna()
        {
            RadToolBarItem toolBarItem = RadToolBar1.FindItemByValue("Aggiorna");
            if (toolBarItem != null)
                ((RadToolBarButton)toolBarItem).NavigateUrl = Request.Url.ToString();
        }
        
        /// <summary>
        /// Legge i dati inseriti nell'interfaccia e li inserisce nell'entity passata
        /// </summary>
        /// <param name="entityToFill"></param>
        /// <param name="nuovo"></param>
        public void EstraiValoriDallaView(Entities.Account entityToFill, bool nuovo, out bool sincronizzaAziendeFraSoggettoEdAccount)
        {
            sincronizzaAziendeFraSoggettoEdAccount = false;
            try
            {
                // Leggo i dati dai controlli della pagina e li inserisco nelle property dell'entity
                
                entityToFill.UserName = rtbUserName.Text.ToTrimmedString();
                entityToFill.Nominativo = rtbNominativo.Text.ToTrimmedString();
                entityToFill.Email = rtbEmail.Text.ToTrimmedString();
                entityToFill.Amministratore = (chkAmministratore.Checked) ? true : (bool?)null;
                entityToFill.ValidatoreOfferta = chkValidatore.Checked;
                entityToFill.SolaLettura = (chkSolaLettura.Checked) ? true : (bool?)null;
                entityToFill.Bloccato = (chkBloccaUtente.Checked) ? true : (bool?)null;
                entityToFill.TipologiaEnum = (rbtnStandard.Checked) ? Entities.TipologiaAccountEnum.SeCoGes : Entities.TipologiaAccountEnum.ClienteStandard;

                if (entityToFill.TipologiaEnum == Entities.TipologiaAccountEnum.SeCoGes)
                {
                    entityToFill.CodiceCliente = null;
                    entityToFill.RagioneSociale = null;
                    entityToFill.IdDestinazione = null;
                    entityToFill.DestinazioneMerce = null;
                }
                else
                {
                    entityToFill.CodiceCliente = rcbCliente.SelectedValue;
                    entityToFill.RagioneSociale = rcbCliente.Text;

                    entityToFill.IdDestinazione = null;
                    entityToFill.DestinazioneMerce = null;
                    if (!rblTipoAccountClienteAdmin.Checked)
                    {
                        entityToFill.IdDestinazione = rcbIndirizzi.SelectedValue;
                        entityToFill.DestinazioneMerce = rcbIndirizzi.Text;
                    }

                    if (rblTipoAccountClienteSupervisore.Checked) entityToFill.TipologiaEnum = Entities.TipologiaAccountEnum.ClienteSupervisore;
                    if (rblTipoAccountClienteAdmin.Checked) entityToFill.TipologiaEnum = Entities.TipologiaAccountEnum.ClienteAdmin;
                }

                //Guid? idAnagraficaPersonale = ucAssociazioni.GetIdSoggettoAnagraficaAssociato();
                //sincronizzaAziendeFraSoggettoEdAccount = (idAnagraficaPersonale != null && entityToFill.IDSoggetto != idAnagraficaPersonale);
                //entityToFill.IDSoggetto = idAnagraficaPersonale; 


                // Se alla creazione di un nuovo utente non viene specificata la scadenza della password,
                // il sistema imposta automaticamente una validità di 6 mesi.
                if (nuovo && !rdpDataScadenzaPassword.SelectedDate.HasValue)
                {
                    rdpDataScadenzaPassword.SelectedDate = DateTime.Now.AddMonths(6);
                }

                if (hdPasswordModificata.Value == "1")
                {
                    // La password viene archiviata criptata utilizzando l'algoritmo di codifica SHA-1. 
                    string encryptedPassword = SeCoGes.Utilities.PasswordHelper.GetHashPasswordForStoringInConfigFile(GetPasswordInChiaro());
                    entityToFill.Password = encryptedPassword;
                    hdPasswordModificata.Value = "0";

                    entityToFill.ScadenzaPassword = rdpDataScadenzaPassword.SelectedDate.Value;
                }
            }
            catch
            {
                string messaggio = "Attenzione, sono presenti degli errori nei dati inseriti.";
                MessageHelper.ShowErrorMessage(this, messaggio);
            }
        }

        /// <summary>
        /// Restituisce la password in chiaro inserita dall'utente
        /// </summary>
        /// <returns></returns>
        private string GetPasswordInChiaro()
        {
            return rtbPassword.Text.Trim();
        }

        /// <summary>
        /// Restituisce la conferma della password in chiaro inserita dall'utente
        /// </summary>
        /// <returns></returns>
        private string GetConfermaPasswordInChiaro()
        {
            return rtbConfermaPassword.Text.Trim();
        }

        /// <summary>
        /// Restituisce un oggetto contenente gli eventuali errori di validazione dei dati
        /// </summary>
        /// <returns></returns>
        private MessagesCollector ValidaDati()
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

            if (rfvEmail.IsValid)
            {
                string valoreTextBoxEmail = rtbEmail.Text.ToTrimmedString();
                string[] emailInserite = valoreTextBoxEmail.Split(new string[] { EmailHelper.SEPARATORE_EMAIL }, StringSplitOptions.RemoveEmptyEntries);

                List<string> elencoEmailValide = EmailHelper.GetEmailsValidList(valoreTextBoxEmail, EmailHelper.SEPARATORE_EMAIL);
                if (elencoEmailValide == null || elencoEmailValide.Count <= 0)
                {
                    messaggi.Add("E' necessario inserire delle email valide");
                }
                else if (elencoEmailValide.Count != emailInserite.Length)
                {
                    messaggi.Add("E' necessario verificare la validità di alcune email inserite in quanto alcune di esse non risultano valide");
                }
            }

            if (rbtnCliente.Checked && String.IsNullOrEmpty(rcbCliente.SelectedValue))
            {
                messaggi.Add("E' necessario selezionare obbligatoriamente un Cliente");
            }

            // Il Controllo della password viene effettuato solo se l'utente ha aperto il pannello di gestione password
            // oppure se si sta creando un nuovo utente. Per i nuovi utenti la password è obbligatoria
            if (hdPasswordModificata.Value == "1" || !CurrentId.HasValue)
            {
                string passwordInChiaro = GetPasswordInChiaro();
                string confermaPasswordInChiaro = GetConfermaPasswordInChiaro();

                if (passwordInChiaro != confermaPasswordInChiaro)
                {
                    messaggi.Add("La password e le password di conferma non coincidono.");
                }
                if (passwordInChiaro == String.Empty)
                {
                    messaggi.Add("Indicare una Password di accesso.");
                }
                else if (!revPassword.IsValid)
                {
                    messaggi.Add(revPassword.ErrorMessage);
                }


                // Se alla creazione di un nuovo utente non viene specificata la scadenza della password,
                // il sistema imposta automaticamente una validità di 180 giorni (3 mesi).
                // Nel caso si stia creando un nuovo utente, quindi, non si controlla che sia stata indicata una sacadenza della password
                if (CurrentId.HasValue)
                {
                    if (!rdpDataScadenzaPassword.SelectedDate.HasValue)
                    {
                        messaggi.Add("La data di scadenza della password è obbligatoria.");
                    }
                }
            }

            return messaggi;
        }
                
        #endregion


        #region Gestione delle autorizzazioni

        //private Entities.Account utenteCollegato;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneAccounts;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestionePasswordAccounts;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneImpostazioniAccessoAccounts;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneAssociazioniAccount;


        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneAccounts;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestionePasswordAccounts;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneImpostazioniAccessoAccounts;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneAssociazioniAccount;

        ///// <summary>
        ///// Effettua le operazioni di autenticazione e di registrazione delle autorizzazioni di accesso alle funzionalità della pagina
        ///// </summary>
        ///// <returns></returns>
        //private void GestisciAutorizzazioniBase()
        //{
        //    InformazioniAccountAutenticato infoAccount = InformazioniAccountAutenticato.GetIstance();
        //    Entities.Account utenteCollegato = infoAccount.Account;

        //    try
        //    {
        //        if (utenteCollegato.SolaLettura.HasValue && utenteCollegato.SolaLettura.Value)
        //        {
        //            this.Enabled = false;
        //        }

        //        Autorizzazioni_GestioneAccounts = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneAccounts);
        //        Autorizzazioni_GestionePasswordAccounts = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestionePasswordAccounts);
        //        Autorizzazioni_GestioneImpostazioniAccessoAccounts = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneImpostazioniAccessoAccounts);
        //        Autorizzazioni_GestioneAssociazioniAccount = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneAssociazioniAccount);

        //        if (!Autorizzazioni_GestioneAccounts.Consenti_Visibilità)
        //        {
        //            Response.Redirect("/Home.aspx");
        //            return;
        //        }

        //        PulsanteToolbar_Nuovo.Enabled = Autorizzazioni_GestioneAccounts.Consenti_Inserimento;
        //        PulsanteToolbar_Salva.Enabled = Autorizzazioni_GestioneAccounts.Consenti_Modifica;
        //        this.Enabled = Autorizzazioni_GestioneAccounts.Consenti_Modifica;

        //        rbModificaPassword.Visible = Autorizzazioni_GestionePasswordAccounts.Consenti_Funzionalità;

        //        chkAmministratore.Enabled = Autorizzazioni_GestioneImpostazioniAccessoAccounts.Consenti_Funzionalità;
        //        chkSolaLettura.Enabled = Autorizzazioni_GestioneImpostazioniAccessoAccounts.Consenti_Funzionalità;
        //        chkBloccaUtente.Enabled = Autorizzazioni_GestioneImpostazioniAccessoAccounts.Consenti_Funzionalità;

        //        rtsAccount.Tabs[1].Visible = Autorizzazioni_GestioneAssociazioniAccount.Consenti_Funzionalità;
        //        rtsAccount.Tabs.FindTabByValue("Associazioni").Visible = Autorizzazioni_GestioneAssociazioniAccount.Consenti_Funzionalità;
        //        rpvAssociazioni.Visible = Autorizzazioni_GestioneAssociazioniAccount.Consenti_Funzionalità;
        //    }
        //    catch (Exception ex)
        //    {
        //        SeCoGes.Logging.LogManager.AddLogErrori(ex);
        //        MessageHelper.ShowErrorMessage(this, ex);
        //    }
        //}

        ///// <summary>
        ///// Effettua le operazioni di gestione delle autorizzazioni di accesso alle funzionalità della pagina quando si è inmodalità di Inserimento
        ///// </summary>
        ///// <returns></returns>
        //private void GestisciAutorizzazioniInserimento()
        //{
        //    try
        //    {
        //        // Attiva i controlli che servono in caso di inserimento
        //        PulsanteToolbar_Salva.Enabled = Autorizzazioni_GestioneAccounts.Consenti_Inserimento;
        //        this.Enabled = Autorizzazioni_GestioneAccounts.Consenti_Inserimento;

        //        // Avendo agito sulla proprietà Enabled si devono riapplicare le autorizzazioni sul blocco di gestione delle impostazioni di accesso
        //        chkAmministratore.Enabled = Autorizzazioni_GestioneImpostazioniAccessoAccounts.Consenti_Funzionalità;
        //        chkSolaLettura.Enabled = Autorizzazioni_GestioneImpostazioniAccessoAccounts.Consenti_Funzionalità;
        //        chkBloccaUtente.Enabled = Autorizzazioni_GestioneImpostazioniAccessoAccounts.Consenti_Funzionalità;

        //    }
        //    catch (Exception ex)
        //    {
        //        SeCoGes.Logging.LogManager.AddLogErrori(ex);
        //        MessageHelper.ShowErrorMessage(this, ex);
        //    }
        //}

        /// <summary>
        /// Carica gli oggetti contenenti le informazioni di accesso ai dati ed alle funzionalità esposte dalla pagina
        /// </summary>
        private void CaricaAutorizzazioni()
        {
            //try
            //{
            //    InformazioniAccountAutenticato infoAccount = InformazioniAccountAutenticato.GetIstance();
            //    utenteCollegato = infoAccount.Account;

            //    Autorizzazioni_GestioneAccounts = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneAccounts);
            //    Autorizzazioni_GestionePasswordAccounts = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestionePasswordAccounts);
            //    Autorizzazioni_GestioneImpostazioniAccessoAccounts = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneImpostazioniAccessoAccounts);
            //    Autorizzazioni_GestioneAssociazioniAccount = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneAssociazioniAccount);
            //}
            //catch (Exception ex)
            //{
            //    SeCoGes.Logging.LogManager.AddLogErrori(ex);
            //    MessageHelper.ShowErrorMessage(this, ex);
            //}
        }

        /// <summary>
        /// Blocca o permette l'accesso ai dati ed applica agli oggetti dell'interfaccia lo stile in base alle regole di accesso dell'account corrente
        /// </summary>
        private void ApplicaAutorizzazioni()
        {
            //if (Autorizzazioni_GestioneAccounts == null) throw new ArgumentNullException("Autorizzazioni_GestioneAccounts");
            //if (Autorizzazioni_GestionePasswordAccounts == null) throw new ArgumentNullException("Autorizzazioni_GestionePasswordAccounts");
            //if (Autorizzazioni_GestioneImpostazioniAccessoAccounts == null) throw new ArgumentNullException("Autorizzazioni_GestioneImpostazioniAccessoAccounts");
            //if (Autorizzazioni_GestioneAssociazioniAccount == null) throw new ArgumentNullException("Autorizzazioni_GestioneAssociazioniAccount");


            //// Applicazione regole di accesso di VISIBILITA' 
            //if (!Autorizzazioni_GestioneAccounts.Consenti_Visibilità)
            //{
            //    MessageHelper.RedirectToErrorPageWithMessage(ErrorMessage.ACCESSO_NEGATO_MESSAGE);
            //    return;
            //}


            //// Applicazione regole accesso in MODIFICA ed accesso a FUNZIONALITA'
            //PulsanteToolbar_Nuovo.Enabled = Autorizzazioni_GestioneAccounts.Consenti_Inserimento;
            //PulsanteToolbar_Salva.Enabled = Autorizzazioni_GestioneAccounts.Consenti_Modifica;
            //this.Enabled = Autorizzazioni_GestioneAccounts.Consenti_Modifica;

            //rbModificaPassword.Visible = Autorizzazioni_GestionePasswordAccounts.Consenti_Funzionalità;

            //chkAmministratore.Enabled = Autorizzazioni_GestioneImpostazioniAccessoAccounts.Consenti_Funzionalità;
            //chkSolaLettura.Enabled = Autorizzazioni_GestioneImpostazioniAccessoAccounts.Consenti_Funzionalità;
            //chkBloccaUtente.Enabled = Autorizzazioni_GestioneImpostazioniAccessoAccounts.Consenti_Funzionalità;

            //rtsAccount.Tabs[1].Visible = Autorizzazioni_GestioneAssociazioniAccount.Consenti_Funzionalità;
            //rtsAccount.Tabs.FindTabByValue("Associazioni").Visible = Autorizzazioni_GestioneAssociazioniAccount.Consenti_Funzionalità;
            //rpvAssociazioni.Visible = Autorizzazioni_GestioneAssociazioniAccount.Consenti_Funzionalità;


            //// Applicazione regole accesso ad operazioni di INSERIMENTO
            //if (CurrentId.Equals(Guid.Empty))
            //{
            //    // Applicazione regole di accesso di INSERIMENTO' 
            //    if (!Autorizzazioni_GestioneAccounts.Consenti_Inserimento)
            //    {
            //        MessageHelper.RedirectToErrorPageWithMessage(ErrorMessage.UTENTE_OPERAZIONE_NON_CONSENTITA_MESSAGE);
            //        return;
            //    }

            //    // Attiva i controlli che servono in caso di inserimento
            //    PulsanteToolbar_Salva.Enabled = Autorizzazioni_GestioneAccounts.Consenti_Inserimento;
            //    this.Enabled = Autorizzazioni_GestioneAccounts.Consenti_Inserimento;

            //    // Avendo agito sulla proprietà Enabled si devono riapplicare le autorizzazioni sul blocco di gestione delle impostazioni di accesso
            //    chkAmministratore.Enabled = Autorizzazioni_GestioneImpostazioniAccessoAccounts.Consenti_Funzionalità;
            //    chkSolaLettura.Enabled = Autorizzazioni_GestioneImpostazioniAccessoAccounts.Consenti_Funzionalità;
            //    chkBloccaUtente.Enabled = Autorizzazioni_GestioneImpostazioniAccessoAccounts.Consenti_Funzionalità;
            //}


            //// Applicazione regole accesso in SOLA LETTURA
            //if (utenteCollegato.SolaLettura.HasValue && utenteCollegato.SolaLettura.Value)
            //{
            //    this.Enabled = false;
            //}


            //// Se l'utente autenticato sta modificando la propria scheda utente
            //// allora viene limitato nelle azioni.
            //// Non può cambiarsi il gruppo di appartenenza ed auto-bloccarsi o sbloccarsi.
            //if (!CurrentId.IsNullOrEmpty())
            //{
            //    Logic.Sicurezza.Accounts ll = new Logic.Sicurezza.Accounts();
            //    Entities.Account userInGestione = ll.Find(CurrentId);
            //    if (userInGestione != null)
            //    {
            //        if (userInGestione.UserName.ToLower() == Page.User.Identity.Name.ToLower())
            //        {
            //            rtbUserName.ReadOnly = true;
            //            chkAmministratore.Enabled = false;
            //            chkSolaLettura.Enabled = false;
            //            chkBloccaUtente.Enabled = false;
            //        }
            //    }
            //}
        }



        #endregion
    }
}