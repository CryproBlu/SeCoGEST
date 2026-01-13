using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public partial class AccountOperatore
    {
        public EntityId<Account> IdentifierAccount
        {
            get
            {
                return new EntityId<Account>(IDAccount);
            }
            set
            {
                if (value == null)
                {
                    IDAccount = Guid.Empty;
                }
                else
                {
                    IDAccount = value.Value;
                }
            }
        }

        public EntityId<Operatore> IdentifierOperatore
        {
            get
            {
                return new EntityId<Operatore>(IDOperatore);
            }
            set
            {
                if (value == null)
                {
                    IDOperatore = Guid.Empty;
                }
                else
                {
                    IDOperatore = value.Value;
                }
            }
        }

        public bool AttivaInvioNotifiche
        {
            get
            {
                if(this.InviaNotifiche.HasValue)
                    return this.InviaNotifiche.Value;
                else
                    return false;
            }
        }
    }
}
