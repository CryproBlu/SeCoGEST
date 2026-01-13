using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Data.Metodo
{
    public class Pagamenti : Base.DataLayerBase
    {
        #region Costruttori

        public Pagamenti() : base(false) { }
        public Pagamenti(bool createStandaloneContext) : base(createStandaloneContext) { }
        public Pagamenti(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region READ


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.TABPAGAMENTI> Read()
        {
            return context.TABPAGAMENTIs;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="codicePagamento"></param>
        /// <returns></returns>
        public Entities.TABPAGAMENTI Find(string codicePagamento)
        {
            return Read().Where(x => x.CODICE.ToLower().Trim() == codicePagamento.ToLower().Trim()).SingleOrDefault();
        }

        #endregion
    }
}
