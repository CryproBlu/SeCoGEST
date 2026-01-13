using SeCoGEST.Entities;
using SeCoGEST.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SeCoGes.Utilities;

namespace SeCoGEST.Web.Preventivi
{
    public partial class ConfigurazioniArticoliAggiuntivi : System.Web.UI.Page
    {
        #region Variables

        private bool isReadOnly;

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

        #endregion

        #region Colonne Griglia

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
                    ApplicaRedirectPulsanteAggiorna();
                }
                else
                {
                    // Registrazione degli script necessari ai filtri
                    TelerikRadGridHelper.InjectScriptToHiddenFilterItemIfEmpty(this.Page, rgGriglia);
                }

                // Applica le eventuali restrizioni alle funzionalità della pagina in base all'utente autenticato
                GestisciAutorizzazioni();
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
                //    Response.Redirect(string.Format("/AnalisiCosti/AnalisiCosto.aspx?ID={0}", idValue));
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

            Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi llAnalisiCosti = new Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi();

            try
            {
                Guid idSelectedRow = (Guid)(((GridDataItem)(e.Item)).GetDataKeyValue("ID"));
                if (!idSelectedRow.IsNullOrEmpty())
                {
                    Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo entityToDelete = llAnalisiCosti.Find(new EntityId<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo>(idSelectedRow));
                    if (entityToDelete != null)
                    {
                        llAnalisiCosti.Delete(entityToDelete, false);
                        llAnalisiCosti.SubmitToDatabase();

                        SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - Rimosso l'entity AnalisiVenditaConfigurazioneArticoloAggiuntivo con ID '{1}'.", Request.Url.AbsolutePath, entityToDelete.ID));
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
                Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi llAnalisiCosti = new Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi();
                IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo> elencoAnalisiCosti = llAnalisiCosti.Read();
                rgGriglia.DataSource = elencoAnalisiCosti;
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