using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SeCoGEST.Data
{
    public class Interventi : Base.DataLayerBase
    {
        #region Costruttori

        public Interventi() : base(false) { }
        public Interventi(bool createStandaloneContext) : base(createStandaloneContext) { }
        public Interventi(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Intervento entityToCreate, bool submitChanges)
        {
            context.Interventos.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Intervento> Read()
        {
            return context.Interventos;
        }

        /// <summary>
        /// Restituisce gli Id degli Interventi che sono associati direttamente o indirettamente (tramite gli operatori) ad aree diverse
        /// </summary>
        /// <returns></returns>
        public IQueryable<Guid> ReadIdsInterventiConAreeMultiple()
        {
            return context.IdInterventiConAreeMultiple.Select(x => x.ID);
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.Intervento Find(Guid id)
        {
            return Read().Where(x => x.ID == id).SingleOrDefault();
        }


        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Intervento entityToDelete, bool submitChanges)
        {
            context.Interventos.DeleteOnSubmit(entityToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Elimina le entities passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.Intervento> entitiesToDelete, bool submitChanges)
        {
            context.Interventos.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion

        /// <summary>
        /// Restituiscce l'elenco di Contratti attivi per il Cliente indicato
        /// </summary>
        /// <param name="codiceCliente"></param>
        /// <param name="dataIntervento"></param>
        /// <returns></returns>
        public IEnumerable<Entities.InformazioniContratto> GetContrattiPerCliente(string codiceCliente, DateTime dataIntervento)
        {
            return context.GetContrattiPerCliente(codiceCliente, dataIntervento).ToList();
        }

        /// <summary>
        /// Esegue la procedure che indica a Metodo la generazione del documento relativo all'Intervento indicato
        /// </summary>
        /// <param name="idIntervento"></param>
        [Obsolete("Il metodo CreaDocXMLInterventi è stato deprecato. Utilizzare il nuovo metodo InviaInterventoAContabilita.")]
        public void CreaDocXMLInterventi(Guid idIntervento)
        {
            throw new Exception("Il metodo CreaDocXMLInterventi è stato deprecato. Utilizzare il nuovo metodo InviaInterventoAContabilita.");
            //int retValue = context.CreaDocXMLInterventi(idIntervento);
            //if (retValue != 0)
            //    throw new Exception("La procedura CreaDocXMLInterventi eseguita per generare il documento di Intervento in Metodo ha restituito un valore inatteso: " + retValue.ToString());
        }

        /// <summary>
        /// Esegue la procedure che indica a G7 la generazione del documento relativo all'Intervento indicato
        /// </summary>
        /// <param name="idIntervento"></param>
        /// <param name="apiUri"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool InviaInterventoAContabilita(Guid idIntervento, string apiUri, out string message)
        {
            bool ret = false;
            string apiUriWithParams = string.Concat(apiUri, idIntervento);

            HttpClient client = new HttpClient();
            client.Timeout= new TimeSpan(0,10,0);
            client.BaseAddress = new Uri(apiUriWithParams);
            //client.BaseAddress = new Uri(apiUri);
            client.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            //HttpResponseMessage response = client.GetAsync("?IDTESTA=" + idIntervento.ToString()).Result;
            HttpResponseMessage response = client.GetAsync(apiUriWithParams).Result;
            if (response.IsSuccessStatusCode)
            {
                ret = true;
                message = string.Empty;
            }
            else
            {
                ret = false;
                message = response.Content.ReadAsStringAsync().Result + "************" + apiUriWithParams;
            }

            client.Dispose();
            return ret;
        }

        public string TestG7API()
        {
            string message = string.Empty;
            string url = "http://192.168.1.12/SecogesApi/api/Test/GetTestValues";

            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 10, 0);
            client.BaseAddress = new Uri("http://192.168.1.12/SecogesApi/api/Test/GetTestValues");

            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                message = "IsSuccessStatusCode=True - " + response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                message = response.Content.ReadAsStringAsync().Result;
            }

            client.Dispose();
            return message;
        }
    }
}

