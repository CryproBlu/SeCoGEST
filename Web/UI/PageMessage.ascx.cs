using System;

namespace SeCoGEST.Web.UI
{
    public partial class PageMessage : System.Web.UI.UserControl
    {
        public enum FrameStyles
        {
            Caution = 0,
            Important = 1,
            Note = 2,
            Tip = 3
        }

        //private enum Icons
        //{
        //    Caution = 0,
        //    Important = 1,
        //    Note = 2,
        //    Tip = 3
        //}

        //private enum Colors
        //{
        //    Caution = 0,
        //    Important = 1,
        //    Note = 2,
        //    Tip = 3
        //}

        public FrameStyles FrameStyle
        {
            get
            {
                if (ViewState["FrameStyle"] == null)
                {
                    FrameStyle = FrameStyles.Note;
                }
                return (FrameStyles)ViewState["FrameStyle"];
            }
            set
            {
                ViewState["FrameStyle"] = value;

                string messageTitle = string.Empty;
                switch (value)
                {
                    case FrameStyles.Caution:
                        imgIcon.ImageUrl = "~/UI/Images/Messages/Caution.png";
                        lrPanel.CssClass = "pageMessagePanel pageMessageColorCaution";
                        lrOpenClose.CssClass = "pageMessageColorCaution";
                        messageTitle = "Attenzione";
                        break;

                    case FrameStyles.Important:
                        imgIcon.ImageUrl = "~/UI/Images/Messages/Important.png";
                        lrPanel.CssClass = "pageMessagePanel pageMessageColorImportant";
                        lrOpenClose.CssClass = "pageMessageColorImportant";
                        messageTitle = "Importante";
                        break;

                    case FrameStyles.Note:
                        imgIcon.ImageUrl = "~/UI/Images/Messages/Note.png";
                        lrPanel.CssClass = "pageMessagePanel pageMessageColorNote";
                        lrOpenClose.CssClass = "pageMessageColorNote";
                        messageTitle = "Nota";
                        break;

                    case FrameStyles.Tip:
                        imgIcon.ImageUrl = "~/UI/Images/Messages/Tip.png";
                        lrPanel.CssClass = "pageMessagePanel pageMessageColorTip";
                        lrOpenClose.CssClass = "pageMessageColorTip";
                        messageTitle = "Suggerimento";
                        break;

                    default:
                        imgIcon.ImageUrl = "~/UI/Images/Messages/Note.png";
                        lrPanel.CssClass = "pageMessagePanel pageMessageColorNote";
                        lrOpenClose.CssClass = "pageMessageColorNote";
                        messageTitle = "Nota";
                        break;
                }
                lblTitle.Text = string.Format("<span style='text-decoration: underline;'>{0}</span><br/>", messageTitle);
            }
        }

        //public Icons Icon
        //{
        //    get
        //    {
        //        if (ViewState["Icon"] == null)
        //        {
        //            Icon = Icons.Information;
        //        }
        //        return (Icons)ViewState["Icon"];
        //    }
        //    set
        //    {
        //        ViewState["Icon"] = value;

        //        switch (value)
        //        {
        //            case Icons.Information:
        //                imgIcon.ImageUrl="~/UI/Images/information.png";
        //                break;

        //            case Icons.Help:
        //                imgIcon.ImageUrl = "~/UI/Images/help.png";
        //                break;

        //            case Icons.Note:
        //                imgIcon.ImageUrl = "~/UI/Images/note.png";
        //                break;

        //            default:
        //                imgIcon.ImageUrl = "~/UI/Images/information.png";
        //                break;
        //        }
        //    }
        //}

        //public Colors Color
        //{
        //    get
        //    {
        //        if (ViewState["Color"] == null)
        //        {
        //            Color = Colors.Azure;
        //        }
        //        return (Colors)ViewState["Color"];
        //    }
        //    set
        //    {
        //        ViewState["Color"] = value;

        //        switch (value)
        //        {
        //            case Colors.Azure:
        //                lrPanel.CssClass = "pageMessagePanel pageMessageColorNote";
        //                break;

        //            case Colors.Red:
        //                lrPanel.CssClass = "pageMessagePanel pageMessageColorCaution";
        //                break;

        //            case Colors.Yellow:
        //                lrPanel.CssClass = "pageMessagePanel pageMessageColorImportant";
        //                break;

        //            default:
        //                lrPanel.CssClass = "pageMessagePanel pageMessageColorNote";
        //                break;
        //        }
        //    }
        //}


        public string Message
        {
            get
            {
                return lblMessage.Text;
            }
            set
            {
                string messageTitle = string.Empty;
                switch (FrameStyle)
                {
                    case FrameStyles.Caution: messageTitle = "Attenzione"; break;
                    case FrameStyles.Important: messageTitle = "Importante"; break;
                    case FrameStyles.Note: messageTitle = "Nota"; break;
                    case FrameStyles.Tip: messageTitle = "Suggerimento"; break;
                    default: messageTitle = "Nota"; break;
                }
                lblTitle.Text = string.Format("<span style='text-decoration: underline;'>{0}</span><br/>", messageTitle);
                lblMessage.Text = value;
            }
        }

        // Imposta o restituisce un valore booleano che indica se l'utente ha la possibilità di nascondere/mostrare il messaggio
        public bool AllowUserHide
        {
            get
            {
                if (ViewState["AllowUserHide"] == null)
                {
                    return false;
                }
                return (bool)ViewState["AllowUserHide"];
            }
            set
            {
                ViewState["AllowUserHide"] = value;
            }
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            if (!AllowUserHide) return;

            //base.OnPreRender(e);

            string scriptKey = "hideMessage:" + this.UniqueID;

            if (!Page.ClientScript.IsStartupScriptRegistered(scriptKey))// && !Page.IsPostBack)
            {
                string scriptBlock =
                    @"<script language=""JavaScript"">
                    <!--
                        function %%OnChiudiClicked%% {
                            var lrPanel = $get('%%lrPanel_ClientID%%');
                            if (lrPanel.style.display == 'none')
                            {
                                lrPanel.style.display = 'block'
                            }
                            else
                            {
                                lrPanel.style.display = 'none';
                            }
                        }
                    // -->
                    </script>";

                // divOpenClose
                string funcName = "OnChiudiClicked" + this.UniqueID + "()";
                scriptBlock = scriptBlock.Replace("%%OnChiudiClicked%%", funcName);
                scriptBlock = scriptBlock.Replace("%%lrPanel_ClientID%%", lrPanel.ClientID);

                divOpenClose.Attributes.Add("onclick", funcName);
                divOpenClose.Style.Add("cursor", "pointer");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), scriptKey, scriptBlock);
            }
        }

    }
}