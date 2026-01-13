using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Data
{
    public class InfoOperazioniRecord : Base.DataLayerBase
    {
        #region Costruttori

        public InfoOperazioniRecord() : base(false) { }
        public InfoOperazioniRecord(bool createStandaloneContext) : base(createStandaloneContext) { }
        public InfoOperazioniRecord(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.InfoOperazioneRecord entityToCreate, bool submitChanges)
        {
            context.InfoOperazioneRecords.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.InfoOperazioneRecord> Read()
        {
            return context.InfoOperazioneRecords;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.InfoOperazioneRecord Find(int id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }


        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.InfoOperazioneRecord entityToDelete, bool submitChanges)
        {
            context.InfoOperazioneRecords.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.InfoOperazioneRecord> entitiesToDelete, bool submitChanges)
        {
            context.InfoOperazioneRecords.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}

