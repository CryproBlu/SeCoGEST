using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeCoGEST.Web
{
    public static class WebHelper
    {
        /// <summary>
        /// Effettua il ricaricamento della pagina corrente
        /// </summary>
        public static void ReloadPage()
        {
            if (HttpContext.Current == null) throw new Exception("Il ricaricamento della pagina non può essere effettuato perchè manca il contesto http");

            HttpContext contestoHttp = HttpContext.Current;

            contestoHttp.Response.Redirect(contestoHttp.Request.RawUrl);
        }
    }
}