using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class RepartiUfficio : Base.DataLayerBase
    {
        #region Costruttori

        public RepartiUfficio() : base(false) { }
        public RepartiUfficio(bool createStandaloneContext) : base(createStandaloneContext) { }
        public RepartiUfficio(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.RepartoUfficio entityToCreate, bool submitChanges)
        {
            context.RepartoUfficios.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.RepartoUfficio> Read()
        {
            return context.RepartoUfficios;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.RepartoUfficio Find(Guid id)
        {
            return Read().Where(x => x.Id == id).SingleOrDefault();
        }

        /// <summary>
        /// Restituisce l'entity in base al nome passato
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public Entities.RepartoUfficio Find(string nome)
        {
            return Read().Where(x => x.Reparto == nome).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.RepartoUfficio entityToDelete, bool submitChanges)
        {
            context.RepartoUfficios.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.RepartoUfficio> entitiesToDelete, bool submitChanges)
        {
            context.RepartoUfficios.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}