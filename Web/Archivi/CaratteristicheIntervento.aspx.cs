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
    public partial class CaratteristicheIntervento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "Archivio Caratteristiche di Intervento";
        }


        protected void rgArchiveItems_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Logic.CaratteristicheIntervento llT = new Logic.CaratteristicheIntervento();
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
                Logic.CaratteristicheIntervento llArc = new Logic.CaratteristicheIntervento();
                Entities.CaratteristicaIntervento archiveItem = new Entities.CaratteristicaIntervento();
                if (archiveItem != null)
                {
                    RadNumericTextBox inputNumberControl = (RadNumericTextBox)e.Item.FindControl("rntbNumero");
                    RadTextBox inputTextControl = (RadTextBox)e.Item.FindControl("rtbNome");
                    if (inputNumberControl != null && inputTextControl != null)
                    {
                        archiveItem.Id = (int)inputNumberControl.Value;
                        archiveItem.Nome = inputTextControl.Text.Trim();
                        if (archiveItem.Nome == string.Empty)
                        {
                            archiveMessageControl.Message = "Specificare un Nome della Caratteristica di Intervento.";
                            archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Important;
                            archiveMessageControl.Visible = true;
                            e.Canceled = true;
                        }
                        else
                        {
                            llArc.Create(archiveItem, true);
                            archiveMessageControl.Message = "Nuova Caratteristica di Intervento inserita con successo.";
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
                if (int.TryParse(editedItem.GetDataKeyValue("Id").ToString(), out int itemId))
                {
                    Logic.CaratteristicheIntervento llArc = new Logic.CaratteristicheIntervento();
                    Entities.CaratteristicaIntervento archiveItem = llArc.Find(itemId);
                    if (archiveItem != null)
                    {

                        RadNumericTextBox inputNumberControl = (RadNumericTextBox)e.Item.FindControl("rntbNumero");
                        RadTextBox userControl = (RadTextBox)e.Item.FindControl("rtbNome");
                        if (inputNumberControl != null && userControl != null)
                        {
                            archiveItem.Id = (int)inputNumberControl.Value;
                            archiveItem.Nome = userControl.Text.Trim();
                            if (archiveItem.Nome == string.Empty)
                            {
                                archiveMessageControl.Message = "Specificare un Nome della Caratteristica di Intervento.";
                                archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Important;
                                archiveMessageControl.Visible = true;
                                e.Canceled = true;
                            }
                            else
                            {
                                llArc.SubmitToDatabase();
                                archiveMessageControl.Message = "Caratteristica di Intervento aggiornata con successo.";
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
                if (int.TryParse(editedItem.GetDataKeyValue("Id").ToString(), out int itemId))
                {
                    Logic.CaratteristicheIntervento llArc = new Logic.CaratteristicheIntervento();
                    Entities.CaratteristicaIntervento archiveItem = llArc.Find(itemId);
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