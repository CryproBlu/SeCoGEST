using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using msWord = Microsoft.Office.Interop.Word;

namespace SeCoGEST.Helper
{
    public static class WordHelper
    {
        #region Costanti

        /// <summary>
        /// Restituisce il formato currency senza il simbolo del dollaro
        /// </summary>
        public const string CURRENCY_WITHOUT_SIMBOL_FORMAT_STRING = "{0:#,##0.00}";

        /// <summary>
        /// Restituisce il formato del tempo in ore il cui valore è memorizzato nel db come decimal
        /// </summary>
        public const string DECIMAL_TIME_FORMAT_STRING = "{0:#,##0}";

        /// <summary>
        /// Restituisce il formato relativo alla data sottoforma di giorno/mese/anno
        /// </summary>
        public const string DATE_FORMAT_STRING = "{0:dd/MM/yyyy}";

        /// <summary>
        /// Restituisce il nome del font wingdings utilizzato per inserire i checkbox nei bookmark
        /// </summary>
        public const string FONT_WINGDINGS = "Wingdings";

        /// <summary>
        /// Numero del carattere da utilizzare per inserire il checkbox selezionato nei bookmark
        /// </summary>
        public const int WINGDINGS_SELECTED_FLAG_CHARACTER_NUMBER = 254;

        /// <summary>
        /// Numero del carattere da utilizzare per inserire il checkbox NON selezionato nei bookmark
        /// </summary>
        public const int WINGDINGS_UNSELECTED_FLAG_CHARACTER_NUMBER = 168;

        #endregion

        #region Proprietà

        /// <summary>
        /// Restituisce il formato utilizzato per la formattazione dei valori nel bookmark
        /// </summary>
        public static CultureInfo FormatCulture
        {
            get
            {
                return new CultureInfo("IT-it");
            }
        }

        #endregion

        #region Metodi

        /// <summary>
        /// Effettua l'inizializzazione dell'applicazione di word
        /// </summary>
        /// <param name="wordTemplatepath"></param>
        /// <param name="msDocument"></param>
        /// <returns></returns>
        public static msWord.Application InizializeApplication()
        {
            msWord.Application msWordApplication = new msWord.Application();
            msWordApplication.Visible = false;  // Mettere a "TRUE" se si vuole vedere in tempo reale la generazione del file.

            return msWordApplication;
        }

        /// <summary>
        /// Aggiunge all'istanza di word passata come parametro il modello recuperato dal percordo passato come parametro
        /// </summary>
        /// <param name="msWordApplication"></param>
        /// <param name="wordTemplatepath"></param>
        /// <returns></returns>
        public static msWord.Document AddDocument(msWord.Application msWordApplication, string wordTemplatepath)
        {
            if (msWordApplication == null)
            {
                throw new Exception("L'istanza di Word passata è nulla.");
            }

            if (String.IsNullOrEmpty(wordTemplatepath))
            {
                throw new Exception("Il percorso del modello non è stato valorizzato.");
            }
            else if (!File.Exists(wordTemplatepath))
            {
                throw new Exception("Il modello non esiste.");
            }

            msWord.Document msWordDoc = msWordApplication.Documents.Add(wordTemplatepath);
            msWordDoc.Activate();
            return msWordDoc;
        }

        /// <summary>
        /// Effettua il salvataggio del documento passato come parametro
        /// </summary>
        /// <param name="msWordDoc"></param>
        /// <param name="fullFilePath">Directory in cui dev'essere effettuato il salvataggio ed il nome del file</param>
        /// <param name="saveFormatFile"></param>
        public static void SaveDocument(msWord.Document msWordDoc, string fullFilePath, msWord.WdSaveFormat saveFormatFile = msWord.WdSaveFormat.wdFormatDocument97)
        {
            if (msWordDoc == null)
            {
                throw new ArgumentNullException("L'istanza del documento Word passata è nulla.", "msWordDoc");
            }
            else if (String.IsNullOrEmpty(fullFilePath))
            {
                throw new ArgumentNullException("Non è stato indicato il percorso completo del file.", "fullFilePath");
            }

            object objWordDocumentPath = fullFilePath;
            msWordDoc.SaveAs(objWordDocumentPath, saveFormatFile);
        }

