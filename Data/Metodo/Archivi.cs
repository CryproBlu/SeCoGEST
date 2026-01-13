using System;
using System.Linq;

namespace SeCoGEST.Data.Metodo
{
    public class Archivi : Base.DataLayerBase
    {
        #region Costruttori

        public Archivi() : base(false) { }
        public Archivi(bool createStandaloneContext) : base(createStandaloneContext) { }
        public Archivi(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        #region READ

        /// <summary>
        /// Restituisce tutte le voci dell'archivio Gruppi
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.TABGRUPPI> Read_Gruppi()
        {
            return context.TABGRUPPIs;
        }

        /// <summary>
        /// Restituisce tutte le voci dell'archivio Categorie
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.TABCATEGORIE> Read_Categorie()
        {
            return context.TABCATEGORIEs;
        }

        /// <summary>
        /// Restituisce tutte le voci dell'archivio Categorie Statistiche
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.TABCATEGORIESTAT> Read_CategorieStatistiche()
        {
            return context.TABCATEGORIESTATs;
        }

        #endregion

        #region FIND

        /// <summary>
        /// Restituisce l'entity in base al Codice passato
        /// </summary>
        /// <param name="codice"></param>
        /// <returns></returns>
        public Entities.TABGRUPPI Find_Gruppo(decimal codice)
        {
            return this.Read_Gruppi().FirstOrDefault(x => x.CODICE == codice);
        }

        /// <summary>
        /// Restituisce l'entity in base al Codice passato
        /// </summary>
        /// <param name="codice"></param>
        /// <returns></returns>
        public Entities.TABCATEGORIE Find_Categoria(decimal codice)
        {
            return this.Read_Categorie().FirstOrDefault(x => x.CODICE == codice);
        }

        /// <summary>
        /// Restituisce l'entity in base al Codice passato
        /// </summary>
        /// <param name="codice"></param>
        /// <returns></returns>
        public Entities.TABCATEGORIESTAT Find_CategoriaStatistica(decimal codice)
        {
            return this.Read_CategorieStatistiche().FirstOrDefault(x => x.CODICE == codice);
        }


        #endregion
    }
}
