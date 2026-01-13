using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class ModelliConfigurazioneTicketCliente : Base.DataLayerBase
    {
        #region Costruttori

        public ModelliConfigurazioneTicketCliente() : base(false) { }
        public ModelliConfigurazioneTicketCliente(bool createStandaloneContext) : base(createStandaloneContext) { }
        public ModelliConfigurazioneTicketCliente(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.ModelloConfigurazioneTicketCliente entityToCreate, bool submitChanges)
        {
            context.ModelloConfigurazioneTicketClientes.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.ModelloConfigurazioneTicketCliente> Read()
        {
            return context.ModelloConfigurazioneTicketClientes;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.ModelloConfigurazioneTicketCliente Find(Guid id)
        {
            return Read().Where(x => x.Id == id).SingleOrDefault();
        }

        /// <summary>
        /// Restituisce l'entity in base al nome passato
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public Entities.ModelloConfigurazioneTicketCliente Find(string nome)
        {
            return Read().Where(x => x.Nome == nome).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.ModelloConfigurazioneTicketCliente entityToDelete, bool submitChanges)
        {
            context.ModelloConfigurazioneTicketClientes.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.ModelloConfigurazioneTicketCliente> entitiesToDelete, bool submitChanges)
        {
            context.ModelloConfigurazioneTicketClientes.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}