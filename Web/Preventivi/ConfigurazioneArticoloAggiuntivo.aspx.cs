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

namespace SeCoGEST.Web.Preventivi
{
    public partial class ConfigurazioneArticoloAggiuntivo : System.Web.UI.Page
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
        protected EntityId<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo> currentID
        {
            get
            {
                if (Request.QueryString["ID"] != null)
                    return new EntityId<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo>(Request.QueryString["ID"]);
                else
                    return EntityId<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo>.Empty;
            }
        }

        #region Pulsanti Toolbar

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

        #endregion

        #region Intercettazione Eventi

        protected void Page_Load(object sender, EventArgs e)
        {
            CaricaAutorizzazioni();

            reTemplateDescrizionePreventivo.Modules.Clear();
            reTemplateContratto.Modules.Clear();
            reClausoleVessatorieAggiuntive.Modules.Clear();

            if (!Helper.Web.IsPostOrCallBack(this))
            {
                try
                {
                    SeCoGes.Logging.LogManager.AddLogAccessi(String.Format("Accesso alla pagina '{0}'.", Request.Url.AbsolutePath));

                    Initialize();

                    Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi ll = new Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi();
                    Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo entityToShow = ll.Find(currentID);

                    if (entityToShow != null)
                    {
                        ShowData(entityToShow);
                    }
                    else
                    {
                        ShowData(new Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo());
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

        /// <summary>
        /// Metodo di gestione del cambiamento della tipologia di configurazione
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rcbTipologia_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (int.TryParse(e.Value, out int valoreParsato))
                {
                    ManageComponentVisibilityByTipologia((TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo)valoreParsato);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        protected void rcbGruppo_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                string testoRicerca = (e.Text == null) ? String.Empty : e.Text.ToLower();
                RadComboBox combo = (RadComboBox)sender;
                combo.Items.Clear();

                decimal? codiceGruppo = null;
                string descrizioneGruppo = null;
                decimal? codiceCategoria = null;
                string descrizioneCategoria = null;
                decimal? codiceCategoriaStatistica = null;
                string descrizioneCategoriaStatistica = null;

                if (e.Context != null)
                {
                    if (e.Context.ContainsKey("CodiceGruppo") && decimal.TryParse(e.Context["CodiceGruppo"].ToString(), out decimal codiceGruppoValue))
                    {
                        codiceGruppo = codiceGruppoValue;
                    }

                    if (e.Context.ContainsKey("DescrizioneGruppo"))
                    {
                        descrizioneGruppo = e.Context["DescrizioneGruppo"].ToString();
                    }

                    if (e.Context.ContainsKey("CodiceCategoria") && decimal.TryParse(e.Context["CodiceCategoria"].ToString(), out decimal codiceCategoriaValue))
                    {
                        codiceCategoria = codiceCategoriaValue;
                    }

                    if (e.Context.ContainsKey("DescrizioneCategoria"))
                    {
                        descrizioneCategoria = e.Context["DescrizioneCategoria"].ToString();
                    }

                    if (e.Context.ContainsKey("CodiceCategoriaStatistica") && decimal.TryParse(e.Context["CodiceCategoriaStatistica"].ToString(), out decimal codiceCategoriaStatisticaValue))
                    {
                        codiceCategoriaStatistica = codiceCategoriaStatisticaValue;
                    }

                    if (e.Context.ContainsKey("DescrizioneCategoriaStatistica"))
                    {
                        descrizioneCategoriaStatistica = e.Context["DescrizioneCategoriaStatistica"].ToString();
                    }
                }

                Logic.Metodo.Archivi llArchivi = new Logic.Metodo.Archivi();
                Logic.MappatureGruppiCategorieCategorieStatistiche llMappatureGruppiCategorieCategorieStatistiche = new Logic.MappatureGruppiCategorieCategorieStatistiche(llArchivi);

                IQueryable<Entities.TABGRUPPI> queryBase = from gruppo in llArchivi.Read_Gruppi()
                                                           join mappatura in llMappatureGruppiCategorieCategorieStatistiche.Read(codiceGruppo, descrizioneGruppo, codiceCategoria, descrizioneCategoria, codiceCategoriaStatistica, descrizioneCategoriaStatistica) on gruppo.CODICE equals mappatura.CodiceGruppo
                                                           select gruppo;

                if (!string.IsNullOrWhiteSpace(testoRicerca))
                {
                    queryBase = queryBase.Where(x => x.CODICE.ToString().ToLower().Contains(testoRicerca) || x.DESCRIZIONE.ToLower().Contains(testoRicerca));
                }

                queryBase = queryBase.Distinct();

                int itemsPerRequest = (combo.ItemsPerRequest <= 0) ? 20 : combo.ItemsPerRequest;
                int itemOffset = e.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                int numTotaleUtenti = queryBase.Count();

                if (endOffset > numTotaleUtenti)
                    endOffset = numTotaleUtenti;

                //List<Tuple<string, string, decimal?, decimal?, decimal?>> entities = queryBase.Skip(itemOffset).Take(itemsPerRequest).Select(x => new Tuple<string, string, decimal?, decimal?, decimal?>(x.CODICE, x.DESCRIZIONE, x.GRUPPO, x.CATEGORIA, x.CODCATEGORIASTAT)).ToList();
                List<TABGRUPPI> entities = queryBase.Skip(itemOffset).Take(itemsPerRequest).ToList();

                foreach (TABGRUPPI entity in entities)
                {
                    RadComboBoxItem item = new RadComboBoxItem(entity.DESCRIZIONE, entity.CODICE.ToString());
                    item.DataItem = entity;
                    combo.Items.Add(item);
                }

                combo.DataBind();

                if (numTotaleUtenti > 0)
                {
                    e.Message = String.Format("da <b>1</b> a <b>{0}</b> di <b>{1}</b>", endOffset.ToString(), numTotaleUtenti.ToString());
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


        protected void rcbCategoria_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                string testoRicerca = (e.Text == null) ? String.Empty : e.Text.ToLower();
                RadComboBox combo = (RadComboBox)sender;
                combo.Items.Clear();

                decimal? codiceGruppo = null;
                string descrizioneGruppo = null;
                decimal? codiceCategoria = null;
                string descrizioneCategoria = null;
                decimal? codiceCategoriaStatistica = null;
                string descrizioneCategoriaStatistica = null;

                if (e.Context != null)
                {
                    if (e.Context.ContainsKey("CodiceGruppo") && decimal.TryParse(e.Context["CodiceGruppo"].ToString(), out decimal codiceGruppoValue))
                    {
                        codiceGruppo = codiceGruppoValue;
                    }

                    if (e.Context.ContainsKey("DescrizioneGruppo"))
                    {
                        descrizioneGruppo = e.Context["DescrizioneGruppo"].ToString();
                    }

                    if (e.Context.ContainsKey("CodiceCategoria") && decimal.TryParse(e.Context["CodiceCategoria"].ToString(), out decimal codiceCategoriaValue))
                    {
                        codiceCategoria = codiceCategoriaValue;
                    }

                    if (e.Context.ContainsKey("DescrizioneCategoria"))
                    {
                        descrizioneCategoria = e.Context["DescrizioneCategoria"].ToString();
                    }

                    if (e.Context.ContainsKey("CodiceCategoriaStatistica") && decimal.TryParse(e.Context["CodiceCategoriaStatistica"].ToString(), out decimal codiceCategoriaStatisticaValue))
                    {
                        codiceCategoriaStatistica = codiceCategoriaStatisticaValue;
                    }

                    if (e.Context.ContainsKey("DescrizioneCategoriaStatistica"))
                    {
                        descrizioneCategoriaStatistica = e.Context["DescrizioneCategoriaStatistica"].ToString();
                    }
                }

                Logic.Metodo.Archivi llArchivi = new Logic.Metodo.Archivi();
                Logic.MappatureGruppiCategorieCategorieStatistiche llMappatureGruppiCategorieCategorieStatistiche = new Logic.MappatureGruppiCategorieCategorieStatistiche(llArchivi);

                IQueryable<Entities.TABCATEGORIE> queryBase = from cat in llArchivi.Read_Categorie()
                                                              join mappatura in llMappatureGruppiCategorieCategorieStatistiche.Read(codiceGruppo, descrizioneGruppo, codiceCategoria, descrizioneCategoria, codiceCategoriaStatistica, descrizioneCategoriaStatistica) on cat.CODICE equals mappatura.CodiceCategoria
                                                              select cat;

                if (!string.IsNullOrWhiteSpace(testoRicerca))
                {
                    queryBase = queryBase.Where(x => x.CODICE.ToString().ToLower().Contains(testoRicerca) || x.DESCRIZIONE.ToLower().Contains(testoRicerca));
                }

                queryBase = queryBase.Distinct();

                int itemsPerRequest = (combo.ItemsPerRequest <= 0) ? 20 : combo.ItemsPerRequest;
                int itemOffset = e.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                int numTotaleUtenti = queryBase.Count();

                if (endOffset > numTotaleUtenti)
                    endOffset = numTotaleUtenti;

                //List<Tuple<string, string, decimal?, decimal?, decimal?>> entities = queryBase.Skip(itemOffset).Take(itemsPerRequest).Select(x => new Tuple<string, string, decimal?, decimal?, decimal?>(x.CODICE, x.DESCRIZIONE, x.GRUPPO, x.CATEGORIA, x.CODCATEGORIASTAT)).ToList();
                List<TABCATEGORIE> entities = queryBase.Skip(itemOffset).Take(itemsPerRequest).ToList();

                foreach (TABCATEGORIE entity in entities)
                {
                    RadComboBoxItem item = new RadComboBoxItem(entity.DESCRIZIONE, entity.CODICE.ToString());
                    item.DataItem = entity;
                    combo.Items.Add(item);
                }

                combo.DataBind();

                if (numTotaleUtenti > 0)
                {
                    e.Message = String.Format("da <b>1</b> a <b>{0}</b> di <b>{1}</b>", endOffset.ToString(), numTotaleUtenti.ToString());
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

        protected void rcbCategoriaStatistica_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                string testoRicerca = (e.Text == null) ? String.Empty : e.Text.ToLower();
                RadComboBox combo = (RadComboBox)sender;
                combo.Items.Clear();

                decimal? codiceGruppo = null;
                string descrizioneGruppo = null;
                decimal? codiceCategoria = null;
                string descrizioneCategoria = null;
                decimal? codiceCategoriaStatistica = null;
                string descrizioneCategoriaStatistica = null;

                if (e.Context != null)
                {
                    if (e.Context.ContainsKey("CodiceGruppo") && decimal.TryParse(e.Context["CodiceGruppo"].ToString(), out decimal codiceGruppoValue))
                    {
                        codiceGruppo = codiceGruppoValue;
                    }

                    if (e.Context.ContainsKey("DescrizioneGruppo"))
                    {
                        descrizioneGruppo = e.Context["DescrizioneGruppo"].ToString();
                    }

                    if (e.Context.ContainsKey("CodiceCategoria") && decimal.TryParse(e.Context["CodiceCategoria"].ToString(), out decimal codiceCategoriaValue))
                    {
                        codiceCategoria = codiceCategoriaValue;
                    }

                    if (e.Context.ContainsKey("DescrizioneCategoria"))
                    {
                        descrizioneCategoria = e.Context["DescrizioneCategoria"].ToString();
                    }

                    if (e.Context.ContainsKey("CodiceCategoriaStatistica") && decimal.TryParse(e.Context["CodiceCategoriaStatistica"].ToString(), out decimal codiceCategoriaStatisticaValue))
                    {
                        codiceCategoriaStatistica = codiceCategoriaStatisticaValue;
                    }

                    if (e.Context.ContainsKey("DescrizioneCategoriaStatistica"))
                    {
                        descrizioneCategoriaStatistica = e.Context["DescrizioneCategoriaStatistica"].ToString();
                    }
                }

                Logic.Metodo.Archivi llArchivi = new Logic.Metodo.Archivi();
                Logic.MappatureGruppiCategorieCategorieStatistiche llMappatureGruppiCategorieCategorieStatistiche = new Logic.MappatureGruppiCategorieCategorieStatistiche(llArchivi);

                IQueryable<Entities.TABCATEGORIESTAT> queryBase = from catStat in llArchivi.Read_CategorieStatistiche()
                                                                  join mappatura in llMappatureGruppiCategorieCategorieStatistiche.Read(codiceGruppo, descrizioneGruppo, codiceCategoria, descrizioneCategoria, codiceCategoriaStatistica, descrizioneCategoriaStatistica) on catStat.CODICE equals mappatura.CodiceCategoriaStatistica
                                                                  select catStat;

                if (!string.IsNullOrWhiteSpace(testoRicerca))
                {
                    queryBase = queryBase.Where(x => x.CODICE.ToString().ToLower().Contains(testoRicerca) || x.DESCRIZIONE.ToLower().Contains(testoRicerca));
                }

                queryBase = queryBase.Distinct();

                int itemsPerRequest = (combo.ItemsPerRequest <= 0) ? 20 : combo.ItemsPerRequest;
                int itemOffset = e.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                int numTotaleUtenti = queryBase.Count();

                if (endOffset > numTotaleUtenti)
                    endOffset = numTotaleUtenti;

                //List<Tuple<string, string, decimal?, decimal?, decimal?>> entities = queryBase.Skip(itemOffset).Take(itemsPerRequest).Select(x => new Tuple<string, string, decimal?, decimal?, decimal?>(x.CODICE, x.DESCRIZIONE, x.GRUPPO, x.CATEGORIA, x.CODCATEGORIASTAT)).ToList();
                List<TABCATEGORIESTAT> entities = queryBase.Skip(itemOffset).Take(itemsPerRequest).ToList();

                foreach (TABCATEGORIESTAT entity in entities)
                {
                    RadComboBoxItem item = new RadComboBoxItem(entity.DESCRIZIONE, entity.CODICE.ToString());
                    item.DataItem = entity;
                    combo.Items.Add(item);
                }

                combo.DataBind();

                if (numTotaleUtenti > 0)
                {
                    e.Message = String.Format("da <b>1</b> a <b>{0}</b> di <b>{1}</b>", endOffset.ToString(), numTotaleUtenti.ToString());
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

        /// <summary>
        /// Metodo di gestione dell'evento di richiesta degli articoli 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rcbCodiceArticolo_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                string testoRicerca = (e.Text == null) ? String.Empty : e.Text.ToLower();
                RadComboBox combo = (RadComboBox)sender;
                combo.Items.Clear();                

                decimal? codiceGruppo = null;
                decimal? codiceCategoria = null;
                decimal? codiceCategoriaStatistica = null;                

                if (e.Context != null)
                {
                    if (e.Context.ContainsKey("CodiceGruppo") && decimal.TryParse(e.Context["CodiceGruppo"].ToString(), out decimal codiceGruppoValue))
                    {
                        codiceGruppo = codiceGruppoValue;
                    }

                    if (e.Context.ContainsKey("CodiceCategoria") && decimal.TryParse(e.Context["CodiceCategoria"].ToString(), out decimal codiceCategoriaValue))
                    {
                        codiceCategoria = codiceCategoriaValue;
                    }

                    if (e.Context.ContainsKey("CodiceCategoriaStatistica") && decimal.TryParse(e.Context["CodiceCategoriaStatistica"].ToString(), out decimal codiceCategoriaStatisticaValue))
                    {
                        codiceCategoriaStatistica = codiceCategoriaStatisticaValue;
                    }
                }

                Logic.Metodo.AnagraficheArticoli llAngraficaArticoli = new Logic.Metodo.AnagraficheArticoli();
                Logic.Metodo.ElenchiCompletiArticoli llArts = new Logic.Metodo.ElenchiCompletiArticoli(llAngraficaArticoli);

                IQueryable<Entities.ANAGRAFICAARTICOLI> queryBase = null;

                if (codiceGruppo.HasValue || codiceCategoria.HasValue || codiceCategoriaStatistica.HasValue)
                {
                    queryBase = from art in llArts.Read(DateTime.Today)
                                join anagraficaArt in llAngraficaArticoli.Read(codiceGruppo, codiceCategoria, codiceCategoriaStatistica) on art.CodiceArticolo equals anagraficaArt.CODICE
                                select anagraficaArt;
                }
                else
                {
                    queryBase = from art in llArts.Read(DateTime.Today)
                                join anagraficaArt in llAngraficaArticoli.Read(codiceGruppo, codiceCategoria, codiceCategoriaStatistica) on art.CodiceArticolo equals anagraficaArt.CODICE
                                select anagraficaArt;
                }                                                                        

                if (!string.IsNullOrWhiteSpace(testoRicerca))
                {
                    queryBase = queryBase.Where(x => x.CODICE.ToLower().Contains(testoRicerca) || x.DESCRIZIONE.ToLower().Contains(testoRicerca));
                }

                queryBase = queryBase.Distinct();

                int itemsPerRequest = (combo.ItemsPerRequest <= 0) ? 20 : combo.ItemsPerRequest;
                int itemOffset = e.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                int numTotaleUtenti = queryBase.Count();

                if (endOffset > numTotaleUtenti)
                    endOffset = numTotaleUtenti;

                //List<Tuple<string, string, decimal?, decimal?, decimal?>> entities = queryBase.Skip(itemOffset).Take(itemsPerRequest).Select(x => new Tuple<string, string, decimal?, decimal?, decimal?>(x.CODICE, x.DESCRIZIONE, x.GRUPPO, x.CATEGORIA, x.CODCATEGORIASTAT)).ToList();
                List<ANAGRAFICAARTICOLI> entities = queryBase.Skip(itemOffset).Take(itemsPerRequest).ToList();

                foreach (ANAGRAFICAARTICOLI entity in entities)
                {
                    RadComboBoxItem item = new RadComboBoxItem(entity.DESCRIZIONE, entity.CODICE);
                    item.Attributes.Add("DescrizioneArticolo", entity.DESCRIZIONE);
                    item.Attributes.Add("CodiceGruppo", (entity.GRUPPO.HasValue ? entity.GRUPPO.Value.ToString() : String.Empty));
                    item.Attributes.Add("DescrizioneGruppo", (entity.TABGRUPPI?.DESCRIZIONE ?? String.Empty));
                    item.Attributes.Add("CodiceCategoria", (entity.CATEGORIA.HasValue ? entity.CATEGORIA.Value.ToString() : String.Empty));
                    item.Attributes.Add("DescrizioneCategoria", (entity.TABCATEGORIE?.DESCRIZIONE ?? String.Empty));
                    item.Attributes.Add("CodiceCategoriaStatistica", (entity.CODCATEGORIASTAT.HasValue ? entity.CODCATEGORIASTAT.Value.ToString() : String.Empty));
                    item.Attributes.Add("DescrizioneCategoriaStatistica", (entity.TABCATEGORIESTAT?.DESCRIZIONE ?? String.Empty));
                    item.DataItem = entity;
                    combo.Items.Add(item);
                }

                combo.DataBind();

                if (numTotaleUtenti > 0)
                {
                    e.Message = String.Format("da <b>1</b> a <b>{0}</b> di <b>{1}</b>", endOffset.ToString(), numTotaleUtenti.ToString());
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

        #region rgGrigliaClausoleVessatorie

        /// <summary>
        /// Metodo di gestione dell'evento ItemCommand della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGrigliaClausoleVessatorie_PreRender(object sender, EventArgs e)
        {
            TelerikRadGridHelper.ApplicaTraduzioneDaFileDiResource(rgGrigliaClausoleVessatorie);
        }
        
        /// <summary>
        /// Metodo di gestione dell'evento NeedDataSource della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGrigliaClausoleVessatorie_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (!rgGrigliaClausoleVessatorie.Visible) return;

                Logic.ClausoleVessatorie llAnalisiCosti = new Logic.ClausoleVessatorie();
                IQueryable<Entities.ClausolaVessatoria> elencoAnalisiCosti = llAnalisiCosti.Read();
                rgGrigliaClausoleVessatorie.DataSource = elencoAnalisiCosti;
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
        protected void rgGrigliaClausoleVessatorie_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            //Traduce le voci del menu di filtro della griglia. Questa operazione deve essere fatta ad ogni post
            TelerikHelper.TraduciElementiGriglia(e);
        }

        private Guid[] ElencoClausoleVessatorieSelezionate = null;

        /// <summary>
        /// Metodo di gestione dell'evento ItemDataBound della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGrigliaClausoleVessatorie_ItemDataBound(object sender, GridItemEventArgs e)
        {
            // Inserisce gli attributi necessari per la trasformazione del layout della griglia per dispositivi mobili
            TelerikRadGridHelper.ManageColumnContentOnMobileLayout(rgGrigliaClausoleVessatorie, e);

            if (e?.Item?.DataItem is Entities.ClausolaVessatoria)
            {
                Entities.ClausolaVessatoria clausolaVessatoria = (Entities.ClausolaVessatoria)e.Item.DataItem;
                RadCheckBox rchkClausolaVessatoriaSelezionata = e.Item.FindControl("rchkClausolaVessatoriaSelezionata") as RadCheckBox;
                if (rchkClausolaVessatoriaSelezionata != null)
                {
                    if (ElencoClausoleVessatorieSelezionate == null)
                    {
                        Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi llAnalisiVenditeConfigurazioneArticoliAggiuntivi = new Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi();
                        AnalisiVenditaConfigurazioneArticoloAggiuntivo analisiVenditaConfigurazioneArticoloAggiuntivo = llAnalisiVenditeConfigurazioneArticoliAggiuntivi.Find(currentID);
                        if (analisiVenditaConfigurazioneArticoloAggiuntivo != null)
                        {
                            Logic.AnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie llAnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie = new Logic.AnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie(llAnalisiVenditeConfigurazioneArticoliAggiuntivi);
                            ElencoClausoleVessatorieSelezionate = llAnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie.Read(analisiVenditaConfigurazioneArticoloAggiuntivo).Select(x => x.IDClausolaVessatoria).ToArray();
                        }                        
                    }

                    if (ElencoClausoleVessatorieSelezionate != null)
                    {
                        rchkClausolaVessatoriaSelezionata.Checked = ElencoClausoleVessatorieSelezionate.Contains(clausolaVessatoria.ID);    
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Metodi di gestione

        /// <summary>
        /// Mostra nell'interfaccia i dati dell'entity passata
        /// </summary>
        /// <param name="entity"></param>
        private void ShowData(Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo entityToShow)
        {
            ApplicaRedirectPulsanteAggiorna();

            if (entityToShow == null)
            {
                throw new ArgumentNullException("Parametro nullo", "entityToShow");
            }

            //if (entityToShow.Chiuso.HasValue && entityToShow.Chiuso.Value == true)
            //{
            //    this.Enabled = false;
            //}

            if (!entityToShow.ID.Equals(Guid.Empty))
            {
                lblTitolo.Text = "Configurazione Articolo Aggiuntivo";
            }
            else
            {
                lblTitolo.Text = "Nuova Configurazione Articolo Aggiuntivo";
            }

            this.Title = lblTitolo.Text;

            rcbTipologia.DataSource = SeCoGes.Utilities.EnumHelper.GetDescriptionFromEnum<Entities.TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo, int>();
            rcbTipologia.DataBind();

            RadComboBoxItem itemTipologia = rcbTipologia.FindItemByValue(entityToShow.Tipologia.ToString());
            if (itemTipologia != null) itemTipologia.Selected = true;

            if(entityToShow.TipologiaEnum == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Contratto)
            {
                reTemplateContratto.Content = entityToShow.TestoDescrizioneContratto?.ToString() ?? String.Empty;
            }
            else if (entityToShow.TipologiaEnum == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo)
            {
                reTemplateDescrizionePreventivo.Content = entityToShow.TestoDescrizionePreventivo?.ToString() ?? String.Empty;
            }
            else if (entityToShow.TipologiaEnum == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo_Contratto)
            {
                reTemplateContratto.Content = entityToShow.TestoDescrizioneContratto?.ToString() ?? String.Empty;
                reTemplateDescrizionePreventivo.Content = entityToShow.TestoDescrizionePreventivo?.ToString() ?? String.Empty;
            }
            else
            {
                rtbTestoAvviso.Text = entityToShow.TestoAvviso?.ToString() ?? String.Empty;
            }

            reClausoleVessatorieAggiuntive.Content = entityToShow.ClausoleVessatorieAggiuntive ?? String.Empty;

            ManageComponentVisibilityByTipologia(entityToShow.TipologiaEnum);

            SetComboBoxSelectedValue(rcbGruppoIngresso, entityToShow.CodiceGruppoIn?.ToString() ?? String.Empty);
            SetComboBoxSelectedValue(rcbCategoriaIngresso, entityToShow.CodiceCategoriaIn?.ToString() ?? String.Empty);
            SetComboBoxSelectedValue(rcbCategoriaStatisticaIngresso, entityToShow.CodiceCategoriaStatisticaIn?.ToString() ?? String.Empty);            

            rcbCodiceArticoloIngresso.SelectedValue = entityToShow.CodiceArticoloIn;
            rcbCodiceArticoloIngresso.Text = entityToShow.ANAGRAFICAARTICOLIIn?.DESCRIZIONE ?? entityToShow.CodiceArticoloIn;

            rcbCodiceArticoloUscita.SelectedValue = entityToShow.CodiceArticoloOut;
            rcbCodiceArticoloUscita.Text = entityToShow.ANAGRAFICAARTICOLIOut?.DESCRIZIONE ?? entityToShow.CodiceArticoloOut;

            rtbUM.Text = entityToShow.UnitaMisura;
            rntbQuantità.Value = entityToShow.Quantita.HasValue ? (double?)entityToShow.Quantita : null;
            rntbCosto.Value = entityToShow.Costo.HasValue ? (double?)entityToShow.Costo : null;
            rntbVendita.Value = entityToShow.Vendita.HasValue ? (double?)entityToShow.Vendita : null;
            rntbTotaleCosto.Value = entityToShow.TotaleCosto.HasValue ? (double?)entityToShow.TotaleCosto : null;
            rntbRicaricoValore.Value = entityToShow.RicaricoValuta.HasValue ? (double?)entityToShow.RicaricoValuta : null;
            rntbRicaricoPercentuale.Value = entityToShow.RicaricoPercentuale.HasValue ? (double?)entityToShow.RicaricoPercentuale : null;
            rntbTotaleVendita.Value = entityToShow.TotaleVendita.HasValue ? (double?)entityToShow.TotaleVendita : null;
        }

        /// <summary>
        /// Effettua la ricerca di un valore all'interno del combo passato come parametro e nel caso in cui la ricerca restituisca un elemento, quest'ultimo viene selezionato
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="value"></param>
        private void SetComboBoxSelectedValue(RadComboBox comboBox, string value)
        {
            if (comboBox != null && !String.IsNullOrWhiteSpace(value))
            {
                RadComboBoxItem item = comboBox.FindItemByValue(value);
                if (item != null) item.Selected = true;
            }
        }

        /// <summary>
        /// Effettua la gestione della visbilità dei componenti grafici in base alla tipologia passata come parametro
        /// </summary>
        /// <param name="tipologia"></param>
        private void ManageComponentVisibilityByTipologia(Entities.TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo tipologia)
        {
            ColonnaTestoAvviso.Visible = false;
            ColonnaArticoloAggiuntivo.Visible = false;
            rfvTestoAvviso.Enabled = false;
            rfvCodiceArticoloUscita.Enabled = false;
            rfvTemplateContrattoHtml.Enabled = false;
            ColonnaTemplateContrattoHtml.Visible = false;
            ColonnaClausoleVessatorie.Visible = false;
            ColonnaClausoleVessatorieAggiuntive.Visible = false;

            rfvTemplateDescrizionePreventivoHtml.Enabled = false;
            ColonnaTemplateHtmlDescrizionePreventivo.Visible = false;

            if (tipologia == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.MostrareMessaggio)
            {
                ColonnaTestoAvviso.Visible = true;
                rfvTestoAvviso.Enabled = true;
            }
            else if (tipologia == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.AggiungereArticolo)
            {
                ColonnaArticoloAggiuntivo.Visible = true;
                rfvCodiceArticoloUscita.Enabled = true;
            }
            else if (tipologia != TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Contratto &&
                tipologia != TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo &&
                tipologia != TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo_Contratto)
            {
                ColonnaTestoAvviso.Visible = true;
                ColonnaArticoloAggiuntivo.Visible = true;
                rfvTestoAvviso.Enabled = true;
                rfvCodiceArticoloUscita.Enabled = true;
            }
            else
            {
                if (tipologia == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo ||
                tipologia == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo_Contratto)
                {
                    rfvTemplateDescrizionePreventivoHtml.Enabled = true;
                    ColonnaTemplateHtmlDescrizionePreventivo.Visible = true;
                }
                                
                if (tipologia == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Contratto ||
                    tipologia == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo_Contratto)
                {
                    ColonnaTemplateContrattoHtml.Visible = true;
                    rfvTemplateContrattoHtml.Enabled = true;
                    ColonnaClausoleVessatorie.Visible = true;
                    rgGrigliaClausoleVessatorie.Rebind();
                    ColonnaClausoleVessatorieAggiuntive.Visible = true;
                }
            }
        }


        //private void AddPanelBar(AnalisiCostoRaggruppamento g)
        //{
        //    RadPanelItem itemi = new RadPanelItem(g.Denominazione);
        //    RaggruppamentiPanel.Items.Add(itemi);
        //}

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
            Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo entityToSave = null;

            // Definisco una variabile che conterrà l'Codice dell'entity salvata. 
            // Se per qualche motivo l'entity non viene salvata allora entityId rimarrà String.Empty
            string entityId = String.Empty;
            Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi ll = new Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi();

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
                            "Il documento di analisi che si sta variando non esiste più in archivio."
                                + "\n\rOperazione annullata!");
                    }

                    // Al contrario, se entityToSave è nulla e CurrentId è vuota
                    // vuol dire che la pagina è stata aperta per la creazione di una nuova entità
                    nuovo = true;
                    entityToSave = new Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo();

                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriDallaView(entityToSave, ll);


                    //Creo nuova entity
                    ll.Create(entityToSave, true);
                }
                else
                {
                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriDallaView(entityToSave, ll);
                }

                Logic.AnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie llAnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie = new Logic.AnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie(ll);
                IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria> elencoClausoleVessatorieSelezionate = llAnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie.Read(entityToSave);

                if (elencoClausoleVessatorieSelezionate.Any())
                {
                    llAnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie.Delete(elencoClausoleVessatorieSelezionate, true);
                }

                if (entityToSave.TipologiaEnum == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Contratto ||
                    entityToSave.TipologiaEnum == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo_Contratto)
                {
                    Guid[] elencoIdentificativiClausoleVessatorieSelezionate = GetElencoClausoleVessatorieSelezionate();
                    if ((elencoIdentificativiClausoleVessatorieSelezionate?.Length ?? 0) > 0)
                    {
                        foreach(Guid identificativoClausolaVessatoriaSelezionata in elencoIdentificativiClausoleVessatorieSelezionate)
                        {
                            Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria entity = new AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria();
                            entity.IDClausolaVessatoria = identificativoClausolaVessatoriaSelezionata;
                            entity.IDAnalisiVenditaConfigurazioneArticoloAggiuntivo = entityToSave.ID;

                            llAnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie.Create(entity, true);
                        }
                    }
                }

                // Persisto le modifiche sulla base dati nella transazione
                ll.SubmitToDatabase();

                // Persisto le modifiche sulla base dati effettuando il commit delle modifiche apportate nella transazione
                ll.CommitTransaction();

                SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - {1} l'entity Analisi Vendita Configurazione Articolo Aggiuntivo.", Request.Url.AbsolutePath, ((nuovo) ? "Creato" : "Salvato")));

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

        /// <summary>
        /// Restituisce un oggetto contenente gli eventuali errori di validazione dei dati
        /// </summary>
        /// <returns></returns>
        private MessagesCollector ValidaDati()
        {
            MessagesCollector messaggi = new MessagesCollector();

            //Errori dei validatori client
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

            //if (!rdtpDataRedazione.SelectedDate.HasValue)
            //{
            //    messaggi.Add(rfvDataRedazione.ErrorMessage);
            //}

            //if (rtbTitoloAnalisi.Text.Trim() == string.Empty)
            //{
            //    messaggi.Add(rfvTitoloAnalisi.ErrorMessage);
            //}

            return messaggi;
        }

        /// <summary>
        /// Legge i dati inseriti nell'interfaccia e li inserisce nell'entity passata
        /// </summary>
        /// <param name="entityToFill"></param>
        public void EstraiValoriDallaView(Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo entityToFill, Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi logic)
        {
            if (entityToFill == null) throw new ArgumentNullException("entityToFill", "Parametro nullo");
            if (logic == null) throw new ArgumentNullException("logic", "Parametro nullo");

            if (!String.IsNullOrWhiteSpace(rcbTipologia.SelectedValue) && int.TryParse(rcbTipologia.SelectedValue, out int tipologia))
            {
                entityToFill.TipologiaEnum = (Entities.TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo)tipologia;
            }
            
            if(entityToFill.TipologiaEnum == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.MostrareMessaggio ||
                entityToFill.TipologiaEnum == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.MostrareMessaggio_e_AggiungereArticolo)
            {
                entityToFill.TestoAvviso = rtbTestoAvviso.Text.ToTrimmedString();
            }
            else
            {
                entityToFill.TestoAvviso = String.Empty;
            }

            if (entityToFill.TipologiaEnum == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Contratto ||
                entityToFill.TipologiaEnum == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo_Contratto)
            {
                entityToFill.TestoDescrizioneContratto = reTemplateContratto.Content;
            }
            else
            {
                entityToFill.TestoDescrizioneContratto = String.Empty;
            }

            if (entityToFill.TipologiaEnum == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo ||
                entityToFill.TipologiaEnum == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo_Contratto)
            {
                entityToFill.TestoDescrizionePreventivo = reTemplateDescrizionePreventivo.Content;
            }
            else
            {
                entityToFill.TestoDescrizionePreventivo = String.Empty;
            }

            if (entityToFill.TipologiaEnum == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.AggiungereArticolo ||
                entityToFill.TipologiaEnum == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.MostrareMessaggio_e_AggiungereArticolo)
            {
                entityToFill.UnitaMisura = rtbUM.Text.ToTrimmedString();
                entityToFill.Quantita = (decimal?)rntbQuantità.Value;
                entityToFill.Costo = (decimal?)rntbCosto.Value;
                entityToFill.Vendita = (decimal?)rntbVendita.Value;
                entityToFill.TotaleCosto = (decimal?)rntbTotaleCosto.Value;
                entityToFill.RicaricoValuta = (decimal?)rntbRicaricoValore.Value;
                entityToFill.RicaricoPercentuale = (decimal?)rntbRicaricoPercentuale.Value;
                entityToFill.TotaleVendita = (decimal?)rntbTotaleVendita.Value;
            }
            else
            {
                entityToFill.UnitaMisura = String.Empty;
                entityToFill.Quantita = null;
                entityToFill.Costo = null;
                entityToFill.Vendita = null;
                entityToFill.TotaleCosto = null;
                entityToFill.RicaricoValuta = null;
                entityToFill.RicaricoPercentuale = null;
                entityToFill.TotaleVendita = null;
            }

            if (entityToFill.TipologiaEnum == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.AggiungereArticolo ||
                entityToFill.TipologiaEnum == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.MostrareMessaggio_e_AggiungereArticolo)
            {
                entityToFill.CodiceArticoloOut = rcbCodiceArticoloUscita.SelectedValue;
            }
            else
            {
                entityToFill.CodiceArticoloOut = String.Empty;
            }

            if (!String.IsNullOrWhiteSpace(rcbGruppoIngresso.SelectedValue) && decimal.TryParse(rcbGruppoIngresso.SelectedValue, out decimal codiceGruppoInValue))
            {
                entityToFill.CodiceGruppoIn = codiceGruppoInValue;
                entityToFill.GruppoIn = rcbGruppoIngresso.Text;
            }
            else
            {
                entityToFill.CodiceGruppoIn = null;
                entityToFill.GruppoIn = String.Empty;
            }

            if (!String.IsNullOrWhiteSpace(rcbCategoriaIngresso.SelectedValue) && decimal.TryParse(rcbCategoriaIngresso.SelectedValue, out decimal codiceCategoriaInValue))
            {
                entityToFill.CodiceCategoriaIn = codiceCategoriaInValue;
                entityToFill.CategoriaIn = rcbCategoriaIngresso.Text;
            }
            else
            {
                entityToFill.CodiceCategoriaIn = null;
                entityToFill.CategoriaIn = String.Empty;
            }

            if (!String.IsNullOrWhiteSpace(rcbCategoriaStatisticaIngresso.SelectedValue) && decimal.TryParse(rcbCategoriaStatisticaIngresso.SelectedValue, out decimal codiceCategoriStatisticaInValue))
            {
                entityToFill.CodiceCategoriaStatisticaIn = codiceCategoriStatisticaInValue;
                entityToFill.CategoriaStatisticaIn = rcbCategoriaStatisticaIngresso.Text;
            }
            else
            {
                entityToFill.CodiceCategoriaStatisticaIn = null;
                entityToFill.CategoriaStatisticaIn = String.Empty;
            }

            entityToFill.CodiceArticoloIn = rcbCodiceArticoloIngresso.SelectedValue;

            if (entityToFill.TipologiaEnum == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Contratto ||
                entityToFill.TipologiaEnum == TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.Template_Articolo_Descrizione_Preventivo_Contratto)
            {
                entityToFill.ClausoleVessatorieAggiuntive = reClausoleVessatorieAggiuntive.Content;
            }
            else
            {
                entityToFill.ClausoleVessatorieAggiuntive = String.Empty;
            }

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
        /// Abilita o disabilita i campi dell'interfaccia in base al parametro passato
        /// </summary>
        /// <param name="abilitaCampi"></param>
        private void AbilitaCampi(bool enabled)
        {
            PulsanteToolbar_SeparatoreSalva.Visible = enabled;
            PulsanteToolbar_Salva.Visible = enabled;

            rcbGruppoIngresso.Enabled = enabled;
            rcbGruppoUscita.Enabled = enabled;
            rcbCategoriaIngresso.Enabled = enabled;
            rcbCategoriaUscita.Enabled = enabled;
            rcbCategoriaStatisticaIngresso.Enabled = enabled;
            rcbCategoriaStatisticaUscita.Enabled = enabled;
            rcbCodiceArticoloIngresso.Enabled = enabled;
            rcbCodiceArticoloUscita.Enabled = enabled;
            rtbTestoAvviso.ReadOnly = !enabled;
            rgGrigliaClausoleVessatorie.Enabled = enabled;
        }

        /// <summary>
        /// Carica gli oggetti contenenti le informazioni di accesso ai dati ed alle funzionalità esposte dalla pagina
        /// </summary>
        private void CaricaAutorizzazioni()
        {
        }

        /// <summary>
        /// Blocca o permette l'accesso ai dati ed applica agli oggetti dell'interfaccia lo stile in base alle regole di accesso dell'azienda corrente
        /// </summary>
        private void ApplicaAutorizzazioni()
        {
        }

        /// <summary>
        /// Effettua l'inizializzazione dei controlli nella pagina
        /// </summary>
        private void Initialize()
        {
            Logic.Metodo.Archivi llArchivi = new Logic.Metodo.Archivi();
            Entities.TABGRUPPI[] dataSourceGruppi = llArchivi.Read_Gruppi().ToArray();
            rcbGruppoIngresso.DataSource = dataSourceGruppi;
            rcbGruppoIngresso.DataBind();
            rcbGruppoIngresso.ClearSelection();
            rcbGruppoIngresso.Items.Insert(0, new RadComboBoxItem(String.Empty, String.Empty));

            rcbGruppoUscita.DataSource = dataSourceGruppi;
            rcbGruppoUscita.DataBind();
            rcbGruppoUscita.ClearSelection();
            rcbGruppoUscita.Items.Insert(0, new RadComboBoxItem(String.Empty, String.Empty));

            Entities.TABCATEGORIE[] dataSourceCategorie = llArchivi.Read_Categorie().ToArray();

            rcbCategoriaIngresso.DataSource = dataSourceCategorie;
            rcbCategoriaIngresso.DataBind();
            rcbCategoriaIngresso.ClearSelection();
            rcbCategoriaIngresso.Items.Insert(0, new RadComboBoxItem(String.Empty, String.Empty));

            rcbCategoriaUscita.DataSource = dataSourceCategorie;
            rcbCategoriaUscita.DataBind();
            rcbCategoriaUscita.ClearSelection();
            rcbCategoriaUscita.Items.Insert(0, new RadComboBoxItem(String.Empty, String.Empty));

            Entities.TABCATEGORIESTAT[] dataSourceCategorieStatistiche = llArchivi.Read_CategorieStatistiche().ToArray();

            rcbCategoriaStatisticaIngresso.DataSource = dataSourceCategorieStatistiche;
            rcbCategoriaStatisticaIngresso.DataBind();
            rcbCategoriaStatisticaIngresso.ClearSelection();
            rcbCategoriaStatisticaIngresso.Items.Insert(0, new RadComboBoxItem(String.Empty, String.Empty));

            rcbCategoriaStatisticaUscita.DataSource = dataSourceCategorieStatistiche;
            rcbCategoriaStatisticaUscita.DataBind();
            rcbCategoriaStatisticaUscita.ClearSelection();
            rcbCategoriaStatisticaUscita.Items.Insert(0, new RadComboBoxItem(String.Empty, String.Empty));

            // Reset dei campi articolo
            rcbCodiceArticoloIngresso.ClearSelection();
            rcbCodiceArticoloUscita.ClearSelection();

            rtbTestoAvviso.Text = String.Empty;
        }

        /// <summary>
        /// Restituisce l'elenco delle condizioni particolari selezionate
        /// </summary>
        /// <returns></returns>
        private Guid[] GetElencoClausoleVessatorieSelezionate()
        {
            List<Guid> elencoIdentificativi = new List<Guid>();
            foreach(GridDataItem item in rgGrigliaClausoleVessatorie.MasterTableView.Items)
            {
                RadCheckBox rchkClausolaVessatoriaSelezionata = item.FindControl("rchkClausolaVessatoriaSelezionata") as RadCheckBox;
                if (rchkClausolaVessatoriaSelezionata != null && rchkClausolaVessatoriaSelezionata.Checked.HasValue && rchkClausolaVessatoriaSelezionata.Checked.Value)
                {
                    Guid idClausolaVessatoria = (Guid)item.GetDataKeyValue("ID");
                    elencoIdentificativi.Add(idClausolaVessatoria);
                }
            }
            return elencoIdentificativi.ToArray();
        }

        #endregion
    }
}