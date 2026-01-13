<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="ConfigurazioneArticoloAggiuntivo.aspx.cs" Inherits="SeCoGEST.Web.Preventivi.ConfigurazioneArticoloAggiuntivo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ToolBarButtonClicking(sender, args) {
            if (args != null && args.get_item) {

                var button = args.get_item();
                if (button != null && button.get_commandName) {

                    var messaggioConfermato = true;
                    if (button.get_commandName() == "Aggiorna") {
                        messaggioConfermato = confirm('Aggiornare i dati visualizzati?\n\nEventuali modifiche apportate ai dati e non salvate verranno perse.');
                    }

                    if (!messaggioConfermato) {
                        args.set_cancel(true);
                    }
                }
            }
        }

        function rcbTipologia_OnClientSelectedIndexChanging(sender, args) {
            var required_validator = document.getElementById('<%= PrincipalValidationSummary.ClientID %>');
            required_validator.innerText = ' ';
            required_validator.style.display = "none";
        }

        function rcbGruppoCategoriaCategoriaStatistica_Ingresso_OnClientItemsRequesting(sender, args) {
            try {
                var context = args.get_context();

                context["CodiceGruppo"] = getSelectedValueFromCombo('<%=rcbGruppoIngresso.ClientID%>');
                context["DescrizioneGruppo"] = getTextFromCombo('<%=rcbGruppoIngresso.ClientID%>');

                context["CodiceCategoria"] = getSelectedValueFromCombo('<%=rcbCategoriaIngresso.ClientID%>');
                context["DescrizioneCategoria"] = getTextFromCombo('<%=rcbCategoriaIngresso.ClientID%>');

                context["CodiceCategoriaStatistica"] = getSelectedValueFromCombo('<%=rcbCategoriaStatisticaIngresso.ClientID%>');
                context["DescrizioneCategoriaStatistica"] = getTextFromCombo('<%=rcbCategoriaStatisticaIngresso.ClientID%>');

            } catch (e) {
                alert(e.message);
            }
        }

        function rcbGruppoCategoriaCategoriaStatistica_Uscita_OnClientItemsRequesting(sender, args) {
            try {
                var context = args.get_context();

                context["CodiceGruppo"] = getSelectedValueFromCombo('<%=rcbGruppoUscita.ClientID%>');
                context["DescrizioneGruppo"] = getTextFromCombo('<%=rcbGruppoUscita.ClientID%>');

                context["CodiceCategoria"] = getSelectedValueFromCombo('<%=rcbCategoriaUscita.ClientID%>');
                context["DescrizioneCategoria"] = getTextFromCombo('<%=rcbCategoriaUscita.ClientID%>');

                context["CodiceCategoriaStatistica"] = getSelectedValueFromCombo('<%=rcbCategoriaStatisticaUscita.ClientID%>');
                context["DescrizioneCategoriaStatistica"] = getTextFromCombo('<%=rcbCategoriaStatisticaUscita.ClientID%>');

            } catch (e) {
                alert(e.message);
            }
        }


        function rcbCodiceArticoloIngresso_OnClientItemsRequesting(sender, args) {
            try {
                var context = args.get_context();

                context["CodiceGruppo"] = getSelectedValueFromCombo('<%=rcbGruppoIngresso.ClientID%>');
                context["DescrizioneGruppo"] = getTextFromCombo('<%=rcbGruppoIngresso.ClientID%>');

                context["CodiceCategoria"] = getSelectedValueFromCombo('<%=rcbCategoriaIngresso.ClientID%>');
                context["DescrizioneCategoria"] = getTextFromCombo('<%=rcbCategoriaIngresso.ClientID%>');

                context["CodiceCategoriaStatistica"] = getSelectedValueFromCombo('<%=rcbCategoriaStatisticaIngresso.ClientID%>');
                context["DescrizioneCategoriaStatistica"] = getTextFromCombo('<%=rcbCategoriaStatisticaIngresso.ClientID%>');

            } catch (e) {
                alert(e.message);
            }
        }

        function rcbCodiceArticoloIngresso_OnClientDropDownOpening(sender, args) {
            clearComboRequestItems(sender);
        }

        function rcbCodiceArticoloUscita_OnClientItemsRequesting(sender, args) {
            try {
                var context = args.get_context();

                context["CodiceGruppo"] = getSelectedValueFromCombo('<%=rcbGruppoUscita.ClientID%>');
                context["DescrizioneGruppo"] = getTextFromCombo('<%=rcbGruppoUscita.ClientID%>');

                context["CodiceCategoria"] = getSelectedValueFromCombo('<%=rcbCategoriaUscita.ClientID%>');
                context["DescrizioneCategoria"] = getTextFromCombo('<%=rcbCategoriaUscita.ClientID%>');

                context["CodiceCategoriaStatistica"] = getSelectedValueFromCombo('<%=rcbCategoriaStatisticaUscita.ClientID%>');
                context["DescrizioneCategoriaStatistica"] = getTextFromCombo('<%=rcbCategoriaStatisticaUscita.ClientID%>');

            } catch (e) {
                alert(e.message);
            }
        }

        function rcbCodiceArticoloUscita_OnClientDropDownOpening(sender, args) {
            clearComboRequestItems(sender);
        }

        function rcbGruppoCategoriaCategoriaStatistica_OnClientDropDownOpening(sender, args) {
            clearComboRequestItems(sender);
        }

        function setSelectedValueFromCombo(comboClientId, value) {

            if (value == null) return;

            var combo = $find(comboClientId);

            if (combo != null && combo.findItemByValue) {
                var itemByValue = combo.findItemByValue(value);
                if (itemByValue != null) itemByValue.select();
            }
        }

        function getSelectedValueFromCombo(comboClientId) {
            var combo = $find(comboClientId);

            var valore = "";
            if (combo != null && combo.get_selectedItem) {
                var selectedItem = combo.get_selectedItem();
                if (selectedItem != null && selectedItem.get_value) {
                    valore = selectedItem.get_value();
                }

                if (valore == null || valore == "") {
                    valore = combo.get_value();
                }
            }

            if (valore == null) valore = "";

            if (valore != null && valore != "") {
                if (valore == combo.get_emptyMessage()) valore = "";
            }

            return valore;
        }

        function getTextFromCombo(comboClientId) {
            var combo = $find(comboClientId);

            var valore = "";
            if (combo != null && combo.get_selectedItem) {
                var selectedItem = combo.get_selectedItem();
                if (selectedItem != null && selectedItem.get_text) {
                    valore = selectedItem.get_text();
                }

                if (valore == null || valore == "") {
                    valore = combo.get_text();
                }
            }

            if (valore == null) valore = "";

            if (valore != null && valore != "") {
                if (valore == combo.get_emptyMessage()) valore = "";
            }

            return valore;
        }

        function clearComboRequestItems(combo) {
            try {

                if (combo == null) return;

                combo.requestItems("", false);
                combo.clearItems();
                //combo.clearSelection();
            } catch (e) {
                alert(e.message);
            }
        }

        var modificaManualeInCorso = false;

    function rntbQuantità_OnValueChanged(sender, args) {

        if (modificaManualeInCorso == true) return;

        modificaManualeInCorso = true;

        var rntbQuantità = $find('<%= rntbQuantità.ClientID %>');
        var rntbCosto = $find('<%= rntbCosto.ClientID %>');
        var rntbVendita = $find('<%= rntbVendita.ClientID %>');
        var rntbRicaricoValore = $find('<%= rntbRicaricoValore.ClientID %>');
        var rntbRicaricoPercentuale = $find('<%= rntbRicaricoPercentuale.ClientID %>');
        var rntbTotaleCosto = $find('<%= rntbTotaleCosto.ClientID %>');
        var rntbTotaleVendita = $find('<%= rntbTotaleVendita.ClientID %>');

        if (rntbQuantità != null && rntbCosto != null && rntbVendita != null && rntbRicaricoValore != null && rntbRicaricoPercentuale != null && rntbTotaleCosto != null && rntbTotaleVendita != null) {

            var quantitaValue = rntbQuantità.get_value();
            var costoValue = rntbCosto.get_value();
            var venditaValue = rntbVendita.get_value();

            if (quantitaValue != null && quantitaValue > 0) {
                if (costoValue != null && costoValue > 0) {
                    var totaleCosto = quantitaValue * costoValue;
                    rntbTotaleCosto.set_value(totaleCosto);
                }

                if (venditaValue != null && venditaValue > 0) {
                    var totaleVendita = quantitaValue * venditaValue;
                    rntbTotaleVendita.set_value(totaleVendita);
                }

                if (costoValue != null && costoValue > 0 &&
                    venditaValue != null && venditaValue > 0) {

                    var totaleRicavo = venditaValue - costoValue;
                    rntbRicaricoValore.set_value(totaleRicavo);

                    var totalePercentuale = calcolaRicaricoPercentuale(costoValue, venditaValue);
                    rntbRicaricoPercentuale.set_value(totalePercentuale);
                }
            }
            else {
                rntbTotaleCosto.set_value(0);
                rntbTotaleVendita.set_value(0);
            }
        }

        modificaManualeInCorso = false;
    }

    function rntbCosto_OnValueChanged(sender, args) {

        if (modificaManualeInCorso == true) return;

        modificaManualeInCorso = true;

        var rntbQuantità = $find('<%= rntbQuantità.ClientID %>');
        var rntbCosto = $find('<%= rntbCosto.ClientID %>');
        var rntbVendita = $find('<%= rntbVendita.ClientID %>');
        var rntbRicaricoValore = $find('<%= rntbRicaricoValore.ClientID %>');
        var rntbRicaricoPercentuale = $find('<%= rntbRicaricoPercentuale.ClientID %>');
        var rntbTotaleCosto = $find('<%= rntbTotaleCosto.ClientID %>');
        var rntbTotaleVendita = $find('<%= rntbTotaleVendita.ClientID %>');
        
        if (rntbQuantità != null && rntbCosto != null && rntbVendita != null && rntbRicaricoValore != null && rntbRicaricoPercentuale != null && rntbTotaleCosto != null && rntbTotaleVendita != null) {

            var quantitaValue = rntbQuantità.get_value();
            var costoValue = rntbCosto.get_value();
            var venditaValue = rntbVendita.get_value();
            var ricaricoValoreValue = rntbRicaricoValore.get_value();
            var ricaricoPercentualeValue = rntbRicaricoPercentuale.get_value();

            if (ricaricoValoreValue == null || ricaricoValoreValue == "") {
                rntbRicaricoValore.set_value(0);
                ricaricoValoreValue = 0;
            }

            if (ricaricoPercentualeValue == null || ricaricoPercentualeValue == "") {
                rntbRicaricoPercentuale.set_value(0);
                ricaricoPercentualeValue = 0;
            }

            if ((venditaValue == null  || venditaValue == "") && costoValue != null) {
                rntbVendita.set_value(costoValue);
                venditaValue = costoValue;
            }

            if (quantitaValue == null || quantitaValue == "") {
                rntbQuantità.set_value(0);
                quantitaValue = 0;
            }

            if (quantitaValue != null && quantitaValue > 0 &&
                costoValue != null && costoValue > 0) {

                var totaleCosto = quantitaValue * costoValue;
                rntbTotaleCosto.set_value(totaleCosto);

                if (venditaValue == "" && ricaricoValoreValue != "") {
                    var vendita = costoValue + ricaricoValoreValue;
                    rntbVendita.set_value(vendita);

                    var totaleVendita = vendita * quantitaValue;
                    rntbTotaleVendita.set_value(totaleVendita);

                    var percentuale = calcolaRicaricoPercentuale(costoValue, vendita);
                    rntbRicaricoPercentuale.set_value(percentuale);
                }
                else if (venditaValue == "" && ricaricoPercentualeValue != "") {

                    var ricaricoValoreValue = calcolaRicaricoValoreDaValoreIniziale(costoValue, ricaricoPercentualeValue);
                    var vendita = costoValue + ricaricoValoreValue;
                    rntbVendita.set_value(vendita);

                    var totaleVendita = vendita * quantitaValue;
                    rntbTotaleVendita.set_value(totaleVendita);

                    rntbRicaricoValore.set_value(ricaricoValoreValue);
                }
                else if (venditaValue > 0 && costoValue > 0) {
                    var totaleRicavo = venditaValue - costoValue;
                    rntbRicaricoValore.set_value(totaleRicavo);

                    var totalePercentuale = calcolaRicaricoPercentuale(costoValue, venditaValue);
                    rntbRicaricoPercentuale.set_value(totalePercentuale);
                }
            }
            else {
                rntbTotaleCosto.set_value(0);
                rntbTotaleVendita.set_value(0);
            }
        }

        modificaManualeInCorso = false;
    }

    

    function rntbVendita_OnValueChanged(sender, args) {

        if (modificaManualeInCorso == true) return;

        modificaManualeInCorso = true;

        var rntbQuantità = $find('<%= rntbQuantità.ClientID %>');
        var rntbCosto = $find('<%= rntbCosto.ClientID %>');
        var rntbVendita = $find('<%= rntbVendita.ClientID %>');
        var rntbRicaricoValore = $find('<%= rntbRicaricoValore.ClientID %>');
        var rntbRicaricoPercentuale = $find('<%= rntbRicaricoPercentuale.ClientID %>');
        var rntbTotaleCosto = $find('<%= rntbTotaleCosto.ClientID %>');
        var rntbTotaleVendita = $find('<%= rntbTotaleVendita.ClientID %>');

        if (rntbQuantità != null && rntbCosto != null && rntbVendita != null && rntbRicaricoValore != null && rntbRicaricoPercentuale != null && rntbTotaleCosto != null && rntbTotaleVendita != null) {

            var quantitaValue = rntbQuantità.get_value();
            var costoValue = rntbCosto.get_value();
            var venditaValue = rntbVendita.get_value();
            var ricaricoValoreValue = rntbRicaricoValore.get_value();
            var ricaricoPercentualeValue = rntbRicaricoPercentuale.get_value();

            if (quantitaValue != null && quantitaValue > 0 &&
                venditaValue != null && venditaValue > 0) {

                var totaleVendita = quantitaValue * venditaValue;
                rntbTotaleVendita.set_value(totaleVendita);

                if (costoValue == "" && ricaricoValoreValue != "") {
                    var costo = venditaValue - ricaricoValoreValue;
                    rntbCosto.set_value(costo);

                    var totaleCosto = costo * quantitaValue;
                    rntbTotaleCosto.set_value(totaleCosto);

                    var percentuale = calcolaRicaricoPercentuale(costo, venditaValue);
                    rntbRicaricoPercentuale.set_value(percentuale);
                }
                else if (costoValue == "" && ricaricoPercentualeValue != "") {

                    var ricaricoValoreValue = calcolaRicaricoValoreDaValoreFinale(venditaValue, ricaricoPercentualeValue);
                    var costo = venditaValue - ricaricoValoreValue;
                    rntbCosto.set_value(costo);

                    var totaleCosto = costo * quantitaValue;
                    rntbTotaleCosto.set_value(totaleCosto);

                    rntbRicaricoValore.set_value(ricaricoValoreValue);
                }
                else if (venditaValue > 0 && costoValue > 0) {
                    var totaleRicavo = venditaValue - costoValue;
                    rntbRicaricoValore.set_value(totaleRicavo);

                    var totalePercentuale = calcolaRicaricoPercentuale(costoValue, venditaValue);
                    rntbRicaricoPercentuale.set_value(totalePercentuale);
                }
            }
        }

        modificaManualeInCorso = false;
    }

    function rntbRicaricoValore_OnValueChanged(sender, args) {
        if (modificaManualeInCorso == true) return;

        modificaManualeInCorso = true;

        var rntbQuantità = $find('<%= rntbQuantità.ClientID %>');
        var rntbCosto = $find('<%= rntbCosto.ClientID %>');
        var rntbVendita = $find('<%= rntbVendita.ClientID %>');
        var rntbRicaricoValore = $find('<%= rntbRicaricoValore.ClientID %>');
        var rntbRicaricoPercentuale = $find('<%= rntbRicaricoPercentuale.ClientID %>');
        var rntbTotaleCosto = $find('<%= rntbTotaleCosto.ClientID %>');
        var rntbTotaleVendita = $find('<%= rntbTotaleVendita.ClientID %>');

        if (rntbQuantità != null && rntbCosto != null && rntbVendita != null && rntbRicaricoValore != null && rntbRicaricoPercentuale != null && rntbTotaleCosto != null && rntbTotaleVendita != null) {
            var quantitaValue = rntbQuantità.get_value();
            var costoValue = rntbCosto.get_value();
            var venditaValue = rntbVendita.get_value();
            var ricaricoValoreValue = rntbRicaricoValore.get_value();

            if (quantitaValue != "") {
                if (costoValue != "") {

                    var vendita = costoValue + ricaricoValoreValue;
                    rntbVendita.set_value(vendita);

                    var totaleVendita = vendita * quantitaValue;
                    rntbTotaleVendita.set_value(totaleVendita);

                    var percentuale = calcolaRicaricoPercentuale(costoValue, vendita);
                    rntbRicaricoPercentuale.set_value(percentuale);
                }
                else if (venditaValue != "") {

                    var totaleVendita = quantitaValue * venditaValue;
                    var costo = totaleVendita - ricaricoValoreValue;
                    rntbCosto.set_value(costo);

                    var totaleCosto = costo * quantitaValue;
                    rntbTotaleCosto.set_value(totaleCosto);

                    var percentuale = calcolaRicaricoPercentuale(costo, venditaValue);
                    rntbRicaricoPercentuale.set_value(percentuale);
                }
            }
        }

        modificaManualeInCorso = false;
    }

    function rntbRicaricoPercentuale_OnValueChanged(sender, args) {
        if (modificaManualeInCorso == true) return;

        modificaManualeInCorso = true;

        var rntbQuantità = $find('<%= rntbQuantità.ClientID %>');
        var rntbCosto = $find('<%= rntbCosto.ClientID %>');
        var rntbVendita = $find('<%= rntbVendita.ClientID %>');
        var rntbRicaricoValore = $find('<%= rntbRicaricoValore.ClientID %>');
        var rntbRicaricoPercentuale = $find('<%= rntbRicaricoPercentuale.ClientID %>');
        var rntbTotaleCosto = $find('<%= rntbTotaleCosto.ClientID %>');
        var rntbTotaleVendita = $find('<%= rntbTotaleVendita.ClientID %>');

        if (rntbQuantità != null && rntbCosto != null && rntbVendita != null && rntbRicaricoValore != null && rntbRicaricoPercentuale != null && rntbTotaleCosto != null && rntbTotaleVendita != null) {
            var quantitaValue = rntbQuantità.get_value();
            var costoValue = rntbCosto.get_value();
            var venditaValue = rntbVendita.get_value();
            var ricaricoValoreValue = rntbRicaricoValore.get_value();
            var ricaricoPercentualeValue = rntbRicaricoPercentuale.get_value();

            if (quantitaValue != "" && ricaricoPercentualeValue != "") {
                if (costoValue != "") {
                    
                    var ricaricoValoreValue = calcolaRicaricoValoreDaValoreIniziale(costoValue, ricaricoPercentualeValue);
                    var vendita = costoValue + ricaricoValoreValue;
                    rntbVendita.set_value(vendita);

                    var totaleVendita = vendita * quantitaValue;
                    rntbTotaleVendita.set_value(totaleVendita);

                    rntbRicaricoValore.set_value(ricaricoValoreValue);
                }
                else if (venditaValue != "") {

                    var ricaricoValoreValue = calcolaRicaricoValoreDaValoreFinale(venditaValue, ricaricoPercentualeValue);
                    var costo = venditaValue - ricaricoValoreValue;
                    rntbCosto.set_value(costo);

                    var totaleCosto = costo * quantitaValue;
                    rntbTotaleCosto.set_value(totaleCosto);

                    rntbRicaricoValore.set_value(ricaricoValoreValue);
                }
            }
        }

        modificaManualeInCorso = false;
    }

    function rntbTotaleCosto_OnValueChanged(sender, args) {
        var rntbQuantità = $find('<%= rntbQuantità.ClientID %>');
        var rntbCosto = $find('<%= rntbCosto.ClientID %>');
        var rntbVendita = $find('<%= rntbVendita.ClientID %>');
        var rntbRicaricoValore = $find('<%= rntbRicaricoValore.ClientID %>');
        var rntbRicaricoPercentuale = $find('<%= rntbRicaricoPercentuale.ClientID %>');
        var rntbTotaleCosto = $find('<%= rntbTotaleCosto.ClientID %>');
        var rntbTotaleVendita = $find('<%= rntbTotaleVendita.ClientID %>');

        if (rntbQuantità != null && rntbCosto != null && rntbVendita != null && rntbRicaricoValore != null && rntbRicaricoPercentuale != null && rntbTotaleCosto != null && rntbTotaleVendita != null) {
            var quantitaValue = rntbQuantità.get_value();
            var totaleCosto = rntbTotaleCosto.get_value();

            if (quantitaValue != "" && totaleCosto != "") {

                var costo = totaleCosto / quantitaValue;
                rntbCosto.set_value(costo);
            }
        }
    }

    function rntbTotaleVendita_OnValueChanged(sender, args) {
        var rntbQuantità = $find('<%= rntbQuantità.ClientID %>');
        var rntbCosto = $find('<%= rntbCosto.ClientID %>');
        var rntbVendita = $find('<%= rntbVendita.ClientID %>');
        var rntbRicaricoValore = $find('<%= rntbRicaricoValore.ClientID %>');
        var rntbRicaricoPercentuale = $find('<%= rntbRicaricoPercentuale.ClientID %>');
        var rntbTotaleCosto = $find('<%= rntbTotaleCosto.ClientID %>');
        var rntbTotaleVendita = $find('<%= rntbTotaleVendita.ClientID %>');

        if (rntbQuantità != null && rntbCosto != null && rntbVendita != null && rntbRicaricoValore != null && rntbRicaricoPercentuale != null && rntbTotaleCosto != null && rntbTotaleVendita != null) {
            var quantitaValue = rntbQuantità.get_value();
            var totaleVendita = rntbTotaleVendita.get_value();
            var costoValue = rntbCosto.get_value();

            if (quantitaValue != "" && totaleVendita != "") {
                var vendita = totaleVendita / quantitaValue;
                rntbVendita.set_value(vendita);

                if (costoValue == "") {
                    rntbCosto.set_value(vendita);
                }
            }
        }
    }

    function calcolaRicaricoValoreDaValoreIniziale(valoreIniziale, percentuale) {
        if (valoreIniziale == null || valoreIniziale == "") valoreIniziale = 0;
        if (percentuale == null || percentuale == "") percentuale = 100;

        var valore = (valoreIniziale / 100) * percentuale;
        return valore;
    }

    function calcolaRicaricoValoreDaValoreFinale(valoreFinale, percentuale) {

        if (valoreFinale == null || valoreFinale == "") valoreFinale = 0;
        if (percentuale == null || percentuale == "") percentuale = 100;

        var valore = Math.round((valoreFinale / 100) * percentuale);
        return valore;
    }

    function calcolaRicaricoPercentuale(valoreIniziale, valoreFinale) {
        if (valoreIniziale == null || valoreIniziale == "") valoreIniziale = 0;
        if (valoreFinale == null || valoreFinale == "") valoreFinale = 0;

        var incremento = valoreFinale - valoreIniziale;
        var aumento = 0;

        if (valoreFinale > 0) {
            aumento = (incremento / valoreIniziale) * 100;
        }

        return aumento;
    }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    <asp:Label ID="lblTitolo" runat="server" Text="Gestione Configurazione Articolo Aggiuntivo" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">
    <telerik:RadToolBar ID="RadToolBar1"
        runat="server"
        OnButtonClick="RadToolBar1_ButtonClick"
        OnClientButtonClicking="ToolBarButtonClicking">
        <Items>
            <telerik:RadToolBarButton runat="server" Value="TornaElenco" CommandName="TornaElenco" CausesValidation="False" NavigateUrl="ConfigurazioniArticoliAggiuntivi.aspx" PostBack="False" ImageUrl="~/UI/Images/Toolbar/back.png" Text="Vai all'Elenco" ToolTip="Vai alla pagina di elenco delle Configurazioni degli articoli aggiuntivi" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/refresh.png" PostBack="False" Value="Aggiorna" CommandName="Aggiorna" Text="Aggiorna" ToolTip="Aggiorna i dati visualizzati" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" NavigateUrl="ConfigurazioneArticoloAggiuntivo.aspx" ImageUrl="~/UI/Images/Toolbar/add.png" PostBack="False" CommandName="Nuovo" Value="Nuovo" Text="Nuova Configurazione Articolo Aggiuntivo" ToolTip="Permette la creazione di una nuova Configurazione di un articolo aggiuntivo" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreSalva" />
            <telerik:RadToolBarButton runat="server" CommandName="Salva" ImageUrl="~/UI/Images/Toolbar/Save.png" Text="Salva" Value="Salva" ToolTip="Memorizza i dati visualizzati" />
        </Items>
    </telerik:RadToolBar>

    <asp:ValidationSummary ID="PrincipalValidationSummary" runat="server"
        CssClass="ValidationSummaryStyle"
        HeaderText="&nbsp;Errori di validazione dei dati:"
        DisplayMode="BulletList"
        ShowMessageBox="false"
        ShowSummary="true" />

    <div class="pageBody">
        <telerik:RadAjaxPanel runat="server" LoadingPanelID="RadAjaxLoadingPanelMaster">
            <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                <Rows>
                    <telerik:LayoutRow RowType="Region">
                        <Columns>
                            <telerik:LayoutColumn Span="4" SpanXl="4" SpanLg="6" SpanMd="4" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                <asp:Label ID="lblTipologia" runat="server" Text="Tipologia" AssociatedControlID="rcbTipologia" Font-Bold="true" /><br />
                                <telerik:RadComboBox ID="rcbTipologia" runat="server" DataTextField="Value" DataValueField="Key" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="rcbTipologia_SelectedIndexChanged" CausesValidation="false" OnClientSelectedIndexChanging="rcbTipologia_OnClientSelectedIndexChanging" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow RowType="Region" CssClass="SpaziaturaSuperioreGruppo">
                        <Columns>
                            <telerik:LayoutColumn Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12">
                                <fieldset>
                                    <legend>
                                        <asp:Label ID="lblAllaSelezione" runat="server" Text="&nbsp;Alla seguente selezione...&nbsp;" Font-Bold="true" /></legend>
                                    <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                        <Rows>
                                            <telerik:LayoutRow RowType="Region">
                                                <Columns>
                                                    <telerik:LayoutColumn Span="3" SpanXl="3" SpanLg="3" SpanMd="3" SpanSm="12" SpanXs="12">
                                                        <asp:Label ID="lblGruppoIngresso" runat="server" Text="Gruppo" AssociatedControlID="rcbGruppoIngresso" /><br />
                                                        <telerik:RadComboBox runat="server" ID="rcbGruppoIngresso" DataValueField="CODICE" DataTextField="DESCRIZIONE" Width="100%" CausesValidation="false"
                                                            DropDownAutoWidth="Enabled"
                                                            MarkFirstMatch="false"
                                                            HighlightTemplatedItems="true"
                                                            EmptyMessage="Selezionare un gruppo"
                                                            ItemsPerRequest="20"
                                                            ShowMoreResultsBox="true"
                                                            EnableLoadOnDemand="true"
                                                            EnableItemCaching="false"
                                                            AllowCustomText="true"
                                                            LoadingMessage="Caricamento in corso..."
                                                            OnClientDropDownOpening="rcbGruppoCategoriaCategoriaStatistica_OnClientDropDownOpening"
                                                            OnClientItemsRequesting="rcbGruppoCategoriaCategoriaStatistica_Ingresso_OnClientItemsRequesting"
                                                            OnItemsRequested="rcbGruppo_ItemsRequested">
                                                        </telerik:RadComboBox>
                                                    </telerik:LayoutColumn>
                                                    <telerik:LayoutColumn Span="3" SpanXl="3" SpanLg="3" SpanMd="3" SpanSm="12" SpanXs="12">
                                                        <asp:Label ID="lblCategoriaIngresso" runat="server" Text="Categoria" AssociatedControlID="rcbCategoriaIngresso" /><br />
                                                        <telerik:RadComboBox runat="server" ID="rcbCategoriaIngresso" DataValueField="CODICE" DataTextField="DESCRIZIONE" Width="100%" CausesValidation="false"
                                                            DropDownAutoWidth="Enabled"
                                                            MarkFirstMatch="false"
                                                            HighlightTemplatedItems="true"
                                                            EmptyMessage="Selezionare una categoria"
                                                            ItemsPerRequest="20"
                                                            ShowMoreResultsBox="true"
                                                            EnableLoadOnDemand="true"
                                                            EnableItemCaching="false"
                                                            AllowCustomText="true"
                                                            LoadingMessage="Caricamento in corso..."
                                                            OnClientDropDownOpening="rcbGruppoCategoriaCategoriaStatistica_OnClientDropDownOpening"
                                                            OnClientItemsRequesting="rcbGruppoCategoriaCategoriaStatistica_Ingresso_OnClientItemsRequesting"
                                                            OnItemsRequested="rcbCategoria_ItemsRequested">
                                                        </telerik:RadComboBox>
                                                    </telerik:LayoutColumn>
                                                    <telerik:LayoutColumn Span="3" SpanXl="3" SpanLg="3" SpanMd="3" SpanSm="12" SpanXs="12">
                                                        <asp:Label ID="lblCategoriaStatisticaIngresso" runat="server" Text="Categoria statistica" AssociatedControlID="rcbCategoriaStatisticaIngresso" /><br />
                                                        <telerik:RadComboBox runat="server" ID="rcbCategoriaStatisticaIngresso" DataValueField="CODICE" DataTextField="DESCRIZIONE" Width="100%" CausesValidation="false"
                                                            DropDownAutoWidth="Enabled"
                                                            MarkFirstMatch="false"
                                                            HighlightTemplatedItems="true"
                                                            EmptyMessage="Selezionare una categoria statistica"
                                                            ItemsPerRequest="20"
                                                            ShowMoreResultsBox="true"
                                                            EnableLoadOnDemand="true"
                                                            EnableItemCaching="false"
                                                            AllowCustomText="true"
                                                            LoadingMessage="Caricamento in corso..."
                                                            OnClientDropDownOpening="rcbGruppoCategoriaCategoriaStatistica_OnClientDropDownOpening"
                                                            OnClientItemsRequesting="rcbGruppoCategoriaCategoriaStatistica_Ingresso_OnClientItemsRequesting"
                                                            OnItemsRequested="rcbCategoriaStatistica_ItemsRequested">
                                                        </telerik:RadComboBox>
                                                    </telerik:LayoutColumn>
                                                    <telerik:LayoutColumn Span="3" SpanXl="3" SpanLg="3" SpanMd="3" SpanSm="12" SpanXs="12">
                                                        <asp:Label ID="lblCodiceArticoloIngresso" runat="server" Text="Codice Articolo" AssociatedControlID="rcbCodiceArticoloIngresso" /><br />
                                                        <telerik:RadComboBox ID="rcbCodiceArticoloIngresso" runat="server"
                                                            Width="100%"
                                                            DropDownAutoWidth="Enabled"
                                                            MarkFirstMatch="false"
                                                            HighlightTemplatedItems="true"
                                                            EmptyMessage="Selezionare un Articolo"
                                                            ItemsPerRequest="20"
                                                            ShowMoreResultsBox="true"
                                                            EnableLoadOnDemand="true"
                                                            AllowCustomText="true"
                                                            LoadingMessage="Caricamento in corso..."
                                                            DataValueField="CodiceArticolo"
                                                            DataTextField="Descrizione"
                                                            EnableVirtualScrolling="true"
                                                            OnClientDropDownOpening="rcbCodiceArticoloIngresso_OnClientDropDownOpening"
                                                            OnClientItemsRequesting="rcbCodiceArticoloIngresso_OnClientItemsRequesting"
                                                            OnItemsRequested="rcbCodiceArticolo_ItemsRequested">
                                                            <HeaderTemplate>
                                                                <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                                                    <Rows>
                                                                        <telerik:LayoutRow RowType="Region">
                                                                            <Columns>
                                                                                <telerik:LayoutColumn Span="4">
                                                                                    <asp:Label runat="server" Text="Codice" Font-Bold="true" />
                                                                                </telerik:LayoutColumn>
                                                                                <telerik:LayoutColumn Span="8">
                                                                                    <asp:Label runat="server" Text="Descrizione" Font-Bold="true" />
                                                                                </telerik:LayoutColumn>
                                                                            </Columns>
                                                                        </telerik:LayoutRow>
                                                                    </Rows>
                                                                </telerik:RadPageLayout>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                                                    <Rows>
                                                                        <telerik:LayoutRow RowType="Region">
                                                                            <Columns>
                                                                                <telerik:LayoutColumn Span="4">
                                                                                    <%# DataBinder.Eval(Container.DataItem, "CODICE") %>
                                                                                </telerik:LayoutColumn>
                                                                                <telerik:LayoutColumn Span="8">
                                                                                    <%# DataBinder.Eval(Container.DataItem, "DESCRIZIONE") %>
                                                                                </telerik:LayoutColumn>
                                                                            </Columns>
                                                                        </telerik:LayoutRow>
                                                                    </Rows>
                                                                </telerik:RadPageLayout>
                                                            </ItemTemplate>
                                                        </telerik:RadComboBox>
                                                    </telerik:LayoutColumn>
                                                </Columns>
                                            </telerik:LayoutRow>
                                        </Rows>
                                    </telerik:RadPageLayout>
                                </fieldset>
                            </telerik:LayoutColumn>

                            <telerik:LayoutColumn ID="ColonnaTestoAvviso" runat="server" Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreGruppo">
                                <fieldset>
                                    <legend>
                                        <asp:Label ID="lblTstoMessaggio" runat="server" Text="&nbsp;Viene mostrato il seguente messaggio ...&nbsp;" Font-Bold="true" /></legend>
                                    <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                        <Rows>
                                            <telerik:LayoutRow RowType="Region">
                                                <Columns>
                                                    <telerik:LayoutColumn Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                                        <asp:Label ID="lblTestoAvviso" runat="server" Text="Testo dell'avviso" AssociatedControlID="rtbTestoAvviso" Font-Bold="true" />
                                                        <asp:RequiredFieldValidator ID="rfvTestoAvviso" runat="server" Display="Dynamic"
                                                            ControlToValidate="rtbTestoAvviso"
                                                            ErrorMessage="Il testo dell'avviso è obbligatorio."
                                                            ForeColor="Red">
                                                            <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                                        </asp:RequiredFieldValidator>
                                                        <br />
                                                        <telerik:RadTextBox ID="rtbTestoAvviso" runat="server" EmptyMessage="Inserire il testo dell'avviso da mostrare" Rows="3" TextMode="MultiLine" Width="100%" />
                                                    </telerik:LayoutColumn>
                                                </Columns>
                                            </telerik:LayoutRow>
                                        </Rows>
                                    </telerik:RadPageLayout>
                                </fieldset>
                            </telerik:LayoutColumn>

                            <telerik:LayoutColumn ID="ColonnaArticoloAggiuntivo" runat="server" Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreGruppo">
                                <fieldset>
                                    <legend>
                                        <asp:Label ID="lblVieneAggiuntoUnArticolo" runat="server" Text="&nbsp;Viene aggiunto un articolo con...&nbsp;" Font-Bold="true" /></legend>
                                    <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                        <Rows>
                                            <telerik:LayoutRow RowType="Region">
                                                <Columns>
                                                    <telerik:LayoutColumn Span="3" SpanXl="3" SpanLg="3" SpanMd="3" SpanSm="12" SpanXs="12">
                                                        <asp:Label ID="lblGruppoUscita" runat="server" Text="Gruppo" AssociatedControlID="rcbGruppoUscita" /><br />
                                                        <telerik:RadComboBox runat="server" ID="rcbGruppoUscita" DataValueField="CODICE" DataTextField="DESCRIZIONE" Width="100%" CausesValidation="false"
                                                            DropDownAutoWidth="Enabled"
                                                            MarkFirstMatch="false"
                                                            HighlightTemplatedItems="true"
                                                            EmptyMessage="Selezionare un gruppo"
                                                            ItemsPerRequest="20"
                                                            ShowMoreResultsBox="true"
                                                            EnableLoadOnDemand="true"
                                                            EnableItemCaching="false"
                                                            AllowCustomText="true"
                                                            LoadingMessage="Caricamento in corso..."
                                                            OnClientDropDownOpening="rcbGruppoCategoriaCategoriaStatistica_OnClientDropDownOpening"
                                                            OnClientItemsRequesting="rcbGruppoCategoriaCategoriaStatistica_Uscita_OnClientItemsRequesting"
                                                            OnItemsRequested="rcbGruppo_ItemsRequested">
                                                        </telerik:RadComboBox>
                                                    </telerik:LayoutColumn>
                                                    <telerik:LayoutColumn Span="3" SpanXl="3" SpanLg="3" SpanMd="3" SpanSm="12" SpanXs="12">
                                                        <asp:Label ID="lblCategoriaUscita" runat="server" Text="Categoria" AssociatedControlID="rcbCategoriaUscita" /><br />
                                                        <telerik:RadComboBox runat="server" ID="rcbCategoriaUscita" DataValueField="CODICE" DataTextField="DESCRIZIONE" Width="100%" CausesValidation="false"
                                                            DropDownAutoWidth="Enabled"
                                                            MarkFirstMatch="false"
                                                            HighlightTemplatedItems="true"
                                                            EmptyMessage="Selezionare una categoria"
                                                            ItemsPerRequest="20"
                                                            ShowMoreResultsBox="true"
                                                            EnableLoadOnDemand="true"
                                                            EnableItemCaching="false"
                                                            AllowCustomText="true"
                                                            LoadingMessage="Caricamento in corso..."
                                                            OnClientDropDownOpening="rcbGruppoCategoriaCategoriaStatistica_OnClientDropDownOpening"
                                                            OnClientItemsRequesting="rcbGruppoCategoriaCategoriaStatistica_Uscita_OnClientItemsRequesting"
                                                            OnItemsRequested="rcbCategoria_ItemsRequested">
                                                        </telerik:RadComboBox>
                                                    </telerik:LayoutColumn>
                                                    <telerik:LayoutColumn Span="3" SpanXl="3" SpanLg="3" SpanMd="3" SpanSm="12" SpanXs="12">
                                                        <asp:Label ID="lblCategoriaStatisticaUscita" runat="server" Text="Categoria statistica" AssociatedControlID="rcbCategoriaStatisticaUscita" /><br />
                                                        <telerik:RadComboBox runat="server" ID="rcbCategoriaStatisticaUscita" DataValueField="CODICE" DataTextField="DESCRIZIONE" Width="100%" CausesValidation="false"
                                                            DropDownAutoWidth="Enabled"
                                                            MarkFirstMatch="false"
                                                            HighlightTemplatedItems="true"
                                                            EmptyMessage="Selezionare una categoria statistica"
                                                            ItemsPerRequest="20"
                                                            ShowMoreResultsBox="true"
                                                            EnableLoadOnDemand="true"
                                                            EnableItemCaching="false"
                                                            AllowCustomText="true"
                                                            LoadingMessage="Caricamento in corso..."
                                                            OnClientDropDownOpening="rcbGruppoCategoriaCategoriaStatistica_OnClientDropDownOpening"
                                                            OnClientItemsRequesting="rcbGruppoCategoriaCategoriaStatistica_Uscita_OnClientItemsRequesting"
                                                            OnItemsRequested="rcbCategoriaStatistica_ItemsRequested">
                                                        </telerik:RadComboBox>
                                                    </telerik:LayoutColumn>
                                                    <telerik:LayoutColumn Span="3" SpanXl="3" SpanLg="3" SpanMd="3" SpanSm="12" SpanXs="12">
                                                        <asp:Label ID="lblCodiceArticoloUscita" runat="server" Text="Codice Articolo" AssociatedControlID="rcbCodiceArticoloUscita" Font-Bold="true" />
                                                        <asp:RequiredFieldValidator ID="rfvCodiceArticoloUscita" runat="server" Display="Dynamic"
                                                            ControlToValidate="rcbCodiceArticoloUscita"
                                                            ErrorMessage="Il Codice Articolo è obbligatorio."
                                                            ForeColor="Red">
                                                            <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                                        </asp:RequiredFieldValidator>
                                                        <br />
                                                        <telerik:RadComboBox ID="rcbCodiceArticoloUscita" runat="server"
                                                            Width="100%"
                                                            DropDownAutoWidth="Enabled"
                                                            MarkFirstMatch="false"
                                                            HighlightTemplatedItems="true"
                                                            EmptyMessage="Selezionare un Articolo"
                                                            ItemsPerRequest="20"
                                                            ShowMoreResultsBox="true"
                                                            EnableLoadOnDemand="true"
                                                            EnableItemCaching="false"
                                                            AllowCustomText="true"
                                                            LoadingMessage="Caricamento in corso..."
                                                            DataValueField="CodiceArticolo"
                                                            DataTextField="Descrizione"
                                                            EnableVirtualScrolling="true"
                                                            OnClientDropDownOpening="rcbCodiceArticoloUscita_OnClientDropDownOpening"
                                                            OnClientItemsRequesting="rcbGruppoCategoriaCategoriaStatistica_Uscita_OnClientItemsRequesting"
                                                            OnItemsRequested="rcbCodiceArticolo_ItemsRequested">
                                                            <HeaderTemplate>
                                                                <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                                                    <Rows>
                                                                        <telerik:LayoutRow RowType="Region">
                                                                            <Columns>
                                                                                <telerik:LayoutColumn Span="4">
                                                                                    <asp:Label runat="server" Text="Codice" Font-Bold="true" />
                                                                                </telerik:LayoutColumn>
                                                                                <telerik:LayoutColumn Span="8">
                                                                                    <asp:Label runat="server" Text="Descrizione" Font-Bold="true" />
                                                                                </telerik:LayoutColumn>
                                                                            </Columns>
                                                                        </telerik:LayoutRow>
                                                                    </Rows>
                                                                </telerik:RadPageLayout>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                                                    <Rows>
                                                                        <telerik:LayoutRow RowType="Region">
                                                                            <Columns>
                                                                                <telerik:LayoutColumn Span="4">
                                                                                    <%# DataBinder.Eval(Container.DataItem, "CODICE") %>
                                                                                </telerik:LayoutColumn>
                                                                                <telerik:LayoutColumn Span="8">
                                                                                    <%# DataBinder.Eval(Container.DataItem, "DESCRIZIONE") %>
                                                                                </telerik:LayoutColumn>
                                                                            </Columns>
                                                                        </telerik:LayoutRow>
                                                                    </Rows>
                                                                </telerik:RadPageLayout>
                                                            </ItemTemplate>
                                                        </telerik:RadComboBox>
                                                    </telerik:LayoutColumn>
                                                </Columns>
                                            </telerik:LayoutRow>
                                            <telerik:LayoutRow ID="rigaInformazioniAggiuntiveArticolo" RowType="Region" CssClass="SpaziaturaSuperioreRiga">
                                                <Columns>
                                                    <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="1" SpanMd="4" SpanSm="12" SpanXs="12">
                                                    <asp:Label ID="lblUM" runat="server" Text="UM" AssociatedControlID="rtbUM" />
                                                    <br />
                                                    <telerik:RadTextBox runat="server" ID="rtbUM" Width="100%"></telerik:RadTextBox>
                                                </telerik:LayoutColumn>
                                                <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="1" SpanMd="4" SpanSm="12" SpanXs="12">
                                                    <asp:Label ID="lblQuantità" runat="server" Text="Q.tà" AssociatedControlID="rntbQuantità" />
                                                    <br />
                                                    <telerik:RadNumericTextBox runat="server" ID="rntbQuantità" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" MinValue="0">
                                                        <ClientEvents OnValueChanged="rntbQuantità_OnValueChanged" />
                                                    </telerik:RadNumericTextBox>
                                                </telerik:LayoutColumn>
                                                    <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="1" SpanMd="4" SpanSm="6" SpanXs="12">
                                                        <asp:Label ID="lblCosto" runat="server" Text="Costo unitario" AssociatedControlID="rntbCosto" />
                                                        <br />
                                                        <telerik:RadNumericTextBox runat="server" ID="rntbCosto" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2">
                                                            <ClientEvents OnValueChanged="rntbCosto_OnValueChanged" />
                                                        </telerik:RadNumericTextBox>
                                                    </telerik:LayoutColumn>
                                                    <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="12">
                                                        <asp:Label ID="lblRicaricoValore" runat="server" Text="Ricarico € (su costo unitario)" AssociatedControlID="rntbRicaricoValore" /><br />
                                                        <telerik:RadNumericTextBox runat="server" ID="rntbRicaricoValore" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2">
                                                            <ClientEvents OnValueChanged="rntbRicaricoValore_OnValueChanged" />
                                                        </telerik:RadNumericTextBox>
                                                    </telerik:LayoutColumn>
                                                    <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="12">
                                                        <asp:Label ID="lblRicaricoPercentuale" runat="server" Text="Ricarico % (su costo unitario)" AssociatedControlID="rntbRicaricoPercentuale" /><br />
                                                        <telerik:RadNumericTextBox runat="server" ID="rntbRicaricoPercentuale" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2">
                                                            <ClientEvents OnValueChanged="rntbRicaricoPercentuale_OnValueChanged" />
                                                        </telerik:RadNumericTextBox>
                                                    </telerik:LayoutColumn>
                                                    <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="1" SpanMd="4" SpanSm="6" SpanXs="12">
                                                        <asp:Label ID="lblVendita" runat="server" Text="Prezzo unitario" AssociatedControlID="rntbVendita" />
                                                        <br />
                                                        <telerik:RadNumericTextBox runat="server" ID="rntbVendita" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2">
                                                            <ClientEvents OnValueChanged="rntbVendita_OnValueChanged" />
                                                        </telerik:RadNumericTextBox>
                                                    </telerik:LayoutColumn>
                                                    <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="12">
                                                        <asp:Label ID="lblTotaleCosto" runat="server" Text="Totale costo" AssociatedControlID="rntbTotaleCosto" />
                                                        <br />
                                                        <telerik:RadNumericTextBox runat="server" ID="rntbTotaleCosto" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2">
                                                            <ClientEvents OnValueChanged="rntbTotaleCosto_OnValueChanged" />
                                                        </telerik:RadNumericTextBox>
                                                    </telerik:LayoutColumn>
                                                    <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="12">
                                                        <asp:Label ID="lblTotaleVendita" runat="server" Text="Totale vendita" AssociatedControlID="rntbTotaleVendita" />
                                                        <br />
                                                        <telerik:RadNumericTextBox runat="server" ID="rntbTotaleVendita" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2">
                                                            <ClientEvents OnValueChanged="rntbTotaleVendita_OnValueChanged" />
                                                        </telerik:RadNumericTextBox>
                                                    </telerik:LayoutColumn>
                                                </Columns>
                                            </telerik:LayoutRow>
                                        </Rows>
                                    </telerik:RadPageLayout>
                                </fieldset>
                            </telerik:LayoutColumn>

                            <telerik:LayoutColumn ID="ColonnaTemplateHtmlDescrizionePreventivo" runat="server" Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreGruppo">
                                <fieldset>
                                    <legend>
                                        <asp:Label ID="lblContenutoTemplateDescrizionePreventivo" runat="server" Text="&nbsp;Viene inserito nel preventivo il seguente contenuto...&nbsp;" Font-Bold="true" /></legend>
                                    <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                        <Rows>
                                            <telerik:LayoutRow RowType="Region">
                                                <Columns>
                                                    <telerik:LayoutColumn Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12">
                                                        <asp:Label ID="lblTemplateDescrizionePreventivoHtml" runat="server" Text="Template Descrizione Preventivo" Font-Bold="true" AssociatedControlID="reTemplateDescrizionePreventivo" />
                                                        <asp:RequiredFieldValidator ID="rfvTemplateDescrizionePreventivoHtml" runat="server" Display="Dynamic"
                                                            ControlToValidate="reTemplateDescrizionePreventivo"
                                                            ErrorMessage="Il contenuto del template per la descrizione del preventivo è obbligatorio."
                                                            ForeColor="Red">
                                                            <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                                        </asp:RequiredFieldValidator>
                                                        <br />
                                                        <telerik:RadEditor runat="server" ID="reTemplateDescrizionePreventivo" Height="500px" ToolsFile="~/App_Data/Editor/Settings/Tools.xml" ToolbarMode="Default" Width="100%">
                                                        </telerik:RadEditor>
                                                    </telerik:LayoutColumn>
                                                </Columns>
                                            </telerik:LayoutRow>
                                        </Rows>
                                    </telerik:RadPageLayout>
                                </fieldset>
                            </telerik:LayoutColumn>

                            <telerik:LayoutColumn ID="ColonnaTemplateContrattoHtml" runat="server" Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreGruppo">
                                <fieldset>
                                    <legend>
                                        <asp:Label ID="lblContenutoTemplateContratto" runat="server" Text="&nbsp;Viene inserito nel contratto il seguente contenuto...&nbsp;" Font-Bold="true" /></legend>
                                    <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                        <Rows>
                                            <telerik:LayoutRow RowType="Region">
                                                <Columns>
                                                    <telerik:LayoutColumn Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12">
                                                        <asp:Label ID="lblTemplateContrattoHtml" runat="server" Text="Template Contratto" Font-Bold="true" AssociatedControlID="reTemplateContratto" />
                                                        <asp:RequiredFieldValidator ID="rfvTemplateContrattoHtml" runat="server" Display="Dynamic"
                                                            ControlToValidate="reTemplateContratto"
                                                            ErrorMessage="Il contenuto del template per il contratto è obbligatorio."
                                                            ForeColor="Red">
                                                            <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                                        </asp:RequiredFieldValidator>
                                                        <br />
                                                        <telerik:RadEditor runat="server" ID="reTemplateContratto" Height="500px" ToolsFile="~/App_Data/Editor/Settings/Tools.xml" ToolbarMode="Default" Width="100%">
                                                        </telerik:RadEditor>
                                                    </telerik:LayoutColumn>
                                                </Columns>
                                            </telerik:LayoutRow>
                                        </Rows>
                                    </telerik:RadPageLayout>
                                </fieldset>
                            </telerik:LayoutColumn>

                            <telerik:LayoutColumn ID="ColonnaClausoleVessatorie" runat="server" Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreGruppo">
                                <fieldset>
                                    <legend>
                                        <asp:Label ID="lblClausoleVessatorie" runat="server" Text="&nbsp;Vengono inserite le clausole vessatorie selezionate tra quelle di seguito...&nbsp;" Font-Bold="true" /></legend>
                                    <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                        <Rows>
                                            <telerik:LayoutRow RowType="Region">
                                                <Columns>
                                                    <telerik:LayoutColumn Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12">
                                                        <telerik:RadGrid runat="server"
                                                            ID="rgGrigliaClausoleVessatorie"
                                                            CssClass="GridResponsiveColumns"
                                                            AllowPaging="false"
                                                            PageSize="10"
                                                            GridLines="None"
                                                            SortingSettings-SortToolTip="Clicca qui per ordinare i dati in base a questa colonna"
                                                            AutoGenerateColumns="false"
                                                            OnPreRender="rgGrigliaClausoleVessatorie_PreRender"
                                                            OnNeedDataSource="rgGrigliaClausoleVessatorie_NeedDataSource"
                                                            OnItemCreated="rgGrigliaClausoleVessatorie_ItemCreated"
                                                            OnItemDataBound="rgGrigliaClausoleVessatorie_ItemDataBound"
                                                            PagerStyle-FirstPageToolTip="Prima Pagina"
                                                            PagerStyle-LastPageToolTip="Ultima Pagina"
                                                            PagerStyle-PrevPageToolTip="Pagina Precedente"
                                                            PagerStyle-NextPageToolTip="Pagina Successiva"
                                                            PagerStyle-PagerTextFormat="{4} Elementi da {2} a {3} su {5}, Pagina {0} di {1}">
                                                            <MasterTableView DataKeyNames="ID" TableLayout="Fixed"
                                                                Width="100%"
                                                                AllowFilteringByColumn="false"
                                                                AllowSorting="true"
                                                                AllowMultiColumnSorting="false"
                                                                GridLines="Both"
                                                                NoMasterRecordsText="Nessun dato da visualizzare"
                                                                CommandItemDisplay="None">

                                                                <Columns>
                                                                    <telerik:GridTemplateColumn
                                                                        HeaderStyle-Width="45"
                                                                        ItemStyle-Width="45"
                                                                        HeaderStyle-HorizontalAlign="Center"
                                                                        ItemStyle-HorizontalAlign="Center"
                                                                        Resizable="false"
                                                                        AllowFiltering="false"
                                                                        AllowSorting="false"
                                                                        Exportable="false">
                                                                        <ItemTemplate>
                                                                            <telerik:RadCheckBox ID="rchkClausolaVessatoriaSelezionata" runat="server" AutoPostBack="false" CausesValidation="false" />
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridBoundColumn
                                                                        FilterControlWidth="70%"
                                                                        HeaderStyle-Width="100px"
                                                                        DataField="Codice"
                                                                        CurrentFilterFunction="EqualTo"
                                                                        AutoPostBackOnFilter="true"
                                                                        HeaderText="Codice" />

                                                                    <telerik:GridBoundColumn ItemStyle-Wrap="true"
                                                                        FilterControlWidth="80%"
                                                                        CurrentFilterFunction="Contains"
                                                                        AutoPostBackOnFilter="true"
                                                                        DataField="Descrizione"
                                                                        HeaderText="Descrizione" />

                                                                </Columns>

                                                            </MasterTableView>

                                                            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="false" />
                                                            <GroupingSettings CaseSensitive="false" />

                                                            <ClientSettings EnableRowHoverStyle="true">
                                                                <Selecting AllowRowSelect="false" />
                                                                <Resizing AllowColumnResize="false" EnableRealTimeResize="false" AllowResizeToFit="false" />
                                                            </ClientSettings>

                                                        </telerik:RadGrid>
                                                    </telerik:LayoutColumn>
                                                </Columns>
                                            </telerik:LayoutRow>
                                        </Rows>
                                    </telerik:RadPageLayout>
                                </fieldset>
                            </telerik:LayoutColumn>

                            <telerik:LayoutColumn ID="ColonnaClausoleVessatorieAggiuntive" runat="server" Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreGruppo">
                                <fieldset>
                                    <legend>
                                        <asp:Label ID="lblContenutoClausoleVessatorieAggiuntive" runat="server" Text="&nbsp;Vengono inserite le seguenti clausole vessatorie aggiuntive ...&nbsp;" Font-Bold="true" /></legend>
                                    <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                        <Rows>
                                            <telerik:LayoutRow RowType="Region">
                                                <Columns>
                                                    <telerik:LayoutColumn Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12">
                                                        <asp:Label ID="Label2" runat="server" Text="Condizioni Vessatorie Aggiuntive" AssociatedControlID="reClausoleVessatorieAggiuntive" /><br />
                                                        <telerik:RadEditor runat="server" ID="reClausoleVessatorieAggiuntive" Height="300px" ToolsFile="~/App_Data/Editor/Settings/Tools.xml" ToolbarMode="Default" Width="100%">
                                                        </telerik:RadEditor>
                                                    </telerik:LayoutColumn>
                                                </Columns>
                                            </telerik:LayoutRow>
                                        </Rows>
                                    </telerik:RadPageLayout>
                                </fieldset>
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                </Rows>
            </telerik:RadPageLayout>
        </telerik:RadAjaxPanel>
    </div>

</asp:Content>
