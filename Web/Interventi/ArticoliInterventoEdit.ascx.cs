using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SeCoGes.Utilities;

namespace SeCoGEST.Web.Interventi
{
    public partial class ArticoliInterventoEdit : System.Web.UI.UserControl
    {
        #region Properties

        /// <summary>
        /// Recupera o setta l'id dell'entity InterventoArticolo
        /// </summary>
        public Guid IDEntity
        {
            get
            {
                if (ViewState["IDEntity"] == null)
                {
                    ViewState["IDEntity"] = Guid.Empty;
                }

                return (Guid)ViewState["IDEntity"];
            }
            set
            {
                ViewState["IDEntity"] = value;
            }
        }

        public Entities.EntityId<Entities.Intervento_Articolo> CurrentEntityIdentifier
        {
            get
            {
                return new Entities.EntityId<Entities.Intervento_Articolo>(IDEntity);
            }
        }

        /// <summary>
        /// Recupera o setta l'IDIntervento dell'entity InterventoArticolo
        /// </summary>
        public Guid IDIntervento
        {
            get
            {
                if (ViewState["IDIntervento"] == null)
                {
                    ViewState["IDIntervento"] = Guid.Empty;
                }

                return (Guid)ViewState["IDIntervento"];
            }
            set
            {
                ViewState["IDIntervento"] = value;
            }
        }

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

        ///// <summary>
        ///// Recupera o setta l'IDRapportoAudit_NonConformita dell'entity RapportoAudit_Correzione
        ///// </summary>
        //public Guid IDRapportoAudit_NonConformita
        //{
        //    get
        //    {
        //        if (ViewState["IDRapportoAudit_NonConformita"] == null)
        //        {
        //            ViewState["IDRapportoAudit_NonConformita"] = Guid.Empty;
        //        }

        //        return (Guid)ViewState["IDRapportoAudit_NonConformita"];
        //    }
        //    set
        //    {
        //        ViewState["IDRapportoAudit_NonConformita"] = value;
        //    }
        //}

        ///// <summary>
        ///// Recupera o setta il valore del campo 'Tipo'
        ///// </summary>
        //public byte? Tipo
        //{
        //    get
        //    {
        //        byte? result = null;
        //        if (!String.IsNullOrEmpty(rcbTipo.SelectedValue))
        //        {
        //            // -1 = Nessun valore selezionato
        //            int selectedValue = -1;
        //            if (int.TryParse(rcbTipo.SelectedValue, out selectedValue) && selectedValue > -1)
        //            {
        //                result = (byte)selectedValue;
        //            }
        //        }

        //        return result;
        //    }
        //    set
        //    {
        //        if (value.HasValue)
        //        {
        //            RadComboBoxItem item = rcbTipo.FindItemByValue(value.Value.ToString());
        //            if (item != null)
        //            {
        //                item.Selected = true;
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Recupera o setta il valore del campo 'AzioneDefinita'
        ///// </summary>
        //public string AzioneDefinita
        //{
        //    get
        //    {
        //        return rtbAzioneDefinita.Text;
        //    }
        //    set
        //    {
        //        rtbAzioneDefinita.Text = value;
        //    }
        //}

        ///// <summary>
        ///// Recupera o setta il valore del campo 'AzioneEseguita'
        ///// </summary>
        //public string AzioneEseguita
        //{
        //    get
        //    {
        //        return rtbAzioneEseguita.Text;
        //    }
        //    set
        //    {
        //        rtbAzioneEseguita.Text = value;
        //    }
        //}

        ///// <summary>
        ///// Recupera o setta il valore del campo 'ResponsabileAttuazione'
        ///// </summary>
        //public string ResponsabileAttuazione
        //{
        //    get
        //    {
        //        return rtbResponsabileAttuazione.Text;
        //    }
        //    set
        //    {
        //        rtbResponsabileAttuazione.Text = value;
        //    }
        //}

