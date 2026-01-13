<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="Account.aspx.cs" Inherits="SeCoGEST.Web.Sicurezza.Account" %>

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
                    else if (button.get_commandName() == "Nuovo") {
                        messaggioConfermato = confirm('Si desidera creare una nuovo Account?\n\nEventuali modifiche apportate ai dati e non salvate verranno perse.');
                    }

                    if (!messaggioConfermato) {
                        args.set_cancel(true);
                    }
                }
            }
        }

        // Metodo di gestione dell'evento OnClientClicked relativo al tasto rbModificaPassword
        function <%=RAD_BUTTON_MODIFICA_PASSWORD_ONCLIENTCLICKED_JS_FUNCTION_NAME %>(sender, eventArgs) {
            var hdPasswordModificata = $get("<%=hdPasswordModificata.ClientID %>");
            if (hdPasswordModificata) {
                var divPassword = document.getElementById('divPassword');
                if (divPassword) {
                    if (hdPasswordModificata.value == '0') {
                        divPassword.style.display = 'block';
                        hdPasswordModificata.value = '1';
                    }
                    else {
                        divPassword.style.display = 'none';
                        hdPasswordModificata.value = '0';
                    }
                }
            }
        }

        function chkAmministratore_Checked(ischecked) {
            var tabStrip = $find("<%= rtsAccount.ClientID %>");   
            var tab = tabStrip.findTabByText( "Associazioni");
            //var tab = tabStrip.get_tabs().getTab(tabStrip.get_tabs().get_count()-2);
            tab.set_visible(!ischecked);
        }          
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    <asp:Label ID="lblTitolo" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">

    <script type="text/javascript">

        Sys.Application.add_load(pageLoad);

        function pageLoad() {
            try {                
                var rbtnStandard = document.getElementById('<%=rbtnStandard.ClientID%>');
                if (rbtnStandard != null) {

                    if (!rbtnStandard.checked) {
                        mostraNascondiColonnaComboCliente(true);
                        mostraNascondiTabOperatoriAssociati(false);
                    }
                    else {                        
                        mostraNascondiColonnaComboCliente(false);
                        mostraNascondiTabOperatoriAssociati(true);
                    }
                }                
            } catch (e) {
                showMessage("Attenzione", e.message);
            }

            ImpostaVisibilitàComboIndirizzi();
        }

        function mostraNascondiTabOperatoriAssociati(mostra) {
            if (mostra == null) mostra = false;

            var tabStrip = $find("<%= rtsAccount.ClientID %>");
            if (tabStrip != null) {
                var tab = tabStrip.findTabByText("Operatori Associati");
                if (tab != null) {
                    tab.set_visible(mostra); 
                }
            }            
        }

        function mostraNascondiColonnaComboCliente(mostra) {
            if (mostra == null) mostra = false;

            var lcComboCliente = document.getElementById('<%=lcComboCliente.ClientID%>');
            if (lcComboCliente != null) {
                lcComboCliente.style.display = (mostra) ? "" : "none";                            
            }
        }

        function rbtnStandard_onclick() {            
            mostraNascondiColonnaComboCliente(false);
            mostraNascondiTabOperatoriAssociati(true);
            return true;
        }

        function rbtnCliente_onclick(radioButtonCliente) {

            var hfIsNuovoAccount = $get('<%=hfIsNuovoAccount.ClientID%>');
            if (hfIsNuovoAccount != null && hfIsNuovoAccount.value == "0") {
                var messaggio = "Attenzione!!\nTutti gli eventuali operatori memorizzati verranno rimossi, vuoi continuare?";
                var confirmResult = confirm(messaggio);
                if (!confirmResult) {
                    radioButtonCliente.checked = false;

                    var rbtnStandard = $get('<%=rbtnStandard.ClientID%>');
                    if (rbtnStandard != null) {
                        rbtnStandard.click();
                        return false;
                    }
                }
            }
            
            mostraNascondiColonnaComboCliente(true);
            mostraNascondiTabOperatoriAssociati(false);

            return true;
        }


        
        function rcbCliente_OnClientSelectedIndexChanged(sender, eventArgs) {
            var comboIndirizzi = $find('<%=rcbIndirizzi.ClientID%>');
            comboIndirizzi.clearSelection();
        }

        // Forza la lettura lato server, ad ogni apertura della combo rcbIndirizzi, dei dati relativi alle Destinazioni
        function rcbIndirizzi_OnClientDropDownOpening(sender, arg) {
            sender.clearItems();
            sender.requestItems("", true);
        }

        // Metodo di gestione dell'evento OnClientItemsRequesting relativo al combo che contiene le Destinazioni da selezionare
        // viene utilizzato per passare parametri alla funzione lato server che seleziona le Destinazioni da proporre in base al Cliente selezionato
        function rcbIndirizzi_OnClientItemsRequesting(sender, eventArgs) {
            if (eventArgs == null || !eventArgs.get_context) return;

            var comboCliente = $find('<%=rcbCliente.ClientID%>');
            var codiceCliente = comboCliente.get_value();
            var context = eventArgs.get_context();

            if (context != null) {
                context["CodiceCliente"] = codiceCliente;
            }
        }

        // Mostra o nasconde la combo relativa alle destinazioni alternative dei clienti in base al fatto che l'utente in configurazione abbia la necessità di utilizzarlo
        function ImpostaVisibilitàComboIndirizzi() {
            var rblTipoAccountClienteAdmin = document.getElementById('<%=rblTipoAccountClienteAdmin.ClientID%>');
            var comboIndirizzi = $find('<%=rcbIndirizzi.ClientID%>');
            comboIndirizzi.set_visible(!rblTipoAccountClienteAdmin.checked);
        }

    </script>

    <telerik:RadToolBar ID="RadToolBar1"
        runat="server"
        Width="100%"
        OnButtonClick="RadToolBar1_ButtonClick"
        OnClientButtonClicking="ToolBarButtonClicking">
        <Items>
            <telerik:RadToolBarButton runat="server" Value="TornaElenco" CommandName="TornaElenco" CausesValidation="False" NavigateUrl="Accounts.aspx" PostBack="False" ImageUrl="~/UI/Images/Toolbar/back.png" Text="Vai all'Elenco" ToolTip="Vai alla pagina con l'elenco degli Accounts" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/refresh.png" PostBack="False" Value="Aggiorna" CommandName="Aggiorna" Text="Aggiorna" ToolTip="Aggiorna i dati visualizzati" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" NavigateUrl="Account.aspx" ImageUrl="~/UI/Images/Toolbar/add.png" PostBack="False" CommandName="Nuovo" Value="Nuovo" Text="Nuovo Account" ToolTip="Permette la creazione di un nuovo Account" />
            <telerik:RadToolBarButton runat="server" CommandName="Salva" ImageUrl="~/UI/Images/Toolbar/Save.png" Text="Salva" Value="Salva" ToolTip="Memorizza i dati visualizzati" />
        </Items>
    </telerik:RadToolBar>


    <asp:ValidationSummary ID="ValidationSummary1" runat="server"
        CssClass="ValidationSummaryStyle"
        HeaderText="&nbsp;Errori di validazione dei dati:"
        DisplayMode="BulletList"
        ShowMessageBox="false"
        ShowSummary="true" />

    <div class="pageViewPadding">
        <asp:HiddenField ID="hfIsNuovoAccount" runat="server" Value="" />
        <telerik:RadTabStrip runat="server" ID="rtsAccount" MultiPageID="rmpAccount" SelectedIndex="0" CausesValidation="false">
            <Tabs>
                <telerik:RadTab PageViewID="rpvDatiGenerali" Text="Dati Generali"></telerik:RadTab>
                <telerik:RadTab PageViewID="rpvOperatoriAssociati" Text="Operatori Associati"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="rmpAccount" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="1px" SelectedIndex="0">
            <telerik:RadPageView ID="rpvDatiGenerali" runat="server" CssClass="pageView">
                <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                    <Rows>
                        <telerik:LayoutRow RowType="Region">
                            <Columns>
                                <telerik:LayoutColumn Span="6" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:Label ID="lblUserName" runat="server" Text="UserName" AssociatedControlID="rtbUserName" Font-Bold="true" />&nbsp;
                                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server"
                                        ControlToValidate="rtbUserName"
                                        ErrorMessage="Il nome utilizzato per l'accesso dall'account è obbligatorio."
                                        Text="(Obbligatorio)"
                                        ForeColor="Red">
                                    </asp:RequiredFieldValidator><br />
                                    <telerik:RadTextBox ID="rtbUserName"
                                        runat="server"
                                        Width="100%"
                                        ClientEvents-OnBlur="UpdateValidatorSummary" />
                                </telerik:LayoutColumn>
                                <telerik:LayoutColumn Span="6" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:Label ID="lblNominativo" runat="server" Text="Nominativo" AssociatedControlID="rtbNominativo" Font-Bold="true" />&nbsp;
                                    <asp:RequiredFieldValidator ID="rfvNominativo" runat="server"
                                        ControlToValidate="rtbNominativo"
                                        ErrorMessage="Il Nominativo dell'account è obbligatorio."
                                        Text="(Obbligatorio)"
                                        ForeColor="Red">
                                    </asp:RequiredFieldValidator><br />
                                    <telerik:RadTextBox ID="rtbNominativo"
                                        runat="server"
                                        Width="100%"
                                        ClientEvents-OnBlur="UpdateValidatorSummary" />
                                </telerik:LayoutColumn>
                            </Columns>
                        </telerik:LayoutRow>
                        <telerik:LayoutRow RowType="Region">
                            <Columns>
                                <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:Label ID="lblEmail" runat="server" Text="Email" AssociatedControlID="rtbEmail" Font-Bold="true" />&nbsp;
                                    <asp:RequiredFieldValidator ID="rfvEmail"
                                        runat="server"
                                        Display="Dynamic"
                                        ForeColor="Red"
                                        Text="(Obbligatoria)"
                                        ErrorMessage="L'indirizzo email è obbligatorio"
                                        ControlToValidate="rtbEmail" />
                                    <br />
                                    <telerik:RadTextBox ID="rtbEmail" InputType="Email"
                                        runat="server"
                                        Width="100%"
                                        MaxLength="70"
                                        ClientEvents-OnBlur="UpdateValidatorSummary" />
                                </telerik:LayoutColumn>
                            </Columns>
                        </telerik:LayoutRow>

                        <telerik:LayoutRow RowType="Region">
                            <Columns>
                                <telerik:LayoutColumn Span="2" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:Label ID="lblTipologiaAccount" runat="server" Font-Bold="true" Text="Tipologia Account" /><br />
                                    <div style="padding-top:5px">
                                        <asp:RadioButton ID="rbtnStandard" runat="server" Text="Azienda " TextAlign="Left" GroupName="TipologiaAccount" Checked="true" OnClick="return rbtnStandard_onclick();" />
                                        <asp:RadioButton ID="rbtnCliente" runat="server" Text="Cliente " TextAlign="Left" GroupName="TipologiaAccount" OnClick="return rbtnCliente_onclick(this);" />
                                    </div>
                                </telerik:LayoutColumn>
                                <telerik:LayoutColumn ID="lcComboCliente" runat="server" Span="10" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:Label ID="lblCliente" runat="server" Text="Cliente" Font-Bold="true" AssociatedControlID="rcbCliente"/><br />
                                    <telerik:RadComboBox ID="rcbCliente" runat="server"
                                        Width="100%" DropDownAutoWidth="Disabled"
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
                                        OnItemsRequested="rcbCliente_ItemsRequested">
                                        <ItemTemplate>
                                            <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                                <Rows>
                                                    <telerik:LayoutRow RowType="Region">
                                                        <Columns>
                                                            <telerik:LayoutColumn Span="1">
                                                                <%# Eval("CODCONTO") %>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="6">
                                                                <%# Eval("DSCCONTO1") %>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="2">
                                                                <%# Eval("INDIRIZZO") %>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="2">
                                                                <%# Eval("LOCALITA") %>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="1">
                                                                <%# Eval("PROVINCIA") %>
                                                            </telerik:LayoutColumn>
                                                        </Columns>
                                                    </telerik:LayoutRow>
                                                </Rows>
                                            </telerik:RadPageLayout>
                                        </ItemTemplate>
                                    </telerik:RadComboBox>

                                    <telerik:RadComboBox ID="rcbIndirizzi" runat="server"
                                        Width="100%" DropDownAutoWidth="Disabled"
                                        MarkFirstMatch="false"
                                        HighlightTemplatedItems="true"
                                        EmptyMessage="Indirizzo principale"
                                        ItemsPerRequest="200"
                                        ShowMoreResultsBox="true"
                                        EnableLoadOnDemand="false"
                                        EnableItemCaching="false"
                                        AllowCustomText="false"
                                        LoadingMessage="Caricamento in corso..."
                                        DataValueField="CODICE"
                                        DataTextField="RAGIONESOCIALE"
                                        OnClientDropDownOpening="rcbIndirizzi_OnClientDropDownOpening"
                                        OnClientItemsRequesting="rcbIndirizzi_OnClientItemsRequesting"
                                        OnItemsRequested="rcbIndirizzi_ItemsRequested">
                                        <ItemTemplate>
                                            <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                                <Rows>
                                                    <telerik:LayoutRow RowType="Region">
                                                        <Columns>
                                                            <telerik:LayoutColumn Span="4">
                                                                <%# Eval("RAGIONESOCIALE") %>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="4">
                                                                <%# Eval("INDIRIZZO") %>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="1">
                                                                <%# Eval("CAP") %>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="1">
                                                                <%# Eval("LOCALITA") %>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="1">
                                                                <%# Eval("PROVINCIA") %>
                                                            </telerik:LayoutColumn>
                                                            <telerik:LayoutColumn Span="1">
                                                                <%# Eval("TELEFONO") %>
                                                            </telerik:LayoutColumn>
                                                        </Columns>
                                                    </telerik:LayoutRow>
                                                </Rows>
                                            </telerik:RadPageLayout>
                                        </ItemTemplate>
                                    </telerik:RadComboBox>

                                    <asp:RadioButton runat="server" ID="rblTipoAccountClienteStandard" Text="Utente Standard" GroupName="TipoAccountCliente" onclick="ImpostaVisibilitàComboIndirizzi()" />&nbsp;
                                    <asp:RadioButton runat="server" ID="rblTipoAccountClienteSupervisore" Text="Utente Supervisore" GroupName="TipoAccountCliente" onclick="ImpostaVisibilitàComboIndirizzi()" />
                                    <asp:RadioButton runat="server" ID="rblTipoAccountClienteAdmin" Text="Utente Supervisore Multi Organizzazione" GroupName="TipoAccountCliente" onclick="ImpostaVisibilitàComboIndirizzi()" />&nbsp;
                                </telerik:LayoutColumn>
                            </Columns>
                        </telerik:LayoutRow>

                        <telerik:LayoutRow RowType="Region" CssClass="SpaziaturaSuperioreRiga">
                            <Columns>
                                <telerik:LayoutColumn Span="0" SpanSm="0" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:CheckBox runat="server" ID="chkAmministratore" Text="Utente Super Amministratore" TextAlign="Right" AutoPostBack="true" OnCheckedChanged="chkAmministratore_CheckedChanged" />
                                    <%--                                    <telerik:RadButton runat="server" ID="rchkAmministratore" ButtonType="ToggleButton" ToggleType="CheckBox" Text="Utente Super Amministratore" AutoPostBack="false" OnClientCheckedChanged="AssignValueOnCheckboxTick2"></telerik:RadButton>--%>
                                </telerik:LayoutColumn>
                                <telerik:LayoutColumn Span="0" SpanSm="0" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:CheckBox runat="server" ID="chkValidatore" Text="Utente Validatore Offerta" TextAlign="Right" />
                                </telerik:LayoutColumn>
                                <telerik:LayoutColumn Span="0" SpanSm="0" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:CheckBox runat="server" ID="chkSolaLettura" Text="Accesso in Sola Lettura" TextAlign="Right" />
                                </telerik:LayoutColumn>
                                <telerik:LayoutColumn Span="0" SpanSm="0" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:CheckBox runat="server" ID="chkBloccaUtente" Text="Utente Bloccato" TextAlign="Right" />
                                </telerik:LayoutColumn>
                            </Columns>
                        </telerik:LayoutRow>
                        <telerik:LayoutRow RowType="Region">
                            <Content>
                                <div style="padding: 15px;">
                                    <br />
                                    <br />
                                    <telerik:RadButton ID="rbModificaPassword" runat="server" Text="Gestione password di accesso" ButtonType="LinkButton" AutoPostBack="false" NavigateUrl="#" CausesValidation="false" />

                                    <input id="hdPasswordModificata" type="hidden" runat="server" />
                                    <div id="divPassword" style="border: 2px Solid #ABC1DE; background-color: #FFFFFF; margin-top: 5px; padding: 5px; display: none;">
                                        <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                            <telerik:LayoutRow RowType="Region">
                                                <Columns>
                                                    <telerik:LayoutColumn Span="0" SpanSm="0" SpanXs="12">
                                                        <div style="min-width: 150px;">
                                                            <asp:Label ID="lblPassword" runat="server" Text="Password" AssociatedControlID="rtbPassword" Font-Bold="true" Width="200px" />
                                                        </div>
                                                    </telerik:LayoutColumn>
                                                    <telerik:LayoutColumn Span="0" SpanSm="0" SpanXs="12">
                                                        <telerik:RadTextBox ID="rtbPassword" runat="server" TextMode="Password" Width="200px"></telerik:RadTextBox>
                                                    </telerik:LayoutColumn>
                                                    <telerik:LayoutColumn Span="0" SpanSm="0" SpanXs="12">
                                                        <asp:RegularExpressionValidator ID="revPassword" Display="Dynamic" runat="server"
                                                            ControlToValidate="rtbPassword" ForeColor="Red"
                                                            Text="La password deve contenere almeno una lettera ed un numero."
                                                            ErrorMessage="La password deve contenere almeno una lettera ed un numero."
                                                            ValidationExpression="(.*[0-9]{1,}.*[a-zA-Z]{1,}.*)|(.*[a-zA-Z]{1,}.*[0-9]{1,}.*)">
                                                        </asp:RegularExpressionValidator>
                                                    </telerik:LayoutColumn>
                                                </Columns>
                                            </telerik:LayoutRow>
                                            <telerik:LayoutRow RowType="Region">
                                                <Columns>
                                                    <telerik:LayoutColumn Span="0" SpanSm="0" SpanXs="12">
                                                        <div style="min-width: 150px;">
                                                            <asp:Label ID="lblConfermaPassword" runat="server" Text="Conferma Password" AssociatedControlID="rtbConfermaPassword" Font-Bold="true" />
                                                        </div>
                                                    </telerik:LayoutColumn>
                                                    <telerik:LayoutColumn Span="0" SpanSm="0" SpanXs="12">
                                                        <telerik:RadTextBox ID="rtbConfermaPassword" runat="server" TextMode="Password" Width="200px"></telerik:RadTextBox>
                                                    </telerik:LayoutColumn>
                                                    <telerik:LayoutColumn Span="0" SpanSm="0" SpanXs="12">
                                                        <asp:CompareValidator ID="cvPasswords" runat="server" ForeColor="Red"
                                                            ErrorMessage="La password e le password di conferma non coincidono"
                                                            Text="Mancata corrispondenza"
                                                            ControlToValidate="rtbConfermaPassword"
                                                            ControlToCompare="rtbPassword">
                                                        </asp:CompareValidator>
                                                    </telerik:LayoutColumn>
                                                </Columns>
                                            </telerik:LayoutRow>
                                            <telerik:LayoutRow RowType="Region">
                                                <Columns>
                                                    <telerik:LayoutColumn Span="0" SpanSm="0" SpanXs="12">
                                                        <div style="min-width: 150px;">
                                                            <asp:Label ID="lblScadenzaPassword" runat="server" Text="Scadenza password:" AssociatedControlID="rdpDataScadenzaPassword" />
                                                        </div>
                                                    </telerik:LayoutColumn>
                                                    <telerik:LayoutColumn Span="0" SpanSm="0" SpanXs="12">
                                                        <!-- MaxDate="06/06/2079"  Valore Massimo dello smalldatetime -->
                                                        <telerik:RadDatePicker ID="rdpDataScadenzaPassword" runat="server"
                                                            DatePopupButton-ToolTip="Seleziona una data dal calendario"
                                                            DateInput-DisabledStyle-BackColor="#F5F5F5"
                                                            MaxDate="06/06/2079"
                                                            MinDate="01/01/1900"
                                                            ClientEvents-OnDateSelected="ValidationSummaryOnSubmit"
                                                            Width="200px">
                                                            <Calendar ID="Calendar1" runat="server">
                                                                <SpecialDays>
                                                                    <telerik:RadCalendarDay ItemStyle-CssClass="rcToday" Repeatable="Today" />
                                                                </SpecialDays>
                                                            </Calendar>
                                                        </telerik:RadDatePicker>
                                                    </telerik:LayoutColumn>
                                                </Columns>
                                            </telerik:LayoutRow>
                                        </telerik:RadPageLayout>
                                    </div>
                                </div>
                            </Content>
                        </telerik:LayoutRow>
                    </Rows>
                </telerik:RadPageLayout>
            </telerik:RadPageView>

            <telerik:RadPageView ID="rpvOperatoriAssociati" runat="server" CssClass="pageView">

                <telerik:RadGrid runat="server"
                    ID="rgGrigliaOperatoriAssociati"
                    CssClass="GridResponsiveColumns"
                    AllowPaging="true"
                    PageSize="10"
                    GridLines="None"
                    SortingSettings-SortToolTip="Clicca qui per ordinare i dati in base a questa colonna"
                    AutoGenerateColumns="false"
                    OnPreRender="rgGrigliaOperatoriAssociati_PreRender"
                    OnInsertCommand="rgGrigliaOperatoriAssociati_InsertCommand"
                    OnDeleteCommand="rgGrigliaOperatoriAssociati_DeleteCommand"
                    OnNeedDataSource="rgGrigliaOperatoriAssociati_NeedDataSource"
                    OnItemCreated="rgGrigliaOperatoriAssociati_ItemCreated"
                    OnItemDataBound="rgGrigliaOperatoriAssociati_ItemDataBound"
                    PagerStyle-FirstPageToolTip="Prima Pagina"
                    PagerStyle-LastPageToolTip="Ultima Pagina"
                    PagerStyle-PrevPageToolTip="Pagina Precedente"
                    PagerStyle-NextPageToolTip="Pagina Successiva"
                    PagerStyle-PagerTextFormat="{4} Elementi da {2} a {3} su {5}, Pagina {0} di {1}">
                    <MasterTableView DataKeyNames="IDAccount,IDOperatore" TableLayout="Fixed" EditMode="EditForms"
                        Width="100%"
                        AllowFilteringByColumn="true"
                        AllowSorting="true"
                        AllowMultiColumnSorting="true"
                        GridLines="Both"
                        NoMasterRecordsText="Nessun dato da visualizzare">

                        <Columns>

                            <telerik:GridTemplateColumn UniqueName="CognomeNomeOperatore"
                                ItemStyle-Wrap="true"
                                HeaderText="Cognome Nome Operatore"
                                EditFormHeaderTextFormat="Operatori da associare"
                                DataField="Operatore.CognomeNome">
                                <ItemTemplate>
                                    <%# Eval("Operatore.CognomeNome") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadComboBox ID="rcbOperatore" runat="server"
                                        Width="170px"
                                        EnableItemCaching="true"
                                        DataTextField="CognomeNome"
                                        DataValueField="ID"
                                        CheckBoxes="true"
                                        EnableCheckAllItemsCheckBox="true"
                                        LoadingMessage="Caricamento ..."
                                        Localization-AllItemsCheckedString="Tutti gli elementi"
                                        Localization-CheckAllString="Seleziona tutto"
                                        Localization-ItemsCheckedString="Elementi selezionati"
                                        Localization-NoMatches="Nessun elemento trovato" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridButtonColumn HeaderStyle-Width="40px" UniqueName="ColonnaElimina"
                                Resizable="false"
                                HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Wrap="true"
                                ItemStyle-HorizontalAlign="Center"
                                ConfirmText="Eliminare l'Operatore selezionato?"
                                ConfirmTextFields="Operatore.CognomeNome"
                                ConfirmTextFormatString="Eliminare l'associazione con l'Operatore '{0}'?"
                                ConfirmDialogType="RadWindow"
                                ConfirmTitle="Elimina"
                                ButtonType="ImageButton"
                                ImageUrl="/UI/Images/Toolbar/delete.png"
                                Text="Elimina..."
                                CommandName="Delete" />

                        </Columns>

                        <EditFormSettings>
            			    <EditColumn ButtonType="LinkButton" InsertText="Salva nuova voce" UpdateText="Salva modifiche" CancelText="Annulla"></EditColumn>
            			    <FormTableButtonRowStyle CssClass="EditButtonStyle" />
                        </EditFormSettings>

                    </MasterTableView>

                    <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" />
                    <GroupingSettings CaseSensitive="false" />

                    <ClientSettings EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="True" />
                        <Resizing AllowColumnResize="true" EnableRealTimeResize="true" AllowResizeToFit="false" />
                    </ClientSettings>

                </telerik:RadGrid>

            </telerik:RadPageView>
        </telerik:RadMultiPage>
    </div>

    <div style="display: none;">
        <telerik:RadAjaxManagerProxy runat="server" ID="ramOperatoriAssociati">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgGrigliaOperatoriAssociati">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgGrigliaOperatoriAssociati" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                        <telerik:AjaxUpdatedControl ControlID="RadWindowManagerMaster" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManagerProxy>
        <telerik:RadPersistenceManagerProxy ID="rpmpOperatoriAssociati" runat="server">
            <PersistenceSettings>
                <telerik:PersistenceSetting ControlID="rgGrigliaOperatoriAssociati" />
            </PersistenceSettings>
        </telerik:RadPersistenceManagerProxy>
    </div>

</asp:Content>
