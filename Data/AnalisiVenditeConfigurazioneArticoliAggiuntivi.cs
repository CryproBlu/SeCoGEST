using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class AnalisiVenditeConfigurazioneArticoliAggiuntivi : Base.DataLayerBase
    {
        #region Costruttori

        public AnalisiVenditeConfigurazioneArticoliAggiuntivi() : base(false) { }
        public AnalisiVenditeConfigurazioneArticoliAggiuntivi(bool createStandaloneContext) : base(createStandaloneContext) { }
        public AnalisiVenditeConfigurazioneArticoliAggiuntivi(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo entityToCreate, bool submitChanges)
        {
            context.AnalisiVenditaConfigurazioneArticoloAggiuntivos.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo> Read()
        {
            return context.AnalisiVenditaConfigurazioneArticoloAggiuntivos;
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo Find(Guid id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo entityToDelete, bool submitChanges)
        {
            context.AnalisiVenditaConfigurazioneArticoloAggiuntivos.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo> entitiesToDelete, bool submitChanges)
        {
            context.AnalisiVenditaConfigurazioneArticoloAggiuntivos.DeleteAllOnSubmit(entitiesToDelete);
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
        public IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo> Read(decimal codiceGruppo, decimal codiceCategoria, decimal codiceCategoriaStatistica)
        {
            return Read().Where(x =>
                x.CodiceGruppoIn == codiceGruppo &&
                x.CodiceCategoriaIn == codiceCategoria &&
                x.CodiceCategoriaStatisticaIn == codiceCategoriaStatistica);
        }

        #endregion
    }
}