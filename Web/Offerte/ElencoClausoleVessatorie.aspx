<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="ElencoClausoleVessatorie.aspx.cs" Inherits="SeCoGEST.Web.Offerte.ElencoClausoleVessatorie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    <asp:Label ID="lblTitolo" runat="server" Text="Elenco Clausola Vessatoria" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">
    <telerik:RadToolBar ID="rtbPrincipale"
        runat="server"
        OnClientButtonClicking="ToolBarButtonClicking">
        <Items>
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/24x24/add.png" CommandName="Aggiungi" Value="Aggiungi" Text="Aggiungi Clausola Vessatoria" ToolTip="Permette la creazione di una nuova Clausola Vessatoria" PostBack="false" NavigateUrl="/Offerte/DettagliClausolaVessatoria.aspx" />
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
                    DataNavigateUrlFormatString="/Offerte/DettagliClausolaVessatoria.aspx?ID={0}"
                    AllowFiltering="false"
                    AllowSorting="false"
                    Exportable="false" />

                <telerik:GridBoundColumn
                    FilterControlWidth="50px"
                    HeaderStyle-Width="100px"
                    DataField="Codice"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    HeaderText="Codice" />

                <telerik:GridBoundColumn
                    FilterControlWidth="90%"
                    DataField="Descrizione"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    HeaderText="Descrizione" />

                <telerik:GridButtonColumn HeaderStyle-Width="40px" UniqueName="ColonnaElimina"
                    Resizable="false"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-Wrap="true"
                    ItemStyle-HorizontalAlign="Center"
                    ConfirmText="Eliminare la Clausola Vessatoria selezionata?"
                    ConfirmTextFields="Codice"
                    ConfirmTextFormatString="Eliminare la Clausola Vessatoria '{0}'?"
                    ConfirmDialogType="Classic"
                    ConfirmTitle="Elimina"
                    ButtonType="ImageButton"
                    ImageUrl="/UI/Images/Toolbar/delete.png"
                    Text="Elimina..."
                    CommandName="Delete"
                    Exportable="false" />

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
