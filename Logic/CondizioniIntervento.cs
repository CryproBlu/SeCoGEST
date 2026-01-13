using SeCoGEST.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Logic
{
    public class CondizioniIntervento : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.CondizioniIntervento dalCondizioniIntervento;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public CondizioniIntervento()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public CondizioniIntervento(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public CondizioniIntervento(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dalCondizioniIntervento = new Data.CondizioniIntervento(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.CondizioneIntervento entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                if (entityToCreate.Id.Equals(0))
                {
                    int? maxId = Read().Max(x => x.Id);
                    if (!maxId.HasValue) maxId = 0;
                    entityToCreate.Id = maxId.Value + 1;
                }

                // Salvataggio nel database
                dalCondizioniIntervento.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'CondizioneIntervento': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.CondizioneIntervento> Read()
        {
            return from u in dalCondizioniIntervento.Read() orderby u.Nome select u;
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.CondizioneIntervento Find(int idToFind)
        {
            return dalCondizioniIntervento.Find(idToFind);
        }

        /// <summary>
        /// Restituisce l'entity relativa al nome passato e null se non trova l'entity
        /// </summary>
        /// <param name="nomeCondizioneInterventoToFind"></param>
        /// <returns></returns>
        public Entities.CondizioneIntervento Find(EntityString<CondizioneIntervento> nomeCondizioneInterventoToFind)
        {
            return dalCondizioniIntervento.Find(nomeCondizioneInterventoToFind.Value);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.CondizioneIntervento entityToDelete, bool submitChanges)
        {
            Delete(entityToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.CondizioneIntervento entityToDelete, bool checkAllegati, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dalCondizioniIntervento.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'CondizioneIntervento': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.CondizioneIntervento> entitiesToDelete, bool submitChanges)
        {
            Delete(entitiesToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.CondizioneIntervento> entitiesToDelete, bool checkAllegati, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'CondizioneIntervento': parametro nullo!");
            }
        }

        #endregion
    }
}
