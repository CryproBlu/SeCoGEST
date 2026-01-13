using SeCoGEST.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Logic
{
    public class RepartiUfficio : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.RepartiUfficio dalRepartiUfficio;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public RepartiUfficio()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public RepartiUfficio(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public RepartiUfficio(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dalRepartiUfficio = new Data.RepartiUfficio(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.RepartoUfficio entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                // Salvataggio nel database
                dalRepartiUfficio.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'RepartoUfficio': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.RepartoUfficio> Read()
        {
            return from u in dalRepartiUfficio.Read() orderby u.Reparto select u;
        }


        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.RepartoUfficio Find(Guid idToFind)
        {
            return dalRepartiUfficio.Find(idToFind);
        }

        public Entities.RepartoUfficio Find(EntityId<RepartoUfficio> idToFind)
        {
            return dalRepartiUfficio.Find(idToFind.Value);
        }


        /// <summary>
        /// Restituisce l'entity relativa al nome passato e null se non trova l'entity
        /// </summary>
        /// <param name="nomeRepartoUfficioToFind"></param>
        /// <returns></returns>
        public Entities.RepartoUfficio Find(EntityString<RepartoUfficio> nomeRepartoUfficioToFind)
        {
            return dalRepartiUfficio.Find(nomeRepartoUfficioToFind.Value);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.RepartoUfficio entityToDelete, bool submitChanges)
        {
            Delete(entityToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.RepartoUfficio entityToDelete, bool checkAllegati, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dalRepartiUfficio.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'RepartoUfficio': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.RepartoUfficio> entitiesToDelete, bool submitChanges)
        {
            Delete(entitiesToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.RepartoUfficio> entitiesToDelete, bool checkAllegati, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'RepartoUfficio': parametro nullo!");
            }
        }

        #endregion


        public DateTime GetDataLimiteIntervento(Entities.RepartoUfficio repartoDiRiferimento, TimeSpan possibileAttesa, Entities.CaratteristicaInterventoEnum caratteristicaInterventoCorrente, Entities.Intervento intervento)
        {
            DateTime? dataOraDiRiferimento = null;
            if (caratteristicaInterventoCorrente == CaratteristicaInterventoEnum.RipristinoEntroMinutiDaPresaInCarico)
            {
                dataOraDiRiferimento = intervento.DataPresaInCarico;
            }
            else
            {
                dataOraDiRiferimento = intervento.DataRedazione;
            }

            return GetDataLimiteIntervento(repartoDiRiferimento, possibileAttesa, dataOraDiRiferimento);
        }

        public DateTime GetDataLimiteIntervento(Entities.RepartoUfficio repartoDiRiferimento, TimeSpan possibileAttesa, DateTime? dataOraDiRiferimento)
        {
            if (!dataOraDiRiferimento.HasValue) dataOraDiRiferimento = DateTime.Now;
            DateTime dataOraDiRiferimentoIniziale = dataOraDiRiferimento.Value;
            DateTime dataLimiteIntervento = dataOraDiRiferimento.Value;
            //TimeSpan tempoAtteso = new TimeSpan(0);
            bool primoTurnoDisponibile = true;
            //bool found = false;

            if (repartoDiRiferimento.OrarioRepartoUfficios.Count() == 0)
            {
                return dataLimiteIntervento;
            }

            // Crea l'elenco degli orari di lavoro del reparto ordinato a livello temporale partendo dalla data/ora di riferimento
            List<Entities.OrarioRepartoUfficio> disponibilitàProssimiGiorni = repartoDiRiferimento.OrarioRepartoUfficios.Where(o =>
                            (o.Giorno == (int)dataOraDiRiferimentoIniziale.DayOfWeek && o.OrarioAlle > dataOraDiRiferimentoIniziale.TimeOfDay)
                         || (o.Giorno > (int)dataOraDiRiferimentoIniziale.DayOfWeek))
                            .OrderBy(o => o.Giorno).ThenBy(o => o.OrarioDalle).ToList();

            List<Entities.OrarioRepartoUfficio> disponibilitàGiorni = repartoDiRiferimento.OrarioRepartoUfficios.Where(o =>
                            (o.Giorno < (int)dataOraDiRiferimentoIniziale.DayOfWeek)
                         || (o.Giorno == (int)dataOraDiRiferimentoIniziale.DayOfWeek && o.OrarioAlle <= dataOraDiRiferimentoIniziale.TimeOfDay))
                            .OrderBy(o => o.Giorno).ThenBy(o => o.OrarioDalle).ToList();

            disponibilitàProssimiGiorni.AddRange(disponibilitàGiorni);


            // Recupera l'elenco dei Periodi di Festività da escludere nella ricerca del tempo disponibile
            PeriodiFestivita llPF = new PeriodiFestivita(this);
            List<Entities.PeriodoFestivita> festività = llPF.Read().ToList();

            // Cicla continuamente i turni di lavoro fino a quando tutto il tempo disponibile di attesa viene consumato
            do
            {
                foreach (Entities.OrarioRepartoUfficio orario in disponibilitàProssimiGiorni)
                {
                    // Se il giorno è indicato nei periodi di festività allora incrementa di 1 e passa al giorno successivo
                    bool giornoFestività = true;
                    while (giornoFestività)
                    {
                        giornoFestività = llPF.IsGiornoDiFestivita(dataOraDiRiferimento.Value, festività);
                        if (giornoFestività) dataOraDiRiferimento = dataOraDiRiferimento.Value.Date.AddDays(1);
                    }

                    while (((int)dataOraDiRiferimento.Value.Date.DayOfWeek) != orario.Giorno)
                    {
                        // Se il giorno non è lo stesso del turno corrente allora incrementa di 1 e passa al giorno successivo
                        dataOraDiRiferimento = dataOraDiRiferimento.Value.Date.AddDays(1);
                        primoTurnoDisponibile = false;

                        // Se il giorno è indicato nei periodi di festività allora incrementa di 1 e passa al giorno successivo
                        giornoFestività = true;
                        while (giornoFestività)
                        {
                            giornoFestività = llPF.IsGiornoDiFestivita(dataOraDiRiferimento.Value, festività);
                            if (giornoFestività) dataOraDiRiferimento = dataOraDiRiferimento.Value.Date.AddDays(1);
                        }
                    }

                    // Non dovrebbe essere necessario ma controlla che il giorno del turno sia lo stesso
                    if (((int)dataOraDiRiferimento.Value.Date.DayOfWeek) == orario.Giorno)
                    {

                        // Definisco gli orari di inizio e fine della giornata lavorativa
                        DateTime inizioOrarioLavorativoDelTurno = dataOraDiRiferimento.Value.Date.Add(orario.OrarioDalle);
                        DateTime fineOrarioLavorativoDelTurno = dataOraDiRiferimento.Value.Date.Add(orario.OrarioAlle);





                        // Se al momento della richiesta di intervento il reparto è in turno e l'orario di riferimento è nell'orario lavorativo...
                        if (primoTurnoDisponibile && dataOraDiRiferimento.Value >= inizioOrarioLavorativoDelTurno && dataOraDiRiferimento.Value <= fineOrarioLavorativoDelTurno)
                        {
                            // Calcola la quantità di tempo che può essere attesa nel turno corrente prima di doversi dedicare all'intervento
                            TimeSpan tempoAttesoNelTurno = fineOrarioLavorativoDelTurno.Subtract(dataOraDiRiferimento.Value);

                            // Se il tempo che può essere atteso è superiore al tempo disponibile nel turno
                            // allora vuol dire che l'intervento deve essere eseguito in questo turno...
                            if (tempoAttesoNelTurno >= possibileAttesa)
                            {
                                // In questo caso calcola la data limite per l'intervento e la restituisce al chiamante
                                //dataLimiteIntervento = inizioOrarioLavorativoDelTurno.Add(tempoAttesoNelTurno);
                                dataLimiteIntervento = dataOraDiRiferimento.Value.Add(possibileAttesa);
                                possibileAttesa = new TimeSpan(0);
                                //found = true;
                                break;
                            }

                            // ...altrimenti calcola il tempo atteso in questo turno e passa al prossimo
                            else
                            {
                                //tempoAtteso = tempoAtteso.Add(tempoAttesoNelTurno);
                                possibileAttesa = possibileAttesa.Subtract(tempoAttesoNelTurno);
                            }
                        }


                        // ...invece se al momento della richiesta di intervento il reparto NON E' ANCORA in turno ma lo sarà più tardi nella stessa giornata...
                        else if (!primoTurnoDisponibile || (primoTurnoDisponibile && fineOrarioLavorativoDelTurno > dataOraDiRiferimento.Value))
                        // else //if (!primoTurnoDisponibile)
                        {
                            // Calcola la durata del turno
                            TimeSpan durataDelTurno = fineOrarioLavorativoDelTurno.Subtract(inizioOrarioLavorativoDelTurno);

                            // Se la durata del turno è superiore al tempo di attesa disponibile
                            // allora vuol dire che l'intervento deve essere eseguito in questo turno.
                            if (durataDelTurno >= possibileAttesa)
                            {
                                // In questo caso calcola la data limite per l'intervento e la restituisce al chiamante
                                //dataLimiteIntervento = inizioOrarioLavorativoDelTurno.Add(tempoAttesoNelTurno);
                                dataLimiteIntervento = inizioOrarioLavorativoDelTurno.Add(possibileAttesa);
                                possibileAttesa = new TimeSpan(0);
                                //found = true;
                                break;
                            }

                            // ...altrimenti calcola il tempo atteso in questo turno e passa al prossimo
                            else
                            {
                                //tempoAtteso = tempoAtteso.Add(tempoAttesoNelTurno);
                                possibileAttesa = possibileAttesa.Subtract(durataDelTurno);
                            }



                            //// Calcola quanto tempo c'è a disposizione nel turno lavorativo corrente in base al tempo di possibile attesa.
                            //TimeSpan tempoADisposizioneInQuestoTurno = fineOrarioLavorativoDelTurno.Subtract(dataOraDiRiferimento.Value);

                            //// Se il tempo a disposizione nel turno corrente è sufficiente...
                            //if (tempoADisposizioneInQuestoTurno >= possibileAttesa)
                            //{
                            //    // ...allora calcola il momento del tempo di massima attesa possibile...
                            //    dataLimiteIntervento = dataOraDiRiferimento.Value.Subtract(possibileAttesa);
                            //    //found = true;
                            //    break;
                            //}
                            //else
                            //{
                            //    // ...altrimenti calcola quanto tempo di attesa possibile viene "consumato" in questo turno e lo sottrae dal tempo totale di attesa
                            //    possibileAttesa = possibileAttesa.Subtract(tempoADisposizioneInQuestoTurno);
                            //}

                            //if (dataOraDiRiferimento.Value.Add(possibileAttesa) >= dataOraDiRiferimento.Value.Date.Add(orario.OrarioAlle))
                            //{

                            //}
                            //else
                            //{

                            //}

                        }


                        // ...invece se il turno è relativo ad un giorno successivo alla richiesta di intervento...
                        //else if (primoTurnoDisponibile && dataOraDiRiferimento.Value >= inizioOrarioLavorativoDelTurno)
                        //{

                        //}


                        //// ...invece se il giorno della richiesta di intervento il reparto NON HA un turno di lavoro allora passa al giorno successivo...
                        //else if (primoTurnoDisponibile && dataOraDiRiferimento.Value >= inizioOrarioLavorativoDelTurno)
                        //{
                        //    dataOraDiRiferimento = dataOraDiRiferimento.Value.AddDays(1);
                        //}

                    }
                    //else
                    //{
                    //    // Se il giorno non è lo stesso allora lo incrementa di 1 epassa al giorno successivo
                    //    dataOraDiRiferimento = dataOraDiRiferimento.Value.AddDays(1);
                    //}


                    primoTurnoDisponibile = false;
                }
                //found = true;
            } while (possibileAttesa > new TimeSpan(0));




            return dataLimiteIntervento;
        }

    }
}
