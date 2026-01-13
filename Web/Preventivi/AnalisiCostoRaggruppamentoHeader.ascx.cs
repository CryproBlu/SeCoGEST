using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SeCoGEST.Web.Preventivi
{
    public partial class AnalisiCostoRaggruppamentoHeader : System.Web.UI.UserControl
    {
        public string GetDenominazioneGruppo()
        {
            return txtDenominazione.Text;
        }

        protected void ibNuovoGruppo_Command(object sender, CommandEventArgs e)
        {
            Guid idGruppo = Guid.Parse(e.CommandArgument.ToString());
            Logic.AnalisiCostiRaggruppamenti llGruppi = new Logic.AnalisiCostiRaggruppamenti();

            Entities.AnalisiCostoRaggruppamento gruppoAttuale = llGruppi.Find(new Entities.EntityId<Entities.AnalisiCostoRaggruppamento>(idGruppo));

            Entities.AnalisiCostoRaggruppamento nuovoGruppo = new Entities.AnalisiCostoRaggruppamento();
            nuovoGruppo.IDAnalisiCosto = gruppoAttuale.IDAnalisiCosto;
            nuovoGruppo.Denominazione = "Nuovo gruppo";
            llGruppi.Create(nuovoGruppo, true);
        }

        protected void ibEliminaGruppo_Command(object sender, CommandEventArgs e)
        {
            Guid idGruppo = Guid.Parse(e.CommandArgument.ToString());

            Logic.AnalisiCostiRaggruppamenti llGruppi = new Logic.AnalisiCostiRaggruppamenti();
            Entities.AnalisiCostoRaggruppamento gruppoAttuale = llGruppi.Find(new Entities.EntityId<Entities.AnalisiCostoRaggruppamento>(idGruppo));
            if (gruppoAttuale != null)
            {
                llGruppi.Delete(gruppoAttuale, true);
            }
        }
    }
}