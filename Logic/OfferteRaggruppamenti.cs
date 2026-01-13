using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGEST.Entities;
using SeCoGEST.Helper;
using msWord = Microsoft.Office.Interop.Word;

namespace SeCoGEST.Logic
{
    public class OfferteRaggruppamenti : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.OfferteRaggruppamenti dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public OfferteRaggruppamenti()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public OfferteRaggruppamenti(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public OfferteRaggruppamenti(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.OfferteRaggruppamenti(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.OffertaRaggruppamento entityToCreate, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'OffertaRaggruppamento': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaRaggruppamento> Read()
        {
            return from u in dal.Read() orderby u.Ordine select u;
        }

        /// <summary>
        /// Restituisce l'entity relativa all'IDentificativo passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.OffertaRaggruppamento Find(EntityId<OffertaRaggruppamento> identificativoOffertaRaggruppamento)
        {
            return Find(identificativoOffertaRaggruppamento.Value);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        private Entities.OffertaRaggruppamento Find(Guid idToFind)
        {
            return dal.Find(idToFind);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.OffertaRaggruppamento entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'OffertaRaggruppamento': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.OffertaRaggruppamento> entitiesToDelete, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'OffertaRaggruppamento': parametro nullo!");
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Effettua la clonazione del grupo passato come parametro
        /// </summary>
        /// <param name="entityToClone"></param>
        /// <param name="cloneOffertaArticolos"></param>
        /// <param name="submitToDatabase"></param>
        /// <returns></returns>
        public OffertaRaggruppamento Clone(OffertaRaggruppamento entityToClone, bool cloneOffertaArticolos, bool submitToDatabase)
        {
            OffertaRaggruppamento entity = new OffertaRaggruppamento();
            entity.ID = Guid.NewGuid();
            entity.IDOfferta = entityToClone.IDOfferta;
            entity.IDRaggruppamentoPadre = entityToClone.IDRaggruppamentoPadre;
            entity.Ordine = entityToClone.Ordine;
            entity.Denominazione = entityToClone.Denominazione;
            entity.TotaleCosto = entityToClone.TotaleCosto;
            entity.TotaleVenditaCalcolato = entityToClone.TotaleVenditaCalcolato;
            entity.TotaleRicaricoValuta = entityToClone.TotaleRicaricoValuta;
            entity.TotaleRicaricoPercentuale = entityToClone.TotaleRicaricoPercentuale;
            entity.TotaleVendita = entityToClone.TotaleVendita;
            entity.TotaleCostoCalcolato = entityToClone.TotaleCostoCalcolato;
            entity.TotaleRicaricoValutaCalcolato = entityToClone.TotaleRicaricoValutaCalcolato;
            entity.TotaleRicaricoPercentualeCalcolato = entityToClone.TotaleRicaricoPercentualeCalcolato;
            entity.TotaliModificati = entityToClone.TotaliModificati;
            entity.OpzioneStampaOfferta = entityToClone.OpzioneStampaOfferta;

            Create(entity, submitToDatabase);

            if (cloneOffertaArticolos && entityToClone.OffertaArticolos.Count > 0)
            {
                OfferteArticoli llArticoli = new OfferteArticoli(this);

                foreach (OffertaArticolo articolo in entityToClone.OffertaArticolos.OrderBy(x => x.Ordine))
                {
                    OffertaArticolo nuovoArticolo = llArticoli.Clone(articolo);
                    nuovoArticolo.IDRaggruppamento = entity.ID;

                    llArticoli.Create(nuovoArticolo, submitToDatabase);
                }
            }

            return entity;
        }

        /// <summary>
        /// Restituisce il numero di ordinamento da utilizzare per la nuova entità
        /// </summary>
        /// <returns></returns>
        public int GetNuovoNumeroOrdinamento(OffertaRaggruppamento entity)
        {
            int? max = dal.Read(new EntityId<Offerta>(entity.IDOfferta)).Select(x => (int?)x.Ordine).Max();
            if (max.HasValue)
                return max.Value + 1;
            else
                return 1;
        }

        /// <summary>
        /// Restituisce tutte le entity associate all'entità Offerta
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaRaggruppamento> Read(Entities.Offerta offerta)
        {
            return from u in dal.Read(offerta) orderby u.Ordine select u;
        }
        /// <summary>
        /// Restituisce tutte le entity associate all'entità Offerta
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaRaggruppamento> Read(EntityId<Offerta> idOfferta)
        {
            return from u in dal.Read(idOfferta) orderby u.Ordine select u;
        }

        #endregion
    }
}
