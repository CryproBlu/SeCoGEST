<%@ Page Title="Analisi Costi" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="AnalisiCosto.aspx.cs" Inherits="SeCoGEST.Web.Preventivi.AnalisiCosto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Preventivi/AnalisiCostoRaggruppamentoHeader.ascx" TagPrefix="uc1" TagName="AnalisiCostoRaggruppamentoHeader" %>
<%@ Register Src="~/Preventivi/AnalisiCostoRaggruppamentoEditItem.ascx" TagPrefix="uc1" TagName="AnalisiCostoRaggruppamentoEditItem" %>
<%@ Register Src="~/UI/PageMessage.ascx" TagPrefix="uc1" TagName="PageMessage" %>


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
    <asp:Label ID="lblTitolo" runat="server" Text="Gestione Analisi dei Costi" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">

    <telerik:RadToolBar ID="RadToolBar1"
        runat="server"
        OnButtonClick="RadToolBar1_ButtonClick"
        OnClientButtonClicking="ToolBarButtonClicking">
        <Items>
            <telerik:RadToolBarButton runat="server" Value="TornaElenco" CommandName="TornaElenco" CausesValidation="False" NavigateUrl="AnalisiCosti.aspx" PostBack="False" ImageUrl="~/UI/Images/Toolbar/back.png" Text="Vai all'Elenco" ToolTip="Vai alla pagina di elenco Analisi Costi" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/refresh.png" PostBack="False" Value="Aggiorna" CommandName="Aggiorna" Text="Aggiorna" ToolTip="Aggiorna i dati visualizzati" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" NavigateUrl="AnalisiCosto.aspx" ImageUrl="~/UI/Images/Toolbar/add.png" PostBack="False" CommandName="Nuovo" Value="Nuovo" Text="Nuova Analisi dei Costi" ToolTip="Permette la creazione di un nuovo documento di Analisi dei Costi" Target="_blank" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreSalva" />
            <telerik:RadToolBarButton runat="server" CommandName="Salva" ImageUrl="~/UI/Images/Toolbar/Save.png" Text="Salva" Value="Salva" ToolTip="Memorizza i dati visualizzati" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" NavigateUrl="AnalisiVendita.aspx" ImageUrl="~/UI/Images/Toolbar/add.png" PostBack="False" CommandName="NuovaAnalisiVendita" Value="NuovaAnalisiVendita" Text="Nuova Analisi di Vendita" ToolTip="Crea un nuovo documento di Analisi di Vendita associato a questa Analisi dei Costi" Target="_blank" />
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
                            <telerik:RadNumericTextBox runat="server" ID="rntbNumero" MinValue="1" Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" Width="100%" Font-Bold="true" ReadOnly="true"></telerik:RadNumericTextBox>
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="2" SpanMd="3" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblNumeroRevisione" runat="server" Text="Revisione" AssociatedControlID="rtbNumeroRevisione" /><br />
                            <telerik:RadTextBox runat="server" ID="rtbNumeroRevisione" Width="100%" ReadOnly="true"></telerik:RadTextBox>
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
                
                <telerik:LayoutRow ID="rigaTotaliGlobali" runat="server" RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="2" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12">
                            <telerik:RadLabel ID="lblTotaleCostoGlobale" runat="server" Text="Totale Costo" AssociatedControlID="rntbTotaleCostoGlobale" Font-Bold="true"/>
                            <br />
                            <telerik:RadNumericTextBox runat="server" ID="rntbTotaleCostoGlobale" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" DisplayText='<%#String.Format("{0:c}", Eval("TotaleCosto")) %>' Value='<%# (double?)(decimal?)Eval("TotaleCosto") %>'></telerik:RadNumericTextBox>
                        </telerik:LayoutColumn>
                        <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="2" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12">
                            <telerik:RadLabel ID="lblTotaleVenditaGlobale" runat="server" Text="Totale Vendita" AssociatedControlID="rntbTotaleVenditaGlobale" Font-Bold="true" />
                            <br />
                            <telerik:RadNumericTextBox runat="server" ID="rntbTotaleVenditaGlobale" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" DisplayText='<%#String.Format("{0:c}", Eval("TotaleVendita")) %>' Value='<%# (double?)(decimal?)Eval("TotaleVendita") %>'></telerik:RadNumericTextBox>
                        </telerik:LayoutColumn>
                        <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="2" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12">
                            <telerik:RadLabel ID="lblTotaleRedditivitaGlobale" runat="server" Text="Totale Redditività" AssociatedControlID="rntbTotaleRedditivitaGlobale" Font-Bold="true" />
                            <br />
                            <telerik:RadNumericTextBox runat="server" ID="rntbTotaleRedditivitaGlobale" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" DisplayText='<%#String.Format("{0:c} ({1}%)", Eval("TotaleRicaricoValuta"), Eval("TotaleRicaricoPercentuale")) %>' Value='<%# (double?)(decimal?)Eval("TotaleRicaricoPercentuale") %>'></telerik:RadNumericTextBox>
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>



                <telerik:LayoutRow runat="server" ID="rowRaggruppamenti" RowType="Region" Visible="false">
                    <Columns>
                        <telerik:LayoutColumn CssClass="SpaziaturaSuperioreGruppo">

                            <asp:Repeater runat="server" ID="repRaggruppamenti" OnItemCommand="repRaggruppamenti_ItemCommand">
                                <ItemTemplate>

                                    <asp:HiddenField runat="server" ID="hdIdArticoloInEdit" />
                                    <asp:HiddenField runat="server" ID="hdIdRaggruppamento" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />

                                    <asp:Panel runat="server" ID="panelHeader" BorderStyle="None">
                                        <uc1:AnalisiCostoRaggruppamentoHeader runat="server" ID="HeaderRaggruppamento"></uc1:AnalisiCostoRaggruppamentoHeader>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="panelContent" BackColor="white" Style="padding: 10px; padding-bottom: 10px; overflow-x: hidden;">

                                        <asp:Button runat="server" ID="btAggiungiArticolo" Text="Aggiungi articolo..." Visible="true"
                                            CommandName="AggiungiArticolo" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                                        <asp:Button runat="server" ID="btSalvaArticolo" Text="Salva" Visible="false"
                                            CommandName="SalvaArticolo" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                                        <asp:Button runat="server" ID="btAggiornaArticolo" Text="Salva" Visible="false"
                                            CommandName="AggiornaArticolo" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                                        <asp:Button runat="server" ID="btAnnullaEdit" Text="Annulla" Visible="false"
                                            CommandName="AnnullaEdit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' />
                                        <asp:Button runat="server" ID="btClonaGruppo" Text="Clona gruppo..." Visible="true"
                                            CommandName="ClonaGruppo" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' OnClientClick="alert('Funzionalità ancora non implementata!');" />
                                        <br />

                                        <uc1:AnalisiCostoRaggruppamentoEditItem runat="server" ID="EditItemRaggruppamento" Visible="false"></uc1:AnalisiCostoRaggruppamentoEditItem>
                                        <uc1:PageMessage runat="server" ID="pmMessaggioConfigurazioneAggiuntiva" Visible="false" />
                                        <asp:Button ID="btEsecuzioneConfigurazioniArticoliAggiuntivi" runat="server" Text="Esecuzione Configurazione Articoli Aggiuntivi..." Visible="false" OnClientClick="javascript:if (!confirm('Eseguire le configurazioni articoli aggiuntive trovate?')) {return false;} " OnClick="btEsecuzioneConfigurazioniArticoliAggiuntivi_Click" />


                                        <br />
                                        <telerik:RadGrid runat="server"
                                            ID="rgGrigliaArticoli"
                                            CssClass="GridResponsiveColumns"
                                            AllowPaging="false"
                                            AllowSorting="false"
                                            AllowMultiRowSelection="false"
                                            AllowFilteringByColumn="false"
                                            GridLines="None"
                                            AutoGenerateColumns="false"
                                            OnNeedDataSource="rgGrigliaArticoli_NeedDataSource"
                                            OnDetailTableDataBind="rgGrigliaArticoli_DetailTableDataBind"
                                            OnPreRender="rgGrigliaArticoli_PreRender"
                                            OnItemCommand="rgGrigliaArticoli_ItemCommand"
                                            OnDeleteCommand="rgGrigliaArticoli_DeleteCommand"
                                            Width="100%">
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
                                                    <%--                                                    <telerik:GridClientSelectColumn UniqueName="SelectionColumn" HeaderStyle-Width="40px"></telerik:GridClientSelectColumn>--%>
                                                    <%--                                                    <telerik:GridEditCommandColumn HeaderStyle-Width="50" ButtonType="FontIconButton"
                                                         UpdateText="Aggiorna" CancelText="Cancel" EditText="Modifica" ></telerik:GridEditCommandColumn>--%>

                                                    <telerik:GridButtonColumn HeaderStyle-Width="40" UniqueName="ColonnaModifica"
                                                        Resizable="false"
                                                        HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-Wrap="true"
                                                        ItemStyle-HorizontalAlign="Center"
                                                        ButtonType="ImageButton"
                                                        ImageUrl="/UI/Images/Toolbar/16x16/edit.png"
                                                        Text="Modifica..."
                                                        CommandName="Modifica" />

                                                    <telerik:GridBoundColumn SortExpression="CodiceGruppo" HeaderStyle-Width="200px" HeaderText="Gruppo" HeaderButtonType="TextButton"
                                                        DataField="TABGRUPPI.DESCRIZIONE">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn SortExpression="CodiceCategoria" HeaderStyle-Width="200px" HeaderText="Categoria" HeaderButtonType="TextButton"
                                                        DataField="TABCATEGORIE.DESCRIZIONE">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn SortExpression="CodiceCategoriaStatistica" HeaderStyle-Width="200px" HeaderText="Categoria Statistica" HeaderButtonType="TextButton"
                                                        DataField="TABCATEGORIESTAT.DESCRIZIONE">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn SortExpression="Descrizione" HeaderStyle-Width="100%" HeaderText="Descrizione" HeaderButtonType="TextButton"
                                                        DataField="Descrizione">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn SortExpression="UnitaMisura" HeaderText="UM" HeaderButtonType="TextButton"
                                                        DataField="UnitaMisura" HeaderStyle-Width="50px">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn SortExpression="TotaleCosto" HeaderText="Totale Costo" HeaderButtonType="TextButton"
                                                        DataField="TotaleCosto" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="150px">
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

                                        <telerik:LayoutRow ID="rigaTotaliGruppo" runat="server" RowType="Region" Visible="True">
                                            <Columns>
                                                <telerik:LayoutColumn CssClass="SpaziaturaSuperioreGruppo" Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12">
                                                    <hr />
                                                    <h2>TOTALI</h2>
                                                </telerik:LayoutColumn>
                                                <telerik:LayoutColumn CssClass="" Span="4" SpanXl="4" SpanLg="4" SpanMd="4" SpanSm="12" SpanXs="12">&nbsp;</telerik:LayoutColumn>
                                                <telerik:LayoutColumn CssClass="AllineaDestra" Span="2" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12">
                                                    <br />
                                                    <asp:Button runat="server" ID="SalvaModificheGruppo" Text="Salva" Visible="true" CommandName="SalvaModificheGruppo" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' OnClick="SalvaModificheGruppo_Click" />
                                                    <asp:Button runat="server" ID="AnnullaModificheGruppo" Text="Annulla" Visible="true" CommandName="AnnullaModificheGruppo" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' OnClick="AnnullaModificheGruppo_Click" />
                                                </telerik:LayoutColumn>
                                                <telerik:LayoutColumn CssClass="" Span="2" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12">
                                                    <telerik:RadLabel ID="lblTotaleCosto" runat="server" Text="Totale Costo" AssociatedControlID="rntbTotaleCosto" />
                                                    <br />
                                                    <telerik:RadNumericTextBox runat="server" ID="rntbTotaleCosto" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" DisplayText='<%#String.Format("{0:c}", Eval("TotaleCosto")) %>' Value='<%# (double?)(decimal?)Eval("TotaleCosto") %>'></telerik:RadNumericTextBox>
                                                </telerik:LayoutColumn>
                                                <telerik:LayoutColumn CssClass="" Span="2" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12">
                                                    <telerik:RadLabel ID="lblTotaleVendita" runat="server" Text="Totale Vendita" AssociatedControlID="rntbTotaleVendita" />
                                                    <br />
                                                    <telerik:RadNumericTextBox runat="server" ID="rntbTotaleVendita" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" DisplayText='<%#String.Format("{0:c}", Eval("TotaleVendita")) %>' Value='<%# (double?)(decimal?)Eval("TotaleVendita") %>'></telerik:RadNumericTextBox>
                                                </telerik:LayoutColumn>
                                                <telerik:LayoutColumn CssClass="" Span="2" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12">
                                                    <telerik:RadLabel ID="lblTotaleRedditivita" runat="server" Text="Totale Redditività" AssociatedControlID="rntbTotaleRedditivita" />
                                                    <br />
                                                    <telerik:RadNumericTextBox runat="server" ID="rntbTotaleRedditivita" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" DisplayText='<%#String.Format("{0:c} ({1}%)", Eval("TotaleRicaricoValuta"), Eval("TotaleRicaricoPercentuale")) %>' Value='<%# (double?)(decimal?)Eval("TotaleRicaricoPercentuale") %>'></telerik:RadNumericTextBox>
                                                </telerik:LayoutColumn>
                                            </Columns>
                                        </telerik:LayoutRow>

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
                                        ExpandDirection="Vertical"></ajaxToolkit:CollapsiblePanelExtender>

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
        <%--        <telerik:RadAjaxManagerProxy runat="server" ID="ramAnalisiCosti">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgGrigliaArticoli">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgGrigliaArticoli" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                        <telerik:AjaxUpdatedControl ControlID="RadWindowManagerMaster" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManagerProxy>--%>
        <%--        <telerik:RadPersistenceManagerProxy ID="rpmpAnalisiCosti" runat="server">
            <PersistenceSettings>
                <telerik:PersistenceSetting ControlID="rgGrigliaArticoli" />
            </PersistenceSettings>
        </telerik:RadPersistenceManagerProxy>--%>
    </div>

</asp:Content>
