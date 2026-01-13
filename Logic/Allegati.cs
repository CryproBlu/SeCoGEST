using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGEST.Entities;
//using SeCoGEST.Entities.Applicazione;
//using SeCoGEST.Entities.Documentazione;
using SeCoGEST.Infrastructure;

namespace SeCoGEST.Logic
{
    public class Allegati : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// DataLayer utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.Allegati dal;


        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public Allegati()
            : base(false)
        {
            CreateDal();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public Allegati(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDal();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public Allegati(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDal();
        }



        /// <summary>
        /// Crea un DataLayer che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDal()
        {
            dal = new Data.Allegati(this.context);
        }

        #endregion


        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Allegato entityToCreate, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'Allegato': parametro nullo!");
            }
        }

        /// <summary>
        /// Effettua la creazione di un record relativo al soggetto il cui id è passato come parametro
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="idLegame"></param>
        /// <param name="tipo"></param>
        /// <param name="submitChanges"></param>
        public void Create<T>(Entities.Allegato entity, EntityId<T> idLegame, InfoOperazioneTabellaEnum tipo, bool submitChanges) where T : class
        {
            Entities.Allegato allegatoDocumento = entity;
            allegatoDocumento.IDLegame = idLegame.Value;

            // Viene memorizzato l'allegato documento passato come parametro ..
            Create(allegatoDocumento, submitChanges);
        }


        #region Read

        /// <summary>
        /// Restituisce tutti i Corsi
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Allegato> Read()
        {
            return from u 
                   in dal.Read() 
                   orderby u.DataArchiviazione descending, u.NomeFile ascending
                   select u;
        }
        
        /// <summary>
        /// Restituisce tutte le entity in base al valore dei parametri passati
        /// </summary>
        /// <param name="idLegame"></param>
        /// <returns></returns>
        public IQueryable<Entities.Allegato> Read<T>(EntityId<T> idLegame) where T : class
        {
            return Read().Where(x => x.IDLegame == idLegame.Value);
        }

        #endregion


        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.Allegato Find(Guid idToFind)
        {
            return dal.Find(idToFind);
        }


        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Allegato entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'Allegato': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.Allegato> entitiesToDelete, bool submitChanges)
        {
            if (entitiesToDelete != null)
            {
                dal.Delete(entitiesToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'Allegato': parametro nullo!");
            }
        }

        /// <summary>
        /// Effettua la cancellazione degli allegati associati all'entità il cui id è passato come parametro
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="idLegame"></param>
        /// <param name="tipo"></param>
        /// <param name="submitChanges"></param>
        public void Delete<T>(EntityId<T> idLegame, bool submitChanges) where T : class
        {
            // Vengono recuperati i documenti legati all'ID da eliminare
            IQueryable<Entities.Allegato> allegati = Read<T>(idLegame);

            // Nel caso in cui i documenti esistano, vengono eliminati
            if (allegati != null && allegati.Count() > 0)
            {
                Delete(allegati, submitChanges);
            }
        }

        #endregion
    }
}
