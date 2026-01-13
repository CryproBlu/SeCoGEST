using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class Intervento_ConfigurazioniTipologieTicketCliente : Base.DataLayerBase
    {
        #region Costruttori

        public Intervento_ConfigurazioniTipologieTicketCliente() : base(false) { }
        public Intervento_ConfigurazioniTipologieTicketCliente(bool createStandaloneContext) : base(createStandaloneContext) { }
        public Intervento_ConfigurazioniTipologieTicketCliente(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.Intervento_ConfigurazioneTipologiaTicketCliente entityToCreate, bool submitChanges)
        {
            context.Intervento_ConfigurazioneTipologiaTicketClientes.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.Intervento_ConfigurazioneTipologiaTicketCliente> Read()
        {
            return context.Intervento_ConfigurazioneTipologiaTicketClientes;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.Intervento_ConfigurazioneTipologiaTicketCliente Find(Guid id)
        {
            return Read().Where(x => x.Id == id).SingleOrDefault();
        }


        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.Intervento_ConfigurazioneTipologiaTicketCliente entityToDelete, bool submitChanges)
        {
            context.Intervento_ConfigurazioneTipologiaTicketClientes.DeleteOnSubmit(entityToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Elimina le entities passate
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(IEnumerable<Entities.Intervento_ConfigurazioneTipologiaTicketCliente> entitiesToDelete, bool submitChanges)
        {
            context.Intervento_ConfigurazioneTipologiaTicketClientes.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}