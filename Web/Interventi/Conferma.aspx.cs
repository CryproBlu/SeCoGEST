using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SeCoGEST.Web.Interventi
{
    public partial class Conferma : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            string id = Request.QueryString["id"];
            string motivo = Request.QueryString["m"];

            if(Guid.TryParse(id, out Guid gid))
            {
                if(motivo == "0")
                {
                    lblMessaggio.Text = "Il ticket di assistenza è stato inviato correttamente al team di supporto che ti risponderà il prima possibile!";
                    hlPageLink.Text = "Visualizza Ticket";
                    hlPageLink.NavigateUrl = $"/Interventi/Ticket.aspx?ID={gid}";
                }
            }
        }
    }
}