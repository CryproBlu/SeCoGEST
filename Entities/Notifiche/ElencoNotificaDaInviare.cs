using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public class ElencoNotificaDaInviare
    {
        #region Properties Pubbliche

        public bool EsistonoNotifiche
        {
            get
            {
                return (ElencoNotifiche.Count > 0);
            }
        }

        public bool EsistonoNotificheAiResponsabili
        {
            get
            {
                return (ElencoNotificheAiResponsabili.Count > 0);
            }
        }

        public bool EsistonoNotificheInterventiSenzaOperatore
        {
            get
            {
                return (ElencoNotificheInterventiSenzaOperatore.Count > 0);
            }
        }

        public bool EsistonoNotificheInterventiSLA
        {
            get
            {
                return (ElencoNotificheInterventiSLA.Count > 0);
            }
        }

        #endregion

        #region Properties Private

        private List<NotificaDaInviareDTO> ElencoNotifiche { get; set; }

        private List<NotificaDaInviareAdEmailSpecificaDTO> ElencoNotificheAiResponsabili { get; set; }

        private List<NotificaDaInviareAdEmailSpecificaDTO> ElencoNotificheInterventiSenzaOperatore { get; set; }

        private List<NotificaDaInviareAdEmailSpecificaDTO> ElencoNotificheInterventiSLA { get; set; }
        
        #endregion

        #region Costruttori

        public ElencoNotificaDaInviare()
        {
            ElencoNotifiche = new List<NotificaDaInviareDTO>();
            ElencoNotificheAiResponsabili = new List<NotificaDaInviareAdEmailSpecificaDTO>();
            ElencoNotificheInterventiSenzaOperatore = new List<NotificaDaInviareAdEmailSpecificaDTO>();
            ElencoNotificheInterventiSLA = new List<NotificaDaInviareAdEmailSpecificaDTO>();
        }

        #endregion

        #region Metodi Pubblici

        #region Notifiche

        /// <summary>
        /// Aggiunge alla collection, l'elenco di dto passato come parametro
        /// </summary>
        /// <param name="elencoNotificheDaInviare"></param>
        public void Add(IEnumerable<NotificaDaInviareDTO> elencoNotificheDaInviare)
        {
            if (elencoNotificheDaInviare != null && elencoNotificheDaInviare.Count() > 0)
            {
                foreach (NotificaDaInviareDTO notificaDaInviare in elencoNotificheDaInviare)
                {
                    Add(notificaDaInviare);
                }
            }
        }

        /// <summary>
        /// Aggiunge alla collection interna, il dto passato come parametro
        /// </summary>
        /// <param name="notificaDaInviare"></param>
        /// <param name="mergeInformationIfExists"></param>
        public void Add(NotificaDaInviareDTO notificaDaInviare, bool mergeInformationIfExists = true)
        {
            if (notificaDaInviare == null) throw new ArgumentNullException("notificaDaInviare", "Parametro nullo");

            if (!IsNotificaEsistente(notificaDaInviare))
            {
                ElencoNotifiche.Add(notificaDaInviare);
            }
            else if (notificaDaInviare.EsistonoNotificheInformazioniDaInviare && mergeInformationIfExists)
            {
                NotificaDaInviareDTO notificaEsistente = GetNotificaEsistente(notificaDaInviare);
                if (notificaEsistente != null)
                {
                    notificaEsistente.AggiungiNotificheInformazioni(notificaDaInviare.GetElencoNotificheInformazioni());
                }
            }
        }

        /// <summary>
        /// Effettua la pulizia dell'elenco delle notifiche presenti nella collection delle notifiche da inviare
        /// </summary>
        public void ClearNotifiche()
        {
            if (EsistonoNotifiche)
            {
                foreach (NotificaDaInviareDTO notifica in ElencoNotifiche)
                {
                    if (notifica.EsistonoNotificheInformazioniDaInviare)
                    {
                        notifica.PulisciElencoNotificheInformazioni();
                    }
                }

                ElencoNotifiche.Clear();
            }
        }

        /// <summary>
        /// Restituisce l'elenco delle notiche presenti nella collection interna
        /// </summary>
        public List<NotificaDaInviareDTO> GetNotifiche()
        {
            return ElencoNotifiche.ToList();
        }

        #endregion

        #region Notifiche Ai Responsabili

        /// <summary>
        /// Aggiunge alla collection, l'elenco di dto passato come parametro
        /// </summary>
        /// <param name="elencoNotificheDaInviare"></param>
        public void Add(IEnumerable<NotificaDaInviareAdEmailSpecificaDTO> elencoNotificheDaInviare)
        {
            if (elencoNotificheDaInviare != null && elencoNotificheDaInviare.Count() > 0)
            {
                foreach (NotificaDaInviareAdEmailSpecificaDTO notificaDaInviare in elencoNotificheDaInviare)
                {
                    Add(notificaDaInviare);
                }
            }
        }

        /// <summary>
        /// Aggiunge alla collection interna, il dto passato come parametro
        /// </summary>
        /// <param name="notificaDaInviare"></param>
        /// <param name="mergeInformationIfExists"></param>
        public void Add(NotificaDaInviareAdEmailSpecificaDTO notificaDaInviare, bool mergeInformationIfExists = true)
        {
            if (notificaDaInviare == null) throw new ArgumentNullException("notificaDaInviare", "Parametro nullo");

            if (!IsNotificaAiResposabiliEsistente(notificaDaInviare))
            {
                ElencoNotificheAiResponsabili.Add(notificaDaInviare);
            }
            else if (notificaDaInviare.EsistonoNotificheInformazioniDaInviare && mergeInformationIfExists)
            {
                NotificaDaInviareAdEmailSpecificaDTO notificaEsistente = GetNotificaAiResponsabiliEsistente(notificaDaInviare);
                if (notificaEsistente != null)
                {
                    notificaEsistente.AggiungiNotificheInformazioni(notificaDaInviare.GetElencoNotificheInformazioni());
                }
            }
        }

        /// <summary>
        /// Effettua la pulizia dell'elenco delle notifiche ai responsabili presenti nella collection delle notifiche da inviare
        /// </summary>
        public void ClearNotificheAiResponsabili()
        {
            if (EsistonoNotificheAiResponsabili)
            {
                foreach (NotificaDaInviareAdEmailSpecificaDTO notifica in ElencoNotificheAiResponsabili)
                {
                    if (notifica.EsistonoNotificheInformazioniDaInviare)
                    {
                        notifica.PulisciElencoNotificheInformazioni();
                    }
                }

                ElencoNotificheAiResponsabili.Clear();
            }
        }

        /// <summary>
        /// Restituisce l'elenco delle notiche ai responsabili presenti nella collection interna
        /// </summary>
        public List<NotificaDaInviareAdEmailSpecificaDTO> GetNotificheAiResponsabili()
        {
            return ElencoNotificheAiResponsabili.ToList();
        }

        #endregion        

        #region Notifiche Per Incarichi Senza Operatori

        /// <summary>
        /// Aggiunge alla collection, l'elenco di dto passato come parametro
        /// </summary>
        /// <param name="elencoNotificheDaInviare"></param>
        public void AddNotificaInterventoSenzaOperatore(IEnumerable<NotificaDaInviareAdEmailSpecificaDTO> elencoNotificheDaInviare)
        {
            if (elencoNotificheDaInviare != null && elencoNotificheDaInviare.Count() > 0)
            {
                foreach (NotificaDaInviareAdEmailSpecificaDTO notificaDaInviare in elencoNotificheDaInviare)
                {
                    AddNotificaInterventoSenzaOperatore(notificaDaInviare);
                }
            }
        }

        /// <summary>
        /// Aggiunge alla collection interna, il dto passato come parametro
        /// </summary>
        /// <param name="notificaDaInviare"></param>
        /// <param name="mergeInformationIfExists"></param>
        public void AddNotificaInterventoSenzaOperatore(NotificaDaInviareAdEmailSpecificaDTO notificaDaInviare, bool mergeInformationIfExists = true)
        {
            if (notificaDaInviare == null) throw new ArgumentNullException("notificaDaInviare", "Parametro nullo");

            if (!IsNotificaInterventoSenzaOperatoreEsistente(notificaDaInviare))
            {
                ElencoNotificheInterventiSenzaOperatore.Add(notificaDaInviare);
            }
            else if (notificaDaInviare.EsistonoNotificheInformazioniDaInviare && mergeInformationIfExists)
            {
                NotificaDaInviareAdEmailSpecificaDTO notificaEsistente = GetNotificaInterventoSenzaOperatoreEsistente(notificaDaInviare);
                if (notificaEsistente != null)
                {
                    notificaEsistente.AggiungiNotificheInformazioni(notificaDaInviare.GetElencoNotificheInformazioni());
                }
            }
        }

        /// <summary>
        /// Effettua la pulizia dell'elenco delle notifiche relative agli interventi a cui non sono stati associati operatori, presenti nella collection delle notifiche da inviare
        /// </summary>
        public void ClearNotificheInterventiSenzaOperatore()
        {
            if (EsistonoNotificheInterventiSenzaOperatore)
            {
                foreach (NotificaDaInviareAdEmailSpecificaDTO notifica in ElencoNotificheInterventiSenzaOperatore)
                {
                    if (notifica.EsistonoNotificheInformazioniDaInviare)
                    {
                        notifica.PulisciElencoNotificheInformazioni();
                    }
                }

                ElencoNotificheInterventiSenzaOperatore.Clear();
            }
        }

        /// <summary>
        /// Restituisce l'elenco delle notiche relative agli interventi non chiusi presenti nella collection interna
        /// </summary>
        public List<NotificaDaInviareAdEmailSpecificaDTO> GetNotificheInterventiSenzaOperatore()
        {
            return ElencoNotificheInterventiSenzaOperatore.ToList();
        }

        #endregion        

        #region Notifiche Per SLA

        /// <summary>
        /// Aggiunge alla collection, l'elenco di dto passato come parametro
        /// </summary>
        /// <param name="elencoNotificheDaInviare"></param>
        public void AddNotificaInterventoSLA(IEnumerable<NotificaDaInviareAdEmailSpecificaDTO> elencoNotificheDaInviare)
        {
            if (elencoNotificheDaInviare != null && elencoNotificheDaInviare.Count() > 0)
            {
                foreach (NotificaDaInviareAdEmailSpecificaDTO notificaDaInviare in elencoNotificheDaInviare)
                {
                    AddNotificaInterventoSLA(notificaDaInviare);
                }
            }
        }

        /// <summary>
        /// Aggiunge alla collection interna, il dto passato come parametro
        /// </summary>
        /// <param name="notificaDaInviare"></param>
        /// <param name="mergeInformationIfExists"></param>
        public void AddNotificaInterventoSLA(NotificaDaInviareAdEmailSpecificaDTO notificaDaInviare, bool mergeInformationIfExists = true)
        {
            if (notificaDaInviare == null) throw new ArgumentNullException("notificaDaInviare", "Parametro nullo");

            if (!IsNotificaInterventoSLAEsistente(notificaDaInviare))
            {
                ElencoNotificheInterventiSLA.Add(notificaDaInviare);
            }
            else if (notificaDaInviare.EsistonoNotificheInformazioniDaInviare && mergeInformationIfExists)
            {
                NotificaDaInviareAdEmailSpecificaDTO notificaEsistente = GetNotificaInterventoSLAEsistente(notificaDaInviare);
                if (notificaEsistente != null)
                {
                    notificaEsistente.AggiungiNotificheInformazioni(notificaDaInviare.GetElencoNotificheInformazioni());
                }
            }
        }

        /// <summary>
        /// Effettua la pulizia dell'elenco delle notifiche relative agli interventi SLA
        /// </summary>
        public void ClearNotificheInterventiSLA()
        {
            if (EsistonoNotificheInterventiSLA)
            {
                foreach (NotificaDaInviareAdEmailSpecificaDTO notifica in ElencoNotificheInterventiSLA)
                {
                    if (notifica.EsistonoNotificheInformazioniDaInviare)
                    {
                        notifica.PulisciElencoNotificheInformazioni();
                    }
                }

                ElencoNotificheInterventiSLA.Clear();
            }
        }

        /// <summary>
        /// Restituisce l'elenco delle notiche relative agli interventi non chiusi presenti nella collection interna
        /// </summary>
        public List<NotificaDaInviareAdEmailSpecificaDTO> GetNotificheInterventiSLA()
        {
            return ElencoNotificheInterventiSLA.ToList();
        }

        #endregion        

        /// <summary>
        /// Effettua la pulizia dell'elenco delle notifiche presenti nelle varie collection
        /// </summary>
        public void Clear()
        {
            ClearNotifiche();
            ClearNotificheAiResponsabili();
            ClearNotificheInterventiSenzaOperatore();
            ClearNotificheInterventiSLA();
        }        

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Verifica se la notifica da aggiungere è già stata aggiunta alla collection globale
        /// </summary>
        /// <param name="notificaDaAggiungere"></param>
        /// <returns></returns>
        private bool IsNotificaEsistente(NotificaDaInviareDTO notificaDaAggiungere)
        {
            if (notificaDaAggiungere == null) throw new ArgumentNullException("notificaDaAggiungere", "Parametro nullo");

            return ElencoNotifiche.Any(x => x.IdentifierOperatore.Value == notificaDaAggiungere.IdentifierOperatore.Value);
        }

        /// <summary>
        /// Restituisce la notifica esistente in base alla notifica passata come parametro
        /// </summary>
        /// <param name="notificaDaAggiungere"></param>
        /// <returns></returns>
        private NotificaDaInviareDTO GetNotificaEsistente(NotificaDaInviareDTO notificaDaAggiungere)
        {
            if (notificaDaAggiungere == null) throw new ArgumentNullException("notificaDaAggiungere", "Parametro nullo");

            return ElencoNotifiche.Where(x => x.IdentifierOperatore.Value == notificaDaAggiungere.IdentifierOperatore.Value)
                                  .SingleOrDefault();
        }

        /// <summary>
        /// Verifica se la notifica ai responsabili da aggiungere è già stata aggiunta alla collection globale
        /// </summary>
        /// <param name="notificaDaAggiungere"></param>
        /// <returns></returns>
        private bool IsNotificaAiResposabiliEsistente(NotificaDaInviareAdEmailSpecificaDTO notificaDaAggiungere)
        {
            if (notificaDaAggiungere == null) throw new ArgumentNullException("notificaDaAggiungere", "Parametro nullo");

            return ElencoNotificheAiResponsabili.Any(x => x.Email.ToLower() == notificaDaAggiungere.Email.ToLower());
        }

        /// <summary>
        /// Restituisce la notifica ai responsabili esistente in base alla notifica passata come parametro
        /// </summary>
        /// <param name="notificaDaAggiungere"></param>
        /// <returns></returns>
        private NotificaDaInviareAdEmailSpecificaDTO GetNotificaAiResponsabiliEsistente(NotificaDaInviareAdEmailSpecificaDTO notificaDaAggiungere)
        {
            if (notificaDaAggiungere == null) throw new ArgumentNullException("notificaDaAggiungere", "Parametro nullo");

            return ElencoNotificheAiResponsabili.Where(x => x.Email.ToLower() == notificaDaAggiungere.Email.ToLower())
                                                .SingleOrDefault();
        }

        /// <summary>
        /// Verifica se la notifica dell'intervento senza operatore, sia da aggiungere è già stata aggiunta alla collection globale
        /// </summary>
        /// <param name="notificaDaAggiungere"></param>
        /// <returns></returns>
        private bool IsNotificaInterventoSenzaOperatoreEsistente(NotificaDaInviareAdEmailSpecificaDTO notificaDaAggiungere)
        {
            if (notificaDaAggiungere == null) throw new ArgumentNullException("notificaDaAggiungere", "Parametro nullo");

            return ElencoNotificheInterventiSenzaOperatore.Any(x => x.Email.ToLower() == notificaDaAggiungere.Email.ToLower());
        }

        /// <summary>
        /// Restituisce la notifica dell'intervento senza operatore in base al dto passato come parmaetro
        /// </summary>
        /// <param name="notificaDaAggiungere"></param>
        /// <returns></returns>
        private NotificaDaInviareAdEmailSpecificaDTO GetNotificaInterventoSenzaOperatoreEsistente(NotificaDaInviareAdEmailSpecificaDTO notificaDaAggiungere)
        {
            if (notificaDaAggiungere == null) throw new ArgumentNullException("notificaDaAggiungere", "Parametro nullo");

            return ElencoNotificheInterventiSenzaOperatore.Where(x => x.Email.ToLower() == notificaDaAggiungere.Email.ToLower())
                                                          .SingleOrDefault();
        }

        /// <summary>
        /// Verifica se la notifica dell'intervento SLA, sia da aggiungere è già stata aggiunta alla collection globale
        /// </summary>
        /// <param name="notificaDaAggiungere"></param>
        /// <returns></returns>
        private bool IsNotificaInterventoSLAEsistente(NotificaDaInviareAdEmailSpecificaDTO notificaDaAggiungere)
        {
            if (notificaDaAggiungere == null) throw new ArgumentNullException("notificaDaAggiungere", "Parametro nullo");

            return ElencoNotificheInterventiSLA.Any(x => x.Email.ToLower() == notificaDaAggiungere.Email.ToLower());
        }

        /// <summary>
        /// Restituisce la notifica dell'intervento SLA in base al dto passato come parmaetro
        /// </summary>
        /// <param name="notificaDaAggiungere"></param>
        /// <returns></returns>
        private NotificaDaInviareAdEmailSpecificaDTO GetNotificaInterventoSLAEsistente(NotificaDaInviareAdEmailSpecificaDTO notificaDaAggiungere)
        {
            if (notificaDaAggiungere == null) throw new ArgumentNullException("notificaDaAggiungere", "Parametro nullo");

            return ElencoNotificheInterventiSLA.Where(x => x.Email.ToLower() == notificaDaAggiungere.Email.ToLower())
                                                          .SingleOrDefault();
        }

        #endregion
    }
}
