using System;
using SeCoGEST.Infrastructure;

namespace SeCoGEST.Logic.Sicurezza
{
    public class SecurityManager : Base.LogicLayerBase
    {
        Entities.Account accountCorrente;


        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.Accounts dal;


        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public SecurityManager(Entities.Account account)
            : base(false)
        {
            CreateDal(account);
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public SecurityManager(bool createStandaloneContext, Entities.Account account)
            : base(createStandaloneContext)
        {
            CreateDal(account);
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public SecurityManager(Base.LogicLayerBase logicLayer, Entities.Account account)
            : base(logicLayer)
        {
            CreateDal(account);
        }


        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDal(Entities.Account account)
        {
            if (account == null)
            {
                throw new Exception(ErrorMessage.UTENTE_SCONOSCIUTO_MESSAGE);
            }
            else if (account.Bloccato.HasValue && account.Bloccato.Value)
            {
                throw new Exception(ErrorMessage.ACCESSO_NEGATO_MESSAGE);
            }

            accountCorrente = account;

            dal = new Data.Accounts(this.context);
        }

        #endregion
        

        //#region Gestione autorizzazioni

        ///// <summary>
        ///// Restituisce un oggetto AutorizzazioniAccount relativo alle autorizzazioni dell'account legato all'istanza per l'azienda e l'area indicate
        ///// </summary>
        ///// <param name="azienda"></param>
        ///// <param name="area"></param>
        ///// <returns>Carica tutte le autorizzazioni per l'area passata sia a livello di Ruolo che di Account e le unisce in un solo oggetto AutorizzazioniAccount</returns>
        //public Entities.Sicurezza.AutorizzazioniAccount GetAutorizzazioniAccount(Entities.AnagraficaAzienda azienda, Entities.Sicurezza.AutorizzazioniAreeEnum area)
        //{
        //    Entities.Sicurezza.AutorizzazioniAccount autorizzazioni = new Entities.Sicurezza.AutorizzazioniAccount();

        //    autorizzazioni.Azienda = azienda;
        //    autorizzazioni.Area = area;
        //    autorizzazioni.Consenti_Funzionalità = false;
        //    autorizzazioni.Consenti_Visibilità = false;
        //    autorizzazioni.Consenti_Inserimento = false;
        //    autorizzazioni.Consenti_Modifica = false;
        //    autorizzazioni.Consenti_Eliminazione = false;

        //    if (accountCorrente == null)
        //    {
        //        return autorizzazioni;
        //    }

        //    if (accountCorrente.Amministratore.HasValue && accountCorrente.Amministratore.Value)
        //    {
        //        autorizzazioni.Consenti_Funzionalità = true;
        //        autorizzazioni.Consenti_Visibilità = true;
        //        autorizzazioni.Consenti_Inserimento = true;
        //        autorizzazioni.Consenti_Modifica = true;
        //        autorizzazioni.Consenti_Eliminazione = true;

        //        return autorizzazioni;
        //    }



        //    // Se l'account è associato ad un Soggetto allora carica tutte le possibili autorizzazioni legate ai propri ruoli
        //    if (accountCorrente.AnagraficaSoggetto != null)
        //    {
        //        Logic.Accademia.RuoliSoggetti llRS = new Accademia.RuoliSoggetti(this);
        //        Logic.Applicazione.AutorizzazioniRuolo llAR = new Applicazione.AutorizzazioniRuolo(this);
        //        Entities.ApplicazioneAutorizzazioneRuolo autorizzazioniRuolo;
        //        foreach(Entities.AccademiaRuoloSoggetto ruoloSoggetto in llRS.Read(accountCorrente.AnagraficaSoggetto, azienda))
        //        {
        //            autorizzazioniRuolo = llAR.Find(azienda.Id, ruoloSoggetto.IDRuolo, area);
        //            if (autorizzazioniRuolo != null)
        //            {
        //                // Registra le autorizzazioni mantenendo le abilitazioni per ogni ruolo
        //                if (!autorizzazioni.Consenti_Funzionalità) autorizzazioni.Consenti_Funzionalità = autorizzazioniRuolo.Consenti_Funzionalità;
        //                if (!autorizzazioni.Consenti_Visibilità) autorizzazioni.Consenti_Visibilità = autorizzazioniRuolo.Consenti_Visibilità;
        //                if (!autorizzazioni.Consenti_Inserimento) autorizzazioni.Consenti_Inserimento = autorizzazioniRuolo.Consenti_Inserimento;
        //                if (!autorizzazioni.Consenti_Modifica) autorizzazioni.Consenti_Modifica = autorizzazioniRuolo.Consenti_Modifica;
        //                if (!autorizzazioni.Consenti_Eliminazione) autorizzazioni.Consenti_Eliminazione = autorizzazioniRuolo.Consenti_Eliminazione;
        //            }
        //        }
        //    }


        //    // Carica le autorizzazioni associate direttamente all'account
        //    Logic.Applicazione.AutorizzazioniAccount llAA = new Applicazione.AutorizzazioniAccount(this);
        //    Entities.ApplicazioneAutorizzazioneAccount autorizzazioniAccount = llAA.Find(azienda.Id, accountCorrente.Id, area);

        //    if (autorizzazioniAccount != null)
        //    {
        //        if (!autorizzazioni.Consenti_Funzionalità) autorizzazioni.Consenti_Funzionalità = autorizzazioniAccount.Consenti_Funzionalità;
        //        if (!autorizzazioni.Consenti_Visibilità) autorizzazioni.Consenti_Visibilità = autorizzazioniAccount.Consenti_Visibilità;
        //        if (!autorizzazioni.Consenti_Inserimento) autorizzazioni.Consenti_Inserimento = autorizzazioniAccount.Consenti_Inserimento;
        //        if (!autorizzazioni.Consenti_Modifica) autorizzazioni.Consenti_Modifica = autorizzazioniAccount.Consenti_Modifica;
        //        if (!autorizzazioni.Consenti_Eliminazione) autorizzazioni.Consenti_Eliminazione = autorizzazioniAccount.Consenti_Eliminazione;
        //    }

        //    return autorizzazioni;
        //}

        ///// <summary>
        ///// Restituisce un oggetto AutorizzazioniAccount relativo alle autorizzazioni dell'account legato all'istanza 
        ///// senza tenere conto dell'azienda e dell'area ma semplicemente analizzando l'accesso da parte di un amministratore
        ///// </summary>
        ///// <returns>Se l'account è relativo ad un amministratore allora restituisce completo accesso altrimenti complete negazioni</returns>
        //public Entities.Sicurezza.AutorizzazioniAccount GetAutorizzazioniAccount()
        //{
        //    Entities.Sicurezza.AutorizzazioniAccount autorizzazioni = new Entities.Sicurezza.AutorizzazioniAccount();

        //    autorizzazioni.Consenti_Funzionalità = false;
        //    autorizzazioni.Consenti_Visibilità = false;
        //    autorizzazioni.Consenti_Inserimento = false;
        //    autorizzazioni.Consenti_Modifica = false;
        //    autorizzazioni.Consenti_Eliminazione = false;

        //    if (accountCorrente == null)
        //    {
        //        return autorizzazioni;
        //    }

        //    if (accountCorrente.Amministratore.HasValue && accountCorrente.Amministratore.Value)
        //    {
        //        autorizzazioni.Consenti_Funzionalità = true;
        //        autorizzazioni.Consenti_Visibilità = true;
        //        autorizzazioni.Consenti_Inserimento = true;
        //        autorizzazioni.Consenti_Modifica = true;
        //        autorizzazioni.Consenti_Eliminazione = true;

        //        return autorizzazioni;
        //    }

        //    return autorizzazioni;
        //}


        //#endregion

        #region Gestione Login e credenziali di accesso

        /// <summary>
        /// Restituisce un valore dell'enumeratore LoginResponseEnum relativo alla risposta di login dell'utente passato
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static LoginResponseEnum Login(string username, string password)
        {            
            //string encryptedPassword = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password.Trim(), "SHA1");

            //Effettua il controllo delle credenziali di accesso dell'utente utilizzando un DataContext privato
            Accounts llU = new Accounts(true);
            Entities.Account utente = llU.Find(username);

            if (utente == null || utente.Password != SeCoGes.Utilities.PasswordHelper.GetHashPasswordForStoringInConfigFile(password)) utente = null;
            if (utente == null)
            {
                return LoginResponseEnum.UtenteSconosciuto;
            }
            else
            {
                if (utente.Bloccato.HasValue && utente.Bloccato.Value)
                {
                    return LoginResponseEnum.UtenteBloccato;
                }

                if (utente.ScadenzaPassword < DateTime.Today)
                {
                    return LoginResponseEnum.PasswordScaduta;
                }

                return LoginResponseEnum.AccessoConsentito;
            }
        }

        /// <summary>
        /// Restituisce un valore dell'enumeratore LoginResponseEnum relativo allo stato di Login dell'utente autenticato.
        /// In pratica ritenta il login dell'utente già autenticato e restituisce il risultato ottenuto.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static LoginResponseEnum GetLoginStatus(Entities.Account account)
        {
            if (account == null)
            {
                return LoginResponseEnum.UtenteSconosciuto;
            }
            else
            {
                if (!account.Bloccato.HasValue || account.Bloccato.Value)
                {
                    return LoginResponseEnum.UtenteBloccato;
                }

                if (account.ScadenzaPassword < DateTime.Today)
                {
                    return LoginResponseEnum.PasswordScaduta;
                }

                return LoginResponseEnum.AccessoConsentito;
            }
        }

        /// <summary>
        /// Modifica la password dell'utente come indicato tramite i parametri passati
        /// </summary>
        /// <param name="account"></param>
        /// <param name="newPassword"></param>
        public static void ChangeUserPassword(Entities.Account account, string newPassword)
        {
            //string newPasswordEncrypted = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, "SHA1");
            string newPasswordEncrypted = SeCoGes.Utilities.PasswordHelper.GetHashPasswordForStoringInConfigFile(newPassword);

            if (account.Password == newPasswordEncrypted)
            {
                throw new Exception("E' necessario inserire una password diversa da quella attuale.");
            }
            else
            {
                account.Password = newPasswordEncrypted;
                account.ScadenzaPassword = DateTime.Today.AddMonths(6);
            }
        }

        #endregion
    }
}
