using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public enum TipologiaNotificaEnum
    {
        [Description("Intervento aperto ma non preso in carico")]
        InterventoApertoNonPresoInCarico = 0,

        [Description("Intervento scaduto ma non preso in carico")]
        InterventoScadutoNonPresoInCarico = 1,

        [Description("Intervento preso in carico ma non chiuso")]
        InterventoPresoInCaricoNonChiuso = 2,

        [Description("Intervento scaduto preso in carico ma non chiuso")]
        InterventoScadutoPresoInCaricoNonChiuso = 3,

        [Description("Intervento senza alcun operatore assegnato")] // NO
        InterventoSenzaAlcunOperatoreAssegnato = 4,

        //[Description("Intervento da notificare in base a SLA")]
        //InterventoDaNotificarePerSLA = 3,



    }
}
