<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="Ticket.aspx.cs" Inherits="SeCoGEST.Web.Interventi.Ticket" %>
<%@ Register Src="OperatoriIntervento.ascx" TagName="OperatoriIntervento" TagPrefix="uc1" %>
<%@ Register Src="DiscussioneIntervento.ascx" TagName="DiscussioneIntervento" TagPrefix="uc1" %>
<%--<%@ Register Src="~/UI/DocumentazioneAllegata.ascx" TagName="DocumentazioneAllegata" TagPrefix="uc1" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        
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

        .reToolBarWrapper
        { 
             /*Nasconde la toolbar dell'editor*/ 
            display: none; 
        }

        .editorContent {
            padding: 7px;
        }





        .DropZone1 {
            width: 300px;
            height: 90px;
            background-color: #357A2B;
            border-color: #CCCCCC;
            color: #767676;
            float: left;
            text-align: center;
            font-size: 16px;
            color: white;
        }

        #DropZone2 {
            width: 300px;
            height: 90px;
            background-color: #357A2B;
            border-color: #CCCCCC;
            color: #767676;
            float: right;
            text-align: center;
            font-size: 16px;
            color: white;
        }

        .demo-container .RadAsyncUpload {
            text-align: center;
            margin-left: 0;
            margin-bottom: 28px;
        }

            .demo-container .RadAsyncUpload .ruFileWrap {
                text-align: left;
            }

        .demo-container .RadUpload .ruUploadProgress {
            width: 210px;
            display: inline-block;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            vertical-align: top;
        }

        html .demo-container .ruFakeInput {
            width: 200px;
        }

        html .RadUpload .ruFileWrap {
            position: relative;
        }
