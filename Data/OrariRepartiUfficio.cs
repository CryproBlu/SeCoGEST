using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class OrariRepartiUfficio : Base.DataLayerBase
    {
        #region Costruttori

        public OrariRepartiUfficio() : base(false) { }
        public OrariRepartiUfficio(bool createStandaloneContext) : base(createStandaloneContext) { }
        public OrariRepartiUfficio(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.OrarioRepartoUfficio entityToCreate, bool submitChanges)
        {
            context.OrarioRepartoUfficios.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OrarioRepartoUfficio> Read()
        {
            return context.OrarioRepartoUfficios;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.OrarioRepartoUfficio Find(Guid id)
        {
            return Read().Where(x => x.Id == id).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.OrarioRepartoUfficio entityToDelete, bool submitChanges)
        {
            context.OrarioRepartoUfficios.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.OrarioRepartoUfficio> entitiesToDelete, bool submitChanges)
        {
            context.OrarioRepartoUfficios.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}