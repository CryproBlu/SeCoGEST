using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Logic.Metodo
{
    public class BancheClienti : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.Metodo.BancheClienti dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public BancheClienti()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public BancheClienti(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public BancheClienti(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.Metodo.BancheClienti(this.context);
        }

        #endregion

        #region Read

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.BANCAAPPCF> Read()
        {
            return dal.Read();
        }

        /// <summary>
        /// Restituisce tutte le entity in base al codice del cliente o fornitore passato
        /// </summary>
        /// <param name="codconto"></param>
        /// <returns></returns>
        public IQueryable<Entities.BANCAAPPCF> Read(string codconto)
        {
            return Read().Where(x => x.CODCONTO.ToLower().Trim() == codconto.ToLower().Trim());
        }

        /// <summary>
        /// Restituisce l'entity in base al codice della banca e al codice del cliente o fornitore
        /// </summary>
        /// <param name="codice"></param>
        /// <param name="codconto"></param>
        /// <returns></returns>
        public Entities.BANCAAPPCF Find(int codice, string codconto)
        {
            return dal.Find(codice, codconto);
        }

        /// <summary>
        /// Applica l'ordinamento dei default alla query passata come parametro
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.BANCAAPPCF> ApplyDefaultOrder(IQueryable<Entities.BANCAAPPCF> query)
        {
            return query.OrderBy(x => x.BANCAAPPOGGIO);
        }

        #endregion
    }
}