        ///// <summary>
        ///// Recupera o setta il valore del campo 'ResponsabileVerifica'
        ///// </summary>
        //public string ResponsabileVerifica
        //{
        //    get
        //    {
        //        return rtbResponsabileVerifica.Text;
        //    }
        //    set
        //    {
        //        rtbResponsabileVerifica.Text = value;
        //    }
        //}

        ///// <summary>
        ///// Recupera o setta il valore del campo 'Termine'
        ///// </summary>
        //public DateTime? Termine
        //{
        //    get
        //    {
        //        return rdpTermine.SelectedDate;
        //    }
        //    set
        //    {
        //        rdpTermine.SelectedDate = value;
        //    }
        //}

        ///// <summary>
        ///// Recupera o setta il valore del campo 'DataAttuazione'
        ///// </summary>
        //public DateTime? DataAttuazione
        //{
        //    get
        //    {
        //        return rdpDataAttuazione.SelectedDate;
        //    }
        //    set
        //    {
        //        rdpDataAttuazione.SelectedDate = value;
        //    }
        //}

        ///// <summary>
        ///// Recupera o setta il valore del campo 'DataVerifica'
        ///// </summary>
        //public DateTime? DataVerifica
        //{
        //    get
        //    {
        //        return rdpDataVerifica.SelectedDate;
        //    }
        //    set
        //    {
        //        rdpDataVerifica.SelectedDate = value;
        //    }
        //}

        ///// <summary>
        ///// Recupera o setta il valore del campo 'Esito'
        ///// </summary>
        //public bool? Esito
        //{
        //    get
        //    {
        //        if (rbEsito.SelectedToggleState == null) return null;

        //        string valueString = rbEsito.SelectedToggleState.Value;
        //        if (valueString == "0")
        //        {
        //            return false;
        //        }
        //        else if (valueString == "1")
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    set
        //    {
        //        if (rbEsito.SelectedToggleState != null)
        //        {
        //            string valueString = String.Empty;
        //            if (!value.HasValue)
        //            {
        //                valueString = "-1";
        //            }
        //            else
        //            {
        //                if (value.Value)
        //                {
        //                    valueString = "1";
        //                }
        //                else
        //                {
        //                    valueString = "0";
        //                }
        //            }

        //            rbEsito.SetSelectedToggleStateByValue(valueString);
        //        }
        //    }
        //}

        ///// <summary>
        ///// Recupera o setta il valore del campo 'Costo'
        ///// </summary>
        //public decimal? Costo
        //{
        //    get
        //    {
        //        return (decimal?)rntbCosto.Value;
        //    }
        //    set
        //    {
        //        rntbCosto.Value = (double?)value;
        //    }
        //}

        ///// <summary>
        ///// Recupera o setta il valore del campo 'RecordEsistente'
        ///// </summary>
        //public bool RecordEsistente
        //{
        //    get
        //    {
        //        if (ViewState["RecordEsistente"] == null)
        //        {
        //            ViewState["RecordEsistente"] = false;
        //        }

        //        return (bool)ViewState["RecordEsistente"];
        //    }
        //    set
        //    {
        //        ViewState["RecordEsistente"] = value;
        //    }
        //}


        /// <summary>
        /// Restituisce lo stato di inizializzazione dell'usercontrol
        /// </summary>
        public bool IsInsertMode
        {
            get
            {
                if (ViewState["IsInsertMode"] == null)
                {
                    ViewState["IsInsertMode"] = false;
                }

                return (bool)ViewState["IsInsertMode"];
            }
            private set
            {
                ViewState["IsInsertMode"] = value;

            }
        }

        /// <summary>
        /// Restituisce lo stato di inizializzazione dell'usercontrol
        /// </summary>
        public bool IsLoaded
        {
            get
            {
                if (ViewState["IsLoaded"] == null)
                {
                    ViewState["IsLoaded"] = false;
                }

                return (bool)ViewState["IsLoaded"];
            }
            private set
            {
                ViewState["IsLoaded"] = value;

            }
        }

        #endregion

        #region Eventi

