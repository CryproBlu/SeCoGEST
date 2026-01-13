using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public partial class Intervento_Articolo
    {

        /// <summary>
        /// Restituisce l'ID utilizzato per poter effettuare ricerche nell'elenco delle Provenienze Articoli
        /// </summary>
        public string IDRicercaInProvenienzaArticoli
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
        /// Restituisce l'ID utilizzato per poter effettuare ricerche nell'ElencoCompletoArticoli
        /// </summary>
        public string IDRicercaInElencoCompletoArticoli
        {
            get
            {
                if (this.TipologiaArticolo == 0)
                {
                    return string.Concat(this.TipologiaArticolo, this.CodiceArticolo);
                }
                else
                {
                    if (this.Progressivo.HasValue)
                    {
                        return string.Concat(this.TipologiaArticolo, this.Progressivo.Value.ToString());
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
        }

        public string DescrizioneProvenienzaArticolo
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
                    return string.Format("{0}{1}", tipo, this.CodiceContratto);
                }
            }
        }

        public string Tempo_Quantita
        {
            get
            {
                string oreTimeString = this.OreTime;
                string quantitaString = (this.Quantita.HasValue ? this.Quantita.Value.ToString() : String.Empty);

                List<string> stringhe = new List<string>();
                if (!String.IsNullOrEmpty(oreTimeString))
                {
                    stringhe.Add(oreTimeString);
                }

                if (!String.IsNullOrEmpty(quantitaString))
                {
                    stringhe.Add(quantitaString);
                }

                return String.Join("/", stringhe);
            }
        }
    }
}
