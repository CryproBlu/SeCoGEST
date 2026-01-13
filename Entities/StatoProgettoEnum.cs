using System.ComponentModel;

namespace SeCoGEST.Entities
{
    public enum StatoProgettoEnum
    {
        [Description("Da Eseguire")]
        DaEseguire = 0,

        [Description("Eseguito")]
        Eseguito = 1,

        [Description("In Gestione")]
        InGestione = 2
    }

    public enum StatoAttivitaProgettoEnum
    {
        [Description("Da Eseguire")]
        DaEseguire = 0,

        [Description("Eseguito")]
        Eseguito = 1,

        [Description("In Gestione")]
        InGestione = 2,

        [Description("Modificato rispetto a contratto")]
        Modificato = 3
    }

}

