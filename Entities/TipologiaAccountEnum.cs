using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public enum TipologiaAccountEnum
    {
        [Description("Se.Co.Ges.")]
        SeCoGes = 0,

        [Description("Cliente Standard")]
        ClienteStandard = 1,

        [Description("Cliente Admin")]
        ClienteAdmin = 2,

        [Description("Cliente Supervisore")]
        ClienteSupervisore = 3
    }
}
