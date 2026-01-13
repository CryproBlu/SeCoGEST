using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGes.Utilities;

namespace SeCoGEST.Entities
{
    public partial class LogInvioNotifica
    {
        public InfoOperazioneTabellaEnum IDTabellaLegameEnum
        {
            get
            {
                return (InfoOperazioneTabellaEnum)IDTabellaLegame;
            }
            set
            {
                IDTabellaLegame = (byte)value;
            }
        }

        public string IDTabellaLegameString
        {
            get
            {
                return IDTabellaLegameEnum.GetDescription();
            }
        }

        public TipologiaNotificaEnum IDNotificaEnum
        {
            get
            {
                return (TipologiaNotificaEnum)IDNotifica;
            }
            set
            {
                IDNotifica = (byte)value;
            }
        }

        public string IDNotificaEnumString
        {
            get
            {
                return IDNotificaEnum.GetDescription();
            }
        }
    }
}
