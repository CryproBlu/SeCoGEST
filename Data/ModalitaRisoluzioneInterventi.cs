using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class ModalitaRisoluzioneInterventi : Base.DataLayerBase
    {
        #region Costruttori

        public ModalitaRisoluzioneInterventi() : base(false) { }
        public ModalitaRisoluzioneInterventi(bool createStandaloneContext) : base(createStandaloneContext) { }
        public ModalitaRisoluzioneInterventi(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.ModalitaRisoluzioneIntervento entityToCreate, bool submitChanges)
        {
            context.ModalitaRisoluzioneInterventos.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.ModalitaRisoluzioneIntervento> Read()
        {
            return context.ModalitaRisoluzioneInterventos;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.ModalitaRisoluzioneIntervento Find(int id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }


        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.ModalitaRisoluzioneIntervento entityToDelete, bool submitChanges)
        {
            context.ModalitaRisoluzioneInterventos.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.ModalitaRisoluzioneIntervento> entitiesToDelete, bool submitChanges)
        {
            context.ModalitaRisoluzioneInterventos.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}