using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SeCoGEST.Web.UC
{
    public partial class UserInfo : System.Web.UI.UserControl
    {
        #region Properties

        public bool DisconnectedMode
        {
            get
            {
                if (ViewState["DisconnectedMode"] == null)
                    DisconnectedMode = false;
                return (bool)ViewState["DisconnectedMode"];
            }
            set
            {
                ViewState["DisconnectedMode"] = value;
            }
        }

        #endregion
    }
}