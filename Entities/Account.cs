using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGes.Utilities;

namespace SeCoGEST.Entities
{
    public partial class Account
    {
        public EntityId<Account> Identifier
        {
            get
            {
                return new EntityId<Account>(ID);
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

        public string AmministratoreString
        {
            get
            {
                string result = String.Empty;
                if (Amministratore.HasValue && Amministratore.Value)
                {
                    result = "Si";
                }
                else
                {
                    result = "No";
                }

                return result;
            }
        }


        public string SolaLetturaString
        {
            get
            {
                string result = String.Empty;
                if (SolaLettura.HasValue && SolaLettura.Value)
                {
                    result = "Si";
                }
                else
                {
                    result = "No";
                }

                return result;
            }
        }


        public string BloccatoString
        {
            get
            {
                string result = String.Empty;
                if (Bloccato.HasValue && Bloccato.Value)
                {
                    result = "Si";
                }
                else
                {
                    result = "No";
                }

                return result;
            }
        }

        public TipologiaAccountEnum TipologiaEnum
        { 
            get
            {
                return (TipologiaAccountEnum)Tipologia;
            }
            set
            {
                Tipologia = (byte)value;
            }
        }

        public string TipologiaString
        {
            get
            {
                return TipologiaEnum.GetDescription();
            }
        }

        public string NominativoConEmail
        {
            get
            {
                return $"{Nominativo} (Email: {Email})";
            }
        }
    }
}
