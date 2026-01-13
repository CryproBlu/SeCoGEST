using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGes.Utilities;

namespace SeCoGEST.Entities
{
    public partial class AnalisiVenditaConfigurazioneArticoloAggiuntivo
    {
        public TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo TipologiaEnum
        { 
            get
            {
                return (TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo)Tipologia;
            }
            set
            {
                Tipologia = (byte)value;
            }
        }

        public string TipologiaString
        {
            get
            {
                return TipologiaEnum.GetDescription();
            }
        }
    }
}
