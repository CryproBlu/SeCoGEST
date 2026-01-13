using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Globalization;


namespace SeCoGEST.Infrastructure
{
    public static class ConfigurationKeys
    {
        #region Properties

        /// <summary>
        /// Restituisce l'url dell'applicazione
        /// </summary>
        public static string URL_APPLICAZIONE
        {
            get
            {
                string keyValue = GetStringValue("UrlApplicazione");
                return (String.IsNullOrEmpty(keyValue) ? String.Empty : keyValue);
            }
        }

        /// <summary>
        /// Restituisce il Titolo dell'applicazione
        /// </summary>
        public static string TITOLO_APPLICAZIONE
        {
            get
            {
                string keyValue = GetStringValue("TitoloApplicazione");
                return (String.IsNullOrEmpty(keyValue) ? "[Titolo applicazione]" : keyValue);
            }
        }

        /// <summary>
        /// Restituisce la denominazione dell'azienda
        /// </summary>
        public static string DENOMINAZIONE_AZIENDA
        {
            get
            {
                return GetStringValue("DenominazioneAzienda");
            }
        }
        public static string DENOMINAZIONE_AZIENDA_ALTRO
        {
            get
            {
                return GetStringValue("DenominazioneAziendaAltro");
            }
        }

        /// <summary>
        /// Restituisce il numero dei minuti massimo che devono passare per effettuare l'invio della notifica relativa agli interventi senza operatori
        /// </summary>
        public static int MINUTI_MASSIMI_INVIO_NOTIFICA_INTERVENTI_SENZA_OPERATORI
        {
            get
            {
                string valore = GetStringValue("MinutiMassimiInvioNotificaInterventiSenzaOperatori");
                int valoreInt = 0;
                if (Int32.TryParse(valore, out valoreInt))
                {
                    return valoreInt;
                }
                else
                {
                    return 60;
                }
            }
        }

