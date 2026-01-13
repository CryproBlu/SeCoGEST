using System.ComponentModel;

namespace SeCoGEST.Entities
{
    public enum TipologiaAllegatoEnum : byte
    {
        [Description("Generico")]
        Generico = 0,

        [Description("Offerta")]
        Offerta = 1,

        [Description("Progetto")]
        Progetto = 2,

        [Description("AttivitaProgetto")]
        AttivitaProgetto = 3
    }
}