using System;
using System.Linq;

namespace SeCoGEST.Data.Metodo
{
    public class ElenchiCompletiArticoli : Base.DataLayerBase
    {
        #region Costruttori

        public ElenchiCompletiArticoli() : base(false) { }
        public ElenchiCompletiArticoli(bool createStandaloneContext) : base(createStandaloneContext) { }
        public ElenchiCompletiArticoli(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region READ


        ///// <summary>
        ///// Restituisce tutte le entity
        ///// </summary>
        ///// <returns></returns>
        //public IQueryable<Entities.ElencoCompletoArticoli> Read()
        //{
        //    return context.ElencoCompletoArticolis;
        //}

        /// <summary>
        /// Restituisce tutti gli articoli attivi nella data indicata
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.ElencoCompletoArticoli> Read(DateTime dataValidita)
        {
            return context.GetElencoCompletoArticoli(dataValidita);
        }

        ///// <summary>
        ///// Restituisce l'entity in base all'ID passato
        ///// </summary>
        ///// <param name="idToFind"></param>
        ///// <returns></returns>
        //public Entities.ElencoCompletoArticoli Find(string codice)
        //{
        //    return this.Read().Where(x => x.CodiceArticolo == codice).SingleOrDefault();
        //}

        /// <summary>
        /// Restituisce l'entity in base all'ID passato cercandola fra quelli attivi nella data passata
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.ElencoCompletoArticoli Find(string codice, DateTime dataValidita)
        {
            return this.Read(dataValidita).Where(x => x.CodiceArticolo == codice).SingleOrDefault();
        }
        #endregion

        /// <summary>
        /// Restituisce l'elenco di Unità di Misura associate all'articolo
        /// </summary>
        /// <param name="codiceArticolo"></param>
        /// <returns></returns>
        public IQueryable<Entities.ARTICOLIUNITAMISURA> UnitaMisuraPerArticolo(string codiceArticolo)
        {
            return context.ARTICOLIUNITAMISURAs.Where(x => x.CODART == codiceArticolo);
        }
    }
}
