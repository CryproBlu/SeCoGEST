using SeCoGEST.Entities;
using SeCoGEST.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Logic
{
    public class OfferteAccountValidatori : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.OfferteAccountValidatori dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public OfferteAccountValidatori()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public OfferteAccountValidatori(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public OfferteAccountValidatori(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.OfferteAccountValidatori(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.OffertaAccountValidatore entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                if (entityToCreate.IDOfferta.Equals(Guid.Empty)) throw new Exception("L'identificativo dell'offerta non è stato valorizzato");
                if (entityToCreate.IDAccount.Equals(Guid.Empty)) throw new Exception("L'identificativo dell'account non è stato valorizzato");

                // Salvataggio nel database
                dal.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'OffertaAccountValidatore': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaAccountValidatore> Read()
        {
            return from u in dal.Read() orderby u.Offerta.Numero descending, u.Account.Nominativo select u;
        }

        /// <summary>
        /// Restituisce l'entity relativa agli identificativi passati e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.OffertaAccountValidatore Find(EntityId<Offerta> identificativoOfferta, EntityId<Account> identificativoAccount)
        {
            return Find(identificativoOfferta.Value, identificativoAccount.Value);
        }

        /// <summary>
        /// Restituisce l'entity relativa agli identificativi passati e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        private Entities.OffertaAccountValidatore Find(Guid idOfferta, Guid idAccount)
        {
            return dal.Find(idOfferta, idAccount);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.OffertaAccountValidatore entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'OffertaAccountValidatore': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.OffertaAccountValidatore> entitiesToDelete, bool submitChanges)
        {
            if (entitiesToDelete != null)
            {
                if (entitiesToDelete.Count() > 0)
                {
                    entitiesToDelete.ToList().ForEach(x => Delete(x, submitChanges));
                }
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'OffertaAccountValidatore': parametro nullo!");
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Effettua la clonazione dell'entità passata come parametro
        /// </summary>
        /// <param name="entityToClone"></param>
        /// <returns></returns>
        public OffertaAccountValidatore Clone(OffertaAccountValidatore entityToClone)
        {
            OffertaAccountValidatore entity = new OffertaAccountValidatore();
            entity.IDOfferta = entityToClone.IDOfferta;
            entity.IDAccount = entityToClone.IDAccount;
            entity.EffettuatoValidazione = entityToClone.EffettuatoValidazione;
            entity.DataOraValidazione = entityToClone.DataOraValidazione;

            return entity;
        }

        /// <summary>
        /// Restituisce tutte le entities in base all'identificativo dell'offerta passato come parametro
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaAccountValidatore> Read(EntityId<Offerta> identificativoOfferta)
        {
            Guid idToCompare = (identificativoOfferta?.Value ?? Guid.Empty);
            return Read().Where(x => x.IDOfferta == idToCompare);
        }


        /// <summary>
        /// Verifica se l'offerta indicata può essere validata dall'account il cui identificativo è passato come parametro
        /// </summary>
        /// <param name="identificativoOfferta"></param>
        /// <param name="identificativoAccount"></param>
        /// <returns></returns>
        public bool CanValidateOfferta(EntityId<Offerta> identificativoOfferta, EntityId<Account> identificativoAccount)
        {
            if (identificativoOfferta == null) throw new ArgumentNullException(nameof(identificativoOfferta), "Parametro nullo");
            if (identificativoAccount == null) throw new ArgumentNullException(nameof(identificativoAccount), "Parametro nullo");

            Sicurezza.Accounts llAccount = new Sicurezza.Accounts(this);
            Account account = llAccount.Find(identificativoAccount);

            if (account == null) throw new Exception($"L'account con ID \"{identificativoAccount.Value}\" non esiste nella fonte dati");

            Logic.Offerte llOfferte = new Offerte(this);
            Offerta offerta = llOfferte.Find(identificativoOfferta);

            if (offerta == null) throw new Exception($"L'offerta con ID \"{identificativoOfferta.Value}\" non esiste nella fonte dati");

            bool canValidateOfferta = false;

            if (offerta.StatoEnum == StatoOffertaEnum.InValidazione)
            {
                if (account.Amministratore.HasValue && account.Amministratore.Value)
                {
                    canValidateOfferta = true;
                }
                else
                {
                    OffertaAccountValidatore offertaAccount = Find(identificativoOfferta, identificativoAccount);
                    canValidateOfferta = (offertaAccount != null);
                }
            }

            return canValidateOfferta;
        }

        #endregion
    }
}
