using SeCoGEST.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Logic
{
    public class AnagraficaSoggettiProprieta : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.AnagraficaSoggettiProprieta dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public AnagraficaSoggettiProprieta()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public AnagraficaSoggettiProprieta(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public AnagraficaSoggettiProprieta(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.AnagraficaSoggettiProprieta(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.AnagraficaSoggetti_Proprieta entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                // Salvataggio nel database
                dal.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'AnagraficaSoggetti_Proprieta': parametro nullo!");
            }
        }

        public IQueryable<AnagraficaSoggetti_Proprieta> Read()
        {
            return dal.Read();
        }

        public IQueryable<AnagraficaSoggetti_Proprieta> ReadByProperty(Entities.AnagraficaSoggetti_ProprietaEnums property)
        {
            return dal.Read().Where(c => c.Proprietà == property.ToString());
        }

        /// <summary>
        /// Restituisce tutte le proprietà del soggetto
        /// </summary>
        /// <returns></returns>
        public IQueryable<AnagraficaSoggetti_Proprieta> Read(string codiceCliente)
        {
            return dal.Read().Where(c => c.CodiceCliente == codiceCliente);
        }

        /// <summary>
        /// Restituisce l'entity relativa ai parametri passati
        /// </summary>
        /// <param name="codiceCliente"></param>
        /// <param name="proprietà"></param>
        /// <returns></returns>
        public Entities.AnagraficaSoggetti_Proprieta Find(string codiceCliente, string proprietà)
        {
            return dal.Find(codiceCliente, proprietà);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(AnagraficaSoggetti_Proprieta entityToDelete, bool submitChanges)
        {
            Delete(entityToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.AnagraficaSoggetti_Proprieta entityToDelete, bool checkAllegati, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dal.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'AnagraficaSoggetti_Proprieta': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.AnagraficaSoggetti_Proprieta> entitiesToDelete, bool submitChanges)
        {
            dal.Delete(entitiesToDelete, submitChanges);
        }

        #endregion


        public static List<string> ReadElencoProprieta()
        {
            return SeCoGes.Utilities.EnumHelper.GetValoriEnumeratore<Entities.AnagraficaSoggetti_ProprietaEnums>();
            //return Enum.GetValues(typeof(Entities.AnagraficaSoggetti_ProprietaEnums));
        }
    }
}
