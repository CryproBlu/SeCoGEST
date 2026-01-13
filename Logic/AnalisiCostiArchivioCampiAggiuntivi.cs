using System;
using System.Collections.Generic;
using System.Linq;
using SeCoGEST.Entities;

namespace SeCoGEST.Logic
{
    public class AnalisiCostiArchivioCampiAggiuntivi : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.AnalisiCostiArchivioCampiAggiuntivi dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public AnalisiCostiArchivioCampiAggiuntivi()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public AnalisiCostiArchivioCampiAggiuntivi(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public AnalisiCostiArchivioCampiAggiuntivi(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.AnalisiCostiArchivioCampiAggiuntivi(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.AnalisiCostoArchivioCampoAggiuntivo entityToCreate, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'AnalisiCostoArchivioCampoAggiuntivo': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoArchivioCampoAggiuntivo> Read()
        {
            return from u in dal.Read() orderby u.CodiceGruppo, u.CodiceCategoria, u.CodiceCategoriaStatistica, u.Ordine select u;
        }

        /// <summary>
        /// Restituisce l'entity relativa all'IDentificativo passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.AnalisiCostoArchivioCampoAggiuntivo Find(EntityId<AnalisiCostoArchivioCampoAggiuntivo> identificativoAnalisiCostoArchivioCampoAggiuntivo)
        {
            return Find(identificativoAnalisiCostoArchivioCampoAggiuntivo.Value);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        private Entities.AnalisiCostoArchivioCampoAggiuntivo Find(Guid idToFind)
        {
            return dal.Find(idToFind);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.AnalisiCostoArchivioCampoAggiuntivo entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'AnalisiCostoArchivioCampoAggiuntivo': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.AnalisiCostoArchivioCampoAggiuntivo> entitiesToDelete, bool submitChanges)
        {
            if (entitiesToDelete != null)
            {
                if (entitiesToDelete.Count() > 0)
                {
                    entitiesToDelete.ToList().ForEach(x => Delete(x, submitChanges));
                }
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'AnalisiCostoArchivioCampoAggiuntivo': parametro nullo!");
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Restituisce tutte le entity associate ai valori di gruppo, categoria e categoria statistica indicati
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoArchivioCampoAggiuntivo> Read(decimal codiceGruppo, decimal codiceCategoria, decimal codiceCategoriaStatistica)
        {
            return from u in dal.Read(codiceGruppo, codiceCategoria, codiceCategoriaStatistica) orderby u.CodiceGruppo, u.CodiceCategoria, u.CodiceCategoriaStatistica, u.Ordine select u;
        }

        #endregion
    }
}