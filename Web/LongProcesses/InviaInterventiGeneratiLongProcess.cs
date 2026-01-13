using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace SeCoGEST.Web.LongProcesses
{
    public class InviaInterventiGeneratiLongProcess
    {
        #region Properties

        private List<Entities.EntityId<Entities.Intervento>> ElencoInterventiDaGenerare { get; set; }
        private Entities.EntityId<Entities.Account> IDAccountSicurezza { get; set; }

        #endregion

        #region Metodi Pubblici

        /// <summary>
        /// Effettua la generazione e l'invio di interventi in base ai parametri passati
        /// </summary>
        /// <param name="parametri"></param>
        public void GeneraInviaInterventi(object parametri)
        {
            lock (typeof(InviaInterventiGeneratiLongProcess))
            {
                try
                {
                    // Vengono recuperati i parametri necessari per effettuare la generazione ed invio degli interventi
                    object[] parameters = (object[])parametri;
                    ElencoInterventiDaGenerare = (List<Entities.EntityId<Entities.Intervento>>)parameters[0];
                    IDAccountSicurezza = (Entities.EntityId<Entities.Account>)parameters[1];

                    // Viene recuperato l'anagrafica dell'account a cui dev'essere inviata l'email
                    Logic.Sicurezza.Accounts llAccounts = new Logic.Sicurezza.Accounts();
                    Entities.Account entityAccount = llAccounts.Find(IDAccountSicurezza.Value);

                    if (entityAccount == null)
                    {
                        throw new Exception(String.Format("L'account con ID \"{0}\" non è presente nella fonte dati.", IDAccountSicurezza));
                    }
                    else if (String.IsNullOrEmpty(entityAccount.Email))
                    {
                        throw new Exception(String.Format("Non è possobile effettuare la generazione e l'invio dell'elenco degli interventi in quanto nell'account \"{0}\" (Username: \"{1}\") non è stata indicata una email.", entityAccount.Nominativo, entityAccount.UserName));
                    }

                    // Nel caso in cui esista almeno un intervento da generare ..
                    if (ElencoInterventiDaGenerare != null && ElencoInterventiDaGenerare.Count > 0)
                    {
                        List<FileInfo> elenco = new List<FileInfo>();
                        List<Entities.EntityId<Entities.Intervento>> elencoInterventiNonGenerati = new List<Entities.EntityId<Entities.Intervento>>();

                        GeneraInterventi(out elenco, out elencoInterventiNonGenerati);
                        EmailManager.InviaEmailInterventiGenerati(elenco, elencoInterventiNonGenerati, entityAccount);                        
                    }
                }
                catch (Exception ex)
                {
                    Exception errore = new Exception("GeneraInviaInterventi", ex);
                    EmailManager.InviaEmailAvvisoErrore(String.Empty, errore, String.Empty, String.Empty);
                }                
            }
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Effettua la generazione degli interventi passati come parametro al metodo "GeneraInviaInterventi"
        /// </summary>
        /// <param name="elencoInterventiGenerati"></param>
        /// <param name="elencoIdentificativiInterventiNonGenerati"></param>
        private void GeneraInterventi(out List<FileInfo> elencoInterventiGenerati, out List<Entities.EntityId<Entities.Intervento>> elencoIdentificativiInterventiNonGenerati)
        {
            List<FileInfo> elencoFileInterventiGenerati = new List<FileInfo>();
            List<Entities.EntityId<Entities.Intervento>> elencoIdInterventi = new List<Entities.EntityId<Entities.Intervento>>();

            // Per ogni identificativo nell'elenco degli interventi da generare
            foreach (Entities.EntityId<Entities.Intervento> identificativoIntervento in ElencoInterventiDaGenerare)
            {
                try
                {
                    // Effettua la generazione dell'intervento correte del ciclo ed il percorso viene aggiunto alla lista dei file generati
                    string percorsoInterventoGenerato = Logic.GestoreDocumenti.GeneraIntervento(identificativoIntervento);
                    elencoFileInterventiGenerati.Add(new FileInfo(percorsoInterventoGenerato));
                }
                catch(Exception ex)
                {
                    // Viene aggiunto l'identificativo del documento la cui generazione è andata in errore
                    elencoIdInterventi.Add(identificativoIntervento);
                }
            }

            elencoInterventiGenerati = elencoFileInterventiGenerati;
            elencoIdentificativiInterventiNonGenerati = elencoIdInterventi;
        }

        #endregion

    }
}