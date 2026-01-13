using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SeCoGEST.Web.Offerte
{
    public partial class SegnalibriGenerazioneOfferta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack || !IsCallback)
            {
                rptTabellaSegnalibri.DataSource = Logic.DocumentiDaGenerare.GeneratoreOfferte.GetBookmarksInfo();
                rptTabellaSegnalibri.DataBind();
            }
        }
    }
}