using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class ConfigurazioniTipologieTicketCliente : Base.DataLayerBase
    {
        #region Costruttori

        public ConfigurazioniTipologieTicketCliente() : base(false) { }
        public ConfigurazioniTipologieTicketCliente(bool createStandaloneContext) : base(createStandaloneContext) { }
        public ConfigurazioniTipologieTicketCliente(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.ConfigurazioneTipologiaTicketCliente entityToCreate, bool submitChanges)
        {
            context.ConfigurazioneTipologiaTicketClientes.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.ConfigurazioneTipologiaTicketCliente> Read()
        {
            return context.ConfigurazioneTipologiaTicketClientes;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.ConfigurazioneTipologiaTicketCliente Find(Guid id)
        {
            return Read().Where(x => x.Id == id).SingleOrDefault();
        }


        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.ConfigurazioneTipologiaTicketCliente entityToDelete, bool submitChanges)
        {
            context.ConfigurazioneTipologiaTicketClientes.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.ConfigurazioneTipologiaTicketCliente> entitiesToDelete, bool submitChanges)
        {
            context.ConfigurazioneTipologiaTicketClientes.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}