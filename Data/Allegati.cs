using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Data
{
    public class Allegati : Base.DataLayerBase
    {
        #region Costruttori

        public Allegati() : base(false) { }
        public Allegati(bool createStandaloneContext) : base(createStandaloneContext) { }
        public Allegati(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Allegato entityToCreate, bool submitChanges)
        {
            context.Allegatoes.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Allegato> Read()
        {
            return context.Allegatoes;
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.Allegato Find(Guid idToFind)
        {
            return Read().Where(x => x.ID == idToFind).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Allegato entityToDelete, bool submitChanges)
        {
            context.Allegatoes.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.Allegato> entitiesToDelete, bool submitChanges)
        {
            context.Allegatoes.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}
