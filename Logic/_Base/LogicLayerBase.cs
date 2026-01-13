using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Logic.Base
{
    public abstract class LogicLayerBase
    {
        /// <summary>
        /// DataContext utilizzato dal LogicLayer che eredità questa classe
        /// </summary>
        internal Data.Base.DatabaseDataContext context;


        /// <summary>
        /// Crea l'istanza della classe utilizzando il DataContext globale condiviso
        /// </summary>
        public LogicLayerBase() : this(false) { }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare se utilizzare un DataContext privato o quello globale condiviso
        /// </summary>
        /// <param name="createStandaloneContext"></param>
        public LogicLayerBase(bool createStandaloneContext)
        {
            context = Data.Base.DataAccess.GetDataContext(createStandaloneContext);
        }

        /// <summary>
        /// Crea l'istanza della classe permettendo al chiamante di specificare il LogicLayer collegato.
        /// L'istanza creata utilizzerà il DataContext interno al LogicLayer passato per effettuare le operazioni sulla base dati
        /// </summary>
        /// <param name="logicLayer"></param>
        public LogicLayerBase(LogicLayerBase logicLayer)
        {
            if (logicLayer == null) throw new ArgumentNullException("Il LogicLayer passato ha valore null!");
            this.context = logicLayer.context;
        }



        /// <summary>
        /// Memorizza nel database le modifiche applicate alle Entities del DataContext in uso
        /// </summary>
        public void SubmitToDatabase()
        {
            Data.Base.Database db = new Data.Base.Database(this.context);
            db.SubmitChanges();
        }

        public void StartTransaction()
        {
            if (this.context.Connection.State == System.Data.ConnectionState.Closed) this.context.Connection.Open();
            this.context.Transaction = this.context.Connection.BeginTransaction();
        }
        public void RollbackTransaction()
        {
            this.context.Transaction.Rollback();
        }
        public void CommitTransaction()
        {
            this.context.Transaction.Commit();
        }
    }
}
