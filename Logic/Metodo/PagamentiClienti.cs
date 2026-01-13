//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SeCoGEST.Logic.Metodo
//{
//    public class PagamentiClienti : Base.LogicLayerBase
//    {
//        #region Costruttori e DAL interno

//        /// <summary>
//        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
//        /// </summary>
//        private Data.Metodo.PagamentiClienti dal;

//        /// <summary>
//        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
//        /// </summary>
//        public PagamentiClienti()
//            : base(false)
//        {
//            CreateDalAndLogic();
//        }

//        /// <summary>
//        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
//        /// </summary>
//        /// <param name="createStandaloneContext"></param>
//        public PagamentiClienti(bool createStandaloneContext)
//            : base(createStandaloneContext)
//        {
//            CreateDalAndLogic();
//        }

//        /// <summary>
//        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
//        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
//        /// </summary>
//        /// <param name="logicLayer"></param>
//        public PagamentiClienti(Base.LogicLayerBase logicLayer)
//            : base(logicLayer)
//        {
//            CreateDalAndLogic();
//        }



//        /// <summary>
//        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
//        /// </summary>
//        private void CreateDalAndLogic()
//        {
//            dal = new Data.Metodo.PagamentiClienti(this.context);
//        }

//        #endregion

//        #region Read

//        /// <summary>
//        /// Restituisce tutte le entities
//        /// </summary>
//        /// <returns></returns>
//        public IQueryable<Entities.ANAGRAFICARISERVATICF> Read()
//        {
//            return dal.Read();
//        }

//        /// <summary>
//        /// Restituisce l'entity in base all'esercizio e al codice del cliente o fornitore passati come parametro
//        /// </summary>
//        /// <param name="codice"></param>
//        /// <param name="codconto"></param>
//        /// <returns></returns>
//        public Entities.ANAGRAFICARISERVATICF Find(int codice, string codconto)
//        {
//            return dal.Find(codice, codconto);
//        }

//        /// <summary>
//        /// Applica alla query passata come parametro, l'ordinamento di default
//        /// </summary>
//        /// <param name="query"></param>
//        /// <returns></returns>
//        public IQueryable<Entities.ANAGRAFICARISERVATICF> ApplyDefaultOrder(IQueryable<Entities.ANAGRAFICARISERVATICF> query)
//        {
//            return query.OrderBy(x => x.TABPAGAMENTI.DESCRIZIONE);
//        }

//        #endregion
//    }
//}
