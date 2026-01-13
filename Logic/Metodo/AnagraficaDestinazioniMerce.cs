using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Logic.Metodo
{
    public class AnagraficaDestinazioniMerce: Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.Metodo.DestinazioniDiverse dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public AnagraficaDestinazioniMerce()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public AnagraficaDestinazioniMerce(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public AnagraficaDestinazioniMerce(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.Metodo.DestinazioniDiverse(this.context);
        }

        #endregion


        /// <summary>
        /// Restituisce tutte le Destinazioni relative al Cliente passato
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Entities.AnagraficaDestinazioneMerce> Read(string codiceCliente)
        {
            return Read(codiceCliente, true);
            //List<Entities.AnagraficaDestinazioneMerce> destinazioni = new List<Entities.AnagraficaDestinazioneMerce>();

            //Data.Metodo.AnagraficheClienti dalClienti = new Data.Metodo.AnagraficheClienti(this.context);
            //Entities.AnagraficaClienti cliente = dalClienti.Find(codiceCliente);
            //if (cliente == null) return destinazioni;

            //Entities.AnagraficaDestinazioneMerce destinazionePrincipale = new Entities.AnagraficaDestinazioneMerce(cliente);
            //IEnumerable<Entities.AnagraficaDestinazioneMerce> destinazioniDiverse = (from u in dal.Read(codiceCliente) select new Entities.AnagraficaDestinazioneMerce(u));

            //if (destinazionePrincipale != null) destinazioni.Add(destinazionePrincipale);
            //destinazioni = destinazioni.Concat(destinazioniDiverse).ToList();

            //return destinazioni;
        }

        /// <summary>
        /// Restituisce tutte le Destinazioni relative al Cliente passato
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Entities.AnagraficaDestinazioneMerce> Read(string codiceCliente, bool includiPrincipale)
        {
            List<Entities.AnagraficaDestinazioneMerce> destinazioni = new List<Entities.AnagraficaDestinazioneMerce>();

            Data.Metodo.AnagraficheClienti dalClienti = new Data.Metodo.AnagraficheClienti(this.context);
            Entities.AnagraficaClienti cliente = dalClienti.Find(codiceCliente);
            if (cliente == null) return destinazioni;

            if (includiPrincipale)
            {
                Entities.AnagraficaDestinazioneMerce destinazionePrincipale = new Entities.AnagraficaDestinazioneMerce(cliente);
                if (destinazionePrincipale != null) destinazioni.Add(destinazionePrincipale);
            }

            IEnumerable<Entities.AnagraficaDestinazioneMerce> destinazioniDiverse = (from u in dal.Read(codiceCliente) select new Entities.AnagraficaDestinazioneMerce(u));
            destinazioni = destinazioni.Concat(destinazioniDiverse).ToList();

            return destinazioni;
        }

    }
}
