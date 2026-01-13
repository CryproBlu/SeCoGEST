using System;
using System.Collections.Generic;
using System.Linq;
using SeCoGEST.Entities;

namespace SeCoGEST.Logic
{
    public class AnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.AnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public AnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public AnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public AnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.AnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                if (entityToCreate.IDAnalisiVenditaConfigurazioneArticoloAggiuntivo.Equals(Guid.Empty)) throw new Exception($"La proprietà \"{nameof(AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria.IDAnalisiVenditaConfigurazioneArticoloAggiuntivo)}\" non contiene un identificativo valido");
                if (entityToCreate.IDClausolaVessatoria.Equals(Guid.Empty)) throw new Exception($"La proprietà \"{nameof(AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria.IDClausolaVessatoria)}\" non contiene un identificativo valido");

                // Salvataggio nel database
                dal.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria> Read()
        {
            return from u in dal.Read() select u;
        }

        /// <summary>
        /// Restituisce l'entity relativa all'IDentificativo passato e null se non trova l'entity
        /// </summary>
        /// <param name="identificativoAnalisiVenditaConfigurazioneArticoloAggiuntivo"></param>
        /// <param name="identificativoClausolaVessatoria"></param>
        /// <returns></returns>
        public Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria Find(EntityId<AnalisiVenditaConfigurazioneArticoloAggiuntivo> identificativoAnalisiVenditaConfigurazioneArticoloAggiuntivo, EntityId<ClausolaVessatoria> identificativoClausolaVessatoria)
        {
            return Find(identificativoAnalisiVenditaConfigurazioneArticoloAggiuntivo.Value, identificativoClausolaVessatoria.Value);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idAnalisiVenditaConfigurazioneArticoloAggiuntivoToFind"></param>
        /// <param name="idClausolaVessatoriaToFind"></param>
        /// <returns></returns>
        private Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria Find(Guid idAnalisiVenditaConfigurazioneArticoloAggiuntivoToFind, Guid idClausolaVessatoriaToFind)
        {
            return dal.Find(idAnalisiVenditaConfigurazioneArticoloAggiuntivoToFind, idClausolaVessatoriaToFind);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria> entitiesToDelete, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria': parametro nullo!");
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Restituisce tutte le entity associate all'entità AnalisiVenditaConfigurazioneArticoloAggiuntivo in base all'entità passata come parametro
        /// </summary>
        /// <param name="analisiVenditaConfigurazioneArticoloAggiuntivo"></param>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria> Read(Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo analisiVenditaConfigurazioneArticoloAggiuntivo)
        {
            return from u in dal.Read(analisiVenditaConfigurazioneArticoloAggiuntivo) select u;
        }

        /// <summary>
        /// Restituisce tutte le entity associate all'entità ClausolaVessatoria in base all'entità passata come parametro
        /// </summary>
        /// <param name="clausolaVessatoria"></param>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria> Read(Entities.ClausolaVessatoria clausolaVessatoria)
        {
            return from u in dal.Read(clausolaVessatoria) select u;
        }

        #endregion
    }
}