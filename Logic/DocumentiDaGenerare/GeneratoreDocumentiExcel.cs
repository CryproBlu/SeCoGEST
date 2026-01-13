using SeCoGEST.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using msExcel = Microsoft.Office.Interop.Excel;

namespace SeCoGEST.Logic.DocumentiDaGenerare
{
    public abstract class GeneratoreDocumentiExcel : Base.LogicLayerBase, IDisposable
    {
        #region Proprietà e variabili

        protected msExcel.Application application = null;
        protected msExcel.Workbook workbook = null;
        private object percorsoTemplate = null;


        #endregion

        #region Costruttori e DAL interno

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public GeneratoreDocumentiExcel()
            : base(false)
        {
            CreateLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public GeneratoreDocumentiExcel(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public GeneratoreDocumentiExcel(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateLogic();
        }

        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        protected abstract void CreateLogic();

        #endregion

        #region Metodi Pubblici

        /// <summary>
        /// Effettua il rilascio delle risorse occupate dall'istanza corrente
        /// </summary>
        public virtual void Dispose()
        {
            RilasciaApplicazioneWordDocumento();
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Effettua l'inizializzazione del documento utilizzando il template passato come parametro
        /// </summary>
        /// <param name="contenutoDocumento"></param>
        /// <param name="nomeFile"></param>
        /// <param name="generatePdf"></param>
        public void GeneraDocumento(out byte[] contenutoDocumento, out string nomeFile, bool generatePdf = true)
        {
            contenutoDocumento = null;
            nomeFile = String.Empty;

            RilasciaApplicazioneWordDocumento();

            string estensione = (generatePdf) ? "pdf" : "xlsx";
            string fileName = $"{Guid.NewGuid()}.{estensione}";
            string percorsoCompletoDestinazioneFile = Path.Combine(Infrastructure.ConfigurationKeys.PERCORSO_DIRECTORY_FILE_TEMPORANEI, fileName);

            try
            {
                application = ExcelHelper.InizializeApplication();
                workbook = application.Workbooks.Add();
                workbook.Activate();

                PopolaDocumento(application, workbook);

                if (generatePdf)
                {
                    ExcelHelper.SavePdfDocument(workbook, percorsoCompletoDestinazioneFile);
                }
                else
                {
                    ExcelHelper.SaveDocument(workbook, percorsoCompletoDestinazioneFile);
                }

                nomeFile = Path.GetFileName(fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                RilasciaApplicazioneWordDocumento();

                if (File.Exists(percorsoCompletoDestinazioneFile))
                {
                    contenutoDocumento = File.ReadAllBytes(percorsoCompletoDestinazioneFile);
                    File.Delete(percorsoCompletoDestinazioneFile);
                }

                // Vengono rilasciate le risorse occupate per la generazione del documento 

            }
        }

        /// <summary>
        /// Effettua le risorse occupate dall'applicativo di generazione dei documenti
        /// </summary>
        protected void RilasciaApplicazioneWordDocumento()
        {
            if (application != null || workbook != null) ExcelHelper.RilascioRisorseWord(application, workbook, ref percorsoTemplate);
            percorsoTemplate = null;
        }

        /// <summary>
        /// Effettua il popolamento del documeto passato come parametro, documento generato dall'applicativo passato come parametro
        /// </summary>
        /// <param name="application"></param>
        /// <param name="workbook"></param>
        protected abstract void PopolaDocumento(msExcel.Application application, msExcel.Workbook workbook);

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Effettua il settaggio del grassetto a una cella di un foglio in base ai dati passati come parametro
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="indiceRiga"></param>
        /// <param name="indiceColonna"></param>
        protected void SettaGrassetto(msExcel.Worksheet worksheet, int indiceRiga, int indiceColonna)
        {
            msExcel.Range cella = worksheet.Cells[indiceRiga, indiceColonna];
            cella.Font.Bold = true;
        }

        /// <summary>
        /// Effettua il settaggio dei bordi a una cella di un foglio in base ai dati passati come parametro
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="indiceRiga"></param>
        /// <param name="indiceColonna"></param>
        /// <param name="borderWeight"></param>
        protected void SettaBordo(msExcel.Worksheet worksheet, int indiceRiga, int indiceColonna, msExcel.XlBorderWeight borderWeight = msExcel.XlBorderWeight.xlThin)
        {
            msExcel.Range cella = worksheet.Cells[indiceRiga, indiceColonna];
            cella.Borders[msExcel.XlBordersIndex.xlEdgeLeft].Weight = msExcel.XlBorderWeight.xlThin;
            cella.Borders[msExcel.XlBordersIndex.xlEdgeTop].Weight = msExcel.XlBorderWeight.xlThin;
            cella.Borders[msExcel.XlBordersIndex.xlEdgeRight].Weight = msExcel.XlBorderWeight.xlThin;
            cella.Borders[msExcel.XlBordersIndex.xlEdgeBottom].Weight = msExcel.XlBorderWeight.xlThin;
        }

        #endregion
    }
}
