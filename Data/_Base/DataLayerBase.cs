using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Data.Base
{
    public abstract class DataLayerBase
    {
        /// <summary>
        /// DataContext utilizzato dal DataAccessLayer che eredità questa classe
        /// </summary>
        internal DatabaseDataContext context;



        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public DataLayerBase() : this(false) { }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public DataLayerBase(bool createStandaloneContext)
        {
            context = Base.DataAccess.GetDataContext(createStandaloneContext);
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il DataContext da utilizzare
        /// </summary>
        /// <param name="contextToUse"></param>
        public DataLayerBase(DatabaseDataContext contextToUse)
        {
            if (contextToUse == null) throw new ArgumentNullException("Il DataContext passato ha valore null!");
            this.context = contextToUse;
        }




        /// <summary>
        /// Memorizza nel database le modifiche applicate alle Entities del DataContext in uso
        /// </summary>
        public void SubmitToDatabase()
        {
            Database db = new Database(this.context);
            db.SubmitChanges();
        }
    }
}
