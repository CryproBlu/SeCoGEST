using System;
using System.Web.UI.WebControls;

namespace SeCoGEST.Web.UI
{
    public class DocumentoAllegatoInItemDataBoundEventArgs : EventArgs
    {
        public Entities.Allegato DocumentoAllegato { get; private set; }

        public ImageButton TastoElimina { get; private set; }

        public DocumentoAllegatoInItemDataBoundEventArgs(Entities.Allegato documentoAllegato, ImageButton tastoElimina)
        {
            this.DocumentoAllegato = documentoAllegato;
            this.TastoElimina = tastoElimina;
        }
    }
}