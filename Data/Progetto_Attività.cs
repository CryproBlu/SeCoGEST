using System;
using System.Collections.Generic;
using System.Linq;

using SeCoGEST.Entities;

namespace SeCoGEST.Data
{
    public class Progetto_Attività : Base.DataLayerBase
    {
        #region Costruttori

        public Progetto_Attività() : base(false) { }
        public Progetto_Attività(bool createStandaloneContext) : base(createStandaloneContext) { }
        public Progetto_Attività(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Progetto_Attivita entityToCreate, bool submitChanges)
        {
            context.Progetto_Attivitas.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Progetto_Attivita> Read()
        {
            return context.Progetto_Attivitas;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.Progetto_Attivita Find(Guid id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }


        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Progetto_Attivita entityToDelete, bool submitChanges)
        {
            context.Progetto_Attivitas.DeleteOnSubmit(entityToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Elimina le entities passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.Progetto_Attivita> entitiesToDelete, bool submitChanges)
        {
            context.Progetto_Attivitas.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion


        public IList<Entities.Progetto_AttivitaConAllegati> ReadWithAttachments(Guid idProgetto, string filtroNomeAllegato = null)
        {
            // 1) Query base delle attività del progetto
            IQueryable<Entities.Progetto_Attivita> qAttivita = Read().Where(a => a.IDProgetto == idProgetto);

            // 2) Se c'è filtro sugli allegati, limito le attività
            if (!string.IsNullOrEmpty(filtroNomeAllegato))
            {
                qAttivita = qAttivita.Where(a =>
                    context.Allegatoes.Any(al =>
                        al.TipologiaAllegato == TipologiaAllegatoEnum.AttivitaProgetto.GetHashCode() &&
                        al.IDLegame == a.ID &&
                        al.NomeFile.Contains(filtroNomeAllegato)));
            }

            // 3) Materializzo le attività filtrate
            var attivitaList = qAttivita.ToList();
            if (attivitaList.Count == 0)
                return new List<Entities.Progetto_AttivitaConAllegati>();

            var idsAttivita = attivitaList.Select(a => a.ID).ToList();

            // 4) Recupero tutti i nomi degli allegati delle attività in UNA sola query
            IQueryable<AllegatoLeggero> qAllegati = context.Allegatoes
                .Where(al => idsAttivita.Contains(al.IDLegame))
                .Select(al => new AllegatoLeggero
                {
                    IDLegame = al.IDLegame,
                    NomeFile = al.NomeFile
                });

            // (opzionale) Se vuoi coerenza totale col filtro, puoi filtrare anche qui:
            if (!string.IsNullOrEmpty(filtroNomeAllegato))
            {
                //qNomiAllegati = qNomiAllegati.Where(al => al.Contains(filtroNomeAllegato));
                qAllegati = qAllegati.Where(al => al.NomeFile.Contains(filtroNomeAllegato));
            }

            var allegatiPerAttivita = qAllegati
                .GroupBy(al => al.IDLegame)
                .ToDictionary(
                    g => g.Key,
                    g => string.Join(", ", g.Select(al => al.NomeFile)));

            // 5) Proiezione nel DTO Progetto_AttivitaConAllegati
            var result = attivitaList.Select(a =>
            {
                string nomiAllegati;
                if (!allegatiPerAttivita.TryGetValue(a.ID, out nomiAllegati))
                    nomiAllegati = string.Empty;

                return new Entities.Progetto_AttivitaConAllegati
                {
                    Intervento = a.Intervento,
                    ID = a.ID,
                    IDProgetto = a.IDProgetto,
                    Progetto = a.Progetto,
                    Ordine = a.Ordine,
                    DataInserimento = a.DataInserimento,
                    Descrizione = a.Descrizione,
                    Scadenza = a.Scadenza,
                    IDOperatoreAssegnato = a.IDOperatoreAssegnato,
                    OperatoreAssegnatoCognomeNome = a.OperatoreAssegnatoCognomeNome,
                    OperatoreAssegnato = a.Operatore,
                    IDOperatoreEsecutore = a.IDOperatoreEsecutore,
                    OperatoreEsecutoreCognomeNome = a.OperatoreEsecutoreCognomeNome,
                    OperatoreEsecutore = a.Esecutore,
                    IDTicket = a.IDTicket,
                    Ticket = a.Ticket,
                    NumeroTicket = a.NumeroTicket,
                    DataInizio = a.DataInizio,
                    OraInizio = a.OraInizio,
                    DataFine = a.DataFine,
                    OraFine = a.OraFine,
                    StatoEnum = a.StatoEnum,
                    Stato = a.Stato,
                    StatoString = a.StatoString,
                    NoteContratto = a.NoteContratto,
                    NoteOperatore = a.NoteOperatore,
                    NomiAllegati = nomiAllegati
                };
            }).ToList();

            return result;
        }
    }
}