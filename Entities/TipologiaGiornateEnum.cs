using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public enum TipologiaGiornateEnum
    {
        [Description("Giorni Lavorativi")]
        GiorniLavorativi = 0,

        [Description("Giorni Solari")]
        GiorniSolari = 1,
    }
}
