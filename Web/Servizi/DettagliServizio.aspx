<%@ Page Title="Dettagli Servizio" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="DettagliServizio.aspx.cs" Inherits="SeCoGEST.Web.Servizi.DettagliServizio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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

    <style type="text/css">
        .RadPageLayoutOnRadComboBox {
            margin-top: -23px !important;
            margin-left: 23px !important;
        }

            .RadPageLayoutOnRadComboBox .t-col {
                padding-left: 0px !important;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    <asp:Label ID="lblTitolo" runat="server" Text="Dettagli Servizio" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">

    <telerik:RadToolBar ID="RadToolBar1"
        runat="server"
        OnButtonClick="RadToolBar1_ButtonClick"
        OnClientButtonClicking="ToolBarButtonClicking">
        <Items>
            <telerik:RadToolBarButton runat="server" Value="TornaElenco" CommandName="TornaElenco" CausesValidation="False" NavigateUrl="ElencoServizi.aspx" PostBack="False" ImageUrl="~/UI/Images/Toolbar/back.png" Text="Vai all'Elenco" ToolTip="Vai alla pagina di elenco Servizi" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/refresh.png" PostBack="False" Value="Aggiorna" CommandName="Aggiorna" Text="Aggiorna" ToolTip="Aggiorna i dati visualizzati" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreNuovo" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" NavigateUrl="DettagliServizio.aspx" ImageUrl="~/UI/Images/Toolbar/add.png" PostBack="False" CommandName="Nuovo" Value="Nuovo" Text="Nuovo Servizio" ToolTip="Permette la creazione di un nuovo Servizio" Target="_blank" />
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

        <div style="margin-bottom: 10px; border: 0px solid;">
            <uc1:PageMessage runat="server" ID="PageMessage" FrameStyle="Note" Visible="false" />
        </div>

        <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
            <Rows>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="1" SpanMd="1" SpanSm="3" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblCodiceServizio" runat="server" Text="Codice" Font-Bold="true" AssociatedControlID="rtbCodiceServizio" />
                            <asp:RequiredFieldValidator ID="rfvServizio" runat="server" Display="Dynamic"
                                ControlToValidate="rtbCodiceServizio"
                                ErrorMessage="Codice è obbligatorio."
                                ForeColor="Red">
                            <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                            </asp:RequiredFieldValidator>
                            <br />
                            <telerik:RadTextBox runat="server" ID="rtbCodiceServizio" Width="100%"></telerik:RadTextBox>
                        </telerik:LayoutColumn>
                        <telerik:LayoutColumn Span="11" SpanXl="11" SpanLg="11" SpanMd="11" SpanSm="9" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblNomeServizio" runat="server" Text="Nome" Font-Bold="true" AssociatedControlID="rtbNomeServizio" />
                            <asp:RequiredFieldValidator ID="rfvNomeServizio" runat="server" Display="Dynamic"
                                ControlToValidate="rtbNomeServizio"
                                ErrorMessage="Nome è obbligatorio."
                                ForeColor="Red">
                            <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                            </asp:RequiredFieldValidator>
                            <br />
                            <telerik:RadTextBox runat="server" ID="rtbNomeServizio" Width="100%"></telerik:RadTextBox>
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblDescrizioneServizio" runat="server" Font-Bold="true" Text="Descrizione" AssociatedControlID="reDescrizioneServizio" /><br />
                            <telerik:RadEditor runat="server" ID="reDescrizioneServizio" ToolsFile="~/App_Data/Editor/Settings/Tools.xml" ToolbarMode="Default" Width="100%" Height="430px" BorderWidth="1" BorderStyle="Solid" />
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>
                <telerik:LayoutRow ID="rigaSelezioneArticoli" runat="server" RowType="Region">
                    <Content>
                        <telerik:RadAjaxPanel ID="rapSelezioneArticoli" runat="server" OnAjaxRequest="rapSelezioneArticoli_AjaxRequest">
                            <telerik:RadPageLayout ID="rplSelezioneArticoli" runat="server" Width="100%" HtmlTag="None" GridType="Fluid" Visible="false">
                                <Rows>
                                    <telerik:LayoutRow RowType="Region">
                                        <Columns>
                                            <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreGruppo">
                                                <fieldset style="background-color: #fff;">
                                                    <legend><strong>Selezione Articoli</strong></legend>
                                                    <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                                        <Rows>
                                                            <telerik:LayoutRow RowType="Region">
                                                                <Columns>
                                                                    <telerik:LayoutColumn Span="3" SpanXl="3" SpanLg="3" SpanMd="3" SpanSm="12" SpanXs="12">
                                                                        <asp:Label ID="lblGruppo" runat="server" Text="Gruppo" AssociatedControlID="rcbGruppo" /><br />
                                                                        <telerik:RadComboBox runat="server" ID="rcbGruppo" DataValueField="CODICE" DataTextField="DESCRIZIONE" Width="100%" 
                                                                            CausesValidation="false"
                                                                            ValidationGroup="SelezioneArticoli"
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
                                                                            OnClientDropDownOpening="rcbGruppoCategoriaCategoriaStatistica_OnClientDropDownOpening"
                                                                            OnClientItemsRequesting="rcbGruppoCategoriaCategoriaStatistica__OnClientItemsRequesting"
                                                                            OnItemsRequested="rcbGruppo_ItemsRequested">
                                                                        </telerik:RadComboBox>
                                                                    </telerik:LayoutColumn>
                                                                    <telerik:LayoutColumn Span="3" SpanXl="3" SpanLg="3" SpanMd="3" SpanSm="12" SpanXs="12">
                                                                        <asp:Label ID="lblCategoria" runat="server" Text="Categoria" AssociatedControlID="rcbCategoria" /><br />
                                                                        <telerik:RadComboBox runat="server" ID="rcbCategoria" DataValueField="CODICE" DataTextField="DESCRIZIONE" Width="100%" 
                                                                            CausesValidation="false"
                                                                            ValidationGroup="SelezioneArticoli"
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
                                                                            OnClientDropDownOpening="rcbGruppoCategoriaCategoriaStatistica_OnClientDropDownOpening"
                                                                            OnClientItemsRequesting="rcbGruppoCategoriaCategoriaStatistica__OnClientItemsRequesting"
                                                                            OnItemsRequested="rcbCategoria_ItemsRequested">
                                                                        </telerik:RadComboBox>
                                                                    </telerik:LayoutColumn>
                                                                    <telerik:LayoutColumn Span="3" SpanXl="3" SpanLg="3" SpanMd="3" SpanSm="12" SpanXs="12">
                                                                        <asp:Label ID="lblCategoriaStatistica" runat="server" Text="Categoria statistica" AssociatedControlID="rcbCategoriaStatistica" /><br />
                                                                        <telerik:RadComboBox runat="server" ID="rcbCategoriaStatistica" DataValueField="CODICE" DataTextField="DESCRIZIONE" Width="100%" 
                                                                            CausesValidation="false"
                                                                            ValidationGroup="SelezioneArticoli"
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
                                                                            OnClientDropDownOpening="rcbGruppoCategoriaCategoriaStatistica_OnClientDropDownOpening"
                                                                            OnClientItemsRequesting="rcbGruppoCategoriaCategoriaStatistica__OnClientItemsRequesting"
                                                                            OnItemsRequested="rcbCategoriaStatistica_ItemsRequested">
                                                                        </telerik:RadComboBox>
                                                                    </telerik:LayoutColumn>
                                                                    <telerik:LayoutColumn Span="3" SpanXl="3" SpanLg="3" SpanMd="3" SpanSm="12" SpanXs="12">
                                                                        <asp:Label ID="lblCodiceArticolo" runat="server" Text="Codice Articolo" AssociatedControlID="rcbCodiceArticolo" /><br />
                                                                        <telerik:RadComboBox ID="rcbCodiceArticolo" runat="server"
                                                                            CausesValidation="false"
                                                                            ValidationGroup="SelezioneArticoli"
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
                                                                            OnClientDropDownOpening="rcbCodiceArticolo_OnClientDropDownOpening"
                                                                            OnClientItemsRequesting="rcbCodiceArticolo_OnClientItemsRequesting"
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
                                                            <telerik:LayoutRow RowType="Region">
                                                                <Columns>
                                                                    <telerik:LayoutColumn OffsetXl="10" OffsetMd="10" OffsetLg="10" OffsetSm="12" OffsetXs="12" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreGruppo" Style="text-align: right;">
                                                                        <telerik:RadButton ID="rbSalvaArticoli" runat="server" Text="Salva" ButtonType="LinkButton" Icon-PrimaryIconCssClass="rbSave" ValidationGroup="SelezioneArticoli" OnClientClicking="rbSalvaArticoli_OnClientClicking" OnClick="rbSalvaArticoli_Click"></telerik:RadButton>
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
                <telerik:LayoutRow runat="server" ID="rowArticoli" RowType="Region" Visible="false">
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
                                <MasterTableView DataKeyNames="CodiceAnagraficaArticolo" TableLayout="Fixed"
                                    Caption="ELENCO ARTICOLI"
                                    Width="100%"
                                    AllowFilteringByColumn="false"
                                    AllowSorting="false"
                                    AllowMultiColumnSorting="false"
                                    GridLines="Both"
                                    NoMasterRecordsText="&nbsp;Questo gruppo non contiene ancora articoli!"
                                    CommandItemSettings-ShowRefreshButton="true"
                                    CommandItemSettings-ShowAddNewRecordButton="true"
                                    CommandItemDisplay="Top"
                                    CommandItemSettings-AddNewRecordText="Collega Articolo"
                                    CommandItemSettings-RefreshText="Aggiorna">

                                    <Columns>

                                        <telerik:GridBoundColumn SortExpression="CodiceGruppo" HeaderText="Gruppo" HeaderButtonType="TextButton"
                                            DataField="ANAGRAFICAARTICOLI.TABGRUPPI.DESCRIZIONE">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn SortExpression="CodiceCategoria" HeaderText="Categoria" HeaderButtonType="TextButton"
                                            DataField="ANAGRAFICAARTICOLI.TABCATEGORIE.DESCRIZIONE">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn SortExpression="CodiceCategoriaStatistica" HeaderText="Categoria Statistica" HeaderButtonType="TextButton"
                                            DataField="ANAGRAFICAARTICOLI.TABCATEGORIESTAT.DESCRIZIONE">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn SortExpression="Descrizione" HeaderText="Descrizione" HeaderButtonType="TextButton"
                                            DataField="ANAGRAFICAARTICOLI.Descrizione">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridButtonColumn HeaderStyle-Width="40" UniqueName="ColonnaElimina"
                                            Resizable="false"
                                            HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-Wrap="true"
                                            ItemStyle-HorizontalAlign="Center"
                                            ConfirmText="Rimuovere l'articolo selezionato?"
                                            ConfirmTextFields="ANAGRAFICAARTICOLI.Descrizione, CodiceAnagraficaArticolo"
                                            ConfirmTextFormatString="Eliminare l'articolo '{0} ({1})'?"
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
                                    <ClientEvents OnCommand="rgGrigliaArticoli_OnCommand" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>
            </Rows>
        </telerik:RadPageLayout>
    </div>

    <telerik:RadAjaxManagerProxy runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rbSalvaArticoli">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgGrigliaArticoli" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rbAnnullaInserimentoArticoli">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgGrigliaArticoli" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>

    <script type="text/javascript">
        function rgGrigliaArticoli_OnCommand(sender, args) {
            if (args.get_commandName() == "<%=RadGrid.InitInsertCommandName%>") {
                args.set_cancel(true);

                var rapSelezioneArticoli = $find("<%=rapSelezioneArticoli.ClientID%>");
                if (rapSelezioneArticoli != null) {
                    rapSelezioneArticoli.ajaxRequest("<%=NomeEventoAjaxVisualizzazionePannelloSelezioneArticoli%>");
                }
            }
        }

        function clearComboRequestItems(combo) {
            try {

                if (combo == null) return;

                combo.requestItems("", false);
                combo.clearItems();
                //combo.clearSelection();
            } catch (e) {
                alert(e.message);
            }
        }

        function rcbGruppoCategoriaCategoriaStatistica_OnClientDropDownOpening(sender, args) {
            clearComboRequestItems(sender);
        }

        function rcbGruppoCategoriaCategoriaStatistica__OnClientItemsRequesting(sender, args) {
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

        function rcbGruppoCategoriaCategoriaStatistica__OnClientItemsRequesting(sender, args) {
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

        function rcbCodiceArticolo_OnClientDropDownOpening(sender, args) {
            clearComboRequestItems(sender);
        }

         function rcbCodiceArticolo_OnClientItemsRequesting(sender, args) {
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

        function getSelectedValueFromCombo(comboClientId) {
            var combo = $find(comboClientId);

            var valore = "";
            if (combo != null && combo.get_selectedItem) {
                var selectedItem = combo.get_selectedItem();
                if (selectedItem != null && selectedItem.get_value) {
                    valore = selectedItem.get_value();
                }

                if (valore == null || valore == "") {
                    valore = combo.get_value();
                }
            }

            if (valore == null) valore = "";

            if (valore != null && valore != "") {
                if (valore == combo.get_emptyMessage()) valore = "";
            }

            return valore;
        }

        function getTextFromCombo(comboClientId) {
            var combo = $find(comboClientId);

            var valore = "";
            if (combo != null && combo.get_selectedItem) {
                var selectedItem = combo.get_selectedItem();
                if (selectedItem != null && selectedItem.get_text) {
                    valore = selectedItem.get_text();
                }

                if (valore == null || valore == "") {
                    valore = combo.get_text();
                }
            }

            if (valore == null) valore = "";

            if (valore != null && valore != "") {
                if (valore == combo.get_emptyMessage()) valore = "";
            }

            return valore;
        }

        function rbSalvaArticoli_OnClientClicking(sender, args) {
            var valoreCodiceArticoloSelezionato = getSelectedValueFromCombo("<%=rcbCodiceArticolo.ClientID%>");
            
            if (valoreCodiceArticoloSelezionato == null || valoreCodiceArticoloSelezionato.trim() == "") {
                args.set_cancel(true);

                showMessage("Selezione Articolo", "Per proseguire è necessario selezionare l'articolo da aggiungere");
            }
        }

    </script>

</asp:Content>
