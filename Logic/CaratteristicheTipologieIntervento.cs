using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Logic
{
    public class CaratteristicheTipologieIntervento : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.CaratteristicheTipologieIntervento dalCaratteristicheTipologieIntervento;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public CaratteristicheTipologieIntervento()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public CaratteristicheTipologieIntervento(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public CaratteristicheTipologieIntervento(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dalCaratteristicheTipologieIntervento = new Data.CaratteristicheTipologieIntervento(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.CaratteristicaTipologiaIntervento entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                // Salvataggio nel database
                dalCaratteristicheTipologieIntervento.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'CaratteristicaTipologiaIntervento': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.CaratteristicaTipologiaIntervento> Read()
        {
            return from u in dalCaratteristicheTipologieIntervento.Read() select u;
        }

        /// <summary>
        /// Restituisce le entities per la configurazione indicata
        /// </summary>
        /// <param name="idConfigurazione"></param>
        /// <returns></returns>
        public IQueryable<Entities.CaratteristicaTipologiaIntervento> Read(Guid idConfigurazione)
        {
            return Read().Where(x => x.IdConfigurazione == idConfigurazione);
        }


        /// <summary>
        /// Restituisce l'entity in base agli id passati
        /// </summary>
        /// <param name="IdConfigurazione"></param>
        /// <param name="IdCaratteristica"></param>
        /// <returns></returns>
        public Entities.CaratteristicaTipologiaIntervento Find(Guid IdConfigurazione, int IdCaratteristica)
        {
            return dalCaratteristicheTipologieIntervento.Find(IdConfigurazione, IdCaratteristica);
        }


        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.CaratteristicaTipologiaIntervento entityToDelete, bool submitChanges)
        {
            Delete(entityToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.CaratteristicaTipologiaIntervento entityToDelete, bool checkAllegati, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dalCaratteristicheTipologieIntervento.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'CaratteristicaTipologiaIntervento': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.CaratteristicaTipologiaIntervento> entitiesToDelete, bool submitChanges)
        {
            Delete(entitiesToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.CaratteristicaTipologiaIntervento> entitiesToDelete, bool checkAllegati, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'CaratteristicaTipologiaIntervento': parametro nullo!");
            }
        }

        #endregion

    }
}
