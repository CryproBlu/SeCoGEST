<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="DettagliClausolaVessatoria.aspx.cs" Inherits="SeCoGEST.Web.Offerte.DettagliClausoleVessatorie" %>
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
    <asp:Label ID="lblTitolo" runat="server" Text="Dettagli Clausola Vessatoria" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">

    <telerik:RadToolBar ID="RadToolBar1"
        runat="server"
        OnButtonClick="RadToolBar1_ButtonClick"
        OnClientButtonClicking="ToolBarButtonClicking">
        <Items>
            <telerik:RadToolBarButton runat="server" Value="TornaElenco" CommandName="TornaElenco" CausesValidation="False" NavigateUrl="ElencoClausoleVessatorie.aspx" PostBack="False" ImageUrl="~/UI/Images/Toolbar/back.png" Text="Vai all'Elenco" ToolTip="Vai alla pagina di elenco delle clausole vessatorie" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/refresh.png" PostBack="False" Value="Aggiorna" CommandName="Aggiorna" Text="Aggiorna" ToolTip="Aggiorna i dati visualizzati" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreNuovo" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" NavigateUrl="DettagliClausolaVessatoria.aspx" ImageUrl="~/UI/Images/Toolbar/add.png" PostBack="False" CommandName="Nuovo" Value="Nuovo" Text="Nuova Clausola Vessatoria" ToolTip="Permette la creazione di una nuova Clausola Vessatoria" />
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
                         <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="1" SpanMd="1" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblIndiceClausolaVessatoria" runat="server" Text="Codice" Font-Bold="true" AssociatedControlID="rtbCodiceClausolaVessatoria" />
                              <asp:RequiredFieldValidator ID="rfvCodice" runat="server" Display="Dynamic"
                                ControlToValidate="rtbCodiceClausolaVessatoria"
                                ErrorMessage="Codice è obbligatorio."
                                ForeColor="Red">
                                <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                            </asp:RequiredFieldValidator>
                             <br />
                            <telerik:RadTextBox runat="server" ID="rtbCodiceClausolaVessatoria" Width="100%"></telerik:RadTextBox>
                        </telerik:LayoutColumn>
                     </Columns>
                 </telerik:LayoutRow>
                 <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblTestoClausolaVessatoria" runat="server" Font-Bold="true" Text="Descrizione" AssociatedControlID="reDescrizioneClausolaVessatoria" /><br />
                            <telerik:RadEditor runat="server" ID="reDescrizioneClausolaVessatoria" ToolsFile="~/App_Data/Editor/Settings/Tools.xml" ToolbarMode="Default" Width="100%" Height="430px" BorderWidth="1" BorderStyle="Solid" />
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>
             </Rows>
         </telerik:RadPageLayout>
    </div>

</asp:Content>
