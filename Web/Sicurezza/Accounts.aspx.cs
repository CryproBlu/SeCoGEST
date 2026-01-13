using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SeCoGEST.Infrastructure;
using SeCoGEST.Helper;
using Telerik.Web.UI;
using SeCoGes.Utilities;
using System.Text;

namespace SeCoGEST.Web.Sicurezza
{
    public partial class Accounts : System.Web.UI.Page
    {
        #region Properties

        /// <summary>
        /// Restituisce true se lo stato della griglia dev'essere memorizzato
        /// </summary>
        private bool SaveState { get; set; }

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

                    TelerikRadGridHelper.ManageExelExportSettings(rgGriglia, "Elenco_Accounts", true);
                }
                else
                {
                    // Registrazione degli script necessari ai filtri
                    TelerikRadGridHelper.InjectScriptToHiddenFilterItemIfEmpty(this.Page, rgGriglia);
                }

                // Esegue l'autenticazione e la registrazione delle autorizzazioni di accesso alle funzionalità dell pagina
                //GestisciAutorizzazioni();
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
            //        Response.Redirect(string.Format("/Sicurezza/Account.aspx?ID={0}", idValue));
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
            //if (Autorizzazioni_GestioneAccounts == null || !Autorizzazioni_GestioneAccounts.Consenti_Eliminazione) return;

            Logic.Sicurezza.Accounts ll = new Logic.Sicurezza.Accounts();

            try
            {
                Guid idSelectedRow = (Guid)(((GridDataItem)(e.Item)).GetDataKeyValue("ID"));
                if (!idSelectedRow.IsNullOrEmpty())
                {
                    Entities.Account entityToDelete = ll.Find(idSelectedRow);
                    if (entityToDelete != null)
                    {
                        Entities.Account utenteCollegato = Logic.Sicurezza.CurrentSession.GetLoggedAccount();
                        if (utenteCollegato == null)
                        {
                            throw new Exception(ErrorMessage.UTENTE_SCONOSCIUTO_MESSAGE);
                        }
                        else if (utenteCollegato.ID.Equals(idSelectedRow))
                        {
                            throw new Exception("Non è possibile eliminare l'account con il quale si ha avuto accesso!");
                        }

                        ll.Delete(entityToDelete, true);
                        SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - Rimosso l'entity Account con lo UserName '{1}'.", Request.Url.AbsolutePath, entityToDelete.UserName));

                        // Rimuove le informazioni delle eventuali sessioni aperte dall'utente obbligandolo al log-out
                        //InformazioniAccountAutenticato.RimuoviInformazioniAccount(entityToDelete.UserName);
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
            //if (Autorizzazioni_GestioneAccounts == null || !Autorizzazioni_GestioneAccounts.Consenti_Visibilità) return;

            // Legge e mostra gli Accounts accessibili dall'utente autenticato
            try
            {
                IQueryable<Entities.Account> accounts;
                Logic.Sicurezza.Accounts llAccounts = new Logic.Sicurezza.Accounts();
                accounts = llAccounts.Read();
                rgGriglia.DataSource = accounts;
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

            if (e != null && 
                e.Item != null && 
                e.Item is GridDataItem &&
                e.Item.DataItem is Entities.Account)
            {
                Entities.Account account = (Entities.Account)e.Item.DataItem;

                InformazioniAccountAutenticato accountCollegato = InformazioniAccountAutenticato.GetIstance();
                
                if (accountCollegato != null &&
                    accountCollegato.Account != null &&
                    accountCollegato.Account.UserName == account.UserName)
                {
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    TableCell cellaTastoElimina = dataItem["ColonnaElimina"];

                    if (cellaTastoElimina != null && 
                        cellaTastoElimina.Controls != null &&
                        cellaTastoElimina.Controls.Count > 0)
                    {
                        Control controlTastoElimina = cellaTastoElimina.Controls[0];
                        if (controlTastoElimina is ImageButton)
                        {
                            ImageButton tastoELimina = (ImageButton)controlTastoElimina;
                            tastoELimina.Visible = false;
                            cellaTastoElimina.ToolTip = "Non è possibile eliminare il proprio account";
                        }
                    }
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
                rgGriglia.Columns[rgGriglia.Columns.Count - 1].Visible = !IsReadOnly;
        }

        #endregion



        //#region Gestione delle autorizzazioni


        //private Entities.AutorizzazioniAccount Autorizzazioni_GestioneAccounts;

        ///// <summary>
        ///// Effettua le operazioni di autenticazione e di registrazione delle autorizzazioni di accesso alle funzionalità della pagina
        ///// </summary>
        ///// <returns></returns>
        //private void GestisciAutorizzazioni()
        //{
        //    InformazioniAccountAutenticato infoAccount = InformazioniAccountAutenticato.GetIstance();
        //    Entities.Account utenteCollegato = infoAccount.Account;

        //    try
        //    {
        //        if (utenteCollegato == null)
        //        {
        //            MessageHelper.RedirectToErrorPageWithMessage(ErrorMessage.UTENTE_SCONOSCIUTO_MESSAGE);
        //            return;
        //        }

        //        Autorizzazioni_GestioneAccounts = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneAccounts);

        //        // Applicazione regole di accesso di VISIBILITA' 
        //        if (Autorizzazioni_GestioneAccounts == null || !Autorizzazioni_GestioneAccounts.Consenti_Visibilità)
        //        {
        //            MessageHelper.RedirectToErrorPageWithMessage(ErrorMessage.ACCESSO_NEGATO_MESSAGE);
        //            return;
        //        }

        //        if (utenteCollegato.SolaLettura.HasValue && utenteCollegato.SolaLettura.Value)
        //        {
        //            IsReadOnly = true;
        //        }

        //        rgGriglia.Visible = Autorizzazioni_GestioneAccounts.Consenti_Visibilità;
        //        ColonnaModifica.Visible = Autorizzazioni_GestioneAccounts.Consenti_Visibilità || Autorizzazioni_GestioneAccounts.Consenti_Modifica;
        //        ColonnaElimina.Visible = Autorizzazioni_GestioneAccounts.Consenti_Eliminazione;
        //    }
        //    catch (Exception ex)
        //    {
        //        SeCoGes.Logging.LogManager.AddLogErrori(ex);
        //        MessageHelper.ShowErrorMessage(this, ex);
        //    }
        //}

        //#endregion

    }
}