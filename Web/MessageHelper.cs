using System;
using System.Web;
using System.Web.UI;
using SeCoGEST.Helper;
using SeCoGEST.Logic;
using Telerik.Web.UI;

namespace SeCoGEST.Web
{
    public static class MessageHelper
    {
        #region Finestre di errore a video

        /// <summary>
        /// Mostra una RadAlert utilizzando i parametri passati come parametr
        /// </summary>
        /// <param name="page"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public static void ShowMessage(Page page, string title, string message)
        {
            RadWindowManager rwm = MasterPageHelper.GetRadWindowManager(page);
            if (rwm != null)
            {
                WindowHelper.ShowAlert(rwm, title, message);
            }
        }

        /// <summary>
        /// Restituisce il messaggio d'errore analizzando l'Exception passata e controllando l'esistenza di un messaggio specifico relativo alla violazione di un indice del database
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetErrorMessage(Exception ex)
        {
            if (ex == null) return String.Empty;

            return GetErrorMessage(ex.Message);
        }

        /// <summary>
        /// Restituisce il messaggio d'errore analizzando l'Exception passata e controllando l'esistenza di un messaggio specifico relativo alla violazione di un indice del database
        /// </summary>
        /// <param name="exceptionMessage">Messaggio di errore</param>
        /// <returns></returns>
        public static string GetErrorMessage(string exceptionMessage)
        {
            return exceptionMessage;
            //MessaggiPerIndice ll = new MessaggiPerIndice();

            //string dbIndexMessage = ll.GetMessaggioPerTestoContenenteIndiceDatabase(exceptionMessage);
            //if (!String.IsNullOrEmpty(dbIndexMessage))
            //{
            //    dbIndexMessage = String.Concat(dbIndexMessage, Environment.NewLine, Environment.NewLine, "Dettagli errore: ", exceptionMessage);
            //}
            //return dbIndexMessage != String.Empty ? dbIndexMessage : exceptionMessage;
        }

        /// <summary>
        /// Mostra il messaggio di errore passato come parametro
        /// </summary>
        /// <param name="page"></param>
        /// <param name="errorMessage"></param>
        public static void ShowErrorMessage(Page page, string errorMessage)
        {
            ShowErrorMessage(page, "Attenzione", errorMessage);
        }

        /// <summary>
        /// Mostra il messaggio di errore passato come parametro
        /// </summary>
        /// <param name="page"></param>
        /// <param name="errorMessage"></param>
        /// <param name="jsCallbackFunctionName"></param>
        public static void ShowErrorMessageWithJSCallbackFunction(Page page, string errorMessage, string jsCallbackFunctionName)
        {
            ShowErrorMessage(page, "Attenzione", errorMessage, jsCallbackFunctionName);
        }

        /// <summary>
        /// Mostra il messaggio di errore passato come parametro
        /// </summary>
        /// <param name="page"></param>
        /// <param name="title"></param>
        /// <param name="errorMessage"></param>
        public static void ShowErrorMessage(Page page, string title, string errorMessage)
        {
            ShowErrorMessage(page, title, errorMessage, null);
        }

        /// <summary>
        /// Mostra il messaggio di errore passato come parametro
        /// </summary>
        /// <param name="page"></param>
        /// <param name="title"></param>
        /// <param name="errorMessage"></param>
        /// <param name="jsCallbackFunctionName"></param>
        public static void ShowErrorMessage(Page page, string title, string errorMessage, string jsCallbackFunctionName)
        {
            RadWindowManager rwm = MasterPageHelper.GetRadWindowManager(page);
            if (rwm != null)
            {
                WindowHelper.ShowAlert(rwm, title, GetErrorMessage(errorMessage), jsCallbackFunctionName);
            }
        }

        /// <summary>
        /// Mostra il messaggio di errore relativo all'exception passata (completo dei messaggi delle eccezioni interne)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="errorMessage"></param>
        public static void ShowErrorMessage(Page page, Exception ex)
        {
            string message = SeCoGes.Utilities.ExceptionHelper.GetCompleteErrorMessage(ex);
            ShowErrorMessage(page, "Attenzione", message);
        }

        /// <summary>
        /// Mostra il messaggio di errore relativo all'exception passata (completo dei messaggi delle eccezioni interne)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="errorMessage"></param>
        public static void ShowErrorMessage(Page page, string title, Exception ex)
        {
            string message = SeCoGes.Utilities.ExceptionHelper.GetCompleteErrorMessage(ex);
            ShowErrorMessage(page, title, message);
        }

        #endregion

        /// <summary>
        /// Memorizza nella sessione corrente un messaggio rintracciabile tramite l'Id indicato
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        public static void SetMessage(Guid id, string message)
        {
            HttpContext.Current.Session[id.ToString()] = message;
        }

        /// <summary>
        /// Recupera delle variabili di sessione il messaggio relativo all'Id indicato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetMessage(Guid id)
        {
            if (HttpContext.Current.Session[id.ToString()] != null)
            {
                return (string)HttpContext.Current.Session[id.ToString()];
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Redirige l'utente alla pagina di visualizzazione del messaggio relativo all'id indicato
        /// </summary>
        /// <param name="messageId"></param>
        public static void RedirectToMessageErrorPage(Guid messageId)
        {
            HttpContext.Current.Response.Redirect(string.Format("/AccessDenied.aspx?messageId={0}", messageId.ToString()), false);
        }

        /// <summary>
        /// Redirige l'utente alla pagina di visualizzazione del messaggio passato
        /// </summary>
        /// <param name="message"></param>
        public static Guid RedirectToErrorPageWithMessage(string message)
        {
            Guid messageId = Guid.NewGuid();
            SetMessage(messageId, message);
            HttpContext.Current.Response.Redirect(string.Format("/AccessDenied.aspx?messageId={0}", messageId.ToString()), false);
            return messageId;
        }
    }
}