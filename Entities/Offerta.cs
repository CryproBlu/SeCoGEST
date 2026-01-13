using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGes.Utilities;

namespace SeCoGEST.Entities
{
    public partial class Offerta
    {
        public StatoOffertaEnum StatoEnum
        {
            get
            {
                return (StatoOffertaEnum)this.Stato;
            }
            set
            {
                this.Stato = (byte)value;
            }
        }

        public string StatoEnumString
        {
            get
            {
                return StatoEnum.GetDescription();
            }
        }

        public EntityId<Offerta> Identifier
        {
            get
            {
                return new EntityId<Offerta>(ID);
            }
            set
            {
                ID = value?.Value ?? Guid.Empty;
            }
        }

        public TipologiaGiornateEnum? TipologiaTempiDiConsegnaEnum
        {
            get
            {
                return (TipologiaTempiDiConsegna.HasValue) ? (TipologiaGiornateEnum)TipologiaTempiDiConsegna.Value : (TipologiaGiornateEnum?)null;
            }
            set
            {
                this.TipologiaTempiDiConsegna = (value.HasValue) ? (byte)value : (byte?)null;
            }
        }

        public TipologiaGiornateEnum? TipologiaGiorniValiditaEnum
        {
            get
            {
                return (TipologiaGiorniValidita.HasValue) ? (TipologiaGiornateEnum)TipologiaGiorniValidita.Value : (TipologiaGiornateEnum?)null;
            }
            set
            {
                this.TipologiaGiorniValidita = (value.HasValue) ? (byte)value : (byte?)null;
            }
        }
    }
}
