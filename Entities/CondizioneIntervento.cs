using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public partial class CondizioneIntervento
    {
        public CondizioneInterventoEnum? CondizioneInterventoEnumValue
        {
            get
            {
                if(this.Id > 0)
                {
                    if (Enum.IsDefined(typeof(Entities.CondizioneInterventoEnum), this.Id))
                    {
                        return (Entities.CondizioneInterventoEnum)this.Id;
                    }
                }
                return null;
            }
        }
    }
}
