using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class Progetto_Operatori : Base.DataLayerBase
    {
        #region Costruttori

        public Progetto_Operatori() : base(false) { }
        public Progetto_Operatori(bool createStandaloneContext) : base(createStandaloneContext) { }
        public Progetto_Operatori(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Progetto_Operatore entityToCreate, bool submitChanges)
        {
            context.Progetto_Operatores.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Progetto_Operatore> Read()
        {
            return context.Progetto_Operatores;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.Progetto_Operatore Find(Guid id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }


        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Progetto_Operatore entityToDelete, bool submitChanges)
        {
            context.Progetto_Operatores.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.Progetto_Operatore> entitiesToDelete, bool submitChanges)
        {
            context.Progetto_Operatores.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}