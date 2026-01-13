using SeCoGEST.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace SeCoGEST.Web
{
    public static class RadPersistenceHelper
    {
        #region Costanti

        /// <summary>
        /// Contiene il prefisso da inserire nelle chiavi dell'oggetto RadPersistenceManager
        /// </summary>
        private const string PERSISTENCE_KEY_PREFIX = "PERSISTENCE_KEY";

        #endregion

        #region Metodi Pubblici

        /// <summary>
        /// Restituisce l'oggetto Telerik.Web.UI.RadPersistenceManager
        /// </summary>
        /// <param name="pageObject"></param>
        /// <returns></returns>
        public static Telerik.Web.UI.RadPersistenceManager GetPersistenceManager(Page pageObject)
        {
            if (pageObject == null)
            {
                throw new ArgumentNullException("Parametro nullo.", "pageObject");
            }

            Telerik.Web.UI.RadPersistenceManager persistenceManagerInstance = Telerik.Web.UI.RadPersistenceManager.GetCurrent(pageObject);
            if (persistenceManagerInstance == null && MasterPageHelper.GetRadPersistenceManager(pageObject) != null)
            {
                persistenceManagerInstance = MasterPageHelper.GetRadPersistenceManager(pageObject);
            }

            if (persistenceManagerInstance == null)
            {
                throw new Exception("Non è stato possibile recuperare l'oggetto 'Telerik.Web.UI.RadPersistenceManager'.");
            }
            else
            {
                return persistenceManagerInstance;
            }
        }

        /// <summary>
        /// Associa il provider 'PersistenceSessionProvider' all'oggetto Telerik.Web.UI.RadPersistenceManager, Utilizzare nel Page_Init
        /// </summary>
        /// <param name="pageObject"></param>
        public static void AssociatePersistenceSessionProvider(Page pageObject)
        {
            if (pageObject == null)
            {
                throw new ArgumentNullException("Parametro nullo.", "pageObject");
            }

            Telerik.Web.UI.RadPersistenceManager persistenceManagerInstance = GetPersistenceManager(pageObject);

            if (persistenceManagerInstance == null)
            {
                throw new Exception("Non è stato possibile recuperare l'oggetto 'Telerik.Web.UI.RadPersistenceManager'.");
            }
            else
            {
                persistenceManagerInstance.StorageProvider = new Providers.PersistenceSessionProvider(GetProviderKeyName(pageObject));
            }
        }

        /// <summary>
        /// Chiama il metodo LoadState dell'oggetto Telerik.Web.UI.RadPersistenceManager
        /// </summary>
        /// <param name="pageObject"></param>
        public static void LoadState(Page pageObject)
        {
            if (pageObject == null)
            {
                throw new ArgumentNullException("Parametro nullo.", "pageObject");
            }

            Telerik.Web.UI.RadPersistenceManager persistenceManagerInstance = GetPersistenceManager(pageObject);

            if (persistenceManagerInstance == null)
            {
                throw new Exception("Non è stato possibile recuperare l'oggetto 'Telerik.Web.UI.RadPersistenceManager'.");
            }
            else
            {
                if (HttpContext.Current != null &&
                    HttpContext.Current.Session != null &&
                    HttpContext.Current.Session[GetProviderKeyName(pageObject)] != null)
                {
                    persistenceManagerInstance.LoadState();
                }
            }
        }

        /// <summary>
        /// Chiama il metodo SaveState dell'oggetto Telerik.Web.UI.RadPersistenceManager
        /// </summary>
        /// <param name="pageObject"></param>
        public static void SaveState(Page pageObject)
        {
            if (pageObject == null)
            {
                throw new ArgumentNullException("Parametro nullo.", "pageObject");
            }

            Telerik.Web.UI.RadPersistenceManager persistenceManagerInstance = GetPersistenceManager(pageObject);

            if (persistenceManagerInstance == null)
            {
                throw new Exception("Non è stato possibile recuperare l'oggetto 'Telerik.Web.UI.RadPersistenceManager'.");
            }
            else
            {
                persistenceManagerInstance.SaveState();
            }
        }

        /// <summary>
        /// Restituisce tutte le chiavi utilizzate dal RadPersistenceManager per memorizzare lo stato degli oggetti 
        /// </summary>
        /// <returns></returns>
        public static IDictionary<string, string> GetPersistenceSettingsCollection()
        {
            return GetPersistenceSettingsCollection(false);
        }

        /// <summary>
        /// Restituisce tutte le chiavi utilizzate dal RadPersistenceManager per memorizzare lo stato degli oggetti 
        /// </summary>
        /// <param name="manageException"></param>
        /// <returns></returns>
        public static IDictionary<string, string> GetPersistenceSettingsCollection(bool manageException)
        {
            return GetPersistenceSettingsCollection(null, manageException);
        }

        /// <summary>
        /// Restituisce tutte le chiavi utilizzate dal RadPersistenceManager per memorizzare lo stato degli oggetti 
        /// </summary>
        /// <param name="skinManager"></param>
        /// <returns></returns>
        public static IDictionary<string, string> GetPersistenceSettingsCollection(Telerik.Web.UI.RadSkinManager skinManager)
        {
            return GetPersistenceSettingsCollection(skinManager, false);
        }

        /// <summary>
        /// Restituisce tutte le chiavi utilizzate dal RadPersistenceManager per memorizzare lo stato degli oggetti 
        /// </summary>
        /// <param name="skinManager"></param>
        /// <param name="manageException"></param>
        /// <returns></returns>
        public static IDictionary<string, string> GetPersistenceSettingsCollection(Telerik.Web.UI.RadSkinManager skinManager, bool manageException)
        {
            Dictionary<string, string> persistenceCollection = new Dictionary<string, string>();
            try
            {
                if (HttpContext.Current == null || HttpContext.Current.Session == null || HttpContext.Current.Session.Keys.Count <= 0) { return persistenceCollection; }

                foreach (string key in HttpContext.Current.Session.Keys)
                {
                    if (key.StartsWith(PERSISTENCE_KEY_PREFIX) && HttpContext.Current.Session[key] is string)
                    {
                        persistenceCollection.Add(key, (string)HttpContext.Current.Session[key]);
                    }
                }

                if (skinManager != null)
                {
                    persistenceCollection.Add(skinManager.PersistenceKey, skinManager.Skin);
                }

                return persistenceCollection;
            }
            catch (Exception ex)
            {
                if (manageException)
                {
                    return persistenceCollection;
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Inserisce nella Session tutte le chiavi e valori, presenti nel dictionary passato come parametro, necessari all'oggetto RadPersistenceManager per impostare dei valori predefiniti agli oggetti
        /// </summary>
        /// <param name="persistenceSettingsCollection"></param>
        public static void SetPersistenceSettingsCollection(IDictionary<string, string> persistenceSettingsCollection)
        {
            SetPersistenceSettingsCollection(persistenceSettingsCollection, true, false);
        }

        /// <summary>
        /// Inserisce nella Session tutte le chiavi e valori, presenti nel dictionary passato come parametro, necessari all'oggetto RadPersistenceManager per impostare dei valori predefiniti agli oggetti
        /// </summary>
        /// <param name="persistenceSettingsCollection">true per sovrascrivere il valore di una chiave presente nella session</param>
        /// <param name="overrideSessionValueIfKeyExist"></param>
        public static void SetPersistenceSettingsCollection(IDictionary<string, string> persistenceSettingsCollection, bool overrideSessionValueIfKeyExist)
        {
            SetPersistenceSettingsCollection(persistenceSettingsCollection, overrideSessionValueIfKeyExist, false);
        }

        /// <summary>
        /// Inserisce nella Session tutte le chiavi e valori, presenti nel dictionary passato come parametro, necessari all'oggetto RadPersistenceManager per impostare dei valori predefiniti agli oggetti
        /// </summary>
        /// <param name="persistenceSettingsCollection"></param>
        /// <param name="overrideSessionValueIfKeyExist">true per sovrascrivere il valore di una chiave presente nella session</param>
        /// <param name="manageException"></param>
        public static void SetPersistenceSettingsCollection(IDictionary<string, string> persistenceSettingsCollection, bool overrideSessionValueIfKeyExist, bool manageException)
        {
            try
            {
                if (persistenceSettingsCollection == null)
                {
                    throw new ArgumentNullException("Parametro nullo.", "persistenceSettingsCollection");
                }

                if (HttpContext.Current == null || HttpContext.Current.Session == null || HttpContext.Current.Session.Keys.Count <= 0) { return; }

                if (persistenceSettingsCollection.Count > 0)
                {
                    foreach (KeyValuePair<string, string> keyValuePair in persistenceSettingsCollection)
                    {
                        if (HttpContext.Current.Session[keyValuePair.Key] == null ||
                            (HttpContext.Current.Session[keyValuePair.Key] != null && overrideSessionValueIfKeyExist))
                        {
                            HttpContext.Current.Session[keyValuePair.Key] = keyValuePair.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (manageException)
                {
                    return;
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Recupera dalla pagina il nome della chiave utilizzata dal PersistenceSessionProvider
        /// </summary>
        /// <param name="pageObject"></param>
        /// <returns></returns>
        public static string GetProviderKeyName(Page pageObject)
        {
            return GetProviderKeyName(pageObject, false);
        }

        /// <summary>
        /// Recupera dalla pagina il nome della chiave utilizzata dal PersistenceSessionProvider
        /// </summary>
        /// <param name="pageObject"></param>
        /// <param name="manageException"></param>
        /// <returns></returns>
        public static string GetProviderKeyName(Page pageObject, bool manageException)
        {
            try
            {
                if (pageObject == null)
                {
                    throw new ArgumentNullException("Parametro nullo.", "pageObject");
                }
                string pathAndQuery = pageObject.Request.Url.PathAndQuery;
                string telerikParameter = String.Format("&{0}=", ConfigurationKeys.TELERIK_URI_IDENTIFIER).ToLower();
                // Verifico la presenza del identificativo uri della telerik
                if (!String.IsNullOrEmpty(pathAndQuery) && pathAndQuery.ToLower().Contains(telerikParameter))
                {
                    // Recupero l'index in cui si trova l'uri
                    int radUriIndex = pathAndQuery.ToLower().LastIndexOf(telerikParameter);
                    if (radUriIndex > 0)
                    {
                        // Recupero l'uri telerik e lo sostituisco con la stringa vuota
                        string pathToReplace = pathAndQuery.Substring(radUriIndex);
                        pathAndQuery = pathAndQuery.Replace(pathToReplace, String.Empty);
                    }
                }

                pathAndQuery = pathAndQuery.Replace('/', '_');

                return String.Concat(PERSISTENCE_KEY_PREFIX, pathAndQuery);
            }
            catch (Exception ex)
            {
                if (manageException)
                {
                    return String.Empty;
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Recupera dalla pagina il valore della chiave utilizzata dal PersistenceSessionProvider
        /// </summary>
        /// <param name="pageObject"></param>
        /// <returns></returns>
        public static string GetProviderKeyValue(Page pageObject)
        {
            return GetProviderKeyValue(pageObject, false);
        }

        /// <summary>
        /// Recupera dalla pagina il valore della chiave utilizzata dal PersistenceSessionProvider
        /// </summary>
        /// <param name="pageObject"></param>
        /// <returns></returns>
        public static string GetProviderKeyValue(Page pageObject, bool manageException)
        {
            try
            {
                string keyName = GetProviderKeyName(pageObject, manageException);

                if (String.IsNullOrEmpty(keyName))
                {
                    throw new Exception("Non è stato possibile recuperare il nome della chiave.");
                }

                if (HttpContext.Current == null || HttpContext.Current.Session == null || HttpContext.Current.Session.Keys.Count <= 0) { return String.Empty; }

                return HttpContext.Current.Session[keyName] as string;
            }
            catch (Exception ex)
            {
                if (manageException)
                {
                    return String.Empty;
                }
                else
                {
                    throw ex;
                }
            }
        }

        #endregion
    }
}