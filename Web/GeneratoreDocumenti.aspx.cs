using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SeCoGEST.Web
{
    public partial class GeneratoreDocumenti : System.Web.UI.Page
    {
        #region Properties

        /// <summary>
        /// Restituisce l'ID del documento passato tramite QueryString 
        /// </summary>
        private Guid? IDDocumentoDaGenerare
        {
            get
            {
                Guid? valueToReturn = null;

                if (Request.QueryString[GeneratoreDocumentiHelper.NOME_PARAMETRO_IDENTIFICATORE_ENTITY] != String.Empty)
                {
                    Guid id;
                    if (Guid.TryParse(Request.QueryString[GeneratoreDocumentiHelper.NOME_PARAMETRO_IDENTIFICATORE_ENTITY], out id))
                    {
                        valueToReturn = id;
                    }
                }

                return valueToReturn;
            }
        }

        /// <summary>
        /// Restituisce la tipologia del documento da generare
        /// </summary>
        private TipologiaDocumentiDaGenerareEnum? TipoDocumentoDaGenerare
        {
            get
            {
                TipologiaDocumentiDaGenerareEnum? valueToReturn = null;

                if (Request.QueryString[GeneratoreDocumentiHelper.NOME_PARAMETRO_TIPO_DOCUMENTO] != String.Empty)
                {
                    int tipoDocumento;
                    if (Int32.TryParse(Request.QueryString[GeneratoreDocumentiHelper.NOME_PARAMETRO_TIPO_DOCUMENTO], out tipoDocumento))
                    {
                        valueToReturn = (TipologiaDocumentiDaGenerareEnum)tipoDocumento;
                    }
                }

                return valueToReturn;
            }
        }

        /// <summary>
        /// Restituisce il nome del documento passato tramite QueryString. Il nome viene passato perchè l'entity Documento non contiene informazioni sul nome del file.
        /// </summary>
        private string NomeDocumentoPerDownload
        {
            get
            {
                string valueToReturn = String.Empty;
                if (!String.IsNullOrEmpty(Request.QueryString[GeneratoreDocumentiHelper.NOME_PARAMETRO_NOME_DOCUMENTO_PER_DOWNLOAD])) 
                {
                    valueToReturn = Request.QueryString[GeneratoreDocumentiHelper.NOME_PARAMETRO_NOME_DOCUMENTO_PER_DOWNLOAD].Trim();
                } 
                else if (IDDocumentoDaGenerare.HasValue)
                {
                    valueToReturn = IDDocumentoDaGenerare.Value.ToString();
                }
                else
                {
                    valueToReturn = Guid.NewGuid().ToString();
                }

                return valueToReturn;
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
                SeCoGes.Utilities.MessagesCollector errori = ControllaParametri();
                if (errori.HaveMessages)
                {
                    throw new Exception(errori.ToString(Helper.HtmlEnvironment.NewLine));
                }

                string percorsoFileGenerato = GeneraDocumento();

                if (Logic.GestoreDocumenti.FileGeneratoEsiste(percorsoFileGenerato, false))
                {
                    // Viene mandato in download il file
                    Helper.Web.DownloadFile(percorsoFileGenerato);
                }
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);
                Response.Write("Si è verificato il seguente errore: <br />" + ex.Message);
            }
        }

        /// <summary>
        /// Effettua il controllo dei parametri passati alla querystring
        /// </summary>
        /// <returns></returns>
        private SeCoGes.Utilities.MessagesCollector ControllaParametri()
        {
            SeCoGes.Utilities.MessagesCollector errori = new SeCoGes.Utilities.MessagesCollector();

            if (!IDDocumentoDaGenerare.HasValue)
            {
                errori.Add("L'indentificativo del documento da generare non è valorizzato");
            }

            if (!TipoDocumentoDaGenerare.HasValue)
            {
                errori.Add("La tipologia del documento da generare non è valorizzata");
            }

            return errori;
        }

        /// <summary>
        /// Effettua la generazione del documento in base ai parametri nella querystring
        /// </summary>
        /// <returns></returns>
        private string GeneraDocumento()
        {
            string percorsoFileGenerato = String.Empty;

            switch (TipoDocumentoDaGenerare.Value)
            {
                case TipologiaDocumentiDaGenerareEnum.Intervento:
                    percorsoFileGenerato = Logic.GestoreDocumenti.GeneraIntervento(new Entities.EntityId<Entities.Intervento>(IDDocumentoDaGenerare));
                    break;

                default:
                    throw new Exception("La tipologia di documento passata non è gestita");
            }

            return percorsoFileGenerato;
        }

        #endregion
    }
}