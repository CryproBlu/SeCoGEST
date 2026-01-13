using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SeCoGEST.Web.WebServices
{
    /// <summary>
    /// Summary description for APIService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class APIService : System.Web.Services.WebService
    {

        [WebMethod]
        public string GetMediaTempoTickets(DateTime dataDal, DateTime dataAl, bool soloAperti)
        {
            try
            {
                Logic.Interventi llInterventi = new Logic.Interventi();
                IQueryable<Entities.Intervento> interventi = null;
                if(soloAperti)
                {
                    interventi = llInterventi.Read(Entities.StatoInterventoEnum.Aperto, Entities.StatoInterventoEnum.InGestione, Entities.StatoInterventoEnum.Eseguito);
                }
                else
                {
                    interventi = llInterventi.Read();
                }
                interventi = interventi.Where(i => i.DataRedazione >= dataDal && i.DataRedazione <= dataAl);



                var tempiInterventi = interventi.Select(i => new { DataInizio = i.DataRedazione, DataFine = i.Intervento_Operatores.Min(x => x.DataPresaInCarico) != null ? i.Intervento_Operatores.Min(x => x.DataPresaInCarico).Value : DateTime.Now });

                if (tempiInterventi.Count() > 0)
                {
                    return tempiInterventi.Average(i => (i.DataFine - i.DataInizio).TotalMinutes).ToString("F");
                }
                else
                {
                    return 0.ToString("F");
                }
            }
            catch(Exception ex)
            {
                return "--";
            }

        }
    }
}
