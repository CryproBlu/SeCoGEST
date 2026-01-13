//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SeCoGEST.Data.Metodo
//{
//    public class PagamentiClienti : Base.DataLayerBase
//    {
//        #region Costruttori

//        public PagamentiClienti() : base(false) { }
//        public PagamentiClienti(bool createStandaloneContext) : base(createStandaloneContext) { }
//        public PagamentiClienti(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

//        #endregion

//        #region READ


//        /// <summary>
//        /// Restituisce tutte le entity
//        /// </summary>
//        /// <returns></returns>
//        public IQueryable<Entities.ANAGRAFICARISERVATICF> Read()
//        {
//            return context.ANAGRAFICARISERVATICFs;
//        }

//        /// <summary>
//        /// Restituisce l'entity in base all'esercizio e al codice del cliente o fornitore passati come parametro
//        /// </summary>
//        /// <param name="esercizio"></param>
//        /// <param name="codconto"></param>
//        /// <returns></returns>
//        public Entities.ANAGRAFICARISERVATICF Find(int esercizio, string codconto)
//        {
//            return Read().Where(x => x.ESERCIZIO == esercizio && codconto.ToLower().Trim() == codconto.ToLower().Trim()).SingleOrDefault();
//        }

//        #endregion
//    }
//}
