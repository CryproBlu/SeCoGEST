using SeCoGEST.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Logic
{
    public class Progetti : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.Progetti dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public Progetti()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public Progetti(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public Progetti(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.Progetti(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Progetto entityToCreate, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'Progetto': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Progetto> Read()
        {
            return from u in dal.Read() orderby u.Numero descending select u;
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.Progetto Find(EntityId<Entities.Progetto> identificativoServizio)
        {
            return dal.Find(identificativoServizio.Value);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="numeroToFind"></param>
        /// <returns></returns>
        public Entities.Progetto Find(int numeroToFind)
        {
            return Read().FirstOrDefault(x => x.Numero == numeroToFind);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="numeroToFind"></param>
        /// <returns></returns>
        public Entities.Progetto Find(EntityString<Entities.Progetto> numeroToFind)
        {
            if(int.TryParse(numeroToFind.Value, out int num))
            {
                return Read().FirstOrDefault(x => x.Numero == num);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Progetto entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'Progetto': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.Progetto> entitiesToDelete, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'Progetto': parametro nullo!");
            }
        }

        #endregion

        #region Funzioni Spedifiche

        /// <summary>
        /// Restituisce il numero da utilizzare per il prossimo nuovo Progetto
        /// </summary>
        /// <returns></returns>
        public int GetNuovoNumero()
        {
            int? max = dal.Read().Select(x => (int?)x.Numero).Max();
            if (max.HasValue)
                return max.Value + 1;
            else
                return 1;
        }

        public Entities.Progetto Clona(EntityId<Entities.Progetto> entityId)
        {
            Entities.Progetto prjToClone = Find(entityId);
            if(prjToClone == null)
            {
                return null;
            }


            Entities.Progetto newPrj = new Entities.Progetto();
            newPrj.ID = Guid.NewGuid();
            newPrj.Numero = GetNuovoNumero();
            newPrj.DataRedazione = DateTime.Now;
            newPrj.Note = string.Empty;
            newPrj.Stato = (byte)Entities.StatoProgettoEnum.DaEseguire.GetHashCode();
            newPrj.Chiuso = false;

            newPrj.NumeroCommessa = prjToClone.NumeroCommessa;
            newPrj.CodiceContratto = prjToClone.CodiceContratto;
            newPrj.IDReferenteCliente = prjToClone.IDReferenteCliente;
            newPrj.IDDPO = prjToClone.IDDPO;
            newPrj.CodiceCliente = prjToClone.CodiceCliente;
            newPrj.RagioneSociale = prjToClone.RagioneSociale;
            newPrj.IdDestinazione = prjToClone.IdDestinazione;
            newPrj.DestinazioneMerce = prjToClone.DestinazioneMerce;
            newPrj.Indirizzo = prjToClone.Indirizzo;
            newPrj.CAP = prjToClone.CAP;
            newPrj.Localita = prjToClone.Localita;
            newPrj.Provincia = prjToClone.Provincia;
            newPrj.Telefono = prjToClone.Telefono;
            newPrj.Titolo = prjToClone.Titolo;
            newPrj.Descrizione = prjToClone.Descrizione;


            foreach(Entities.Progetto_Attivita pa in prjToClone.Progetto_Attivita)
            {
                Entities.Progetto_Attivita newPrjAtt = new Entities.Progetto_Attivita();
                newPrjAtt.ID = Guid.NewGuid();
                newPrjAtt.Progetto = newPrj;
                newPrjAtt.IDTicket = null;
                newPrjAtt.Stato = (byte)Entities.StatoProgettoEnum.DaEseguire.GetHashCode();
                newPrjAtt.Scadenza = null;
                newPrjAtt.IDOperatoreAssegnato = null;
                newPrjAtt.IDOperatoreEsecutore = null;
                newPrjAtt.NoteContratto = string.Empty;
                newPrjAtt.NoteOperatore = string.Empty;

                newPrjAtt.Ordine = pa.Ordine;
                newPrjAtt.Descrizione = pa.Descrizione;
            }

            //foreach (Entities.Progetto_Operatore po in prjToClone.Progetto_Operatore)
            //{
            //    Entities.Progetto_Operatore newPrjOp = new Entities.Progetto_Operatore();
            //    newPrjOp.ID = Guid.NewGuid();
            //    newPrjOp.Progetto = newPrj;
            //    newPrjOp.Ruolo = 0;

            //    newPrjOp.IDOperatore = po.IDOperatore;
            //}

            Create(newPrj, true);

            return newPrj;
        }

        #endregion
    }
}
