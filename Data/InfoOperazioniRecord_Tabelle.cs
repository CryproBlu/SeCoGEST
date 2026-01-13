using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Data
{
    public class InfoOperazioniRecord_Tabelle : Base.DataLayerBase
    {
        #region Costruttori

        public InfoOperazioniRecord_Tabelle() : base(false) { }
        public InfoOperazioniRecord_Tabelle(bool createStandaloneContext) : base(createStandaloneContext) { }
        public InfoOperazioniRecord_Tabelle(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.InfoOperazioneRecord_Tabella entityToCreate, bool submitChanges)
        {
            context.InfoOperazioneRecord_Tabellas.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.InfoOperazioneRecord_Tabella> Read()
        {
            return context.InfoOperazioneRecord_Tabellas;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.InfoOperazioneRecord_Tabella Find(byte id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }


        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.InfoOperazioneRecord_Tabella entityToDelete, bool submitChanges)
        {
            context.InfoOperazioneRecord_Tabellas.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.InfoOperazioneRecord_Tabella> entitiesToDelete, bool submitChanges)
        {
            context.InfoOperazioneRecord_Tabellas.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}

