using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Logic
{
    public class PeriodiFestivita : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.PeriodiFestivita dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public PeriodiFestivita()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public PeriodiFestivita(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public PeriodiFestivita(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.PeriodiFestivita(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.PeriodoFestivita entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                // Salvataggio nel database
                dal.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'PeriodoFestivita': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.PeriodoFestivita> Read()
        {
            return dal.Read().OrderBy(x=> x.Id);
        }
        
        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.PeriodoFestivita Find(int id)
        {
            return dal.Find(id);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.PeriodoFestivita entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'PeriodoFestivita': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.PeriodoFestivita> entitiesToDelete, bool submitChanges)
        {
            if (entitiesToDelete != null)
            {
                dal.Delete(entitiesToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'PeriodoFestivita': parametro nullo!");
            }
        }

        #endregion

        public bool IsGiornoDiFestivita(DateTime giorno)
        {
            return IsGiornoDiFestivita(giorno, this.Read().ToList());
        }
        public bool IsGiornoDiFestivita(DateTime giorno, List<Entities.PeriodoFestivita> festivita)
        {
            foreach (Entities.PeriodoFestivita festa in festivita)
            {
                // Se la festività è indicata solo in base al numero del giorno...
                if(festa.Giorno.HasValue && !festa.Mese.HasValue && !festa.Anno.HasValue)
                {
                    if(giorno.Day == festa.Giorno.Value) return true;
                }
                // Se la festività è indicata solo in base al numero del mese...
                if (!festa.Giorno.HasValue && festa.Mese.HasValue && !festa.Anno.HasValue)
                {
                    if (giorno.Month == festa.Mese.Value) return true;
                }
                // Se la festività è indicata solo in base al numero dell'anno...
                if (!festa.Giorno.HasValue && !festa.Mese.HasValue && festa.Anno.HasValue)
                {
                    if(giorno.Year == festa.Anno.Value) return true;
                }


                // Se la festività è indicata in base al numero del giorno e del mese...
                if (festa.Giorno.HasValue && festa.Mese.HasValue && !festa.Anno.HasValue)
                {
                    if(giorno.Day == festa.Giorno.Value && giorno.Month == festa.Mese.Value) return true;
                }
                // Se la festività è indicata in base al numero del mese e dell'anno...
                if (!festa.Giorno.HasValue && festa.Mese.HasValue && festa.Anno.HasValue)
                {
                    if(giorno.Month == festa.Mese.Value && giorno.Year == festa.Anno.Value) return true;
                }
                // Se la festività è indicata in base al numero del giorno e dell'anno...
                if (festa.Giorno.HasValue && !festa.Mese.HasValue && festa.Anno.HasValue)
                {
                    if (giorno.Day == festa.Giorno.Value && giorno.Year == festa.Anno.Value) return true;
                }


                // Se la festività è indicata in base alla data completa...
                if (festa.Giorno.HasValue && festa.Mese.HasValue && festa.Anno.HasValue)
                {
                    if (giorno.Day == festa.Giorno.Value && giorno.Month == festa.Mese.Value && giorno.Year == festa.Anno.Value) return true;
                }
            }

            return false;
        }
    }
}
