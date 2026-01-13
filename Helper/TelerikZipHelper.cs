using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Zip;

namespace SeCoGEST.Helper
{
    public static class TelerikZipHelper
    {
        /// <summary>
        /// Effettua la compressione dei file passati come parametro restuendoli sotto forma di MemoryStream
        /// </summary>
        /// <param name="elencoFileDaComprimere"></param>
        /// <param name="scatenaExceptionSeFileNonEsiste"></param>
        /// <param name="eliminaFileDopoCompressione"></param>
        /// <returns></returns>
        public static MemoryStream EffettuaCompressione(List<FileInfo> elencoFileDaComprimere, bool scatenaExceptionSeFileNonEsiste = false, bool eliminaFileDopoCompressione = true)
        {
            MemoryStream memStream = new MemoryStream();

            using (ZipArchive archive = new ZipArchive(memStream, ZipArchiveMode.Create, true, null))
            {
                foreach (FileInfo fileDaComprimere in elencoFileDaComprimere)
                {
                    if (fileDaComprimere.Exists)
                    {
                        using (ZipArchiveEntry entry = archive.CreateEntry(fileDaComprimere.Name))
                        {
                            BinaryWriter writer = new BinaryWriter(entry.Open());
                            writer.Write(File.ReadAllBytes(fileDaComprimere.FullName));
                            writer.Flush();
                        }

                        if (eliminaFileDopoCompressione)
                        {
                            fileDaComprimere.Delete();
                        }
                    }
                    else
                    {
                        if (scatenaExceptionSeFileNonEsiste)
                        {
                            throw new FileNotFoundException("File non trovato", fileDaComprimere.FullName);
                        }
                    }
                }
            }

            memStream.Seek(0, SeekOrigin.Begin);

            return memStream;
        }
    }
}
