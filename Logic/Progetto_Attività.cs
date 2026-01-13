using SeCoGEST.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Logic
{
    public class Progetto_Attività : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.Progetto_Attività dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public Progetto_Attività()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public Progetto_Attività(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public Progetto_Attività(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.Progetto_Attività(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Progetto_Attivita entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                if (entityToCreate.ID.Equals(Guid.Empty))
                {
                    entityToCreate.ID = Guid.NewGuid();
                }

                // Salvataggio nel database
                dal.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'Progetto_Attività': parametro nullo!");
            }
        }

        public IQueryable<Entities.Progetto_Attivita> ReadAll()
        {
            return dal.Read();
        }


        /// <summary>
        /// Restituisce tutte le entities relative al progetto indicato
        /// </summary>
        /// <param name="idProgetto"></param>
        /// <returns></returns>
        public IQueryable<Entities.Progetto_Attivita> Read(Guid idProgetto)
        {
            return from u in dal.Read() where u.IDProgetto == idProgetto orderby u.Ordine select u;
        }
        public IQueryable<Entities.Progetto_Attivita> Read(EntityId<Entities.Progetto>  idProgetto)
        {
            return Read(idProgetto.Value);
        }



        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.Progetto_Attivita Find(Guid idToFind)
        {
            return dal.Find(idToFind);
        }
        public Entities.Progetto_Attivita Find(EntityId<Progetto_Attivita> idToFind)
        {
            return dal.Find(idToFind.Value);
        }



        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Progetto_Attivita entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                Guid idProgetto = entityToDelete.IDProgetto;
                short ordine = entityToDelete.Ordine;

                dal.Delete(entityToDelete, submitChanges);

                ChangeOrderAfterDelete(new EntityId<Entities.Progetto>(idProgetto), ordine, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'Progetto_Attività': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.Progetto_Attivita> entitiesToDelete, bool submitChanges)
        {
            if (entitiesToDelete != null)
            {
                if (entitiesToDelete.Count() > 0)
                {
                    entitiesToDelete.ToList().ForEach(x => Delete(x, submitChanges));
                }
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'Progetto_Attività': parametro nullo!");
            }
        }

        #endregion

        /// <summary>
        /// Effettua l'aggiornamento dell'ordine delle attività
        /// </summary>
        /// <param name="idToFind"></param>
        /// <param name="upOrder"></param>
        /// <param name="submitChanges"></param>
        /// <returns></returns>
        public bool ChangeOrder(EntityId<Progetto_Attivita> idToFind, bool upOrder, bool submitChanges)
        {
            if (idToFind == null) throw new ArgumentNullException(nameof(idToFind));

            Progetto_Attivita record = Find(idToFind);
            if (record == null) throw new Exception("L'attività di progetto richiesta non esiste nella base dati");

            short order = record.Ordine;
            bool requireGridUpdate = false;
            IQueryable<Progetto_Attivita> attivitàDaAggiornare = Read(record.IDProgetto).Where(x => x.ID != record.ID);
            if (attivitàDaAggiornare.Any())
            {
                if (upOrder)
                {
                    attivitàDaAggiornare = attivitàDaAggiornare.Where(x => x.Ordine == (order - 1));
                }
                else
                {
                    attivitàDaAggiornare = attivitàDaAggiornare.Where(x => x.Ordine == (order + 1));
                }

                if (attivitàDaAggiornare.Any())
                {
                    requireGridUpdate = true;

                    if (upOrder)
                    {
                        record.Ordine = (short)(order - 1);
                    }
                    else
                    {
                        record.Ordine = (short)(order + 1);
                    }

                    foreach (Progetto_Attivita articoloDaAggiornare in attivitàDaAggiornare)
                    {
                        if (upOrder)
                        {
                            articoloDaAggiornare.Ordine += 1;
                        }
                        else
                        {
                            articoloDaAggiornare.Ordine -= 1;
                        }
                    }
                }

                if (submitChanges) SubmitToDatabase();
            }

            return requireGridUpdate;
        }

        public void ChangeOrderAfterDelete(EntityId<Progetto> idProgetto, short ordineElementoRimosso, bool submitChanges)
        {
            if (idProgetto == null) throw new ArgumentNullException(nameof(idProgetto));

            var attivitàProgetto = Read(idProgetto).Where(a => a.Ordine >= ordineElementoRimosso);
            if (attivitàProgetto.Any())
            {
                foreach (var attivita in attivitàProgetto)
                {
                    attivita.Ordine = (short)(attivita.Ordine - 1);
                }
                if (submitChanges) SubmitToDatabase();
            }
        }



        public List<Allegato> GetAllegati(Guid idAttivita)
        {
            var dalAllegati = new Data.Allegati(this.context);
            return dalAllegati.Read().Where(a => 
                a.TipologiaAllegato == TipologiaAllegatoEnum.AttivitaProgetto.GetHashCode() && 
                a.IDLegame == idAttivita).ToList();
        }

        public IList<Entities.Progetto_AttivitaConAllegati> ReadWithAttachments(Guid idProgetto, string filtroNomeAllegato = null)
        {
            var dati = dal.ReadWithAttachments(idProgetto, filtroNomeAllegato).OrderBy(a => a.Ordine).ToList();
            return dati;
        }
    }
}