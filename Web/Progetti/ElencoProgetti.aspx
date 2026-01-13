<%@ Page Title="Elenco Progetti" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="ElencoProgetti.aspx.cs" Inherits="SeCoGEST.Web.Progetti.ElencoProgetti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    <asp:Label ID="lblTitolo" runat="server" Text="Elenco Progetti" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">

    <style type="text/css">
        .RadToolBar .RadToolBarButtonRightAlign {
            position:absolute;
            right:11px;
            top:10px;
        }
    </style>
    
    <telerik:RadToolBar ID="rtbPrincipale" runat="server" OnButtonClick="rtbPrincipale_ButtonClick">
        <Items>
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/24x24/add.png" CommandName="Aggiungi" Value="Aggiungi" Text="Aggiungi Progetto" ToolTip="Permette la creazione di un nuovo Progetto" PostBack="false" NavigateUrl="/Progetti/DettagliProgetto.aspx" />
            <telerik:RadToolBarButton IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/24x24/xls.png" CommandName="Esporta" Value="Esporta" Text="Esporta Elenco Progetti" ToolTip="Permette l'esportazione in Excel dei progetti visualizzati" PostBack="true" />            
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
        <MasterTableView DataKeyNames="ID" TableLayout="Fixed"
            Width="100%"
            AllowFilteringByColumn="true"
            AllowSorting="true"
            AllowMultiColumnSorting="true"
            GridLines="Both"
            NoMasterRecordsText="Nessun dato da visualizzare"
            CommandItemDisplay="None">
            <CommandItemSettings ExportToExcelText="Esporta in excel" ShowExportToExcelButton="true" />
            <Columns>
                <telerik:GridHyperLinkColumn 
                    HeaderStyle-Width="50"
                    ItemStyle-Width="50"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    ImageUrl="/UI/Images/Toolbar/edit.png"
                    Text="Apri..."
                    Resizable="false"
                    DataNavigateUrlFields="ID"
                    DataNavigateUrlFormatString="/Progetti/DettagliProgetto.aspx?ID={0}"
                    AllowFiltering="false"
                    AllowSorting="false"
                    Exportable="false"/>

                <telerik:GridButtonColumn HeaderStyle-Width="50px" UniqueName="ColonnaClona"
                    Resizable="false"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-Wrap="true"
                    ItemStyle-HorizontalAlign="Center"
                    ConfirmText="Clona il Progetto selezionato?"
                    ConfirmTextFields="Titolo"
                    ConfirmTextFormatString="Clonare il Progetto '{0}'?"
                    ConfirmDialogType="RadWindow"
                    ConfirmTitle="Clona"
                    ButtonType="ImageButton"
                    ImageUrl="/UI/Images/Toolbar/clone.png"
                    Text="Clona..."
                    CommandName="Clona" 
                    Exportable="false"/>

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    FilterControlWidth="50px"
                    HeaderStyle-Width="80px"
                    CurrentFilterFunction="EqualTo"
                    AutoPostBackOnFilter="true"
                    ItemStyle-HorizontalAlign="Center"
                    DataField="Numero"
                    HeaderText="Numero" />

                <telerik:GridDateTimeColumn
                    CurrentFilterFunction="Contains"                    
                    AutoPostBackOnFilter="true"
                    ItemStyle-HorizontalAlign="Center"
                    DataField="DataRedazione"
                    HeaderText="Data Redazione"
                    HeaderStyle-Width="100px">
                </telerik:GridDateTimeColumn>

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"                    
                    AutoPostBackOnFilter="true"
                    DataField="DenominazioneCliente"
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
                    HeaderStyle-Width="100px"
                    DataField="NumeroCommessa"
                    HeaderText="Commessa" />
                
                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"                    
                    AutoPostBackOnFilter="true"
                    HeaderStyle-Width="100px"
                    DataField="CodiceContratto"
                    HeaderText="Contratto" />
                                
                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"                    
                    AutoPostBackOnFilter="true"
                    DataField="ReferenteCliente.CognomeNome"
                    HeaderText="Referente Cliente" />
                                
                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"                    
                    AutoPostBackOnFilter="true"
                    DataField="NomeCompletoDPO"
                    HeaderText="DPO" />

                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                    HeaderStyle-Width="110px"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"                    
                    AutoPostBackOnFilter="true"
                    DataField="StatoString"
                    HeaderText="Stato" />
                
                <telerik:GridBoundColumn UniqueName="ChiusoString"
                    ItemStyle-Wrap="true"
                    HeaderStyle-Width="80px"
                    ItemStyle-HorizontalAlign="Center"
                    FilterControlWidth="80%"
                    CurrentFilterFunction="Contains"                    
                    AutoPostBackOnFilter="true"
                    DataField="ChiusoString"
                    HeaderText="Chiuso" />

                <telerik:GridButtonColumn HeaderStyle-Width="50px" UniqueName="ColonnaElimina"
                    Resizable="false"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-Wrap="true"
                    ItemStyle-HorizontalAlign="Center"
                    ConfirmText="Eliminare il Progetto selezionato?"
                    ConfirmTextFields="Titolo"
                    ConfirmTextFormatString="Eliminare il Progetto '{0}'?"
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
        <ExportSettings Excel-AutoFitColumnWidth="AutoFitAll" Excel-Format="Xlsx"></ExportSettings>
        <ClientSettings EnableRowHoverStyle="true">
            <Selecting AllowRowSelect="True" />
            <Resizing AllowColumnResize="true" EnableRealTimeResize="true" AllowResizeToFit="false" />
        </ClientSettings>

    </telerik:RadGrid>

    <div style="display: none;">
        <telerik:RadAjaxManagerProxy runat="server" ID="ramProgetti">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgGriglia">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgGriglia" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                        <telerik:AjaxUpdatedControl ControlID="RadWindowManagerMaster" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManagerProxy>
        <telerik:RadPersistenceManagerProxy ID="rpmpProgetti" runat="server">
            <PersistenceSettings>
                <telerik:PersistenceSetting ControlID="rgGriglia" />
            </PersistenceSettings>
        </telerik:RadPersistenceManagerProxy>
    </div>

</asp:Content>
