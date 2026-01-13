using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class ClausoleVessatorie : Base.DataLayerBase
    {
        #region Costruttori

        public ClausoleVessatorie() : base(false) { }
        public ClausoleVessatorie(bool createStandaloneContext) : base(createStandaloneContext) { }
        public ClausoleVessatorie(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.ClausolaVessatoria entityToCreate, bool submitChanges)
        {
            context.ClausolaVessatorias.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.ClausolaVessatoria> Read()
        {
            return context.ClausolaVessatorias;
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="idClausolaVessatoria"></param>
        /// <returns></returns>
        public Entities.ClausolaVessatoria Find(Guid idClausolaVessatoria)
        {
            return Read().Where(x => x.ID == idClausolaVessatoria).SingleOrDefault();
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="codeClausolaVessatoria"></param>
        /// <returns></returns>
        public Entities.ClausolaVessatoria Find(string codeClausolaVessatoria)
        {
            return Read().Where(x => x.Codice.ToLower().Trim() == codeClausolaVessatoria.ToLower().Trim()).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.ClausolaVessatoria entityToDelete, bool submitChanges)
        {
            context.ClausolaVessatorias.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.ClausolaVessatoria> entitiesToDelete, bool submitChanges)
        {
            context.ClausolaVessatorias.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}