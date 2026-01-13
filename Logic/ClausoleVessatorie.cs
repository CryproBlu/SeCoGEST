using System;
using System.Collections.Generic;
using System.Linq;
using SeCoGEST.Entities;

namespace SeCoGEST.Logic
{
    public class ClausoleVessatorie : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.ClausoleVessatorie dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public ClausoleVessatorie()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public ClausoleVessatorie(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public ClausoleVessatorie(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.ClausoleVessatorie(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.ClausolaVessatoria entityToCreate, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'ClausolaVessatoria': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.ClausolaVessatoria> Read()
        {
            return from u in dal.Read() orderby u.Codice select u;
        }

        /// <summary>
        /// Restituisce l'entity relativa all'IDentificativo passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.ClausolaVessatoria Find(EntityId<ClausolaVessatoria> identificativoClausolaVessatoria)
        {
            return Find(identificativoClausolaVessatoria.Value);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'codice passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.ClausolaVessatoria Find(EntityString<ClausolaVessatoria> codiceClausolaVessatoria)
        {
            return Find(codiceClausolaVessatoria.Value);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        private Entities.ClausolaVessatoria Find(Guid idToFind)
        {
            return dal.Find(idToFind);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="codeToFind"></param>
        /// <returns></returns>
        private Entities.ClausolaVessatoria Find(string codeToFind)
        {
            return dal.Find(codeToFind);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.ClausolaVessatoria entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'ClausolaVessatoria': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.ClausolaVessatoria> entitiesToDelete, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'ClausolaVessatoria': parametro nullo!");
            }
        }

        #endregion
    }
}