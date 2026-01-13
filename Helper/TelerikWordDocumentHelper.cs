using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Flow.FormatProviders.Pdf;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Editing;

namespace SeCoGEST.Helper
{
    public static class TelerikWordDocumentHelper
    {
        #region Operazioni sul file

        /// <summary>
        /// Effettua la lettura del file di word, il cui percorso viene passato come parametro, mettendolo in memoria
        /// </summary>
        /// <param name="percorsoCompletoFileDocx"></param>
        /// <returns></returns>
        public static RadFlowDocument LeggiFile(string percorsoCompletoFileDocx)
        {
            // Viene verificato il contenuto del parametro "docxFileFullPath"
            // Se il percordo del file non è valorizzato oppure non esiste viene genrato un errore
            if (String.IsNullOrWhiteSpace(percorsoCompletoFileDocx))
            {
                throw new ArgumentNullException("percorsoCompletoFileDocx", "Il percorso del file passato non è stato valorizzato");
            }
            else if (!File.Exists(percorsoCompletoFileDocx))
            {
                throw new FileNotFoundException("Il file passato non esiste", percorsoCompletoFileDocx);
            }

            percorsoCompletoFileDocx = percorsoCompletoFileDocx.Trim();

            // Viene verificata l'estensione presente nel percorso del documento passato come parametro. Se non si tratta di un docx viene scatenata una exception
            if (Path.GetExtension(percorsoCompletoFileDocx).ToLower() != ".docx")
            {
                throw new Exception("Il file non è un documento di word con estensione \".docx\"");
            }

            // Viene creata una nuova istanza del provider relativo ai documenti di word
            DocxFormatProvider formatProvider = new DocxFormatProvider();

            // Viene letto e messo in memoria, il documento di word il cui persorso è stato passato come parametro
            RadFlowDocument document = formatProvider.Import(File.ReadAllBytes(percorsoCompletoFileDocx));

            return document;
        }

        /// <summary>
        /// Effettua l'esportazione del documento word, presente nel parametro "documento", in formato pdf applicando il percorso completo presente nel parametro "percorsoFileDiDestinazione"
        /// </summary>
        /// <param name="documento"></param>
        /// <param name="percorsoFileDiDestinazione"></param>
        public static void EsportaInPdf(RadFlowDocument documento, string percorsoFileDiDestinazione)
        {
            // Viene verificato il parametro "documento", se è null viene scatenato un errore
            if (documento == null) throw new ArgumentNullException("documento", "L'oggetto che contiene il documento da esportare in pdf risulta non valorizzato");

            // Viene verificato il parametro relativo al percordo di destinazione del file.
            // Se il percorso è vuoto oppure non ha come estensione ".pdf" viene generato un errore

            if (String.IsNullOrWhiteSpace(percorsoFileDiDestinazione))
            {
                throw new ArgumentNullException("percorsoFileDiDestinazione", "Il percorso di destinazione del file non è stato valorizzato");
            }
            else if (Path.GetExtension(percorsoFileDiDestinazione).ToLower() != ".pdf")
            {
                throw new Exception("Il documento di word può essere esportato solo in formato \".pdf\"");
            }

            // Viene creata una nuova istanza del provider relativo ai pdf associato ai documenti di word
            PdfFormatProvider pdfProvider = new PdfFormatProvider();
            byte[] renderedBytes = null;

            // Viene effettuata l'esportazione del documento word in pdf inserendolo nella variabile "renderedBytes"
            using (MemoryStream ms = new MemoryStream())
            {
                pdfProvider.Export(documento, ms);
                renderedBytes = ms.ToArray();
            }

            // Il file pdf contenuti nella variabile "renderedBytes", viene scritto su disco
            File.WriteAllBytes(percorsoFileDiDestinazione, renderedBytes);
        }

        #endregion

        #region Operazioni sui bookmark

        /// <summary>
        /// Effettua la sostituzione del contenuto del bookmark, presente nel parametro "contenutoBookmark", sostituendolo con il contenuto presente nel parametro "testoSostitutivo"
        /// </summary>
        /// <param name="documento"></param>
        /// <param name="contenutoBookmark"></param>
        /// <param name="testoSostitutivo"></param>
        public static void BookmarkSostituisciContenutoBookmark(RadFlowDocument documento, string contenutoBookmark, string testoSostitutivo)
        {
            // Viene verificato il parametro "documento", se è null viene scatenato un errore
            if (documento == null) throw new ArgumentNullException("documento", "L'oggetto che contiene il documento di word risulta non valorizzato");

            // Viene verificato il parametro "bookmarkName", se è null viene scatenato un errore
            if (contenutoBookmark == null) throw new ArgumentNullException("contenutoBookmark", "Il contenuto del bookmark non è stato valorizzato");

            if (String.IsNullOrWhiteSpace(testoSostitutivo)) testoSostitutivo = String.Empty;

            // Viene recuperato l'elenco degli oggetti contenuti nei bookmark che hanno come contenuto quello passato come parametro
            List<Run> elencoContenutoBookmarkDaSostituire = documento.EnumerateChildrenOfType<Run>().Where(x => x.Text.ToLower().Trim() == contenutoBookmark.Trim().ToLower()).ToList();
            
            // Nel caso in cui l'elenco sia valorizzato e contenga almeno un elemento ..
            if (elencoContenutoBookmarkDaSostituire != null && elencoContenutoBookmarkDaSostituire.Count > 0)
            {
                // Per ogni elemento recuperato viene sostituto il testo
                foreach(Run elementoContenutoBookmark in elencoContenutoBookmarkDaSostituire)
                {
                    elementoContenutoBookmark.Text = String.Empty;
                    elementoContenutoBookmark.GetEditorBefore().InsertText(testoSostitutivo);
                }
            }
        }

