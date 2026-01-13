<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="ConfigurazioniArticoliAggiuntivi.aspx.cs" Inherits="SeCoGEST.Web.Preventivi.ConfigurazioniArticoliAggiuntivi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    <asp:Label ID="lblTitolo" runat="server" Text="Elenco Configurazioni Articoli Aggiuntivi" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">
    <telerik:RadToolBar ID="rtbPrincipale"
        runat="server"
        OnClientButtonClicking="ToolBarButtonClicking">
        <Items>
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/24x24/add.png" CommandName="Aggiungi" Value="Aggiungi" Text="Aggiungi Configurazione Articolo Aggiuntivo" ToolTip="Permette la creazione di una nuova Configurazione Articolo Aggiuntivo" PostBack="false" NavigateUrl="/Preventivi/ConfigurazioneArticoloAggiuntivo.aspx" />
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
                    DataNavigateUrlFormatString="/Preventivi/ConfigurazioneArticoloAggiuntivo.aspx?ID={0}"
                    AllowFiltering="false"
                    AllowSorting="false"
                    Exportable="false" />

                <telerik:GridBoundColumn
                    FilterControlWidth="70%"
                    HeaderStyle-Width="100px"
                    DataField="TipologiaString"
                    CurrentFilterFunction="EqualTo"
                    AutoPostBackOnFilter="true"
                    HeaderText="Tipologia" />                                

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    DataField="GruppoIn"
                    HeaderText="Gruppo" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    DataField="CategoriaIn"
                    HeaderText="Categoria" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    DataField="CategoriaStatisticaIn"
                    HeaderText="Categoria Statistica" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"
                    AutoPostBackOnFilter="true"
                    DataField="CodiceArticoloIn"
                    HeaderText="Cod. Art." />

                <telerik:GridButtonColumn HeaderStyle-Width="40px" UniqueName="ColonnaElimina"
                    Resizable="false"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-Wrap="true"
                    ItemStyle-HorizontalAlign="Center"
                    ConfirmText="Eliminare la Configurazione articoli aggiuntivo selezionata?"
                    ConfirmDialogType="RadWindow"
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
        <telerik:RadAjaxManagerProxy runat="server" ID="ramConfigurazioniArticoliAggiuntivi">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgGriglia">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgGriglia" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                        <telerik:AjaxUpdatedControl ControlID="RadWindowManagerMaster" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManagerProxy>
        <telerik:RadPersistenceManagerProxy ID="rpmpConfigurazioniArticoliAggiuntivi" runat="server">
            <PersistenceSettings>
                <telerik:PersistenceSetting ControlID="rgGriglia" />
            </PersistenceSettings>
        </telerik:RadPersistenceManagerProxy>
    </div>
</asp:Content>
