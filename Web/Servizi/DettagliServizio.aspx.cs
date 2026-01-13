using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SeCoGes.Utilities;
using SeCoGEST.Entities;
using Telerik.Web.UI;

namespace SeCoGEST.Web.Servizi
{
    public partial class DettagliServizio : System.Web.UI.Page
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
        protected EntityId<Entities.Servizio> currentID
        {
            get
            {
                if (Request.QueryString["ID"] != null)
                    return new EntityId<Entities.Servizio>(Request.QueryString["ID"]);
                else
                    return EntityId<Entities.Servizio>.Empty;
            }
        }

        /// <summary>
        /// Nome dell'evento ajax da utilizzare per gestire la visualizzazione della selezione degli articoli
        /// </summary>
        protected string NomeEventoAjaxVisualizzazionePannelloSelezioneArticoli
        {
            get
            {
                return $"{typeof(DettagliServizio).Name}_{ClientID}_VisualizzazionePannelloSelezioneArticoli";
            }
        }

        #region Pulsanti Toolbar

        /// <summary>
        /// Restituisce un riferimento al separatore SeparatoreNuovo della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_SeparatoreNuovo
        {
            get
            {
                return RadToolBar1.FindItemByValue("SeparatoreNuovo");
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

        #endregion

        #region Intercettazione Eventi

        /// <summary>
        /// Metodo di gestione dell'evento Init della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            RadPersistenceHelper.AssociatePersistenceSessionProvider(this);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CaricaAutorizzazioni();

            reDescrizioneServizio.Modules.Clear();

            if (!Helper.Web.IsPostOrCallBack(this))
            {
                RadPersistenceHelper.LoadState(this);

                try
                {
                    SeCoGes.Logging.LogManager.AddLogAccessi(String.Format("Accesso alla pagina '{0}'.", Request.Url.AbsolutePath));

                    Logic.Servizi ll = new Logic.Servizi();
                    Entities.Servizio entityToShow = ll.Find(currentID);
                    //Enabled = false;

                    if (entityToShow != null)
                    {
                        rowArticoli.Visible = true;
                        ShowData(entityToShow, ll);                        
                    }
                    else
                    {
                        rowArticoli.Visible = false;
                        ShowData(new Entities.Servizio(), ll);
                    }

                    ApplicaAutorizzazioni();
                }
                catch (Exception ex)
                {
                    SeCoGes.Logging.LogManager.AddLogErrori(ex);
                    MessageHelper.ShowErrorMessage(this, ex);
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
            RadPersistenceHelper.SaveState(this);

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

        protected void rgGrigliaArticoli_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                string idSelectedRow = (string)(((GridDataItem)(e.Item)).GetDataKeyValue("CodiceAnagraficaArticolo"));
                if (!String.IsNullOrWhiteSpace(idSelectedRow))
                {
                    Logic.ServiziArticoli ll = new Logic.ServiziArticoli();
                    Entities.ServizioArticolo entityToDelete = ll.Find(currentID, new EntityString<Entities.ANAGRAFICAARTICOLI>(idSelectedRow));
                    if (entityToDelete != null)
                    {
                        ll.Delete(entityToDelete, true);

                        SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - Rimosso l'entity ServizioArticolo '{1}' dal servizio '{2}'", Request.Url.AbsolutePath, entityToDelete.CodiceAnagraficaArticolo, entityToDelete.IDServizio));
                    }
                }
            }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);
                MessageHelper.ShowErrorMessage(Page, "Operazione di Eliminazione ServizioArticolo non riuscita, è stato riscontrato il seguente errore:<br />" + ex.Message);
                e.Canceled = true;
            }
        }

