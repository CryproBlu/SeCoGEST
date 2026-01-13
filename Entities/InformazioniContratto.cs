using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public partial class InformazioniContratto
    {
        /// <summary>
        /// Restituisce un ID univoco composto dall'unione dei campi TipologiaArticolo e CodiceContratto
        /// </summary>
        public string IDUnivoco
        {
            get
            {
                if (this.TipologiaArticolo == 0)
                {
                    return "0";
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(this.CodiceContratto))
                    {
                        return string.Concat(this.TipologiaArticolo, this.CodiceContratto.ToString());
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// Restituisce una stringa che rappresenta la descrizione della provenienza del gruppo di articoli
        /// </summary>
        public string DescrizioneProvenienza
        {
            get
            {
                if (this.TipologiaArticolo == 0)
                {
                    return "Articoli di Magazzino";
                }
                else
                {

                    string tipo = string.Empty;
                    switch (this.TipologiaArticolo)
                    {
                        case 1: tipo = "Articoli del Contratto N.";
                            break;
                        case 2: tipo = "Articoli Prepagati del Contratto N.";
                            break;
                        case 3: tipo = "Tariffe Standard Contratto N.";
                            break;
                        case 4: tipo = "Addebiti Contratto N.";
                            break;
                        default: tipo = "";
                            break;
                    }
                    return string.Format("{0}{1} - {2}", tipo, this.CodiceContratto, this.DescrizioneContratto);
                }
            }
        }


    }
}
