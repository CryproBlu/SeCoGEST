using SeCoGEST.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SeCoGEST.Web.Archivi
{
    public partial class AnagraficaSoggettiProprieta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack && !IsCallback)
            {
                if (Request.QueryString["Cod"] != null)
                {
                    Logic.Metodo.AnagraficheClienti ll = new Logic.Metodo.AnagraficheClienti();
                    Entities.AnagraficaClienti cliente = ll.Find(Request.QueryString["Cod"]);
                    if(cliente != null)
                    {
                        CodiceCliente = cliente.CODCONTO;

                        rcbCliente.Text = string.Concat(cliente.DSCCONTO1, "", cliente.DSCCONTO2).Trim();
                        rcbCliente.SelectedValue = CodiceCliente;
                        rcbCliente_SelectedIndexChanged(rcbCliente, null);
                    }
                }
            }
        }

        #region Selezione del Cliente

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
                    //item.Attributes.Add("State", entity.STATOALTRO.HasValue ? entity.STATOALTRO.Value.ToString() : "0");
                    //item.Attributes.Add("Note", entity.NOTE1);
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

        protected void rcbCliente_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(rcbCliente.SelectedValue.Trim()))
            {
                CodiceCliente = rcbCliente.SelectedValue.Trim();
                rgArchiveItems.Visible = true;
                rgArchiveItems.Rebind();
            }
            else
            {
                CodiceCliente = string.Empty;
                rgArchiveItems.Visible = false;
            }
        }

        #endregion



        #region Griglia proprietà

        protected void rgArchiveItems_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if(CodiceCliente != string.Empty)
            {
                Logic.AnagraficaSoggettiProprieta llT = new Logic.AnagraficaSoggettiProprieta();
                rgArchiveItems.DataSource = llT.Read(CodiceCliente);
            }
        }

        protected void rgArchiveItems_ItemCreated(object sender, GridItemEventArgs e)
        {
            //Traduce le voci del menu di filtro della griglia. Questa operazione deve essere fatta ad ogni post
            Helper.TelerikHelper.TraduciElementiGriglia(e);
        }

        protected void rgArchiveItems_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.InitInsertCommandName) //"Add new" button clicked
            {
                e.Canceled = true;
                e.Item.OwnerTableView.InsertItem();
                GridEditableItem insertedItem = e.Item.OwnerTableView.GetInsertItem();
                RadDropDownList rddlProprieta = insertedItem.FindControl("rddlProprieta") as RadDropDownList;
                if(rddlProprieta != null)
                {
                    List<string> props = Logic.AnagraficaSoggettiProprieta.ReadElencoProprieta();
                    //props.Insert(0, "");
                    rddlProprieta.DataSource = props;
                    rddlProprieta.DataBind();
                }

                GridEditCommandColumn editColumn = (GridEditCommandColumn)rgArchiveItems.MasterTableView.GetColumn("EditCommandColumn");
                editColumn.Visible = false;
            }
            else if (e.CommandName == RadGrid.RebindGridCommandName && e.Item.OwnerTableView.IsItemInserted)
            {
                e.Canceled = true;
            }
            else
            {
                GridEditCommandColumn editColumn = (GridEditCommandColumn)rgArchiveItems.MasterTableView.GetColumn("EditCommandColumn");
                if (!editColumn.Visible)
                    editColumn.Visible = true;
            }
        }

        protected void rgArchiveItems_InsertCommand(object sender, GridCommandEventArgs e)
        {
            PageMessage archiveMessageControl = (PageMessage)e.Item.FindControl("ArchiveMessages");
            archiveMessageControl.Visible = false;
            try
            {
                Logic.AnagraficaSoggettiProprieta llArc = new Logic.AnagraficaSoggettiProprieta();
                Entities.AnagraficaSoggetti_Proprieta archiveItem = new Entities.AnagraficaSoggetti_Proprieta();
                if (archiveItem != null)
                {
                    //System.Web.UI.HtmlControls.HtmlInputText inputTextControl = (System.Web.UI.HtmlControls.HtmlInputText)e.Item.FindControl("txtDescription");
                    RadDropDownList rddlProprieta = (RadDropDownList)e.Item.FindControl("rddlProprieta");
                    RadCheckBox rchkValore = (RadCheckBox)e.Item.FindControl("rchkValore");
                    if(rddlProprieta.SelectedText == "DefaultVisibilitaTicketCliente")
                    {
                        if (rchkValore != null)
                        {
                            archiveItem.CodiceCliente = CodiceCliente;
                            archiveItem.Proprietà = rddlProprieta.SelectedText;
                            archiveItem.Valore = rchkValore.Checked.ToString();
                            //if (archiveItem.Valore == string.Empty)
                            //{
                            //    archiveMessageControl.Message = "Specificare un Nome della Tipologia di Intervento.";
                            //    archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Important;
                            //    archiveMessageControl.Visible = true;
                            //    e.Canceled = true;
                            //}
                            //else
                            //{
                                llArc.Create(archiveItem, true);
                                archiveMessageControl.Message = "Nuova Tipologia di Intervento inserita con successo.";
                                archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Note;
                                archiveMessageControl.Visible = true;
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                archiveMessageControl.Message = "Si è verificato un errore al salvataggio della voce di archivio: " + ex.Message;
                archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Caution;
                archiveMessageControl.Visible = true;
                e.Canceled = true;
            }
        }

        protected void rgArchiveItems_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            PageMessage archiveMessageControl = (PageMessage)e.Item.FindControl("ArchiveMessages");
            archiveMessageControl.Visible = false;

            try
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                if (string.IsNullOrEmpty(editedItem.GetDataKeyValue("Proprietà").ToString()))
                {
                    Logic.AnagraficaSoggettiProprieta llArc = new Logic.AnagraficaSoggettiProprieta();
                    Entities.AnagraficaSoggetti_Proprieta archiveItem = llArc.Find(CodiceCliente, editedItem.GetDataKeyValue("Proprietà").ToString());
                    if (archiveItem != null)
                    {
                        //System.Web.UI.HtmlControls.HtmlInputText userControl = (System.Web.UI.HtmlControls.HtmlInputText)e.Item.FindControl("txtDescription");
                        RadTextBox userControl = (RadTextBox)e.Item.FindControl("rtbValore");
                        if (userControl != null)
                        {
                            archiveItem.Proprietà = userControl.Text.Trim();
                            if (archiveItem.Valore == string.Empty)
                            {
                                archiveMessageControl.Message = "Specificare un Nome della Tipologia di Intervento.";
                                archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Important;
                                archiveMessageControl.Visible = true;
                                e.Canceled = true;
                            }
                            else
                            {
                                llArc.SubmitToDatabase();
                                archiveMessageControl.Message = "Tipologia di Intervento aggiornata con successo.";
                                archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Note;
                                archiveMessageControl.Visible = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                archiveMessageControl.Message = "Si è verificato un errore al salvataggio della voce di archivio: " + ex.Message;
                archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Caution;
                archiveMessageControl.Visible = true;
                e.Canceled = true;
            }
        }

        protected void rgArchiveItems_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                string codcli = editedItem.GetDataKeyValue("CodiceCliente").ToString();
                string prop = editedItem.GetDataKeyValue("Proprietà").ToString();

                if (!string.IsNullOrEmpty(codcli) && !string.IsNullOrEmpty(prop))
                {
                    Logic.AnagraficaSoggettiProprieta llArc = new Logic.AnagraficaSoggettiProprieta();
                    Entities.AnagraficaSoggetti_Proprieta archiveItem = llArc.Find(codcli, prop);
                    if (archiveItem != null)
                    {
                        llArc.Delete(archiveItem, true);
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
                string errorMessage = $"radalert('Si è verificato un errore al salvataggio della voce di archivio: {ex.Message.Replace("'", "")}', 330, 210 'Errore');";
                errorMessage = errorMessage.Replace("\n", "");
                errorMessage = errorMessage.Replace("\r", "");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", errorMessage, true);
                e.Canceled = true;
            }
        }

        #endregion

    }
}