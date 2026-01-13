using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Logic.Metodo
{
    public class Pagamenti : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.Metodo.Pagamenti dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public Pagamenti()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public Pagamenti(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public Pagamenti(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.Metodo.Pagamenti(this.context);
        }

        #endregion

        #region Read

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.TABPAGAMENTI> Read(bool applyDefaultOrder = true)
        {
            IQueryable<Entities.TABPAGAMENTI> query = dal.Read();
            return (applyDefaultOrder) ? ApplyDefaultOrder(query) : query;
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="codicePagamento"></param>
        /// <returns></returns>
        public Entities.TABPAGAMENTI Find(string codicePagamento)
        {
            return dal.Find(codicePagamento);
        }

        /// <summary>
        /// Applica alla query passata come parametro, l'ordinamento di default
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryable<Entities.TABPAGAMENTI> ApplyDefaultOrder(IQueryable<Entities.TABPAGAMENTI> query)
        {
            return query.OrderBy(x => x.DESCRIZIONE);
        }

        #endregion
    }
}
