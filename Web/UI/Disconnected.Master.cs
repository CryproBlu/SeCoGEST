using System;

namespace SeCoGEST.Web.UI
{
    public partial class Disconnected : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Gestione Ticketing";
            if (!Helper.Web.IsPostOrCallBack(this.Page))
            {
                lblVersione.Text = String.Concat("Ver: ", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            }
        }
    }
}