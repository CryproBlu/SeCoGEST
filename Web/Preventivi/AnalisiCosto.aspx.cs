using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using SeCoGes.Utilities;
using SeCoGEST.Entities;
using SeCoGEST.Web.UI;
using Telerik.Web.UI;

namespace SeCoGEST.Web.Preventivi
{
    public partial class AnalisiCosto : System.Web.UI.Page
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
        protected EntityId<Entities.AnalisiCosto> currentID
        {
            get
            {
                if (Request.QueryString["ID"] != null)
                    return new EntityId<Entities.AnalisiCosto>(Request.QueryString["ID"]);
                else
                    return EntityId<Entities.AnalisiCosto>.Empty;
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

        #endregion

        #region Intercettazione Eventi

        protected void Page_Load(object sender, EventArgs e)
        {
            CaricaAutorizzazioni();

            if (!Helper.Web.IsPostOrCallBack(this))
            {
                try
                {
                    SeCoGes.Logging.LogManager.AddLogAccessi(String.Format("Accesso alla pagina '{0}'.", Request.Url.AbsolutePath));

                    Logic.AnalisiCosti ll = new Logic.AnalisiCosti();
                    Entities.AnalisiCosto entityToShow = ll.Find(currentID);


                    if (entityToShow != null)
                    {
                        ShowData(entityToShow);
                    }
                    else
                    {
                        ShowData(new Entities.AnalisiCosto());
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

        #endregion

        #region Metodi di gestione

        /// <summary>
        /// Mostra nell'interfaccia i dati dell'entity passata
        /// </summary>
        /// <param name="entity"></param>
        private void ShowData(Entities.AnalisiCosto entityToShow)
        {
            ApplicaRedirectPulsanteAggiorna();

            if (entityToShow == null)
            {
                throw new ArgumentNullException("Parametro nullo", "entityToShow");
            }

            //if (entityToShow.Chiuso.HasValue && entityToShow.Chiuso.Value == true)
            //{
            //    this.Enabled = false;
            //}

            if (!entityToShow.ID.Equals(Guid.Empty))
            {
                lblTitolo.Text = string.Format("Analisi Costi N.{0}", entityToShow.Numero);

                rowRaggruppamenti.Visible = true;


                //repRaggruppamenti.Items.Clear();

                Logic.AnalisiCostiRaggruppamenti llRag = new Logic.AnalisiCostiRaggruppamenti();
                IQueryable<Entities.AnalisiCostoRaggruppamento> gruppi = llRag.Read(entityToShow);

                repRaggruppamenti.DataSource = gruppi;
                repRaggruppamenti.DataBind();

                rigaTotaliGlobali.Visible = gruppi.Any() && gruppi.Any(g => g.AnalisiCostoArticolos.Any());

                //IQueryable<Entities.AnalisiCostoRaggruppamento> gruppi = llRag.Read(entityToShow);
                ////foreach(AnalisiCostoRaggruppamento g in gruppi)
                ////{
                ////    AddPanelBar(g);
                ////}



                //repRaggruppamenti.DataSource = gruppi;
                //repRaggruppamenti.DataBind();

                //foreach (RadPanelItem item in RaggruppamentiPanel.Items)
                //{
                //    item.HeaderTemplate = new HeaderTemplateClass();
                //    item.ApplyHeaderTemplate();
                //    item.DataBind();
                //}

                //for (int i = 0; i < RaggruppamentiPanel.Items.Count; i++)
                //RaggruppamentiPanel.Items[i].DataBind();



            }
            else
            {
                lblTitolo.Text = "Nuova Analisi Costi";

                Logic.AnalisiCosti ll = new Logic.AnalisiCosti();
                entityToShow.Numero = ll.GetNuovoNumero();
                entityToShow.NumeroRevisione = 0;
                entityToShow.Data = DateTime.Now;
                rigaTotaliGlobali.Visible = false;
            }

            this.Title = lblTitolo.Text;

            rntbNumero.Value = entityToShow.Numero;
            rtbNumeroRevisione.Text = entityToShow.NumeroRevisione.ToString();
            rdtpDataRedazione.SelectedDate = entityToShow.Data;
            rtbTitoloAnalisi.Text = entityToShow.Titolo;

            rntbTotaleCostoGlobale.Value = (double?)(entityToShow.TotaleCosto ?? 0);
            rntbTotaleCostoGlobale.DisplayText = String.Format("{0:c}", entityToShow.TotaleCosto ?? 0);

            rntbTotaleVenditaGlobale.Value = (double?)(entityToShow.TotaleVendita ?? 0);
            rntbTotaleVenditaGlobale.DisplayText = String.Format("{0:c}", entityToShow.TotaleVendita ?? 0);

            rntbTotaleRedditivitaGlobale.Value = (double?)(entityToShow.TotaleRicaricoValuta ?? 0);
            rntbTotaleRedditivitaGlobale.DisplayText = String.Format("{0:c} ({1}%)", entityToShow.TotaleRicaricoValuta ?? 0, entityToShow.TotaleRicaricoPercentuale ?? 0);
        }


        //private void AddPanelBar(AnalisiCostoRaggruppamento g)
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
            Entities.AnalisiCosto entityToSave = null;

            // Definisco una variabile che conterrà l'Codice dell'entity salvata. 
            // Se per qualche motivo l'entity non viene salvata allora entityId rimarrà String.Empty
            string entityId = String.Empty;
            Logic.AnalisiCosti ll = new Logic.AnalisiCosti();
            Logic.AnalisiCostiRaggruppamenti llRagg = new Logic.AnalisiCostiRaggruppamenti(ll);

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
                            "Il documento di analisi che si sta variando non esiste più in archivio."
                                + "\n\rOperazione annullata!");
                    }

                    // Al contrario, se entityToSave è nulla e CurrentId è vuota
                    // vuol dire che la pagina è stata aperta per la creazione di una nuova entità
                    nuovo = true;
                    entityToSave = new Entities.AnalisiCosto();

                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriDallaView(entityToSave, ll);


                    //Creo nuova entity
                    ll.Create(entityToSave, true);

                    // Alla prima creazione viene sempre aggiunto un primo raggruppamento
                    Entities.AnalisiCostoRaggruppamento gruppo = new AnalisiCostoRaggruppamento();
                    gruppo.AnalisiCosto = entityToSave;
                    gruppo.Ordine = 0;
                    gruppo.Denominazione = "Gruppo 1";

                    
                    llRagg.Create(gruppo, true);
                }
                else
                {
                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriDallaView(entityToSave, ll);

                    foreach(RepeaterItem groupHeader in repRaggruppamenti.Items)
                    {
                        AnalisiCostoRaggruppamentoHeader headerRaggruppamento = (AnalisiCostoRaggruppamentoHeader)groupHeader.FindControl("HeaderRaggruppamento");
                        HiddenField hd = (HiddenField)groupHeader.FindControl("hdIdRaggruppamento");
                        AnalisiCostoRaggruppamento acr = llRagg.Find(new EntityId<AnalisiCostoRaggruppamento>(new Guid(hd.Value)));
                        acr.Denominazione = headerRaggruppamento.GetDenominazioneGruppo();
                        llRagg.SubmitToDatabase();
                    }
                }


                // Persisto le modifiche sulla base dati nella transazione
                ll.SubmitToDatabase();

                // Persisto le modifiche sulla base dati effettuando il commit delle modifiche apportate nella transazione
                ll.CommitTransaction();

                SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - {1} l'entity Analisi Costo del {2:d} con Titolo '{3}'.", Request.Url.AbsolutePath, ((nuovo) ? "Creato" : "Salvato"), entityToSave.Data, entityToSave.Titolo));

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


        /// <summary>
        /// Restituisce un oggetto contenente gli eventuali errori di validazione dei dati
        /// </summary>
        /// <returns></returns>
        private MessagesCollector ValidaDati()
        {
            MessagesCollector messaggi = new MessagesCollector();

            //Errori dei validatori client
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

            if (!rdtpDataRedazione.SelectedDate.HasValue)
            {
                messaggi.Add(rfvDataRedazione.ErrorMessage);
            }

            if (rtbTitoloAnalisi.Text.Trim() == string.Empty)
            {
                messaggi.Add(rfvTitoloAnalisi.ErrorMessage);
            }

            return messaggi;
        }

        /// <summary>
        /// Legge i dati inseriti nell'interfaccia e li inserisce nell'entity passata
        /// </summary>
        /// <param name="entityToFill"></param>
        public void EstraiValoriDallaView(Entities.AnalisiCosto entityToFill, Logic.AnalisiCosti logic)
        {
            if (entityToFill == null)
            {
                throw new ArgumentNullException("Parametro nullo", "entityToFill");
            }

            entityToFill.Data = rdtpDataRedazione.SelectedDate.Value;
            entityToFill.Titolo = rtbTitoloAnalisi.Text.Trim();

            entityToFill.TotaleCosto = (decimal?)((rntbTotaleCostoGlobale.Value ?? (double?)null));
            entityToFill.TotaleVendita = (decimal?)((rntbTotaleVenditaGlobale.Value ?? (double?)null));
            entityToFill.TotaleRicaricoValuta = (decimal?)((rntbTotaleRedditivitaGlobale.Value ?? (double?)null));
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
            PulsanteToolbar_SeparatoreSalva.Visible = enabled;
            PulsanteToolbar_Salva.Visible = enabled;

            rntbNumero.Enabled = enabled;
            rdtpDataRedazione.Enabled = enabled;
            rtbTitoloAnalisi.Enabled = enabled;
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
            AnalisiCostoRaggruppamentoEditItem EditItemRaggruppamento = (AnalisiCostoRaggruppamentoEditItem)e.Item.FindControl("EditItemRaggruppamento");
            RadGrid rgGrigliaArticoli = (RadGrid)e.Item.FindControl("rgGrigliaArticoli");
            HiddenField hdIdArticoloInEdit = (HiddenField)e.Item.FindControl("hdIdArticoloInEdit");
            LayoutRow rigaTotaliGruppo = (LayoutRow)e.Item.FindControl("rigaTotaliGruppo");

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
                    EditItemRaggruppamento.Inizializza();
                    rgGrigliaArticoli.Visible = false;
                    rigaTotaliGruppo.Visible = false;
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
                    break;

                case "AnnullaEdit":
                case "AnnullaModificheGruppo":
                    hdIdArticoloInEdit.Value = string.Empty;
                    btAggiungiArticolo.Visible = true;
                    btSalvaArticolo.Visible = false;
                    btAggiornaArticolo.Visible = false;
                    btAnnullaEdit.Visible = false;
                    btClonaGruppo.Visible = true;
                    EditItemRaggruppamento.Visible = false;
                    rgGrigliaArticoli.Visible = true;
                    rigaTotaliGruppo.Visible = true;
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
        //            Logic.AnalisiCostiArticoli llArt = new Logic.AnalisiCostiArticoli();
        //            rgGrigliaArticoli.DataSource = llArt.Read((AnalisiCostoRaggruppamento)e.Item.DataItem);
        //            //rgGrigliaArticoli.DataBind();
        //        }
        //    }
        //}



        private void SalvaArticolo(AnalisiCostoRaggruppamentoEditItem editItemRaggruppamento, RepeaterItem repeaterItem, Guid idGruppo, Guid idArticolo)
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
                Button btEsecuzioneConfigurazioniArticoliAggiuntivi = (Button)repeaterItem.FindControl("btEsecuzioneConfigurazioniArticoliAggiuntivi");
                PageMessage pmMessaggioConfigurazioneAggiuntiva = (PageMessage)repeaterItem.FindControl("pmMessaggioConfigurazioneAggiuntiva");
                

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




                // Salvataggio Articolo
                // Se viene passato l'ID del Gruppo si tratta di un inserimenti di un nuovo articolo 
                // altrimenti se viene passato l'ID dell'Articolo si tratta di un Aggiornamento di un articolo esistente
                AnalisiCostoArticolo articolo;
                Logic.AnalisiCostiArticoli llArt = new Logic.AnalisiCostiArticoli();
                if (idGruppo != Guid.Empty && idArticolo == Guid.Empty)
                {
                    articolo = new AnalisiCostoArticolo();
                    articolo.IDRaggruppamento = idGruppo;
                }
                else
                {
                    articolo = llArt.Find(new Entities.EntityId<Entities.AnalisiCostoArticolo>(idArticolo));
                }
                //AnalisiCostoArticolo articolo = new AnalisiCostoArticolo();
                articolo.CodiceGruppo = codiceGruppo;
                articolo.CodiceCategoria = codiceCategoria;
                articolo.CodiceCategoriaStatistica = codiceCategoriaStatistica;
                articolo.CodiceArticolo = rcbCodiceArticolo.SelectedValue;
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
                    Logic.AnalisiCostiArticoloCampiAggiuntivi llArtCampiAgg = new Logic.AnalisiCostiArticoloCampiAggiuntivi(llArt);

                    int ordine = -1;
                    Repeater repCampiAggiuntivi = (Repeater)editItemRaggruppamento.FindControl("repCampiAggiuntivi");
                    foreach(RepeaterItem item in repCampiAggiuntivi.Items)
                    {
                        ordine++;
                        Entities.AnalisiCostoArticoloCampoAggiuntivo campoAggiuntivo = new AnalisiCostoArticoloCampoAggiuntivo();
                        campoAggiuntivo.IDAnalisiCostoArticolo = articolo.ID;
                        campoAggiuntivo.Ordine = ordine;
                    
                        Label caName = (Label)item.FindControl("lblNomeCampoAggiuntivo");
                        TextBox caValue = (TextBox)item.FindControl("txtValoreCampoAggiuntivo");
                        campoAggiuntivo.NomeCampo = caName.Text;
                        campoAggiuntivo.TipoCampo = "";
                        campoAggiuntivo.Valore = caValue.Text;

                        llArtCampiAgg.Create(campoAggiuntivo, false);
                    }


                    // Persiste tutte le modifiche su database
                    llArt.SubmitToDatabase();

                }
                else
                {
                    llArt.SubmitToDatabase();


                    // Aggiornamento valori campi aggiuntivi
                    Logic.AnalisiCostiArticoloCampiAggiuntivi llArtCampiAgg = new Logic.AnalisiCostiArticoloCampiAggiuntivi(llArt);

                    Repeater repCampiAggiuntivi = (Repeater)editItemRaggruppamento.FindControl("repCampiAggiuntivi");
                    foreach (RepeaterItem item in repCampiAggiuntivi.Items)
                    {
                        HiddenField hdIdCampoAgg = (HiddenField)item.FindControl("hdIdCampoAggiuntivo");

                        Entities.AnalisiCostoArticoloCampoAggiuntivo campoAggiuntivo = llArtCampiAgg.Find(new EntityId<AnalisiCostoArticoloCampoAggiuntivo>(new Guid(hdIdCampoAgg.Value)));
                        TextBox caValue = (TextBox)item.FindControl("txtValoreCampoAggiuntivo");
                        campoAggiuntivo.Valore = caValue.Text;

                        llArtCampiAgg.SubmitToDatabase();
                    }
                }

                Logic.AnalisiCosti llAnalisiCosti = new Logic.AnalisiCosti(llArt);
                llAnalisiCosti.RicalcolaTotali(articolo.AnalisiCostoRaggruppamento.IDAnalisiCosto);

                Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi analisiVenditeConfigurazioneArticoliAggiuntivi = new Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi(llArt);

                IQueryable<AnalisiVenditaConfigurazioneArticoloAggiuntivo> queryConfigurazioneArticoliAggiuntivi = analisiVenditeConfigurazioneArticoliAggiuntivi.Read(rcbCodiceArticolo.SelectedValue, codiceGruppo, codiceCategoria, codiceCategoriaStatistica);

                pmMessaggioConfigurazioneAggiuntiva.Message = String.Empty;
                pmMessaggioConfigurazioneAggiuntiva.Visible = false;
                btEsecuzioneConfigurazioniArticoliAggiuntivi.Visible = false;

                if (queryConfigurazioneArticoliAggiuntivi.Any())
                {
                    AnalisiVenditaConfigurazioneArticoloAggiuntivo[] configurazioniMessaggi = queryConfigurazioneArticoliAggiuntivi.Where(x => x.Tipologia == (byte)TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.MostrareMessaggio || x.Tipologia == (byte)TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.MostrareMessaggio_e_AggiungereArticolo).ToArray();
                    pmMessaggioConfigurazioneAggiuntiva.Visible = configurazioniMessaggi?.Length > 0;

                    if (pmMessaggioConfigurazioneAggiuntiva.Visible)
                    {
                        string messaggio = $"<ul><li>{String.Join("</li><li>", configurazioniMessaggi.Select(x => x.TestoAvviso))}</li></ul>";

                        pmMessaggioConfigurazioneAggiuntiva.Message = messaggio;
                    }

                    // Correggere nella visualizzazione del tasto di esecuzione delle configurazioni aggiuntive
                    AnalisiVenditaConfigurazioneArticoloAggiuntivo[] configurazioniAggiuntaArticoli = queryConfigurazioneArticoliAggiuntivi.Where(x => x.Tipologia == (byte)TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.AggiungereArticolo || x.Tipologia == (byte)TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo.MostrareMessaggio_e_AggiungereArticolo).ToArray();

                    btEsecuzioneConfigurazioniArticoliAggiuntivi.Visible = configurazioniAggiuntaArticoli?.Length > 0;

                    if (btEsecuzioneConfigurazioniArticoliAggiuntivi.Visible)
                    {
                        //foreach(AnalisiVenditaConfigurazioneArticoloAggiuntivo configurazioneArticolo in configurazioniAggiuntaArticoli)
                        //{
                        //    AnalisiCostoArticolo articoloAggiuntivo = new AnalisiCostoArticolo();
                        //    articoloAggiuntivo.IDRaggruppamento = idGruppo;
                        //    articoloAggiuntivo.Descrizione = configurazioneArticolo.CodiceArticoloOut;
                        //    articoloAggiuntivo.CodiceGruppo = configurazioneArticolo.CodiceGruppoOut;
                        //    articoloAggiuntivo.CodiceCategoria = configurazioneArticolo.CodiceCategoriaOut;
                        //    articoloAggiuntivo.CodiceCategoriaStatistica = configurazioneArticolo.CodiceCategoriaStatisticaOut;
                        //    articoloAggiuntivo.CodiceArticolo = configurazioneArticolo.CodiceArticoloOut;

                        //    llArt.Create(articoloAggiuntivo, true);
                        //}

                        string elencoIdConfigurazioniAggiuntaArticoli = String.Join("###", configurazioniAggiuntaArticoli.Select(x => x.ID.ToString()));

                        btEsecuzioneConfigurazioniArticoliAggiuntivi.CommandArgument = $"{idGruppo}@@@{elencoIdConfigurazioniAggiuntaArticoli}";

                        //Helper.Web.ReloadPage(this, currentID.ToString());
                    }
                }
                else
                {
                    
                    Helper.Web.ReloadPage(this, currentID.ToString());
                }

                

                //Helper.Web.ReloadPage(this, currentID.ToString());

                //// Visualizza il nuovo articolo nella griglia
                //RadGrid rgGrigliaArticoli = (RadGrid)repeaterItem.FindControl("rgGrigliaArticoli");
                //if (rgGrigliaArticoli != null)
                //{
                //    Logic.AnalisiCostiRaggruppamenti llRag = new Logic.AnalisiCostiRaggruppamenti(llArt);
                //    AnalisiCostoRaggruppamento gruppo = llRag.Find(new EntityId<AnalisiCostoRaggruppamento>(idGruppo));

                //    rgGrigliaArticoli.DataSource = llArt.Read(gruppo);
                //    rgGrigliaArticoli.DataBind();
                //}


            }
            catch (Exception ex)
            {
                // TODO manage exception!!!!

                MessageHelper.ShowErrorMessage(Page, ex);
            }
        }

        protected void btEsecuzioneConfigurazioniArticoliAggiuntivi_Click(object sender, EventArgs e)
        {
            try
            {
                Button button = sender as Button;
                if (button != null)
                {
                    string[] identificativi = button.CommandArgument.Split(new string[] { "@@@" }, StringSplitOptions.RemoveEmptyEntries);

                    if (identificativi?.Length >= 2 && Guid.TryParse(identificativi[0], out Guid idGruppo))
                    {
                        string[] elencoIdAnalisiVenditaConfigurazioneArticoloAggiuntivo = identificativi[1].Split(new string[] { "###" }, StringSplitOptions.RemoveEmptyEntries);
                        if (elencoIdAnalisiVenditaConfigurazioneArticoloAggiuntivo?.Length > 0)
                        {
                            Logic.AnalisiCosti llAnalisiCosti = new Logic.AnalisiCosti();
                            Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi analisiVenditeConfigurazioneArticoliAggiuntivi = new Logic.AnalisiVenditeConfigurazioneArticoliAggiuntivi(llAnalisiCosti);
                            Logic.AnalisiCostiArticoli llArt = new Logic.AnalisiCostiArticoli(llAnalisiCosti);

                            foreach (string idAnalisiVenditaConfigurazioneArticoloAggiuntivo in elencoIdAnalisiVenditaConfigurazioneArticoloAggiuntivo)
                            {
                                if (Guid.TryParse(idAnalisiVenditaConfigurazioneArticoloAggiuntivo, out Guid idConfigurazioneAggiuntivo))
                                {
                                    AnalisiVenditaConfigurazioneArticoloAggiuntivo configurazioneArticolo = analisiVenditeConfigurazioneArticoliAggiuntivi.Find(new EntityId<AnalisiVenditaConfigurazioneArticoloAggiuntivo>(idConfigurazioneAggiuntivo));
                                    if (configurazioneArticolo != null)
                                    {
                                        AnalisiCostoArticolo articoloAggiuntivo = new AnalisiCostoArticolo();
                                        articoloAggiuntivo.IDRaggruppamento = idGruppo;
                                        articoloAggiuntivo.Descrizione = configurazioneArticolo.ANAGRAFICAARTICOLIOut.DESCRIZIONE;                                        
                                        articoloAggiuntivo.CodiceArticolo = configurazioneArticolo.CodiceArticoloOut;

                                        llArt.Create(articoloAggiuntivo, false);
                                    }
                                }
                            }

                            llAnalisiCosti.SubmitToDatabase();

                            llAnalisiCosti.RicalcolaTotali(currentID.Value);

                            Helper.Web.ReloadPage(this, currentID.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(Page, ex);
            }
        }

        protected void rgGrigliaArticoli_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            if (e.DetailTableView.Name == "CampiAggiuntivi")
            {
                string articoloID = dataItem.GetDataKeyValue("ID").ToString();
                Logic.AnalisiCostiArticoli llArt = new Logic.AnalisiCostiArticoli();
                Logic.AnalisiCostiArticoloCampiAggiuntivi llArtAgg = new Logic.AnalisiCostiArticoloCampiAggiuntivi(llArt);
                Entities.AnalisiCostoArticolo currentArt = llArt.Find(new Entities.EntityId<AnalisiCostoArticolo>(articoloID));

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
                    Logic.AnalisiCostiArticoli ll = new Logic.AnalisiCostiArticoli();
                    Entities.AnalisiCostoArticolo entityToDelete = ll.Find(new EntityId<Entities.AnalisiCostoArticolo>(idSelectedRow));
                    if (entityToDelete != null)
                    {
                        AnalisiCostoRaggruppamento gruppo = entityToDelete.AnalisiCostoRaggruppamento;
                        string descrizione = entityToDelete.Descrizione;
                        string nomeGruppo = gruppo.Denominazione;
                        Guid idAnalisiCosti = entityToDelete.AnalisiCostoRaggruppamento.IDAnalisiCosto;
                        string numeroAnalisiCosti = entityToDelete.AnalisiCostoRaggruppamento.AnalisiCosto.Titolo;
                        ll.Delete(entityToDelete, true);

                        SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - Rimosso l'entity AnalisiCostoArticolo '{1}' dal gruppo '{2}' nell'Analisi Costi {3}.", Request.Url.AbsolutePath, descrizione, nomeGruppo, numeroAnalisiCosti));

                        Logic.AnalisiCosti llAnalisi = new Logic.AnalisiCosti(ll);
                        llAnalisi.RicalcolaTotali(idAnalisiCosti);
                        ((RadGrid)sender).DataSource = ll.Read(gruppo);
                        ((RadGrid)sender).DataBind();

                        Helper.Web.ReloadPage(this, currentID.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);
                MessageHelper.ShowErrorMessage(Page, "Operazione di Eliminazione AnalisiCostoArticolo non riuscita, è stato riscontrato il seguente errore:<br />" + ex.Message);
                e.Canceled = true;
            }
        }

        protected void rgGrigliaArticoli_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "ExpandCollapse")
            {
                return;
            }


            Panel divGruppo = (Panel)((RadGrid)sender).Parent;
            if (divGruppo != null)
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    string artID = dataItem.GetDataKeyValue("ID").ToString();
                    Guid articoloID;
                    if (Guid.TryParse(artID, out articoloID))
                    {
                        HiddenField hdIdArticoloInEdit = (HiddenField)divGruppo.FindControl("hdIdArticoloInEdit");
                        Button btAggiungiArticolo = (Button)divGruppo.FindControl("btAggiungiArticolo");
                        Button btSalvaArticolo = (Button)divGruppo.FindControl("btSalvaArticolo");
                        Button btAggiornaArticolo = (Button)divGruppo.FindControl("btAggiornaArticolo");
                        Button btAnnullaEdit = (Button)divGruppo.FindControl("btAnnullaEdit");
                        Button btClonaGruppo = (Button)divGruppo.FindControl("btClonaGruppo");
                        AnalisiCostoRaggruppamentoEditItem EditItemRaggruppamento = (AnalisiCostoRaggruppamentoEditItem)divGruppo.FindControl("EditItemRaggruppamento");
                        LayoutRow rigaTotaliGruppo = (LayoutRow)divGruppo.FindControl("rigaTotaliGruppo");
                        RadGrid rgGrigliaArticoli = (RadGrid)sender;

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
                                EditItemRaggruppamento.Inizializza(articoloID);
                                rgGrigliaArticoli.Visible = false;
                                rigaTotaliGruppo.Visible = false;
                                break;
                        }
                    }

                }
            }
        }

