using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    [Serializable()]
    public class EntityInt<T> : EntityIdentifier<T, int> 
        where T : class
    {
        public EntityInt() : this(int.MinValue)
        {
        }

        public EntityInt(int identifier):
            base(identifier)
        {
        }
    }
}
