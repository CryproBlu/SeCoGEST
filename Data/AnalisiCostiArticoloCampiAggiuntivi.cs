using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class AnalisiCostiArticoloCampiAggiuntivi : Base.DataLayerBase
    {
        #region Costruttori

        public AnalisiCostiArticoloCampiAggiuntivi() : base(false) { }
        public AnalisiCostiArticoloCampiAggiuntivi(bool createStandaloneContext) : base(createStandaloneContext) { }
        public AnalisiCostiArticoloCampiAggiuntivi(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.AnalisiCostoArticoloCampoAggiuntivo entityToCreate, bool submitChanges)
        {
            context.AnalisiCostoArticoloCampoAggiuntivos.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoArticoloCampoAggiuntivo> Read()
        {
            return context.AnalisiCostoArticoloCampoAggiuntivos;
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.AnalisiCostoArticoloCampoAggiuntivo Find(Guid id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.AnalisiCostoArticoloCampoAggiuntivo entityToDelete, bool submitChanges)
        {
            context.AnalisiCostoArticoloCampoAggiuntivos.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.AnalisiCostoArticoloCampoAggiuntivo> entitiesToDelete, bool submitChanges)
        {
            context.AnalisiCostoArticoloCampoAggiuntivos.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Restituisce tutte le entity associate all'entità AnalisiCostoArticolo
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoArticoloCampoAggiuntivo> Read(Entities.AnalisiCostoArticolo articolo)
        {
            return Read().Where(x => x.IDAnalisiCostoArticolo == articolo.ID);
        }
        /// <summary>
        /// Restituisce tutte le entity associate all'entità AnalisiCostoArticolo
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoArticoloCampoAggiuntivo> Read(Entities.EntityId<Entities.AnalisiCostoArticolo> idArticolo)
        {
            return Read().Where(x => x.IDAnalisiCostoArticolo == idArticolo.Value);
        }
        
        #endregion
    }
}