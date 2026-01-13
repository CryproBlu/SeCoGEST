using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeCoGEST.Logic.Sicurezza
{
    public class Accounts : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.Accounts dal;


        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public Accounts()
            : base(false)
        {
            CreateDal();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public Accounts(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDal();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public Accounts(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDal();
        }


        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDal()
        {
            dal = new Data.Accounts(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="utenteCheEsegueOperazione"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Account entityToCreate, Entities.Account utenteCheEsegueOperazione, bool submitChanges)
        {
            if (entityToCreate != null && utenteCheEsegueOperazione != null)
            {
                //Modifica dati
                if (entityToCreate.ID.Equals(Guid.Empty))
                {
                    entityToCreate.ID = Guid.NewGuid();
                }

                //entityToCreate.DataCreazione = DateTime.Now;
                //entityToCreate.UtenteCreazione = utenteCheEsegueOperazione.UserName;
                //entityToCreate.DataModifica = entityToCreate.DataCreazione;
                //entityToCreate.UtenteModifica = utenteCheEsegueOperazione.UserName;

                //Submit
                dal.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'Account': parametro nullo!");
            }

        }

        /// <summary>
        /// Restituisce tutti gli Accounts
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Account> Read()
        {
            return from u in dal.Read() orderby u.UserName select u;
        }

        /// <summary>
        /// Restituisce tutte le entity filtrate in base al parametro email ed in base allo stato di blocco
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public IQueryable<Entities.Account> ReadByEmail(string email)
        {
            return this.Read().Where(x => x.Email == email);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="identificativo"></param>
        /// <returns></returns>
        public Entities.Account Find(Entities.EntityId<Entities.Account> identificativo)
        {
            return Find(identificativo?.Value ?? Guid.Empty);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.Account Find(Guid idToFind)
        {
            return dal.Find(idToFind);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'username passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.Account Find(string userName)
        {
            return dal.Read().Where(x => x.UserName == userName).SingleOrDefault();
        }



        /// <summary>
        /// Aggiorna l'entity passata e, a richiesta, fa il submit
        /// </summary>
        /// <param name="entityToUpdate"></param>
        /// <param name="utenteCheEsegueOperazione"></param>
        /// <param name="submitChanges"></param>
        public void Update(Entities.Account entityToUpdate, Entities.Account utenteCheEsegueOperazione, bool submitChanges)
        {
            if (entityToUpdate != null && utenteCheEsegueOperazione != null)
            {
                //Modifica dati
                //entityToUpdate.DataModifica = DateTime.Now;
                //entityToUpdate.UtenteModifica = utenteCheEsegueOperazione.UserName;

                //Submit
                if (submitChanges == true)
                {
                    context.SubmitChanges();
                }
            }
            else
            {
                throw new ArgumentNullException("Errore aggiornamento entity 'Account': parametro nullo!");
            }

        }



        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Account entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'Account': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.Account> entitiesToDelete, bool submitChanges)
        {
            if (entitiesToDelete != null)
            {
                dal.Delete(entitiesToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'Account': parametro nullo!");
            }
        }

        #endregion

        #region Funzioni Specifiche

        /// <summary>
        /// Effettua l'invio di creazione dell'account passato come parametro
        /// </summary>
        /// <param name="accountCreato"></param>
        /// <param name="passwordInChiaro"></param>
        public void InviaEmailCreazioneAccount(Entities.Account accountCreato, string passwordInChiaro)
        {
            if (accountCreato == null) throw new ArgumentNullException("accountCreato", "Parametro nullo");
            if (String.IsNullOrEmpty(passwordInChiaro)) throw new ArgumentNullException("passwordInChiaro", "Parametro nullo");

            string corpoHTML = CreaCorpoHtmlPerEmailCreazioneAccount(accountCreato, passwordInChiaro);
            string corpoPlainText = CreaCorpoPlainTextPerEmailCreazioneAccount(accountCreato, passwordInChiaro);
            string oggetto = String.Join(" - ", Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE, "Registrazione al Ticket System");
            string mittente = Infrastructure.ConfigurationKeys.MITTENTE_EMAIL_PER_CREAZIONE_ACCOUNT_DI_ACCESSO;

            string[] destinatari = new string[] { accountCreato.Email };

            Helper.EmailHelper.InviaEmail(oggetto, corpoPlainText, corpoHTML, mittente, destinatari);
        }

        /// <summary>
        /// Effettua la lettura degli accoutn validatori che non risultano bloccati
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Account> ReadValidatori()
        {
            return Read().Where(x => x.ValidatoreOfferta && (!x.Bloccato.HasValue || x.Bloccato.Value));
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Effettua la creazione del corpo in html per l'email relativa alla creazione di un account
        /// </summary>
        /// <param name="accountCreato"></param>
        /// <param name="passwordInChiaro"></param>
        /// <returns></returns>
        private string CreaCorpoHtmlPerEmailCreazioneAccount(Entities.Account accountCreato, string passwordInChiaro)
        {
            if (accountCreato == null) throw new ArgumentNullException("accountCreato", "Parametro nullo");

            StringBuilder sb = new StringBuilder();

            string clienteNominativo = (accountCreato.TipologiaEnum == Entities.TipologiaAccountEnum.ClienteStandard || accountCreato.TipologiaEnum == Entities.TipologiaAccountEnum.ClienteAdmin || accountCreato.TipologiaEnum == Entities.TipologiaAccountEnum.ClienteSupervisore) ? "Cliente" : accountCreato.Nominativo;
            sb.AppendLine(String.Format("Gentile {0}, grazie per esserti registrato al nostro Sistema di Ticketing Online.{1}", clienteNominativo, Helper.Web.HTML_NEW_LINE));
            sb.AppendLine(String.Format("Per accedere al Ticket System basta cliccare sul seguente link <a href=\"{0}\">{0}</a>, utilizzando le seguenti credenziali:{1}", Infrastructure.ConfigurationKeys.URL_APPLICAZIONE, Helper.Web.HTML_NEW_LINE));
            sb.AppendLine(Helper.Web.HTML_NEW_LINE);
            sb.AppendLine(String.Format("Username: {0}{1}", accountCreato.UserName, Helper.Web.HTML_NEW_LINE));
            sb.AppendLine(String.Format("Password: {0}{1}", passwordInChiaro, Helper.Web.HTML_NEW_LINE));
            sb.AppendLine(Helper.Web.HTML_NEW_LINE);
            sb.AppendLine("Quando avrai effettuato l'accesso con il tuo account sarai ini grado di:");
            sb.AppendLine("<ul style=\"list-style-type:disc;\">");
            sb.AppendLine("<li>Aprire le richieste di assistenza</li>");
            sb.AppendLine("<li>Verificare lo stato delle assistenze</li>");
            sb.AppendLine("<li>Modificare le informazioni relative al tuo account</li>");
            sb.AppendLine("<li>Cambiare la tua password (da fare dopo il primo accesso)</li>");
            sb.AppendLine("</ul>");
            sb.AppendLine(Helper.Web.HTML_NEW_LINE);
            sb.AppendLine(String.Format("Per qualsiasi chiarimento contattaci via e-mail all’indirizzo <a href=\"mailto:{0}\">{0}</a> oppure telefonicamente al numero {1}.{2}", Infrastructure.ConfigurationKeys.DESTINATARIO_PREDEFINITO_EMAIL, Infrastructure.ConfigurationKeys.NUMERO_TELEFONO_AZIENDA, Helper.Web.HTML_NEW_LINE));
            sb.AppendLine(String.Format("Cordiali saluti.{0}", Helper.Web.HTML_NEW_LINE));
            sb.AppendLine(Helper.Web.HTML_NEW_LINE);
            sb.AppendLine(String.Format("\"Servizio Clienti\".{0}", Helper.Web.HTML_NEW_LINE));

            return sb.ToString();
        }

        /// <summary>
        /// Effettua la creazione del corpo plain text per l'email relativa alla creazione di un account
        /// </summary>
        /// <param name="accountCreato"></param>
        /// <param name="passwordInChiaro"></param>
        /// <returns></returns>
        private string CreaCorpoPlainTextPerEmailCreazioneAccount(Entities.Account accountCreato, string passwordInChiaro)
        {
            if (accountCreato == null) throw new ArgumentNullException("accountCreato", "Parametro nullo");

            StringBuilder sb = new StringBuilder();

            string clienteNominativo = (accountCreato.TipologiaEnum == Entities.TipologiaAccountEnum.ClienteStandard || accountCreato.TipologiaEnum == Entities.TipologiaAccountEnum.ClienteAdmin || accountCreato.TipologiaEnum == Entities.TipologiaAccountEnum.ClienteSupervisore) ? "Cliente" : accountCreato.Nominativo;
            sb.AppendLine(String.Format("Gentile {0}, grazie per esserti registrato al nostro Sistema di Ticketing Online.{1}", clienteNominativo, Helper.Web.PLAIN_TEXT_NEW_LINE));
            sb.AppendLine(String.Format("Per accedere al Ticket System basta cliccare sul seguente link \"{0}\", utilizzando le seguenti credenziali:{1}", Infrastructure.ConfigurationKeys.URL_APPLICAZIONE, Helper.Web.PLAIN_TEXT_NEW_LINE));
            sb.AppendLine(Helper.Web.PLAIN_TEXT_NEW_LINE);
            sb.AppendLine(String.Format("Username: {0}{1}", accountCreato.UserName, Helper.Web.PLAIN_TEXT_NEW_LINE));
            sb.AppendLine(String.Format("Password: {0}{1}", passwordInChiaro, Helper.Web.PLAIN_TEXT_NEW_LINE));
            sb.AppendLine(Helper.Web.PLAIN_TEXT_NEW_LINE);
            sb.AppendLine(String.Format("Quando avrai effettuato l'accesso con il tuo account, sarai ini grado di:{0}", Helper.Web.PLAIN_TEXT_NEW_LINE));
            sb.AppendLine(String.Format("   - Aprire le richieste di assistenza{0}", Helper.Web.PLAIN_TEXT_NEW_LINE));
            sb.AppendLine(String.Format("   - Verificare lo stato delle assistenze{0}", Helper.Web.PLAIN_TEXT_NEW_LINE));
            sb.AppendLine(String.Format("   - Modificare le informazioni relative al tuo account{0}", Helper.Web.PLAIN_TEXT_NEW_LINE));
            sb.AppendLine(String.Format("   - Cambiare la tua password (da fare dopo il primo accesso){0}", Helper.Web.PLAIN_TEXT_NEW_LINE));
            sb.AppendLine(Helper.Web.PLAIN_TEXT_NEW_LINE);
            sb.AppendLine(String.Format("Per qualsiasi chiarimento contattaci via e-mail all’indirizzo {0} oppure telefonicamente al numero {1}.{2}", Infrastructure.ConfigurationKeys.DESTINATARIO_PREDEFINITO_EMAIL, Infrastructure.ConfigurationKeys.NUMERO_TELEFONO_AZIENDA, Helper.Web.PLAIN_TEXT_NEW_LINE));
            sb.AppendLine(String.Format("Cordiali saluti.{0}", Helper.Web.PLAIN_TEXT_NEW_LINE));
            sb.AppendLine(Helper.Web.PLAIN_TEXT_NEW_LINE);
            sb.AppendLine(String.Format("\"Servizio Clienti\".{0}", Helper.Web.PLAIN_TEXT_NEW_LINE));

            return sb.ToString();
        }

        #endregion
    }
}
