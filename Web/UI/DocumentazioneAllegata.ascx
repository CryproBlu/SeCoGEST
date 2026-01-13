<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentazioneAllegata.ascx.cs" Inherits="SeCoGEST.Web.UI.DocumentazioneAllegata" %>

<style type="text/css">    
    .RadUpload.CaricamentoAllegato .ruSelectWrap {
        margin: 0;
    }
</style>

<%--<telerik:RadAjaxManagerProxy ID="RadAjaxProxy" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="AllegatiPanel">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="rgGrigliaAllegati" LoadingPanelID="RadAjaxLoadingPanelMaster"></telerik:AjaxUpdatedControl>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>--%>


    <telerik:RadPageLayout runat="server" ID="AllegatiPanel" Width="100%" HtmlTag="None" GridType="Fluid">
        <Rows>
            <telerik:LayoutRow RowType="Region" >
                <Columns>
                    <telerik:LayoutColumn  Style="padding:0px;">
                        <telerik:RadAjaxPanel runat="server" LoadingPanelID="RadAjaxLoadingPanelMaster">
                        <telerik:RadGrid runat="server"
                            ID="rgGrigliaAllegati"
                            AllowPaging="false"
                            AllowFilteringByColumn="false"
                            PageSize="10"
                            GridLines="None"
                            SortingSettings-SortToolTip="Clicca qui per ordinare i dati in base a questa colonna"
                            AutoGenerateColumns="false"
                            OnDeleteCommand="rgGrigliaAllegati_DeleteCommand"
                            OnNeedDataSource="rgGrigliaAllegati_NeedDataSource"
                            OnItemCreated="rgGrigliaAllegati_ItemCreated"
                            OnItemDataBound="rgGrigliaAllegati_ItemDataBound"
                            PagerStyle-FirstPageToolTip="Prima Pagina"
                            PagerStyle-LastPageToolTip="Ultima Pagina"
                            PagerStyle-PrevPageToolTip="Pagina Precedente"
                            PagerStyle-NextPageToolTip="Pagina Successiva"
                            PagerStyle-PagerTextFormat="{4} Elementi da {2} a {3} su {5}, Pagina {0} di {1}">
                            <MasterTableView DataKeyNames="ID" TableLayout="Fixed"
                                Width="100%"
                                AllowFilteringByColumn="false"
                                AllowSorting="true"
                                AllowMultiColumnSorting="true"
                                GridLines="Both"
                                NoMasterRecordsText="Nessun dato da visualizzare">

                                <Columns>
                                    <telerik:GridBoundColumn DataField="ID"
                                        HeaderText="ID"
                                        ReadOnly="true"
                                        ForceExtractValue="Always"
                                        Visible="false"
                                        AutoPostBackOnFilter="false" />

                                    <telerik:GridDateTimeColumn ItemStyle-Wrap="true"
                                        HeaderStyle-Width="110px"
                                        UniqueName="ColonnaDataArchiviazione"
                                        Resizable="false"
                                        HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center"
                                        FilterImageToolTip="Applica filtro"
                                        DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                        DataField="DataArchiviazione"
                                        HeaderText="Data Archiviazione" />

                                    <telerik:GridBoundColumn ItemStyle-Wrap="true"
                                        HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-HorizontalAlign="Left"
                                        FilterImageToolTip="Applica filtro"
                                        DataField="NomeFile"
                                        HeaderText="Nome File"
                                        Resizable="true" />

    <%--                                <telerik:GridBoundColumn ItemStyle-Wrap="true"
                                        HeaderStyle-HorizontalAlign="Left"
                                        ItemStyle-HorizontalAlign="Left"
                                        FilterImageToolTip="Applica filtro"
                                        DataField="TipologiaAllegatoString"
                                        HeaderText="Tipologia Allegato"
                                        Resizable="true" />--%>

                                    <telerik:GridBoundColumn ItemStyle-Wrap="true"
                                        UniqueName="ColonnaEstensioneFile"
                                        HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center"
                                        FilterImageToolTip="Applica filtro"
                                        DataField="Note"
                                        HeaderText="Note"
                                        Resizable="false" />

                                    <telerik:GridBoundColumn ItemStyle-Wrap="true"
                                        HeaderStyle-Width="200px"
                                        UniqueName="ColonnaUserName"
                                        HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center"
                                        FilterImageToolTip="Applica filtro"
                                        DataField="UserName"
                                        HeaderText="Utente"
                                        Resizable="true" />


                                    <telerik:GridHyperLinkColumn
                                        UniqueName="ColonnaDownload"
                                        HeaderStyle-Width="50"
                                        HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-HorizontalAlign="Center"
                                        ImageUrl="/UI/Images/Toolbar/24x24/save.png"
                                        Text="Scarica Allegato"
                                        DataNavigateUrlFields="ID,NomeFile"
                                        DataNavigateUrlFormatString="/DownloaderAllegato.aspx?ID={0}&Name={1}"
                                        Target="_blank"
                                        Resizable="false" />

                                    <telerik:GridButtonColumn HeaderStyle-Width="50" UniqueName="ColonnaElimina"
                                        Resizable="false"
                                        HeaderStyle-HorizontalAlign="Center"
                                        ItemStyle-Wrap="true"
                                        ItemStyle-HorizontalAlign="Center"
                                        ConfirmText="Eliminare l'allegato selezionato?"
                                        ConfirmTextFields="NomeFile"
                                        ConfirmTextFormatString="Eliminare l'allegato '{0}'?"
                                        ConfirmDialogType="RadWindow"
                                        ConfirmTitle="Elimina"
                                        ButtonType="ImageButton"
                                        ImageUrl="/UI/Images/Toolbar/24x24/delete.png"
                                        Text="Elimina Allegato"
                                        CommandName="Delete" />

                                </Columns>

                            </MasterTableView>

                            <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" />
                            <GroupingSettings CaseSensitive="false" />

                            <ClientSettings EnableRowHoverStyle="true">
                                <Selecting AllowRowSelect="True" />
                                <Resizing AllowColumnResize="true" EnableRealTimeResize="true" AllowResizeToFit="false" />
                            </ClientSettings>

                        </telerik:RadGrid>
