<%@ Page Title="Gestione Accounts" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="Accounts.aspx.cs" Inherits="SeCoGEST.Web.Sicurezza.Accounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    <asp:Label runat="server" Text="Accounts di accesso" />
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
                            window.location.href = "/Sicurezza/Account.aspx";
                        }
                        else {
                            args.set_cancel(false);
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
            NoMasterRecordsText="Nessun dato da visualizzare">

            <Columns>

                <telerik:GridHyperLinkColumn 
                    HeaderStyle-Width="40"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    ImageUrl="/UI/Images/Toolbar/edit.png"
                    Text="Apri..."
                    Resizable="false"
                    DataNavigateUrlFields="ID"
                    DataNavigateUrlFormatString="/Sicurezza/Account.aspx?ID={0}"
                    AllowFiltering="false"
                    AllowSorting="false"/>

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    FilterControlWidth="80%"
                    FilterImageToolTip="Applica filtro"
                    DataField="UserName"
                    HeaderText="UserName" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    FilterControlWidth="80%"
                    FilterImageToolTip="Applica filtro"
                    DataField="Nominativo"
                    HeaderText="Nominativo" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    FilterControlWidth="80%"
                    FilterImageToolTip="Applica filtro"
                    DataField="Email"
                    HeaderText="Email" />

                <telerik:GridDateTimeColumn ItemStyle-Wrap="true"
                    CurrentFilterFunction="EqualTo"
                    AutoPostBackOnFilter="true"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    FilterControlWidth="95"
                    FilterImageToolTip="Applica filtro"
                    DataFormatString="{0:dd/MM/yyyy HH:mm}"
                    DataField="UltimoAccesso"
                    HeaderText="Ultimo Accesso" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    AllowSorting="true"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    FilterControlWidth="70%"
                    FilterImageToolTip="Applica filtro"
                    HeaderText="Tipologia"
                    DataField="TipologiaString" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    AllowSorting="true"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    FilterControlWidth="70%"
                    FilterImageToolTip="Applica filtro"
                    HeaderText="Amministratore"
                    DataField="AmministratoreString" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    AllowSorting="true"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    FilterControlWidth="70%"
                    FilterImageToolTip="Applica filtro"
                    HeaderText="Bloccato"
                    DataField="BloccatoString" />

                <telerik:GridButtonColumn HeaderStyle-Width="40" UniqueName="ColonnaElimina"
                    Resizable="false"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-Wrap="true"
                    ItemStyle-HorizontalAlign="Center"
                    ConfirmText="Eliminare l'account selezionato?"
                    ConfirmDialogType="RadWindow"
                    ConfirmTitle="Elimina"
                    ButtonType="ImageButton"
                    ImageUrl="/UI/Images/Toolbar/delete.png"
                    Text="Elimina..."
                    CommandName="Delete" />

            </Columns>

        </MasterTableView>

        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" />
        <GroupingSettings CaseSensitive="false" />

        <ClientSettings EnableRowHoverStyle="true">
            <Selecting AllowRowSelect="True" />
            <Resizing AllowColumnResize="true" EnableRealTimeResize="true" AllowResizeToFit="false" />
            <ClientEvents OnGridCreated="RadGrid_OnGridCreated" OnCommand="RadGrid_OnCommand" />
        </ClientSettings>

    </telerik:RadGrid>

    <div style="display: none;">
        <telerik:RadAjaxManagerProxy runat="server" ID="ramAccounts">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgGriglia">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgGriglia" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                        <telerik:AjaxUpdatedControl ControlID="RadWindowManagerMaster" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManagerProxy>
        <telerik:RadPersistenceManagerProxy ID="rpmpAccounts" runat="server" >
            <PersistenceSettings>
                <telerik:PersistenceSetting ControlID="rgGriglia" />
            </PersistenceSettings>
        </telerik:RadPersistenceManagerProxy>
    </div>
</asp:Content>

