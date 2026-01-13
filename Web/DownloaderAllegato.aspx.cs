using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SeCoGes.Utilities;
using SeCoGEST.Infrastructure;

namespace SeCoGEST.Web
{
    public partial class DownloaderAllegato : System.Web.UI.Page
    {
        #region Properties

        /// <summary>
        /// Restituisce l'ID del documento passato tramite QueryString 
        /// </summary>
        private Guid IDDocumento
        {
            get
            {
                if (Request.QueryString["ID"] != string.Empty)
                {
                    Guid id;
                    if (Guid.TryParse(Request.QueryString["ID"], out id))
                    {
                        return id;
                    }
                    else
                    {
                        return Guid.Empty;
                    }
                }
                else
                {
                    return Guid.Empty;
                }
            }
        }

        /// <summary>
        /// Restituisce il nome del documento passato tramite QueryString. Il nome viene passato perchè l'entity Documento non contiene informazioni sul nome del file.
        /// </summary>
        private string NomeDocumento
        {
            get
            {
                return (!String.IsNullOrEmpty(Request.QueryString["Name"]) ? Request.QueryString["Name"].ToTrimmedString() : IDDocumento.ToString());
            }
        }

        #endregion

        #region Intercettazione Eventi

        /// <summary>
        /// Metodo di gestione dell'evento Load della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Helper.Web.IsPostOrCallBack(this))
                SeCoGes.Logging.LogManager.AddLogAccessi(String.Format("Accesso alla pagina '{0}'.", Request.Url.AbsolutePath));

            StartDownload();
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Effettua il download del documento ID uguale a quello passato come parametro nella query string
        /// </summary>
        private void StartDownload()
        {
            try
            {
                Logic.Allegati llDocumenti = new Logic.Allegati();
                Entities.Allegato entityAllegata = llDocumenti.Find(IDDocumento);
                if (entityAllegata != null)
                {
                    Helper.Web.DownloadAsFile(NomeDocumento, entityAllegata.FileAllegato.ToArray());
                    //string filePath = Path.Combine(ConfigurationKeys.PERCORSO_TEMPORANEO, NomeDocumento);
                    //File.WriteAllBytes(filePath, entityAllegata.FileAllegato.ToArray());
                    //if (File.Exists(filePath))
                    //{
                    //    Helper.Web.DownloadFile(filePath);
                    //}
                    //else
                    //{
                    //    throw new Exception(String.Format("Non è stato possibile recuperare il file '{0}' generato.", NomeDocumento));
                    //}
                }
                else
                {
                    Response.Write("Il documento richiesto non è stato trovato.");
                }
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);
                Response.Write("Si è verificato il seguente errore: <br />" + ex.Message);
            }
        }

        #endregion
    }
}