        /// <summary>
        /// Effettua l'inserimento del testo, presente nel parametro "text", nel bookmark, cui nome è presente nel parametro "bookmarkName", contenuto nel documento presente nel parametro "documento"
        /// </summary>
        /// <param name="documento"></param>
        /// <param name="nomeBookmark"></param>
        /// <param name="testo"></param>
        /// <param name="inserisciTestoPrima"></param>
        public static void BookmarkInserisciTesto(RadFlowDocument documento, string nomeBookmark, string testo, bool inserisciTestoPrima)
        {
            // Viene verificato il parametro "documento", se è null viene scatenato un errore
            if (documento == null) throw new ArgumentNullException("documento", "L'oggetto che contiene il documento di word risulta non valorizzato");

            // Viene verificato il parametro "bookmarkName", se è null viene scatenato un errore
            if (nomeBookmark == null) throw new ArgumentNullException("nomeBookmark", "Il nome del bookmark non è stato valorizzato");

            // Viene recuperato l'oggetto che contiene i riferimenti al range iniziali del bookmark il cui nome è passato come parametro
            BookmarkRangeStart riferimentoInizioBookmark = documento.EnumerateChildrenOfType<BookmarkRangeStart>().Where(x => x.Bookmark != null && x.Bookmark.Name == nomeBookmark).SingleOrDefault();
            
            // Nel caso in cui i riferimenti iniziali del bookmark non esistano, viene scatenato un errore
            if (riferimentoInizioBookmark == null)
            {
                throw new Exception(String.Format("Il documento non contiene un bookmark denominato \"{0}\"", nomeBookmark));
            }

            // Viene recuperato l'oggetto "RadFlowDocumentEditor" che permette di inserire il testo, recuperandolo prima del bookmark
            RadFlowDocumentEditor editor = (inserisciTestoPrima) ? riferimentoInizioBookmark.GetEditorBefore() : riferimentoInizioBookmark.GetEditorAfter();

            // Nel caso in cui l'editor recuperato non sia valorizzato, viene genrato un errore
            if (editor == null)
            {
                throw new Exception(String.Format("Non è stato possibile recuperare l'oggetto necessario ad inserire del testo nel bookmark denominato \"{0}\"", nomeBookmark));
            }

            // Viene effettuato l'inserimento del testo passato come parametro
            editor.InsertText(testo);
        }

        /// <summary>
        /// Effettua l'eliminazione dei bookmark contenuti nel documento presente nel parametro "documento"
        /// </summary>
        /// <param name="documento"></param>
        public static void BookmarksEliminaTutti(RadFlowDocument documento)
        {
            // Viene verificato il parametro "documento", se è null viene scatenato un errore
            if (documento == null) throw new ArgumentNullException("documento", "L'oggetto che contiene il documento di word risulta non valorizzato");
            
            // Viene recuperato l'elenco dell'oggetto che contiene i riferimenti al range iniziali del bookmark il cui nome è passato come parametro
            List<BookmarkRangeStart> riferimentiInizioBookmarks = documento.EnumerateChildrenOfType<BookmarkRangeStart>().ToList();

            // Nel caso in cui i riferimenti iniziali del bookmark non esistano, viene scatenato un errore
            if (riferimentiInizioBookmarks != null && riferimentiInizioBookmarks.Count > 0)
            {
                // Viene istanziato l'editor necessario per eliminare i bookmark
                RadFlowDocumentEditor editor = new RadFlowDocumentEditor(documento);

                // Per ogno riferimento iniziale dei bookmark ..
                foreach (BookmarkRangeStart riferimentoInizialeBookmark in riferimentiInizioBookmarks)
                {
                    // Viene utilizzato l'editor per eliminare il bookmark presente nell'oggetto relativo ai riferimenti iniziale dei bookmark
                    editor.DeleteBookmark(riferimentoInizialeBookmark.Bookmark);
                }
            }
        }

        #endregion
    }
}
