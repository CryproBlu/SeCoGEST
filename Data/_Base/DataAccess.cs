using System;

namespace SeCoGEST.Data.Base
{
    public class DataAccess
    {
        #region Elementi Privati

        /// <summary>
        /// Key for the DataContext in HttpContext.Current.Items
        /// </summary>
        private const string DATACONTEXT_ITEMS_KEY = "DatabaseDataContextKey";

        private static DatabaseDataContext dataContextInterno;

        private static string GetDatabaseConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
        }

        /// <summary>
        /// Private property to store the DataContext in HttpContext.Current.Items
        /// </summary>
        private static DatabaseDataContext InternalDataContext
        {
            get
            {
                if (System.Web.HttpContext.Current != null)
                {
                    return (DatabaseDataContext)System.Web.HttpContext.Current.Items[DATACONTEXT_ITEMS_KEY];
                }
                else
                {
                    return dataContextInterno;
                }
            }
            set
            {
                if (System.Web.HttpContext.Current != null)
                {
                    System.Web.HttpContext.Current.Items[DATACONTEXT_ITEMS_KEY] = value;
                }
                else
                {
                    dataContextInterno = value;
                }
            }
        }

        #endregion

        /// <summary>
        /// Returns the current data context. If none configured yet, then creates a new one and returns it.
        /// Internal access, so that only the DAL layer can access.
        /// </summary>
        /// <returns>A reference to EntityLayer.DatabaseDataContext</returns>
        internal static DatabaseDataContext CurrentDataContext
        {
            get
            {
                if (InternalDataContext == null)
                {
                    InternalDataContext = new DatabaseDataContext(GetDatabaseConnectionString());
                }
                return InternalDataContext;
            }
        }

        public static DatabaseDataContext GetDataContext(bool createNewInstance)
        {
            DatabaseDataContext context;

            if (createNewInstance)
            {
                context = new DatabaseDataContext(GetDatabaseConnectionString());
                if (context == null)
                    throw new ArgumentNullException("Si è verificato un errore critico durante il tentativo di creazione di una nuova istanza del DataContext!");
            }
            else
            {
                context = CurrentDataContext;
                if (context == null)
                    throw new ArgumentNullException("Si è verificato un errore critico durante il tentativo di lettura del DataContext condiviso!");
            }

            return context;
        }

    }
}
