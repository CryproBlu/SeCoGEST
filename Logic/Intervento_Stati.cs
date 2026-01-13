using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGEST.Entities;

namespace SeCoGEST.Logic
{
    public class Intervento_Stati : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.Intervento_Stati dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public Intervento_Stati()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public Intervento_Stati(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public Intervento_Stati(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.Intervento_Stati(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Intervento_Stato entityToCreate, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'Intervento_Stato': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities in base allo stato del'Intervento_Stato
        /// <param name="intervento"></param>
        /// <param name="ordinamentoPerDataAscendente"></param>
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Intervento_Stato> Read(Entities.Intervento intervento, bool ordinamentoPerDataAscendente = false)
        {
            return Read(new EntityId<Entities.Intervento>(intervento.ID), ordinamentoPerDataAscendente);
        }

        /// <summary>
        /// Restituisce tutte le entities in base allo stato del'Intervento_Stato
        /// </summary>
        /// <param name="idIntervento"></param>
        /// <param name="ordinamentoPerDataAscendente"></param>
        /// <returns></returns>
        public IQueryable<Entities.Intervento_Stato> Read(EntityId<Entities.Intervento> idIntervento, bool ordinamentoPerDataAscendente = false)
        {
            IQueryable<Entities.Intervento_Stato> elencoRecord = dal.Read().Where(x => x.IDIntervento == idIntervento.Value);

            if (ordinamentoPerDataAscendente)
            {
                return elencoRecord.OrderBy(x => x.Data);
            }
            else
            {
                return elencoRecord.OrderByDescending(x => x.Data);
            }            
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.Intervento_Stato Find(EntityId<Entities.Intervento_Stato> idToFind)
        {
            return dal.Find(idToFind.Value);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Intervento_Stato entityToDelete, bool submitChanges)
        {
            Delete(entityToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Intervento_Stato entityToDelete, bool checkAllegati, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'Intervento_Stato': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.Intervento_Stato> entitiesToDelete, bool submitChanges)
        {
            Delete(entitiesToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.Intervento_Stato> entitiesToDelete, bool checkAllegati, bool submitChanges)
        {
            if (entitiesToDelete != null)
            {
                if (entitiesToDelete.Count() > 0)
                {
                    entitiesToDelete.ToList().ForEach(x => Delete(x, checkAllegati, submitChanges));
                }
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'Intervento_Stato': parametro nullo!");
            }
        }

        #endregion
   }
}
