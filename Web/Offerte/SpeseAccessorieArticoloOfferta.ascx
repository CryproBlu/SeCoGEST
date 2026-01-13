<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpeseAccessorieArticoloOfferta.ascx.cs" Inherits="SeCoGEST.Web.Offerte.SpeseAccessorieArticoloOfferta" %>

<telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
    <Rows>
        <telerik:LayoutRow ID="rigaSelezioneArticoli" runat="server" >
            <Content>
                <telerik:RadAjaxPanel ID="rapSelezioneArticoli" runat="server" OnAjaxRequest="rapSelezioneArticoli_AjaxRequest">
                    <telerik:RadPageLayout ID="rplSelezioneArticoli" runat="server" Width="100%" HtmlTag="None" GridType="Fluid" Visible="false">
                        <Rows>
                            <telerik:LayoutRow RowType="Region">
                                <Columns>
                                    <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreGruppo">
                                        <fieldset style="background-color: #fff;">
                                            <legend><strong>Selezione Spesa Accessoria</strong></legend>
                                            <asp:HiddenField ID="hfIdArticoloSpesaAccessoria" runat="server" />
                                            <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                                <Rows>
                                                    <telerik:LayoutRow RowType="Region">
                                                        <Columns>
                                                            <telerik:LayoutColumn Span="12">
                                                                <asp:ValidationSummary ID="vsSeelzioneArticolo" runat="server"
                                                                    CssClass="ValidationSummaryStyle"
                                                                    ValidationGroup="SelezioneArticoloPerSpeseDiGestione"
                                                                    HeaderText="&nbsp;Errori di validazione dei dati:"
                                                                    DisplayMode="BulletList"
                                                                    ShowMessageBox="false"
                                                                    ShowSummary="true" />
                                                            </telerik:LayoutColumn>
                                                        </Columns>
                                                    </telerik:LayoutRow>
                                                    <telerik:LayoutRow RowType="Region">
                                                        <Columns>
                                                            <telerik:LayoutColumn Span="3" SpanXl="3" SpanLg="3" SpanMd="3" SpanSm="12" SpanXs="12">
                                                                <asp:Label ID="lblGruppo" runat="server" Text="Gruppo" AssociatedControlID="rcbGruppo" /><br />
                                                                <telerik:RadComboBox runat="server" ID="rcbGruppo" DataValueField="CODICE" DataTextField="DESCRIZIONE" Width="100%"
                                                                    CausesValidation="false"
                                                                    ValidationGroup="SelezioneArticoloPerSpeseDiGestione"
                                                                    DropDownAutoWidth="Enabled"
                                                                    MarkFirstMatch="false"
                                                                    HighlightTemplatedItems="true"
                                                                    EmptyMessage="Selezionare un gruppo"
                                                                    ItemsPerRequest="20"
                                                                    ShowMoreResultsBox="true"
                                                                    EnableLoadOnDemand="true"
                                                                    EnableItemCaching="false"
                                                                    AllowCustomText="true"
                                                                    LoadingMessage="Caricamento in corso..."
                                                                    OnItemsRequested="rcbGruppo_ItemsRequested">
                                                                </telerik:RadComboBox>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="3" SpanXl="3" SpanLg="3" SpanMd="3" SpanSm="12" SpanXs="12">
                                                                <asp:Label ID="lblCategoria" runat="server" Text="Categoria" AssociatedControlID="rcbCategoria" /><br />
                                                                <telerik:RadComboBox runat="server" ID="rcbCategoria" DataValueField="CODICE" DataTextField="DESCRIZIONE" Width="100%"
                                                                    CausesValidation="false"
                                                                    ValidationGroup="SelezioneArticoloPerSpeseDiGestione"
                                                                    DropDownAutoWidth="Enabled"
                                                                    MarkFirstMatch="false"
                                                                    HighlightTemplatedItems="true"
                                                                    EmptyMessage="Selezionare una categoria"
                                                                    ItemsPerRequest="20"
                                                                    ShowMoreResultsBox="true"
                                                                    EnableLoadOnDemand="true"
                                                                    EnableItemCaching="false"
                                                                    AllowCustomText="true"
                                                                    LoadingMessage="Caricamento in corso..."
                                                                    OnItemsRequested="rcbCategoria_ItemsRequested">
                                                                </telerik:RadComboBox>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="3" SpanXl="3" SpanLg="3" SpanMd="3" SpanSm="12" SpanXs="12">
                                                                <asp:Label ID="lblCategoriaStatistica" runat="server" Text="Categoria statistica" AssociatedControlID="rcbCategoriaStatistica" /><br />
                                                                <telerik:RadComboBox runat="server" ID="rcbCategoriaStatistica" DataValueField="CODICE" DataTextField="DESCRIZIONE" Width="100%"
                                                                    CausesValidation="false"
                                                                    ValidationGroup="SelezioneArticoloPerSpeseDiGestione"
                                                                    DropDownAutoWidth="Enabled"
                                                                    MarkFirstMatch="false"
                                                                    HighlightTemplatedItems="true"
                                                                    EmptyMessage="Selezionare una categoria statistica"
                                                                    ItemsPerRequest="20"
                                                                    ShowMoreResultsBox="true"
                                                                    EnableLoadOnDemand="true"
                                                                    EnableItemCaching="false"
                                                                    AllowCustomText="true"
                                                                    LoadingMessage="Caricamento in corso..."
                                                                    OnItemsRequested="rcbCategoriaStatistica_ItemsRequested">
                                                                </telerik:RadComboBox>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="3" SpanXl="3" SpanLg="3" SpanMd="3" SpanSm="12" SpanXs="12">
                                                                <asp:Label ID="lblCodiceArticolo" runat="server" Text="Codice Articolo" AssociatedControlID="rcbCodiceArticolo" Font-Bold="true" />
                                                                <asp:RequiredFieldValidator ID="rfvCodiceArticolo" runat="server" Display="Dynamic"
                                                                    ControlToValidate="rcbCodiceArticolo"
                                                                    ValidationGroup="SelezioneArticoloPerSpeseDiGestione"
                                                                    ErrorMessage="Il codice articolo è obbligatorio."
                                                                    ForeColor="Red">
                                                                    <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                                                </asp:RequiredFieldValidator>
                                                                <br />
                                                                <telerik:RadComboBox ID="rcbCodiceArticolo" runat="server"
                                                                    CausesValidation="false"
                                                                    ValidationGroup="SelezioneArticoloPerSpeseDiGestione"
                                                                    Width="100%"
                                                                    DropDownAutoWidth="Enabled"
                                                                    MarkFirstMatch="false"
                                                                    HighlightTemplatedItems="true"
                                                                    EmptyMessage="Selezionare un Articolo"
                                                                    ItemsPerRequest="20"
                                                                    ShowMoreResultsBox="true"
                                                                    EnableLoadOnDemand="true"
                                                                    AllowCustomText="true"
                                                                    LoadingMessage="Caricamento in corso..."
                                                                    DataValueField="CodiceArticolo"
                                                                    DataTextField="Descrizione"
                                                                    EnableVirtualScrolling="true"
                                                                    OnItemsRequested="rcbCodiceArticolo_ItemsRequested">
                                                                    <HeaderTemplate>
                                                                        <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                                                            <Rows>
                                                                                <telerik:LayoutRow RowType="Region">
                                                                                    <Columns>
                                                                                        <telerik:LayoutColumn Span="4">
                                                                                            <asp:Label runat="server" Text="Codice" Font-Bold="true" />
                                                                                        </telerik:LayoutColumn>
                                                                                        <telerik:LayoutColumn Span="8">
                                                                                            <asp:Label runat="server" Text="Descrizione" Font-Bold="true" />
                                                                                        </telerik:LayoutColumn>
                                                                                    </Columns>
                                                                                </telerik:LayoutRow>
                                                                            </Rows>
                                                                        </telerik:RadPageLayout>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                                                            <Rows>
                                                                                <telerik:LayoutRow RowType="Region">
                                                                                    <Columns>
                                                                                        <telerik:LayoutColumn Span="4">
                                                                                            <%# DataBinder.Eval(Container.DataItem, "CODICE") %>
                                                                                        </telerik:LayoutColumn>
                                                                                        <telerik:LayoutColumn Span="8">
                                                                                            <%# DataBinder.Eval(Container.DataItem, "DESCRIZIONE") %>
                                                                                        </telerik:LayoutColumn>
                                                                                    </Columns>
                                                                                </telerik:LayoutRow>
                                                                            </Rows>
                                                                        </telerik:RadPageLayout>
                                                                    </ItemTemplate>
                                                                </telerik:RadComboBox>
                                                            </telerik:LayoutColumn>
                                                        </Columns>
                                                    </telerik:LayoutRow>
                                                    <telerik:LayoutRow ID="rigaInformazioniAggiuntiveArticolo" RowType="Region" CssClass="SpaziaturaSuperioreRiga">
                                                        <Columns>
                                                            <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="1" SpanMd="4" SpanSm="12" SpanXs="12">
                                                                <asp:Label ID="lblUM" runat="server" Text="UM" AssociatedControlID="rtbUM"  Font-Bold="true"/>
                                                                <asp:RequiredFieldValidator ID="rfvUM" runat="server" Display="Dynamic"
                                                                    ControlToValidate="rtbUM"
                                                                    ValidationGroup="SelezioneArticoloPerSpeseDiGestione"
                                                                    ErrorMessage="L'unità di misura è obbligatoria."
                                                                    ForeColor="Red">
                                                                    <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                                                </asp:RequiredFieldValidator>
                                                                <br />
                                                                <telerik:RadTextBox runat="server" ID="rtbUM" Width="100%" ValidationGroup="SelezioneArticoloPerSpeseDiGestione"></telerik:RadTextBox>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="1" SpanMd="4" SpanSm="12" SpanXs="12">
                                                                <asp:Label ID="lblQuantità" runat="server" Text="Q.tà" AssociatedControlID="rntbQuantità" Font-Bold="true" />
                                                                <asp:RequiredFieldValidator ID="rfvQuantità" runat="server" Display="Dynamic"
                                                                    ControlToValidate="rntbQuantità"
                                                                    ValidationGroup="SelezioneArticoloPerSpeseDiGestione"
                                                                    ErrorMessage="La quantità è obbligatoria."
                                                                    ForeColor="Red">
                                                                    <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                                                </asp:RequiredFieldValidator>
                                                                <br />
                                                                <telerik:RadNumericTextBox runat="server" ID="rntbQuantità" ValidationGroup="SelezioneArticoloPerSpeseDiGestione" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" MinValue="0">
                                                                </telerik:RadNumericTextBox>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="1" SpanMd="4" SpanSm="6" SpanXs="12">
                                                                <asp:Label ID="lblCosto" runat="server" Text="Costo unitario" AssociatedControlID="rntbCosto" Font-Bold="true" />
                                                                <asp:RequiredFieldValidator ID="rfvCosto" runat="server" Display="Dynamic"
                                                                    ControlToValidate="rntbCosto"
                                                                    ValidationGroup="SelezioneArticoloPerSpeseDiGestione"
                                                                    ErrorMessage="Il costo unitario è obbligatorio."
                                                                    ForeColor="Red">
                                                                    <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                                                </asp:RequiredFieldValidator>
                                                                <br />
                                                                <telerik:RadNumericTextBox runat="server" ID="rntbCosto" ValidationGroup="SelezioneArticoloPerSpeseDiGestione" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2">
                                                                </telerik:RadNumericTextBox>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="12">
                                                                <asp:Label ID="lblRicaricoValore" runat="server" Text="Ricarico € (su costo unitario)" AssociatedControlID="rntbRicaricoValore" /><br />
                                                                <telerik:RadNumericTextBox runat="server" ID="rntbRicaricoValore" ValidationGroup="SelezioneArticoloPerSpeseDiGestione" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2">
                                                                </telerik:RadNumericTextBox>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="12">
                                                                <asp:Label ID="lblRicaricoPercentuale" runat="server" Text="Ricarico % (su costo unitario)" AssociatedControlID="rntbRicaricoPercentuale" /><br />
                                                                <telerik:RadNumericTextBox runat="server" ID="rntbRicaricoPercentuale" ValidationGroup="SelezioneArticoloPerSpeseDiGestione" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2">
                                                                </telerik:RadNumericTextBox>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="1" SpanMd="4" SpanSm="6" SpanXs="12">
                                                                <asp:Label ID="lblVendita" runat="server" Text="Prezzo unitario" AssociatedControlID="rntbVendita" Font-Bold="true" />
                                                                <asp:RequiredFieldValidator ID="rfvVendita" runat="server" Display="Dynamic"
                                                                    ControlToValidate="rntbVendita"
                                                                    ValidationGroup="SelezioneArticoloPerSpeseDiGestione"
                                                                    ErrorMessage="Il prezzo unitario è obbligatorio."
                                                                    ForeColor="Red">
                                                                    <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                                                </asp:RequiredFieldValidator>
                                                                <br />
                                                                <telerik:RadNumericTextBox runat="server" ID="rntbVendita" ValidationGroup="SelezioneArticoloPerSpeseDiGestione" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2">
                                                                </telerik:RadNumericTextBox>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="12">
                                                                <asp:Label ID="lblTotaleCosto" runat="server" Text="Totale costo" AssociatedControlID="rntbTotaleCosto" Font-Bold="true" />
                                                                <asp:RequiredFieldValidator ID="rfvTotaleCosto" runat="server" Display="Dynamic"
                                                                    ControlToValidate="rntbTotaleCosto"
                                                                    ValidationGroup="SelezioneArticoloPerSpeseDiGestione"
                                                                    ErrorMessage="Il totale costo è obbligatorio."
                                                                    ForeColor="Red">
                                                                    <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                                                </asp:RequiredFieldValidator>
                                                                <br />
                                                                <telerik:RadNumericTextBox runat="server" ID="rntbTotaleCosto" ValidationGroup="SelezioneArticoloPerSpeseDiGestione" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2">
                                                                </telerik:RadNumericTextBox>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="12">
                                                                <asp:Label ID="lblTotaleVendita" runat="server" Text="Totale vendita" AssociatedControlID="rntbTotaleVendita" Font-Bold="true" />
                                                                <asp:RequiredFieldValidator ID="rfvTotaleVendita" runat="server" Display="Dynamic"
                                                                    ControlToValidate="rntbTotaleVendita"
                                                                    ValidationGroup="SelezioneArticoloPerSpeseDiGestione"
                                                                    ErrorMessage="Il totale vendita è obbligatorio."
                                                                    ForeColor="Red">
                                                                    <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                                                </asp:RequiredFieldValidator>
                                                                <br />
                                                                <telerik:RadNumericTextBox runat="server" ID="rntbTotaleVendita" ValidationGroup="SelezioneArticoloPerSpeseDiGestione" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2">
                                                                </telerik:RadNumericTextBox>
                                                            </telerik:LayoutColumn>
                                                        </Columns>
                                                    </telerik:LayoutRow>
                                                    <telerik:LayoutRow RowType="Region">
                                                        <Columns>
                                                            <telerik:LayoutColumn OffsetXl="10" OffsetMd="10" OffsetLg="10" OffsetSm="12" OffsetXs="12" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreGruppo" Style="text-align: right;">
                                                                <telerik:RadButton ID="rbSalvaArticoli" runat="server" Text="Salva" ButtonType="LinkButton" Icon-PrimaryIconCssClass="rbSave" ValidationGroup="SelezioneArticoloPerSpeseDiGestione" OnClick="rbSalvaArticoli_Click"></telerik:RadButton>
                                                                <telerik:RadButton ID="rbAnnullaInserimentoArticoli" runat="server" Text="Annulla" ButtonType="LinkButton" Icon-PrimaryIconCssClass="rbCancel" ConfirmSettings-Title="Richiesta Conferma" ConfirmSettings-UseRadConfirm="true" ConfirmSettings-ConfirmText="Annullare l'inserimento degli articoli?" CausesValidation="false" OnClick="rbAnnullaInserimentoArticoli_Click"></telerik:RadButton>
                                                            </telerik:LayoutColumn>
                                                        </Columns>
                                                    </telerik:LayoutRow>
                                                </Rows>
                                            </telerik:RadPageLayout>
                                        </fieldset>
                                    </telerik:LayoutColumn>
                                </Columns>
                            </telerik:LayoutRow>
                        </Rows>
                    </telerik:RadPageLayout>
                </telerik:RadAjaxPanel>
            </Content>
        </telerik:LayoutRow>
        <telerik:LayoutRow runat="server" ID="rowArticoli" >
            <Columns>
                <telerik:LayoutColumn CssClass="SpaziaturaSuperioreGruppo">
                    <telerik:RadGrid runat="server"
                        ID="rgGrigliaArticoli"
                        AllowPaging="false"
                        AllowSorting="false"
                        AllowMultiRowSelection="false"
                        AllowFilteringByColumn="false"
                        GridLines="None"
                        AutoGenerateColumns="false"
                        OnNeedDataSource="rgGrigliaArticoli_NeedDataSource"
                        OnDeleteCommand="rgGrigliaArticoli_DeleteCommand"
                        Width="100%">
                        <MasterTableView DataKeyNames="ID" TableLayout="Fixed"
                            Caption="ELENCO SPESE ACCESSORIE"
                            Width="100%"
                            AllowFilteringByColumn="false"
                            AllowSorting="false"
                            AllowMultiColumnSorting="false"
                            GridLines="Both"
                            NoMasterRecordsText="&nbsp;Questo gruppo non contiene spese accessorie!"
                            CommandItemSettings-ShowRefreshButton="true"
                            CommandItemSettings-ShowAddNewRecordButton="true"
                            CommandItemDisplay="Top"
                            CommandItemSettings-AddNewRecordText="Collega Spesa Accessoria"
                            CommandItemSettings-RefreshText="Aggiorna">

                            <Columns>

                                <telerik:GridBoundColumn SortExpression="CodiceGruppo" HeaderText="Gruppo" HeaderButtonType="TextButton"
                                    DataField="DescrizioneGruppo">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="CodiceCategoria" HeaderText="Categoria" HeaderButtonType="TextButton"
                                    DataField="DescrizioneCategoria">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="CodiceCategoriaStatistica" HeaderText="Categoria Statistica" HeaderButtonType="TextButton"
                                    DataField="DescrizioneCategoriaStatistica">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="Descrizione" HeaderText="Descrizione" HeaderButtonType="TextButton"
                                    DataField="DescrizioneArticolo">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn SortExpression="UnitaDiMisura" HeaderText="UM" HeaderButtonType="TextButton"
                                    DataField="UnitaDiMisura" HeaderStyle-Width="50px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="Quantita" HeaderText="Qta" HeaderButtonType="TextButton"
                                    DataField="Quantita" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="TotaleCosto" HeaderText="Tot. Costo" HeaderButtonType="TextButton"
                                    DataField="TotaleCosto" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="TotaleVendita" HeaderText="Tot. Vendita" HeaderButtonType="TextButton"
                                    DataField="TotaleVendita" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="TotaleRicarico" HeaderText="Tot. Ricarico" HeaderButtonType="TextButton"
                                    DataField="RicaricoValore" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="RicaricoPercentuale" HeaderText="% Ricarico" HeaderButtonType="TextButton"
                                    DataField="RicaricoPercentuale" DataFormatString="{0:F2} %" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                                </telerik:GridBoundColumn>

                                <telerik:GridButtonColumn HeaderStyle-Width="40" UniqueName="ColonnaElimina"
                                    Resizable="false"
                                    HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-Wrap="true"
                                    ItemStyle-HorizontalAlign="Center"
                                    ConfirmText="L'operazione è irreversibile, rimuovere la spesa accessoria selezionata?"
                                    ConfirmTextFields="ANAGRAFICAARTICOLI.Descrizione, CodiceAnagraficaArticolo"
                                    ConfirmTextFormatString="Eliminare la spesa accessoria '{0} ({1})'?"
                                    ConfirmDialogType="RadWindow"
                                    ConfirmTitle="Elimina"
                                    ButtonType="ImageButton"
                                    ImageUrl="/UI/Images/Toolbar/16x16/delete.png"
                                    Text="Elimina..."
                                    CommandName="Delete" />
                            </Columns>

                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="True" />
                            <Resizing AllowColumnResize="true" EnableRealTimeResize="true" AllowResizeToFit="false" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:LayoutColumn>
            </Columns>
        </telerik:LayoutRow>
    </Rows>
