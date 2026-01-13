using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Data
{
    public class OfferteAccountValidatori : Base.DataLayerBase
    {
        #region Costruttori

        public OfferteAccountValidatori() : base(false) { }
        public OfferteAccountValidatori(bool createStandaloneContext) : base(createStandaloneContext) { }
        public OfferteAccountValidatori(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.OffertaAccountValidatore entityToCreate, bool submitChanges)
        {
            context.OffertaAccountValidatores.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaAccountValidatore> Read()
        {
            return context.OffertaAccountValidatores;
        }

        /// <summary>
        /// Restituisce l'entity in base agli identificativi passati
        /// </summary>
        /// <param name="idOfferta"></param>
        /// <param name="idAccount"></param>
        /// <returns></returns>
        public Entities.OffertaAccountValidatore Find(Guid idOfferta, Guid idAccount)
        {
            return Read().Where(x => x.IDOfferta == idOfferta && x.IDAccount == idAccount).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.OffertaAccountValidatore entityToDelete, bool submitChanges)
        {
            context.OffertaAccountValidatores.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.OffertaAccountValidatore> entitiesToDelete, bool submitChanges)
        {
            context.OffertaAccountValidatores.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}
