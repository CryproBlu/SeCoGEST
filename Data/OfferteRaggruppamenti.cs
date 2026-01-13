using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class OfferteRaggruppamenti : Base.DataLayerBase
    {
        #region Costruttori

        public OfferteRaggruppamenti() : base(false) { }
        public OfferteRaggruppamenti(bool createStandaloneContext) : base(createStandaloneContext) { }
        public OfferteRaggruppamenti(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.OffertaRaggruppamento entityToCreate, bool submitChanges)
        {
            context.OffertaRaggruppamentos.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaRaggruppamento> Read()
        {
            return context.OffertaRaggruppamentos;
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.OffertaRaggruppamento Find(Guid id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.OffertaRaggruppamento entityToDelete, bool submitChanges)
        {
            context.OffertaRaggruppamentos.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.OffertaRaggruppamento> entitiesToDelete, bool submitChanges)
        {
            context.OffertaRaggruppamentos.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Restituisce tutte le entity associate all'entità Offerta
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaRaggruppamento> Read(Entities.Offerta offerta)
        {
            return Read().Where(x => x.IDOfferta == offerta.ID);
        }

        /// <summary>
        /// Restituisce tutte le entity associate all'entità Offerta
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaRaggruppamento> Read(Entities.EntityId<Entities.Offerta> idOfferta)
        {
            return Read().Where(x => x.IDOfferta == idOfferta.Value);
        }

        #endregion
    }
}