using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SeCoGEST.Entities;

namespace SeCoGEST.Logic
{
    public class AnalisiVenditeConfigurazioneArticoliAggiuntivi : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.AnalisiVenditeConfigurazioneArticoliAggiuntivi dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public AnalisiVenditeConfigurazioneArticoliAggiuntivi()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public AnalisiVenditeConfigurazioneArticoliAggiuntivi(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public AnalisiVenditeConfigurazioneArticoliAggiuntivi(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.AnalisiVenditeConfigurazioneArticoliAggiuntivi(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo entityToCreate, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'AnalisiVenditaConfigurazioneArticoloAggiuntivo': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo> Read()
        {
            return from u in dal.Read() orderby u.CodiceGruppoIn, u.CodiceCategoriaIn, u.CodiceCategoriaStatisticaIn select u;
        }

        /// <summary>
        /// Restituisce l'entity relativa all'IDentificativo passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo Find(EntityId<AnalisiVenditaConfigurazioneArticoloAggiuntivo> identificativoAnalisiVenditaConfigurazioneArticoloAggiuntivo)
        {
            return Find(identificativoAnalisiVenditaConfigurazioneArticoloAggiuntivo.Value);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        private Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo Find(Guid idToFind)
        {
            return dal.Find(idToFind);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'AnalisiVenditaConfigurazioneArticoloAggiuntivo': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo> entitiesToDelete, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'AnalisiVenditaConfigurazioneArticoloAggiuntivo': parametro nullo!");
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Restituisce tutte le entity associate ai valori di gruppo, categoria e categoria statistica indicati
        /// </summary>
        /// <param name="codiceGruppo"></param>
        /// <param name="codiceCategoria"></param>
        /// <param name="codiceCategoriaStatistica"></param>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo> Read(decimal codiceGruppo, decimal codiceCategoria, decimal codiceCategoriaStatistica)
        {
            return from u in dal.Read(codiceGruppo, codiceCategoria, codiceCategoriaStatistica) orderby u.CodiceGruppoIn, u.CodiceCategoriaIn, u.CodiceCategoriaStatisticaIn select u;
        }

        /// <summary>
        /// Restituisce tutte le entity relative ad una configurazione vuota
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo> ReadEmptyConfiguration()
        {
            return Read().Where(x =>
                (x.CodiceArticoloIn == null || x.CodiceArticoloIn.Trim() == String.Empty) &&
                !x.CodiceGruppoIn.HasValue &&
                !x.CodiceCategoriaIn.HasValue &&
                !x.CodiceCategoriaStatisticaIn.HasValue)
                .OrderBy(x => x.CodiceArticoloIn)
                .ThenBy(x => x.CodiceGruppoIn)
                .ThenBy(x => x.CodiceCategoriaIn)
                .ThenBy(x => x.CodiceCategoriaStatisticaIn);
        }

        /// <summary>
        /// Restituisce tutte le entity relative ad una configurazione vuota relativa alla tipologia passata
        /// </summary>
        /// <param name="tipologia"></param>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo> ReadEmptyConfiguration(TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo tipologia)
        {
            return ReadEmptyConfiguration().Where(x => x.Tipologia == (byte)tipologia);
        }

        /// <summary>
        /// Restituisce tutte le entity associate all'articolo dell'offerta passato come parametro
        /// </summary>
        /// <param name="offertaArticolo"></param>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo> Read(Entities.OffertaArticolo offertaArticolo)
        {
            return Read(offertaArticolo.CodiceArticolo, offertaArticolo.CodiceGruppo, offertaArticolo.CodiceCategoria, offertaArticolo.CodiceCategoriaStatistica);
        }

        /// <summary>
        /// Restituisce tutte le entity associate ai valori passati come parametro
        /// </summary>
        /// <param name="codiceArticolo"></param>
        /// <param name="codiceGruppo"></param>
        /// <param name="codiceCategoria"></param>
        /// <param name="codiceCategoriaStatistica"></param>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo> Read(string codiceArticolo, decimal? codiceGruppo, decimal? codiceCategoria, decimal? codiceCategoriaStatistica)
        {
            IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo> query = Read();

            Expression<Func<AnalisiVenditaConfigurazioneArticoloAggiuntivo, bool>> condizione = null;

            if (!String.IsNullOrWhiteSpace(codiceArticolo))
            {
                condizione = (x) => x.CodiceArticoloIn.Trim().ToLower() == codiceArticolo.Trim().ToLower();
            }

            if (codiceGruppo.HasValue)
            {
                Expression<Func<AnalisiVenditaConfigurazioneArticoloAggiuntivo, bool>> expressionTemp = x => x.CodiceGruppoIn.HasValue && x.CodiceGruppoIn.Value == codiceGruppo;

                if (condizione == null)
                {
                    condizione = expressionTemp;
                }
                else
                {
                    condizione = Helper.LinqHelper.OrElse<AnalisiVenditaConfigurazioneArticoloAggiuntivo>(condizione, expressionTemp);
                }
            }

            if (codiceCategoria.HasValue)
            {
                Expression<Func<AnalisiVenditaConfigurazioneArticoloAggiuntivo, bool>> expressionTemp = x => x.CodiceCategoriaIn.HasValue && x.CodiceCategoriaIn.Value == codiceCategoria;

                if (condizione == null)
                {
                    condizione = expressionTemp;
                }
                else
                {
                    condizione = Helper.LinqHelper.OrElse<AnalisiVenditaConfigurazioneArticoloAggiuntivo>(condizione, expressionTemp);
                }
            }

            if (codiceCategoriaStatistica.HasValue)
            {

                Expression<Func<AnalisiVenditaConfigurazioneArticoloAggiuntivo, bool>> expressionTemp = x => x.CodiceCategoriaStatisticaIn.HasValue && x.CodiceCategoriaStatisticaIn.Value == codiceCategoriaStatistica;

                if (condizione == null)
                {
                    condizione = expressionTemp;
                }
                else
                {
                    condizione = Helper.LinqHelper.OrElse<AnalisiVenditaConfigurazioneArticoloAggiuntivo>(condizione, expressionTemp);
                }
            }

            if (condizione != null)
            {
                query = query.Where(condizione);
            }

            query = query.OrderBy(x => x.CodiceArticoloIn)
                .ThenBy(x => x.CodiceGruppoIn)
                .ThenBy(x => x.CodiceCategoriaIn)
                .ThenBy(x => x.CodiceCategoriaStatisticaIn);

            return query;
        }

        /// <summary>
        /// Restituisce tutte le entity associate all'articolo dell'offerta passato come parametro
        /// </summary>
        /// <param name="offertaArticolo"></param>
        /// <param name="tipologia"></param>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo> Read(Entities.OffertaArticolo offertaArticolo, TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo tipologia)
        {
            return Read(offertaArticolo).Where(x => x.Tipologia == (byte)tipologia);
        }

        /// <summary>
        /// Restituisce tutte le entity associate ai valori passati come parametro
        /// </summary>
        /// <param name="codiceArticolo"></param>
        /// <param name="codiceGruppo"></param>
        /// <param name="codiceCategoria"></param>
        /// <param name="codiceCategoriaStatistica"></param>
        /// <param name="tipologia"></param>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo> Read(string codiceArticolo, decimal? codiceGruppo, decimal? codiceCategoria, decimal? codiceCategoriaStatistica, TipologiaAnalisiVenditaConfigurazioneArticoloAggiuntivo tipologia)
        {
            return Read(codiceArticolo, codiceGruppo, codiceCategoria, codiceCategoriaStatistica).Where(x => x.Tipologia == (byte)tipologia);
        }

        #endregion
    }
}