using System.Linq;

namespace SeCoGEST.Logic.Metodo
{
    public class AnagraficheArticoli : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.Metodo.AnagraficheArticoli dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public AnagraficheArticoli()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public AnagraficheArticoli(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public AnagraficheArticoli(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.Metodo.AnagraficheArticoli(this.context);
        }

        #endregion

        #region Read

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.ANAGRAFICAARTICOLI> Read()
        {
            return from u in dal.Read() orderby u.CODICE, u.DESCRIZIONE select u;
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="codconto"></param>
        /// <returns></returns>
        public Entities.ANAGRAFICAARTICOLI Find(string codconto)
        {
            return dal.Find(codconto);
        }

        /// <summary>
        /// Restituisce tutte le entities in base al codice del gruppo, categoria, categoria statistica passati come parametro
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.ANAGRAFICAARTICOLI> Read(decimal? codiceGruppo, decimal? codiceCategoria, decimal? codiceCategoriaStatistica)
        {
            IQueryable<Entities.ANAGRAFICAARTICOLI> queryBase = Read();

            if (codiceGruppo.HasValue) queryBase = queryBase.Where(x => x.GRUPPO.Value == codiceGruppo.Value);
            if (codiceCategoria.HasValue) queryBase = queryBase.Where(x => x.CATEGORIA.Value == codiceCategoria.Value);
            if (codiceCategoriaStatistica.HasValue) queryBase = queryBase.Where(x => x.CODCATEGORIASTAT.Value == codiceCategoriaStatistica.Value);

            return queryBase;
        }

        #endregion
    }
}
