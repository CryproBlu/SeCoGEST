using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SeCoGEST.Helper
{
    public static class RadEditorHelper
    {
        #region Metodi Pubblici

        /// <summary>
        /// Effettua il parsing dell'html passato come parametro in modo da avere le immagini in formato base64
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ParseHtmlToImageSource(string html)
        {
            try
            {
                string basePath = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                string modifiedHTML = html;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                var srcTags = doc.DocumentNode.SelectNodes("//img");
                if (srcTags != null)
                {
                    foreach (var item in doc.DocumentNode.SelectNodes("//img[@src]"))//select only those img that have a src attribute..ahh not required to do [@src] i guess
                    {
                        string value = item.Attributes["src"].Value.ToString();

                        if (!(value.Contains("data:image/") && value.IndexOf("base64") > 0))
                        {
                            //var width = item.Attributes["width"].Value.Replace("px", "");
                            //var height = item.Attributes["height"].Value.Replace("px", ""); 

                            string extension = Path.GetExtension(item.Attributes["src"].Value.ToString().ToLower());

                            if (extension.Contains("."))
                            {
                                extension = extension.Remove(extension.IndexOf("."), extension.IndexOf(".") + 1);
                            }

                            item.Attributes["src"].Value = MakeImageSrcData(basePath + item.Attributes["src"].Value.ToString(), extension);

                            modifiedHTML = modifiedHTML.Replace(value, item.Attributes["src"].Value.ToString());
                        }
                    }
                }

                //doc.ToString();

                //doc.Save("yourFile");//dont forget to save

                return modifiedHTML;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Effettua la rimozione di un tasto in base ad un nome, dall'editor passato come parametro
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="name"></param>
        public static void RemoveButton(Telerik.Web.UI.RadEditor editor, string name)
        {
            if (editor == null) throw new ArgumentNullException("editor", "Parametro nullo");

            foreach (Telerik.Web.UI.EditorToolGroup group in editor.Tools)
            {
                Telerik.Web.UI.EditorTool tool = group.FindTool(name);
                if (tool != null)
                {
                    group.Tools.Remove(tool);
                }
            }
        } 

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Effettua il download dell'immagine restituendone la sua forma in base64
        /// </summary>
        /// <param name="url"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        private static string MakeImageSrcData(string url, string extension)
        {
            try
            {
                WebResponse result = null;
                WebRequest request = WebRequest.Create(url);

                // Get the content
                result = request.GetResponse();

                Stream rStream = result.GetResponseStream();
                byte[] rBytes = ReadFully(rStream);

                return "data:image/" + extension + ";base64," + Convert.ToBase64String(rBytes, Base64FormattingOptions.None);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Restituisce l'elenco dell'array di byte presenti nello stream
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static byte[] ReadFully(Stream input)
        {
            try
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
