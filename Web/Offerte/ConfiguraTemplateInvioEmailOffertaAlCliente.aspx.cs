using SeCoGes.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SeCoGEST.Web.Offerte
{
    public partial class ConfiguraTemplateInvioEmailOffertaAlCliente : System.Web.UI.Page
    {
        #region Properties

        /// <summary>
        /// Recupera o setta la property Enabled degli oggetti nell'usercontrol
        /// </summary>
        private bool Enabled
        {
            get
            {
                if (ViewState["Enabled"] == null)
                {
                    ViewState["Enabled"] = true;
                }

                return (bool)ViewState["Enabled"];
            }
            set
            {
                ViewState["Enabled"] = value;
                AbilitaCampi(value);
            }
        }

        RadToolBarItem PulsanteToolbar_Aggiorna
        {
            get
            {
                return RadToolBar1.FindItemByValue("Aggiorna");
            }
        }

        #endregion

        #region Intercettazione Eventi

        /// <summary>
        /// Metodo di gestione dell'evento Caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            reTestoEmailDaInviareAlCliente.Modules.Clear();

            if (!Helper.Web.IsPostOrCallBack(this))
            {
                SeCoGes.Logging.LogManager.AddLogAccessi(String.Format("Accesso alla pagina '{0}'.", Request.Url.AbsolutePath));

                ShowData();
            }
        }

        /// <summary>
        /// Metodo di gestione dell'evento ButtonClick relativo alla toolbar presente nella pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadToolBar1_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {
            try
            {
                RadToolBarButton clickedButton = (RadToolBarButton)e.Item;

                switch (clickedButton.CommandName)
                {
                    case "Salva":
                        SaveData();
                        break;
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(this, ex);
            }
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Assegna l'indirizzo della pagina da aprire al pulsante Aggiorna.
        /// L'indirizzo è esattamente lo stesso della pagina aperta.
        /// </summary>
        private void ApplicaRedirectPulsanteAggiorna()
        {
            if (PulsanteToolbar_Aggiorna != null)
                ((RadToolBarButton)PulsanteToolbar_Aggiorna).NavigateUrl = Request.Url.ToString();
        }

        /// <summary>
        /// Abilita o disabilita i campi dell'interfaccia in base al parametro passato
        /// </summary>
        /// <param name="abilitaCampi"></param>
        private void AbilitaCampi(bool enabled)
        {
            reTestoEmailDaInviareAlCliente.Enabled = enabled;
        }

        /// <summary>
        ///  Memorizza i dati presenti nei controlli dell'interfaccia
        /// </summary>
        /// <param name="reloadPageAfterSave"></param>
        private void SaveData(bool reloadPageAfterSave = true)
        {
            MessagesCollector erroriDiValidazione = ValidaDati();
            if (erroriDiValidazione.HaveMessages)
            {
                // Se qualcosa non va bene mostro un avviso all'utente
                MessageHelper.ShowErrorMessage(this, erroriDiValidazione.ToString("<br />"));
                return;
            }
            else
            {
                try
                {
                    File.WriteAllText(Infrastructure.ConfigurationKeys.PERCORSO_TEMPLATE_EMAIL_INVIO_OFFERTA_AL_CLIENTE, reTestoEmailDaInviareAlCliente.Content);
                }
                catch (Exception ex)
                {
                    SeCoGes.Logging.LogManager.AddLogErrori(ex);
                    MessageHelper.ShowErrorMessage(this, ex.Message);
                }
                finally
                {
                    if (reloadPageAfterSave)
                    {
                        Helper.Web.ReloadPage(this);
                    }
                }
            }
        }

        /// <summary>
        /// Restituisce un oggetto contenente gli eventuali errori di validazione dei dati
        /// </summary>
        /// <returns></returns>
        private MessagesCollector ValidaDati()
        {
            MessagesCollector messaggi = new MessagesCollector();

            if (String.IsNullOrEmpty(reTestoEmailDaInviareAlCliente.Content))
            {
                messaggi.Add("Il testo email per l'invio dell'offerta al cliente è obbligatorio");
            }

            return messaggi;
        }

        /// <summary>
        /// Mostra nell'interfaccia i dati modificabili dall'utente
        /// </summary>
        private void ShowData()
        {
            ApplicaRedirectPulsanteAggiorna();

            if (File.Exists(Infrastructure.ConfigurationKeys.PERCORSO_TEMPLATE_EMAIL_INVIO_OFFERTA_AL_CLIENTE))
            {
                reTestoEmailDaInviareAlCliente.Content = File.ReadAllText(Infrastructure.ConfigurationKeys.PERCORSO_TEMPLATE_EMAIL_INVIO_OFFERTA_AL_CLIENTE);
            }
        }

        #endregion
    }
}