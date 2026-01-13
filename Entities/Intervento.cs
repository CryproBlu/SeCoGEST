using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGes.Utilities;

namespace SeCoGEST.Entities
{
    public partial class Intervento
    {
        public EntityId<Intervento> Identifier
        {
            get
            {
                return new EntityId<Intervento>(ID);
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

        public string RagioneSocialeConDestinazione
        {
            get
            {
                return $"{this.RagioneSociale} {this.DestinazioneMerce.ToTrimmedString()}".Trim();
            }
        }

        public string IndirizzoCompleto
        {
            get
            {
                return String.Join(", ", Indirizzo, Localita, CAP, Provincia);
            }
        }

        public StatoInterventoEnum? StatoEnum
        {
            get
            {
                return (StatoInterventoEnum?)Stato;
            }
        }

        public string StatoString
        {
            get
            {
                return (StatoEnum.HasValue) ? StatoEnum.Value.GetDescription() : String.Empty;
            }
        }
        public string StatoStringForCustomers
        {
            get
            {
                if(StatoEnum.HasValue)
                {
                    if(StatoEnum == StatoInterventoEnum.Validato)
                    {
                        return "Chiuso";
                    }
                    else
                    {
                        return StatoString;
                    }
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        //public TipologiaInterventoEnum? TipologiaEnum
        //{
        //    get
        //    {
        //        return (TipologiaInterventoEnum?)Tipologia;
        //    }
        //}

        //public string TipologiaString
        //{
        //    get
        //    {
        //        return (TipologiaEnum.HasValue) ? TipologiaEnum.Value.GetDescription() : String.Empty;
        //    }
        //}

        public string TipologiaString
        {
            get
            {
                return this.TipologiaIntervento == null ? string.Empty : this.TipologiaIntervento.Nome;
            }
        }

        public string ChiamataString
        {
            get
            {
                return (Chiamata.HasValue && Chiamata.Value) ? "Si" : "No";
            }
        }

        public string UrgenteString
        {
            get
            {
                return (Urgente.HasValue && Urgente.Value) ? "Si" : "No";
            }
        }

        public string InternoString
        {
            get
            {
                return (Interno.HasValue && Interno.Value) ? "Si" : "No";
            }
        }

        public string ChiusoString
        {
            get
            {
                return (Chiuso.HasValue && Chiuso.Value) ? "Si" : "No";
            }
        }

        public string DescrizioniModiDiRisoluzione
        {
            get
            {
                if (Intervento_Operatores != null && Intervento_Operatores.Count > 0)
                {
                    IEnumerable<string> modiDiRisoluzione = Intervento_Operatores.Where(x=> x.ModalitaRisoluzioneIntervento != null)
                                                                                 .Select(x => x.ModalitaRisoluzioneIntervento.Descrizione)
                                                                                 .Distinct();

                    if (modiDiRisoluzione == null) modiDiRisoluzione = new List<string>();

                    return String.Join(Helper.Web.HTML_NEW_LINE, modiDiRisoluzione);
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        public string Operatori
        {
            get
            {
                if (Intervento_Operatores != null && Intervento_Operatores.Count > 0)
                {
                    IEnumerable<string> operatori = Intervento_Operatores.Select(x => x.CognomeNomeOperatore).Distinct();

                    if (operatori == null) operatori = new List<string>();

                    return String.Join(Helper.Web.HTML_NEW_LINE, operatori);
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        public DateTime? TempoLimiteRestante
        {
            get
            {
                return this.Intervento_CaratteristicaTipologiaInterventos.Min(c => c.DataLimite);
            }
        }

        /// <summary>
        /// Restituisce data ed ora della prima presa in carico dell'intervento
        /// </summary>
        public DateTime? DataPresaInCarico
        {
            get
            {
                IEnumerable<Entities.Intervento_Operatore> intOpers = this.Intervento_Operatores.Where(x => x.PresaInCarico.HasValue && x.PresaInCarico.Value);
                if(intOpers.Any())
                {
                    return intOpers.Min(x => x.DataPresaInCarico.Value);
                }
                else
                {
                    return null;
                }
            }
        }

        public string DescrizioneSLAIntervento { get; set; }


        /// <summary>
        /// Restituisce un valore booleano che indica se almeno un articolo è impostato per essere fatturato
        /// </summary>
        public bool DaFatturare
        {
            get
            {
                return this.Intervento_Articolos.Any(x => x.DaFatturare.HasValue && x.DaFatturare.Value);
            }
        }
        /// <summary>
        /// Restituisce un valore testo SI/NO che indica se almeno un articolo è impostato per essere fatturato
        /// </summary>
        public string DaFatturareString
        {
            get
            {
                return DaFatturare ? "Si" : "No";
            }
        }

        /// <summary>
        /// Restituisce un valore booleano che indica se per tutti gli articoli è stato indicato un tempo
        /// </summary>
        public bool TuttiGliArticoloConTempiIndicati
        {
            get
            {
                return !this.Intervento_Articolos.Any(x => string.IsNullOrEmpty(x.OreTime) && !x.Quantita.HasValue);
            }
        }

    }
}
