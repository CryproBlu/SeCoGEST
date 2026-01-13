using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGEST.Entities;
using SeCoGEST.Helper;
using msWord = Microsoft.Office.Interop.Word;

namespace SeCoGEST.Logic
{
    public class Interventi : Base.LogicLayerBase
    {
        //public enum TipologiaLetturaInterventi
        //{
        //    Tutti = -1,
        //    SoloAperti = 0,
        //    SoloEseguiti = 1,
        //    SoloChiusi = 2,
        //    SoloValidati = 3
        //}

        public enum TipologiaLetturaInterventiCliente
        {
            Tutti = -1,
            SoloNonVisibili = 0,
            SoloVisibili = 1
        }

        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.Interventi dal;

        private Logic.Intervento_Stati llInterventiStati;
        private Logic.Sicurezza.Accounts llAccount;
        private Logic.AccountsOperatori llAccountsOperatori;
        private Logic.Intervento_Operatori llInterventiOperatori;
        private Logic.LogInvioNotifiche llLogInvioNotifiche;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public Interventi()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public Interventi(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public Interventi(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.Interventi(this.context);
            llInterventiStati = new Intervento_Stati(this);
            llAccount = new Sicurezza.Accounts(this);
            llAccountsOperatori = new AccountsOperatori(this);
            llInterventiOperatori = new Intervento_Operatori(this);
            llLogInvioNotifiche = new LogInvioNotifiche(this);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Intervento entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                if (entityToCreate.ID.Equals(Guid.Empty))
                {
                    entityToCreate.ID = Guid.NewGuid();
                    entityToCreate.Numero = GetNuovoNumeroIntervento();
                }

                // Salvataggio nel database
                dal.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'Intervento': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Intervento> Read()
        {
            return from u in dal.Read() orderby u.Numero descending select u;
        }

        /// <summary>
        /// Restituisce tutte le entities in base allo stato del'Intervento
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Intervento> Read(params StatoInterventoEnum[] stato)
        {
            IQueryable<Entities.Intervento> queryBase = dal.Read();
            if (stato.Length != 0)
            {
                List<int> stati = stato.Select(x => x.GetHashCode()).ToList();
                queryBase = queryBase.Where(x => x.Stato.HasValue && stati.Contains(x.Stato.Value));
            }
            return queryBase.OrderBy(x => x.Numero);
        }

        /// <summary>
        /// Restituisce tutte le entities in base all'account passato
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Intervento> ReadByAccount(Account account)
        {
            if (account.Amministratore.HasValue && account.Amministratore.Value)
            {
                return Read();
            }
            else
            {
                return Read().Where(intervento => intervento.Intervento_Operatores.Any(interventoOperatore => interventoOperatore.Operatore != null && interventoOperatore.Operatore.AccountOperatores.Any(accountOperatore => accountOperatore.IDAccount == account.ID)));

                //List<Operatore> operatoriAssociatiAlloAccount = llAccountsOperatori.ReadOperatori(account.Identifier).ToList();
                //List<Intervento_Operatore> interventiAssociatiAgliOperatori = llInterventiOperatori.Read(operatoriAssociatiAlloAccount).ToList();
                //List<Guid> elencoIDIntervento = interventiAssociatiAgliOperatori.Select(x => x.IDIntervento).ToList();

                //// Vengono recuperati tutti gli interventi in cui sono stati associati degli operatori che sono anche associati all'operatore passato come parametro
                //return Read().Where(intervento => elencoIDIntervento.Contains(intervento.ID));
            }
        }

        /// <summary>
        /// Restituisce tutti gli interventi accessibili dall'account cliente ed alla tipologia di lettura da applicare
        /// </summary>
        /// <param name="accountCliente"></param>
        /// <param name="tipologiaLettura"></param>
        /// <returns></returns>
        public IQueryable<Entities.Intervento> ReadForAccountCliente(Account accountCliente, TipologiaLetturaInterventiCliente tipologiaLettura = TipologiaLetturaInterventiCliente.Tutti)
        {
            if (accountCliente == null) throw new ArgumentNullException("accountLoggato", "Parametro nullo");

            IQueryable<Entities.Intervento> elencoInterventi = null;

            // Gli interventi vengono restituiti in base al ruolo dell'account autenticato
            if (accountCliente.TipologiaEnum == TipologiaAccountEnum.ClienteStandard)
            {
                // Se l'account autenticato è un ClienteStandard allora vede solo gli interventi della propria organizzazione e creati da se stesso
                elencoInterventi = Read().Where(x => 
                    x.CodiceCliente == accountCliente.CodiceCliente && 
                    x.IDAccountUtenteRiferimento == accountCliente.ID);
            }
            else if (accountCliente.TipologiaEnum == TipologiaAccountEnum.ClienteSupervisore)
            {
                // Se l'account autenticato è un ClienteSupervisore allora vede solo gli interventi della propria organizzazione ma creati da qualsiasi utente
                elencoInterventi = Read().Where(x => 
                    x.CodiceCliente == accountCliente.CodiceCliente &&
                    x.IdDestinazione == accountCliente.IdDestinazione);
            }
            else if (accountCliente.TipologiaEnum == TipologiaAccountEnum.ClienteAdmin) // || accountLoggato.TipologiaEnum == TipologiaAccountEnum.SeCoGes)
            {
                // Se l'account autenticato è un ClienteAdmin allora vede gli interventi della propria organizzazione, di tutte quelle collegate e creati da qualsiasi utente
                elencoInterventi = Read().Where(x => x.CodiceCliente == accountCliente.CodiceCliente);


                //Logic.Metodo.AnagraficaDestinazioniMerce llDest = new Logic.Metodo.AnagraficaDestinazioniMerce(this);
                //List<string> clientiDestinazioni = llDest.Read(accountLoggato.CodiceCliente).Select(x => x.RagioneSociale).ToList();
                //Logic.Metodo.AnagraficheClienti llCli = new Logic.Metodo.AnagraficheClienti(this);
                //List<string> codiciClientiDestinazioni = llCli.Read().Where(c => clientiDestinazioni.Contains((c.DSCCONTO1 + c.DSCCONTO2).Trim())).Select(c => c.CODCONTO).ToList();
                ////List<string> codiciClientiDestinazioni = ll.Read(accountLoggato.CodiceCliente).Select(x => x.CodiceDestinazione.ToString()).ToList();
                //elencoInterventi = Read().Where(x => codiciClientiDestinazioni.Contains(x.CodiceCliente));
            }
            else if (accountCliente.TipologiaEnum == TipologiaAccountEnum.SeCoGes)
            {
                throw new Exception("Questa procedura è utilizzabile solamente dall'account di un cliente");
            }


            // Legge tutti gli interventi associati al cliente indicato
            //IQueryable<Entities.Intervento> elencoInterventi = Read().Where(x => x.CodiceCliente == accountLoggato.CodiceCliente);



                if (tipologiaLettura == TipologiaLetturaInterventiCliente.SoloVisibili)
            {
                // Filtra gli interventi mantenendo solo quelli indicati come "Visibili al cliente"
                elencoInterventi = elencoInterventi.Where(x => x.VisibileAlCliente);
            }
            else if (tipologiaLettura == TipologiaLetturaInterventiCliente.SoloNonVisibili)
            {
                // Filtra gli interventi mantenendo solo quelli indicati come "Non visibili al cliente"
                elencoInterventi = elencoInterventi.Where(x => !x.VisibileAlCliente);
            }


            //if (accountLoggato.TipologiaEnum == TipologiaAccountEnum.ClienteStandard)
            //{
            //    // Filtra gli interventi mantenendo solo quelli creati dall'utente indicato
            //    elencoInterventi = elencoInterventi.Where(x => x.IDAccountUtenteRiferimento == accountLoggato.ID);
            //}

            return elencoInterventi;
        }

        /// <summary>
        /// Restituisce tutti gli intervento che risultano non chiusi e non validati
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Intervento> ReadNonChiusiNonValidati()
        {
            IQueryable<Entities.Intervento> queryBase = Read();
            queryBase = queryBase.Where(x => x.Stato.HasValue &&
                                             x.Stato.Value != (byte)StatoInterventoEnum.Chiuso &&
                                             x.Stato.Value != (byte)StatoInterventoEnum.Validato);
            return queryBase;
        }

        /// <summary>
        /// Restituisce tutte le entities che non sono state associate ad un operatore ed alla tipologia di lettura da applicare
        /// </summary>
        /// <param name="accountLoggato"></param>
        /// <param name="tipologiaLettura"></param>
        /// <returns></returns>
        public IEnumerable<Entities.Intervento> ReadSenzaOperatoriAssociati(TipologiaLetturaInterventiCliente tipologiaLettura = TipologiaLetturaInterventiCliente.Tutti)
        {
            IQueryable<Entities.Intervento> elencoInterventi = Read().Where(x => (!x.Interno.HasValue || !x.Interno.Value) &&
                                                                                 (x.Chiuso.HasValue && x.Chiuso.Value) &&
                                                                                 (x.Intervento_Operatores == null || x.Intervento_Operatores.Count <= 0));

            if (tipologiaLettura == TipologiaLetturaInterventiCliente.SoloVisibili)
            {
                elencoInterventi = elencoInterventi.Where(x => x.VisibileAlCliente);
            }
            else if (tipologiaLettura == TipologiaLetturaInterventiCliente.SoloNonVisibili)
            {
                elencoInterventi = elencoInterventi.Where(x => !x.VisibileAlCliente);
            }

            List<Entities.Intervento> elencoInterventiList = elencoInterventi.ToList();

            List<Entities.Intervento> elencoInterventiOperatoriDaNotificare = new List<Intervento>();
            if (elencoInterventiList != null && elencoInterventiList.Count > 0)
            {
                foreach (Entities.Intervento intervento in elencoInterventiList)
                {
                    DateTime? ultimoInvio = llLogInvioNotifiche.GetDataUltimoInvioNotifica<Entities.Intervento>(intervento.Identifier, InfoOperazioneTabellaEnum.Intervento, TipologiaNotificaEnum.InterventoPresoInCaricoNonChiuso);
                    if (!ultimoInvio.HasValue)
                    {
                        ultimoInvio = intervento.DataRedazione;
                    }

                    DateTime dataAttuale = DateTime.Now;
                    TimeSpan periodoTrascorso = dataAttuale.Subtract(ultimoInvio.Value);
                    TimeSpan periodoNonLavorativo = llLogInvioNotifiche.GetTotalePeriodoNonLavorativo(dataAttuale, ultimoInvio.Value);
                    TimeSpan periodoLavorativo = periodoTrascorso.Subtract(periodoNonLavorativo);

                    if (intervento.AnagraficaClienti == null || Math.Abs(periodoLavorativo.TotalMinutes) > Math.Abs(Infrastructure.ConfigurationKeys.MINUTI_MASSIMI_INVIO_NOTIFICA_INTERVENTI_SENZA_OPERATORI))
                    {
                        elencoInterventiOperatoriDaNotificare.Add(intervento);
                    }
                }
            }

            return elencoInterventiOperatoriDaNotificare;
        }

        //public IQueryable<Entities.Intervento> ReadInterventiConConfigurazioneTipologiaTicketCliente(params StatoInterventoEnum[] stato)
        //{
        //    IQueryable<Entities.Intervento> queryBase = Read(stato);
        //    queryBase = queryBase.Where(x => x.ConfigurazioneTipologiaTicketCliente != null);
        //    return queryBase;
        //}

        //public List<Entities.Intervento> ReadTicketsConConfigurazionePerNotifiche(TipologiaLetturaInterventi tipologiaLettura = TipologiaLetturaInterventi.SoloAperti)
        //{
        //    IQueryable<Entities.Intervento> queryBase = ReadInterventiConConfigurazioneTipologiaTicketCliente(tipologiaLettura);
        //    List<Entities.Intervento> interventi = new List<Intervento>();

        //    foreach (Entities.Intervento intervento in queryBase)
        //    {
        //        if(llLogInvioNotifiche.IsPrimoInvioEffettuato<Entities.Intervento>(intervento.Identifier, TipologiaNotificaEnum.InterventoDaNotificarePerSLA))
        //        {
        //            continue;
        //        }
        //        else
        //        {
        //            interventi.Add(intervento);
        //        }
        //    }

        //    return interventi;
        //}



        /// <summary>
        /// Restituisce gli Id degli Interventi che sono associati direttamente o indirettamente (tramite gli operatori) ad aree diverse
        /// </summary>
        /// <returns></returns>
        public IQueryable<Guid> ReadIdsInterventiConAreeMultiple()
        {
            return dal.ReadIdsInterventiConAreeMultiple();
        }


        /// <summary>
        /// Restituisce l'entity relativa all'IDentificativo passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.Intervento Find(EntityId<Intervento> identificativoIntervento)
        {
            return Find(identificativoIntervento.Value);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.Intervento Find(Guid idToFind)
        {
            return dal.Find(idToFind);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Intervento entityToDelete, bool submitChanges)
        {
            Delete(entityToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Intervento entityToDelete, bool checkAllegati, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'Intervento': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.Intervento> entitiesToDelete, bool submitChanges)
        {
            Delete(entitiesToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.Intervento> entitiesToDelete, bool checkAllegati, bool submitChanges)
        {
            if (entitiesToDelete != null)
            {
                if (entitiesToDelete.Count() > 0)
                {
                    entitiesToDelete.ToList().ForEach(x => Delete(x, checkAllegati, submitChanges));
                }
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'Intervento': parametro nullo!");
            }
        }

        #endregion

        #region Funzioni Spedifiche

        /// <summary>
        /// Restituisce true se l'intervento associato all'identificativo passato è stato preso in carico
        /// </summary>
        /// <param name="identificativoIntervento"></param>
        /// <returns></returns>
        public bool IsPresoInCarico(EntityId<Intervento> identificativoIntervento)
        {
            return Read().Any(i => i.ID == identificativoIntervento.Value &&
                                           i.Intervento_Operatores.Any(io => io.PresaInCarico.HasValue && io.PresaInCarico.Value));
        }

        /// <summary>
        /// Restituisce l'account che ha effettuato la creazione dell'intervento
        /// </summary>
        /// <param name="identificativoIntervento"></param>
        /// <returns></returns>
        public Entities.Account GetAccountCreazione(EntityId<Intervento> identificativoIntervento)
        {
            Entities.Account accountDaRestituire = null;

            if (identificativoIntervento != null && identificativoIntervento.HasValue)
            {
                // Per recuperare l'account che ha creato l'intervento è necessario avere tutti i record relativi agli stati,
                // ordinarli per data in modo ascendente e prendere il primo
                IQueryable<Intervento_Stato> elencoStatiIntervento = llInterventiStati.Read(identificativoIntervento, true);

                if (elencoStatiIntervento != null && elencoStatiIntervento.Count() > 0)
                {
                    Entities.Intervento_Stato statoInterventoCreazione = elencoStatiIntervento.FirstOrDefault();
                    if (statoInterventoCreazione != null)
                    {
                        accountDaRestituire = llAccount.Find(statoInterventoCreazione.NomeUtente);
                    }
                }
            }

            return accountDaRestituire;
        }

        /// <summary>
        /// Restituisce il numero da utilizzare per il prossimo nuovo bollettino
        /// </summary>
        /// <returns></returns>
        public int GetNuovoNumeroIntervento()
        {
            int? max = dal.Read().Select(x => (int?)x.Numero).Max();
            if (max.HasValue)
                return max.Value + 1;
            else
                return 1;
        }

        /// <summary>
        /// Restituiscce l'elenco di Contratti attivi per il Cliente indicato
        /// </summary>
        /// <param name="codiceCliente"></param>
        /// <param name="dataIntervento"></param>
        /// <returns></returns>
        public IEnumerable<Entities.InformazioniContratto> GetContrattiPerCliente(string codiceCliente, DateTime dataIntervento)
        {
            return dal.GetContrattiPerCliente(codiceCliente, dataIntervento);
        }

        /// <summary>
        /// Esegue la procedure che indica a Metodo la generazione del documento relativo all'Intervento indicato
        /// </summary>
        /// <param name="idIntervento"></param>
        [Obsolete("Il metodo CreaDocXMLInterventi è stato deprecato. Utilizzare il nuovo metodo InviaInterventoAContabilita.")]
        public void CreaDocXMLInterventi(Guid idIntervento)
        {
            dal.CreaDocXMLInterventi(idIntervento);
        }

        /// <summary>
        /// Esegue la procedure che indica a G7 la generazione del documento relativo all'Intervento indicato
        /// </summary>
        /// <param name="idIntervento"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool InviaInterventoAContabilita(Guid idIntervento, out string message)
        {
            return dal.InviaInterventoAContabilita(idIntervento, Infrastructure.ConfigurationKeys.G7_TICKET_IMPORT_API_URL, out message);
        }

        public string TestG7API()
        {
            return dal.TestG7API();
        }


        /// <summary>
        /// Effettua la generazione del documento di intervento in base all'identificativo passato come parametro, salvandolo nella directory il cui percorso è passato come parametro
        /// </summary>
        /// <param name="identificativoIntervento"></param>
        /// <param name="directoryDiSalvataggio"></param>
        /// <returns></returns>
        public string GeneraDocumento(EntityId<Intervento> identificativoIntervento, string directoryDiSalvataggio)
        {
            // Viene verificato il settaggio dell'identificatico dell'intervento .. Nel caso il parametro non sia valorizzato ..
            if (identificativoIntervento == null)
            {
                // Viene scatenato un errore ..
                throw new ArgumentNullException("identificativoIntervento", "Parametro nullo");
            }
            else
            {
                // Nel caso in cui l'entity relativa all'intervento, recuperato mediante l'identificativo passato come parametro, non sia valorizzata ..
                Intervento entityIntervento = Find(identificativoIntervento.Value);
                if (entityIntervento == null)
                {
                    // Viene scatenato un errore ..
                    throw new ArgumentException(String.Format("L'intervento con l'identificativo passato, \"{0}\", non è presente nella fonte dati", identificativoIntervento.Value));
                }

                // Nel caso in cui la directory passata come parametro non esista ..
                if (!Directory.Exists(directoryDiSalvataggio))
                {
                    // Viene creata la directory
                    Directory.CreateDirectory(directoryDiSalvataggio);
                }

                // Viene generato il nome del file in cui salvare il documento
                string nomeDocumentoGenerato = GestoreDocumenti.GetNomeDocumentoIntervento(entityIntervento);

                // Vengono inizializzate le variabili necessarie per la generazione del documento
                Exception errore = null;
                string fileDocumentoGenerato = Path.Combine(directoryDiSalvataggio, nomeDocumentoGenerato);
                msWord.Application application = null;
                msWord.Document document = null;

                object template = Infrastructure.ConfigurationKeys.PERCORSO_MODELLO_DOCUMENTO_INTERVENTO;
                if (entityIntervento.IdTipologia.HasValue)
                {
                    if(new Logic.TipologieIntervento().GetVersioneModelloEsportazione(entityIntervento.IdTipologia.Value) == 2)
                    {
                        template = Infrastructure.ConfigurationKeys.PERCORSO_MODELLO_DOCUMENTO_INTERVENTO_Versione2;
                    }
                    //if(Infrastructure.ConfigurationKeys.IDS_TIPOLOGIE_INTERVENTO_PER_MODELLO_DOCUMENTO_VERSIONE_2.Contains(entityIntervento.IdTipologia.Value.ToString().ToUpper()))
                    //{
                    //    template = Infrastructure.ConfigurationKeys.PERCORSO_MODELLO_DOCUMENTO_INTERVENTO_Versione2;
                    //}
                    //if (entityIntervento.Tipologia.Value == TipologiaInterventoEnum.Consulenza.GetHashCode() ||
                    //   entityIntervento.Tipologia.Value == TipologiaInterventoEnum.MUA.GetHashCode() ||
                    //   entityIntervento.Tipologia.Value == TipologiaInterventoEnum.DPO.GetHashCode() ||
                    //   entityIntervento.Tipologia.Value == TipologiaInterventoEnum.Formazione.GetHashCode())
                    //    template = Infrastructure.ConfigurationKeys.PERCORSO_MODELLO_DOCUMENTO_INTERVENTO_Versione2;
                }


                // Nel caso in cui nella directory temporanea esista un file con lo stesso nome di quello da generare ..
                if (File.Exists(fileDocumentoGenerato))
                {
                    // Viene rimosso il file
                    File.Delete(fileDocumentoGenerato);
                }

                try
                {
                    // Vengono inizializzati sia il word che il documento che contiene il modello del documento di intervento
                    application = WordHelper.InizializeApplication();
                    document = WordHelper.AddDocument(application, template.ToString());

                    // Viene popolato il documento di intervento in base all'intervento recuperato ..
                    PopolaDocumentoIntervento(application, document, entityIntervento);

                    // Viene effettuato il salvataggio del documento ..
                    WordHelper.SaveDocument(document, fileDocumentoGenerato, msWord.WdSaveFormat.wdFormatPDF);
                }
                catch (Exception ex)
                {
                    // Viene memorizzato l'errore ed azzerato il percorso del documento generato 
                    errore = ex;
                    fileDocumentoGenerato = "";
                }
                finally
                {
                    // Vengono rilasciate le risorse occupate per la generazione del documento 
                    WordHelper.RilascioRisorseWord(application, document, ref template);
                }

                // Nel caso in cui l'errore sia valorizzato, viene scatenato l'errore
                if (errore != null) throw errore;

                // Viene restituito il percorso del documento generato 
                return fileDocumentoGenerato;
            }
        }

        #endregion

        #region Funzioni Accessorie

        #region Settaggio di valori

        /// <summary>
        /// Effettua il popolamento del documento relativo all'intervento passato come parametro
        /// </summary>
        /// <param name="application"></param>
        /// <param name="document"></param>
        /// <param name="entityIntervento"></param>
        private void PopolaDocumentoIntervento(msWord.Application application, msWord.Document document, Intervento entityIntervento)
        {
            // Viene inizializzata la collection che contiene gli errori 
            SeCoGes.Utilities.MessagesCollector errori = new SeCoGes.Utilities.MessagesCollector();

            // Vengono controllati i parametri passati alla funzione
            if (application == null)
            {
                errori.Add("Parametro nullo: 'application'");
            }

            if (document == null)
            {
                errori.Add("Parametro nullo: 'document'");
            }

            if (entityIntervento == null)
            {
                errori.Add("Parametro nullo: 'entityIntervento'");
            }

            // Nel caso in cui ci siano degli errori, viene generato un errore ..
            if (errori.HaveMessages) throw new Exception(errori.ToString(Environment.NewLine));

            // Vengono settati i bookmark relativi all'entity dell'intervento tecnico
            WordHelper.SettaBookMark(document, "NumeroIntervento", entityIntervento.Numero);
            WordHelper.SettaBookMark(document, "CommessaNumero", entityIntervento.NumeroCommessa);

            string tipologiaIntervento = string.Empty;
            if (entityIntervento.IdTipologia.HasValue)
            {
                tipologiaIntervento = entityIntervento.TipologiaIntervento.Nome;
            }
            WordHelper.SettaBookMark(document, "TipologiaIntervento", tipologiaIntervento);

            WordHelper.SettaBookMarkConValoreDateTimeNullabile(document, "DataRichiestaIntervento", entityIntervento.DataRedazione);
            WordHelper.SettaBookMarkConValoreDateTimeNullabile(document, "DataEsecuzioneIntervento", entityIntervento.DataPrevistaIntervento);

            string ragioneSociale = entityIntervento.RagioneSociale.Trim();
            if (!string.IsNullOrWhiteSpace(entityIntervento.DestinazioneMerce)) ragioneSociale += " c/o: " + entityIntervento.DestinazioneMerce.Trim();
            WordHelper.SettaBookMark(document, "Cliente", ragioneSociale);

            WordHelper.SettaBookMark(document, "LuogoIntervento", entityIntervento.IndirizzoCompleto);
            WordHelper.SettaBookMark(document, "Telefono", entityIntervento.Telefono);
            WordHelper.SettaBookMark(document, "OggettoIntervento", entityIntervento.Oggetto);
            WordHelper.SettaBookMark(document, "Referente", entityIntervento.ReferenteChiamata);
            WordHelper.SettaBookMark(document, "DescrizioneIntervento", entityIntervento.Definizione);

            Logic.Intervento_Operatori llIntervento_Operatori = new Intervento_Operatori(this);

            // Viene recuperato l'elenco degli interventi effettuati daglio operatori
            IQueryable<Entities.Intervento_Operatore> interventoOperatori = llIntervento_Operatori.ReadValidiPerReportIntervento(entityIntervento.Identifier);

            // Nel caso in cui esistano degli interventi ..
            if (interventoOperatori != null && interventoOperatori.Count() > 0)
            {
                // Venono inizializzate le variabili necessarie al popolamento dei bookmark relativi agli operatori
                DateTime dataInizioInterventoMinima = interventoOperatori.Where(x => x.InizioIntervento.HasValue).Min(x => x.InizioIntervento.Value);
                DateTime dataInizioInterventoMassima = interventoOperatori.Where(x => x.FineIntervento.HasValue).Max(x => x.FineIntervento.Value);
                IEnumerable<string> elencoCognomeNomeOperatore = interventoOperatori.Select(x => x.Operatore.CognomeNome);
                int totaleInMinuti = interventoOperatori.Where(x => x.TotaleMinuti.HasValue).Sum(x => x.TotaleMinuti.Value);

                TimeSpan totaleInMinutiTimeSpan = new TimeSpan(0, totaleInMinuti, 0);

                // Successivamente all'inizializzazione delle variabili, vengono popolati i bookmark relativi agli operatori
                WordHelper.SettaBookMarkConValoreDateTimeNullabile(document, "DataOraInizio", dataInizioInterventoMinima, "{0:dd/MM/yyyy HH:mm}");
                WordHelper.SettaBookMarkConValoreDateTimeNullabile(document, "DataOraFine", dataInizioInterventoMassima, "{0:dd/MM/yyyy HH:mm}");

                int oreTotali = (int)totaleInMinutiTimeSpan.TotalHours;

                WordHelper.SettaBookMark(document, "TotaleOre", String.Format("Ore: {0} - Minuti: {1}", oreTotali, totaleInMinutiTimeSpan.Minutes));

                // Nel caso in cui l'elenco dei cognomi e nomi degli operatori dell'intervento non sia vuoto e contenga almeno un elemento, allora viene settato il contenuto del bookmark relativo agli operatori che hanno effettuato l'intervento
                if (elencoCognomeNomeOperatore != null && elencoCognomeNomeOperatore.Count() > 0)
                {
                    WordHelper.SettaBookMark(document, "EseguitoDa", String.Join(", ", elencoCognomeNomeOperatore));
                }
            }

            // Viene popolata la sezione relativa ai tecnici
            PopolaSezioneInterventiOperatori(document, interventoOperatori);

            // Viene popolata la tabella che dovrebbe contenere l'elenco dei tecnici che devono firmare il documento di intervento
            PopolaTabellaEseguitoDaOperatoriIntervento(document, interventoOperatori);
        }

        /// <summary>
        /// Effettua il popolamento della sezione relativa ai tecnici che hanno effettuato l'intervento 
        /// </summary>
        /// <param name="document"></param>
        /// <param name="interventoOperatori"></param>
        private void PopolaSezioneInterventiOperatori(msWord.Document document, IQueryable<Entities.Intervento_Operatore> interventoOperatori)
        {
            // Viene inizializzata la collection che contiene gli errori 
            SeCoGes.Utilities.MessagesCollector errori = new SeCoGes.Utilities.MessagesCollector();

            // Vengono controllati i parametri passati alla funzione

            if (document == null)
            {
                errori.Add("Parametro nullo: 'document'");
            }

            if (interventoOperatori == null)
            {
                errori.Add("Parametro nullo: 'interventoOperatori'");
            }

            // Nel caso in cui ci siano degli errori, viene generato un errore ..
            if (errori.HaveMessages) throw new Exception(errori.ToString(Environment.NewLine));

            // Viene messo in memoria l'elenco degli operatori
            List<Entities.Intervento_Operatore> interventoOperatoriList = interventoOperatori.ToList();

            // Viene recuperato il numero degli operatori presenti in memoria
            int numeroOperatori = interventoOperatoriList.Count;

            if (numeroOperatori > 0)
            {
                // Viene popolata la sezione relativa ai tecnici
                PopolaTabellaInterventiOperatori(document, interventoOperatoriList);
            }
        }

        /// <summary>
        /// Effettua il popolamento della sezione relativa ai tecnici che hanno effettuato l'intervento tecnico
        /// </summary>
        /// <param name="document"></param>
        /// <param name="interventoOperatoriList"></param>
        private void PopolaTabellaInterventiOperatori(msWord.Document document, List<Entities.Intervento_Operatore> interventoOperatoriList)
        {
            // Viene inizializzata la collection che contiene gli errori 
            SeCoGes.Utilities.MessagesCollector errori = new SeCoGes.Utilities.MessagesCollector();

            // Vengono controllati i parametri passati alla funzione

            if (document == null)
            {
                errori.Add("Parametro nullo: 'document'");
            }

            if (interventoOperatoriList == null)
            {
                errori.Add("Parametro nullo: 'interventoOperatori'");
            }

            // Nel caso in cui ci siano degli errori, viene generato un errore ..
            if (errori.HaveMessages) throw new Exception(errori.ToString(Environment.NewLine));

            // Nel caso in cui l'elenco degli interventi svolti dagli operatori sia vuoto, viene bloccata l'esecuzione di popolamento della tabella
            if (interventoOperatoriList.Count <= 0) return;

            msWord.Table tableElencoOperatori = GetTabellaInterventiOperatori(document);

            // Nel caso in cui l'oggetto sia nulla ..
            if (tableElencoOperatori == null)
            {
                return;
                // Viene scatenato un errore ..
                //throw new Exception("Non è stato possibile recuperare il bookmark relativo alla tabella che dovrebbe contenere l'elenco dei tecnici");
            }
            else
            {
                // Dalla tabella degli operatori viene recuperata la seconda riga perchè la prima contiene le intestazioni
                msWord.Row rowOperatore = tableElencoOperatori.Rows[2];

                // Viene recuperato il numero degli operatori
                int numeroOperatori = interventoOperatoriList.Count;

                // Per ogni operatore presente nell'elenco ..
                for (int i = 0; i < numeroOperatori; i++)
                {
                    // Nel caso in cui la variabile non sia zero (0)
                    if (i != 0)
                    {
                        // Viene aggiunga una nuova riga ..
                        rowOperatore = tableElencoOperatori.Rows.Add();
                    }

                    // Viene recuperato l'intervento dell'operatore dalla lista mediante la variabile del ciclo
                    Entities.Intervento_Operatore interventoOperartore = interventoOperatoriList[i];

                    // Vengono settate le celle della riga ..
                    msWord.Cell cellaCognomeNomeOperatore = rowOperatore.Cells[1];
                    cellaCognomeNomeOperatore.Range.Text = interventoOperartore.CognomeNomeOperatore;

                    msWord.Cell cellaRisoluzioneIntervento = rowOperatore.Cells[2];
                    cellaRisoluzioneIntervento.Range.Text = interventoOperartore.DescrizioneModalitaRisoluzioneIntervento;

                    msWord.Cell cellaTotaleMinuti = rowOperatore.Cells[3];
                    cellaTotaleMinuti.Range.Text = interventoOperartore.TotaleMinuti.ToString();

                    msWord.Cell cellaInizioIntervento = rowOperatore.Cells[4];
                    cellaInizioIntervento.Range.Text = (interventoOperartore.InizioIntervento.HasValue ? interventoOperartore.InizioIntervento.Value.ToString("dd/MM/yyyy HH:mm") : String.Empty);

                    msWord.Cell cellaFineIntervento = rowOperatore.Cells[5];
                    cellaFineIntervento.Range.Text = (interventoOperartore.FineIntervento.HasValue ? interventoOperartore.FineIntervento.Value.ToString("dd/MM/yyyy HH:mm") : String.Empty);
                }
            }
        }

        /// <summary>
        /// Effettua il popolamento della tabella che dovrebbe contenere l'elenco dei tecnici che devono firmare il documento di intervento
        /// </summary>
        /// <param name="document"></param>
        /// <param name="interventoOperatoriList"></param>
        private void PopolaTabellaEseguitoDaOperatoriIntervento(msWord.Document document, IQueryable<Entities.Intervento_Operatore> elencoInterventoOperatori)
        {
            // Viene inizializzata la collection che contiene gli errori 
            SeCoGes.Utilities.MessagesCollector errori = new SeCoGes.Utilities.MessagesCollector();

            // Vengono controllati i parametri passati alla funzione

            if (document == null)
            {
                errori.Add("Parametro nullo: 'document'");
            }

            if (elencoInterventoOperatori == null)
            {
                errori.Add("Parametro nullo: 'elencoInterventoOperatori'");
            }

            // Nel caso in cui ci siano degli errori, viene generato un errore ..
            if (errori.HaveMessages) throw new Exception(errori.ToString(Environment.NewLine));

            // Viene recuperato l'elenco degli operatori che hanno effettuato gli interventi, sotto forma di lista
            List<Entities.Intervento_Operatore> interventoOperatoriList = elencoInterventoOperatori.ToList();

            // Viene recuperata la tabella degli operatori che hanno effettuato gli interventi
            msWord.Table tabellaEseguitoDaOperatoriIntervento = GetTabellaEseguitoDaOperatoriIntervento(document);

            // Nel caso in cui l'oggetto sia nulla ..
            if (tabellaEseguitoDaOperatoriIntervento == null)
            {
                // Viene scatenato un errore ..
                throw new Exception("Non è stato possibile recuperare il bookmark relativo alla tabella che dovrebbe contenere l'elenco dei tecnici che devono firmare il documento di intervento");
            }
            else
            {
                // Nel caso in cui la lista degli operatori non contenga nemmeno un operatore, 
                // la tabella che dovrebbe contenere l'elenco dei tecnici che devono firmare il documento di intervento, non ha più senso, quindi ..
                if (interventoOperatoriList.Count <= 0)
                {
                    // Viene eliminata la tabella 
                    tabellaEseguitoDaOperatoriIntervento.Delete();
                }
                else
                {
                    // Viene recuperato l'elenco dei cognomi e nomi degli operatori ..
                    IEnumerable<string> elencoCognomeNomeOperatori = interventoOperatoriList.OrderBy(x => x.CognomeNomeOperatore)
                                                                                            .Select(x => x.CognomeNomeOperatore);

                    // Viene recuperata la lista dei cognomi e nomi degli operatori in modo univoco
                    List<string> elencoOperatori = elencoCognomeNomeOperatori.Distinct().ToList();

                    // Dalla tabella degli operatori viene recuperata la prima riga perchè è l'unica presente nella tabella ed è vuota
                    msWord.Row rowOperatore = tabellaEseguitoDaOperatoriIntervento.Rows[1];

                    // Viene recuperato il numero degli operatori
                    int numeroOperatori = elencoOperatori.Count;

                    // Per ogni operatore presente nell'elenco ..
                    for (int i = 0; i < numeroOperatori; i++)
                    {
                        // Nel caso in cui la variabile non sia zero (0)
                        if (i != 0)
                        {
                            // Viene aggiunga una nuova riga ..
                            rowOperatore = tabellaEseguitoDaOperatoriIntervento.Rows.Add();
                        }

                        // Viene settata la cella in cui dev'essere inserito il cognome e nome dell'operatore della riga ..
                        msWord.Cell cellaCognomeNomeOperatore = rowOperatore.Cells[1];
                        cellaCognomeNomeOperatore.Range.Text = elencoOperatori[i];
                    }
                }
            }
        }

        #endregion

        #region Recupero di valori

        /// <summary>
        /// Restituisce la tabella relativa agli interventi degli operatori. Prima della restituzione, viene effettuato il controllo sulle righe e colonne di cui è caratterizzata quest'ultima
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private msWord.Table GetTabellaInterventiOperatori(msWord.Document document)
        {
            // Nel caso in cui il documento non sia valorizzato, viene generato un errore
            if (document == null) throw new ArgumentNullException("document", "Parametro nullo");

            // Viene recuperato l'oggetto che contiene la tabella degli operatori
            msWord.Range rangeTabellaOperatori = WordHelper.GetBookmarkRange(document, "TabellaInterventiOperatori");

            // Nel caso in cui l'oggetto sia nulla ..
            if (rangeTabellaOperatori == null)
            {
                return null;
                // Viene scatenato un errore ..
                //throw new Exception("Non è stato possibile recuperare il bookmark relativo alla tabella che dovrebbe contenere l'elenco dei tecnici");
            }
            // Nel caso in cui l'oggetto non contenga almeno una tabella ..
            else if (rangeTabellaOperatori.Tables == null || rangeTabellaOperatori.Tables.Count <= 0)
            {
                // Viene scatenato un errore ..
                throw new Exception("Il bookmark relativo alla tabella che dovrebbe contenere l'elenco dei tecnici, non contiene nessuna tabella");
            }
            else
            {
                // Viene recperata la tabella degli operatori dall'oggetto recuperato il precedenza
                msWord.Table tableElencoOperatori = rangeTabellaOperatori.Tables[1];

                // Nel caso in cui la tabella non contenga almeno 2 righe ..
                if (tableElencoOperatori.Rows == null || tableElencoOperatori.Rows.Count < 2)
                {
                    // Viene scatenato un errore 
                    throw new Exception("La tabella che dovrebbe contenere l'elenco dei tecnici, non possiede il numero di righe necessarie per effettuare l'operazione di inserimento di quest'ultimi");
                }
                // Nel caso in cui la tabella non contenga almeno 5 colonne ..
                else if (tableElencoOperatori.Columns == null || tableElencoOperatori.Columns.Count < 5)
                {
                    // Viene scatenato un errore
                    throw new Exception("La tabella che dovrebbe contenere l'elenco dei tecnici, non possiede il numero di colonne necessarie per effettuare l'operazione di inserimento di quest'ultimi");
                }
                else
                {
                    return tableElencoOperatori;
                }
            }
        }

        /// <summary>
        /// Restituisce la tabella relativa all'elenco dei tecnici che devono firmare il documento di intervento. Prima della restituzione, viene effettuato il controllo sulle righe e colonne di cui è caratterizzata quest'ultima
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private msWord.Table GetTabellaEseguitoDaOperatoriIntervento(msWord.Document document)
        {
            // Nel caso in cui il documento non sia valorizzato, viene generato un errore
            if (document == null) throw new ArgumentNullException("document", "Parametro nullo");

            // Viene recuperato l'oggetto che contiene la tabella degli operatori
            msWord.Range rangeTabellaOperatori = WordHelper.GetBookmarkRange(document, "TabellaEseguitoDaOperatoriIntervento");

            // Nel caso in cui l'oggetto sia nulla ..
            if (rangeTabellaOperatori == null)
            {
                // Viene scatenato un errore ..
                throw new Exception("Non è stato possibile recuperare il bookmark relativo alla tabella che dovrebbe contenere l'elenco dei tecnici che devono firmare il documento di intervento");
            }
            // Nel caso in cui l'oggetto non contenga almeno una tabella ..
            else if (rangeTabellaOperatori.Tables == null || rangeTabellaOperatori.Tables.Count <= 0)
            {
                // Viene scatenato un errore ..
                throw new Exception("Il bookmark relativo alla tabella che dovrebbe contenere l'elenco dei tecnici che devono firmare il documento di intervento, non contiene nessuna tabella");
            }
            else
            {
                // Viene recperata la tabella degli operatori dall'oggetto recuperato il precedenza
                msWord.Table tableElencoOperatori = rangeTabellaOperatori.Tables[1];

                // Nel caso in cui la tabella non contenga almeno 2 righe ..
                if (tableElencoOperatori.Rows == null || tableElencoOperatori.Rows.Count < 1)
                {
                    // Viene scatenato un errore 
                    throw new Exception("La tabella che dovrebbe contenere l'elenco dei tecnici che devono firmare il documento di intervento, non possiede il numero di righe necessarie per effettuare l'operazione di inserimento di quest'ultimi");
                }
                else
                {
                    return tableElencoOperatori;
                }
            }
        }

        #endregion

        #endregion
    }
}
