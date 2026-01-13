using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    [Serializable()]
    public class EntityId<T> where T : class
    {
        private Guid? ID;

        public EntityId()
        {
            this.ID = null;
        }

        public EntityId(Guid id)
        {
            this.ID = id;
        }

        public EntityId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                this.ID = null;
            }
            else
            {
                this.ID = new Guid(id);
            }
        }
        public EntityId(object id)
        {
            if (id is string)
                if (string.IsNullOrWhiteSpace((string)id))
                {
                    this.ID = null;
                }
                else
                {
                    this.ID = new Guid((string)id);
                }
            else
            {
                Guid gid;
                if (Guid.TryParse(id.ToString(), out gid))
                {
                    this.ID = gid;
                }
                else
                    throw new ArgumentException("Valore di ID non compatibile col tipo Guid");
            }
        }

        public Guid Value
        {
            get
            {
                if (this.ID.HasValue)
                    return this.ID.Value;
                else
                    return Guid.Empty;
            }
            set
            {
                this.ID = value;
            }
        }

        public bool HasValue
        {
            get
            {
                if(this.ID.HasValue)
                {
                    if (this.ID.Value.Equals(Guid.Empty))
                        return false;
                    else
                        return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override string ToString()
        {
            if (this.ID.HasValue)
                return this.ID.ToString();
            else
                return string.Empty;
        }

        public static EntityId<T> Empty
        {
            get
            {
                return new EntityId<T>(Guid.Empty);
            }
        }
    }
}
