using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class MappatureGruppiCategorieCategorieStatistiche : Base.DataLayerBase
    {
        #region Costruttori

        public MappatureGruppiCategorieCategorieStatistiche() : base(false) { }
        public MappatureGruppiCategorieCategorieStatistiche(bool createStandaloneContext) : base(createStandaloneContext) { }
        public MappatureGruppiCategorieCategorieStatistiche(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.MappaturaGruppoCategoriaCategoriaStatistica entityToCreate, bool submitChanges)
        {
            context.MappaturaGruppoCategoriaCategoriaStatisticas.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.MappaturaGruppoCategoriaCategoriaStatistica> Read()
        {
            return context.MappaturaGruppoCategoriaCategoriaStatisticas;
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.MappaturaGruppoCategoriaCategoriaStatistica Find(int id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.MappaturaGruppoCategoriaCategoriaStatistica entityToDelete, bool submitChanges)
        {
            context.MappaturaGruppoCategoriaCategoriaStatisticas.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.MappaturaGruppoCategoriaCategoriaStatistica> entitiesToDelete, bool submitChanges)
        {
            context.MappaturaGruppoCategoriaCategoriaStatisticas.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}
