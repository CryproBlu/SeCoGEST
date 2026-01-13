using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Web.UI;

namespace SeCoGEST.Helper
{
    public static class WindowHelper
    {
        /// <summary>
        /// Mostra il messaggio di passato come parametro
        /// </summary>
        /// <param name="windowManager"></param>
        /// <param name="message"></param>
        public static void ShowAlert(RadWindowManager windowManager, string message)
        {
            ShowAlert(windowManager, "Attenzione", message);
        }

        /// <summary>
        /// Mostra il messaggio di passato come parametro
        /// </summary>
        /// <param name="windowManager"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public static void ShowAlert(RadWindowManager windowManager, string title, string message)
        {
            ShowAlert(windowManager, title, message, null);
        }

        /// <summary>
        /// Mostra il messaggio di passato come parametro
        /// </summary>
        /// <param name="windowManager"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="jsCallbackFunctionName"></param>
        public static void ShowAlert(RadWindowManager windowManager, string title, string message, string jsCallbackFunctionName)
        {
            if (windowManager == null)
            {
                throw new ArgumentNullException("Parametro nullo.", "windowManager");
            }

            windowManager.RadAlert(Web.ReplaceForHTML(message), 500, null, title, jsCallbackFunctionName);
        }
    }
}


