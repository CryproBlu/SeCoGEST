using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public partial class Progetto
    {
        public string DenominazioneCliente
        {
            get
            {
                if(this.DestinazioneMerce != null && 
                   this.DestinazioneMerce.Trim() != string.Empty &&
                   this.DestinazioneMerce.Trim() != this.RagioneSociale.Trim())
                {
                    return $"{this.RagioneSociale} - {this.DestinazioneMerce}";

                }
                else
                {
                    return this.RagioneSociale;
                }
            }
        }

        public StatoProgettoEnum StatoEnum
        {
            get
            {
                return (StatoProgettoEnum)this.Stato;
            }
            set
            {
                this.Stato = (byte)value;
            }
        }

        public string StatoString
        {
            get
            {
                return EnumHelper.GetDescription(this.StatoEnum);
                //string stato = string.Empty;
                //switch (this.StatoEnum)
                //{
                //    case StatoProgettoEnum.DaEseguire:
                //        stato = EnumHelper.GetDescription(StatoProgettoEnum.DaEseguire);
                //        stato =  "Da Eseguire";
                //        break;

                //    case StatoProgettoEnum.Eseguito:
                //        stato = "Eseguito";
                //        break;
                //}

                //return stato;
            }
        }

        public string NomeCompletoDPO
        {
            get
            {
                if (this.DPO != null)
                {
                    return this.DPO.CognomeNome;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string ChiusoString
        {
            get
            {
                return this.Chiuso ? "SI" : "NO";
            }
        }

    }
}
