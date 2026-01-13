using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeCoGEST.Data.Metodo
{
    public class DestinazioniDiverse : Base.DataLayerBase
    {
        #region Costruttori

        public DestinazioniDiverse() : base(false) { }
        public DestinazioniDiverse(bool createStandaloneContext) : base(createStandaloneContext) { }
        public DestinazioniDiverse(Base.DatabaseDataContext contextToUse) : base(contextToUse) { }

        #endregion

        /// <summary>
        /// Restituisce tutte le Destinazioni relative al Cliente passato
        /// </summary>
        /// <returns></returns>
        public IQueryable<Entities.DESTINAZIONIDIVERSE> Read(string codiceCliente)
        {
            return context.DESTINAZIONIDIVERSEs.Where(x => x.CODCONTO == codiceCliente).OrderBy(x => x.CODICE);
        }

    }
}
