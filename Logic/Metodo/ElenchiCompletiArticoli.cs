using System;
using System.Linq;

namespace SeCoGEST.Logic.Metodo
{
    public class ElenchiCompletiArticoli : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.Metodo.ElenchiCompletiArticoli dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public ElenchiCompletiArticoli()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public ElenchiCompletiArticoli(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public ElenchiCompletiArticoli(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.Metodo.ElenchiCompletiArticoli(this.context);
        }

        #endregion

        #region Read

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.ElencoCompletoArticoli> Read(DateTime dataValidita)
        {
            return from u in dal.Read(dataValidita) orderby u.CodiceArticolo, u.Descrizione select u;
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="codconto"></param>
        /// <returns></returns>
        public Entities.ElencoCompletoArticoli Find(string codconto, DateTime dataValidita)
        {
            return dal.Find(codconto, dataValidita);
        }

        #endregion

        /// <summary>
        /// Restituisce l'elenco di Unità di Misura associate all'articolo
        /// </summary>
        /// <param name="codiceArticolo"></param>
        /// <returns></returns>
        public IQueryable<Entities.ARTICOLIUNITAMISURA> UnitaMisuraPerArticolo(string codiceArticolo)
        {
            return dal.UnitaMisuraPerArticolo(codiceArticolo);
        }

        /// <summary>
        /// Restituisce un valore booleano che indica se l'unità di misura indicata è associata all'articolo indicato
        /// </summary>
        /// <param name="codiceArticolo"></param>
        /// <param name="um"></param>
        /// <returns></returns>
        public bool UnitaDiMisuraAssociata(string codiceArticolo, string um)
        {
            return UnitaMisuraPerArticolo(codiceArticolo).Select(x => x.UM).Contains(um);
        }

    }
}
