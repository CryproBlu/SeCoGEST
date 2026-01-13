using System;
using SeCoGEST.Entities;
using Telerik.Web.UI;

namespace SeCoGEST.Web.Interventi
{
    public partial class ArticoliIntervento : System.Web.UI.UserControl
    {
        #region Chiavi e Costanti

        protected const string NOME_FUNZIONE_MOSTRA_FINESTRA_DETTGALIO = "MostraFinestraDettaglioArticolo";

        protected const string NOME_FUNZIONE_CHIUSURA_FINESTRA_DETTGALIO_REBIND_GRIGLIA = "ChiudiFinestraDettaglioArticoloAndRebind";

        private int rowindex = -1;

        #endregion

        #region Properties

        /// <summary>
        /// Id dell'Intervento al quale sono legati gli articoli in gestione
        /// </summary>
        public EntityId<Entities.Intervento> IDIntervento
        {
            get
            {
                if (ViewState["IDIntervento"] != null)
                {
                    return new EntityId<Entities.Intervento>(ViewState["IDIntervento"]);
                }
                else
                {
                    return EntityId<Entities.Intervento>.Empty;
                }
            }
            set
            {
                if (value.HasValue)
                {
                    ViewState["IDIntervento"] = value.Value;
                }
                else
                {
                    ViewState["IDIntervento"] = null;
                }
            }
        }

        /// <summary>
        /// Codice del Cliente al quale si riferiscono gli abbonamenti analizzati dallo User Control
        /// </summary>
        public string CodiceCliente
        {
            get
            {
                if (ViewState["CodiceCliente"] != null)
                {
                    return ViewState["CodiceCliente"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["CodiceCliente"] = value;
            }
        }

        /// <summary>
        /// Data dell'Intervento utilizzata per filtrare l'elenco degli articoli disponibili
        /// </summary>
        public DateTime DataIntervento
        {
            get
            {
                if (ViewState["DataIntervento"] != null)
                {
                    return (DateTime)ViewState["DataIntervento"];
                }
                else
                {
                    return DateTime.Now;
                }
            }
            set
            {
                ViewState["DataIntervento"] = value;
            }
        }



        /// <summary>
        /// Imposta o restituisce un valore booleano che indica se l'oggetto è abilitato o meno
        /// </summary>
        public bool Enabled
        {
            get
            {
                if (ViewState["Enabled"] == null)
                {
                    return true;
                }
                else
                {
                    return (bool)ViewState["Enabled"];
                }
            }
            set
            {
                ViewState["Enabled"] = value;

                // Attivo/Disattivo i controlli della griglia in base al valore di Enabled
                rgGriglia.MasterTableView.CommandItemDisplay = value ? GridCommandItemDisplay.Top : GridCommandItemDisplay.None;

                if (rgGriglia != null && rgGriglia.Columns.Count > 0)
                {
                    rgGriglia.Columns[0].Visible = value; // Colonna Modifica
                    rgGriglia.Columns[rgGriglia.Columns.Count - 1].Visible = value; // Colonna Elimina
                }
            }
        }

        #endregion

        #region Gestione eventi griglia
        protected void rgGriglia_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (this.IDIntervento.HasValue)
            {
                Logic.Intervento_Articoli llA = new Logic.Intervento_Articoli();
                rgGriglia.DataSource = llA.Read(this.IDIntervento);
            }
            else
            {
                rgGriglia.DataSource = null;
            }
        }
        
        protected void rgGriglia_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e == null || String.IsNullOrEmpty(e.CommandName)) return;

            if (e.CommandName == "ModificaCommand")
            {
                GridDataItem item = (GridDataItem)e.Item;
                rowindex = item.ItemIndex;

                string idValue = item.GetDataKeyValue("ID").ToString();

                Logic.Intervento_Articoli llII = new Logic.Intervento_Articoli();
                ucArticoliInterventoEdit.CodiceCliente = this.CodiceCliente;
                ucArticoliInterventoEdit.DataIntervento = this.DataIntervento;
                ucArticoliInterventoEdit.ShowData(llII.Find(new Entities.EntityId<Entities.Intervento_Articolo>(idValue)));

                rwArticoliInterventoEdit.Title = "Modifica articolo";

                MasterPageHelper.GetRadAjaxManager(Page).ResponseScripts.Add(String.Format("{0}();", NOME_FUNZIONE_MOSTRA_FINESTRA_DETTGALIO));
                e.Canceled = true;
            }
            else if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                rwArticoliInterventoEdit.Title = "Inserimento nuovo articolo";
                ucArticoliInterventoEdit.CodiceCliente = this.CodiceCliente;
                ucArticoliInterventoEdit.DataIntervento = this.DataIntervento;
                ucArticoliInterventoEdit.InitializeForInsert(IDIntervento.Value);

                MasterPageHelper.GetRadAjaxManager(Page).ResponseScripts.Add(String.Format("{0}();", NOME_FUNZIONE_MOSTRA_FINESTRA_DETTGALIO));

                e.Item.OwnerTableView.IsItemInserted = true;
                e.Item.Edit = false;
                rgGriglia.EditIndexes.Clear();
                e.Canceled = true;
            }
            else if (e.CommandName == RadGrid.DeleteCommandName)
            {
                try
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    rowindex = item.ItemIndex;

                    string idValue = item.GetDataKeyValue("ID").ToString();
                    Guid id;
                    if (Guid.TryParse(idValue , out id))
                    {
                        Logic.Intervento_Articoli llI = new Logic.Intervento_Articoli();
                        Entities.Intervento_Articolo art = llI.Find(new EntityId<Intervento_Articolo>(id));
                        if (art != null) llI.Delete(art, true);
                    }
                }
                catch (Exception ex)
                {
                    e.Canceled = true;
                    // Mostro il messaggio d'errore all'utente
                    MessageHelper.ShowErrorMessage(this.Page, ex.Message);
                }
            }
        }

        #endregion

        #region Gestione eventi oggetto di edit

        /// <summary>
        /// Metodo di gestione dell'evento di salvataggio dell'articolo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucArticoliInterventoEdit_OnSaved(object sender, EventArgs e)
        {
            MasterPageHelper.GetRadAjaxManager(Page).ResponseScripts.Add(String.Format("{0}();", NOME_FUNZIONE_CHIUSURA_FINESTRA_DETTGALIO_REBIND_GRIGLIA));
        }

        /// <summary>
        /// Metodo di gestione dell'evento di annullamento della modifica o inserimento di un articolo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucArticoliInterventoEdit_OnCanceled(object sender, EventArgs e)
        {
            MasterPageHelper.GetRadAjaxManager(Page).ResponseScripts.Add(String.Format("{0}();", NOME_FUNZIONE_CHIUSURA_FINESTRA_DETTGALIO_REBIND_GRIGLIA));
        }

        #endregion
    }
}