using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class Intervento_Discussioni : Base.DataLayerBase
    {
        #region Costruttori

        public Intervento_Discussioni() : base(false) { }
        public Intervento_Discussioni(bool createStandaloneContext) : base(createStandaloneContext) { }
        public Intervento_Discussioni(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Intervento_Discussione entityToCreate, bool submitChanges)
        {
            context.Intervento_Discussiones.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Intervento_Discussione> Read()
        {
            return context.Intervento_Discussiones;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.Intervento_Discussione Find(Guid id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }


        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Intervento_Discussione entityToDelete, bool submitChanges)
        {
            context.Intervento_Discussiones.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.Intervento_Discussione> entitiesToDelete, bool submitChanges)
        {
            context.Intervento_Discussiones.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}

