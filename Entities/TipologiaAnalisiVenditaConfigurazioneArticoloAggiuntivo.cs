using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public enum TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo
    {
        [Description("Mostrare messaggio")]
        MostrareMessaggio = 0,

        [Description("Aggiungere articolo")]
        AggiungereArticolo = 1,

        [Description("Mostrare messaggio e inserire articolo")]
        MostrareMessaggio_e_AggiungereArticolo = 2,

        [Description("Template articolo per descrizione preventivo")]
        Template_Articolo_Descrizione_Preventivo = 3,

        [Description("Template articolo per contratto")]
        Template_Articolo_Contratto = 4,

        [Description("Template articolo per descrizione preventivo e contratto")]
        Template_Articolo_Descrizione_Preventivo_Contratto = 5,
    }
}
