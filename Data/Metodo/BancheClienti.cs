using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Data.Metodo
{
    public class BancheClienti : Base.DataLayerBase
    {
        #region Costruttori

        public BancheClienti() : base(false) { }
        public BancheClienti(bool createStandaloneContext) : base(createStandaloneContext) { }
        public BancheClienti(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region READ


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.BANCAAPPCF> Read()
        {
            return context.BANCAAPPCFs;
        }

        /// <summary>
        /// Restituisce l'entity in base al codice della banca e al codice del cliente o fornitore
        /// </summary>
        /// <param name="codice"></param>
        /// <param name="codconto"></param>
        /// <returns></returns>
        public Entities.BANCAAPPCF Find(int codice, string codconto)
        {
            return Read().Where(x => x.CODCONTO.ToLower().Trim() == codconto.ToLower().Trim() && x.CODICE == codice).SingleOrDefault();
        }

        #endregion
    }
}
