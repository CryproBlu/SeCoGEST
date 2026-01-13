using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Data
{
    public class VociPredefiniteInterventi : Base.DataLayerBase
    {
        #region Costruttori

        public VociPredefiniteInterventi() : base(false) { }
        public VociPredefiniteInterventi(bool createStandaloneContext) : base(createStandaloneContext) { }
        public VociPredefiniteInterventi(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.VocePredefinitaIntervento> Read()
        {
            return context.VocePredefinitaInterventos;
        }
    }
}