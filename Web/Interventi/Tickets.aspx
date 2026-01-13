<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="Tickets.aspx.cs" Inherits="SeCoGEST.Web.Interventi.Tickets" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    <asp:Label ID="lblTitolo" runat="server" Text="Elenco dei Ticket" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">

    <telerik:RadScriptBlock runat="server">
        <script type="text/javascript">

            // Metodo di gestione dell'evento OnCommand della griglia
            function RadGrid_OnCommand(sender, args) {
                try {
                    if (args != null && args.get_commandName && args.set_cancel) {
                        var commandName = args.get_commandName();
                        if (commandName == '<%=RadGrid.InitInsertCommandName%>') {
                            args.set_cancel(true);
                            window.location.href = "/Interventi/Ticket.aspx";
                        }
                    }
                } catch (ex) {
                    alert(ex.message);
                }
            }

        </script>
    </telerik:RadScriptBlock>
    
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
            NoMasterRecordsText="Nessun dato da visualizzare">

            <Columns>

                <telerik:GridHyperLinkColumn 
                    HeaderStyle-Width="50"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    ImageUrl="/UI/Images/Toolbar/edit.png"
                    Text="Apri..."
                    Resizable="false"
                    DataNavigateUrlFields="ID"
                    DataNavigateUrlFormatString="/Interventi/Ticket.aspx?ID={0}"
                    AllowFiltering="false"
                    AllowSorting="false"/>

                <telerik:GridBoundColumn
                    FilterControlWidth="55px"
                    CurrentFilterFunction="EqualTo"
                    AutoPostBackOnFilter="true"
                    HeaderStyle-Width="80px"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    DataField="Numero"
                    HeaderText="Numero" />

                <telerik:GridDateTimeColumn
                    CurrentFilterFunction="EqualTo"
                    AutoPostBackOnFilter="true"
                    HeaderStyle-Width="150px"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    DataField="DataRedazione"
                    HeaderText="Data Redazione"
                    DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true" UniqueName="ColonnaCliente"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    DataField="RagioneSocialeConDestinazione"
                    HeaderText="Ragione Sociale" />

                <telerik:GridBoundColumn
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    DataField="Oggetto"
                    HeaderText="Oggetto" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    FilterControlWidth="80%"
                    HeaderStyle-Width="110px"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    DataField="StatoStringForCustomers"
                    HeaderText="Stato" />                            

                <telerik:GridTemplateColumn ItemStyle-Wrap="true"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"
                    AllowSorting="true"
                    AutoPostBackOnFilter="true"
                    DataField="AccountRiferimento.Nominativo"
                    HeaderText="Riferimento">
                    <ItemTemplate>
                        <%#GetAccountRiferimento(Container.DataItem) %>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>                       

            </Columns>
            <CommandItemSettings AddNewRecordText="Nuovo Ticket" />
        </MasterTableView>

        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" />
        <GroupingSettings CaseSensitive="false" />
        
        <ClientSettings EnableRowHoverStyle="true">
            <Selecting AllowRowSelect="True" />
            <Resizing AllowColumnResize="true" EnableRealTimeResize="true" AllowResizeToFit="false" />
            <ClientEvents OnCommand="RadGrid_OnCommand" />
        </ClientSettings>

    </telerik:RadGrid>

    <div style="display: none;">
        <telerik:RadAjaxManagerProxy runat="server" ID="ramInterventi">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgGriglia">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgGriglia" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                        <telerik:AjaxUpdatedControl ControlID="RadWindowManagerMaster" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManagerProxy>
        <telerik:RadPersistenceManagerProxy ID="rpmpInterventi" runat="server">
            <PersistenceSettings>
                <telerik:PersistenceSetting ControlID="rgGriglia" />
            </PersistenceSettings>
        </telerik:RadPersistenceManagerProxy>
    </div>

</asp:Content>
