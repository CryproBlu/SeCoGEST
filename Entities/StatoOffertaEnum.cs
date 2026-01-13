using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public enum StatoOffertaEnum : byte
    {
        [Description("Aperta")]
        Aperta,

        [Description("In Validazione")]
        InValidazione,

        [Description("Validata Con Esito Positivo")]
        ValidataConEsitoPositivo,

        [Description("Validata Con Esito Negativo")]
        ValidataConEsitoNegativo,

        [Description("Inviata Al Cliente")]
        InviataAlCliente,

        [Description("Accettata Dal Cliente")]
        AccettataDalCliente,

        [Description("Rifiutata Dal Cliente")]
        RifiutataDalCliente
    }
}
