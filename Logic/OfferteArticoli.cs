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
    public class OfferteArticoli : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.OfferteArticoli dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public OfferteArticoli()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public OfferteArticoli(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public OfferteArticoli(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.OfferteArticoli(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.OffertaArticolo entityToCreate, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'OffertaArticolo': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaArticolo> Read()
        {
            return from u in dal.Read() orderby u.Ordine select u;
        }

        /// <summary>
        /// Restituisce l'entity relativa all'IDentificativo passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.OffertaArticolo Find(EntityId<OffertaArticolo> identificativoOffertaArticolo)
        {
            return Find(identificativoOffertaArticolo.Value);
        }

        /// <summary>
        /// Restituisce tutte le entities relative all'identificativo del raggruppamento passato come parametro
        /// </summary>
        /// <param name="identificativoOffertaRaggruppamento"></param>
        /// <returns></returns>
        public IQueryable<Entities.OffertaArticolo> ReadByIDRaggruppamento(EntityId<OffertaRaggruppamento> identificativoOffertaRaggruppamento)
        {
            if (identificativoOffertaRaggruppamento == null) throw new ArgumentNullException(nameof(identificativoOffertaRaggruppamento));

            return Read().Where(x => x.IDRaggruppamento == identificativoOffertaRaggruppamento.Value);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        private Entities.OffertaArticolo Find(Guid idToFind)
        {
            return dal.Find(idToFind);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.OffertaArticolo entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                UpdateOrdine(entityToDelete, submitChanges);
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'OffertaArticolo': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.OffertaArticolo> entitiesToDelete, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'OffertaArticolo': parametro nullo!");
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Effettua l'aggiornamento dell'ordine in base all'entità passata come parametro
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="submitChanges"></param>
        public void UpdateOrdine(OffertaArticolo entity, bool submitChanges)
        {
            IQueryable<OffertaArticolo> query;
            if (entity.IDArticoloPadre.HasValue)
            {
                query = ReadSpeseAccessorie(new EntityId<OffertaArticolo>(entity.IDArticoloPadre));
            }
            else
            {
                query = Read(new EntityId<OffertaRaggruppamento>(entity.IDRaggruppamento));
            }

            query = query.Where(x => x.ID != entity.ID);

            if (query.Any())
            {
                int incr = 0;
                foreach(OffertaArticolo offertaArticolo in query)
                {
                    incr++;
                    offertaArticolo.Ordine = incr;
                }

                if (submitChanges) SubmitToDatabase();
            }
        }

        /// <summary>
        /// Effettua la clonazione dell'entità passata come parametro
        /// </summary>
        /// <param name="entityToClone"></param>
        /// <returns></returns>
        public OffertaArticolo Clone(OffertaArticolo entityToClone)
        {
            OffertaArticolo entity = new OffertaArticolo();
            entity.ID = Guid.NewGuid();
            entity.IDRaggruppamento = entityToClone.IDRaggruppamento;
            entity.Ordine = entityToClone.Ordine;
            entity.CodiceGruppo = entityToClone.CodiceGruppo;
            entity.CodiceCategoria = entityToClone.CodiceCategoria;
            entity.CodiceCategoriaStatistica = entityToClone.CodiceCategoriaStatistica;
            entity.CodiceArticolo = entityToClone.CodiceArticolo;
            entity.Descrizione = entityToClone.Descrizione;
            entity.UnitaMisura = entityToClone.UnitaMisura;
            entity.Costo = entityToClone.Costo;
            entity.Vendita = entityToClone.Vendita;
            entity.Quantita = entityToClone.Quantita;
            entity.TotaleCosto = entityToClone.TotaleCosto;
            entity.RicaricoValuta = entityToClone.RicaricoValuta;
            entity.RicaricoPercentuale = entityToClone.RicaricoPercentuale;
            entity.TotaleVendita = entityToClone.TotaleVendita;
            entity.ContieneCampiAggiuntivi = entityToClone.ContieneCampiAggiuntivi;

            return entity;
        }

        /// <summary>
        /// Restituisce il numero di ordinamento da utilizzare per la nuova entità
        /// </summary>
        /// <returns></returns>
        public int GetNuovoNumeroOrdinamento(OffertaArticolo entity)
        {
            int? max;

            if (entity.IDArticoloPadre.HasValue)
            {
                max = ReadSpeseAccessorie(new EntityId<OffertaArticolo>(entity.IDArticoloPadre)).Select(x => (int?)x.Ordine).Max();
            }
            else
            {
                max = dal.Read(new EntityId<OffertaRaggruppamento>(entity.IDRaggruppamento)).Select(x => (int?)x.Ordine).Max();
            }

            if (max.HasValue)
                return max.Value + 1;
            else
                return 1;
        }

        /// <summary>
        /// Restituisce tutte le entity associate all'entità OffertaRaggruppamento
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaArticolo> Read(Entities.OffertaRaggruppamento raggruppamento)
        {
            return from u in dal.Read(raggruppamento) orderby u.Ordine select u;
        }
        /// <summary>
        /// Restituisce tutte le entity associate all'entità OffertaRaggruppamento
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.OffertaArticolo> Read(EntityId<OffertaRaggruppamento> idRaggruppamento)
        {
            return from u in dal.Read(idRaggruppamento) orderby u.Ordine select u;
        }

        /// <summary>
        /// Effettua l'aggiornamento dell'ordine degli articoli utilizzando l'identificativo dell'articolo a cui dev'essere cambiato l'ordine e il verso dell'ordinamento
        /// </summary>
        /// <param name="idToFind"></param>
        /// <param name="upOrder"></param>
        /// <param name="submitChanges"></param>
        /// <returns></returns>
        public bool UpdateOrderByIdArticolo(EntityId<OffertaArticolo> idToFind, bool upOrder, bool submitChanges)
        {
            if (idToFind == null) throw new ArgumentNullException(nameof(idToFind));

            OffertaArticolo articolo = Find(idToFind);
            if (articolo == null) throw new Exception("L'articolo richiesto non esiste nella base dati");

            int order = articolo.Ordine;
            bool requireGridUpdate = false;
            if (articolo.IDRaggruppamento.HasValue)
            {
                IQueryable<OffertaArticolo> articoliDaAggiornare = ReadByIDRaggruppamento(new EntityId<OffertaRaggruppamento>(articolo.IDRaggruppamento.Value)).Where(x => x.ID != articolo.ID);
                if (articoliDaAggiornare.Any())
                {
                    if (upOrder)
                    {
                        articoliDaAggiornare = articoliDaAggiornare.Where(x => x.Ordine == (order - 1));
                    }
                    else
                    {
                        articoliDaAggiornare = articoliDaAggiornare.Where(x => x.Ordine == (order + 1));
                    }

                    if (articoliDaAggiornare.Any())
                    {
                        requireGridUpdate = true;

                        if (upOrder)
                        {
                            articolo.Ordine = order - 1;
                        }
                        else
                        {
                            articolo.Ordine = order + 1;
                        }

                        foreach (OffertaArticolo articoloDaAggiornare in articoliDaAggiornare)
                        {
                            if (upOrder)
                            {
                                articoloDaAggiornare.Ordine += 1;
                            }
                            else
                            {
                                articoloDaAggiornare.Ordine -= 1;
                            }
                        }
                    }

                    if (submitChanges) SubmitToDatabase();
                }
            }

            return requireGridUpdate;
        }

        /// <summary>
        /// Restituisce l'elenco delle spese accessorie collegate all'articolo il cui identificativo è stato passato come parametro
        /// </summary>
        /// <param name="identificativoOffertaArticolo"></param>
        /// <returns></returns>
        public IQueryable<OffertaArticolo> ReadSpeseAccessorie(EntityId<OffertaArticolo> identificativoOffertaArticolo)
        {
            return Read().Where(x => x.IDArticoloPadre == identificativoOffertaArticolo.Value);
        }

        #endregion
    }
}
