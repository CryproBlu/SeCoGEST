using System;

namespace SeCoGEST.Entities
{
    //[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    //public class CaratteristicaInterventoAttribute : Attribute
    //{
    //    public CaratteristicaInterventoAttribute(string idCaratteristicaIntervento) : this(int.Parse(idCaratteristicaIntervento)) { }

    //    public CaratteristicaInterventoAttribute(int idCaratteristicaIntervento)
    //    {
    //        IdCaratteristicaIntervento = idCaratteristicaIntervento;
    //    }

    //    public int IdCaratteristicaIntervento { get; private set; }
    //}


    public enum CaratteristicaInterventoEnum
    {
        //[CaratteristicaIntervento(0)]
        CaratteristicaGenerica = 0,
        RispostaEntroMinuti = 1,
        PresaInCaricoEntroMinuti = 2,
        RipristinoEntroMinuti = 3,
        RipristinoEntroMinutiDaPresaInCarico = 4
    }

    //public class CaratteristicaInterventoParametri
    //{
    //    public CaratteristicaInterventoEnum CaratteristicaIntervento { get; set; }
    //}

    //public class CaratteristicaInterventoParametri_PresaInCaricoEntro : CaratteristicaInterventoParametri
    //{
    //    public int Minuti { get; set; }
    //}
}
