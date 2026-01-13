using SeCoGes.Utilities;
using SeCoGEST.Entities;
using SeCoGEST.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.Skins;
using Telerik.Windows.Documents.Fixed.Model.Navigation;

namespace SeCoGEST.Web.Interventi
{
    public partial class Ticket : System.Web.UI.Page
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

        #endregion

        #region Definizione pulsanti toolbar
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

        #endregion

        #region Intercettazione Eventi

        /// <summary>
        /// Metodo di gestione dell'evento Load della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            MasterPageHelper.SetMenuVisibile(this, false);
            CaricaAutorizzazioni();

            if (!Helper.Web.IsPostOrCallBack(this))
            {
                try
                {
                    SeCoGes.Logging.LogManager.AddLogAccessi(String.Format("Accesso alla pagina '{0}'.", Request.Url.AbsolutePath));

                    ConfiguraEditorHTML();

                    Logic.Interventi ll = new Logic.Interventi();
                    Entities.Intervento entityToShow = ll.Find(currentID.Value);


                    if (entityToShow != null)
                    {
                        ShowDataForEdit(entityToShow);
                    }
                    else
                    {
                        ShowDataForNew();
                        //Entities.Intervento entitySubstituteToShow = ll.Find(currentSubstituteID.Value);
                        //if (entitySubstituteToShow != null)
                        //{
                        //    ShowDataForNew();
                        //}
                        //else
                        //{
                        //    ShowDataForNew(entitySubstituteToShow);
                        //}
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
                        txtModitvazioneSostituzione.Text = string.Empty;
                        SaveData();
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

        #endregion

        #region Metodi di gestione

        /// <summary>
        /// Mostra nell'interfaccia i dati dell'entity passata
        /// </summary>
        /// <param name="entity"></param>
        private void ShowDataForNew() //Entities.Intervento entitySubstituteToShow = null)
        {
            ApplicaRedirectPulsanteAggiorna();
            PulsanteToolbar_SeparatoreSostituisci.Visible = false;
            PulsanteToolbar_Sostituisci.Visible = false;
            lcTicketProvenienza.Visible = false;
            lcTicketSostitutivo.Visible = false;

            //if (entitySubstituteToShow != null)
            //{
            //    lblTitolo.Text = "Nuovo Ticket in sostituzione al N." + entitySubstituteToShow.Numero;
            //}
            //else
            //{
            //    lblTitolo.Text = "Nuovo Ticket";
            //}
            lblTitolo.Text = "Nuovo Ticket";
            this.Title = lblTitolo.Text;

            Logic.Interventi llInterventi = new Logic.Interventi();

            rntbNumeroIntervento.Value = llInterventi.GetNuovoNumeroIntervento();
            rdtpDataRedazione.SelectedDate = DateTime.Now;
            rtbStato.Text = StatoInterventoEnum.Aperto.GetDescription();
            //repMessaggi.Visible = false;
            //lrAllegati.Visible = false;


            InformazioniAccountAutenticato utenteLoggato = InformazioniAccountAutenticato.GetIstance();
            if (utenteLoggato != null && utenteLoggato.Account != null)
            {
                Logic.Metodo.AnagraficheClienti llAnagraficaClienti = new Logic.Metodo.AnagraficheClienti(llInterventi);
                Entities.AnagraficaClienti anagraficaCliente = llAnagraficaClienti.Find(utenteLoggato.Account.CodiceCliente);

                if (anagraficaCliente != null)
                {
                    hfCodiceCliente.Value = anagraficaCliente.CODCONTO;
                    rtbRagioneSocialeCliente.Text = anagraficaCliente.DSCCONTO1;
                    rtbIndirizzo.Text = anagraficaCliente.INDIRIZZO;
                    rtbCAP.Text = anagraficaCliente.CAP;
                    rtbLocalita.Text = anagraficaCliente.LOCALITA;
                    rtbProvincia.Text = anagraficaCliente.PROVINCIA;
                    rtbTelefono.Text = anagraficaCliente.TELEFONO;

                    GestioneIntestazioneCliente(anagraficaCliente.CODCONTO, utenteLoggato.Account);
                }
                else
                {
                    throw new Exception(String.Format("Non è stato possibile recuperare le informazioni anagrafiche per l'utente '{0}' (Codice Cliente: {1})", utenteLoggato.Account.UserName, utenteLoggato.Account.CodiceCliente));
                }

                rtbOggetto.Text = String.Empty;
                rtbOggetto.ReadOnly = false;

                //reRichiesteProblematicheRiscontrate.Content = String.Empty;
                txtRichiesteProblematicheRiscontrate.Text = String.Empty;
                lrDiscussioni.Visible = false;
                lrRichiesteProblematicheRiscontrate.Visible = true;
            }
            else
            {
                throw new Exception(Infrastructure.ErrorMessage.UTENTE_SCONOSCIUTO_MESSAGE);
            }

            FillComboTipologieIntervento();

            //if (entitySubstituteToShow == null)
            //{
            //    InformazioniAccountAutenticato utenteLoggato = InformazioniAccountAutenticato.GetIstance();
            //    if (utenteLoggato != null && utenteLoggato.Account != null)
            //    {
            //        Logic.Metodo.AnagraficheClienti llAnagraficaClienti = new Logic.Metodo.AnagraficheClienti(llInterventi);
            //        Entities.AnagraficaClienti anagraficaCliente = llAnagraficaClienti.Find(utenteLoggato.Account.CodiceCliente);

            //        if (anagraficaCliente != null)
            //        {
            //            hfCodiceCliente.Value = anagraficaCliente.CODCONTO;
            //            rtbRagioneSocialeCliente.Text = anagraficaCliente.DSCCONTO1;
            //            rtbIndirizzo.Text = anagraficaCliente.INDIRIZZO;
            //            rtbCAP.Text = anagraficaCliente.CAP;
            //            rtbLocalita.Text = anagraficaCliente.LOCALITA;
            //            rtbProvincia.Text = anagraficaCliente.PROVINCIA;
            //            rtbTelefono.Text = anagraficaCliente.TELEFONO;
            //        }
            //        else
            //        {
            //            throw new Exception(String.Format("Non è stato possibile recuperare le informazioni anagrafiche per l'utente '{0}' (Codice Cliente: {1})", utenteLoggato.Account.UserName, utenteLoggato.Account.CodiceCliente));
            //        }

            //        rtbOggetto.Text = String.Empty;
            //        reRichiesteProblematicheRiscontrate.Content = String.Empty;
            //    }
            //    else
            //    {
            //        throw new Exception(Infrastructure.ErrorMessage.UTENTE_SCONOSCIUTO_MESSAGE);
            //    }
            //}

            //else
            //{
            //    hfCodiceCliente.Value = entitySubstituteToShow.CodiceCliente;
            //    rtbRagioneSocialeCliente.Text = entitySubstituteToShow.RagioneSociale;
            //    rtbIndirizzo.Text = entitySubstituteToShow.Indirizzo;
            //    rtbCAP.Text = entitySubstituteToShow.CAP;
            //    rtbLocalita.Text = entitySubstituteToShow.Localita;
            //    rtbProvincia.Text = entitySubstituteToShow.Provincia;
            //    rtbTelefono.Text = entitySubstituteToShow.Telefono;

            //    rtbOggetto.Text = String.Empty;
            //    reRichiesteProblematicheRiscontrate.Content = String.Empty;

            //    lcTicketProvenienza.Visible = true;
            //    rtbTicketProvenienza.Text = entitySubstituteToShow.Numero.ToString(); ;
            //}
        }

        /// <summary>
        /// Mostra nell'interfaccia i dati dell'entity passata
        /// </summary>
        /// <param name="entity"></param>
        private void ShowDataForEdit(Entities.Intervento entityToShow)
        {
            ApplicaRedirectPulsanteAggiorna();
            PulsanteToolbar_SeparatoreSostituisci.Visible = true;
            PulsanteToolbar_Sostituisci.Visible = true;
            lcTicketProvenienza.Visible = false;
            lcTicketSostitutivo.Visible = false;
            lrDescrizioneIntervento.Visible = true;


            if (entityToShow == null)
            {
                throw new ArgumentNullException("Parametro nullo", "entityToShow");
            }

            Entities.Intervento_Stato ultimoStato = entityToShow.Intervento_Statos.OrderByDescending(s => s.Data).FirstOrDefault();

            //if(entityToShow.Intervento_Statos.Any(s => s.StatoEnum == StatoInterventoEnum.Chiuso))
            if ((ultimoStato != null && 
                (ultimoStato.StatoEnum == StatoInterventoEnum.Chiuso || ultimoStato.StatoEnum == StatoInterventoEnum.Sostituito || ultimoStato.StatoEnum == StatoInterventoEnum.Validato)) ||
                (entityToShow.Chiuso.HasValue && entityToShow.Chiuso.Value))
            {
                this.Enabled = false;
                PulsanteToolbar_SeparatoreSostituisci.Visible = false;
                PulsanteToolbar_Sostituisci.Visible = false;
                //ucDocumentazioneAllegata.IsDeleteEnabled = false;
                rfvRichiesteProblematicheRiscontrate.Enabled = false;
            }
            //if (entityToShow.Chiuso.HasValue && entityToShow.Chiuso.Value == true)
            //{
            //    this.Enabled = false;
            //    PulsanteToolbar_SeparatoreSostituisci.Visible = false;
            //    PulsanteToolbar_Sostituisci.Visible = false;
            //    //ucDocumentazioneAllegata.IsDeleteEnabled = false;
            //    rfvRichiesteProblematicheRiscontrate.Enabled = false;
            //}

            lblTitolo.Text = string.Format("Ticket N.{0}", entityToShow.Numero);
            this.Title = lblTitolo.Text;

            rntbNumeroIntervento.Value = entityToShow.Numero;
            rdtpDataRedazione.SelectedDate = entityToShow.DataRedazione;
            rtbStato.Text = entityToShow.StatoStringForCustomers;

            hfCodiceCliente.Value = entityToShow.CodiceCliente;
            rtbRagioneSocialeCliente.Text = entityToShow.RagioneSociale;
            hfIdDestinazione.Value = entityToShow.IdDestinazione;
            rtbDestinazione.Text = entityToShow.DestinazioneMerce;
            rtbDestinazione.Visible = hfIdDestinazione.Value != string.Empty;

            rtbIndirizzo.Text = entityToShow.Indirizzo;
            rtbCAP.Text = entityToShow.CAP;
            rtbLocalita.Text = entityToShow.Localita;
            rtbProvincia.Text = entityToShow.Provincia;
            rtbTelefono.Text = entityToShow.Telefono;

            rtbOggetto.Text = entityToShow.Oggetto;
            rtbOggetto.ReadOnly = true;




            ShowDiscussion(entityToShow);
            //reRichiesteProblematicheRiscontrate.Content = entityToShow.RichiesteProblematicheRiscontrate;

            //txtRichiesteProblematicheRiscontrate.Text = entityToShow.RichiesteProblematicheRiscontrate;
            //if(entityToShow.Intervento_Discussiones.Any())
            //{
            //    txtRichiesteProblematicheRiscontrate.Text = String.Empty;
            //    //lrRichiesteProblematicheRiscontrate.Visible = false;
            //    repDiscussioni.DataSource = entityToShow.Intervento_Discussiones.OrderBy(x => x.DataCommento);
            //    repDiscussioni.DataBind();
            //}
            //else
            //{
            //    //lrRichiesteProblematicheRiscontrate.Visible = true;
            //}




            rtbDefinizione.Text = entityToShow.Definizione;

            if (entityToShow.InterventoOriginale != null)
            {
                lcTicketProvenienza.Visible = true;
                hlLinkTicketProvenienza.Text = entityToShow.InterventoOriginale.Numero.ToString();
                hlLinkTicketProvenienza.NavigateUrl = this.Request.Path + "?id=" + entityToShow.InterventoOriginale.ID.ToString();
                txtMotivazioneSostituzioneTicketProvenienza.Text = entityToShow.InterventoOriginale.MotivazioneSostituzione;
                lblTitolo.Text = $"Ticket n.{entityToShow.Numero} in sostituzione al n.{entityToShow.InterventoOriginale.Numero}";
            }
            if (entityToShow.InterventoSostitutivo != null)
            {
                lcTicketSostitutivo.Visible = true;
                hlLinkTicketSostitutivo.Text = entityToShow.InterventoSostitutivo.Numero.ToString();
                hlLinkTicketSostitutivo.NavigateUrl = this.Request.Path + "?id=" + entityToShow.InterventoSostitutivo.ID.ToString();
                txtMotivazioneSostituzioneTicketSostitutivo.Text = entityToShow.MotivazioneSostituzione;
            }


            FillComboTipologieIntervento();
            if(entityToShow.IDConfigurazioneTipologiaTicket != null)
            {
                rcbTipologiaIntervento.Visible = false;
                rtbTipologiaIntervento.Visible = true;
                rtbTipologiaIntervento.Text = entityToShow.ConfigurazioneTipologiaTicketCliente.NomeEstesoTipologiaIntervento;

                //RadComboBoxItem itemConfigurazioneTipologiaTicket = rcbTipologiaIntervento.FindItemByValue(entityToShow.IDConfigurazioneTipologiaTicket.ToString());
                //if(itemConfigurazioneTipologiaTicket != null)
                //{
                //    itemConfigurazioneTipologiaTicket.Selected = true;
                //}
                //else
                //{
                //    // Se la Tipologia Intervento memorizzata nel ticket non è visibile all'utente allora la mostra in una casella in sola lettura
                //    rcbTipologiaIntervento.Visible = false;
                //    rtbTipologiaIntervento.Visible = true;
                //    rtbTipologiaIntervento.Text = entityToShow.ConfigurazioneTipologiaTicketCliente.NomeEstesoTipologiaIntervento;
                //}
                ShowConfigurazioneTipologiaIntervento(entityToShow.IDConfigurazioneTipologiaTicket.ToString());
            }

            lrInfoOperatori.Visible = true;
            ucOperatoriIntervento.Visible = true;
            ucOperatoriIntervento.IDIntervento = new EntityId<Entities.Intervento>(entityToShow.ID);
            ucOperatoriIntervento.Inizializza();


            //lrAllegati.Visible = true;
            //ucDocumentazioneAllegata.Inizializza();
        }


        private void ShowDiscussion(Entities.Intervento entityToShow)
        {
            InformazioniAccountAutenticato accountAutenticato = InformazioniAccountAutenticato.GetIstance();


            // La casella di testo di inserimento della richiesta viene visualizzata sempre...
            lrRichiesteProblematicheRiscontrate.Visible = true;
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
            Entities.Intervento_Discussione primaDiscussione = null;

            int numOfDiscussions = elencoDiscussioni.Count();
            if (numOfDiscussions == 0)
            {
                return;
            }

            else
            {
                primaDiscussione = elencoDiscussioni.FirstOrDefault();
                diPrimaRichiesta.ShowDiscussione(primaDiscussione, true);
                lrPrimaRichiesta.Visible = true;

                txtRichiesteProblematicheRiscontrate.Text = String.Empty;
                elencoDiscussioni = elencoDiscussioni.Except(new Entities.Intervento_Discussione[] { primaDiscussione });
            }


            //if (numOfDiscussions > 1)
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



            //if (numOfDiscussions >= 2)
            //{
            //    if (!interventoBloccato)
            //    {
            //        elencoDiscussioni = elencoDiscussioni.OrderByDescending(x => x.DataCommento);
            //        Entities.Intervento_Discussione ultimaDiscussione = elencoDiscussioni.FirstOrDefault();
            //        elencoDiscussioni = elencoDiscussioni.OrderBy(x => x.DataCommento);
            //        if (ultimaDiscussione != null)
            //        {
            //            if (ultimaDiscussione.Account.TipologiaEnum != TipologiaAccountEnum.SeCoGes)
            //            {
            //                //IDUltimaDiscussione = ultimaDiscussione.ID;
            //                txtRichiesteProblematicheRiscontrate.Text = ultimaDiscussione.Commento;
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
        /// Se il codice passato si riferisce ad un cliente che ha più destinazioni (che possono essere altri clienti collegati)
        /// allora viene mostrata una combobox che permette all'utente di selezionare la giusta destinazione/cliente.
        /// Altrimenti vengono mostrati i dati dell'azienda corrispondente senza possibilità di modifica.
        /// 
        /// 
        /// Visualizza nell'interfaccia i dati relativi all'azienda dell'utente autenticato in base al suo ruolo ed alla situazione aziendale
        /// </summary>
        /// <param name="codiceCliente"></param>
        private void GestioneIntestazioneCliente(string codiceCliente, Entities.Account utenteLoggato)
        {
            rtbDestinazione.Visible = false;
            hfIdDestinazione.Value = string.Empty;
            rcbIndirizzi.Visible = false;


            Logic.Metodo.AnagraficaDestinazioniMerce ll = new Logic.Metodo.AnagraficaDestinazioniMerce();
            IEnumerable<Entities.AnagraficaDestinazioneMerce> destinazioni = ll.Read(codiceCliente, false);

            // Se l'utente che sta aprendo il ticket è un Cliente Standard... 
            if (utenteLoggato.TipologiaEnum == TipologiaAccountEnum.ClienteStandard ||
                utenteLoggato.TipologiaEnum == TipologiaAccountEnum.ClienteSupervisore)
            {
                // ...se è configurato per la gestione dei tickets della sola azienda principale...
                if (utenteLoggato.IdDestinazione == null)
                {
                    return;
                }
                else // ...altrimenti se è configurato per la gestione dei tickets di una azienda secondaria...
                {
                    Entities.AnagraficaDestinazioneMerce destinazione = destinazioni.FirstOrDefault(d => d.CodiceDestinazione.ToString() == utenteLoggato.IdDestinazione);
                    if(destinazione != null)
                    {
                        rtbDestinazione.Visible = true;
                        rtbDestinazione.Text = destinazione.RagioneSociale;
                        hfIdDestinazione.Value = destinazione.CodiceDestinazione.ToString();
                        rtbIndirizzo.Text = destinazione.Indirizzo;
                        rtbCAP.Text = destinazione.CAP;
                        rtbLocalita.Text = destinazione.Localita;
                        rtbProvincia.Text = destinazione.Provincia;
                        rtbTelefono.Text = destinazione.Telefono;
                    }
                }
            }
            else if (utenteLoggato.TipologiaEnum == TipologiaAccountEnum.ClienteAdmin)
            {
                if (destinazioni.Count() == 0)
                {
                    return;
                    //rtbRagioneSocialeCliente.Visible = true;
                    //rcbCliente.Visible = false;

                    //Entities.AnagraficaDestinazioneMerce anagraficaCliente = destinazioni.First();
                    //hfCodiceCliente.Value = anagraficaCliente.CodiceCliente;
                    //rtbRagioneSocialeCliente.Text = anagraficaCliente.RagioneSociale;
                    //rtbIndirizzo.Text = anagraficaCliente.Indirizzo;
                    //rtbCAP.Text = anagraficaCliente.CAP;
                    //rtbLocalita.Text = anagraficaCliente.Localita;
                    //rtbProvincia.Text = anagraficaCliente.Provincia;
                    //rtbTelefono.Text = anagraficaCliente.Telefono;
                }
                else
                {
                    //rtbRagioneSocialeCliente.Visible = false;
                    //rcbCliente.Visible = true;

                    //rcbCliente.DataSource = destinazioni;
                    //rcbCliente.DataBind();
                    rcbIndirizzi.Visible = true;

                    foreach (Entities.AnagraficaDestinazioneMerce entity in destinazioni)
                    {
                        RadComboBoxItem item = new RadComboBoxItem(entity.RagioneSociale, entity.CodiceDestinazione.ToString());
                        item.Attributes.Add("Ind", entity.Indirizzo);
                        item.Attributes.Add("CAP", entity.CAP);
                        item.Attributes.Add("Loc", entity.Localita);
                        item.Attributes.Add("Prov", entity.Provincia);
                        item.Attributes.Add("Tel", entity.Telefono);
                        item.DataItem = entity;
                        rcbIndirizzi.Items.Add(item);
                    }
                    RadComboBoxItem emptyItem = new RadComboBoxItem(string.Empty, string.Empty);
                    emptyItem.Attributes.Add("Ind", string.Empty);
                    emptyItem.Attributes.Add("CAP", string.Empty);
                    emptyItem.Attributes.Add("Loc", string.Empty);
                    emptyItem.Attributes.Add("Prov", string.Empty);
                    emptyItem.Attributes.Add("Tel", string.Empty);
                    emptyItem.DataItem = null;
                    rcbIndirizzi.Items.Add(emptyItem);

                    rcbIndirizzi.DataBind();
                }

            }


            // Se l'utente che sta aprendo il ticket è un Cliente Supervisore

            // Se l'utente che sta aprendo il ticket è un Cliente Supervisore Multi Organizzazione




        }

        /// <summary>
        ///  Memorizza i dati presenti nei controlli dell'interfaccia
        /// </summary>
        /// <param name="reloadPageAfterSave"></param>
        private void SaveData(bool reloadPageAfterSave = true)
        {
            // Valida i dati nella pagina
            MessagesCollector erroriDiValidazione = ValidaDati();

            InformazioniAccountAutenticato utenteLoggato = InformazioniAccountAutenticato.GetIstance();
            if (utenteLoggato == null || utenteLoggato.Account == null)
            {
                erroriDiValidazione.Add(Infrastructure.ErrorMessage.UTENTE_SCONOSCIUTO_MESSAGE);
            }

            if (erroriDiValidazione.HaveMessages)
            {
                throw new Exception(erroriDiValidazione.ToString("<br />"));
                // Se qualcosa non va bene mostro un avviso all'utente
                //MessageHelper.ShowErrorMessage(this, erroriDiValidazione.ToString("<br />"));
                //return;
            }

            // Definisco una variabile per memorizzare l'entità da salvare
            Entities.Intervento entityToSave = null;

            // Definisco una variabile che conterrà l'Codice dell'entity salvata. 
            // Se per qualche motivo l'entity non viene salvata allora entityId rimarrà String.Empty
            string entityId = String.Empty;
            bool inviaNotificaCreazioneTicket = false;
            bool inviaEmailNuovaDiscussione = false;

            //List<Entities.Intervento_Operatore> elencoOperatoriCreati = new List<Intervento_Operatore>();

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
                            "Il Ticket che si sta variando non esiste più in archivio."
                                + "\n\rOperazione annullata!");
                    }

                    // Al contrario, se entityToSave è nulla e CurrentId è vuota
                    // vuol dire che la pagina è stata aperta per la creazione di una nuova entità
                    nuovo = true;
                    entityToSave = new Entities.Intervento();
                    entityToSave.IDAccountUtenteRiferimento = InformazioniAccountAutenticato.GetIstance().Account.ID;

                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriDallaView(entityToSave, true);


                    //Creo nuova entity
                    logicInterventi.Create(entityToSave, true);




                    Entities.Intervento_Stato nuovoStato = new Intervento_Stato();
                    nuovoStato.Intervento = entityToSave;
                    nuovoStato.StatoEnum = Entities.StatoInterventoEnum.Aperto;
                    nuovoStato.NomeUtente = Helper.Web.GetLoggedUserName();
                    nuovoStato.Data = DateTime.Now;
                    Logic.Intervento_Stati llIS = new Logic.Intervento_Stati(logicInterventi);
                    llIS.Create(nuovoStato, true);

                    //elencoOperatoriCreati = AggiungiElencoOperatoriImpostatiNelWebConfig(ll, entityToSave, true);
                }
                else
                {
                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriDallaView(entityToSave, false);
                }


                Entities.Intervento_Discussione newDiscussione = new Intervento_Discussione();
                if(!string.IsNullOrEmpty(entityToSave.RichiesteProblematicheRiscontrate))
                {
                    //if(!entityToSave.Intervento_Discussiones.Any())
                    //{                        
                        newDiscussione.Intervento = entityToSave;
                        newDiscussione.IDAccount = utenteLoggato.Account.ID;
                        newDiscussione.Commento = entityToSave.RichiesteProblematicheRiscontrate;
                        newDiscussione.DataCommento = DateTime.Now;
                        Logic.Intervento_Discussioni llID = new Logic.Intervento_Discussioni(logicInterventi);
                        llID.Create(newDiscussione, true);
                    inviaEmailNuovaDiscussione = true;
                    //}
                }
                else
                {
                    throw new Exception("E' obbligatorio specificare il testo relativo alle Richieste e/o Problematiche Riscontrate!");
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



                // Persisto le modifiche sulla base dati nella transazione
                logicInterventi.SubmitToDatabase();

                // Persisto le modifiche sulla base dati effettuando il commit delle modifiche apportate nella transazione
                logicInterventi.CommitTransaction();

                SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - {1} l'entity Intervento (Ticket) con Oggetto '{2}'.", Request.Url.AbsolutePath, ((nuovo) ? "Creato" : "Salvato"), entityToSave.Oggetto));

                // Memorizzo l'Codice dell'entità
                entityId = entityToSave.ID.ToString();

                if (nuovo)
                {
                    inviaNotificaCreazioneTicket = true;
                }

            }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);

                logicInterventi.RollbackTransaction();

                // ...e mostro il messaggio d'errore all'utente
                MessageHelper.ShowErrorMessage(this, ex.Message);
            }
            finally
            {
                if (inviaNotificaCreazioneTicket)
                {
                    EmailManager.InviaEmailCreazioneInterventoDaParteDelCliente(utenteLoggato.Account, entityToSave);
                    //EmailManager.InviaCopiaEmailCreazioneInterventoDaParteDelClienteDestinatariSpecifici(utenteLoggato.Account, entityToSave);

                    //if (elencoOperatoriCreati != null && elencoOperatoriCreati.Count > 0)
                    //{
                    //    EmailManager.InviaEmailAssegnazioneIntervento(entityToSave, elencoOperatoriCreati);
                    //}

                    if(entityToSave.ConfigurazioneTipologiaTicketCliente != null)
                    {                        
                        if (entityToSave.ConfigurazioneTipologiaTicketCliente.Operatore != null)
                        {
                            List<Entities.Operatore> ops = new List<Entities.Operatore>();
                            ops.Add(entityToSave.ConfigurazioneTipologiaTicketCliente.Operatore);
                            EmailManager.InviaEmailAssegnazioneIntervento(entityToSave, ops);
                        }
                    }
                }
                else
                {
                    if(inviaEmailNuovaDiscussione)
                    {
                        EmailManager.InviaEmailNuovaDiscussione(entityToSave, true);
                    }
                }

                // Alla fine, se il salvataggio è andato a buon fine (entityId != Guid.Empty)
                // allora ricarico la pagina aprendola in modifica
                if (entityId != String.Empty && reloadPageAfterSave)
                {
                    Response.Redirect($"Conferma.aspx?id={entityId}&m=0");
                    //Helper.Web.ReloadPage(this, entityId);
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

            //if (String.IsNullOrEmpty(rcbTipologiaDocumento.SelectedValue))
            //{
            //    throw new Exception("Non è stata selezionata nessuna tipologia di documento.");
            //}

            // Viene creata l'istanza dell'entity Documento relativa agli allegati
            Entities.Allegato entityToCreate = new Entities.Allegato();
            entityToCreate.NomeFile = Path.GetFileName(fileCaricato.FileName);
            entityToCreate.UserName = utenteCollegato.UserName;

            //byte tipologiaDocumentoByte = Byte.Parse(rcbTipologiaDocumento.SelectedValue);
            //entityToCreate.TipologiaAllegatoEnum = (Entities..TipologiaAllegatoEnum)tipologiaDocumentoByte;
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

        private void SubstituteTicket(bool reloadPageAfterSave = true)
        {
            try
            {
                SaveData(false);
                Entities.Intervento interventoSostitutivo = ChiudiSostituisciIntervento();
                if(interventoSostitutivo != null)
                {
                    this.Response.Redirect(string.Format("{0}?{1}={2}", this.Request.Path, "ID", interventoSostitutivo.ID.ToString()));
                }
            }
            catch(Exception ex)
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

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Effettua mette in relazione gli operatori, il cui id è presente in una chiave nel web.config, con l'intervento passato come parametro
        /// </summary>
        /// <param name="llInterventi"></param>
        /// <param name="entityToSave"></param>
        /// <param name="submitChanges"></param>
        private List<Entities.Intervento_Operatore> AggiungiElencoOperatoriImpostatiNelWebConfig(Logic.Interventi llInterventi, Entities.Intervento entityToSave, bool submitChanges)
        {
            if (llInterventi == null) throw new ArgumentNullException("llInterventi", "Parametro nullo");
            if (entityToSave == null) throw new ArgumentNullException("entityToSave", "Parametro nullo");

            List<Entities.Intervento_Operatore> elencoDaRestituire = new List<Intervento_Operatore>();

            if (Infrastructure.ConfigurationKeys.IDOPERATORI_GESTIONALE_PER_ASSOCIAZIONE_CON_INTERVENTO_APERTO_DAL_CLIENTE != null && 
                Infrastructure.ConfigurationKeys.IDOPERATORI_GESTIONALE_PER_ASSOCIAZIONE_CON_INTERVENTO_APERTO_DAL_CLIENTE.Length > 0)
            {
                Logic.Intervento_Operatori llInterventoOperatori = new Logic.Intervento_Operatori(llInterventi);
                Logic.Operatori llOperatori = new Logic.Operatori(llInterventi);

                foreach(string idOperatore in Infrastructure.ConfigurationKeys.IDOPERATORI_GESTIONALE_PER_ASSOCIAZIONE_CON_INTERVENTO_APERTO_DAL_CLIENTE)
                {
                    Guid idOperatoreValido = Guid.Empty;
                    if (Guid.TryParse(idOperatore, out idOperatoreValido))
                    {
                        Entities.Operatore operatore = llOperatori.Find(new EntityId<Operatore>(idOperatore));
                        if (operatore != null)
                        {
                            Entities.Intervento_Operatore operatoreDatabase = new Intervento_Operatore();
                            operatoreDatabase.IDIntervento = entityToSave.ID;
                            operatoreDatabase.IDOperatore = idOperatoreValido;
                            operatoreDatabase.IDModalitaRisoluzione = null;
                            operatoreDatabase.PresaInCarico = null;
                            operatoreDatabase.DataPresaInCarico = null;
                            operatoreDatabase.InizioIntervento = null;
                            operatoreDatabase.FineIntervento = null;
                            operatoreDatabase.DurataMinuti = null;
                            operatoreDatabase.Note = null;

                            llInterventoOperatori.Create(operatoreDatabase, submitChanges);

                            elencoDaRestituire.Add(operatoreDatabase);
                        }                        
                    }
                }
            }

            return elencoDaRestituire;
        }

        /// <summary>
        /// Abilita o disabilita i campi dell'interfaccia in base al parametro passato
        /// </summary>
        /// <param name="abilitaCampi"></param>
        private void AbilitaCampi(bool enabled)
        {
            PulsanteToolbar_SeparatoreSalva.Visible = enabled;
            PulsanteToolbar_Salva.Visible = enabled;

            if(!enabled) rtbOggetto.ReadOnly = !enabled;

            //reRichiesteProblematicheRiscontrate.Enabled = enabled;
            txtRichiesteProblematicheRiscontrate.Enabled = enabled;
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
        /// <param name="isNew"></param>
        public void EstraiValoriDallaView(Entities.Intervento entityToFill, bool isNew)
        {
            if (entityToFill == null)
            {
                throw new ArgumentNullException("Parametro nullo", "entityToFill");
            }

            if (isNew)
            {
                entityToFill.Numero = (int)rntbNumeroIntervento.Value;
                entityToFill.DataRedazione = rdtpDataRedazione.SelectedDate.Value;
                entityToFill.CodiceCliente = hfCodiceCliente.Value.ToTrimmedString();
                entityToFill.RagioneSociale = rtbRagioneSocialeCliente.Text.Trim();

                if(rcbIndirizzi.Visible)
                {
                    entityToFill.IdDestinazione = rcbIndirizzi.SelectedValue;
                    entityToFill.DestinazioneMerce = rcbIndirizzi.Text.Trim();
                }
                else
                {
                    entityToFill.IdDestinazione = hfIdDestinazione.Value != string.Empty ? hfIdDestinazione.Value.ToTrimmedString() : null;
                    entityToFill.DestinazioneMerce = rtbDestinazione.Text.Trim();
                }

                entityToFill.Indirizzo = rtbIndirizzo.Text.Trim();
                entityToFill.CAP = rtbCAP.Text.Trim();
                entityToFill.Localita = rtbLocalita.Text.Trim();
                entityToFill.Provincia = rtbProvincia.Text.Trim();
                entityToFill.Telefono = rtbTelefono.Text.Trim();
                entityToFill.Interno = false;
                entityToFill.VisibileAlCliente = true;
            }

            entityToFill.Oggetto = rtbOggetto.Text.Trim();
            entityToFill.RichiesteProblematicheRiscontrate = txtRichiesteProblematicheRiscontrate.Text; // Helper.RadEditorHelper.ParseHtmlToImageSource(reRichiesteProblematicheRiscontrate.Content);
            entityToFill.MotivazioneSostituzione = txtModitvazioneSostituzione.Text.Trim();



            Guid? IDConfigurazioneTipologiaTicket = entityToFill.IDConfigurazioneTipologiaTicket;


            // Salva le informazioni sulla configurazione della tipologia intervento solamente se l'utente ne ha la possibilità di utilizzo
            if(rcbTipologiaIntervento.Visible && !rtbTipologiaIntervento.Visible)
            {
                if (rcbTipologiaIntervento.SelectedValue != string.Empty)
                {
                    entityToFill.IDConfigurazioneTipologiaTicket = new Guid(rcbTipologiaIntervento.SelectedValue);

                    var config = new Logic.ConfigurazioniTipologieTicketCliente().Find(new EntityId<ConfigurazioneTipologiaTicketCliente>(entityToFill.IDConfigurazioneTipologiaTicket));
                    if(config != null)
                    {
                        var tipologia = new Logic.TipologieIntervento().Find(new EntityId<TipologiaIntervento>(config.IdTipologia));
                        if(tipologia != null)
                        {
                            entityToFill.TipologiaIntervento = tipologia;
                        }

                    }
                }
                else
                {
                    entityToFill.IDConfigurazioneTipologiaTicket = null;
                    throw new Exception("E' obbligatorio specificare la Tipologia dell'intervento richiesto!");
                }


                if (entityToFill.IDConfigurazioneTipologiaTicket != IDConfigurazioneTipologiaTicket)
                {
                    SalvaInformazioniConfigurazione(entityToFill);
                }
            }
            //else if (!rcbTipologiaIntervento.Visible && rtbTipologiaIntervento.Visible)
            //{

            //}

        }

        private void SalvaInformazioniConfigurazione(Entities.Intervento entityToFill)
        {
            //Entities.Intervento_OrarioRepartoUfficio
        }


        /// <summary>
        /// Restituisce un oggetto contenente gli eventuali errori di validazione dei dati
        /// </summary>
        /// <returns></returns>
        private MessagesCollector ValidaDati()
        {
            MessagesCollector messaggi = new MessagesCollector();

            // Errori dei validatori client
            Page.Validate();
            if (!Page.IsValid)
            {
                foreach (IValidator validatore in Page.Validators)
                {

                    if (!validatore.IsValid)
                    {
                        messaggi.Add(validatore.ErrorMessage);
                    }
                }
                if (messaggi.HaveMessages) return messaggi;
            }

            if (rcbTipologiaIntervento.Visible && !rtbTipologiaIntervento.Visible)
            {
                if (rcbTipologiaIntervento.SelectedValue == string.Empty)
                {
                    messaggi.Add("E' obbligatorio specificare la Tipologia dell'intervento richiesto!");
                }
            }

            if(string.IsNullOrWhiteSpace(txtRichiesteProblematicheRiscontrate.Text))
            {
                messaggi.Add("E' obbligatorio specificare il testo relativo alle Richieste e/o Problematiche Riscontrate!");
            }

            //if (!rntbNumeroIntervento.Value.HasValue)
            //{
            //    messaggi.Add(rfvNumeroIntervento.ErrorMessage);
            //}

            return messaggi;
        }

        /// <summary>
        /// Effettua la configurazione dell'editor html
        /// </summary>
        private void ConfiguraEditorHTML()
        {
            //reRichiesteProblematicheRiscontrate.EnsureToolsFileLoaded();

            //List<Telerik.Web.UI.EditorToolGroup> groupsToRemove = new List<Telerik.Web.UI.EditorToolGroup>();
            //foreach (Telerik.Web.UI.EditorToolGroup group in reRichiesteProblematicheRiscontrate.Tools)
            //{
            //    groupsToRemove.Add(group);

            //    List<Telerik.Web.UI.EditorTool> toolsToRemove = new List<Telerik.Web.UI.EditorTool>();
            //    foreach (Telerik.Web.UI.EditorTool tool in group.Tools)
            //    {
            //        if (tool != null)
            //        {
            //            toolsToRemove.Add(tool);
            //        }
            //    }
            //    foreach (Telerik.Web.UI.EditorTool t in toolsToRemove)
            //    {
            //        group.Tools.Remove(t);
            //    }
            //}
            //foreach (Telerik.Web.UI.EditorToolGroup g in groupsToRemove)
            //{
            //    reRichiesteProblematicheRiscontrate.Tools.Remove(g);
            //}
            //return;


            //reRichiesteProblematicheRiscontrate.EnsureToolsFileLoaded();

            //Helper.RadEditorHelper.RemoveButton(reRichiesteProblematicheRiscontrate, "DocumentManager");
            //Helper.RadEditorHelper.RemoveButton(reRichiesteProblematicheRiscontrate, "FlashManager");
            //Helper.RadEditorHelper.RemoveButton(reRichiesteProblematicheRiscontrate, "MediaManager");
            //Helper.RadEditorHelper.RemoveButton(reRichiesteProblematicheRiscontrate, "TemplateManager");
            //Helper.RadEditorHelper.RemoveButton(reRichiesteProblematicheRiscontrate, "CSSClass");
            //Helper.RadEditorHelper.RemoveButton(reRichiesteProblematicheRiscontrate, "FormatStripper");
            //Helper.RadEditorHelper.RemoveButton(reRichiesteProblematicheRiscontrate, "ModuleManager");
            //Helper.RadEditorHelper.RemoveButton(reRichiesteProblematicheRiscontrate, "About");

            //// Aggiunge le formattazioni di paragrafo personalizzate
            //reRichiesteProblematicheRiscontrate.Paragraphs.Add("Rimuovi formattazione", "<p style=''>");
            //reRichiesteProblematicheRiscontrate.Paragraphs.Add("Interlinea 1", "<p style='line-height: 1;'>");
            //reRichiesteProblematicheRiscontrate.Paragraphs.Add("Interlinea 2", "<p style='line-height: 2;'>");
            //reRichiesteProblematicheRiscontrate.Paragraphs.Add("Interlinea 3", "<p style='line-height: 3;'>");
            //reRichiesteProblematicheRiscontrate.Paragraphs.Add("<h1>Heading 1</h1>", "<h1>");
            //reRichiesteProblematicheRiscontrate.Paragraphs.Add("<h2>Heading 2</h2>", "<h2>");
            //reRichiesteProblematicheRiscontrate.Paragraphs.Add("<h3>Heading 3</h3>", "<h3>");
            //reRichiesteProblematicheRiscontrate.Paragraphs.Add("<h4>Heading 4</h4>", "<h4>");
            //reRichiesteProblematicheRiscontrate.Paragraphs.Add("<h5>Heading 5</h5>", "<h5>");
            //reRichiesteProblematicheRiscontrate.Paragraphs.Add("<h6>Heading 6</h6>", "<h6>");
        }


        private void FillComboTipologieIntervento()
        {
            bool isAdmin = false;
            InformazioniAccountAutenticato utenteLoggato = InformazioniAccountAutenticato.GetIstance();
            if (utenteLoggato.Account.Amministratore.HasValue && utenteLoggato.Account.Amministratore.Value)
            {
                isAdmin = true;
            }


            rcbTipologiaIntervento.ClearSelection();

            if(hfCodiceCliente.Value.Trim() != string.Empty)
            {
                rcbTipologiaIntervento.DataSource = new Logic.ConfigurazioniTipologieTicketCliente().ReadAttivePerCliente(hfCodiceCliente.Value.Trim(), rdtpDataRedazione.SelectedDate, !isAdmin);
                rcbTipologiaIntervento.Enabled = true;
            }
            else
            {
                rcbTipologiaIntervento.DataSource = new List<Entities.ConfigurazioneTipologiaTicketCliente>();
                rcbTipologiaIntervento.Enabled = false;
            }

            rcbTipologiaIntervento.DataBind();
        }

        #endregion

        #region Gestione delle autorizzazioni

        //private Entities.ApplicazioneAccount utenteCollegato;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneAziende;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneDisattivazioneAzienda;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestioneAnniAccademici;
        //private Entities.Sicurezza.AutorizzazioniAccount Autorizzazioni_GestionePeriodiAccademiciPerAnnoAccademico;

        /// <summary>
        /// Carica gli oggetti contenenti le informazioni di accesso ai dati ed alle funzionalità esposte dalla pagina
        /// </summary>
        private void CaricaAutorizzazioni()
        {
            //try
            //{
            //    InformazioniAccountAutenticato infoAccount = InformazioniAccountAutenticato.GetIstance();
            //    utenteCollegato = infoAccount.Account;

            //    Autorizzazioni_GestioneAziende = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneAziende);
            //    Autorizzazioni_GestioneDisattivazioneAzienda = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneDisattivazioneAzienda);
            //    Autorizzazioni_GestioneAnniAccademici = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestioneAnniAccademici);
            //    Autorizzazioni_GestionePeriodiAccademiciPerAnnoAccademico = infoAccount.GetAutorizzazioniAccount(Entities.Sicurezza.AutorizzazioniAreeEnum.GestionePeriodiAccademiciPerAnnoAccademico);
            //}
            //catch (Exception ex)
            //{
            //    SeCoGes.Logging.LogManager.AddLogErrori(ex);
            //    MessageHelper.ShowErrorMessage(this, ex);
            //}
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

        #region Allegati

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

        ///// <summary>
        ///// Metodo di gestione dell'evento AjaxRequest relativo al pannello ajax della tab Allegati
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void rapAllegati_AjaxRequest(object sender, AjaxRequestEventArgs e)
        //{
        //    if (!currentID.HasValue)
        //    {
        //        ucDocumentazioneAllegata.IsReadOnly = true;
        //    }
        //    ucDocumentazioneAllegata.Inizializza();
        //}

        #endregion

        protected void rcbTipologiaIntervento_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ShowConfigurazioneTipologiaIntervento(e.Value);
        }

        void ShowConfigurazioneTipologiaIntervento(string idConfigurazioneIntervento)
        {
            lblUfficioOrariReparto.Text = string.Empty;
            lblCaratteristicheTipologiaIntervento.Text = string.Empty;
            lblDataLimite.Text = string.Empty;
            tableTicketConfigurationInfo.Visible = false;

            if (ConfigurationKeys.MOSTRA_DETTAGLI_CONFIGURAZIONE_NEL_TICKET && idConfigurazioneIntervento != string.Empty)
            {
                tableTicketConfigurationInfo.Visible = true;

                Logic.ConfigurazioniTipologieTicketCliente llCTTC = new Logic.ConfigurazioniTipologieTicketCliente();
                Entities.ConfigurazioneTipologiaTicketCliente configurazioneSelezionata = llCTTC.Find(new EntityId<ConfigurazioneTipologiaTicketCliente>(idConfigurazioneIntervento));

                if (configurazioneSelezionata != null)
                {
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
                        lblDataLimite.Text = tempoPiùRestrittivo.Ticks > 0 ? DateTime.Now.Add(tempoPiùRestrittivo).ToString("dddd dd MMMM yyyy alle  HH:mm") : "";
                    }

                }
            }
        }

        protected void repDiscussioni_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.DataItem != null && e.Item.DataItem is Entities.Intervento_Discussione)
            {
                Entities.Intervento_Discussione disc = (Entities.Intervento_Discussione)e.Item.DataItem;
                DiscussioneIntervento uc = (DiscussioneIntervento)e.Item.FindControl("diDiscussione");
                if(uc != null)
                {
                    uc.ShowDiscussione(disc);
                }
            }
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                txtModitvazioneSostituzione.Text = string.Empty;
                SaveData();
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }
    }
}