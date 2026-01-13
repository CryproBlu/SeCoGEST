using SeCoGEST.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Logic
{
    public class MappatureGruppiCategorieCategorieStatistiche : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.MappatureGruppiCategorieCategorieStatistiche daMappatureGruppiCategorieCategorieStatistiche;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public MappatureGruppiCategorieCategorieStatistiche()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public MappatureGruppiCategorieCategorieStatistiche(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public MappatureGruppiCategorieCategorieStatistiche(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            daMappatureGruppiCategorieCategorieStatistiche = new Data.MappatureGruppiCategorieCategorieStatistiche(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.MappaturaGruppoCategoriaCategoriaStatistica entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                // Salvataggio nel database
                daMappatureGruppiCategorieCategorieStatistiche.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'MappaturaGruppoCategoriaCategoriaStatistica': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.MappaturaGruppoCategoriaCategoriaStatistica> Read()
        {
            return from u in daMappatureGruppiCategorieCategorieStatistiche.Read() orderby u.DescrizioneGruppo, u.DescrizioneCategoria, u.DescrizioneCategoriaStatistica select u;
        }

        /// <summary>
        /// Restituisce le entità in base al valore dei parametri passati
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.MappaturaGruppoCategoriaCategoriaStatistica> Read(decimal? codiceGruppo, string descrizioneGruppo, decimal? codiceCategoria, string descrizioneCategoria, decimal? codiceCategoriaStatistica, string descrizioneCategoriaStatistica)
        {
            IQueryable<Entities.MappaturaGruppoCategoriaCategoriaStatistica> query = Read();

            if (codiceGruppo.HasValue)
            {
                query = query.Where(x => x.CodiceGruppo == codiceGruppo.Value);
            }
            else if (!String.IsNullOrWhiteSpace(descrizioneGruppo))
            {
                query = query.Where(x => x.DescrizioneGruppo.ToLower().Trim() == descrizioneGruppo.ToString().Trim());
            }

            if (codiceCategoria.HasValue)
            {
                query = query.Where(x => x.CodiceCategoria == codiceCategoria.Value);
            }
            else if (!String.IsNullOrWhiteSpace(descrizioneCategoria))
            {
                query = query.Where(x => x.DescrizioneCategoria.ToLower().Trim() == descrizioneCategoria.ToString().Trim());
            }

            if (codiceCategoriaStatistica.HasValue)
            {
                query = query.Where(x => x.CodiceCategoriaStatistica == codiceCategoriaStatistica.Value);
            }
            else if (!String.IsNullOrWhiteSpace(descrizioneCategoriaStatistica))
            {
                query = query.Where(x => x.DescrizioneCategoriaStatistica.ToLower().Trim() == descrizioneCategoriaStatistica.ToString().Trim());
            }

            return query;
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.MappaturaGruppoCategoriaCategoriaStatistica Find(EntityInt<MappaturaGruppoCategoriaCategoriaStatistica> idToFind)
        {
            return daMappatureGruppiCategorieCategorieStatistiche.Find(idToFind.Value);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.MappaturaGruppoCategoriaCategoriaStatistica entityToDelete, bool submitChanges)
        {
            Delete(entityToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.MappaturaGruppoCategoriaCategoriaStatistica entityToDelete, bool checkAllegati, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                daMappatureGruppiCategorieCategorieStatistiche.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'MappaturaGruppoCategoriaCategoriaStatistica': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.MappaturaGruppoCategoriaCategoriaStatistica> entitiesToDelete, bool submitChanges)
        {
            Delete(entitiesToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.MappaturaGruppoCategoriaCategoriaStatistica> entitiesToDelete, bool checkAllegati, bool submitChanges)
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
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'MappaturaGruppoCategoriaCategoriaStatistica': parametro nullo!");
            }
        }

        #endregion

    }
}
