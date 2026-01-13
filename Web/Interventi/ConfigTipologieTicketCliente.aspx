<%@ Page Title="Configurazione Tipologie Ticket Cliente" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="ConfigTipologieTicketCliente.aspx.cs" Inherits="SeCoGEST.Web.Interventi.ConfigTipologieTicketCliente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    <asp:Label ID="lblTitolo" runat="server" Text="Configurazione Tipologie Ticket Cliente" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">

    <telerik:RadScriptBlock runat="server">
        <script type="text/javascript">

            // Forza la lettura lato server, ad ogni apertura della combo rcbArticoli, dei dati relativi agli Articoli in base ai parametri indicati nella pagina
            function rcbArticoli_OnClientDropDownOpening(sender, arg) {
                sender.clearItems();
                sender.requestItems("", true);
            }

            // Metodo di gestione dell'evento OnClientItemsRequesting relativo al combo che contiene gli Articoli da selezionare
            // viene utilizzato per passare parametri alla funzione lato server che seleziona gli Articoli da proporre in base alla Provenienza selezionata
            function rcbArticoli_OnClientItemsRequesting(sender, eventArgs) {
                if (eventArgs == null || !eventArgs.get_context) return;

                var comboProvenienzaArticolo = $find('<%=rcbProvenienzaArticolo.ClientID%>');
                var provenienza = comboProvenienzaArticolo.get_value();
                var context = eventArgs.get_context();

                if (context != null) {
                    context["ProvenienzaArticolo"] = provenienza;
                }
            }

            function rcbArticoli_OnClientSelectedIndexChanged(sender, eventArgs) {
                var descrizione = eventArgs.get_item().get_attributes().getAttribute("DescrizioneArticolo");
                var rtbDescrizionePersonalizzataArticolo = $find('<%=rtbDescrizionePersonalizzataArticolo.ClientID%>');
                rtbDescrizionePersonalizzataArticolo.set_value(descrizione);
            }

            function validateComborcbArticoli(source, args) {
                args.IsValid = false;
                var combo = $find("<%=rcbArticoli.ClientID %>");
                if (combo) {
                    if (combo.get_selectedItem()) {
                        args.IsValid = true;
                        return;
                    }
                }
                args.IsValid = false;
            }

            function rcbCliente_OnClientSelectedIndexChanged(sender, eventArgs) {
                var State = eventArgs.get_item().get_attributes().getAttribute("State");
                var Note = eventArgs.get_item().get_attributes().getAttribute("Note");
                ScriviInfoCliente(State, Note);
            }

            function ScriviInfoCliente(State, Note) {
                var lblStatoCliente = document.getElementById('<%=lblStatoCliente.ClientID%>');
                var lblNoteCliente = document.getElementById('<%=lblNoteCliente.ClientID%>');

                lblNoteCliente.innerHTML = Note;

                if (State != "X") {
                    if (State == "0") {
                        lblStatoCliente.innerHTML = "";
                        lblStatoCliente.style.color = "black";
                        lblStatoCliente.style.backgroundColor = "transparent";
                    }
                    else if (State == "1") {
                        lblStatoCliente.innerHTML = "&nbsp;CLIENTE SEGNALATO&nbsp;";
                        lblStatoCliente.style.color = "yellow";
                        lblStatoCliente.style.backgroundColor = "#3F48CC";
                    }
                    else if (State == "2") {
                        lblStatoCliente.innerHTML = "&nbsp;CLIENTE BLOCCATO&nbsp;";
                        lblStatoCliente.style.color = "Orange";
                        lblStatoCliente.style.backgroundColor = "#880015";
                    }
                }
            }


            function rcbProvenienzaArticolo_OnClientSelectedIndexChanged(sender, eventArgs) {
                var rcbArticoli = $find('<%=rcbArticoli.ClientID%>');
                rcbArticoli.clearSelection();
            //    rcbArticoli.clearItems();
            }

        </script>
    </telerik:RadScriptBlock>

    
    <!-- Window manager -->
	<telerik:RadWindowManager RenderMode="Lightweight" ID="windowManager1" runat="server" EnableShadow="true" Style="border: solid 5px green; background-color:red; box-sizing: padding-box; z-index: 100001" Localization-Cancel="Annulla" ></telerik:RadWindowManager>
	<!-- /Window manager -->

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
                        <telerik:LayoutColumn Span="6">

                            <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                <Rows>

                                    <telerik:LayoutRow RowType="Region">
                                        <Columns>
                                            <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                                                <asp:Label ID="Label1" runat="server" Text="Cliente" AssociatedControlID="rcbCliente" Font-Bold="true" />&nbsp;<asp:Label ID="lblStatoCliente" runat="server" AssociatedControlID="rcbCliente" Font-Bold="true" />&nbsp;<asp:Label ID="lblNoteCliente" runat="server" AssociatedControlID="rcbCliente" Font-Bold="true" BackColor="Yellow" />
                                                <asp:RequiredFieldValidator ID="rfvCliente" runat="server"
                                                    ControlToValidate="rcbCliente"
                                                    ErrorMessage="Il Cliente è obbligatorio."
                                                    ForeColor="Red">
                                                    <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                                </asp:RequiredFieldValidator><br />
                                                <telerik:RadComboBox ID="rcbCliente" runat="server" ShowWhileLoading="true"
                                                    Width="100%" DropDownAutoWidth="Disabled" DropDownWidth="1000"
                                                    MarkFirstMatch="false"
                                                    HighlightTemplatedItems="true"
                                                    EmptyMessage="Selezionare un Soggetto"
                                                    ItemsPerRequest="20"
                                                    ShowMoreResultsBox="true"
                                                    EnableLoadOnDemand="true"
                                                    EnableItemCaching="false"
                                                    AllowCustomText="true"
                                                    LoadingMessage="Caricamento in corso..."
                                                    DataValueField="CODCONTO"
                                                    DataTextField="DSCCONTO1"
                                                    OnClientSelectedIndexChanged="rcbCliente_OnClientSelectedIndexChanged"
                                                    OnItemsRequested="rcbCliente_ItemsRequested"
                                                    OnSelectedIndexChanged="rcbCliente_SelectedIndexChanged" AutoPostBack="true">
                                                    <ItemTemplate>
                                                        <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                                            <Rows>
                                                                <telerik:LayoutRow RowType="Region">
                                                                    <Columns>
                                                                        <telerik:LayoutColumn Span="1">
                                                                            <%# Eval("CODCONTO") %>
                                                                        </telerik:LayoutColumn>
                                                                        <telerik:LayoutColumn Span="11">
                                                                            <%# Eval("DSCCONTO1") %>
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

                                </Rows>
                            </telerik:RadPageLayout>

                            <telerik:RadPageLayout runat="server" ID="rplConfigurator" Visible="false" Width="100%" HtmlTag="None" GridType="Fluid">
                                <Rows>                                    
                                    <telerik:LayoutRow RowType="Region">
                                        <Columns>
                                            <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="12">
                                                <%--<asp:Label runat="server" ID="lblCliente" Text="Codice Cliente" AssociatedControlID="rtbCodiceCliente"></asp:Label><br />
                                                <telerik:RadTextBox runat="server" ID="rtbCodiceCliente"></telerik:RadTextBox>&nbsp;
                                                <telerik:RadButton runat="server" ID="btSetCliente" Text="Imposta Cliente" OnClick="btSetCliente_Click"></telerik:RadButton>
                                                &nbsp;&nbsp;--%>
                                                <telerik:RadButton runat="server" ID="rbNuovo" Text="Nuova configurazione" OnClick="rbNuovo_Click"></telerik:RadButton>
                                                &nbsp;&nbsp;
                                                <telerik:RadDatePicker ID="rdpScadenzaContratto" runat="server"
                                                    DateInput-EmptyMessage="Scadenza..."
                                                    DatePopupButton-ToolTip="Seleziona una data dal calendario"
                                                    DateInput-DisabledStyle-BackColor="#F5F5F5">
                                                    <Calendar ID="Calendar1" runat="server">
                                                        <SpecialDays>
                                                            <telerik:RadCalendarDay ItemStyle-CssClass="rcToday" Repeatable="Today" />
                                                        </SpecialDays>
                                                    </Calendar>
                                                </telerik:RadDatePicker>
                                            </telerik:LayoutColumn>
                                        </Columns>
                                    </telerik:LayoutRow>

                                    <telerik:LayoutRow RowType="Region">
                                        <Columns>
                                            <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="12">
                                                <asp:Label runat="server" ID="lblProvenienzaArticolo" Text="Provenienza articolo" AssociatedControlID="rcbProvenienzaArticolo"></asp:Label><br />
                                                <telerik:RadComboBox ID="rcbProvenienzaArticolo" runat="server"
                                                    HighlightTemplatedItems="true"
                                                    Width="100%" ZIndex="10000"
                                                    OnClientSelectedIndexChanged="rcbProvenienzaArticolo_OnClientSelectedIndexChanged">
                                                    <ItemTemplate>
                                                        <span style='<%#GetStyleByProvenienza(Container) %>'><%# DataBinder.Eval(Container, "Text") %></span>
                                                    </ItemTemplate>
                                                </telerik:RadComboBox>
                                            </telerik:LayoutColumn>
                                        </Columns>
                                    </telerik:LayoutRow>

                                    <telerik:LayoutRow RowType="Region">
                                        <Columns>
                                            <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="12">
                                                <asp:Label runat="server" ID="lblArticoli" Text="Articolo" AssociatedControlID="rcbArticoli"></asp:Label>&nbsp;&nbsp;
                                                <%--<asp:CustomValidator ID="cvArticoli" runat="server"
                                                    Text="Obbligatorio" ErrorMessage="E' obbligatorio selezionare un articolo dall'elenco proposto."
                                                    ControlToValidate="rcbArticoli"
                                                    ForeColor="Red" Font-Bold="true"
                                                    ClientValidationFunction="validateComborcbArticoli" ValidateEmptyText="True"
                                                    ValidationGroup="FinestraArticoli">  
                                                </asp:CustomValidator>--%>
                                                <br />
                                                <telerik:RadComboBox ID="rcbArticoli" runat="server" ZIndex="10000" ValidationGroup="FinestraArticoli"
                                                    DataValueField="ID"
                                                    DataTextField="CodiceArticolo"
                                                    MarkFirstMatch="false"
                                                    EmptyMessage="Selezionare un articolo dalla lista"
                                                    Width="100%"
                                                    ShowMoreResultsBox="true"
                                                    EnableLoadOnDemand="true"
                                                    EnableItemCaching="false"
                                                    AllowCustomText="false"
                                                    HighlightTemplatedItems="true"
                                                    CausesValidation="true"
                                                    OnClientDropDownOpening="rcbArticoli_OnClientDropDownOpening"
                                                    OnClientItemsRequesting="rcbArticoli_OnClientItemsRequesting"
                                                    OnClientSelectedIndexChanged="rcbArticoli_OnClientSelectedIndexChanged"
                                                    OnItemsRequested="rcbArticoli_ItemsRequested">
                                                    <HeaderTemplate>
                                                        <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                                            <Rows>
                                                                <telerik:LayoutRow RowType="Region">
                                                                    <Columns>
                                                                        <telerik:LayoutColumn Span="3">
                                                                            <asp:Label runat="server" Text="Codice Articolo" Font-Bold="true" />
                                                                        </telerik:LayoutColumn>
                                                                        <telerik:LayoutColumn Span="9">
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
                                                                        <telerik:LayoutColumn Span="3">
                                                                            <%# DataBinder.Eval(Container.DataItem, "CodiceArticolo") %>
                                                                        </telerik:LayoutColumn>
                                                                        <telerik:LayoutColumn Span="9">
                                                                            <%# DataBinder.Eval(Container.DataItem, "DescrizioneConNote") %>
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
                                            <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="12">
                                                <asp:Label runat="server" ID="lblDescrizionePersonalizzataArticolo" Text="Descrizione personalizzata articolo" AssociatedControlID="rtbDescrizionePersonalizzataArticolo"></asp:Label>&nbsp;&nbsp;
                                                <asp:RequiredFieldValidator runat="server" ID="rfvDescrizionePersonalizzataArticolo"
                                                    Text="Obbligatorio" ErrorMessage="La Descrizione personalizzata dell'articolo è obbligatoria."
                                                    ControlToValidate="rtbDescrizionePersonalizzataArticolo"
                                                    ForeColor="Red" Font-Bold="true" Display="Static"
                                                    ValidationGroup="FinestraArticoli"></asp:RequiredFieldValidator>
                                                <br />
                                                <telerik:RadTextBox runat="server" ID="rtbDescrizionePersonalizzataArticolo" Width="100%" Height="70px" TextMode="MultiLine" ValidationGroup="FinestraArticoli" ClientEvents-OnBlur="UpdateValidatorSummary"></telerik:RadTextBox>
                                            </telerik:LayoutColumn>
                                        </Columns>
                                    </telerik:LayoutRow>






                                    <telerik:LayoutRow RowType="Region">
                                        <Columns>
                                            <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="12">
                                                <telerik:RadAjaxPanel runat="server" LoadingPanelID="">
                                                    <table style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 33%;">
                                                                <asp:Label runat="server" ID="lblTipologiaIntervento" Text="Tipologia Intervento" AssociatedControlID="rddlTipologiaIntervento"></asp:Label><br />
                                                                <telerik:RadDropDownList runat="server" ID="rddlTipologiaIntervento"
                                                                    CausesValidation="false" Width="100%"
                                                                    DataValueField="Id" DataTextField="Nome">
                                                                </telerik:RadDropDownList>
                                                            </td>
                                                            <td style="width: 33%;">
                                                                <asp:Label runat="server" ID="lblReparto" Text="Reparto" AssociatedControlID="rddlReparto"></asp:Label><br />
                                                                <telerik:RadDropDownList runat="server" ID="rddlReparto"
                                                                    CausesValidation="false" Width="100%"
                                                                    DataValueField="Id" DataTextField="Reparto"
                                                                    AutoPostBack="true"
                                                                    OnSelectedIndexChanged="rddlReparto_SelectedIndexChanged">
                                                                </telerik:RadDropDownList>
                                                            </td>
                                                            <td style="width: 33%;">
                                                                <asp:Label runat="server" ID="lblCondizione" Text="Condizione" AssociatedControlID="rddlCondizione"></asp:Label><br />
                                                                <telerik:RadDropDownList runat="server" ID="rddlCondizione"
                                                                    CausesValidation="false" Width="100%"
                                                                    DataValueField="Id" DataTextField="Nome">
                                                                </telerik:RadDropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div runat="server" id="divOrariReparto" style="width: 100%; background-color: white; border: solid 1px silver;" visible="false">
                                                        <asp:Label runat="server" ID="lblUfficioOrariReparto" BorderWidth="10" BorderColor="White"></asp:Label>
                                                    </div>
                                                </telerik:RadAjaxPanel>
                                            </telerik:LayoutColumn>
                                        </Columns>
                                    </telerik:LayoutRow>

                                    <telerik:LayoutRow RowType="Region">
                                        <Columns>
                                            <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="12">
                                                <asp:Label runat="server" ID="lblCaratteristicheIntervento" Text="Caratteristiche/Proprietà Intervento" AssociatedControlID="rlbCaratteristicheIntervento"></asp:Label><br />
                                                <telerik:RadComboBox runat="server" ID="rcbModelli"
                                                    AllowCustomText="false"
                                                    EmptyMessage="Selezionare un modello..."
                                                    CausesValidation="false" Width="100%"
                                                    DataValueField="Id" DataTextField="Nome"
                                                    AutoPostBack="true"
                                                    OnSelectedIndexChanged="rcbModelli_SelectedIndexChanged">
                                                </telerik:RadComboBox>
                                                <br />

                                                <telerik:RadListBox runat="server" ID="rlbCaratteristicheIntervento"
                                                    CheckBoxes="false"
                                                    Width="100%" Height="200px"
                                                    DataValueField="IdCaratteristica" DataTextField="NomeCaratteristicaIntervento"
                                                    OnItemDataBound="rlbCaratteristicheIntervento_ItemDataBound">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkboxStatus" runat="server" TextAlign="Right" Text='<%#Eval("NomeCaratteristicaIntervento") %>' Checked='<%#IsCaratteristicaChecked(Container.DataItem) %>' />
                                                        <div runat="server" id="divParams" style="float: right;">
                                                            Ore:
                                                            <telerik:RadNumericTextBox runat="server" ID="rntbOre" Width="90px" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                                                            
                                                            Minuti:
                                                            <telerik:RadNumericTextBox runat="server" ID="rntbMinuti" Width="90px" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>

