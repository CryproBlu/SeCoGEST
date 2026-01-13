using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class OfferteArchivioCampiAggiuntivi : Base.DataLayerBase
    {
        #region Costruttori

        public OfferteArchivioCampiAggiuntivi() : base(false) { }
        public OfferteArchivioCampiAggiuntivi(bool createStandaloneContext) : base(createStandaloneContext) { }
        public OfferteArchivioCampiAggiuntivi(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.OffertaArchivioCampoAggiuntivo entityToCreate, bool submitChanges)
        {
            context.OffertaArchivioCampoAggiuntivos.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaArchivioCampoAggiuntivo> Read()
        {
            return context.OffertaArchivioCampoAggiuntivos;
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.OffertaArchivioCampoAggiuntivo Find(Guid id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.OffertaArchivioCampoAggiuntivo entityToDelete, bool submitChanges)
        {
            context.OffertaArchivioCampoAggiuntivos.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.OffertaArchivioCampoAggiuntivo> entitiesToDelete, bool submitChanges)
        {
            context.OffertaArchivioCampoAggiuntivos.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Restituisce tutte le entity associate ai valori di gruppo, categoria e categoria statistica indicati
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaArchivioCampoAggiuntivo> Read(decimal codiceGruppo, decimal codiceCategoria, decimal codiceCategoriaStatistica)
        {
            return Read().Where(x =>
                x.CodiceGruppo == codiceGruppo &&
                x.CodiceCategoria == codiceCategoria &&
                x.CodiceCategoriaStatistica == codiceCategoriaStatistica);
        }

        #endregion
    }
}