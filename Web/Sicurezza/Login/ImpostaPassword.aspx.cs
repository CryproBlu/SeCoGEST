using System;
using System.Net.Mail;
using System.Text;
using SeCoGEST.Helper;
using SeCoGEST.Logic.Sicurezza;

namespace SeCoGEST.Web.Sicurezza.Login
{
    public partial class ImpostaPassword : System.Web.UI.Page
    {
        #region Variabili

        /// <summary>
        /// Variabile contenente l'istanze del logic layer degli utenti
        /// </summary>
        private Logic.Sicurezza.Accounts llU = new Logic.Sicurezza.Accounts();

        /// <summary>
        /// Variabile di memorizzazione dell'entity utente
        /// </summary>
        private Entities.Account _currentUtente;

        #endregion

        #region Properties

        /// <summary>
        /// Restituisce l'utente corrente in pase alla chiave contenuta nella QueryString
        /// </summary>
        private Entities.Account currentUtente
        {
            get
            {
                if (_currentUtente == null)
                {
                    if (Request.QueryString["resPwdKey"] != null)
                    {
                        _currentUtente = llU.Find(SeCoGes.Utilities.GuidUtility.NewGuid(Request.QueryString["resPwdKey"]));
                    }
                    else
                    {
                        Response.End();
                        return null;
                    }
                }
                return _currentUtente;
            }
        }

        #endregion

        #region Intercettazione Eventi

        /// <summary>
        /// Metodo di gestione dell'evento Load della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Helper.Web.IsPostOrCallBack(this))
            {
                SeCoGes.Logging.LogManager.AddLogAccessi(Helper.Web.GetClientIpAddress(), String.Format("Accesso alla pagina '{0}'.", Request.Url.AbsolutePath));

                tableMessaggio.Visible = false;
                divReimposta.Visible = false;
                rbReimposta.Visible = false;
                if (!IsPostBack)
                {
                    if (currentUtente != null)
                    {
                        lblNomeUtente.Text = currentUtente.UserName;
                        lblIndirizzoEmail.Text = currentUtente.Email;
                        tableMessaggio.Visible = true;
                        divReimposta.Visible = true;
                        rbReimposta.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento Click relativo al tasto Reimposta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbReimposta_Click(object sender, EventArgs e)
        {
            ReimpostaPassword();
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Effettua l'operazione dei reimpostazione della password
        /// </summary>
        private void ReimpostaPassword()
        {
            AlertImage.Visible = false;

            Entities.Account utenteRichiedenteReset = currentUtente;
            if (utenteRichiedenteReset == null)
            {
                Response.End();
                return;
            }

            try
            {
                string nuovaPassword = Guid.NewGuid().ToString().Replace("-", "").Substring(1, 10);

                //Logic.Utenti llU = new Logic.Utenti();
                SecurityManager.ChangeUserPassword(utenteRichiedenteReset, nuovaPassword);
                utenteRichiedenteReset.ScadenzaPassword = DateTime.Today.AddDays(-1);



                MailMessage mail = new MailMessage();
                mail.To.Add(new MailAddress(utenteRichiedenteReset.Email, "Destinatario"));
                mail.Subject = Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE + " - Reimpostazione della password di accesso";

                StringBuilder messaggioMail = new StringBuilder();
                messaggioMail.Append(Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE + "<br />");
                messaggioMail.Append("MODIFICA DATI DI ACCESSO:<br />");
                messaggioMail.Append("<br />Gentile {0},");
                messaggioMail.Append("<br />Il tuo Username è il seguente:");
                messaggioMail.Append("<br />{1}");
                messaggioMail.Append("<br />");
                messaggioMail.Append("<br />Come da te richiesto, la tua password è stata reimpostata. La tua nuova password è:");
                messaggioMail.Append("<br />{2}");


                mail.Body = String.Format(messaggioMail.ToString(),
                                        utenteRichiedenteReset.Nominativo,
                                        utenteRichiedenteReset.UserName,
                                        nuovaPassword);



                mail.IsBodyHtml = true;


                //MittenteEmail
                SmtpClient smtpClient = new SmtpClient();

                divReimposta.Visible = true;
                try
                {
                    smtpClient.Send(mail);

                    llU.SubmitToDatabase();

                    SeCoGes.Logging.LogManager.AddLogOperazioni(Helper.Web.GetClientIpAddress(), String.Format("{0} - Richiesto il reset della password per l'utente '{1}'.", Request.Url.AbsolutePath, utenteRichiedenteReset.UserName));
                    //llU.LogManager.AddLogAccessi(String.Format("Reset della password per l'utente {0}.", utenteRichiedenteReset.Username))

                    lblResult.Text = "Operazione effettuata: Una e-mail contenente la tua nuova password è stata inviata all'indirizzo " + utenteRichiedenteReset.Email;
                    rbReimposta.Visible = false;

                }
                catch (SmtpException smtpEx)
                {
                    AlertImage.Visible = true;
                    lblResult.ForeColor = System.Drawing.Color.Red;
                    lblResult.Text = string.Format("Operazione SMTP non riuscita, è stato riscontrato il seguente errore:<br />{0}", smtpEx.Message);

                    //Logic.LogManager.AddLogErrori(string.Concat(string.Format("Reset password: si è verificato un problema nell'invio della mail al contenente la nuova password per l'utente {0}", utenteRichiedenteReset.Username), smtpEx.Message), TraceEventType.Critical);

                }
                catch (Exception ex)
                {
                    AlertImage.Visible = true;
                    lblResult.ForeColor = System.Drawing.Color.Red;
                    lblResult.Text = string.Format("Operazione non riuscita, è stato riscontrato il seguente errore:<br />{0}", ex.Message);

                    //Logic.LogManager.AddLogErrori(string.Concat(string.Format("Reset password: si è verificato un problema nell'invio della mail al contenente la nuova password per l'utente {0}", utenteRichiedenteReset.Username), ex.Message), TraceEventType.Critical);
                }


            }
            catch (Exception ex)
            {
                AlertImage.Visible = true;
                lblResult.ForeColor = System.Drawing.Color.Red;
                lblResult.Text = string.Format("Operazione non riuscita, è stato riscontrato il seguente errore:<br />{0}", ex.Message);

                //Logic.LogManager.AddLogErrori(string.Concat(string.Format("Si è verificato un problema nella procedura di reset della password per l'utente {0}", utenteRichiedenteReset.Username), ex.Message), TraceEventType.Critical);
            }
        }

        #endregion        
    }
}