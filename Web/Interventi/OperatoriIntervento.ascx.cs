using System;
using System.Collections;
using System.Web.UI.WebControls;
using SeCoGEST.Infrastructure;
using SeCoGEST.Helper;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Linq;
using SeCoGEST.Entities;

namespace SeCoGEST.Web.Interventi
{
    public partial class OperatoriIntervento : System.Web.UI.UserControl
    {
        #region Property

        #region Pubbliche

        /// <summary>
        /// Imposta o restituisce l'id dell'Entity Intervento alla quale sono associati gli Operatori in gestione
        /// </summary>
        //public Guid IDIntervento
        //{
        //    get
        //    {
        //        Guid idIntervento = Guid.Empty;
        //        if (ViewState["IDIntervento"] != null)
        //        {
        //            idIntervento = (Guid)ViewState["IDIntervento"];
        //        }

        //        return idIntervento;
        //    }
        //    set
        //    {
        //        ViewState["IDIntervento"] = value;
        //    }
        //}
        public EntityId<Entities.Intervento> IDIntervento
        {
            get
            {
                if (ViewState["IDIntervento"] != null)
                {
                    return new EntityId<Entities.Intervento>(ViewState["IDIntervento"]);
                }
                else
                {
                    return EntityId<Entities.Intervento>.Empty;
                }
            }
            set
            {
                if (value.HasValue)
                {
                    ViewState["IDIntervento"] = value.Value;
                }
                else
                {
                    ViewState["IDIntervento"] = null;
                }
            }
        }

        /// <summary>
        /// Imposta o restituisce un valore che indica se gli oggetti della pagina sono attivi o meno
        /// </summary>
        public bool Enabled
        {
            get
            {
                if (ViewState["Enabled"] == null)
                {
                    return true;
                }
                else
                {
                    return (bool)ViewState["Enabled"];
                }
            }
            set
            {
                ViewState["Enabled"] = value;

                // Attivo/Disattivo i controlli della griglia in base al valore indicato
                rgGriglia.MasterTableView.CommandItemDisplay = value ? GridCommandItemDisplay.Top : GridCommandItemDisplay.None;
                //rgGriglia.Columns.FindByUniqueName("ColonnaModifica").Display = value;
                rgGriglia.Columns.FindByUniqueName("ColonnaElimina").Display = value;
            }
        }

        /// <summary>
        /// Imposta o restituisce un valore che indica se le note sono visibili o meno attivi o meno
        /// </summary>
        public bool NoteVisibili
        {
            get
            {
                if (ViewState["NoteVisibili"] == null)
                {
                    return true;
                }
                else
                {
                    return (bool)ViewState["NoteVisibili"];
                }
            }
            set
            {
                ViewState["NoteVisibili"] = value;

                rgGriglia.Columns.FindByUniqueName("ColonnaNote").Display = value;
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// Imposta o restituisce un valore che indica se l'usercontrol è stato inizializzato
        /// </summary>
        private bool UserControlInizializzato
        {
            get
            {
                if (ViewState["UserControlInizializzato"] == null)
                {
                    return false;
                }
                else
                {
                    return (bool)ViewState["UserControlInizializzato"];
                }
            }
            set
            {
                ViewState["UserControlInizializzato"] = value;
            }
        }


        Logic.Operatori _logicOperatori = null;
        private Logic.Operatori LogicOperatori
        {
            get
            {
                if (_logicOperatori == null) _logicOperatori = new Logic.Operatori();
                return _logicOperatori;
            }
        }

        Logic.ModalitaRisoluzioneInterventi _logicModalitaRisoluzioneInterventi = null;
        private Logic.ModalitaRisoluzioneInterventi LogicModalitaRisoluzioneInterventi
        {
            get
            {
                if (_logicModalitaRisoluzioneInterventi == null) _logicModalitaRisoluzioneInterventi = new Logic.ModalitaRisoluzioneInterventi();
                return _logicModalitaRisoluzioneInterventi;
            }
        }

        Logic.Intervento_Operatori _logicInterventoOperatori = null;
        private Logic.Intervento_Operatori LogicInterventoOperatori
        {
            get
            {
                if (_logicInterventoOperatori == null) _logicInterventoOperatori = new Logic.Intervento_Operatori();
                return _logicInterventoOperatori;
            }
        }

        #endregion

        #endregion

        #region Intercettazione eventi

        ///// <summary>
        ///// Metodo che gestisce l'evento Load della pagina
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!Helper.Web.IsPostOrCallBack(Page))
        //    {
        //        TelerikRadGridHelper.ManageExelExportSettings(rgGriglia, "Elenco_Opratori_Intervento", true);
        //    }
        //}

        #region Griglia

        /// <summary>
        /// Metodo di gestione dell'evento ItemCommand della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGriglia_PreRender(object sender, EventArgs e)
        {
            TelerikRadGridHelper.ApplicaTraduzioneDaFileDiResource(rgGriglia);

            //foreach (GridItem item in rgGriglia.Items)
            //{
            //    if (item is GridItem)
            //    {
            //        GridItem dataItem = (GridItem)item;

            //        CheckBox chkPrioritario = (CheckBox)dataItem.FindControl("chkPrioritario");
            //        if (chkPrioritario != null)
            //        {
            //            chkPrioritario.GroupName = "ContattoPrioritario3";
            //        }
            //    }

            //}
        }


        /// <summary>
        /// Metodo di gestione 'NeedDataSource' dell'evento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGriglia_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<Entities.Intervento_Operatore> elencoOperatori = null;

            if (!UserControlInizializzato)
            {
                elencoOperatori = GetDataSource(LogicInterventoOperatori).ToList();

                // Decommentare questo codice per fare in modo che l'interfaccia mostri sempre un record vuoto per l'inserimento
                //int numeroDefaultRecordPresentati = 2;
                //int numeroOperatori = elencoOperatori.Count;
                //if (numeroOperatori >= numeroDefaultRecordPresentati)
                //{
                //    if (Enabled) AggiungiRigaVuotaAlDataSourceDellaGriglia(elencoOperatori, false);
                //}
                //else
                //{
                //    for (int i = numeroOperatori; i < numeroDefaultRecordPresentati; i++)
                //    {
                //        if (Enabled) AggiungiRigaVuotaAlDataSourceDellaGriglia(elencoOperatori, false);
                //    }
                //}
            }
            else
            {
                elencoOperatori = OttieniElencoOperatoriDallaGriglia(false);
            }

            rgGriglia.DataSource = elencoOperatori;
        }

