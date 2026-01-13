using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public enum InfoOperazioneTabellaEnum
    {
        [Description("SeCoGEST.Entities.Intervento")]
        Intervento = 0,

        [Description("SeCoGEST.Entities.Intervento_Operatore")]
        InterventoOperatore = 1,
    }
}
