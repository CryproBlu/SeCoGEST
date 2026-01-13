using System;
using System.Collections.Generic;
using System.Linq;

namespace SeCoGEST.Data
{
    public class AnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie : Base.DataLayerBase
    {
        #region Costruttori

        public AnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie() : base(false) { }
        public AnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie(bool createStandaloneContext) : base(createStandaloneContext) { }
        public AnalisiVenditeConfigurazioneArticoliAggiuntiviClausoleVessatorie(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region CRUD

        /// <summary>
        /// Aggiunge una nuova entity
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <param name="submitChanges"></param>
        public void Create(Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria entityToCreate, bool submitChanges)
        {
            context.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatorias.InsertOnSubmit(entityToCreate);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria> Read()
        {
            return context.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatorias;
        }

        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="idAnalisiVenditaConfigurazioneArticoloAggiuntivo"></param>
        /// <param name="idClausolaVessatoria"></param>
        /// <returns></returns>
        public Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria Find(Guid idAnalisiVenditaConfigurazioneArticoloAggiuntivo, Guid idClausolaVessatoria)
        {
            return Read().Where(x => x.IDAnalisiVenditaConfigurazioneArticoloAggiuntivo == idAnalisiVenditaConfigurazioneArticoloAggiuntivo && x.IDClausolaVessatoria == idClausolaVessatoria).SingleOrDefault();
        }

        /// <summary>
        /// Elimina l'entity passata
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <param name="submitChanges"></param>
        public void Delete(Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria entityToDelete, bool submitChanges)
        {
            context.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatorias.DeleteOnSubmit(entityToDelete);
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
        public void Delete(IEnumerable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria> entitiesToDelete, bool submitChanges)
        {
            context.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatorias.DeleteAllOnSubmit(entitiesToDelete);
            if (submitChanges == true)
            {
                SubmitToDatabase();
            }
        }

        #endregion

        #region Custom

        /// <summary>
        /// Restituisce tutte le entity associate all'entità AnalisiVenditaConfigurazioneArticoloAggiuntivo
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria> Read(Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivo analisiVenditaConfigurazioneArticoloAggiuntivo)
        {
            return Read().Where(x => x.IDAnalisiVenditaConfigurazioneArticoloAggiuntivo == analisiVenditaConfigurazioneArticoloAggiuntivo.ID);
        }

        /// <summary>
        /// Restituisce tutte le entity associate all'entità CondizioneParticolare
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnalisiVenditaConfigurazioneArticoloAggiuntivoClausolaVessatoria> Read(Entities.ClausolaVessatoria clausolaVessatoria)
        {
            return Read().Where(x => x.IDClausolaVessatoria == clausolaVessatoria.ID);
        }

        #endregion
    }
}