        public event EventHandler Saved;
        public event EventHandler Canceled;

        #endregion

        #region Metodi di gestione

        /// <summary>
        /// Visualizza i dati dell'entity passata come parametro
        /// </summary>
        public void InitializeForInsert(Guid idIntervento)
        {
            IsInsertMode = true;
            IDIntervento = idIntervento;
            IDEntity = Guid.Empty;

            rcbProvenienzaArticolo.ClearSelection();
            rcbProvenienzaArticolo.SelectedValue = "";
            rcbProvenienzaArticolo.Text = "";
            FillComboProvenienzaArticoli();

            rcbArticoli.ClearSelection();
            rcbArticoli.SelectedValue = "";
            rcbArticoli.Text = "";

            rtbDescrizionePersonalizzataArticolo.Text = String.Empty;
            rmtbTempoImpiegato.Text = String.Empty;
            rntbQuantita.Value = null;

            chkDaFatturare.Checked = true;

            rtbNote.Text = string.Empty;
        }

        /// <summary>
        /// Visualizza i dati dell'entity passata come parametro
        /// </summary>
        /// <param name="valoreCorrente"></param>
        public void ShowData(Entities.Intervento_Articolo entity)
        {
            if (entity == null) return;

            IsInsertMode = false;

            IDEntity = entity.ID;
            IDIntervento = entity.IDIntervento;


            FillComboProvenienzaArticoli();
            RadComboBoxItem selItemProvArt = rcbProvenienzaArticolo.FindItemByValue(entity.IDRicercaInProvenienzaArticoli);
            if (selItemProvArt != null) selItemProvArt.Selected = true;


            RadComboBoxItem selItemArt = new RadComboBoxItem(entity.CodiceArticolo, entity.IDRicercaInElencoCompletoArticoli);
            selItemArt.Selected = true;
            rcbArticoli.Items.Add(selItemArt);

            rtbDescrizionePersonalizzataArticolo.Text = entity.Descrizione;

            if (!String.IsNullOrEmpty(entity.OreTime))
            {
                string[] tempo = entity.OreTime.Split(':');
                rmtbTempoImpiegato.TextWithLiterals = String.Format("{0}:{1}", tempo[0], tempo[1]);
            }
            else
            {
                rmtbTempoImpiegato.Text = String.Empty;
            }

            rntbQuantita.Value = entity.Quantita;

            if (!String.IsNullOrEmpty(entity.OreTime) && !entity.Quantita.HasValue)
            {
                rntbQuantita.Enabled = false;
                rmtbTempoImpiegato.Enabled = true;
            }
            else if (String.IsNullOrEmpty(entity.OreTime) && entity.Quantita.HasValue)
            {
                rmtbTempoImpiegato.Enabled = false;
                rntbQuantita.Enabled = true;
            }

            //if (entity.TipologiaArticolo == 4)
            //{
            //    lcTempo.Style.Add("display", "none");
            //    lcQuantita.Style.Add("display", "block");
            //}
            //else
            //{
            //    lcTempo.Style.Add("display", "block");
            //    lcQuantita.Style.Add("display", "none");
            //}


            chkDaFatturare.Checked = entity.DaFatturare.HasValue ? entity.DaFatturare.Value : true;

            rtbNote.Text = entity.Note;
        }

