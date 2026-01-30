using SeCoGEST.Entities;
using SeCoGEST.Helper;
using SeCoGEST.Web;
using SeCoGEST.Web.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SeCoGEST.Web.Progetti
{
    public partial class AttivitaProgetto : System.Web.UI.UserControl
    {
        public Guid? IDProgetto
        {
            get
            {
                if(ViewState["IDProgetto"] == null)
                    return null;
                else
                    return (Guid)ViewState["IDProgetto"];
            }
            set
            {
                ViewState["IDProgetto"] = value;
            }
        }


        Logic.Operatori _logicOperatori = null;
        Logic.Operatori LogicOperatori
        {
            get
            {
                if (_logicOperatori == null) _logicOperatori = new Logic.Operatori();
                return _logicOperatori;
            }
        }


        public class AttivitaSelezionabile
        {
            public bool Selezionato { get; set; }
            public string DescrizioneAttivita { get; set; }
        }





        protected void Page_Load(object sender, EventArgs e)
        {
            //Traduce le voci del menu di filtro della griglia. Questa operazione deve essere fatta ad ogni post
            TelerikHelper.TraduciMenuFiltro(rgAttivita.FilterMenu);

            //if (!this.Page.IsPostBack && !this.Page.IsCallback)
            //{
            //    var attivitaProgetto = GetElencoAttivitàProgettoConAllegati();
            //    rgAttivita.DataSource = attivitaProgetto;
            //    rgAttivita.DataBind();
            //}
        }


        protected void rgAttivita_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            rgAttivita.DataSource = GetElencoAttivitàProgettoConAllegati();
        }

        protected void rgAttivita_ItemCreated(object sender, GridItemEventArgs e)
        {
            //Traduce le voci del menu di filtro della griglia. Questa operazione deve essere fatta ad ogni post
            TelerikHelper.TraduciElementiGriglia(e);
        }

        protected void rgAttivita_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
            {
                GridEditFormItem dataItem = (GridEditFormItem)e.Item;
                try
                {
                    Entities.Progetto_AttivitaConAllegati entity = null;
                    if (dataItem != null && dataItem.DataItem is Entities.Progetto_AttivitaConAllegati)
                    {
                        entity = (Entities.Progetto_AttivitaConAllegati)dataItem.DataItem;
                    }


                    RadComboBox rcbOperatoreAssegnato = (RadComboBox)dataItem.FindControl("rcbOperatoreAssegnato");
                    if (rcbOperatoreAssegnato != null)
                    {
                        rcbOperatoreAssegnato.DataSource = LogicOperatori.ReadActive();
                        rcbOperatoreAssegnato.DataBind();
                        TelerikHelper.InsertBlankComboBoxItem(rcbOperatoreAssegnato);

                        if (entity != null && entity.IDOperatoreAssegnato.HasValue)
                        {
                            RadComboBoxItem item = rcbOperatoreAssegnato.FindItemByValue(entity.IDOperatoreAssegnato.ToString());
                            if(item != null)
                            {
                                item.Selected = true;
                            }
                            else
                            {
                                Entities.Operatore opAssegnato = LogicOperatori.Find(new EntityId<Entities.Operatore>(entity.IDOperatoreAssegnato));
                                if(opAssegnato != null)
                                {
                                    RadComboBoxItem newItem = new RadComboBoxItem(opAssegnato.CognomeNome, opAssegnato.ID.ToString());
                                    rcbOperatoreAssegnato.Items.Add(newItem);
                                    newItem.Selected = true;
                                }
                            }
                        }
                    }


                    RadComboBox rcbOperatoreEsecutore = (RadComboBox)dataItem.FindControl("rcbOperatoreEsecutore");
                    if (rcbOperatoreEsecutore != null)
                    {
                        rcbOperatoreEsecutore.DataSource = LogicOperatori.ReadActive();
                        rcbOperatoreEsecutore.DataBind();
                        TelerikHelper.InsertBlankComboBoxItem(rcbOperatoreEsecutore);

                        if (entity != null && entity.IDOperatoreEsecutore.HasValue)
                        {
                            RadComboBoxItem item = rcbOperatoreEsecutore.FindItemByValue(entity.IDOperatoreEsecutore.ToString());
                            if (item != null)
                            {
                                item.Selected = true;
                            }
                            else
                            {
                                Entities.Operatore opEsecutore = LogicOperatori.Find(new EntityId<Entities.Operatore>(entity.IDOperatoreEsecutore));
                                if (opEsecutore != null)
                                {
                                    RadComboBoxItem newItem = new RadComboBoxItem(opEsecutore.CognomeNome, opEsecutore.ID.ToString());
                                    rcbOperatoreEsecutore.Items.Add(newItem);
                                    newItem.Selected = true;
                                }
                            }
                        }
                    }


                    RadComboBox rcbTicket = (RadComboBox)dataItem.FindControl("rcbTicket");
                    if (rcbTicket != null)
                    {
                        if (entity != null && entity.IDTicket.HasValue)
                        {
                            rcbTicket.Text = entity.Intervento.Numero.ToString();
                            rcbTicket.SelectedValue = entity.Intervento.ID.ToString();
                        }
                    }


                    RadComboBox rcbStato = (RadComboBox)dataItem.FindControl("rcbStato");
                    if (rcbStato != null)
                    {
                        if(rcbStato.Items.Count() == 0)
                        {
                            rcbStato.Items.Add(new RadComboBoxItem("Da Eseguire", StatoAttivitaProgettoEnum.DaEseguire.GetHashCode().ToString()));
                            rcbStato.Items.Add(new RadComboBoxItem("In Gestione", StatoAttivitaProgettoEnum.InGestione.GetHashCode().ToString()));
                            rcbStato.Items.Add(new RadComboBoxItem("Eseguito", StatoAttivitaProgettoEnum.Eseguito.GetHashCode().ToString()));
                            rcbStato.Items.Add(new RadComboBoxItem("Modificato rispetto a contratto", StatoAttivitaProgettoEnum.Modificato.GetHashCode().ToString()));
                        }
                        if (entity != null)
                        {
                            RadComboBoxItem item = rcbStato.FindItemByValue(entity.Stato.ToString());
                            if (item != null)
                            {
                                item.Selected = true;
                            }
                        }
                    }


                    Logic.Progetto_Attività llPA = new Logic.Progetto_Attività();
                    Repeater rptAllegatiAttivita = (Repeater)dataItem.FindControl("rptAllegatiAttivita");
                    rptAllegatiAttivita.DataSource = llPA.GetAllegati(entity.ID);
                    rptAllegatiAttivita.DataBind();
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowErrorMessage(Page, ex);
                }
            }






            else if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                if(dataItem.DataItem is Entities.Progetto_AttivitaConAllegati)
                {
                    HyperLink hlOpenTicket = (HyperLink)dataItem.FindControl("hlOpenTicket");
                    if(hlOpenTicket != null)
                    {
                        Entities.Progetto_AttivitaConAllegati entity = (Entities.Progetto_AttivitaConAllegati)dataItem.DataItem;
                        if(entity.IDTicket != null)
                        {
                            hlOpenTicket.Target = "_blank";
                            hlOpenTicket.Text = entity.Ticket.Numero.ToString();
                            hlOpenTicket.NavigateUrl = $"/Interventi/Intervento.aspx?ID={entity.IDTicket}";
                        }
                        else
                        {
                            hlOpenTicket.Target = "_self";
                            hlOpenTicket.Text = "Nuovo Ticket";

                            if(entity.Descrizione.Trim() == string.Empty)
                            {
                                hlOpenTicket.Enabled = false;
                            }
                            else
                            {
                                hlOpenTicket.NavigateUrl = $"/Interventi/Intervento.aspx?AttivitaId={entity.ID}";
                            }
                        }
                    }
                }


            }
        }

        protected void rgAttivita_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item is GridDataItem dataItem && 
                (e.CommandName == "MoveUp" || e.CommandName == "MoveDown"))
            {
                RadGrid griglia = (RadGrid)sender;
                int index = int.Parse(e.CommandArgument.ToString());
                GridDataItem rigaCliccata = griglia.MasterTableView.Items[index];

                string artID = rigaCliccata.GetDataKeyValue("ID").ToString();
                if (Guid.TryParse(artID, out Guid attivitaID))
                {
                    Logic.Progetto_Attività llPA = new Logic.Progetto_Attività();
                    if (e.CommandName == "MoveUp")
                    {
                        bool updateGrid = llPA.ChangeOrder(new EntityId<Progetto_Attivita>(attivitaID), true, true);
                        if (updateGrid && sender is RadGrid grid) grid.Rebind();
                    }
                    else if (e.CommandName == "MoveDown")
                    {
                        bool updateGrid = llPA.ChangeOrder(new EntityId<Progetto_Attivita>(attivitaID), false, true);
                        if (updateGrid && sender is RadGrid grid) grid.Rebind();
                    }
                }
            }
        }

        protected void rgAttivita_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem ei = ((GridEditableItem)(e.Item));
            if (Guid.TryParse(ei.GetDataKeyValue("ID").ToString(), out Guid itemId))
            {

                RadDatePicker rdpDataInserimento = (RadDatePicker)ei.FindControl("rdpDataInserimento");
                RadDatePicker rdpDataInizio = (RadDatePicker)ei.FindControl("rdpDataInizio");
                RadTimePicker rtpOraInizio = (RadTimePicker)ei.FindControl("rtpOraInizio");
                RadDatePicker rdpDataFine = (RadDatePicker)ei.FindControl("rdpDataFine");
                RadTimePicker rtpOraFine = (RadTimePicker)ei.FindControl("rtpOraFine");
                RadTextBox rtbNome = (RadTextBox)ei.FindControl("rtbNome");
                RadDateTimePicker rdtpScadenza = (RadDateTimePicker)ei.FindControl("rdtpScadenza");
                RadComboBox rcbOperatoreAssegnato = (RadComboBox)ei.FindControl("rcbOperatoreAssegnato");
                RadComboBox rcbOperatoreEsecutore = (RadComboBox)ei.FindControl("rcbOperatoreEsecutore");
                RadComboBox rcbTicket = (RadComboBox)ei.FindControl("rcbTicket");
                RadComboBox rcbStato = (RadComboBox)ei.FindControl("rcbStato");
                RadTextBox rtbNoteContratto = (RadTextBox)ei.FindControl("rtbNoteContratto");
                RadTextBox rtbNoteOperatore = (RadTextBox)ei.FindControl("rtbNoteOperatore");
                Repeater rptAllegatiAttivita = (Repeater)ei.FindControl("rptAllegatiAttivita");

                Logic.Progetto_Attività llPA = new Logic.Progetto_Attività();
                Entities.Progetto_Attivita pa = llPA.Find(itemId);
                if(pa != null)
                {
                    Guid? idOperatoreAssegnatoPrecedente = pa.IDOperatoreAssegnato;
                    Guid? idOperatoreEsecutorePrecedente = pa.IDOperatoreEsecutore;

                    pa.DataInserimento = rdpDataInserimento.SelectedDate.Value;
                    pa.DataInizio = rdpDataInizio.SelectedDate;
                    pa.OraInizio = rtpOraInizio.SelectedTime;
                    pa.DataFine = rdpDataFine.SelectedDate;
                    pa.OraFine = rtpOraFine.SelectedTime;
                    pa.Descrizione = rtbNome.Text.Trim();
                    pa.NoteContratto = rtbNoteContratto.Text.Trim();
                    pa.NoteOperatore = rtbNoteOperatore.Text.Trim();
                    pa.Scadenza = rdtpScadenza.SelectedDate;

                    Guid? idOperatoreAssegnatoSelezionato = null;
                    if (Guid.TryParse(rcbOperatoreAssegnato.SelectedValue, out Guid IDA))
                    {
                        pa.IDOperatoreAssegnato = IDA;
                        idOperatoreAssegnatoSelezionato = IDA;
                    }
                    else
                    {
                        pa.IDOperatoreAssegnato = null;
                    }

                    Guid? idOperatoreEsecutoreSelezionato = null;
                    if (Guid.TryParse(rcbOperatoreEsecutore.SelectedValue, out Guid IDE))
                    {
                        pa.IDOperatoreEsecutore = IDE;
                        idOperatoreEsecutoreSelezionato = IDE;
                    }
                    else
                    {
                        pa.IDOperatoreEsecutore = null;
                    }

                    if (byte.TryParse(rcbStato.SelectedValue, out byte sta))
                    {
                        pa.Stato = sta;
                    }
                    else
                    {
                        pa.Stato = 0;
                    }

                    if (Guid.TryParse(rcbTicket.SelectedValue, out Guid IDT))
                    {
                        pa.IDTicket = IDT;
                    }
                    else
                    {
                        pa.IDTicket = null;
                    }

                    llPA.SubmitToDatabase();

                    InviaNotificaCambioOperatoriAttivita(pa, idOperatoreAssegnatoPrecedente, idOperatoreAssegnatoSelezionato, idOperatoreEsecutorePrecedente, idOperatoreEsecutoreSelezionato);


                    var llA = new Logic.Allegati(llPA);

                    // Recupero l'elenco degli allegati attualmente associati all'attività corrente
                    var allegatiAttuali = llPA.GetAllegati(pa.ID);

                    // Recupero del controllo di upload per la registrazione degli eventuali nuovi allegati
                    var rauAllegati = (RadAsyncUpload)ei.FindControl("rauAllegati");
                    if (rauAllegati != null && rauAllegati.UploadedFiles.Count > 0)
                    {
                        foreach (UploadedFile file in rauAllegati.UploadedFiles)
                        {
                            // Salva il nuovo allegato e lo associa all'attività
                            //SalvaAllegatoAttivita(pa, file, llPA);
                            Entities.Allegato nuovoAllegato = CreaFileAllegato(file, pa);
                            llA.Create(nuovoAllegato, true);
                        }
                    }

                    // Ora controlla l'eventuale rimozione di allegati. 
                    foreach (Allegato allegatoAttuale in allegatiAttuali)
                    {
                        // Se l'allegato non è presente fra quelli presenti nel repeater allora vuol dire che l'utente l'ha rimosso...
                        bool trovato = false;
                        foreach (RepeaterItem item in rptAllegatiAttivita.Items)
                        {
                            HiddenField hfIdAllegato = (HiddenField)item.FindControl("hfIdAllegato");
                            if (hfIdAllegato != null)
                            {
                                if (hfIdAllegato.Value.Equals(allegatoAttuale.ID.ToString(), StringComparison.OrdinalIgnoreCase))
                                {
                                    trovato = true;
                                    break;
                                }
                            }
                        }
                        if(!trovato) {
                            // Viene recuperato la relazione del documento da cancellare ..
                            Entities.Allegato entityAllegatoDaCancellare = llA.Find(allegatoAttuale.ID);

                            // Nel caso in cui l'entity di relazione esista ..
                            if (entityAllegatoDaCancellare != null)
                            {
                                // Infine viene utilizzato il logiclayer per eliminare l'allegato da cancellare
                                llA.Delete(entityAllegatoDaCancellare, true);
                            }
                        }
                    }
                }
            }
        }

        private void InviaNotificaCambioOperatoriAttivita(Entities.Progetto_Attivita attivita, Guid? idOperatoreAssegnatoPrecedente, Guid? idOperatoreAssegnatoSelezionato, Guid? idOperatoreEsecutorePrecedente, Guid? idOperatoreEsecutoreSelezionato)
        {
            if (attivita == null)
            {
                return;
            }

            Dictionary<Guid, List<string>> operatoriDaNotificare = new Dictionary<Guid, List<string>>();

            if (idOperatoreAssegnatoPrecedente != idOperatoreAssegnatoSelezionato && idOperatoreAssegnatoSelezionato.HasValue)
            {
                operatoriDaNotificare[idOperatoreAssegnatoSelezionato.Value] = new List<string> { "operatore assegnato" };
            }

            if (idOperatoreEsecutorePrecedente != idOperatoreEsecutoreSelezionato && idOperatoreEsecutoreSelezionato.HasValue)
            {
                if (!operatoriDaNotificare.ContainsKey(idOperatoreEsecutoreSelezionato.Value))
                {
                    operatoriDaNotificare[idOperatoreEsecutoreSelezionato.Value] = new List<string>();
                }

                operatoriDaNotificare[idOperatoreEsecutoreSelezionato.Value].Add("operatore esecutore");
            }

            if (operatoriDaNotificare.Count == 0)
            {
                return;
            }

            foreach (KeyValuePair<Guid, List<string>> operatoreDaNotificare in operatoriDaNotificare)
            {
                Entities.Operatore operatore = LogicOperatori.Find(new EntityId<Entities.Operatore>(operatoreDaNotificare.Key));
                if (operatore != null)
                {
                    EmailManager.InviaEmailAssegnazioneAttivitaProgetto(attivita, operatore, operatoreDaNotificare.Value);
                }
            }
        }


        private Entities.Allegato CreaFileAllegato(UploadedFile fileCaricato, Entities.Progetto_Attivita attivita)
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

            entityToCreate.IDLegame = attivita.ID;
            entityToCreate.TipologiaAllegatoEnum = TipologiaAllegatoEnum.AttivitaProgetto;
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
        /// Metodo di gestione dell'evento AjaxRequest relativo al pannello ajax della tab Allegati
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rapAllegati_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            DocumentazioneAllegata ucDocumentazioneAllegata = sender as DocumentazioneAllegata;
            if (!IDProgetto.HasValue)
            {
                ucDocumentazioneAllegata.IsReadOnly = true;
            }
            ucDocumentazioneAllegata.Inizializza();
        }


        protected void rptAllegatiAttivita_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Reference the Repeater Item.
                RepeaterItem item = e.Item;

                //Reference the data value.
                if (item.DataItem is Entities.Allegato && item.DataItem != null)
                {
                    Guid attachmentId = ((Entities.Allegato)item.DataItem).ID;
                    string fileName = ((Entities.Allegato)item.DataItem).NomeFile;

                    //Reference the Controls.
                    HyperLink hlDownload = item.FindControl("hlDownload") as HyperLink;
                    if (hlDownload != null)
                    {
                        hlDownload.NavigateUrl = $"~/DownloaderAllegato.aspx?ID={attachmentId}&Name={fileName}";
                    }
                }
            }
        }

        protected void rptAllegatiAttivita_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                Repeater rptAllegatiAttivita = source as Repeater;
                // Ottieni la lista attuale dal Repeater
                var items = new List<dynamic>();
                foreach (RepeaterItem item in rptAllegatiAttivita.Items)
                {
                    HyperLink hlDownload = (HyperLink)item.FindControl("hlDownload");
                    HiddenField hfIdAllegato = (HiddenField)item.FindControl("hfIdAllegato");

                    if (hlDownload != null && hfIdAllegato != null)
                    {
                        // Mantieni solo quelli diversi dal CommandArgument
                        if (!hfIdAllegato.Value.Equals(e.CommandArgument.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            items.Add(new { ID = hfIdAllegato.Value, NomeFile = hlDownload.Text });
                        }
                    }
                }

                // Aggiorna il Repeater con la lista senza l'elemento rimosso
                rptAllegatiAttivita.DataSource = items;
                rptAllegatiAttivita.DataBind();
            }
        }

        protected void rgAttivita_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                GridDataItem editedItem = e.Item as GridDataItem;
                if (Guid.TryParse(editedItem.GetDataKeyValue("ID").ToString(), out Guid itemId))
                {
                    Logic.Progetto_Attività llArc = new Logic.Progetto_Attività();
                    Entities.Progetto_Attivita archiveItem = llArc.Find(new EntityId<Progetto_Attivita>(itemId));
                    if (archiveItem != null)
                    {
                        llArc.Delete(archiveItem, true);
                    }
                    else
                    {
                        string message = "radalert('Attività da rimuovere non trovata.', 330, 210, 'Errore');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", message, true);
                        e.Canceled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"radalert('Si è verificato un errore: {ex.Message.Replace("'", "")}', 330, 210 'Errore');";
                errorMessage = errorMessage.Replace("\n", "");
                errorMessage = errorMessage.Replace("\r", "");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", errorMessage, true);
                e.Canceled = true;
            }
        }



        private List<Entities.Attivita> GetArchivioAttività()
        {
            return new Logic.Attività().Read().ToList();
        }

        private List<Entities.Progetto_Attivita> GetElencoAttivitàProgetto()
        {
            if (IDProgetto.HasValue)
            {
                Logic.Progetto_Attività llPA = new Logic.Progetto_Attività();
                return llPA.Read(IDProgetto.Value).ToList();
            }
            else
                return new List<Entities.Progetto_Attivita>();
        }

        private List<Entities.Progetto_AttivitaConAllegati> GetElencoAttivitàProgettoConAllegati()
        {
            if(IDProgetto.HasValue)
            {
                Logic.Progetto_Attività llPA = new Logic.Progetto_Attività();
                return llPA.ReadWithAttachments(IDProgetto.Value).ToList();
            }
            else
                return new List<Entities.Progetto_AttivitaConAllegati>();
        }

        protected void SelezioneAttivitaToolbar_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {
            if (IDProgetto.HasValue)
            {
                RadToolBarButton clickedButton = (RadToolBarButton)e.Item;
                if (clickedButton.CommandName == "Import")
                {

                    List<Entities.Progetto_AttivitaConAllegati> attivitàProgetto = GetElencoAttivitàProgettoConAllegati();
                    int numAttivitàProgetto = 0;
                    if(attivitàProgetto.Count() > 0) numAttivitàProgetto = attivitàProgetto.Max(ap => ap.Ordine);

                    bool aggiornata = false;

                    Logic.Progetto_Attività llPA = new Logic.Progetto_Attività();
                    foreach (RepeaterItem item in repAttivita.Items)
                    {
                        CheckBox chk = (CheckBox)item.FindControl("chkAttivita");
                        if (chk != null && chk.Checked)
                        {
                            //if (!attivitàProgetto.Any(ap => ap.Descrizione == chk.Text)) // NON AGGIUNGE LE DESCRIZIONI GIA' PRESENTI
                            //{
                                numAttivitàProgetto++;
                                Entities.Progetto_Attivita pa = new Entities.Progetto_Attivita();
                                pa.ID = Guid.NewGuid();
                                pa.IDProgetto = IDProgetto.Value;
                                pa.DataInserimento = DateTime.Now.Date;
                                pa.Descrizione = chk.Text;
                                pa.Stato = 0;
                                pa.Ordine = (short)numAttivitàProgetto;
                                llPA.Create(pa, true);

                                aggiornata = true;
                            //}
                        }
                    }

                    if(aggiornata) rgAttivita.Rebind();
                }
            }

            SelezioneAttivitaWindow.VisibleOnPageLoad = false;
        }

        protected void rapWindow_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            SelezioneAttivitaWindow.VisibleOnPageLoad = true;

            repAttivita.DataSource = GetArchivioAttività()
                .Select(attivita => new AttivitaSelezionabile
                {
                    DescrizioneAttivita = attivita.Descrizione
                    // Selezionato resta false di default
                })
                .ToList();

            repAttivita.DataBind();

            //List<AttivitaSelezionabile> attivitaSelezionabili = new List<AttivitaSelezionabile>();
            //foreach (Entities.Attivita attivita in GetArchivioAttività())
            //{
            //    AttivitaSelezionabile attSel = new AttivitaSelezionabile();
            //    attSel.DescrizioneAttivita = attivita.Descrizione;
            //    attSel.Selezionato = GetElencoAttivitàProgetto().Any(a => a.Descrizione == attivita.Descrizione);
            //    //attSel.Selezionato = GetElencoAttivitàProgettoConAllegati().Any(a => a.Descrizione == attivita.Descrizione);
            //    attivitaSelezionabili.Add(attSel);
            //}

            //repAttivita.DataSource = attivitaSelezionabili;
            //repAttivita.DataBind();
        }

        protected void rcbTicket_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                string testoRicerca = (e.Text == null) ? String.Empty : e.Text.ToLower();
                RadComboBox combo = (RadComboBox)sender;
                combo.Items.Clear();

                //Carica tutti i tickets che contengono il testo digitato dall'utente
                Logic.Interventi ll = new Logic.Interventi();
                IQueryable<Entities.Intervento> queryBase = ll.Read();

                if (!string.IsNullOrWhiteSpace(testoRicerca))
                {
                    queryBase = queryBase.Where(x => x.Numero.ToString().ToLower().Contains(testoRicerca) ||
                                                x.RagioneSociale.ToLower().Contains(testoRicerca));
                }

                int itemsPerRequest = (combo.ItemsPerRequest <= 0) ? 20 : combo.ItemsPerRequest;
                int itemOffset = e.NumberOfItems;
                int endOffset = itemOffset + itemsPerRequest;
                int numTotaleUtenti = queryBase.Count();


                if (endOffset > numTotaleUtenti)
                    endOffset = numTotaleUtenti;

                IEnumerable<Entities.Intervento> entities = queryBase.Skip(itemOffset).Take(itemsPerRequest);
                foreach (Entities.Intervento entity in entities)
                {
                    RadComboBoxItem item = new RadComboBoxItem(entity.Numero.ToString(), entity.ID.ToString());
                    item.DataItem = entity;
                    combo.Items.Add(item);
                }
                combo.DataBind();

                if (numTotaleUtenti > 0)
                {
                    e.Message = String.Format("Tickets (da <b>1</b> a <b>{0}</b> di <b>{1}</b>)", endOffset.ToString(), numTotaleUtenti.ToString());
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


        public void SelezionaAttivita(Guid idAttivita)
        {
            GridDataItem itemTrovato = null;

            foreach (GridDataItem dataItem in rgAttivita.MasterTableView.Items)
            {
                object key = dataItem.GetDataKeyValue("ID");
                if (key != null &&
                    Guid.TryParse(key.ToString(), out Guid g) &&
                    g == idAttivita)
                {
                    itemTrovato = dataItem;
                    break;
                }
            }

            if (itemTrovato == null)
                return;

            // 1) evidenzio la riga
            itemTrovato.Selected = true;

            // 2) registro uno script che, lato client, scrolla fino alla riga selezionata
            string script = @"
                function scrollToSelectedAttivita() {
                    var grid = $find('" + rgAttivita.ClientID + @"');
                    if (!grid) return;

                    var master = grid.get_masterTableView();
                    var items = master.get_selectedItems();
                    if (!items || items.length == 0) return;

                    var rowElement = items[0].get_element();
                    if (rowElement && rowElement.scrollIntoView) {
                        // true = allinea in alto; se vuoi più 'centrale', puoi usare false
                        rowElement.scrollIntoView(true);
                    }
                }

                Sys.Application.add_load(scrollToSelectedAttivita);";

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "scrollToSelectedAttivita",
                script,
                true
            );
        }

    }
}