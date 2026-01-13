using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
//using SeCoGes.Hdemia.Entities.;
//using SeCoGes.Hdemia.Helper;
//using SeCoGes.Hdemia.Infrastructure.Applicazione;
using SeCoGes.Utilities;
using Telerik.Web.UI;

namespace SeCoGEST.Web.UI
{
    public partial class DocumentazioneAllegata : System.Web.UI.UserControl
    {
        #region Properties

        /// <summary>
        /// Recupera o setta il read only della pagina
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                if (ViewState["IsReadOnly"] == null)
                {
                    ViewState["IsReadOnly"] = false;
                }

                return (bool)ViewState["IsReadOnly"];
            }
            set
            {
                ViewState["IsReadOnly"] = value;
                GestisciReadOnly();
            }
        }

        /// <summary>
        /// Recupera o setta il valore di IsDeleteEnabled 
        /// </summary>
        public bool IsDeleteEnabled
        {
            get
            {
                if (ViewState["IsDeleteEnabled"] == null)
                {
                    ViewState["IsDeleteEnabled"] = false;
                }

                return (bool)ViewState["IsDeleteEnabled"];
            }
            set
            {
                ViewState["IsDeleteEnabled"] = value;
                GestisciDeleteEnabled();
            }
        }

        /// <summary>
        /// Recupera o setta il valore di IsDeleteEnabled 
        /// </summary>
        public bool IsUploadEnabled
        {
            get
            {
                if (ViewState["IsUploadEnabled"] == null)
                {
                    ViewState["IsUploadEnabled"] = true;
                }

                return (bool)ViewState["IsUploadEnabled"];
            }
            set
            {
                ViewState["IsUploadEnabled"] = value;
                lrCaricamentoFile.Visible = value;
            }
        }


        /// <summary>
        /// Recupera o setta l'attivazione dell'ajax
        /// </summary>
        public bool EnableAjax
        {
            get
            {
                if (ViewState["EnableAjax"] == null)
                {
                    ViewState["EnableAjax"] = true;
                }

                return (bool)ViewState["EnableAjax"];
            }
            set
            {
                ViewState["EnableAjax"] = value;
                GestisciReadOnly();
                GestisciDeleteEnabled();
            }
        }

        private GridColumn ColonnaEstensioneFile
        {
            get
            {
                if (rgGrigliaAllegati.Columns != null)
                {
                    return rgGrigliaAllegati.Columns.FindByUniqueName("ColonnaEstensioneFile");
                }
                else
                {
                    return null;
                }
            }
        }

        private GridColumn ColonnaUserName
        {
            get
            {
                if (rgGrigliaAllegati.Columns != null)
                {
                    return rgGrigliaAllegati.Columns.FindByUniqueName("ColonnaUserName");
                }
                else
                {
                    return null;
                }
            }
        }

        private GridColumn ColonnaDataArchiviazione
        {
            get
            {
                if (rgGrigliaAllegati.Columns != null)
                {
                    return rgGrigliaAllegati.Columns.FindByUniqueName("ColonnaDataArchiviazione");
                }
                else
                {
                    return null;
                }
            }
        }

        private GridColumn ColonnaElimina
        {
            get
            {
                if (rgGrigliaAllegati.Columns != null)
                {
                    return rgGrigliaAllegati.Columns.FindByUniqueName("ColonnaElimina");
                }
                else
                {
                    return null;
                }
            }
        }

        public IQueryable<Entities.Allegato> DataSource { get; set; }

        #endregion

        #region Costanti

        //private string EVENT_TARGET_MEMORIZZA_FILE_CARICATI = "MemorizzaFileCaricati";

        #endregion

        #region Eventi

        /// <summary>
        /// Evento che viene scatenato quando è necessario recuperare i dati
        /// </summary>
        public event EventHandler NeedDataSource;

        /// <summary>
        /// Evento che viene scatenato quando è necessario effettuare la cancellazione dell'allegato selezionato
        /// </summary>
        public event EventHandler<DocumentoAllegatoDaSalvareEventArgs> NeedDeleteAllegato;

        /// <summary>
        /// Evento che viene scatenato quando è necessario effettuare il salvataggio dell'allegato caricato
        /// </summary>
        public event EventHandler<DocumentoAllegatoDaSalvareEventArgs> NeedSaveAllegato;

        /// <summary>
        /// Evento che viene scatenato quando viene creata la riga della griglia
        /// </summary>
        public event EventHandler<DocumentoAllegatoInItemDataBoundEventArgs> ItemDataBound;

        #endregion

        #region Metodi Pubblici

        /// <summary>
        /// Effettua l'inizializzazione dell'usercontrl
        /// </summary>
        //public void Inizializza(Entities..ScopoTipologiaAllegatoEnum scopoTipologiaAllegatoDaGestire)
        public void Inizializza()
        {
            //GestisciDeleteEnabled();
            rgGrigliaAllegati.Rebind();
            //FillComboTipologieDocumento();
        }

        #endregion

        #region Intercettazione Eventi

        /// <summary>
        /// Metodo di gestione del metodo load della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Request != null &&
            //    Request["__EVENTTARGET"] == EVENT_TARGET_MEMORIZZA_FILE_CARICATI &&
            //    Request["__EVENTARGUMENT"] == ClientID)
            //{
            //    GestisciFileCaricato();
            //}

            //Traduce le voci del menu di filtro della griglia. Questa operazione deve essere fatta ad ogni post            
            Helper.TelerikHelper.TraduciMenuFiltro(rgGrigliaAllegati.FilterMenu);

            // Viene ignettato il codice javascript necessario per l'usercontrol
            InjectJavascript();
            InjectAjaxManagerProxy();
            
            //if (!SeCoGes.Hdemia.Helper.Web.IsPostOrCallBack(Page))
            //{
            //    FillComboTipologieDocumento();
            //}
        }

        /// <summary>
        /// Metodo di gestione dell'evento Carica, quest'ultimo avvia l'upload del file selezionato e lo memorizza nel db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbCarica_Click(object sender, EventArgs e)
        {
            try
            {
                GestisciFileCaricato();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(Page, ex);
            }
        }

        #region Griglia Allegati

        /// <summary>
        /// Metodo di gestione dell'evento DeleteCommand della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGrigliaAllegati_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                Guid idSelectedRow = (Guid)(((GridDataItem)(e.Item)).GetDataKeyValue("ID"));
                if (!idSelectedRow.IsNullOrEmpty())
                {
                    Logic.Allegati ll = new Logic.Allegati();
                    Entities.Allegato entityToDelete = ll.Find(idSelectedRow);

                    RaiseOnNeedDeleteAllegato(entityToDelete);
                }
            }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);
                MessageHelper.ShowErrorMessage(Page, ex);
                e.Canceled = true;
            }
            finally
            {
                rgGrigliaAllegati.Rebind();
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento NeedDataSource della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGrigliaAllegati_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                RaiseOnNeedDataSource();
                rgGrigliaAllegati.DataSource = DataSource;
            }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);
                MessageHelper.ShowErrorMessage(Page, ex);
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento ItemCreated della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGrigliaAllegati_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            //Traduce le voci del menu di filtro della griglia. Questa operazione deve essere fatta ad ogni post
            Helper.TelerikHelper.TraduciElementiGriglia(e);
        }

        /// <summary>
        /// Metodo di gestione dell'evento ItemDataBound della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGrigliaAllegati_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = (GridDataItem)e.Item;

                    if (e.Item.DataItem is Entities.Allegato)
                    {
                        Entities.Allegato entity = (Entities.Allegato)e.Item.DataItem;
                        TableCell colonnaElimina = dataItem["ColonnaElimina"];
                        ImageButton ibElimina = colonnaElimina.Controls[0] as ImageButton;

                        RaiseOnItemDataBound(entity, ibElimina);
                    }
                }
            }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);
                MessageHelper.ShowErrorMessage(Page, ex);
                e.Canceled = true;
            }
        }

        #endregion

        #endregion
        
        #region Funzioni Accessorie

        /// <summary>
        /// Effettua la gestione del readonly sulla griglia
        /// </summary>
        private void GestisciReadOnly()
        {
            if (ColonnaEstensioneFile != null)
                ColonnaEstensioneFile.Visible = !IsReadOnly;

            if (ColonnaUserName != null)
                ColonnaUserName.Visible = !IsReadOnly;

            if (ColonnaDataArchiviazione != null)
                ColonnaDataArchiviazione.Visible = !IsReadOnly;

            if (ColonnaElimina != null)
                ColonnaElimina.Visible = !IsReadOnly;

            lrCaricamentoFile.Visible = !IsReadOnly;
            rtbNote.ReadOnly = IsReadOnly;
            //rcbTipologiaDocumento.Enabled = !IsReadOnly;
        }

        /// <summary>
        /// Effettua la gestione del DeleteEnabled sulla griglia
        /// </summary>
        private void GestisciDeleteEnabled()
        {
            if (ColonnaElimina != null)
                ColonnaElimina.Visible = IsDeleteEnabled;
        }

        /// <summary>
        /// Effettua la gestione del file caricato dall'oggetto RadUpload
        /// </summary>
        private void GestisciFileCaricato()
        {
            // Nel caso in cui esistano dei file caricati ..
            if (ruCaricamentoAllegato.UploadedFiles != null || ruCaricamentoAllegato.UploadedFiles.Count > 0)
            {
                // Per ogni file caricato ..
                foreach (UploadedFile fileCaricato in ruCaricamentoAllegato.UploadedFiles)
                {
                    // Viene richiamato il metodo che effettua la memorizzazione del file corrente (nel ciclo)
                    MemorizzaFile(fileCaricato);
                }

                // Vengono rimossi i file caricati e la griglia ricaricata
                ruCaricamentoAllegato.UploadedFiles.Clear();
                rtbNote.Text = String.Empty;
                //rcbTipologiaDocumento.ClearSelection();
                rgGrigliaAllegati.Rebind();
            }
            else
            {
                throw new Exception("Non è stato selezionato nessun file.");
            }

            //// Nel caso in cui esistano dei file caricati ..
            //if (Request.Files != null && Request.Files.Count > 0)
            //{
            //    HttpPostedFile fileCaricato = Request.Files[0];

            //    // Viene richiamato il metodo che effettua la memorizzazione del file corrente (nel ciclo)
            //    MemorizzaFile(fileCaricato);

            //    // Vengono rimossi i file caricati e la griglia ricaricata
            //    //ruCaricamentoAllegato.UploadedFiles.Clear();
            //    rgGrigliaAllegati.Rebind();

            //    rtbNote.Text = String.Empty;
            //}
            //else
            //{
            //    throw new Exception("Non è stato selezionato nessun file.");
            //}
        }

        ///// <summary>
        ///// Effettua la memorizzazione del file caricato nel database, più precisamente nella tabella "Allegato.Documento"
        ///// </summary>
        ///// <param name="fileCaricato"></param>
        //private void MemorizzaFileNelDatabase(UploadedFile fileCaricato)
        //{
        //    if (fileCaricato == null)
        //    {
        //        throw new ArgumentNullException("fileCaricato", "Parametro nullo.");
        //    }

        //    Entities.ApplicazioneAccount utenteCollegato = InformazioniAccountAutenticato.GetIstance().Account;
        //    if (utenteCollegato == null)
        //    {
        //        throw new Exception(ErrorMessage.UTENTE_SCONOSCIUTO_MESSAGE);
        //    }

        //    // Viene creata l'istanza dell'entity Documento relativa agli allegati
        //    Entities.Allegato entityToCreate = new Entities.Allegato();
        //    entityToCreate.NomeFile = fileCaricato.GetName();
        //    entityToCreate.UserName = utenteCollegato.UserName;
        //    entityToCreate.Compresso = false;
        //    entityToCreate.DataArchiviazione = DateTime.Now;

        //    // Viene letto il contenuto del file .. ed inserita nella property che memorizzerà il file nel database
        //    byte[] byteFileOriginale = new byte[fileCaricato.ContentLength];
        //    fileCaricato.InputStream.Read(byteFileOriginale, 0, (int)fileCaricato.ContentLength);
        //    entityToCreate.FileAllegato = byteFileOriginale;

        //    RaiseOnNeedSaveAllegato(entityToCreate);
        //}
        
        /// <summary>
        /// Effettua la memorizzazione del file caricato nel database, più precisamente nella tabella "Allegato.Documento"
        /// </summary>
        /// <param name="fileCaricato"></param>
        private void MemorizzaFile(UploadedFile fileCaricato)
        {
            if (fileCaricato == null)
            {
                throw new ArgumentNullException("fileCaricato", "Parametro nullo.");
            }

            Entities.Account utenteCollegato = InformazioniAccountAutenticato.GetIstance().Account;
            if (utenteCollegato == null)
            {
                throw new Exception("Utente non riconosciuto!");
            }

            //if (String.IsNullOrEmpty(rcbTipologiaDocumento.SelectedValue))
            //{
            //    throw new Exception("Non è stata selezionata nessuna tipologia di documento.");
            //}

            // Viene creata l'istanza dell'entity Documento relativa agli allegati
            Entities.Allegato entityToCreate = new Entities.Allegato();
            entityToCreate.NomeFile = Path.GetFileName(fileCaricato.FileName);
            entityToCreate.UserName = utenteCollegato.UserName;

            //byte tipologiaDocumentoByte = Byte.Parse(rcbTipologiaDocumento.SelectedValue);
            //entityToCreate.TipologiaAllegatoEnum = (Entities..TipologiaAllegatoEnum)tipologiaDocumentoByte;

            entityToCreate.Note = rtbNote.Text.Trim();
            entityToCreate.Compresso = false;
            entityToCreate.DataArchiviazione = DateTime.Now;

            // Viene letto il contenuto del file .. ed inserita nella property che memorizzerà il file nel database
            byte[] byteFileOriginale = new byte[fileCaricato.ContentLength];
            fileCaricato.InputStream.Read(byteFileOriginale, 0, (int)fileCaricato.ContentLength);
            entityToCreate.FileAllegato = byteFileOriginale;

            RaiseOnNeedSaveAllegato(entityToCreate);
        }

        /// <summary>
        /// Effettua l'inserimento del javascript necessario per effettuare il caricamento di un file successivamente alla selezione
        /// </summary>
        private void InjectJavascript()
        {
            //StringBuilder sbCaricamentoAllegato = new StringBuilder();
            //sbCaricamentoAllegato.AppendLine("function (sender, eventArgs) {");
            //sbCaricamentoAllegato.AppendLine(String.Format("ruCaricamentoAllegato_OnClientFileSelected(sender, eventArgs, '{0}', '{1}');", EVENT_TARGET_MEMORIZZA_FILE_CARICATI, ClientID));
            //sbCaricamentoAllegato.AppendLine("}");

            //ruCaricamentoAllegato.OnClientFileSelected = sbCaricamentoAllegato.ToString();

            StringBuilder sbControlloCaricamentoFile = new StringBuilder();
            sbControlloCaricamentoFile.AppendLine("function (sender, eventArgs) {");
            //sbControlloCaricamentoFile.AppendLine(String.Format("rbCarica_OnClientClicking(sender, eventArgs, '{0}', '{1}');", ruCaricamentoAllegato.ClientID, rcbTipologiaDocumento.ClientID));
            sbControlloCaricamentoFile.AppendLine(String.Format("rbCarica_OnClientClicking(sender, eventArgs, '{0}', null);", ruCaricamentoAllegato.ClientID));
            sbControlloCaricamentoFile.AppendLine("}");
            rbCarica.OnClientClicking = sbControlloCaricamentoFile.ToString();

            StringBuilder sbRadProgressManagerClientProgressStarted = new StringBuilder();
            sbRadProgressManagerClientProgressStarted.AppendLine("function (sender, eventArgs) {");
            sbRadProgressManagerClientProgressStarted.AppendLine(String.Format("OnRadProgressManagerClientProgressStarted(sender, eventArgs, '{0}', '{1}');", ClientID, rpaCarcamentoAllegati.ClientID));
            sbRadProgressManagerClientProgressStarted.AppendLine("}");

            rpmCaricamentoAllegati.OnClientProgressStarted = sbRadProgressManagerClientProgressStarted.ToString();
        }

        /// <summary>
        /// Effettua l'injection dell'ajax manager che gestisce le operazioni sulla griglia degli allegati
        /// </summary>
        private void InjectAjaxManagerProxy()
        {
            if (!EnableAjax) return;

            divContenitoreAjax.Controls.Clear();

            // Vengono recuperati gli oggetti RadAjaxLoadingPanel e RadWindowManager
            RadAjaxLoadingPanel ralpMaster = MasterPageHelper.GetRadAjaxLoadingPanel(Page);
            RadWindowManager rwmMaster = MasterPageHelper.GetRadWindowManager(Page);

            // Vengono creati gli oggetti AjaxUpdatedControl da inserire nelle impostazioni dell'oggetto RadAjaxManagerProxy
            AjaxUpdatedControl upcGrigliaAllegati = new AjaxUpdatedControl(rgGrigliaAllegati.ID, ralpMaster.ID);
            AjaxUpdatedControl upcRadWindowManager = new AjaxUpdatedControl(rwmMaster.ID, String.Empty);

            // Viene creato l'oggetto AjaxSetting da inserire nell'oggetto RadAjaxManagerProxy
            AjaxSetting asGrigliaAllegati = new AjaxSetting(rgGrigliaAllegati.ID);
            asGrigliaAllegati.UpdatedControls.Add(upcGrigliaAllegati);
            asGrigliaAllegati.UpdatedControls.Add(upcRadWindowManager);

            // Viene creato l'oggetto RadAjaxManagerProxy da inserire nel div contenitore per l'oggetto appena creato
            RadAjaxManagerProxy ramCaricamentoAllegati = new RadAjaxManagerProxy();
            ramCaricamentoAllegati.ID = "ramCaricamentoAllegati";
            ramCaricamentoAllegati.AjaxSettings.Add(asGrigliaAllegati);

            // Viene aggiunto il controllo RadAjaxManagerProxy nell'apposito contenitore
            divContenitoreAjax.Controls.Add(ramCaricamentoAllegati);
        }

        /// <summary>
        /// Scatena l'evento di richiesta del salvataggio dell'entity Allegato_Documento passata come parametro
        /// </summary>
        /// <param name="allegatoDocumento"></param>
        private void RaiseOnNeedDataSource()
        {
            if (NeedDataSource != null)
            {
                NeedDataSource(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Scatena l'evento di richiesta del salvataggio dell'entity Allegato_Documento passata come parametro
        /// </summary>
        /// <param name="documentoAllegato"></param>
        private void RaiseOnNeedSaveAllegato(Entities.Allegato documentoAllegato)
        {
            if (NeedSaveAllegato != null)
            {
                DocumentoAllegatoDaSalvareEventArgs eventArgs = new DocumentoAllegatoDaSalvareEventArgs();
                eventArgs.DocumentoAllegato = documentoAllegato;
                NeedSaveAllegato(this, eventArgs);
            }
        }

        /// <summary>
        /// Scatena l'evento di richiesta della cancellazione dell'entity Allegato_Documento passata come parametro
        /// </summary>
        /// <param name="allegatoDocumento"></param>
        private void RaiseOnNeedDeleteAllegato(Entities.Allegato documentoAllegato)
        {
            if (NeedDeleteAllegato != null)
            {
                DocumentoAllegatoDaSalvareEventArgs eventArgs = new DocumentoAllegatoDaSalvareEventArgs();
                eventArgs.DocumentoAllegato = documentoAllegato;
                NeedDeleteAllegato(this, eventArgs);
            }
        }

        /// <summary>
        /// Scatena l'evento di gestione della riga della griglia in fase di costruzione
        /// </summary>
        /// <param name="allegatoDocumento"></param>
        private void RaiseOnItemDataBound(Entities.Allegato documentoAllegato, ImageButton tastoElimina)
        {
            if (ItemDataBound != null)
            {
                DocumentoAllegatoInItemDataBoundEventArgs eventArgs = new DocumentoAllegatoInItemDataBoundEventArgs(documentoAllegato, tastoElimina);
                ItemDataBound(this, eventArgs);
            }
        }

        ///// <summary>
        ///// Effettua il popolamento del combo relativo alle tipologie di documento
        ///// </summary>
        //private void FillComboTipologieDocumento()
        //{
        //    Logic.Allegati llAllegati = new Logic.Allegati();
        //    System.Collections.Generic.IDictionary<byte, string> elencoVoci = Logic.Allegati.ReadDescrizioneTipologieDocumenti(this.ScopoTipologiaAllegatoDaGestire);
        //    rcbTipologiaDocumento.DataSource = elencoVoci;
        //    rcbTipologiaDocumento.DataBind();
        //}

        #endregion
    }
}