        protected void rgGrigliaArticoli_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            HiddenField hd = (HiddenField)((RadGrid)sender).Parent.Parent.FindControl("hdIdRaggruppamento");
            if (currentID != null && currentID.Value != null && !currentID.Value.IsNullOrEmpty())
            {
                Logic.ServiziArticoli llArt = new Logic.ServiziArticoli();
                ((RadGrid)sender).DataSource = llArt.Read(currentID);
            }
        }

        protected void rapSelezioneArticoli_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            string argumentName = (e?.Argument ?? String.Empty);

            if (argumentName == NomeEventoAjaxVisualizzazionePannelloSelezioneArticoli)
            {
                rplSelezioneArticoli.Visible = true;
            }
        }

        protected void rbSalvaArticoli_Click(object sender, EventArgs e)
        {
            string codiceArticoloSelezionato = rcbCodiceArticolo.SelectedValue;

            if (!String.IsNullOrWhiteSpace(codiceArticoloSelezionato))
            {
                Logic.ServiziArticoli llServiziArticoli = new Logic.ServiziArticoli();
                Entities.ServizioArticolo servizioArticolo = llServiziArticoli.Find(currentID, new EntityString<ANAGRAFICAARTICOLI>(codiceArticoloSelezionato));
                if (servizioArticolo == null)
                {
                    servizioArticolo = new ServizioArticolo();
                    servizioArticolo.IDServizio = currentID.Value;
                    servizioArticolo.CodiceAnagraficaArticolo = codiceArticoloSelezionato;

                    llServiziArticoli.Create(servizioArticolo, true);
                    rgGrigliaArticoli.Rebind();
                }
            }

            ClearArticoliCombos();
            rplSelezioneArticoli.Visible = false;
        }

        protected void rbAnnullaInserimentoArticoli_Click(object sender, EventArgs e)
        {
            ClearArticoliCombos();
            rplSelezioneArticoli.Visible = false;
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
                Logic.ServiziArticoli llServiziArticoli = new Logic.ServiziArticoli();

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

                IQueryable<Entities.ServizioArticolo> queryServiziArticoliAggiunti = llServiziArticoli.Read(currentID);
                if (queryServiziArticoliAggiunti.Any())
                {
                    queryBase = queryBase.Where(x => !queryServiziArticoliAggiunti.Any(sa => sa.CodiceAnagraficaArticolo == x.CODICE));
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

        #endregion

        #region Metodi di gestione

        /// <summary>
        /// Mostra nell'interfaccia i dati dell'entity passata
        /// </summary>
        /// <param name="entity"></param>
        private void ShowData(Entities.Servizio entityToShow, Logic.Servizi llOfferte)
        {
            ApplicaRedirectPulsanteAggiorna();

            if (entityToShow == null)
            {
                throw new ArgumentNullException("Parametro nullo", "entityToShow");
            }

            if (!entityToShow.ID.Equals(Guid.Empty))
            {
                lblTitolo.Text = string.Format("Servizio '{0}' ({1})", entityToShow.Nome, entityToShow.Codice);
            }
            else
            {
                lblTitolo.Text = "Nuova Servizio";                
            }

            rtbCodiceServizio.Text = entityToShow.Codice;
            rtbNomeServizio.Text = entityToShow.Nome;
            reDescrizioneServizio.Content = entityToShow.Descrizione;
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
        /// Abilita o disabilita i campi dell'interfaccia in base al parametro passato
        /// </summary>
        /// <param name="abilitaCampi"></param>
        private void AbilitaCampi(bool enabled)
        {
            PulsanteToolbar_SeparatoreNuovo.Visible = enabled;
            PulsanteToolbar_Nuovo.Visible = enabled;
            PulsanteToolbar_SeparatoreSalva.Visible = enabled;
            PulsanteToolbar_Salva.Visible = enabled;

            rtbCodiceServizio.ReadOnly = !enabled;
            rtbNomeServizio.ReadOnly = !enabled;
            reDescrizioneServizio.Enabled = enabled;
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
            Entities.Servizio entityToSave = null;

            // Definisco una variabile che conterrà l'Codice dell'entity salvata. 
            // Se per qualche motivo l'entity non viene salvata allora entityId rimarrà String.Empty
            string entityId = String.Empty;
            Logic.Servizi ll = new Logic.Servizi();

            try
            {
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
                            "Il Servizio che si sta variando non esiste più in archivio."
                                + "\n\rOperazione annullata!");
                    }

                    // Al contrario, se entityToSave è nulla e CurrentId è vuota
                    // vuol dire che la pagina è stata aperta per la creazione di una nuova entità
                    nuovo = true;
                    entityToSave = new Entities.Servizio();

                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriDallaView(entityToSave, ll);


                    //Creo nuova entity
                    ll.Create(entityToSave, false);
                }
                else
                {
                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriDallaView(entityToSave, ll);
                }


                // Persisto le modifiche sulla base dati nella transazione
                ll.SubmitToDatabase();

                SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - {1} l'entity Servizio con Codice '{2}' e Nome '{3}'.", Request.Url.AbsolutePath, ((nuovo) ? "Creato" : "Salvato"), entityToSave.Codice, entityToSave.Nome));

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
        /// Restituisce un oggetto contenente gli eventuali errori di validazione dei dati
        /// </summary>
        /// <returns></returns>
        private MessagesCollector ValidaDati()
        {
            MessagesCollector messaggi = new MessagesCollector();

            //Errori dei validatori client
            Page.Validate(String.Empty);
            if (!Page.IsValid)
            {
                foreach (IValidator validatore in Page.GetValidators(String.Empty))
                {

                    if (!validatore.IsValid)
                    {
                        messaggi.Add(validatore.ErrorMessage);
                    }
                }
                if (messaggi.HaveMessages) return messaggi;
            }

            Logic.Servizi ll = new Logic.Servizi();
            Entities.Servizio entityToSave = ll.Find(currentID);

            Entities.Servizio entitaTrovata = ll.Find(new EntityString<Servizio>(rtbCodiceServizio.Text.Trim()));
            if ((entityToSave == null && entitaTrovata != null) ||
                (entityToSave != null && entitaTrovata != null && entityToSave.ID != entitaTrovata.ID))
            {
                messaggi.Add("Il codice inserito è già stato utilizzato");
            }

            return messaggi;
        }

        /// <summary>
        /// Legge i dati inseriti nell'interfaccia e li inserisce nell'entity passata
        /// </summary>
        /// <param name="entityToFill"></param>
        public void EstraiValoriDallaView(Entities.Servizio entityToFill, Logic.Servizi logic)
        {
            if (entityToFill == null)
            {
                throw new ArgumentNullException("Parametro nullo", "entityToFill");
            }

            entityToFill.Codice = rtbCodiceServizio.Text.Trim();
            entityToFill.Nome = rtbNomeServizio.Text.Trim();            
            entityToFill.Descrizione = reDescrizioneServizio.Content.Trim();            
        }

        /// <summary>
        /// Effettua la publizia e dei valori dei combo relativi alla selezione dell'articolo
        /// </summary>
        private void ClearArticoliCombos()
        {
            rcbGruppo.ClearSelection();
            rcbGruppo.SelectedValue = String.Empty;
            rcbGruppo.Text = String.Empty;

            rcbCategoria.ClearSelection();
            rcbCategoria.SelectedValue = String.Empty;
            rcbCategoria.Text = String.Empty;

            rcbCategoriaStatistica.ClearSelection();
            rcbCategoriaStatistica.SelectedValue = String.Empty;
            rcbCategoriaStatistica.Text = String.Empty;

            rcbCodiceArticolo.ClearSelection();
            rcbCodiceArticolo.SelectedValue = String.Empty;
            rcbCodiceArticolo.Text = String.Empty;
        }

        #endregion
    }
}