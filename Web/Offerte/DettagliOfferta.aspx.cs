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
using Telerik.Web.UI;

namespace SeCoGEST.Web.Offerte
{
    public partial class DettagliOfferta : System.Web.UI.Page
    {
        #region Costants

        private const string SEPARATORE_ELENCO_CONFIGURZIONE_ARTICOLI = "###";
        private const string SEPARATORE_INFORMAZIONI_AGGIUNTA_CONFIGURAZIONI = "@@@";
        protected const string COMANDO_AGGIORNAMENTO_METODI_PAGAMENTO = "aggiorna_metodi_pagamento";

        #endregion

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
        protected EntityId<Entities.Offerta> currentID
        {
            get
            {
                if (Request.QueryString["ID"] != null)
                    return new EntityId<Entities.Offerta>(Request.QueryString["ID"]);
                else
                    return EntityId<Entities.Offerta>.Empty;
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

        /// <summary>
        /// Restituisce un riferimento al separatore del di richiesta della conferma
        /// </summary>
        RadToolBarItem PulsanteToolbar_SeparatoreRichiestaConferma
        {
            get
            {
                return RadToolBar1.FindItemByValue("SeparatoreRichiestaConferma");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante RichiediValidazione della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_RichiediValidazione
        {
            get
            {
                return RadToolBar1.FindItemByValue("RichiediValidazione");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante ConfermaOfferta della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_ConfermaOfferta
        {
            get
            {
                return RadToolBar1.FindItemByValue("ConfermaOfferta");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante RifiutoOfferta della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_RifiutoOfferta
        {
            get
            {
                return RadToolBar1.FindItemByValue("RifiutoOfferta");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al separatore SeparatoreEsportaArticoli della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_SeparatoreEsportaArticoli
        {
            get
            {
                return RadToolBar1.FindItemByValue("SeparatoreEsportaArticoli");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante EsportaArticoli della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_EsportaArticoli
        {
            get
            {
                return RadToolBar1.FindItemByValue("EsportaArticoli") as RadToolBarItem;
            }
        }

        /// <summary>
        /// Restituisce un riferimento al separatore SeparatoreGeneraDocumenti della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_SeparatoreGeneraDocumenti
        {
            get
            {
                return RadToolBar1.FindItemByValue("SeparatoreGeneraDocumenti");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante GeneraDocumento della toolbar
        /// </summary>
        RadToolBarDropDown PulsanteToolbar_GeneraDocumento
        {
            get
            {
                return RadToolBar1.FindItemByText("Genera Documento") as RadToolBarDropDown;
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante CaricaDocumento della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_CaricaDocumento
        {
            get
            {
                return RadToolBar1.FindItemByValue("CaricaDocumento");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al separatore SeparatoreInvioOffertaAlCliente della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_SeparatoreInvioOffertaAlCliente
        {
            get
            {
                return RadToolBar1.FindItemByValue("SeparatoreInvioOffertaAlCliente");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante InviaOffertaAlCliente della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_InviaOffertaAlCliente
        {
            get
            {
                return RadToolBar1.FindItemByValue("InviaOffertaAlCliente");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante ConfermaOffertaDaParteDelCliente della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_ConfermaOffertaDaParteDelCliente
        {
            get
            {
                return RadToolBar1.FindItemByValue("ConfermaOffertaDaParteDelCliente");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante RifiutoOffertaDaParteDelCliente della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_RifiutoOffertaDaParteDelCliente
        {
            get
            {
                return RadToolBar1.FindItemByValue("RifiutoOffertaDaParteDelCliente");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al separatore SeparatoreCreaRevisione della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_SeparatoreCreaRevisione
        {
            get
            {
                return RadToolBar1.FindItemByValue("SeparatoreCreaRevisione");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante CreaRevisione della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_CreaRevisione
        {
            get
            {
                return RadToolBar1.FindItemByValue("CreaRevisione");
            }
        }

        /// <summary>
        /// Restituisce un riferimento al pulsante ClonaOfferta della toolbar
        /// </summary>
        RadToolBarItem PulsanteToolbar_ClonaOfferta
        {
            get
            {
                return RadToolBar1.FindItemByValue("ClonaOfferta");
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
            rapMetodiDiPagamento.LoadingPanelID = MasterPageHelper.GetRadAjaxLoadingPanelID(this);

            CaricaAutorizzazioni();

            reTestoPieDiPagina.Modules.Clear();
            reTestoIntestazioni.Modules.Clear();
            reTestoSezionePagamenti.Modules.Clear();
            reTestoEmailDaInviareAlCliente.Modules.Clear();

            if (!Helper.Web.IsPostOrCallBack(this))
            {
                RadPersistenceHelper.LoadState(this);

                try
                {
                    SeCoGes.Logging.LogManager.AddLogAccessi(String.Format("Accesso alla pagina '{0}'.", Request.Url.AbsolutePath));

                    Logic.Offerte ll = new Logic.Offerte();
                    Entities.Offerta entityToShow = ll.Find(currentID);
                    //Enabled = false;

                    if (entityToShow != null)
                    {
                        ShowData(entityToShow, ll);
                    }
                    else
                    {
                        PulsanteToolbar_SeparatoreEsportaArticoli.Visible = false;
                        PulsanteToolbar_EsportaArticoli.Visible = false;

                        ShowData(new Entities.Offerta(), ll);
                    }

                    ApplicaAutorizzazioni();
                }
                catch (Exception ex)
                {
                    SeCoGes.Logging.LogManager.AddLogErrori(ex);
                    MessageHelper.ShowErrorMessage(this, ex);
                }

                PopolaAggiungiTemplateOffertaPerGenerazioneDocumento();
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

                    case "RichiediValidazione":
                        ManageRichiediValidazione();
                        break;

                    case "ConfermaOfferta":
                        ManageValidazioneOffertaConEsitoPositivo();
                        break;

                    case "RifiutoOfferta":
                        ManageValidazioneOffertaConEsitoNegativo();
                        break;

                    case "EsportaArticoli":
                        ManageEsportaArticoli();
                        break;

                    case "GeneraDocumento":
                        ManageGeneraDocumento(clickedButton.Value);
                        break;

                    case "CaricaDocumento":
                        ManageCaricaDocumento();
                        break;

                    case "InviaOffertaAlCliente":
                        ManageInviaOffertaAlCliente();
                        break;

                    case "ConfermaOffertaDaParteDelCliente":
                        ManageConfermaOffertaDaParteDelCliente();
                        break;

                    case "RifiutoOffertaDaParteDelCliente":
                        ManageRifiutoOffertaDaParteDelCliente();
                        break;

                    case "CreaRevisione":
                        ManageCreaRevisione();
                        break;

                    case "ClonaOfferta":
                        ManageClonaOfferta();
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

        #endregion

        #region Metodi di gestione

        /// <summary>
        /// Mostra nell'interfaccia i dati dell'entity passata
        /// </summary>
        /// <param name="entity"></param>
        private void ShowData(Entities.Offerta entityToShow, Logic.Offerte llOfferte)
        {
            ApplicaRedirectPulsanteAggiorna();

            if (entityToShow == null)
            {
                throw new ArgumentNullException("Parametro nullo", "entityToShow");
            }

            if (!llOfferte.IsLastRevision(entityToShow))
            {
                this.Enabled = false;
            }
            else
            {
                if (entityToShow.Chiuso.HasValue && entityToShow.Chiuso.Value == true)
                {
                    this.Enabled = false;
                    PulsanteToolbar_SeparatoreRichiestaConferma.Visible = false;
                    PulsanteToolbar_RichiediValidazione.Visible = false;
                    PulsanteToolbar_ConfermaOfferta.Visible = false;
                    PulsanteToolbar_RifiutoOfferta.Visible = false;
                }
            }

            Logic.OfferteRaggruppamenti llRag = new Logic.OfferteRaggruppamenti(llOfferte);

            if (!entityToShow.ID.Equals(Guid.Empty))
            {
                PulsanteToolbar_ClonaOfferta.Visible = true;

                if (Enabled)
                {
                    if (entityToShow.StatoEnum == StatoOffertaEnum.Aperta || entityToShow.StatoEnum == StatoOffertaEnum.ValidataConEsitoNegativo)
                    {
                        Enabled = true;
                        PulsanteToolbar_SeparatoreRichiestaConferma.Visible = true;
                        PulsanteToolbar_RichiediValidazione.Visible = true;
                    }
                    else
                    {
                        Enabled = false;

                        if (entityToShow.StatoEnum == StatoOffertaEnum.InValidazione)
                        {
                            Entities.Account utenteCollegato = InformazioniAccountAutenticato.GetIstance().Account;
                            if (utenteCollegato == null)
                            {
                                throw new Exception("Utente non riconosciuto!");
                            }

                            Logic.OfferteAccountValidatori llOfferteAccountValidatori = new Logic.OfferteAccountValidatori();

                            if (llOfferteAccountValidatori.CanValidateOfferta(entityToShow.Identifier, utenteCollegato.Identifier))
                            {
                                Enabled = true;
                            }

                            PulsanteToolbar_SeparatoreRichiestaConferma.Visible = true;
                            PulsanteToolbar_ConfermaOfferta.Visible = true;
                            PulsanteToolbar_RifiutoOfferta.Visible = true;
                        }
                        else if (entityToShow.StatoEnum == StatoOffertaEnum.ValidataConEsitoPositivo)
                        {
                            PulsanteToolbar_SeparatoreGeneraDocumenti.Visible = true;
                            PulsanteToolbar_GeneraDocumento.Visible = true;
                            PulsanteToolbar_CaricaDocumento.Visible = true;
                            rowDocumentiCaricati.Visible = true;
                            ucDocumentazioneCaricata.IsDeleteEnabled = true;
                            ucDocumentazioneCaricata.EnableAjax = false;
                            ucDocumentazioneCaricata.IsUploadEnabled = false;
                            CheckVisibilitaPulsanteInvioAlCliente();
                        }
                        else if (entityToShow.StatoEnum == StatoOffertaEnum.InviataAlCliente)
                        {
                            PulsanteToolbar_SeparatoreGeneraDocumenti.Visible = true;
                            PulsanteToolbar_ConfermaOffertaDaParteDelCliente.Visible = true;
                            PulsanteToolbar_RifiutoOffertaDaParteDelCliente.Visible = true;

                            rowDocumentiCaricati.Visible = true;
                            ucDocumentazioneCaricata.IsDeleteEnabled = false;
                            ucDocumentazioneCaricata.EnableAjax = false;
                            ucDocumentazioneCaricata.IsUploadEnabled = false;
                        }
                        else if (entityToShow.StatoEnum == StatoOffertaEnum.RifiutataDalCliente)
                        {
                            PulsanteToolbar_SeparatoreCreaRevisione.Visible = true;
                            PulsanteToolbar_CreaRevisione.Visible = true;
                        }
                        else
                        {
                            PulsanteToolbar_SeparatoreNuovo.Visible = true;
                            PulsanteToolbar_Nuovo.Visible = true;
                            rowDocumentiCaricati.Visible = true;
                            ucDocumentazioneCaricata.IsDeleteEnabled = false;
                            ucDocumentazioneCaricata.EnableAjax = false;
                            ucDocumentazioneCaricata.IsUploadEnabled = false;
                        }
                    }
                }

                if (entityToShow.StatoEnum == StatoOffertaEnum.Aperta ||
                    entityToShow.StatoEnum == StatoOffertaEnum.InValidazione )
                {
                    PulsanteToolbar_SeparatoreGeneraDocumenti.Visible = true;
                    PulsanteToolbar_GeneraDocumento.Visible = true;

                    PulsanteToolbar_CaricaDocumento.Visible = true;
                    rowDocumentiCaricati.Visible = true;
                    ucDocumentazioneCaricata.IsDeleteEnabled = true;
                    ucDocumentazioneCaricata.EnableAjax = false;
                    ucDocumentazioneCaricata.IsUploadEnabled = false;
                }

                lblTitolo.Text = string.Format("Offerta N.{0}", entityToShow.Numero);

                rowRaggruppamenti.Visible = true;
                lbAggiungiNuovoRaggruppamento.Visible = Enabled;


                //repRaggruppamenti.Items.Clear();


                IQueryable<Entities.OffertaRaggruppamento> gruppi = llRag.Read(entityToShow);
                //foreach(OffertaRaggruppamento g in gruppi)
                //{
                //    AddPanelBar(g);
                //}

                repRaggruppamenti.DataSource = gruppi;
                repRaggruppamenti.DataBind();

                SettaVisibilitaTotaliGlobali(CanViewTotaliGlobali(gruppi));

                //foreach (RadPanelItem item in RaggruppamentiPanel.Items)
                //{
                //    item.HeaderTemplate = new HeaderTemplateClass();
                //    item.ApplyHeaderTemplate();
                //    item.DataBind();
                //}

                //for (int i = 0; i < RaggruppamentiPanel.Items.Count; i++)
                //RaggruppamentiPanel.Items[i].DataBind();

                //rntbTotaleCostoGlobaleCalcolato.DisplayText = String.Format("{0:c}", entityToShow.TotaleCostoCalcolato ?? 0);
                //rntbTotaleVenditaGlobaleCalcolato.DisplayText = String.Format("{0:c}", entityToShow.TotaleVenditaCalcolato ?? 0);
                //rntbTotaleRedditivitaGlobaleCalcolato.DisplayText = String.Format("{0:c} ({1}%)", entityToShow.TotaleRicaricoValutaCalcolato ?? 0, entityToShow.TotaleRicaricoPercentualeCalcolato ?? 0);


                //rntbTotaleCostoGlobale.Value = (double?)(entityToShow.TotaleCosto ?? 0);
                //rntbTotaleCostoGlobale.DisplayText = String.Format("{0:c}", entityToShow.TotaleCosto ?? 0);

                //rntbTotaleVenditaGlobale.Value = (double?)(entityToShow.TotaleVendita ?? 0);
                //rntbTotaleVenditaGlobale.DisplayText = String.Format("{0:c}", entityToShow.TotaleVendita ?? 0);

                //rntbTotaleRedditivitaGlobale.Value = (double?)(entityToShow.TotaleRicaricoValuta ?? 0);
                //rntbTotaleRedditivitaGlobale.DisplayText = String.Format("{0:c} ({1}%)", entityToShow.TotaleRicaricoValuta ?? 0, entityToShow.TotaleRicaricoPercentuale ?? 0);


                rntbTotaleCostoGlobaleCalcolato.Value = (double?)(entityToShow.TotaleCostoCalcolato ?? 0);
                rntbTotaleCostoGlobaleCalcolato.DisplayText = String.Format("{0:c}", entityToShow.TotaleCostoCalcolato ?? 0);

                rntbTotaleVenditaGlobaleCalcolato.Value = (double?)(entityToShow.TotaleVenditaConSpesaCacolato ?? 0);
                rntbTotaleVenditaGlobaleCalcolato.DisplayText = String.Format("{0:c}", entityToShow.TotaleVenditaConSpesaCacolato ?? 0);

                rntbTotaleRedditivitaGlobaleCalcolato.Value = (double?)(entityToShow.TotaleRicaricoValutaCalcolato ?? 0);
                rntbTotaleRedditivitaGlobaleCalcolato.DisplayText = String.Format("{0:c} ({1}%)", entityToShow.TotaleRicaricoValutaCalcolato ?? 0, entityToShow.TotaleRicaricoPercentualeCalcolato ?? 0);


                rntbTotaleCostoGlobale.Value = (double?)(entityToShow.TotaleCosto ?? 0);
                rntbTotaleCostoGlobale.DisplayText = String.Format("{0:c}", entityToShow.TotaleCosto ?? 0);

                rntbTotaleVenditaGlobale.Value = (double?)(entityToShow.TotaleVenditaConSpesa ?? 0);
                rntbTotaleVenditaGlobale.DisplayText = String.Format("{0:c}", entityToShow.TotaleVenditaConSpesa ?? 0);

                rntbTotaleRedditivitaGlobale.Value = (double?)(entityToShow.TotaleRicaricoValuta ?? 0);
                rntbTotaleRedditivitaGlobale.DisplayText = String.Format("{0:c} ({1}%)", entityToShow.TotaleRicaricoValuta ?? 0, entityToShow.TotaleRicaricoPercentuale ?? 0);

                if (entityToShow.StatoEnum == StatoOffertaEnum.InValidazione)
                {
                    PageMessage.Visible = true;
                    if (entityToShow.OffertaAccountValidatores.Any(x => x.EffettuatoValidazione))
                    {
                        OffertaAccountValidatore validatoreOfferta = entityToShow.OffertaAccountValidatores.FirstOrDefault(x => x.EffettuatoValidazione);
                        PageMessage.Message = String.Format("L'offerta è stata validata {0:dd/MM/yyyy} alle {0:HH:mm}", validatoreOfferta.DataOraValidazione);
                    }
                    else
                    {
                        PageMessage.Message = "L'offerta è attualmente in fase di validazione";
                    }
                }
            }
            else
            {
                lblTitolo.Text = "Nuova Offerta";

                Logic.Offerte ll = new Logic.Offerte();
                entityToShow.Numero = ll.GetNuovoNumero();
                entityToShow.NumeroRevisione = 0;
                entityToShow.Data = DateTime.Now;
                entityToShow.TipologiaTempiDiConsegnaEnum = TipologiaGiornateEnum.GiorniLavorativi;
                lbAggiungiNuovoRaggruppamento.Visible = false;
                rigaTotaliGlobali.Visible = false;

                SettaVisibilitaTotaliGlobali(false);
            }

            this.Title = lblTitolo.Text;

            //RigaMetodiPagamento.Visible = !String.IsNullOrWhiteSpace(entityToShow.CodiceCliente);
            rntbNumero.Value = entityToShow.Numero;
            rtbNumeroRevisione.Text = entityToShow.NumeroRevisione.ToString();            
            rdtpDataRedazione.SelectedDate = entityToShow.Data;
            rtbTitoloOfferta.Text = entityToShow.Titolo;
            rcbCliente.SelectedValue = entityToShow.CodiceCliente;
            rcbCliente.Text = entityToShow.RagioneSociale;
            rcbIndirizzi.Text = entityToShow.DestinazioneMerce;
            rtbIndirizzo.Text = entityToShow.Indirizzo;
            rtbCAP.Text = entityToShow.CAP;
            rtbLocalita.Text = entityToShow.Localita;
            rtbProvincia.Text = entityToShow.Provincia;
            rtbTelefono.Text = entityToShow.Telefono;
            rtbCodiceCommessa.Text = entityToShow.CodiceCommessa;            
            rtbNoteInterne.Text = entityToShow.NoteInterne;
            reTestoPieDiPagina.Content = entityToShow.TestoPieDiPagina;
            reTestoIntestazioni.Content = entityToShow.TestoIntestazione;
            reTestoSezionePagamenti.Content = entityToShow.TestoSezionePagamenti;
            //rcbCodiceIban.Text = entityToShow.CodiceIBAN;

            FillMetodiDiPagamento(entityToShow.CodicePagamento, llRag);

            if (PagamentoRichiedeIban(entityToShow.CodicePagamento))
            {
                FillElencoIban(entityToShow, llRag);
                ColonnaIban.Visible = true;
            }
            else
            {
                ColonnaIban.Visible = false;
            }

            FillTipologiaTempiDiConsegna();
            FillTipologiaGiorniValidita();
            
            rntbTempiDiConsegna.Value = entityToShow.TempiDiConsegna;

            if (entityToShow.TipologiaTempiDiConsegna.HasValue)
            {
                RadComboBoxItem item = rcbTipologiaTempiDiConsegna.FindItemByValue(entityToShow.TipologiaTempiDiConsegna.Value.ToString());
                if (item != null) item.Selected = true;
            }

            rntbGrioniValidita.Value = entityToShow.GiorniValidita;

            if (entityToShow.TipologiaGiorniValidita.HasValue)
            {
                RadComboBoxItem item = rcbTipologiaGiorniValidita.FindItemByValue(entityToShow.TipologiaGiorniValidita.Value.ToString());
                if (item != null) item.Selected = true;
            }
        }

        private void FillMetodiDiPagamento(string codiceMetodoDiPagamentoDaSelezionare, Logic.Base.LogicLayerBase logicLayerBase)
        {
            if (String.IsNullOrWhiteSpace(codiceMetodoDiPagamentoDaSelezionare)) codiceMetodoDiPagamentoDaSelezionare = String.Empty;
            Logic.Metodo.Pagamenti llPagamenti = (logicLayerBase != null) ? new Logic.Metodo.Pagamenti(logicLayerBase) : new Logic.Metodo.Pagamenti();
            List<Entities.TABPAGAMENTI> elencoMetodiDiPagamento = llPagamenti.Read().ToList();

            rcbMetodiDiPagamento.Items.Clear();
            rcbMetodiDiPagamento.ClearSelection();
            rcbMetodiDiPagamento.DataSource = elencoMetodiDiPagamento;
            rcbMetodiDiPagamento.DataBind();

            if (!String.IsNullOrWhiteSpace(codiceMetodoDiPagamentoDaSelezionare))
            {
                RadComboBoxItem item = rcbMetodiDiPagamento.FindItemByValue(codiceMetodoDiPagamentoDaSelezionare);
                if (item != null) item.Selected = true;
            }

            rcbMetodiDiPagamento.Items.Insert(0, new RadComboBoxItem(String.Empty, String.Empty));
        }

        private void FillElencoIban(Entities.Offerta entityToShow, Logic.Base.LogicLayerBase logicLayerBase)
        {
            FillElencoIban(entityToShow.CodiceCliente, entityToShow.CodiceIBAN, logicLayerBase);
        }

        private void FillElencoIban(string codiceCliente, string codiceIban, Logic.Base.LogicLayerBase logicLayerBase)
        {
            if (String.IsNullOrEmpty(codiceCliente))
            {
                rcbCodiceIban.Text = codiceIban;
            }
            else
            {
                Logic.Metodo.BancheClienti llBancheClienti = (logicLayerBase != null) ? new Logic.Metodo.BancheClienti(logicLayerBase) : new Logic.Metodo.BancheClienti();
                List<Entities.BANCAAPPCF> elencoMetodiDiPagamento = llBancheClienti.Read(codiceCliente).ToList();

                rcbCodiceIban.DataSource = elencoMetodiDiPagamento;
                rcbCodiceIban.DataBind();

                if (!String.IsNullOrWhiteSpace(codiceIban))
                {
                    RadComboBoxItem item = rcbCodiceIban.FindItemByValue(codiceIban);
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        rcbCodiceIban.Text = codiceIban;
                    }
                }
            }
        }

        private bool PagamentoRichiedeIban(string codiceMetodoDiPagamento)
        {
            if (codiceMetodoDiPagamento == null) codiceMetodoDiPagamento = String.Empty;
            return codiceMetodoDiPagamento.ToLower().Trim().StartsWith("b");
        }

        //private void AddPanelBar(OffertaRaggruppamento g)
        //{
        //    RadPanelItem itemi = new RadPanelItem(g.Denominazione);
        //    RaggruppamentiPanel.Items.Add(itemi);
        //}

        /// <summary>
        ///  Memorizza i dati presenti nei controlli dell'interfaccia
        /// </summary>
        /// <param name="reloadPageAfterSave"></param>
        private void SaveData(bool reloadPageAfterSave = true)
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
            Entities.Offerta entityToSave = null;

            // Definisco una variabile che conterrà l'Codice dell'entity salvata. 
            // Se per qualche motivo l'entity non viene salvata allora entityId rimarrà String.Empty
            string entityId = String.Empty;
            Logic.Offerte ll = new Logic.Offerte();
            Logic.OfferteRaggruppamenti llRagg = new Logic.OfferteRaggruppamenti(ll);

            try
            {
                ll.StartTransaction();

                //Se currentID contiene un Codice allora cerco l'entity nel database
                if (currentID.HasValue)
                {
                    entityToSave = ll.Find(currentID);
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
                            "L'offerta che si sta variando non esiste più in archivio."
                                + "\n\rOperazione annullata!");
                    }

                    // Al contrario, se entityToSave è nulla e CurrentId è vuota
                    // vuol dire che la pagina è stata aperta per la creazione di una nuova entità
                    nuovo = true;
                    entityToSave = new Entities.Offerta();

                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriDallaView(entityToSave, ll);


                    //Creo nuova entity
                    ll.Create(entityToSave, true);

                    // Alla prima creazione viene sempre aggiunto un primo raggruppamento
                    Entities.OffertaRaggruppamento gruppo = new OffertaRaggruppamento();
                    gruppo.Offerta = entityToSave;
                    gruppo.Ordine = 0;
                    gruppo.Denominazione = "Gruppo 1";
                    gruppo.OpzioneStampaOffertaEnum = OffertaRaggruppamentoOpzioneStampaOffertaEnum.TutteLeOpzioni;


                    llRagg.Create(gruppo, true);
                }
                else
                {
                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriDallaView(entityToSave, ll);

                    foreach (RepeaterItem groupHeader in repRaggruppamenti.Items)
                    {
                        OffertaRaggruppamentoHeader headerRaggruppamento = (OffertaRaggruppamentoHeader)groupHeader.FindControl("HeaderRaggruppamento");
                        HiddenField hd = (HiddenField)groupHeader.FindControl("hdIdRaggruppamento");
                        OffertaRaggruppamento acr = llRagg.Find(new EntityId<OffertaRaggruppamento>(new Guid(hd.Value)));
                        acr.Denominazione = headerRaggruppamento.GetDenominazioneGruppo();
                        llRagg.SubmitToDatabase();
                    }
                }


                // Persisto le modifiche sulla base dati nella transazione
                ll.SubmitToDatabase();

                // Persisto le modifiche sulla base dati effettuando il commit delle modifiche apportate nella transazione
                ll.CommitTransaction();

                SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - {1} l'entity Offerta del {2:d} con Titolo '{3}'.", Request.Url.AbsolutePath, ((nuovo) ? "Creato" : "Salvato"), entityToSave.Data, entityToSave.Titolo));

                // Memorizzo l'Codice dell'entità
                entityId = entityToSave.ID.ToString();

                Logic.Offerte llOfferte = new Logic.Offerte(ll);
                llOfferte.RicalcolaTotali(entityToSave.ID);
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

            if (!rdtpDataRedazione.SelectedDate.HasValue)
            {
                messaggi.Add(rfvDataRedazione.ErrorMessage);
            }

            if (rtbTitoloOfferta.Text.Trim() == string.Empty)
            {
                messaggi.Add(rfvTitoloOfferta.ErrorMessage);
            }

            return messaggi;
        }

        /// <summary>
        /// Legge i dati inseriti nell'interfaccia e li inserisce nell'entity passata
        /// </summary>
        /// <param name="entityToFill"></param>
        public void EstraiValoriDallaView(Entities.Offerta entityToFill, Logic.Offerte logic)
        {
            if (entityToFill == null)
            {
                throw new ArgumentNullException("Parametro nullo", "entityToFill");
            }

            entityToFill.Data = rdtpDataRedazione.SelectedDate.Value;
            entityToFill.Titolo = rtbTitoloOfferta.Text.Trim();
            entityToFill.CodiceCliente = rcbCliente.SelectedValue;
            entityToFill.RagioneSociale = rcbCliente.Text.Trim();
            entityToFill.DestinazioneMerce = rcbIndirizzi.Text.Trim();
            entityToFill.Indirizzo = rtbIndirizzo.Text.Trim();
            entityToFill.CAP = rtbCAP.Text.Trim();
            entityToFill.Localita = rtbLocalita.Text.Trim();
            entityToFill.Provincia = rtbProvincia.Text.Trim();
            entityToFill.Telefono = rtbTelefono.Text.Trim();
            entityToFill.CodiceCommessa = rtbCodiceCommessa.Text.Trim();
            
            entityToFill.NoteInterne = rtbNoteInterne.Text.Trim();
            entityToFill.TestoPieDiPagina = reTestoPieDiPagina.Content.Trim();
            entityToFill.TestoIntestazione = reTestoIntestazioni.Content.Trim();
            entityToFill.TestoSezionePagamenti = reTestoSezionePagamenti.Content.Trim();
            entityToFill.CodicePagamento = rcbMetodiDiPagamento.SelectedValue.Trim();
            entityToFill.DescrizionePagamento = rcbMetodiDiPagamento.Text.Trim();
            entityToFill.CodiceIBAN = rcbCodiceIban.Text.Trim();

            bool totaliModificati = (entityToFill.TotaleCosto != (decimal?)rntbTotaleCostoGlobale.Value || entityToFill.TotaleVendita != (decimal?)rntbTotaleVenditaGlobale.Value);

            entityToFill.TotaleCosto = (decimal?)((rntbTotaleCostoGlobale.Value ?? (double?)null));
            entityToFill.TotaleVendita = (decimal?)((rntbTotaleVenditaGlobale.Value ?? (double?)null));
            //entityToFill.TotaleRicaricoValuta = (decimal?)((rntbTotaleRedditivitaGlobale.Value * 100 ?? (double?)null));
            entityToFill.TotaleRicaricoValuta = entityToFill.TotaleVendita - entityToFill.TotaleCosto;
            entityToFill.TotaleRicaricoPercentuale = Math.Round(Helper.GenericHelper.GetPercentualeVariazione(entityToFill.TotaleCosto, entityToFill.TotaleVendita), 2);
            entityToFill.TotaliModificati = totaliModificati;

            entityToFill.GiorniValidita = (int?)rntbGrioniValidita.Value;

            if (!String.IsNullOrWhiteSpace(rcbTipologiaGiorniValidita.SelectedValue) && byte.TryParse(rcbTipologiaGiorniValidita.SelectedValue, out byte tipologiaGiorniValidita))
            {
                entityToFill.TipologiaGiorniValidita = tipologiaGiorniValidita;
            }
            else
            {
                entityToFill.TipologiaGiorniValidita = null;
            }

            entityToFill.TempiDiConsegna = (int?)rntbTempiDiConsegna.Value;

            if (!String.IsNullOrWhiteSpace(rcbTipologiaTempiDiConsegna.SelectedValue) && byte.TryParse(rcbTipologiaTempiDiConsegna.SelectedValue, out byte tipologiaTempiDiConsegna))
            {
                entityToFill.TipologiaTempiDiConsegna = tipologiaTempiDiConsegna;
            }
            else
            {
                entityToFill.TipologiaTempiDiConsegna = null;
            }
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
        /// Abilita o disabilita i campi dell'interfaccia in base al parametro passato
        /// </summary>
        /// <param name="abilitaCampi"></param>
        private void AbilitaCampi(bool enabled)
        {
            PulsanteToolbar_SeparatoreNuovo.Visible = enabled;
            PulsanteToolbar_Nuovo.Visible = enabled;
            PulsanteToolbar_SeparatoreSalva.Visible = enabled;
            PulsanteToolbar_Salva.Visible = enabled;

            rntbNumero.ReadOnly = !enabled;
            rdtpDataRedazione.Enabled = enabled;
            rtbTitoloOfferta.ReadOnly = !enabled;

            rntbGrioniValidita.ReadOnly = !enabled;
            rcbTipologiaGiorniValidita.Enabled = enabled;

            rntbTempiDiConsegna.ReadOnly = !enabled;
            rcbTipologiaTempiDiConsegna.Enabled = enabled;

            rcbCliente.Enabled = enabled;
            rcbCliente.Enabled = enabled;
            rcbIndirizzi.Enabled = enabled;
            rtbIndirizzo.ReadOnly = !enabled;
            rtbCAP.ReadOnly = !enabled;
            rtbLocalita.ReadOnly = !enabled;
            rtbProvincia.ReadOnly = !enabled;
            rtbTelefono.ReadOnly = !enabled;
            lbAggiungiNuovoRaggruppamento.Visible = enabled;
            rtbCodiceCommessa.ReadOnly = !enabled;
            
            rtbNoteInterne.ReadOnly = !enabled;
            reTestoPieDiPagina.Enabled = enabled;
            reTestoIntestazioni.Enabled = enabled;
            reTestoSezionePagamenti.Enabled = enabled;
            rcbMetodiDiPagamento.Enabled = enabled;
            rcbCodiceIban.Enabled = enabled;
            rntbTotaleCostoGlobale.ReadOnly = !enabled;
            rntbTotaleVenditaGlobale.ReadOnly = !enabled;
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

        protected void repRaggruppamenti_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Button btAggiungiArticolo = (Button)e.Item.FindControl("btAggiungiArticolo");
            Button btSalvaArticolo = (Button)e.Item.FindControl("btSalvaArticolo");
            Button btAggiornaArticolo = (Button)e.Item.FindControl("btAggiornaArticolo");
            Button btAnnullaEdit = (Button)e.Item.FindControl("btAnnullaEdit");
            Button btClonaGruppo = (Button)e.Item.FindControl("btClonaGruppo");
            OffertaRaggruppamentoEditItem EditItemRaggruppamento = (OffertaRaggruppamentoEditItem)e.Item.FindControl("EditItemRaggruppamento");
            RadGrid rgGrigliaArticoli = (RadGrid)e.Item.FindControl("rgGrigliaArticoli");
            HiddenField hdIdArticoloInEdit = (HiddenField)e.Item.FindControl("hdIdArticoloInEdit");
            LayoutRow rigaTotaliGruppo = (LayoutRow)e.Item.FindControl("rigaTotaliGruppo");
            LayoutRow rigaOpzioniStampa = (LayoutRow)e.Item.FindControl("rigaOpzioniStampa");

            switch (e.CommandName)
            {
                case "AggiungiArticolo":
                    hdIdArticoloInEdit.Value = string.Empty;
                    btAggiungiArticolo.Visible = false;
                    btSalvaArticolo.Visible = true;
                    btAggiornaArticolo.Visible = false;
                    btAnnullaEdit.Visible = true;
                    btClonaGruppo.Visible = false;
                    EditItemRaggruppamento.Visible = true;
                    EditItemRaggruppamento.Inizializza(btSalvaArticolo.ValidationGroup);
                    rgGrigliaArticoli.Visible = true;
                    rigaTotaliGruppo.Visible = false;
                    rigaOpzioniStampa.Visible = false;
                    break;

                case "SalvaArticolo":
                    hdIdArticoloInEdit.Value = string.Empty;
                    Guid idGruppo = new Guid(e.CommandArgument.ToString());
                    SalvaArticolo(EditItemRaggruppamento, e.Item, idGruppo, Guid.Empty);
                    btAggiungiArticolo.Visible = true;
                    btSalvaArticolo.Visible = false;
                    btAggiornaArticolo.Visible = false;
                    btAnnullaEdit.Visible = false;
                    btClonaGruppo.Visible = true;
                    EditItemRaggruppamento.Visible = false;
                    rgGrigliaArticoli.Visible = true;
                    rigaTotaliGruppo.Visible = false;
                    rigaOpzioniStampa.Visible = false;
                    break;

                case "AggiornaArticolo":
                    Guid idArticolo = new Guid(hdIdArticoloInEdit.Value.ToString());
                    SalvaArticolo(EditItemRaggruppamento, e.Item, Guid.Empty, idArticolo);
                    btAggiungiArticolo.Visible = true;
                    btSalvaArticolo.Visible = false;
                    btAggiornaArticolo.Visible = false;
                    btAnnullaEdit.Visible = false;
                    btClonaGruppo.Visible = true;
                    EditItemRaggruppamento.Visible = false;
                    rgGrigliaArticoli.Visible = true;
                    rigaTotaliGruppo.Visible = false;
                    rigaOpzioniStampa.Visible = false;
                    break;

                case "AnnullaEdit":
                    hdIdArticoloInEdit.Value = string.Empty;
                    btAggiungiArticolo.Visible = true;
                    btSalvaArticolo.Visible = false;
                    btAggiornaArticolo.Visible = false;
                    btAnnullaEdit.Visible = false;
                    btClonaGruppo.Visible = true;
                    EditItemRaggruppamento.Visible = false;
                    rgGrigliaArticoli.Visible = true;
                    rigaTotaliGruppo.Visible = true;
                    rigaOpzioniStampa.Visible = true;
                    break;


                case "ClonaGruppo":
                    break;

            }
        }

        //protected void repRaggruppamenti_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        RadGrid rgGrigliaArticoli = (RadGrid)e.Item.FindControl("rgGrigliaArticoli");
        //        if (rgGrigliaArticoli != null)
        //        {
        //            Logic.OfferteArticoli llArt = new Logic.OfferteArticoli();
        //            rgGrigliaArticoli.DataSource = llArt.Read((OffertaRaggruppamento)e.Item.DataItem);
        //            //rgGrigliaArticoli.DataBind();
        //        }
        //    }
        //}



        private void SalvaArticolo(OffertaRaggruppamentoEditItem editItemRaggruppamento, RepeaterItem repeaterItem, Guid idGruppo, Guid idArticolo)
        {
            try
            {
                RadComboBox rcbGruppo = (RadComboBox)editItemRaggruppamento.FindControl("rcbGruppo");
                RadComboBox rcbCategoria = (RadComboBox)editItemRaggruppamento.FindControl("rcbCategoria");
                RadComboBox rcbCategoriaStatistica = (RadComboBox)editItemRaggruppamento.FindControl("rcbCategoriaStatistica");

                RadComboBox rcbCodiceArticolo = (RadComboBox)editItemRaggruppamento.FindControl("rcbCodiceArticolo");
                RadTextBox rtbDescrizione = (RadTextBox)editItemRaggruppamento.FindControl("rtbDescrizione");
                RadTextBox rtbUM = (RadTextBox)editItemRaggruppamento.FindControl("rtbUM");
                RadNumericTextBox rntbQuantità = (RadNumericTextBox)editItemRaggruppamento.FindControl("rntbQuantità");
                RadNumericTextBox rntbCosto = (RadNumericTextBox)editItemRaggruppamento.FindControl("rntbCosto");
                RadNumericTextBox rntbVendita = (RadNumericTextBox)editItemRaggruppamento.FindControl("rntbVendita");
                RadNumericTextBox rntbTotaleCosto = (RadNumericTextBox)editItemRaggruppamento.FindControl("rntbTotaleCosto");
                RadNumericTextBox rntbRicaricoValore = (RadNumericTextBox)editItemRaggruppamento.FindControl("rntbRicaricoValore");
                RadNumericTextBox rntbRicaricoPercentuale = (RadNumericTextBox)editItemRaggruppamento.FindControl("rntbRicaricoPercentuale");
                RadNumericTextBox rntbTotaleVendita = (RadNumericTextBox)editItemRaggruppamento.FindControl("rntbTotaleVendita");
                SpeseAccessorieArticoloOfferta SpeseAccessorieArticoloOfferta = (SpeseAccessorieArticoloOfferta)editItemRaggruppamento.FindControl("SpeseAccessorieArticoloOfferta");
                Panel pnlContenitoreMessaggi = repeaterItem.FindControl("pnlContenitoreMessaggi") as Panel;
                Repeater rptMessaggiAutomatici = repeaterItem.FindControl("rptMessaggiAutomatici") as Repeater;
                Panel pnlContenitoreArticoliAutomatici = repeaterItem.FindControl("pnlContenitoreArticoliAutomatici") as Panel;
                Repeater rptArticoliAutomatici = repeaterItem.FindControl("rptArticoliAutomatici") as Repeater;
                Button btnAggiungiArticoliAutomaticiAlGruppo = repeaterItem.FindControl("btnAggiungiArticoliAutomaticiAlGruppo") as Button;
                Button btnProcediSenzaInserireArticoliAutomaticiAlGruppo = repeaterItem.FindControl("btnProcediSenzaInserireArticoliAutomaticiAlGruppo") as Button;
                Button btnChiudiMessaggio = repeaterItem.FindControl("btnChiudiMessaggio") as Button;


                decimal decimalValue = 0;

                decimal? codiceGruppo = null;
                if (decimal.TryParse(rcbGruppo.SelectedValue, out decimalValue))
                {
                    codiceGruppo = decimalValue;
                }
                decimal? codiceCategoria = null;
                if (decimal.TryParse(rcbCategoria.SelectedValue, out decimalValue))
                {
                    codiceCategoria = decimalValue;
                }
                decimal? codiceCategoriaStatistica = null;
                if (decimal.TryParse(rcbCategoriaStatistica.SelectedValue, out decimalValue))
                {
                    codiceCategoriaStatistica = decimalValue;
                }


                bool isNuovoArticolo = false;

                // Salvataggio Articolo
                // Se viene passato l'ID del Gruppo si tratta di un inserimenti di un nuovo articolo 
                // altrimenti se viene passato l'ID dell'Articolo si tratta di un Aggiornamento di un articolo esistente
                OffertaArticolo articolo;
                Logic.OfferteArticoli llArt = new Logic.OfferteArticoli();
                if (idGruppo != Guid.Empty && idArticolo == Guid.Empty)
                {
                    articolo = new OffertaArticolo();
                    articolo.IDRaggruppamento = idGruppo;
                    isNuovoArticolo = true;
                }
                else
                {
                    articolo = llArt.Find(new Entities.EntityId<Entities.OffertaArticolo>(idArticolo));
                }
                //OffertaArticolo articolo = new OffertaArticolo();
                articolo.CodiceGruppo = codiceGruppo;
                articolo.CodiceCategoria = codiceCategoria;
                articolo.CodiceCategoriaStatistica = codiceCategoriaStatistica;
                articolo.CodiceArticolo = rcbCodiceArticolo.SelectedValue;

                if (!String.IsNullOrWhiteSpace(articolo.CodiceArticolo))
                {
                    Logic.Metodo.AnagraficheArticoli anagraficheArticoli = new Logic.Metodo.AnagraficheArticoli(llArt);
                    Entities.ANAGRAFICAARTICOLI anagraficaArticolo = anagraficheArticoli.Find(articolo.CodiceArticolo);

                    if (anagraficaArticolo != null)
                    {
                        articolo.CodiceGruppo = anagraficaArticolo.GRUPPO;
                        articolo.CodiceCategoria = anagraficaArticolo.CATEGORIA;
                        articolo.CodiceCategoriaStatistica = anagraficaArticolo.CODCATEGORIASTAT;
                    }
                }

                articolo.Descrizione = rtbDescrizione.Text.Trim();
                articolo.UnitaMisura = rtbUM.Text.Trim();
                articolo.Quantita = (decimal?)rntbQuantità.Value;
                articolo.Costo = (decimal?)rntbCosto.Value;
                articolo.Vendita = (decimal?)rntbVendita.Value;
                articolo.TotaleCosto = (decimal?)rntbTotaleCosto.Value;
                articolo.RicaricoValuta = (decimal?)rntbRicaricoValore.Value;
                articolo.RicaricoPercentuale = (decimal?)rntbRicaricoPercentuale.Value;
                articolo.TotaleVendita = (decimal?)rntbTotaleVendita.Value;

                if (idGruppo != Guid.Empty && idArticolo == Guid.Empty)
                {
                    llArt.Create(articolo, false);




                    // Salvataggio valori campi aggiuntivi
                    Logic.OfferteArticoloCampiAggiuntivi llArtCampiAgg = new Logic.OfferteArticoloCampiAggiuntivi(llArt);

                    int ordine = -1;
                    Repeater repCampiAggiuntivi = (Repeater)editItemRaggruppamento.FindControl("repCampiAggiuntivi");
                    foreach (RepeaterItem item in repCampiAggiuntivi.Items)
                    {
                        ordine++;
                        Entities.OffertaArticoloCampoAggiuntivo campoAggiuntivo = new OffertaArticoloCampoAggiuntivo();
                        campoAggiuntivo.IDOffertaArticolo = articolo.ID;
                        campoAggiuntivo.Ordine = ordine;

                        Label caName = (Label)item.FindControl("lblNomeCampoAggiuntivo");
                        TextBox caValue = (TextBox)item.FindControl("txtValoreCampoAggiuntivo");
                        campoAggiuntivo.NomeCampo = caName.Text;
                        campoAggiuntivo.TipoCampo = "";
                        campoAggiuntivo.Valore = caValue.Text;

                        llArtCampiAgg.Create(campoAggiuntivo, false);
                    }

                    SpeseAccessorieArticoloOfferta.SpesaAccessoriaArticoloOfferta[] elencoSpeseAccessorie = SpeseAccessorieArticoloOfferta.GetElencoSpesaAccessoriaArticoloOfferta();

                    if (elencoSpeseAccessorie != null && elencoSpeseAccessorie.Length > 0)
                    {
                        elencoSpeseAccessorie = elencoSpeseAccessorie.Where(x => !x.Eliminato).ToArray();

                        if (elencoSpeseAccessorie != null && elencoSpeseAccessorie.Length > 0)
                        {
                            int incr = 0;
                            foreach(SpeseAccessorieArticoloOfferta.SpesaAccessoriaArticoloOfferta dtoSpesaAccessorie in elencoSpeseAccessorie)
                            {
                                incr++;

                                Entities.OffertaArticolo entity = SpeseAccessorieArticoloOfferta.Cast(dtoSpesaAccessorie);
                                entity.IDArticoloPadre = articolo.ID;

                                llArt.Create(entity, false);
                                entity.Ordine = incr;
                            }
                        }
                    }

                    // Persiste tutte le modifiche su database
                    llArt.SubmitToDatabase();

                }
                else
                {
                    SpeseAccessorieArticoloOfferta.SpesaAccessoriaArticoloOfferta[] elencoSpeseAccessorie = SpeseAccessorieArticoloOfferta.GetElencoSpesaAccessoriaArticoloOfferta();

                    if (elencoSpeseAccessorie != null && elencoSpeseAccessorie.Length > 0)
                    {
                        SpeseAccessorieArticoloOfferta.SpesaAccessoriaArticoloOfferta[] elencoSpeseAccessorieDaCancellare = elencoSpeseAccessorie.Where(x => x.ID != Guid.Empty && x.Eliminato).ToArray();
                        if (elencoSpeseAccessorieDaCancellare != null && elencoSpeseAccessorieDaCancellare.Length > 0)
                        {
                            foreach (SpeseAccessorieArticoloOfferta.SpesaAccessoriaArticoloOfferta dtoSpesaAccessorie in elencoSpeseAccessorieDaCancellare)
                            {
                                OffertaArticolo entityToDelete = llArt.Find(new EntityId<OffertaArticolo>(dtoSpesaAccessorie.ID));
                                if (entityToDelete != null)
                                {
                                    llArt.Delete(entityToDelete, false);
                                }

                                llArt.SubmitToDatabase();
                            }
                        }

                        SpeseAccessorieArticoloOfferta.SpesaAccessoriaArticoloOfferta[] elencoSpeseAccessorieDaCreare = elencoSpeseAccessorie.Where(x => x.Creato).ToArray();
                        if (elencoSpeseAccessorieDaCreare != null && elencoSpeseAccessorieDaCreare.Length > 0)
                        {
                            int incr = 0; // metodo recupero ordine massimo per spesa accessoria
                            foreach (SpeseAccessorieArticoloOfferta.SpesaAccessoriaArticoloOfferta dtoSpesaAccessorie in elencoSpeseAccessorieDaCreare)
                            {
                                incr++;

                                Entities.OffertaArticolo entity = SpeseAccessorieArticoloOfferta.Cast(dtoSpesaAccessorie);
                                entity.IDArticoloPadre = articolo.ID;

                                llArt.Create(entity, false);
                            }

                            llArt.SubmitToDatabase();
                        }

                        SpeseAccessorieArticoloOfferta.SpesaAccessoriaArticoloOfferta[] elencoSpeseAccessorieDaModificare = elencoSpeseAccessorie.Where(x => x.Aggiornato).ToArray();
                        if (elencoSpeseAccessorieDaModificare != null && elencoSpeseAccessorieDaModificare.Length > 0)
                        {
                            foreach (SpeseAccessorieArticoloOfferta.SpesaAccessoriaArticoloOfferta dtoSpesaAccessorie in elencoSpeseAccessorieDaModificare)
                            {
                                OffertaArticolo entityToDelete = llArt.Find(new EntityId<OffertaArticolo>(dtoSpesaAccessorie.ID));
                                if (entityToDelete != null)
                                {
                                    SpeseAccessorieArticoloOfferta.Fill(entityToDelete, dtoSpesaAccessorie);
                                }
                            }

                            llArt.SubmitToDatabase();
                        }
                    }

                    llArt.SubmitToDatabase();


                    // Aggiornamento valori campi aggiuntivi
                    Logic.OfferteArticoloCampiAggiuntivi llArtCampiAgg = new Logic.OfferteArticoloCampiAggiuntivi(llArt);

                    Repeater repCampiAggiuntivi = (Repeater)editItemRaggruppamento.FindControl("repCampiAggiuntivi");
                    foreach (RepeaterItem item in repCampiAggiuntivi.Items)
                    {
                        HiddenField hdIdCampoAgg = (HiddenField)item.FindControl("hdIdCampoAggiuntivo");

                        Entities.OffertaArticoloCampoAggiuntivo campoAggiuntivo = llArtCampiAgg.Find(new EntityId<OffertaArticoloCampoAggiuntivo>(new Guid(hdIdCampoAgg.Value)));
                        TextBox caValue = (TextBox)item.FindControl("txtValoreCampoAggiuntivo");
                        campoAggiuntivo.Valore = caValue.Text;

                        llArtCampiAgg.SubmitToDatabase();
                    }
                }

                Logic.Offerte llOfferte = new Logic.Offerte(llArt);
                llOfferte.RicalcolaTotali(articolo.OffertaRaggruppamento.IDOfferta);//correggere metodo
                Entities.Offerta offerta = articolo.OffertaRaggruppamento.Offerta;

                if (isNuovoArticolo)
                {
                    Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi analisiVenditeConfigurazioneArticoliAggiuntivi = new Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi(llArt);

                    IQueryable<AnalisiVenditaConfigurazioneArticoloAggiuntivo> queryConfigurazioneArticoliAggiuntivi = analisiVenditeConfigurazioneArticoliAggiuntivi.Read(articolo);

                    btnAggiungiArticoliAutomaticiAlGruppo.CommandArgument = String.Empty;

                    if (queryConfigurazioneArticoliAggiuntivi.Any())
                    {
                        AnalisiVenditaConfigurazioneArticoloAggiuntivo[] configurazioniMessaggi = queryConfigurazioneArticoliAggiuntivi.Where(x => x.Tipologia == (byte)TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.MostrareMessaggio || x.Tipologia == (byte)TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.MostrareMessaggio_e_AggiungereArticolo).ToArray();

                        if (configurazioniMessaggi?.Length > 0)
                        {
                            pnlContenitoreMessaggi.Visible = true;

                            rptMessaggiAutomatici.DataSource = configurazioniMessaggi.Select(x => x.TestoAvviso).ToArray();
                            rptMessaggiAutomatici.DataBind();
                        }
                        else
                        {
                            pnlContenitoreMessaggi.Visible = false;
                        }

                        AnalisiVenditaConfigurazioneArticoloAggiuntivo[] configurazioniAggiuntaArticoli = queryConfigurazioneArticoliAggiuntivi.Where(x => x.Tipologia == (byte)TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.AggiungereArticolo || x.Tipologia == (byte)TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.MostrareMessaggio_e_AggiungereArticolo).ToArray();
                        if (configurazioniAggiuntaArticoli?.Length > 0)
                        {
                            pnlContenitoreArticoliAutomatici.Visible = true;

                            rptArticoliAutomatici.DataSource = configurazioniAggiuntaArticoli.Select(x => (x.ANAGRAFICAARTICOLIOut != null) ? $"{x.ANAGRAFICAARTICOLIOut?.DESCRIZIONE} ({x.ANAGRAFICAARTICOLIOut.CODICE})" : String.Empty).ToArray();
                            rptArticoliAutomatici.DataBind();

                            string elencoIdConfigurazioniAggiuntaArticoli = String.Join(SEPARATORE_ELENCO_CONFIGURZIONE_ARTICOLI, configurazioniAggiuntaArticoli.Select(x => x.ID.ToString()));
                            btnAggiungiArticoliAutomaticiAlGruppo.CommandArgument = $"{idGruppo}{SEPARATORE_INFORMAZIONI_AGGIUNTA_CONFIGURAZIONI}{elencoIdConfigurazioniAggiuntaArticoli}";
                        }
                        else
                        {
                            pnlContenitoreArticoliAutomatici.Visible = false;
                        }

                        btnAggiungiArticoliAutomaticiAlGruppo.Visible = configurazioniAggiuntaArticoli?.Length > 0;
                        btnProcediSenzaInserireArticoliAutomaticiAlGruppo.Visible = btnAggiungiArticoliAutomaticiAlGruppo.Visible;
                        btnChiudiMessaggio.Visible = !(pnlContenitoreMessaggi.Visible && pnlContenitoreArticoliAutomatici.Visible);

                        if (!pnlContenitoreMessaggi.Visible && !pnlContenitoreArticoliAutomatici.Visible)
                        {
                            RebindRaggruppamentiAndShowDataOfferta(offerta, llOfferte);
                        }
                    }
                    else
                    {
                        RebindRaggruppamentiAndShowDataOfferta(offerta, llOfferte);
                    }
                }
                else
                {
                    RebindRaggruppamentiAndShowDataOfferta(offerta, llOfferte);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        protected void rgGrigliaArticoli_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            if (e.DetailTableView.Name == "CampiAggiuntivi")
            {
                string articoloID = dataItem.GetDataKeyValue("ID").ToString();
                Logic.OfferteArticoli llArt = new Logic.OfferteArticoli();
                Logic.OfferteArticoloCampiAggiuntivi llArtAgg = new Logic.OfferteArticoloCampiAggiuntivi(llArt);
                Entities.OffertaArticolo currentArt = llArt.Find(new Entities.EntityId<OffertaArticolo>(articoloID));

                e.DetailTableView.DataSource = llArtAgg.Read(currentArt);
            }
        }

        protected void rgGrigliaArticoli_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                Guid idSelectedRow = (Guid)(((GridDataItem)(e.Item)).GetDataKeyValue("ID"));
                if (!idSelectedRow.IsNullOrEmpty())
                {
                    Logic.OfferteArticoli ll = new Logic.OfferteArticoli();
                    Entities.OffertaArticolo entityToDelete = ll.Find(new EntityId<Entities.OffertaArticolo>(idSelectedRow));
                    if (entityToDelete != null)
                    {
                        OffertaRaggruppamento gruppo = entityToDelete.OffertaRaggruppamento;
                        Offerta offerta = gruppo.Offerta;
                        string descrizione = entityToDelete.Descrizione;
                        string nomeGruppo = gruppo.Denominazione;
                        Guid idOfferte = entityToDelete.OffertaRaggruppamento.IDOfferta;
                        string numeroOfferte = entityToDelete.OffertaRaggruppamento.Offerta.Titolo;
                        ll.Delete(entityToDelete, true);

                        SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - Rimosso l'entity OffertaArticolo '{1}' dal gruppo '{2}' nell'Offerta {3}.", Request.Url.AbsolutePath, descrizione, nomeGruppo, numeroOfferte));

                        Logic.Offerte llOfferta = new Logic.Offerte(ll);
                        llOfferta.RicalcolaTotali(idOfferte);
                        ((RadGrid)sender).DataSource = ll.Read(gruppo);
                        ((RadGrid)sender).DataBind();

                        RebindRaggruppamentiAndShowDataOfferta(offerta, ll);
                    }
                }
            }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);
                MessageHelper.ShowErrorMessage(Page, "Operazione di Eliminazione OffertaArticolo non riuscita, è stato riscontrato il seguente errore:<br />" + ex.Message);
                e.Canceled = true;
            }
        }

        protected void rgGrigliaArticoli_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "ExpandCollapse")
            {
                return;
            }

            if (e.Item is GridDataItem dataItem)
            {
                string artID = dataItem.GetDataKeyValue("ID").ToString();
                if (Guid.TryParse(artID, out Guid articoloID))
                {
                    if (e.CommandName == "Clona")
                    {
                        Logic.OfferteArticoli llOfferteArticoli = new Logic.OfferteArticoli();

                        OffertaArticolo articolo = llOfferteArticoli.Find(new EntityId<OffertaArticolo>(articoloID));
                        if (articolo != null)
                        {
                            int ordineDaApplicare = llOfferteArticoli.GetNuovoNumeroOrdinamento(articolo);

                            OffertaArticolo articoloClonato = llOfferteArticoli.Clone(articolo);
                            if (articoloClonato != null)
                            {
                                articoloClonato.Ordine = ordineDaApplicare;
                                llOfferteArticoli.Create(articoloClonato, true);

                                Logic.Offerte llOfferte = new Logic.Offerte(true);
                                Offerta offerta = llOfferte.Find(currentID);

                                llOfferte.RicalcolaTotali(offerta.ID);

                                RebindRaggruppamentiAndShowDataOfferta(offerta, llOfferte);
                            }
                        }
                    }
                    else if (e.CommandName == "MoveUp")
                    {
                        Logic.OfferteArticoli llOfferteArticoli = new Logic.OfferteArticoli();
                        bool updateGrid = llOfferteArticoli.UpdateOrderByIdArticolo(new EntityId<OffertaArticolo>(articoloID), true, true);
                        if (updateGrid && sender is RadGrid grid) grid.Rebind();                        
                    }
                    else if (e.CommandName == "MoveDown")
                    {
                        Logic.OfferteArticoli llOfferteArticoli = new Logic.OfferteArticoli();
                        bool updateGrid = llOfferteArticoli.UpdateOrderByIdArticolo(new EntityId<OffertaArticolo>(articoloID), false, true);
                        if (updateGrid && sender is RadGrid grid) grid.Rebind();
                    }
                    else
                    {
                        Panel divGruppo = (Panel)((RadGrid)sender).Parent;
                        if (divGruppo != null)
                        {
                            HiddenField hdIdArticoloInEdit = (HiddenField)divGruppo.FindControl("hdIdArticoloInEdit");
                            Button btAggiungiArticolo = (Button)divGruppo.FindControl("btAggiungiArticolo");
                            Button btSalvaArticolo = (Button)divGruppo.FindControl("btSalvaArticolo");
                            Button btAggiornaArticolo = (Button)divGruppo.FindControl("btAggiornaArticolo");
                            Button btAnnullaEdit = (Button)divGruppo.FindControl("btAnnullaEdit");
                            Button btClonaGruppo = (Button)divGruppo.FindControl("btClonaGruppo");
                            OffertaRaggruppamentoEditItem EditItemRaggruppamento = (OffertaRaggruppamentoEditItem)divGruppo.FindControl("EditItemRaggruppamento");
                            RadGrid rgGrigliaArticoli = (RadGrid)sender;
                            LayoutRow rigaTotaliGruppo = (LayoutRow)divGruppo.FindControl("rigaTotaliGruppo");
                            LayoutRow rigaOpzioniStampa = (LayoutRow)divGruppo.FindControl("rigaOpzioniStampa");

                            hdIdArticoloInEdit.Value = articoloID.ToString();

                            switch (e.CommandName)
                            {
                                case "Modifica":
                                    btAggiungiArticolo.Visible = false;
                                    btSalvaArticolo.Visible = false;
                                    btAggiornaArticolo.Visible = true;
                                    btAnnullaEdit.Visible = true;
                                    btClonaGruppo.Visible = false;
                                    EditItemRaggruppamento.Visible = true;
                                    EditItemRaggruppamento.Inizializza(articoloID, btSalvaArticolo.ValidationGroup);
                                    rgGrigliaArticoli.Visible = false;
                                    rigaTotaliGruppo.Visible = false;
                                    rigaOpzioniStampa.Visible = false;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        protected void rgGrigliaArticoli_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            HiddenField hd = (HiddenField)((RadGrid)sender).Parent.Parent.FindControl("hdIdRaggruppamento");
            if (hd != null && !String.IsNullOrWhiteSpace(hd.Value))
            {
                Logic.OfferteArticoli llArt = new Logic.OfferteArticoli();
                Logic.OfferteRaggruppamenti llAcr = new Logic.OfferteRaggruppamenti(llArt);
                OffertaRaggruppamento acr = llAcr.Find(new EntityId<OffertaRaggruppamento>(new Guid(hd.Value)));
                ((RadGrid)sender).DataSource = llArt.Read(acr);
            }
        }

        protected void rgGrigliaArticoli_PreRender(object sender, EventArgs e)
        {
            // Nasconde l'icona di espansione gerarchica della griglia per la visualizzazione dei campi aggiuntivi se questi non esistono
            HideExpandColumnRecursive(((RadGrid)sender).MasterTableView);
        }
        public void HideExpandColumnRecursive(GridTableView tableView)
        {
            // Hiding the expand/collapse images when no records 
            // http://docs.telerik.com/devtools/aspnet-ajax/controls/grid/how-to/hierarchy/hiding-the-expand-collapse-images-when-no-records
            GridItem[] nestedViewItems = tableView.GetItems(GridItemType.NestedView);
            foreach (GridNestedViewItem nestedViewItem in nestedViewItems)
            {
                foreach (GridTableView nestedView in nestedViewItem.NestedTableViews)
                {
                    //if (nestedView.Items.Count == 0)
                    if (nestedView.ParentItem["ContieneCampiAggiuntivi"].Text.ToBoolean() == false)
                    {
                        TableCell cell = nestedView.ParentItem["ExpandColumn"];
                        cell.Controls[0].Visible = false;
                        cell.Text = "&nbsp";
                        nestedViewItem.Visible = false;
                    }
                    if (nestedView.HasDetailTables)
                    {
                        HideExpandColumnRecursive(nestedView);
                    }
                }
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
                    Logic.Metodo.AnagraficaDestinazioniMerce ll = new Logic.Metodo.AnagraficaDestinazioniMerce();
                    queryBase = ll.Read(codiceCliente);
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

                foreach (Entities.AnagraficaDestinazioneMerce entity in queryBase.Skip(itemOffset).Take(itemsPerRequest))
                {
                    RadComboBoxItem item = new RadComboBoxItem(entity.RagioneSociale, entity.CodiceDestinazione.ToString());
                    item.Attributes.Add("Ind", entity.Indirizzo);
                    item.Attributes.Add("CAP", entity.CAP);
                    item.Attributes.Add("Loc", entity.Localita);
                    item.Attributes.Add("Prov", entity.Provincia);
                    item.Attributes.Add("Tel", entity.Telefono);
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

        protected void HeaderRaggruppamento_Command(object sender, EventArgs args)
        {
            //WebHelper.ReloadPage();

            Logic.Offerte llOfferte = new Logic.Offerte();
            Offerta offerta = llOfferte.Find(currentID);
            llOfferte.RicalcolaTotali(offerta.ID);

            RebindRaggruppamentiAndShowDataOfferta(offerta, llOfferte);
        }

        private void RebindGrigliaArticoli(RepeaterItem repeaterItem)
        {
            RadGrid rgGrigliaArticoli = repeaterItem.FindControl("rgGrigliaArticoli") as RadGrid;
            if (rgGrigliaArticoli != null) rgGrigliaArticoli.Rebind();
        }

        protected void repRaggruppamenti_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item != null && e.Item.DataItem is Entities.OffertaRaggruppamento)
            {
                RepeaterItem repeaterItem = e.Item;
                Entities.OffertaRaggruppamento offertaGruppamento = (Entities.OffertaRaggruppamento)e.Item.DataItem;

                HiddenField hdIdRaggruppamento = repeaterItem.FindControl("hdIdRaggruppamento") as HiddenField;
                if (hdIdRaggruppamento != null)
                {
                    hdIdRaggruppamento.Value = offertaGruppamento.ID.ToString();
                }

                OffertaRaggruppamentoHeader HeaderRaggruppamento = repeaterItem.FindControl("HeaderRaggruppamento") as OffertaRaggruppamentoHeader;
                if (HeaderRaggruppamento != null)
                {
                    HeaderRaggruppamento.SetEnabled(Enabled);
                }

                Button btAggiungiArticolo = repeaterItem.FindControl("btAggiungiArticolo") as Button;
                if (btAggiungiArticolo != null)
                {
                    btAggiungiArticolo.ValidationGroup = $"GruppoArticoli_{offertaGruppamento.ID.ToString()}";
                    btAggiungiArticolo.CommandArgument = offertaGruppamento.ID.ToString();
                    if (!Enabled) btAggiungiArticolo.Visible = false;
                }

                Button btSalvaArticolo = repeaterItem.FindControl("btSalvaArticolo") as Button;
                if (btSalvaArticolo != null)
                {
                    btSalvaArticolo.ValidationGroup = $"GruppoArticoli_{offertaGruppamento.ID.ToString()}";
                    btSalvaArticolo.CommandArgument = offertaGruppamento.ID.ToString();
                    if (!Enabled) btSalvaArticolo.Visible = false;
                }

                Button btAggiornaArticolo = repeaterItem.FindControl("btAggiornaArticolo") as Button;
                if (btAggiornaArticolo != null)
                {
                    btAggiornaArticolo.ValidationGroup = $"GruppoArticoli_{offertaGruppamento.ID.ToString()}";
                    btAggiornaArticolo.CommandArgument = offertaGruppamento.ID.ToString();
                    if (!Enabled) btAggiornaArticolo.Visible = false;
                }

                Button btAnnullaEdit = repeaterItem.FindControl("btAnnullaEdit") as Button;
                if (btAnnullaEdit != null)
                {
                    btAnnullaEdit.ValidationGroup = $"GruppoArticoli_{offertaGruppamento.ID.ToString()}";
                    btAnnullaEdit.CommandArgument = offertaGruppamento.ID.ToString();
                    if (!Enabled) btAnnullaEdit.Visible = false;
                }

                Button btClonaGruppo = repeaterItem.FindControl("btClonaGruppo") as Button;
                if (btClonaGruppo != null)
                {
                    btClonaGruppo.ValidationGroup = $"GruppoArticoli_{offertaGruppamento.ID.ToString()}";
                    btClonaGruppo.CommandArgument = offertaGruppamento.ID.ToString();
                    if (!Enabled) btClonaGruppo.Visible = false;
                }

                Button btSalvaModificheGruppo = repeaterItem.FindControl("btSalvaModificheGruppo") as Button;
                if (btSalvaModificheGruppo != null)
                {
                    btSalvaModificheGruppo.ValidationGroup = $"TotaliGruppo_{offertaGruppamento.ID.ToString()}";
                    btSalvaModificheGruppo.CommandArgument = offertaGruppamento.ID.ToString();
                    if (!Enabled) btSalvaModificheGruppo.Visible = false;
                }

                Button btAnnullaModificheGruppo = repeaterItem.FindControl("btAnnullaModificheGruppo") as Button;
                if (btAnnullaModificheGruppo != null)
                {
                    btAnnullaModificheGruppo.ValidationGroup = $"TotaliGruppo_{offertaGruppamento.ID.ToString()}";
                    btAnnullaModificheGruppo.CommandArgument = offertaGruppamento.ID.ToString();
                    if (!Enabled) btAnnullaModificheGruppo.Visible = false;
                }

                RadNumericTextBox rntbTotaleCostoCalcolato = repeaterItem.FindControl("rntbTotaleCostoCalcolato") as RadNumericTextBox;
                if (rntbTotaleCostoCalcolato != null)
                {
                    rntbTotaleCostoCalcolato.ValidationGroup = $"TotaliGruppo_{offertaGruppamento.ID.ToString()}";
                    rntbTotaleCostoCalcolato.Value = (double?)offertaGruppamento.TotaleCostoCalcolato;
                    rntbTotaleCostoCalcolato.DisplayText = String.Format("{0:c}", offertaGruppamento.TotaleCostoCalcolato);
                }

                RadNumericTextBox rntbTotaleVenditaCalcolato = repeaterItem.FindControl("rntbTotaleVenditaCalcolato") as RadNumericTextBox;
                if (rntbTotaleVenditaCalcolato != null)
                {
                    rntbTotaleVenditaCalcolato.ValidationGroup = $"TotaliGruppo_{offertaGruppamento.ID.ToString()}";
                    rntbTotaleVenditaCalcolato.Value = (double?)offertaGruppamento.TotaleVenditaConSpesaCacolato;
                    rntbTotaleVenditaCalcolato.DisplayText = String.Format("{0:c}", offertaGruppamento.TotaleVenditaConSpesaCacolato);
                }

                RadNumericTextBox rntbTotaleRedditivitaCalcolato = repeaterItem.FindControl("rntbTotaleRedditivitaCalcolato") as RadNumericTextBox;
                if (rntbTotaleRedditivitaCalcolato != null)
                {
                    rntbTotaleRedditivitaCalcolato.ValidationGroup = $"TotaliGruppo_{offertaGruppamento.ID.ToString()}";
                    rntbTotaleRedditivitaCalcolato.Value = (double?)offertaGruppamento.TotaleRicaricoValuta;
                    rntbTotaleRedditivitaCalcolato.DisplayText = String.Format("{0:c} ({1}%)", offertaGruppamento.TotaleRicaricoValuta, offertaGruppamento.TotaleRicaricoPercentualeCalcolato);
                }

                RadNumericTextBox rntbTotaleCosto = repeaterItem.FindControl("rntbTotaleCosto") as RadNumericTextBox;
                if (rntbTotaleCosto != null)
                {
                    rntbTotaleCosto.ValidationGroup = $"TotaliGruppo_{offertaGruppamento.ID.ToString()}";
                    rntbTotaleCosto.Value = (double?)offertaGruppamento.TotaleCosto;
                    rntbTotaleCosto.DisplayText = String.Format("{0:c}", offertaGruppamento.TotaleCosto);                    
                    if (!Enabled) rntbTotaleCosto.ReadOnly = true;
                }

                RadNumericTextBox rntbTotaleVendita = repeaterItem.FindControl("rntbTotaleVendita") as RadNumericTextBox;
                if (rntbTotaleVendita != null)
                {
                    rntbTotaleVendita.ValidationGroup = $"TotaliGruppo_{offertaGruppamento.ID.ToString()}";
                    rntbTotaleVendita.Value = (double?)offertaGruppamento.TotaleVenditaConSpesa;
                    rntbTotaleVendita.DisplayText = String.Format("{0:c}", offertaGruppamento.TotaleVenditaConSpesa);                    
                    if (!Enabled) rntbTotaleVendita.ReadOnly = true;
                }

                RadNumericTextBox rntbTotaleRedditivita = repeaterItem.FindControl("rntbTotaleRedditivita") as RadNumericTextBox;
                if (rntbTotaleRedditivita != null)
                {
                    rntbTotaleRedditivita.ValidationGroup = $"TotaliGruppo_{offertaGruppamento.ID.ToString()}";
                    rntbTotaleRedditivita.Value = (double?)offertaGruppamento.TotaleRicaricoValuta;
                    rntbTotaleRedditivita.DisplayText = String.Format("{0:c} ({1}%)", offertaGruppamento.TotaleRicaricoValuta, offertaGruppamento.TotaleRicaricoPercentuale);
                }

                RadGrid rgGrigliaArticoli = repeaterItem.FindControl("rgGrigliaArticoli") as RadGrid;
                if (rgGrigliaArticoli != null)
                {
                    GridColumn ColonnaModifica = rgGrigliaArticoli.Columns.FindByUniqueName("ColonnaModifica");
                    if (ColonnaModifica != null) ColonnaModifica.Visible = Enabled;

                    GridColumn ColonnaElimina = rgGrigliaArticoli.Columns.FindByUniqueName("ColonnaElimina");
                    if (ColonnaElimina != null) ColonnaElimina.Visible = Enabled;

                    rgGrigliaArticoli.Rebind();
                }

                Panel pnlContenitoreMessaggi = repeaterItem.FindControl("pnlContenitoreMessaggi") as Panel;
                if (pnlContenitoreMessaggi != null) pnlContenitoreMessaggi.Visible = false;

                Repeater rptMessaggiAutomatici = repeaterItem.FindControl("rptMessaggiAutomatici") as Repeater;
                if (rptMessaggiAutomatici != null)
                {
                    rptMessaggiAutomatici.DataSource = new string[] { };
                    rptMessaggiAutomatici.DataBind();
                }

                Panel pnlContenitoreArticoliAutomatici = repeaterItem.FindControl("pnlContenitoreArticoliAutomatici") as Panel;
                if (pnlContenitoreArticoliAutomatici != null) pnlContenitoreArticoliAutomatici.Visible = false;

                Repeater rptArticoliAutomatici = repeaterItem.FindControl("rptArticoliAutomatici") as Repeater;
                if (rptArticoliAutomatici != null)
                {
                    rptArticoliAutomatici.DataSource = new string[] { };
                    rptArticoliAutomatici.DataBind();
                }

                Button btnAggiungiArticoliAutomaticiAlGruppo = repeaterItem.FindControl("btnAggiungiArticoliAutomaticiAlGruppo") as Button;
                if (btnAggiungiArticoliAutomaticiAlGruppo != null) btnAggiungiArticoliAutomaticiAlGruppo.Visible = false;

                Button btnProcediSenzaInserireArticoliAutomaticiAlGruppo = repeaterItem.FindControl("btnProcediSenzaInserireArticoliAutomaticiAlGruppo") as Button;
                if (btnAggiungiArticoliAutomaticiAlGruppo != null) btnProcediSenzaInserireArticoliAutomaticiAlGruppo.Visible = false;

                Button btnChiudiMessaggio = repeaterItem.FindControl("btnChiudiMessaggio") as Button;
                if (btnChiudiMessaggio != null) btnChiudiMessaggio.Visible = false;

                RadCheckBoxList rcblOpzioniGruppo = repeaterItem.FindControl("rcblOpzioniGruppo") as RadCheckBoxList;
                if (rcblOpzioniGruppo != null)
                {
                    rcblOpzioniGruppo.ValidationGroup = $"OpzioniGruppo_{offertaGruppamento.ID.ToString()}";
                    if (!Enabled) rcblOpzioniGruppo.Enabled = false;

                    FillListOpzioniStampaOffertaGruppo(rcblOpzioniGruppo, offertaGruppamento);
                }

                Button btSalvaModificheOpzioniGruppo = repeaterItem.FindControl("btSalvaModificheOpzioniGruppo") as Button;
                if (btSalvaModificheOpzioniGruppo != null)
                {
                    btSalvaModificheOpzioniGruppo.ValidationGroup = $"OpzioniGruppo_{offertaGruppamento.ID.ToString()}";
                    btSalvaModificheOpzioniGruppo.CommandArgument = offertaGruppamento.ID.ToString();
                    if (!Enabled) btSalvaModificheOpzioniGruppo.Visible = false;
                }

                Button btAnnullaModificheOpzioniGruppo = repeaterItem.FindControl("btAnnullaModificheOpzioniGruppo") as Button;
                if (btAnnullaModificheOpzioniGruppo != null)
                {
                    btAnnullaModificheOpzioniGruppo.ValidationGroup = $"OpzioniGruppo_{offertaGruppamento.ID.ToString()}";
                    btAnnullaModificheOpzioniGruppo.CommandArgument = offertaGruppamento.ID.ToString();
                    if (!Enabled) btAnnullaModificheOpzioniGruppo.Visible = false;
                }
            }
        }

       

        private void RebindRaggruppamentiAndShowDataOfferta(Entities.Offerta offerta, Logic.Base.LogicLayerBase logicLayerBase)
        {
            if (offerta == null) throw new ArgumentNullException(nameof(offerta), "Parametro nullo");

            Logic.OfferteRaggruppamenti llRag = (logicLayerBase != null) ? new Logic.OfferteRaggruppamenti(logicLayerBase) : new Logic.OfferteRaggruppamenti();
            IQueryable<Entities.OffertaRaggruppamento> gruppi = llRag.Read(offerta);

            repRaggruppamenti.DataSource = gruppi;
            repRaggruppamenti.DataBind();

            SettaVisibilitaTotaliGlobali(CanViewTotaliGlobali(gruppi));

            Logic.Offerte llOfferte = new Logic.Offerte(llRag);
            ShowData(offerta, llOfferte);
        }

        private bool CanViewTotaliGlobali(IQueryable<Entities.OffertaRaggruppamento> gruppi)
        {
            return gruppi.Any() && gruppi.Any(g => g.OffertaArticolos.Any());
        }

        private void SettaVisibilitaTotaliGlobali(bool visible)
        {
            ColonnaTotaleCostoGlobale.Visible = visible;
            ColonnaTotaleVenditaGlobale.Visible = visible;
            ColonnaTotaleRedditivitaGlobale.Visible = visible;
        }

        protected void btSalvaModificheGruppo_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                if (Guid.TryParse(button.CommandArgument, out Guid idGruppo))
                {
                    Logic.OfferteRaggruppamenti llOrg = new Logic.OfferteRaggruppamenti();
                    OffertaRaggruppamento gruppo = llOrg.Find(new EntityId<OffertaRaggruppamento>(idGruppo));

                    if (gruppo != null)
                    {
                        RadNumericTextBox rntbTotaleCosto = (RadNumericTextBox)button.Parent.FindControl("rntbTotaleCosto");
                        RadNumericTextBox rntbTotaleVendita = (RadNumericTextBox)button.Parent.FindControl("rntbTotaleVendita");
                        RadNumericTextBox rntbTotaleRedditivita = (RadNumericTextBox)button.Parent.FindControl("rntbTotaleRedditivita");

                        if (rntbTotaleCosto != null &&
                            rntbTotaleVendita != null &&
                            rntbTotaleRedditivita != null)
                        {
                            bool totaliModificati = (gruppo.TotaleCosto != (decimal?)rntbTotaleCosto.Value || gruppo.TotaleVendita != (decimal?)rntbTotaleVendita.Value);

                            gruppo.TotaleCosto = (decimal?)rntbTotaleCosto.Value;                            
                            gruppo.TotaleVenditaConSpesa = (decimal?)rntbTotaleVendita.Value;
                            gruppo.TotaleVendita = gruppo.TotaleVenditaConSpesa - gruppo.TotaleSpesa;

                            //gruppo.TotaleRicaricoPercentuale = ((decimal?)rntbTotaleRedditivita.Value) * 100;

                            gruppo.TotaleRicaricoValuta = gruppo.TotaleVendita - gruppo.TotaleCosto;
                            gruppo.TotaleRicaricoPercentuale = Math.Round(Helper.GenericHelper.GetPercentualeVariazione(gruppo.TotaleCosto, gruppo.TotaleVendita), 2);
                            gruppo.TotaliModificati = totaliModificati;

                            llOrg.SubmitToDatabase();

                            Logic.Offerte llOfferte = new Logic.Offerte(llOrg);
                            llOfferte.RicalcolaTotali(currentID.Value);

                            RebindRaggruppamentiAndShowDataOfferta(gruppo.Offerta, llOrg);
                            //Helper.Web.ReloadPage(this, gruppo.AnalisiCosto.ID.ToString());
                        }
                    }
                }
            }
        }

        protected void btAnnullaModificheGruppo_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                if (Guid.TryParse(button.CommandArgument, out Guid idGruppo))
                {
                    Logic.OfferteRaggruppamenti llOrg = new Logic.OfferteRaggruppamenti();
                    OffertaRaggruppamento gruppo = llOrg.Find(new EntityId<OffertaRaggruppamento>(idGruppo));

                    if (gruppo != null)
                    {
                        RadNumericTextBox rntbTotaleCosto = (RadNumericTextBox)button.Parent.FindControl("rntbTotaleCosto");
                        RadNumericTextBox rntbTotaleVendita = (RadNumericTextBox)button.Parent.FindControl("rntbTotaleVendita");
                        RadNumericTextBox rntbTotaleRedditivita = (RadNumericTextBox)button.Parent.FindControl("rntbTotaleRedditivita");

                        if (rntbTotaleCosto != null &&
                            rntbTotaleVendita != null &&
                            rntbTotaleRedditivita != null)
                        {
                            rntbTotaleCosto.Value = (double?)gruppo.TotaleCosto;
                            rntbTotaleVendita.Value = (double?)gruppo.TotaleVenditaConSpesa;
                            rntbTotaleRedditivita.Value = (double?)gruppo.TotaleRicaricoPercentuale;
                        }
                    }
                }
            }
        }

        protected void btClonaGruppo_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                try
                {
                    if (Guid.TryParse(button.CommandArgument, out Guid idGruppo))
                    {
                        Logic.OfferteRaggruppamenti llOrg = new Logic.OfferteRaggruppamenti();
                        Logic.OfferteArticoli llOArt = new Logic.OfferteArticoli(llOrg);
                        OffertaRaggruppamento gruppo = llOrg.Find(new EntityId<OffertaRaggruppamento>(idGruppo));

                        if (gruppo != null)
                        {
                            OffertaRaggruppamento nuovoGruppo = llOrg.Clone(gruppo, true, false);
                            nuovoGruppo.IDRaggruppamentoPadre = gruppo.IDRaggruppamentoPadre;
                            nuovoGruppo.Ordine = llOrg.GetNuovoNumeroOrdinamento(nuovoGruppo);
                            nuovoGruppo.Denominazione = $"{gruppo.Denominazione} (copia)";

                            //OffertaRaggruppamento nuovoGruppo = new OffertaRaggruppamento();
                            //nuovoGruppo.ID = Guid.NewGuid();
                            //nuovoGruppo.IDOfferta = gruppo.Offerta.ID;
                            //nuovoGruppo.IDRaggruppamentoPadre = gruppo.IDRaggruppamentoPadre;
                            //nuovoGruppo.Ordine = gruppo.Offerta.OffertaRaggruppamentos.Count + 1;
                            //nuovoGruppo.Denominazione = $"{gruppo.Denominazione} (copia)";
                            //nuovoGruppo.TotaleCosto = gruppo.TotaleCosto;
                            //nuovoGruppo.TotaleVenditaCalcolato = gruppo.TotaleVenditaCalcolato;
                            //nuovoGruppo.TotaleRicaricoValuta = gruppo.TotaleRicaricoValuta;
                            //nuovoGruppo.TotaleRicaricoPercentuale = gruppo.TotaleRicaricoPercentuale;
                            //nuovoGruppo.TotaleVendita = gruppo.TotaleVendita;

                            //llOrg.Create(nuovoGruppo, false);

                            //if (gruppo.OffertaArticolos.Count > 0)
                            //{
                            //    foreach(OffertaArticolo articolo in gruppo.OffertaArticolos.OrderBy(x => x.Ordine))
                            //    {
                            //        OffertaArticolo nuovoArticolo = llOArt.Clone(articolo);
                            //        nuovoArticolo.IDRaggruppamento = nuovoGruppo.ID;

                            //        llOArt.Create(nuovoArticolo, true);
                            //    }
                            //}

                            llOrg.SubmitToDatabase();

                            Logic.Offerte llOfferte = new Logic.Offerte(llOrg);
                            llOfferte.RicalcolaTotali(gruppo.Offerta.ID);

                            RebindRaggruppamentiAndShowDataOfferta(gruppo.Offerta, llOfferte);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowErrorMessage(this, ex);
                }
            }
        }

        protected void lbAggiungiNuovoRaggruppamento_Click(object sender, EventArgs e)
        {
            try
            {
                Logic.Offerte llOff = new Logic.Offerte();
                Logic.OfferteRaggruppamenti llOrg = new Logic.OfferteRaggruppamenti(llOff);

                Offerta offerta = llOff.Find(currentID);
                if (offerta != null)
                {
                    OffertaRaggruppamento nuovoGruppo = new OffertaRaggruppamento();
                    nuovoGruppo.ID = Guid.NewGuid();
                    nuovoGruppo.IDOfferta = offerta.ID;
                    nuovoGruppo.IDRaggruppamentoPadre = null;
                    nuovoGruppo.Ordine = offerta.OffertaRaggruppamentos.Count + 1;
                    nuovoGruppo.Denominazione = $"Nuovo Gruppo";
                    nuovoGruppo.OpzioneStampaOffertaEnum = OffertaRaggruppamentoOpzioneStampaOffertaEnum.TutteLeOpzioni;

                    llOrg.Create(nuovoGruppo, true);

                    RebindRaggruppamentiAndShowDataOfferta(offerta, llOff);
                }
                else
                {
                    throw new Exception("L'offerta corrente non esiste nella base dati. Tornare all'elenco.");
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        protected void btnChiudiMessaggio_Click(object sender, EventArgs e)
        {
            DoRebindRaggruppamenti();
        }

        protected void btnProcediSenzaInserireArticoliAutomaticiAlGruppo_Click(object sender, EventArgs e)
        {
            DoRebindRaggruppamenti();
        }

        protected void btnAggiungiArticoliAutomaticiAlGruppo_Click(object sender, EventArgs e)
        {
            try
            {
                Button button = sender as Button;
                if (button != null)
                {
                    string[] identificativi = button.CommandArgument.Split(new string[] { SEPARATORE_INFORMAZIONI_AGGIUNTA_CONFIGURAZIONI }, StringSplitOptions.RemoveEmptyEntries);

                    if (identificativi?.Length >= 2 && Guid.TryParse(identificativi[0], out Guid idGruppo))
                    {
                        string[] elencoIdAnalisiVenditaConfigurazioneArticoloAggiuntivo = identificativi[1].Split(new string[] { SEPARATORE_ELENCO_CONFIGURZIONE_ARTICOLI }, StringSplitOptions.RemoveEmptyEntries);
                        if (elencoIdAnalisiVenditaConfigurazioneArticoloAggiuntivo?.Length > 0)
                        {
                            Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi analisiVenditeConfigurazioneArticoliAggiuntivi = new Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi();
                            Logic.OfferteArticoli llArt = new Logic.OfferteArticoli(analisiVenditeConfigurazioneArticoliAggiuntivi);

                            foreach (string idAnalisiVenditaConfigurazioneArticoloAggiuntivo in elencoIdAnalisiVenditaConfigurazioneArticoloAggiuntivo)
                            {
                                if (Guid.TryParse(idAnalisiVenditaConfigurazioneArticoloAggiuntivo, out Guid idConfigurazioneAggiuntivo))
                                {
                                    AnalisiVenditaConfigurazioneArticoloAggiuntivo configurazioneArticolo = analisiVenditeConfigurazioneArticoliAggiuntivi.Find(new EntityId<AnalisiVenditaConfigurazioneArticoloAggiuntivo>(idConfigurazioneAggiuntivo));
                                    if (configurazioneArticolo != null)
                                    {
                                        OffertaArticolo articoloAggiuntivo = new OffertaArticolo();
                                        articoloAggiuntivo.ID = Guid.NewGuid();
                                        articoloAggiuntivo.IDRaggruppamento = idGruppo;
                                        articoloAggiuntivo.Descrizione = configurazioneArticolo.ANAGRAFICAARTICOLIOut?.DESCRIZIONE ?? String.Empty;
                                        articoloAggiuntivo.CodiceArticolo = configurazioneArticolo.CodiceArticoloOut;
                                        articoloAggiuntivo.CodiceCategoria = configurazioneArticolo.ANAGRAFICAARTICOLIOut?.CATEGORIA;
                                        articoloAggiuntivo.CodiceCategoriaStatistica = configurazioneArticolo.ANAGRAFICAARTICOLIOut?.CODCATEGORIASTAT;
                                        articoloAggiuntivo.CodiceGruppo = configurazioneArticolo.ANAGRAFICAARTICOLIOut?.GRUPPO;
                                        articoloAggiuntivo.UnitaMisura = configurazioneArticolo.UnitaMisura;
                                        articoloAggiuntivo.Quantita = configurazioneArticolo.Quantita;
                                        articoloAggiuntivo.Costo = configurazioneArticolo.Costo;
                                        articoloAggiuntivo.Vendita = configurazioneArticolo.Vendita;
                                        articoloAggiuntivo.TotaleCosto = configurazioneArticolo.TotaleCosto;
                                        articoloAggiuntivo.RicaricoValuta = configurazioneArticolo.RicaricoValuta;
                                        articoloAggiuntivo.RicaricoPercentuale = configurazioneArticolo.RicaricoPercentuale;
                                        articoloAggiuntivo.TotaleVendita = configurazioneArticolo.TotaleVendita;

                                        llArt.Create(articoloAggiuntivo, false);
                                    }
                                }
                            }

                            llArt.SubmitToDatabase();
                        }
                    }
                }

                DoRebindRaggruppamenti();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        private void DoRebindRaggruppamenti()
        {
            Logic.Offerte llOfferte = new Logic.Offerte();
            Entities.Offerta offerta = llOfferte.Find(currentID);
            if (offerta != null)
            {
                RebindRaggruppamentiAndShowDataOfferta(offerta, llOfferte);
            }
        }

        protected void btnAnnulla_Click(object sender, EventArgs e)
        {
            rwRichiestaValidazione.VisibleOnPageLoad = false;
            ClearFinestraRichiestaValidazione();
        }

        protected void btnProsegui_Click(object sender, EventArgs e)
        {
            rwRichiestaValidazione.VisibleOnPageLoad = false;

            try
            {
                if (rbElencoValidatori.CheckedItems.Count <= 0) throw new Exception("Per proseguire è necessario selezionare almeno un validatore.");

                List<Guid> elencoIdAccount = new List<Guid>();
                foreach (RadComboBoxItem item in rbElencoValidatori.CheckedItems)
                {
                    if (Guid.TryParse(item.Value, out Guid idAccount))
                    {
                        elencoIdAccount.Add(idAccount);
                    }
                }

                ClearFinestraRichiestaValidazione();

                Logic.Offerte llOfferte = new Logic.Offerte();
                Entities.Offerta offerta = llOfferte.Find(currentID);
                if (offerta != null)
                {
                    Logic.OfferteAccountValidatori llOfferteAccountValidatori = new Logic.OfferteAccountValidatori(llOfferte);
                    Logic.Sicurezza.Accounts llAccounts = new Logic.Sicurezza.Accounts(llOfferte);

                    if (offerta.OffertaAccountValidatores.Count > 0)
                    {
                        llOfferteAccountValidatori.Delete(offerta.OffertaAccountValidatores, true);
                    }

                    List<Account> elencoAccount = new List<Account>();

                    foreach (Guid idAccount in elencoIdAccount)
                    {
                        Account account = llAccounts.Find(idAccount);
                        if (account == null) throw new Exception($"L'account con ID \"{idAccount}\" non esiste nella base dati");

                        elencoAccount.Add(account);

                        Entities.OffertaAccountValidatore validatore = llOfferteAccountValidatori.Find(offerta.Identifier, account.Identifier);
                        if (validatore == null)
                        {
                            OffertaAccountValidatore entity = new OffertaAccountValidatore();
                            entity.Offerta = offerta;
                            entity.Account = account;

                            llOfferteAccountValidatori.Create(entity, true);
                        }
                    }

                    offerta.StatoEnum = StatoOffertaEnum.InValidazione;
                    llOfferte.SubmitToDatabase();

                    if (elencoAccount.Count > 0)
                    {
                        EmailManager.InziaEmailRichiestaValidazioneOfferta(currentID);
                    }

                    RicaricaPagina();
                }
                else
                {
                    throw new Exception("Non è stato possibile recuperare le informazioni dell'offerta correte. Tornare all'elenco e riaprire l'offerta");
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        private void RicaricaPagina()
        {
            Helper.Web.ReloadPage(this, currentID.ToString());
        }

        private void ManageRichiediValidazione()
        {
            rfvValidatori.Enabled = true;
            rwRichiestaValidazione.VisibleOnPageLoad = true;
            FillComboValidatori();
        }

        private void ClearFinestraRichiestaValidazione()
        {
            rfvValidatori.Enabled = false;
            rbElencoValidatori.ClearCheckedItems();
            rbElencoValidatori.ClearSelection();
            rbElencoValidatori.DataSource = new Account[] { };
            rbElencoValidatori.DataBind();
        }

        private void FillComboValidatori()
        {
            Logic.Sicurezza.Accounts llAccounts = new Logic.Sicurezza.Accounts();
            rbElencoValidatori.DataSource = llAccounts.ReadValidatori();
            rbElencoValidatori.DataBind();
        }

        private void ManageValidazioneOffertaConEsitoPositivo()
        {
            try
            {
                Logic.Offerte llOfferte = new Logic.Offerte();
                Entities.Offerta offerta = llOfferte.Find(currentID);
                if (offerta != null)
                {
                    Logic.OfferteAccountValidatori llOfferteAccountValidatori = new Logic.OfferteAccountValidatori(llOfferte);
                    Entities.Account account = InformazioniAccountAutenticato.GetIstance().Account;

                    Entities.OffertaAccountValidatore validatore = llOfferteAccountValidatori.Find(offerta.Identifier, account.Identifier);
                    if (validatore != null)
                    {
                        validatore.EffettuatoValidazione = true;
                        validatore.DataOraValidazione = DateTime.Now;
                    }

                    offerta.StatoEnum = StatoOffertaEnum.ValidataConEsitoPositivo;
                    llOfferte.SubmitToDatabase();

                    Helper.Web.ReloadPage(this, offerta.ID.ToString());
                }
                else
                {
                    throw new Exception("Non è stato possibile recuperare le informazioni dell'offerta correte. Tornare all'elenco e riaprire l'offerta");
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        private void ManageValidazioneOffertaConEsitoNegativo()
        {
            try
            {
                Logic.Offerte llOfferte = new Logic.Offerte();
                Entities.Offerta offerta = llOfferte.Find(currentID);
                if (offerta != null)
                {
                    Logic.OfferteAccountValidatori llOfferteAccountValidatori = new Logic.OfferteAccountValidatori(llOfferte);
                    IQueryable<Entities.OffertaAccountValidatore> validatores = llOfferteAccountValidatori.Read(offerta.Identifier);

                    if (validatores.Any())
                    {
                        llOfferteAccountValidatori.Delete(validatores.ToArray(), false);
                    }

                    offerta.StatoEnum = StatoOffertaEnum.ValidataConEsitoNegativo;
                    llOfferte.SubmitToDatabase();

                    Helper.Web.ReloadPage(this, offerta.ID.ToString());
                }
                else
                {
                    throw new Exception("Non è stato possibile recuperare le informazioni dell'offerta correte. Tornare all'elenco e riaprire l'offerta");
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        private void ManageEsportaArticoli()
        {
            try
            {
                if (currentID != new EntityId<Offerta>(Guid.Empty))
                {
                    Logic.Offerte llOfferte = new Logic.Offerte();
                    Entities.Offerta offerta = llOfferte.Find(currentID);
                    if (offerta != null)
                    {
                        string nomeDocumento = $"Esportazione Articoli Offerta {offerta.Numero}";
                        Logic.DocumentiDaGenerare.GeneraEsportazioneArticoliOfferte llGeneraEsportazioneArticoliOfferte = new Logic.DocumentiDaGenerare.GeneraEsportazioneArticoliOfferte(currentID);

                        byte[] fileContent = null;
                        string tempFileName = String.Empty;

                        llGeneraEsportazioneArticoliOfferte.GeneraDocumento(out fileContent, out tempFileName, generatePdf: false);
                        string fileName = llGeneraEsportazioneArticoliOfferte.GeneraNomeFile(nomeDocumento, Path.GetExtension(tempFileName));
                        Helper.Web.DownloadAsFile(fileName, fileContent);
                    }
                    else
                    {
                        throw new Exception("Non è stato possibile recuperare le informazioni dell'offerta correte. Tornare all'elenco e riaprire l'offerta");
                    }
                }
                else
                {
                    throw new Exception("Non è stato possibile recuperare le informazioni dell'offerta correte. Tornare all'elenco e riaprire l'offerta");
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        private void ManageGeneraDocumento(string modelloDocumentoDaGenerare)
        {
            try
            {
                string percorsoCompletoModelloDaGenerare = Path.Combine(Infrastructure.ConfigurationKeys.PERCORSO_DIRECTORY_TEMPLATE_OFFERTE, modelloDocumentoDaGenerare);
                if (!File.Exists(percorsoCompletoModelloDaGenerare)) throw new FileNotFoundException($"Il file di template per generare l'offerta da inviare (\"{percorsoCompletoModelloDaGenerare}\") non esiste", percorsoCompletoModelloDaGenerare);

                
                if (currentID != new EntityId<Offerta>(Guid.Empty))
                {

                    Logic.DocumentiDaGenerare.GeneratoreOfferte llOfferte = new Logic.DocumentiDaGenerare.GeneratoreOfferte(currentID);

                    byte[] fileContent = null;
                    string tempFileName = String.Empty;

                    llOfferte.GeneraDocumento(percorsoCompletoModelloDaGenerare, out fileContent, out tempFileName, generatePdf: false);
                    string fileName = llOfferte.GeneraNomeFile(modelloDocumentoDaGenerare, Path.GetExtension(tempFileName));
                    Helper.Web.DownloadAsFile(fileName, fileContent);
                }
                else
                {
                    throw new Exception("Non è stato possibile recuperare le informazioni dell'offerta correte. Tornare all'elenco e riaprire l'offerta");
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        private void ManageCaricaDocumento()
        {
            rwCaricaDocumento.VisibleOnPageLoad = true;
            rfvTipologiaDocumento.Enabled = true;
            rcbTipologiaDocumento.DataSource = SeCoGes.Utilities.EnumHelper.GetDescriptionFromEnum<Entities.TipologiaAllegatoEnum, byte>();
            rcbTipologiaDocumento.DataBind();

            if (rcbTipologiaDocumento.Items.Count > 0) rcbTipologiaDocumento.Items[0].Selected = true;
        }

        protected void ucDocumentazioneCaricata_NeedDataSource(object sender, EventArgs e)
        {
            UI.DocumentazioneAllegata ucDocumentazioneAllegata = (UI.DocumentazioneAllegata)sender;
            ucDocumentazioneAllegata.DataSource = GetDocumentiCaricatiInOfferta();
        }

        private IQueryable<Entities.Allegato> GetDocumentiCaricatiInOfferta()
        {
            Logic.Allegati llAllegati = new Logic.Allegati();
            IQueryable<Entities.Allegato> elencoAllegatiCompleto = llAllegati.Read(currentID);

            // Su richiesta, nell'elenco dev'essere presente solo l'ultima versione dell'offerta caricata
            IQueryable<Entities.Allegato> elencoAllegatiOfferta = elencoAllegatiCompleto.Where(x => x.TipologiaAllegato == (byte)TipologiaAllegatoEnum.Offerta).Take(1);
            IQueryable<Entities.Allegato> elencoAllegatiGenerali = elencoAllegatiCompleto.Where(x => x.TipologiaAllegato != (byte)TipologiaAllegatoEnum.Offerta);

            return elencoAllegatiOfferta.Union(elencoAllegatiGenerali);
        }

        protected void btnCaricaDocumento_Click(object sender, EventArgs e)
        {
            rwCaricaDocumento.VisibleOnPageLoad = false;

            try
            {
                if (ruCaricamentoDocumento.UploadedFiles.Count <= 0) throw new Exception("Per proseguire è necessario caricare un documento");

                foreach (UploadedFile fileCaricato in ruCaricamentoDocumento.UploadedFiles)
                {
                    // Viene richiamato il metodo che effettua la memorizzazione del file corrente (nel ciclo)
                    MemorizzaFile(fileCaricato);
                }

                CheckVisibilitaPulsanteInvioAlCliente();
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
            finally
            {
                ClearFinestraCaricamentoDocumento();
            }
        }

        protected void btnAnnullaCaricamentoDocumento_Click(object sender, EventArgs e)
        {
            rwCaricaDocumento.VisibleOnPageLoad = false;
            rfvTipologiaDocumento.Enabled = false;
            ClearFinestraCaricamentoDocumento();
        }



        private void MemorizzaFile(UploadedFile fileCaricato)
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
            entityToCreate.ID = Guid.NewGuid();
            entityToCreate.NomeFile = Path.GetFileName(fileCaricato.FileName);
            entityToCreate.UserName = utenteCollegato.UserName;

            byte tipologiaDocumentoByte = Byte.Parse(rcbTipologiaDocumento.SelectedValue);
            entityToCreate.TipologiaAllegatoEnum = (Entities.TipologiaAllegatoEnum)tipologiaDocumentoByte;

            entityToCreate.Note = rtbNoteDocumentoDaCaricare.Text.Trim();
            entityToCreate.Compresso = false;
            entityToCreate.DataArchiviazione = DateTime.Now;

            // Viene letto il contenuto del file .. ed inserita nella property che memorizzerà il file nel database
            byte[] byteFileOriginale = new byte[fileCaricato.ContentLength];
            fileCaricato.InputStream.Read(byteFileOriginale, 0, (int)fileCaricato.ContentLength);
            entityToCreate.FileAllegato = byteFileOriginale;
            entityToCreate.IDLegame = currentID.Value;

            Logic.Allegati llAllegati = new Logic.Allegati();
            llAllegati.Create(entityToCreate, true);
        }

        private void ClearFinestraCaricamentoDocumento()
        {
            ruCaricamentoDocumento.UploadedFiles.Clear();
            rtbNoteDocumentoDaCaricare.Text = String.Empty;
            rcbTipologiaDocumento.ClearSelection();
            rcbTipologiaDocumento.Items.Clear();
            ucDocumentazioneCaricata.Inizializza();
        }

        protected void ucDocumentazioneCaricata_NeedDeleteAllegato(object sender, UI.DocumentoAllegatoDaSalvareEventArgs e)
        {
            try
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

                        CheckVisibilitaPulsanteInvioAlCliente();
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
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        private void CheckVisibilitaPulsanteInvioAlCliente()
        {
            bool esistonoDocumentiCaricati = GetDocumentiCaricatiInOfferta().Any();
            PulsanteToolbar_SeparatoreInvioOffertaAlCliente.Visible = esistonoDocumentiCaricati;
            PulsanteToolbar_InviaOffertaAlCliente.Visible = esistonoDocumentiCaricati;
        }

        private void ManageInviaOffertaAlCliente()
        {
            if (File.Exists(Infrastructure.ConfigurationKeys.PERCORSO_TEMPLATE_EMAIL_INVIO_OFFERTA_AL_CLIENTE))
            {
                rfvElencoDocumentiOffertaCaricati.Enabled = true;
                rfvIndirizzoEmailInvioOfferta.Enabled = true;
                cvIndirizzoEmailInvioOfferta.Enabled = true;
                rwInvioOffertaAlCliente.VisibleOnPageLoad = true;
                reTestoEmailDaInviareAlCliente.Content = File.ReadAllText(Infrastructure.ConfigurationKeys.PERCORSO_TEMPLATE_EMAIL_INVIO_OFFERTA_AL_CLIENTE);

                Logic.Offerte ll = new Logic.Offerte();
                Entities.Offerta entityToShow = ll.Find(currentID);
                if (entityToShow != null && !String.IsNullOrEmpty(entityToShow.CodiceCliente))
                {
                    Logic.Metodo.AnagraficheClienti llAnagraficheClienti = new Logic.Metodo.AnagraficheClienti(ll);
                    AnagraficaClienti anagraficaClienti = llAnagraficheClienti.Find(entityToShow.CodiceCliente);
                    if (anagraficaClienti != null)
                    {
                        tbIndirizzoEmailInvioOfferta.Text = anagraficaClienti.TELEX;
                    }
                }

                rcbDocumentiOffertaCaricati.DataSource = GetDocumentiCaricatiInOfferta();
                rcbDocumentiOffertaCaricati.DataBind();
            }
            else
            {
                MessageHelper.ShowErrorMessage(this, "Il file relativo al template dell'email per l'invio dell'offerta al cliente non esite.");
            }
        }

        private void ClearFinestraInviaOffertaAlCliente()
        {
            reTestoEmailDaInviareAlCliente.Content = String.Empty;
            tbIndirizzoEmailInvioOfferta.Text = String.Empty;
            rfvElencoDocumentiOffertaCaricati.Enabled = false;
            rfvIndirizzoEmailInvioOfferta.Enabled = false;
            cvIndirizzoEmailInvioOfferta.Enabled = false;
            rcbDocumentiOffertaCaricati.DataSource = new List<Entities.Allegato>();
            rcbDocumentiOffertaCaricati.DataBind();
        }

        protected void btnInviaOffertaAlCliente_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(reTestoEmailDaInviareAlCliente.Content)) throw new Exception("Per procedere è necessario inserire il testo del messaggio da inviare al cliente");
                if (rcbDocumentiOffertaCaricati.CheckedItems.Count <= 0) throw new Exception("Per procedere è necessario selezionare almeno un documento caricato nell'offerta");

                Page.Validate(btnInviaOffertaAlCliente.ValidationGroup);
                if (!Page.IsValid)
                {
                    MessagesCollector messaggi = new MessagesCollector();

                    foreach (IValidator validatore in Page.GetValidators(btnInviaOffertaAlCliente.ValidationGroup))
                    {

                        if (!validatore.IsValid)
                        {
                            messaggi.Add(validatore.ErrorMessage);
                        }
                    }

                    if (messaggi.HaveMessages)
                    {
                        MessageHelper.ShowErrorMessage(this, messaggi.ToString("<br/>"));
                        return;
                    }
                }

                Logic.Allegati llAllegati = new Logic.Allegati();
                List<Entities.Allegato> elencoAllegatiSelezionati = new List<Entities.Allegato>();

                foreach (RadComboBoxItem elementoSelezionato in rcbDocumentiOffertaCaricati.CheckedItems)
                {
                    if (Guid.TryParse(elementoSelezionato.Value, out Guid idAllegato))
                    {
                        Entities.Allegato allegato = llAllegati.Find(idAllegato);
                        if (allegato == null) throw new Exception("L'allegato selezionato non esiste nella base dati. Ricaricare la pagina per avere i dati aggiornati.");
                        else elencoAllegatiSelezionati.Add(allegato);
                    }
                }

                Logic.Offerte llOfferte = new Logic.Offerte();
                Entities.Offerta offerta = llOfferte.Find(currentID);
                if (offerta != null)
                {
                    List<MemoryStream> elencoStreamDatiDaChiudere = new List<MemoryStream>();

                    using (System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(Infrastructure.ConfigurationKeys.MITTENTE_EMAIL_OFFERTA, tbIndirizzoEmailInvioOfferta.Text))
                    {
                        message.IsBodyHtml = true;
                        message.Body = reTestoEmailDaInviareAlCliente.Content;
                        message.Subject = $"Invio offerta N.{offerta.Numero} - {offerta.Titolo}";

                        if (elencoAllegatiSelezionati.Count > 0)
                        {
                            foreach (Entities.Allegato allegato in elencoAllegatiSelezionati)
                            {
                                MemoryStream ms = new MemoryStream(allegato.FileAllegato.ToArray());
                                elencoStreamDatiDaChiudere.Add(ms);
                                message.Attachments.Add(new System.Net.Mail.Attachment(ms, allegato.NomeFile));
                            }
                        }

                        System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                        smtpClient.Send(message);
                    }

                    if (elencoStreamDatiDaChiudere.Count > 0)
                    {
                        foreach (MemoryStream ms in elencoStreamDatiDaChiudere)
                        {
                            if (ms != null)
                            {
                                ms.Dispose();
                            }
                        }

                        elencoStreamDatiDaChiudere.Clear();
                    }

                    offerta.TestoEmailInviataAlCliente = reTestoEmailDaInviareAlCliente.Content;
                    offerta.StatoEnum = StatoOffertaEnum.InviataAlCliente;
                    llOfferte.SubmitToDatabase();

                    rwInvioOffertaAlCliente.VisibleOnPageLoad = false;
                    ClearFinestraInviaOffertaAlCliente();

                    RicaricaPagina();
                }
                else
                {
                    throw new Exception("Non è stato possibile recuperare le informazioni dell'offerta correte. Tornare all'elenco e riaprire l'offerta");
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        protected void btnAnnullaInvioOffertaAlCliente_Click(object sender, EventArgs e)
        {
            rwInvioOffertaAlCliente.VisibleOnPageLoad = false;
            ClearFinestraInviaOffertaAlCliente();
        }

        private void ManageConfermaOffertaDaParteDelCliente()
        {
            try
            {
                Logic.Offerte llOfferte = new Logic.Offerte();
                Entities.Offerta offerta = llOfferte.Find(currentID);
                if (offerta != null)
                {
                    offerta.StatoEnum = StatoOffertaEnum.AccettataDalCliente;
                    llOfferte.SubmitToDatabase();

                    Helper.Web.ReloadPage(this, offerta.ID.ToString());
                }
                else
                {
                    throw new Exception("Non è stato possibile recuperare le informazioni dell'offerta correte. Tornare all'elenco e riaprire l'offerta");
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        private void ManageRifiutoOffertaDaParteDelCliente()
        {
            try
            {
                Logic.Offerte llOfferte = new Logic.Offerte();
                Entities.Offerta offerta = llOfferte.Find(currentID);
                if (offerta != null)
                {
                    offerta.StatoEnum = StatoOffertaEnum.RifiutataDalCliente;
                    llOfferte.SubmitToDatabase();

                    Helper.Web.ReloadPage(this, offerta.ID.ToString());
                }
                else
                {
                    throw new Exception("Non è stato possibile recuperare le informazioni dell'offerta correte. Tornare all'elenco e riaprire l'offerta");
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        protected void rapMetodiDiPagamento_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(e.Argument) && e.Argument.Trim().Equals(COMANDO_AGGIORNAMENTO_METODI_PAGAMENTO.Trim(), StringComparison.InvariantCultureIgnoreCase))
            {
                ColonnaIban.Visible = false;
                rcbCodiceIban.Text = String.Empty;
                rcbCodiceIban.Items.Clear();
                rcbCodiceIban.ClearSelection();

                FillMetodiDiPagamento(String.Empty, null);
            }
        }

        protected void rcbMetodiDiPagamento_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (PagamentoRichiedeIban(e.Value))
            {
                ColonnaIban.Visible = true;
                FillElencoIban(rcbCliente.SelectedValue, String.Empty, null);
            }
            else
            {
                ColonnaIban.Visible = false;
                rcbCodiceIban.Text = String.Empty;
                rcbCodiceIban.Items.Clear();
                rcbCodiceIban.ClearSelection();
            }
        }

        private void ManageCreaRevisione()
        {
            try
            {
                Logic.Offerte llOfferte = new Logic.Offerte();
                Entities.Offerta offerta = llOfferte.Find(currentID);
                if (offerta != null)
                {
                    Entities.Offerta offertaClonata = llOfferte.CreateRevision(offerta, true);

                    Helper.Web.ReloadPage(this, offertaClonata.ID.ToString());
                }
                else
                {
                    throw new Exception("Non è stato possibile recuperare le informazioni dell'offerta correte. Tornare all'elenco e riaprire l'offerta");
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        private void ManageClonaOfferta()
        {
            try
            {
                Logic.Offerte llOfferte = new Logic.Offerte();
                Entities.Offerta offerta = llOfferte.Find(currentID);
                if (offerta != null)
                {
                    Entities.Offerta offertaClonata = llOfferte.Clone(offerta, true);

                    Helper.Web.ReloadPage(this, offertaClonata.ID.ToString());
                }
                else
                {
                    throw new Exception("Non è stato possibile recuperare le informazioni dell'offerta correte. Tornare all'elenco e riaprire l'offerta");
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        /// <summary>
        /// Effettua il popolamento dei tasti necessari per la generazione del documento relativo all'offerta
        /// </summary>
        private void PopolaAggiungiTemplateOffertaPerGenerazioneDocumento()
        {
            if (PulsanteToolbar_GeneraDocumento != null && PulsanteToolbar_GeneraDocumento.Visible)
            {
                string[] elencoNomiTemplateOffertePerGenerazioneDocumento = GetElencoNomiTemplateOffertePerGenerazioneDocumento();
                if ((elencoNomiTemplateOffertePerGenerazioneDocumento?.Length ?? 0) > 0)
                {                    
                    foreach (string nomeTemplateOfferta in elencoNomiTemplateOffertePerGenerazioneDocumento)
                    {
                        RadToolBarButton tastoDocumento = new RadToolBarButton(nomeTemplateOfferta) { Value = nomeTemplateOfferta, CommandName = "GeneraDocumento" };
                        PulsanteToolbar_GeneraDocumento.Buttons.Add(tastoDocumento);
                    }
                }
            }
        }

        /// <summary>
        /// Restituisce l'elenco dei template delle offerte per la generazione del documento
        /// </summary>
        /// <returns></returns>
        private string[] GetElencoNomiTemplateOffertePerGenerazioneDocumento()
        {
            List<string> elencoNomiModelli = new List<string>();

            if (Directory.Exists(Infrastructure.ConfigurationKeys.PERCORSO_DIRECTORY_TEMPLATE_OFFERTE))
            {
                string[] percorsoCompletoTemplates = Directory.GetFiles(Infrastructure.ConfigurationKeys.PERCORSO_DIRECTORY_TEMPLATE_OFFERTE);
                if (percorsoCompletoTemplates != null && percorsoCompletoTemplates.Length > 0)
                {
                    foreach(string percorsoCompletoFile in percorsoCompletoTemplates)
                    {
                        string nomeFile = Path.GetFileName(percorsoCompletoFile);
                        elencoNomiModelli.Add(nomeFile);
                    }
                }
            }

            return elencoNomiModelli.OrderBy(x => x).ToArray();
        }

        /// <summary>
        /// Effettua il popolamento del combo relativo alle tipologia dei tempi di consegna
        /// </summary>
        private void FillTipologiaTempiDiConsegna()
        {
            rcbTipologiaTempiDiConsegna.DataSource = SeCoGes.Utilities.EnumHelper.GetDescriptionFromEnum<TipologiaGiornateEnum, int>();
            rcbTipologiaTempiDiConsegna.DataBind();
        }

        /// <summary>
        /// Effettua il popolamento del combo relativo alle tipologia dei giorni di validità dell'offerta
        /// </summary>
        private void FillTipologiaGiorniValidita()
        {
            rcbTipologiaGiorniValidita.DataSource = SeCoGes.Utilities.EnumHelper.GetDescriptionFromEnum<TipologiaGiornateEnum, int>();
            rcbTipologiaGiorniValidita.DataBind();
        }

        /// <summary>
        /// Effettua il riempimento della lista che contiene le opzioni di stampa del gruppo dell'offerta
        /// </summary>
        /// <param name="rcblOpzioniGruppo"></param>
        /// <param name="offertaGruppamento"></param>
        private void FillListOpzioniStampaOffertaGruppo(RadCheckBoxList rcblOpzioniGruppo, OffertaRaggruppamento offertaGruppamento)
        {
            List<object> dataSource = new List<object>();

            Dictionary<int, string> chiaviValori = SeCoGes.Utilities.EnumHelper.GetDescriptionFromEnum<OffertaRaggruppamentoOpzioneStampaOffertaEnum, int>(true);
            if (chiaviValori != null)
            {
                int chiaveNessunaOpzioneDaEliminare = (int)OffertaRaggruppamentoOpzioneStampaOffertaEnum.NessunaOpzione;
                if (chiaviValori.ContainsKey(chiaveNessunaOpzioneDaEliminare)) chiaviValori.Remove(chiaveNessunaOpzioneDaEliminare);

                int chiaveTutteLeOpzioniDaEliminare = (int)OffertaRaggruppamentoOpzioneStampaOffertaEnum.TutteLeOpzioni;
                if (chiaviValori.ContainsKey(chiaveTutteLeOpzioniDaEliminare)) chiaviValori.Remove(chiaveTutteLeOpzioniDaEliminare);

                if (chiaviValori.Count > 0)
                {
                    foreach (KeyValuePair<int, string> chiaveValore in chiaviValori)
                    {
                        bool selectedValue = offertaGruppamento.OpzioneStampaOffertaEnum.HasFlag((OffertaRaggruppamentoOpzioneStampaOffertaEnum)chiaveValore.Key);
                        dataSource.Add(new { Selected = selectedValue, Text = chiaveValore.Value, Value = chiaveValore.Key });
                    }
                }
            }

            rcblOpzioniGruppo.DataSource = dataSource;
            rcblOpzioniGruppo.DataBind();
        }

        /// <summary>
        /// Effettua il caricamento delle opzioni relative alla stampa del gruppo nell'offerta
        /// </summary>
        /// <param name="llOferteRaggruppamenti"></param>
        /// <param name="saveUndoChangeButton"></param>
        /// <param name="rcblOpzioniStampaOffertaGruppo"></param>
        private void CaricaOpzioniStampaOffertaGruppo(Logic.OfferteRaggruppamenti llOferteRaggruppamenti, Guid idOffertaGruppamento, RadCheckBoxList rcblOpzioniStampaOffertaGruppo)
        {
            if (idOffertaGruppamento != Guid.Empty && rcblOpzioniStampaOffertaGruppo != null)
            {
                if (llOferteRaggruppamenti == null) llOferteRaggruppamenti = new Logic.OfferteRaggruppamenti();

                Entities.OffertaRaggruppamento offertaRaggruppamento = llOferteRaggruppamenti.Find(new EntityId<OffertaRaggruppamento>(idOffertaGruppamento));
                if (offertaRaggruppamento != null)
                {
                    FillListOpzioniStampaOffertaGruppo(rcblOpzioniStampaOffertaGruppo, offertaRaggruppamento);
                }
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento click relativo al tasto di salvataggio delle opzioni di stampa del gruppo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btSalvaModificheOpzioniGruppo_Click(object sender, EventArgs e)
        {
            if (sender is Button button && Guid.TryParse(button.CommandArgument, out Guid idOffertaGruppamento))
            {
                RadCheckBoxList rcblOpzioniStampaOffertaGruppo = GetControlloListaByButton(button);
                string[] values = rcblOpzioniStampaOffertaGruppo.SelectedValues;

                OffertaRaggruppamentoOpzioneStampaOffertaEnum valoreDaMemorizzare = OffertaRaggruppamentoOpzioneStampaOffertaEnum.NessunaOpzione;

                if (values != null && values.Length > 0)
                {
                    for(int i = 0; i < values.Length; i++)
                    {
                        if (int.TryParse(values[i], out int intValue))
                        {
                            if (i == 0)
                            {
                                valoreDaMemorizzare = (OffertaRaggruppamentoOpzioneStampaOffertaEnum)intValue;
                            }
                            else
                            {
                                valoreDaMemorizzare |= (OffertaRaggruppamentoOpzioneStampaOffertaEnum)intValue;
                            }
                        }
                    }
                }

                Logic.OfferteRaggruppamenti llOferteRaggruppamenti = new Logic.OfferteRaggruppamenti();
                Entities.OffertaRaggruppamento offertaRaggruppamento = llOferteRaggruppamenti.Find(new EntityId<OffertaRaggruppamento>(idOffertaGruppamento));
                if (offertaRaggruppamento != null)
                {
                    offertaRaggruppamento.OpzioneStampaOffertaEnum = valoreDaMemorizzare;
                    llOferteRaggruppamenti.SubmitToDatabase();

                    CaricaOpzioniStampaOffertaGruppo(llOferteRaggruppamenti, idOffertaGruppamento, rcblOpzioniStampaOffertaGruppo);
                }
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento click relativo al tasto di annullamento delle modifiche alle opzioni di stampa del gruppo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btAnnullaModificheOpzioniGruppo_Click(object sender, EventArgs e)
        {
            if (sender is Button button && Guid.TryParse(button.CommandArgument, out Guid idOffertaGruppamento))
            {
                RadCheckBoxList rcblOpzioniStampaOffertaGruppo = GetControlloListaByButton(button);
                CaricaOpzioniStampaOffertaGruppo(null, idOffertaGruppamento, rcblOpzioniStampaOffertaGruppo);
            }
        }

        private RadCheckBoxList GetControlloListaByButton(Button button)
        {
            RadCheckBoxList controlToreturn = null;
            Control parentRowsControl = button?.Parent?.Parent?.Parent;
            if (parentRowsControl != null)
            {
                controlToreturn = parentRowsControl.FindControl("rcblOpzioniGruppo") as RadCheckBoxList;
            }

            return controlToreturn;
        }
    }
}