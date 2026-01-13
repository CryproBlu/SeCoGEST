using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Data
{
    public class LogInvioNotifiche : Base.DataLayerBase
    {
        #region Costruttori

        public LogInvioNotifiche() : base(false) { }
        public LogInvioNotifiche(bool createStandaloneContext) : base(createStandaloneContext) { }
        public LogInvioNotifiche(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.LogInvioNotifica entityToCreate, bool submitChanges)
        {
            context.LogInvioNotificas.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.LogInvioNotifica> Read()
        {
            return context.LogInvioNotificas;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.LogInvioNotifica Find(int id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }


        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.LogInvioNotifica entityToDelete, bool submitChanges)
        {
            context.LogInvioNotificas.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.LogInvioNotifica> entitiesToDelete, bool submitChanges)
        {
            context.LogInvioNotificas.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion

        #region Funzioni Specifiche

        /// <summary>
        /// Restituisce la data dell'ultimo invio di una notifica in base ai parametri passati
        /// </summary>
        /// <param name="idLegame"></param>
        /// <param name="idTabellaLegame"></param>
        /// <param name="idNotifica"></param>
        /// <returns></returns>
        public DateTime? GetDataUltimoInvioNotifica(Guid idLegame, byte idTabellaLegame, byte idNotifica)
        {
            return context.GetDataUltimoInvioNotifica(idLegame, idTabellaLegame, idNotifica);
        }

        /// <summary>
        /// Restituisce un valore booleano che indica se la notifica specificata dai parametri passati è già stata inviata o meno
        /// </summary>
        /// <param name="idLegame"></param>
        /// <param name="idTabellaLegame"></param>
        /// <param name="idNotifica"></param>
        /// <returns></returns>
        public bool EsisteInvioNotifica(Guid idLegame, byte idTabellaLegame, byte idNotifica)
        {
            return context.LogInvioNotificas.Any(x => x.IDLegame == idLegame && x.IDTabellaLegame == idTabellaLegame && x.IDNotifica == idNotifica);
        }

        /// <summary>
        /// Restituisce un valore booleano che indica se la notifica specificata dai parametri passati è già stata inviata o meno
        /// </summary>
        /// <param name="idLegame"></param>
        /// <param name="idTabellaLegame"></param>
        /// <param name="idNotifica"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public bool EsisteInvioNotifica(Guid idLegame, byte idTabellaLegame, byte idNotifica, string note)
        {
            if(string.IsNullOrEmpty(note))
            {
                return EsisteInvioNotifica(idLegame, idTabellaLegame, idNotifica);
            }
            else
            {
                return context.LogInvioNotificas.Any(x => x.IDLegame == idLegame && x.IDTabellaLegame == idTabellaLegame && x.IDNotifica == idNotifica && x.Note == note);
            }
        }

        #endregion
    }
}

