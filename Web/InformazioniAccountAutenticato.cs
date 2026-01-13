using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using SeCoGes.Utilities;
using SeCoGEST.Entities;

namespace SeCoGEST.Web
{
    public class InformazioniAccountAutenticato
    {
        public string Username { get; private set; }

        /// <summary>
        /// Restituisce l'Entity ApplicazioneAccount relativa all'Account attualmente autenticato
        /// </summary>
        public Entities.Account Account { get; private set; }
        
        public InformazioniSessione SessioneCorrente
        {
            get
            {
                IList<InformazioniAccountAutenticato> asi = (List<InformazioniAccountAutenticato>)HttpContext.Current.Application["InformazioniAccountAutenticato"];
                if (asi != null)
                {
                    InformazioniAccountAutenticato informazioniAccount = asi.FirstOrDefault(x => x.Username.ToLower() == HttpContext.Current.User.Identity.Name.ToLower());
                    if (informazioniAccount != null)
                    {
                        return informazioniAccount.WebSession.FirstOrDefault(x => x.IDSessione == HttpContext.Current.Session.SessionID);
                    }
                }
                return null;
            }
        }

        
        private IList<InformazioniSessione> WebSession { get; set; }



        /// <summary>
        /// Costruttore Privato non richiamabile
        /// </summary>
        private InformazioniAccountAutenticato()
        {
        }


        /// <summary>
        /// Ritorno una Singleton
        /// </summary>
        /// <returns></returns>
        public static InformazioniAccountAutenticato GetIstance()
        {
            // Controlla che in Application sia già memorizzata una istanza della lista di informazioni sugli accounts autenticati
            IList<InformazioniAccountAutenticato> asi;

            if (HttpContext.Current.Application["InformazioniAccountAutenticato"] == null)
            {
                asi = new List<InformazioniAccountAutenticato>();
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application["InformazioniAccountAutenticato"] = asi;
                HttpContext.Current.Application.UnLock();
            }
            else
            {
                asi = (List<InformazioniAccountAutenticato>)HttpContext.Current.Application["InformazioniAccountAutenticato"];
            }


            // Controllo se nella lista di informazioni sugli accounts autenticati memorizzata in Application 
            // esistono già informazioni sull'account autenticato
            InformazioniAccountAutenticato informazioniAccount = asi.FirstOrDefault(x => x.Username.ToLower() == HttpContext.Current.User.Identity.Name.ToLower());
            if (informazioniAccount == null)
            {
                // Se non esistono li aggiunge
                informazioniAccount = new InformazioniAccountAutenticato();




                // -- USERNAME --
                informazioniAccount.Username = HttpContext.Current.User.Identity.Name;




                // -- ACCOUNT --
                Logic.Sicurezza.Accounts llS = new Logic.Sicurezza.Accounts();
                Entities.Account loggedAccount = llS.Find(informazioniAccount.Username);
                // Se l'utente loggato è inesistente oppure bloccato allora chiudo la sessione
                if (loggedAccount == null || (loggedAccount.Bloccato.HasValue && loggedAccount.Bloccato.Value))
                {
                    RimuoviInformazioniAccount();
                    FormsAuthentication.SignOut();
                    HttpContext.Current.Response.Redirect("~/Home.aspx");
                    return null;
                }
                else
                {
                    informazioniAccount.Account = loggedAccount;
                }

                // -- WEBSESSION --
                InformazioniSessione infoSessioneCorrente = new InformazioniSessione();
                infoSessioneCorrente.IDSessione = System.Web.HttpContext.Current.Session.SessionID;
                infoSessioneCorrente.Ip = Helper.Web.GetClientIpAddress();

                informazioniAccount.WebSession = new List<InformazioniSessione>();
                informazioniAccount.WebSession.Add(infoSessioneCorrente);



                asi.Add(informazioniAccount);
            }
            else
            {
                InformazioniSessione infoSessioneCorrente = informazioniAccount.WebSession.FirstOrDefault(x => x.IDSessione == HttpContext.Current.Session.SessionID);
                if (infoSessioneCorrente == null)
                {
                    infoSessioneCorrente = new InformazioniSessione();
                    infoSessioneCorrente.IDSessione = System.Web.HttpContext.Current.Session.SessionID;
                    infoSessioneCorrente.Ip = Helper.Web.GetClientIpAddress();
                    informazioniAccount.WebSession.Add(infoSessioneCorrente);
                }
            }
            return informazioniAccount;
        }


        public static void RimuoviInformazioniAccount()
        {
            RimuoviInformazioniAccount(HttpContext.Current.User.Identity.Name);
        }

        public static void RimuoviInformazioniAccount(string accountUserName)
        {
            IList<InformazioniAccountAutenticato> asi = (List<InformazioniAccountAutenticato>)HttpContext.Current.Application["InformazioniAccountAutenticato"];
            if (asi != null)
            {
                List<InformazioniAccountAutenticato> asiUtente = asi.Where(x => x.Username.ToLower() == accountUserName.ToLower()).ToList();
                if (asiUtente.Count() > 0)
                {
                    HttpContext.Current.Application.Lock();
                    foreach (InformazioniAccountAutenticato informazioniSessioneCorrente in asiUtente)
                    {
                        asi.Remove(informazioniSessioneCorrente);
                    }
                    HttpContext.Current.Application.UnLock();
                }
            }
        }