        #region BookMarks

        /// <summary>
        /// Recupera tutti i bookmark presenti nel documento word
        /// </summary>
        /// <param name="msWordDoc"></param>
        /// <returns>IDictionary<nome del bookmark, msWord.Range del bookmark></returns>
        public static IDictionary<string, msWord.Range> GetBookmarks(msWord.Document msWordDoc)
        {
            IDictionary<string, msWord.Range> bookMarkRecuperati = new Dictionary<string, msWord.Range>();

            if (msWordDoc == null)
                return bookMarkRecuperati;

            foreach (msWord.Bookmark d in msWordDoc.Bookmarks)
            {
                bookMarkRecuperati.Add(d.Name, d.Range);
            }

            return bookMarkRecuperati;
        }

        /// <summary>
        /// Indica se esiste un bookmark
        /// </summary>
        /// <param name="msWordDoc">modello di word aperto</param>
        /// <param name="nomeBookMark">nome del bookmark</param>
        /// <returns></returns>
        public static bool BookmarkExist(msWord.Document msWordDoc, string nomeBookMark)
        {
            return msWordDoc.Bookmarks.Exists(nomeBookMark);
        }

        /// <summary>
        /// Restiuisce il range del bookmark il cui nome viene passato come parametro
        /// </summary>
        /// <param name="msWordDoc">modello di word aperto</param>
        /// <param name="nomeBookMark">nome del bookmark</param>
        /// <returns></returns>
        public static msWord.Range GetBookmarkRange(msWord.Document msWordDoc, string nomeBookMark)
        {
            if (msWordDoc.Bookmarks.Exists(nomeBookMark))
            {
                return msWordDoc.Bookmarks[nomeBookMark].Range;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Effettua il settaggio di un bookmark
        /// </summary>
        /// <param name="msWordDoc"></param>
        /// <param name="nomeBookMark">nome del bookmark</param>
        /// <param name="valoreBookMark">valore da fornire al bookmark</param>
        /// <returns>fornisce un boolea</returns>
        public static void SettaBookMark(msWord.Document msWordDoc,
                                        string nomeBookMark,
                                        object valoreBookMark)
        {
            if (msWordDoc == null) return;

            if (BookmarkExist(msWordDoc, nomeBookMark))
            {
                msWordDoc.Bookmarks[nomeBookMark].Range.Text = ((valoreBookMark != null) ? valoreBookMark.ToString() : String.Empty);
            }
        }

        /// <summary>
        /// Setta i bookmark con font Wingdings tramite valori booleani, true per visualizzare la casella (carattere 168) checkata (carattere 254)
        /// </summary>
        /// <param name="nomeBookMark"></param>
        /// <param name="valoreBooleano"></param>
        /// <param name="managerReport">istanza del manager report</param>
        public static void SettaBookMarkWingdingsConValoreBooleano(msWord.Document msWordDoc, string nomeBookMark, bool? valoreBooleano)
        {
            if (msWordDoc == null) return;

            if (BookmarkExist(msWordDoc, nomeBookMark))
            {
                msWord.Range bookmark = msWordDoc.Bookmarks[nomeBookMark].Range;

                if (valoreBooleano.HasValue && valoreBooleano.Value)
                {
                    /// Effettua l'inserimento di un simbolo , 
                    /// in questo caso il simbolo ha un font Wingdings,
                    /// all'interno del bookmark
                    bookmark.InsertSymbol(Font: FONT_WINGDINGS, CharacterNumber: WINGDINGS_SELECTED_FLAG_CHARACTER_NUMBER, Unicode: true);
                }
                else
                {
                    bookmark.InsertSymbol(Font: FONT_WINGDINGS, CharacterNumber: WINGDINGS_UNSELECTED_FLAG_CHARACTER_NUMBER, Unicode: true);
                }
            }
        }

        /// <summary>
        /// Setta i bookmark con font Wingdings tramite valori booleani, true per visualizzare la casella (carattere 168) checkata (carattere 254) e testo
        /// </summary>
        /// <param name="nomeBookMark"></param>
        /// <param name="valoreBooleano"></param>
        /// <param name="managerReport">istanza del manager report</param>
        public static void SettaBookMarkWingdingsConValoreBooleanoETesto(msWord.Document msWordDoc, string nomeBookMark, bool? valoreBooleano, string testo)
        {
            if (msWordDoc == null) return;

            if (BookmarkExist(msWordDoc, nomeBookMark))
            {
                SettaBookMarkWingdingsConValoreBooleano(msWordDoc, nomeBookMark, valoreBooleano);
                msWord.Range bookmark = msWordDoc.Bookmarks[nomeBookMark].Range;
                bookmark.InsertAfter(testo);
            }
        }

        /// <summary>
        /// Setta i bookmark che si aspettano un data
        /// </summary>
        /// <param name="msWordDoc"></param>
        /// <param name="nomeBookMark"></param>
        /// <param name="valoreDateTime"></param>
        /// <param name="dateFormat"></param>
        public static void SettaBookMarkConValoreDateTimeNullabile(msWord.Document msWordDoc, string nomeBookMark, DateTime? valoreDateTime, string dateFormat = DATE_FORMAT_STRING)
        {
            if (msWordDoc == null) return;

            if (valoreDateTime.HasValue)
            {
                string stringFormat = (String.IsNullOrWhiteSpace(dateFormat) ? DATE_FORMAT_STRING : dateFormat);
                SettaBookMark(msWordDoc, nomeBookMark, String.Format(stringFormat, valoreDateTime.Value));
            }
            else
            {
                SettaBookMark(msWordDoc, nomeBookMark, null);
            }
        }

        /// <summary>
        /// Setta i bookmark che si aspettano un decimal
        /// </summary>
        /// <param name="msWordDoc"></param>
        /// <param name="nomeBookMark"></param>
        /// <param name="valoreDecimal"></param>
        /// <param name="decimalFormat"></param>
        public static void SettaBookMarkConValoreDecimalNullabile(msWord.Document msWordDoc, string nomeBookMark, decimal? valoreDecimal, string decimalFormat = CURRENCY_WITHOUT_SIMBOL_FORMAT_STRING)
        {
            if (msWordDoc == null) return;

            if (valoreDecimal.HasValue)
            {
                SettaBookMark(msWordDoc, nomeBookMark, String.Format(FormatCulture, (String.IsNullOrWhiteSpace(decimalFormat) ? CURRENCY_WITHOUT_SIMBOL_FORMAT_STRING : decimalFormat), valoreDecimal.Value));
            }
            else
            {
                SettaBookMark(msWordDoc, nomeBookMark, null);
            }
        }

        #endregion

        #region Rilascio Oggetti

        /// <summary>
        /// Effettua il rilascio delle risorse degli oggetti COM
        /// </summary>
        /// <param name="o"></param>
        public static void RilascioOggettiCOM(object o)
        {
            try
            {
                if (o == null || o != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
                }
            }
            catch
            {
            }
            finally
            {
                o = null;
            }
        }

        /// <summary>
        /// Effettua il rilascio di tutti gli oggetti
        /// </summary>
        /// <param name="msWordApp"></param>
        /// <param name="msWordDoc"></param>
        /// <param name="msWordTemplate"></param>
        public static void RilascioRisorseWord(msWord.Application msWordApp, msWord.Document msWordDoc, ref object msWordTemplate)
        {
            ///rilascio gli oggetti

            if (msWordDoc != null)
            {
                msWordDoc.Close(null, null, null);
                RilascioOggettiCOM(msWordDoc);
            }

            if (msWordApp != null)
            {
                msWordApp.Application.Quit(null, null, null);
                RilascioOggettiCOM(msWordApp);
            }

            msWordTemplate = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #endregion

        #endregion
    }
}
