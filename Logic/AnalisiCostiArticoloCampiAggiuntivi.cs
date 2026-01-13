using System;
using System.Collections.Generic;
using System.Linq;
using SeCoGEST.Entities;

namespace SeCoGEST.Logic
{
    public class AnalisiCostiArticoloCampiAggiuntivi : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.AnalisiCostiArticoloCampiAggiuntivi dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public AnalisiCostiArticoloCampiAggiuntivi()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public AnalisiCostiArticoloCampiAggiuntivi(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public AnalisiCostiArticoloCampiAggiuntivi(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.AnalisiCostiArticoloCampiAggiuntivi(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.AnalisiCostoArticoloCampoAggiuntivo entityToCreate, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'AnalisiCostoArticoloCampoAggiuntivo': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoArticoloCampoAggiuntivo> Read()
        {
            return from u in dal.Read() orderby u.Ordine select u;
        }

        /// <summary>
        /// Restituisce l'entity relativa all'IDentificativo passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.AnalisiCostoArticoloCampoAggiuntivo Find(EntityId<AnalisiCostoArticoloCampoAggiuntivo> identificativoAnalisiCostoArticoloCampoAggiuntivo)
        {
            return Find(identificativoAnalisiCostoArticoloCampoAggiuntivo.Value);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        private Entities.AnalisiCostoArticoloCampoAggiuntivo Find(Guid idToFind)
        {
            return dal.Find(idToFind);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.AnalisiCostoArticoloCampoAggiuntivo entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'AnalisiCostoArticoloCampoAggiuntivo': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.AnalisiCostoArticoloCampoAggiuntivo> entitiesToDelete, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'AnalisiCostoArticoloCampoAggiuntivo': parametro nullo!");
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Restituisce tutte le entity associate all'entità AnalisiCostoArticolo
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoArticoloCampoAggiuntivo> Read(Entities.AnalisiCostoArticolo articolo)
        {
            return from u in dal.Read(articolo) orderby u.Ordine select u;
        }
        /// <summary>
        /// Restituisce tutte le entity associate all'entità AnalisiCostoArticolo
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoArticoloCampoAggiuntivo> Read(EntityId<AnalisiCostoArticolo> idArticolo)
        {
            return from u in dal.Read(idArticolo) orderby u.Ordine select u;
        }

        #endregion
    }
}