using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGes.Utilities;

namespace SeCoGEST.Entities
{
    public partial class OffertaRaggruppamento
    {
        public OffertaRaggruppamentoOpzioneStampaOffertaEnum OpzioneStampaOffertaEnum
        {
            get
            {
                return (OffertaRaggruppamentoOpzioneStampaOffertaEnum)OpzioneStampaOfferta;
            }
            set
            {
                OpzioneStampaOfferta = (int)value;
            }
        }

        public string OpzioneStampaOffertaDescription
        {
            get
            {
                return OpzioneStampaOffertaEnum.GetDescription();
            }
        }
    }
}
