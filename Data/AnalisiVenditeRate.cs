using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class AnalisiVenditeRate : Base.DataLayerBase
    {
        #region Costruttori

        public AnalisiVenditeRate() : base(false) { }
        public AnalisiVenditeRate(bool createStandaloneContext) : base(createStandaloneContext) { }
        public AnalisiVenditeRate(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.AnalisiVenditaRata entityToCreate, bool submitChanges)
        {
            context.AnalisiVenditaRatas.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVenditaRata> Read()
        {
            return context.AnalisiVenditaRatas;
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.AnalisiVenditaRata Find(Guid id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.AnalisiVenditaRata entityToDelete, bool submitChanges)
        {
            context.AnalisiVenditaRatas.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.AnalisiVenditaRata> entitiesToDelete, bool submitChanges)
        {
            context.AnalisiVenditaRatas.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Restituisce tutte le entity associate all'entità AnalisiVendita
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVenditaRata> Read(Entities.AnalisiVendita analisiVendita)
        {
            return Read().Where(x => x.IDAnalisiVendita == analisiVendita.ID);
        }

        #endregion
    }
}