using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Web.UI.PersistenceFramework;

namespace SeCoGEST.Web.Providers
{
    public class PersistenceSessionProvider : IStateStorageProvider
    {
        #region Costruttori

        public PersistenceSessionProvider(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("Parametro nullo.", "key");
            }

            StorageKey = key;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property di necessaria a memorizzare la chiave passata dal costruttore
        /// </summary>
        private string StorageKey { get; set; }

        #endregion

        #region Metodi Pubblici

        /// <summary>
        /// Restituisce lo stato degli oggetti recuperandoli dalla sessione in base alla chiave passata nel costruttore in modo tale che non sovrascriva gli stati
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string LoadStateFromStorage(string key)
        {
            return HttpContext.Current.Session[StorageKey] as string;
        }

        /// <summary>
        /// Memorizza lo stato degli oggetti nella sessione con la chiave passata nel costruttore in modo tale che non sovrascriva gli stati
        /// </summary>
        /// <param name="key"></param>
        /// <param name="serializedState"></param>
        public void SaveStateToStorage(string key, string serializedState)
        {
            HttpContext.Current.Session[StorageKey] = serializedState;
        }

        #endregion
    }
}