/*        .DropZone1 {
            width: 100%;
            height: 60px;
            background-color: #357A2B;
            border-color: #CCCCCC;
            color: #767676;
            float: left;
            text-align: center;
            font-size: 16px;
            color: white;
        }

        .fileUploader-container .RadAsyncUpload {
            text-align: center;
            margin-left: 0;
            margin-bottom: 280px;
        }

        .fileUploader-container .RadAsyncUpload .ruFileWrap {
            text-align: left;
        }

        .fileUploader-container .RadUpload .ruUploadProgress {
            width: 210px;
            display: inline-block;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            vertical-align: top;
        }

        html .fileUploader-container .ruFakeInput {
            width: 200px;
        }

        html .RadUpload .ruFileWrap {
            position: relative;
        }
*/    </style>
    
    <script type="text/javascript" src="/UI/js/UploadFiles.js"></script>

    <script type="text/javascript">
        function ToolBarButtonClicking(sender, args) {
            if (args != null && args.get_item) {

                var button = args.get_item();
                if (button != null && button.get_commandName) {
                    var messaggioConfermato = true;
                    if (button.get_commandName() == "Aggiorna") {
                        messaggioConfermato = confirm('Aggiornare i dati visualizzati?\n\nEventuali modifiche apportate ai dati e non salvate verranno perse.');
                    }
                    else if (button.get_commandName() == "Sostituisci") {
                        messaggioConfermato = false;
                        showDialog();
                    }


                    if (!messaggioConfermato) {
                        args.set_cancel(true);
                    }
                }
            }
        }

        // Metodo di gestione dell'evento client OnRequestStart del pannello ajax relativo agli allegati
        function rapAllegati_ClientEvents_OnRequestStart(sender, args) {
            if (args.set_enableAjax) {
                args.set_enableAjax(false);
            }
        }

        function setCustomPosition(sender, args) {
            sender.moveTo(sender.getWindowBounds().x, 280);
        }

        function showDialog() {
            var wnd = $find('<%=modalPopup.ClientID%>');
            wnd.show();
        }
        function closeDialog() {
            var wnd = $find('<%=modalPopup.ClientID%>');
            wnd.close();
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

<%--            var rcbIndirizzi = $find('<%=rcbIndirizzi.ClientID%>');
                    rcbIndirizzi.clearSelection();

                    var rcbTipologiaInterventoTicket = $find('<%=rcbTipologiaInterventoTicket.ClientID%>');
            rcbTipologiaInterventoTicket.clearSelection();

            __doPostBack("<%=rcbTipologiaInterventoTicket.ClientID %>", "");--%>
        }

        function ScriviIndirizzo(Ind, CAP, Loc, Prov, Tel, State, Note, DefaultVisibilitaTicketCliente) {
            var rtbIndirizzo = $find('<%=rtbIndirizzo.ClientID%>');
                    var rtbCAP = $find('<%=rtbCAP.ClientID%>');
                    var rtbLocalita = $find('<%=rtbLocalita.ClientID%>');
                    var rtbProvincia = $find('<%=rtbProvincia.ClientID%>');
                    var rtbTelefono = $find('<%=rtbTelefono.ClientID%>');
<%--            var lblStatoCliente = document.getElementById('<%=lblStatoCliente.ClientID%>');
            var lblNoteCliente = document.getElementById('<%=lblNoteCliente.ClientID%>');
            var lblNoteCliente = document.getElementById('<%=lblNoteCliente.ClientID%>');
            var chkVisibileAlCliente = document.getElementById('<%=chkVisibileAlCliente.ClientID%>');--%>

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

            chkVisibileAlCliente.checked = false;
            if (DefaultVisibilitaTicketCliente == "true") {
                chkVisibileAlCliente.checked = true;
            }
        }






        // Metodo di gestione dell'evento OnClientItemsRequesting relativo al combo che contiene le Destinazioni da selezionare
        // viene utilizzato per passare parametri alla funzione lato server che seleziona le Destinazioni da proporre in base al Cliente selezionato
        function rcbIndirizzi_OnClientItemsRequesting(sender, eventArgs) {
            if (eventArgs == null || !eventArgs.get_context) return;

            var hdCodiceCliente = $find('<%=hfCodiceCliente.ClientID%>');
            var codiceCliente = comboCliente.value;
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
            var DefaultVisibilitaTicketCliente = eventArgs.get_item().get_attributes().getAttribute("DefaultVisibilitaTicketCliente");
            ScriviIndirizzo(Ind, CAP, Loc, Prov, Tel, "X", DefaultVisibilitaTicketCliente);
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    <asp:Label ID="lblTitolo" runat="server" Text="Gestione Ticket" />
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
        OnClientButtonClicking="ToolBarButtonClicking"
        OnButtonClick="RadToolBar1_ButtonClick">
        <Items>
            <telerik:RadToolBarButton runat="server" Value="TornaElenco" CommandName="TornaElenco" CausesValidation="False" NavigateUrl="Tickets.aspx" PostBack="False" ImageUrl="~/UI/Images/Toolbar/back.png" Text="Vai all'Elenco" ToolTip="Vai alla pagina di elenco dei Ticket" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/refresh.png" PostBack="False" Value="Aggiorna" CommandName="Aggiorna" Text="Aggiorna" ToolTip="Aggiorna i dati visualizzati" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True"/>
            <telerik:RadToolBarButton runat="server" CausesValidation="False" NavigateUrl="Ticket.aspx" ImageUrl="~/UI/Images/Toolbar/add.png" PostBack="False" CommandName="Nuovo" Value="Nuovo" Text="Nuovo Ticket" ToolTip="Permette la creazione di un nuovo Ticket" Target="_blank" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreSalva" />
            <telerik:RadToolBarButton runat="server" CommandName="Salva" ImageUrl="~/UI/Images/Toolbar/24x24/send.png" Text="Invia richiesta" Value="Salva" ToolTip="Invia la richiesta al servizio di supporto tecnico." />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreSostituisci" />
            <telerik:RadToolBarButton runat="server" CommandName="Sostituisci" ImageUrl="~/UI/Images/Toolbar/CheckBoxes.png" Text="Chiudi e Sostituisci" Value="Sostituisci" ToolTip="Chiude il ticket corrente e ne apre uno sostitutivo" />
        </Items>
    </telerik:RadToolBar>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server"
        CssClass="ValidationSummaryStyle"
        HeaderText="&nbsp;Errori di validazione dei dati:"
        DisplayMode="BulletList"
        ShowMessageBox="false"
        ShowSummary="true" />

    <telerik:RadWindow RenderMode="Lightweight" ID="modalPopup" runat="server" Width="450px" Height="250px" Modal="true" Behaviors="Close,Move" OffsetElementID="main" OnClientShow="setCustomPosition" Style="z-index: 100001;">
        <ContentTemplate>
            <p style="text-align: center;">
                Indicare la motivazione della richiesta di sostituzione del ticket:
            </p>
            <div style="width=100%; padding: 0px 15px 15px 15px;">
                <asp:TextBox runat="server" ID="txtModitvazioneSostituzione" TextMode="MultiLine" Rows="5" Width="100%"></asp:TextBox>
            </div>
            <div style="padding-left: 15px;">
                <asp:Button runat="server" ID="btSostituisci" Text="Chiudi e Sostituisci Ticket" OnClientClick="return confirm('Chiudere questo ticket ed aprirne uno nuovo sostitutivo?')" OnClick="btSostituisci_Click"/>&nbsp;&nbsp;&nbsp;
                <asp:Button runat="server" ID="btAnnullaSostituzione" Text="Annulla" OnClientClick="closeDialog();return false;" />
            </div>
        </ContentTemplate>
    </telerik:RadWindow>

    <div class="pageBody">

        <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
            <Rows>

                <%-- Numero e Data Intervento --%>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="2" SpanMd="4" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblNumeroIntervento" runat="server" Text="Numero Intervento" AssociatedControlID="rntbNumeroIntervento" Font-Bold="true"/>&nbsp;
                            <asp:RequiredFieldValidator ID="rfvNumeroIntervento" runat="server" Display="Dynamic"
                                ControlToValidate="rntbNumeroIntervento"
                                ErrorMessage="Il Numero di Intervento è obbligatorio." 
                                ForeColor="Red">
                                <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                            </asp:RequiredFieldValidator>
                            <br />
                            <telerik:RadNumericTextBox runat="server" ID="rntbNumeroIntervento" MinValue="1" Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" Width="100%" Font-Bold="true" ReadOnly="true"></telerik:RadNumericTextBox>
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="2" SpanMd="4" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblDataRedazione" runat="server" Text="Data Redazione" AssociatedControlID="rdtpDataRedazione" Font-Bold="true"/>&nbsp;
                            <asp:RequiredFieldValidator ID="rfvDataRedazione" runat="server" Display="Dynamic"
                                ControlToValidate="rdtpDataRedazione"
                                ErrorMessage="La Data di Redazione è obbligatoria." 
                                ForeColor="Red">
                                <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                            </asp:RequiredFieldValidator><br />
                            <telerik:RadDateTimePicker ID="rdtpDataRedazione" Width="100%" runat="server"
                                DateInput-ReadOnly="true"
                                TimePopupButton-Visible="false"
                                DatePopupButton-Visible="false">
                                <TimeView runat="server"
                                    ShowHeader="false"
                                    StartTime="07:00:00"
                                    Interval="00:15:00"
                                    EndTime="19:00:00"
                                    Columns="4">
                                </TimeView>
                            </telerik:RadDateTimePicker>
                        </telerik:LayoutColumn>
                        
                        <telerik:LayoutColumn Span="2" SpanMd="4" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblStato" runat="server" Text="Stato" AssociatedControlID="rtbStato"/><br />
                            <telerik:RadTextBox ID="rtbStato" runat="server" ReadOnly="true" Width="100%" />
                        </telerik:LayoutColumn>


                        <telerik:LayoutColumn Span="3" SpanMd="6" SpanSm="6" SpanXs="6" CssClass="SpaziaturaSuperioreRiga" runat="server" ID="lcTicketProvenienza" Visible="false">
                            <asp:Label ID="lblLinkTicketProvenienza" runat="server" Text="Ticket di provenienza" AssociatedControlID="hlLinkTicketProvenienza"/>
                            <asp:HyperLink runat="server" ID="hlLinkTicketProvenienza" Target="_blank" ToolTip="Apri Ticket di provenienza"></asp:HyperLink>
                            <br />
                            <asp:TextBox runat="server" ID="txtMotivazioneSostituzioneTicketProvenienza" TextMode="MultiLine" Rows="3" Width="100%" ReadOnly="true"></asp:TextBox>
                            <%--<div style="width:100%; border: solid 1px #687DC1; background-color: white; padding: 5px;"></div>--%>
                        </telerik:LayoutColumn>
                        <telerik:LayoutColumn Span="3" SpanMd="6" SpanSm="6" SpanXs="6" CssClass="SpaziaturaSuperioreRiga" runat="server" ID="lcTicketSostitutivo" Visible="false">
                            <asp:Label ID="lblLinkTicketSostitutivo" runat="server" Text="Ticket sostitutivo" AssociatedControlID="hlLinkTicketSostitutivo"/>
                            <asp:HyperLink runat="server" ID="hlLinkTicketSostitutivo" Target="_blank" ToolTip="Apri Ticket sostitutivo"></asp:HyperLink>
                            <br />
                            <asp:TextBox runat="server" ID="txtMotivazioneSostituzioneTicketSostitutivo" TextMode="MultiLine" Rows="3" Width="100%" ReadOnly="true"></asp:TextBox>
                            <%--<div style="width:100%; border: solid 1px #687DC1; background-color: white; padding: 5px;"></div>--%>
                        </telerik:LayoutColumn>

                    </Columns>
                </telerik:LayoutRow>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn >
                            <br />
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>


                <%-- Cliente --%>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="12" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblCliente" runat="server" Text="Ragione Sociale" AssociatedControlID="rtbRagioneSocialeCliente" Font-Bold="true"/><br/>
<%--                            <telerik:RadComboBox ID="rcbCliente" runat="server" Visible="false"
                                Width="100%" DropDownAutoWidth="Disabled"
                                HighlightTemplatedItems="true"
                                DataValueField="CODICE"
                                DataTextField="RAGIONESOCIALE"
                                OnClientSelectedIndexChanged="rcbCliente_OnClientSelectedIndexChanged">
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
                            </telerik:RadComboBox>--%>

                            <telerik:RadTextBox ID="rtbRagioneSocialeCliente" runat="server" Width="100%" ReadOnly="true"/>
                            <asp:HiddenField ID="hfCodiceCliente" runat="server" />

                            <telerik:RadTextBox ID="rtbDestinazione" runat="server" Width="100%" ReadOnly="true" Visible="false"/>
                            <asp:HiddenField ID="hfIdDestinazione" runat="server" />

                            <telerik:RadComboBox ID="rcbIndirizzi" runat="server" Visible="false"
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
                                OnClientItemsRequesting="rcbIndirizzi_OnClientItemsRequesting"
                                OnClientSelectedIndexChanged="rcbIndirizzi_OnClientSelectedIndexChanged">
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
                            <telerik:RadTextBox runat="server" ID="rtbIndirizzo" Width="100%" ReadOnly="true"></telerik:RadTextBox>
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="1" SpanSm="1" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblCAP" runat="server" Text="CAP" AssociatedControlID="rtbCAP" /><br />
                            <telerik:RadTextBox runat="server" ID="rtbCAP" Width="100%" ReadOnly="true"></telerik:RadTextBox>
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="3" SpanSm="4" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblLocalita" runat="server" Text="Localita" AssociatedControlID="rtbLocalita" /><br />
                            <telerik:RadTextBox runat="server" ID="rtbLocalita" Width="100%" ReadOnly="true"></telerik:RadTextBox>
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="1" SpanSm="1" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblProvincia" runat="server" Text="Prov." AssociatedControlID="rtbProvincia" /><br />
                            <telerik:RadTextBox runat="server" ID="rtbProvincia" Width="100%" ReadOnly="true"></telerik:RadTextBox>
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="3" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblTelefono" runat="server" Text="Telefono" AssociatedControlID="rtbTelefono" /><br />
                            <telerik:RadTextBox runat="server" ID="rtbTelefono" Width="100%" ReadOnly="true"></telerik:RadTextBox>
                        </telerik:LayoutColumn>

                    </Columns>
                </telerik:LayoutRow>



                <%-- Caratteristiche Intervento (Tipologia Ticket) --%>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="12">

                            <telerik:RadAjaxPanel runat="server" LoadingPanelID="RadAjaxLoadingPanelMaster">
                                <asp:Label runat="server" ID="lblTipologiaIntervento" Text="Tipologia Intervento" AssociatedControlID="rcbTipologiaIntervento"></asp:Label><br />
                                <telerik:RadComboBox runat="server" ID="rcbTipologiaIntervento" 
                                    EmptyMessage="Selezionare una Tipologia di Intervento fra quelle proposte"
                                    CausesValidation="false" Width="100%"
                                    DataValueField="Id" DataTextField="NomeEstesoTipologiaIntervento"
                                    AutoPostBack="true"
                                    OnSelectedIndexChanged="rcbTipologiaIntervento_SelectedIndexChanged">
                                </telerik:RadComboBox>
                                <telerik:RadTextBox runat="server" ID="rtbTipologiaIntervento" Width="100%" ReadOnly="true" Visible="false"></telerik:RadTextBox>

                                <table style="width: 100%;" border="1" runat="server" id="tableTicketConfigurationInfo" visible="false">
                                    <tr>
                                        <td style="background-color: yellow; vertical-align: top; width: 50%; padding: 5px;">
                                            <asp:Label runat="server" ID="lblUfficioOrariReparto"></asp:Label>
                                        </td>
                                        <td style="background-color: orange; vertical-align: top; width: 50%; padding: 5px;">
                                            <asp:Label runat="server" ID="lblCaratteristicheTipologiaIntervento"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="padding: 7px; background-color: white;">
                                            <strong>Data limite:  <asp:Label runat="server" ID="lblDataLimite"></asp:Label></strong> (la notifica via email verrà inviata un ora prima)
                                        </td>
                                    </tr>
                                </table>
                            </telerik:RadAjaxPanel>

                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>



                <%-- Oggetto --%>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="12" SpanMd="12" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreGruppo">
                            <asp:Label ID="lblOggetto" runat="server" Text="Oggetto" AssociatedControlID="rtbOggetto" Font-Bold="true" />&nbsp;
                            <asp:RequiredFieldValidator ID="rfvOggetto" runat="server" Display="Static"
                                ControlToValidate="rtbOggetto"
                                ErrorMessage="Il campo 'Oggetto' è obbligatorio." 
                                ForeColor="Red">
                                <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                            </asp:RequiredFieldValidator>
                            <br />
                            <telerik:RadTextBox runat="server" ID="rtbOggetto" Width="100%"></telerik:RadTextBox>
                       </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>





                <%-- Richieste / Risposte (chat) --%>
                <telerik:LayoutRow RowType="Region" runat="server" ID="lrPrimaRichiesta" Visible="false">
                    <Columns>
                        <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreGruppo">
                            <uc1:DiscussioneIntervento runat="server" id="diPrimaRichiesta"></uc1:DiscussioneIntervento>
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>


                <telerik:LayoutRow RowType="Region" runat="server" ID="lrDiscussioni">
                    <Columns>
                        <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreGruppo">
                            <asp:Repeater runat="server" ID="repDiscussioni" OnItemDataBound="repDiscussioni_ItemDataBound">
                                <ItemTemplate>
                                    <uc1:DiscussioneIntervento runat="server" id="diDiscussione"></uc1:DiscussioneIntervento>
                                </ItemTemplate>
                            </asp:Repeater>
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>

                <telerik:LayoutRow RowType="Region" runat="server" ID="lrRichiesteProblematicheRiscontrate" Visible="false">
                    <Columns>
                        <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreGruppo">
                            <asp:Label ID="lblRichiesteProblematicheRiscontrate" runat="server" Text="Comunica con l’assistenza" Font-Bold="true" AssociatedControlID="txtRichiesteProblematicheRiscontrate" />&nbsp;
                            <asp:RequiredFieldValidator ID="rfvRichiesteProblematicheRiscontrate" runat="server" Display="Dynamic"
                                ControlToValidate="txtRichiesteProblematicheRiscontrate"
                                ErrorMessage="E' necessario indicare i dettagli della richiesta nel campo 'Comunica con l’assistenza'."
                                ForeColor="Red">
                                <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                            </asp:RequiredFieldValidator>
                            <br />
                            <telerik:RadTextBox runat="server" ID="txtRichiesteProblematicheRiscontrate" Width="100%" Height="200px" TextMode="MultiLine"></telerik:RadTextBox>
                        </telerik:LayoutColumn>
                        <telerik:LayoutColumn Span="12">
                            <table style="width: 100%">
                                <tr>
                                    <td style="vertical-align:top; padding-top: 7px;">
                                        <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="RadAsyncUpload1" MultipleFileSelection="Automatic" />
                                        I files allegati vengono memorizzati al salvataggio e solamente se viene scritto del testo nella casella delle Richieste.
                                    </td>
                                    <td style="vertical-align:top; text-align: right; padding-top: 4px;">
                                        <asp:Repeater runat="server" ID="repAllegati" OnItemCommand="repAllegati_ItemCommand">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lbAllegato" CausesValidation="false"
                                                    CommandArgument='<%#Eval("ID") %>' CommandName="Download">
                                                        <span style="white-space: nowrap;"><img src="/UI/Images/Menu/download.png" width="16" style="margin-left: 10px;margin-right: 3px;" /><%#Eval("NomeFile")%></span>
                                                </asp:LinkButton>&nbsp;
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </td>
                                    <td style="width:100px;">
                                        <telerik:RadButton runat="server" ID="btSave" Text="Invia richiesta" OnClick="btSave_Click" Height="35px" Width="150px" ToolTip="Invia la richiesta al servizio di supporto tecnico.">
                                            <Icon PrimaryIconUrl="/UI/Images/Toolbar/24x24/send.png" PrimaryIconLeft="5px" PrimaryIconTop="5px" PrimaryIconHeight="24px" PrimaryIconWidth="24px" />
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                        </telerik:LayoutColumn>

                    </Columns>
                </telerik:LayoutRow>





            <%--<telerik:LayoutRow RowType="Region" runat="server">
                    <Columns>
                        <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreGruppo">
                            <div class="demo-container size-wide" style="text-align: right;">
                                    <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="RadAsyncUpload1" MultipleFileSelection="Automatic" DropZones=".DropZone1,#DropZone2" style="text-align: right;" />
 
                                    <div class="DropZone1">
                                        <p>Custom Drop Zone</p>
                                        <p>Drop Files Here</p>
                                    </div>
                                    <div id="DropZone2">
                                        <p>Custom Drop Zone</p>
                                        <p>Drop Files Here</p>
                                    </div>
                                </div>
                            <table width="100%">
                                <tr>
                                    <td style="vertical-align: top;">
                                        <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="RadAsyncUpload1" MultipleFileSelection="Automatic" DropZones=".DropZone1#DropZone2" />
                                    </td>
                                    <td style="vertical-align: top; width: 100%; padding-left: 10px;">
                                        <div id="DropZone1" class="DropZone1">
                                            <p>Trascina qui eventuali files da allegare</p>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>--%>


                <%-- Descrizione Intervento --%>
                <telerik:LayoutRow RowType="Region" runat="server" ID="lrDescrizioneIntervento" Visible="false">
                    <Columns>
                        <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreGruppo">
                            <asp:Label ID="lblDefinizione" runat="server" Text="Descrizione Finale Intervento" AssociatedControlID="rtbDefinizione" /><br />
                            <telerik:RadTextBox runat="server" ID="rtbDefinizione" Width="100%" Height="200px" TextMode="MultiLine" ReadOnly="true"></telerik:RadTextBox>
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>


                <%-- Operatori --%>
                <telerik:LayoutRow RowType="Region" runat="server" ID="lrInfoOperatori" Visible="false">
                    <Columns>
                        <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreGruppo">
                            <asp:Label ID="lblOperatoriIntervento" runat="server" Text="Informazioni Operatore" AssociatedControlID="ucOperatoriIntervento" /><br />
                            <uc1:OperatoriIntervento runat="server" ID="ucOperatoriIntervento" Enabled="false" NoteVisibili="false" />
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>


<%--                <telerik:LayoutRow RowType="Region" runat="server" id="lrAllegati">
                    <Columns>
                        <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreGruppo">
                            <telerik:RadAjaxPanel ID="rapAllegati" runat="server" LoadingPanelID="RadAjaxLoadingPanelMaster" OnAjaxRequest="rapAllegati_AjaxRequest" ClientEvents-OnRequestStart="rapAllegati_ClientEvents_OnRequestStart">
                                <asp:Label runat="server" Text="Allegati" /><br />
                                <uc1:DocumentazioneAllegata runat="server" ID="ucDocumentazioneAllegata"
                                    OnNeedSaveAllegato="ucDocumentazioneAllegata_NeedSaveAllegato"
                                    OnNeedDeleteAllegato="ucDocumentazioneAllegata_NeedDeleteAllegato"
                                    OnNeedDataSource="ucDocumentazioneAllegata_NeedDataSource" />
                            </telerik:RadAjaxPanel>
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>--%>

            </Rows>
        </telerik:RadPageLayout>

        <br />
        <br />
        <br />
    </div>

    <telerik:RadScriptBlock runat="server">
        <script type="text/javascript">
            //<![CDATA[
            Sys.Application.add_load(function () {
                demo.initialize();
            });
            //Sys.Application.add_load(function () {
            //    fileUploader.initialize();
            //});
            //]]>
        </script>
    </telerik:RadScriptBlock>

</asp:Content>