        protected void rgGriglia_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (e.Item as GridDataItem);
                RadDateTimePicker rdtpInizioIntervento = (RadDateTimePicker)e.Item.FindControl("rdtpInizioIntervento");
                RadDateTimePicker rdtpFineIntervento = (RadDateTimePicker)e.Item.FindControl("rdtpFineIntervento");
                RadNumericTextBox rntbDurataMinuti = (RadNumericTextBox)e.Item.FindControl("rntbDurataMinuti");
                RadNumericTextBox rntbPausaMinuti = (RadNumericTextBox)e.Item.FindControl("rntbPausaMinuti");

                rdtpInizioIntervento.ClientEvents.OnDateSelected = string.Format("function(sender, args) [ DateSelected('{0}', '{1}', '{2}'); ]", rdtpInizioIntervento.ClientID, rdtpFineIntervento.ClientID, rntbDurataMinuti.ClientID).Replace("[", "{").Replace("]", "}");
                rdtpFineIntervento.ClientEvents.OnDateSelected = string.Format("function(sender, args) [ DateSelected('{0}', '{1}', '{2}'); ]", rdtpInizioIntervento.ClientID, rdtpFineIntervento.ClientID, rntbDurataMinuti.ClientID).Replace("[", "{").Replace("]", "}");
            }
        }

        protected void rgGriglia_ItemDataBound(object sender, GridItemEventArgs e)
        {
            // Inserisce gli attributi necessari per la trasformazione del layout della griglia per dispositivi mobili
            TelerikRadGridHelper.ManageColumnContentOnMobileLayout(rgGriglia, e);



            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                try
                {
                    Entities.Intervento_Operatore entity = null;
                    if (dataItem != null && dataItem.DataItem is Entities.Intervento_Operatore)
                    {
                        entity = (Entities.Intervento_Operatore)dataItem.DataItem;
                    }

                    dataItem["RowIndex"].Text = (e.Item.ItemIndex + 1).ToString();

                    RadComboBox rcbOperatori = (RadComboBox)dataItem.FindControl("rcbOperatori");
                    Label lblOperatore = (Label)dataItem.FindControl("lblOperatore");
                    HiddenField hdIdOperatore = (HiddenField)dataItem.FindControl("hdIdOperatore");
                    if (rcbOperatori != null && lblOperatore != null && hdIdOperatore != null)
                    {
                        if(entity == null ||
                            entity.ID == Guid.Empty)
                        {
                            rcbOperatori.DataSource = LogicOperatori.Read();
                            rcbOperatori.DataBind();
                            TelerikHelper.InsertBlankComboBoxItem(rcbOperatori);
                            rcbOperatori.Visible = true;

                            if(entity.IDOperatore != Guid.Empty)
                            {
                                var citem = rcbOperatori.FindItemByValue(entity.IDOperatore.ToString());
                                if (citem != null) citem.Selected = true;
                            }

                            lblOperatore.Visible = false;
                            hdIdOperatore.Value = String.Empty;
                        }
                        else
                        {
                            rcbOperatori.Visible = false;
                            hdIdOperatore.Value = entity.IDOperatore.ToString();
                            if(entity.Operatore != null)
                            {
                                lblOperatore.Text = entity.CognomeNomeOperatore;
                            }
                            else
                            {
                                var op = LogicOperatori.Find(new EntityId<Entities.Operatore>(entity.IDOperatore));
                                if(op != null) lblOperatore.Text = op.CognomeNome;
                            }
                            lblOperatore.Visible = true;
                       }
                    }

                    RadComboBox rcbModalitaRisoluzione = (RadComboBox)dataItem.FindControl("rcbModalitaRisoluzione");
                    if (rcbModalitaRisoluzione != null)
                    {
                        rcbModalitaRisoluzione.DataSource = LogicModalitaRisoluzioneInterventi.Read();
                        rcbModalitaRisoluzione.DataBind();
                        TelerikHelper.InsertBlankComboBoxItem(rcbModalitaRisoluzione, "", "-1");
                        if (entity != null && !entity.IDModalitaRisoluzione.Equals(Guid.Empty))
                        {
                            RadComboBoxItem itemModalitaRisoluzione = rcbModalitaRisoluzione.FindItemByValue(entity.IDModalitaRisoluzione.ToString());
                            if (itemModalitaRisoluzione != null)
                            {
                                itemModalitaRisoluzione.Selected = true;
                            }
                        }
                    }

                    RadDateTimePicker rdtpInizioIntervento = (RadDateTimePicker)dataItem.FindControl("rdtpInizioIntervento");
                    if (rdtpInizioIntervento != null)
                    {
                        //rdtpInizioIntervento.Width = Unit.Pixel(200);
                        if (entity != null && entity.InizioIntervento.HasValue)
                        {
                            rdtpInizioIntervento.SelectedDate = entity.InizioIntervento;
                        }
                    }

                    CheckBox chkPresaInCarico = (CheckBox)dataItem.FindControl("chkPresaInCarico");
                    if (chkPresaInCarico != null)
                    {
                        if (entity != null)
                        {
                            chkPresaInCarico.Checked = (entity.PresaInCarico.HasValue && entity.PresaInCarico.Value);
                        }
                        if(rdtpInizioIntervento != null)
                        {
                            chkPresaInCarico.Attributes["onclick"] = $"ImpostaInizioIntervento(this, '{rdtpInizioIntervento.ClientID}');";
                        }
                    }

                    RadDateTimePicker rdtpFineIntervento = (RadDateTimePicker)dataItem.FindControl("rdtpFineIntervento");
                    if (rdtpFineIntervento != null)
                    {
                        if (entity != null && entity.FineIntervento.HasValue)
                        {
                            rdtpFineIntervento.SelectedDate = entity.FineIntervento;
                        }
                    }

                    RadNumericTextBox rntbDurataMinuti = (RadNumericTextBox)dataItem.FindControl("rntbDurataMinuti");
                    if (rntbDurataMinuti != null)
                    {
                        if (entity != null && entity.DurataMinuti.HasValue)
                        {
                            rntbDurataMinuti.Value = entity.DurataMinuti;
                        }
                    }

                    RadNumericTextBox rntbPausaMinuti = (RadNumericTextBox)dataItem.FindControl("rntbPausaMinuti");
                    if (rntbPausaMinuti != null)
                    {
                        if (entity != null && entity.PausaMinuti.HasValue)
                        {
                            rntbPausaMinuti.Value = entity.PausaMinuti;
                        }
                    }


                    ImageButton deleteButton = (ImageButton)dataItem["ColonnaElimina"].Controls[0];
                    if (entity == null || entity.ID == Guid.Empty)
                    {
                        deleteButton.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowErrorMessage(Page, ex);
                }
            }
        }

        /// <summary>
        /// Metodo di gestione 'DeleteCommand' dell'evento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGriglia_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem)
                {
                    GridEditableItem oggettoDaModificare = (GridEditableItem)e.Item;
                    Guid idOggettoSelezionato = (Guid)oggettoDaModificare.GetDataKeyValue("ID");

                    string errorMessage = String.Format("La ricerca dell'entità 'Intervento_Operatore' con ID '{0}' non ha restituito nessun valore.", idOggettoSelezionato);

                    List<Entities.Intervento_Operatore> elencoOperatoriIntervento = OttieniElencoOperatoriDallaGriglia(false);
                    if (elencoOperatoriIntervento == null)
                    {
                        throw new Exception(errorMessage);
                    }

                    Entities.Intervento_Operatore operatoreDaEliminare = elencoOperatoriIntervento.Where(x => x.ID == idOggettoSelezionato).FirstOrDefault();
                    if (operatoreDaEliminare == null)
                    {
                        throw new Exception(errorMessage);
                    }
                    else
                    {
                        bool elementoRimosso = elencoOperatoriIntervento.Remove(operatoreDaEliminare);
                        if (!elementoRimosso)
                        {
                            throw new Exception(String.Format("Non è stato possibile eliminare l'entità 'Intervento_Operatore' con ID '{0}'.", idOggettoSelezionato));
                        }

                        //AggiungiRigaVuotaAlDataSourceDellaGriglia(elencoInformazioniContatto, false);

                        rgGriglia.DataSource = elencoOperatoriIntervento;
                        rgGriglia.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                e.Canceled = true;
                MessageHelper.ShowErrorMessage(Page, ex);
            }
        }

        /// <summary>
        /// Metodo di gestione 'ItemCommand' dell'evento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rgGriglia_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                e.Canceled = true;

                List<Entities.Intervento_Operatore> elencoInformazioniContatto = OttieniElencoOperatoriDallaGriglia(false);
                AggiungiRigaVuotaAlDataSourceDellaGriglia(elencoInformazioniContatto, false);
                rgGriglia.DataSource = elencoInformazioniContatto;
                rgGriglia.DataBind();
            }
        }

        #endregion

        #endregion

        #region Metodi Pubblici

        /// <summary>
        /// Effettua l'inizializzazione dell'usercontrol
        /// </summary>
        public void Inizializza()
        {
            rgGriglia.Rebind();
            UserControlInizializzato = true;
        }

        /// <summary>
        /// Effettua la validazione dei dati presenti nella griglia
        /// </summary>
        /// <param name="statoIntervento"></param>
        /// <returns></returns>
        public SeCoGes.Utilities.MessagesCollector ValidaDati(StatoInterventoEnum statoIntervento)
        {
            Logic.Operatori llOperatori = new Logic.Operatori();
            SeCoGes.Utilities.MessagesCollector listaErrori = new SeCoGes.Utilities.MessagesCollector();

            List<Entities.Intervento_Operatore> elencoOperatori = OttieniElencoOperatoriDallaGriglia(false);
            if (elencoOperatori != null && elencoOperatori.Count > 0)
            {
                for (int i = 0; i < elencoOperatori.Count; i++)
                {
                    Entities.Intervento_Operatore operatore = elencoOperatori[i];
                    if (!IsRigaVuota(operatore))
                    {
                        //if (operatore.IDOperatore.Equals(Guid.Empty))
                        //{
                        //    listaErrori.Add(String.Format("Riga {0}: Indicare il Nome dell'Operatore", i + 1));
                        //}

                        Entities.Operatore entityOperatore = llOperatori.Find(new EntityId<Operatore>(operatore.IDOperatore));
                        if (entityOperatore == null)
                        {
                            listaErrori.Add(String.Format("Riga {0}: L'Operatore selezionato non è presente nella fonte dati", i + 1));
                        }

                        if (entityOperatore != null && entityOperatore.Area)
                        {
                            if (statoIntervento == StatoInterventoEnum.Chiuso || statoIntervento == StatoInterventoEnum.Validato)
                            {
                                //listaErrori.Add(String.Format("Riga {0}: E' necessario indicare un operatore che sia utilizzato come area oppure rimuovere la riga", i + 1));

                                if (operatore.IDModalitaRisoluzione.HasValue)
                                {
                                    listaErrori.Add(String.Format("Riga {0}: Non è possibile indicare una Modalità di Risoluzione per gli operatori che sono impostati come area", i + 1));
                                }
                                if (operatore.InizioIntervento.HasValue)
                                {
                                    listaErrori.Add(String.Format("Riga {0}: Non è possibile indicare la Data di Inizio Intervento per gli operatori che sono impostati come area", i + 1));
                                }
                                if (operatore.FineIntervento.HasValue)
                                {
                                    listaErrori.Add(String.Format("Riga {0}: Non è possibile indicare la Data di Fine Intervento per gli operatori che sono impostati come area", i + 1));
                                }
                                if (operatore.DurataMinuti.HasValue)
                                {
                                    listaErrori.Add(String.Format("Riga {0}: Non è possibile indicare la Durata dell'Intervento per gli operatori che sono impostati come area", i + 1));
                                }
                                if (operatore.PausaMinuti.HasValue)
                                {
                                    listaErrori.Add(String.Format("Riga {0}: Non è possibile indicare la Pausa dell'Intervento per gli operatori che sono impostati come area", i + 1));
                                }
                            }
                        }
                        else
                        {
                            if (statoIntervento == StatoInterventoEnum.Chiuso || statoIntervento == StatoInterventoEnum.Validato)
                            {
                                if (operatore.IDModalitaRisoluzione < 0)
                                {
                                    listaErrori.Add(String.Format("Riga {0}: Indicare una Modalità di Risoluzione", i + 1));
                                }
                                if (operatore.InizioIntervento == DateTime.MinValue)
                                {
                                    listaErrori.Add(String.Format("Riga {0}: Indicare la Data di Inizio Intervento", i + 1));
                                }
                                if (operatore.FineIntervento == DateTime.MinValue)
                                {
                                    listaErrori.Add(String.Format("Riga {0}: Indicare la Data di Fine Intervento", i + 1));
                                }
                                if (operatore.DurataMinuti == 0)
                                {
                                    listaErrori.Add(String.Format("Riga {0}: Indicare la Durata dell'Intervento", i + 1));
                                }
                            }
                        }                                                
                    }
                }
            }

            return listaErrori;
        }

        /// <summary>
        /// Effettua il salvataggio dei dati presenti nella griglia
        /// </summary>
        /// <param name="logicLayer"></param>
        /// <param name="submitChanges"></param>
        /// <param name="idIntervento"></param>
        /// <param name="elencoOperatoriCreatiModificati"></param>
        public List<Entities.Intervento_Operatore> SalvaDati(Logic.Base.LogicLayerBase logicLayer, bool submitChanges, Entities.Intervento intervento, out List<Entities.Intervento_Operatore> elencoOperatoriCreatiModificati)
        {
            if (logicLayer == null) throw new ArgumentNullException("logicLayer");

            this.IDIntervento = new EntityId<Entities.Intervento>(intervento.ID);

            List<Entities.Intervento_Operatore> elencoOperatoriInGriglia = OttieniElencoOperatoriDallaGriglia(true);
            Logic.Intervento_Operatori ll = new Logic.Intervento_Operatori(logicLayer);

            List<Entities.Intervento_Operatore> elencoOperatoriCreatiTemp = new List<Intervento_Operatore>();

            if (elencoOperatoriInGriglia.Count > 0)
            {
                InserisciAggiornaElementi(ll, submitChanges, elencoOperatoriInGriglia, out elencoOperatoriCreatiTemp);
                DateTime? primaData = elencoOperatoriInGriglia.Min(o => o.InizioIntervento);
                intervento.DataPrevistaIntervento = primaData;
            }
            else
            {
                intervento.DataPrevistaIntervento = null;
            }

            EliminaDalDatabaseGliOperatoriRimossiDallaGriglia(ll, submitChanges, elencoOperatoriInGriglia);
            elencoOperatoriCreatiModificati = elencoOperatoriCreatiTemp;

            return elencoOperatoriInGriglia;
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Effettua il rebind della griglia
        /// </summary>
        private void RebindGriglia()
        {
            List<Entities.Intervento_Operatore> elencoInformazioniContatto = OttieniElencoOperatoriDallaGriglia(false);
            rgGriglia.DataSource = elencoInformazioniContatto;
            rgGriglia.DataBind();
        }

        /// <summary>
        /// Effettua la rimozione, dal database, degliOperatori rimossi dalla griglia
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="submitChanges"></param>
        /// <param name="elencoOperatoriInGriglia"></param>
        private void EliminaDalDatabaseGliOperatoriRimossiDallaGriglia(Logic.Intervento_Operatori logic, bool submitChanges, List<Entities.Intervento_Operatore> elencoOperatoriInGriglia)
        {
            if (logic == null) logic = new Logic.Intervento_Operatori();
            if (elencoOperatoriInGriglia == null) elencoOperatoriInGriglia = new List<Entities.Intervento_Operatore>();

            // Viene recuperato l'elenco degli operatori...
            IQueryable<Entities.Intervento_Operatore> elencoOperatoriDatabase = GetDataSource(logic);

            // Nel caso in cui esistano degli operatori...
            if (elencoOperatoriDatabase != null && elencoOperatoriDatabase.Count() > 0)
            {
                // Vengono recuperati gli id degli elementi presenti nella griglia
                IEnumerable<Guid> elencoIdentificativoOperatori = elencoOperatoriInGriglia.Select(x => x.ID);

                // Vengono recuperati gli operatori da rimuovere. Esclude quelli che sono presenti nella griglia dall'elenco di quelli presenti nel database
                IQueryable<Entities.Intervento_Operatore> elencoOperatoriDaRimuovere = elencoOperatoriDatabase.Where(x => !elencoIdentificativoOperatori.Contains(x.ID));

                // Nel caso in cui esistano degli elementi da eliminare ..
                if (elencoOperatoriDaRimuovere != null && elencoOperatoriDaRimuovere.Count() > 0)
                {
                    // Viene utilizzato l'apposito logic layer per eliminarli
                    logic.Delete(elencoOperatoriDaRimuovere, submitChanges);
                }
            }
        }

        /// <summary>
        /// Effettua l'inserimento e/o l'aggiornamento degli elementi, passati come parametro, nel database
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="submitChanges"></param>
        /// <param name="elencoOperatoriInGriglia"></param>
        /// <param name="elencoOperatoriCreatiModificati"></param>
        private void InserisciAggiornaElementi(Logic.Intervento_Operatori logic, bool submitChanges, List<Entities.Intervento_Operatore> elencoOperatoriInGriglia, out List<Entities.Intervento_Operatore> elencoOperatoriCreatiModificati)
        {
            if (logic == null) logic = new Logic.Intervento_Operatori();
            if (elencoOperatoriInGriglia == null) elencoOperatoriInGriglia = new List<Entities.Intervento_Operatore>();

            List<Entities.Intervento_Operatore> elencoOperatoriCreatiModificatiTemp = new List<Entities.Intervento_Operatore>();

            if (elencoOperatoriInGriglia.Count > 0)
            {
                foreach (Entities.Intervento_Operatore operatore in elencoOperatoriInGriglia)
                {
                    Entities.Intervento_Operatore operatoreDatabase = logic.Find(new EntityId<Intervento_Operatore>(operatore.ID));
                    if (operatoreDatabase == null)
                    {
                        logic.Create(operatore, submitChanges);

                        elencoOperatoriCreatiModificatiTemp.Add(operatore);
                    }
                    else
                    {
                        bool aggiuntiOperatoreModificatoAllaLista = (operatoreDatabase.IDOperatore != operatore.IDOperatore);

                        operatoreDatabase.IDIntervento = this.IDIntervento.Value;
                        operatoreDatabase.IDOperatore = operatore.IDOperatore;
                        operatoreDatabase.IDModalitaRisoluzione = operatore.IDModalitaRisoluzione;
                        operatoreDatabase.InizioIntervento = operatore.InizioIntervento;
                        operatoreDatabase.FineIntervento = operatore.FineIntervento;
                        operatoreDatabase.DurataMinuti = operatore.DurataMinuti;
                        operatoreDatabase.PausaMinuti = operatore.PausaMinuti; 
                        operatoreDatabase.Note = operatore.Note;


                        operatoreDatabase.PresaInCarico = operatore.PresaInCarico;
                        operatoreDatabase.DataPresaInCarico = operatore.DataPresaInCarico;

                        // Se si indica una Data di Inizio Intervento UGUALE o PRECEDENTE la data corrente allora imposta il flag PresaInCarico = true
                        // La DataPresaInCarico non può avere una data successiva la data di InizioIntervento 
                        if (operatoreDatabase.InizioIntervento.HasValue && 
                           operatoreDatabase.InizioIntervento.Value < DateTime.Now)
                        {
                            operatoreDatabase.PresaInCarico = true;

                            if(!operatoreDatabase.DataPresaInCarico.HasValue) operatoreDatabase.DataPresaInCarico = operatoreDatabase.InizioIntervento;
                        }
                        if (operatoreDatabase.InizioIntervento.HasValue && 
                            operatoreDatabase.DataPresaInCarico.HasValue && 
                            operatoreDatabase.InizioIntervento.Value < operatoreDatabase.DataPresaInCarico.Value)
                        {
                            operatoreDatabase.PresaInCarico = true;
                            operatoreDatabase.DataPresaInCarico = operatoreDatabase.InizioIntervento;
                        }
                        if(!operatoreDatabase.DataPresaInCarico.HasValue) operatoreDatabase.PresaInCarico = false;



                        if (submitChanges) logic.SubmitToDatabase();

                        if (aggiuntiOperatoreModificatoAllaLista)
                        {
                            elencoOperatoriCreatiModificatiTemp.Add(operatoreDatabase);
                        }
                    }
                }
            }

            elencoOperatoriCreatiModificati = elencoOperatoriCreatiModificatiTemp;
        }

        /// <summary>
        /// Recupera dal database il datasource da applicare alla griglia
        /// </summary>
        /// <returns></returns>
        private IQueryable<Entities.Intervento_Operatore> GetDataSource()
        {
            return GetDataSource(null);
        }

        /// <summary>
        /// Recupera dal database il datasource da applicare alla griglia
        /// </summary>
        /// <param name="llInterventoOperatori"></param>
        /// <returns></returns>
        private IQueryable<Entities.Intervento_Operatore> GetDataSource(Logic.Intervento_Operatori llInterventoOperatori)
        {
            if (llInterventoOperatori == null)
            {
                llInterventoOperatori = new Logic.Intervento_Operatori();
            }

            return llInterventoOperatori.Read(this.IDIntervento);
        }

        /// <summary>
        /// Restituisce l'elenco delle anagrafiche relative agli indirizzi recuperandole in base ai valori presenti nelle righe della griglia
        /// </summary>
        /// <param name="escludiRigheVuote"></param>
        /// <returns></returns>
        private List<Entities.Intervento_Operatore> OttieniElencoOperatoriDallaGriglia(bool escludiRigheVuote)
        {
            List<Entities.Intervento_Operatore> elencoInformazioniContatto = new List<Entities.Intervento_Operatore>();
            if (rgGriglia.Items != null && rgGriglia.Items.Count > 0)
            {
                foreach (GridDataItem item in rgGriglia.Items)
                {
                    Entities.Intervento_Operatore interventoOperatore = new Entities.Intervento_Operatore();
                    interventoOperatore.ID = (Guid)item.GetDataKeyValue("ID");

                    interventoOperatore.IDIntervento = this.IDIntervento.Value;


                    RadComboBox rcbOperatori = TelerikRadGridHelper.FindControl<RadComboBox>(item, "rcbOperatori");
                    HiddenField hdIdOperatore = (HiddenField)item.FindControl("hdIdOperatore");
                    if (rcbOperatori != null && rcbOperatori.Visible && !String.IsNullOrWhiteSpace(rcbOperatori.SelectedValue))
                    {
                        interventoOperatore.IDOperatore = new Guid(rcbOperatori.SelectedValue);
                    }
                    else if(Guid.TryParse(hdIdOperatore.Value, out Guid opId))
                    {
                        interventoOperatore.IDOperatore = opId;
                    }
                    //if (interventoOperatore.Operatore == null && interventoOperatore.IDOperatore != Guid.Empty)
                    //{
                    //    var op = LogicOperatori.Find(new EntityId<Entities.Operatore>(interventoOperatore.IDOperatore));
                    //    if (op != null) interventoOperatore.Operatore = op;
                    //}


                    RadComboBox rcbModalitaRisoluzione = TelerikRadGridHelper.FindControl<RadComboBox>(item, "rcbModalitaRisoluzione");
                    if (rcbModalitaRisoluzione != null && !String.IsNullOrWhiteSpace(rcbModalitaRisoluzione.SelectedValue))
                    {
                        int idModalitaIntervento = int.Parse(rcbModalitaRisoluzione.SelectedValue);

                        Entities.EntityInt<Entities.ModalitaRisoluzioneIntervento> identifierModalitaIntervento = new Entities.EntityInt<Entities.ModalitaRisoluzioneIntervento>(idModalitaIntervento);
                        Entities.ModalitaRisoluzioneIntervento entityModalitaRisoluzioneIntervento = LogicModalitaRisoluzioneInterventi.Find(identifierModalitaIntervento);
                        if (entityModalitaRisoluzioneIntervento != null)
                        {
                            interventoOperatore.IDModalitaRisoluzione = entityModalitaRisoluzioneIntervento.ID;
                        }                        
                    }

                    RadDateTimePicker rdtpInizioIntervento = TelerikRadGridHelper.FindControl<RadDateTimePicker>(item, "rdtpInizioIntervento");
                    if (rdtpInizioIntervento != null && rdtpInizioIntervento.SelectedDate.HasValue)
                    {
                        interventoOperatore.InizioIntervento = rdtpInizioIntervento.SelectedDate.Value;
                    }

                    RadDateTimePicker rdtpFineIntervento = TelerikRadGridHelper.FindControl<RadDateTimePicker>(item, "rdtpFineIntervento");
                    if (rdtpFineIntervento != null && rdtpFineIntervento.SelectedDate.HasValue)
                    {
                        interventoOperatore.FineIntervento = rdtpFineIntervento.SelectedDate.Value;
                    }

                    RadNumericTextBox rntbDurataMinuti = TelerikRadGridHelper.FindControl<RadNumericTextBox>(item, "rntbDurataMinuti");
                    if (rntbDurataMinuti != null && rntbDurataMinuti.Value.HasValue)
                    {
                        interventoOperatore.DurataMinuti = (int)rntbDurataMinuti.Value.Value;
                    }
                    
                    RadNumericTextBox rntbPausaMinuti = TelerikRadGridHelper.FindControl<RadNumericTextBox>(item, "rntbPausaMinuti");
                    if (rntbPausaMinuti != null && rntbPausaMinuti.Value.HasValue)
                    {
                        interventoOperatore.PausaMinuti = (int)rntbPausaMinuti.Value.Value;
                    }

                    CheckBox chkPresaInCarico = TelerikRadGridHelper.FindControl<CheckBox>(item, "chkPresaInCarico");
                    if (chkPresaInCarico != null)
                    {
                        if (chkPresaInCarico.Checked)
                        {
                            interventoOperatore.PresaInCarico = true;

                            Label lblDataPresaInCarico = TelerikRadGridHelper.FindControl<Label>(item, "lblDataPresaInCarico");
                            if(lblDataPresaInCarico != null)
                            {
                                if(DateTime.TryParse(lblDataPresaInCarico.Text.Trim(), out DateTime dataPresaInCarico))
                                {
                                    interventoOperatore.DataPresaInCarico = dataPresaInCarico;
                                }
                            }
                            if (!interventoOperatore.DataPresaInCarico.HasValue) interventoOperatore.DataPresaInCarico = DateTime.Now;
                            if(interventoOperatore.InizioIntervento.HasValue && interventoOperatore.InizioIntervento.Value < interventoOperatore.DataPresaInCarico)
                            {
                                interventoOperatore.DataPresaInCarico = interventoOperatore.InizioIntervento.Value;
                            }
                        }
                        else
                        {
                            interventoOperatore.PresaInCarico = null;
                            interventoOperatore.DataPresaInCarico = null;
                        }                        
                    }

                    string note = TelerikRadGridHelper.GetTextFromRadTextBox(item, "rtbNote");
                    interventoOperatore.Note = note;

                    bool isRigaVuota = IsRigaVuota(interventoOperatore);

                    if (!isRigaVuota || (isRigaVuota && !escludiRigheVuote))
                    {
                        if(interventoOperatore.IDOperatore != Guid.Empty)
                        {
                            elencoInformazioniContatto.Add(interventoOperatore);
                        }
                    }
                }
            }

            return elencoInformazioniContatto;
        }

        /// <summary>
        /// Restituisce True se le informazioni sul contatto non sono valorizzate
        /// </summary>
        /// <param name="entityInterventoOperatore"></param>
        /// <returns></returns>
        private bool IsRigaVuota(Entities.Intervento_Operatore entityInterventoOperatore)
        {
            if (entityInterventoOperatore == null) throw new ArgumentNullException("entityInterventoOperatore");

            bool isEmpty = false;

            if (entityInterventoOperatore.IDOperatore == Guid.Empty &&
                (!entityInterventoOperatore.IDModalitaRisoluzione.HasValue || entityInterventoOperatore.IDModalitaRisoluzione == -1) &&
                (!entityInterventoOperatore.PresaInCarico.HasValue || !entityInterventoOperatore.PresaInCarico.Value) &&
                (!entityInterventoOperatore.InizioIntervento.HasValue || entityInterventoOperatore.InizioIntervento == DateTime.MinValue) &&
                (!entityInterventoOperatore.FineIntervento.HasValue || entityInterventoOperatore.FineIntervento == DateTime.MinValue) &&
                (!entityInterventoOperatore.DurataMinuti.HasValue || entityInterventoOperatore.DurataMinuti == 0) && 
                (!entityInterventoOperatore.PausaMinuti.HasValue || entityInterventoOperatore.PausaMinuti == 0) && 
                String.IsNullOrWhiteSpace(entityInterventoOperatore.Note))
            {
                isEmpty = true;
            }

            return isEmpty;
        }

        /// <summary>
        /// Aggiunge una riga vuota al datasource della griglia passato come parametro
        /// </summary>
        /// <param name="dataSourceGriglia"></param>
        /// <param name="aggiungiComePrimoElemento"></param>
        private void AggiungiRigaVuotaAlDataSourceDellaGriglia(List<Entities.Intervento_Operatore> dataSourceGriglia, bool aggiungiComePrimoElemento)
        {
            if (dataSourceGriglia == null)
            {
                dataSourceGriglia = new List<Entities.Intervento_Operatore>();
            }
            else
            {
                foreach(Entities.Intervento_Operatore interv in dataSourceGriglia)
                {
                    if (interv.InizioIntervento.HasValue && interv.InizioIntervento.Value <= DateTime.Now) interv.PresaInCarico = true;
                }
            }

            Entities.Intervento_Operatore nuovoInterventoOperatore = CreaNuovoInterventoOperatore();

            if(nuovoInterventoOperatore.InizioIntervento.HasValue && nuovoInterventoOperatore.InizioIntervento.Value <= DateTime.Now) nuovoInterventoOperatore.PresaInCarico = true;

            if (aggiungiComePrimoElemento)
            {
                dataSourceGriglia.Insert(0, nuovoInterventoOperatore);
            }
            else
            {
                dataSourceGriglia.Add(nuovoInterventoOperatore);
            }
        }

        /// <summary>
        /// Effettua la creazione di un nuovo Intervento Operatore
        /// </summary>
        /// <returns></returns>
        private Entities.Intervento_Operatore CreaNuovoInterventoOperatore()
        {
            Entities.Intervento_Operatore nuovoOperatore = new Entities.Intervento_Operatore();
            //nuovoOperatore.ID = Guid.NewGuid();

            nuovoOperatore.IDIntervento = this.IDIntervento.Value;
            //nuovoOperatore.InizioIntervento = DateTime.Now;

            return nuovoOperatore;
        }
        internal void AggiungiRigheSoloSeVuoto(int numOperatoriDaAggiungere)
        {
            if (numOperatoriDaAggiungere == 0) numOperatoriDaAggiungere = 1;

            List<Entities.Intervento_Operatore> elencoInformazioniContatto = OttieniElencoOperatoriDallaGriglia(false);
            if(elencoInformazioniContatto.Count() == 0)
            {
                AggiungiRighe(numOperatoriDaAggiungere);
            }
        }

        internal void AggiungiRighe(int numOperatoriDaAggiungere)
        {
            if (numOperatoriDaAggiungere == 0) numOperatoriDaAggiungere = 1;

            List<Entities.Intervento_Operatore> elencoInformazioniContatto = OttieniElencoOperatoriDallaGriglia(false);
            int numDefaultRecordPresentati = elencoInformazioniContatto.Count() + 1;
            for (int i = 0; i < numOperatoriDaAggiungere; i++)
            {
                AggiungiRigaVuotaAlDataSourceDellaGriglia(elencoInformazioniContatto, false);
            }
            rgGriglia.DataSource = elencoInformazioniContatto;
            rgGriglia.DataBind();
        }

        #endregion

    }
}