        protected void rgGrigliaArticoli_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            HiddenField hd = (HiddenField)((RadGrid)sender).Parent.FindControl("hdIdRaggruppamento");
            AnalisiCostoRaggruppamentoEditItem EditItemRaggruppamento = (AnalisiCostoRaggruppamentoEditItem)((RadGrid)sender).Parent.FindControl("EditItemRaggruppamento");
            if (hd != null)
            {
                Logic.AnalisiCostiArticoli llArt = new Logic.AnalisiCostiArticoli();
                Logic.AnalisiCostiRaggruppamenti llAcr = new Logic.AnalisiCostiRaggruppamenti(llArt);
                AnalisiCostoRaggruppamento acr = llAcr.Find(new EntityId<AnalisiCostoRaggruppamento>(new Guid(hd.Value)));
                IQueryable<Entities.AnalisiCostoArticolo> dataSource = llArt.Read(acr);

                ((RadGrid)sender).DataSource = dataSource;
                
                LayoutRow rigaTotaliGruppo = (LayoutRow)((RadGrid)sender).Parent.FindControl("rigaTotaliGruppo");
                if (rigaTotaliGruppo != null) rigaTotaliGruppo.Visible = dataSource.Any() && !EditItemRaggruppamento.Visible;
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

        protected void SalvaModificheGruppo_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                if (Guid.TryParse(button.CommandArgument, out Guid idGruppo))
                {
                    Logic.AnalisiCostiRaggruppamenti llRag = new Logic.AnalisiCostiRaggruppamenti();
                    Entities.AnalisiCostoRaggruppamento gruppo = llRag.Find(new EntityId<AnalisiCostoRaggruppamento>(idGruppo));

                    if (gruppo != null)
                    {
                        RadNumericTextBox rntbTotaleCosto = (RadNumericTextBox)button.Parent.FindControl("rntbTotaleCosto");
                        RadNumericTextBox rntbTotaleVendita = (RadNumericTextBox)button.Parent.FindControl("rntbTotaleVendita");
                        RadNumericTextBox rntbTotaleRedditivita = (RadNumericTextBox)button.Parent.FindControl("rntbTotaleRedditivita");

                        if (rntbTotaleCosto != null &&
                            rntbTotaleVendita != null &&
                            rntbTotaleRedditivita != null)
                        {
                            gruppo.TotaleCosto = (decimal?)rntbTotaleCosto.Value;
                            gruppo.TotaleVendita = (decimal?)rntbTotaleVendita.Value;
                            gruppo.TotaleRicaricoPercentuale = (decimal?)rntbTotaleRedditivita.Value;

                            llRag.SubmitToDatabase();

                            Helper.Web.ReloadPage(this, gruppo.AnalisiCosto.ID.ToString());
                        }
                    }                    
                }
            }
        }

        protected void AnnullaModificheGruppo_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                CollapsiblePanelExtender cpe = (CollapsiblePanelExtender)button.Parent.FindControl("cpe");
                cpe.Collapsed = true;
                cpe.ClientState = true.ToString().ToLower();
            }
        }

        
    }
}