using SeCoGEST.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Logic
{
    public class Intervento_ConfigurazioniTipologieTicketCliente : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.Intervento_ConfigurazioniTipologieTicketCliente dalConfigurazioniTipologieTicketCliente;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public Intervento_ConfigurazioniTipologieTicketCliente()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public Intervento_ConfigurazioniTipologieTicketCliente(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public Intervento_ConfigurazioniTipologieTicketCliente(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dalConfigurazioniTipologieTicketCliente = new Data.Intervento_ConfigurazioniTipologieTicketCliente(this.context);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Intervento_ConfigurazioneTipologiaTicketCliente entityToCreate, bool submitChanges)
        {
            if (entityToCreate != null)
            {
                if (entityToCreate.Id.Equals(Guid.Empty))
                {
                    entityToCreate.Id = Guid.NewGuid();
                }

                // Salvataggio nel database
                dalConfigurazioniTipologieTicketCliente.Create(entityToCreate, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante la creazione dell'entity 'Intervento_ConfigurazioneTipologiaTicketCliente': parametro nullo!");
            }
        }

        /// <summary>
        /// Restituisce tutte le entities
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Intervento_ConfigurazioneTipologiaTicketCliente> Read()
        {
            return from u in dalConfigurazioniTipologieTicketCliente.Read() select u;
        }

        /// <summary>
        /// Restituisce tutte le configurazioni memorizzate per cliente
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Intervento_ConfigurazioneTipologiaTicketCliente> Read(string codiceCliente)
        {
            return from u in dalConfigurazioniTipologieTicketCliente.Read() where u.CodiceCliente == codiceCliente select u;
        }

        /// <summary>
        /// Restituisce tutte le configurazioni memorizzate per cliente e valide fino alla data passata
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Intervento_ConfigurazioneTipologiaTicketCliente> Read(string codiceCliente, DateTime dataTicket)
        {
            return from u in dalConfigurazioniTipologieTicketCliente.Read() where u.CodiceCliente == codiceCliente && (u.ScadenzaContratto == null || u.ScadenzaContratto > dataTicket) select u;
        }

        /// <summary>
        /// Restituisce tutte le tipolgoe attive per cliente
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Entities.Intervento_ConfigurazioneTipologiaTicketCliente> ReadAttivePerCliente(string codiceCliente, DateTime? dataIntervento)
        {
            if (!dataIntervento.HasValue) dataIntervento = DateTime.Now;

            Interventi ll = new Interventi();
            IEnumerable<InformazioniContratto> elencoInfoContratti = ll.GetContrattiPerCliente(codiceCliente, dataIntervento.Value);
            IEnumerable<string> elencoContrattiAttiviCliente = elencoInfoContratti.Select(c => c.CodiceContratto).Distinct();

            Intervento_ConfigurazioniTipologieTicketCliente llConf = new Intervento_ConfigurazioniTipologieTicketCliente();
            IQueryable<Intervento_ConfigurazioneTipologiaTicketCliente> configurazioniCliente_Attive = llConf.Read(codiceCliente, dataIntervento.Value);

            IEnumerable<Intervento_ConfigurazioneTipologiaTicketCliente> configurazioniCliente_ConScedenzaCustom = configurazioniCliente_Attive.Where(x => x.ScadenzaContratto != null);
            IEnumerable<Intervento_ConfigurazioneTipologiaTicketCliente> configurazioniCliente_SenzaScedenzaCustom = configurazioniCliente_Attive.Where(x => x.ScadenzaContratto == null);

            IEnumerable<Intervento_ConfigurazioneTipologiaTicketCliente> tipologieApplicabili = configurazioniCliente_SenzaScedenzaCustom.Where(c => elencoContrattiAttiviCliente.Contains(c.CodiceContratto));

            tipologieApplicabili = tipologieApplicabili.Union(configurazioniCliente_ConScedenzaCustom).Distinct();

            return tipologieApplicabili;
        }



        /// <summary>
        /// Restituisce l'entity relativa all'ID passato e null se non trova l'entity
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.Intervento_ConfigurazioneTipologiaTicketCliente Find(EntityId<Intervento_ConfigurazioneTipologiaTicketCliente> idToFind)
        {
            return dalConfigurazioniTipologieTicketCliente.Find(idToFind.Value);
        }


        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Intervento_ConfigurazioneTipologiaTicketCliente entityToDelete, bool submitChanges)
        {
            Delete(entityToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Intervento_ConfigurazioneTipologiaTicketCliente entityToDelete, bool checkAllegati, bool submitChanges)
        {
            if (entityToDelete != null)
            {
                dalConfigurazioniTipologieTicketCliente.Delete(entityToDelete, submitChanges);
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione dell'entity 'Intervento_ConfigurazioneTipologiaTicketCliente': parametro nullo!");
            }
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.Intervento_ConfigurazioneTipologiaTicketCliente> entitiesToDelete, bool submitChanges)
        {
            Delete(entitiesToDelete, false, submitChanges);
        }

        /// <summary>
        /// Elimina le entity passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="checkAllegati"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.Intervento_ConfigurazioneTipologiaTicketCliente> entitiesToDelete, bool checkAllegati, bool submitChanges)
        {
            if (entitiesToDelete != null)
            {
                if (entitiesToDelete.Count() > 0)
                {
                    entitiesToDelete.ToList().ForEach(x => Delete(x, checkAllegati, submitChanges));
                }
            }
            else
            {
                throw new ArgumentNullException("Errore durante l'eliminazione delle entities 'Intervento_ConfigurazioneTipologiaTicketCliente': parametro nullo!");
            }
        }

        #endregion

    }
}
