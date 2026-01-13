using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.Device.Detection;
using Telerik.Web.UI;
using SeCoGes.Utilities;

namespace SeCoGEST.Helper
{
    public static class TelerikRadGridHelper
    {        
        /// <summary>
        /// Applica la traduzione presente nella cartella delle lingue (App_GlobalResources)
        /// </summary>
        /// <param name="griglia"></param>
        public static void ApplicaTraduzioneDaFileDiResource(RadGrid griglia)
        {
            ApplicaTraduzioneDaFileDiResource(griglia, "it-IT", true);
        }

        /// <summary>
        /// Applica la traduzione presente nella cartella delle lingue (App_GlobalResources)
        /// </summary>
        /// <param name="griglia"></param>
        /// <param name="culture"></param>
        /// <param name="rebindGrid"></param>
        public static void ApplicaTraduzioneDaFileDiResource(RadGrid griglia, string culture, bool rebindGrid)
        {
            if (griglia != null)
            {
                CultureInfo newCulture = CultureInfo.CreateSpecificCulture(culture);
                griglia.LocalizationPath = "/App_GlobalResources";
                griglia.Culture = newCulture;
                if (rebindGrid)
                {
                    griglia.Rebind();
                }
            }
        }

        /// <summary>
        /// Gestisce il contenuto delle colonne per il layout dei dipositivi mobile
        /// </summary>
        /// <param name="griglia"></param>
        /// <param name="e"></param>
        public static void ManageColumnContentOnMobileLayout(RadGrid griglia, GridItemEventArgs e)
        {
            if (griglia != null && griglia.Columns != null && griglia.Columns.Count > 0 && e != null && e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;

                foreach (GridColumn colonna in griglia.Columns)
                {
                    try
                    {
                        dataItem[colonna].Attributes.Add("data-title", colonna.HeaderText);
                    }
                    catch (Exception) { continue; }                    
                }
            }
        }

        #region Ricerca Valori

        /// <summary>
        /// Restituisce un controllo, in base ad un ID passato come parametro, dal parametro item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="controlID"></param>
        /// <returns></returns>
        public static T FindControl<T>(GridItem item, string controlID) where T : Control
        {
            if (item == null) return default(T);

            return item.FindControl(controlID) as T;
        }

        #endregion

        #region Recupero Valori

