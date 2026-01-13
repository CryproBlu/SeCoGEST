using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public partial class OffertaArticolo
    {
        public decimal? TotaleRicarico
        {
            get
            {
                decimal? valore = null;
                if (Quantita.HasValue)
                {
                    if (RicaricoValuta.HasValue)
                    {
                        valore = Quantita.Value * RicaricoValuta.Value;
                    }
                    else if (TotaleCosto.HasValue && TotaleVendita.HasValue)
                    {
                        valore = TotaleVendita.Value - TotaleCosto.Value;
                    }
                }

                return valore;
            }
        }

        public decimal? TotaleSpesaSottoArticoli
        {
            get
            {
                decimal? totale = 0;

                if (this.OffertaArticoloSpeses != null && this.OffertaArticoloSpeses.Count > 0)
                {
                    foreach(OffertaArticolo sottoArticolo in this.OffertaArticoloSpeses)
                    {
                        if (sottoArticolo.TotaleVendita.HasValue)
                        {
                            if (!totale.HasValue) totale = 0;
                            totale += sottoArticolo.TotaleVendita.Value;
                        }
                    }
                }

                return totale;
            }
        }
    }
}
