using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class CaratteristicheTipologieIntervento : Base.DataLayerBase
    {
        #region Costruttori

        public CaratteristicheTipologieIntervento() : base(false) { }
        public CaratteristicheTipologieIntervento(bool createStandaloneContext) : base(createStandaloneContext) { }
        public CaratteristicheTipologieIntervento(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.CaratteristicaTipologiaIntervento entityToCreate, bool submitChanges)
        {
            context.CaratteristicaTipologiaInterventos.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.CaratteristicaTipologiaIntervento> Read()
        {
            return context.CaratteristicaTipologiaInterventos;
        }

        /// <summary>
        /// Restituisce l'entity in base agli id passati
        /// </summary>
        /// <param name="IdConfigurazione"></param>
        /// <param name="IdCaratteristica"></param>
        /// <returns></returns>
        public Entities.CaratteristicaTipologiaIntervento Find(Guid IdConfigurazione, int IdCaratteristica)
        {
            return Read().Where(x => x.IdConfigurazione == IdConfigurazione && x.IdCaratteristica == IdCaratteristica).SingleOrDefault();
        }


        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.CaratteristicaTipologiaIntervento entityToDelete, bool submitChanges)
        {
            context.CaratteristicaTipologiaInterventos.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.CaratteristicaTipologiaIntervento> entitiesToDelete, bool submitChanges)
        {
            context.CaratteristicaTipologiaInterventos.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}