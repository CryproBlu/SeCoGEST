using System;
using System.Linq;

namespace SeCoGEST.Logic.Metodo
{
    public class Archivi : Base.LogicLayerBase
    {
        #region Costruttori e DAL interno

        /// <summary>
        /// Data utilizzato da tutte le operazioni su database fatte tramite questa istanza
        /// </summary>
        private Data.Metodo.Archivi dal;

        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public Archivi()
            : base(false)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public Archivi(bool createStandaloneContext)
            : base(createStandaloneContext)
        {
            CreateDalAndLogic();
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public Archivi(Base.LogicLayerBase logicLayer)
            : base(logicLayer)
        {
            CreateDalAndLogic();
        }



        /// <summary>
        /// Crea un Data che utilizza il DataContext specificato nella classe base LogicLayerBase
        /// </summary>
        private void CreateDalAndLogic()
        {
            dal = new Data.Metodo.Archivi(this.context);
        }

        #endregion

        #region Read

        /// <summary>
        /// Restituisce tutte i Gruppi
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.TABGRUPPI> Read_Gruppi()
        {
            return from u in dal.Read_Gruppi() orderby u.CODICE, u.DESCRIZIONE select u;
        }

        /// <summary>
        /// Restituisce tutte le Categorie
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.TABCATEGORIE> Read_Categorie()
        {
            return from u in dal.Read_Categorie() orderby u.CODICE, u.DESCRIZIONE select u;
        }

        /// <summary>
        /// Restituisce tutte le CategorieStatistiche
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.TABCATEGORIESTAT> Read_CategorieStatistiche()
        {
            return from u in dal.Read_CategorieStatistiche() orderby u.CODICE, u.DESCRIZIONE select u;
        }

        #endregion

        #region Find

        /// <summary>
        /// Restituisce l'entity relativa al Codice passato
        /// </summary>
        /// <param name="codice"></param>
        /// <returns></returns>
        public Entities.TABGRUPPI Find_Gruppo(decimal codice)
        {
            return dal.Find_Gruppo(codice);
        }

        /// <summary>
        /// Restituisce l'entity relativa al Codice passato
        /// </summary>
        /// <param name="codice"></param>
        /// <returns></returns>
        public Entities.TABCATEGORIE Find_Categoria(decimal codice)
        {
            return dal.Find_Categoria(codice);
        }

        /// <summary>
        /// Restituisce l'entity relativa al Codice passato
        /// </summary>
        /// <param name="codice"></param>
        /// <returns></returns>
        public Entities.TABCATEGORIESTAT Find_CategoriaStatistica(decimal codice)
        {
            return dal.Find_CategoriaStatistica(codice);
        }
        
        #endregion
    }
}