</telerik:RadAjaxPanel>
                    </telerik:LayoutColumn>
                </Columns>
            </telerik:LayoutRow>
            <telerik:LayoutRow ID="lrCaricamentoFile" runat="server" RowType="Region" >
                <Columns>
                    <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="0">
                        <asp:Label runat="server" Text="File da caricare" AssociatedControlID="ruCaricamentoAllegato"/><br />
                        <telerik:RadUpload ID="ruCaricamentoAllegato" runat="server" ControlObjectsVisibility="None" InitialFileInputsCount="1" MaxFileInputsCount="1" Visible="true" />        
                    </telerik:LayoutColumn>
    <%--                <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="4">
                        <asp:Label runat="server" Text="Tipologia Documento" AssociatedControlID="rcbTipologiaDocumento"/><br />
                        <telerik:RadComboBox ID="rcbTipologiaDocumento" runat="server" 
                            Width="100%"
                            DataValueField="Key"
                            DataTextField="Value"
                            HighlightTemplatedItems="true"
                            MarkFirstMatch="false"
                            EmptyMessage="Seleziona una tipologia di documento"
                            AllowCustomText="false" 
                            EnableItemCaching="true"/>
                    </telerik:LayoutColumn>--%>
                    <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="4">
                        <asp:Label runat="server" Text="Note" AssociatedControlID="rtbNote"/><br />
                        <telerik:RadTextBox ID="rtbNote" runat="server" Width="100%"/>
                    </telerik:LayoutColumn>
                    <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="0">
                        <br />
                        <telerik:RadButton ID="rbCarica" runat="server" Text="Carica" OnClientClicking="rbCarica_OnClientClicking" Height="25px" OnClick="rbCarica_Click"/>
                    </telerik:LayoutColumn>
                </Columns>
            </telerik:LayoutRow>
            <telerik:LayoutRow >
                <Columns>
                    <telerik:LayoutColumn>
                        <br />
                    </telerik:LayoutColumn>
                </Columns>
            </telerik:LayoutRow>
        </Rows>
    </telerik:RadPageLayout>



<telerik:RadProgressArea id="rpaCarcamentoAllegati" runat="server" Style="position:absolute;top:150px;left:150px;width:700px;z-index:9999" OnClientProgressBarUpdating="LockPage" />
<telerik:RadProgressManager id="rpmCaricamentoAllegati" runat="server" />
<div id="divContenitoreAjax" runat="server" style="display: none;"></div> 
