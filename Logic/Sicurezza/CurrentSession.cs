using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Logic.Sicurezza
{
    public static class CurrentSession
    {
        /// <summary>
        /// Restituisce l'istanza di tipo Account relativa all'utente autenticato
        /// </summary>
        /// <returns></returns>
        public static Entities.Account GetLoggedAccount()
        {
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity.Name != null)
            {
                Accounts ll = new Accounts();
                return ll.Find(System.Web.HttpContext.Current.User.Identity.Name);
            }
            else
            {
                return null;
            }
        }
    }
}
