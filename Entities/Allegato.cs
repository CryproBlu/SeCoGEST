using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGes.Utilities;

namespace SeCoGEST.Entities
{
    public partial class Allegato
    {
        public TipologiaAllegatoEnum TipologiaAllegatoEnum
        {
            get
            {
                return (TipologiaAllegatoEnum)TipologiaAllegato;
            }
            set
            {
                TipologiaAllegato = (byte)value;
            }
        }

        public string TipologiaAllegatoString
        {
            get
            {
                return TipologiaAllegatoEnum.GetDescription();
            }
        }
    }



    public class AllegatoLeggero
    {
        public string NomeFile { get;set;}

        public Guid IDLegame { get; set; }
    }

}
