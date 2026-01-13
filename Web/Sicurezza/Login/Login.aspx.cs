using System;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using SeCoGEST.Helper;
using SeCoGes.Utilities;

namespace SeCoGEST.Web.Sicurezza.Login
{
    public partial class Login : System.Web.UI.Page
    {
        #region Intercettazione Eventi

        /// <summary>
        /// Metodo di gestione dell'evento Load della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE + " - Pagina di Login";
            lbPageTitle.Text = Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE;

            if (Infrastructure.InformazioniSessione.BloccaAccessi())
            {
                PannelloMessaggioOffLine.Visible = true;
                LoginPanel.Visible = false;
                ModificaPasswordPanel.Visible = false;
                return;
            }

            ModificaPassword.OnPasswordChanged += new ModificaPassword.PasswordChangedEventHandler(ModificaPassword_PasswordChanged);
            ModificaPassword.OnErrorChangingPassword += new ModificaPassword.ErrorChangingPasswordEventHandler(ModificaPassword_ErrorChangingPassword);

            //Se è la prima esecuzione della pagina (non è un post o una chiamata ajax) effettuo l'inizializzazione della pagina
            if (!Helper.Web.IsPostOrCallBack(this.Page))
            {
                if (Request.QueryString["mod"] == "1" && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    LoginPanel.Visible = false;
                    lblTitoloModificaPassword.Text = "Modifica password utente:";
                    ModificaPasswordPanel.Visible = true;
                    ModificaPassword.UserName = HttpContext.Current.User.Identity.Name;
                    ModificaPassword.Show();
                }
                else if (Request.QueryString["mod"] == "2" && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    if (Page.User.Identity.IsAuthenticated)
                        FormsAuthentication.SignOut();
                    Response.Redirect("~/Login/Login.aspx");
                }
                else
                {
                    LoginPanel.Visible = true;
                    ModificaPasswordPanel.Visible = false;
                    //Imposto il focus predefinito sulla casella di testo del nome utente
                    txtUsername.Focus();
                }

                lblVersione.Text = String.Concat("Ver: ", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento Click dell'oggetto lbLogin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbLogin_Click(object sender, System.EventArgs e)
        {
            try
            {
                MessagesCollector errorList = ValidaDati();
                if (errorList != null && errorList.HaveMessages)
                {
                    throw new Exception(errorList.ToString("<br/>"));
                }

                string nomeUtente = txtUsername.Text.Trim();

                Logic.Sicurezza.LoginResponseEnum loginResponse = Logic.Sicurezza.SecurityManager.Login(nomeUtente, txtPassword.Text.Trim());
                switch (loginResponse)
                {
                    case Logic.Sicurezza.LoginResponseEnum.AccessoConsentito:
                        FormsAuthentication.SetAuthCookie(nomeUtente, chkRemember.Checked);
                        AggiurnaInformazioniUtente(nomeUtente);
                        SeCoGes.Logging.LogManager.AddLogAccessi(nomeUtente, "Effettuato login.");
                        DoRedirect();
                        break;

                    case Logic.Sicurezza.LoginResponseEnum.UtenteBloccato:
                        //...altrimenti mostro un messaggio di avviso di accesso negato.
                        lblMessage.Text = "Accesso non consentito.";
                        break;

                    case Logic.Sicurezza.LoginResponseEnum.PasswordScaduta:
                        LoginPanel.Visible = false;
                        lblTitoloModificaPassword.Text = "La password è scaduta ed è necessario modificarla";
                        ModificaPasswordPanel.Visible = true;
                        ModificaPassword.UserName = nomeUtente;
                        ModificaPassword.Show();
                        break;

                    default:
                        //...altrimenti mostro un messaggio di avviso di accesso negato.
                        lblMessage.Text = "Nome Utente o Password errati!";
                        break;
                }
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                SeCoGes.Logging.LogManager.AddLogErrori(ex);
                ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Recupera la pagina richiesta e viene effettuato il redirect a quest'ultima
        /// </summary>
        private void DoRedirect()
        {
            var redirectUrl = Request.QueryString["ReturnUrl"];
            if (String.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = "~/Home.aspx";
            }

            Response.Redirect(redirectUrl, false);
        }

        /// <summary>
        /// Effettua l'aggiornamento della informazioni dell'utente che ha effettuato il login
        /// </summary>
        /// <param name="userName"></param>
        private void AggiurnaInformazioniUtente(string userName)
        {
            Logic.Sicurezza.Accounts llAccounts = new Logic.Sicurezza.Accounts();
            Entities.Account account = llAccounts.Find(userName);

            if (account == null)
            {
                throw new Exception(String.Format("La ricerca dell'account con UserName '{0}' non ha restituito nessun valore.", userName));
            }

            account.UltimoAccesso = DateTime.Now;
            llAccounts.SubmitToDatabase();
        }

        /// <summary>
        /// Gestisce gli errori dovuti al cambiamento della password
        /// </summary>
        /// <param name="ex"></param>
        private void ModificaPassword_ErrorChangingPassword(System.Exception ex)
        {
            ShowErrorMessage(ex.Message);
        }

        /// <summary>
        /// Gestisce gli elementi grafici al completamento del cambio password
        /// </summary>
        /// <param name="UserName"></param>
        protected void ModificaPassword_PasswordChanged(string UserName)
        {
            ModificaPasswordPanel.Visible = false;

            if (Request.QueryString["mod"] == "1")
            {
                ShowErrorMessage("Password modificata correttamente.");
            }
            else
            {
                LoginPanel.Visible = true;
                txtUsername.Text = UserName;
                txtPassword.Focus();
            }
        }

        /// <summary>
        /// Restituisce un oggetto contenente gli eventuali errori di validazione dei dati
        /// </summary>
        /// <returns></returns>
        private MessagesCollector ValidaDati()
        {
            MessagesCollector errorList = new MessagesCollector();

            if (!rfvUsername.IsValid)
            {
                errorList.Add(rfvUsername.ErrorMessage);
            }

            if (!rfvPassword.IsValid)
            {
                errorList.Add(rfvPassword.ErrorMessage);
            }

            return errorList;
        }

        /// <summary>
        /// Mostra il messaggio di errore passato come parametro
        /// </summary>
        /// <param name="errorMessage"></param>
        private void ShowErrorMessage(string errorMessage)
        {
            if (String.IsNullOrEmpty(errorMessage)) 
            {
                errorMessage = String.Empty;
            }

            string error = Helper.Web.ReplaceForHTML(errorMessage);
            error = error.Replace("\"", "\'");
            error = error.Replace("'", "\'");
            RadWindowManager1.RadAlert(error, 500, null, "Attenzione", null);
        }
        #endregion
    }
}