<%@ Page Title="Elenco Interventi" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="Interventi.aspx.cs" Inherits="SeCoGEST.Web.Interventi.Interventi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    <asp:Label ID="lblTitolo" runat="server" Text="Elenco Interventi" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">

    <style type="text/css">
        .RadToolBar .RadToolBarButtonRightAlign {
            position: absolute;
            right: 11px;
            top: 10px;
        }
    </style>

    <telerik:radscriptblock runat="server">
        <script type="text/javascript">

            // Metodo di gestione dell'evento OnCommand della griglia
           <%-- function RadGrid_OnCommand(sender, args) {
                try {
                    if (args != null && args.get_commandName && args.set_cancel) {
                        var commandName = args.get_commandName();
                        if (commandName == '<%=RadGrid.InitInsertCommandName%>') {
                            args.set_cancel(true);
                            window.location.href = "/Interventi/Intervento.aspx";
                        }
                    }
                } catch (ex) {
                    alert(ex.message);
                }
            }--%>

            // Metodo di gestione dell'evento clicking relativo alla toolbar
            function ToolBarButtonClicking(sender, args) {
                try {
                    debugger;
                    if (args != null && args.get_item) {

                        var button = args.get_item();
                        if (button != null && button.get_commandName) {

                            var messaggioConfermato = true;
                            var commandName = button.get_commandName();

                            if (commandName == "GeneraInviaInterventiSelezionati") {
                                if (EsistonoInterventiSelezionati()) {
                                    messaggioConfermato = confirm('Effettuare la generazione degli interventi selezionati?');
                                }
                                else {
                                    throw new Error("Non è stato selezionato nessun intervento.");
                                }
                            }

                            if (commandName == "ValidaInterventiSelezionati") {
                                if (EsistonoInterventiSelezionati()) {
                                    messaggioConfermato = confirm('Effettuare la validazione degli interventi selezionati?');
                                }
                                else {
                                    throw new Error("Non è stato selezionato nessun intervento.");
                                }
                            }

                            if (!messaggioConfermato) {
                                args.set_cancel(true);
                            }
                        }
                    }
                } catch (ex) {
                    showMessage("Attenzione", ex.message);
                    args.set_cancel(true);
                }
            }

            // Ritorna true se esitono degli interventi selezionati
            function EsistonoInterventiSelezionati() {
                var griglia = $find('<%=rgGriglia.ClientID%>');
                var masterTableView = griglia.get_masterTableView();
                var dataItems = masterTableView.get_dataItems();

                var informazioniDocumentiDaFirmare = [];
                var resultValue = false;

                if (dataItems != null && dataItems.length > 0) {
                    for (var i = 0; i < dataItems.length; i++) {
                        var checkBoxSelezionato = dataItems[i].findElement("chkSelezionato");
                        if (checkBoxSelezionato != null && checkBoxSelezionato.checked) {
                            resultValue = true;
                            break;
                        }
                    }
                }

                return resultValue;
            }

            var stateCheckedChanged = 0;
            function rcbInterventoApertoChiuso_ClientItemChecked(sender, args) {
                stateCheckedChanged = 1;
            }

            //var filterCheckedChanged = 0;
            //function rcbFiltriIntervento_ClientItemChecked(sender, args) {
            //    filterCheckedChanged = 1;
            //}

            // Metodo di gestione dell'evento DropDownClosed relativo alla combo di selezione filtri
            // Aggiorna i dati della griglia alla chiusura della combo
            function rcbInterventoApertoChiuso_ClientDropDownClosed(sender, args) {
                if (stateCheckedChanged == 1) {
                    stateCheckedChanged = 0;

                    var btApplyFilter = document.getElementById('<%=btApplyFilter.ClientID%>');
                    btApplyFilter.click();

<%--                    var griglia = $find('<%=rgGriglia.ClientID%>');
                    var masterTableView = griglia.get_masterTableView();
                    masterTableView.rebind();--%>
                }
            }

            
