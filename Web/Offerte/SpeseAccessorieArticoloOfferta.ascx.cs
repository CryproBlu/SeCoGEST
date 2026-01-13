using SeCoGes.Utilities;
using SeCoGEST.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SeCoGEST.Web.Offerte
{
    public partial class SpeseAccessorieArticoloOfferta : System.Web.UI.UserControl
    {
        #region Classi

        [Serializable]
        public class SpesaAccessoriaArticoloOfferta
        {
            public Guid ID { get; set; }
            public decimal Gruppo { get; set; }
            public string DescrizioneGruppo { get; set; }
            public decimal Categoria { get; set; }
            public string DescrizioneCategoria { get; set; }
            public decimal CategoriaStatistica { get; set; }
            public string DescrizioneCategoriaStatistica { get; set; }
            public string CodiceArticolo { get; set; }
            public string DescrizioneArticolo { get; set; }
            public string UnitaDiMisura { get; set; }
            public decimal Quantita { get; set; }
            public decimal CostoUnitario { get; set; }
            public decimal RicaricoValore { get; set; }
            public decimal RicaricoPercentuale { get; set; }
            public decimal PrezzoUnitario { get; set; }
            public decimal TotaleCosto { get; set; }
            public decimal TotaleVendita { get; set; }

            public bool Creato { get; set; }
            public bool Aggiornato { get; set; }
            public bool Eliminato { get; set; }
        }

        #endregion

        #region Eventi

        public EventHandler UserRequestSaveArticle;
        public EventHandler UserRequestSaveUndoChange;

        #endregion

        #region Costanti

        /// <summary>
        /// Nome dell'evento ajax da utilizzare per gestire la visualizzazione della selezione degli articoli
        /// </summary>
        protected string NomeEventoAjaxVisualizzazionePannelloSelezioneArticoli
        {
            get
            {
                return $"{typeof(SpeseAccessorieArticoloOfferta).Name}_{ClientID}_VisualizzazionePannelloSelezioneArticoli";
            }
        }

        /// <summary>
        /// Nome dell'evento per la gestione dell'evento lato cliente "OnCommand" della griglia degli articoli
        /// </summary>
        protected string NomeEvento_GrigliaArticoli_OnCommand_ClientSide
        {
            get
            {
                return $"{typeof(SpeseAccessorieArticoloOfferta).Name}_{ClientID}_rgGrigliaArticoli_OnCommand";
            }
        }

        /// <summary>
        /// Nome dell'evento per la gestione dell'evento lato cliente "OnClientDropDownOpening" del combo relativo al gruppo, categoria, categoria statistica, codice articolo
        /// </summary>
        protected string NomeEvento_GruppoCategoriaCategoriaStatisticaCodiceArticolo_OnClientDropDownOpening_ClientSide
        {
            get
            {
                return $"{typeof(SpeseAccessorieArticoloOfferta).Name}_{ClientID}_rcbGruppoCategoriaCategoriaStatistica_OnClientDropDownOpening";
            }
        }

        /// <summary>
        /// Nome dell'evento per la gestione dell'evento lato cliente "OnClientItemsRequesting" del combo relativo al gruppo, categoria, categoria statistica, codice articolo
        /// </summary>
        protected string NomeEvento_GruppoCategoriaCategoriaStatisticaCodiceArticolo_OnClientItemsRequesting_ClientSide
        {
            get
            {
                return $"{typeof(SpeseAccessorieArticoloOfferta).Name}_{ClientID}_rcbGruppoCategoriaCategoriaStatisticaCodiceArticolo__OnClientItemsRequesting";
            }
        }

        /// <summary>
        /// Nome dell'evento per la gestione dell'evento lato cliente "OnClientClicking" del tasto di salvataggio dell'articolo selezionato
        /// </summary>
        protected string NomeEvento_SalvaArticoli_OnClientClicking_ClientSide
        {
            get
            {
                return $"{typeof(SpeseAccessorieArticoloOfferta).Name}_{ClientID}_rbSalvaArticoli_OnClientClicking";
            }
        }

        protected string NomeEvento_Quantità_OnValueChanged_ClientSide
        {
            get
            {
                return $"{typeof(SpeseAccessorieArticoloOfferta).Name}_{ClientID}_rntbQuantità_OnValueChanged";
            }
        }

        protected string NomeEvento_Costo_OnValueChanged_ClientSide
        {
            get
            {
                return $"{typeof(SpeseAccessorieArticoloOfferta).Name}_{ClientID}_rntbCosto_OnValueChanged";
            }
        }

        protected string NomeEvento_Vendita_OnValueChanged_ClientSide
        {
            get
            {
                return $"{typeof(SpeseAccessorieArticoloOfferta).Name}_{ClientID}_rntbVendita_OnValueChanged";
            }
        }

        protected string NomeEvento_RicaricoValore_OnValueChanged_ClientSide
        {
            get
            {
                return $"{typeof(SpeseAccessorieArticoloOfferta).Name}_{ClientID}_rntbRicaricoValore_OnValueChanged";
            }
        }

        protected string NomeEvento_RicaricoPercentuale_OnValueChanged_ClientSide
        {
            get
            {
                return $"{typeof(SpeseAccessorieArticoloOfferta).Name}_{ClientID}_rntbRicaricoPercentuale_OnValueChanged";
            }
        }

        protected string NomeEvento_TotaleCosto_OnValueChanged_ClientSide
        {
            get
            {
                return $"{typeof(SpeseAccessorieArticoloOfferta).Name}_{ClientID}_rntbTotaleCosto_OnValueChanged";
            }
        }

        protected string NomeEvento_TotaleVendita_OnValueChanged_ClientSide
        {
            get
            {
                return $"{typeof(SpeseAccessorieArticoloOfferta).Name}_{ClientID}_rntbTotaleVendita_OnValueChanged";
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Restituisce l'ID corrente
        /// </summary>
        protected Guid CurrentID
        {
            get
            {
                if (ViewState["ID"] == null) ViewState["ID"] = Guid.Empty;
                return (Guid)ViewState["ID"];
            }
            set
            {
                ViewState["ID"] = value;
            }
        }

        /// <summary>
        /// Contiene il valore che indica se il controllo gestisce la selezione delle spese di gestione di un articolo nuovo o meno
        /// </summary>
        public bool IsNew
        {
            get
            {
                if (ViewState["IsNew"] == null) ViewState["IsNew"] = false;
                return (bool)ViewState["IsNew"];
            }
            private set
            {
                ViewState["IsNew"] = value;
            }
        }

        /// <summary>
        /// Restituisce e setta la proprietà relativa al datasource da applicare alla griglia degli articoli
        /// </summary>
        protected SpesaAccessoriaArticoloOfferta[] DataSource
        {
            get
            {
                return (SpesaAccessoriaArticoloOfferta[])ViewState["DataSource"];
            }
            set
            {
                ViewState["DataSource"] = value;
            }
        }

        #endregion

        #region Metodi Pubblici

        /// <summary>
        /// Restituisce l'elenco degli articoli relativo alle spese accessorie
        /// </summary>
        /// <returns></returns>
        public SpesaAccessoriaArticoloOfferta[] GetElencoSpesaAccessoriaArticoloOfferta()
        {
            return DataSource;
        }

        /// <summary>
        /// Effettua l'inizializzazione dell'usercontrol
        /// </summary>
        public void Initialize()
        {
            CurrentID = Guid.Empty;
            IsNew = true;
            rgGrigliaArticoli.Rebind();
        }

        public void Initialize(EntityId<OffertaArticolo> identificativoOffertaArticolo)
        {
            CurrentID = identificativoOffertaArticolo.Value;
            IsNew = false;
            rgGrigliaArticoli.Rebind();
        }

        /// <summary>
        /// Effettua il casto dell'entitò offerta articolo nel dto relativo alla spesa accessoria
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public SpesaAccessoriaArticoloOfferta Cast(OffertaArticolo entity)
        {
            SpesaAccessoriaArticoloOfferta dto = new SpesaAccessoriaArticoloOfferta();
            dto.ID = entity.ID;
            dto.Gruppo = entity.ANAGRAFICAARTICOLI.TABGRUPPI.CODICE;
            dto.DescrizioneGruppo = entity.ANAGRAFICAARTICOLI.TABGRUPPI.DESCRIZIONE;
            dto.Categoria = entity.ANAGRAFICAARTICOLI.TABCATEGORIE.CODICE;
            dto.DescrizioneCategoria = entity.ANAGRAFICAARTICOLI.TABCATEGORIE.DESCRIZIONE;
            dto.CategoriaStatistica = entity.ANAGRAFICAARTICOLI.TABCATEGORIESTAT.CODICE;
            dto.DescrizioneCategoriaStatistica = entity.ANAGRAFICAARTICOLI.TABCATEGORIESTAT.DESCRIZIONE;
            dto.CodiceArticolo = entity.ANAGRAFICAARTICOLI.CODICE;
            dto.DescrizioneArticolo = entity.ANAGRAFICAARTICOLI.DESCRIZIONE;
            dto.UnitaDiMisura = entity.UnitaMisura;
            dto.Quantita = (entity.Quantita.HasValue) ? entity.Quantita.Value : 0;
            dto.CostoUnitario = (entity.Costo.HasValue) ? entity.Costo.Value : 0;
            dto.RicaricoValore = (entity.RicaricoValuta.HasValue) ? entity.RicaricoValuta.Value : 0;
            dto.RicaricoPercentuale = (entity.RicaricoPercentuale.HasValue) ? entity.RicaricoPercentuale.Value : 0;
            dto.PrezzoUnitario = (entity.Vendita.HasValue) ? entity.Vendita.Value : 0;
            dto.TotaleCosto = (entity.TotaleCosto.HasValue) ? entity.TotaleCosto.Value : 0;
            dto.TotaleVendita = (entity.TotaleVendita.HasValue) ? entity.TotaleVendita.Value : 0;

            return dto;
        }

        /// <summary>
        /// Effettua il casto dell'entitò offerta articolo nel dto relativo alla spesa accessoria
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public OffertaArticolo Cast(SpesaAccessoriaArticoloOfferta dto)
        {
            OffertaArticolo entity = new OffertaArticolo();
            Fill(entity, dto);
            return entity;
        }

        /// <summary>
        /// Effettua il casto dell'entitò offerta articolo nel dto relativo alla spesa accessoria
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public void Fill(OffertaArticolo entity, SpesaAccessoriaArticoloOfferta dto)
        {           
            entity.ID = dto.ID;
            entity.CodiceGruppo = dto.Gruppo;
            entity.CodiceCategoria = dto.Categoria;
            entity.CodiceCategoriaStatistica = dto.CategoriaStatistica;
            entity.CodiceArticolo = dto.CodiceArticolo;
            entity.Descrizione = dto.DescrizioneArticolo;
            entity.UnitaMisura = dto.UnitaDiMisura;
            entity.Quantita = dto.Quantita;
            entity.Costo = dto.CostoUnitario;
            entity.RicaricoValuta = dto.RicaricoValore;
            entity.RicaricoPercentuale = dto.RicaricoPercentuale;
            entity.Vendita = dto.PrezzoUnitario;
            entity.TotaleCosto = dto.TotaleCosto;
            entity.TotaleVendita = dto.TotaleVendita;
        }

        #endregion

        #region Intercettazione Eventi

        /// <summary>
        /// Metodo di gestione dell'evento di caricamento delle pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitializeControlForClientSideFunction();
        }

        /// <summary>
        /// Metodo di gestione dell'evento di cancellazione di un record della griglia degli articoli
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGrigliaArticoli_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                Guid idSelectedRow = (Guid)(((GridDataItem)(e.Item)).GetDataKeyValue("ID"));
                if (!idSelectedRow.IsNullOrEmpty() && DataSource != null)
                {
                    SpesaAccessoriaArticoloOfferta spesaAccessoria = DataSource.FirstOrDefault(x => x.ID == idSelectedRow);
                    if (spesaAccessoria != null) spesaAccessoria.Eliminato = true;
                }
            }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);
                MessageHelper.ShowErrorMessage(Page, "Operazione di Eliminazione SpesaAccessoriaArticoloOfferta non riuscita, è stato riscontrato il seguente errore:<br />" + ex.Message);
                e.Canceled = true;
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento di richiesta dei dati nella griglia degli articoli
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGrigliaArticoli_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (DataSource == null)
            {
                Logic.OfferteArticoli llOfferteArticoli = new Logic.OfferteArticoli();
                DataSource = llOfferteArticoli.ReadSpeseAccessorie(new EntityId<OffertaArticolo>(CurrentID)).Select(x => Cast(x)).ToArray();
                if (DataSource == null) DataSource = new SpesaAccessoriaArticoloOfferta[] { };
            }

            ((RadGrid)sender).DataSource = DataSource.Where(x => !x.Eliminato).ToArray();
        }

        /// <summary>
        /// Metodo di gestione dell'evento di una richiesta post ajax
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rapSelezioneArticoli_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            string argumentName = (e?.Argument ?? String.Empty);

            if (argumentName == NomeEventoAjaxVisualizzazionePannelloSelezioneArticoli)
            {
                rplSelezioneArticoli.Visible = true;
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento click relativo al tasto "Salva"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbSalvaArticoli_Click(object sender, EventArgs e)
        {
            RadButton button = sender as RadButton;
            if (button == null) return;

            Page.Validate(button.ValidationGroup);

            if (ValidatorIsValidByValidationGroup(button.ValidationGroup))
            {
                SpesaAccessoriaArticoloOfferta dto = ExtractData();
                if (dto != null) DataSource = DataSource.Union(new SpesaAccessoriaArticoloOfferta[] { dto }).ToArray();

                RaiseOnUserRequestSaveArticle();

                ClearSelectArticleControls();
                rplSelezioneArticoli.Visible = false;
                rgGrigliaArticoli.Rebind();
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento click relativo al tasto "Annulla"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbAnnullaInserimentoArticoli_Click(object sender, EventArgs e)
        {
            RaiseOnUserRequestSaveUndoChange();
            ClearSelectArticleControls();
            rplSelezioneArticoli.Visible = false;
            rgGrigliaArticoli.Rebind();
        }

        /// <summary>
        /// Metodo di gestione dell'evento di richiesta dei gruppi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


        /// <summary>
        /// Metodo di gestione dell'evento di richiesta delle categorie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Metodo di gestione dell'evento di richiesta delle categorie statistiche
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                if (DataSource != null && DataSource.Any())
                {
                    queryBase = queryBase.Where(x => !DataSource.Any(sa => sa.CodiceArticolo == x.CODICE));
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

        #region Funzioni accessorie

        /// <summary>
        /// Effettua l'inizializzazione dei controlli per la registrazione agli eventi lato client necessari per gestire l'usercontrol
        /// </summary>
        private void InitializeControlForClientSideFunction()
        {
            rgGrigliaArticoli.ClientSettings.ClientEvents.OnCommand = NomeEvento_GrigliaArticoli_OnCommand_ClientSide;

            rcbGruppo.OnClientDropDownOpening = NomeEvento_GruppoCategoriaCategoriaStatisticaCodiceArticolo_OnClientDropDownOpening_ClientSide;
            rcbCategoria.OnClientDropDownOpening = NomeEvento_GruppoCategoriaCategoriaStatisticaCodiceArticolo_OnClientDropDownOpening_ClientSide;
            rcbCategoriaStatistica.OnClientDropDownOpening = NomeEvento_GruppoCategoriaCategoriaStatisticaCodiceArticolo_OnClientDropDownOpening_ClientSide;
            rcbCodiceArticolo.OnClientDropDownOpening = NomeEvento_GruppoCategoriaCategoriaStatisticaCodiceArticolo_OnClientDropDownOpening_ClientSide;

            rcbGruppo.OnClientItemsRequesting = NomeEvento_GruppoCategoriaCategoriaStatisticaCodiceArticolo_OnClientItemsRequesting_ClientSide;
            rcbCategoria.OnClientItemsRequesting = NomeEvento_GruppoCategoriaCategoriaStatisticaCodiceArticolo_OnClientItemsRequesting_ClientSide;
            rcbCategoriaStatistica.OnClientItemsRequesting = NomeEvento_GruppoCategoriaCategoriaStatisticaCodiceArticolo_OnClientItemsRequesting_ClientSide;
            rcbCodiceArticolo.OnClientItemsRequesting = NomeEvento_GruppoCategoriaCategoriaStatisticaCodiceArticolo_OnClientItemsRequesting_ClientSide;

            rbSalvaArticoli.OnClientClicking = NomeEvento_SalvaArticoli_OnClientClicking_ClientSide;

            rntbQuantità.ClientEvents.OnValueChanged = NomeEvento_Quantità_OnValueChanged_ClientSide;
            rntbCosto.ClientEvents.OnValueChanged = NomeEvento_Quantità_OnValueChanged_ClientSide;
            rntbRicaricoValore.ClientEvents.OnValueChanged = NomeEvento_RicaricoValore_OnValueChanged_ClientSide;
            rntbRicaricoPercentuale.ClientEvents.OnValueChanged = NomeEvento_RicaricoPercentuale_OnValueChanged_ClientSide;
            rntbVendita.ClientEvents.OnValueChanged = NomeEvento_Vendita_OnValueChanged_ClientSide;
            rntbTotaleCosto.ClientEvents.OnValueChanged = NomeEvento_TotaleCosto_OnValueChanged_ClientSide;
            rntbTotaleVendita.ClientEvents.OnValueChanged = NomeEvento_TotaleVendita_OnValueChanged_ClientSide;
        }

        /// <summary>
        /// Effettua la publizia e dei valori degli oggetti relativi alla selezione di un articolo
        /// </summary>
        private void ClearSelectArticleControls()
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

            rntbQuantità.Value = null;
            rntbCosto.Value = null;
            rntbRicaricoValore.Value = null;
            rntbRicaricoPercentuale.Value = null;
            rntbVendita.Value = null;
            rntbTotaleCosto.Value = null;
            rntbTotaleVendita.Value = null;
        }

        /// <summary>
        /// Effettua l'estrazione dei dati dai campi presenti nell'usercontrol
        /// </summary>
        /// <returns></returns>
        private SpesaAccessoriaArticoloOfferta ExtractData()
        {
            SpesaAccessoriaArticoloOfferta dto = new SpesaAccessoriaArticoloOfferta();
            dto.ID = (Guid.TryParse(hfIdArticoloSpesaAccessoria.Value, out Guid idArticoloSpesaAccessorie) ? idArticoloSpesaAccessorie : Guid.NewGuid());
            dto.Creato = String.IsNullOrWhiteSpace(hfIdArticoloSpesaAccessoria.Value);
            dto.Aggiornato = !String.IsNullOrWhiteSpace(hfIdArticoloSpesaAccessoria.Value);

            dto.Gruppo = (decimal.TryParse(rcbGruppo.SelectedValue, out decimal codiceGruppo) ? codiceGruppo : 0);
            dto.DescrizioneGruppo = rcbGruppo.Text;
            dto.Categoria = (decimal.TryParse(rcbCategoria.SelectedValue, out decimal codiceCategoria) ? codiceCategoria : 0);
            dto.DescrizioneCategoria = rcbCategoria.Text;
            dto.CategoriaStatistica = (decimal.TryParse(rcbCategoriaStatistica.SelectedValue, out decimal codiceCategoriaStatistica) ? codiceCategoriaStatistica : 0);
            dto.DescrizioneCategoriaStatistica = rcbCategoriaStatistica.Text;
            dto.CodiceArticolo = rcbCodiceArticolo.SelectedValue;
            dto.DescrizioneArticolo = rcbCodiceArticolo.Text;
            dto.UnitaDiMisura = rtbUM.Text.ToTrimmedString();
            dto.Quantita = (rntbQuantità.Value.HasValue ? (decimal)rntbQuantità.Value : 0);
            dto.CostoUnitario = (rntbCosto.Value.HasValue ? (decimal)rntbCosto.Value : 0);
            dto.RicaricoValore = (rntbRicaricoValore.Value.HasValue ? (decimal)rntbRicaricoValore.Value : 0);
            dto.RicaricoPercentuale = (rntbRicaricoPercentuale.Value.HasValue ? (decimal)rntbRicaricoPercentuale.Value : 0);
            dto.PrezzoUnitario = (rntbVendita.Value.HasValue ? (decimal)rntbVendita.Value : 0);
            dto.TotaleCosto = (rntbTotaleCosto.Value.HasValue ? (decimal)rntbTotaleCosto.Value : 0);
            dto.TotaleVendita = (rntbTotaleVendita.Value.HasValue ? (decimal)rntbTotaleVendita.Value : 0);
            return dto;
        }

        /// <summary>
        /// Valorizza i capi presenti nell'usercontrol in base al dto passato come parametro
        /// </summary>
        /// <param name="dto"></param>
        private void FillComponent(SpesaAccessoriaArticoloOfferta dto)
        {
            hfIdArticoloSpesaAccessoria.Value = dto.ID.ToString();

            rcbGruppo.SelectedValue = dto.Gruppo.ToString();
            rcbGruppo.Text = dto.DescrizioneGruppo;

            rcbCategoria.SelectedValue = dto.Categoria.ToString();
            rcbCategoria.Text = dto.DescrizioneCategoria;

            rcbCategoriaStatistica.SelectedValue = dto.Categoria.ToString();
            rcbCategoriaStatistica.Text = dto.DescrizioneCategoria;

            rcbCodiceArticolo.SelectedValue = dto.CodiceArticolo;
            rcbCodiceArticolo.Text = dto.DescrizioneArticolo;

            rtbUM.Text = dto.UnitaDiMisura;
            rntbQuantità.Value = (double)dto.Quantita;
            rntbCosto.Value = (double)dto.CostoUnitario;
            rntbRicaricoValore.Value = (double)dto.RicaricoValore;
            rntbRicaricoPercentuale.Value = (double)dto.RicaricoPercentuale;
            rntbVendita.Value = (double)dto.PrezzoUnitario;
            rntbTotaleCosto.Value = (double)dto.TotaleCosto;
            rntbTotaleVendita.Value = (double)dto.TotaleVendita;
        }

        /// <summary>
        /// Scatena l'evento relativo alla richiesta, da paret dell'utente, di salvare le spese accessorie selezionate
        /// </summary>
        private void RaiseOnUserRequestSaveArticle()
        {
            if (UserRequestSaveArticle != null) UserRequestSaveArticle(this, EventArgs.Empty);
        }

        /// <summary>
        /// Scatena l'evento relativo alla richiesta, da paret dell'utente, di annullare le modifiche relative alle spese accessorie
        /// </summary>
        private void RaiseOnUserRequestSaveUndoChange()
        {
            if (UserRequestSaveUndoChange != null) UserRequestSaveUndoChange(this, EventArgs.Empty);
        }

        /// <summary>
        /// Recupera i validatori in base al gruppo di validazione fornito e verifica che tutti siano validi
        /// </summary>
        /// <param name="validationGroup"></param>
        /// <returns></returns>
        private bool ValidatorIsValidByValidationGroup(string validationGroup)
        {
            ValidatorCollection validatori = Page.GetValidators(validationGroup);

            bool returnValue = true;

            if (validatori != null && validatori.Count > 0)
            {
                foreach (IValidator validatore in validatori)
                {
                    if (!validatore.IsValid)
                    {
                        returnValue = false;
                        break;
                    }
                }
            }

            return returnValue;
        }

        #endregion
    }
}