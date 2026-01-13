using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public partial class CaratteristicaTipologiaIntervento
    {
        public string NomeCaratteristicaIntervento
        {
            get
            {
                if(this.CaratteristicaIntervento == null)
                {
                    return string.Empty;
                }
                else
                {
                    return this.CaratteristicaIntervento.Nome;
                }
            }
        }
    }
}
