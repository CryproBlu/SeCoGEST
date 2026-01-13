using System;
using System.Collections.Generic;
using System.Linq;
using SeCoGes.Utilities;

namespace SeCoGEST.Data
{
    public class Operatori : Base.DataLayerBase
    {
        #region Costruttori

        public Operatori() : base(false) { }
        public Operatori(bool createStandaloneContext) : base(createStandaloneContext) { }
        public Operatori(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Operatore entityToCreate, bool submitChanges)
        {
            context.Operatores.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Operatore> Read()
        {
            return context.Operatores;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.Operatore Find(Guid id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }

        /// <summary>
        /// Restituisce l'entity in base al cognome e nome passato
        /// </summary>
        /// <param name="cognome"></param>
        /// <param name="nome"></param>
        /// <returns></returns>
        public Entities.Operatore Find(string cognome, string nome)
        {
            return Read().Where(x => x.Cognome.ToLower() == cognome.ToTrimmedString().ToLower() && 
                                     x.Nome.ToLower() == nome.ToTrimmedString().ToLower())
                         .SingleOrDefault();
        }


        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Operatore entityToDelete, bool submitChanges)
        {
            context.Operatores.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.Operatore> entitiesToDelete, bool submitChanges)
        {
            context.Operatores.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}