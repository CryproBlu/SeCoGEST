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
    public partial class OffertaRaggruppamentoEditItem : System.Web.UI.UserControl
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!Helper.Web.IsPostOrCallBack(this.Page))
        //    {
        //        Inizializza();
        //    }
        //}


        public void Inizializza(string validationGroup)
        {
            // Reset dei campi principali di selezione
            Logic.Metodo.Archivi llArchivi = new Logic.Metodo.Archivi();
            rcbGruppo.DataSource = llArchivi.Read_Gruppi();
            rcbCategoria.DataSource = llArchivi.Read_Categorie();
            rcbCategoriaStatistica.DataSource = llArchivi.Read_CategorieStatistiche();
            rcbGruppo.DataBind();
            rcbCategoria.DataBind();
            rcbCategoriaStatistica.DataBind();

            rcbGruppo.Enabled = true;
            rcbCategoria.Enabled = true;
            rcbCategoriaStatistica.Enabled = true;
            rcbCodiceArticolo.Enabled = true;

            rcbGruppo.ClearSelection();
            rcbCategoria.ClearSelection();
            rcbCategoriaStatistica.ClearSelection();


            // Reset dei campi articolo
            rcbCodiceArticolo.ClearSelection();
            rcbCodiceArticolo.Items.Clear();
            rcbCodiceArticolo.Text = String.Empty;
            rcbCodiceArticolo.SelectedValue = String.Empty;

            rtbDescrizione.Text = string.Empty;
            rtbUM.Text = string.Empty;
            rntbQuantità.Value = null;
            rntbCosto.Value = null;
            rntbVendita.Value = null;
            rntbTotaleCosto.Value = null;
            rntbRicaricoValore.Value = null;
            rntbRicaricoPercentuale.Value = null;
            rntbTotaleVendita.Value = null;

            // Reset dei campi aggiuntivi
            //repCampiAggiuntivi.DataSource = null;
            //repCampiAggiuntivi.DataSourceID = null;
            //repCampiAggiuntivi.DataBind();
            //repCampiAggiuntivi.Visible = false;


            // Reset dello stato degli oggetti
            //lbContinua.Visible = true;
            //rowAddArticoloCodiciSelezione.Visible = false;
            //rowAddArticolo.Visible = false;

            rfvGruppo.ValidationGroup = validationGroup;
            rcbGruppo.ValidationGroup = validationGroup;

            rfvCategoria.ValidationGroup = validationGroup;
            rcbCategoria.ValidationGroup = validationGroup;

            rfvCategoriaStatistica.ValidationGroup = validationGroup;
            rcbCategoriaStatistica.ValidationGroup = validationGroup;

            rfvCodiceArticolo.ValidationGroup = validationGroup;
            rcbCodiceArticolo.ValidationGroup = validationGroup;

            rfvDescrizione.ValidationGroup = validationGroup;
            rtbDescrizione.ValidationGroup = validationGroup;

            rfvUM.ValidationGroup = validationGroup;
            rtbUM.ValidationGroup = validationGroup;

            rfvQuantità.ValidationGroup = validationGroup;
            rntbQuantità.ValidationGroup = validationGroup;

            //rfvCosto.ValidationGroup = validationGroup;
            rntbCosto.ValidationGroup = validationGroup;

            rfvVendita.ValidationGroup = validationGroup;
            rntbVendita.ValidationGroup = validationGroup;

            rntbRicaricoValore.ValidationGroup = validationGroup;
            rntbRicaricoPercentuale.ValidationGroup = validationGroup;

            rntbTotaleCosto.ValidationGroup = validationGroup;

            rfvTotaleVendita.ValidationGroup = validationGroup;
            rntbTotaleVendita.ValidationGroup = validationGroup;
        }

        public void Inizializza(Guid idOffertaArticolo, string validationGroup)
        {
            Inizializza(validationGroup);

            Logic.OfferteArticoli llArt = new Logic.OfferteArticoli();
            Entities.OffertaArticolo art = llArt.Find(new Entities.EntityId<Entities.OffertaArticolo>(idOffertaArticolo));

            // Valorizzazione dei campi principali di selezione
            if (art.CodiceGruppo.HasValue)
            {
                rcbGruppo.SelectedValue = art.CodiceGruppo.Value.ToString();
                rcbGruppo.Text = art.TABGRUPPI?.DESCRIZIONE ?? rcbGruppo.SelectedValue;
            }

            if (art.CodiceCategoria.HasValue)
            {
                rcbCategoria.SelectedValue = art.CodiceCategoria.Value.ToString();
                rcbCategoria.Text = art.TABCATEGORIE?.DESCRIZIONE ?? rcbCategoria.SelectedValue;
            }

            if (art.CodiceCategoriaStatistica.HasValue)
            {
                rcbCategoriaStatistica.SelectedValue = art.CodiceCategoriaStatistica.Value.ToString();
                rcbCategoriaStatistica.Text = art.TABCATEGORIESTAT?.DESCRIZIONE ?? rcbCategoriaStatistica.SelectedValue;
            }

            //rcbGruppo.Enabled = false;
            //rcbCategoria.Enabled = false;
            //rcbCategoriaStatistica.Enabled = false;

            // Valorizzazione dei campi articolo
            rcbCodiceArticolo.SelectedValue = art.CodiceArticolo.ToString();
            rcbCodiceArticolo.Text = art.ANAGRAFICAARTICOLI?.DESCRIZIONE ?? String.Empty;
            //rcbCodiceArticolo.Enabled = false;

            rtbDescrizione.Text = art.Descrizione;
            rtbUM.Text = art.UnitaMisura;
            rntbQuantità.Value = art.Quantita.HasValue ? (double?)art.Quantita : null;
            rntbCosto.Value = art.Costo.HasValue ? (double?)art.Costo : null;
            rntbVendita.Value = art.Vendita.HasValue ? (double?)art.Vendita : null;
            rntbTotaleCosto.Value = art.TotaleCosto.HasValue ? (double?)art.TotaleCosto : null;
            rntbRicaricoValore.Value = art.RicaricoValuta.HasValue ? (double?)art.RicaricoValuta : null;
            rntbRicaricoPercentuale.Value = art.RicaricoPercentuale.HasValue ? (double?)art.RicaricoPercentuale : null;
            rntbTotaleVendita.Value = art.TotaleVendita.HasValue ? (double?)art.TotaleVendita : null;

            // Valorizzazione dei campi aggiuntivi
            //List<Entities.OffertaArticoloCampoAggiuntivo> campiAggiuntivi = art.OffertaArticoloCampoAggiuntivos.ToList();
            //repCampiAggiuntivi.DataSource = campiAggiuntivi;
            //repCampiAggiuntivi.DataBind();
            //repCampiAggiuntivi.Visible = campiAggiuntivi.Count() > 0;

            // Reset dello stato degli oggetti
            //lbContinua.Visible = false;
            //rowAddArticoloCodiciSelezione.Visible = true;
            //rowAddArticolo.Visible = true;

            SpeseAccessorieArticoloOfferta.Initialize(new Entities.EntityId<Entities.OffertaArticolo>(idOffertaArticolo));
        }


        protected void lbContinua_Click(object sender, EventArgs e)
        {
            rcbGruppo.Enabled = false;
            rcbCategoria.Enabled = false;
            rcbCategoriaStatistica.Enabled = false;

            //lbContinua.Visible = false;
            //rowAddArticoloCodiciSelezione.Visible = true;
            //rowAddArticolo.Visible = true;


            // Proposgo gli eventuali campi aggiuntivi indicati in configurazione
            //decimal codiceGruppo = decimal.Parse(rcbGruppo.SelectedValue);
            //decimal codiceCategoria = decimal.Parse(rcbCategoria.SelectedValue);
            //decimal codiceCategoriaStatistica = decimal.Parse(rcbCategoriaStatistica.SelectedValue);

            //Logic.OfferteArchivioCampiAggiuntivi llCampiAgg = new Logic.OfferteArchivioCampiAggiuntivi();
            //IQueryable<Entities.OffertaArchivioCampoAggiuntivo> campiAggiuntivi = llCampiAgg.Read(codiceGruppo, codiceCategoria, codiceCategoriaStatistica);

            //repCampiAggiuntivi.DataSource = campiAggiuntivi;
            //repCampiAggiuntivi.DataBind();
            //repCampiAggiuntivi.Visible = true;

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

        protected void rcbCodiceArticolo_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
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
    }
}