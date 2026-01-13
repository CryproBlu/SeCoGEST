using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SeCoGEST.Logic.Sicurezza;

namespace SeCoGEST.Web.Sicurezza.Login
{
    public partial class ModificaPassword : System.Web.UI.UserControl
    {
        #region Properties

        /// <summary>
        /// Imposta o restituisci la stringa relativa allo UserName
        /// </summary>
        public string UserName
        {
            get
            {
                if (ViewState["Username"] == null)
                {
                    return string.Empty;
                }
                else
                {
                    return Convert.ToString(ViewState["Username"]);
                }
            }
            set
            {
                ViewState["Username"] = value;
            }
        }

        #endregion

        #region Delegate ed Eventi

        public delegate void PasswordChangedEventHandler(string username);
        public event PasswordChangedEventHandler OnPasswordChanged;

        public delegate void ErrorChangingPasswordEventHandler(Exception exception);
        public event ErrorChangingPasswordEventHandler OnErrorChangingPassword;

        #endregion

        #region Intercettazione Eventi

        /// <summary>
        /// Metodo di gestione dell'evento Click relativo al link button Change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbChange_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                if (txtPassword.Text.Trim().Equals(txtConfermaPassword.Text.Trim()))
                {
                    Logic.Sicurezza.Accounts llU = new Logic.Sicurezza.Accounts();
                    Entities.Account utente = llU.Find(UserName);

                    if (utente != null)
                    {
                        SecurityManager.ChangeUserPassword(utente, txtPassword.Text.Trim());
                        llU.SubmitToDatabase();


                        SeCoGes.Logging.LogManager.AddLogOperazioni(String.Format("{0} - Password modificata correttamente.", Request.Url.AbsolutePath));

                        if (OnPasswordChanged != null)
                            OnPasswordChanged(utente.UserName);
                    }
                }
            }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);

                if (OnErrorChangingPassword != null)
                    OnErrorChangingPassword(ex);
            }
        }

        #endregion

        #region Metodi di gestione

        /// <summary>
        /// Visualizza i dati dell'utente e attiva il focus sulla casella di immissione della password
        /// </summary>
        public void Show()
        {
            txtUsername.Text = UserName;

            //Imposto il focus predefinito sulla casella di testo della password
            txtPassword.Focus();
        }

        #endregion
    }
}