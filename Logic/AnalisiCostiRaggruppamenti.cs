using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGEST.Entities;
using SeCoGEST.Helper;
//using msWord = Microsoft.Office.Interop.Word;

namespace SeCoGEST.Logic
{
    public class AnalisiCostiRaggruppamenti : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.AnalisiCostiRaggruppamenti dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public AnalisiCostiRaggruppamenti()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public AnalisiCostiRaggruppamenti(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public AnalisiCostiRaggruppamenti(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.AnalisiCostiRaggruppamenti(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.AnalisiCostoRaggruppamento entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                if (entityToCreate.ID.Equals(Guid.Empty))
                {
                    entityToCreate.ID = Guid.NewGuid();
                    entityToCreate.Ordine = GetNuovoNumeroOrdinamento(entityToCreate);
                }

                // Salvataggio nel database
                dal.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'AnalisiCostoRaggruppamento': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoRaggruppamento> Read()
        {
            return from u in dal.Read() orderby u.Ordine select u;
        }

        /// <summary>
        /// Restituisce l'entity relativa all'IDentificativo passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.AnalisiCostoRaggruppamento Find(EntityId<AnalisiCostoRaggruppamento> identificativoAnalisiCostoRaggruppamento)
        {
            return Find(identificativoAnalisiCostoRaggruppamento.Value);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        private Entities.AnalisiCostoRaggruppamento Find(Guid idToFind)
        {
            return dal.Find(idToFind);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.AnalisiCostoRaggruppamento entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'AnalisiCostoRaggruppamento': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.AnalisiCostoRaggruppamento> entitiesToDelete, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'AnalisiCostoRaggruppamento': parametro nullo!");
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Restituisce il numero di ordinamento da utilizzare per la nuova entità
        /// </summary>
        /// <returns></returns>
        private int GetNuovoNumeroOrdinamento(AnalisiCostoRaggruppamento entity)
        {
            int? max = dal.Read(new EntityId<AnalisiCosto>(entity.IDAnalisiCosto)).Select(x => (int?)x.Ordine).Max();
            if (max.HasValue)
                return max.Value + 1;
            else
                return 1;
        }

        /// <summary>
        /// Restituisce tutte le entity associate all'entità AnalisiCosto
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoRaggruppamento> Read(Entities.AnalisiCosto analisiCosto)
        {
            return from u in dal.Read(analisiCosto) orderby u.Ordine select u;
        }
        /// <summary>
        /// Restituisce tutte le entity associate all'entità AnalisiCosto
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCostoRaggruppamento> Read(EntityId<AnalisiCosto> idAnalisiCosto)
        {
            return from u in dal.Read(idAnalisiCosto) orderby u.Ordine select u;
        }

        #endregion
    }
}
