using SeCoGEST.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Logic
{
    public static class GestoreDocumenti
    {

        #region Verifica

        /// <summary>
        /// Verifica che il percorso, passato come parametro, esista. Questo metodo serve per centralizzare gli eventuali messaggi di errore sui file generati dalla classe
        /// </summary>
        /// <param name="percorsoFileGenerato"></param>
        /// <param name="manageException"></param>
        /// <returns></returns>
        public static bool FileGeneratoEsiste(string percorsoFileGenerato, bool manageException = false)
        {
            // Nel caso in cui il percorso restuito non sia valorizzato ..
            if (String.IsNullOrEmpty(percorsoFileGenerato))
            {
                if (manageException)
                {
                    return false;
                }
                else
                {
                    // Viene scatenato un errore ..
                    throw new Exception("E' stata generato un errore nella generazione del documento. Il percorso del documento non è valido");
                }                
            }
            // Nel caso in cui il file non esista
            else if (!System.IO.File.Exists(percorsoFileGenerato))
            {
                if (manageException)
                {
                    return false;
                }
                else
                {
                    // Viene scatenato un errore ..
                    throw new Exception("E' stata generato un errore nella generazione del documento. Il file generato non esiste non è valido");
                }
                
            }
            // Altrimenti ..
            else
            {
                return true;
            }
        }

        #endregion

        #region Intervento

        /// <summary>
        /// Effettua la generazione del documento del documento relativo all'intervento il cui identificativo è passato come parametro
        /// </summary>
        /// <param name="identificativoIntervento"></param>
        /// <returns></returns>
        public static string GeneraIntervento(EntityId<Intervento> identificativoIntervento)
        {
            // Nel caso in cui l'identificativo dell'intervento non sia valorizzato oppure sia relativo ad un intervento nuovo ..
            if (identificativoIntervento == null || identificativoIntervento.Value == EntityId<Intervento>.Empty.Value) throw new Exception("Non è possibile effettuare la generazione di documento alla creazione di un intervento");

            Logic.Interventi llIntervento = new Logic.Interventi();

            // Viene generato il documento relativo all'intervento corrente ..
            string percorsoFileGenerato = llIntervento.GeneraDocumento(identificativoIntervento, Infrastructure.ConfigurationKeys.PERCORSO_TEMPORANEO);
            return percorsoFileGenerato;
        }

        /// <summary>
        /// Restituisce il nome da applicare al documento dell'intervento
        /// </summary>
        /// <param name="entityIntervento"></param>
        /// <returns></returns>
        public static string GetNomeDocumentoIntervento(Intervento entityIntervento)
        {
            if (entityIntervento == null) throw new ArgumentNullException("entityIntervento", "Parametro nullo");

            // Viene generato il nome del file in cui salvare il documento
            string nomeDocumentoGenerato = String.Format("Documento_intervento_numero_{0}_del_{1:yyyyMMdd}_{1:HHmm}.pdf", entityIntervento.Numero, entityIntervento.DataRedazione);

            return nomeDocumentoGenerato;
        }

        #endregion
    }
}
