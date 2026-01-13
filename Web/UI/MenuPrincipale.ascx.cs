using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SeCoGEST.Web.UI
{
    public partial class MenuPrincipale : System.Web.UI.UserControl
    {
        #region Properties

        /// <summary>
        /// Restituisce o setta la property enabled del menu di navigazione
        /// </summary>
        public bool Enabled
        {
            get
            {
                return rnBarraDiNavigazione.Enabled;
            }
            set
            {
                rnBarraDiNavigazione.Enabled = value;
            }
        }

        /// <summary>
        /// Restituisce o setta la visibilità del menu
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return rnBarraDiNavigazione.Visible;
            }
            set
            {
                rnBarraDiNavigazione.Visible = value;
            }
        }

        #endregion

        #region Intercettazione Eventi

        /// <summary>
        /// Metodo di gestione dell'evento load della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CaricaAutorizzazioni();

            InformazioniAccountAutenticato infoAccountCollegato = InformazioniAccountAutenticato.GetIstance();
            if (infoAccountCollegato != null && infoAccountCollegato.Account != null)
            {
                if(infoAccountCollegato.Account.Amministratore.HasValue && infoAccountCollegato.Account.Amministratore.Value)
                {
                    CreaMenuAdmin();
                }
                else
                {
                    if (infoAccountCollegato.Account.TipologiaEnum == Entities.TipologiaAccountEnum.SeCoGes)
                    {
                        CreaMenuAccountStandard();
                    }
                    else
                    {
                        CreaMenuAccountCliente();
                    }
                }
            }            
        }

        #endregion

        #region Metodi di Gestione

        /// <summary>
        /// Gestisce l'inserimento delle voci del menu Applicazione
        /// </summary>
        private void CreaVociMenu_Applicazione()
        {
            NavigationNode nodoApplicazione = new NavigationNode("Applicazione");
            CreaNavigationNode(nodoApplicazione, "Pagina Principale", "~/Home.aspx");
            CreaNavigationNode(nodoApplicazione, "Accounts di Accesso", "~/Sicurezza/Accounts.aspx");
            CreaNavigationNode(nodoApplicazione, "Elenco Ruoli", "~/Archivi/Operatori.aspx");
            CreaNavigationNode(nodoApplicazione, "Elenco Clienti", "~/Archivi/AnagraficaSoggetti.aspx");
            CreaNavigationNode(nodoApplicazione, "Configurazione Proprietà Clienti", "~/Archivi/AnagraficaSoggettiProprieta.aspx");
            rnBarraDiNavigazione.Nodes.Add(nodoApplicazione);
        }
        


        /// <summary>
        /// Gestisce l'inserimento delle voci del menu Procedure
        /// </summary>
        private void CreaVociMenu_Procedure()
        {
            NavigationNode nodoProcedure = new NavigationNode("Procedure");

            NavigationNode nodoInterventi = new NavigationNode("Interventi");
            CreaNavigationNode(nodoInterventi, "Elenco", "~/Interventi/Interventi.aspx");
            CreaNavigationNode(nodoInterventi, "Nuovo", "~/Interventi/Intervento.aspx");
            nodoProcedure.Nodes.Add(nodoInterventi);

            NavigationNode nodoOfferte = new NavigationNode("Offerte");
            CreaNavigationNode(nodoOfferte, "Elenco", "~/Offerte/ElencoOfferte.aspx");
            CreaNavigationNode(nodoOfferte, "Nuova", "~/Offerte/DettagliOfferta.aspx");
            nodoProcedure.Nodes.Add(nodoOfferte);

            NavigationNode nodoClausoleVesatorie = new NavigationNode("Clausole Vessatorie");
            CreaNavigationNode(nodoClausoleVesatorie, "Elenco", "~/Offerte/ElencoClausoleVessatorie.aspx");
            CreaNavigationNode(nodoClausoleVesatorie, "Nuova", "~/Offerte/DettagliClausolaVessatoria.aspx");
            nodoProcedure.Nodes.Add(nodoClausoleVesatorie);

            NavigationNode nodoConfigurazioneOfferte = new NavigationNode("Configurazione Offerte");
            CreaNavigationNode(nodoConfigurazioneOfferte, "Segnalibri Generazioni Offerte ", "~/Offerte/SegnalibriGenerazioneOfferta.aspx");       
            CreaNavigationNode(nodoConfigurazioneOfferte, "Articoli Aggiuntivi", "~/Preventivi/ConfigurazioniArticoliAggiuntivi.aspx");
            CreaNavigationNode(nodoConfigurazioneOfferte, "Template Invio Email Offerta Al Cliente", "~/Offerte/ConfiguraTemplateInvioEmailOffertaAlCliente.aspx");
            nodoProcedure.Nodes.Add(nodoConfigurazioneOfferte);

            NavigationNode nodoServizi = new NavigationNode("Servizi");
            CreaNavigationNode(nodoServizi, "Elenco", "~/Servizi/ElencoServizi.aspx");
            CreaNavigationNode(nodoServizi, "Nuovo", "~/Servizi/DettagliServizio.aspx");
            nodoProcedure.Nodes.Add(nodoServizi);

            NavigationNode nodoProgetti = new NavigationNode("Progetti");
            CreaNavigationNode(nodoProgetti, "Elenco", "~/Progetti/ElencoProgetti.aspx");
            CreaNavigationNode(nodoProgetti, "Nuovo", "~/Progetti/DettagliProgetto.aspx");
            CreaNavigationNode(nodoProgetti, "", "");
            CreaNavigationNode(nodoProgetti, "Archivio Attività", "~/Archivi/Attivita.aspx");
            CreaNavigationNode(nodoProgetti, "Analisi Situazione Progetti per Attivita", "~/Progetti/AnalisiSituazioneAttivita.aspx");
            nodoProcedure.Nodes.Add(nodoProgetti);

            rnBarraDiNavigazione.Nodes.Add(nodoProcedure);
        }

        /// <summary>
        /// Gestisce l'inserimento delle voci del menu Procedure
        /// </summary>
        private void CreaVociMenu_Ticketing(bool forAdmin)
        {
            NavigationNode nodoTicketing = new NavigationNode("Ticketing");
            if (!forAdmin) CreaNavigationNode(nodoTicketing, "Elenco Ticket", "~/Interventi/Tickets.aspx");
            if (!forAdmin) CreaNavigationNode(nodoTicketing, "Nuovo Ticket", "~/Interventi/Ticket.aspx");

            //if (forAdmin) CreaNavigationNode(nodoProcedure, "Elenco Configurazioni tipologie Ticket", "~/Interventi/ConfigTipologieTicketCliente.aspx");
            if (forAdmin) CreaNavigationNode(nodoTicketing, "Configurazione tipologie Ticket", "~/Interventi/ConfigTipologieTicketCliente.aspx");
            if (forAdmin) CreaNavigationNode(nodoTicketing, "Archivio Tipologie di Intervento", "~/Archivi/TipologieIntervento.aspx");
            //if (forAdmin) CreaNavigationNode(nodoTicketing, "Archivio Caratteristiche di gestione Intervento", "~/Archivi/CaratteristicheIntervento.aspx");
            if (forAdmin) CreaNavigationNode(nodoTicketing, "Archivio Reparti", "~/Archivi/RepartiUfficio.aspx");
            if (forAdmin) CreaNavigationNode(nodoTicketing, "Archivio Periodi di Festività", "~/Archivi/PeriodiFestivita.aspx");

            rnBarraDiNavigazione.Nodes.Add(nodoTicketing);
        }

        /// <summary>
        /// Gestisce l'inserimento delle voci del menu Procedure
        /// </summary>
        private void CreaVociMenu_AnalisiDati()
        {
            //NavigationNode nodoProcedure = new NavigationNode("Reports");
            //CreaNavigationNode(nodoProcedure, "Risconti passivi", "~/AnalisiDati/RiscontiPassivi.aspx");
            //CreaNavigationNode(nodoProcedure, "Statistiche fatturato", "~/AnalisiDati/StatisticheFatturato.aspx");
            //rnBarraDiNavigazione.Nodes.Add(nodoProcedure);
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Effettua la creazione del menu per gli amministratori
        /// </summary>
        private void CreaMenuAdmin()
        {
            CreaVociMenu_Applicazione();
            CreaVociMenu_Procedure();
            CreaVociMenu_Ticketing(true);
            CreaVociMenu_AnalisiDati();
            //GestisciAggiungiVociMenuPerArchivi();
            //GestisciAggiungiVociMenuPerGruppoAnagrafiche();
        }

        /// <summary>
        /// Effettua la creazione del menu
        /// </summary>
        private void CreaMenuAccountStandard()
        {
            CreaVociMenu_Applicazione();
            CreaVociMenu_Procedure();
            CreaVociMenu_AnalisiDati();
            //GestisciAggiungiVociMenuPerArchivi();
            //GestisciAggiungiVociMenuPerGruppoAnagrafiche();
        }

        /// <summary>
        /// Effettua la creazione del menu
        /// </summary>
        private void CreaMenuAccountCliente()
        {
            CreaVociMenu_Ticketing(false);
        }

        /// <summary>
        /// Effettua la creazione di un nodo di navigazione
        /// </summary>
        /// <param name="nodo"></param>
        /// <param name="testo"></param>
        /// <param name="navigateUrl"></param>
        /// <returns></returns>
        private void CreaNavigationNode(NavigationNode nodo, string testo, string navigateUrl)
        {
            if (nodo == null) throw new ArgumentNullException("nodo");
            nodo.Nodes.Add(new NavigationNode(testo, navigateUrl));
        }

        #endregion



        #region Gestione delle autorizzazioni

        //private Entities.Account utenteCollegato;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneAccounts;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneAziende;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneImpostazioneAutorizzazioni;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneAnagraficaSoggetti;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneDipartimenti;        
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneScuole;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneCorsi;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneIndirizzi;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneUnitaFormative;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneAttivitaFormative;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneElencoClassi;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneDefinizioneClassi;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneIscrizioni;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneModalitaPagamenti;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneReports;


        /// <summary>
        /// Carica gli oggetti contenenti le informazioni di accesso ai dati ed alle funzionalità esposte dalla pagina
        /// </summary>
        private void CaricaAutorizzazioni()
        {
            try
            {
                //InformazioniAccountAutenticato infoAccount = InformazioniAccountAutenticato.GetIstance();
                //utenteCollegato = infoAccount.Account;

                //Autorizzazioni_GestioneAccounts = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneAccounts);
                //Autorizzazioni_GestioneAziende = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneAziende);
                //Autorizzazioni_GestioneImpostazioneAutorizzazioni = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneImpostazioneAutorizzazioni);
                //Autorizzazioni_GestioneAnagraficaSoggetti = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneAnagraficaSoggetti);
                //Autorizzazioni_GestioneDipartimenti = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneDipartimenti);                
                //Autorizzazioni_GestioneScuole = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneScuole);
                //Autorizzazioni_GestioneCorsi = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneCorsi);
                //Autorizzazioni_GestioneIndirizzi = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneIndirizzi);
                //Autorizzazioni_GestioneUnitaFormative = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneUnitaFormative);
                //Autorizzazioni_GestioneAttivitaFormative = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneAttivitaFormative);
                //Autorizzazioni_GestioneElencoClassi = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneElencoClassi);
                //Autorizzazioni_GestioneDefinizioneClassi = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneDefinizioneClassi);
                //Autorizzazioni_GestioneIscrizioni = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneIscrizioni);
                //Autorizzazioni_GestioneModalitaPagamenti = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneModalitaPagamento);
            }
            catch (Exception ex)
            {
                //SeCoGes.Logging.LogManager.AddLogErrori(ex);
                MessageHelper.ShowErrorMessage(Page, ex);
            }
        }

        #endregion
    }
}