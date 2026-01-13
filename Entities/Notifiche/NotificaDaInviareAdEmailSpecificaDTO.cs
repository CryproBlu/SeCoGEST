using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public class NotificaDaInviareAdEmailSpecificaDTO
    {
        #region Properties Pubbliche

        public string Email { get; private set; }

        public string Nominativo { get; private set; }

        public bool EsistonoNotificheInformazioniDaInviare
        {
            get
            {
                return (ElencoNotificheInformazioni.Count > 0);
            }
        }
                
        #endregion

        #region Properties Private

        private IDictionary<TipologiaNotificaEnum, List<InformazioneNotiticaDaInviareDTO>> ElencoNotificheInformazioni { get; set; }
        
        #endregion

        #region Costruttori

        public NotificaDaInviareAdEmailSpecificaDTO(string email, string nominativo, TipologiaNotificaEnum tipologiaNotifica, List<InformazioneNotiticaDaInviareDTO> elencoInformazioni = null)
        {
            InizializzaIstanza(email, nominativo, tipologiaNotifica, elencoInformazioni);
        }

        #endregion

        #region Metodi Pubblici

        /// <summary>
        /// Effettua l'aggiunta della tipologia di notifica e dell'informazione da inviare per quest'ultima
        /// </summary>
        /// <param name="tipologiaNotifica"></param>
        /// <param name="informazione"></param>
        public void AggiungiNotificaInformazione(TipologiaNotificaEnum tipologiaNotifica, InformazioneNotiticaDaInviareDTO informazione)
        {
            if (informazione == null) throw new ArgumentNullException("informazione", "Parametro nullo");

            AggiungiNotificaInformazioni(tipologiaNotifica, new List<InformazioneNotiticaDaInviareDTO>() { informazione });
        }

        /// <summary>
        /// Effettua l'aggiunta della tipologia di notifica e dell'elenco di informazioni da inviare per quest'ultima
        /// </summary>
        /// <param name="tipologiaNotifica"></param>
        /// <param name="elencoInformazioni"></param>
        public void AggiungiNotificaInformazioni(TipologiaNotificaEnum tipologiaNotifica, List<InformazioneNotiticaDaInviareDTO> elencoInformazioni)
        {
            // Nel caso in cui l'elenco delle informazioni sia nullo, viene inizializzato con una lista vuota
            if (elencoInformazioni == null) elencoInformazioni = new List<InformazioneNotiticaDaInviareDTO>();

            // Nel caso in cui l'elenco delle notifiche delle informazioni non contenga la tipologia di notifica passata come parametro
            if (!ElencoNotificheInformazioni.ContainsKey(tipologiaNotifica))
            {
                // Viene aggiunto all'elenco, la tipologia passata come parametro ed una lista di informazioni vuota in modo che 
                // l'elenco passato venga ripulito da eventuali doppioni
                ElencoNotificheInformazioni.Add(tipologiaNotifica, new List<InformazioneNotiticaDaInviareDTO>());
            }

            // Viene inizializzata une lista di informazioni delle notifiche da inviare temporanea
            List<InformazioneNotiticaDaInviareDTO> elencoInformazioniTemp = new List<InformazioneNotiticaDaInviareDTO>();

            // Per ogni elemento presente nell'elenco delle informazioni passato come parametro ..
            foreach (InformazioneNotiticaDaInviareDTO dtoInformazioneNotificaDaInviare in elencoInformazioni)
            {
                // Viene verificato che la lista temporanea non contenga già le informazioni inserite.
                // Nel caso in cui il controllo risulti negativo ..
                if (!elencoInformazioniTemp.Any(x => x.InformazioneHTML == dtoInformazioneNotificaDaInviare.InformazioneHTML &&
                                                    x.InformazionePlainText == dtoInformazioneNotificaDaInviare.InformazionePlainText))
                {
                    // Viene aggiunto il dto nella lista temporanea
                    elencoInformazioniTemp.Add(dtoInformazioneNotificaDaInviare);
                }
            }

            // Viene recuperato l'elenco delle informazioni associare alla tipologia di notifica passata
            List<InformazioneNotiticaDaInviareDTO> elencoInformazioniPerTipologiaNotifica = ElencoNotificheInformazioni[tipologiaNotifica];

            // Per ogni record recuperato ..
            foreach (InformazioneNotiticaDaInviareDTO dtoInformazioneNotificaDaInviare in elencoInformazioniPerTipologiaNotifica)
            {
                // Viene verificato che la lista temporanea non contenga già le informazioni inserite.
                // Nel caso in cui il controllo risulti negativo ..
                if (!elencoInformazioniTemp.Any(x => x.InformazioneHTML == dtoInformazioneNotificaDaInviare.InformazioneHTML &&
                                                     x.InformazionePlainText == dtoInformazioneNotificaDaInviare.InformazionePlainText))
                {
                    // Viene aggiunto il dto nella lista temporanea
                    elencoInformazioniTemp.Add(dtoInformazioneNotificaDaInviare);
                }
            }

            // Visto che la lista temporanea contiene l'elenco delle informazioni in modo univoco,
            // viene settato l'elenco delle informazioni associate alla tipologia di notifica
            ElencoNotificheInformazioni[tipologiaNotifica] = elencoInformazioniTemp;
        }
        
        /// <summary>
        /// Aggiunge alla collection, l'elenco delle notifiche e relative informazioni
        /// </summary>
        /// <param name="elencoNotifiche"></param>
        public void AggiungiNotificheInformazioni(IDictionary<TipologiaNotificaEnum, List<InformazioneNotiticaDaInviareDTO>> elencoNotifiche)
        {
            if (elencoNotifiche != null && elencoNotifiche.Count > 0)
            {
                foreach (KeyValuePair<TipologiaNotificaEnum, List<InformazioneNotiticaDaInviareDTO>> informazioniNotifiche in elencoNotifiche)
                {
                    AggiungiNotificaInformazioni(informazioniNotifiche.Key, informazioniNotifiche.Value);
                }
            }
        }

        /// <summary>
        /// Effettua la pulizia degli elementi presenti nell'elenco delle informazioni
        /// </summary>
        public void PulisciElencoNotificheInformazioni()
        {
            if (EsistonoNotificheInformazioniDaInviare)
            {
                foreach (KeyValuePair<TipologiaNotificaEnum, List<InformazioneNotiticaDaInviareDTO>> notificaInformazioni in ElencoNotificheInformazioni)
                {
                    notificaInformazioni.Value.Clear();
                }

                ElencoNotificheInformazioni.Clear();
            }
        }

        /// <summary>
        /// Restituisce l'elenco delle notifiche presenti nella collection
        /// </summary>
        /// <returns></returns>
        public IDictionary<TipologiaNotificaEnum, List<InformazioneNotiticaDaInviareDTO>> GetElencoNotificheInformazioni()
        {
            IDictionary<TipologiaNotificaEnum, List<InformazioneNotiticaDaInviareDTO>> dictionaryDaRestituire = new Dictionary<TipologiaNotificaEnum, List<InformazioneNotiticaDaInviareDTO>>();

            if (EsistonoNotificheInformazioniDaInviare)
            {
                foreach (KeyValuePair<TipologiaNotificaEnum, List<InformazioneNotiticaDaInviareDTO>> notificaInformazioni in ElencoNotificheInformazioni)
                {
                    dictionaryDaRestituire.Add(notificaInformazioni.Key, notificaInformazioni.Value);
                }
            }

            return dictionaryDaRestituire;
        }
        
        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Effettua l'inizializzazione delle property dell'istanza in base ai parametri passati
        /// </summary>
        /// <param name="email"></param>
        /// <param name="tipologiaNotifica"></param>
        /// <param name="elencoInformazioni"></param>
        private void InizializzaIstanza(string email, string nominativo, TipologiaNotificaEnum tipologiaNotifica, List<InformazioneNotiticaDaInviareDTO> elencoInformazioni = null)
        {
            if (String.IsNullOrEmpty(email)) throw new ArgumentNullException("email", "Parametro nullo");

            Email = email;

            if (String.IsNullOrWhiteSpace(nominativo))
            {
                nominativo = "Utente";
            }

            Nominativo = nominativo;

            ElencoNotificheInformazioni = new Dictionary<TipologiaNotificaEnum, List<InformazioneNotiticaDaInviareDTO>>();
            AggiungiNotificaInformazioni(tipologiaNotifica, elencoInformazioni);
        }

        #endregion
    }
}
