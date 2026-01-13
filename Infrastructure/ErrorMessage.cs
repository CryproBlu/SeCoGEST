using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Infrastructure
{
    public class ErrorMessage
    {
        private ErrorMessage() { }

        public const string UTENTE_SCONOSCIUTO_MESSAGE = "Non è stato possibile recuperare le informazioni sull'utente collegato.";
        public const string ACCESSO_NEGATO_MESSAGE = "Accesso non consentito.";
        public const string UTENTE_OPERAZIONE_NON_CONSENTITA_MESSAGE = "L'utente corrente non è abilitato ad effettuare questa operazione.";
    }
}
