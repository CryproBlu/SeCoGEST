using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class AnalisiCostiArchivioCampiAggiuntivi : Base.DataLayerBase
    {
        #region Costruttori

        public AnalisiCostiArchivioCampiAggiuntivi() : base(false) { }
        public AnalisiCostiArchivioCampiAggiuntivi(bool createStandaloneContext) : base(createStandaloneContext) { }
        public AnalisiCostiArchivioCampiAggiuntivi(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.AnalisiCostoArchivioCampoAggiuntivo entityToCreate, bool submitChanges)
        {
            context.AnalisiCostoArchivioCampoAggiuntivos.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoArchivioCampoAggiuntivo> Read()
        {
            return context.AnalisiCostoArchivioCampoAggiuntivos;
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.AnalisiCostoArchivioCampoAggiuntivo Find(Guid id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.AnalisiCostoArchivioCampoAggiuntivo entityToDelete, bool submitChanges)
        {
            context.AnalisiCostoArchivioCampoAggiuntivos.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.AnalisiCostoArchivioCampoAggiuntivo> entitiesToDelete, bool submitChanges)
        {
            context.AnalisiCostoArchivioCampoAggiuntivos.DeleteAllOnSubmit(entitiesToDelete);
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
        public IQueryable<Entities.AnalisiCostoArchivioCampoAggiuntivo> Read(decimal codiceGruppo, decimal codiceCategoria, decimal codiceCategoriaStatistica)
        {
            return Read().Where(x =>
                x.CodiceGruppo == codiceGruppo &&
                x.CodiceCategoria == codiceCategoria &&
                x.CodiceCategoriaStatistica == codiceCategoriaStatistica);
        }

        #endregion
    }
}