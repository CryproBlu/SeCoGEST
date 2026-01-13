using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using msExcel = Microsoft.Office.Interop.Excel;

namespace SeCoGEST.Helper
{
    public static class ExcelHelper
    {
        #region Metodi

        /// <summary>
        /// Effettua l'inizializzazione dell'applicazione di word
        /// </summary>
        /// <param name="wordTemplatepath"></param>
        /// <param name="msDocument"></param>
        /// <returns></returns>
        public static msExcel.Application InizializeApplication()
        {
            msExcel.Application msWordApplication = new msExcel.Application();
            msWordApplication.Visible = false;  // Mettere a "TRUE" se si vuole vedere in tempo reale la generazione del file.

            return msWordApplication;
        }

        /// <summary>
        /// Aggiunge all'istanza di word passata come parametro il modello recuperato dal percordo passato come parametro
        /// </summary>
        /// <param name="msExcelApplication"></param>
        /// <param name="excelFilePath"></param>
        /// <returns></returns>
        public static msExcel.Workbook AddWorkbooks(msExcel.Application msExcelApplication, string excelFilePath)
        {
            if (msExcelApplication == null)
            {
                throw new Exception("L'istanza di Word passata è nulla.");
            }

            if (String.IsNullOrEmpty(excelFilePath))
            {
                throw new Exception("Il percorso del modello non è stato valorizzato.");
            }
            else if (!File.Exists(excelFilePath))
            {
                throw new Exception("Il modello non esiste.");
            }

            msExcel.Workbook workbook = msExcelApplication.Workbooks.Add(excelFilePath);

            workbook.Activate();
            return workbook;
        }

        /// <summary>
        /// Effettua il salvataggio del documento passato come parametro
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="fullFilePath">Directory in cui dev'essere effettuato il salvataggio ed il nome del file</param>
        /// <param name="saveFormatFile"></param>
        public static void SaveDocument(msExcel.Workbook workbook, string fullFilePath, msExcel.XlFileFormat saveFormatFile = msExcel.XlFileFormat.xlWorkbookDefault)
        {
            if (workbook == null)
            {
                throw new ArgumentNullException("L'istanza del documento Word passata è nulla.", "workbook");
            }
            else if (String.IsNullOrEmpty(fullFilePath))
            {
                throw new ArgumentNullException("Non è stato indicato il percorso completo del file.", "fullFilePath");
            }

            object objWordDocumentPath = fullFilePath;
            workbook.SaveAs(objWordDocumentPath, saveFormatFile);
        }

        /// <summary>
        /// Effettua il salvataggio del documento passato come parametro in pdf
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="fullFilePath">Directory in cui dev'essere effettuato il salvataggio ed il nome del file</param>
        /// <param name="saveFormatFile"></param>
        public static void SavePdfDocument(msExcel.Workbook workbook, string fullFilePath)
        {
            if (workbook == null)
            {
                throw new ArgumentNullException("L'istanza del documento Word passata è nulla.", "workbook");
            }
            else if (String.IsNullOrEmpty(fullFilePath))
            {
                throw new ArgumentNullException("Non è stato indicato il percorso completo del file.", "fullFilePath");
            }

            object objWordDocumentPath = fullFilePath;
            workbook.ExportAsFixedFormat(msExcel.XlFixedFormatType.xlTypePDF, objWordDocumentPath);
        }

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
        /// <param name="workbook"></param>
        /// <param name="msWordTemplate"></param>
        public static void RilascioRisorseWord(msExcel.Application msWordApp, msExcel.Workbook workbook, ref object msWordTemplate)
        {
            ///rilascio gli oggetti

            if (workbook != null)
            {
                try { workbook.Close(false, null, null); } catch { }
                RilascioOggettiCOM(workbook);
            }

            if (msWordApp != null)
            {
                try { msWordApp.Application.Quit(); } catch { }                
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
