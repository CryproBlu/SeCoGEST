using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.UI;

namespace SeCoGEST.Helper
{
    public static class Web
    {
        #region Costanti

        /// <summary>
        /// Restituisce il testo da inserire all'elemento vuoto del combo
        /// </summary>
        public const string COMBO_BOX_EMPTY_ITEM_TEXT = "NESSUN VALORE";

        /// <summary>
        /// Restituisce la codifica in html dello spazio
        /// </summary>
        public const string HTML_WHITE_SPACE = "&nbsp;";

        /// <summary>
        /// Restituisce la codifica in html del ritorno a capo
        /// </summary>
        public const string HTML_NEW_LINE = "<br />";

        /// <summary>
        /// Restituisce la codifica del ritorno a capo per la versione plain text dell'email
        /// </summary>
        public const string PLAIN_TEXT_NEW_LINE = "\n";

        #endregion

        #region Funzioni di utilità per le pagine

        /// <summary>
        /// Cerca l'elemento, il cui ID è passato come parametro, in modo ricorsivo nel controllo passato come parametro
        /// </summary>
        /// <param name="control"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T FindControlRecursive<T>(Control control, string id)
            where T : Control
        {
            if (control == null) return null;
            //try to find the control at the current level
            Control ctrl = control.FindControl(id);

            if (ctrl == null)
            {
                //search the children
                foreach (Control child in control.Controls)
                {
                    ctrl = FindControlRecursive<T>(child, id);

                    if (ctrl != null) break;
                }
            }
            return (T)ctrl;
        }


        /// <summary>
        /// Restituisce un valore booleano che indica se la pagina è stata richiamata da un Post o da una chiamata asincrona Ajax
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static bool IsPostOrCallBack(System.Web.UI.Page page)
        {
            if (page.IsPostBack)
                return true;
            if (page.IsCallback)
                return true;

            return false;
        }

        /// <summary>
        /// Indica al browser di ricaricare la pagina passata
        /// </summary>
        /// <param name="pageToReload"></param>
        /// <param name="idEntity"></param>
        public static void ReloadPage(System.Web.UI.Page pageToReload, string idEntity)
        {
            ReloadPage(pageToReload, "ID", idEntity);
        }

        /// <summary>
        /// Indica al browser di ricaricare la pagina passata
        /// </summary>
        /// <param name="pageToReload"></param>
        /// <param name="queryStringIdentifier"></param>
        /// <param name="idEntity"></param>
        public static void ReloadPage(System.Web.UI.Page pageToReload, string queryStringIdentifier, string idEntity)
        {
            if (pageToReload == null) { throw new ArgumentNullException("pageToReload", "Parametro nullo."); }

            if (String.IsNullOrEmpty(queryStringIdentifier))
            {
                queryStringIdentifier = "ID";
            }

            pageToReload.Response.Redirect(string.Format("{0}?{1}={2}", pageToReload.Request.Path, queryStringIdentifier, idEntity.ToString()));
        }

        /// <summary>
        /// Indica al browser di ricaricare la pagina passata
        /// </summary>
        /// <param name="pageToReload"></param>
        /// <param name="idEntity"></param>
        public static void ReloadPage(System.Web.UI.Page pageToReload)
        {
            if (pageToReload == null) { throw new ArgumentNullException("pageToReload", "Parametro nullo."); }

            pageToReload.Response.Redirect(string.Format("{0}", pageToReload.Request.Url.PathAndQuery));
        }


        /// <summary>
        /// Indica al browser di ricaricare la pagina passata specificando l'ID dell'entiti e la Tab selezionata
        /// </summary>
        /// <param name="pageToReload"></param>
        /// <param name="queryStringIdentifier"></param>
        /// <param name="idEntity"></param>
        public static void ReloadPageWithIdAndTab(System.Web.UI.Page pageToReload, string idEntity, int tabIndex)
        {
            if (pageToReload == null) { throw new ArgumentNullException("pageToReload", "Parametro nullo."); }


            pageToReload.Response.Redirect(string.Format("{0}?ID={1}&tab={2}", pageToReload.Request.AppRelativeCurrentExecutionFilePath.ToString(), idEntity, tabIndex), false);
            //pageToReload.Response.Redirect(string.Format("{0}?{1}={2}", pageToReload.Request.Path, queryStringIdentifier, idEntity.ToString()));
        }


