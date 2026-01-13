using System;
using System.Collections.Generic;
using System.Linq;
using SeCoGes.Utilities;
using SeCoGEST.Entities;
using Telerik.Web.UI;
using SeCoGEST.Logic;
using static System.Windows.Forms.AxHost;
using System.Web.UI.WebControls;
using System.IO;
using SeCoGEST.Infrastructure;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;
using System.Collections;

namespace SeCoGEST.Web.Interventi
{
    public partial class Intervento : System.Web.UI.Page
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
        /// Restituisce l'ID corrente
        /// </summary>
        protected EntityId<Entities.Intervento> currentID
        {
            get
            {
                if (Request.QueryString["ID"] != null)
                    return new EntityId<Entities.Intervento>(Request.QueryString["ID"]);
                else
                    return EntityId<Entities.Intervento>.Empty;
            }
        }

        private Guid? IDUltimaDiscussione
        {
            get
            {
                if (ViewState["IDUltimaDiscussione"] == null)
                {
                    return null;
                }
                else
                {
                    return Guid.Parse(ViewState["IDUltimaDiscussione"].ToString());
                }
            }
            set
            {
                ViewState["IDUltimaDiscussione"] = value;
            }
        }

        #region Pulsanti Toolbar

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
        /// Restituisce un riferimento al separatore del pulsante Salva
        /// </summary>
        RadToolBarItem PulsanteToolbar_SeparatoreSostituisci
        {
            get
            {
                return RadToolBar1.FindItemByValue("SeparatoreSostituisci");
            }
        }
        /// <summary>
        /// Restituisce un riferimento al pulsante Sostituisci della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_Sostituisci
        {
            get
            {
                return RadToolBar1.FindItemByValue("Sostituisci");
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

        /// <summary>
        /// Restituisce un riferimento al separatore del pulsante Chiudi
        /// </summary>
        RadToolBarItem PulsanteToolbar_SeparatoreChiudi
        {
            get
            {
                return RadToolBar1.FindItemByValue("SeparatoreChiudi");
            }
        }
        /// <summary>
        /// Restituisce un riferimento al pulsante Salva della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_Chiudi
        {
            get
            {
                return RadToolBar1.FindItemByValue("Chiudi");
            }
        }


        /// <summary>
        /// Restituisce un riferimento al separatore del pulsante GeneraDocumento
        /// </summary>
        RadToolBarItem PulsanteToolbar_SeparatoreGeneraDocumento
        {
            get
            {
                return RadToolBar1.FindItemByValue("SeparatoreGeneraDocumento");
            }
        }
        /// <summary>
        /// Restituisce un riferimento al pulsante GeneraDocumento della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_GeneraDocumento
        {
            get
            {
                return RadToolBar1.FindItemByValue("GeneraDocumento");
            }
        }


        /// <summary>
        /// Restituisce un riferimento al separatore del pulsante RiapriIntervento
        /// </summary>
        RadToolBarItem PulsanteToolbar_SeparatoreRiapriIntervento
        {
            get
            {
                return RadToolBar1.FindItemByValue("SeparatoreRiapriIntervento");
            }
        }
        /// <summary>
        /// Restituisce un riferimento al pulsante RiapriIntervento della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_RiapriIntervento
        {
            get
            {
                return RadToolBar1.FindItemByValue("RiapriIntervento");
            }
        }
        #endregion

        #endregion

        #region Intercettazione Eventi

        /// <summary>
        /// Metodo di gestione dell'evento Load della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CaricaAutorizzazioni();

            if (!Helper.Web.IsPostOrCallBack(this))
            {
                try
                {
                    SeCoGes.Logging.LogManager.AddLogAccessi(String.Format("Accesso alla pagina '{0}'.", Request.Url.AbsolutePath));

                    FillComboStatoIntervento();
                    FillComboTipologiaIntervento();

                    Logic.Interventi ll = new Logic.Interventi();
                    Entities.Intervento entityToShow = ll.Find(currentID.Value);


                    if (entityToShow != null)
                    {
                        //Popola_AnniAccademici(entityToShow.Id);
                        ShowData(entityToShow);
                    }
                    else
                    {
                        //Popola_AnniAccademici(null);
                        ShowData(new Entities.Intervento());
                    }

                    ApplicaAutorizzazioni();
                }
                catch (Exception ex)
                {
                    SeCoGes.Logging.LogManager.AddLogErrori(ex);
                    MessageHelper.ShowErrorMessage(this, ex.Message);
                }
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento PreRender della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ApplicaAutorizzazioni();

            // Dopo tutto, eseguo il PreRender definito nella BaseWebUIPage che applica le eventuali restrizioni di accesso ai controlli della pagina
            //base.Page_PreRender(sender, e);
        }

        /// <summary>
        /// Metodo di gestione dell'evento ButtonClick relativo alla toolbar presente nella pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadToolBar1_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {
            try
            {
                RadToolBarButton clickedButton = (RadToolBarButton)e.Item;

                switch (clickedButton.CommandName)
                {
                    case "Salva":
                        txtMotivazioneSostituzione.Text = string.Empty;
                        SaveData();
                        break;

                    case "Chiudi":
                        ChiudiIntervento();
                        break;

                    case "GeneraDocumento":
                        GeneraDocumento();
                        break;

                    case "RiapriIntervento":
                        RiapriIntervento();
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
        protected void btSostituisci_Click(object sender, EventArgs e)
        {
            SubstituteTicket();
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
                    if(propCliente != null)
                    {
                        if(bool.TryParse(propCliente.Valore, out bool valore))
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
                if(cliente != null)
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

        #region Griglia Voci Predefinite Intervento

        /// <summary>
        /// Metodo di gestione dell'evento ItemCommand relativo alla griglia degli utenti
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        protected void rgVociPredefiniteIntervento_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {

            if (e != null && !String.IsNullOrEmpty(e.CommandName))
            {
                if (e.Item is GridDataItem)
                {
                    string noteValue = ((GridDataItem)e.Item).GetDataKeyValue("Descrizione").ToString();

                    switch (e.CommandName)
                    {

                        case "InserisciDescrizioneInterventoPrima":
                            rtbDefinizione.Text = String.Join(Environment.NewLine, noteValue, rtbDefinizione.Text);
                            rpbVociPredefiniteIntervento.CollapseAllItems();
                            break;
                        case "InserisciDescrizioneInterventoDopo":
                            rtbDefinizione.Text = String.Join(Environment.NewLine, rtbDefinizione.Text, noteValue);
                            rpbVociPredefiniteIntervento.CollapseAllItems();
                            break;
                        default:
                            return;
                    }
                }
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento NeedDataSource relativo alla griglia degli utenti
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        protected void rgVociPredefiniteIntervento_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Logic.VociPredefiniteInterventi llVociPredefiniteInterventi = new Logic.VociPredefiniteInterventi();
            rgVociPredefiniteIntervento.DataSource = llVociPredefiniteInterventi.Read();
        }

        /// <summary>
        /// Metodo di gestione dell'evento ItemCreate relativo alla griglia degli utenti
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        protected void rgVociPredefiniteIntervento_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            // Traduce le voci del menu di filtro della griglia. Questa operazione deve essere fatta ad ogni post
            Helper.TelerikHelper.TraduciElementiGriglia(e);
        }

        #endregion

        #endregion

        #region Metodi di gestione

        /// <summary>
        /// Mostra nell'interfaccia i dati dell'entity passata
        /// </summary>
        /// <param name="entity"></param>
        private void ShowData(Entities.Intervento entityToShow)
        {
            ApplicaRedirectPulsanteAggiorna();
            PulsanteToolbar_SeparatoreSostituisci.Visible = true;
            PulsanteToolbar_Sostituisci.Visible = true;
            lcTicketProvenienza.Visible = false;
            lcTicketSostitutivo.Visible = false;
            //lrDescrizioneIntervento.Visible = true;

            InformazioniAccountAutenticato infoAccount = InformazioniAccountAutenticato.GetIstance();

            if (entityToShow == null)
            {
                throw new ArgumentNullException("Parametro nullo", "entityToShow");
            }

            if (entityToShow.Intervento_Statos.Any(s => s.StatoEnum == StatoInterventoEnum.Chiuso))
            {
                //this.Enabled = false;
                PulsanteToolbar_SeparatoreSostituisci.Visible = false;
                PulsanteToolbar_Sostituisci.Visible = false;
                ucDocumentazioneAllegata.IsDeleteEnabled = utenteCollegato != null && utenteCollegato.Amministratore.HasValue && utenteCollegato.Amministratore.Value;
                //rfvRichiesteProblematicheRiscontrate.Enabled = false;
            }
            if (entityToShow.Chiuso.HasValue && entityToShow.Chiuso.Value == true)
            {
                this.Enabled = false;

                if(infoAccount != null && infoAccount.Account != null && infoAccount.Account.TipologiaEnum != TipologiaAccountEnum.SeCoGes)
                {
                    ucDocumentazioneAllegata.IsReadOnly = true;
                }
            }

            if (!entityToShow.ID.Equals(Guid.Empty))
            {
                lblTitolo.Text = string.Format("Intervento N.{0}", entityToShow.Numero);

                chkVisibileAlCliente.Checked = entityToShow.VisibileAlCliente;
                lrArticoli.Visible = true;
                ucArticoliIntervento.Visible = true;
                ucArticoliIntervento.IDIntervento = new EntityId<Entities.Intervento>(entityToShow.ID);
                ucArticoliIntervento.CodiceCliente = entityToShow.CodiceCliente;
                ucArticoliIntervento.DataIntervento = entityToShow.DataPrevistaIntervento.HasValue ? entityToShow.DataPrevistaIntervento.Value : entityToShow.DataRedazione;


                lrInfoOperatori.Visible = true;
                ucOperatoriIntervento.Visible = true;
                ucOperatoriIntervento.IDIntervento = new EntityId<Entities.Intervento>(entityToShow.ID);
                ucOperatoriIntervento.Inizializza();
                if(this.Enabled) ucOperatoriIntervento.AggiungiRigheSoloSeVuoto(2);
                lrAllegati.Visible = true;
                ucDocumentazioneAllegata.Inizializza();


                ////rcbTipologiaInterventoTicket.SelectedValue = entityToShow.IDConfigurazioneTipologiaTicket;
                //if (entityToShow.IDConfigurazioneTipologiaTicket != null)
                //{
                //    ConfigurazioneTipologiaTicketCliente confCliente = new Logic.ConfigurazioniTipologieTicketCliente().Find(new EntityId<ConfigurazioneTipologiaTicketCliente>(entityToShow.IDConfigurazioneTipologiaTicket));
                //    rcbTipologiaInterventoTicket.SelectedValue = confCliente.Id.ToString();
                //    rcbTipologiaInterventoTicket.Text = confCliente.NomeEstesoTipologiaIntervento;
                //    //RadComboBoxItem itemConfigurazioneTipologiaTicket = rcbTipologiaInterventoTicket.FindItemByValue(entityToShow.IDConfigurazioneTipologiaTicket.ToString());
                //    //if (itemConfigurazioneTipologiaTicket != null)
                //    //{
                //    //    itemConfigurazioneTipologiaTicket.Selected = true;
                //    //}
                //    ShowConfigurazioneTipologiaIntervento(confCliente.Id.ToString());
                //}
                //else
                //{
                //    rcbTipologiaInterventoTicket.SelectedValue = string.Empty;
                //    rcbTipologiaInterventoTicket.Text = string.Empty;
                //}
            }
            else
            {
                lblTitolo.Text = "Nuovo Intervento";

                chkVisibileAlCliente.Checked = false;
                lrArticoli.Visible = false;
                ucArticoliIntervento.Visible = false;

                lrInfoOperatori.Visible = false;
                ucOperatoriIntervento.Visible = false;

                lrPrimaRichiesta.Visible = false; 
                lrAllegati.Visible = false;

                Logic.Interventi ll = new Logic.Interventi();
                entityToShow.Numero = ll.GetNuovoNumeroIntervento();
                entityToShow.DataRedazione = DateTime.Now;



                // Se la pagina è stata aperta per la creazione di un Intervento legato ad una Attività di progetto allora lo associa
                if (Request.QueryString["AttivitaId"] != null && Guid.TryParse(Request.QueryString["AttivitaId"], out Guid idAttivita))
                {
                    Logic.Progetto_Attività llPA = new Progetto_Attività();
                    Entities.Progetto_Attivita attivita = llPA.Find(idAttivita);
                    if (attivita != null && attivita.IDTicket == null)
                    {
                        entityToShow.Definizione = attivita.Descrizione;

                        if(attivita.Progetto.CodiceCliente != string.Empty)
                        {
                            entityToShow.CodiceCliente = attivita.Progetto.CodiceCliente;
                        }
                        if (attivita.Progetto.RagioneSociale != string.Empty)
                        {
                            entityToShow.RagioneSociale = attivita.Progetto.RagioneSociale;
                        }
                        if (attivita.Progetto.NumeroCommessa != string.Empty)
                        {
                            entityToShow.NumeroCommessa = attivita.Progetto.NumeroCommessa;
                        }
                    }
                }

            }

            this.Title = lblTitolo.Text;

            rntbNumeroIntervento.Value = entityToShow.Numero;
            rdtpDataRedazione.SelectedDate = entityToShow.DataRedazione;
            rdtpDataPrevistaIntervento.SelectedDate = entityToShow.DataPrevistaIntervento;
            rcbCliente.SelectedValue = entityToShow.CodiceCliente;
            rcbCliente.Text = entityToShow.RagioneSociale;

            
            Logic.Metodo.AnagraficheClienti llAnagraficheClienti = new Logic.Metodo.AnagraficheClienti();
            Entities.AnagraficaClienti cliente = llAnagraficheClienti.Find(entityToShow.CodiceCliente);
            if(cliente != null)
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
            rtbOggetto.Text = entityToShow.Oggetto;
            rtbDefinizione.Text = entityToShow.Definizione;
            rtbNumeroCommessa.Text = entityToShow.NumeroCommessa;
            rtbNote.Text = entityToShow.Note;            

            if (entityToShow.Stato.HasValue)
            {
                RadComboBoxItem rcbi = rcbStatoIntervento.FindItemByValue(entityToShow.Stato.Value.ToString());
                if (rcbi != null)
                {
                    rcbi.Selected = true;
                    if (rcbi.Value == StatoInterventoEnum.Validato.GetHashCode().ToString()) // Se l'intervento è Validato ma non ancora Chiuso allora si consente la Chiusura
                    {
                        if (!entityToShow.Chiuso.HasValue || entityToShow.Chiuso.Value == false)
                        {
                            PulsanteToolbar_SeparatoreChiudi.Visible = true;
                            PulsanteToolbar_Chiudi.Visible = true;
                        }
                    }
                }
            }

            if (entityToShow.IdTipologia.HasValue)
            {
                RadComboBoxItem rci = rcbTipologiaIntervento.FindItemByValue(entityToShow.IdTipologia.Value.ToString());
                if (rci != null)
                {
                    rci.Selected = true;
                }
            }


            chkChiamata.Checked = (entityToShow.Chiamata.HasValue && entityToShow.Chiamata.Value);
            rtbReferenteChiamata.Text = entityToShow.ReferenteChiamata;

            chkUrgente.Checked = (entityToShow.Urgente.HasValue && entityToShow.Urgente.Value);
            chkInterventoInterno.Checked = (entityToShow.Interno.HasValue && entityToShow.Interno.Value);

            rdtpDataNotifica.SelectedDate = entityToShow.DataNotifica;
            rtbTestoNotifica.Text = entityToShow.TestoNotifica;




            //reRichiesteProblematicheRiscontrate.Content = entityToShow.RichiesteProblematicheRiscontrate;
            //IDUltimaDiscussione = null;
            //txtRichiesteProblematicheRiscontrate.Text = entityToShow.RichiesteProblematicheRiscontrate;
            //if (entityToShow.Intervento_Discussiones.Any())
            //{
            //    txtRichiesteProblematicheRiscontrate.Text = String.Empty;

            //    IEnumerable<Entities.Intervento_Discussione> discussioni = entityToShow.Intervento_Discussiones.OrderBy(x => x.DataCommento);
            //    Entities.Intervento_Discussione primaDiscussione = discussioni.FirstOrDefault();
            //    if(primaDiscussione != null)
            //    {
            //        discussioni = discussioni.Except(new Entities.Intervento_Discussione[] { primaDiscussione });
            //        diPrimaRichiesta.ShowDiscussione(primaDiscussione, true);
            //        //rtbPrimaRichiesta.Text = primaRichiesta.Commento;
            //        lrPrimaRichiesta.Visible = true;
            //    }
            //    repDiscussioni.DataSource = discussioni;
            //    repDiscussioni.DataBind();


            //    // Se l'ultima discussione è relativa allo stesso utente che sta editando l'intervento, allora fa in modo che non venga storicizzata nella chat ma che rimanga sempre la discussione attiva
            //    Entities.Intervento_Discussione ultimaDiscussione = discussioni.OrderByDescending(x => x.DataCommento).FirstOrDefault();
            //    if (ultimaDiscussione == null && primaDiscussione != null) ultimaDiscussione = primaDiscussione;
            //    if (ultimaDiscussione != null)
            //    {
            //        if (ultimaDiscussione.IDAccount == infoAccount.Account.ID)
            //        {
            //            IDUltimaDiscussione = ultimaDiscussione.ID;
            //            txtRichiesteProblematicheRiscontrate.Text = ultimaDiscussione.Commento;
            //        }
            //    }
            //}
            //GestisciVisualizzazioneRadEditorRichiesteProblematicheRiscontrate(entityToShow);
            ShowDiscussion(entityToShow);



            if (entityToShow.InterventoOriginale != null)
            {
                lcTicketProvenienza.Visible = true;
                hlLinkTicketProvenienza.Text = entityToShow.InterventoOriginale.Numero.ToString();
                hlLinkTicketProvenienza.NavigateUrl = this.Request.Path + "?id=" + entityToShow.InterventoOriginale.ID.ToString();
                txtMotivazioneSostituzioneTicketProvenienza.Text = entityToShow.InterventoOriginale.MotivazioneSostituzione;
                lblTitolo.Text = $"Intervento n.{entityToShow.Numero} in sostituzione al n.{entityToShow.InterventoOriginale.Numero}";
            }
            if (entityToShow.InterventoSostitutivo != null)
            {
                lcTicketSostitutivo.Visible = true;
                hlLinkTicketSostitutivo.Text = entityToShow.InterventoSostitutivo.Numero.ToString();
                hlLinkTicketSostitutivo.NavigateUrl = this.Request.Path + "?id=" + entityToShow.InterventoSostitutivo.ID.ToString();
                txtMotivazioneSostituzioneTicketSostitutivo.Text = entityToShow.MotivazioneSostituzione;
            }


            if (entityToShow.IDConfigurazioneTipologiaTicket != null)
            {
                ConfigurazioneTipologiaTicketCliente confCliente = new Logic.ConfigurazioniTipologieTicketCliente().Find(new EntityId<ConfigurazioneTipologiaTicketCliente>(entityToShow.IDConfigurazioneTipologiaTicket));
                rcbTipologiaInterventoTicket.SelectedValue = confCliente.Id.ToString();
                rcbTipologiaInterventoTicket.Text = confCliente.NomeEstesoTipologiaIntervento;
                ShowConfigurazioneTipologiaIntervento(confCliente.Id.ToString());
            }
            else
            {
                rcbTipologiaInterventoTicket.SelectedValue = string.Empty;
                rcbTipologiaInterventoTicket.Text = string.Empty;
            }





            GestisciVisualizzazioneTastoGenerazioneDocumento();
            GestisciVisualizzazioneTastoRiapriIntervento(entityToShow);
        }

        /// <summary>
        /// Gestisce la visualizzazione dei messaggi di richiesta e risposta.
        /// La casella di testo di inserimento della richiesta viene visualizzata sempre.
        /// L'unico caso nel quale non viene mostrata è ad intervento chiuso.
        /// Questa casella viene sempre mostrata vuolta e pronta a ricevere il testo di una nuova discussione tranne nel caso in cui l'ultima discussione sia del medesimo utente che sta visualizzando l'intervento.
        /// In questo caso la casella viene usata per l'edit dell'ultima discussione.
        /// La serie di discussioni viene mostrata in un elenco di caselle colorate in base all'utente che le ha scritte.
        /// La prima discussione viene invece sempre mostrata in una casella bianca simile a quella di inserimento iniziale.
        /// </summary>
        private void ShowDiscussion(Entities.Intervento entityToShow)
        {
            InformazioniAccountAutenticato accountAutenticato = InformazioniAccountAutenticato.GetIstance();


            // La casella di testo di inserimento della richiesta viene visualizzata sempre...
            lrRichiesteProblematicheRiscontrate.Visible = true;
            //txtRichiesteProblematicheRiscontrate.Visible = false;
            txtRichiesteProblematicheRiscontrate.Text = String.Empty;

            bool interventoBloccato = false;
            // ...l'unico caso nel quale non viene mostrata è ad intervento chiuso.
            if (entityToShow.StatoEnum == StatoInterventoEnum.Chiuso ||
                entityToShow.StatoEnum == StatoInterventoEnum.Sostituito ||
                entityToShow.StatoEnum == StatoInterventoEnum.Validato)
            {
                lrRichiesteProblematicheRiscontrate.Visible = false;
                interventoBloccato = true;
            }



            IEnumerable<Entities.Intervento_Discussione> elencoDiscussioni = entityToShow.Intervento_Discussiones.OrderBy(x => x.DataCommento);

            int numOfDiscussions = elencoDiscussioni.Count();
            if (numOfDiscussions == 0)
            {
                lrRichiesteProblematicheRiscontrate.Visible = false;
                return;
            }
            else
            {
                Entities.Intervento_Discussione primaDiscussione = elencoDiscussioni.FirstOrDefault();
                diPrimaRichiesta.ShowDiscussione(primaDiscussione, true);
                lrPrimaRichiesta.Visible = true;

                txtRichiesteProblematicheRiscontrate.Text = String.Empty;
                elencoDiscussioni = elencoDiscussioni.Except(new Entities.Intervento_Discussione[] { primaDiscussione });
            }

            //else if (numOfDiscussions == 1)
            //{
            //    Entities.Intervento_Discussione primaDiscussione = elencoDiscussioni.FirstOrDefault();
            //    if (primaDiscussione.Account.TipologiaEnum == TipologiaAccountEnum.SeCoGes)
            //    {
            //        txtRichiesteProblematicheRiscontrate.Text = entityToShow.RichiesteProblematicheRiscontrate;
            //        txtRichiesteProblematicheRiscontrate.Visible = true;
            //        IDUltimaDiscussione = primaDiscussione.ID;
            //        repAllegati.DataSource = new Logic.Allegati().Read(new EntityId<Entities.Intervento_Discussione>(primaDiscussione.ID));
            //        repAllegati.DataBind();
            //    }
            //    else
            //    {
            //        diPrimaRichiesta.ShowDiscussione(primaDiscussione, true);
            //        lrPrimaRichiesta.Visible = true;
            //    }
            //    return;
            //}

            //else if (numOfDiscussions > 1)
            //{
            //    elencoDiscussioni = elencoDiscussioni.OrderBy(x => x.DataCommento);
            //    Entities.Intervento_Discussione primaDiscussione = elencoDiscussioni.FirstOrDefault();
            //    if (primaDiscussione != null)
            //    {
            //        elencoDiscussioni = elencoDiscussioni.Except(new Entities.Intervento_Discussione[] { primaDiscussione });
            //        diPrimaRichiesta.ShowDiscussione(primaDiscussione, true);
            //        lrPrimaRichiesta.Visible = true;
            //    }
            //}



            //if(numOfDiscussions >= 2)
            //{
            //    if (!interventoBloccato)
            //    {
            //        elencoDiscussioni = elencoDiscussioni.OrderByDescending(x => x.DataCommento);
            //        Entities.Intervento_Discussione ultimaDiscussione = elencoDiscussioni.FirstOrDefault();
            //        elencoDiscussioni = elencoDiscussioni.OrderBy(x => x.DataCommento);
            //        if (ultimaDiscussione != null)
            //        {
            //            if (ultimaDiscussione.Account.TipologiaEnum == TipologiaAccountEnum.SeCoGes)
            //            {
            //                IDUltimaDiscussione = ultimaDiscussione.ID;
            //                lblRichiesteProblematicheRiscontrate.Text = "Risposta all'utente o ulteriori richieste";
            //                txtRichiesteProblematicheRiscontrate.Text = ultimaDiscussione.Commento;
            //                txtRichiesteProblematicheRiscontrate.Visible = true;
            //                repAllegati.DataSource = new Logic.Allegati().Read(new EntityId<Entities.Intervento_Discussione>(ultimaDiscussione.ID));
            //                repAllegati.DataBind();

            //                elencoDiscussioni = elencoDiscussioni.Except(new Entities.Intervento_Discussione[] { ultimaDiscussione });
            //            }
            //        }
            //    }
            //}

            repDiscussioni.DataSource = elencoDiscussioni;
            repDiscussioni.DataBind();
        }

        protected void repAllegati_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Guid.TryParse(e.CommandArgument.ToString(), out Guid idAllegato))
            {
                Logic.Allegati llDocumenti = new Logic.Allegati();
                Entities.Allegato entityAllegato = llDocumenti.Find(idAllegato);
                if (entityAllegato != null)
                {
                    Helper.Web.DownloadAsFile(entityAllegato.NomeFile, entityAllegato.FileAllegato.ToArray());
                    //string filePath = Path.Combine(ConfigurationKeys.PERCORSO_TEMPORANEO, entityAllegato.NomeFile);
                    //File.WriteAllBytes(filePath, entityAllegato.FileAllegato.ToArray());
                    //if (File.Exists(filePath))
                    //{
                    //    Helper.Web.DownloadFile(filePath);
                    //}
                    //else
                    //{
                    //    throw new Exception(String.Format("Non è stato possibile recuperare il file '{0}'.", entityAllegato.NomeFile));
                    //}
                }
            }
        }

        /// <summary>
        ///  Memorizza i dati presenti nei controlli dell'interfaccia
        /// </summary>
        /// <param name="reloadPageAfterSave"></param>
        private void SaveData(bool reloadPageAfterSave = true)
        {
            // Valida i dati nella pagina
            MessagesCollector erroriDiValidazione = ValidaDati();
            bool dontRaiseError = false;


            InformazioniAccountAutenticato utenteLoggato = InformazioniAccountAutenticato.GetIstance();
            if (utenteLoggato == null || utenteLoggato.Account == null)
            {
                erroriDiValidazione.Add(Infrastructure.ErrorMessage.UTENTE_SCONOSCIUTO_MESSAGE);
            }

            StatoInterventoEnum statoIntervento = (StatoInterventoEnum)rcbStatoIntervento.SelectedValue.ToInteger();
            MessagesCollector erroriValidazioneOperatori = ucOperatoriIntervento.ValidaDati(statoIntervento);

            if (erroriValidazioneOperatori.HaveMessages)
            {
                erroriDiValidazione.Add(erroriValidazioneOperatori);
            }

            if (erroriDiValidazione.HaveMessages)
            {
                // Se qualcosa non va bene mostro un avviso all'utente
                MessageHelper.ShowErrorMessage(this, erroriDiValidazione.ToString("<br />"));
                return;
            }

            // Definisco una variabile per memorizzare l'entità da salvare
            Entities.Intervento entityToSave = null;

            // Definisco una variabile che conterrà l'Codice dell'entity salvata. 
            // Se per qualche motivo l'entity non viene salvata allora entityId rimarrà String.Empty
            Guid entityId = Guid.Empty;
            bool inviaEmailChiusuraBollettino = false;
            bool inviaEmailNuovaDiscussione = false;
            List<Entities.Intervento_Operatore> elencoOperatoriCreatiModificati = new List<Intervento_Operatore>();
            Logic.Interventi logicInterventi = new Logic.Interventi();

            try
            {
                logicInterventi.StartTransaction();

                //Se currentID contiene un Codice allora cerco l'entity nel database
                if (currentID.HasValue)
                {
                    entityToSave = logicInterventi.Find(currentID.Value);
                }

                // Definisco una variabile che indica se si deve compiere una azione di creazione di una nuova entity o di modifica
                bool nuovo = false;                

                if (entityToSave == null)
                {
                    if (currentID.HasValue)
                    {
                        // Se CurrentId ha un valore ma entityToSave è null allora vuol dire che la pagina è stata aperta
                        // per modificare un entità che adesso non esiste più nella base dati.
                        // In questo caso avviso l'utente
                        throw new Exception(
                            "L'Intervento che si sta variando non esiste più in archivio."
                                + "\n\rOperazione annullata!");
                    }

                    // Al contrario, se entityToSave è nulla e CurrentId è vuota
                    // vuol dire che la pagina è stata aperta per la creazione di una nuova entità
                    nuovo = true;
                    entityToSave = new Entities.Intervento();
                    entityToSave.IDAccountUtenteRiferimento = InformazioniAccountAutenticato.GetIstance().Account.ID;

                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriDallaView(entityToSave);

                    //Creo nuova entity
                    logicInterventi.Create(entityToSave, true);
                }
                else
                {
                    if (!CanCurrentUserSaveThis(entityToSave)) throw new Exception(String.Format("Non si hanno i permessi per eseguire l'operazione indicata."));

                    // Se l'Intervento è già chiuso allora salva solo le Note...
                    if (entityToSave.Chiuso.HasValue && entityToSave.Chiuso.Value)
                    {
                        entityToSave.Note = rtbNote.Text.Trim();
                    }
                    else //...altrimenti salva tutti i dati inseriti.
                    {
                        // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                        EstraiValoriDallaView(entityToSave);
                        List<Entities.Intervento_Operatore> elencoOperatoriInGriglia = ucOperatoriIntervento.SalvaDati(logicInterventi, true, entityToSave, out elencoOperatoriCreatiModificati);

                        if (elencoOperatoriInGriglia == null || elencoOperatoriInGriglia.Count <= 0)
                        {
                            throw new Exception(String.Format("Per completare il salvataggio è necessario indicare almeno un operatore"));
                        }
                    }
                }

                // Controllo dello stato dell'intervento...
                if (rcbStatoIntervento.SelectedValue != "")
                {
                    byte stato = (byte)rcbStatoIntervento.SelectedValue.ToInteger();

                    // Se lo stato dell'intervento è cambiato allora ne registra i cambiamenti
                    if (!entityToSave.Stato.HasValue || entityToSave.Stato.Value.ToString() != rcbStatoIntervento.SelectedValue)
                    {
                        if (stato == StatoInterventoEnum.Validato.GetHashCode()) //  || stato == StatoInterventoEnum.Chiuso.GetHashCode()
                        {
                            if (entityToSave.Intervento_Articolos.Count() == 0)
                            {
                                throw new Exception($"La validazione dell'intervento richiede che sia indicato almeno un articolo!");
                            }
                            //else if (entityToSave.Intervento_Articolos.Count() == 1)
                            //{
                            //    ControllaArticoliPerInserimentoTempi(entityToSave);
                            //}
                            
                            if (!entityToSave.TuttiGliArticoloConTempiIndicati)
                            {
                                throw new Exception("La validazione dell'intervento richiede che sia indicato il tempo a tutti gli articoli!");
                            }
                        }
                        //if (stato == StatoInterventoEnum.Chiuso.GetHashCode())
                        //{
                        //    ControllaArticoliPerInserimentoTempi(entityToSave);
                        //}
                        
                        Entities.Intervento_Stato nuovoStato = new Intervento_Stato();
                        nuovoStato.Intervento = entityToSave;
                        nuovoStato.Stato = stato;
                        nuovoStato.NomeUtente = Helper.Web.GetLoggedUserName();
                        nuovoStato.Data = DateTime.Now;
                        Logic.Intervento_Stati llIS = new Logic.Intervento_Stati(logicInterventi);
                        llIS.Create(nuovoStato, true);

                        if (nuovoStato.StatoEnum == StatoInterventoEnum.Chiuso) // || nuovoStato.StatoEnum == StatoInterventoEnum.Validato)
                        {
                            inviaEmailChiusuraBollettino = true;
                            ControllaArticoliPerInserimentoTempi(entityToSave);
                        }
                    }
                    else
                    {
                        if (stato == StatoInterventoEnum.Chiuso.GetHashCode())
                        {
                            ControllaArticoliPerInserimentoTempi(entityToSave);
                        }
                    }
                }
                else
                {
                    throw new Exception("Lo stato di un intervento deve essere specificato!");
                }







                Entities.Intervento_Discussione newDiscussione = new Intervento_Discussione();
                if (!string.IsNullOrEmpty(entityToSave.RichiesteProblematicheRiscontrate))
                {
                    Logic.Intervento_Discussioni llID = new Logic.Intervento_Discussioni(logicInterventi);
                    if (IDUltimaDiscussione.HasValue)
                    {
                        newDiscussione = llID.Find(new EntityId<Entities.Intervento_Discussione>(IDUltimaDiscussione.Value));
                        if (newDiscussione == null) //throw new Exception("Impossibile aggiornare l'intervento!");
                        {
                            newDiscussione = new Intervento_Discussione();
                            newDiscussione.Intervento = entityToSave;
                            newDiscussione.IDAccount = utenteLoggato.Account.ID;
                            newDiscussione.Commento = entityToSave.RichiesteProblematicheRiscontrate;
                            newDiscussione.DataCommento = DateTime.Now;
                            llID.Create(newDiscussione, true);
                            inviaEmailNuovaDiscussione = true;
                        }
                        if (newDiscussione.IDAccount != utenteLoggato.Account.ID) //throw new Exception("Impossibile aggiornare l'intervento!");
                        {
                            newDiscussione = new Intervento_Discussione();
                            newDiscussione.Intervento = entityToSave;
                            newDiscussione.IDAccount = utenteLoggato.Account.ID;
                            newDiscussione.Commento = entityToSave.RichiesteProblematicheRiscontrate;
                            newDiscussione.DataCommento = DateTime.Now;
                            llID.Create(newDiscussione, true);
                            inviaEmailNuovaDiscussione = true;
                        }

                        newDiscussione.Intervento = entityToSave;
                        newDiscussione.IDAccount = utenteLoggato.Account.ID;
                        newDiscussione.Commento = entityToSave.RichiesteProblematicheRiscontrate;
                        newDiscussione.DataCommento = DateTime.Now;
                        llID.SubmitToDatabase();
                    }
                    else
                    {
                        newDiscussione.Intervento = entityToSave;
                        newDiscussione.IDAccount = utenteLoggato.Account.ID;
                        newDiscussione.Commento = entityToSave.RichiesteProblematicheRiscontrate;
                        newDiscussione.DataCommento = DateTime.Now;
                        llID.Create(newDiscussione, true);
                        inviaEmailNuovaDiscussione = true;
                    }




                    if (RadAsyncUpload1.UploadedFiles.Count > 0)
                    {
                        Logic.Allegati logicAllegati = new Logic.Allegati(logicInterventi);
                        foreach (UploadedFile file in RadAsyncUpload1.UploadedFiles)
                        {
                            Entities.Allegato nuovoAllegato = CreaFileAllegato(file, newDiscussione);
                            logicAllegati.Create(nuovoAllegato, true);
                        }
                    }
                }





                // Persisto le modifiche sulla base dati nella transazione
                logicInterventi.SubmitToDatabase();

                // Persisto le modifiche sulla base dati effettuando il commit delle modifiche apportate nella transazione
                logicInterventi.CommitTransaction();

                SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - {1} l'entity Intervento con Oggetto '{2}'.", Request.Url.AbsolutePath, ((nuovo) ? "Creato" : "Salvato"), entityToSave.Oggetto));

                // Memorizzo l'ID dell'entità
                entityId = entityToSave.ID;






                // Se la pagina è stata aperta per la creazione di un Intervento legato ad una Attività di progetto allora lo associa
                if (Request.QueryString["AttivitaId"] != null && Guid.TryParse(Request.QueryString["AttivitaId"], out Guid idAttivita))
                {
                    Logic.Progetto_Attività llPA = new Progetto_Attività();
                    Entities.Progetto_Attivita attivita = llPA.Find(idAttivita);
                    if (attivita != null && attivita.IDTicket == null)
                    {
                        attivita.IDTicket = entityId;
                        llPA.SubmitToDatabase();
                    }
                }








                if (inviaEmailChiusuraBollettino)
                {
                    // Effettua l'invio dell'email di chiusura del bollettino
                    EmailManager.InviaEmailChiusuraIntervento(entityToSave, Helper.Web.GetLoggedUserName());

                    SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - Inviato email per Chiusura definitiva dell'Intervento numero '{1}'.", Request.Url.AbsolutePath, entityToSave.Numero));
                }

                if (elencoOperatoriCreatiModificati != null && elencoOperatoriCreatiModificati.Count > 0)
                {
                    EmailManager.InviaEmailAssegnazioneIntervento(entityToSave, elencoOperatoriCreatiModificati.Select(o => o.Operatore));
                }
                
                if (inviaEmailNuovaDiscussione)
                {
                    EmailManager.InviaEmailNuovaDiscussione(entityToSave, false);
                }


                // Alla fine, se il salvataggio è andato a buon fine (entityId != Guid.Empty)
                // allora ricarico la pagina aprendola in modifica
                if (entityId.ToString() != String.Empty && reloadPageAfterSave)
                {
                    dontRaiseError = true;
                    Helper.Web.ReloadPage(this, entityId.ToString());
                }







            }
            catch (Exception ex)
            {
                if(dontRaiseError) return;
                SeCoGes.Logging.LogManager.AddLogErrori(ex);

                logicInterventi.RollbackTransaction();

                // ...e mostro il messaggio d'errore all'utente
                MessageHelper.ShowErrorMessage(this, ex.Message);
            }
        }

        private static void ControllaArticoliPerInserimentoTempi(Entities.Intervento entityIntervento)
        {
            // Se l'intervento ha SOLO un articolo allora controlla che abbia anche dei tempi assegnati altrimenti vengono assegnati automaticamente i tempi totali degli operatori
            if(entityIntervento.Intervento_Articolos.Count() == 1)
            {
                Entities.Intervento_Articolo art = entityIntervento.Intervento_Articolos.First();
                if(art.Ore == 0)
                {
                    double totaleMinutiOperatori = 0;
                    foreach(Entities.Intervento_Operatore operatoreIntervento in entityIntervento.Intervento_Operatores)
                    {
                        totaleMinutiOperatori += operatoreIntervento.TotaleMinuti.HasValue ? operatoreIntervento.TotaleMinuti.Value : 0;
                    }

                    if(totaleMinutiOperatori > 0)
                    {
                        int ore = (int)totaleMinutiOperatori / 60;
                        int min = (int)totaleMinutiOperatori % 60;
                        art.Ore = (decimal)totaleMinutiOperatori / 60;
                        art.OreTime = String.Format("{0:00}:{1:00}:00", ore, min);
                    }
                }
            }
        }

        private Entities.Allegato CreaFileAllegato(UploadedFile fileCaricato, Entities.Intervento_Discussione discussione)
        {
            if (fileCaricato == null)
            {
                throw new ArgumentNullException("fileCaricato", "Parametro nullo.");
            }

            Entities.Account utenteCollegato = InformazioniAccountAutenticato.GetIstance().Account;
            if (utenteCollegato == null)
            {
                throw new Exception("Utente non riconosciuto!");
            }

            // Viene creata l'istanza dell'entity Documento relativa agli allegati
            Entities.Allegato entityToCreate = new Entities.Allegato();
            entityToCreate.NomeFile = Path.GetFileName(fileCaricato.FileName);
            entityToCreate.UserName = utenteCollegato.UserName;

            entityToCreate.IDLegame = discussione.ID;
            entityToCreate.TipologiaAllegatoEnum = TipologiaAllegatoEnum.Generico;
            entityToCreate.Note = String.Empty;
            entityToCreate.Compresso = false;
            entityToCreate.DataArchiviazione = DateTime.Now;

            // Viene letto il contenuto del file .. ed inserita nella property che memorizzerà il file nel database
            byte[] byteFileOriginale = new byte[fileCaricato.ContentLength];
            fileCaricato.InputStream.Read(byteFileOriginale, 0, (int)fileCaricato.ContentLength);
            entityToCreate.FileAllegato = byteFileOriginale;

            return entityToCreate;
        }


        /// <summary>
        /// Restituisce un valore booleano che indica se l'utente corrente può salvare i dati immessi nell'interfaccia per questo intervento
        /// </summary>
        /// <param name="intervento"></param>
        /// <returns></returns>
        bool CanCurrentUserSaveThis(Entities.Intervento intervento)
        {
            InformazioniAccountAutenticato infoAccount = InformazioniAccountAutenticato.GetIstance();
            if (infoAccount == null) return false;
            if (infoAccount.Account == null) return false;
            if (infoAccount.Account.Amministratore.HasValue && infoAccount.Account.Amministratore.Value) return true;

            StatoInterventoEnum statoInterventoDaInterfaccia = (StatoInterventoEnum)rcbStatoIntervento.SelectedValue.ToInteger();
            if (intervento.StatoEnum == statoInterventoDaInterfaccia) return true;

            //if(statoInterventoDaInterfaccia == StatoInterventoEnum.Validato ||
            //   statoInterventoDaInterfaccia == StatoInterventoEnum.Chiuso)
            //{
            //    return false;
            //}
            if (statoInterventoDaInterfaccia == StatoInterventoEnum.Validato)
            {
                return false;
            }


            return true;
        }
        /// <summary>
        /// Restituisce un valore booleano che indica se l'utente corrente può chiudere questo intervento
        /// </summary>
        /// <param name="intervento"></param>
        /// <returns></returns>
        bool CanCurrentUserCloseThis(Entities.Intervento intervento)
        {
            InformazioniAccountAutenticato infoAccount = InformazioniAccountAutenticato.GetIstance();
            if (infoAccount == null) return false;
            if (infoAccount.Account == null) return false;
            if (infoAccount.Account.Amministratore.HasValue && infoAccount.Account.Amministratore.Value) return true;

            return false;
        }


        /// <summary>
        /// Effettua la Chiusura definitiva dell'intervento che da questo momento in poi non potrà più essere modificato.
        /// </summary>
        private void ChiudiIntervento()
        {
            if (!currentID.HasValue) return;

            bool transactionStarted = false;
            bool salvato = false;
            Logic.Interventi ll = new Logic.Interventi();

            try
            {
                Entities.Intervento interventoDaChiudere = ll.Find(currentID.Value);
                if (interventoDaChiudere != null)
                {
                    if (!interventoDaChiudere.DataPrevistaIntervento.HasValue)
                    {
                        MessageHelper.ShowMessage(this.Page, "Chisura Intervento", "La Chiusura del bollettino prevede che sia stata specificata la Data dell'intervento!");
                        return;
                    }

                    if (!CanCurrentUserCloseThis(interventoDaChiudere)) throw new Exception(String.Format("Non si hanno i permessi per Chiudere questo intervento."));


                    ll.StartTransaction();
                    transactionStarted = true;

                    interventoDaChiudere.Chiuso = true;
                    ll.SubmitToDatabase();

                    if (ll.InviaInterventoAContabilita(interventoDaChiudere.ID, out string message))
                    {
                        ll.CommitTransaction();
                    }
                    else
                    {
                        throw new Exception(message);
                    }

                    //ll.CreaDocXMLInterventi(interventoDaChiudere.ID);
                    //ll.CommitTransaction();

                    transactionStarted = false;

                    salvato = true;
                    SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - Chiusura definitiva dell'Intervento numero '{1}'.", Request.Url.AbsolutePath, interventoDaChiudere.Numero));
                }
            }
            catch (Exception ex)
            {
                if (transactionStarted) ll.RollbackTransaction();

                SeCoGes.Logging.LogManager.AddLogErrori(ex);

                // ...e mostro il messaggio d'errore all'utente
                MessageHelper.ShowErrorMessage(this, ex.Message);
            }
            finally
            {
                if (salvato)
                    Helper.Web.ReloadPage(this, currentID.Value.ToString());
            }
        }

        private void SubstituteTicket(bool reloadPageAfterSave = true)
        {
            try
            {
                SaveData(false);
                Entities.Intervento interventoSostitutivo = ChiudiSostituisciIntervento();
                if (interventoSostitutivo != null)
                {
                    this.Response.Redirect(string.Format("{0}?{1}={2}", this.Request.Path, "ID", interventoSostitutivo.ID.ToString()));
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex.Message);
            }
        }

        /// <summary>
        /// Effettua la Chiusura definitiva dell'intervento che da questo momento in poi non potrà più essere modificato.
        /// </summary>
        private Entities.Intervento ChiudiSostituisciIntervento()
        {
            if (!currentID.HasValue) return null;

            bool transactionStarted = false;
            Logic.Interventi ll = new Logic.Interventi();
            Entities.Intervento interventoSostitutivo = null;

            try
            {
                Entities.Intervento interventoDaChiudere = ll.Find(currentID.Value);
                if (interventoDaChiudere != null)
                {
                    // Controlla che tutti i dati degli operatori siano stati indicati correttamente
                    //if(interventoDaChiudere.Intervento_Operatores.Any(x => 
                    //    !x.IDModalitaRisoluzione.HasValue ||
                    //    !x.DataPresaInCarico.HasValue ||
                    //    !x.FineIntervento.HasValue))
                    //{
                    //    throw new Exception("Specificare per ogni Operatore la Modalità di risoluzione, l'Inizio e la Fine dell'Intervento.");
                    //}

                    //if (!interventoDaChiudere.DataPrevistaIntervento.HasValue)
                    //{
                    //    MessageHelper.ShowMessage(this.Page, "Chisura Intervento", "La Chiusura del bollettino prevede che sia stata specificata la Data dell'intervento!");
                    //    return null;
                    //}

                    ll.StartTransaction();
                    transactionStarted = true;


                    interventoSostitutivo = new Entities.Intervento();
                    interventoSostitutivo.Numero = ll.GetNuovoNumeroIntervento();
                    interventoSostitutivo.DataRedazione = DateTime.Now;
                    interventoSostitutivo.IDAccountUtenteRiferimento = interventoDaChiudere.IDAccountUtenteRiferimento; // InformazioniAccountAutenticato.GetIstance().Account.ID;
                    interventoSostitutivo.CodiceCliente = interventoDaChiudere.CodiceCliente;
                    interventoSostitutivo.RagioneSociale = interventoDaChiudere.RagioneSociale;
                    interventoSostitutivo.IdDestinazione = interventoDaChiudere.IdDestinazione;
                    interventoSostitutivo.DestinazioneMerce = interventoDaChiudere.DestinazioneMerce;
                    interventoSostitutivo.Indirizzo = interventoDaChiudere.Indirizzo;
                    interventoSostitutivo.CAP = interventoDaChiudere.CAP;
                    interventoSostitutivo.Localita = interventoDaChiudere.Localita;
                    interventoSostitutivo.Provincia = interventoDaChiudere.Provincia;
                    interventoSostitutivo.Telefono = interventoDaChiudere.Telefono;
                    interventoSostitutivo.Oggetto = string.Empty;
                    interventoSostitutivo.Interno = false;
                    interventoSostitutivo.VisibileAlCliente = true;
                    ll.Create(interventoSostitutivo, true);

                    //AggiungiElencoOperatoriImpostatiNelWebConfig(ll, interventoSostitutivo, true);
                    Logic.Intervento_Operatori llIO = new Logic.Intervento_Operatori();
                    foreach (Entities.Intervento_Operatore operatoriIntervento in interventoDaChiudere.Intervento_Operatores)
                    {
                        Entities.Intervento_Operatore operatoriInterventoNuovo = new Intervento_Operatore();
                        operatoriInterventoNuovo.ID = Guid.NewGuid();
                        operatoriInterventoNuovo.IDOperatore = operatoriIntervento.IDOperatore;
                        operatoriInterventoNuovo.IDModalitaRisoluzione = operatoriIntervento.IDModalitaRisoluzione;
                        operatoriInterventoNuovo.InizioIntervento = operatoriIntervento.InizioIntervento;
                        operatoriInterventoNuovo.FineIntervento = operatoriIntervento.FineIntervento;
                        operatoriInterventoNuovo.DurataMinuti = operatoriIntervento.DurataMinuti;
                        operatoriInterventoNuovo.PausaMinuti = operatoriIntervento.PausaMinuti;
                        operatoriInterventoNuovo.PresaInCarico = operatoriIntervento.PresaInCarico;
                        operatoriInterventoNuovo.DataPresaInCarico = operatoriIntervento.DataPresaInCarico;
                        operatoriInterventoNuovo.Note = operatoriIntervento.Note;
                        interventoSostitutivo.Intervento_Operatores.Add(operatoriInterventoNuovo);
                        llIO.Create(operatoriInterventoNuovo, true);
                    }

                    Entities.Intervento_Stato nuovoStato = new Intervento_Stato();
                    nuovoStato.Intervento = interventoSostitutivo;
                    nuovoStato.StatoEnum = Entities.StatoInterventoEnum.Aperto;
                    nuovoStato.NomeUtente = Helper.Web.GetLoggedUserName();
                    nuovoStato.Data = DateTime.Now;
                    Logic.Intervento_Stati llIS = new Logic.Intervento_Stati(ll);
                    llIS.Create(nuovoStato, true);



                    interventoDaChiudere.IDInterventoSostitutivo = interventoSostitutivo.ID;
                    interventoDaChiudere.Chiuso = true;

                    Entities.Intervento_Stato nuovoStatoInterventoDaChiudere = new Intervento_Stato();
                    nuovoStatoInterventoDaChiudere.Intervento = interventoDaChiudere;
                    nuovoStatoInterventoDaChiudere.StatoEnum = Entities.StatoInterventoEnum.Sostituito;
                    nuovoStatoInterventoDaChiudere.NomeUtente = Helper.Web.GetLoggedUserName();
                    nuovoStatoInterventoDaChiudere.Data = DateTime.Now;
                    llIS.Create(nuovoStatoInterventoDaChiudere, true);

                    ll.SubmitToDatabase();

                    if (ll.InviaInterventoAContabilita(interventoDaChiudere.ID, out string message))
                    {
                        ll.CommitTransaction();
                    }
                    else
                    {
                        throw new Exception(message);
                    }
                    //ll.CreaDocXMLInterventi(interventoDaChiudere.ID);
                    //ll.CommitTransaction();

                    transactionStarted = false;

                    SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - Chiusura definitiva dell'Intervento numero '{1}'.", Request.Url.AbsolutePath, interventoDaChiudere.Numero));
                    SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - Creazione Intervento '{1}' sostitutivo di '{2}'.", Request.Url.AbsolutePath, interventoSostitutivo.Numero, interventoDaChiudere.Numero));
                }
            }
            catch (Exception ex)
            {
                if (transactionStarted) ll.RollbackTransaction();
                interventoSostitutivo = null;

                SeCoGes.Logging.LogManager.AddLogErrori(ex);

                // ...e mostro il messaggio d'errore all'utente
                MessageHelper.ShowErrorMessage(this, ex.Message);
            }

            return interventoSostitutivo;
        }

        /// <summary>
        /// Effettua la generazione del documento relativo all'intervento corrente
        /// </summary>
        private void GeneraDocumento()
        {
            // Nel caso in cui l'identificativo dell'intervento non sia valorizzato oppure sia relativo ad un intervento nuovo ..
            if (currentID == null || currentID.Value == EntityId<Intervento>.Empty.Value) throw new Exception("Non è possibile effettuare la generazione di documento alla creazione di un intervento");

            // Viene effettuato il salvataggio dei dati senza effettuare il reload della pagina
            SaveData(false);
            
            // Viene generato il documento relativo all'intervento corrente ..
            string percorsoFileGenerato = GestoreDocumenti.GeneraIntervento(currentID);

            if (GestoreDocumenti.FileGeneratoEsiste(percorsoFileGenerato, false))
            {
                // Viene mandato in download il file
                Helper.Web.DownloadFile(percorsoFileGenerato);
            }
        }

        private void RiapriIntervento()
        {
            // Nel caso in cui l'identificativo dell'intervento non sia valorizzato oppure sia relativo ad un intervento nuovo ..
            if (currentID == null || currentID.Value == EntityId<Intervento>.Empty.Value) return;

            // Questa funzionalità è abilitata solamente per gli utenti amministratori
            InformazioniAccountAutenticato infoAccount = InformazioniAccountAutenticato.GetIstance();
            Entities.Account utenteCollegato = infoAccount.Account;
            if (utenteCollegato.Amministratore.HasValue && utenteCollegato.Amministratore.Value)
            {
                // Viene effettuato il salvataggio dei dati senza effettuare il reload della pagina
                if (currentID.HasValue)
                {
                    Logic.Interventi ll = new Logic.Interventi();
                    Entities.Intervento entityToSave = ll.Find(currentID.Value);
                    if (entityToSave != null)
                    {
                        entityToSave.Chiuso = false;
                        ll.SubmitToDatabase();

                        SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - Riapertura Intervento numero '{1}'.", Request.Url.AbsolutePath, entityToSave.Numero));

                        Helper.Web.ReloadPage(this, entityToSave.ID.ToString());
                    }
                }
            }
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Effettua la gestione della visualizzazione del tasto che effettua la generazione del documento relativo all'intervento corrente
        /// </summary>
        private void GestisciVisualizzazioneTastoGenerazioneDocumento()
        {
            // La visualizzazione del tasto di generazione avviene solo nel caso in cui l'intervento corrente non sia nuovo
            if (currentID != null && currentID.Value != EntityId<Intervento>.Empty.Value)
            {
                // Il tasto di generazione ed il relativo separatore, vengono mostrati solo se esiste una loro istanza
                if (PulsanteToolbar_SeparatoreGeneraDocumento != null) PulsanteToolbar_SeparatoreGeneraDocumento.Visible = true;
                if (PulsanteToolbar_GeneraDocumento != null) PulsanteToolbar_GeneraDocumento.Visible = true;
            }
        }

        /// <summary>
        /// Effettua la gestione della visualizzazione del tasto di riapertura dell'intervento corrente
        /// </summary>
        private void GestisciVisualizzazioneTastoRiapriIntervento(Entities.Intervento entityToShow)
        {
            PulsanteToolbar_SeparatoreRiapriIntervento.Visible = false;
            PulsanteToolbar_RiapriIntervento.Visible = false;

            // La visualizzazione del tasto Riapri Intervento è valida SOLO per gli Amministratori
            // ed avviene solo nel caso in cui l'intervento corrente non sia nuovo
            if (entityToShow != null && entityToShow.Chiuso.HasValue && entityToShow.Chiuso.Value)
            {
                InformazioniAccountAutenticato infoAccount = InformazioniAccountAutenticato.GetIstance();
                Entities.Account utenteCollegato = infoAccount.Account;

                if (utenteCollegato.Amministratore.HasValue && utenteCollegato.Amministratore.Value)
                {
                    // Il tasto di generazione ed il relativo separatore, vengono mostrati solo se esiste una loro istanza
                    if (PulsanteToolbar_SeparatoreRiapriIntervento != null) PulsanteToolbar_SeparatoreRiapriIntervento.Visible = true;
                    if (PulsanteToolbar_RiapriIntervento != null) PulsanteToolbar_RiapriIntervento.Visible = true;
                }
            }
        }


        /// <summary>
        /// Abilita o disabilita i campi dell'interfaccia in base al parametro passato
        /// </summary>
        /// <param name="abilitaCampi"></param>
        private void AbilitaCampi(bool enabled)
        {
            //PulsanteToolbar_SeparatoreSalva.Visible = enabled;
            //PulsanteToolbar_Salva.Visible = enabled;

            rntbNumeroIntervento.ReadOnly = !enabled;
            rdtpDataRedazione.Enabled = enabled;
            //rdtpDataPrevistaIntervento.Enabled = enabled;
            rcbCliente.Enabled = enabled;
            rcbCliente.Enabled = enabled;
            rcbIndirizzi.Enabled = enabled;
            rtbIndirizzo.ReadOnly = !enabled;
            rtbCAP.ReadOnly = !enabled;
            rtbLocalita.ReadOnly = !enabled;
            rtbProvincia.ReadOnly = !enabled;
            rtbTelefono.ReadOnly = !enabled;
            rtbOggetto.ReadOnly = !enabled;
            rtbDefinizione.ReadOnly = !enabled;
            rtbNumeroCommessa.ReadOnly = !enabled;
            rcbStatoIntervento.Enabled = enabled;
            rcbTipologiaIntervento.Enabled = enabled;
            chkChiamata.Enabled = enabled;
            rtbReferenteChiamata.ReadOnly = !enabled;
            chkUrgente.Enabled = enabled;
            chkInterventoInterno.Enabled = enabled;
            chkVisibileAlCliente.Enabled = enabled;
            //reRichiesteProblematicheRiscontrate.Enabled = !enabled;
            txtRichiesteProblematicheRiscontrate.Enabled = !enabled;
            //rtbNote.Enabled = enabled;
            rdtpDataNotifica.Enabled = enabled;
            rtbTestoNotifica.ReadOnly = !enabled;
            rpbVociPredefiniteIntervento.Visible = enabled;
            ucArticoliIntervento.Enabled = enabled;
            ucOperatoriIntervento.Enabled = enabled;
            tdAggiungiOperatori.Visible= enabled;
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
        /// Legge i dati inseriti nell'interfaccia e li inserisce nell'entity passata
        /// </summary>
        /// <param name="entityToFill"></param>
        public void EstraiValoriDallaView(Entities.Intervento entityToFill)
        {
            if (entityToFill == null)
            {
                throw new ArgumentNullException("Parametro nullo", "entityToFill");
            }

            //if (rcbAnnoAccademicoAttivo.SelectedValue != "")
            //{
            //    entityToFill.AnnoAccademicoAttivo = rcbAnnoAccademicoAttivo.SelectedValue;
            //}

            entityToFill.Numero = (int)rntbNumeroIntervento.Value;
            entityToFill.DataRedazione = rdtpDataRedazione.SelectedDate.Value;
            entityToFill.DataPrevistaIntervento = rdtpDataPrevistaIntervento.SelectedDate;
            entityToFill.CodiceCliente = rcbCliente.SelectedValue;
            entityToFill.RagioneSociale = rcbCliente.Text.Trim();
            entityToFill.IdDestinazione = rcbIndirizzi.SelectedValue;
            entityToFill.DestinazioneMerce = rcbIndirizzi.Text.Trim();
            entityToFill.Indirizzo = rtbIndirizzo.Text.Trim();
            entityToFill.CAP = rtbCAP.Text.Trim();
            entityToFill.Localita = rtbLocalita.Text.Trim();
            entityToFill.Provincia = rtbProvincia.Text.Trim();
            entityToFill.Telefono = rtbTelefono.Text.Trim();
            entityToFill.Oggetto = rtbOggetto.Text.Trim();
            entityToFill.Definizione = rtbDefinizione.Text.Trim();
            entityToFill.NumeroCommessa = rtbNumeroCommessa.Text.Trim();
            entityToFill.Note = rtbNote.Text.Trim();
            entityToFill.IdTipologia = (!String.IsNullOrEmpty(rcbTipologiaIntervento.SelectedValue) ? Guid.Parse(rcbTipologiaIntervento.SelectedValue) : (Guid?)null);
            entityToFill.Chiamata = (chkChiamata.Checked ? true : (bool?)null);
            entityToFill.ReferenteChiamata = rtbReferenteChiamata.Text.Trim();
            entityToFill.Urgente = (chkUrgente.Checked ? true : (bool?)null);
            entityToFill.Interno = (chkInterventoInterno.Checked ? true : (bool?)null);
            entityToFill.VisibileAlCliente = chkVisibileAlCliente.Checked;

            if(rdtpDataNotifica.SelectedDate != entityToFill.DataNotifica || rtbTestoNotifica.Text.Trim() != entityToFill.TestoNotifica) entityToFill.Notificata = false;
            entityToFill.DataNotifica = rdtpDataNotifica.SelectedDate;
            entityToFill.TestoNotifica = rtbTestoNotifica.Text.Trim();


            //entityToFill.RichiesteProblematicheRiscontrate = Helper.RadEditorHelper.ParseHtmlToImageSource(reRichiesteProblematicheRiscontrate.Content);
            entityToFill.RichiesteProblematicheRiscontrate = txtRichiesteProblematicheRiscontrate.Text;

            entityToFill.MotivazioneSostituzione = txtMotivazioneSostituzione.Text.Trim();


            if (rcbTipologiaInterventoTicket.SelectedValue != string.Empty)
            {
                entityToFill.IDConfigurazioneTipologiaTicket = new Guid(rcbTipologiaInterventoTicket.SelectedValue);
            }
            else
            {
                entityToFill.IDConfigurazioneTipologiaTicket = null;
            }
        }

        /// <summary>
        /// Restituisce un oggetto contenente gli eventuali errori di validazione dei dati
        /// </summary>
        /// <returns></returns>
        private MessagesCollector ValidaDati()
        {
            MessagesCollector messaggi = new MessagesCollector();

            // Errori dei validatori client
            //Page.Validate();
            //if (!Page.IsValid)
            //{
            //    foreach (IValidator validatore in Page.Validators)
            //    {
                    
            //        if (!validatore.IsValid)
            //        {
            //            messaggi.Add(validatore.ErrorMessage);
            //        }
            //    }
            //    if (messaggi.HaveMessages) return messaggi;
            //}

            //if (!rntbNumeroIntervento.Value.HasValue)
            //{
            //    messaggi.Add(rfvNumeroIntervento.ErrorMessage);
            //}

            return messaggi;
        }

        /// <summary>
        /// Effettua il popolamento del combo relativo agli stati dell'intervento
        /// </summary>
        private void FillComboStatoIntervento()
        {
            IDictionary<byte, string> elencoStatoIntervento = SeCoGes.Utilities.EnumHelper.GetDescriptionFromEnum<Entities.StatoInterventoEnum, byte>();
            rcbStatoIntervento.DataSource = elencoStatoIntervento;
            rcbStatoIntervento.DataBind();
        }

        /// <summary>
        /// Effettua il popolamento del combo relativo alla tipologie di intervento
        /// </summary>
        private void FillComboTipologiaIntervento()
        {
            //IDictionary<byte, string> elencoTipologiaIntervento = SeCoGes.Utilities.EnumHelper.GetDescriptionFromEnum<Entities.TipologiaInterventoEnum, byte>();
            rcbTipologiaIntervento.DataSource = new Logic.TipologieIntervento().Read();
            rcbTipologiaIntervento.DataBind();

            rcbTipologiaIntervento.Items.Insert(0, new RadComboBoxItem(String.Empty, String.Empty));
        }

        /// <summary>
        /// Gestisce la visualizzazione dell'editor della textbox in cui il cliente ha inserito le proprie richieste/ problematiche
        /// </summary>
        private void GestisciVisualizzazioneRadEditorRichiesteProblematicheRiscontrate(Entities.Intervento entityToShow)
        {
            if(entityToShow.ID == Guid.Empty)
            {
                // Se l'intervento è nuovo allora mostra sempre la casella di inserimento del testo dell'intervento...
                lrRichiesteProblematicheRiscontrate.Visible = true;
            }
            else
            {
                // ...altrimenti la mostra solamente se l'utente che sta visualizzando l'intervento è lo stesso di quello che ha scritto l'ultima discussione.
                // In questo caso la casella di inserimento del testo dell'intervento mostra l'ultima discussione in edit per l'utente
                InformazioniAccountAutenticato accountAutenticato = InformazioniAccountAutenticato.GetIstance();

                //if (currentID != null && currentID.HasValue)
                //{
                    Logic.Interventi llInterventi = new Logic.Interventi();
                    Entities.Account accountCreazione = llInterventi.GetAccountCreazione(currentID);

                    if (accountCreazione != null && (accountCreazione.TipologiaEnum == TipologiaAccountEnum.ClienteStandard || accountCreazione.TipologiaEnum == Entities.TipologiaAccountEnum.ClienteAdmin || accountCreazione.TipologiaEnum == Entities.TipologiaAccountEnum.ClienteSupervisore))
                    {
                        lrRichiesteProblematicheRiscontrate.Visible = true;
                    }
                //}
                //else
                //{
                //    lrRichiesteProblematicheRiscontrate.Visible = true;
                //}

            }
        }

        #endregion

        #region Gestione delle autorizzazioni

        private Entities.Account utenteCollegato;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneAziende;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneDisattivazioneAzienda;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneAnniAccademici;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestionePeriodiAccademiciPerAnnoAccademico;

        /// <summary>
        /// Carica gli oggetti contenenti le informazioni di accesso ai dati ed alle funzionalità esposte dalla pagina
        /// </summary>
        private void CaricaAutorizzazioni()
        {
            try
            {
                InformazioniAccountAutenticato infoAccount = InformazioniAccountAutenticato.GetIstance();
                utenteCollegato = infoAccount.Account;

                //Autorizzazioni_GestioneAziende = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneAziende);
                //Autorizzazioni_GestioneDisattivazioneAzienda = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneDisattivazioneAzienda);
                //Autorizzazioni_GestioneAnniAccademici = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneAnniAccademici);
                //Autorizzazioni_GestionePeriodiAccademiciPerAnnoAccademico = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestionePeriodiAccademiciPerAnnoAccademico);
            }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        /// <summary>
        /// Blocca o permette l'accesso ai dati ed applica agli oggetti dell'interfaccia lo stile in base alle regole di accesso dell'azienda corrente
        /// </summary>
        private void ApplicaAutorizzazioni()
        {
            //if (Autorizzazioni_GestioneAziende == null) throw new ArgumentNullException("Autorizzazioni_GestioneAziende");
            //if (Autorizzazioni_GestioneDisattivazioneAzienda == null) throw new ArgumentNullException("Autorizzazioni_GestioneDisattivazioneAzienda");


            //// Applicazione regole di accesso di VISIBILITA' 
            //if (!Autorizzazioni_GestioneAziende.Consenti_Visibilità)
            //{
            //    MessageHelper.RedirectToErrorPageWithMessage(ErrorMessage.ACCESSO_NEGATO_MESSAGE);
            //    return;
            //}
            //else if (!currentID.HasValue && !Autorizzazioni_GestioneAziende.Consenti_Inserimento) // Applicazione regole di accesso di INSERIMENTO
            //{
            //    MessageHelper.RedirectToErrorPageWithMessage(ErrorMessage.UTENTE_OPERAZIONE_NON_CONSENTITA_MESSAGE);
            //    return;
            //}

            //// Applicazione regole di accesso di INSERIMENTO
            //if (!Autorizzazioni_GestioneAziende.Consenti_Inserimento)
            //{
            //    PulsanteToolbar_Nuovo.Enabled = false;
            //}

            //// Applicazione regole accesso in MODIFICA ed accesso a FUNZIONALITA' di disattivazione dell'azienda
            //if (!Autorizzazioni_GestioneDisattivazioneAzienda.Consenti_Funzionalità)
            //{
            //    chkDisattivato.Enabled = false;
            //}

            //// Applicazione regole accesso in MODIFICA ed accesso a FUNZIONALITA'
            //if (currentID.HasValue && !Autorizzazioni_GestioneAziende.Consenti_Modifica)
            //{
            //    PulsanteToolbar_Salva.Enabled = false;
            //    this.Enabled = false;
            //}

            //// Vengono gestiti i permessi relativi alla gestione degli anni accademici
            //GestisciPermessiAnniAccademici();

            //// Applicazione regole accesso in SOLA LETTURA
            //if (utenteCollegato.SolaLettura.HasValue && utenteCollegato.SolaLettura.Value)
            //{
            //    this.Enabled = false;
            //}
        }

        #endregion





        /// <summary>
        /// Metodo di gestione dell'evento OnNeedDataSource dell'usercontrol CorsiFrequentatiList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ucDocumentazioneAllegata_NeedDataSource(object sender, EventArgs e)
        {
            UI.DocumentazioneAllegata ucDocumentazioneAllegata = (UI.DocumentazioneAllegata)sender;

            Logic.Allegati llAllegati = new Logic.Allegati();
            ucDocumentazioneAllegata.DataSource = llAllegati.Read(currentID);
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
                e.DocumentoAllegato.IDLegame = currentID.Value;
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

        /// <summary>
        /// Metodo di gestione dell'evento AjaxRequest relativo al pannello ajax della tab Allegati
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rapAllegati_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (!currentID.HasValue)
            {
                //ucpmAllegati.Visible = true;
                ucDocumentazioneAllegata.IsReadOnly = true;
            }

            //divAllegati.Visible = true;
            ucDocumentazioneAllegata.Inizializza();
            //hfGrigliaAllegatiCaricata.Value = VALORE_TAB_CARICATA;
        }




        protected void rcbTipologiaInterventoTicket_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            string testoRicerca = (e.Text == null) ? String.Empty : e.Text.ToLower();
            RadComboBox combo = (RadComboBox)sender;
            combo.Items.Clear();

            IEnumerable<Entities.ConfigurazioneTipologiaTicketCliente> queryBase;
            string clienteSelezionato = string.Empty;
            if (e.Context.ContainsKey("ClienteSelezionato"))
            {
                clienteSelezionato = e.Context["ClienteSelezionato"].ToString();
            }

            if (clienteSelezionato.Trim() != string.Empty)
            {
                queryBase = new Logic.ConfigurazioniTipologieTicketCliente().ReadAttivePerCliente(clienteSelezionato.Trim(), rdtpDataRedazione.SelectedDate, false);
            }
            else
            {
                queryBase = new List<Entities.ConfigurazioneTipologiaTicketCliente>();
            }


            foreach (Entities.ConfigurazioneTipologiaTicketCliente entity in queryBase)
            {
                RadComboBoxItem item = new RadComboBoxItem(entity.NomeEstesoTipologiaIntervento, entity.Id.ToString());
                if(entity.CondizioneIntervento != null &&
                   entity.CondizioneIntervento.CondizioneInterventoEnumValue == CondizioneInterventoEnum.UrgenteBloccante)
                {
                    item.Attributes.Add("Urgente", "1");
                }
                else
                {
                    item.Attributes.Add("Urgente", "0");
                }

                item.DataItem = entity;
                combo.Items.Add(item);
            }
        }

        protected void rcbTipologiaInterventoTicket_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Guid idConfigurazioneAttuale = Guid.Empty;
            string idConfigurazioneIntervento = e.Value;

            ShowConfigurazioneTipologiaIntervento(idConfigurazioneIntervento);



            // Imposta le informazioni sulla configurazione della tipologia intervento
            if (rcbTipologiaIntervento.Visible)
            {
                var config = new Logic.ConfigurazioniTipologieTicketCliente().Find(new EntityId<ConfigurazioneTipologiaTicketCliente>(idConfigurazioneIntervento));
                if (config != null)
                {
                    var tipologia = new Logic.TipologieIntervento().Find(new EntityId<TipologiaIntervento>(config.IdTipologia));
                    if (tipologia != null)
                    {
                        var item = rcbTipologiaIntervento.FindItemByValue(config.IdTipologia.ToString());
                        if(item != null) item.Selected = true;
                    }

                }
            }
        }

        void ShowConfigurazioneTipologiaIntervento(string idConfigurazioneIntervento)
        {
            lblUfficioOrariReparto.Text = string.Empty;
            lblCaratteristicheTipologiaIntervento.Text = string.Empty;
            lblDataLimite.Text = string.Empty;
            tableTicketConfigurationInfo.Visible = false;
            if (idConfigurazioneIntervento != string.Empty)
            {
                tableTicketConfigurationInfo.Visible = true;

                Logic.ConfigurazioniTipologieTicketCliente llCTTC = new Logic.ConfigurazioniTipologieTicketCliente();
                Entities.ConfigurazioneTipologiaTicketCliente configurazioneSelezionata = llCTTC.Find(new EntityId<ConfigurazioneTipologiaTicketCliente>(idConfigurazioneIntervento));

                if (configurazioneSelezionata != null)
                {
                    int oreNotifica = 1;
                    if (configurazioneSelezionata != null &&
                        configurazioneSelezionata.TipologiaIntervento != null &&
                        configurazioneSelezionata.TipologiaIntervento.OreNotifica.HasValue &&
                        configurazioneSelezionata.TipologiaIntervento.OreNotifica.Value > 0)
                    {
                        oreNotifica = configurazioneSelezionata.TipologiaIntervento.OreNotifica.Value;
                    }
                    if(oreNotifica == 1)
                    {
                        lblTempoNotifica.Text = "(la notifica via email verrà inviata <strong>1 ora prima</strong>)";
                    }
                    else
                    {
                        lblTempoNotifica.Text = $"(la notifica via email verrà inviata <strong>{oreNotifica} ore prima</strong>)";
                    }



                    // Imposta l'eventuale flag di urgenza in base alla configurazione selezionata
                    if (configurazioneSelezionata.CondizioneIntervento != null && 
                        configurazioneSelezionata.CondizioneIntervento.CondizioneInterventoEnumValue == CondizioneInterventoEnum.UrgenteBloccante)
                    {
                        chkUrgente.Checked = true;
                    }
                    //else
                    //{
                    //    chkUrgente.Checked = false;
                    //}

                    // Informazioni sugli orari del Reparto di assistenza
                    Logic.RepartiUfficio llRU = new Logic.RepartiUfficio(llCTTC);
                    Entities.RepartoUfficio repartoUfficio = llRU.Find(new Entities.EntityId<RepartoUfficio>(configurazioneSelezionata.IdRepartoUfficio));
                    if (repartoUfficio != null)
                    {
                        lblUfficioOrariReparto.Text = $"<strong>Orari apertura {repartoUfficio.Reparto}</strong><br />";
                        foreach (Entities.OrarioRepartoUfficio orario in repartoUfficio.OrarioRepartoUfficios.OrderBy(o => o.Giorno).ThenBy(o => o.OrarioDalle))
                        {
                            string nomeGiorno = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.DayNames[orario.Giorno];
                            string text = $"{nomeGiorno} dalle {orario.OrarioDalle} alle {orario.OrarioAlle}<br />";
                            lblUfficioOrariReparto.Text += text;
                        }
                    }


                    // Informazioni sugli SLA dell'intervento
                    Logic.CaratteristicheTipologieIntervento llCTI = new Logic.CaratteristicheTipologieIntervento(llCTTC);
                    IEnumerable<Entities.CaratteristicaTipologiaIntervento> caratteristicheIntervento = llCTI.Read(configurazioneSelezionata.Id);
                    lblCaratteristicheTipologiaIntervento.Text = $"<strong>SLA Intervento</strong><br />";

                    TimeSpan tempoPiùRestrittivo = new TimeSpan(0);

                    foreach (Entities.CaratteristicaTipologiaIntervento archCar in caratteristicheIntervento)
                    {
                        if (Enum.IsDefined(typeof(Entities.CaratteristicaInterventoEnum), archCar.IdCaratteristica))
                        {
                            Entities.CaratteristicaInterventoEnum ciEnum = (Entities.CaratteristicaInterventoEnum)archCar.IdCaratteristica;
                            switch (ciEnum)
                            {
                                case CaratteristicaInterventoEnum.PresaInCaricoEntroMinuti:
                                case CaratteristicaInterventoEnum.RispostaEntroMinuti:
                                case CaratteristicaInterventoEnum.RipristinoEntroMinuti:
                                case CaratteristicaInterventoEnum.RipristinoEntroMinutiDaPresaInCarico:
                                    string tempo = string.Empty;
                                    if (!string.IsNullOrWhiteSpace(archCar.Parametri))
                                    {
                                        long giorni = 0;
                                        long ore = 0;
                                        long minuti = 0;
                                        if (long.TryParse(archCar.Parametri, out minuti))
                                        {
                                            if (minuti >= 1440)
                                            {
                                                giorni = minuti / 1440;
                                                minuti = minuti % 1440;
                                                //ore = minuti % 1440;
                                                //minuti = minuti % 60;
                                            }
                                            if (minuti >= 60)
                                            {
                                                ore = minuti / 60;
                                                minuti = minuti % 60;
                                            }

                                            TimeSpan ts = new TimeSpan((int)giorni, (int)ore, (int)minuti, 0, 0);
                                            tempo = string.Format("{0:%d} giorni {0:%h} ore {0:%m} minuti", ts);// ts.ToString("{0:%d}gg {0:%h}ore {0:%m}min");
                                            if (tempoPiùRestrittivo.Ticks == 0)
                                            {
                                                tempoPiùRestrittivo = ts;
                                            }
                                            else
                                            {
                                                if (ts < tempoPiùRestrittivo) tempoPiùRestrittivo = ts;
                                            }
                                        }

                                        //if (long.TryParse(archCar.Parametri, out long par))
                                        //{
                                        //    TimeSpan ts = new TimeSpan(par);
                                        //    tempo = ts.ToString();

                                        //    if (tempoPiùRestrittivo.Ticks == 0)
                                        //    {
                                        //        tempoPiùRestrittivo = ts;
                                        //    }
                                        //    else
                                        //    {
                                        //        if (ts < tempoPiùRestrittivo) tempoPiùRestrittivo = ts;
                                        //    }
                                        //}
                                    }
                                    lblCaratteristicheTipologiaIntervento.Text += $"{archCar.NomeCaratteristicaIntervento} - Tempo: {tempo}<br />";
                                    break;
                                default:
                                    lblCaratteristicheTipologiaIntervento.Text += $"{archCar.NomeCaratteristicaIntervento}<br />";
                                    break;
                            }
                        }
                        else
                        {
                            lblCaratteristicheTipologiaIntervento.Text += $"{archCar.NomeCaratteristicaIntervento}<br />";
                        }
                    }

                    if (repartoUfficio != null)
                    {
                        DateTime dataLimite = llRU.GetDataLimiteIntervento(repartoUfficio, tempoPiùRestrittivo, rdtpDataRedazione.SelectedDate);
                        lblDataLimite.Text = dataLimite.ToString("dddd dd MMMM yyyy alle HH:mm");
                    }
                    else
                    {
                        lblDataLimite.Text = tempoPiùRestrittivo.Ticks > 0 ? DateTime.Now.Add(tempoPiùRestrittivo).ToString("dddd dd MMMM yyyy alle HH:mm") : "";
                    }
                }
            }
        }

        protected void btAddOperatore_Click(object sender, EventArgs e)
        {
            Logic.Interventi ll = new Logic.Interventi();
            Entities.Intervento entityToShow = ll.Find(currentID.Value);

            if (entityToShow != null)
            {
                int numOperatoriDaAggiungere = 1;
                if (rntbNumOperatoriDaAggiungere.Value.HasValue)
                {
                    numOperatoriDaAggiungere = (int)rntbNumOperatoriDaAggiungere.Value;
                }

                ucOperatoriIntervento.IDIntervento = new EntityId<Entities.Intervento>(entityToShow.ID);
                ucOperatoriIntervento.AggiungiRighe(numOperatoriDaAggiungere);
            }
        }

        protected void repDiscussioni_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null && e.Item.DataItem is Entities.Intervento_Discussione)
            {
                Entities.Intervento_Discussione disc = (Entities.Intervento_Discussione)e.Item.DataItem;
                DiscussioneIntervento uc = (DiscussioneIntervento)e.Item.FindControl("diDiscussione");
                if (uc != null)
                {
                    uc.ShowDiscussione(disc);
                }
            }
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            txtMotivazioneSostituzione.Text = string.Empty;
            SaveData();
        }
    }
}