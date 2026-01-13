using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    /// <summary>
    /// Enumeratore contenente la tipologia di cambiamento effettuato da un'entity
    /// </summary>
    public enum InfoTipoOperazioneRecordEnum
    {
        [Description("Creazione")]
        Creazione = 0,

        [Description("Modifica")]
        Modifica = 1,

        [Description("Eliminazione")]
        Eliminazione = 2
    }
}