        /// <summary>
        /// Salva nel database i dati presenti nell'interfaccia grafica
        /// </summary>
        public bool SaveData()
        {
            bool isNew = !CurrentEntityIdentifier.HasValue;

            // Valida i dati nella pagina
            SeCoGes.Utilities.MessagesCollector erroriDiValidazione = ValidaDati(new string[] { "rfvRichiesteProblematicheRiscontrate" });
            if (erroriDiValidazione.HaveMessages)
            {
                // Se qualcosa non va bene mostro un avviso all'utente
                throw new Exception(erroriDiValidazione.ToString("<br />"));
            }

            try
            {
                Logic.Intervento_Articoli ll = new Logic.Intervento_Articoli();
                Entities.Intervento_Articolo entityToSave = null;
                //Se CurrentId contiene un ID allora cerco l'entity nel database
                if (!isNew)
                {
                    entityToSave = ll.Find(new Entities.EntityId<Entities.Intervento_Articolo>(IDEntity));
                }

                // Definisco una variabile che indica se si deve compiere una azione di creazione di una nuova entity o di modifica
                bool nuovo = false;

                if (entityToSave == null)
                {
                    nuovo = true;
                    entityToSave = new Entities.Intervento_Articolo();
                    // Legge i dati inseriti nell'interfaccia e li inserisce nell'entity
                    EstraiValoriDallaView(entityToSave, true);
                    entityToSave.ID = Guid.NewGuid();

                    //Creo nuova entity tramite una stored procedure
                    ll.Create(entityToSave, false);
                }
                else
                {
                    EstraiValoriDallaView(entityToSave, false);
                }
                ll.SubmitToDatabase();

                SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - {1} l'entity Intervento:Articolo con Codice '{2}'.", Request.Url.AbsolutePath, ((nuovo) ? "Creato" : "Salvato"), entityToSave.CodiceArticolo));

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
        public SeCoGes.Utilities.MessagesCollector ValidaDati(params string[] validatorNamesToExclusde)
        {
            SeCoGes.Utilities.MessagesCollector messaggi = new SeCoGes.Utilities.MessagesCollector();

            Page.Validate();
            if (!Page.IsValid)
            {
                List<string> validationErrors = new List<string>();
                foreach(BaseValidator validator in Page.Validators)
                {
                    if (!validatorNamesToExclusde.Contains(validator.ID))
                    {
                        if(!validator.IsValid)
                        {
                            validationErrors.Add(validator.ErrorMessage);
                        }
                    }
                }
                if(validationErrors.Any()) messaggi.Add("Attenzione, sono presenti alcuni errori nei dati inseriti! Correggerli e riprovare." + Environment.NewLine + string.Join(Environment.NewLine, validationErrors));
            }

            return messaggi;
        }

        /// <summary>
        /// Legge i dati inseriti nell'interfaccia e li inserisce nell'entity passata
        /// </summary>
        /// <param name="entityToFill"></param>
        /// <param name="isNew"></param>
        public string EstraiValoriDallaView(Entities.Intervento_Articolo entityToFill, bool isNew)
        {
            if (entityToFill == null)
            {
                entityToFill = new Entities.Intervento_Articolo();
            }
            if (isNew)
            {
                entityToFill.ID = IDEntity;
                entityToFill.IDIntervento = IDIntervento;
            }

            //if (isNew && !IsInsertMode)
            //{
            //    entityToFill.IDRapportoAudit_NonConformita = IDRapportoAudit_NonConformita;
            //}
            //else
            //{
            //    Guid idRapportoAudit_NonConformita = Guid.Empty;
            //    Guid.TryParse(rcbRapportoAuditNonConformita.SelectedValue, out idRapportoAudit_NonConformita);

            //    entityToFill.IDRapportoAudit_NonConformita = idRapportoAudit_NonConformita;
            //}



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

            entityToFill.DaFatturare = chkDaFatturare.Checked;
            entityToFill.Note = rtbNote.Text.Trim();

            if (!rntbQuantita.Value.HasValue && 
                !String.IsNullOrEmpty(rmtbTempoImpiegato.TextWithLiterals) &&
                rmtbTempoImpiegato.TextWithLiterals.Trim() != ":00")
            {
                string[] elencoTempi = rmtbTempoImpiegato.TextWithLiterals.Split(new string[] { ":" }, StringSplitOptions.None);


                int hours = 0;
                int.TryParse(elencoTempi[0], out hours);

                int minutes = 0;
                int.TryParse(elencoTempi[1], out minutes);

                if (hours <= 0 && minutes <= 0)
                {
                    entityToFill.OreTime = String.Empty;
                    entityToFill.Ore = 0;
                }
                else
                {
                    entityToFill.OreTime = String.Format("{0:00}:{1:00}:00", hours, minutes);

                    TimeSpan tsTempoImpiegato = new TimeSpan(hours, minutes, 0);
                    entityToFill.Ore = Convert.ToDecimal(tsTempoImpiegato.TotalHours);
                }
            }
            else
            {
                entityToFill.OreTime = String.Empty;
                entityToFill.Ore = 0;
            }

            entityToFill.Quantita = (int?)rntbQuantita.Value;

            // In base al fatto che sia stata indicata una Quantità oppure un Tempo controlla che in Metodo all'articolo in questione sia stata associata la corretta Unità di Musura
            if (entityToFill.OreTime != string.Empty)
            {
                // Se è stato specificato un Tempo l'articolo deve avere l'unità di misura "ORA"
                if (!llArts.UnitaDiMisuraAssociata(entityToFill.CodiceArticolo, "ORA"))
                {
                    MessageHelper.ShowErrorMessage(Page, "All'articolo indicato non è associata l'Unità di Misura 'ORA'.");
                }
            }
            if (entityToFill.Quantita.HasValue)
            {
                // Se è stato specificato una Quantita l'articolo deve avere l'unità di misura "NR"
                if (!llArts.UnitaDiMisuraAssociata(entityToFill.CodiceArticolo, "NR"))
                {
                    MessageHelper.ShowErrorMessage(Page, "All'articolo indicato non è associata l'Unità di Misura 'NR'.");
                }
            }

            return string.Empty;
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Scatena l'evento Saved
        /// </summary>
        private void RaiseSaved()
        {
            if (Saved != null)
            {
                Saved(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Scatena l'evento Canceled
        /// </summary>
        private void RaiseCanceled()
        {
            if (Canceled != null)
            {
                Canceled(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Effettua il popolamento del combo Tipo
        /// </summary>
        private void FillComboProvenienzaArticoli()
        {
            lblProvenienzaArticolo.Text = string.Format("Provenienza articoli (Cliente: {0})", CodiceCliente);

            rcbProvenienzaArticolo.Items.Clear();
            rcbProvenienzaArticolo.ClearSelection();

            RadComboBoxItem rcbi = new RadComboBoxItem("Articoli di Magazzino", "0");
            rcbProvenienzaArticolo.Items.Add(rcbi);

            Logic.Interventi ll = new Logic.Interventi();
            IEnumerable<Entities.InformazioniContratto> elencoInfoContratti = ll.GetContrattiPerCliente(CodiceCliente, DataIntervento);

            foreach(Entities.InformazioniContratto infoContratto in elencoInfoContratti) //.Where(x => x.TipologiaArticolo > 0))
            {
                rcbi = new RadComboBoxItem(infoContratto.DescrizioneProvenienza, infoContratto.IDUnivoco);
                rcbProvenienzaArticolo.Items.Add(rcbi);
            }

            rcbProvenienzaArticolo.DataBind();
        }

        #endregion

        protected void rbSalvaValore_Click(object sender, EventArgs e)
        {
            try
            {
                SaveData();
                RaiseSaved();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(Page, ex.Message);
            }
        }

        protected void rbAnnullaModificaValore_Click(object sender, EventArgs e)
        {
            RaiseCanceled();
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
                    string provenienzaArt = e.Context["ProvenienzaArticolo"].ToString().Substring(0,1);
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
                    queryBase = queryBase.Where(x => (x.CodiceArticolo).ToLower().Contains(testoRicerca));
                    //queryBase = queryBase.Where(x => (x.CodiceArticolo + " - " + x.Descrizione).ToLower().Contains(testoRicerca));
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
                    case 1: tipo = "Articoli del Contratto";
                        break;
                    case 2: tipo = "Articoli Prepagati";
                        break;
                    case 3: tipo = "Tariffe Standard";
                        break;
                    case 4: tipo = "Addebiti";
                        break;
                    default: tipo = "Articoli";
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
    }
}