<%@ Page Title="Analisi Vendita" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="AnalisiVendita.aspx.cs" Inherits="SeCoGEST.Web.Preventivi.AnalisiVendita" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Preventivi/AnalisiCostoRaggruppamentoHeader.ascx" TagPrefix="uc1" TagName="AnalisiCostoRaggruppamentoHeader" %>
<%@ Register Src="~/Preventivi/AnalisiCostoRaggruppamentoEditItem.ascx" TagPrefix="uc1" TagName="AnalisiCostoRaggruppamentoEditItem" %>

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
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    <asp:Label ID="lblTitolo" runat="server" Text="Gestione Analisi di Vendita" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">

    <telerik:RadToolBar ID="RadToolBar1"
        runat="server"
        OnButtonClick="RadToolBar1_ButtonClick"
        OnClientButtonClicking="ToolBarButtonClicking">
        <Items>
            <telerik:RadToolBarButton runat="server" Value="TornaElenco" CommandName="TornaElenco" CausesValidation="False" NavigateUrl="AnalisiVendite.aspx" PostBack="False" ImageUrl="~/UI/Images/Toolbar/back.png" Text="Vai all'Elenco" ToolTip="Vai alla pagina di elenco Analisi Vendite" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/refresh.png" PostBack="False" Value="Aggiorna" CommandName="Aggiorna" Text="Aggiorna" ToolTip="Aggiorna i dati visualizzati" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" NavigateUrl="AnalisiVendita.aspx" ImageUrl="~/UI/Images/Toolbar/add.png" PostBack="False" CommandName="Nuovo" Value="Nuovo" Text="Nuova Analisi dei Vendite" ToolTip="Permette la creazione di un nuovo documento di Analisi dei Vendite" Target="_blank" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreSalva" />
            <telerik:RadToolBarButton runat="server" CommandName="Salva" ImageUrl="~/UI/Images/Toolbar/Save.png" Text="Salva" Value="Salva" ToolTip="Memorizza i dati visualizzati" />
        </Items>
    </telerik:RadToolBar>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server"
        CssClass="ValidationSummaryStyle"
        HeaderText="&nbsp;Errori di validazione dei dati:"
        DisplayMode="BulletList"
        ShowMessageBox="false"
        ShowSummary="true" />

    <div class="pageBody">

        <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
            <Rows>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="2" SpanMd="3" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblNumero" runat="server" Text="Numero" AssociatedControlID="rntbNumero" Font-Bold="true" /><br />
                            <telerik:RadTextBox runat="server" ID="rntbNumero" Width="100%" Font-Bold="true" ReadOnly="true"></telerik:RadTextBox>
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="2" SpanMd="3" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblNumeroAnalisiCosto" runat="server" Text="N. Analisi Costo" AssociatedControlID="rtbNumeroAnalisiCosto" /><br />
                            <telerik:RadTextBox runat="server" ID="rtbNumeroAnalisiCosto" Width="100%" ReadOnly="true"></telerik:RadTextBox>
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="3" SpanMd="3" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblDataRedazione" runat="server" Text="Data Redazione" AssociatedControlID="rdtpDataRedazione" Font-Bold="true" />&nbsp;
                            <asp:RequiredFieldValidator ID="rfvDataRedazione" runat="server" Display="Dynamic"
                                ControlToValidate="rdtpDataRedazione"
                                ErrorMessage="La Data di Redazione è obbligatoria."
                                ForeColor="Red">
                                <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                            </asp:RequiredFieldValidator><br />
                            <telerik:RadDateTimePicker ID="rdtpDataRedazione" Width="100%" runat="server">
                                <TimeView runat="server"
                                    ShowHeader="false"
                                    StartTime="07:00:00"
                                    Interval="00:15:00"
                                    EndTime="19:00:00"
                                    Columns="4">
                                </TimeView>
                            </telerik:RadDateTimePicker>
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="2" SpanMd="3" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblModalitàOfferta" runat="server" Text="Modalità offerta" AssociatedControlID="rcbModalitàOfferta" /><br />
                            <telerik:RadComboBox ID="rcbModalitàOfferta" runat="server" 
                                Width="100%" ZIndex="10000">
                                <Items>
                                    <telerik:RadComboBoxItem Text="Pagamento in unica soluzione" Value="0" />
                                    <telerik:RadComboBoxItem Text="Pagamento rateale" Value="1" />
                                    <telerik:RadComboBoxItem Text="Noleggio" Value="2" />
                                </Items>
                            </telerik:RadComboBox>   
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="8" SpanXl="8" SpanLg="5" SpanMd="12" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblTitoloAnalisi" runat="server" Text="Titolo" Font-Bold="true" AssociatedControlID="rtbTitoloAnalisi" />&nbsp;
                            <asp:RequiredFieldValidator ID="rfvTitoloAnalisi" runat="server" Display="Dynamic"
                                ControlToValidate="rtbTitoloAnalisi"
                                ErrorMessage="Il Titolo del documento è obbligatorio."
                                ForeColor="Red">
                                <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                            </asp:RequiredFieldValidator>
                            <br />
                            <telerik:RadTextBox runat="server" ID="rtbTitoloAnalisi" Width="100%"></telerik:RadTextBox>
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>


                
                <telerik:LayoutRow runat="server" ID="rowRaggruppamenti" RowType="Region" Visible="false">
                    <Columns>
                        <telerik:LayoutColumn CssClass="SpaziaturaSuperioreGruppo">

                            <asp:Repeater runat="server" ID="repRaggruppamenti" 
                                OnItemDataBound="repRaggruppamenti_ItemDataBound" OnItemCommand="repRaggruppamenti_ItemCommand" >
                                <ItemTemplate>
                                    <asp:Panel runat="server" ID="panelHeader" BorderStyle="None">
                                        <%--<uc1:AnalisiVenditaRaggruppamentoHeader runat="server" ID="HeaderRaggruppamento"></uc1:AnalisiVenditaRaggruppamentoHeader>--%>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="panelContent" BackColor="white" style="padding:10px; padding-bottom:50px;">

                                        <asp:Button runat="server" ID="btAggiungiArticolo" Text="Aggiungi articolo..." Visible="true"
                                            CommandName="AggiungiArticolo" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'/>
                                        <asp:Button runat="server" ID="btSalvaArticolo" Text="Salva" Visible="false"
                                            CommandName="SalvaArticolo" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'/>
                                        <asp:Button runat="server" ID="btAnnullaEdit" Text="Annulla" Visible="false"
                                            CommandName="AnnullaEdit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>'/>
                                        <br />

                                        <%--<uc1:AnalisiVenditaRaggruppamentoEditItem runat="server" ID="EditItemRaggruppamento" Visible="false"></uc1:AnalisiVenditaRaggruppamentoEditItem>--%>

                                        <br />
                                        <telerik:RadGrid runat="server" 
                                            ID="rgGrigliaArticoli"
                                            CssClass="GridResponsiveColumns"
                                            AllowPaging="false"
                                            AllowSorting="false"
                                            AllowFilteringByColumn="false"
                                            GridLines="None"
                                            AutoGenerateColumns="false"
                                            OnDetailTableDataBind="rgGrigliaArticoli_DetailTableDataBind"
                                            OnPreRender="rgGrigliaArticoli_PreRender"
                                            OnDeleteCommand="rgGrigliaArticoli_DeleteCommand">
                                            <MasterTableView DataKeyNames="ID" TableLayout="Fixed" 
                                                Caption="ELENCO ARTICOLI DEL GRUPPO"
                                                Width="100%"
                                                AllowFilteringByColumn="false"
                                                AllowSorting="false"
                                                AllowMultiColumnSorting="false"
                                                GridLines="Both"
                                                NoMasterRecordsText="&nbsp;Questo gruppo non contiene ancora articoli!"
                                                CommandItemSettings-ShowRefreshButton="false"
                                                CommandItemSettings-ShowAddNewRecordButton="false"
                                                CommandItemDisplay="None">

                                                <DetailTables>
                                                    <telerik:GridTableView DataKeyNames="ID" Name="CampiAggiuntivi" Width="100%" 
                                                        NoDetailRecordsText="&nbsp;Non sono stati registrati dati aggiuntivi per questo articolo.">
                                                        <Columns>
                                                            <telerik:GridBoundColumn SortExpression="NomeCampo" HeaderText="NomeCampo" HeaderButtonType="TextButton"
                                                                DataField="NomeCampo" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn SortExpression="Valore" HeaderText="Valore" HeaderButtonType="TextButton"
                                                                DataField="Valore">
                                                            </telerik:GridBoundColumn>
                                                        </Columns>

                                                    </telerik:GridTableView>
                                                </DetailTables>

                                                <Columns>
                                                    <telerik:GridEditCommandColumn ButtonType="FontIconButton" UpdateText="Update" CancelText="Cancel" EditText="Modifica" ></telerik:GridEditCommandColumn>
                                                    <telerik:GridBoundColumn SortExpression="CodiceGruppo" HeaderText="Gruppo" HeaderButtonType="TextButton"
                                                        DataField="TABGRUPPI.DESCRIZIONE">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn SortExpression="CodiceCategoria" HeaderText="Categoria" HeaderButtonType="TextButton"
                                                        DataField="TABCATEGORIE.DESCRIZIONE">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn SortExpression="CodiceCategoriaStatistica" HeaderText="Categoria Statistica" HeaderButtonType="TextButton"
                                                        DataField="TABCATEGORIESTAT.DESCRIZIONE">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn SortExpression="Descrizione" HeaderText="Descrizione" HeaderButtonType="TextButton"
                                                        DataField="Descrizione">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn SortExpression="UnitaMisura" HeaderText="UM" HeaderButtonType="TextButton"
                                                        DataField="UnitaMisura" HeaderStyle-Width="50px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn SortExpression="TotaleVendita" HeaderText="Totale Vendita" HeaderButtonType="TextButton"
                                                        DataField="TotaleVendita" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="150px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn SortExpression="TotaleVendita" HeaderText="Totale Vendita" HeaderButtonType="TextButton"
                                                        DataField="TotaleVendita" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="150px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridButtonColumn HeaderStyle-Width="40" UniqueName="ColonnaElimina"
                                                        Resizable="false"
                                                        HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-Wrap="true"
                                                        ItemStyle-HorizontalAlign="Center"
                                                        ConfirmText="Rimuovere l'articolo selezionato?"
                                                        ConfirmTextFields="Descrizione"
                                                        ConfirmTextFormatString="Eliminare l'articolo '{0}'?"
                                                        ConfirmDialogType="RadWindow"
                                                        ConfirmTitle="Elimina"
                                                        ButtonType="ImageButton"
                                                        ImageUrl="/UI/Images/Toolbar/16x16/delete.png"
                                                        Text="Elimina..."
                                                        CommandName="Delete" />

                                                    <telerik:GridBoundColumn DataField="ContieneCampiAggiuntivi" UniqueName="ContieneCampiAggiuntivi" Display="false"></telerik:GridBoundColumn>
                                                </Columns>


                                            </MasterTableView>
                                            <ClientSettings EnableRowHoverStyle="true">
                                                <Selecting AllowRowSelect="True" />
                                                <Resizing AllowColumnResize="true" EnableRealTimeResize="true" AllowResizeToFit="false" />
                                            </ClientSettings>
                                        </telerik:RadGrid>

                                    </asp:Panel>
                    
                                    <ajaxToolkit:CollapsiblePanelExtender ID="cpe" runat="Server"
                                        CollapseControlID="panelHeader" 
                                        ExpandControlID="panelHeader" 
                                        TargetControlID="panelContent"
                                        CollapsedSize="0"
                                        Collapsed="True"
                                        AutoCollapse="False"
                                        AutoExpand="False"
                                        ScrollContents="false"
                                        ExpandDirection="Vertical" >
                                    </ajaxToolkit:CollapsiblePanelExtender>
                                </ItemTemplate>
                            </asp:Repeater>

                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>












            </Rows>
        </telerik:RadPageLayout>

        <br />
        <br />
        <br />
    </div>

    
    <div style="display: none;">
        <telerik:RadAjaxManagerProxy runat="server" ID="ramAnalisiVendite">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgGrigliaArticoli">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgGrigliaArticoli" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                        <telerik:AjaxUpdatedControl ControlID="RadWindowManagerMaster" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManagerProxy>
<%--        <telerik:RadPersistenceManagerProxy ID="rpmpAnalisiVendite" runat="server">
            <PersistenceSettings>
                <telerik:PersistenceSetting ControlID="rgGrigliaArticoli" />
            </PersistenceSettings>
        </telerik:RadPersistenceManagerProxy>--%>
    </div>

</asp:Content>
