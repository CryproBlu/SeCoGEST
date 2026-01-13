using System;
using System.Linq;
using SeCoGEST.Helper;
using SeCoGes.Utilities;
using Telerik.Web.UI;
using SeCoGEST.Entities;
using SeCoGEST.Logic;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Threading;
using SeCoGEST.Web.LongProcesses;
using System.ComponentModel;
using SeCoGEST.Infrastructure;

namespace SeCoGEST.Web.Interventi
{
    public partial class Interventi : System.Web.UI.Page
    {
        #region Variables

        private bool isReadOnly;
        private bool isUserAdmin;

        #endregion

        #region Properties

        /// <summary>
        /// Recupera o setta il read only della pagina
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return isReadOnly;
            }
            set
            {
                isReadOnly = value;
                GestisciReadOnly();
            }
        }

        /// <summary>
        /// Indica se la pagina è visitata da un amministratore
        /// </summary>
        public bool IsUserAdmin
        {
            get
            {
                return isUserAdmin;
            }
            set
            {
                isUserAdmin = value;
                GestisciReadOnly();
            }
        }

        /// <summary>
        /// Restituisce true se lo stato della griglia dev'essere memorizzato
        /// </summary>
        private bool SaveState { get; set; }

        #endregion

        #region Colonne Toolbars

        /// <summary>
        /// Restituisce un riferimento al pulsante Aggiorna della toolbar
        /// </summary>
        private RadToolBarButton PulsanteToolbar_Aggiorna
        {
            get
            {
                return rtbPrincipale.FindItemByValue("Aggiorna") as RadToolBarButton;
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante "Genera Invia" della toolbar
        /// </summary>
        private RadToolBarButton PulsanteToolbar_GeneraInviaInterventiSelezionati
        {
            get
            {
                return rtbPrincipale.FindItemByValue("GeneraInviaInterventiSelezionati") as RadToolBarButton;
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante "Valida Interventi" della toolbar
        /// </summary>
        private RadToolBarButton PulsanteToolbar_ValidaInterventi
        {
            get
            {
                return rtbPrincipale.FindItemByValue("ValidaInterventiSelezionati") as RadToolBarButton;
            }
        }
        private RadToolBarButton PulsanteToolbar_SeparatoreValidaInterventiSelezionati
        {
            get
            {
                return rtbPrincipale.FindItemByValue("SeparatoreValidaInterventiSelezionati") as RadToolBarButton;
            }
        }        

        /// <summary>
        /// Restituisce un riferimento al pulsante "Esporta Interventi" della toolbar
        /// </summary>
        private RadToolBarButton PulsanteToolbar_EsportaInterventi
        {
            get
            {
                return rtbPrincipale.FindItemByValue("EsportaInterventi") as RadToolBarButton;
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante FiltroGriglia della toolbar
        /// </summary>
        private RadToolBarButton PulsanteToolbar_FiltroGriglia
        {
            get
            {
                return rtbPrincipale.FindItemByValue("FiltroGriglia") as RadToolBarButton;
            }
        }

        /// <summary>
        /// Restituisce un riferimento al combobox contenente i Filtri nella toolbar
        /// </summary>
        private RadComboBox ComboFiltriIntervento
        {
            get
            {
                if (PulsanteToolbar_FiltroGriglia != null)
                {
                    RadComboBox rcbFiltriIntervento = PulsanteToolbar_FiltroGriglia.FindControl("rcbFiltriIntervento") as RadComboBox;
                    return rcbFiltriIntervento;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Restituisce un riferimento al combobox presente nel pulsante FiltroGriglia della toolbar
        /// </summary>
        private RadComboBox ComboFiltriPerStatoIntervento
        {
            get
            {
                if (PulsanteToolbar_FiltroGriglia != null)
                {
                    RadComboBox comboFiltriPerStatoIntervento = PulsanteToolbar_FiltroGriglia.FindControl("rcbInterventoApertoChiuso") as RadComboBox;
                    return comboFiltriPerStatoIntervento;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region Colonne Griglia

        private GridColumn ColonnaModifica
        {
            get
            {
                return rgGriglia.MasterTableView.GetColumn("ColonnaModificaPost");
            }
        }
        private GridColumn ColonnaElimina
        {
            get
            {
                return rgGriglia.MasterTableView.GetColumn("ColonnaElimina");
            }
        }

        private GridColumn ColonnaSelezionato
        {
            get
            {
                return rgGriglia.MasterTableView.GetColumn("ColonnaSelezionato");
            }
        }

        #endregion

        #region Intercettazione Eventi

        #region Pagina

        /// <summary>
        /// Metodo di gestione dell'evento Init della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            RadPersistenceHelper.AssociatePersistenceSessionProvider(this);
        }

        /// <summary>
        /// Metodo di gestione dell'evento Load della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                IsUserAdmin = false;

                //Traduce le voci del menu di filtro della griglia. Questa operazione deve essere fatta ad ogni post
                TelerikHelper.TraduciMenuFiltro(rgGriglia.FilterMenu);

                if (!Helper.Web.IsPostOrCallBack(this))
                {
                    RadPersistenceHelper.LoadState(this);

                    //TelerikRadGridHelper.ManageExelExportSettings(rgGriglia, "Elenco_Interventi", true);

                    ApplicaRedirectPulsanteAggiorna();
                    FillComboFiltriInterventi();
                }
                else
                {
                    // Registrazione degli script necessari ai filtri
                    TelerikRadGridHelper.InjectScriptToHiddenFilterItemIfEmpty(this.Page, rgGriglia);
                }

                // Applica le eventuali restrizioni alle funzionalità della pagina in base all'utente autenticato
                GestisciAutorizzazioni();

                //if (IsPostBack && Request["__EVENTTARGET"] == "SvuotaFiltri") // Caso in cui viene richiesto lo svuotamento dei filtri
                //{
                //    TelerikRadGridHelper.ClearFilters(rgGriglia, true);
                //}
            }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento PreRender della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (SaveState)
            {
                RadPersistenceHelper.SaveState(this);
            }
        }

        #endregion

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
        /// Metodo di gestione dell'evento ItemCommand della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGriglia_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            SaveState = true;
            RadPersistenceHelper.SaveState(this);

            if (e == null) return;

            switch (e.CommandName)
            {
                //case "ModificaCommand":
                //    Guid idValue = TelerikRadGridHelper.GetDataKeyValueFromGridCommandEventArgsItem<Guid>(rgGriglia, e, "ID");
                //    Response.Redirect(string.Format("/Interventi/Intervento.aspx?ID={0}", idValue));
                //    break;

                default:
                    break;

            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento DeleteCommand della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGriglia_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {

            Logic.Interventi llInterventi = new Logic.Interventi();

            try
            {
                Guid idSelectedRow = (Guid)(((GridDataItem)(e.Item)).GetDataKeyValue("ID"));
                if (!idSelectedRow.IsNullOrEmpty())
                {
                    Entities.Intervento entityToDelete = llInterventi.Find(idSelectedRow);
                    if (entityToDelete != null)
                    {
                        //Entities.ApplicazioneAccount utenteCollegato = InformazioniSessione.GetIstance().AccountCollegato;
                        //if (utenteCollegato == null)
                        //{
                        //    throw new Exception(ErrorMessage.UTENTE_SCONOSCIUTO_MESSAGE);
                        //}

                        llInterventi.Delete(entityToDelete, false);
                        llInterventi.SubmitToDatabase();

                        SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - Rimosso l'entity Intervento con ID '{1}'.", Request.Url.AbsolutePath, entityToDelete.ID));
                    }
                }
            }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);
                MessageHelper.ShowErrorMessage(Page, "Operazione di Eliminazione non riuscita, è stato riscontrato il seguente errore:<br />" + ex.Message);
                e.Canceled = true;
            }
            finally
            {
                rgGriglia.Rebind();
            }
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
                InformazioniAccountAutenticato utenteLoggato = InformazioniAccountAutenticato.GetIstance();
                if (utenteLoggato != null && utenteLoggato.Account != null)
                {
                    Account account = utenteLoggato.Account;

                    Logic.Interventi llInterventi = new Logic.Interventi();

                    IQueryable<Entities.Intervento> elencoInterventi = llInterventi.ReadByAccount(utenteLoggato.Account);
                    List<StatoInterventoEnum> filtriPerStatoDaApplicare = GetFiltriPerStatoDaApplicareAllaGriglia();
                    List<FiltroInterventoEnum> filtriDaApplicare = GetFiltriInterventoDaApplicareAllaGriglia();
                    elencoInterventi = ApplicaFiltri_Stato(elencoInterventi, filtriDaApplicare, filtriPerStatoDaApplicare, llInterventi);
                    //elencoInterventi = ApplicaFiltroDataSource(elencoInterventi, filtriPerStatoDaApplicare);

                    rgGriglia.DataSource = elencoInterventi;

                    //rtbPrincipale.Visible = (elencoInterventi.Count() > 0);
                }
                else
                {
                    throw new Exception(Infrastructure.ErrorMessage.UTENTE_SCONOSCIUTO_MESSAGE);
                }
            }
            catch (Exception ex)
            {
                //rtbPrincipale.Visible = false;
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

            if (e != null && e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;

                if (item.DataItem is Entities.Intervento)
                {
                    Entities.Intervento intervento = (Entities.Intervento)item.DataItem;

                    System.Web.UI.WebControls.TableCell cellaGeneraIntervento = item["ColonnaGeneraIntervento"];
                    System.Web.UI.WebControls.HyperLink hlGeneraIntervento = (System.Web.UI.WebControls.HyperLink)cellaGeneraIntervento.Controls[0];

                    hlGeneraIntervento.NavigateUrl = GeneratoreDocumentiHelper.GetPercorsoGenerazioneDocumenti<Entities.Intervento>(intervento.Identifier,
                                                                                                                                    TipologiaDocumentiDaGenerareEnum.Intervento,
                                                                                                                                    GestoreDocumenti.GetNomeDocumentoIntervento(intervento));

                    hlGeneraIntervento.ToolTip = "Effettua la generazione del documento relativo all'intervento corrente";

                    if (intervento.Urgente.HasValue && intervento.Urgente.HasValue && 
                        intervento.StatoEnum.HasValue &&
                        intervento.StatoEnum.Value != StatoInterventoEnum.Chiuso && 
                        intervento.StatoEnum.Value != StatoInterventoEnum.Validato)
                    {
                        Logic.Interventi llInterventi = new Logic.Interventi();
                        if (!llInterventi.IsPresoInCarico(intervento.Identifier))
                        {
                            e.Item.BackColor = ConfigurationKeys.ColoreIntervento_ApertoUrgente;
                            e.Item.ToolTip = "Intervento Urgente - Non Preso in carico";
                        }
                    }

                    if (intervento.StatoEnum.HasValue)
                    {
                        if (intervento.StatoEnum.Value == StatoInterventoEnum.Aperto && intervento.Urgente.HasValue && intervento.Urgente.Value)
                        {
                            e.Item.BackColor = ConfigurationKeys.ColoreIntervento_ApertoUrgente;
                        }

                        else if (intervento.StatoEnum.Value == StatoInterventoEnum.InGestione)
                        {
                            e.Item.BackColor = ConfigurationKeys.ColoreIntervento_InGestione;
                        }

                        else if (intervento.StatoEnum.Value == StatoInterventoEnum.Eseguito)
                        {
                            e.Item.BackColor = ConfigurationKeys.ColoreIntervento_Eseguito;
                        }

                        else if (intervento.StatoEnum.Value == StatoInterventoEnum.Chiuso)
                        {
                            e.Item.BackColor = ConfigurationKeys.ColoreIntervento_Chiuso;
                        }

                        else if (intervento.StatoEnum.Value == StatoInterventoEnum.Validato)
                        {
                            e.Item.BackColor = ConfigurationKeys.ColoreIntervento_Validato;
                        }

                        else if (intervento.StatoEnum.Value == StatoInterventoEnum.Sostituito)
                        {
                            e.Item.BackColor = ConfigurationKeys.ColoreIntervento_Sostituito;
                        }


                        e.Item.ToolTip = String.Format("Intervento {0}", intervento.StatoString);
                   }
                }
            }
        }

        #endregion

        #region Toolbar

        /// <summary>
        /// Metodo di gestione dell'evento click della toolbar presente nella pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rtbPrincipale_ButtonClick(object sender, RadToolBarEventArgs e)
        {
            try
            {
                if (e != null && e.Item != null)
                {
                    string value = e.Item.Value;

                    if (value == PulsanteToolbar_GeneraInviaInterventiSelezionati.Value)
                    {
                        EffettuaGenerazioneInvioInterventiSelezionati();
                    }
                    else if (value == PulsanteToolbar_EsportaInterventi.Value)
                    {
                        EffettuaEsportazioneInterventiInFileExcel();
                    }
                    else if (value == PulsanteToolbar_ValidaInterventi.Value)
                    {
                        EffettuaValidazioneInterventiSelezionati();
                    }
                }
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        /// <summary>
        /// Effettua l'esportazione di tutta la griglia degli interventi in formato excel
        /// </summary>
        private void EffettuaEsportazioneInterventiInFileExcel()
        {
            rgGriglia.ExportSettings.FileName = "Elenco_Interventi";
            rgGriglia.ExportSettings.IgnorePaging = true;
            rgGriglia.ExportSettings.ExportOnlyData = true;
            rgGriglia.ExportSettings.OpenInNewWindow = true;
            rgGriglia.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
            rgGriglia.ExportSettings.Excel.FileExtension = "xls";
            rgGriglia.ExportToExcel();
        }

        #endregion

        protected void btApplyFilter_Click(object sender, EventArgs e)
        {
            InformazioniAccountAutenticato accountAutenticato = InformazioniAccountAutenticato.GetIstance();
            if (accountAutenticato != null &&
                accountAutenticato.SessioneCorrente != null)
            {
                accountAutenticato.SessioneCorrente.FiltriPerStatoGrigliaIntervento = GetFiltriPerStatoDaApplicareAllaGriglia();
                accountAutenticato.SessioneCorrente.FiltriGrigliaIntervento = GetFiltriInterventoDaApplicareAllaGriglia();
            }

            rgGriglia.Rebind();
        }

        //#region Combo rcbInterventoApertoChiuso

        ///// <summary>
        ///// Metodo di gestione dell'evento CheckAllCheck del combo presente nel pulsante FiltroGriglia della toolbar
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void rcbInterventoApertoChiuso_CheckAllCheck(object sender, RadComboBoxCheckAllCheckEventArgs e)
        //{
        //    InformazioniAccountAutenticato accountAutenticato = InformazioniAccountAutenticato.GetIstance();
        //    if (accountAutenticato != null &&
        //        accountAutenticato.SessioneCorrente != null)
        //    {
        //        accountAutenticato.SessioneCorrente.FiltriGrigliaIntervento = GetFiltriDaApplicareAllaGriglia();
        //    }

        //    rgGriglia.Rebind();
        //}

        ///// <summary>
        ///// Metodo di gestione dell'evento CheckAllCheck del combo presente nel pulsante FiltroGriglia della toolbar
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void rcbInterventoApertoChiuso_ItemChecked(object sender, RadComboBoxItemEventArgs e)
        //{
        //    InformazioniAccountAutenticato accountAutenticato = InformazioniAccountAutenticato.GetIstance();
        //    if (accountAutenticato != null &&
        //        accountAutenticato.SessioneCorrente != null)
        //    {
        //        accountAutenticato.SessioneCorrente.FiltriGrigliaIntervento = GetFiltriDaApplicareAllaGriglia();
        //    }

        //    rgGriglia.Rebind();
        //}

        //#endregion

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Assegna l'indirizzo della pagina da aprire al pulsante Aggiorna.
        /// L'indirizzo è esattamente lo stesso della pagina aperta.
        /// </summary>
        private void ApplicaRedirectPulsanteAggiorna()
        {
            if (PulsanteToolbar_Aggiorna != null)
                PulsanteToolbar_Aggiorna.NavigateUrl = Request.Url.ToString();
        }

        /// <summary>
        /// Effettua la gestione del readonly sulla griglia
        /// </summary>
        private void GestisciReadOnly()
        {
            if (rgGriglia != null && rgGriglia.Columns.Count > 0)
            {
                ColonnaSelezionato.Visible = !IsReadOnly;
                //ColonnaElimina.Visible = !IsReadOnly;
                //ColonnaElimina.Visible = IsUserAdmin;
                PulsanteToolbar_ValidaInterventi.Visible = !IsReadOnly;
                PulsanteToolbar_SeparatoreValidaInterventiSelezionati.Visible = !IsReadOnly;
            }                
        }

        /// <summary>
        /// Restituisce l'elenco degli interventi selezionati nella griglia degli interventi
        /// </summary>
        /// <returns></returns>
        private List<Entities.EntityId<Entities.Intervento>> ReadInterventiSelezionati()
        {
            List<Entities.EntityId<Entities.Intervento>> elencoIdentificativiInterventiDaGenerare = new List<EntityId<Entities.Intervento>>();

            foreach (GridDataItem item in rgGriglia.MasterTableView.Items)
            {
                CheckBox chkSelezionato = item.FindControl("chkSelezionato") as CheckBox;
                if (chkSelezionato == null)
                {
                    throw new Exception("Non è stato possibile recuperare l'oggetto che indica la selezione di un intervento");
                }
                else
                {
                    if (chkSelezionato.Checked)
                    {
                        Guid idIntervento = (Guid)item.GetDataKeyValue("ID");
                        elencoIdentificativiInterventiDaGenerare.Add(new EntityId<Entities.Intervento>(idIntervento));
                    }                    
                }
            }

            return elencoIdentificativiInterventiDaGenerare;
        }

        /// <summary>
        /// Effettua la generazione e l'invio degli interventi selezionati nella griglia
        /// </summary>
        private void EffettuaGenerazioneInvioInterventiSelezionati()
        {
            List<Entities.EntityId<Entities.Intervento>> elencoInterventi = ReadInterventiSelezionati();
            if (elencoInterventi == null || elencoInterventi.Count <= 0)
            {
                throw new Exception("Non è stato selezionato nessun intervento.");
            }
            else
            {
                InformazioniAccountAutenticato istanza = InformazioniAccountAutenticato.GetIstance();
                if (istanza.Account == null)
                {
                    throw new Exception("Non è stato possibile recuperare le informazioni relative all'utente loggato.");
                }
                else if (String.IsNullOrEmpty(istanza.Account.Email))
                {
                    throw new Exception(String.Format("Per effettuare la generazione e l'invio degli incarichi generati è necessario indicare una email nella pagina relativa al proprio profilo."));
                }

                InviaInterventiGeneratiLongProcess longProcess = new InviaInterventiGeneratiLongProcess();
                object[] elencoParametri = new object[2];
                elencoParametri[0] = elencoInterventi;
                elencoParametri[1] = new Entities.EntityId<Entities.Account>(istanza.Account.ID);

                object parametri = (object)elencoParametri;

                ParameterizedThreadStart ts = new ParameterizedThreadStart(longProcess.GeneraInviaInterventi);
                Thread thd = new Thread(ts);
                thd.IsBackground = true;
                thd.Start(parametri);

                string message = String.Format("Al completamento dell'operazione di generazione ed invio dell'intervento verrà inviata una email all'indirizzo <strong>{0}</strong> che conterrà l'esito della procedura.", istanza.Account.Email);
                MessageHelper.ShowMessage(this, "Operazione di Generazione ed Invio Intervento", message);
            }
        }

        private void EffettuaValidazioneInterventiSelezionati()
        {
            List<Entities.EntityId<Entities.Intervento>> elencoInterventi = ReadInterventiSelezionati();
            if (elencoInterventi == null || elencoInterventi.Count <= 0)
            {
                throw new Exception("Non è stato selezionato nessun intervento.");
            }
            else
            {
                InformazioniAccountAutenticato istanza = InformazioniAccountAutenticato.GetIstance();
                if (istanza.Account == null)
                {
                    throw new Exception("Non è stato possibile recuperare le informazioni relative all'utente loggato.");
                }
                else if (String.IsNullOrEmpty(istanza.Account.Email))
                {
                    throw new Exception(String.Format("Per effettuare la generazione e l'invio degli incarichi generati è necessario indicare una email nella pagina relativa al proprio profilo."));
                }



                List<Entities.EntityId<Entities.Intervento>> elencoIdInterventiConProblemi = new List<Entities.EntityId<Entities.Intervento>>();

                Logic.Interventi logicInterventi = new Logic.Interventi();

                // Per ogni identificativo nell'elenco degli interventi da generare
                foreach (Entities.EntityId<Entities.Intervento> identificativoIntervento in elencoInterventi)
                {
                    bool transactionStarted = false;

                    try
                    {
                        // Effettua la validazione dell'intervento corrente del ciclo
                        Entities.Intervento interventoCorrente = logicInterventi.Find(identificativoIntervento);
                        if(interventoCorrente != null)
                        {
                            if (!ValidaDatiInterventoPerValidazione(interventoCorrente).HaveMessages)
                            {
                                // Se lo stato dell'intervento è corretto allora lo valida e lo chiude
                                if (interventoCorrente.StatoEnum != StatoInterventoEnum.Validato)
                                {
                                    //logicInterventi.CreaDocXMLInterventi(interventoCorrente.ID);
                                    if (logicInterventi.InviaInterventoAContabilita(interventoCorrente.ID, out string message))
                                    {
                                        logicInterventi.StartTransaction();
                                        transactionStarted = true;

                                        Entities.Intervento_Stato nuovoStato = new Intervento_Stato();
                                        nuovoStato.Intervento = interventoCorrente;
                                        nuovoStato.StatoEnum = StatoInterventoEnum.Validato;
                                        nuovoStato.NomeUtente = Helper.Web.GetLoggedUserName();
                                        nuovoStato.Data = DateTime.Now;
                                        Logic.Intervento_Stati llIS = new Logic.Intervento_Stati(logicInterventi);
                                        llIS.Create(nuovoStato, false);

                                        interventoCorrente.Chiuso = true;
                                        logicInterventi.SubmitToDatabase();

                                        logicInterventi.CommitTransaction();
                                    }
                                    else
                                    {
                                        throw new Exception(message);
                                    }
                                }
                            }
                            else
                            {
                                elencoIdInterventiConProblemi.Add(identificativoIntervento);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Viene aggiunto l'identificativo del documento la cui generazione è andata in errore
                        elencoIdInterventiConProblemi.Add(identificativoIntervento);
                        if(transactionStarted) logicInterventi.RollbackTransaction();
                    }
                }


                if(elencoIdInterventiConProblemi.Count() > 0)
                {
                    string elencoId = string.Join(", ", elencoIdInterventiConProblemi);
                    MessageHelper.ShowMessage(this, "Validazione e Chiusura Interventi", "Operazione di Validazione e Chiusura Interventi Completata con errori: " + elencoId);
                }
                else
                {
                    MessageHelper.ShowMessage(this, "Validazione e Chiusura Interventi", "Operazione di Validazione e Chiusura Interventi Completata!");
                }
            }
        }

        /// <summary>
        /// Effettua la validazione dei dati presenti nella griglia
        /// </summary>
        /// <param name="statoIntervento"></param>
        /// <returns></returns>
        public SeCoGes.Utilities.MessagesCollector ValidaDatiInterventoPerValidazione(Entities.Intervento interventoCorrente)
        {
            //Logic.Operatori llOperatori = new Logic.Operatori();
            SeCoGes.Utilities.MessagesCollector listaErrori = new SeCoGes.Utilities.MessagesCollector();

            if (!interventoCorrente.DataPrevistaIntervento.HasValue)
            {
                listaErrori.Add($"Intervento: {interventoCorrente.Numero} - Indicare la Data dell'intervento!");
            }

            if (interventoCorrente.Intervento_Articolos.Count() == 0)
            {
                listaErrori.Add($"La validazione dell'intervento {interventoCorrente.Numero} richiede che sia indicato almeno un articolo!");
            }

            if (!interventoCorrente.TuttiGliArticoloConTempiIndicati)
            {
                listaErrori.Add($"La validazione dell'intervento {interventoCorrente.Numero} richiede che sia indicato il tempo a tutti gli articoli!");
            }

            List<Entities.Intervento_Operatore> elencoOperatori = interventoCorrente.Intervento_Operatores.ToList();
            if (elencoOperatori != null && elencoOperatori.Count > 0)
            {
                foreach (Entities.Intervento_Operatore operatoreCorrente in elencoOperatori)
                {
                    if (operatoreCorrente != null && operatoreCorrente.Operatore.Area)
                    {
                        if (interventoCorrente.StatoEnum == StatoInterventoEnum.Chiuso || interventoCorrente.StatoEnum == StatoInterventoEnum.Validato)
                        {
                            if (operatoreCorrente.IDModalitaRisoluzione.HasValue)
                            {
                                listaErrori.Add($"Intervento: {interventoCorrente.Numero}, Operatore: {operatoreCorrente.CognomeNomeOperatore} - Non è possibile indicare una Modalità di Risoluzione per gli operatori che sono impostati come area");
                            }
                            if (operatoreCorrente.InizioIntervento.HasValue)
                            {
                                listaErrori.Add($"Intervento: {interventoCorrente.Numero}, Operatore: {operatoreCorrente.CognomeNomeOperatore} - Non è possibile indicare la Data di Inizio Intervento per gli operatori che sono impostati come area");
                            }
                            if (operatoreCorrente.FineIntervento.HasValue)
                            {
                                listaErrori.Add($"Intervento: {interventoCorrente.Numero}, Operatore: {operatoreCorrente.CognomeNomeOperatore} - Non è possibile indicare la Data di Fine Intervento per gli operatori che sono impostati come area");
                            }
                            if (operatoreCorrente.DurataMinuti.HasValue)
                            {
                                listaErrori.Add($"Intervento: {interventoCorrente.Numero}, Operatore: {operatoreCorrente.CognomeNomeOperatore} - Non è possibile indicare la Durata dell'Intervento per gli operatori che sono impostati come area");
                            }
                            if (operatoreCorrente.PausaMinuti.HasValue)
                            {
                                listaErrori.Add($"Intervento: {interventoCorrente.Numero}, Operatore: {operatoreCorrente.CognomeNomeOperatore} - Non è possibile indicare la Pausa dell'Intervento per gli operatori che sono impostati come area");
                            }
                        }
                    }
                    else
                    {
                        if (interventoCorrente.StatoEnum == StatoInterventoEnum.Chiuso || interventoCorrente.StatoEnum == StatoInterventoEnum.Validato)
                        {
                            if (operatoreCorrente.IDModalitaRisoluzione < 0)
                            {
                                listaErrori.Add($"Intervento: {interventoCorrente.Numero}, Operatore: {operatoreCorrente.CognomeNomeOperatore} - Indicare una Modalità di Risoluzione");
                            }
                            if (operatoreCorrente.InizioIntervento == DateTime.MinValue)
                            {
                                listaErrori.Add($"Intervento: {interventoCorrente.Numero}, Operatore: {operatoreCorrente.CognomeNomeOperatore} - Indicare la Data di Inizio Intervento");
                            }
                            if (operatoreCorrente.FineIntervento == DateTime.MinValue)
                            {
                                listaErrori.Add($"Intervento: {interventoCorrente.Numero}, Operatore: {operatoreCorrente.CognomeNomeOperatore} - Indicare la Data di Fine Intervento");
                            }
                            if (operatoreCorrente.DurataMinuti == 0)
                            {
                                listaErrori.Add($"Intervento: {interventoCorrente.Numero}, Operatore: {operatoreCorrente.CognomeNomeOperatore} - Indicare la Durata dell'Intervento");
                            }
                        }
                    }
                }
            }

            return listaErrori;
        }

        #endregion

        #region Filtri

        /// <summary>
        /// Restituisce il data source da applicare al combo presente nel pulsante FiltroGriglia della toolbar
        /// </summary>
        /// <returns></returns>
        private IDictionary<int, string> GetDataSourceComboFiltriIntervento()
        {
            IDictionary<int, string> valoreDaRestituire = SeCoGes.Utilities.EnumHelper.GetDescriptionFromEnum<FiltroInterventoEnum, int>();
            return valoreDaRestituire;
        }

        /// <summary>
        /// Restituisce l'elenco dei filtri selezionati presenti in sessione
        /// </summary>
        /// <returns></returns>
        //private List<StatoInterventoEnum> GetElencoFiltriSelezionatiDaSessione()
        //{
        //    List<StatoInterventoEnum> elencoFiltri = new List<StatoInterventoEnum>();

        //    InformazioniAccountAutenticato accountAutenticato = InformazioniAccountAutenticato.GetIstance();
        //    if (accountAutenticato != null &&
        //        accountAutenticato.SessioneCorrente != null &&
        //        accountAutenticato.SessioneCorrente.FiltriGrigliaIntervento != null &&
        //        accountAutenticato.SessioneCorrente.FiltriGrigliaIntervento.Count > 0)
        //    {
        //        elencoFiltri = accountAutenticato.SessioneCorrente.FiltriGrigliaIntervento;

        //        if (elencoFiltri == null) elencoFiltri = new List<StatoInterventoEnum>();
        //    }

        //    if (elencoFiltri.Count <= 0)
        //    {
        //        elencoFiltri.Add(StatoInterventoEnum.Aperto);
        //    }

        //    return elencoFiltri;
        //}


        //private void FillComboFiltriInterventi()
        //{
        //    if (ComboFiltriSelezionati != null)
        //    {
        //        ComboFiltriSelezionati.ClearSelection();
        //        ComboFiltriSelezionati.ClearCheckedItems();
        //        ComboFiltriSelezionati.Items.Clear();

        //        List<StatoInterventoEnum> elencoFiltri = GetElencoFiltriSelezionatiDaSessione();

        //        IDictionary<int, string> dataSource = GetDataSourceComboInterventoApertoChiuso();
        //        foreach (KeyValuePair<int, string> valori in dataSource)
        //        {
        //            RadComboBoxItem item = new RadComboBoxItem(valori.Value, valori.Key.ToString());
        //            item.Checked = elencoFiltri.Any(x => x == (StatoInterventoEnum)valori.Key);
        //            ComboInterventoApertoChiuso.Items.Add(item);
        //        }
        //        //ComboInterventoApertoChiuso.DataSource = dataSource;
        //        //ComboInterventoApertoChiuso.DataBind();
        //    }





        //    combo.Items.Clear();
        //    combo.Items.Add(new RadComboBoxItem("Interventi Chiusi con più di un Articolo", "100"));
        //    combo.Items.Add(new RadComboBoxItem("Interventi Chiusi con Operatori di Aree Diverse", "101"));
        //    combo.Items.Add(new RadComboBoxItem("Interventi Chiusi con Operatori di Aree Diverse", "102"));
        //}

        /// <summary>
        /// Restituisce il data source da applicare al combo presente nel pulsante FiltroGriglia della toolbar
        /// </summary>
        /// <returns></returns>
        private IDictionary<int, string> GetDataSourceComboFiltriPerStatoIntervento()
        {
            IDictionary<int, string> valoreDaRestituire = SeCoGes.Utilities.EnumHelper.GetDescriptionFromEnum<StatoInterventoEnum, int>();
            return valoreDaRestituire;
        }

        /// <summary>
        /// Effettua il riepimento delle combo presenti nel pulsante FiltroGriglia della toolbar
        /// </summary>
        private void FillComboFiltriInterventi()
        {
            if (ComboFiltriPerStatoIntervento != null)
            { 
                ComboFiltriPerStatoIntervento.ClearSelection();
                ComboFiltriPerStatoIntervento.ClearCheckedItems();
                ComboFiltriPerStatoIntervento.Items.Clear();

                List<StatoInterventoEnum> elencoFiltri = GetElencoFiltriPerStatoSelezionatiDaSessione();
                
                IDictionary<int, string> dataSource = GetDataSourceComboFiltriPerStatoIntervento();
                foreach(KeyValuePair<int, string> valori in dataSource)
                {
                    RadComboBoxItem item = new RadComboBoxItem(valori.Value, valori.Key.ToString());
                    item.Checked = elencoFiltri.Any(x=> x == (StatoInterventoEnum)valori.Key);
                    ComboFiltriPerStatoIntervento.Items.Add(item);
                }
            }

            if (ComboFiltriIntervento != null)
            {
                ComboFiltriIntervento.ClearSelection();
                ComboFiltriIntervento.ClearCheckedItems();
                ComboFiltriIntervento.Items.Clear();

                List<FiltroInterventoEnum> elencoFiltri = GetElencoFiltriInterventoSelezionatiDaSessione();

                IDictionary<int, string> dataSource = GetDataSourceComboFiltriIntervento();
                foreach (KeyValuePair<int, string> valori in dataSource)
                {
                    RadComboBoxItem item = new RadComboBoxItem(valori.Value, valori.Key.ToString());
                    item.Checked = elencoFiltri.Any(x => x == (FiltroInterventoEnum)valori.Key);
                    ComboFiltriIntervento.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Restituisce l'elenco dei filtri selezionati presenti in sessione
        /// </summary>
        /// <returns></returns>
        private List<FiltroInterventoEnum> GetElencoFiltriInterventoSelezionatiDaSessione()
        {
            List<FiltroInterventoEnum> elencoFiltri = new List<FiltroInterventoEnum>();

            InformazioniAccountAutenticato accountAutenticato = InformazioniAccountAutenticato.GetIstance();
            if (accountAutenticato != null &&
                accountAutenticato.SessioneCorrente != null &&
                accountAutenticato.SessioneCorrente.FiltriGrigliaIntervento != null &&
                accountAutenticato.SessioneCorrente.FiltriGrigliaIntervento.Count > 0)
            {
                elencoFiltri = accountAutenticato.SessioneCorrente.FiltriGrigliaIntervento;

                if (elencoFiltri == null) elencoFiltri = new List<FiltroInterventoEnum>();
            }

            //if (elencoFiltri.Count <= 0)
            //{
            //    elencoFiltri.Add(FiltroInterventoEnum.);
            //}

            return elencoFiltri;
        }

        /// <summary>
        /// Restituisce l'elenco dei filtri selezionati presenti in sessione
        /// </summary>
        /// <returns></returns>
        private List<StatoInterventoEnum> GetElencoFiltriPerStatoSelezionatiDaSessione()
        {
            List<StatoInterventoEnum> elencoFiltri = new List<StatoInterventoEnum>();

            InformazioniAccountAutenticato accountAutenticato = InformazioniAccountAutenticato.GetIstance();
            if (accountAutenticato != null &&
                accountAutenticato.SessioneCorrente != null &&
                accountAutenticato.SessioneCorrente.FiltriPerStatoGrigliaIntervento != null &&
                accountAutenticato.SessioneCorrente.FiltriPerStatoGrigliaIntervento.Count > 0)
            {
                elencoFiltri = accountAutenticato.SessioneCorrente.FiltriPerStatoGrigliaIntervento;

                if (elencoFiltri == null) elencoFiltri = new List<StatoInterventoEnum>();
            }

            if (elencoFiltri.Count <= 0)
            {
                elencoFiltri.Add(StatoInterventoEnum.Aperto);
            }

            return elencoFiltri;
        }


        /// <summary>
        /// Restituisce i filtri da applicare alla griglia presente nella pagina
        /// </summary>
        /// <returns></returns>
        private List<FiltroInterventoEnum> GetFiltriInterventoDaApplicareAllaGriglia()
        {
            List<FiltroInterventoEnum> elencoFiltri = new List<FiltroInterventoEnum>();

            if (ComboFiltriIntervento != null &&
                ComboFiltriIntervento.CheckedItems != null &&
                ComboFiltriIntervento.CheckedItems.Count > 0)
            {
                foreach (RadComboBoxItem elementoSelezionato in ComboFiltriIntervento.CheckedItems)
                {
                    int valoreSelezionato = -1;
                    if (Int32.TryParse(elementoSelezionato.Value, out valoreSelezionato))
                    {
                        elencoFiltri.Add((FiltroInterventoEnum)valoreSelezionato);
                    }
                }
            }

            return elencoFiltri;
        }

        /// <summary>
        /// Restituisce i filtri da applicare alla griglia presente nella pagina
        /// </summary>
        /// <returns></returns>
        private List<StatoInterventoEnum> GetFiltriPerStatoDaApplicareAllaGriglia()
        {
            List<StatoInterventoEnum> elencoFiltri = new List<StatoInterventoEnum>();

            if (ComboFiltriPerStatoIntervento != null &&
                ComboFiltriPerStatoIntervento.CheckedItems != null &&
                ComboFiltriPerStatoIntervento.CheckedItems.Count > 0)
            {
                foreach (RadComboBoxItem elementoSelezionato in ComboFiltriPerStatoIntervento.CheckedItems)
                {
                    int valoreSelezionato = -1;
                    if (Int32.TryParse(elementoSelezionato.Value, out valoreSelezionato))
                    {
                        elencoFiltri.Add((StatoInterventoEnum)valoreSelezionato);
                    }
                }
            }

            return elencoFiltri;
        }


        /// <summary>
        /// Restituisce l'elenco dei record presenti nel datasouce passato come parametro e ne effettua il filtro in base ai filtri selezionati
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="filtriDaApplicare"></param>
        /// <param name="filtriPerStatoDaApplicare"></param>
        /// <returns></returns>
        private IQueryable<Entities.Intervento> ApplicaFiltri_Stato(IQueryable<Entities.Intervento> dataSource, List<FiltroInterventoEnum> filtriDaApplicare, List<StatoInterventoEnum> filtriPerStatoDaApplicare, Logic.Interventi logicInterventi)
        {
            IQueryable<Entities.Intervento> elencoDaRestituire = dataSource;
            if (elencoDaRestituire == null)
            {
                return (new List<Entities.Intervento>()).AsQueryable();
            }

            // Filtra per Stato
            if (filtriPerStatoDaApplicare != null && filtriPerStatoDaApplicare.Count > 0)
            {
                elencoDaRestituire = elencoDaRestituire.Where(x => x.Stato.HasValue && filtriPerStatoDaApplicare.Contains((StatoInterventoEnum)x.Stato.Value));
            }


            // Filtra per filtri preimpostati
            foreach(FiltroInterventoEnum filtro in filtriDaApplicare)
            {
                //// Tutti i filtri attualmente esistenti prevedono la restituzione dei soli interventi CHIUSI
                //// quindi, se non è già indicato nel filtri per stato passati, applica sempre il filtro sullo stato "chiuso"
                //if(!filtriPerStatoDaApplicare.Contains(StatoInterventoEnum.Chiuso))
                //{
                //    elencoDaRestituire = elencoDaRestituire.Where(x => x.Stato.HasValue && x.Stato.Value == StatoInterventoEnum.Chiuso.GetHashCode());
                //}

                switch (filtro)
                {
                    case FiltroInterventoEnum.InterventiConPiùDiUnArticolo:
                        elencoDaRestituire = elencoDaRestituire.Where(x => x.Intervento_Articolos.Count() > 1);
                        break;

                    case FiltroInterventoEnum.InterventiConOperatoriAreeDiverse:
                        //Il filtro non è applicabile perchè un operatore può appartenere a più aree
                       elencoDaRestituire = elencoDaRestituire.Where(x => logicInterventi.ReadIdsInterventiConAreeMultiple().Contains(x.ID));
                        break;

                    case FiltroInterventoEnum.InterventiConModalitàRisoluzioneDiverse:
                        elencoDaRestituire = elencoDaRestituire.Where(x => x.Intervento_Operatores.Select(o => o.IDModalitaRisoluzione).Distinct().Count() > 1);
                        break;
                }
            }

            return elencoDaRestituire.OrderByDescending(x => x.Numero);            
        }

        #endregion


        #region Gestione delle autorizzazioni


        //private Entities.AutorizzazioniAccount Autorizzazioni_GestioneAttivitaFormative;

        /// <summary>
        /// Effettua le operazioni di autenticazione e di registrazione delle autorizzazioni di accesso alle funzionalità della pagina
        /// </summary>
        /// <returns></returns>
        private void GestisciAutorizzazioni()
        {
            InformazioniAccountAutenticato infoAccount = InformazioniAccountAutenticato.GetIstance();
            Entities.Account utenteCollegato = infoAccount.Account;

            if (!utenteCollegato.Amministratore.HasValue || utenteCollegato.Amministratore.Value == false)
            {
                IsReadOnly = true;
                IsUserAdmin = false;
            }
            else
            {
                IsUserAdmin = true;
            }

            if(utenteCollegato.TipologiaEnum != TipologiaAccountEnum.SeCoGes)
            {
                Response.Redirect("/Interventi/Tickets.aspx");
            }

            //try
            //{
            //    if (utenteCollegato == null)
            //    {
            //        MessageHelper.RedirectToErrorPageWithMessage(ErrorMessage.UTENTE_SCONOSCIUTO_MESSAGE);
            //        return;
            //    }

            //    Autorizzazioni_GestioneAttivitaFormative = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneAttivitaFormative);

            //    // Applicazione regole di accesso di VISIBILITA' 
            //    if (Autorizzazioni_GestioneAttivitaFormative == null || !Autorizzazioni_GestioneAttivitaFormative.Consenti_Visibilità)
            //    {
            //        MessageHelper.RedirectToErrorPageWithMessage(ErrorMessage.ACCESSO_NEGATO_MESSAGE);
            //        return;
            //    }

            //    // Applicazione regole di accesso di INSERIMENTO
            //    if (!Autorizzazioni_GestioneAttivitaFormative.Consenti_Inserimento)
            //    {
            //        rgGriglia.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
            //    }

            //    // Applicazione regole di accesso di ELIMINAZIONE
            //    if (!Autorizzazioni_GestioneAttivitaFormative.Consenti_Eliminazione)
            //    {
            //        ColonnaElimina.Visible = false;
            //    }

            //    // Applicazione regole accesso in SOLA LETTURA
            //    if (utenteCollegato.SolaLettura.HasValue && utenteCollegato.SolaLettura.Value)
            //    {
            //        IsReadOnly = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    SeCoGes.Logging.LogManager.AddLogErrori(ex);
            //    MessageHelper.ShowErrorMessage(this, ex);
            //}
        }

        #endregion

        protected void btnTestG7Api_Click(object sender, EventArgs e)
        {
            Logic.Interventi ll = new Logic.Interventi();
            string message = ll.TestG7API();
            MessageHelper.ShowMessage(this, "Esecuzione TEST API", message);
        }
    }
}