</telerik:RadPageLayout>

<script type="text/javascript">
    function <%=NomeEvento_GrigliaArticoli_OnCommand_ClientSide %>(sender, args) {
        if (args.get_commandName() == "<%=RadGrid.InitInsertCommandName%>") {
            args.set_cancel(true);

            var rapSelezioneArticoli = $find("<%=rapSelezioneArticoli.ClientID%>");
            if (rapSelezioneArticoli != null) {
                rapSelezioneArticoli.ajaxRequest("<%=NomeEventoAjaxVisualizzazionePannelloSelezioneArticoli%>");
            }
        }
    }


    function <%=NomeEvento_GruppoCategoriaCategoriaStatisticaCodiceArticolo_OnClientDropDownOpening_ClientSide%>(sender, args) {
        clearComboRequestItems(sender);
    }

    function <%=NomeEvento_GruppoCategoriaCategoriaStatisticaCodiceArticolo_OnClientItemsRequesting_ClientSide%>(sender, args) {
        try {
            var context = args.get_context();

            context["CodiceGruppo"] = getSelectedValueFromCombo('<%=rcbGruppo.ClientID%>');
            context["DescrizioneGruppo"] = getTextFromCombo('<%=rcbGruppo.ClientID%>');

            context["CodiceCategoria"] = getSelectedValueFromCombo('<%=rcbCategoria.ClientID%>');
            context["DescrizioneCategoria"] = getTextFromCombo('<%=rcbCategoria.ClientID%>');

            context["CodiceCategoriaStatistica"] = getSelectedValueFromCombo('<%=rcbCategoriaStatistica.ClientID%>');
            context["DescrizioneCategoriaStatistica"] = getTextFromCombo('<%=rcbCategoriaStatistica.ClientID%>');

        } catch (e) {
            alert(e.message);
        }
    }


    function <%=NomeEvento_SalvaArticoli_OnClientClicking_ClientSide%>(sender, args) {
        var valoreCodiceArticoloSelezionato = getSelectedValueFromCombo("<%=rcbCodiceArticolo.ClientID%>");

        if (valoreCodiceArticoloSelezionato == null || valoreCodiceArticoloSelezionato.trim() == "") {
            args.set_cancel(true);

            showMessage("Selezione Articolo", "Per proseguire è necessario selezionare l'articolo da aggiungere");
        }
    }

    function <%=NomeEvento_Quantità_OnValueChanged_ClientSide%>(sender, args) {

        if (modificaManualeInCorso == true) return;

        modificaManualeInCorso = true;

        var rntbQuantità = $find('<%= rntbQuantità.ClientID %>');
        var rntbCosto = $find('<%= rntbCosto.ClientID %>');
        var rntbVendita = $find('<%= rntbVendita.ClientID %>');
        var rntbRicaricoValore = $find('<%= rntbRicaricoValore.ClientID %>');
        var rntbRicaricoPercentuale = $find('<%= rntbRicaricoPercentuale.ClientID %>');
        var rntbTotaleCosto = $find('<%= rntbTotaleCosto.ClientID %>');
        var rntbTotaleVendita = $find('<%= rntbTotaleVendita.ClientID %>');

        if (rntbQuantità != null && rntbCosto != null && rntbVendita != null && rntbRicaricoValore != null && rntbRicaricoPercentuale != null && rntbTotaleCosto != null && rntbTotaleVendita != null) {

            var quantitaValue = rntbQuantità.get_value();
            var costoValue = rntbCosto.get_value();
            var venditaValue = rntbVendita.get_value();

            if (quantitaValue != null && quantitaValue > 0) {
                if (costoValue != null && costoValue > 0) {
                    var totaleCosto = quantitaValue * costoValue;
                    rntbTotaleCosto.set_value(totaleCosto);
                }

                if (venditaValue != null && venditaValue > 0) {
                    var totaleVendita = quantitaValue * venditaValue;
                    rntbTotaleVendita.set_value(totaleVendita);
                }

                if (costoValue != null && costoValue > 0 &&
                    venditaValue != null && venditaValue > 0) {

                    var totaleRicavo = venditaValue - costoValue;
                    rntbRicaricoValore.set_value(totaleRicavo);

                    var totalePercentuale = calcolaRicaricoPercentuale(costoValue, venditaValue);
                    rntbRicaricoPercentuale.set_value(totalePercentuale);
                }
            }
            else {
                rntbTotaleCosto.set_value(0);
                rntbTotaleVendita.set_value(0);
            }
        }

        modificaManualeInCorso = false;
    }

    function <%=NomeEvento_Costo_OnValueChanged_ClientSide%>(sender, args) {

        if (modificaManualeInCorso == true) return;

        modificaManualeInCorso = true;

        var rntbQuantità = $find('<%= rntbQuantità.ClientID %>');
        var rntbCosto = $find('<%= rntbCosto.ClientID %>');
        var rntbVendita = $find('<%= rntbVendita.ClientID %>');
        var rntbRicaricoValore = $find('<%= rntbRicaricoValore.ClientID %>');
        var rntbRicaricoPercentuale = $find('<%= rntbRicaricoPercentuale.ClientID %>');
        var rntbTotaleCosto = $find('<%= rntbTotaleCosto.ClientID %>');
        var rntbTotaleVendita = $find('<%= rntbTotaleVendita.ClientID %>');
        
        if (rntbQuantità != null && rntbCosto != null && rntbVendita != null && rntbRicaricoValore != null && rntbRicaricoPercentuale != null && rntbTotaleCosto != null && rntbTotaleVendita != null) {

            var quantitaValue = rntbQuantità.get_value();
            var costoValue = rntbCosto.get_value();
            var venditaValue = rntbVendita.get_value();
            var ricaricoValoreValue = rntbRicaricoValore.get_value();
            var ricaricoPercentualeValue = rntbRicaricoPercentuale.get_value();

            if (ricaricoValoreValue == null || ricaricoValoreValue == "") {
                rntbRicaricoValore.set_value(0);
                ricaricoValoreValue = 0;
            }

            if (ricaricoPercentualeValue == null || ricaricoPercentualeValue == "") {
                rntbRicaricoPercentuale.set_value(0);
                ricaricoPercentualeValue = 0;
            }

            if ((venditaValue == null  || venditaValue == "") && costoValue != null) {
                rntbVendita.set_value(costoValue);
                venditaValue = costoValue;
            }

            if (quantitaValue == null || quantitaValue == "") {
                rntbQuantità.set_value(0);
                quantitaValue = 0;
            }

            if (quantitaValue != null && quantitaValue > 0 &&
                costoValue != null && costoValue > 0) {

                var totaleCosto = quantitaValue * costoValue;
                rntbTotaleCosto.set_value(totaleCosto);

                if (venditaValue == "" && ricaricoValoreValue != "") {
                    var vendita = costoValue + ricaricoValoreValue;
                    rntbVendita.set_value(vendita);

                    var totaleVendita = vendita * quantitaValue;
                    rntbTotaleVendita.set_value(totaleVendita);

                    var percentuale = calcolaRicaricoPercentuale(costoValue, vendita);
                    rntbRicaricoPercentuale.set_value(percentuale);
                }
                else if (venditaValue == "" && ricaricoPercentualeValue != "") {

                    var ricaricoValoreValue = calcolaRicaricoValoreDaValoreIniziale(costoValue, ricaricoPercentualeValue);
                    var vendita = costoValue + ricaricoValoreValue;
                    rntbVendita.set_value(vendita);

                    var totaleVendita = vendita * quantitaValue;
                    rntbTotaleVendita.set_value(totaleVendita);

                    rntbRicaricoValore.set_value(ricaricoValoreValue);
                }
                else if (venditaValue > 0 && costoValue > 0) {
                    var totaleRicavo = venditaValue - costoValue;
                    rntbRicaricoValore.set_value(totaleRicavo);

                    var totalePercentuale = calcolaRicaricoPercentuale(costoValue, venditaValue);
                    rntbRicaricoPercentuale.set_value(totalePercentuale);
                }
            }
            else {
                rntbTotaleCosto.set_value(0);
                rntbTotaleVendita.set_value(0);
            }
        }

        modificaManualeInCorso = false;
    }

    function <%=NomeEvento_Vendita_OnValueChanged_ClientSide%>(sender, args) {

        if (modificaManualeInCorso == true) return;

        modificaManualeInCorso = true;

        var rntbQuantità = $find('<%= rntbQuantità.ClientID %>');
        var rntbCosto = $find('<%= rntbCosto.ClientID %>');
        var rntbVendita = $find('<%= rntbVendita.ClientID %>');
        var rntbRicaricoValore = $find('<%= rntbRicaricoValore.ClientID %>');
        var rntbRicaricoPercentuale = $find('<%= rntbRicaricoPercentuale.ClientID %>');
        var rntbTotaleCosto = $find('<%= rntbTotaleCosto.ClientID %>');
        var rntbTotaleVendita = $find('<%= rntbTotaleVendita.ClientID %>');

        if (rntbQuantità != null && rntbCosto != null && rntbVendita != null && rntbRicaricoValore != null && rntbRicaricoPercentuale != null && rntbTotaleCosto != null && rntbTotaleVendita != null) {

            var quantitaValue = rntbQuantità.get_value();
            var costoValue = rntbCosto.get_value();
            var venditaValue = rntbVendita.get_value();
            var ricaricoValoreValue = rntbRicaricoValore.get_value();
            var ricaricoPercentualeValue = rntbRicaricoPercentuale.get_value();

            if (quantitaValue != null && quantitaValue > 0 &&
                venditaValue != null && venditaValue > 0) {

                var totaleVendita = quantitaValue * venditaValue;
                rntbTotaleVendita.set_value(totaleVendita);

                if (costoValue == "" && ricaricoValoreValue != "") {
                    var costo = venditaValue - ricaricoValoreValue;
                    rntbCosto.set_value(costo);

                    var totaleCosto = costo * quantitaValue;
                    rntbTotaleCosto.set_value(totaleCosto);

                    var percentuale = calcolaRicaricoPercentuale(costo, venditaValue);
                    rntbRicaricoPercentuale.set_value(percentuale);
                }
                else if (costoValue == "" && ricaricoPercentualeValue != "") {

                    var ricaricoValoreValue = calcolaRicaricoValoreDaValoreFinale(venditaValue, ricaricoPercentualeValue);
                    var costo = venditaValue - ricaricoValoreValue;
                    rntbCosto.set_value(costo);

                    var totaleCosto = costo * quantitaValue;
                    rntbTotaleCosto.set_value(totaleCosto);

                    rntbRicaricoValore.set_value(ricaricoValoreValue);
                }
                else if (venditaValue > 0 && costoValue > 0) {
                    var totaleRicavo = venditaValue - costoValue;
                    rntbRicaricoValore.set_value(totaleRicavo);

                    var totalePercentuale = calcolaRicaricoPercentuale(costoValue, venditaValue);
                    rntbRicaricoPercentuale.set_value(totalePercentuale);
                }
            }
        }

        modificaManualeInCorso = false;
    }

    function <%=NomeEvento_RicaricoValore_OnValueChanged_ClientSide%>(sender, args) {
        if (modificaManualeInCorso == true) return;

        modificaManualeInCorso = true;

        var rntbQuantità = $find('<%= rntbQuantità.ClientID %>');
        var rntbCosto = $find('<%= rntbCosto.ClientID %>');
        var rntbVendita = $find('<%= rntbVendita.ClientID %>');
        var rntbRicaricoValore = $find('<%= rntbRicaricoValore.ClientID %>');
        var rntbRicaricoPercentuale = $find('<%= rntbRicaricoPercentuale.ClientID %>');
        var rntbTotaleCosto = $find('<%= rntbTotaleCosto.ClientID %>');
        var rntbTotaleVendita = $find('<%= rntbTotaleVendita.ClientID %>');

        if (rntbQuantità != null && rntbCosto != null && rntbVendita != null && rntbRicaricoValore != null && rntbRicaricoPercentuale != null && rntbTotaleCosto != null && rntbTotaleVendita != null) {
            var quantitaValue = rntbQuantità.get_value();
            var costoValue = rntbCosto.get_value();
            var venditaValue = rntbVendita.get_value();
            var ricaricoValoreValue = rntbRicaricoValore.get_value();

            if (quantitaValue != "") {
                if (costoValue != "") {

                    var vendita = costoValue + ricaricoValoreValue;
                    rntbVendita.set_value(vendita);

                    var totaleVendita = vendita * quantitaValue;
                    rntbTotaleVendita.set_value(totaleVendita);

                    var percentuale = calcolaRicaricoPercentuale(costoValue, vendita);
                    rntbRicaricoPercentuale.set_value(percentuale);
                }
                else if (venditaValue != "") {

                    var totaleVendita = quantitaValue * venditaValue;
                    var costo = totaleVendita - ricaricoValoreValue;
                    rntbCosto.set_value(costo);

                    var totaleCosto = costo * quantitaValue;
                    rntbTotaleCosto.set_value(totaleCosto);

                    var percentuale = calcolaRicaricoPercentuale(costo, venditaValue);
                    rntbRicaricoPercentuale.set_value(percentuale);
                }
            }
        }

        modificaManualeInCorso = false;
    }

    function <%=NomeEvento_RicaricoPercentuale_OnValueChanged_ClientSide%>(sender, args) {
        if (modificaManualeInCorso == true) return;

        modificaManualeInCorso = true;

        var rntbQuantità = $find('<%= rntbQuantità.ClientID %>');
        var rntbCosto = $find('<%= rntbCosto.ClientID %>');
        var rntbVendita = $find('<%= rntbVendita.ClientID %>');
        var rntbRicaricoValore = $find('<%= rntbRicaricoValore.ClientID %>');
        var rntbRicaricoPercentuale = $find('<%= rntbRicaricoPercentuale.ClientID %>');
        var rntbTotaleCosto = $find('<%= rntbTotaleCosto.ClientID %>');
        var rntbTotaleVendita = $find('<%= rntbTotaleVendita.ClientID %>');

        if (rntbQuantità != null && rntbCosto != null && rntbVendita != null && rntbRicaricoValore != null && rntbRicaricoPercentuale != null && rntbTotaleCosto != null && rntbTotaleVendita != null) {
            var quantitaValue = rntbQuantità.get_value();
            var costoValue = rntbCosto.get_value();
            var venditaValue = rntbVendita.get_value();
            var ricaricoValoreValue = rntbRicaricoValore.get_value();
            var ricaricoPercentualeValue = rntbRicaricoPercentuale.get_value();

            if (quantitaValue != "" && ricaricoPercentualeValue != "") {
                if (costoValue != "") {
                    
                    var ricaricoValoreValue = calcolaRicaricoValoreDaValoreIniziale(costoValue, ricaricoPercentualeValue);
                    var vendita = costoValue + ricaricoValoreValue;
                    rntbVendita.set_value(vendita);

                    var totaleVendita = vendita * quantitaValue;
                    rntbTotaleVendita.set_value(totaleVendita);

                    rntbRicaricoValore.set_value(ricaricoValoreValue);
                }
                else if (venditaValue != "") {

                    var ricaricoValoreValue = calcolaRicaricoValoreDaValoreFinale(venditaValue, ricaricoPercentualeValue);
                    var costo = venditaValue - ricaricoValoreValue;
                    rntbCosto.set_value(costo);

                    var totaleCosto = costo * quantitaValue;
                    rntbTotaleCosto.set_value(totaleCosto);

                    rntbRicaricoValore.set_value(ricaricoValoreValue);
                }
            }
        }

        modificaManualeInCorso = false;
    }

    function <%=NomeEvento_TotaleCosto_OnValueChanged_ClientSide%>(sender, args) {
        var rntbQuantità = $find('<%= rntbQuantità.ClientID %>');
        var rntbCosto = $find('<%= rntbCosto.ClientID %>');
        var rntbVendita = $find('<%= rntbVendita.ClientID %>');
        var rntbRicaricoValore = $find('<%= rntbRicaricoValore.ClientID %>');
        var rntbRicaricoPercentuale = $find('<%= rntbRicaricoPercentuale.ClientID %>');
        var rntbTotaleCosto = $find('<%= rntbTotaleCosto.ClientID %>');
        var rntbTotaleVendita = $find('<%= rntbTotaleVendita.ClientID %>');

        if (rntbQuantità != null && rntbCosto != null && rntbVendita != null && rntbRicaricoValore != null && rntbRicaricoPercentuale != null && rntbTotaleCosto != null && rntbTotaleVendita != null) {
            var quantitaValue = rntbQuantità.get_value();
            var totaleCosto = rntbTotaleCosto.get_value();

            if (quantitaValue != "" && totaleCosto != "") {

                var costo = totaleCosto / quantitaValue;
                rntbCosto.set_value(costo);
            }
        }
    }

    function <%=NomeEvento_TotaleVendita_OnValueChanged_ClientSide%>(sender, args) {
        var rntbQuantità = $find('<%= rntbQuantità.ClientID %>');
        var rntbCosto = $find('<%= rntbCosto.ClientID %>');
        var rntbVendita = $find('<%= rntbVendita.ClientID %>');
        var rntbRicaricoValore = $find('<%= rntbRicaricoValore.ClientID %>');
        var rntbRicaricoPercentuale = $find('<%= rntbRicaricoPercentuale.ClientID %>');
        var rntbTotaleCosto = $find('<%= rntbTotaleCosto.ClientID %>');
        var rntbTotaleVendita = $find('<%= rntbTotaleVendita.ClientID %>');

        if (rntbQuantità != null && rntbCosto != null && rntbVendita != null && rntbRicaricoValore != null && rntbRicaricoPercentuale != null && rntbTotaleCosto != null && rntbTotaleVendita != null) {
            var quantitaValue = rntbQuantità.get_value();
            var totaleVendita = rntbTotaleVendita.get_value();
            var costoValue = rntbCosto.get_value();

            if (quantitaValue != "" && totaleVendita != "") {
                var vendita = totaleVendita / quantitaValue;
                rntbVendita.set_value(vendita);

                if (costoValue == "") {
                    rntbCosto.set_value(vendita);
                }
            }
        }
    }

</script>

<telerik:RadAjaxManagerProxy runat="server" >
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="rbSalvaArticoli">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl  ControlID="rgGrigliaArticoli"/>
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>
