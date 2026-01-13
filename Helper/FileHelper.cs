using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGes.Utilities;

namespace SeCoGEST.Helper
{
    public static class FileHelper
    {
        #region Metodi Pubblici

        /// <summary>
        /// Effettua la cancellazione dell'elenco dei file passati come parametro
        /// </summary>
        /// <param name="elencoFile"></param>
        public static void EliminaElencoFile(IEnumerable<FileInfo> elencoFile)
        {
            if (elencoFile != null && elencoFile.Count() > 0)
            {
                // Per ogni file presente nell'elenco ..
                foreach (FileInfo file in elencoFile)
                {
                    // Nel caso in cui il file esista ..
                    if (file.Exists && !IsFileInUso(file))
                    {
                        // Viene eliminato il file ..
                        file.Delete();
                    }
                }
            }
        }
        /// <summary>
        /// Effettua la cancellazione dell'elenco dei file passati come parametro
        /// </summary>
        /// <param name="elencoFile"></param>
        public static void EliminaElencoFile222222222222222222222222(IEnumerable<FileInfo> elencoFile)
        {
            if (elencoFile != null && elencoFile.Count() > 0)
            {
                // Per ogni file presente nell'elenco ..
                foreach (FileInfo file in elencoFile)
                {
                    // Nel caso in cui il file esista ..
                    if (file.Exists && !IsFileInUso(file))
                    {
                        // Viene eliminato il file ..
                        file.Delete();
                    }
                }
            }
        }

        /// <summary>
        /// Restituisce false se il file esiste e non è occupato da altri processi
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsFileInUso(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file", "Parametro nullo");
            }
            else if (!file.Exists)
            {
                throw new FileNotFoundException("Il file passato come parametro non esiste", file.FullName);
            }

            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None)) { }

                return false;
            }
            catch (IOException)
            {
                return true;
            }
        }

        /// <summary>
        /// Verifica che il file passato come parametro esiste, in caso positivo, viene eliminato se il parametro "eliminaFileSeEsiste" contenga il valore true
        /// </summary>
        /// <param name="percorsoCompletoFile"></param>
        /// <param name="eliminaFileSeEsiste"></param>
        public static void ControllaEsistenzaPerSostituzione(string percorsoCompletoFile, bool eliminaFileSeEsiste)
        {
            ControllaEsistenzaPerSostituzione(new FileInfo(percorsoCompletoFile), eliminaFileSeEsiste);
        }

        /// <summary>
        /// Effettua lo spostamento di un file da un percorso ad un altro. Nel caso in cui il file di destinazione esista ed il valore del parametro "eliminaFileDestinazioneSeEsiste" sia true, quest'ultimo viene eliminato
        /// </summary>
        /// <param name="percorsoCompletoFile"></param>
        /// <param name="percorsoCompletoFileDestinazione"></param>
        /// <param name="eliminaFileDestinazioneSeEsiste"></param>
        public static void Sposta(string percorsoCompletoFile, string percorsoCompletoFileDestinazione, bool eliminaFileDestinazioneSeEsiste)
        {
            Sposta(new FileInfo(percorsoCompletoFile), new FileInfo(percorsoCompletoFileDestinazione), eliminaFileDestinazioneSeEsiste);
        }

        /// <summary>
        /// Effettua la copia dell'elenco dei file passati nella directory il cui percorso è presente nel parametro "directoryDiDestinazione"
        /// </summary>
        /// <param name="fileNonConvertiti"></param>
        /// <param name="directoryDiDestinazione"></param>
        /// <param name="eliminaFileDestinazioneSeEsiste"></param>
        /// <param name="fileNonConvertitiCopiati"></param>
        /// <returns></returns>
        public static MessagesCollector CopiaElencoFile(IList<FileInfo> fileNonConvertiti, DirectoryInfo directoryDiDestinazione, bool eliminaFileDestinazioneSeEsiste, out IList<FileInfo> fileNonConvertitiCopiati)
        {
            if (directoryDiDestinazione == null) throw new ArgumentNullException("directoryDiDestinazione", "Parametro nullo");

            MessagesCollector errori = new MessagesCollector();
            IList<FileInfo> elencoFileNonConvertitiCopiati = new List<FileInfo>();

            // Nel caso in cui l'elenco dei file sia vuoto ..
            if (fileNonConvertiti == null || fileNonConvertiti.Count <= 0)
            {
                // Viene interrotta la procedura restituenso la collection degli errori
                fileNonConvertitiCopiati = elencoFileNonConvertitiCopiati;
                return errori;
            }

            // Nel caso in cui la directory di destinazione non esista, viene creata
            if (!directoryDiDestinazione.Exists)
            {
                directoryDiDestinazione.Create();
                directoryDiDestinazione.Refresh();
            }

            // Per ogni file presente nell'elenco ..
            foreach (FileInfo file in fileNonConvertiti)
            {
                // Viene generato il percorso di destinazione del file da copiare
                string nuovoPercorso = Path.Combine(directoryDiDestinazione.FullName, file.Name);

                try
                {
                    // Viene verificata l'esistenza del file di destinazione ..
                    ControllaEsistenzaPerSostituzione(nuovoPercorso, eliminaFileDestinazioneSeEsiste);

                    // Viene effettuata la copia del file ..
                    File.Copy(file.FullName, nuovoPercorso, eliminaFileDestinazioneSeEsiste);

                    // Viene aggiunto all'elenco dei file copiati, il file corrente
                    elencoFileNonConvertitiCopiati.Add(new FileInfo(nuovoPercorso));
                }
                catch (Exception ex)
                {
                    string errorMessage = String.Format("Errore nello spostamento del file da '{1}' a '{2}'.{0}Dettaglio errore: {3}",
                                                        Environment.NewLine,
                                                        file.FullName,
                                                        nuovoPercorso,
                                                        ex.Message);

                    errori.Add(errorMessage);
                }
            }

            fileNonConvertitiCopiati = elencoFileNonConvertitiCopiati;
            return errori;
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Verifica che il file passato come parametro esiste, in caso positivo, viene eliminato se il parametro "eliminaFileSeEsiste" contenga il valore true
        /// </summary>
        /// <param name="file"></param>
        /// <param name="eliminaFileSeEsiste"></param>
        private static void ControllaEsistenzaPerSostituzione(FileInfo file, bool eliminaFileSeEsiste)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file", "Parametro nullo");
            }

            bool result = file.Exists;

            if (file.Exists)
            {
                if (eliminaFileSeEsiste && !IsFileInUso(file))
                {
                    file.Delete();
                }
            }
        }

        /// <summary>
        /// Effettua lo spostamento di un file da un percorso ad un altro. Nel caso in cui il file di destinazione esista ed il valore del parametro "eliminaFileDestinazioneSeEsiste" sia true, quest'ultimo viene eliminato
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileDestinazione"></param>
        /// <param name="eliminaFileDestinazioneSeEsiste"></param>
        private static void Sposta(FileInfo file, FileInfo fileDestinazione, bool eliminaFileDestinazioneSeEsiste)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file", "Parametro nullo");
            }
            else if (!file.Exists)
            {
                throw new FileNotFoundException("Il file indicato non esiste", file.FullName);
            }

            if (fileDestinazione == null)
            {
                throw new ArgumentNullException("fileDestinazione", "Parametro nullo");
            }

            ControllaEsistenzaPerSostituzione(fileDestinazione, eliminaFileDestinazioneSeEsiste);

            file.MoveTo(fileDestinazione.FullName);
        }

        #endregion
    }
}
