using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    [Serializable()]
    public class EntityIdentifier<ENTITY_TYPE, VALUE_TYPE>
        where ENTITY_TYPE : class
    {
        private VALUE_TYPE Identifier;

        public EntityIdentifier()
        {
            this.Identifier = default(VALUE_TYPE);
        }

        public EntityIdentifier(VALUE_TYPE identifier)
        {
            this.Identifier = identifier;
        }

        public VALUE_TYPE Value
        {
            get
            {
                return this.Identifier;
            }
        }
    }
}
