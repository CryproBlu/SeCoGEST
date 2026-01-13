using SeCoGEST.Web.LongProcesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;

namespace SeCoGEST.Web.WebServices
{
    /// <summary>
    /// Summary description for Notificatore
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Notificatore : System.Web.Services.WebService
    {
        /// <summary>
        /// Effettua l'invio di tutte le notifiche richieste dall'amministrazione
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [WebMethod]
        public bool InviaTutteLeNotifiche(string userName, string password)
        {
            try
            {
                Logic.Sicurezza.LoginResponseEnum loginResponse = Logic.Sicurezza.SecurityManager.Login(userName, password);
                if (loginResponse == Logic.Sicurezza.LoginResponseEnum.AccessoConsentito)
                {
                    InviaNotificheLongProcess longProcess = new InviaNotificheLongProcess();
                    ParameterizedThreadStart ts = new ParameterizedThreadStart(longProcess.EffettuaInvioGlobale);
                    Thread thd = new Thread(ts);
                    thd.IsBackground = true;
                    thd.Start(null);

                    return true;
                }
                else
                {
                    throw new Exception(String.Format("Accesso non consentito per l'account che ha come UserName '{0}'", userName));
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
