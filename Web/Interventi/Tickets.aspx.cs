using SeCoGEST.Entities;
using SeCoGEST.Helper;
using SeCoGEST.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SeCoGEST.Web.Interventi
{
    public partial class Tickets : System.Web.UI.Page
    {
        #region Properties

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
                MasterPageHelper.SetMenuVisibile(this, false);

                //Traduce le voci del menu di filtro della griglia. Questa operazione deve essere fatta ad ogni post
                TelerikHelper.TraduciMenuFiltro(rgGriglia.FilterMenu);

                if (!Helper.Web.IsPostOrCallBack(this))
                {
                    RadPersistenceHelper.LoadState(this);

                    TelerikRadGridHelper.ManageExelExportSettings(rgGriglia, "Elenco_Ticket", true);
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
            //        Response.Redirect(string.Format("/Interventi/Ticket.aspx?ID={0}", idValue));
            //        break;

            //    default:
            //        break;

            //}
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
                if (utenteLoggato == null || utenteLoggato.Account == null) throw new Exception("Impossibile recuperare i dati, non risulta alcun utente collegato");

                Logic.Interventi llInterventi = new Logic.Interventi();
                IQueryable<Entities.Intervento> interventi = llInterventi.ReadForAccountCliente(utenteLoggato.Account, Logic.Interventi.TipologiaLetturaInterventiCliente.SoloVisibili);
                rgGriglia.DataSource = interventi;
            }
            catch (Exception ex)
            {
                rgGriglia.DataSource = new List<Entities.Intervento>();
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


                    if (intervento.Urgente.HasValue && intervento.Urgente.HasValue &&
                        intervento.StatoEnum.HasValue &&
                        intervento.StatoEnum.Value != Entities.StatoInterventoEnum.Chiuso &&
                        intervento.StatoEnum.Value != Entities.StatoInterventoEnum.Validato)
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
                        
                        //if (intervento.StatoEnum.Value == Entities.StatoInterventoEnum.Chiuso)
                        //{
                        //    e.Item.BackColor = System.Drawing.Color.LightGreen; //System.Drawing.Color.LightCoral;
                        //    e.Item.ToolTip = String.Format("Intervento {0}", intervento.StatoStringForCustomers);
                        //}
                        //else if (intervento.StatoEnum.Value == Entities.StatoInterventoEnum.Validato)
                        //{
                        //    e.Item.BackColor = System.Drawing.Color.LightGreen;
                        //    e.Item.ToolTip = String.Format("Intervento {0}", intervento.StatoStringForCustomers);
                        //}
                        //else if (intervento.StatoEnum.Value == Entities.StatoInterventoEnum.Eseguito)
                        //{
                        //    e.Item.BackColor = System.Drawing.Color.Yellow;
                        //    e.Item.ToolTip = String.Format("Intervento {0}", intervento.StatoStringForCustomers);
                        //}
                    }
                }
            }
        }

        protected string GetAccountRiferimento(object obj)
        {
            if (obj != null)
            {
                if (obj is Entities.Intervento)
                {
                    if (((Entities.Intervento)obj).AccountRiferimento != null)
                    {
                        if (((Entities.Intervento)obj).AccountRiferimento.TipologiaEnum == Entities.TipologiaAccountEnum.SeCoGes)
                        {
                            return ConfigurationKeys.DENOMINAZIONE_AZIENDA;
                        }
                        else
                        {
                            return ((Entities.Intervento)obj).AccountRiferimento.Nominativo;

                        }
                    }
                }
            }
            return string.Empty;
        }

        #endregion

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
            if (infoAccount != null)
            {
                Entities.Account utenteCollegato = infoAccount.Account;

                if (utenteCollegato != null)
                {
                    if(utenteCollegato.TipologiaEnum == Entities.TipologiaAccountEnum.ClienteStandard)
                    {
                        GridColumn colonnaCliente = rgGriglia.Columns.FindByUniqueName("ColonnaCliente");
                        colonnaCliente.Visible = false;
                    }
                }
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
    }
}