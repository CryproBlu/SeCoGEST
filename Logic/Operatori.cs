using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGEST.Entities;
using SeCoGes.Utilities;

namespace SeCoGEST.Logic
{
    public class Operatori : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.Operatori dalOperatori;

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Logic.AccountsOperatori llAccountsOperatori;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public Operatori()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public Operatori(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public Operatori(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dalOperatori = new Data.Operatori(this.context);
            llAccountsOperatori = new Logic.AccountsOperatori(this);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Operatore entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                if (entityToCreate.ID.Equals(Guid.Empty))
                {
                    entityToCreate.ID = Guid.NewGuid();
                }

                // Salvataggio nel database
                dalOperatori.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'Operatore': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Operatore> Read()
        {
            return from u in dalOperatori.Read() orderby u.Area descending, u.CognomeNome ascending select u;
        }

        public IQueryable<Entities.Operatore> ReadActive()
        {
            return from u in dalOperatori.Read() where u.Attivo orderby u.Area descending, u.CognomeNome ascending select u;
        }

        public IQueryable<Entities.Operatore> ReadActiveOperators()
        {
            return from u in dalOperatori.Read() where !u.Area && u.Attivo orderby u.CognomeNome ascending select u;
            //return from u in dalOperatori.Read() where !u.Area && u.Attivo orderby u.CognomeNome ascending select u;
        }

        /// <summary>
        /// Restituisce l'elenco degli operatori che non risultano associati all'account il cui identificativo è pasato come parametro
        /// </summary>
        /// <param name="identifierAccount"></param>
        /// <returns></returns>
        public IQueryable<Entities.Operatore> ReadOperatoriAccountNonAssociati(EntityId<Account> identifierAccount)
        {
            if (identifierAccount == null) throw new ArgumentNullException("identifierAccount", "Parametro nullo");

            IQueryable<Guid> elencoIDOperatoriAssociati = llAccountsOperatori.Read(identifierAccount).Select(x => x.IDOperatore).Distinct();
            return Read().Where(x => !elencoIDOperatoriAssociati.Contains(x.ID));
        }


        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.Operatore Find(EntityId<Operatore> idToFind)
        {
            return dalOperatori.Find(idToFind.Value);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'cognome e nome passato e null se non trova l'entity
        /// </summary>
        /// <param name="cognomeOperatoreToFind"></param>
        /// <param name="nomeOperatoreToFind"></param>
        /// <returns></returns>
        public Entities.Operatore Find(EntityString<Operatore> cognomeOperatoreToFind, EntityString<Operatore> nomeOperatoreToFind)
        {
            return dalOperatori.Find(cognomeOperatoreToFind.Value, nomeOperatoreToFind.Value);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Operatore entityToDelete, bool submitChanges)
        {
            Delete(entityToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Operatore entityToDelete, bool checkAllegati, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dalOperatori.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'Operatore': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.Operatore> entitiesToDelete, bool submitChanges)
        {
            Delete(entitiesToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.Operatore> entitiesToDelete, bool checkAllegati, bool submitChanges)
        {
            if (entitiesToDelete != null)
            {
                if (entitiesToDelete.Count() > 0)
                {
                    entitiesToDelete.ToList().ForEach(x => Delete(x, checkAllegati, submitChanges));
                }
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'Operatore': parametro nullo!");
            }
        }

        #endregion

        #region Funzioni Specifiche

        /// <summary>
        /// Restituisce tutte le email valide, dei resposabili, in modo univoco, dall'operatore passato come parametro
        /// </summary>
        /// <param name="operatore"></param>
        /// <param name="restituisciEmailUnivoche"></param>
        /// <returns></returns>
        public List<string> GetValidEmailsListResponsabileFromOperatore(Operatore operatore, bool restituisciEmailUnivoche = true)
        {
            List<string> elencoIndirizziEmailValidi = new List<string>();
            List<string> elencoEmailResponsabileValide = Helper.EmailHelper.GetEmailsValidList(operatore.EmailResponsabile);
            if (elencoEmailResponsabileValide != null && elencoEmailResponsabileValide.Count > 0)
            {
                foreach (string emailResponsabile in elencoEmailResponsabileValide)
                {
                    if (!elencoIndirizziEmailValidi.Contains(emailResponsabile))
                    {
                        elencoIndirizziEmailValidi.Add(emailResponsabile);
                    }
                }
            }

            return elencoIndirizziEmailValidi;
        }

        /// <summary>
        /// Restituisce tutte le email valide, in modo univoco, dall'operatore passato come parametro
        /// </summary>
        /// <param name="operatore"></param>
        /// <param name="restituisciEmailUnivoche"></param>
        /// <param name="aggiungiEmailResponsabileSeAlloAccountNonSonoAssociateEmail"></param>
        /// <returns></returns>
        public List<string> GetValidEmailsListFromOperatore(Operatore operatore, bool restituisciEmailUnivoche = true, bool aggiungiEmailResponsabileSeAlloAccountNonSonoAssociateEmail = true, bool soloPerNotifiche = true)
        {
            if (operatore == null)
            {
                throw new ArgumentNullException("operatore", "Parametro nullo");
            }


            List<string> elencoIndirizziEmailValidi = new List<string>();
            bool inviaEmailAlResponsabile = false;

            if(operatore.Area)
            {
                inviaEmailAlResponsabile = true;
            }
            else
            {
                IQueryable<Entities.Account> elencoAccount;
                if (soloPerNotifiche)
                {
                    elencoAccount = llAccountsOperatori.ReadAccountAbilitatiNotifiche(operatore.Identifier);
                }
                else
                {
                    elencoAccount = llAccountsOperatori.ReadAccount(operatore.Identifier);
                }
                //IQueryable<Entities.AccountOperatore> elencoAccountOperatore = llAccountsOperatori.Read().Where(x => x.IDOperatore == operatore.Identifier.Value);

                //IQueryable<Entities.Account> elencoAccount = llAccountsOperatori.ReadAccountAbilitatiNotifiche(operatore.Identifier);
                if (elencoAccount != null && elencoAccount.Count() > 0)
                {
                    foreach (Entities.Account account in elencoAccount)
                    {
                        List<string> elencoEmailValide = Helper.EmailHelper.GetEmailsValidList(account.Email);
                        if (elencoEmailValide != null && elencoEmailValide.Count > 0)
                        {
                            foreach (string email in elencoEmailValide)
                            {
                                // Nel caso in cui sia stato indicato di non restituire l'elenco univoco oppure 
                                // l'elenco delle email non contiene l'email corrente ..
                                if (!restituisciEmailUnivoche || !elencoIndirizziEmailValidi.Contains(email))
                                {
                                    // Viene aggiunta l'email
                                    elencoIndirizziEmailValidi.Add(email);
                                }
                            }
                        }
                    }
                }
                else
                {
                    inviaEmailAlResponsabile = aggiungiEmailResponsabileSeAlloAccountNonSonoAssociateEmail;
                }
            }




            if (inviaEmailAlResponsabile)
            {
                List<string> elencoEmailResponsabileValide = Helper.EmailHelper.GetEmailsValidList(operatore.EmailResponsabile);
                if (elencoEmailResponsabileValide != null && elencoEmailResponsabileValide.Count > 0)
                {
                    foreach (string emailResponsabile in elencoEmailResponsabileValide)
                    {
                        // Nel caso in cui sia stato indicato di non restituire l'elenco univoco oppure 
                        // l'elenco delle email non contiene l'email corrente ..
                        if (!restituisciEmailUnivoche || !elencoIndirizziEmailValidi.Contains(emailResponsabile))
                        {
                            // Viene aggiunta l'email
                            elencoIndirizziEmailValidi.Add(emailResponsabile);
                        }
                    }
                }
            }

            return elencoIndirizziEmailValidi;
        }

        #endregion
    }
}