        public static void RimuoviInformazioniDiTuttiGliAccounts()
        {
            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application.Remove("InformazioniAccountAutenticato");
            HttpContext.Current.Application.UnLock();
        }
        

        public class InformazioniSessione
        {
            public string IDSessione { get; set; }


            private string ip;
            public string Ip
            {
                get
                {
                    return ip;
                }
                set
                {
                    ip = value;

                    if (value.IsValidIpAddress())
                    {
                        ClasseIp = value.Substring(0, value.LastIndexOf('.'));
                    }
                }
            }

            public string ClasseIp
            {
                get;
                private set;
            }

            public List<StatoInterventoEnum> FiltriPerStatoGrigliaIntervento { get; set; }
            public List<FiltroInterventoEnum> FiltriGrigliaIntervento { get; set; }
        }



        //#region Funzioni di gestione delle informazioni sulle modifiche dei dati

        //private static Dictionary<string, Guid> ValoriDiSessione
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Session["ArrayValoriMonitorati"] == null)
        //            HttpContext.Current.Session["ArrayValoriMonitorati"] = new Dictionary<string, Guid>();

        //        return (Dictionary<string, Guid>)HttpContext.Current.Session["ArrayValoriMonitorati"];
        //    }
        //}

        ///// <summary>
        ///// Dictionary memorizzato a livello di Application utilizzato per condividere le informazioni sulle modifiche dei dati fra utenti
        ///// Tramite questo oggetto un utente può memorizzare dei dati all'interno di variabili private e riutilizzarli senza ricaricarli 
        ///// fino a quando un altro utente li avrà modificati
        ///// </summary>
        //private static Dictionary<string, Guid> ValoriMonitoratiLivelloApplicazione
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Application["ArrayValoriMonitorati"] == null)
        //            HttpContext.Current.Application["ArrayValoriMonitorati"] = new Dictionary<string, Guid>();

        //        return (Dictionary<string, Guid>)HttpContext.Current.Application["ArrayValoriMonitorati"];
        //    }
        //}

        ///// <summary>
        ///// Imposta come MODIFICATO il dato relativo all'identificatore passato
        ///// </summary>
        ///// <param name="identificatore"></param>
        //public static void SetValoreMonitoratoModificato(string identificatore)
        //{
        //    if (!ValoriMonitoratiLivelloApplicazione.ContainsKey(identificatore))
        //    {
        //        ValoriMonitoratiLivelloApplicazione.Add(identificatore, Guid.NewGuid());
        //    }
        //    else
        //    {
        //        ValoriMonitoratiLivelloApplicazione[identificatore] = Guid.NewGuid();
        //    }
        //}

        ///// <summary>
        ///// Restituisce un bool che indica se il valore dei dati relativi all'identificatore passato è cambiato o meno
        ///// </summary>
        ///// <param name="identificatore"></param>
        ///// <param name="valoreMemorizzato"></param>
        ///// <returns></returns>
        //public static bool GetValoreMonitoratoModificato(string identificatore)
        //{
        //    bool esisteInApplicazione = ValoriMonitoratiLivelloApplicazione.ContainsKey(identificatore);
        //    bool esisteInSessione = ValoriDiSessione.ContainsKey(identificatore);

        //    if (esisteInApplicazione && esisteInSessione)
        //    {
        //        return !ValoriMonitoratiLivelloApplicazione[identificatore].Equals(ValoriDiSessione[identificatore]);
        //    }

        //    if (!esisteInApplicazione && esisteInSessione)
        //    {
        //        SetValoreMonitoratoModificato(identificatore);
        //        return true;
        //    }

        //    if (esisteInApplicazione && !esisteInSessione)
        //    {
        //        ValoriDiSessione.Add(identificatore, ValoriMonitoratiLivelloApplicazione[identificatore]);
        //        return true;
        //    }

        //    if (!esisteInApplicazione && !esisteInSessione)
        //    {
        //        return false;
        //    }
        //    return false;
        //}

        //public static void RefreshValoreMonitorato(string identificatore)
        //{
        //    bool esisteInApplicazione = ValoriMonitoratiLivelloApplicazione.ContainsKey(identificatore);
        //    bool esisteInSessione = ValoriDiSessione.ContainsKey(identificatore);

        //    if (esisteInApplicazione && esisteInSessione)
        //    {
        //        ValoriDiSessione[identificatore] = ValoriMonitoratiLivelloApplicazione[identificatore];
        //    }

        //    if (!esisteInApplicazione && esisteInSessione)
        //    {
        //        SetValoreMonitoratoModificato(identificatore);
        //    }

        //    if (esisteInApplicazione && !esisteInSessione)
        //    {
        //        ValoriDiSessione.Add(identificatore, ValoriMonitoratiLivelloApplicazione[identificatore]);
        //    }
        //}

        //#endregion
    }
}