<%--            function rcbFiltriIntervento_ClientDropDownClosed(sender, args) {
                if (filterCheckedChanged == 1) {
                    filterCheckedChanged = 0;

                    var btApplyFilter = document.getElementById('<%=btApplyFilter.ClientID%>');
                    btApplyFilter.click();
                }
            }--%>


            function Checked(btn) {
                var grid = $find("<%=rgGriglia.ClientID %>");
                var masterTable = grid.get_masterTableView();
                var btnValue = btn.value;
                for (var i = 0; i < masterTable.get_dataItems().length; i++) {
                    var gridItemElement = masterTable.get_dataItems()[i].findElement("CheckBox2");
                    if (btnValue == "Check") {
                        gridItemElement.checked = true;
                        btn.value = "UnCheck";
                    }
                    else {
                        gridItemElement.checked = false;
                        btn.value = "Check";
                    }
                }
            }

            function CheckAll(id) {
                var masterTable = $find("<%= rgGriglia.ClientID %>").get_masterTableView();
                var row = masterTable.get_dataItems();
                if (id.checked == true) {
                    for (var i = 0; i < row.length; i++) {
                        masterTable.get_dataItems()[i].findElement("chkSelezionato").checked = true;
                    }
                }
                else {
                    for (var i = 0; i < row.length; i++) {
                        masterTable.get_dataItems()[i].findElement("chkSelezionato").checked = false;
                    }
                }
            }
        </script>
    </telerik:radscriptblock>

    <telerik:radtoolbar id="rtbPrincipale"
        runat="server"
        OnClientButtonClicking="ToolBarButtonClicking"
        OnButtonClick="rtbPrincipale_ButtonClick">
        <Items>
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/24x24/add.png" CommandName="Aggiungi" Value="Aggiungi" Text="Aggiungi Intervento" ToolTip="Permette la creazione di un nuovo Intervento" PostBack="false" NavigateUrl="/Interventi/Intervento.aspx" />
            <telerik:RadToolBarButton IsSeparator="true" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/24x24/refresh.png" CommandName="Aggiorna" Value="Aggiorna" Text="Aggiorna" ToolTip="Aggiorna i dati visualizzati" PostBack="False" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/24x24/print.png" CommandName="GeneraInviaInterventiSelezionati" Value="GeneraInviaInterventiSelezionati" Text="Genera ed Invia Interventi Selezionati" ToolTip="Effettua la generazione degli interventi selezionati e li invia al tuo indirizzo email" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/basecircle.png" CommandName="ValidaInterventiSelezionati" Value="ValidaInterventiSelezionati" Text="Valida Interventi Selezionati" ToolTip="Effettua la validazione degli interventi selezionati e li invia a Metodo" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreValidaInterventiSelezionati" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/24x24/xls.png" CommandName="EsportaInterventi" Value="EsportaInterventi" Text="Esporta Griglia Interventi" ToolTip="Permette l'esportazione in Excel degli interventi visualizzati" PostBack="true" />            
            <telerik:RadToolBarButton runat="server" CausesValidation="False" CommandName="FiltroGriglia" Value="FiltroGriglia" CssClass="RadToolBarButtonRightAlign">
                <ItemTemplate>
                    <div style="background-color: white; padding-left: 10px;">
                        <span style="color:black;">Filtri:</span>
                        <telerik:RadComboBox ID="rcbFiltriIntervento" runat="server" EmptyMessage="Seleziona"
                            Width="350px"
                            ForeColor="black"
                            HighlightTemplatedItems="true"
                            DataValueField="Key"
                            DataTextField="Value"
                            EnableCheckAllItemsCheckBox="true" 
                            MarkFirstMatch="false"
                            CheckBoxes="true" 
                            Localization-CheckAllString="Seleziona Tutto" 
                            Localization-ItemsCheckedString="Elementi Selezionati" 
                            Localization-AllItemsCheckedString="Selezionati Tutti" 
                            OnClientCheckAllChecked="rcbInterventoApertoChiuso_ClientItemChecked"
                            OnClientItemChecked="rcbInterventoApertoChiuso_ClientItemChecked"
                            OnClientDropDownClosed="rcbInterventoApertoChiuso_ClientDropDownClosed">
                        </telerik:RadComboBox>

                        <telerik:RadComboBox ID="rcbInterventoApertoChiuso" runat="server" EmptyMessage="Seleziona"
                            Width="150px"
                            ForeColor="black"
                            HighlightTemplatedItems="true"
                            DataValueField="Key"
                            DataTextField="Value"
                            EnableCheckAllItemsCheckBox="true" 
                            MarkFirstMatch="false"
                            CheckBoxes="true" 
                            Localization-CheckAllString="Seleziona Tutto" 
                            Localization-ItemsCheckedString="Elementi Selezionati" 
                            Localization-AllItemsCheckedString="Selezionati Tutti" 
                            OnClientCheckAllChecked="rcbInterventoApertoChiuso_ClientItemChecked"
                            OnClientItemChecked="rcbInterventoApertoChiuso_ClientItemChecked"
                            OnClientDropDownClosed="rcbInterventoApertoChiuso_ClientDropDownClosed"/>
                    </div>
                </ItemTemplate>
            </telerik:RadToolBarButton>
        </Items>
    </telerik:radtoolbar>

    <asp:Button runat="server" ID="btApplyFilter" OnClick="btApplyFilter_Click" style="visibility:hidden;display:none;" />

    <telerik:radgrid runat="server"
        id="rgGriglia"
        cssclass="GridResponsiveColumns"
        allowpaging="true"
        pagesize="10"
        gridlines="None"
        sortingsettings-sorttooltip="Clicca qui per ordinare i dati in base a questa colonna"
        autogeneratecolumns="false"
        onprerender="rgGriglia_PreRender"
        onitemcommand="rgGriglia_ItemCommand"
        ondeletecommand="rgGriglia_DeleteCommand"
        onneeddatasource="rgGriglia_NeedDataSource"
        onitemcreated="rgGriglia_ItemCreated"
        onitemdatabound="rgGriglia_ItemDataBound"
        pagerstyle-firstpagetooltip="Prima Pagina"
        pagerstyle-lastpagetooltip="Ultima Pagina"
        pagerstyle-prevpagetooltip="Pagina Precedente"
        pagerstyle-nextpagetooltip="Pagina Successiva"
        pagerstyle-pagertextformat="{4} Elementi da {2} a {3} su {5}, Pagina {0} di {1}">
        <MasterTableView DataKeyNames="ID" TableLayout="Fixed" EditMode="PopUp"
            Width="100%"
            AllowFilteringByColumn="true"
            AllowSorting="true"
            AllowMultiColumnSorting="true"
            GridLines="Both"
            NoMasterRecordsText="Nessun dato da visualizzare"
            CommandItemDisplay="None">

            <Columns>
                <telerik:GridTemplateColumn ItemStyle-Wrap="true" UniqueName="ColonnaSelezionato"
                    HeaderText=""
                    HeaderStyle-Width="40px"
                    ItemStyle-Width="40px"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    AllowFiltering="false"
                    AllowSorting="false"
                    Exportable="false">
                    <HeaderTemplate> 
                        <input type="checkbox" runat="server" value="check" ID="checkbox"  onclick="CheckAll(this);"  />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelezionato" runat="server" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridHyperLinkColumn 
                    HeaderStyle-Width="40"
                    ItemStyle-Width="40"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    ImageUrl="/UI/Images/Toolbar/16x16/edit.png"
                    Text="Apri..."
                    Resizable="false"
                    DataNavigateUrlFields="ID"
                    DataNavigateUrlFormatString="/Interventi/Intervento.aspx?ID={0}"
                    AllowFiltering="false"
                    AllowSorting="false"
                    Exportable="false"/>

                <telerik:GridBoundColumn
                    FilterControlWidth="45px"
                    HeaderStyle-Width="70px"
                    DataField="Numero"
                    CurrentFilterFunction="EqualTo"
                    AutoPostBackOnFilter="true"
                    HeaderText="Numero" />

                <telerik:GridDateTimeColumn
                    DataField="DataRedazione"
                    HeaderText="Data Redazione"
                    CurrentFilterFunction="EqualTo"
                    AutoPostBackOnFilter="true"
                    DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                <telerik:GridDateTimeColumn
                    DataField="DataPrevistaIntervento"
                    HeaderText="Data Intervento"
                    CurrentFilterFunction="EqualTo"
                    AutoPostBackOnFilter="true"
                    DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"                    
                    AutoPostBackOnFilter="true"
                    DataField="Oggetto"
                    HeaderText="Oggetto" />
                                
                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    HeaderStyle-Width="80px"
                    DataField="CodiceCliente"
                    HeaderText="Cliente" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    DataField="RagioneSociale"
                    HeaderText="Ragione Sociale" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    DataField="DestinazioneMerce"
                    HeaderText="Destinazione Merce" />