        #endregion

        #region Utility in genere

        /// <summary>
        /// Ritorna una stringa contenente l'indirizzo ip del client dell'utente che ha fatto la richiesta web
        /// </summary>
        /// <returns></returns>
        public static string GetClientIpAddress()
        {
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }

        /// <summary>
        /// Ritorna una stringa contenente l'indirizzo ip del server
        /// </summary>
        /// <returns></returns>
        public static string GetServerIpAddress()
        {
            IPHostEntry host;
            IPAddress localIP = null;
            host = Dns.GetHostEntry(Dns.GetHostName());

            localIP = host.AddressList
                .Where(x => x.AddressFamily == AddressFamily.InterNetwork)
                .FirstOrDefault();

            if (localIP != null)
                return localIP.ToString();
            else
                return string.Empty;
        }

        /// <summary>
        /// Restituisce l'username presente nell'HttpContext
        /// </summary>
        public static string GetLoggedUserName()
        {
            if (HttpContext.Current != null &&
                HttpContext.Current.User != null &&
                HttpContext.Current.User.Identity != null &&
                !String.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                return HttpContext.Current.User.Identity.Name;
            }
            else
            {
                return String.Empty;
            }
        }

        #endregion

        #region Funzioni di utilità per gli errori

        /// <summary>
        /// Converte il testo passato in forma HTML
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ReplaceForHTML(string text)
        {
            if (text == null)
            {
                return string.Empty;
            }
            else
            {
                return text.Replace("\\", @"\").
                    Replace("'", @"\'").
                    Replace("\"", @"\'").
                    Replace(((char)13).ToString(), "<br />").
                    Replace(((char)10).ToString(), "");
            }
        }

        /// <summary>
        /// Converte il testo passato in una forma comprensibile a Javascript
        /// </summary>
        /// <param name="text">Testo da tradurre</param>
        /// <returns>Testo tradotto in una forma comprensibile a Javascript</returns>
        public static string ReplaceForJavascript(string text)
        {
            if (text == null)
            {
                return string.Empty;
            }
            else
            {
                return text.Replace("'", @"\'").
                    Replace("\"", @"\'").
                    Replace("<br />", @"\n").
                    Replace("<br/>", @"\n").
                    Replace("<br>", @"\n").
                    Replace(((char)13).ToString(), @"\n").
                    Replace(((char)10).ToString(), "");
            }
        }

        #endregion

        #region Funzioni di utilità di Download files

        /// <summary>
        /// Retituisce il ContentType Header della risposta HTTP in base all'estensione del file passata
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        private static string GetContentTypeByFileExtension(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }

        /// <summary>
        /// Effettua la pulizia del nome passato per passarlo come secondo parametro al metodo 'DownloadFile'
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string ClearFileNameForDownload(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                fileName = String.Empty;
            }
            else
            {
                fileName = fileName.Trim();
            }

            fileName = fileName.Replace(@"\", "_");
            fileName = fileName.Replace("/", "_");
            fileName = fileName.Replace(":", "_");
            fileName = fileName.Replace("*", "_");
            fileName = fileName.Replace("?", "_");
            fileName = fileName.Replace('"', '_');
            fileName = fileName.Replace("<", "_");
            fileName = fileName.Replace(">", "_");
            fileName = fileName.Replace("|", "_");
            fileName = fileName.Replace("_", "_");

            do
            {
                fileName = fileName.Replace("__", "_");
                fileName = fileName.Replace("  ", " ");
            }
            while (fileName.Contains("__") || fileName.Contains("  "));

            return fileName;
        }

        /// <summary>
        /// Esegue il download del file indicato
        /// </summary>
        /// <param name="percorsoFile"></param>
        public static void DownloadFile(string percorsoFile)
        {
            if (!File.Exists(percorsoFile))
                throw new FileNotFoundException("Il file specificato non esiste.", percorsoFile);

            // Ottieni il nome del file
            string fileName = Path.GetFileName(percorsoFile);

            // Leggi il contenuto del file in un array di byte
            byte[] fileContent = File.ReadAllBytes(percorsoFile);

            // Chiama la funzione per effettuare il download
            DownloadAsFile(fileName, fileContent);

            //FileInfo file = new FileInfo(percorsoFile);

            //// Checking if file exists
            //if (file.Exists)
            //{
            //    // Clear the content of the response
            //    HttpContext.Current.Response.ClearContent();

            //    // LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
            //    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + file.Name + "\"");

            //    // Add the file size into the response header
            //    HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());

            //    // Set the ContentType
            //    HttpContext.Current.Response.ContentType = GetContentTypeByFileExtension(file.Extension);

            //    // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
            //    HttpContext.Current.Response.TransmitFile(file.FullName);

            //    // Flush the response to allow temporary file deletion
            //    HttpContext.Current.Response.Flush();

            //    //HttpContext.Current.Response.End() 'Scoperto con un try catch che il metodo andava in exception, ma permetteva lo stesso l'apertura/salvataggio del file
            //    HttpContext.Current.ApplicationInstance.CompleteRequest();
            //}
        }

        /// <summary>
        /// Avvia il download del file indicato 
        /// </summary>
        /// <param name="fileToDownload"></param>
        /// <param name="fileNameForTheUser"></param>
        /// <remarks></remarks>
        [Obsolete]
        public static void DownloadFile(string fileToDownload, string fileNameForTheUser)
        {
            //Invio il file all'utente
            if (fileToDownload == null)
                return;
            if (string.IsNullOrEmpty(fileToDownload.Trim()))
                return;
            if (!System.IO.File.Exists(fileToDownload))
                return;


            System.IO.FileStream MyFileStream = default(System.IO.FileStream);
            long FileSize = 0;

            MyFileStream = new System.IO.FileStream(fileToDownload, System.IO.FileMode.Open);
            FileSize = MyFileStream.Length;

            byte[] BufferX = new byte[Convert.ToInt32(FileSize) + 1];
            MyFileStream.Read(BufferX, 0, Convert.ToInt32(FileSize));
            MyFileStream.Close();

            System.IO.FileInfo fileDownloadInfo = new System.IO.FileInfo(fileToDownload);
            fileNameForTheUser = ClearFileNameForDownload(fileNameForTheUser);
            if (String.IsNullOrEmpty(fileNameForTheUser))
            {
                fileNameForTheUser = fileDownloadInfo.Name.Replace(fileDownloadInfo.Extension, String.Empty);
            }

            HttpContext.Current.Response.ContentType = GetContentTypeByFileExtension(fileDownloadInfo.Extension);
            HttpContext.Current.Response.AddHeader("Content-Disposition", String.Format("attachment; filename=\"{0}{1}\"", fileNameForTheUser, fileDownloadInfo.Extension));
            HttpContext.Current.Response.BinaryWrite(BufferX);
            //HttpContext.Current.Response.End() 'Scoperto con un try catch che il metodo andava in exception, ma permetteva lo stesso l'apertura/salvataggio del file
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Avvia il download dei dati indicati sotto forma di file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="textString"></param>
        /// <param name="encoding"></param>
        [Obsolete]
        public static void DownloadAsFile(string fileName, string textString, System.Text.Encoding encoding)
        {
            if (textString == null)
                return;
            if (string.IsNullOrEmpty(textString.Trim()))
                return;

            long FileSize = 0;
            FileSize = textString.Trim().Length;

            byte[] BufferX = new byte[Convert.ToInt32(FileSize) + 1];
            BufferX = encoding.GetBytes(textString);

            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            System.Web.HttpContext.Current.Response.BinaryWrite(BufferX);
            //HttpContext.Current.Response.End() 'Scoperto con un try catch che il metodo andava in exception, ma permetteva lo stesso l'apertura/salvataggio del file
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Avvia il download dei dati indicati sotto forma di file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileContent"></param>
        public static void DownloadAsFile(string fileName, byte[] fileContent)
        {
            if (fileContent == null)
                return;
            if (fileContent.Length == 0)
                return;

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ContentType = GetContentTypeByFileExtension(System.IO.Path.GetExtension(fileName));
            HttpContext.Current.Response.AddHeader("Content-Length", fileContent.Length.ToString());
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            HttpContext.Current.Response.BinaryWrite(fileContent);
            //HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End(); //'Scoperto con un try catch che il metodo andava in exception, ma permetteva lo stesso l'apertura/salvataggio del file
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        #endregion
    }
}
