using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public partial class Operatore
    {
        public EntityId<Operatore> Identifier
        {
            get
            {
                return new EntityId<Operatore>(ID);
            }
            set
            {
                ID = (value != null) ? value.Value : Guid.Empty;
            }
        }

        public string AreaString
        {
            get
            {
                return (Area) ? "Si" : "No";
            }
        }

        public string AttivoString
        {
            get
            {
                return (Attivo) ? "Si" : "No";
            }
        }
    }
}
