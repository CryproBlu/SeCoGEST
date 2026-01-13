using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public partial class ElencoCompletoArticoli
    {
        public string DescrizioneCompleta
        {
            get
            {
                return string.Format("{0} - {1}", this.CodiceArticolo, this.Descrizione);
            }
        }
        public string DescrizioneConNote
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Note))
                {
                    return this.Descrizione;
                }
                else
                {
                    return string.Format("{0} - {1}", this.Descrizione, this.Note);
                }
            }
        }
    }
}
