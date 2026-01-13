using SeCoGes.Utilities;
using SeCoGEST.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SeCoGEST.Web.Interventi
{
    public partial class ConfigTipologieTicketCliente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                InformazioniAccountAutenticato utenteLoggato = InformazioniAccountAutenticato.GetIstance();
                if (utenteLoggato == null || utenteLoggato.Account == null)
                {
                    Response.Redirect("/Home.aspx");
                    return;
                }
                if (!utenteLoggato.Account.Amministratore.HasValue || utenteLoggato.Account.Amministratore.Value == false)
                {
                    Response.Redirect("/Home.aspx");
                    return;
                }

                DataIntervento = DateTime.Now;
                //rtbCodiceCliente.Text = "C  1585";
                //rtbCodiceCliente.Text = string.Empty;
                //CodiceCliente = rtbCodiceCliente.Text.Trim();
                InitializeForInsert();
                rgArchiveItems.Rebind();
            }
        }


        protected void rbNuovo_Click(object sender, EventArgs e)
        {
            InitializeForInsert();
        }


        #region Properties

        /// <summary>
        /// Recupera o setta l'id della Configurazione Corrente
        /// </summary>
        public Guid IDConfigurazioneCorrente
        {
            get
            {
                if (ViewState["IDConfigurazioneCorrente"] == null)
                {
                    ViewState["IDConfigurazioneCorrente"] = Guid.Empty;
                }

                return (Guid)ViewState["IDConfigurazioneCorrente"];
            }
            set
            {
                ViewState["IDConfigurazioneCorrente"] = value;
            }
        }

        public Entities.EntityId<Entities.ConfigurazioneTipologiaTicketCliente> CurrentEntityIdentifier
        {
            get
            {
                return new Entities.EntityId<Entities.ConfigurazioneTipologiaTicketCliente>(IDConfigurazioneCorrente);
            }
        }

        ///// <summary>
        ///// Recupera o setta l'IDIntervento dell'entity InterventoArticolo
        ///// </summary>
        //public Guid IDIntervento
        //{
        //    get
        //    {
        //        if (ViewState["IDIntervento"] == null)
        //        {
        //            ViewState["IDIntervento"] = Guid.Empty;
        //        }

        //        return (Guid)ViewState["IDIntervento"];
        //    }
        //    set
        //    {
        //        ViewState["IDIntervento"] = value;
        //    }
        //}

        /// <summary>
        /// Imposta o restituisce il Codice del Cliente al quale si riferisce l'intervento
        /// </summary>
        public string CodiceCliente
        {
            get
            {
                if (ViewState["CodiceCliente"] != null)
                {
                    return ViewState["CodiceCliente"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["CodiceCliente"] = value;
            }
        }

        /// <summary>
        /// Data dell'Intervento utilizzata per filtrare l'elenco degli articoli disponibili
        /// </summary>
        public DateTime DataIntervento
        {
            get
            {
                if (ViewState["DataIntervento"] != null)
                {
                    return (DateTime)ViewState["DataIntervento"];
                }
                else
                {
                    return DateTime.Now;
                }
            }
            set
            {
                ViewState["DataIntervento"] = value;
            }
        }

        #endregion
        

        #region Metodi di gestione

        /// <summary>
        /// Visualizza i dati dell'entity passata come parametro
        /// </summary>
        public void InitializeForInsert()
        {
            IDConfigurazioneCorrente = Guid.Empty;

            rcbProvenienzaArticolo.ClearSelection();
            rcbProvenienzaArticolo.SelectedValue = "";
            rcbProvenienzaArticolo.Text = "";
            FillCombo_ProvenienzaArticoli();
            FillCombo_TipologieIntervento();
            FillCombo_Reparti();
            FillCombo_CondizioniIntervento();
            FillList_CaratteristicheIntervento(null);
            FillCombo_ModelliCaratteristicheIntervento();
            FillCombo_OperatoriRiferimento();

            rcbArticoli.ClearSelection();
            rcbArticoli.SelectedValue = "";
            rcbArticoli.Text = "";

            //Salva Nuova Configurazione
            rtbDescrizionePersonalizzataArticolo.Text = String.Empty;
            btSaveConfiguration.Text = "Salva Nuova Configurazione";
        }

        /// <summary>
        /// Visualizza i dati dell'entity passata come parametro
        /// </summary>
        /// <param name="valoreCorrente"></param>
        public void ShowData(Entities.ConfigurazioneTipologiaTicketCliente entity, bool cloning = false)
        {
            if (entity == null) return;

            IDConfigurazioneCorrente = entity.Id;

            rdpScadenzaContratto.Clear();
            rdpScadenzaContratto.SelectedDate = entity.ScadenzaContratto;

            FillCombo_ProvenienzaArticoli();
            RadComboBoxItem selItemProvArt = rcbProvenienzaArticolo.FindItemByValue(entity.IDRicercaInProvenienzaArticoli);
            if (selItemProvArt != null) selItemProvArt.Selected = true;


            //RadComboBoxItem selItemArt = new RadComboBoxItem(entity.CodiceArticolo, entity.IDRicercaInElencoCompletoArticoli);
            //selItemArt.Selected = true;
            //rcbArticoli.Items.Add(selItemArt);
            rcbArticoli.ClearSelection();
            rcbArticoli.SelectedValue = entity.IDRicercaInElencoCompletoArticoli;
            rcbArticoli.Text = entity.CodiceArticolo;

            if(entity.IdOperatore.HasValue)
            {
                RadComboBoxItem operatoreRiferimento = rcbOperatoreDiRiferimento.FindItemByValue(entity.IdOperatore.Value.ToString());
                if (operatoreRiferimento != null) operatoreRiferimento.Selected = true;
            }
            else
            {
                rcbOperatoreDiRiferimento.ClearSelection();
            }


            rtbDescrizionePersonalizzataArticolo.Text = entity.Descrizione;
            chkVisibilePerCliente.Checked = entity.VisibilePerCliente.HasValue ? entity.VisibilePerCliente.Value : false;

            SetConfigurazione_TipologieIntervento(entity);
            //rddlTipologiaIntervento.ClearSelection();
            //var rddlItem = rddlTipologiaIntervento.FindItemByValue(entity.IdTipologia.ToString());
            //if (rddlItem != null) rddlItem.Selected = true;

            SetConfigurazione_Reparti(entity);
            //rddlReparto.ClearSelection();
            //rddlItem = rddlReparto.FindItemByValue(entity.IdRepartoUfficio.ToString());
            //if (rddlItem != null) rddlItem.Selected = true;
            //rddlReparto_SelectedIndexChanged(rddlReparto, null);


            SetConfigurazione_CondizioniIntervento(entity);
            //rddlCondizione.ClearSelection();
            //if(entity.IdCondizione.HasValue)
            //{
            //    var rddlCondizItem = rddlCondizione.FindItemByValue(entity.IdCondizione.ToString());
            //    if (rddlCondizItem != null) rddlCondizItem.Selected = true;
            //}

            FillList_CaratteristicheIntervento(entity);
            FillCheckList_CaratteristicheIntervento(entity);


            if (cloning)
            {
                IDConfigurazioneCorrente = Guid.Empty;
                btSaveConfiguration.Text = "Salva Nuova Configurazione";
            }
        }

        /// <summary>
        /// Salva nel database i dati presenti nell'interfaccia grafica
        /// </summary>
        public bool SaveData()
        {
            bool isNew = !CurrentEntityIdentifier.HasValue;

            // Valida i dati nella pagina
            SeCoGes.Utilities.MessagesCollector erroriDiValidazione = ValidaDati();
            if (erroriDiValidazione.HaveMessages)
            {
                // Se qualcosa non va bene mostro un avviso all'utente
                //throw new Exception(erroriDiValidazione.ToString("<br />"));
                string msg = erroriDiValidazione.ToString("<br />");
                //msg = "Ciao";
                //string message = $"radalert('{msg}', 330, 210, 'Errore');";
                //string message = $"window.alert('{msg}');";
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "erroralert", message);
                MessageHelper.ShowMessage(this, "Errore", msg);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", message, true);
                return false;
            }

            try
            {
                Logic.ConfigurazioniTipologieTicketCliente ll = new Logic.ConfigurazioniTipologieTicketCliente();
                Entities.ConfigurazioneTipologiaTicketCliente entityToSave = null;
                //Se CurrentId contiene un ID allora cerco l'entity nel database
                if (!isNew)
                {
                    entityToSave = ll.Find(CurrentEntityIdentifier);
                }

                // Definisco una variabile che indica se si deve compiere una azione di creazione di una nuova entity o di modifica
                bool nuovo = false;

                if (entityToSave == null)
                {
                    nuovo = true;
                    entityToSave = new Entities.ConfigurazioneTipologiaTicketCliente();
                    entityToSave.Id = Guid.NewGuid();

                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriPrincipaliDallaView(entityToSave);

                    //Creo nuova entity tramite una stored procedure
                    ll.Create(entityToSave, false);

                    AssociaCaratteristiche(entityToSave, true);
                }
                else
                {
                    EstraiValoriPrincipaliDallaView(entityToSave);
                    AssociaCaratteristiche(entityToSave, false);
                }
                ll.SubmitToDatabase();

                IDConfigurazioneCorrente = entityToSave.Id;
                rgArchiveItems.Rebind();

                SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - {1} l'entity ConfigurazioneTipologiaTicketCliente con Id '{2}'.", Request.Url.AbsolutePath, ((nuovo) ? "Creato" : "Salvato"), entityToSave.Id));

                return true;
            }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);

                throw ex;
            }
        }

        /// <summary>
        /// Restituisce un oggetto contenente gli eventuali errori di validazione dei dati
        /// </summary>
        /// <returns></returns>
        public SeCoGes.Utilities.MessagesCollector ValidaDati()
        {
            SeCoGes.Utilities.MessagesCollector messaggi = new SeCoGes.Utilities.MessagesCollector();

            Page.Validate();
            if (!Page.IsValid)
            {
                messaggi.Add("Attenzione, alcuni dati necessari non sono stati valorizzati oppure sono presenti errori nei dati inseriti!");
            }

            if(!messaggi.HaveMessages)
            {
                if (rcbCliente.SelectedValue == string.Empty)
                {
                    messaggi.Add("E' necessario specificare il Cliente.");
                }
                if (rcbArticoli.SelectedValue == string.Empty)
                {
                    messaggi.Add("E' necessario specificare l'Articolo.");
                }
                if (string.IsNullOrWhiteSpace(rtbDescrizionePersonalizzataArticolo.Text))
                {
                    messaggi.Add("E' necessario dare una Descrizione dell'Articolo.");
                }
                if(rddlTipologiaIntervento.SelectedValue == string.Empty)
                {
                    messaggi.Add("E' necessario specificare la tipologia di intervento.");
                }
                if (rddlReparto.SelectedValue == string.Empty)
                {
                    messaggi.Add("E' necessario specificare il Reparto.");
                }
            }

            return messaggi;
        }

        /// <summary>
        /// Legge i dati inseriti nell'interfaccia e li inserisce nell'entity passata
        /// </summary>
        /// <param name="entityToFill"></param>
        /// <param name="isNew"></param>
        public string EstraiValoriPrincipaliDallaView(Entities.ConfigurazioneTipologiaTicketCliente entityToFill)
        {
            if (entityToFill == null)
            {
                throw new Exception("Empty entityToFill");
                //entityToFill = new Entities.ConfigurazioneTipologiaTicketCliente();
            }
            //if (isNew)
            //{
            //    entityToFill.Id = Guid.NewGuid(); // IDEntity;
            //}

            entityToFill.ScadenzaContratto = rdpScadenzaContratto.SelectedDate;

            string provenienzaArticolo = rcbArticoli.SelectedValue.Substring(0, 1);
            decimal? progressivo;

            if (provenienzaArticolo == "0") //Articoli da magazzino (SelectedValue = 0 + codicearticolo)
            {
                progressivo = null;
            }
            else //Articoli da magazzino (SelectedValue = numerotipologia + progressivo)
            {
                progressivo = rcbArticoli.SelectedValue.Substring(1).ToNullableDecimal();
            }


            entityToFill.TipologiaArticolo = int.Parse(provenienzaArticolo);
            entityToFill.Progressivo = progressivo;
            entityToFill.CodiceCliente = this.CodiceCliente;
            entityToFill.Descrizione = rtbDescrizionePersonalizzataArticolo.Text.Trim();


            Logic.Metodo.ElenchiCompletiArticoli llArts = new Logic.Metodo.ElenchiCompletiArticoli();
            Entities.ElencoCompletoArticoli articoloDaArchivio = llArts.Read(DataIntervento).FirstOrDefault(x => x.ID == rcbArticoli.SelectedValue);

            if (articoloDaArchivio != null)
            {
                entityToFill.CodiceContratto = articoloDaArchivio.CodiceContratto;
                entityToFill.CodiceArticolo = articoloDaArchivio.CodiceArticolo;
            }
            else
            {
                throw new Exception("Impossibile trovare l'articolo selezionato!");
            }

            entityToFill.VisibilePerCliente = chkVisibilePerCliente.Checked;

            entityToFill.IdTipologia = Guid.Parse(rddlTipologiaIntervento.SelectedValue);
            entityToFill.IdRepartoUfficio = Guid.Parse(rddlReparto.SelectedValue);

            if(rddlCondizione.SelectedValue != string.Empty)
            {
                entityToFill.IdCondizione = int.Parse(rddlCondizione.SelectedValue);
            }
            else
            {
                entityToFill.IdCondizione = null;
            }


            if (rcbOperatoreDiRiferimento.SelectedValue != string.Empty)
            {
                entityToFill.IdOperatore = Guid.Parse(rcbOperatoreDiRiferimento.SelectedValue);
            }
            else
            {
                entityToFill.IdOperatore = null;
            }


            return string.Empty;
        }

        public string AssociaCaratteristiche(Entities.ConfigurazioneTipologiaTicketCliente entityToFill, bool isNew)
        {
            if (entityToFill == null)
            {
                throw new Exception("Empty entityToFill");
            }


            Logic.CaratteristicheTipologieIntervento logic = new Logic.CaratteristicheTipologieIntervento();
            List<Entities.CaratteristicaTipologiaIntervento> caratteristicheAttuali = new List<Entities.CaratteristicaTipologiaIntervento>();
            //List<Entities.CaratteristicaTipologiaIntervento> caratteristicheNuove = new List<Entities.CaratteristicaTipologiaIntervento>();
            List<Entities.CaratteristicaTipologiaIntervento> caratteristicheAttualiDaEliminare = new List<Entities.CaratteristicaTipologiaIntervento>();

            if (!isNew)
            {
                // Registra le caratteristiche salvate nel db per questa configurazione
                caratteristicheAttuali = logic.Read(entityToFill.Id).ToList();
            }


            List<RadListBoxItem> elencoCaratteristicheSelezionate = new List<RadListBoxItem>(); // rlbCaratteristicheIntervento.CheckedItems.ToList();
            foreach (RadListBoxItem item in rlbCaratteristicheIntervento.Items)
            {
                CheckBox chkboxStatus = (CheckBox)item.FindControl("chkboxStatus");
                if (chkboxStatus.Checked)
                {
                    elencoCaratteristicheSelezionate.Add(item);
                    if (int.TryParse(item.Value, out int idc))
                    {
                        Entities.CaratteristicaTipologiaIntervento caratteristicaDaAggiornare = caratteristicheAttuali.FirstOrDefault(x => x.IdCaratteristica == idc);
                        //if (!caratteristicheAttuali.Any(x => x.IdCaratteristica == idc)) 
                        if (caratteristicaDaAggiornare == null)
                        {
                            Entities.CaratteristicaTipologiaIntervento nuovaCaratteristica = new Entities.CaratteristicaTipologiaIntervento();
                            nuovaCaratteristica.IdConfigurazione = entityToFill.Id;
                            nuovaCaratteristica.IdCaratteristica = idc;
                            AssegnaParametri(item, nuovaCaratteristica);
                            logic.Create(nuovaCaratteristica, false);
                            //caratteristicheNuove.Add(nuovaCaratteristica);
                        }
                        else
                        {
                            AssegnaParametri(item, caratteristicaDaAggiornare);
                            //logic.SubmitToDatabase();
                        }
                    }
                }
            }




            //foreach (RadListBoxItem item in elencoCaratteristicheSelezionate)
            //{
            //    if(int.TryParse(item.Value, out int idc))
            //    {
            //        if (!caratteristicheAttuali.Any(x => x.IdCaratteristica == idc))
            //        {
            //            Entities.CaratteristicaTipologiaIntervento nuovaCaratteristica = new Entities.CaratteristicaTipologiaIntervento();
            //            nuovaCaratteristica.IdConfigurazione = entityToFill.Id;
            //            nuovaCaratteristica.IdCaratteristica = idc;
            //            AssegnaParametri(item, nuovaCaratteristica);
            //            logic.Create(nuovaCaratteristica, false);
            //            //caratteristicheNuove.Add(nuovaCaratteristica);
            //        }
            //    }
            //}


            if (!isNew)
            {
                // Elimina tutte le caratteristiche salvate nel db per questa configurazione
                foreach(Entities.CaratteristicaTipologiaIntervento caratteristicaAttuale in caratteristicheAttuali)
                {
                    if (!elencoCaratteristicheSelezionate.Any(x => int.Parse(x.Value) == caratteristicaAttuale.IdCaratteristica))
                    {
                        caratteristicheAttualiDaEliminare.Add(caratteristicaAttuale);
                    }
                }


                if(caratteristicheAttualiDaEliminare.Count() > 0)
                {
                    logic.Delete(caratteristicheAttualiDaEliminare, false);
                }
            }

            return string.Empty;
        }

        private void AssegnaParametri(RadListBoxItem item, Entities.CaratteristicaTipologiaIntervento caratteristica)
        {
            if (Enum.IsDefined(typeof(Entities.CaratteristicaInterventoEnum), caratteristica.IdCaratteristica))
            {
                Entities.CaratteristicaInterventoEnum c = (Entities.CaratteristicaInterventoEnum)caratteristica.IdCaratteristica;
                switch (c)
                {
                    case CaratteristicaInterventoEnum.PresaInCaricoEntroMinuti:
                    case CaratteristicaInterventoEnum.RipristinoEntroMinuti:
                    case CaratteristicaInterventoEnum.RispostaEntroMinuti:
                    case CaratteristicaInterventoEnum.RipristinoEntroMinutiDaPresaInCarico:
                        int ore = 0;
                        int minuti = 0;
                        RadNumericTextBox rntbOre = (RadNumericTextBox)item.FindControl("rntbOre");
                        RadNumericTextBox rntbMinuti = (RadNumericTextBox)item.FindControl("rntbMinuti");
                        if (rntbOre.Value.HasValue) ore = (int)rntbOre.Value;
                        if (rntbMinuti.Value.HasValue) minuti = (int)rntbMinuti.Value;
                        if (ore > 0)
                        {
                            minuti += ore * 60;
                        }
                        caratteristica.Parametri = minuti.ToString();

                        //RadTimePicker rtpTempo = (RadTimePicker)item.FindControl("rtpTempo");
                        //if(rtpTempo.SelectedTime.HasValue)
                        //{
                        //    caratteristica.Parametri = rtpTempo.SelectedTime.Value.Ticks.ToString();
                        //}
                        //else
                        //{
                        //    caratteristica.Parametri = string.Empty;
                        //}

                        break;
                    default:
                        caratteristica.Parametri = null;
                        break;
                }
            }
            else
            {
                caratteristica.Parametri = null;
            }
        }

        #endregion



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


        protected void rcbArticoli_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                string testoRicerca = (e.Text == null) ? String.Empty : e.Text.ToLower();
                RadComboBox combo = (RadComboBox)sender;
                combo.Items.Clear();

                Logic.Metodo.ElenchiCompletiArticoli llArts = new Logic.Metodo.ElenchiCompletiArticoli();

                // Carica tutti gli Articoli memorizzzati
                IQueryable<Entities.ElencoCompletoArticoli> queryBase = llArts.Read(DataIntervento);


                // Se è stato specificata la Provenienza filtra gli Articoli restituendo solo quelli del gruppo indicato
                int provenienza = 0;
                string codiceContratto = string.Empty;
                if (e.Context.ContainsKey("ProvenienzaArticolo"))
                {
                    string provenienzaArt = e.Context["ProvenienzaArticolo"].ToString().Substring(0, 1);
                    codiceContratto = e.Context["ProvenienzaArticolo"].ToString().Substring(1);
                    if (!int.TryParse(provenienzaArt, out provenienza)) provenienza = 0;
                }
                queryBase = queryBase.Where(x => x.TipologiaArticolo == provenienza);


                if (provenienza > 0)
                {
                    queryBase = queryBase.Where(x => x.CodiceCliente == this.CodiceCliente);

                    if (codiceContratto.Length > 0)
                    {
                        queryBase = queryBase.Where(x => x.CodiceContratto == codiceContratto);
                    }
                }

                // controllo della data di validità degli articoli
                queryBase = queryBase.Where(x => x.DataAttivazione <= this.DataIntervento && x.DataChiusura >= this.DataIntervento);


                if (!string.IsNullOrWhiteSpace(testoRicerca))
                {
                    queryBase = queryBase.Where(x => (x.CodiceArticolo + " - " + x.Descrizione).ToLower().Contains(testoRicerca));
                }

                int itemsPerRequest = (combo.ItemsPerRequest <= 0) ? 20 : combo.ItemsPerRequest;
                int itemOffset = e.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                int numTotaleUtenti = queryBase.Count();

                if (endOffset > numTotaleUtenti)
                    endOffset = numTotaleUtenti;

                IEnumerable<Entities.ElencoCompletoArticoli> entities = queryBase.Skip(itemOffset).Take(itemsPerRequest);

                foreach (Entities.ElencoCompletoArticoli entity in entities)
                {
                    RadComboBoxItem item = new RadComboBoxItem(entity.CodiceArticolo, entity.ID);
                    item.Attributes.Add("DescrizioneArticolo", entity.Descrizione);
                    item.Attributes.Add("NoteArticolo", entity.Note);
                    item.DataItem = entity;
                    combo.Items.Add(item);
                }

                combo.DataBind();


                string tipo;
                switch (provenienza)
                {
                    case 1:
                        tipo = "Articoli del Contratto";
                        break;
                    case 2:
                        tipo = "Articoli Prepagati";
                        break;
                    case 3:
                        tipo = "Tariffe Standard";
                        break;
                    case 4:
                        tipo = "Addebiti";
                        break;
                    default:
                        tipo = "Articoli";
                        break;
                }

                if (numTotaleUtenti > 0)
                {
                    e.Message = String.Format("{2} (da <b>1</b> a <b>{0}</b> di <b>{1}</b>)", endOffset.ToString(), numTotaleUtenti.ToString(), tipo);
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






        /// <summary>
        /// Effettua il popolamento del combo Tipo
        /// </summary>
        private void FillCombo_ProvenienzaArticoli()
        {
            lblProvenienzaArticolo.Text = string.Format("Provenienza articoli (Cliente: {0})", CodiceCliente);

            rcbProvenienzaArticolo.Items.Clear();
            rcbProvenienzaArticolo.ClearSelection();

            RadComboBoxItem rcbi = new RadComboBoxItem("Articoli di Magazzino", "0");
            rcbProvenienzaArticolo.Items.Add(rcbi);

            Logic.Interventi ll = new Logic.Interventi();
            IEnumerable<Entities.InformazioniContratto> elencoInfoContratti = ll.GetContrattiPerCliente(CodiceCliente, DataIntervento);

            foreach (Entities.InformazioniContratto infoContratto in elencoInfoContratti) //.Where(x => x.TipologiaArticolo > 0))
            {
                rcbi = new RadComboBoxItem(infoContratto.DescrizioneProvenienza, infoContratto.IDUnivoco);
                rcbProvenienzaArticolo.Items.Add(rcbi);
            }

            rcbProvenienzaArticolo.DataBind();
        }

        protected string GetStyleByProvenienza(object comboItem)
        {
            string st = "";
            if (comboItem != null && comboItem is RadComboBoxItem)
            {
                if (((RadComboBoxItem)comboItem).Value.StartsWith("4"))
                    st = "color: red; font-weight: bold;";
            }
            return st;
        }


               


        private void FillCombo_TipologieIntervento()
        {
            rddlTipologiaIntervento.ClearSelection();
            rddlTipologiaIntervento.DataSource = new Logic.TipologieIntervento().Read();
            rddlTipologiaIntervento.DataBind();
            DropDownListItem emptyItem = new DropDownListItem(string.Empty, string.Empty);
            rddlTipologiaIntervento.Items.Insert(0, emptyItem);
        }
        private void SetConfigurazione_TipologieIntervento(Entities.ConfigurazioneTipologiaTicketCliente configurazione)
        {
            rddlTipologiaIntervento.ClearSelection();
            DropDownListItem rddlItem = rddlTipologiaIntervento.FindItemByValue(configurazione.IdTipologia.ToString());
            if (rddlItem != null) rddlItem.Selected = true;
        }



        private void FillCombo_Reparti()
        {
            rddlReparto.ClearSelection();
            rddlReparto.DataSource = new Logic.RepartiUfficio().Read();
            rddlReparto.DataBind();
            DropDownListItem emptyItem = new DropDownListItem(string.Empty, string.Empty);
            rddlReparto.Items.Insert(0, emptyItem);
        }
        private void SetConfigurazione_Reparti(Entities.ConfigurazioneTipologiaTicketCliente configurazione)
        {
            rddlReparto.ClearSelection();
            DropDownListItem rddlItem = rddlReparto.FindItemByValue(configurazione.IdRepartoUfficio.ToString());
            if (rddlItem != null) rddlItem.Selected = true;
            rddlReparto_SelectedIndexChanged(rddlReparto, null);
        }



        private void FillCombo_CondizioniIntervento()
        {
            rddlCondizione.ClearSelection();
            rddlCondizione.DataSource = new Logic.CondizioniIntervento().Read();
            rddlCondizione.DataBind();
            DropDownListItem emptyItem = new DropDownListItem(string.Empty, string.Empty);
            rddlCondizione.Items.Insert(0, emptyItem);
        }
        private void SetConfigurazione_CondizioniIntervento(Entities.ConfigurazioneTipologiaTicketCliente configurazione)
        {
            rddlCondizione.ClearSelection();
            if (configurazione.IdCondizione.HasValue)
            {
                DropDownListItem rddlItem = rddlCondizione.FindItemByValue(configurazione.IdCondizione.ToString());
                if (rddlItem != null) rddlItem.Selected = true;
            }
        }



        private void FillCombo_ModelliCaratteristicheIntervento()
        {
            List<ModelloConfigurazioneTicketCliente> dataSource = new Logic.ModelliConfigurazioneTicketCliente().Read().ToList();
            dataSource.Insert(0, new ModelloConfigurazioneTicketCliente() { Id = Guid.Empty, Nome = string.Empty });
            rcbModelli.ClearSelection();
            rcbModelli.DataSource = dataSource;
            rcbModelli.DataBind();

            // Si inserisce un entità vuota di ModelloConfigurazioneTicketCliente perchè, così facendo, la selezione di quella entità nella combobox, fa si che si rimuovano i valori presenti nei controlli.
            // Se, al contrario, si inserisse semplicemente un nuovo ComboBoxItem, questa pulizia dei controlli non avverrebbe.
            //rcbModelli.Items.Insert(0, new RadComboBoxItem());
        }

        private void FillCombo_OperatoriRiferimento()
        {
            rcbOperatoreDiRiferimento.ClearSelection();
            rcbOperatoreDiRiferimento.DataSource = new Logic.Operatori().Read();
            rcbOperatoreDiRiferimento.DataBind();
            rcbOperatoreDiRiferimento.Items.Insert(0, new RadComboBoxItem());
        }


        private void FillList_CaratteristicheIntervento(Entities.ConfigurazioneTipologiaTicketCliente entity)
        {
            List<Entities.CaratteristicaTipologiaIntervento> caratteristicheToBind = new List<CaratteristicaTipologiaIntervento>();
            List<Entities.CaratteristicaTipologiaIntervento> caratteristicheIntervento = new List<CaratteristicaTipologiaIntervento>();
            IEnumerable<Entities.CaratteristicaIntervento> archivioCaratteristiche = new Logic.CaratteristicheIntervento().Read();
            if (entity != null)
            {
                caratteristicheIntervento = new Logic.CaratteristicheTipologieIntervento().Read(entity.Id).ToList();
            }

            foreach (Entities.CaratteristicaIntervento archCar in archivioCaratteristiche)
            {
                Entities.CaratteristicaTipologiaIntervento c = caratteristicheIntervento.FirstOrDefault(x => x.IdCaratteristica == archCar.Id);
                if(c == null)
                {
                    c = new CaratteristicaTipologiaIntervento();
                    c.IdCaratteristica = archCar.Id;
                    c.CaratteristicaIntervento = archCar;
                    //c.IdConfigurazione
                }
                caratteristicheToBind.Add(c);
            }

            rlbCaratteristicheIntervento.DataSource = caratteristicheToBind; // new Logic.CaratteristicheIntervento().Read();
            rlbCaratteristicheIntervento.DataBind();
            rlbCaratteristicheIntervento.ClearChecked();
        }

        private void FillCheckList_CaratteristicheIntervento(Entities.ConfigurazioneTipologiaTicketCliente entity)
        {
            rlbCaratteristicheIntervento.ClearChecked();
            foreach (var caratteristica in entity.CaratteristicaTipologiaInterventos)
            {
                var carCheck = rlbCaratteristicheIntervento.FindItemByValue(caratteristica.IdCaratteristica.ToString());
                if (carCheck != null) carCheck.Checked = true;
            }
        }

        private void FillList_CaratteristicheInterventoDaModello(Guid idModelloConfigurazioneTicketCliente)
        {
            List<Entities.CaratteristicaTipologiaIntervento> caratteristicheToBind = new List<CaratteristicaTipologiaIntervento>();
            List<Entities.ModelloConfigurazioneCaratteristicheTicketCliente> caratteristicheIntervento = new List<ModelloConfigurazioneCaratteristicheTicketCliente>();
            IEnumerable<Entities.CaratteristicaIntervento> archivioCaratteristiche = new Logic.CaratteristicheIntervento().Read();
            if (idModelloConfigurazioneTicketCliente != Guid.Empty)
            {
                caratteristicheIntervento = new Logic.ModelliConfigurazioneCaratteristicheTicketCliente().Read(idModelloConfigurazioneTicketCliente).ToList();
            }

            foreach (Entities.CaratteristicaIntervento archCar in archivioCaratteristiche)
            {
                Entities.ModelloConfigurazioneCaratteristicheTicketCliente m = caratteristicheIntervento.FirstOrDefault(x => x.IdCaratteristica == archCar.Id);
                Entities.CaratteristicaTipologiaIntervento c; // = caratteristicheIntervento.FirstOrDefault(x => x.IdCaratteristica == archCar.Id);
                if (m == null)
                {
                    c = new CaratteristicaTipologiaIntervento();
                    c.IdCaratteristica = archCar.Id;
                    c.CaratteristicaIntervento = archCar;
                }
                else
                {
                    c = new CaratteristicaTipologiaIntervento();
                    c.IdCaratteristica = m.IdCaratteristica;
                    c.CaratteristicaIntervento = m.CaratteristicaIntervento;
                    c.IdConfigurazione = Guid.NewGuid();
                    c.Parametri = m.Parametri;
                }
                caratteristicheToBind.Add(c);
            }

            rlbCaratteristicheIntervento.DataSource = caratteristicheToBind;
            rlbCaratteristicheIntervento.DataBind();
            rlbCaratteristicheIntervento.ClearChecked();
        }

        protected bool IsCaratteristicaChecked(object caratteristica)
        {
            bool value = false;

            if(caratteristica is Entities.CaratteristicaTipologiaIntervento)
            {
                if(((Entities.CaratteristicaTipologiaIntervento)caratteristica).IdConfigurazione == Guid.Empty)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return value;
        }







        protected void btSaveConfiguration_Click(object sender, EventArgs e)
        {
            SaveData();
        }


        protected void rgArchiveItems_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if(CodiceCliente == string.Empty)
            {
                rgArchiveItems.DataSource = new List<Entities.ConfigurazioneTipologiaTicketCliente>();
            }
            else
            {
                Logic.ConfigurazioniTipologieTicketCliente logic = new Logic.ConfigurazioniTipologieTicketCliente();
                rgArchiveItems.DataSource = logic.Read(CodiceCliente);
            }
        }

        protected void rgArchiveItems_ItemCreated(object sender, GridItemEventArgs e)
        {
            //Traduce le voci del menu di filtro della griglia. Questa operazione deve essere fatta ad ogni post
            Helper.TelerikHelper.TraduciElementiGriglia(e);
        }

        protected void rgArchiveItems_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                if (Guid.TryParse(editedItem.GetDataKeyValue("Id").ToString(), out Guid itemId))
                {
                    Logic.ConfigurazioniTipologieTicketCliente llArc = new Logic.ConfigurazioniTipologieTicketCliente();
                    Entities.ConfigurazioneTipologiaTicketCliente archiveItem = llArc.Find(new EntityId<Entities.ConfigurazioneTipologiaTicketCliente>(itemId));
                    if (archiveItem != null)
                    {
                        llArc.Delete(archiveItem, true);
                        InitializeForInsert();
                    }
                    else
                    {
                        string message = "radalert('La voce da eliminare non è stata trovata.', 330, 210, 'Errore');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", message, true);
                        e.Canceled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"radalert('Si è verificato un errore al salvataggio della voce di configurazione: {ex.Message.Replace("'", "")}', 330, 210 'Errore');";
                errorMessage = errorMessage.Replace("\n", "");
                errorMessage = errorMessage.Replace("\r", "");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", errorMessage, true);
                e.Canceled = true;
            }
        }

        protected void rgArchiveItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dataItem = rgArchiveItems.SelectedItems[0] as GridDataItem;
            if (dataItem != null)
            {
                IDConfigurazioneCorrente = new Guid(dataItem["Id"].Text);
                Logic.ConfigurazioniTipologieTicketCliente llArc = new Logic.ConfigurazioniTipologieTicketCliente();
                Entities.ConfigurazioneTipologiaTicketCliente archiveItem = llArc.Find(CurrentEntityIdentifier);
                ShowData(archiveItem);
                btSaveConfiguration.Text = "Aggiorna Configurazione";

                //var name = dataItem["ProductName"].Text;
                //Literal1.Text += String.Format("{0}<br/>", name);
            }
        }

        protected void rgArchiveItems_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if(e.CommandName == "Clone")
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                if (Guid.TryParse(editedItem.GetDataKeyValue("Id").ToString(), out Guid itemId))
                {
                    Logic.ConfigurazioniTipologieTicketCliente llArc = new Logic.ConfigurazioniTipologieTicketCliente();
                    Entities.ConfigurazioneTipologiaTicketCliente archiveItem = llArc.Find(new EntityId<Entities.ConfigurazioneTipologiaTicketCliente>(itemId));
                    if (archiveItem != null)
                    {
                        ShowData(archiveItem, true);
                        btSaveConfiguration.Text = "Salva Nuova Configurazione";
                    }
                }
            }
        }



        protected void rddlReparto_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            divOrariReparto.Visible = false;
            lblUfficioOrariReparto.Text = string.Empty;
            if (rddlReparto.SelectedValue != string.Empty)
            {
                Logic.RepartiUfficio llTI = new Logic.RepartiUfficio();
                Logic.RepartiUfficio llRU = new Logic.RepartiUfficio();
                Entities.RepartoUfficio repartoUfficio = llRU.Find(new Entities.EntityId<RepartoUfficio>(rddlReparto.SelectedValue));
                if (repartoUfficio != null)
                {
                    divOrariReparto.Visible = true;
                    lblUfficioOrariReparto.Text = $"<strong>Orari apertura {repartoUfficio.Reparto}</strong><br />";
                    foreach (Entities.OrarioRepartoUfficio orario in repartoUfficio.OrarioRepartoUfficios)
                    {
                        string nomeGiorno = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.DayNames[orario.Giorno];
                        string text = $"{nomeGiorno} dalle {orario.OrarioDalle:hh\\:mm} alle {orario.OrarioAlle:hh\\:mm}<br />";
                        lblUfficioOrariReparto.Text += text;
                    }
                }
            }
        }

        protected void rlbCaratteristicheIntervento_ItemDataBound(object sender, RadListBoxItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                var div = e.Item.FindControl("divParams");
                Entities.CaratteristicaTipologiaIntervento ci = (Entities.CaratteristicaTipologiaIntervento)e.Item.DataItem;
                if(Enum.IsDefined(typeof(Entities.CaratteristicaInterventoEnum), ci.IdCaratteristica))
                {
                    Entities.CaratteristicaInterventoEnum c = (Entities.CaratteristicaInterventoEnum)ci.IdCaratteristica;
                    switch(c)
                    {
                        case CaratteristicaInterventoEnum.PresaInCaricoEntroMinuti:
                        case CaratteristicaInterventoEnum.RipristinoEntroMinuti:
                        case CaratteristicaInterventoEnum.RispostaEntroMinuti:
                        case CaratteristicaInterventoEnum.RipristinoEntroMinutiDaPresaInCarico:
                            div.Visible = true;

                            int ore = 0;
                            int minuti = 0;
                            if (!string.IsNullOrWhiteSpace(ci.Parametri))
                            {
                                if (int.TryParse(ci.Parametri, out minuti))
                                {
                                    if (minuti >= 60)
                                    {
                                        ore = minuti / 60;
                                        minuti = minuti % 60;
                                    }
                                }
                            }

                            RadNumericTextBox rntbOre = (RadNumericTextBox)e.Item.FindControl("rntbOre");
                            RadNumericTextBox rntbMinuti = (RadNumericTextBox)e.Item.FindControl("rntbMinuti");
                            if (ore > 0) rntbOre.Value = ore; else rntbOre.Value = null;
                            if (minuti > 0) rntbMinuti.Value = minuti; else rntbMinuti.Value = null;

                            //RadTimePicker rtpTempo = (RadTimePicker)e.Item.FindControl("rtpTempo");
                            //if(rtpTempo != null)
                            //{
                            //    if (!string.IsNullOrWhiteSpace(ci.Parametri))
                            //    {
                            //        if (long.TryParse(ci.Parametri, out long ticks))
                            //        {
                            //            rtpTempo.SelectedTime = new TimeSpan(ticks);
                            //        }
                            //        else
                            //        {
                            //            rtpTempo.SelectedDate = null;
                            //            rtpTempo.SelectedTime = null;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        rtpTempo.SelectedDate = null;
                            //        rtpTempo.SelectedTime = null;
                            //    }
                            //}
                            break;

                        default:
                            div.Visible = false;
                            break;
                    }
                }
                else
                {
                    div.Visible = false;
                }
            }
        }


        protected void rcbModelli_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (rcbModelli.SelectedValue != string.Empty)
            {
                if (Guid.TryParse(rcbModelli.SelectedValue, out Guid idModello))
                {
                    FillList_CaratteristicheInterventoDaModello(idModello);
                    Logic.ModelliConfigurazioneCaratteristicheTicketCliente llM = new Logic.ModelliConfigurazioneCaratteristicheTicketCliente();
                    IQueryable<Entities.ModelloConfigurazioneCaratteristicheTicketCliente> configurazioni = llM.Read(idModello);
                }
            }
        }

        protected void rcbCliente_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(rcbCliente.SelectedValue.Trim()))
            {
                CodiceCliente = rcbCliente.SelectedValue.Trim();
            }
            else
            {
                CodiceCliente = string.Empty;
            }
            //rtbCodiceCliente.Text = CodiceCliente;

            rplConfigurator.Visible = CodiceCliente != string.Empty;

            InitializeForInsert();
            rgArchiveItems.Rebind();
        }

        protected void btSetCliente_Click(object sender, EventArgs e)
        {
            //CodiceCliente = rtbCodiceCliente.Text.Trim();
            InitializeForInsert();
            rgArchiveItems.Rebind();
        }

        protected void rcbOperatoreDiRiferimento_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

        }
    }
}