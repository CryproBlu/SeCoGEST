using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class OfferteArticoli : Base.DataLayerBase
    {
        #region Costruttori

        public OfferteArticoli() : base(false) { }
        public OfferteArticoli(bool createStandaloneContext) : base(createStandaloneContext) { }
        public OfferteArticoli(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.OffertaArticolo entityToCreate, bool submitChanges)
        {
            context.OffertaArticolos.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaArticolo> Read()
        {
            return context.OffertaArticolos;
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.OffertaArticolo Find(Guid id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.OffertaArticolo entityToDelete, bool submitChanges)
        {
            context.OffertaArticolos.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.OffertaArticolo> entitiesToDelete, bool submitChanges)
        {
            context.OffertaArticolos.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Restituisce tutte le entity associate all'entità OffertaRaggruppamento
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaArticolo> Read(Entities.OffertaRaggruppamento raggruppamento)
        {
            return Read().Where(x => x.IDRaggruppamento == raggruppamento.ID);
        }

        /// <summary>
        /// Restituisce tutte le entity associate all'entità OffertaRaggruppamento
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaArticolo> Read(Entities.EntityId<Entities.OffertaRaggruppamento> idRaggruppamento)
        {
            return Read().Where(x => x.IDRaggruppamento == idRaggruppamento.Value);
        }

        #endregion
    }
}