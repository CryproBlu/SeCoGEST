using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using SeCoGEST.Web.UI;
using Telerik.Web.UI;

namespace SeCoGEST.Web
{
    public static class MasterPageHelper
    {
        #region Metodi Pubblici

        /// <summary>
        /// Restituisce il RadAjaxManager contenuto nella master associata alla pagina passata come parametro
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public static RadAjaxManager GetRadAjaxManager(Page currentPage)
        {
            Main masterPage = GetMasterFromPage(currentPage);
            return (masterPage != null) ? masterPage.RadAjaxManager : null;
        }

        ///// <summary>
        ///// Restituisce il RadSkinManager contenuto nella master associata alla pagina passata come parametro
        ///// </summary>
        ///// <param name="currentPage"></param>
        ///// <returns></returns>
        //public static RadSkinManager GetRadSkinManager(Page currentPage)
        //{
        //    Main masterPage = GetMasterFromPage(currentPage);
        //    return (masterPage != null) ? masterPage.RadSkinManager : null;
        //}

        /// <summary>
        /// Restituisce il RadPersistenceManager contenuto nella master associata alla pagina passata come parametro
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public static RadPersistenceManager GetRadPersistenceManager(Page currentPage)
        {
            Main masterPage = GetMasterFromPage(currentPage);
            return (masterPage != null) ? masterPage.RadPersistenceManager : null;
        }

        /// <summary>
        /// Restituisce il RadFormDecorator contenuto nella master associata alla pagina passata come parametro
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public static RadFormDecorator GetRadFormDecorator(Page currentPage)
        {
            Main masterPage = GetMasterFromPage(currentPage);
            return (masterPage != null) ? masterPage.RadFormDecorator : null;
        }

        /// <summary>
        /// Restituisce il RadWindowManager contenuto nella master associata alla pagina passata come parametro
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public static RadWindowManager GetRadWindowManager(Page currentPage)
        {
            Main masterPage = GetMasterFromPage(currentPage);
            return (masterPage != null) ? masterPage.RadWindowManager : null;
        }

        /// <summary>
        /// Restituisce il RadAjaxLoadingPanel contenuto nella master associata alla pagina passata come parametro
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public static RadAjaxLoadingPanel GetRadAjaxLoadingPanel(Page currentPage)
        {
            Main masterPage = GetMasterFromPage(currentPage);
            return (masterPage != null) ? masterPage.RadAjaxLoadingPanel : null;
        }

        /// <summary>
        /// Restituisce la master page di tipo Master dalla pagina passata come parametro
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public static Main GetMasterFromPage(Page currentPage)
        {
            if (currentPage == null || !(currentPage.Master is Main))
            {
                return null;
            }
            else
            {
                return currentPage.Master as Main;
            }
        }










        /// <summary>
        /// Recupera il valore delle property MenuVisibile presente nella Master
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public static bool GetMenuVisibile(Page currentPage)
        {
            Main masterPage = GetMasterFromPage(currentPage);
            return (masterPage != null) ? masterPage.MenuVisibile : false;
        }

        /// <summary>
        /// Setta il valore delle property MenuVisibile presente nella Master
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="value"></param>
        public static void SetMenuVisibile(Page currentPage, bool value)
        {
            Main masterPage = GetMasterFromPage(currentPage);
            if (masterPage != null)
            {
                masterPage.MenuVisibile = value;
            }
        }

        ///// <summary>
        ///// Restituisce il nome della skin scelta
        ///// </summary>
        ///// <param name="currentPage"></param>
        ///// <returns></returns>
        //public static string GetRadSkinName(Page currentPage)
        //{
        //    string skinName = String.Empty;
        //    RadSkinManager skinManager = GetRadSkinManager(currentPage);
        //    if (skinManager != null)
        //    {
        //        skinName = skinManager.Skin;
        //    }

        //    return skinName;
        //}


        /// <summary>
        /// Restituisce l'ID dell'oggetto RadWindowManager
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public static string GetRadWindowManagerID(Page currentPage)
        {
            string radWindowManagerID = String.Empty;
            RadWindowManager windowManager = GetRadWindowManager(currentPage);
            if (windowManager != null)
            {
                radWindowManagerID = windowManager.ID;
            }

            return radWindowManagerID;
        }

        /// <summary>
        /// Restituisce l'ID dell'oggetto RadAjaxLoadingPanel
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public static string GetRadAjaxLoadingPanelID(Page currentPage)
        {
            string loadingPanelID = String.Empty;
            RadAjaxLoadingPanel loadingPanel = GetRadAjaxLoadingPanel(currentPage);
            if (loadingPanel != null)
            {
                loadingPanelID = loadingPanel.ID;
            }

            return loadingPanelID;
        }

        #endregion
    }
}