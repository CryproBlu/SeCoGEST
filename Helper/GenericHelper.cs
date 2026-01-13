using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Helper
{
    public static class GenericHelper
    {
        /// <summary>
        /// Calcola la percentuale di variazione di due numeri
        /// </summary>
        /// <param name="valoreIniziale"></param>
        /// <param name="valoreFinale"></param>
        /// <returns></returns>
        public static decimal GetPercentualeVariazione(decimal? valoreIniziale, decimal? valoreFinale)
        {
            // http://support.azerouno.it/support/solutions/articles/5000644841-che-differenza-c-%C3%A8-tra-margine-e-ricarico-
            if (!valoreIniziale.HasValue) valoreIniziale = 0;
            if (!valoreFinale.HasValue) valoreFinale = 0;

            decimal incremento = valoreFinale.Value - valoreIniziale.Value;
            decimal aumento = 0;

            if (valoreFinale.Value > 0)
            {
                aumento = (incremento / valoreIniziale.Value) * 100;
            }

            return aumento;
        }
    }
}
