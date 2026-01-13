using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class AnalisiCostiArticoli : Base.DataLayerBase
    {
        #region Costruttori

        public AnalisiCostiArticoli() : base(false) { }
        public AnalisiCostiArticoli(bool createStandaloneContext) : base(createStandaloneContext) { }
        public AnalisiCostiArticoli(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.AnalisiCostoArticolo entityToCreate, bool submitChanges)
        {
            context.AnalisiCostoArticolos.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoArticolo> Read()
        {
            return context.AnalisiCostoArticolos;
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.AnalisiCostoArticolo Find(Guid id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.AnalisiCostoArticolo entityToDelete, bool submitChanges)
        {
            context.AnalisiCostoArticolos.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.AnalisiCostoArticolo> entitiesToDelete, bool submitChanges)
        {
            context.AnalisiCostoArticolos.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Restituisce tutte le entity associate all'entità AnalisiCostoRaggruppamento
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoArticolo> Read(Entities.AnalisiCostoRaggruppamento raggruppamento)
        {
            return Read().Where(x => x.IDRaggruppamento == raggruppamento.ID);
        }

        /// <summary>
        /// Restituisce tutte le entity associate all'entità AnalisiCostoRaggruppamento
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoArticolo> Read(Entities.EntityId<Entities.AnalisiCostoRaggruppamento> idRaggruppamento)
        {
            return Read().Where(x => x.IDRaggruppamento == idRaggruppamento.Value);
        }


        /// <summary>
        /// Restituisce tutte le entity associate all'entità AnalisiVendita
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoArticolo> Read(Entities.AnalisiVendita analisiVendita)
        {
            return Read().Where(x => x.IDAnalisiVendita == analisiVendita.ID);
        }

        #endregion
    }
}