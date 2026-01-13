using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGEST.Entities;

namespace SeCoGEST.Logic
{
    public class Intervento_Operatori : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.Intervento_Operatori dal;
        private Logic.LogInvioNotifiche llLogInvioNotifiche;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public Intervento_Operatori()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public Intervento_Operatori(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public Intervento_Operatori(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.Intervento_Operatori(this.context);
            llLogInvioNotifiche = new LogInvioNotifiche(this);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Intervento_Operatore entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                if (entityToCreate.ID.Equals(Guid.Empty))
                {
                    entityToCreate.ID = Guid.NewGuid();
                }

                // Salvataggio nel database
                dal.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'Intervento_Operatore': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities in base allo stato del'Intervento_Operatore
        /// </summary>
        /// <returns></returns>
        public IQueryable<Intervento_Operatore> Read(Intervento intervento)
        {
            return Read(new EntityId<Entities.Intervento>(intervento.ID));
        }

        /// <summary>
        /// Restituisce tutte le entities in base allo stato del'Intervento_Operatore
        /// </summary>
        /// <returns></returns>
        public IQueryable<Intervento_Operatore> Read(EntityId<Intervento> idIntervento)
        {
            return dal.Read().Where(x => x.IDIntervento == idIntervento.Value).OrderBy(x => x.InizioIntervento);
        }

        /// <summary>
        /// Restituisce tutte le entities che risultano valide per il report dell'intervento (l'operatore non dev'essere una area, dev'essere preso in carico e deve avere una durata in minuti maggiore di zero)
        /// </summary>
        /// <returns></returns>
        public IQueryable<Intervento_Operatore> ReadValidiPerReportIntervento(EntityId<Intervento> idIntervento)
        {
            return Read(idIntervento).Where(x => !x.Operatore.Area && 
                                                 x.DurataMinuti.HasValue &&
                                                 x.DurataMinuti.Value > 0);
        }

        /// <summary>
        /// Restituisce tutte le entities in base agli operatori passati
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Intervento_Operatore> Read(IEnumerable<Entities.Operatore> elencoOperatori)
        {
            IEnumerable<Guid> elencoIdentificativiOperatore = elencoOperatori.Select(x => x.ID);
            return dal.Read().Where(x => elencoIdentificativiOperatore.Contains(x.IDOperatore)).OrderBy(x => x.InizioIntervento);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.Intervento_Operatore Find(EntityId<Intervento_Operatore> idToFind)
        {
            return dal.Find(idToFind.Value);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Intervento_Operatore entityToDelete, bool submitChanges)
        {
            Delete(entityToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Intervento_Operatore entityToDelete, bool checkAllegati, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'Intervento_Operatore': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.Intervento_Operatore> entitiesToDelete, bool submitChanges)
        {
            Delete(entitiesToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.Intervento_Operatore> entitiesToDelete, bool checkAllegati, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'Intervento_Operatore': parametro nullo!");
            }
        }

        #endregion

        #region Funzioni Specifiche

        ///// <summary>
        ///// Restituisce l'elenco degli interventi non chiusi e non validati raggruppati per operatore
        ///// </summary>
        ///// <param name="alertMancataChiusuraIntervento">alertMancataChiusuraIntervento</param>
        ///// <returns></returns>
        //public List<IGrouping<Entities.Operatore, Guid>> GetInterventiNonChiusiNonValidatiRaggruppatiPerOperatore()
        //{
        //    List<Entities.Intervento_Operatore> elencoInterventiOperatoriFiltrata =
        //        dal.Read().Where(x => x.Intervento != null &&
        //                                x.Intervento.Stato.HasValue &&
        //                                x.Intervento.Stato.Value != (byte)StatoInterventoEnum.Chiuso &&
        //                                x.Intervento.Stato.Value != (byte)StatoInterventoEnum.Validato &&
        //                                (!x.Intervento.Interno.HasValue || !x.Intervento.Interno.Value) &&
        //                                x.PresaInCarico.HasValue &&
        //                                x.PresaInCarico.Value).ToList();

        //    List<Entities.Intervento_Operatore> elencoInterventiOperatoriDaNotificare = new List<Intervento_Operatore>();
        //    if (elencoInterventiOperatoriFiltrata != null && elencoInterventiOperatoriFiltrata.Count > 0)
        //    {
        //        foreach (Entities.Intervento_Operatore interventoOperatore in elencoInterventiOperatoriFiltrata)
        //        {
        //            if (interventoOperatore.Intervento != null)
        //            {
        //                DateTime? ultimoInvio = llLogInvioNotifiche.GetDataUltimoInvioNotifica<Entities.Intervento>(interventoOperatore.Intervento.Identifier, InfoOperazioneTabellaEnum.Intervento, TipologiaNotificaEnum.InterventoPresoInCaricoNonChiuso);
        //                if (!ultimoInvio.HasValue)
        //                {
        //                    if (interventoOperatore.DataPresaInCarico.HasValue)
        //                    {
        //                        ultimoInvio = interventoOperatore.DataPresaInCarico.Value;
        //                    }
        //                    else
        //                    {
        //                        ultimoInvio = DateTime.Now;
        //                    }
        //                }

        //                DateTime dataAttuale = DateTime.Now;
        //                TimeSpan periodoTrascorso = dataAttuale.Subtract(ultimoInvio.Value);
        //                TimeSpan periodoNonLavorativo = llLogInvioNotifiche.GetTotalePeriodoNonLavorativo(dataAttuale, ultimoInvio.Value);
        //                TimeSpan periodoLavorativo = periodoTrascorso.Subtract(periodoNonLavorativo);

        //                if (interventoOperatore.Intervento.AnagraficaClienti == null || periodoLavorativo.TotalMinutes > interventoOperatore.Intervento.AnagraficaClienti.MINUTIMAXCHIUSURA)
        //                {
        //                    elencoInterventiOperatoriDaNotificare.Add(interventoOperatore);
        //                }
        //            }
        //        }
        //    }

        //    List<IGrouping<Entities.Operatore, Guid>> raggruppamentoInterventiPerOperatoreDaRestituire = 
        //        elencoInterventiOperatoriDaNotificare.OrderBy(x => x.Intervento.Numero)
        //                                             .GroupBy(key => key.Operatore, informazioneUnivocaDaRecuperare => informazioneUnivocaDaRecuperare.Intervento.ID)
        //                                             .ToList();
        //    //List<IGrouping<Entities.Operatore, Guid>> raggruppamentoInterventiPerOperatore =
        //    //    dal.Read().Where(x => x.Intervento != null &&
        //    //                            x.Intervento.Stato.HasValue &&
        //    //                            x.Intervento.Stato.Value != (byte)StatoInterventoEnum.Chiuso &&
        //    //                            x.Intervento.Stato.Value != (byte)StatoInterventoEnum.Validato &&
        //    //                            x.Intervento.Interno.HasValue &&
        //    //                            x.Intervento.Interno.Value &&
        //    //                            x.PresaInCarico.HasValue &&
        //    //                            x.PresaInCarico.Value &&
        //    //                            x.DataPresaInCarico.HasValue &&
        //    //                            x.DataPresaInCarico <= dataDaVerificare)
        //    //              .OrderBy(x => x.Intervento.Numero)
        //    //              .GroupBy(key => key.Operatore, informazioneUnivocaDaRecuperare => informazioneUnivocaDaRecuperare.Intervento.ID)
        //    //              .ToList();

        //    return raggruppamentoInterventiPerOperatoreDaRestituire;
        //}

        ///// <summary>
        ///// Restituisce l'elenco degli interventi non chiusi e non validati e non presi in carico raggruppati per operatore
        ///// </summary>
        ///// <param name="alertMancataChiusuraIntervento"></param>
        ///// <returns></returns>
        //public List<IGrouping<Entities.Operatore, Guid>> GetInterventiNonChiusiNonValidatiNonPresiInCaricoRaggruppatiPerOperatore()
        //{
        //    List<Entities.Intervento_Operatore> elencoInterventiOperatoriFiltrata =
        //        dal.Read().Where(x => x.Intervento != null &&
        //                                x.Intervento.Stato.HasValue &&
        //                                x.Intervento.Stato.Value != (byte)StatoInterventoEnum.Chiuso &&
        //                                x.Intervento.Stato.Value != (byte)StatoInterventoEnum.Validato &&
        //                                (!x.Intervento.Interno.HasValue || !x.Intervento.Interno.Value) &&
        //                                (!x.PresaInCarico.HasValue || !x.PresaInCarico.Value) &&
        //                                !x.DataPresaInCarico.HasValue).ToList();


        //    List<Entities.Intervento_Operatore> elencoInterventiOperatoriDaNotificare = new List<Intervento_Operatore>();
        //    if (elencoInterventiOperatoriFiltrata != null && elencoInterventiOperatoriFiltrata.Count > 0)
        //    {
        //        foreach (Entities.Intervento_Operatore interventoOperatore in elencoInterventiOperatoriFiltrata)
        //        {
        //            if (interventoOperatore.Intervento != null)
        //            {
        //                DateTime? ultimoInvio = llLogInvioNotifiche.GetDataUltimoInvioNotifica<Entities.Intervento>(interventoOperatore.Intervento.Identifier, InfoOperazioneTabellaEnum.Intervento, TipologiaNotificaEnum.InterventoApertoNonPresoInCarico);
        //                if (!ultimoInvio.HasValue)
        //                {
        //                    ultimoInvio = interventoOperatore.Intervento.DataRedazione;
        //                }

        //                DateTime dataAttuale = DateTime.Now;
        //                TimeSpan periodoTrascorso = dataAttuale.Subtract(ultimoInvio.Value);
        //                TimeSpan periodoNonLavorativo = llLogInvioNotifiche.GetTotalePeriodoNonLavorativo(dataAttuale, ultimoInvio.Value);
        //                TimeSpan periodoLavorativo = periodoTrascorso.Subtract(periodoNonLavorativo);

        //                if (interventoOperatore.Intervento.AnagraficaClienti == null || periodoLavorativo.TotalMinutes > interventoOperatore.Intervento.AnagraficaClienti.MINUTIMAXPRESAINCARICO)
        //                {
        //                    elencoInterventiOperatoriDaNotificare.Add(interventoOperatore);
        //                }
        //            }
        //        }
        //    }

        //    List<IGrouping<Entities.Operatore, Guid>> raggruppamentoInterventiPerOperatoreDaRestituire = 
        //        elencoInterventiOperatoriDaNotificare.OrderBy(x => x.Intervento.Numero)
        //                                             .GroupBy(key => key.Operatore, informazioneUnivocaDaRecuperare => informazioneUnivocaDaRecuperare.Intervento.ID)
        //                                             .ToList();
        //    //List<IGrouping<Entities.Operatore, Guid>> raggruppamentoInterventiPerOperatore =
        //    //    dal.Read().Where(x => x.Intervento != null &&
        //    //                            x.Intervento.Stato.HasValue &&
        //    //                            x.Intervento.Stato.Value != (byte)StatoInterventoEnum.Chiuso &&
        //    //                            x.Intervento.Stato.Value != (byte)StatoInterventoEnum.Validato &&
        //    //                            //x.Intervento.DataRedazione <= dataDaVerificare &&
        //    //                            x.Intervento.Interno.HasValue &&
        //    //                            x.Intervento.Interno.Value &&
        //    //                            (!x.PresaInCarico.HasValue || !x.PresaInCarico.Value) &&
        //    //                            !x.DataPresaInCarico.HasValue)
        //    //              .OrderBy(x => x.Intervento.Numero)
        //    //              .GroupBy(key => key.Operatore, informazioneUnivocaDaRecuperare => informazioneUnivocaDaRecuperare.Intervento.ID)
        //    //              .ToList();

        //    return raggruppamentoInterventiPerOperatoreDaRestituire;
        //}

        /// <summary>
        /// Restituisce l'elenco degli interventi non chiusi e non validati raggruppati per operatore
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Entities.Intervento_Operatore> GetInterventiNonChiusiNonValidati()
        {
            List<Entities.Intervento_Operatore> elencoInterventiOperatoriFiltrata =
                dal.Read().Where(x => x.Intervento != null &&
                                        x.Intervento.Stato.HasValue &&
                                        x.Intervento.Stato.Value != (byte)StatoInterventoEnum.Chiuso &&
                                        x.Intervento.Stato.Value != (byte)StatoInterventoEnum.Validato &&
                                        (!x.Intervento.Interno.HasValue || !x.Intervento.Interno.Value) &&
                                        x.PresaInCarico.HasValue &&
                                        x.PresaInCarico.Value).ToList();

            List<Entities.Intervento_Operatore> elencoInterventiOperatoriDaNotificare = new List<Intervento_Operatore>();
            if (elencoInterventiOperatoriFiltrata != null && elencoInterventiOperatoriFiltrata.Count > 0)
            {
                foreach (Entities.Intervento_Operatore interventoOperatore in elencoInterventiOperatoriFiltrata)
                {
                    if (interventoOperatore.Intervento != null)
                    {
                        DateTime? ultimoInvio = llLogInvioNotifiche.GetDataUltimoInvioNotifica<Entities.Intervento>(interventoOperatore.Intervento.Identifier, InfoOperazioneTabellaEnum.Intervento, TipologiaNotificaEnum.InterventoPresoInCaricoNonChiuso);
                        if (!ultimoInvio.HasValue)
                        {
                            if (interventoOperatore.DataPresaInCarico.HasValue)
                            {
                                ultimoInvio = interventoOperatore.DataPresaInCarico.Value;
                            }
                            else
                            {
                                ultimoInvio = DateTime.Now;
                            }
                        }

                        DateTime dataAttuale = DateTime.Now;
                        TimeSpan periodoTrascorso = dataAttuale.Subtract(ultimoInvio.Value);
                        TimeSpan periodoNonLavorativo = llLogInvioNotifiche.GetTotalePeriodoNonLavorativo(dataAttuale, ultimoInvio.Value);
                        TimeSpan periodoLavorativo = periodoTrascorso.Subtract(periodoNonLavorativo);

                        if (interventoOperatore.Intervento.AnagraficaClienti == null || Math.Abs(periodoLavorativo.TotalMinutes) > Math.Abs(interventoOperatore.Intervento.AnagraficaClienti.MINUTIMAXCHIUSURA))
                        {
                            elencoInterventiOperatoriDaNotificare.Add(interventoOperatore);
                        }
                    }
                }
            }
            
            return elencoInterventiOperatoriDaNotificare.OrderBy(x => x.Intervento.Numero);
        }

        /// <summary>
        /// Restituisce l'elenco degli interventi non chiusi e non validati e non presi in carico raggruppati per operatore
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Intervento_Operatore> GetInterventiNonChiusiNonValidatiNonPresiInCarico()
        {
            List<Entities.Intervento_Operatore> elencoInterventiOperatoriFiltrata =
                dal.Read().Where(x => x.Intervento != null &&
                                        x.Intervento.Stato.HasValue &&
                                        x.Intervento.Stato.Value != (byte)StatoInterventoEnum.Chiuso &&
                                        x.Intervento.Stato.Value != (byte)StatoInterventoEnum.Validato &&
                                        (!x.Intervento.Interno.HasValue || !x.Intervento.Interno.Value) &&
                                        (!x.PresaInCarico.HasValue || !x.PresaInCarico.Value) &&
                                        !x.DataPresaInCarico.HasValue).ToList();

            


            List<Entities.Intervento_Operatore> elencoInterventiOperatoriDaNotificare = new List<Intervento_Operatore>();
            if (elencoInterventiOperatoriFiltrata != null && elencoInterventiOperatoriFiltrata.Count > 0)
            {
                foreach (Entities.Intervento_Operatore interventoOperatore in elencoInterventiOperatoriFiltrata)
                {
                    if (interventoOperatore.Intervento != null)
                    {
                        DateTime? ultimoInvio = llLogInvioNotifiche.GetDataUltimoInvioNotifica<Entities.Intervento>(interventoOperatore.Intervento.Identifier, InfoOperazioneTabellaEnum.Intervento, TipologiaNotificaEnum.InterventoApertoNonPresoInCarico);
                        //if (!ultimoInvio.HasValue)
                        //{
                        //    ultimoInvio = interventoOperatore.Intervento.DataRedazione;
                        //}
                        if (ultimoInvio.HasValue) // La notifica deve essere inviata UNA SOLA VOLTA 
                        {
                            continue;
                        }


                        DateTime dataAttuale = DateTime.Now;
                        TimeSpan periodoTrascorso = dataAttuale.Subtract(ultimoInvio.Value);
                        TimeSpan periodoNonLavorativo = llLogInvioNotifiche.GetTotalePeriodoNonLavorativo(dataAttuale, ultimoInvio.Value);
                        TimeSpan periodoLavorativo = periodoTrascorso.Subtract(periodoNonLavorativo);

                        if (interventoOperatore.Intervento.AnagraficaClienti == null || Math.Abs(periodoLavorativo.TotalMinutes) > Math.Abs(interventoOperatore.Intervento.AnagraficaClienti.MINUTIMAXPRESAINCARICO))
                        {
                            elencoInterventiOperatoriDaNotificare.Add(interventoOperatore);
                        }
                    }
                }
            }

            return elencoInterventiOperatoriDaNotificare.OrderBy(x => x.Intervento.Numero);
        }

        

        /// <summary>
        /// Restituisce il totale della durata degli interventi in minuti effettuando il calcolo sugli interventi recuperati in base all'intervento passato come parametro
        /// </summary>
        /// <param name="intervento"></param>
        /// <returns></returns>
        public int GetTotaleDurataMinuti(Entities.Intervento intervento)
        {
            if (intervento == null) throw new ArgumentNullException("intervento", "Parametro nullo");

            IQueryable<Entities.Intervento_Operatore> interventiOperatore = Read(intervento);

            return (interventiOperatore != null && interventiOperatore.Count() > 0) ? interventiOperatore.Where(x => x.DurataMinuti.HasValue).Sum(x => x.DurataMinuti.Value) : 0;
        }

        /// <summary>
        /// Restituisce la data di inizio minima presente tra i vari interventi recuperati in base all'intervento passato come parametro
        /// </summary>
        /// <param name="intervento"></param>
        /// <returns></returns>
        public DateTime? GetDataInizioInterventoMinima(Entities.Intervento intervento)
        {
            if (intervento == null) throw new ArgumentNullException("intervento", "Parametro nullo");

            IQueryable<Entities.Intervento_Operatore> interventiOperatore = Read(intervento);

            return (interventiOperatore != null && interventiOperatore.Count() > 0) ? interventiOperatore.Min(x => x.InizioIntervento) : (DateTime?)null;
        }

        /// <summary>
        /// Restituisce la data di fine massima presente tra i vari interventi recuperati in base all'intervento passato come parametro
        /// </summary>
        /// <param name="intervento"></param>
        /// <returns></returns>
        public DateTime? GetDataFineInterventoMassima(Entities.Intervento intervento)
        {
            if (intervento == null) throw new ArgumentNullException("intervento", "Parametro nullo");

            IQueryable<Entities.Intervento_Operatore> interventiOperatore = Read(intervento);

            return (interventiOperatore != null && interventiOperatore.Count() > 0) ? interventiOperatore.Max(x => x.FineIntervento) : (DateTime?)null;
        }

        /// <summary>
        /// Restituisce l'elenco caratterizzato dal cognome e nome dell'operatore, recuperati in base all'intervento passato come parametro, in modo univoco
        /// </summary>
        /// <param name="intervento"></param>
        /// <returns></returns>
        public string GetElencoCognomeNomeOperatoriString(Entities.Intervento intervento, string separetor = ", ")
        {
            IEnumerable<string> elencoCognomeNomeOperatori = GetElencoCognomeNomeOperatori(intervento);

            return (elencoCognomeNomeOperatori != null && elencoCognomeNomeOperatori.Count() > 0) ? String.Join(separetor, elencoCognomeNomeOperatori) : String.Empty;
        }

        /// <summary>
        /// Restituisce l'elenco caratterizzato dal cognome e nome dell'operatore, recuperati in base all'intervento passato come parametro, in modo univoco
        /// </summary>
        /// <param name="intervento"></param>
        /// <returns></returns>
        public IQueryable<string> GetElencoCognomeNomeOperatori(Entities.Intervento intervento)
        {
            return GetElencoOperatori(intervento).Select(x => x.CognomeNome).Distinct();
        }

        /// <summary>
        /// Restituisce l'elenco delle entity relative agli operatori collegati agli interventi recuperati in base all'intervento passato come parametro  
        /// </summary>
        /// <param name="intervento"></param>
        /// <returns></returns>
        public IQueryable<Entities.Operatore> GetElencoOperatori(Entities.Intervento intervento)
        {
            if (intervento == null) throw new ArgumentNullException("intervento", "Parametro nullo");

            return Read(intervento).Select(x => x.Operatore);
        }
        
        #endregion
    }
}
