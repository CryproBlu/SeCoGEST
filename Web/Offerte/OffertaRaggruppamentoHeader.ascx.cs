using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SeCoGEST.Web.Offerte
{
    public partial class OffertaRaggruppamentoHeader : System.Web.UI.UserControl
    {
        #region Dichiarazione Eventi

        public event EventHandler GruppoAdded;
        public event EventHandler GruppoDeleted;

        #endregion

        #region Metodi Pubblici

        public string GetDenominazioneGruppo()
        {
            return txtDenominazione.Text;
        }

        public void SetEnabled(bool enabled)
        {
            ibNuovoGruppo.Visible = enabled;
            ibEliminaGruppo.Visible = enabled;
            txtDenominazione.ReadOnly = !enabled;
        }

        #endregion

        #region Intercettazione Eventi

        protected void ibNuovoGruppo_Command(object sender, CommandEventArgs e)
        {
            Guid idGruppo = Guid.Parse(e.CommandArgument.ToString());
            Logic.OfferteRaggruppamenti llGruppi = new Logic.OfferteRaggruppamenti();

            Entities.OffertaRaggruppamento gruppoAttuale = llGruppi.Find(new Entities.EntityId<Entities.OffertaRaggruppamento>(idGruppo));

            Entities.OffertaRaggruppamento nuovoGruppo = new Entities.OffertaRaggruppamento();
            nuovoGruppo.IDOfferta = gruppoAttuale.IDOfferta;
            nuovoGruppo.Denominazione = "Nuovo gruppo";
            llGruppi.Create(nuovoGruppo, true);

            RaiseGruppoAdded();
        }

        protected void ibEliminaGruppo_Command(object sender, CommandEventArgs e)
        {
            Guid idGruppo = Guid.Parse(e.CommandArgument.ToString());

            Logic.OfferteRaggruppamenti llGruppi = new Logic.OfferteRaggruppamenti();
            Entities.OffertaRaggruppamento gruppoAttuale = llGruppi.Find(new Entities.EntityId<Entities.OffertaRaggruppamento>(idGruppo));
            if (gruppoAttuale != null)
            {
                llGruppi.Delete(gruppoAttuale, true);
            }

            RaiseGruppoDeleted();
        }

        protected void txtDenominazione_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Guid.TryParse(hfIdGruppo.Value, out Guid idGruppo))
                {
                    Logic.OfferteRaggruppamenti llGruppi = new Logic.OfferteRaggruppamenti();
                    Entities.OffertaRaggruppamento gruppoAttuale = llGruppi.Find(new Entities.EntityId<Entities.OffertaRaggruppamento>(idGruppo));
                    if (gruppoAttuale != null)
                    {
                        if (!String.IsNullOrWhiteSpace(txtDenominazione.Text))
                        {
                            gruppoAttuale.Denominazione = txtDenominazione.Text.Trim();
                            llGruppi.SubmitToDatabase();
                        }
                        else
                        {
                            throw new Exception("La denominazione del gruppo è obbligatoria");
                        }
                    }
                    else
                    {
                        throw new Exception("Il gruppo non esiste nella fonte dati, ricaricare la pagina");
                    }
                }
                else
                {
                    throw new Exception("Non è stato possibile recuperare l'identificativo del gruppo");
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(Page, ex);
            }
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Effettua lo scatenamento dell'evento di aggiunta di un gruppo
        /// </summary>
        private void RaiseGruppoAdded()
        {
            if (GruppoAdded != null) GruppoAdded(this, EventArgs.Empty);
        }

        /// <summary>
        /// Effettua lo scatenamento dell'evento di rimozione di un gruppo
        /// </summary>
        private void RaiseGruppoDeleted()
        {
            if (GruppoDeleted != null) GruppoDeleted(this, EventArgs.Empty);
        }

        #endregion
    }
}