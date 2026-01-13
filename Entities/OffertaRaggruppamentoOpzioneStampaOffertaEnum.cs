using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    [Flags]
    public enum OffertaRaggruppamentoOpzioneStampaOffertaEnum
    {
        [Description("Nessuna Opzione")]
        NessunaOpzione = int.MinValue,

        [Description("Mostra Gruppo")]
        MostraRaggruppamento = 1 << 0,

        [Description("Mostra Intestazioni Gruppo")]
        MostraIntestazioniRaggruppamento = 1 << 1,        

        [Description("Mostra Totali Gruppo")]
        MostraTotaliRaggruppamento = 1 << 2,

        [Description("Mostra Articoli")]
        MostraArticoli = 1 << 3,

        [Description("Mostra Intestazioni Articoli")]
        MostraIntestazioniArticoli = 1 << 4,

        [Description("Mostra Totali Articolo")]
        MostraTotaliArticolo = 1 << 5,

        [Description("Tutte Le Opzioni")]
        TutteLeOpzioni = int.MaxValue
    }
}
