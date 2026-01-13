using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using SeCoGes.Logging;
using SeCoGEST.Helper;
using SeCoGEST.Infrastructure;
using SeCoGEST.Web.LongProcesses;

namespace SeCoGEST.Web
{
    public class Global : System.Web.HttpApplication
    {
        #region Intercettazione Eventi

        protected void Application_Start(object sender, EventArgs e)
        {
                        Telerik.Reporting.Services.WebApi.ReportsControllerConfiguration.RegisterRoutes(System.Web.Http.GlobalConfiguration.Configuration);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }
        
        protected void Application_Error(object sender, EventArgs e)
        {
            // Effettua l'invio di un'email contenente l'errore completo generato nell'applicazione
            //EmailManager.InviaEmailAvvisoErrore(Helper.Web.GetLoggedUserName(), Server.GetLastError().GetBaseException(), Request.Url.ToString(), Request.QueryString.ToString());
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Elimina tutte le cartelle ed i files temporanei creati dall'applicazione
        /// </summary>
        private void EliminaFilesTemporanei()
        {
            try
            {
                // Definizioni directory temporanea
                String percorsoTemporaneo = ConfigurationKeys.PERCORSO_TEMPORANEO;

                DirectoryInfo percorsoTemporaneoInfo = new DirectoryInfo(percorsoTemporaneo);
                if (percorsoTemporaneoInfo.Exists)
                {
                    FileInfo[]  files = percorsoTemporaneoInfo.GetFiles();
                    if (files != null && files.Length > 0)
                    {
                        FileHelper.EliminaElencoFile(files);
                    }

                    foreach (DirectoryInfo dir in percorsoTemporaneoInfo.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.AddLogErrori(ex);
            }
        }
        
        #endregion

        // Ver DEV
    }
}