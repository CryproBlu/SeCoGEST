<%@ Page Title="Dettagli Progetto" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="DettagliProgetto.aspx.cs" Inherits="SeCoGEST.Web.Progetti.DettagliProgetto" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/UI/PageMessage.ascx" TagPrefix="uc1" TagName="PageMessage" %>
<%@ Register Src="~/UI/DocumentazioneAllegata.ascx" TagName="DocumentazioneAllegata" TagPrefix="uc1" %>
<%@ Register Src="~/UI/AssociatoreOperatori.ascx" TagName="AssociatoreOperatori" TagPrefix="uc1" %>
<%@ Register Src="~/Progetti/AttivitaProgetto.ascx" TagName="AttivitaProgetto" TagPrefix="uc1" %>

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

                    if (button.get_commandName() == "Chiudi") {
                        messaggioConfermato = confirm('Chiudere questo progetto?');
                    }

                    if (!messaggioConfermato) {
                        args.set_cancel(true);
                    }
                }
            }
        }

        function rcbCliente_OnClientSelectedIndexChanged(sender, eventArgs) {
            var Ind = eventArgs.get_item().get_attributes().getAttribute("Ind");
            var CAP = eventArgs.get_item().get_attributes().getAttribute("CAP");
            var Loc = eventArgs.get_item().get_attributes().getAttribute("Loc");
            var Prov = eventArgs.get_item().get_attributes().getAttribute("Prov");
            var Tel = eventArgs.get_item().get_attributes().getAttribute("Tel");
            var State = eventArgs.get_item().get_attributes().getAttribute("State");
            var Note = eventArgs.get_item().get_attributes().getAttribute("Note");
            var DefaultVisibilitaTicketCliente = eventArgs.get_item().get_attributes().getAttribute("DefaultVisibilitaTicketCliente");
            ScriviIndirizzo(Ind, CAP, Loc, Prov, Tel, State, Note, DefaultVisibilitaTicketCliente);

            var rcbIndirizzi = $find('<%=rcbIndirizzi.ClientID%>');
            rcbIndirizzi.clearSelection();

<%--            var rcbTipologiaInterventoTicket = $find('<%=rcbTipologiaInterventoTicket.ClientID%>');
            rcbTipologiaInterventoTicket.clearSelection();--%>

<%--            __doPostBack("<%=rcbTipologiaInterventoTicket.ClientID %>", "");--%>
            //__doPostBack("", "");
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
        function rcbIndirizzi_OnClientSelectedIndexChanged(sender, eventArgs) {
            var Ind = eventArgs.get_item().get_attributes().getAttribute("Ind");
            var CAP = eventArgs.get_item().get_attributes().getAttribute("CAP");
            var Loc = eventArgs.get_item().get_attributes().getAttribute("Loc");
            var Prov = eventArgs.get_item().get_attributes().getAttribute("Prov");
            var Tel = eventArgs.get_item().get_attributes().getAttribute("Tel");
            var State = eventArgs.get_item().get_attributes().getAttribute("State");
            var Note = eventArgs.get_item().get_attributes().getAttribute("Note");
            var DefaultVisibilitaTicketCliente = eventArgs.get_item().get_attributes().getAttribute("DefaultVisibilitaTicketCliente");
            ScriviIndirizzo(Ind, CAP, Loc, Prov, Tel, State, Note, DefaultVisibilitaTicketCliente);
        }

        function ScriviIndirizzo(Ind, CAP, Loc, Prov, Tel, State, Note, DefaultVisibilitaTicketCliente) {
            var rtbIndirizzo = $find('<%=rtbIndirizzo.ClientID%>');
            var rtbCAP = $find('<%=rtbCAP.ClientID%>');
            var rtbLocalita = $find('<%=rtbLocalita.ClientID%>');
            var rtbProvincia = $find('<%=rtbProvincia.ClientID%>');
            var rtbTelefono = $find('<%=rtbTelefono.ClientID%>');
            var lblStatoCliente = document.getElementById('<%=lblStatoCliente.ClientID%>');
            var lblNoteCliente = document.getElementById('<%=lblNoteCliente.ClientID%>');

            rtbIndirizzo.set_value(Ind);
            rtbCAP.set_value(CAP);
            rtbLocalita.set_value(Loc);
            rtbProvincia.set_value(Prov);
            rtbTelefono.set_value(Tel);
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

        // Metodo di gestione dell'evento client OnRequestStart del pannello ajax relativo agli allegati
        function rapAllegati_ClientEvents_OnRequestStart(sender, args) {
            if (args.set_enableAjax) {
                args.set_enableAjax(false);
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

        #btn-back-to-top {
            position: fixed;
            bottom: 20px;
            right: 20px;
            width: 50px;
            height: 50px;
            display: none;
            cursor: pointer;
            z-index: 10000;            
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    <asp:Label ID="lblTitolo" runat="server" Text="Dettagli Progetto" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">

    <img id="btn-back-to-top" src="/UI/Images/arrow-up.png" title="Torna all'inizio"/>

    <script type="text/javascript">

        //Get the button
        var mybutton = document.getElementById("btn-back-to-top");

        // When the user scrolls down 20px from the top of the document, show the button
        window.onscroll = function () {
            scrollFunction();
        };

        function scrollFunction() {
            if (
                document.body.scrollTop > 20 ||
                document.documentElement.scrollTop > 20
            ) {
                mybutton.style.display = "block";
            } else {
                mybutton.style.display = "none";
            }
        }
        // When the user clicks on the button, scroll to the top of the document
        mybutton.addEventListener("click", backToTop);

        function backToTop() {
            document.body.scrollTop = 0;
            document.documentElement.scrollTop = 0;
        }

    </script>

    <telerik:RadToolBar ID="RadToolBar1"
        runat="server"
        OnButtonClick="RadToolBar1_ButtonClick"
        OnClientButtonClicking="ToolBarButtonClicking">
        <Items>
            <telerik:RadToolBarButton runat="server" Value="TornaElenco" CommandName="TornaElenco" CausesValidation="False" NavigateUrl="ElencoProgetti.aspx" PostBack="False" ImageUrl="~/UI/Images/Toolbar/back.png" Text="Vai all'Elenco" ToolTip="Vai alla pagina di elenco Progetti" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/refresh.png" PostBack="False" Value="Aggiorna" CommandName="Aggiorna" Text="Aggiorna" ToolTip="Aggiorna i dati visualizzati" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreNuovo" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" NavigateUrl="DettagliProgetto.aspx" ImageUrl="~/UI/Images/Toolbar/add.png" PostBack="False" CommandName="Nuovo" Value="Nuovo" Text="Nuovo Progetto" ToolTip="Permette la creazione di un nuovo Progetto" Target="_blank" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreSalva" />
            <telerik:RadToolBarButton runat="server" CommandName="Salva" ImageUrl="~/UI/Images/Toolbar/Save.png" Text="Salva" Value="Salva" ToolTip="Memorizza i dati visualizzati" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreChiudi" />
            <telerik:RadToolBarButton runat="server" CommandName="Chiudi" ImageUrl="~/UI/Images/Toolbar/basecircle.png" Text="Chiudi" Value="Chiudi" ToolTip="Chiude definitivamente il progetto." />
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
                <%-- Intestazione Progetto --%>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="1" SpanMd="1" SpanSm="3" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblNumeroProgetto" runat="server" Text="Numero" Font-Bold="true" AssociatedControlID="rntbNumeroProgetto" />
                            <asp:RequiredFieldValidator ID="rfvProgetto" runat="server" Display="Dynamic"
                                ControlToValidate="rntbNumeroProgetto"
                                ErrorMessage="Il Numero del progetto è obbligatorio."
                                ForeColor="Red">
                            <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                            </asp:RequiredFieldValidator>
                            <br />
                            <telerik:RadNumericTextBox runat="server" ID="rntbNumeroProgetto" MinValue="1" Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" Width="100%" Font-Bold="true" ReadOnly="true"></telerik:RadNumericTextBox>
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="2" SpanMd="3" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
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

                        <telerik:LayoutColumn Span="3" SpanMd="3" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <table border="0" style="width:100%;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblNumeroCommessa" runat="server" Text="Numero Commessa" AssociatedControlID="rtbNumeroCommessa" /><br />
                                        <telerik:RadTextBox runat="server" ID="rtbNumeroCommessa" Width="100%"></telerik:RadTextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblCodiceContratto" runat="server" Text="Codice Contratto" AssociatedControlID="rtbCodiceContratto" /><br />
                                        <telerik:RadTextBox runat="server" ID="rtbCodiceContratto" Width="100%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>

                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="2" SpanMd="2" SpanSm="5" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblReferenteCliente" runat="server" Text="Referente Cliente" AssociatedControlID="rcbReferenteCliente" />
                            <asp:RequiredFieldValidator ID="rfvReferenteCliente" runat="server" Display="Dynamic"
                                ControlToValidate="rcbReferenteCliente"
                                ErrorMessage="Il Referente del Cliente è obbligatorio."
                                ForeColor="Red">
                                <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                            </asp:RequiredFieldValidator><br />
                            <telerik:RadComboBox ID="rcbReferenteCliente" runat="server" Width="100%"
                                AllowCustomText="false"
                                DataValueField="ID"
                                DataTextField="CognomeNome" />
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="2" SpanMd="2" SpanSm="5" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblDPO" runat="server" Text="DPO" AssociatedControlID="rcbDPOs" /><br />
                            <telerik:RadComboBox ID="rcbDPOs" runat="server" Width="100%"
                                AllowCustomText="false"
                                DataValueField="ID"
                                DataTextField="CognomeNome" />
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="2" SpanMd="2" SpanSm="2" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblStato" runat="server" Text="Stato" AssociatedControlID="rcbStato" /><br />
                            <telerik:RadComboBox ID="rcbStato" runat="server" Width="100%" AllowCustomText="false" />
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>


                <%-- Cliente --%>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblCliente" runat="server" Text="Cliente" AssociatedControlID="rcbCliente" Font-Bold="true" />&nbsp;<asp:Label ID="lblStatoCliente" runat="server" AssociatedControlID="rcbCliente" Font-Bold="true" />&nbsp;<asp:Label ID="lblNoteCliente" runat="server" AssociatedControlID="rcbCliente" Font-Bold="true" BackColor="Yellow" />
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
                        </telerik:LayoutColumn>
                        <telerik:LayoutColumn Span="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblDestinazioniMerce" runat="server" Text="Utente finale" AssociatedControlID="rcbIndirizzi" /><br />
                            <telerik:RadComboBox ID="rcbIndirizzi" runat="server"
                                Width="100%" DropDownAutoWidth="Disabled" DropDownWidth="1000"
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
                                OnClientSelectedIndexChanged="rcbIndirizzi_OnClientSelectedIndexChanged"
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
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>


                <%-- Indirizzo --%>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="4" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblIndirizzo" runat="server" Text="Indirizzo" AssociatedControlID="rtbIndirizzo" /><br />
                            <telerik:RadTextBox runat="server" ID="rtbIndirizzo" Width="100%"></telerik:RadTextBox>
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="1" SpanSm="1" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblCAP" runat="server" Text="CAP" AssociatedControlID="rtbCAP" /><br />
                            <telerik:RadTextBox runat="server" ID="rtbCAP" Width="100%"></telerik:RadTextBox>
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="3" SpanSm="4" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblLocalita" runat="server" Text="Localita" AssociatedControlID="rtbLocalita" /><br />
                            <telerik:RadTextBox runat="server" ID="rtbLocalita" Width="100%"></telerik:RadTextBox>
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="1" SpanSm="1" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblProvincia" runat="server" Text="Prov." AssociatedControlID="rtbProvincia" /><br />
                            <telerik:RadTextBox runat="server" ID="rtbProvincia" Width="100%"></telerik:RadTextBox>
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="3" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblTelefono" runat="server" Text="Telefono" AssociatedControlID="rtbTelefono" /><br />
                            <telerik:RadTextBox runat="server" ID="rtbTelefono" Width="100%"></telerik:RadTextBox>
                        </telerik:LayoutColumn>

                    </Columns>
                </telerik:LayoutRow>


                <%-- Dati Progetto --%>
                <telerik:LayoutRow RowType="Region" CssClass="SpaziaturaSuperioreRiga">
                    <Columns>
                        <telerik:LayoutColumn Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="9" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblTitoloProgetto" runat="server" Text="Titolo" Font-Bold="true" AssociatedControlID="rtbTitoloProgetto" />
                            <asp:RequiredFieldValidator ID="rfvTitoloProgetto" runat="server" Display="Dynamic"
                                ControlToValidate="rtbTitoloProgetto"
                                ErrorMessage="Il Titolo è obbligatorio."
                                ForeColor="Red">
                            <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                            </asp:RequiredFieldValidator>
                            <br />
                            <telerik:RadTextBox runat="server" ID="rtbTitoloProgetto" Width="100%"></telerik:RadTextBox>
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>

                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblDescrizioneProgetto" runat="server" Text="Descrizione" AssociatedControlID="reDescrizioneProgetto" /><br />
                            <telerik:RadEditor runat="server" ID="reDescrizioneProgetto" ToolsFile="~/App_Data/Editor/Settings/Tools.xml" ToolbarMode="Default" Width="100%" Height="200px" BorderWidth="1" BorderStyle="Solid" />
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>


                <%-- Attività Progetto --%>
                <telerik:LayoutRow RowType="Region" CssClass="SpaziaturaSuperioreRiga" runat="server" ID="lrAttivita">
                    <Columns>
                        <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                            Attività<br />
                            <uc1:AttivitaProgetto runat="server" ID="ucAttivitaProgetto" />
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>


                <%-- Note Interne / Operatori Cliente --%>
                <telerik:LayoutRow RowType="Region" CssClass="SpaziaturaSuperioreRiga">
                    <Columns>
                        <telerik:LayoutColumn Span="9" CssClass="SpaziaturaSuperioreRiga" runat="server" ID="lcNote">
                            <asp:Label ID="lblNote" runat="server" Text="Note" AssociatedControlID="rtbNote" /><br />
                            <telerik:RadTextBox runat="server" ID="rtbNote" TextMode="MultiLine" Width="100%" Height="125px"></telerik:RadTextBox>
                        </telerik:LayoutColumn>
                        <telerik:LayoutColumn Span="3" CssClass="SpaziaturaSuperioreRiga" runat="server" ID="lcOperatoriCliente">
                            <asp:Label ID="lblOperatoriCliente" runat="server" Text="Operatori cliente" AssociatedControlID="AssociaOperatori" /><br />
                            <uc1:AssociatoreOperatori runat="server" ID="AssociaOperatori"
                                OnAssociaOperatore="AssociaOperatori_AssociaOperatore"
                                OnLeggiOperatori="AssociaOperatori_LeggiOperatori" 
                                OnRimuoviOperatore="AssociaOperatori_RimuoviOperatore" />
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>


                <%-- Allegati --%>
                <telerik:LayoutRow RowType="Region" runat="server" id="lrAllegati">
                    <Columns>
                        <telerik:LayoutColumn Span="12">
                            <telerik:RadAjaxPanel ID="rapAllegati" runat="server" LoadingPanelID="RadAjaxLoadingPanelMaster" OnAjaxRequest="rapAllegati_AjaxRequest" ClientEvents-OnRequestStart="rapAllegati_ClientEvents_OnRequestStart">
                                <asp:Label runat="server" Text="Allegati" /><br />
                                <uc1:DocumentazioneAllegata runat="server" ID="ucDocumentazioneAllegata"
                                    OnNeedSaveAllegato="ucDocumentazioneAllegata_NeedSaveAllegato"
                                    OnNeedDeleteAllegato="ucDocumentazioneAllegata_NeedDeleteAllegato"
                                    OnNeedDataSource="ucDocumentazioneAllegata_NeedDataSource" />
                            </telerik:RadAjaxPanel>
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>

            </Rows>
        </telerik:RadPageLayout>

        <br />
        <br />
    </div>

</asp:Content>
