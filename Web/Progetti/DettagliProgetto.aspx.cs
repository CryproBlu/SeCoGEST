using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SeCoGes.Utilities;
using SeCoGEST.Entities;
using SeCoGEST.Helper;
using SeCoGEST.Web.Archivi;
using SeCoGEST.Web.UI;
using Telerik.Web.UI;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace SeCoGEST.Web.Progetti
{
    public partial class DettagliProgetto : System.Web.UI.Page
    {
        #region Properties

        /// <summary>
        /// Recupera o setta la property Enabled degli oggetti nell'usercontrol
        /// </summary>
        private bool Enabled
        {
            get
            {
                if (ViewState["Enabled"] == null)
                {
                    ViewState["Enabled"] = true;
                }

                return (bool)ViewState["Enabled"];
            }
            set
            {
                ViewState["Enabled"] = value;
                AbilitaCampi(value);
            }
        }

        /// <summary>
        /// Restituisce l'ID corrente del Progetto
        /// </summary>
        protected EntityId<Entities.Progetto> currentIDProgetto
        {
            get
            {
                if (Request.QueryString["ID"] != null)
                    return new EntityId<Entities.Progetto>(Request.QueryString["ID"]);
                else
                    return EntityId<Entities.Progetto>.Empty;
            }
        }
        /// <summary>
        /// Restituisce l'ID corrente dell'Attività di Progetto
        /// </summary>
        protected EntityId<Entities.Progetto_Attivita> currentIDAttivita
        {
            get
            {
                if (Request.QueryString["IDAttivita"] != null)
                    return new EntityId<Entities.Progetto_Attivita>(Request.QueryString["IDAttivita"]);
                else
                    return EntityId<Entities.Progetto_Attivita>.Empty;
            }
        }


        #region Pulsanti Toolbar

        /// <summary>
        /// Restituisce un riferimento al separatore SeparatoreNuovo della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_SeparatoreNuovo
        {
            get
            {
                return RadToolBar1.FindItemByValue("SeparatoreNuovo");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante Nuovo della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_Nuovo
        {
            get
            {
                return RadToolBar1.FindItemByValue("Nuovo");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al separatore del pulsante Salva
        /// </summary>
        RadToolBarItem PulsanteToolbar_SeparatoreSalva
        {
            get
            {
                return RadToolBar1.FindItemByValue("SeparatoreSalva");
            }
        }
        /// <summary>
        /// Restituisce un riferimento al pulsante Salva della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_Salva
        {
            get
            {
                return RadToolBar1.FindItemByValue("Salva");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante Aggiorna della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_Aggiorna
        {
            get
            {
                return RadToolBar1.FindItemByValue("Aggiorna");
            }
        }

        RadToolBarItem PulsanteToolbar_SeparatoreChiudi
        {
            get
            {
                return RadToolBar1.FindItemByValue("SeparatoreChiudi");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante Chiudi della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_Chiudi
        {
            get
            {
                return RadToolBar1.FindItemByValue("Chiudi");
            }
        }

        #endregion

        #endregion

        #region Intercettazione Eventi

        /// <summary>
        /// Metodo di gestione dell'evento Init della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            RadPersistenceHelper.AssociatePersistenceSessionProvider(this);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CaricaAutorizzazioni();

            reDescrizioneProgetto.Modules.Clear();

            if (!Helper.Web.IsPostOrCallBack(this))
            {
                RadPersistenceHelper.LoadState(this);

                try
                {
                    SeCoGes.Logging.LogManager.AddLogAccessi(String.Format("Accesso alla pagina '{0}'.", Request.Url.AbsolutePath));

                    Logic.Progetti ll = new Logic.Progetti();
                    Entities.Progetto entityToShow = ll.Find(currentIDProgetto);
                    //Enabled = false;

                    if (entityToShow != null)
                    {
                        //rowArticoli.Visible = true;
                        ShowData(entityToShow, ll);                        
                    }
                    else
                    {
                        //rowArticoli.Visible = false;
                        ShowData(new Entities.Progetto(), ll);
                    }

                    ApplicaAutorizzazioni();
                }
                catch (Exception ex)
                {
                    SeCoGes.Logging.LogManager.AddLogErrori(ex);
                    MessageHelper.ShowErrorMessage(this, ex);
                }
            }
        }

        ///// <summary>
        ///// Metodo di gestione dell'evento PreRender della pagina
        ///// </summary>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ApplicaAutorizzazioni();

            if (!IsPostBack)
            {
                // Leggo l'IDAttivita dalla query string
                if (currentIDAttivita.HasValue)
                {
                    ucAttivitaProgetto.SelezionaAttivita(currentIDAttivita.Value);
                }
            }
        }

        private void CaricaOperatori(Guid? idRefString, Guid? idDpoString)
        {
            Logic.Operatori llOperatori = new Logic.Operatori();
            List<Entities.Operatore> operatoriREF = llOperatori.ReadActive().ToList();
            //List<Entities.Operatore> operatoriREF = llOperatori.Read().Where(o => !o.Area && o.Attivo).ToList();
            List<Entities.Operatore> operatoriDPO = operatoriREF.ToList();

            if (idRefString != null)
            {
                if (!operatoriREF.Any(o => o.ID == idRefString))
                {
                    operatoriREF.Add(llOperatori.Find(new EntityId<Entities.Operatore>(idRefString)));
                }
            }

            if (idDpoString != null)
            {
                if (!operatoriDPO.Any(o => o.ID == idDpoString))
                {
                    operatoriDPO.Add(llOperatori.Find(new EntityId<Entities.Operatore>(idDpoString)));
                }
            }

            rcbReferenteCliente.DataSource = operatoriREF;
            rcbReferenteCliente.DataBind();
            TelerikHelper.InsertBlankComboBoxItem(rcbReferenteCliente);

            rcbDPOs.DataSource = operatoriDPO;
            rcbDPOs.DataBind();
            TelerikHelper.InsertBlankComboBoxItem(rcbDPOs);
        }

        /// <summary>
        /// Metodo di gestione dell'evento ButtonClick relativo alla toolbar presente nella pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadToolBar1_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {
            RadPersistenceHelper.SaveState(this);

            try
            {
                RadToolBarButton clickedButton = (RadToolBarButton)e.Item;

                switch (clickedButton.CommandName)
                {
                    case "Salva":
                        SaveData();
                        break;

                    case "Chiudi":
                        SaveData(true, true);
                        break;

                    default:
                        break;
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        protected void rcbCliente_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                string testoRicerca = (e.Text == null) ? String.Empty : e.Text.ToLower();
                RadComboBox combo = (RadComboBox)sender;
                combo.Items.Clear();

                //Carica tutti i Clienti accessibili dall'account corrente e che contengono il testo digitato dall'utente
                Logic.Metodo.AnagraficheClienti ll = new Logic.Metodo.AnagraficheClienti();
                IQueryable<Entities.AnagraficaClienti> queryBase = ll.Read();

                if (!string.IsNullOrWhiteSpace(testoRicerca))
                {
                    queryBase = queryBase.Where(x => x.DSCCONTO1.ToLower().Contains(testoRicerca) ||
                                                x.DSCCONTO2.ToLower().Contains(testoRicerca) ||
                                                x.CODCONTO.ToLower().Contains(testoRicerca));
                }

                int itemsPerRequest = (combo.ItemsPerRequest <= 0) ? 20 : combo.ItemsPerRequest;
                int itemOffset = e.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                int numTotaleUtenti = queryBase.Count();


                if (endOffset > numTotaleUtenti)
                    endOffset = numTotaleUtenti;

                IEnumerable<Entities.AnagraficaClienti> entities = queryBase.Skip(itemOffset).Take(itemsPerRequest);

                foreach (Entities.AnagraficaClienti entity in entities)
                {
                    RadComboBoxItem item = new RadComboBoxItem(entity.DSCCONTO1, entity.CODCONTO);
                    item.Attributes.Add("Ind", entity.INDIRIZZO);
                    item.Attributes.Add("CAP", entity.CAP);
                    item.Attributes.Add("Loc", entity.LOCALITA);
                    item.Attributes.Add("Prov", entity.PROVINCIA);
                    item.Attributes.Add("Tel", entity.TELEFONO);
                    item.Attributes.Add("State", entity.STATOALTRO.HasValue ? entity.STATOALTRO.Value.ToString() : "0");
                    item.Attributes.Add("Note", entity.NOTE1);
                    item.DataItem = entity;

                    Logic.AnagraficaSoggettiProprieta llArc = new Logic.AnagraficaSoggettiProprieta();
                    Entities.AnagraficaSoggetti_Proprieta propCliente = llArc.Find(entity.CODCONTO, Entities.AnagraficaSoggetti_ProprietaEnums.DefaultVisibilitaTicketCliente.ToString());
                    if (propCliente != null)
                    {
                        if (bool.TryParse(propCliente.Valore, out bool valore))
                        {
                            item.Attributes.Add("DefaultVisibilitaTicketCliente", propCliente.Valore.ToLower());
                        }
                    }



                    // Colora le righe in base allo stato del cliente (si analizza il campo STATOALTRO di Metodo per l'esercizio relativo all'anno solare corrente)
                    if (entity.STATOALTRO.HasValue)
                    {
                        if (entity.STATOALTRO.Value == 1)
                        {
                            item.ForeColor = System.Drawing.Color.Black;
                            item.BackColor = System.Drawing.Color.Yellow;
                        }
                        else if (entity.STATOALTRO.Value == 2)
                        {
                            item.ForeColor = System.Drawing.Color.White;
                            item.BackColor = System.Drawing.Color.Red;
                        }
                    }
                    combo.Items.Add(item);
                }

                combo.DataBind();

                if (numTotaleUtenti > 0)
                {
                    e.Message = String.Format("Clienti (da <b>1</b> a <b>{0}</b> di <b>{1}</b>)", endOffset.ToString(), numTotaleUtenti.ToString());
                }
                else
                {
                    e.Message = "Nessuna corrispondenza";
                }
            }
            catch (Exception ex)
            {
                e.Message = ex.Message;
            }
        }

        protected void rcbIndirizzi_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                RadComboBox combo = (RadComboBox)sender;
                combo.Items.Clear();

                IEnumerable<Entities.AnagraficaDestinazioneMerce> queryBase;

                string codiceCliente = string.Empty;
                if (e.Context.ContainsKey("CodiceCliente") && e.Context["CodiceCliente"].ToString() != string.Empty)
                {
                    codiceCliente = e.Context["CodiceCliente"].ToString();
                    Logic.Metodo.AnagraficaDestinazioniMerce llDest = new Logic.Metodo.AnagraficaDestinazioniMerce();
                    queryBase = llDest.Read(codiceCliente);
                }
                else
                {
                    queryBase = new List<Entities.AnagraficaDestinazioneMerce>();
                }


                int itemsPerRequest = (combo.ItemsPerRequest <= 0) ? 20 : combo.ItemsPerRequest;
                int itemOffset = e.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                int numTotaleUtenti = queryBase.Count();


                if (endOffset > numTotaleUtenti)
                    endOffset = numTotaleUtenti;


                string noteCliente = string.Empty;
                string statoCliente = "X";
                Logic.Metodo.AnagraficheClienti llAnag = new Logic.Metodo.AnagraficheClienti();
                Entities.AnagraficaClienti cliente = llAnag.Find(codiceCliente);
                if (cliente != null)
                {
                    statoCliente = cliente.STATOALTRO.HasValue ? cliente.STATOALTRO.Value.ToString() : "0";
                    noteCliente = cliente.NOTE1;
                }

                string defaultVisibilitaTicketCliente = string.Empty;
                Logic.AnagraficaSoggettiProprieta llArc = new Logic.AnagraficaSoggettiProprieta();
                Entities.AnagraficaSoggetti_Proprieta propCliente = llArc.Find(codiceCliente, Entities.AnagraficaSoggetti_ProprietaEnums.DefaultVisibilitaTicketCliente.ToString());
                if (propCliente != null)
                {
                    if (bool.TryParse(propCliente.Valore, out bool valore))
                    {
                        defaultVisibilitaTicketCliente = propCliente.Valore.ToLower();
                    }
                }

                foreach (Entities.AnagraficaDestinazioneMerce entity in queryBase.Skip(itemOffset).Take(itemsPerRequest))
                {
                    RadComboBoxItem item = new RadComboBoxItem(entity.RagioneSociale, entity.CodiceDestinazione.ToString());
                    item.Attributes.Add("Ind", entity.Indirizzo);
                    item.Attributes.Add("CAP", entity.CAP);
                    item.Attributes.Add("Loc", entity.Localita);
                    item.Attributes.Add("Prov", entity.Provincia);
                    item.Attributes.Add("Tel", entity.Telefono);
                    item.Attributes.Add("State", statoCliente);
                    item.Attributes.Add("Note", noteCliente);
                    item.Attributes.Add("DefaultVisibilitaTicketCliente", defaultVisibilitaTicketCliente);
                    item.DataItem = entity;
                    combo.Items.Add(item);
                }

                combo.DataBind();

                if (numTotaleUtenti > 0)
                {
                    e.Message = String.Format("Destinazioni (da <b>1</b> a <b>{0}</b> di <b>{1}</b>)", endOffset.ToString(), numTotaleUtenti.ToString());
                }
                else
                {
                    e.Message = "Nessuna destinazione";
                }
            }
            catch (Exception ex)
            {
                e.Message = ex.Message;
            }
        }

        #endregion

        #region Metodi di gestione

        /// <summary>
        /// Mostra nell'interfaccia i dati dell'entity passata
        /// </summary>
        /// <param name="entity"></param>
        private void ShowData(Entities.Progetto entityToShow, Logic.Progetti llOfferte)
        {
            ApplicaRedirectPulsanteAggiorna();

            if (entityToShow == null)
            {
                throw new ArgumentNullException("Parametro nullo", "entityToShow");
            }

            if (rcbStato.Items.Count() == 0)
            {
                rcbStato.Items.Add(new RadComboBoxItem("Da Eseguire", StatoProgettoEnum.DaEseguire.GetHashCode().ToString()));
                rcbStato.Items.Add(new RadComboBoxItem("In Gestione", StatoProgettoEnum.InGestione.GetHashCode().ToString()));
                rcbStato.Items.Add(new RadComboBoxItem("Eseguito", StatoProgettoEnum.Eseguito.GetHashCode().ToString()));
            }

            if (entityToShow.Chiuso)
            {
                this.Enabled = false;
                ucDocumentazioneAllegata.IsDeleteEnabled = false;
                ucDocumentazioneAllegata.IsReadOnly = true;
            }




            if (!entityToShow.ID.Equals(Guid.Empty))
            {
                lblTitolo.Text = string.Format("Progetto '{0}' ({1})", entityToShow.Titolo, entityToShow.Numero);
                CaricaOperatori(entityToShow.IDReferenteCliente, entityToShow.IDDPO);
                ucAttivitaProgetto.IDProgetto = entityToShow.ID;

                lrAttivita.Visible = true;

                lcNote.Span = 9;
                lcOperatoriCliente.Visible = true;
                AssociaOperatori.AllowAdd = true;
                AssociaOperatori.AllowDelete = true;

                lrAllegati.Visible = true;
                ucDocumentazioneAllegata.Inizializza();
            }
            else
            {
                lblTitolo.Text = "Nuovo Progetto";
                entityToShow.Numero = new Logic.Progetti().GetNuovoNumero();
                entityToShow.DataRedazione = DateTime.Now;
                CaricaOperatori(null, null);

                PulsanteToolbar_SeparatoreChiudi.Visible = false;
                PulsanteToolbar_Chiudi.Visible = false;

                lrAttivita.Visible = false;

                lcNote.Span = 12;
                lcOperatoriCliente.Visible = false;
                AssociaOperatori.AllowAdd = false;
                AssociaOperatori.AllowDelete = false;

                lrAllegati.Visible = false;
            }

            rntbNumeroProgetto.Value = entityToShow.Numero;
            rtbTitoloProgetto.Text = entityToShow.Titolo;
            rdtpDataRedazione.SelectedDate = entityToShow.DataRedazione;
            reDescrizioneProgetto.Content = entityToShow.Descrizione;
            rcbCliente.SelectedValue = entityToShow.CodiceCliente;
            rcbCliente.Text = entityToShow.RagioneSociale;


            Logic.Metodo.AnagraficheClienti llAnagraficheClienti = new Logic.Metodo.AnagraficheClienti();
            Entities.AnagraficaClienti cliente = llAnagraficheClienti.Find(entityToShow.CodiceCliente);
            if (cliente != null)
            {
                string stato = cliente.STATOALTRO.HasValue ? cliente.STATOALTRO.Value.ToString() : "0";
                if (stato != "X")
                {
                    if (stato == "0")
                    {
                        lblStatoCliente.Text = "";
                        lblStatoCliente.ForeColor = System.Drawing.Color.Black;
                        lblStatoCliente.BackColor = System.Drawing.Color.Transparent;
                    }
                    else if (stato == "1")
                    {
                        lblStatoCliente.Text = "&nbsp;CLIENTE SEGNALATO&nbsp;";
                        lblStatoCliente.ForeColor = System.Drawing.Color.Yellow;
                        lblStatoCliente.BackColor = System.Drawing.ColorTranslator.FromHtml("#3F48CC");
                    }
                    else if (stato == "2")
                    {
                        lblStatoCliente.Text = "&nbsp;CLIENTE BLOCCATO&nbsp;";
                        lblStatoCliente.ForeColor = System.Drawing.Color.Orange;
                        lblStatoCliente.BackColor = System.Drawing.ColorTranslator.FromHtml("#880015");
                    }
                }
                lblNoteCliente.Text = cliente.NOTE1;
            }

            rcbIndirizzi.SelectedValue = entityToShow.IdDestinazione;
            rcbIndirizzi.Text = entityToShow.DestinazioneMerce;
            rtbIndirizzo.Text = entityToShow.Indirizzo;
            rtbCAP.Text = entityToShow.CAP;
            rtbLocalita.Text = entityToShow.Localita;
            rtbProvincia.Text = entityToShow.Provincia;
            rtbTelefono.Text = entityToShow.Telefono;
            rtbNumeroCommessa.Text = entityToShow.NumeroCommessa;
            rtbCodiceContratto.Text = entityToShow.CodiceContratto;
            rtbNote.Text = entityToShow.Note;

            RadComboBoxItem selectedRef = rcbReferenteCliente.FindItemByValue(entityToShow.IDReferenteCliente.ToString());
            if(selectedRef != null) selectedRef.Selected = true;

            if(entityToShow.IDDPO.HasValue)
            {
                RadComboBoxItem selectedDPO = rcbDPOs.FindItemByValue(entityToShow.IDDPO.Value.ToString());
                if (selectedDPO != null) selectedDPO.Selected = true;
            }

            RadComboBoxItem item = rcbStato.FindItemByValue(entityToShow.Stato.ToString());
            if (item != null)
            {
                item.Selected = true;
            }
            else
            {
                rcbStato.SelectedValue = StatoProgettoEnum.DaEseguire.GetHashCode().ToString();
            }
            //if (entityToShow.Stato == StatoProgettoEnum.Eseguito.GetHashCode())
            //{
            //}
        }

        /// <summary>
        /// Assegna l'indirizzo della pagina da aprire al pulsante Aggiorna.
        /// L'indirizzo è esattamente lo stesso della pagina aperta.
        /// </summary>
        private void ApplicaRedirectPulsanteAggiorna()
        {
            if (PulsanteToolbar_Aggiorna != null)
                ((RadToolBarButton)PulsanteToolbar_Aggiorna).NavigateUrl = Request.Url.ToString();
        }

        /// <summary>
        /// Carica gli oggetti contenenti le informazioni di accesso ai dati ed alle funzionalità esposte dalla pagina
        /// </summary>
        private void CaricaAutorizzazioni()
        {
        }

        /// <summary>
        /// Blocca o permette l'accesso ai dati ed applica agli oggetti dell'interfaccia lo stile in base alle regole di accesso dell'azienda corrente
        /// </summary>
        private void ApplicaAutorizzazioni()
        {
        }

        /// <summary>
        /// Abilita o disabilita i campi dell'interfaccia in base al parametro passato
        /// </summary>
        /// <param name="abilitaCampi"></param>
        private void AbilitaCampi(bool enabled)
        {
            PulsanteToolbar_SeparatoreNuovo.Visible = enabled;
            PulsanteToolbar_Nuovo.Visible = enabled;
            PulsanteToolbar_SeparatoreSalva.Visible = enabled;
            PulsanteToolbar_Salva.Visible = enabled;
            PulsanteToolbar_SeparatoreChiudi.Visible = enabled;
            PulsanteToolbar_Chiudi.Visible = enabled;

            rntbNumeroProgetto.ReadOnly = !enabled;
            rtbTitoloProgetto.ReadOnly = !enabled;
            reDescrizioneProgetto.Enabled = enabled;
        }

        /// <summary>
        ///  Memorizza i dati presenti nei controlli dell'interfaccia
        /// </summary>
        /// <param name="reloadPageAfterSave"></param>
        private void SaveData(bool reloadPageAfterSave = true, bool closeProjectOnSave = false)
        {
            // Valida i dati nella pagina
            MessagesCollector erroriDiValidazione = ValidaDati();
            if (erroriDiValidazione.HaveMessages)
            {
                // Se qualcosa non va bene mostro un avviso all'utente
                MessageHelper.ShowErrorMessage(this, erroriDiValidazione.ToString("<br />"));
                return;
            }

            // Definisco una variabile per memorizzare l'entità da salvare
            Entities.Progetto entityToSave = null;

            // Definisco una variabile che conterrà l'Codice dell'entity salvata. 
            // Se per qualche motivo l'entity non viene salvata allora entityId rimarrà String.Empty
            string entityId = String.Empty;
            Logic.Progetti ll = new Logic.Progetti();

            try
            {
                //Se currentID contiene un Codice allora cerco l'entity nel database
                if (currentIDProgetto.HasValue)
                {
                    entityToSave = ll.Find(currentIDProgetto);
                }

                // Definisco una variabile che indica se si deve compiere una azione di creazione di una nuova entity o di modifica
                bool nuovo = false;

                if (entityToSave == null)
                {
                    if (currentIDProgetto.HasValue)
                    {
                        // Se CurrentId ha un valore ma entityToSave è null allora vuol dire che la pagina è stata aperta
                        // per modificare un entità che adesso non esiste più nella base dati.
                        // In questo caso avviso l'utente
                        throw new Exception(
                            "Il Progetto che si sta variando non esiste più in archivio."
                                + "\n\rOperazione annullata!");
                    }

                    // Al contrario, se entityToSave è nulla e CurrentId è vuota
                    // vuol dire che la pagina è stata aperta per la creazione di una nuova entità
                    nuovo = true;
                    entityToSave = new Entities.Progetto();

                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriDallaView(entityToSave, ll);

                    //Creo nuova entity
                    ll.Create(entityToSave, false);
                }
                else
                {
                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriDallaView(entityToSave, ll);
                }

                if (closeProjectOnSave)
                {
                    entityToSave.Chiuso = true;
                }

                // Persisto le modifiche sulla base dati nella transazione
                ll.SubmitToDatabase();

                SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - {1} l'entity Progetto con Numero '{2}' e Nome '{3}'.", Request.Url.AbsolutePath, ((nuovo) ? "Creato" : "Salvato"), entityToSave.Numero, entityToSave.Titolo));

                // Memorizzo l'Codice dell'entità
                entityId = entityToSave.ID.ToString();
            }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);

                ll.RollbackTransaction();

                // ...e mostro il messaggio d'errore all'utente
                MessageHelper.ShowErrorMessage(this, ex.Message);
            }
            finally
            {
                // Alla fine, se il salvataggio è andato a buon fine (entityId != Guid.Empty)
                // allora ricarico la pagina aprendola in modifica
                if (entityId != String.Empty && reloadPageAfterSave)
                {
                    Helper.Web.ReloadPage(this, entityId);
                }
            }
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Restituisce un oggetto contenente gli eventuali errori di validazione dei dati
        /// </summary>
        /// <returns></returns>
        private MessagesCollector ValidaDati()
        {
            MessagesCollector messaggi = new MessagesCollector();

            //Errori dei validatori client
            Page.Validate(String.Empty);
            if (!Page.IsValid)
            {
                foreach (IValidator validatore in Page.GetValidators(String.Empty))
                {

                    if (!validatore.IsValid)
                    {
                        messaggi.Add(validatore.ErrorMessage);
                    }
                }
                if (messaggi.HaveMessages) return messaggi;
            }

            Logic.Progetti ll = new Logic.Progetti();
            Entities.Progetto entityToSave = ll.Find(currentIDProgetto);

            Entities.Progetto entitaTrovata = ll.Find(new EntityString<Progetto>(rntbNumeroProgetto.Text.Trim()));
            if ((entityToSave == null && entitaTrovata != null) ||
                (entityToSave != null && entitaTrovata != null && entityToSave.ID != entitaTrovata.ID))
            {
                messaggi.Add("Il Numero Progetto inserito è già stato utilizzato");
            }

            if (rcbReferenteCliente.SelectedValue.Trim() == string.Empty || !Guid.TryParse(rcbReferenteCliente.SelectedValue, out Guid idref))
            {
                messaggi.Add("Il Referente del Cliente è obbligatorio.");
            }

            return messaggi;
        }

        /// <summary>
        /// Legge i dati inseriti nell'interfaccia e li inserisce nell'entity passata
        /// </summary>
        /// <param name="entityToFill"></param>
        public void EstraiValoriDallaView(Entities.Progetto entityToFill, Logic.Progetti logic)
        {
            if (entityToFill == null)
            {
                throw new ArgumentNullException("Parametro nullo", "entityToFill");
            }

            entityToFill.Numero = (int)rntbNumeroProgetto.Value;
            entityToFill.DataRedazione = rdtpDataRedazione.SelectedDate.Value;
            entityToFill.NumeroCommessa = rtbNumeroCommessa.Text.Trim();
            entityToFill.CodiceContratto = rtbCodiceContratto.Text.Trim();
            entityToFill.Stato = byte.Parse(rcbStato.SelectedValue);
            entityToFill.Titolo = rtbTitoloProgetto.Text.Trim();            
            entityToFill.Descrizione = reDescrizioneProgetto.Content.Trim();
            entityToFill.Note = rtbNote.Text.Trim();

            entityToFill.CodiceCliente = rcbCliente.SelectedValue;
            entityToFill.RagioneSociale = rcbCliente.Text.Trim();
            entityToFill.IdDestinazione = rcbIndirizzi.SelectedValue;
            entityToFill.DestinazioneMerce = rcbIndirizzi.Text.Trim();
            entityToFill.Indirizzo = rtbIndirizzo.Text.Trim();
            entityToFill.CAP = rtbCAP.Text.Trim();
            entityToFill.Localita = rtbLocalita.Text.Trim();
            entityToFill.Provincia = rtbProvincia.Text.Trim();
            entityToFill.Telefono = rtbTelefono.Text.Trim();

            if (Guid.TryParse(rcbReferenteCliente.SelectedValue, out Guid idref))
            {
                entityToFill.IDReferenteCliente = idref;
            }
            else
            {
                throw new ArgumentNullException("Parametro nullo", "Referente Cliente");
            }
 
            if(Guid.TryParse(rcbDPOs.SelectedValue, out Guid iddpo))
            {
                entityToFill.IDDPO = iddpo;
            }
            else
            {
                entityToFill.IDDPO = null;
            }

        }


        /// <summary>
        /// Metodo di gestione dell'evento AjaxRequest relativo al pannello ajax della tab Allegati
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rapAllegati_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (!currentIDProgetto.HasValue)
            {
                ucDocumentazioneAllegata.IsReadOnly = true;
            }
            ucDocumentazioneAllegata.Inizializza();
        }


        /// <summary>
        /// Metodo di gestione dell'evento OnNeedDataSource dell'usercontrol CorsiFrequentatiList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDocumentazioneAllegata_NeedDataSource(object sender, EventArgs e)
        {
            UI.DocumentazioneAllegata ucDocumentazioneAllegata = (UI.DocumentazioneAllegata)sender;

            Logic.Allegati llAllegati = new Logic.Allegati();
            ucDocumentazioneAllegata.DataSource = llAllegati.Read(currentIDProgetto);
        }


        /// <summary>
        /// Metodo di gestione dell'evento OnNeedSaveAllegato dell'usercontrol CorsiFrequentatiList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDocumentazioneAllegata_NeedSaveAllegato(object sender, UI.DocumentoAllegatoDaSalvareEventArgs e)
        {
            // Nel caso in cui esista l'allegato documento da salvare ..
            if (e != null && e.DocumentoAllegato != null)
            {
                // Viene istanziato il logiclayer dei documenti allegati e degli allegati delle anagrafiche
                Logic.Allegati llAllegati = new Logic.Allegati();
                e.DocumentoAllegato.IDLegame = currentIDProgetto.Value;
                llAllegati.Create(e.DocumentoAllegato, true);
            }
            else
            {
                throw new Exception("L'allegato da salvare passato è nullo.");
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento OnNeedDeleteAllegato dell'usercontrol CorsiFrequentatiList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDocumentazioneAllegata_NeedDeleteAllegato(object sender, UI.DocumentoAllegatoDaSalvareEventArgs e)
        {
            // Nel caso in cui esista l'allegato documento da salvare ..
            if (e != null && e.DocumentoAllegato != null)
            {
                // Viene istanziato il logiclayer degli allegati
                Logic.Allegati llAllegati = new Logic.Allegati();

                // Viene recuperato la relazione del documento da cancellare ..
                Entities.Allegato entityAllegatoDaCancellare = llAllegati.Find(e.DocumentoAllegato.ID);

                // Nel caso in cui l'entity di relazione esista ..
                if (entityAllegatoDaCancellare != null)
                {
                    // Infine viene utilizzato il logiclayer per eliminare l'allegato da cancellare
                    llAllegati.Delete(entityAllegatoDaCancellare, true);
                }
                else
                {
                    throw new Exception("L'allegato da eliminare non esiste.");
                }
            }
            else
            {
                throw new Exception("L'allegato da eliminare passato è nullo.");
            }
        }

        #endregion


        #region Operatori Progetto

        private List<Entities.Operatore> GetOperatoriList()
        {
            Logic.Progetto_Operatori llOP = new Logic.Progetto_Operatori();
            var operatoriprogetto = llOP.Read(currentIDProgetto.Value);
            return operatoriprogetto.Select(o => o.Operatore).ToList();
        }

        protected void AssociaOperatori_LeggiOperatori(object sender, EventArgs e)
        {
            AssociaOperatori.SetDataSource(GetOperatoriList());
        }

        protected void AssociaOperatori_AssociaOperatore(object sender, AssociaOperatoriEventArgs e)
        {
            if (e.Operatore != null)
            {
                if (!GetOperatoriList().Select(o => o.ID).Contains(e.Operatore.ID))
                {
                    Entities.Progetto_Operatore po = new Progetto_Operatore();
                    po.IDProgetto = currentIDProgetto.Value;
                    po.IDOperatore = e.Operatore.ID;
                    Logic.Progetto_Operatori llOP = new Logic.Progetto_Operatori();
                    llOP.Create(po, true);
                    AssociaOperatori.Refresh();
                }
            }

        }

        protected void AssociaOperatori_RimuoviOperatore(object sender, AssociaOperatoriEventArgs e)
        {
            if (e.Operatore != null)
            {
                Logic.Progetto_Operatori llOP = new Logic.Progetto_Operatori();
                Entities.Progetto_Operatore po = llOP.Find(currentIDProgetto.Value, e.Operatore.ID);
                if (po != null)
                {
                    llOP.Delete(po, true);
                }
            }
        }

        #endregion
    }
}