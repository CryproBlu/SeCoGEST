using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class TipologieIntervento : Base.DataLayerBase
    {
        #region Costruttori

        public TipologieIntervento() : base(false) { }
        public TipologieIntervento(bool createStandaloneContext) : base(createStandaloneContext) { }
        public TipologieIntervento(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.TipologiaIntervento entityToCreate, bool submitChanges)
        {
            context.TipologiaInterventos.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.TipologiaIntervento> Read()
        {
            return context.TipologiaInterventos;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.TipologiaIntervento Find(Guid id)
        {
            return Read().Where(x => x.Id == id).SingleOrDefault();
        }

        /// <summary>
        /// Restituisce l'entity in base al nome passato
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public Entities.TipologiaIntervento Find(string nome)
        {
            return Read().Where(x => x.Nome == nome).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.TipologiaIntervento entityToDelete, bool submitChanges)
        {
            context.TipologiaInterventos.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.TipologiaIntervento> entitiesToDelete, bool submitChanges)
        {
            context.TipologiaInterventos.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}