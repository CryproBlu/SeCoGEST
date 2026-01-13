<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OffertaRaggruppamentoEditItem.ascx.cs" Inherits="SeCoGEST.Web.Offerte.OffertaRaggruppamentoEditItem" %>
<%@ Register Src="~/Offerte/SpeseAccessorieArticoloOfferta.ascx" TagPrefix="uc1" TagName="SpeseAccessorieArticoloOfferta" %>


<script type="text/javascript">

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

    function rcbCodiceArticolo_OnClientSelectedIndexChanged(sender, args) {
        try {
            var item = args.get_item();
            if (item != null) {
                var attributes = item.get_attributes();

                debugger;

                if (attributes != null) {
                    var DescrizioneArticolo = attributes.getAttribute("DescrizioneArticolo");
                    if (DescrizioneArticolo != null && DescrizioneArticolo != "") {
                        var rtbDescrizione = $find('<%=rtbDescrizione.ClientID%>');
                        if (rtbDescrizione != null) {
                            rtbDescrizione.set_value(DescrizioneArticolo);
                        }
                    }

                    var CodiceGruppo = attributes.getAttribute("CodiceGruppo");
                    var DescrizioneGruppo = attributes.getAttribute("DescrizioneGruppo");
                    if (CodiceGruppo != null && CodiceGruppo != "" &&
                        DescrizioneGruppo != null && DescrizioneGruppo != "") {

                        var rcbGruppo = $find('<%=rcbGruppo.ClientID%>');
                        if (rcbGruppo != null) {
                            rcbGruppo.set_value(CodiceGruppo);
                            rcbGruppo.set_text(DescrizioneGruppo);
                            rcbGruppo.trackChanges();
                            rcbGruppo.commitChanges();
                        }
                    }

                    var CodiceCategoria = attributes.getAttribute("CodiceCategoria");
                    var DescrizioneCategoria = attributes.getAttribute("DescrizioneCategoria");
                    if (CodiceCategoria != null && CodiceCategoria != "" && 
                        DescrizioneCategoria != null && DescrizioneCategoria != "") {

                        var rcbCategoria = $find('<%=rcbCategoria.ClientID%>');
                        if (rcbCategoria != null) {
                            rcbCategoria.set_value(CodiceCategoria);
                            rcbCategoria.set_text(DescrizioneCategoria);
                            rcbCategoria.trackChanges();
                            rcbCategoria.commitChanges();
                        }
                    }

                    var CodiceCategoriaStatistica = attributes.getAttribute("CodiceCategoriaStatistica");
                    var DescrizioneCategoriaStatistica = attributes.getAttribute("DescrizioneCategoriaStatistica");
                    if (CodiceCategoriaStatistica != null && CodiceCategoriaStatistica != "" &&
                        DescrizioneCategoriaStatistica != null && DescrizioneCategoriaStatistica != "") {

                        var rcbCategoriaStatistica = $find('<%=rcbCategoriaStatistica.ClientID%>');
                        if (rcbCategoriaStatistica != null) {
                            rcbCategoriaStatistica.set_value(CodiceCategoriaStatistica);
                            rcbCategoriaStatistica.set_text(DescrizioneCategoriaStatistica);
                            rcbCategoriaStatistica.trackChanges();
                            rcbCategoriaStatistica.commitChanges();
                        }
                    }
                }
            }
        } catch (e) {
            alert(e.message);
        }
    }

    function rcbGruppo_OnClientSelectedIndexChanged(sender, args) {
        clearComboRequestItemsByComboClientId('<%=rcbCategoria.ClientID%>');
        clearComboRequestItemsByComboClientId('<%=rcbCategoriaStatistica.ClientID%>');
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

    function clearComboRequestItemsByComboClientId(comboClientId) {
        try {
            var combo = $find(comboClientId);
            clearComboRequestItems(combo);

            //if (combo != null) {
            //    combo.set_text("");
            //    combo.set_value("");
            //}
        } catch (e) {
            alert(e.message);
        }
    }

    function rcbCodiceArticolo_OnClientDropDownOpening(sender, args) {
        clearComboRequestItems(sender);
    }

    function rcbCodiceArticolo_OnClientItemsRequesting(sender, args) {
        try {
            var context = args.get_context();

            context["CodiceGruppo"] = getSelectedValueFromCombo('<%=rcbGruppo.ClientID%>');
            context["CodiceCategoria"] = getSelectedValueFromCombo('<%=rcbCategoria.ClientID%>');
            context["CodiceCategoriaStatistica"] = getSelectedValueFromCombo('<%=rcbCategoriaStatistica.ClientID%>');

        } catch (e) {
            alert(e.message);
        }
    }

    function rcbGruppoCategoriaCategoriaStatistica_OnClientDropDownOpening(sender, args) {
        try {
            sender.requestItems("", false);
            sender.clearItems();

        } catch (e) {
            alert(e.message);
        }
    }

    function rcbGruppoCategoriaCategoriaStatistica_OnClientItemsRequesting(sender, args) {
        try {
            var context = args.get_context();

            context["CodiceGruppo"] = getSelectedValueFromCombo('<%=rcbGruppo.ClientID%>');
            context["DescrizioneGruppo"] = getTextFromCombo('<%=rcbGruppo.ClientID%>');

            context["CodiceCategoria"] = getSelectedValueFromCombo('<%=rcbCategoria.ClientID%>');
            context["DescrizioneCategoria"] = getTextFromCombo('<%=rcbCategoria.ClientID%>');

            context["CodiceCategoriaStatistica"] = getSelectedValueFromCombo('<%=rcbCategoriaStatistica.ClientID%>');            
            context["DescrizioneCategoriaStatistica"] = getTextFromCombo('<%=rcbCategoriaStatistica.ClientID%>');

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

<telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
    <Rows>
        <telerik:LayoutRow RowType="Row" CssClass="OffertaRaggruppamentoExpanderItem SpaziaturaSuperioreGruppo">
            <Columns>
                <telerik:LayoutColumn Span="4" SpanXl="4" SpanLg="4" SpanMd="4" SpanSm="12" SpanXs="12">
                    <asp:Label ID="lblGruppo" runat="server" Text="Gruppo" Font-Bold="true" AssociatedControlID="rcbGruppo" />
                    <asp:RequiredFieldValidator ID="rfvGruppo" runat="server" Display="Dynamic"
                        ControlToValidate="rcbGruppo"
                        ErrorMessage="Il gruppo è obbligatorio."
                        ForeColor="Red">
                        <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                    </asp:RequiredFieldValidator>
                    <br />
                    <telerik:RadComboBox runat="server" ID="rcbGruppo" DataValueField="CODICE" DataTextField="DESCRIZIONE" Width="100%" CausesValidation="false"
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
                        OnClientItemsRequesting="rcbGruppoCategoriaCategoriaStatistica_OnClientItemsRequesting"
                        OnClientSelectedIndexChanged="rcbGruppo_OnClientSelectedIndexChanged"
                        OnItemsRequested="rcbGruppo_ItemsRequested">
                    </telerik:RadComboBox>
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="4" SpanXl="4" SpanLg="4" SpanMd="4" SpanSm="12" SpanXs="12">
                    <asp:Label ID="lblCategoria" runat="server" Text="Categoria" Font-Bold="true" AssociatedControlID="rcbCategoria" />
                    <asp:RequiredFieldValidator ID="rfvCategoria" runat="server" Display="Dynamic"
                        ControlToValidate="rcbCategoria"
                        ErrorMessage="La categoria è obbligatoria."
                        ForeColor="Red">
                        <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                    </asp:RequiredFieldValidator>
                    <br />
                    <telerik:RadComboBox runat="server" ID="rcbCategoria" DataValueField="CODICE" DataTextField="DESCRIZIONE" Width="100%" CausesValidation="false"
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
                        OnClientItemsRequesting="rcbGruppoCategoriaCategoriaStatistica_OnClientItemsRequesting"
                        OnItemsRequested="rcbCategoria_ItemsRequested">
                    </telerik:RadComboBox>
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="4" SpanXl="4" SpanLg="4" SpanMd="4" SpanSm="12" SpanXs="12">
                    <asp:Label ID="lblCategoriaStatistica" runat="server" Font-Bold="true" Text="Categoria statistica" AssociatedControlID="rcbCategoriaStatistica" />
                    <asp:RequiredFieldValidator ID="rfvCategoriaStatistica" runat="server" Display="Dynamic"
                        ControlToValidate="rcbCategoriaStatistica"
                        ErrorMessage="La categoria statistica è obbligatoria."
                        ForeColor="Red">
                        <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                    </asp:RequiredFieldValidator>
                    <br />
                    <telerik:RadComboBox runat="server" ID="rcbCategoriaStatistica" DataValueField="CODICE" DataTextField="DESCRIZIONE" Width="100%" CausesValidation="false"
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
                        OnClientItemsRequesting="rcbGruppoCategoriaCategoriaStatistica_OnClientItemsRequesting"
                        OnItemsRequested="rcbCategoriaStatistica_ItemsRequested">
                    </telerik:RadComboBox>
                </telerik:LayoutColumn>
            </Columns>
        </telerik:LayoutRow>

        <telerik:LayoutRow RowType="Row" CssClass="OffertaRaggruppamentoExpanderItem SpaziaturaSuperioreRiga">
            <Columns>
                <telerik:LayoutColumn Span="3" SpanXl="2" SpanLg="2" SpanMd="3" SpanSm="12" SpanXs="12">
                    <asp:Label ID="lblCodiceArticolo" runat="server" Text="Codice Articolo" Font-Bold="true" AssociatedControlID="rcbCodiceArticolo" />
                    <asp:RequiredFieldValidator ID="rfvCodiceArticolo" runat="server" Display="Dynamic"
                        ControlToValidate="rcbCodiceArticolo"
                        ErrorMessage="Il codice articolo è obbligatorio."
                        ForeColor="Red">
                        <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                    </asp:RequiredFieldValidator>
                    <br />
                    <telerik:RadComboBox ID="rcbCodiceArticolo" runat="server" CausesValidation="false"
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
                        OnClientSelectedIndexChanged="rcbCodiceArticolo_OnClientSelectedIndexChanged"
                        OnClientDropDownOpening="rcbCodiceArticolo_OnClientDropDownOpening"
                        OnClientItemsRequesting="rcbCodiceArticolo_OnClientItemsRequesting"
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
                <telerik:LayoutColumn Span="3" SpanXl="6" SpanLg="6" SpanMd="5" SpanSm="12" SpanXs="12">
                    <asp:Label ID="lblDescrizione" runat="server" Text="Descrizione" Font-Bold="true" AssociatedControlID="rtbDescrizione" />
                    <asp:RequiredFieldValidator ID="rfvDescrizione" runat="server" Display="Dynamic"
                        ControlToValidate="rtbDescrizione"
                        ErrorMessage="La descrizione è obbligatoria."
                        ForeColor="Red">
                        <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                    </asp:RequiredFieldValidator>
                    <br />
                    <telerik:RadTextBox runat="server" ID="rtbDescrizione" Width="100%"></telerik:RadTextBox>
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="3" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12">
                    <asp:Label ID="lblUM" runat="server" Text="UM" Font-Bold="true" AssociatedControlID="rtbUM" />
                    <asp:RequiredFieldValidator ID="rfvUM" runat="server" Display="Dynamic"
                        ControlToValidate="rtbUM"
                        ErrorMessage="L'unità di misura è obbligatoria."
                        ForeColor="Red">
                        <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                    </asp:RequiredFieldValidator>
                    <br />
                    <telerik:RadTextBox runat="server" ID="rtbUM" Width="100%"></telerik:RadTextBox>
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="3" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12">
                    <asp:Label ID="lblQuantità" runat="server" Text="Q.tà" Font-Bold="true" AssociatedControlID="rntbQuantità" />
                    <asp:RequiredFieldValidator ID="rfvQuantità" runat="server" Display="Dynamic"
                        ControlToValidate="rntbQuantità"
                        ErrorMessage="La quantità è obbligatoria."
                        ForeColor="Red">
                        <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                    </asp:RequiredFieldValidator>
                    <br />
                    <telerik:RadNumericTextBox runat="server" ID="rntbQuantità" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" MinValue="0">
                        <ClientEvents OnValueChanged="rntbQuantità_OnValueChanged" />
                    </telerik:RadNumericTextBox>
                </telerik:LayoutColumn>
            </Columns>
        </telerik:LayoutRow>

        <telerik:LayoutRow RowType="Row" CssClass="OffertaRaggruppamentoExpanderItem SpaziaturaSuperioreRiga">
            <Columns>
                <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="12">
                    <asp:Label ID="lblCosto" runat="server" Text="Costo unitario" AssociatedControlID="rntbCosto" />
                    <br />
                    <telerik:RadNumericTextBox runat="server" ID="rntbCosto" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2">
                        <ClientEvents OnValueChanged="rntbCosto_OnValueChanged" />
                    </telerik:RadNumericTextBox>
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="12">
                    <asp:Label ID="lblRicaricoValore" runat="server" Text="Ricarico € (su costo unitario)" AssociatedControlID="rntbRicaricoValore" /><br />
                    <telerik:RadNumericTextBox runat="server" ID="rntbRicaricoValore" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2">
                        <ClientEvents  OnValueChanged="rntbRicaricoValore_OnValueChanged" />
                    </telerik:RadNumericTextBox>
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="12">
                    <asp:Label ID="lblRicaricoPercentuale" runat="server" Text="Ricarico % (su costo unitario)" AssociatedControlID="rntbRicaricoPercentuale" /><br />
                    <telerik:RadNumericTextBox runat="server" ID="rntbRicaricoPercentuale" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2">
                        <ClientEvents  OnValueChanged="rntbRicaricoPercentuale_OnValueChanged" />
                    </telerik:RadNumericTextBox>
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="12">
                    <asp:Label ID="lblVendita" runat="server" Text="Prezzo unitario" Font-Bold="true" AssociatedControlID="rntbVendita" />
                    <asp:RequiredFieldValidator ID="rfvVendita" runat="server" Display="Dynamic"
                        ControlToValidate="rntbVendita"
                        ErrorMessage="Il prezzo unitario è obbligatorio."
                        ForeColor="Red">
                        <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                    </asp:RequiredFieldValidator>
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
                    <asp:Label ID="lblTotaleVendita" runat="server" Text="Totale vendita" Font-Bold="true" AssociatedControlID="rntbTotaleVendita" />
                    <asp:RequiredFieldValidator ID="rfvTotaleVendita" runat="server" Display="Dynamic"
                        ControlToValidate="rntbTotaleVendita"
                        ErrorMessage="Il totale vendita è obbligatorio."
                        ForeColor="Red">
                        <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                    </asp:RequiredFieldValidator>
                    <br />
                    <telerik:RadNumericTextBox runat="server" ID="rntbTotaleVendita" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2">
                        <ClientEvents OnValueChanged="rntbTotaleVendita_OnValueChanged" />
                    </telerik:RadNumericTextBox>
                </telerik:LayoutColumn>
            </Columns>
        </telerik:LayoutRow>

        <telerik:LayoutRow RowType="Row" CssClass="OffertaRaggruppamentoExpanderItem SpaziaturaSuperioreRiga">
            <Columns>
                <telerik:LayoutColumn Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12">
                    <uc1:SpeseAccessorieArticoloOfferta runat="server" ID="SpeseAccessorieArticoloOfferta" />
                </telerik:LayoutColumn>
            </Columns>
        </telerik:LayoutRow>

    </Rows>
</telerik:RadPageLayout>

<asp:Repeater runat="server" ID="repCampiAggiuntivi" Visible="false">
    <HeaderTemplate>
        <table border="0" style="width: 100%; padding-top: 15px;">
            <thead>
                <tr class="OffertaRaggruppamentoExpanderHeader">
                    <th colspan="2" style="height: 20px;">Campi aggiuntivi
                    </th>
                </tr>
            </thead>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td style="width: 150px">
                <asp:HiddenField runat="server" ID="hdIdCampoAggiuntivo" Value='<%#Eval("ID") %>' />
                <asp:Label runat="server" ID="lblNomeCampoAggiuntivo" Text='<%#Eval("NomeCampo") %>'></asp:Label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtValoreCampoAggiuntivo" Text='<%#Eval("Valore") %>' Width="100%"></asp:TextBox>
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
