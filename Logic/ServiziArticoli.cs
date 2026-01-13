using SeCoGEST.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Logic
{
    public class ServiziArticoli : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.ServiziArticoli dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public ServiziArticoli()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public ServiziArticoli(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public ServiziArticoli(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.ServiziArticoli(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.ServizioArticolo entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                // Salvataggio nel database
                dal.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'ServizioArticolo': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.ServizioArticolo> Read()
        {
            return dal.Read().OrderBy(x => x.CodiceAnagraficaArticolo);
        }

        /// <summary>
        /// Restituisce tutte le entities relative all'identificativo passato come parametro
        /// </summary>
        /// <param name="identificativoServizio"></param>
        /// <returns></returns>
        public IQueryable<Entities.ServizioArticolo> Read(EntityId<Servizio> identificativoServizio)
        {
            return Read().Where(x => x.IDServizio == identificativoServizio.Value);
        }

        /// <summary>
        /// Restituisce tutte le entities relative al codice dell'articolo passato come parametro
        /// </summary>
        /// <param name="codiceAnagraficaArticolo"></param>
        /// <returns></returns>
        public IQueryable<Entities.ServizioArticolo> Read(EntityString<ANAGRAFICAARTICOLI> codiceAnagraficaArticolo)
        {
            return Read().Where(x => x.CodiceAnagraficaArticolo.ToLower().Trim() == codiceAnagraficaArticolo.Value.ToLower().Trim());
        }

        /// <summary>
        /// Restituisce tutte le entities relative ai codici dell'articolo passato come parametro
        /// </summary>
        /// <param name="codiciAnagraficaArticolo"></param>
        /// <returns></returns>
        public IQueryable<Entities.ServizioArticolo> Read(string[] codiciAnagraficaArticolo)
        {
            if (codiciAnagraficaArticolo == null) codiciAnagraficaArticolo = new string[] { };
            codiciAnagraficaArticolo = codiciAnagraficaArticolo.Select(x => x.ToLower().Trim()).ToArray();

            return Read().Where(x => codiciAnagraficaArticolo.Contains(x.CodiceAnagraficaArticolo.ToLower().Trim()));
        }

        /// <summary>
        /// Restituisce tutte le entities relative al codice passato come parametro
        /// </summary>
        /// <param name="codiceAnagraficaArticolo"></param>
        /// <returns></returns>
        public Entities.ServizioArticolo Find(EntityString<ANAGRAFICAARTICOLI> codiceAnagraficaArticolo)
        {
            return Read().FirstOrDefault(x => x.CodiceAnagraficaArticolo.ToLower().Trim() == codiceAnagraficaArticolo.Value.ToLower().Trim());
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.ServizioArticolo Find(EntityId<Servizio> identificativoServizio, EntityString<ANAGRAFICAARTICOLI> codiceAnagraficaArticolo)
        {
            return Read().FirstOrDefault(x => x.IDServizio == identificativoServizio.Value && x.CodiceAnagraficaArticolo.ToLower().Trim() == codiceAnagraficaArticolo.Value.ToLower().Trim());
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.ServizioArticolo entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'ServizioArticolo': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.ServizioArticolo> entitiesToDelete, bool submitChanges)
        {
            if (entitiesToDelete != null)
            {
                dal.Delete(entitiesToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'ServizioArticolo': parametro nullo!");
            }
        }

        #endregion
    }
}
