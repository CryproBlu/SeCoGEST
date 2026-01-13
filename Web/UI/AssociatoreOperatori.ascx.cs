using SeCoGEST.Entities;
using SeCoGEST.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SeCoGEST.Web.UI
{
    public class AssociaOperatoriEventArgs : EventArgs 
    {
        public Entities.Operatore Operatore { get; set; }
    }

    public delegate void LeggiOperatoriEventHandler(object s, EventArgs e);
    public delegate void AssociaOperatoriEventHandler(object s, AssociaOperatoriEventArgs e);
    public delegate void RimuoviOperatoriEventHandler(object s, AssociaOperatoriEventArgs e);

    public partial class AssociatoreOperatori : System.Web.UI.UserControl
    {
        public event LeggiOperatoriEventHandler LeggiOperatori;
        public event AssociaOperatoriEventHandler AssociaOperatore;
        public event RimuoviOperatoriEventHandler RimuoviOperatore;

        public bool AllowAdd
        {
            get
            {
                if (ViewState["AllowAdd"] == null)
                    return true;
                else
                    return (bool)ViewState["AllowAdd"];
            }
            set
            {
                ViewState["AllowAdd"] = value;
            }
        }

        public bool AllowDelete
        {
            get
            {
                if (ViewState["AllowDelete"] == null)
                    return true;
                else
                    return (bool)ViewState["AllowDelete"];
            }
            set
            {
                ViewState["AllowDelete"] = value;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if(!this.Page.IsPostBack && !this.Page.IsCallback)
            {
                FillArchive();
                Refresh();
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            tbAddNewOperator.Visible = AllowAdd;
            rgItems.Columns[1].Visible = AllowDelete;
            
        }

        private void FillArchive()
        {
            Logic.Operatori llO = new Logic.Operatori();
            rcbNuovoOperatore.DataSource = llO.ReadActive();
            rcbNuovoOperatore.DataBind();
            TelerikHelper.InsertBlankComboBoxItem(rcbNuovoOperatore);
        }

        public void SetDataSource(List<Entities.Operatore> listaOperatori)
        {            
            rgItems.DataSource = listaOperatori;
        }

        public void Refresh()
        {
            if (this.LeggiOperatori != null) this.LeggiOperatori(this, null);
            rgItems.DataBind();
        }

        protected void rgItems_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (this.LeggiOperatori != null) this.LeggiOperatori(this, null);
        }

        protected void rgItems_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (this.RimuoviOperatore != null)
            {
                try
                {
                    GridDataItem editedItem = e.Item as GridDataItem;
                    if (Guid.TryParse(editedItem.GetDataKeyValue("ID").ToString(), out Guid itemId))
                    {
                        Logic.Operatori llArc = new Logic.Operatori();
                        Entities.Operatore archiveItem = llArc.Find(new EntityId<Entities.Operatore>(itemId));
                        if (archiveItem != null)
                        {
                            AssociaOperatoriEventArgs arg = new AssociaOperatoriEventArgs();
                            arg.Operatore = archiveItem;
                            this.RimuoviOperatore(this, arg);
                        }
                        else
                        {
                            string message = "radalert('Operatore da rimuovere non trovato.', 330, 210, 'Errore');";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", message, true);
                            e.Canceled = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = $"radalert('Si è verificato un errore: {ex.Message.Replace("'", "")}', 330, 210 'Errore');";
                    errorMessage = errorMessage.Replace("\n", "");
                    errorMessage = errorMessage.Replace("\r", "");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", errorMessage, true);
                    e.Canceled = true;
                }
            }
        }

        protected void imgButtonNewOperator_Click(object sender, ImageClickEventArgs e)
        {
            if(rcbNuovoOperatore.SelectedValue != string.Empty)
            {
                if(Guid.TryParse(rcbNuovoOperatore.SelectedValue, out Guid idO))
                {
                    Logic.Operatori llArc = new Logic.Operatori();
                    Entities.Operatore op = llArc.Find(new EntityId<Entities.Operatore>(idO));
                    if (this.AssociaOperatore != null)
                    {
                        AssociaOperatoriEventArgs arg = new AssociaOperatoriEventArgs();
                        arg.Operatore = op;
                        this.AssociaOperatore(this, arg);
                    }
                }
            }
        }
    }
}