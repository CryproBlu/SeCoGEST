using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGes.Utilities;

namespace SeCoGEST.Entities
{
    public partial class Intervento_Stato
    {
        public StatoInterventoEnum StatoEnum
        {
            get
            {
                return (StatoInterventoEnum)Stato;
            }
            set
            {
                Stato = (byte)value;
            }
        }

        public string StatoString
        {
            get
            {
                return StatoEnum.GetDescription();
            }
        }
    }
}
