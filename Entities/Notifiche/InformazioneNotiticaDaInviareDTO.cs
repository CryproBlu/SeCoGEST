using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Entities
{
    public class InformazioneNotiticaDaInviareDTO
    {
        #region Properties

        public string InformazioneHTML { get; private set; }

        public string InformazionePlainText { get; private set; }

        #endregion

        #region Costruttori

        public InformazioneNotiticaDaInviareDTO(string informazioneHtml, string informazionePlaintText)
        {
            InizializzaProperties(informazioneHtml, informazionePlaintText);
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Effettua l'inizializzazione delle properties dell'istanza utilizzando i parametri passati al costruttore
        /// </summary>
        /// <param name="informazioneHtml"></param>
        /// <param name="informazionePlaintText"></param>
        private void InizializzaProperties(string informazioneHtml, string informazionePlaintText)
        {
            if (String.IsNullOrWhiteSpace(informazioneHtml)) informazioneHtml = String.Empty;
            if (String.IsNullOrWhiteSpace(informazionePlaintText)) informazionePlaintText = String.Empty;

            InformazioneHTML = informazioneHtml;
            InformazionePlainText = informazionePlaintText;
        }

        #endregion
    }
}
