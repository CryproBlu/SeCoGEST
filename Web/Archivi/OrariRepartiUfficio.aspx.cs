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
    public partial class OrariRepartiUfficio : System.Web.UI.Page
    {
        private Guid? GetRepartoUfficioId()
        {
            if (Request.QueryString["rep"] == null) return null;
            if(Guid.TryParse(Request.QueryString["rep"], out Guid id))
            {
                return id;
            }
            else
            {
                return null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "Orari Ufficio";
            Guid? repId = GetRepartoUfficioId();
            if (repId != null)
            {
                Entities.RepartoUfficio rep = new Logic.RepartiUfficio().Find(repId.Value);
                if(rep != null)
                {
                    this.Title = $"Orari Ufficio {rep.Reparto}";
                    hlMostraReparto.Visible = true;
                    //hlMostraReparto.Text = $"Torna all'elenco reparti '{rep.Reparto}'";
                    //hlMostraReparto.NavigateUrl = $"/Archivi/RepartiUfficio.aspx?id '{rep.Reparto}'";
                }
            }
            lblPageTitle.Text = this.Title;
        }

        protected void rgArchiveItems_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Guid? repId = GetRepartoUfficioId();
            if(repId == null)
            {
                rgArchiveItems.DataSource = new List<Entities.RepartoUfficio>();
                return;
            }

            Logic.OrariRepartiUfficio llT = new Logic.OrariRepartiUfficio();
            rgArchiveItems.DataSource = llT.Read(repId.Value);
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
                Logic.OrariRepartiUfficio llArc = new Logic.OrariRepartiUfficio();
                Entities.OrarioRepartoUfficio archiveItem = new Entities.OrarioRepartoUfficio();
                if (archiveItem != null)
                {
                    RadDropDownList rddlGiorni = (RadDropDownList)e.Item.FindControl("rddlGiorni");
                    if(rddlGiorni == null || rddlGiorni.SelectedIndex < 0)
                    {
                        archiveMessageControl.Message = "Specificare il Giorno relativo all'orario indicato.";
                        archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Important;
                        archiveMessageControl.Visible = true;
                        e.Canceled = true;
                        return;
                    }

                    RadTimePicker rtpDalle = (RadTimePicker)e.Item.FindControl("rtpDalle");
                    if (rtpDalle == null || !rtpDalle.SelectedTime.HasValue)
                    {
                        archiveMessageControl.Message = "Specificare l'orario di inizio lavori.";
                        archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Important;
                        archiveMessageControl.Visible = true;
                        e.Canceled = true;
                        return;
                    }

                    RadTimePicker rtpAlle = (RadTimePicker)e.Item.FindControl("rtpAlle");
                    if (rtpAlle == null || !rtpAlle.SelectedTime.HasValue)
                    {
                        archiveMessageControl.Message = "Specificare l'orario di fine lavori.";
                        archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Important;
                        archiveMessageControl.Visible = true;
                        e.Canceled = true;
                        return;
                    }



                    archiveItem.Id = Guid.NewGuid();
                    archiveItem.IdRepartoUfficio = GetRepartoUfficioId().Value;
                    archiveItem.Giorno = byte.Parse(rddlGiorni.SelectedValue);
                    archiveItem.OrarioDalle = rtpDalle.SelectedTime.Value;
                    archiveItem.OrarioAlle = rtpAlle.SelectedTime.Value;


                    llArc.Create(archiveItem, true);
                    archiveMessageControl.Message = "Nuovo Orario inserito con successo.";
                    archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Note;
                    archiveMessageControl.Visible = true;
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
                    Logic.OrariRepartiUfficio llArc = new Logic.OrariRepartiUfficio();
                    Entities.OrarioRepartoUfficio archiveItem = llArc.Find(new EntityId<Entities.OrarioRepartoUfficio>(itemId));
                    if (archiveItem != null)
                    {
                        RadDropDownList rddlGiorni = (RadDropDownList)e.Item.FindControl("rddlGiorni");
                        if (rddlGiorni == null || rddlGiorni.SelectedIndex < 0)
                        {
                            archiveMessageControl.Message = "Specificare il Giorno relativo all'orario indicato.";
                            archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Important;
                            archiveMessageControl.Visible = true;
                            e.Canceled = true;
                            return;
                        }

                        RadTimePicker rtpDalle = (RadTimePicker)e.Item.FindControl("rtpDalle");
                        if (rtpDalle == null || !rtpDalle.SelectedTime.HasValue)
                        {
                            archiveMessageControl.Message = "Specificare l'orario di inizio lavori.";
                            archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Important;
                            archiveMessageControl.Visible = true;
                            e.Canceled = true;
                            return;
                        }

                        RadTimePicker rtpAlle = (RadTimePicker)e.Item.FindControl("rtpAlle");
                        if (rtpAlle == null || !rtpAlle.SelectedTime.HasValue)
                        {
                            archiveMessageControl.Message = "Specificare l'orario di fine lavori.";
                            archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Important;
                            archiveMessageControl.Visible = true;
                            e.Canceled = true;
                            return;
                        }



                        archiveItem.Giorno = byte.Parse(rddlGiorni.SelectedValue);
                        archiveItem.OrarioDalle = rtpDalle.SelectedTime.Value;
                        archiveItem.OrarioAlle = rtpAlle.SelectedTime.Value;


                        llArc.SubmitToDatabase();
                        archiveMessageControl.Message = "Orario aggiornato con successo.";
                        archiveMessageControl.FrameStyle = PageMessage.FrameStyles.Note;
                        archiveMessageControl.Visible = true;
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
                    Logic.OrariRepartiUfficio llArc = new Logic.OrariRepartiUfficio();
                    Entities.OrarioRepartoUfficio archiveItem = llArc.Find(new EntityId<Entities.OrarioRepartoUfficio>(itemId));
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

        protected void rgArchiveItems_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && (e.Item as GridEditableItem).IsInEditMode)
            {
                GridEditFormItem editItem = (GridEditFormItem)e.Item;
                Entities.OrarioRepartoUfficio orario = null;

                if(editItem.DataItem is Entities.OrarioRepartoUfficio)
                {
                    orario = (Entities.OrarioRepartoUfficio)editItem.DataItem;
                }


                RadDropDownList rddl = (RadDropDownList)editItem.FindControl("rddlGiorni");
                if (rddl != null)
                {
                    rddl.Items.Clear();
                    for (int i = 0; i < 7; i++)
                    {
                        DropDownListItem item = new DropDownListItem(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.DayNames[i], i.ToString());
                        rddl.Items.Add(item);
                    }

                    if(orario != null) rddl.SelectedValue = orario.Giorno.ToString();
                }

                RadTimePicker rtpDalle = (RadTimePicker)editItem.FindControl("rtpDalle");
                if(rtpDalle != null)
                {
                    if (orario != null) rtpDalle.SelectedTime = orario.OrarioDalle;
                }

                RadTimePicker rtpAlle = (RadTimePicker)editItem.FindControl("rtpAlle");
                if (rtpAlle != null)
                {
                    if (orario != null) rtpAlle.SelectedTime = orario.OrarioAlle;
                }

            }

        }
    }
}