using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class Intervento_CaratteristicheTipologieIntervento : Base.DataLayerBase
    {
        #region Costruttori

        public Intervento_CaratteristicheTipologieIntervento() : base(false) { }
        public Intervento_CaratteristicheTipologieIntervento(bool createStandaloneContext) : base(createStandaloneContext) { }
        public Intervento_CaratteristicheTipologieIntervento(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Intervento_CaratteristicaTipologiaIntervento entityToCreate, bool submitChanges)
        {
            context.Intervento_CaratteristicaTipologiaInterventos.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Intervento_CaratteristicaTipologiaIntervento> Read()
        {
            return context.Intervento_CaratteristicaTipologiaInterventos;
        }

        /// <summary>
        /// Restituisce l'entity in base agli id passati
        /// </summary>
        /// <param name="IdIntervento"></param>
        /// <param name="IdCaratteristica"></param>
        /// <returns></returns>
        public Entities.Intervento_CaratteristicaTipologiaIntervento Find(Guid IdIntervento, int IdCaratteristica)
        {
            return Read().Where(x => x.IdIntervento == IdIntervento && x.IdCaratteristica == IdCaratteristica).SingleOrDefault();
        }


        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Intervento_CaratteristicaTipologiaIntervento entityToDelete, bool submitChanges)
        {
            context.Intervento_CaratteristicaTipologiaInterventos.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.Intervento_CaratteristicaTipologiaIntervento> entitiesToDelete, bool submitChanges)
        {
            context.Intervento_CaratteristicaTipologiaInterventos.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}