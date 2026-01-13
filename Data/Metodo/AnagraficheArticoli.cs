using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Data.Metodo
{
    public class AnagraficheArticoli : Base.DataLayerBase
    {
        #region Costruttori

        public AnagraficheArticoli() : base(false) { }
        public AnagraficheArticoli(bool createStandaloneContext) : base(createStandaloneContext) { }
        public AnagraficheArticoli(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region READ


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.ANAGRAFICAARTICOLI> Read()
        {
            return context.ANAGRAFICAARTICOLIs;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.ANAGRAFICAARTICOLI Find(string codconto)
        {
            return Read().Where(x => x.CODICE == codconto).SingleOrDefault();
        }

        #endregion
    }
}
