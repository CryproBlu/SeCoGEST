<%@ Page Title="Elenco Offerte" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="ElencoOfferte.aspx.cs" Inherits="SeCoGEST.Web.Offerte.ElencoOfferte" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    <asp:Label ID="lblTitolo" runat="server" Text="Elenco Offerte" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">

    <style type="text/css">
        .RadToolBar .RadToolBarButtonRightAlign {
            position:absolute;
            right:11px;
            top:10px;
        }
    </style>

    <telerik:RadScriptBlock runat="server">
        <script type="text/javascript">

            // Metodo di gestione dell'evento OnCommand della griglia
           <%-- function RadGrid_OnCommand(sender, args) {
                try {
                    if (args != null && args.get_commandName && args.set_cancel) {
                        var commandName = args.get_commandName();
                        if (commandName == '<%=RadGrid.InitInsertCommandName%>') {
                            args.set_cancel(true);
                            window.location.href = "/Offerte/DettagliOfferta.aspx";
                        }
                    }
                } catch (ex) {
                    alert(ex.message);
                }
            }--%>

            // Metodo di gestione dell'evento clicking relativo alla toolbar
            function ToolBarButtonClicking(sender, args) {
                try {
                    if (args != null && args.get_item) {

                        var button = args.get_item();
                        if (button != null && button.get_commandName) {

                            var messaggioConfermato = true;
                            var commandName = button.get_commandName();

                            if (commandName == "GeneraIntervento") {

                                if (EsistonoOfferteSelezionate()) {
                                    messaggioConfermato = confirm('Effettuare la generazione degli Offerte selezionate?');
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

            // Ritorna true se esitono Offerte selezionate
            function EsistonoOfferteSelezionate() {
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

        </script>
    </telerik:RadScriptBlock>

    <telerik:RadToolBar ID="rtbPrincipale"
        runat="server"
        OnClientButtonClicking="ToolBarButtonClicking">
        <Items>
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/24x24/add.png" CommandName="Aggiungi" Value="Aggiungi" Text="Aggiungi Offerta" ToolTip="Permette la creazione di una nuova Offerta" PostBack="false" NavigateUrl="/Offerte/DettagliOfferta.aspx" />
            <telerik:RadToolBarButton IsSeparator="true" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/24x24/refresh.png" CommandName="Aggiorna" Value="Aggiorna" Text="Aggiorna" ToolTip="Aggiorna i dati visualizzati" PostBack="False" />
        </Items>
    </telerik:RadToolBar>

    <telerik:RadGrid runat="server"
        ID="rgGriglia"
        CssClass="GridResponsiveColumns"
        AllowPaging="true"
        PageSize="10"
        GridLines="None"
        SortingSettings-SortToolTip="Clicca qui per ordinare i dati in base a questa colonna"
        AutoGenerateColumns="false"
        OnPreRender="rgGriglia_PreRender"
        OnItemCommand="rgGriglia_ItemCommand"
        OnDeleteCommand="rgGriglia_DeleteCommand"
        OnNeedDataSource="rgGriglia_NeedDataSource"
        OnItemCreated="rgGriglia_ItemCreated"
        OnItemDataBound="rgGriglia_ItemDataBound"
        PagerStyle-FirstPageToolTip="Prima Pagina"
        PagerStyle-LastPageToolTip="Ultima Pagina"
        PagerStyle-PrevPageToolTip="Pagina Precedente"
        PagerStyle-NextPageToolTip="Pagina Successiva"
        PagerStyle-PagerTextFormat="{4} Elementi da {2} a {3} su {5}, Pagina {0} di {1}">
        <MasterTableView DataKeyNames="ID" TableLayout="Fixed" EditMode="PopUp"
            Width="100%"
            AllowFilteringByColumn="true"
            AllowSorting="true"
            AllowMultiColumnSorting="true"
            GridLines="Both"
            NoMasterRecordsText="Nessun dato da visualizzare"
            CommandItemDisplay="None">

            <Columns>
                <telerik:GridHyperLinkColumn 
                    HeaderStyle-Width="45"
                    ItemStyle-Width="45"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    ImageUrl="/UI/Images/Toolbar/edit.png"
                    Text="Apri..."
                    Resizable="false"
                    DataNavigateUrlFields="ID"
                    DataNavigateUrlFormatString="/Offerte/DettagliOfferta.aspx?ID={0}"
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

                <telerik:GridBoundColumn
                    FilterControlWidth="45px"
                    HeaderStyle-Width="70px"
                    DataField="NumeroRevisione"
                    CurrentFilterFunction="EqualTo"
                    AutoPostBackOnFilter="true"
                    HeaderText="Revisione" />

                <telerik:GridDateTimeColumn
                    DataField="Data"
                    HeaderText="Data"
                    CurrentFilterFunction="EqualTo"
                    AutoPostBackOnFilter="true"
                    HeaderStyle-Width="100px"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                
                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    DataField="CodiceCliente"
                    HeaderText="Codice Cliente" />

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
                    DataField="Titolo"
                    HeaderText="Titolo" />
                                
                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    HeaderStyle-Width="120px"
                    HeaderStyle-HorizontalAlign="Right"
                    ItemStyle-HorizontalAlign="Right"
                    DataField="TotaleCosto"
                    DataFormatString="{0:c}"
                    HeaderText="Totale Costo" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    HeaderStyle-Width="120px"
                    HeaderStyle-HorizontalAlign="Right"
                    ItemStyle-HorizontalAlign="Right"
                    DataField="TotaleRicaricoValuta"
                    DataFormatString="{0:c}"
                    HeaderText="Ricarico €" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    HeaderStyle-Width="100px"
                    HeaderStyle-HorizontalAlign="Right"
                    ItemStyle-HorizontalAlign="Right"
                    DataField="TotaleRicaricoPercentuale"
                    DataFormatString="{0}%"
                    HeaderText="Ricarico %" />

                <telerik:GridBoundColumn
                    FilterControlWidth="80px"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    HeaderStyle-Width="120px"
                    HeaderStyle-HorizontalAlign="Right"
                    ItemStyle-HorizontalAlign="Right"
                    DataField="TotaleSpesa"
                    DataFormatString="{0:c}"
                    HeaderText="Totale Spese" />

                <telerik:GridBoundColumn
                    FilterControlWidth="80px"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    HeaderStyle-Width="120px"
                    HeaderStyle-HorizontalAlign="Right"
                    ItemStyle-HorizontalAlign="Right"
                    DataField="TotaleVenditaConSpesa"
                    DataFormatString="{0:c}"
                    HeaderText="Totale Vendita" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    HeaderStyle-Width="100px"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"                    
                    AutoPostBackOnFilter="true"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    DataField="StatoEnumString"
                    HeaderText="Stato" />

                <telerik:GridCheckBoxColumn Visible="false" Display="false"
                    FilterControlWidth="45px"
                    CurrentFilterFunction="EqualTo"
                    AutoPostBackOnFilter="true"
                    HeaderStyle-Width="70px"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    DataField="Chiuso"
                    HeaderText="Chiuso" />

                <telerik:GridButtonColumn HeaderStyle-Width="40px" UniqueName="ColonnaElimina"
                    Resizable="false"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-Wrap="true"
                    ItemStyle-HorizontalAlign="Center"
                    ConfirmText="Eliminare l'Offerta selezionata?"
                    ConfirmTextFields="Numero"
                    ConfirmTextFormatString="Eliminare l'Offerta numero {0}?"
                    ConfirmDialogType="RadWindow"
                    ConfirmTitle="Elimina"
                    ButtonType="ImageButton"
                    ImageUrl="/UI/Images/Toolbar/delete.png"
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
        </ClientSettings>

    </telerik:RadGrid>

    <div style="display: none;">
        <telerik:RadAjaxManagerProxy runat="server" ID="ramOfferte">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgGriglia">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgGriglia" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                        <telerik:AjaxUpdatedControl ControlID="RadWindowManagerMaster" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManagerProxy>
        <telerik:RadPersistenceManagerProxy ID="rpmpOfferte" runat="server">
            <PersistenceSettings>
                <telerik:PersistenceSetting ControlID="rgGriglia" />
            </PersistenceSettings>
        </telerik:RadPersistenceManagerProxy>
    </div>

</asp:Content>
