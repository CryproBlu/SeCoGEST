using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SeCoGEST.Entities;
using Telerik.Web.UI;

namespace SeCoGEST.Web.UI
{
    public partial class Main : System.Web.UI.MasterPage
    {
        #region Property

        #region Pubbliche

        /// <summary>
        /// Restituisce l'oggetto Telerik.Web.UI.RadPersistenceManager necessario per memorizzare lo stato degli oggetti
        /// </summary>
        public Telerik.Web.UI.RadPersistenceManager RadPersistenceManager
        {
            get
            {
                return mainRadPersistenceManager;
            }
        }

        /// <summary>
        /// Restituisce l'oggetto Telerik.Web.UI.RadFormDecorator necessario per memorizzare l'aspetto degli oggetti
        /// </summary>
        public Telerik.Web.UI.RadFormDecorator RadFormDecorator
        {
            get
            {
                return mainRadFormDecorator;
            }
        }

        /// <summary>
        /// Restituisce l'oggetto Telerik.Web.UI.RadWindowManager necessario per mostrare gli errori
        /// </summary>
        public Telerik.Web.UI.RadWindowManager RadWindowManager
        {
            get
            {
                return RadWindowManagerMaster;
            }
        }

        /// <summary>
        /// Restituisce l'oggetto Telerik.Web.UI.RadAjaxManager necessario per mostrare gli errori
        /// </summary>
        public Telerik.Web.UI.RadAjaxManager RadAjaxManager
        {
            get
            {
                return RadAjaxManagerMaster;
            }
        }

        /// <summary>
        /// Restituisce l'oggetto Telerik.Web.UI.RadAjaxLoadingPanel necessario per mostrare all'utente l'animazione di attesa
        /// </summary>
        public Telerik.Web.UI.RadAjaxLoadingPanel RadAjaxLoadingPanel
        {
            get
            {
                return RadAjaxLoadingPanelMaster;
            }
        }


        /// <summary>
        /// Recupera o setta le impostazioni di visibilità del menù
        /// </summary>
        public bool MenuVisibile
        {
            get
            {
                return ucMenuPrincipale.IsVisible;
            }
            set
            {
                ucMenuPrincipale.IsVisible = value;
            }
        }

        #endregion

        #endregion

        #region Costanti

        protected const string MENU_UTENTE_LOGOUT_ITEM_VALUE = "LogoutItem";

        protected const string MENU_UTENTE_LOGGEDUSERNAME_ITEM_VALUE = "LoggedUserNameItem";

        #endregion

        #region Intercettazione Eventi

        /// <summary>
        /// Metodo di gestione dell'evento Load della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.CacheControl = "no-cache";
            //Response.AddHeader("pragma", "no-cache");
            //Response.Expires = 0;
            try
            {
                if (Infrastructure.InformazioniSessione.BloccaAccessi())
                {
                    string s = @"<script type='text/javascript'>window.alert('L\'applicazione è in attesa di aggiornamento.\nSi prega di chiudere il programma il prima possibile in quanto tale operazione non è fattibile se l\'applicazione è in uso.\nGrazie.');</script>";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowAlert", s);
                }


                if (!Helper.Web.IsPostOrCallBack(this.Page))
                {
                    SetDataServer();
                    CreaMenuUtente();
                }
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                MessageHelper.ShowErrorMessage(Page, ex);
            }            
        }

        /// <summary>
        /// Metodo di gestione dell'evento ServiceRequest nell'oggetto XmlHttpPanel che gestisce l'aggiornamento della data 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadXmlHttpPanel1_ServiceRequest(object sender, Telerik.Web.UI.RadXmlHttpPanelEventArgs e)
        {
            SetDataServer();
        }

        /// <summary>
        /// Gestisce l'evento ItemClick sul menu dell'utente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rmMenuUtente_ItemClick(object sender, Telerik.Web.UI.RadMenuEventArgs e)
        {
            if (e != null && e.Item != null)
            {
                switch (e.Item.Value)
                {
                    case MENU_UTENTE_LOGOUT_ITEM_VALUE:
                        EffettuaLogOut();
                        break;
                }
            }
        }

        #endregion

        #region Funzioni Accessorie

        /// <summary>
        /// Recupera la versione della dll del progetto Web
        /// </summary>
        /// <returns></returns>
        protected string GetApplicationVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Effettua il settaggio della data del server nel campo apposito
        /// </summary>
        private void SetDataServer()
        {
            //lblDatetime.Text = String.Concat(DateTimeUtility.GiornoInLettere(System.DateTime.Now.DayOfWeek), ", ", System.DateTime.Now.Date.ToString("dd MMMM yyyy"), ", ", System.DateTime.Now.ToShortTimeString());
            lblDatetime.Text = System.DateTime.Now.Date.ToShortDateString() + ' ' + System.DateTime.Now.ToShortTimeString();
        }




        /// <summary>
        /// Esegue le operazioni necessarie per effettuare il logout dall'applicazione
        /// </summary>
        private void EffettuaLogOut()
        {
            string username = String.Empty;

            //if (InformazioniAccountAutenticato.GetIstance() != null &&
            //    InformazioniAccountAutenticato.GetIstance().Account != null)
            //{
            //    username = InformazioniAccountAutenticato.GetIstance().Account.UserName;
            //}

            //Effettuo il LogOut dell'utente e lo rimando alla pagina iniziale' di LogIn
            FormsAuthentication.SignOut();
            //InformazioniAccountAutenticato.RimuoviInformazioniAccount();
            //SeCoGes.Logging.LogManager.AddLogAccessi(username, "Effettuato il logout.");

            Session.Clear();
            Session.Abandon();

            Response.Redirect("/Home.aspx", true);
        }

        /// <summary>
        /// Effettua la creazione del menu in base all'utente loggato
        /// </summary>
        private void CreaMenuUtente()
        {
            string username = this.Page.User.Identity.Name;

            //if (InformazioniAccountAutenticato.GetIstance() != null &&
            //    InformazioniAccountAutenticato.GetIstance().Account != null)
            //{
            //    username = InformazioniAccountAutenticato.GetIstance().Account.UserName;
            //}

            // Viene creato l'elemento del menu che effettua il logout 
            RadMenuItem logoutItem = new RadMenuItem("Logout");
            logoutItem.Value = MENU_UTENTE_LOGOUT_ITEM_VALUE;

            // Viene creato l'elemento che mostra l'username dell'utente loggato
            RadMenuItem loggedUserNameItem = new RadMenuItem(username);
            loggedUserNameItem.Value = MENU_UTENTE_LOGGEDUSERNAME_ITEM_VALUE;
            loggedUserNameItem.PostBack = false;

            // Vengono aggiunti gli elementi da relativi alla voce appena creata
            loggedUserNameItem.Items.Add(logoutItem);

            // Viene aggiunto al menu il ramo principale
            rmMenuUtente.Items.Add(loggedUserNameItem);
        }

        #endregion
    }
}