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
    public partial class PeriodiFestivita : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void rgArchiveItems_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Logic.PeriodiFestivita llT = new Logic.PeriodiFestivita();
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
                Logic.PeriodiFestivita llArc = new Logic.PeriodiFestivita();
                Entities.PeriodoFestivita archiveItem = new Entities.PeriodoFestivita();
                if (archiveItem != null)
                {
                    
                    RadTextBox rtbFestivita = (RadTextBox)e.Item.FindControl("rtbFestivita");
                    RadNumericTextBox rntbGiorno = (RadNumericTextBox)e.Item.FindControl("rntbGiorno");
                    RadNumericTextBox rntbMese = (RadNumericTextBox)e.Item.FindControl("rntbMese");
                    RadNumericTextBox rntbAnno = (RadNumericTextBox)e.Item.FindControl("rntbAnno");
                    if (rntbGiorno != null && rntbGiorno != null && rntbAnno != null)
                    {                        
                        archiveItem.Festivita = rtbFestivita.Text.Trim();
                        archiveItem.Giorno = (int?)rntbGiorno.Value;
                        archiveItem.Mese = (int?)rntbMese.Value;
                        archiveItem.Anno = (int?)rntbAnno.Value;
                        if ((archiveItem.Giorno == 0 && archiveItem.Mese == 0 && archiveItem.Anno == 0) ||
                            (!archiveItem.Giorno.HasValue && !archiveItem.Mese.HasValue && !archiveItem.Anno.HasValue))
                        {
                            archiveMessageControl.Message = "Specificare almeno un valore!";
                            archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Important;
                            archiveMessageControl.Visible = true;
                            e.Canceled = true;
                        }
                        else
                        {
                            llArc.Create(archiveItem, true);
                            archiveMessageControl.Message = "Nuovo periodo di festività inserito con successo.";
                            archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Note;
                            archiveMessageControl.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                archiveMessageControl.Message = "Si è verificato un errore al salvataggio del periodo di festività: " + ex.Message;
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
                    Logic.PeriodiFestivita llArc = new Logic.PeriodiFestivita();
                    Entities.PeriodoFestivita archiveItem = llArc.Find(itemId);
                    if (archiveItem != null)
                    {
                        RadTextBox rtbFestivita = (RadTextBox)e.Item.FindControl("rtbFestivita");
                        RadNumericTextBox rntbGiorno = (RadNumericTextBox)e.Item.FindControl("rntbGiorno");
                        RadNumericTextBox rntbMese = (RadNumericTextBox)e.Item.FindControl("rntbMese");
                        RadNumericTextBox rntbAnno = (RadNumericTextBox)e.Item.FindControl("rntbAnno");
                        if (rntbGiorno != null && rntbGiorno != null && rntbAnno != null)
                        {
                            archiveItem.Festivita = rtbFestivita.Text.Trim();
                            archiveItem.Giorno = (int?)rntbGiorno.Value;
                            archiveItem.Mese = (int?)rntbMese.Value;
                            archiveItem.Anno = (int?)rntbAnno.Value;
                            if ((archiveItem.Giorno == 0 && archiveItem.Mese == 0 && archiveItem.Anno == 0) ||
                                (!archiveItem.Giorno.HasValue && !archiveItem.Mese.HasValue && !archiveItem.Anno.HasValue))
                            {
                                archiveMessageControl.Message = "Specificare almeno un valore!";
                                archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Important;
                                archiveMessageControl.Visible = true;
                                e.Canceled = true;
                            }
                            else
                            {
                                llArc.SubmitToDatabase();
                                archiveMessageControl.Message = "Periodo di festività aggiornato con successo.";
                                archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Note;
                                archiveMessageControl.Visible = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                archiveMessageControl.Message = "Si è verificato un errore al salvataggio del periodo di festività: " + ex.Message;
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
                    Logic.PeriodiFestivita llArc = new Logic.PeriodiFestivita();
                    Entities.PeriodoFestivita archiveItem = llArc.Find(itemId);
                    if (archiveItem != null)
                    {
                        llArc.Delete(archiveItem, true);
                    }
                    else
                    {
                        string message = "radalert('Il periodo di festività da eliminare non è stata trovato.', 330, 210, 'Errore');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", message, true);
                        e.Canceled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"radalert('Si è verificato un errore al salvataggio del periodo di festività: {ex.Message.Replace("'", "")}', 330, 210 'Errore');";
                errorMessage = errorMessage.Replace("\n", "");
                errorMessage = errorMessage.Replace("\r", "");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", errorMessage, true);
                e.Canceled = true;
            }

        }
    }
}