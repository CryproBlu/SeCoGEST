using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class OfferteArticoloCampiAggiuntivi : Base.DataLayerBase
    {
        #region Costruttori

        public OfferteArticoloCampiAggiuntivi() : base(false) { }
        public OfferteArticoloCampiAggiuntivi(bool createStandaloneContext) : base(createStandaloneContext) { }
        public OfferteArticoloCampiAggiuntivi(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.OffertaArticoloCampoAggiuntivo entityToCreate, bool submitChanges)
        {
            context.OffertaArticoloCampoAggiuntivos.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaArticoloCampoAggiuntivo> Read()
        {
            return context.OffertaArticoloCampoAggiuntivos;
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.OffertaArticoloCampoAggiuntivo Find(Guid id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.OffertaArticoloCampoAggiuntivo entityToDelete, bool submitChanges)
        {
            context.OffertaArticoloCampoAggiuntivos.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.OffertaArticoloCampoAggiuntivo> entitiesToDelete, bool submitChanges)
        {
            context.OffertaArticoloCampoAggiuntivos.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Restituisce tutte le entity associate all'entità OffertaArticolo
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaArticoloCampoAggiuntivo> Read(Entities.OffertaArticolo articolo)
        {
            return Read().Where(x => x.IDOffertaArticolo == articolo.ID);
        }
        /// <summary>
        /// Restituisce tutte le entity associate all'entità OffertaArticolo
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaArticoloCampoAggiuntivo> Read(Entities.EntityId<Entities.OffertaArticolo> idArticolo)
        {
            return Read().Where(x => x.IDOffertaArticolo == idArticolo.Value);
        }
        
        #endregion
    }
}