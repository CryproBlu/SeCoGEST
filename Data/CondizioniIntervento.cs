using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class CondizioniIntervento : Base.DataLayerBase
    {
        #region Costruttori

        public CondizioniIntervento() : base(false) { }
        public CondizioniIntervento(bool createStandaloneContext) : base(createStandaloneContext) { }
        public CondizioniIntervento(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.CondizioneIntervento entityToCreate, bool submitChanges)
        {
            context.CondizioneInterventos.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.CondizioneIntervento> Read()
        {
            return context.CondizioneInterventos;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.CondizioneIntervento Find(int id)
        {
            return Read().Where(x => x.Id == id).SingleOrDefault();
        }

        /// <summary>
        /// Restituisce l'entity in base al nome passato
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public Entities.CondizioneIntervento Find(string nome)
        {
            return Read().Where(x => x.Nome == nome).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.CondizioneIntervento entityToDelete, bool submitChanges)
        {
            context.CondizioneInterventos.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.CondizioneIntervento> entitiesToDelete, bool submitChanges)
        {
            context.CondizioneInterventos.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}