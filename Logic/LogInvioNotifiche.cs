using SeCoGEST.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Logic
{
    public class LogInvioNotifiche : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.LogInvioNotifiche dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public LogInvioNotifiche()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public LogInvioNotifiche(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public LogInvioNotifiche(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.LogInvioNotifiche(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.LogInvioNotifica entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                // Salvataggio nel database
                dal.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'LogInvioNotifica': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.LogInvioNotifica> Read()
        {
            return dal.Read().OrderByDescending(x=> x.Data);
        }

        /// <summary>
        /// Restituisce l'elenco dei log inviati, in base ai parametri passati
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identificativo"></param>
        /// <param name="tipologiaNotifica"></param>
        /// <returns></returns>
        public IQueryable<LogInvioNotifica> Read<T>(EntityId<T> identificativo, TipologiaNotificaEnum tipologiaNotifica)
            where T : class
        {
            return Read().Where(x => x.IDLegame == identificativo.Value && x.IDNotifica == (byte)tipologiaNotifica);
        }
        
        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.LogInvioNotifica Find(Entities.EntityInt<Entities.LogInvioNotifica> identificativoNotifica)
        {
            return dal.Find(identificativoNotifica.Value);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.LogInvioNotifica entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'LogInvioNotifica': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.LogInvioNotifica> entitiesToDelete, bool submitChanges)
        {
            if (entitiesToDelete != null)
            {
                dal.Delete(entitiesToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'LogInvioNotifica': parametro nullo!");
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
        public DateTime? GetDataUltimoInvioNotifica<T>(Entities.EntityId<T> identificativoLegame, Entities.InfoOperazioneTabellaEnum idTabellaLegame, Entities.TipologiaNotificaEnum idNotifica)
            where T : class
        {
            DateTime? dataUltimoInvio = dal.GetDataUltimoInvioNotifica(identificativoLegame.Value, (byte)idTabellaLegame, (byte)idNotifica);
            return dataUltimoInvio;
        }

        /// <summary>
        /// Restituisce un periodo globale riferito al periodo non lavorativo
        /// </summary>
        /// <param name="dataInizio"></param>
        /// <param name="dataFine"></param>
        /// <returns></returns>
        public TimeSpan GetTotalePeriodoNonLavorativo(DateTime dataInizio, DateTime dataFine)
        {
            return new TimeSpan(0, 0, 0, 0);
        }

        /// <summary>
        /// Restituisce il numero di invii effettuati in base ai parametri passati
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identificativo"></param>
        /// <param name="tipologiaNotifica"></param>
        /// <returns></returns>
        public int GetCountInviiEffettuati<T>(EntityId<T> identificativo, TipologiaNotificaEnum tipologiaNotifica)
            where T : class
        {
            IQueryable<LogInvioNotifica> elencoNotifiche = Read<T>(identificativo, tipologiaNotifica);
            int count = elencoNotifiche.Count();

            return count;
        }

        /// <summary>
        /// Restituisce true se la lettura dei log delle notifiche indica che il primo invio è guà stato effettuato
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identificativo"></param>
        /// <param name="tipologiaNotifica"></param>
        /// <returns></returns>
        public bool IsPrimoInvioEffettuato<T>(EntityId<T> identificativo, TipologiaNotificaEnum tipologiaNotifica)
            where T : class
        {
            return Read<T>(identificativo, tipologiaNotifica).Any();
        }

        /// <summary>
        /// Restituisce la data dell'ultimo invio di una notifica in base ai parametri passati
        /// </summary>
        /// <param name="idLegame"></param>
        /// <param name="idTabellaLegame"></param>
        /// <param name="idNotifica"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public bool IsInvioNotificaGiàEffettuato<T>(Entities.EntityId<T> identificativoLegame, Entities.InfoOperazioneTabellaEnum idTabellaLegame, Entities.TipologiaNotificaEnum idNotifica, string note)
            where T : class
        {
            return dal.EsisteInvioNotifica(identificativoLegame.Value, (byte)idTabellaLegame, (byte)idNotifica, note);
        }

        #endregion
    }
}
