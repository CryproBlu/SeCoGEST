using SeCoGEST.Entities;
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
    public partial class TipologieIntervento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "Archivio Tipologie di Intervento";
        }


        protected void rgArchiveItems_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Logic.TipologieIntervento llT = new Logic.TipologieIntervento();
            rgArchiveItems.DataSource = llT.Read();
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
                Logic.TipologieIntervento llArc = new Logic.TipologieIntervento();
                Entities.TipologiaIntervento archiveItem = new Entities.TipologiaIntervento();
                if (archiveItem != null)
                {
                    //System.Web.UI.HtmlControls.HtmlInputText inputTextControl = (System.Web.UI.HtmlControls.HtmlInputText)e.Item.FindControl("txtDescription");
                    RadTextBox inputTextControl = (RadTextBox)e.Item.FindControl("rtbNome");
                    if (inputTextControl != null)
                    {
                        archiveItem.Id = Guid.NewGuid();
                        archiveItem.Nome = inputTextControl.Text.Trim();
                        if (archiveItem.Nome == string.Empty)
                        {
                            archiveMessageControl.Message = "Specificare un Nome della Tipologia di Intervento.";
                            archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Important;
                            archiveMessageControl.Visible = true;
                            e.Canceled = true;
                        }
                        else
                        {
                            RadNumericTextBox rntbOreNotifica = (RadNumericTextBox)e.Item.FindControl("rntbOreNotifica");
                            if(rntbOreNotifica != null)
                            {
                                archiveItem.OreNotifica = (int?)rntbOreNotifica.Value;
                            }
                            RadNumericTextBox rntbVersioneModelloEsportazione = (RadNumericTextBox)e.Item.FindControl("rntbVersioneModelloEsportazione");
                            if (rntbVersioneModelloEsportazione != null)
                            {
                                archiveItem.VersioneModelloEsportazione = (int?)rntbVersioneModelloEsportazione.Value;
                            }

                            llArc.Create(archiveItem, true);
                            archiveMessageControl.Message = "Nuova Tipologia di Intervento inserita con successo.";
                            archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Note;
                            archiveMessageControl.Visible = true;
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
                if (Guid.TryParse(editedItem.GetDataKeyValue("Id").ToString(), out Guid itemId))
                {
                    Logic.TipologieIntervento llArc = new Logic.TipologieIntervento();
                    Entities.TipologiaIntervento archiveItem = llArc.Find(new EntityId<Entities.TipologiaIntervento>(itemId));
                    if (archiveItem != null)
                    {
                        //System.Web.UI.HtmlControls.HtmlInputText userControl = (System.Web.UI.HtmlControls.HtmlInputText)e.Item.FindControl("txtDescription");
                        RadTextBox userControl = (RadTextBox)e.Item.FindControl("rtbNome");
                        if (userControl != null)
                        {
                            archiveItem.Nome = userControl.Text.Trim();
                            if (archiveItem.Nome == string.Empty)
                            {
                                archiveMessageControl.Message = "Specificare un Nome della Tipologia di Intervento.";
                                archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Important;
                                archiveMessageControl.Visible = true;
                                e.Canceled = true;
                            }
                            else
                            {
                                RadNumericTextBox rntbOreNotifica = (RadNumericTextBox)e.Item.FindControl("rntbOreNotifica");
                                if (rntbOreNotifica != null)
                                {
                                    archiveItem.OreNotifica = (int?)rntbOreNotifica.Value;
                                }
                                RadNumericTextBox rntbVersioneModelloEsportazione = (RadNumericTextBox)e.Item.FindControl("rntbVersioneModelloEsportazione");
                                if (rntbVersioneModelloEsportazione != null)
                                {
                                    archiveItem.VersioneModelloEsportazione = (int?)rntbVersioneModelloEsportazione.Value;
                                }

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
                if (Guid.TryParse(editedItem.GetDataKeyValue("Id").ToString(), out Guid itemId))
                {
                    Logic.TipologieIntervento llArc = new Logic.TipologieIntervento();
                    Entities.TipologiaIntervento archiveItem = llArc.Find(new EntityId<Entities.TipologiaIntervento>(itemId));
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

    }
}