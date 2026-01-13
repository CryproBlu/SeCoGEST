using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class AnalisiVendite : Base.DataLayerBase
    {
        #region Costruttori

        public AnalisiVendite() : base(false) { }
        public AnalisiVendite(bool createStandaloneContext) : base(createStandaloneContext) { }
        public AnalisiVendite(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.AnalisiVendita entityToCreate, bool submitChanges)
        {
            context.AnalisiVenditas.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVendita> Read()
        {
            return context.AnalisiVenditas;
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.AnalisiVendita Find(Guid id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.AnalisiVendita entityToDelete, bool submitChanges)
        {
            context.AnalisiVenditas.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.AnalisiVendita> entitiesToDelete, bool submitChanges)
        {
            context.AnalisiVenditas.DeleteAllOnSubmit(entitiesToDelete);
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
        public IQueryable<Entities.AnalisiVendita> Read(Entities.AnalisiCosto analisiCosto)
        {
            return Read().Where(x => x.IDAnalisiCosto == analisiCosto.ID);
        }

        /// <summary>
        /// Restituisce tutte le entity associate all'entità AnalisiCostoRaggruppamento
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVendita> Read(Entities.AnalisiCostoRaggruppamento raggruppamento)
        {
            return Read().Where(x => x.IDRaggruppamento == raggruppamento.ID);
        }

        #endregion
    }
}