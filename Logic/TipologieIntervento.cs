using SeCoGEST.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Logic
{
    public class TipologieIntervento : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.TipologieIntervento dalTipologieIntervento;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public TipologieIntervento()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public TipologieIntervento(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public TipologieIntervento(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dalTipologieIntervento = new Data.TipologieIntervento(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.TipologiaIntervento entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                if (entityToCreate.Id.Equals(Guid.Empty))
                {
                    entityToCreate.Id = Guid.NewGuid();
                }

                // Salvataggio nel database
                dalTipologieIntervento.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'TipologiaIntervento': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.TipologiaIntervento> Read()
        {
            return from u in dalTipologieIntervento.Read() orderby u.Nome select u;
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.TipologiaIntervento Find(EntityId<TipologiaIntervento> idToFind)
        {
            return dalTipologieIntervento.Find(idToFind.Value);
        }

        /// <summary>
        /// Restituisce l'entity relativa al nome passato e null se non trova l'entity
        /// </summary>
        /// <param name="nomeTipologiaInterventoToFind"></param>
        /// <returns></returns>
        public Entities.TipologiaIntervento Find(EntityString<TipologiaIntervento> nomeTipologiaInterventoToFind)
        {
            return dalTipologieIntervento.Find(nomeTipologiaInterventoToFind.Value);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.TipologiaIntervento entityToDelete, bool submitChanges)
        {
            Delete(entityToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.TipologiaIntervento entityToDelete, bool checkAllegati, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dalTipologieIntervento.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'TipologiaIntervento': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.TipologiaIntervento> entitiesToDelete, bool submitChanges)
        {
            Delete(entitiesToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.TipologiaIntervento> entitiesToDelete, bool checkAllegati, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'TipologiaIntervento': parametro nullo!");
            }
        }

        #endregion


        /// <summary>
        /// Restituisce un numero intero relativo alla versione del modello di word utilizzato per le esportazioni
        /// </summary>
        /// <param name="idTipologia"></param>
        /// <returns></returns>
        public int GetVersioneModelloEsportazione(Guid idTipologia)
        {
            int versione = 1;
            Entities.TipologiaIntervento tipologiaIntervento =  dalTipologieIntervento.Find(idTipologia);
            if(tipologiaIntervento != null)
            {
                if(tipologiaIntervento.VersioneModelloEsportazione.HasValue)
                {
                    if(tipologiaIntervento.VersioneModelloEsportazione.Value >= 2)
                    {
                        versione = tipologiaIntervento.VersioneModelloEsportazione.Value;
                    }
                }
            }
            return versione;
        }

    }
}
