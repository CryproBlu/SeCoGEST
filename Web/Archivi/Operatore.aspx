<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="Operatore.aspx.cs" Inherits="SeCoGEST.Web.Archivi.Operatore" %>

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
                        messaggioConfermato = confirm('Si desidera Creare un nuovo Ruolo?\n\nEventuali modifiche apportate ai dati e non salvate verranno perse.');
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
    <asp:Label ID="lblTitolo" runat="server" Text="Gestione Ruolo" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">

    <telerik:RadToolBar ID="RadToolBar1"
        runat="server"
        OnButtonClick="RadToolBar1_ButtonClick"
        OnClientButtonClicking="ToolBarButtonClicking">
        <Items>
            <telerik:RadToolBarButton runat="server" Value="TornaElenco" CommandName="TornaElenco" CausesValidation="False" NavigateUrl="Operatori.aspx" PostBack="False" ImageUrl="~/UI/Images/Toolbar/back.png" Text="Vai all'Elenco" ToolTip="Vai alla pagina di elenco Ruoli" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/refresh.png" PostBack="False" Value="Aggiorna" CommandName="Aggiorna" Text="Aggiorna" ToolTip="Aggiorna i dati visualizzati" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" NavigateUrl="Operatore.aspx" ImageUrl="~/UI/Images/Toolbar/add.png" PostBack="False" CommandName="Nuovo" Value="Nuovo" Text="Nuovo Ruolo" ToolTip="Permette la creazione di un nuovo Ruolo" Target="_blank" />
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
                        <telerik:LayoutColumn Span="6" SpanMd="6" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblCognome" runat="server" Text="Cognome" AssociatedControlID="rtbCognome" Font-Bold="true" />&nbsp;
                            <asp:RequiredFieldValidator ID="rfvCognome" runat="server" Display="Dynamic"
                                ControlToValidate="rtbCognome"
                                ErrorMessage="Il Cognome è obbligatorio."
                                ForeColor="Red">
                                <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                            </asp:RequiredFieldValidator>
                            <br />
                            <telerik:RadTextBox runat="server" ID="rtbCognome" Width="100%"></telerik:RadTextBox>
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="6" SpanMd="6" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblNome" runat="server" Text="Nome" AssociatedControlID="rtbNome" Font-Bold="true" />&nbsp;
                            <asp:RequiredFieldValidator ID="rfvNome" runat="server" Display="Dynamic"
                                ControlToValidate="rtbNome"
                                ErrorMessage="Il Nome è obbligatorio."
                                ForeColor="Red">
                                <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                            </asp:RequiredFieldValidator>
                            <br />
                            <telerik:RadTextBox runat="server" ID="rtbNome" Width="100%"></telerik:RadTextBox>
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblEmailResponsabile" runat="server" Text="Email di riferimento" AssociatedControlID="rtbEmailResponsabile" />
                            <br />
                            <telerik:RadTextBox ID="rtbEmailResponsabile" InputType="Email"
                                runat="server"
                                Width="100%"
                                ClientEvents-OnBlur="UpdateValidatorSummary" />
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="6" SpanMd="6" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:CheckBox ID="chkArea" runat="server"
                                Text="Utilizza come Area"
                                TextAlign="Right"
                                ToolTip="Se selezionato sta ad indicare che l'operatore corrente viene utilizzato come area, altrimenti si tratta di un normale operatore" />
                        </telerik:LayoutColumn>
                        <telerik:LayoutColumn Span="6" SpanMd="6" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:CheckBox ID="chkDisabilitato" runat="server" Text="Disabilitato" TextAlign="Right" />
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>

                <telerik:LayoutRow ID="lrGrigliaAccountAssociati" runat="server" RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="12" SpanMd="12" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">

                            <br />
                            <br />

                            <telerik:RadGrid runat="server"
                                ID="rgGriglia"
                                CssClass="GridResponsiveColumns"
                                AllowPaging="true"
                                PageSize="10"
                                GridLines="None"
                                SortingSettings-SortToolTip="Clicca qui per ordinare i dati in base a questa colonna"
                                AutoGenerateColumns="false"
                                OnPreRender="rgGriglia_PreRender"
                                OnNeedDataSource="rgGriglia_NeedDataSource"
                                OnItemCreated="rgGriglia_ItemCreated"
                                OnItemDataBound="rgGriglia_ItemDataBound"
                                PagerStyle-FirstPageToolTip="Prima Pagina"
                                PagerStyle-LastPageToolTip="Ultima Pagina"
                                PagerStyle-PrevPageToolTip="Pagina Precedente"
                                PagerStyle-NextPageToolTip="Pagina Successiva"
                                PagerStyle-PagerTextFormat="{4} Elementi da {2} a {3} su {5}, Pagina {0} di {1}">
                                <MasterTableView DataKeyNames="IDAccount,IDOperatore" TableLayout="Fixed" 
                                    Width="100%"
                                    AllowFilteringByColumn="true"
                                    AllowSorting="true"
                                    AllowMultiColumnSorting="true"
                                    GridLines="Both"
                                    Caption="ELENCO ACCOUNT ASSOCIATI"
                                    NoMasterRecordsText="Nessun dato da visualizzare">

                                    <Columns>

                                        <telerik:GridBoundColumn ItemStyle-Wrap="true"
                                            FilterControlWidth="80%"
                                            FilterImageToolTip="Applica filtro"
                                            DataField="Account.UserName"
                                            HeaderText="UserName" />

                                        <telerik:GridBoundColumn ItemStyle-Wrap="true"
                                            FilterControlWidth="80%"
                                            FilterImageToolTip="Applica filtro"
                                            DataField="Account.Nominativo"
                                            HeaderText="Nominativo" />

                                        <telerik:GridBoundColumn ItemStyle-Wrap="true"
                                            FilterControlWidth="80%"
                                            FilterImageToolTip="Applica filtro"
                                            DataField="Account.Email"
                                            HeaderText="Email" />

<%--                                        <telerik:GridCheckBoxColumn
                                            HeaderText="Notifiche"
                                            HeaderStyle-Width="90px"
                                            ItemStyle-HorizontalAlign="Center"
                                            DataField="InviaNotifiche">
                                        </telerik:GridCheckBoxColumn>--%>

                                        <telerik:GridTemplateColumn
                                            HeaderText="Notifiche"
                                            HeaderStyle-Width="90px"
                                            ItemStyle-HorizontalAlign="Center"
                                            AllowFiltering="false">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkInviaNotifiche" runat="server" AutoPostBack="true" Checked='<%# Bind("AttivaInvioNotifiche") %>' OnCheckedChanged="chkInviaNotifiche_CheckedChanged" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>

                                </MasterTableView>

                                <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" />
                                <GroupingSettings CaseSensitive="false" />

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


        <div style="display: none;">
            <telerik:RadAjaxManagerProxy runat="server" ID="ramOperatori">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="rgGriglia">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="rgGriglia" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                            <telerik:AjaxUpdatedControl ControlID="RadWindowManagerMaster" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManagerProxy>
        </div>

        <br />
        <br />
        <br />
    </div>

</asp:Content>
