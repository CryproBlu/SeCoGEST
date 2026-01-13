using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGes.Utilities;

namespace SeCoGEST.Entities
{
    public partial class Progetto_Attivita
    {
         public StatoAttivitaProgettoEnum StatoEnum
        {
            get
            {
                return (StatoAttivitaProgettoEnum)this.Stato;
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
                string stato = string.Empty;
                switch (this.StatoEnum)
                {
                    case StatoAttivitaProgettoEnum.DaEseguire:
                        stato = "Da Eseguire";
                        break;

                    case StatoAttivitaProgettoEnum.Eseguito:
                        stato = "Eseguito";
                        break;

                    case StatoAttivitaProgettoEnum.InGestione:
                        stato = "In Gestione";
                        break;

                    case StatoAttivitaProgettoEnum.Modificato:
                        stato = "Modificato rispetto a contratto";
                        break;
                }

                return stato;
            }
        }

        public string OperatoreAssegnatoCognomeNome
        {
            get
            {
                if(this.Operatore != null)
                {
                    return this.Operatore.CognomeNome;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string OperatoreEsecutoreCognomeNome
        {
            get
            {
                if (this.Esecutore != null)
                {
                    return this.Esecutore.CognomeNome;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string NumeroTicket
        {
            get
            {
                if(this.Intervento != null)
                {
                    return this.Intervento.Numero.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }


    public class Progetto_AttivitaConAllegati
    {
        public Guid ID { get; set; }
        public Guid IDProgetto { get; set; }
        public short Ordine { get; set; }
        public DateTime DataInserimento { get; set; }
        public string Descrizione { get; set; }
        public DateTime? Scadenza { get; set; }
        public Guid? IDOperatoreAssegnato { get; set; }
        public Operatore OperatoreAssegnato { get; set; }
        public string OperatoreAssegnatoCognomeNome { get; set; }
        public Guid? IDOperatoreEsecutore { get; set; }
        public Operatore OperatoreEsecutore { get; set; }
        public string OperatoreEsecutoreCognomeNome { get; set; }
        public Guid? IDTicket { get; set; }
        public DateTime? DataInizio { get; set; }
        public TimeSpan? OraInizio { get; set; }
        public DateTime? DataFine { get; set; }
        public TimeSpan? OraFine { get; set; }
        public StatoAttivitaProgettoEnum StatoEnum { get; set; }
        public byte Stato { get; set; }
        public string StatoString { get; set; }
        public string NoteContratto { get; set; }
        public string NoteOperatore { get; set; }
        public Progetto Progetto { get; set; }
        public Intervento Intervento { get; set; }
        public Intervento Ticket { get; set; }
        public string NumeroTicket { get; set; }
        public string NomiAllegati { get; set; }
    }
}
