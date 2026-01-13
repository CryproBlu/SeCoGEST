using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class AnalisiCostiRaggruppamenti : Base.DataLayerBase
    {
        #region Costruttori

        public AnalisiCostiRaggruppamenti() : base(false) { }
        public AnalisiCostiRaggruppamenti(bool createStandaloneContext) : base(createStandaloneContext) { }
        public AnalisiCostiRaggruppamenti(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.AnalisiCostoRaggruppamento entityToCreate, bool submitChanges)
        {
            context.AnalisiCostoRaggruppamentos.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoRaggruppamento> Read()
        {
            return context.AnalisiCostoRaggruppamentos;
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.AnalisiCostoRaggruppamento Find(Guid id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.AnalisiCostoRaggruppamento entityToDelete, bool submitChanges)
        {
            context.AnalisiCostoRaggruppamentos.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.AnalisiCostoRaggruppamento> entitiesToDelete, bool submitChanges)
        {
            context.AnalisiCostoRaggruppamentos.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Restituisce tutte le entity associate all'entità AnalisiCosto
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoRaggruppamento> Read(Entities.AnalisiCosto analisiCosto)
        {
            return Read().Where(x => x.IDAnalisiCosto == analisiCosto.ID);
        }

        /// <summary>
        /// Restituisce tutte le entity associate all'entità AnalisiCosto
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoRaggruppamento> Read(Entities.EntityId<Entities.AnalisiCosto> idAnalisiCosto)
        {
            return Read().Where(x => x.IDAnalisiCosto == idAnalisiCosto.Value);
        }

        #endregion
    }
}