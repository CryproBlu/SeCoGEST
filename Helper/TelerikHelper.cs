using System;
using System.Globalization;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SeCoGEST.Helper
{
    public static class TelerikHelper
    {
        public static void TraduciElementiGriglia(GridItemEventArgs e)
        {
            if (e.Item is GridPagerItem)
            {
                if ((e.Item.FindControl("ChangePageSizeLabel") as Label) != null)
                    (e.Item.FindControl("ChangePageSizeLabel") as Label).Text = "Elementi per pagina";

                if ((e.Item.FindControl("GoToPageLabel") as Label) != null)
                    (e.Item.FindControl("GoToPageLabel") as Label).Text = "Vai alla Pagina";

                if ((e.Item.FindControl("PageOfLabel") as Label) != null)
                    (e.Item.FindControl("PageOfLabel") as Label).Text = (e.Item.FindControl("PageOfLabel") as Label).Text.Replace("of", "di");

                if ((e.Item.FindControl("GoToPageLinkButton") as Button) != null)
                    (e.Item.FindControl("GoToPageLinkButton") as Button).Text = "Vai";

                if ((e.Item.FindControl("ChangePageSizeLinkButton") as Button) != null)
                    (e.Item.FindControl("ChangePageSizeLinkButton") as Button).Text = "Modifica";

            }
        }

        /// <summary>
        /// Traduce in italiano tutte le voci del menù di filtro della Telerik.RadGrid
        /// </summary>
        /// <param name="menu"></param>
        public static void TraduciMenuFiltro(GridFilterMenu menu)
        {
            foreach (RadMenuItem item in menu.Items)
            {
                switch (item.Text)
                {
                    case "NoFilter":
                        item.Text = "Nessun filtro";
                        break;
                    case "Contains":
                        item.Text = "Contiene";
                        break;
                    case "DoesNotContain":
                        item.Text = "Non contiene";
                        break;
                    case "StartsWith":
                        item.Text = "Inizia con";
                        break;
                    case "EndsWith":
                        item.Text = "Finisce con";
                        break;
                    case "EqualTo":
                        item.Text = "Uguale a";
                        break;
                    case "NotEqualTo":
                        item.Text = "Diverso da";
                        break;
                    case "GreaterThan":
                        item.Text = "Maggiore di";
                        break;
                    case "LessThan":
                        item.Text = "Minore di";
                        break;
                    case "GreaterThanOrEqualTo":
                        item.Text = "Maggiore o uguale a";
                        break;
                    case "LessThanOrEqualTo":
                        item.Text = "Minore o uguale a";
                        break;
                    case "Between":
                        item.Text = "Compreso fra";
                        break;
                    case "NotBetween":
                        item.Text = "Non compreso fra";
                        break;
                    case "IsEmpty":
                        item.Text = "E' vuoto";
                        break;
                    case "NotIsEmpty":
                        item.Text = "Non è vuoto";
                        break;
                    case "IsNull":
                        item.Text = "E' nullo";
                        break;
                    case "NotIsNull":
                        item.Text = "Non è nullo";
                        break;
                }
            }
        }

        /// <summary>
        /// Inserisce una riga vuota all'inizio della lista di Items della RadComboBox passata
        /// </summary>
        /// <param name="comboBox"></param>
        public static void InsertBlankComboBoxItem(RadComboBox comboBox)
        {
            RadComboBoxItem emptyItem = new RadComboBoxItem("", "");
            comboBox.Items.Insert(0, emptyItem);
        }

        /// <summary>
        /// Inserisce una riga vuota all'inizio della lista di Items della RadComboBox passata
        /// </summary>
        /// <param name="comboBox">ComboBox nella quale aggiungere la riga vuota</param>
        /// <param name="customText">Testo personalizzato che deve avere la riga vuota</param>
        public static void InsertBlankComboBoxItem(RadComboBox comboBox, string customText)
        {
            RadComboBoxItem emptyItem = new RadComboBoxItem(customText, "");
            comboBox.Items.Insert(0, emptyItem);
        }

        /// <summary>
        /// Inserisce una riga vuota all'inizio della lista di Items della RadComboBox passata
        /// </summary>
        /// <param name="comboBox">ComboBox nella quale aggiungere la riga vuota</param>
        /// <param name="customText">Testo personalizzato che deve avere la riga vuota</param>
        /// <param name="customValue">Valore personalizzato che deve avere la riga vuota</param>
        public static void InsertBlankComboBoxItem(RadComboBox comboBox, string customText, string customValue)
        {
            RadComboBoxItem emptyItem = new RadComboBoxItem(customText, customValue);
            comboBox.Items.Insert(0, emptyItem);
        }

    }
}