<%--                <telerik:GridBoundColumn
                    FilterControlWidth="45px"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    HeaderStyle-Width="70px"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    DataField="ChiamataString"
                    HeaderText="Chiamata" />
--%>
                <telerik:GridBoundColumn
                    FilterControlWidth="25px"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    HeaderStyle-Width="45px"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    DataField="UrgenteString"
                    HeaderText="Urg." />

                <telerik:GridBoundColumn
                    FilterControlWidth="25px"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    HeaderStyle-Width="45px"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    DataField="InternoString"
                    HeaderText="Int." />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    FilterControlWidth="80%"
                    DataField="TipologiaString"
                    HeaderText="Tipologia" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    FilterControlWidth="80%"
                    DataField="Operatori"
                    HeaderText="Operatori" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    FilterControlWidth="70%"
                    HeaderStyle-Width="60px"
                    ItemStyle-HorizontalAlign="Right"
                    DataField="TotaleMinuti"
                    HeaderText="Minuti" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    FilterControlWidth="80%"
                    DataField="DescrizioniModiDiRisoluzione"
                    HeaderText="Modo Risoluzione" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    FilterControlWidth="80%"
                    DataField="DescrizioneStato"
                    HeaderText="Stato" 
                    Resizable="false"/>

                <telerik:GridBoundColumn
                    FilterControlWidth="25px"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    HeaderStyle-Width="45px"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    DataField="DaFatturareString"
                    HeaderText="Da Fatt."
                    HeaderTooltip="Indica se almeno un articolo dell'intervento è da fatturare"/>

                <telerik:GridBoundColumn
                    FilterControlWidth="25px"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    HeaderStyle-Width="45px"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    DataField="ChiusoString"
                    HeaderText="Met."
                    HeaderTooltip="Indica se l'intervento è stato inviato a Metodo"/>

                <telerik:GridHyperLinkColumn HeaderStyle-Width="40px" UniqueName="ColonnaGeneraIntervento"                    
                    AllowFiltering="false"
                    AllowSorting="false"
                    Resizable="false"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-Wrap="true"
                    ItemStyle-HorizontalAlign="Center"
                    HeaderText=""
                    HeaderTooltip=""
                    ImageUrl="/UI/Images/Toolbar/16x16/print.png"
                    Text="Genera Intervento"
                    Target="_blank"
                    Visible="False"
                    Exportable="false"/>
                            
                <telerik:GridButtonColumn HeaderStyle-Width="40px" UniqueName="ColonnaElimina" Visible="false"
                    Resizable="false"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-Wrap="true"
                    ItemStyle-HorizontalAlign="Center"
                    ConfirmText="Eliminare l'Intervento selezionato?"
                    ConfirmTextFields="Numero"
                    ConfirmTextFormatString="Eliminare l'Intervento numero {0}?"
                    ConfirmDialogType="RadWindow"
                    ConfirmTitle="Elimina"
                    ButtonType="ImageButton"
                    ImageUrl="/UI/Images/Toolbar/16x16/delete.png"
                    Text="Elimina..."
                    CommandName="Delete" 
                    Exportable="false"/>

            </Columns>

        </MasterTableView>
        
        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" />
        <GroupingSettings CaseSensitive="false" />

        <ClientSettings EnableRowHoverStyle="true">
            <Selecting AllowRowSelect="True" />
            <Resizing AllowColumnResize="true" EnableRealTimeResize="true" AllowResizeToFit="false" />
            <%--<ClientEvents OnCommand="RadGrid_OnCommand" />--%>
        </ClientSettings>

    </telerik:radgrid>

    <asp:Button runat="server" ID="btnTestG7Api" Text="Test G7 API" OnClick="btnTestG7Api_Click" Visible="false"/>

    <div style="display: none;">
        <telerik:radajaxmanagerproxy runat="server" id="ramInterventi">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgGriglia">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgGriglia" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                        <telerik:AjaxUpdatedControl ControlID="RadWindowManagerMaster" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:radajaxmanagerproxy>
        <telerik:radpersistencemanagerproxy id="rpmpInterventi" runat="server">
            <PersistenceSettings>
                <telerik:PersistenceSetting ControlID="rgGriglia" />
            </PersistenceSettings>
        </telerik:radpersistencemanagerproxy>
    </div>

</asp:Content>
