using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public partial class Intervento_Operatore
    {
        public EntityId<Intervento_Operatore> Identifier
        {
            get
            {
                return new EntityId<Intervento_Operatore>(ID);
            }
            set
            {
                if (value == null)
                {
                    ID = Guid.Empty;
                }
                else
                {
                    ID = value.Value;
                }
            }
        }

        public string CognomeNomeOperatore
        {
            get
            {
                return (Operatore != null) ? Operatore.CognomeNome : String.Empty;
            }
        }

        public string DescrizioneModalitaRisoluzioneIntervento
        {
            get
            {
                return (ModalitaRisoluzioneIntervento != null) ? ModalitaRisoluzioneIntervento.Descrizione : String.Empty;
            }
        }

        public string PresaInCaricoString
        {
            get
            {
                return (PresaInCarico.HasValue && PresaInCarico.Value) ? "Si" : "No";
            }
        }
    }
}
