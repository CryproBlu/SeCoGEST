using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class ModelliConfigurazioneCaratteristicheTicketCliente : Base.DataLayerBase
    {
        #region Costruttori

        public ModelliConfigurazioneCaratteristicheTicketCliente() : base(false) { }
        public ModelliConfigurazioneCaratteristicheTicketCliente(bool createStandaloneContext) : base(createStandaloneContext) { }
        public ModelliConfigurazioneCaratteristicheTicketCliente(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.ModelloConfigurazioneCaratteristicheTicketCliente entityToCreate, bool submitChanges)
        {
            context.ModelloConfigurazioneCaratteristicheTicketClientes.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.ModelloConfigurazioneCaratteristicheTicketCliente> Read()
        {
            return context.ModelloConfigurazioneCaratteristicheTicketClientes;
        }

        /// <summary>
        /// Restituisce tutte le entity del modello passato
        /// </summary>
        /// <param name="idModello"></param>
        /// <returns></returns>
        public IQueryable<Entities.ModelloConfigurazioneCaratteristicheTicketCliente> Read(Guid idModello)
        {
            return Read().Where(x => x.IdModelloConfigurazioneTicketCliente == idModello);
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.ModelloConfigurazioneCaratteristicheTicketCliente entityToDelete, bool submitChanges)
        {
            context.ModelloConfigurazioneCaratteristicheTicketClientes.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.ModelloConfigurazioneCaratteristicheTicketCliente> entitiesToDelete, bool submitChanges)
        {
            context.ModelloConfigurazioneCaratteristicheTicketClientes.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}