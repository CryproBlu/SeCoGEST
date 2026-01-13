using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class CaratteristicheIntervento : Base.DataLayerBase
    {
        #region Costruttori

        public CaratteristicheIntervento() : base(false) { }
        public CaratteristicheIntervento(bool createStandaloneContext) : base(createStandaloneContext) { }
        public CaratteristicheIntervento(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.CaratteristicaIntervento entityToCreate, bool submitChanges)
        {
            context.CaratteristicaInterventos.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.CaratteristicaIntervento> Read()
        {
            return context.CaratteristicaInterventos;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.CaratteristicaIntervento Find(int id)
        {
            return Read().Where(x => x.Id == id).SingleOrDefault();
        }

        /// <summary>
        /// Restituisce l'entity in base al nome passato
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public Entities.CaratteristicaIntervento Find(string nome)
        {
            return Read().Where(x => x.Nome == nome).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.CaratteristicaIntervento entityToDelete, bool submitChanges)
        {
            context.CaratteristicaInterventos.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.CaratteristicaIntervento> entitiesToDelete, bool submitChanges)
        {
            context.CaratteristicaInterventos.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}