<%--                                                            <telerik:RadTimePicker RenderMode="Lightweight" ID="rtpTempo" Width="250px" runat="server" EnableAriaSupport="true" EnableKeyboardNavigation="true">
                                                                <DateInput runat="server" DateFormat="HH:mm" Label="Tempo (ore:minuti):" ToolTip="Indicare il tempo">
                                                                </DateInput>
                                                                <TimeView runat="server" TimeFormat="HH:mm">
                                                                </TimeView>
                                                            </telerik:RadTimePicker>--%>
                                                        </div>
                                                    </ItemTemplate>
                                                </telerik:RadListBox>
                                            </telerik:LayoutColumn>
                                        </Columns>
                                    </telerik:LayoutRow>

                                    <telerik:LayoutRow RowType="Region">
                                        <Columns>
                                            <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="12">
                                                Operatore o Area di riferimento:<br />
                                                <telerik:RadComboBox runat="server" ID="rcbOperatoreDiRiferimento"
                                                    AllowCustomText="false"
                                                    EmptyMessage="Operatore di riferimento"
                                                    CausesValidation="false" Width="100%"
                                                    DataValueField="Id" DataTextField="CognomeNome"
                                                    AutoPostBack="true"
                                                    OnSelectedIndexChanged="rcbOperatoreDiRiferimento_SelectedIndexChanged">
                                                </telerik:RadComboBox>
                                            </telerik:LayoutColumn>
                                        </Columns>
                                    </telerik:LayoutRow>

                                    <telerik:LayoutRow RowType="Region">
                                        <Columns>
                                            <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="12">
                                                <hr />
                                                <telerik:RadButton runat="server" ID="btSaveConfiguration" Text="Salva Configurazione" OnClick="btSaveConfiguration_Click"></telerik:RadButton>
                                                &nbsp;&nbsp;
                                                <asp:CheckBox runat="server" ID="chkVisibilePerCliente" Text="Utilizzabile dal cliente" />
                                            </telerik:LayoutColumn>
                                        </Columns>
                                    </telerik:LayoutRow>
                                       
                                </Rows>
                            </telerik:RadPageLayout>

                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="6">

                            <asp:Label ID="Label2" runat="server" Text="Configurazioni memorizzate" AssociatedControlID="rgArchiveItems" Font-Bold="true" /><br />
                            <telerik:RadGrid RenderMode="Lightweight" ID="rgArchiveItems" runat="server" 
                                GridLines="None" ClientSettings-Selecting-AllowRowSelect="false" 
                                AllowPaging="false" PageSize="10" AllowSorting="True" AutoGenerateColumns="False" ShowStatusBar="true"
                                AllowAutomaticDeletes="False" AllowAutomaticInserts="False" AllowAutomaticUpdates="False"
                                OnNeedDataSource="rgArchiveItems_NeedDataSource"
                                OnItemCreated="rgArchiveItems_ItemCreated"
		                        OnDeleteCommand="rgArchiveItems_DeleteCommand"
                                OnSelectedIndexChanged="rgArchiveItems_SelectedIndexChanged"
                                OnItemCommand="rgArchiveItems_ItemCommand"
                                SortingSettings-SortToolTip="Clicca qui per ordinare i dati in base a questa colonna"
                                PagerStyle-FirstPageToolTip="Prima Pagina"
                                PagerStyle-LastPageToolTip="Ultima Pagina"
                                PagerStyle-PrevPageToolTip="Pagina Precedente"
                                PagerStyle-NextPageToolTip="Pagina Successiva"
                                PagerStyle-PagerTextFormat="{4} Elementi da {2} a {3} su {5}, Pagina {0} di {1}">
                                <MasterTableView CommandItemDisplay="Bottom" CommandItemSettings-ShowAddNewRecordButton="false"
                                    NoMasterRecordsText="<div style='padding: 15px; background-color: #EFCF02; width: 100%'><strong>Nessuna Tipologia configurazione da visualizzare.</strong></div>"
                                    DataKeyNames="Id">
                                    <Columns>
                                        <telerik:GridButtonColumn CommandName="Select" HeaderText="Sel." ImageUrl="/UI/Images/Toolbar/24x24/back.png" ButtonType="ImageButton" UniqueName="Select" ItemStyle-VerticalAlign="Middle"></telerik:GridButtonColumn>
                                        <telerik:GridButtonColumn CommandName="Clone" HeaderText="Clo." ImageUrl="/UI/Images/Toolbar/24x24/clone.png" ButtonType="ImageButton" UniqueName="Clone" ItemStyle-VerticalAlign="Middle"></telerik:GridButtonColumn>
                                        <telerik:GridBoundColumn UniqueName="Id" HeaderText="Id" DataField="Id" Display="false"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Tipologia" HeaderText="Tipologia" DataField="TipologiaIntervento.Nome"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Descrizione" HeaderText="Descrizione" DataField="Descrizione"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn UniqueName="Condizione" HeaderText="Condizione" DataField="CondizioneIntervento.Nome" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                        <telerik:GridDateTimeColumn HeaderText="Scadenza" DataField="ScadenzaContratto" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></telerik:GridDateTimeColumn>
                                        <telerik:GridCheckBoxColumn HeaderText="Util." DataField="VisibilePerCliente" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center"></telerik:GridCheckBoxColumn>
                                        <telerik:GridButtonColumn CommandName="Delete" Text="Elimina" UniqueName="columnDelete" HeaderStyle-Width="50px"
                                            ConfirmDialogType="RadWindow" ConfirmTitle="Conferma eliminazione"
                                            ConfirmText="Eliminare la voce di configurazione selezionata?">
                                        </telerik:GridButtonColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>

                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>
            </Rows>
        </telerik:RadPageLayout>


        <br />
        <br />
        <br />
    </div>

</asp:Content>
