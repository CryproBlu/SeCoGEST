using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class Intervento_OrariRepartiUfficio : Base.DataLayerBase
    {
        #region Costruttori

        public Intervento_OrariRepartiUfficio() : base(false) { }
        public Intervento_OrariRepartiUfficio(bool createStandaloneContext) : base(createStandaloneContext) { }
        public Intervento_OrariRepartiUfficio(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Intervento_OrarioRepartoUfficio entityToCreate, bool submitChanges)
        {
            context.Intervento_OrarioRepartoUfficios.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Intervento_OrarioRepartoUfficio> Read()
        {
            return context.Intervento_OrarioRepartoUfficios;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.Intervento_OrarioRepartoUfficio Find(Guid id)
        {
            return Read().Where(x => x.Id == id).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Intervento_OrarioRepartoUfficio entityToDelete, bool submitChanges)
        {
            context.Intervento_OrarioRepartoUfficios.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.Intervento_OrarioRepartoUfficio> entitiesToDelete, bool submitChanges)
        {
            context.Intervento_OrarioRepartoUfficios.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}