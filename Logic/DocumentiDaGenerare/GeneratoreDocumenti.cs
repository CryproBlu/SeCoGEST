using SeCoGEST.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using msWord = Microsoft.Office.Interop.Word;

namespace SeCoGEST.Logic.DocumentiDaGenerare
{
    public abstract class GeneratoreDocumenti : Base.LogicLayerBase, IDisposable
    {
        #region Proprietà e variabili

        protected msWord.Application application = null;
        protected msWord.Document document = null;
        private object percorsoTemplate = null;
        

        #endregion

        #region Costruttori e DAL interno

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public GeneratoreDocumenti()
            : base(false)
        {
            CreateLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public GeneratoreDocumenti(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public GeneratoreDocumenti(Base.LogicLayerBase logicLayer)
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
        /// <param name="templateWord"></param>
        /// <param name="contenutoDocumento"></param>
        /// <param name="nomeFile"></param>
        /// <param name="generatePdf"></param>
        public void GeneraDocumento(string templateWord, out byte[] contenutoDocumento, out string nomeFile, bool generatePdf = true)
        {
            contenutoDocumento = null;
            nomeFile = String.Empty;

            if (String.IsNullOrWhiteSpace(templateWord)) throw new ArgumentNullException(nameof(templateWord), $"Il percorso del file di template da utilizzare, non è stato indicato");
            if (!File.Exists(templateWord)) throw new ArgumentException($"Il file \"{templateWord}\" non esiste");

            RilasciaApplicazioneWordDocumento();
            percorsoTemplate = templateWord;

            msWord.WdSaveFormat formatoSalvataggio = (generatePdf) ? msWord.WdSaveFormat.wdFormatPDF : msWord.WdSaveFormat.wdFormatDocumentDefault;
            string estensione = (formatoSalvataggio == msWord.WdSaveFormat.wdFormatPDF) ? "pdf" : "docx";
            string fileName = $"{Guid.NewGuid()}.{estensione}";
            string percorsoCompletoDestinazioneFile = Path.Combine(Infrastructure.ConfigurationKeys.PERCORSO_DIRECTORY_FILE_TEMPORANEI, fileName);

            try
            {
                application = WordHelper.InizializeApplication();
                document = WordHelper.AddDocument(application, percorsoTemplate.ToString());

                PopolaDocumento(application, document);

                WordHelper.SaveDocument(document, percorsoCompletoDestinazioneFile, formatoSalvataggio);
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
            if (application != null || document != null) WordHelper.RilascioRisorseWord(application, document, ref percorsoTemplate);
            percorsoTemplate = null;
        }

        /// <summary>
        /// Effettua il popolamento del documeto passato come parametro, documento generato dall'applicativo passato come parametro
        /// </summary>
        /// <param name="application"></param>
        /// <param name="document"></param>
        protected abstract void PopolaDocumento(msWord.Application application, msWord.Document document);

        #endregion
    }
}