        /// <summary>
        /// Recupera il valore di una chiave presente nell'item contenuto nell'event args passato come parametro
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="griglia"></param>
        /// <param name="gridCommandEventArgs"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetDataKeyValueFromGridCommandEventArgsItem<T>(RadGrid griglia, GridCommandEventArgs gridCommandEventArgs, string key)
        {
            T returnValue = default(T);

            if (griglia != null &&
                gridCommandEventArgs != null &&
                !String.IsNullOrEmpty(key))
            {
                GridDataItem dataItem = griglia.Items[int.Parse(gridCommandEventArgs.CommandArgument.ToString())] as GridDataItem;

                if (dataItem != null)
                {
                    returnValue = (T)dataItem.GetDataKeyValue(key);
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Restituisce il valore contenuto nella property "Value" dell'oggetto RadNumericTextBox recuperato in base ai parametri passati
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="controlID"></param>
        /// <returns></returns>
        public static Nullable<T> GetValueFromRadNumericTextBox<T>(GridItem item, string controlID)
            where T : struct
        {
            RadNumericTextBox rntb = TelerikRadGridHelper.FindControl<RadNumericTextBox>(item, controlID);
            if (rntb != null && rntb.Value.HasValue)
            {
                object value = Convert.ChangeType(rntb.Value, typeof(T));
                if (value == null)
                {
                    return null;
                }
                else
                {
                    return (T)value;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Restituisce il valore contenuto nella property "SelectedDate" dell'oggetto RadDatePicker recuperato in base ai parametri passati
        /// </summary>
        /// <param name="item"></param>
        /// <param name="controlID"></param>
        /// <returns></returns>
        public static Nullable<DateTime> GetSelectedDateFromRadDatePicker(GridItem item, string controlID)
        {
            RadDatePicker rdp = TelerikRadGridHelper.FindControl<RadDatePicker>(item, controlID);
            if (rdp != null)
            {
                return rdp.SelectedDate;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Restituisce il valore contenuto nella property "Text" dell'oggetto RadTextBox recuperato in base ai parametri passati
        /// </summary>
        /// <param name="item"></param>
        /// <param name="controlID"></param>
        /// <returns></returns>
        public static string GetTextFromRadTextBox(GridItem item, string controlID)
        {
            return GetTextFromRadTextBox(item, controlID, true);
        }

        /// <summary>
        /// Restituisce il valore contenuto nella property "Text" dell'oggetto RadTextBox recuperato in base ai parametri passati
        /// </summary>
        /// <param name="item"></param>
        /// <param name="controlID"></param>
        /// <param name="doTrim">True per far restituire il testo trimmato</param>
        /// <returns></returns>
        public static string GetTextFromRadTextBox(GridItem item, string controlID, bool doTrim)
        {
            RadTextBox rtb = TelerikRadGridHelper.FindControl<RadTextBox>(item, controlID);
            if (rtb != null)
            {
                if (doTrim)
                {
                    return rtb.Text.ToTrimmedString();
                }
                else
                {
                    return rtb.Text;
                }                
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Restituisce il valore contenuto nella property "Text" dell'oggetto RadComboBox recuperato in base ai parametri passati
        /// </summary>
        /// <param name="item"></param>
        /// <param name="controlID"></param>
        /// <returns></returns>
        public static string GetTextFromRadComboBox(GridItem item, string controlID)
        {
            return GetTextFromRadComboBox(item, controlID, true);
        }

        /// <summary>
        /// Restituisce il valore contenuto nella property "Text" dell'oggetto RadComboBox recuperato in base ai parametri passati
        /// </summary>
        /// <param name="item"></param>
        /// <param name="controlID"></param>
        /// <param name="doTrim">True per far restituire il testo trimmato</param>
        /// <returns></returns>
        public static string GetTextFromRadComboBox(GridItem item, string controlID, bool doTrim)
        {
            RadComboBox rcb = TelerikRadGridHelper.FindControl<RadComboBox>(item, controlID);
            if (rcb != null)
            {
                if (doTrim)
                {
                    return rcb.Text.ToTrimmedString();
                }
                else
                {
                    return rcb.Text;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Restituisce il valore contenuto nella property "Checked" dell'oggetto CheckBox recuperato in base ai parametri passati
        /// </summary>
        /// <param name="item"></param>
        /// <param name="controlID"></param>
        /// <returns></returns>
        public static Nullable<bool> GetCheckedFromCheckBox(GridItem item, string controlID)
        {
            CheckBox chk = TelerikRadGridHelper.FindControl<CheckBox>(item, controlID);
            if (chk != null)
            {
                return chk.Checked;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Restituisce il valore contenuto nella property "Checked" dell'oggetto RadioButton recuperato in base ai parametri passati
        /// </summary>
        /// <param name="item"></param>
        /// <param name="controlID"></param>
        /// <returns></returns>
        public static Nullable<bool> GetCheckedFromRadioButton(GridItem item, string controlID)
        {
            RadioButton rb = TelerikRadGridHelper.FindControl<RadioButton>(item, controlID);
            if (rb != null)
            {
                return rb.Checked;
            }
            else
            {
                return null;
            }
        }
        #endregion        

        #region Settaggio Valori
        
        /// <summary>
        /// Setta il valore contenuto della property "Value" dell'oggetto RadNumericTextBox recuperato in base ai parametri passati
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="controlID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetValueFromRadNumericTextBox<T>(GridItem item, string controlID, Nullable<T> value)
            where T : struct
        {
            RadNumericTextBox rntb = TelerikRadGridHelper.FindControl<RadNumericTextBox>(item, controlID);
            if (rntb != null)
            {
                if (value.HasValue)
                {
                    object valore = Convert.ChangeType(value.Value, typeof(double));
                    rntb.Value = (valore != null) ? (double)valore : (double?)null; 
                }                
            }
        }

        /// <summary>
        /// Setta il valore contenuto della property "SelectedDate" dell'oggetto RadDatePicker recuperato in base ai parametri passati
        /// </summary>
        /// <param name="item"></param>
        /// <param name="controlID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetSelectedDateFromRadDatePicker(GridItem item, string controlID, Nullable<DateTime> value)
        {
            RadDatePicker rdp = TelerikRadGridHelper.FindControl<RadDatePicker>(item, controlID);
            if (rdp != null)
            {
                rdp.SelectedDate = value;
            }
        }

        /// <summary>
        /// Setta il valore contenuto della property "Text" dell'oggetto RadTextBox recuperato in base ai parametri passati
        /// </summary>
        /// <param name="item"></param>
        /// <param name="controlID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetTextFromRadTextBox(GridItem item, string controlID, string value)
        {
            RadTextBox rtb = TelerikRadGridHelper.FindControl<RadTextBox>(item, controlID);
            if (rtb != null)
            {
                rtb.Text = value;
            }
        }

        /// <summary>
        /// Setta il valore contenuto della property "Checked" dell'oggetto CheckBox recuperato in base ai parametri passati
        /// </summary>
        /// <param name="item"></param>
        /// <param name="controlID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetCheckedFromCheckBox(GridItem item, string controlID, Nullable<bool> value)
        {
            CheckBox chk = TelerikRadGridHelper.FindControl<CheckBox>(item, controlID);
            if (chk != null)
            {
                chk.Checked = (value.HasValue && value.Value);
            }
        }

        #endregion        

        #region Gestione dei filtri

        /// <summary>
        /// Svuota i filtri della griglia ed effettua il rebind se il parametro rebindGrid è true
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="rebindGrid"></param>
        public static void ClearFilters(RadGrid grid, bool rebindGrid)
        {
            if (grid != null &&
                grid.MasterTableView != null &&
                grid.MasterTableView.Columns != null &&
                grid.MasterTableView.Columns.Count > 0)
            {
                grid.MasterTableView.FilterExpression = String.Empty;
                foreach (GridColumn column in grid.MasterTableView.Columns)
                {
                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;
                    column.ResetCurrentFilterValue();
                }

                if (rebindGrid)
                {
                    grid.Rebind();
                }
            }
        }

        /// <summary>
        /// Restituisce true se i filtri della griglia sono vuoti
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public static bool FilterIsEmpty(RadGrid grid)
        {
            bool filterIsEmpty = true;

            if (grid != null &&
                grid.MasterTableView != null &&
                grid.MasterTableView.Columns != null &&
                grid.MasterTableView.Columns.Count > 0)
            {
                foreach (GridColumn column in grid.MasterTableView.Columns)
                {
                    if (column.CurrentFilterFunction != GridKnownFunction.NoFilter ||
                        !String.IsNullOrEmpty(column.CurrentFilterValue))
                    {
                        filterIsEmpty = false;
                        break;
                    }
                }
            }

            return filterIsEmpty;
        }

        /// <summary>
        /// Ingnetta lo script necessario a nascondere i filtri se i filtri sono vuoti
        /// </summary>
        /// <param name="pageToInjectScript"></param>
        /// <param name="grid"></param>
        public static void InjectScriptToHiddenFilterItemIfEmpty(Page pageToInjectScript, RadGrid grid)
        {
            if (pageToInjectScript == null || pageToInjectScript.ClientScript == null || grid == null) return;

            if (FilterIsEmpty(grid))
            {
                InjectScriptToHiddenFilterItem(pageToInjectScript, grid);
            }
        }

        /// <summary>
        /// Ingnetta lo script necessario a nascondere i filtri
        /// </summary>
        /// <param name="pageToInjectScript"></param>
        /// <param name="grid"></param>
        public static void InjectScriptToHiddenFilterItem(Page pageToInjectScript, RadGrid grid)
        {
            if (pageToInjectScript == null || pageToInjectScript.ClientScript == null || grid == null) return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("var caricato;");
            sb.AppendLine("function pageLoad() {");
            sb.AppendLine("if (caricato == null) {");
            sb.AppendLine(String.Format("var grid = $find('{0}');", grid.ClientID));
            sb.AppendLine("if (grid != null) {");
            sb.AppendLine("var masterTableView = (grid.get_masterTableView) ? grid.get_masterTableView() : null;");
            sb.AppendLine("if (masterTableView != null && masterTableView.hideFilterItem) {");
            sb.AppendLine("masterTableView.hideFilterItem();");
            sb.AppendLine("caricato = true;");
            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine("</script>");

            pageToInjectScript.ClientScript.RegisterStartupScript(pageToInjectScript.GetType(), Guid.NewGuid().ToString(), sb.ToString());
        }

        /// <summary>
        /// Effettua la gestione dei filtri della griglia e del tasto cerca passato come parametro presenti in una pagina anch'essa passata come parametro
        /// </summary>
        /// <param name="pageToManage"></param>
        /// <param name="griglia"></param>
        /// <param name="radToolBarButton_Cerca"></param>
        public static void ManageFiltering(Page pageToManage, RadGrid griglia, RadToolBarButton radToolBarButton_Cerca)
        {
            if (FilterIsEmpty(griglia))
            {
                InjectScriptToHiddenFilterItem(pageToManage, griglia);
            }
            else
            {
                if (radToolBarButton_Cerca != null)
                    radToolBarButton_Cerca.Checked = true;
            }
        }

        #endregion

        #region Gestione delle esportazioni

        /// <summary>
        /// Effettua la gestione delle impostazioni dei tasti della griglia che permettono di effettuare l'esportazione dei dati presenti nella griglia in vari formati
        /// </summary>
        /// <param name="griglia"></param>
        /// <param name="fileName"></param>
        /// <param name="showAddNewRecordButton"></param>
        /// <param name="showExcelButton"></param>
        /// <param name="showCsvButton"></param>
        /// <param name="showPdfButton"></param>
        /// <param name="showWordButton"></param>
        public static void ManageExportButtonsSettings(RadGrid griglia, string fileName,
            bool showAddNewRecordButton,
            bool showExcelButton,
            bool showCsvButton,
            bool showPdfButton,
            bool showWordButton)
        {
            ManageExportButtonsSettings(griglia, fileName, GridCommandItemDisplay.Top, showAddNewRecordButton, showExcelButton, showCsvButton, showPdfButton, showWordButton);
        }

        /// <summary>
        /// Effettua la gestione delle impostazioni dei tasti della griglia che permettono di effettuare l'esportazione dei dati presenti nella griglia in vari formati
        /// </summary>
        /// <param name="griglia"></param>
        /// <param name="fileName"></param>
        /// <param name="commandItemDisplay"></param>
        /// <param name="showAddNewRecordButton"></param>
        /// <param name="showExcelButton"></param>
        /// <param name="showCsvButton"></param>
        /// <param name="showPdfButton"></param>
        /// <param name="showWordButton"></param>
        public static void ManageExportButtonsSettings(RadGrid griglia, string fileName, 
            GridCommandItemDisplay commandItemDisplay,
            bool showAddNewRecordButton,
            bool showExcelButton,
            bool showCsvButton,
            bool showPdfButton,
            bool showWordButton)
        {
            if (griglia == null) return;

            bool aggiuntoTastoEsportazione = false;

            if (showExcelButton)
            {
                ManageExelExportSettings(griglia, fileName, showAddNewRecordButton);
                aggiuntoTastoEsportazione = true;
            }

            if (showCsvButton)
            {
                ManageCsvExportSettings(griglia, fileName, showAddNewRecordButton);
                aggiuntoTastoEsportazione = true;
            }

            if (showPdfButton)
            {
                ManagePdfExportSettings(griglia, fileName, showAddNewRecordButton);
                aggiuntoTastoEsportazione = true;
            }

            if (showWordButton)
            {
                ManageWordExportSettings(griglia, fileName, showAddNewRecordButton);
                aggiuntoTastoEsportazione = true;
            }

            if (showWordButton)
            {
                ManageWordExportSettings(griglia, fileName, showAddNewRecordButton);
                aggiuntoTastoEsportazione = true;
            }

            if (!aggiuntoTastoEsportazione && showAddNewRecordButton)
            {
                griglia.MasterTableView.CommandItemDisplay = commandItemDisplay;
                griglia.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = showAddNewRecordButton;
            }
        }

        #region Excel

        /// <summary>
        /// Effettua la gestione delle impostazioni della griglia che permettono di effettuare l'esportazione in Excel
        /// </summary>
        /// <param name="griglia"></param>
        /// <param name="fileName"></param>
        /// <param name="showAddNewRecordButton"></param>
        public static void ManageExelExportSettings(RadGrid griglia, string fileName, bool showAddNewRecordButton)
        {
            ManageExelExportSettings(griglia, fileName, showAddNewRecordButton, true);
        }

        /// <summary>
        /// Effettua la gestione delle impostazioni della griglia che permettono di effettuare l'esportazione in Excel
        /// </summary>
        /// <param name="griglia"></param>
        /// <param name="fileName"></param>
        /// <param name="showAddNewRecordButton"></param>
        /// <param name="checkDevice"></param>
        public static void ManageExelExportSettings(RadGrid griglia, string fileName, bool showAddNewRecordButton, bool checkDevice)
        {
            ManageExelExportSettings(griglia, fileName, "Esporta in formato Excel", showAddNewRecordButton, checkDevice);
        }

        /// <summary>
        /// Effettua la gestione delle impostazioni della griglia che permettono di effettuare l'esportazione in Excel
        /// </summary>
        /// <param name="griglia"></param>
        /// <param name="fileName"></param>
        /// <param name="exportButtonText"></param>
        /// <param name="showAddNewRecordButton"></param>
        /// <param name="checkDevice"></param>
        public static void ManageExelExportSettings(RadGrid griglia, string fileName, string exportButtonText, bool showAddNewRecordButton, bool checkDevice)
        {
            ManageExelExportSettings(griglia, fileName, GridCommandItemDisplay.Top, showAddNewRecordButton, true, true, true, checkDevice, exportButtonText, GridExcelExportFormat.Xlsx, "xls");
        }

        /// <summary>
        /// Effettua la gestione delle impostazioni della griglia che permettono di effettuare l'esportazione in Excel
        /// </summary>
        /// <param name="griglia"></param>
        /// <param name="fileName"></param>
        /// <param name="commandItemDisplay"></param>
        /// <param name="showAddNewRecordButton"></param>
        /// <param name="showExportButton"></param>
        /// <param name="ignorePaging"></param>
        /// <param name="openInNewWindow"></param>
        /// <param name="checkDevice"></param>
        /// <param name="exportButtonText"></param>
        /// <param name="formato"></param>
        /// <param name="fileExtension"></param>
        public static void ManageExelExportSettings(RadGrid griglia, string fileName,
            GridCommandItemDisplay commandItemDisplay,
            bool showAddNewRecordButton,
            bool showExportButton,
            bool ignorePaging,
            bool openInNewWindow,
            bool checkDevice,
            string exportButtonText,
            GridExcelExportFormat formato,
            string fileExtension)
        {
            if (griglia == null) return;

            DeviceScreenSize screenDimensions = Detector.GetScreenSize(System.Web.HttpContext.Current.Request.UserAgent);
            if (!checkDevice || checkDevice && screenDimensions == DeviceScreenSize.ExtraLarge)
            {
                griglia.MasterTableView.CommandItemDisplay = commandItemDisplay;
                griglia.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = (griglia.Enabled) ? showAddNewRecordButton : false;
                griglia.MasterTableView.CommandItemSettings.ShowExportToExcelButton = showExportButton;
                griglia.MasterTableView.CommandItemSettings.ExportToExcelText = exportButtonText;
                griglia.ExportSettings.FileName = fileName.ToTrimmedString();
                griglia.ExportSettings.IgnorePaging = ignorePaging;
                griglia.ExportSettings.OpenInNewWindow = openInNewWindow;
                griglia.ExportSettings.Excel.Format = formato;
                griglia.ExportSettings.Excel.FileExtension = fileExtension;
            }
        }

        #endregion

        #region Pdf

        /// <summary>
        /// Effettua la gestione delle impostazioni della griglia che permettono di effettuare l'esportazione in Pdf
        /// </summary>
        /// <param name="griglia"></param>
        /// <param name="fileName"></param>
        /// <param name="showAddNewRecordButton"></param>
        public static void ManagePdfExportSettings(RadGrid griglia, string fileName, bool showAddNewRecordButton)
        {
            ManagePdfExportSettings(griglia, fileName, showAddNewRecordButton, true);
        }

        /// <summary>
        /// Effettua la gestione delle impostazioni della griglia che permettono di effettuare l'esportazione in Pdf
        /// </summary>
        /// <param name="griglia"></param>
        /// <param name="fileName"></param>
        /// <param name="showAddNewRecordButton"></param>
        /// <param name="checkDevice"></param>
        public static void ManagePdfExportSettings(RadGrid griglia, string fileName, bool showAddNewRecordButton, bool checkDevice)
        {
            ManagePdfExportSettings(griglia, fileName, GridCommandItemDisplay.Top, showAddNewRecordButton, true, true, true, checkDevice, "Esporta in formato PDF");
        }

        /// <summary>
        /// Effettua la gestione delle impostazioni della griglia che permettono di effettuare l'esportazione in Pdf
        /// </summary>
        /// <param name="griglia"></param>
        /// <param name="fileName"></param>
        /// <param name="commandItemDisplay"></param>
        /// <param name="showAddNewRecordButton"></param>
        /// <param name="showExportButton"></param>
        /// <param name="ignorePaging"></param>
        /// <param name="openInNewWindow"></param>
        /// <param name="checkDevice"></param>
        /// <param name="exportButtonText"></param>
        public static void ManagePdfExportSettings(RadGrid griglia, string fileName,
            GridCommandItemDisplay commandItemDisplay,
            bool showAddNewRecordButton,
            bool showExportButton,
            bool ignorePaging,
            bool openInNewWindow,
            bool checkDevice,
            string exportButtonText)
        {
            if (griglia == null) return;

            DeviceScreenSize screenDimensions = Detector.GetScreenSize(System.Web.HttpContext.Current.Request.UserAgent);
            if (!checkDevice || checkDevice && screenDimensions == DeviceScreenSize.ExtraLarge)
            {
                griglia.MasterTableView.CommandItemDisplay = commandItemDisplay;
                griglia.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = showAddNewRecordButton;
                griglia.MasterTableView.CommandItemSettings.ShowExportToPdfButton = showExportButton;
                griglia.MasterTableView.CommandItemSettings.ExportToPdfText = exportButtonText;
                griglia.ExportSettings.FileName = fileName.ToTrimmedString();
                griglia.ExportSettings.IgnorePaging = ignorePaging;
                griglia.ExportSettings.OpenInNewWindow = openInNewWindow;
            }
        }

        #endregion

        #region Word

        /// <summary>
        /// Effettua la gestione delle impostazioni della griglia che permettono di effettuare l'esportazione in Word
        /// </summary>
        /// <param name="griglia"></param>
        /// <param name="fileName"></param>
        /// <param name="showAddNewRecordButton"></param>
        public static void ManageWordExportSettings(RadGrid griglia, string fileName, bool showAddNewRecordButton)
        {
            ManageWordExportSettings(griglia, fileName, showAddNewRecordButton, true);
        }

        /// <summary>
        /// Effettua la gestione delle impostazioni della griglia che permettono di effettuare l'esportazione in Word
        /// </summary>
        /// <param name="griglia"></param>
        /// <param name="fileName"></param>
        /// <param name="showAddNewRecordButton"></param>
        /// <param name="checkDevice"></param>
        public static void ManageWordExportSettings(RadGrid griglia, string fileName, bool showAddNewRecordButton, bool checkDevice)
        {
            ManageWordExportSettings(griglia, fileName, GridCommandItemDisplay.Top, showAddNewRecordButton, true, true, true, checkDevice, "Esporta in formato Excel", GridWordExportFormat.Docx);
        }

        /// <summary>
        /// Effettua la gestione delle impostazioni della griglia che permettono di effettuare l'esportazione in Word
        /// </summary>
        /// <param name="griglia"></param>
        /// <param name="fileName"></param>
        /// <param name="commandItemDisplay"></param>
        /// <param name="showAddNewRecordButton"></param>
        /// <param name="showExportButton"></param>
        /// <param name="ignorePaging"></param>
        /// <param name="openInNewWindow"></param>
        /// <param name="checkDevice"></param>
        /// <param name="exportButtonText"></param>
        /// <param name="fileExtension"></param>
        public static void ManageWordExportSettings(RadGrid griglia, string fileName,
            GridCommandItemDisplay commandItemDisplay,
            bool showAddNewRecordButton,
            bool showExportButton,
            bool ignorePaging,
            bool openInNewWindow,
            bool checkDevice,
            string exportButtonText,
            GridWordExportFormat formato)
        {
            if (griglia == null) return;

            DeviceScreenSize screenDimensions = Detector.GetScreenSize(System.Web.HttpContext.Current.Request.UserAgent);
            if (!checkDevice || checkDevice && screenDimensions == DeviceScreenSize.ExtraLarge)
            {
                griglia.MasterTableView.CommandItemDisplay = commandItemDisplay;
                griglia.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = showAddNewRecordButton;
                griglia.MasterTableView.CommandItemSettings.ShowExportToWordButton = showExportButton;
                griglia.MasterTableView.CommandItemSettings.ExportToWordText = exportButtonText;
                griglia.ExportSettings.FileName = fileName.ToTrimmedString();
                griglia.ExportSettings.IgnorePaging = ignorePaging;
                griglia.ExportSettings.OpenInNewWindow = openInNewWindow;
                griglia.ExportSettings.Word.Format = formato;
            }
        }

        #endregion

        #region Csv

        /// <summary>
        /// Effettua la gestione delle impostazioni della griglia che permettono di effettuare l'esportazione in Csv
        /// </summary>
        /// <param name="griglia"></param>
        /// <param name="fileName"></param>
        /// <param name="showAddNewRecordButton"></param>
        public static void ManageCsvExportSettings(RadGrid griglia, string fileName, bool showAddNewRecordButton)
        {
            ManageCsvExportSettings(griglia, fileName, showAddNewRecordButton, true);
        }

        /// <summary>
        /// Effettua la gestione delle impostazioni della griglia che permettono di effettuare l'esportazione in Csv
        /// </summary>
        /// <param name="griglia"></param>
        /// <param name="fileName"></param>
        /// <param name="showAddNewRecordButton"></param>
        /// <param name="checkDevice"></param>
        public static void ManageCsvExportSettings(RadGrid griglia, string fileName, bool showAddNewRecordButton, bool checkDevice)
        {
            ManageCsvExportSettings(griglia, fileName, GridCommandItemDisplay.Top, showAddNewRecordButton, true, true, true, checkDevice, "Esporta in formato Excel", "xls");
        }

        /// <summary>
        /// Effettua la gestione delle impostazioni della griglia che permettono di effettuare l'esportazione in Csv
        /// </summary>
        /// <param name="griglia"></param>
        /// <param name="fileName"></param>
        /// <param name="commandItemDisplay"></param>
        /// <param name="showAddNewRecordButton"></param>
        /// <param name="showExportButton"></param>
        /// <param name="ignorePaging"></param>
        /// <param name="openInNewWindow"></param>
        /// <param name="checkDevice"></param>
        /// <param name="exportButtonText"></param>
        /// <param name="fileExtension"></param>
        public static void ManageCsvExportSettings(RadGrid griglia, string fileName,
            GridCommandItemDisplay commandItemDisplay,
            bool showAddNewRecordButton,
            bool showExportButton,
            bool ignorePaging,
            bool openInNewWindow,
            bool checkDevice,
            string exportButtonText,
            string fileExtension)
        {
            if (griglia == null) return;

            DeviceScreenSize screenDimensions = Detector.GetScreenSize(System.Web.HttpContext.Current.Request.UserAgent);
            if (!checkDevice || checkDevice && screenDimensions == DeviceScreenSize.ExtraLarge)
            {
                griglia.MasterTableView.CommandItemDisplay = commandItemDisplay;
                griglia.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = showAddNewRecordButton;
                griglia.MasterTableView.CommandItemSettings.ShowExportToCsvButton = showExportButton;
                griglia.MasterTableView.CommandItemSettings.ExportToCsvText = exportButtonText;
                griglia.ExportSettings.FileName = fileName.ToTrimmedString();
                griglia.ExportSettings.IgnorePaging = ignorePaging;
                griglia.ExportSettings.OpenInNewWindow = openInNewWindow;
                griglia.ExportSettings.Csv.FileExtension = fileExtension;
            }
        }

        #endregion

        #endregion
    }
}
