using SeCoGEST.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SeCoGes.Utilities;

namespace SeCoGEST.Web.Archivi
{
    public partial class Operatori : System.Web.UI.Page
    {
        #region Properties

        private bool isReadOnly;

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
        /// Restituisce true se lo stato della griglia dev'essere memorizzato
        /// </summary>
        private bool SaveState { get; set; }

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
                //Traduce le voci del menu di filtro della griglia. Questa operazione deve essere fatta ad ogni post
                TelerikHelper.TraduciMenuFiltro(rgGriglia.FilterMenu);

                if (!Helper.Web.IsPostOrCallBack(this))
                {
                    RadPersistenceHelper.LoadState(this);

                    TelerikRadGridHelper.ManageExelExportSettings(rgGriglia, "Elenco_Ruoli", true);
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

            //if (e == null) return;

            //switch (e.CommandName)
            //{
            //    case "ModificaCommand":
            //        Guid idValue = TelerikRadGridHelper.GetDataKeyValueFromGridCommandEventArgsItem<Guid>(rgGriglia, e, "ID");
            //        Response.Redirect(string.Format("/Archivi/Operatore.aspx?ID={0}", idValue));
            //        break;

            //    default:
            //        break;

            //}
        }

        /// <summary>
        /// Metodo di gestione dell'evento DeleteCommand della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGriglia_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {

            Logic.Operatori llOperatori = new Logic.Operatori();

            try
            {
                Guid idSelectedRow = (Guid)(((GridDataItem)(e.Item)).GetDataKeyValue("ID"));
                if (!idSelectedRow.IsNullOrEmpty())
                {
                    Entities.Operatore entityToDelete = llOperatori.Find(new Entities.EntityId<Entities.Operatore>(idSelectedRow));
                    if (entityToDelete != null)
                    {
                        //Entities.ApplicazioneAccount utenteCollegato = InformazioniSessione.GetIstance().AccountCollegato;
                        //if (utenteCollegato == null)
                        //{
                        //    throw new Exception(ErrorMessage.UTENTE_SCONOSCIUTO_MESSAGE);
                        //}

                        llOperatori.Delete(entityToDelete, false);
                        llOperatori.SubmitToDatabase();

                        SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - Rimosso l'entity Operatore con ID '{1}'.", Request.Url.AbsolutePath, entityToDelete.ID));
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
                Logic.Operatori llInterventi = new Logic.Operatori();
                IQueryable<Entities.Operatore> elencoOperatori = llInterventi.Read();
                rgGriglia.DataSource = elencoOperatori;
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

            if (e != null && e.Item != null && e.Item.DataItem is Entities.Operatore)
            {
                Entities.Operatore operatore = (Entities.Operatore)e.Item.DataItem;
                if (operatore.Area)
                {
                    e.Item.BackColor = System.Drawing.Color.LightGray;
                    e.Item.ToolTip = "Ruolo utilizzato come Area";
                }
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
            if (rgGriglia != null && rgGriglia.Columns.Count > 0)
            {
                ColonnaElimina.Visible = !IsReadOnly;
            }
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
            //InformazioniAccountAutenticato infoAccount = InformazioniAccountAutenticato.GetIstance();
            //Entities.ApplicazioneAccount utenteCollegato = infoAccount.Account;

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
    }
}