using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SeCoGes.Utilities;
using System.Net.Mail;

namespace SeCoGEST.Helper
{
    public static class EmailHelper
    {
        #region Costanti

        public const string SEPARATORE_EMAIL = ";";

        #endregion

        #region Metodi Pubblici
        
        /// <summary>
        /// Restituisce true se l'email passata come parametro è valida
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValid(string email)
        {
            if (!String.IsNullOrEmpty(email))
            {
                Regex regExp = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                return regExp.IsMatch(email);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Restituisce tutte le email valide presenti nel campo di testo passato come parametro
        /// </summary>
        /// <param name="emailsTextField"></param>
        /// <param name="separetor">Separatore utilizzato nel campo di testo per l'inserimento di più indirizi email</param>
        /// <returns></returns>
        public static List<string> GetEmailsValidList(string emailsTextField, string separetor = ";")
        {
            List<string> elencoEmailValide = new List<string>();

            emailsTextField = emailsTextField.ToTrimmedString();
            if (String.IsNullOrEmpty(emailsTextField))
            {
                return elencoEmailValide;
            }

            if (String.IsNullOrEmpty(separetor))
            {
                separetor = SEPARATORE_EMAIL;
            }

            if (emailsTextField.Contains(separetor))
            {
                string[] elencoEmals = emailsTextField.Split(new string[] { separetor }, StringSplitOptions.RemoveEmptyEntries);

                if (elencoEmals != null && elencoEmals.Length > 0)
                {
                    foreach (string email in elencoEmals)
                    {
                        string emailTemp = email.ToTrimmedString();

                        if (IsValid(emailTemp))
                        {
                            elencoEmailValide.Add(emailTemp);
                        }
                    }
                }
            }
            else
            {
                if (IsValid(emailsTextField))
                {
                    elencoEmailValide.Add(emailsTextField);
                }
            }

            return elencoEmailValide;
        }

        /// <summary>
        /// Effettua l'invio di una email in base al valore dei parametri passati 
        /// </summary>
        /// <param name="mailSubject"></param>
        /// <param name="testo"></param>
        /// <param name="testoHTML"></param>
        /// <param name="mittente"></param>
        /// <param name="destinatari"></param>
        /// <param name="priority"></param>
        /// <param name="deliveryNotificationOption"></param>
        /// <param name="attachments"></param>
        /// <remarks></remarks>
        public static void InviaEmail(string mailSubject, string testo, string testoHTML, string mittente, string[] destinatari, MailPriority priority = MailPriority.Normal, DeliveryNotificationOptions? deliveryNotificationOption = null, System.IO.FileInfo[] attachments = null)
        {
            if (String.IsNullOrWhiteSpace(mittente))
            {
                throw new ArgumentNullException("mittente", "Non è stato indicato nessun mittente");
            }

            if (destinatari == null)
            {
                destinatari = new string[] { };
            }

            IEnumerable<string> elencoDestinatari = destinatari.Where(x => !String.IsNullOrWhiteSpace(x));
            if (elencoDestinatari == null || elencoDestinatari.Count() <= 0)
            {
                throw new ArgumentNullException("elencoDestinatari", "Non è stato indicato nessun destinatario");
            }

            if (String.IsNullOrEmpty(mailSubject))
            {
                mailSubject = String.Empty;
            }

            if (String.IsNullOrEmpty(testo))
            {
                testo = String.Empty;
            }

            foreach (string destinatario in elencoDestinatari)
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        try
                        {
                            mailMessage.From = new MailAddress(mittente.Trim());
                            mailMessage.To.Add(new MailAddress(destinatario.Trim()));

                            mailMessage.Subject = mailSubject;
                            mailMessage.Body = testo;
                            mailMessage.Priority = priority;

                            if (testoHTML.Trim() != "")
                            {
                                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(testoHTML, null, System.Net.Mime.MediaTypeNames.Text.Html);
                                mailMessage.AlternateViews.Add(htmlView);
                            }

                            if (deliveryNotificationOption != null)
                            {
                                mailMessage.DeliveryNotificationOptions = deliveryNotificationOption.Value;
                            }

                            if (attachments != null && attachments.Count() > 0)
                            {
                                AggiungiAllegati(mailMessage, attachments);
                            }

                            smtp.Send(mailMessage);

                            mailMessage.Dispose();
                        }
                        catch (Exception) { }
                    }
                }
            }
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Aggiunge al messaggio passato come parametro, come mittenti, elenco dei destinatari passati come parametro
        /// </summary>
        /// <param name="mailMessage"></param>
        /// <param name="destinatari"></param>
        /// <remarks></remarks>

        private static void AggiungiDestinatari(MailMessage mailMessage, string[] destinatari)
        {
            if (mailMessage == null)
            {
                throw new ArgumentNullException("mailMessage", "Paramentro nullo");
            }

            if (destinatari == null || destinatari.Count() <= 0)
            {
                throw new Exception("Non è stato indicato nessun destinatario");
            }

            foreach (string destinatario in destinatari)
            {
                if (!String.IsNullOrWhiteSpace(destinatario))
                {
                    mailMessage.To.Add(destinatario);
                }
            }
        }

        /// <summary>
        /// Aggiunge all'oggetto MailMessage, come allegati, i file passati come parametro
        /// </summary>
        /// <param name="mailMessage"></param>
        /// <param name="fileDaAllegare"></param>
        /// <remarks></remarks>

        private static void AggiungiAllegati(MailMessage mailMessage, System.IO.FileInfo[] fileDaAllegare)
        {
            if (mailMessage == null)
            {
                throw new ArgumentNullException("mailMessage", "paramentro nullo");
            }

            if (fileDaAllegare != null && fileDaAllegare.Count() > 0)
            {
                foreach (System.IO.FileInfo file in fileDaAllegare)
                {
                    if (file.Exists)
                    {
                        Attachment allegato = new Attachment(file.FullName);
                        mailMessage.Attachments.Add(allegato);
                    }
                }
            }
        }

        #endregion
    }
}
