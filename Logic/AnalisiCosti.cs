using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeCoGEST.Entities;
using SeCoGEST.Helper;
//using msWord = Microsoft.Office.Interop.Word;

namespace SeCoGEST.Logic
{
    public class AnalisiCosti : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.AnalisiCosti dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public AnalisiCosti()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public AnalisiCosti(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public AnalisiCosti(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.AnalisiCosti(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.AnalisiCosto entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                if (entityToCreate.ID.Equals(Guid.Empty))
                {
                    entityToCreate.ID = Guid.NewGuid();
                    entityToCreate.Numero = GetNuovoNumero();
                }

                // Salvataggio nel database
                dal.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'AnalisiCosto': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiCosto> Read()
        {
            return from u in dal.Read() orderby u.Data descending select u;
        }

        /// <summary>
        /// Restituisce l'entity relativa all'IDentificativo passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.AnalisiCosto Find(EntityId<AnalisiCosto> identificativoAnalisiCosto)
        {
            return Find(identificativoAnalisiCosto.Value);
        }

        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        private Entities.AnalisiCosto Find(Guid idToFind)
        {
            return dal.Find(idToFind);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.AnalisiCosto entityToDelete, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'AnalisiCosto': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.AnalisiCosto> entitiesToDelete, bool submitChanges)
        {
            if (entitiesToDelete != null)
            {
                if (entitiesToDelete.Count() > 0)
                {
                    entitiesToDelete.ToList().ForEach(x => Delete(x, submitChanges));
                }
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'AnalisiCosto': parametro nullo!");
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Restituisce il numero da utilizzare per il prossimo nuovo bollettino
        /// </summary>
        /// <returns></returns>
        public int GetNuovoNumero()
        {
            int? max = dal.Read().Select(x => (int?)x.Numero).Max();
            if (max.HasValue)
                return max.Value + 1;
            else
                return 1;
        }

        /// <summary>
        /// Effettua il calcolo di tutti i totali dell'Analisi Costo e dei relativi Raggruppamenti articoli
        /// </summary>
        /// <param name="analisiCosto"></param>
        public void RicalcolaTotali(Guid idAnalisiCosto)
        {
            Entities.AnalisiCosto analisiCosto = Find(idAnalisiCosto);
            if (analisiCosto == null) return;

            analisiCosto.TotaleCosto = 0;
            analisiCosto.TotaleVendita = 0;
            analisiCosto.TotaleVenditaCalcolato = 0;
            analisiCosto.TotaleRicaricoValuta = 0;
            analisiCosto.TotaleRicaricoPercentuale = 0;

            Logic.AnalisiCostiRaggruppamenti llGruppi = new AnalisiCostiRaggruppamenti(this);

            // Esegue un ciclo per tutti i Raggruppamenti e ne ricalcola i totali
            foreach (AnalisiCostoRaggruppamento gruppo in llGruppi.Read(analisiCosto))
            {
                gruppo.TotaleCosto = gruppo.AnalisiCostoArticolos.Where(x => x.TotaleCosto.HasValue).Select(x => x.TotaleCosto.Value).Sum();
                gruppo.TotaleVendita = gruppo.AnalisiCostoArticolos.Where(x => x.TotaleVendita.HasValue).Select(x => x.TotaleVendita.Value).Sum();

                gruppo.TotaleVenditaCalcolato = gruppo.TotaleVendita;
                gruppo.TotaleRicaricoValuta = gruppo.TotaleVendita - gruppo.TotaleCosto;
                gruppo.TotaleRicaricoPercentuale = Math.Round(Helper.GenericHelper.GetPercentualeVariazione(gruppo.TotaleCosto, gruppo.TotaleVendita), 2);

                if (!gruppo.TotaleCosto.HasValue) gruppo.TotaleCosto = 0;
                if (!gruppo.TotaleVendita.HasValue) gruppo.TotaleVendita = 0;

                analisiCosto.TotaleCosto += gruppo.TotaleCosto;
                analisiCosto.TotaleVendita += gruppo.TotaleVendita;
            }

            analisiCosto.TotaleVenditaCalcolato = analisiCosto.TotaleVendita;
            analisiCosto.TotaleRicaricoValuta = analisiCosto.TotaleVendita - analisiCosto.TotaleCosto;
            analisiCosto.TotaleRicaricoPercentuale = Math.Round(Helper.GenericHelper.GetPercentualeVariazione(analisiCosto.TotaleCosto, analisiCosto.TotaleVendita), 2);

            SubmitToDatabase();
            //this.context.SubmitChanges();
        }

        #endregion
    }
}
