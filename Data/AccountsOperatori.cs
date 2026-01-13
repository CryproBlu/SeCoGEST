using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Data
{
    public class AccountsOperatori : Base.DataLayerBase
    {
        #region Costruttori

        public AccountsOperatori() : base(false) { }
        public AccountsOperatori(bool createStandaloneContext) : base(createStandaloneContext) { }
        public AccountsOperatori(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.AccountOperatore entityToCreate, bool submitChanges)
        {
            context.AccountOperatores.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AccountOperatore> Read()
        {
            return context.AccountOperatores;
        }


        /// <summary>
        /// Restituisce l'entity in base agli identificativi passati e null se non trova l'entity
        /// </summary>
        /// <param name="idAccount"></param>
        /// <param name="idOperatore"></param>
        /// <returns></returns>
        public Entities.AccountOperatore Find(Guid idAccount, Guid idOperatore)
        {
            return Read().Where(x => x.IDAccount == idAccount && x.IDOperatore == idOperatore).SingleOrDefault();
        }

        /// <summary>
        /// Restituisce true se esiste un record associato agli id passati come parametro
        /// </summary>
        /// <param name="idAccount"></param>
        /// <param name="idOperatore"></param>
        /// <returns></returns>
        public bool EsisteRelazione(Guid idAccount, Guid idOperatore)
        {
            return Read().Any(x => x.IDAccount == idAccount && x.IDOperatore == idOperatore);
        }


        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.AccountOperatore entityToDelete, bool submitChanges)
        {
            context.AccountOperatores.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.AccountOperatore> entitiesToDelete, bool submitChanges)
        {
            context.AccountOperatores.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}