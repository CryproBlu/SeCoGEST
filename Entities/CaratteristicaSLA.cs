using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public abstract class CaratteristicaSLA_Tempo
    {
        public TimeSpan Tempo { get; set; }
    }

    public class CaratteristicaSLA_PresaInCaricoEntroTempo: CaratteristicaSLA_Tempo
    {
        //public TimeSpan Tempo { get; set; }
    }

    //public class CaratteristicaSLA_OrariUfficio
    //{
    //    public TimeSpan Tempo { get; set; }
    //}
}
