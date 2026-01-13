using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Logic
{
    public class AccountsOperatori : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.AccountsOperatori dal;


        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public AccountsOperatori()
            : base(false)
        {
            CreateDal();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public AccountsOperatori(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDal();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public AccountsOperatori(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDal();
        }


        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDal()
        {
            dal = new Data.AccountsOperatori(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="identifierAccount"></param>
        /// <param name="identifierOperatore"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.EntityId<Entities.Account> identifierAccount, Entities.EntityId<Entities.Operatore> identifierOperatore, bool submitChanges)
        {
            if (identifierAccount == null) throw new ArgumentNullException("identifierAccount", "Parametro nullo");
            if (identifierOperatore == null) throw new ArgumentNullException("identifierOperatore", "Parametro nullo");

            Entities.AccountOperatore entityAccountOperatore = new Entities.AccountOperatore();
            entityAccountOperatore.IdentifierAccount = identifierAccount;
            entityAccountOperatore.IdentifierOperatore = identifierOperatore;

            dal.Create(entityAccountOperatore, submitChanges);
        }

        /// <summary>
        /// Restituisce tutte le associazioni tra gli operatori e gli account presenti nella fonte dati
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AccountOperatore> Read()
        {
            return from u in dal.Read() orderby u.Operatore.CognomeNome select u;
        }

        /// <summary>
        /// Restituisce tutte le associazioni degli operatori associati all'account il cui id è passato come parametro
        /// </summary>
        /// <param name="identifierAccount"></param>
        /// <returns></returns>
        public IQueryable<Entities.AccountOperatore> Read(Entities.EntityId<Entities.Account> identifierAccount)
        {
            if (identifierAccount == null) throw new ArgumentNullException("identifierAccount", "Parametro nullo");

            return Read().Where(x => x.IDAccount == identifierAccount.Value);
        }

        /// <summary>
        /// Restituisce tutti gli operatori associati all'account il cui id è passato come parametro
        /// </summary>
        /// <param name="identifierAccount"></param>
        /// <returns></returns>
        public IQueryable<Entities.Operatore> ReadOperatori(Entities.EntityId<Entities.Account> identifierAccount)
        {
            if (identifierAccount == null) throw new ArgumentNullException("identifierAccount", "Parametro nullo");

            return Read().Where(x => x.IDAccount == identifierAccount.Value).Select(x=> x.Operatore);
        }

        /// <summary>
        /// Restituisce tutti gli account associati all'operatore il cui id è passato come parametro
        /// </summary>
        /// <param name="identifierOperatore"></param>
        /// <returns></returns>
        public IQueryable<Entities.Account> ReadAccount(Entities.EntityId<Entities.Operatore> identifierOperatore)
        {
            if (identifierOperatore == null) throw new ArgumentNullException("identifierOperatore", "Parametro nullo");

            return Read().Where(x => x.IDOperatore == identifierOperatore.Value).Select(x=> x.Account);
        }

        public IQueryable<Entities.Account> ReadAccountAbilitatiNotifiche(Entities.EntityId<Entities.Operatore> identifierOperatore)
        {
            if (identifierOperatore == null) throw new ArgumentNullException("identifierOperatore", "Parametro nullo");

            return Read().Where(x => x.IDOperatore == identifierOperatore.Value && x.InviaNotifiche.HasValue && x.InviaNotifiche.Value).Select(x => x.Account);
        }

        /// <summary>
        /// Restituisce l'entity in base agli identificativi passati e null se non trova l'entity
        /// </summary>
        /// <param name="identifierAccount"></param>
        /// <param name="identifierOperatore"></param>
        /// <returns></returns>
        public Entities.AccountOperatore Find(Entities.EntityId<Entities.Account> identifierAccount, Entities.EntityId<Entities.Operatore> identifierOperatore)
        {
            if (identifierAccount == null) throw new ArgumentNullException("identifierAccount", "Parametro nullo");
            if (identifierOperatore == null) throw new ArgumentNullException("identifierOperatore", "Parametro nullo");

            return dal.Find(identifierAccount.Value, identifierOperatore.Value);
        }

        /// <summary>
        /// Restituisce true se esiste un record di relazione tra gli id passati come parametro
        /// </summary>
        /// <param name="identifierAccount"></param>
        /// <param name="identifierOperatore"></param>
        /// <returns></returns>
        public bool EsisteRelazione(Entities.EntityId<Entities.Account> identifierAccount, Entities.EntityId<Entities.Operatore> identifierOperatore)
        {
            if (identifierAccount == null) throw new ArgumentNullException("identifierAccount", "Parametro nullo");
            if (identifierOperatore == null) throw new ArgumentNullException("identifierOperatore", "Parametro nullo");

            return dal.EsisteRelazione(identifierAccount.Value, identifierOperatore.Value);
        }
                
        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.AccountOperatore entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'AccountOperatore': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.AccountOperatore> entitiesToDelete, bool submitChanges)
        {
            if (entitiesToDelete != null)
            {
                dal.Delete(entitiesToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'AccountOperatore': parametro nullo!");
            }
        }

        #endregion
    }
}
