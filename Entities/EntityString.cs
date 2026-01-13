using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    [Serializable()]
    public class EntityString<T> : EntityIdentifier<T, string> 
        where T : class
    {
        public EntityString() : this(null)
        {
        }

        public EntityString(string identifier) :
            base(identifier)
        {
        }
    }
}
    