using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SeCoGEST.Helper;

namespace SeCoGEST.Web.Sicurezza.Login
{
    public partial class PasswordDimenticata : System.Web.UI.Page
    {
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
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento Click relativo al tasto di reset della password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btResetPassword_Click(object sender, EventArgs e)
        {
            ResetPassword();
        }

        /// <summary>
        /// Metodo di gestione dell'evento click del tasto di reimpostazione della password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbReimposta_Click(object sender, EventArgs e)
        {
            ResetPassword();
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Effettua le operazioni di reset della password e la gestione della grafica in base alle operazioni eseguite
        /// </summary>
        protected void ResetPassword()
        {
            AlertImage.Visible = false;

            Logic.Sicurezza.Accounts llU = new Logic.Sicurezza.Accounts();
            IQueryable<Entities.Account> utentiAssociatiAllEmail = llU.ReadByEmail(txtEmail.Text);
            if (utentiAssociatiAllEmail == null || utentiAssociatiAllEmail.Count() == 0)
            {
                AlertImage.Visible = true;
                lblResult.ForeColor = System.Drawing.Color.Red;
                lblResult.Text = string.Format("L'indirizzo '{0}' non è stato riconosciuto.<br />Provare con un altro indirizzo e-mail.", txtEmail.Text);
                return;
            }


            try
            {
                // Per ogni entity recuperata creo l'email e la invio
                foreach (Entities.Account utenteAssociatoAllEmail in utentiAssociatiAllEmail)
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(new MailAddress(utenteAssociatoAllEmail.Email, "Destinatario"));
                    mail.Subject = Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE + " - Dati di accesso";

                    //mailMessage.CC utilizzare se è necessario inviare in copia carbone 
                    StringBuilder messaggioMail = new StringBuilder();
                    messaggioMail.Append(Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE + "<br />");
                    messaggioMail.Append("DATI DI ACCESSO:<br />");
                    messaggioMail.Append("<br />");
                    messaggioMail.Append("Gentile {0},");
                    messaggioMail.Append("<br />");
                    messaggioMail.Append("Il tuo Username è il seguente:");
                    messaggioMail.Append("<br />{1}");
                    messaggioMail.Append("<br />");
                    messaggioMail.Append("<br />Se hai dimenticato la password puoi avviare la procedura di reimpostazione cliccando il link sottostante:");
                    messaggioMail.Append("<br /><a target='_blank' href='{4}://{2}/Sicurezza/Login/ImpostaPassword.aspx?resPwdKey={3}'>{4}://{2}/Sicurezza/Login/ImpostaPassword.aspx?resPwdKey={3}</a>");


                    mail.Body = String.Format(messaggioMail.ToString(),
                        utenteAssociatoAllEmail.Nominativo,
                        utenteAssociatoAllEmail.UserName,
                        Request.ServerVariables["HTTP_HOST"],
                        utenteAssociatoAllEmail.ID.ToString(),
                        Request.Url.Scheme
                    );



                    mail.IsBodyHtml = true;


                    //MittenteEmail
                    SmtpClient smtpClient = new SmtpClient();

                    try
                    {
                        smtpClient.Send(mail);

                        lblResult.Text = string.Format(@"Operazione effettuata.<br />Una e-mail contenente i tuoi dati di accesso è stata inviata all'indirizzo '{0}'.<br />Controlla la tua casella di posta.", utenteAssociatoAllEmail.Email);
                    }
                    catch (SmtpException smtpEx)
                    {
                        AlertImage.Visible = true;
                        lblResult.ForeColor = System.Drawing.Color.Red;
                        lblResult.Text = String.Format("SmtpException : {0}", smtpEx.Message);
                    }

                    catch (Exception ex)
                    {
                        AlertImage.Visible = true;
                        lblResult.ForeColor = System.Drawing.Color.Red;
                        lblResult.Text = String.Format("Exception : {0}", ex.Message);
                    }
                }

                SeCoGes.Logging.LogManager.AddLogOperazioni(Helper.Web.GetClientIpAddress(), String.Format("{0} - Richiesto i dati di accesso per l'utente con l'indirizzo email'{1}'.", Request.Url.AbsolutePath, txtEmail.Text));
            }
            catch (Exception ex)
            {
                AlertImage.Visible = true;
                lblResult.ForeColor = System.Drawing.Color.Red;
                lblResult.Text = String.Format("Si è verificato il seguente errore: {0}", ex.Message);
            }
        }

        #endregion
    }
}