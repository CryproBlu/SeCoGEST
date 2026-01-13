using System.Linq;

namespace SeCoGEST.Data.Metodo
{
    public class AnagraficheClienti : Base.DataLayerBase
    {
        #region Costruttori

        public AnagraficheClienti() : base(false) { }
        public AnagraficheClienti(bool createStandaloneContext) : base(createStandaloneContext) { }
        public AnagraficheClienti(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region READ


        /// <summary>
        /// Restituisce tutte le entity
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.AnagraficaClienti> Read()
        {
            return context.AnagraficaClientis;
        }


        /// <summary>
        /// Restituisce l'entity in base all'ID passato
        /// </summary>
        /// <param name="idToFind"></param>
        /// <returns></returns>
        public Entities.AnagraficaClienti Find(string codconto)
        {
            return Read().Where(x => x.CODCONTO == codconto).SingleOrDefault();
        }

        #endregion
    }
}