        /// <summary>
        /// Indica se mostrare i dettagli della configurazione selezionata nella pagina del Ticket
        /// </summary>
        public static bool MOSTRA_DETTAGLI_CONFIGURAZIONE_NEL_TICKET
        {
            get
            {
                string valore = GetStringValue("MostraDettagliConfigurazioneNelTicket");
                if(!string.IsNullOrEmpty(valore))
                {
                    if(valore.Trim().ToLower() == "true")
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Restituisce il mittente delle Email per l'invio delle offerte
        /// </summary>
        public static string MITTENTE_EMAIL_OFFERTA
        {
            get
            {
                return GetStringValue("MittenteEmailOfferta");
            }
        }
        
        /// <summary>
        /// Restituisce il mittente delle Email Tecniche dell'Applicazione
        /// </summary>
        public static string MITTENTE_EMAIL_TECNICHE_APPLICAZIONE
        {
            get
            {
                return GetStringValue("MittenteEmailTecnicheApplicazione");
            }
        }

        /// <summary>
        /// Restituisce il Mittente dell'email da cui inviare alla chiusura dell'intervento
        /// </summary>
        public static string MITTENTE_EMAIL_CHIUSURA_INTERVENTO
        {
            get
            {
                return GetStringValue("MittenteEmailChiusuraIntervento");
            }
        }

        /// <summary>
        /// Restituisce il Mittente dell'email da cui inviare la notifica per un intervento
        /// </summary>
        public static string MITTENTE_EMAIL_NOTIFICA_INTERVENTO
        {
            get
            {
                return GetStringValue("MittenteEmailNotificaIntervento");
            }
        }

        /// <summary>
        /// Restituisce il Mittente dell'email da cui inviare l'email per l'apertura di un bollettino aperto dal cliente
        /// </summary>
        public static string MITTENTE_EMAIL_PER_INTERVENTO_APERTO_DAL_CLIENTE
        {
            get
            {
                return GetStringValue("MittenteEmailPerInterventoApertoDalCliente");
            }
        }
        
        /// <summary>
        /// Restituisce il Mittente dell'email da cui inviare l'email per l'apertura di un bollettino aperto dal cliente
        /// </summary>
        public static string MITTENTE_EMAIL_PER_CREAZIONE_ACCOUNT_DI_ACCESSO
        {
            get
            {
                return GetStringValue("MittenteEmailPerCreazioneAccountDiAccesso");
            }
        }

        /// <summary>
        /// Restituisce il Mittente dell'email da cui inviare l'email relativa alla notifica degli interventi senza operatori
        /// </summary>
        public static string MITTENTE_EMAIL_PER_NOTIFICA_INTERVENTI_SENZA_OPERATORI
        {
            get
            {
                return GetStringValue("MittenteEmailPerNotificaInterventiSenzaOperatori");
            }
        }

        /// <summary>
        /// Restituisce i Destinatari delle Email Tecniche dell'Applicazione
        /// </summary>
        public static string[] DESTINATARI_EMAIL_TECNICHE_APPLICAZIONE
        {
            get
            {
                return GetStringArrayValue("DestinatariEmailTecnicheApplicazione");
            }
        }

        /// <summary>
        /// Restituisce i Destinatari dell'email da inviare alla chiusura dell'intervento
        /// </summary>
        public static string[] DESTINATARI_EMAIL_CHIUSURA_INTERVENTO
        {
            get
            {
                return GetStringArrayValue("DestinatariEmailChiusuraIntervento");
            }
        }


        /// <summary>
        /// Restituisce l'account predefinito come Destinatario delle email
        /// </summary>
        public static string DESTINATARIO_PREDEFINITO_EMAIL
        {
            get
            {
                return GetStringValue("DestinatarioPredefinitoEmail");
            }
        }


        /// <summary>
        /// Restituisce l'account predefinito come Destinatario delle email
        /// </summary>
        public static string NUMERO_TELEFONO_AZIENDA
        {
            get
            {
                return GetStringValue("NumeroTelefonoAzienda");
            }
        }

        /// <summary>
        /// Restituisce gli id degli operatori del gestionale che devono essere associati all'intervento aperto dal cliente
        /// </summary>
        public static string[] IDOPERATORI_GESTIONALE_PER_ASSOCIAZIONE_CON_INTERVENTO_APERTO_DAL_CLIENTE
        {
            get
            {
                return GetStringArrayValue("IDOperatoriGestionaleInvioEmailPerAssociazioneConInterventoApertoDalCliente");
            }
        }

        /// <summary>
        /// Restituisce l'elenco delle email a cui devono essere inviata la notifica relativa agli interventi a cui non sono stati associati gli operatori
        /// </summary>
        public static string[] DESTINATARI_EMAIL_PER_NOTIFICA_INTERVENTI_SENZA_OPERATORI
        {
            get
            {
                return GetStringArrayValue("DestinatariEmailPerNotificaInterventiSenzaOperatori");
            }
        }

        /// <summary>
        /// Restituisce l'elenco delle email a cui deve essere inviata la notifica relativa agli interventi in base allo SLA
        /// </summary>
        public static string[] DESTINATARI_EMAIL_PER_NOTIFICA_INTERVENTI_PER_SLA
        {
            get
            {
                return GetStringArrayValue("DestinatariEmailPerNotificaInterventiPerSLA");
            }
        }

        /// <summary>
        /// Restituisce il percorso temporaneo in cui devono essere salvati i file
        /// </summary>
        public static string PERCORSO_TEMPORANEO
        {
            get
            {
                return GetStringValue("PercorsoTemporaneo");
            }
        }

        /// <summary>
        /// Restituisce il percorso completo relativo al modello del bollettino
        /// </summary>
        public static string PERCORSO_MODELLO_DOCUMENTO_INTERVENTO
        {
            get
            {
                return GetStringValue("PercorsoModelloDocumentoIntervento");
            }
        }

        //public static List<string> IDS_TIPOLOGIE_INTERVENTO_PER_MODELLO_DOCUMENTO_VERSIONE_2
        //{
        //    get
        //    {
        //        return GetStringValue("IdsTipologieInterventoPerModelloDocumentoVersione2").ToUpper().Split(';').ToList();
        //    }
        //}

        /// <summary>
        /// Restituisce il percorso completo relativo al modello del bollettino
        /// </summary>
        public static string PERCORSO_MODELLO_DOCUMENTO_INTERVENTO_Versione2
        {
            get
            {
                return GetStringValue("PercorsoModelloDocumentoInterventoV2");
            }
        }

        /// <summary>
        /// Restituisce il percorso del file che, se esiste, indica che l'accesso all'applicazione è bloccato
        /// </summary>
        public static string PERCORSO_FILE_BLOCCO_ACCESSI
        {
            get
            {
                return GetStringValue("PercorsoFileBloccoAccessi");
            }
        }

        /// <summary>
        /// Restituisce una stringa contenente il parametro che telerik accoda ai parametri nella query string quando l'utente seleziona una tab
        /// </summary>
        public static string TELERIK_URI_IDENTIFIER
        {
            get
            {
                return GetStringValue("TelerikUriIdentifier");
            }
        }
        
        /// <summary>
        /// Restituisce una stringa contenente il percorso del file di template relativo all'offerta al cliente tramite email
        /// </summary>
        public static string PERCORSO_TEMPLATE_EMAIL_INVIO_OFFERTA_AL_CLIENTE
        {
            get
            {
                return GetStringValue("PercorsoTemplateEmailInvioOffertaAlCliente");
            }
        }
        
        /// <summary>
        /// Restituisce una stringa contenente il percorso del file di template che funge da repository ai template delle offerte da generare
        /// </summary>
        public static string PERCORSO_DIRECTORY_TEMPLATE_OFFERTE
        {
            get
            {
                return GetStringValue("PercorsoDirectoryTemplateOfferte");
            }
        }
        
        /// <summary>
        /// Restituisce una stringa contenente il percorso delal directory che contiene dei file temporanei
        /// </summary>
        public static string PERCORSO_DIRECTORY_FILE_TEMPORANEI
        {
            get
            {
                return GetStringValue("PercorsoDirectoryFileTemporanei");
            }
        }





        /// <summary>
        /// Restituisce una stringa contenente l'ID Azienda della quale interrogare i dati in G7
        /// </summary>
        public static string ID_AZIENDA_G7_SITEID
        {
            get
            {
                return GetStringValue("IDAziendaG7_SiteID");
            }
        }

        /// <summary>
        /// Restituisce una stringa contenente l'URL delle API di G7 per l'acquisizione dei tickets
        /// </summary>
        public static string G7_TICKET_IMPORT_API_URL
        {
            get
            {
                return GetStringValue("G7TicketImportAPIURL");
            }
        }


        #region Colori per Stato degli Interventi

        public static System.Drawing.Color ColoreIntervento_Aperto
        {
            get
            {
                return System.Drawing.Color.White;
            }
        }

        public static System.Drawing.Color ColoreIntervento_ApertoUrgente
        {
            get
            {
                return System.Drawing.Color.Orange;
            }
        }

        public static System.Drawing.Color ColoreIntervento_InGestione
        {
            get
            {
                return System.Drawing.Color.LightSkyBlue;
            }
        }

        public static System.Drawing.Color ColoreIntervento_Eseguito
        {
            get
            {
                return System.Drawing.Color.Pink;
            }
        }

        public static System.Drawing.Color ColoreIntervento_Chiuso
        {
            get
            {
                return System.Drawing.Color.LightGreen;
            }
        }

        public static System.Drawing.Color ColoreIntervento_Validato
        {
            get
            {
                return System.Drawing.Color.Silver;
            }
        }

        public static System.Drawing.Color ColoreIntervento_Sostituito
        {
            get
            {
                return System.Drawing.Color.Gray;
            }
        }

        #endregion

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Restituisce un array di stringhe recuperato effettuando lo split del valore recuperato in base alla chiave passata come parametro
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        private static string[] GetStringArrayValue(string keyName, string separator = ",")
        {
            if (String.IsNullOrEmpty(keyName))
            {
                keyName = String.Empty;
            }

            string destinatariEmailString = ConfigurationManager.AppSettings[keyName];
            if (destinatariEmailString == null)
            {
                destinatariEmailString = String.Empty;
            }

            destinatariEmailString = destinatariEmailString.Trim();

            // Viene recuperato l'elenco dei destinatari, splittando la stringa con la virgola ed escludendo gli elementi vuoti
            string[] elencoDestinatariEmail = destinatariEmailString.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            // Nel caso in cui la collection risulti non valorizzata, allora viene inivializzata la collection con un array di stringhe vuoto
            if (elencoDestinatariEmail == null) elencoDestinatariEmail = new string[] { };

            // Nel caso in cui l'elenco degli indirizzi dei destinatari contenga almento un indirizzo ..
            if (elencoDestinatariEmail.Length > 0)
            {
                // Viene recuperato l'elenco dei destinatari effettuando il trim su tutti i valori presenti nell'array
                IEnumerable<string> elencoIndirizzoDestinatariTrimmato = elencoDestinatariEmail.Select(x => x.Trim());

                // Viene richiamato il metodo che restituisce un array di stringhe da un elenco
                return elencoIndirizzoDestinatariTrimmato.ToArray();
            }
            else
            {
                // Viene restiuito l'array di stringhe vuoto
                return elencoDestinatariEmail;
            }
        }

        /// <summary>
        /// Restituisce il valore contenuto nella chiave, il cui nome è passato come parametro, sotto forma di stringa
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        private static string GetStringValue(string keyName)
        {
            if (String.IsNullOrEmpty(keyName))
            {
                keyName = String.Empty;
            }

            if (ConfigurationManager.AppSettings[keyName] == null)
            {
                return String.Empty;
            }
            else
            {
                return ConfigurationManager.AppSettings[keyName].ToString();
            }
        }

        #endregion
    }
}
