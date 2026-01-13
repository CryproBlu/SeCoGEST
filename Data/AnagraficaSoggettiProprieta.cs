using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class AnagraficaSoggettiProprieta : Base.DataLayerBase
    {
        #region Costruttori

        public AnagraficaSoggettiProprieta() : base(false) { }
        public AnagraficaSoggettiProprieta(bool createStandaloneContext) : base(createStandaloneContext) { }
        public AnagraficaSoggettiProprieta(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.AnagraficaSoggetti_Proprieta entityToCreate, bool submitChanges)
        {
            context.AnagraficaSoggetti_Proprietas.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnagraficaSoggetti_Proprieta> Read()
        {
            return context.AnagraficaSoggetti_Proprietas;
        }

        public IQueryable<Entities.AnagraficaSoggetti_Proprieta> ReadByCliente(string codiceCliente)
        {
            return context.AnagraficaSoggetti_Proprietas.Where(x => x.CodiceCliente == codiceCliente);
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entities.AnagraficaSoggetti_Proprieta Find(string codiceCliente, string proprietà)
        {
            return Read().Where(x => x.CodiceCliente == codiceCliente && x.Proprietà == proprietà).SingleOrDefault();
        }


        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.AnagraficaSoggetti_Proprieta entityToDelete, bool submitChanges)
        {
            context.AnagraficaSoggetti_Proprietas.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.AnagraficaSoggetti_Proprieta> entitiesToDelete, bool submitChanges)
        {
            context.AnagraficaSoggetti_Proprietas.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion
    }
}

