using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SeCoGEST.Web.Preventivi
{
    public partial class AnalisiCostoRaggruppamentoEditItem : System.Web.UI.UserControl
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!Helper.Web.IsPostOrCallBack(this.Page))
        //    {
        //        Inizializza();
        //    }
        //}


        public void Inizializza()
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

            rcbGruppo.ClearSelection();
            rcbCategoria.ClearSelection();
            rcbCategoriaStatistica.ClearSelection();


            // Reset dei campi articolo
            rcbCodiceArticolo.ClearSelection();
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
            repCampiAggiuntivi.DataSource = null;
            repCampiAggiuntivi.DataSourceID = null;
            repCampiAggiuntivi.DataBind();
            repCampiAggiuntivi.Visible = false;


            // Reset dello stato degli oggetti
            lbContinua.Visible = true;
            rowAddArticoloCodiciSelezione.Visible = false;
            rowAddArticolo.Visible = false;
        }

        public void Inizializza(Guid idAnalisiCostoArticolo)
        {
            Inizializza();

            Logic.AnalisiCostiArticoli llArt = new Logic.AnalisiCostiArticoli();
            Entities.AnalisiCostoArticolo art = llArt.Find(new Entities.EntityId<Entities.AnalisiCostoArticolo>(idAnalisiCostoArticolo));

            // Valorizzazione dei campi principali di selezione
            if (art.CodiceGruppo.HasValue) rcbGruppo.SelectedValue = art.CodiceGruppo.Value.ToString();
            if (art.CodiceCategoria.HasValue) rcbCategoria.SelectedValue = art.CodiceCategoria.Value.ToString();
            if (art.CodiceCategoriaStatistica.HasValue) rcbCategoriaStatistica.SelectedValue = art.CodiceCategoriaStatistica.Value.ToString();
            rcbGruppo.Enabled = false;
            rcbCategoria.Enabled = false;
            rcbCategoriaStatistica.Enabled = false;

            // Valorizzazione dei campi articolo
            rcbCodiceArticolo.SelectedValue = art.CodiceArticolo.ToString();
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
            List<Entities.AnalisiCostoArticoloCampoAggiuntivo> campiAggiuntivi = art.AnalisiCostoArticoloCampoAggiuntivos.ToList();
            repCampiAggiuntivi.DataSource = campiAggiuntivi;
            repCampiAggiuntivi.DataBind();
            repCampiAggiuntivi.Visible = campiAggiuntivi.Count() > 0;

            // Reset dello stato degli oggetti
            lbContinua.Visible = false;
            rowAddArticoloCodiciSelezione.Visible = true;
            rowAddArticolo.Visible = true;
        }


        protected void lbContinua_Click(object sender, EventArgs e)
        {
            rcbGruppo.Enabled = false;
            rcbCategoria.Enabled = false;
            rcbCategoriaStatistica.Enabled = false;

            lbContinua.Visible = false;
            rowAddArticoloCodiciSelezione.Visible = true;
            rowAddArticolo.Visible = true;


            // Proposgo gli eventuali campi aggiuntivi indicati in configurazione
            decimal codiceGruppo = decimal.Parse(rcbGruppo.SelectedValue);
            decimal codiceCategoria = decimal.Parse(rcbCategoria.SelectedValue);
            decimal codiceCategoriaStatistica = decimal.Parse(rcbCategoriaStatistica.SelectedValue);

            Logic.AnalisiCostiArchivioCampiAggiuntivi llCampiAgg = new Logic.AnalisiCostiArchivioCampiAggiuntivi();
            IQueryable<Entities.AnalisiCostoArchivioCampoAggiuntivo> campiAggiuntivi = llCampiAgg.Read(codiceGruppo, codiceCategoria, codiceCategoriaStatistica);

            repCampiAggiuntivi.DataSource = campiAggiuntivi;
            repCampiAggiuntivi.DataBind();
            repCampiAggiuntivi.Visible = true;

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

                IQueryable<Entities.ElencoCompletoArticoli> queryBase = null;

                if (codiceGruppo.HasValue || codiceCategoria.HasValue || codiceCategoriaStatistica.HasValue)
                {
                    queryBase = from art in llArts.Read(DateTime.Today)
                                join anagraficaArt in llAngraficaArticoli.Read(codiceGruppo, codiceCategoria, codiceCategoriaStatistica) on art.CodiceArticolo equals anagraficaArt.CODICE
                                select art;
                }
                else
                {
                    queryBase = llArts.Read(DateTime.Today);
                }


                if (!string.IsNullOrWhiteSpace(testoRicerca))
                {
                    queryBase = queryBase.Where(x => (x.CodiceArticolo + " - " + x.Descrizione).ToLower().Contains(testoRicerca));
                }

                int itemsPerRequest = (combo.ItemsPerRequest <= 0) ? 20 : combo.ItemsPerRequest;
                int itemOffset = e.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                int numTotaleUtenti = queryBase.Count();

                if (endOffset > numTotaleUtenti)
                    endOffset = numTotaleUtenti;

                IEnumerable<Entities.ElencoCompletoArticoli> entities = queryBase.Skip(itemOffset).Take(itemsPerRequest);

                foreach (Entities.ElencoCompletoArticoli entity in entities)
                {
                    RadComboBoxItem item = new RadComboBoxItem(entity.CodiceArticolo, entity.ID);
                    item.Attributes.Add("DescrizioneArticolo", entity.Descrizione);
                    item.Attributes.Add("NoteArticolo", entity.Note);
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