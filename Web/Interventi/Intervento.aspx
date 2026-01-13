<%@ Page Title="Intervento" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="Intervento.aspx.cs" Inherits="SeCoGEST.Web.Interventi.Intervento" %>

<%@ Register Src="ArticoliIntervento.ascx" TagName="ArticoliIntervento" TagPrefix="uc1" %>
<%@ Register Src="OperatoriIntervento.ascx" TagName="OperatoriIntervento" TagPrefix="uc1" %>
<%@ Register Src="~/UI/DocumentazioneAllegata.ascx" TagName="DocumentazioneAllegata" TagPrefix="uc1" %>
<%@ Register Src="DiscussioneIntervento.ascx" TagName="DiscussioneIntervento" TagPrefix="uc1" %>

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
                    else if (button.get_commandName() == "Chiudi") {
                        messaggioConfermato = confirm('Si desidera Chiudere definitivamente questo Rapporto di Intervento e generare il relativo Documento?\n\nDopo la chiusura i dati non potranno più essere modificati.');
                    }
                    else if (button.get_commandName() == "RiapriIntervento") {
                        messaggioConfermato = confirm('Riaprire questo Rapporto di Intervento?');
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

            var rcbTipologiaInterventoTicket = $find('<%=rcbTipologiaInterventoTicket.ClientID%>');
            rcbTipologiaInterventoTicket.clearSelection();

            __doPostBack("<%=rcbTipologiaInterventoTicket.ClientID %>", "");
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
            var chkVisibileAlCliente = document.getElementById('<%=chkVisibileAlCliente.ClientID%>');
            
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

        // Metodo di gestione dell'evento client OnRequestStart del pannello ajax relativo agli allegati
        function rapAllegati_ClientEvents_OnRequestStart(sender, args) {

            <%--            var hfGrigliaAllegatiCaricata = $get('<%= hfGrigliaAllegatiCaricata.ClientID %>');
            if (hfGrigliaAllegatiCaricata != null && hfGrigliaAllegatiCaricata.value == "1" && args != null) {--%>
            //window.alert('rapAllegati_ClientEvents_OnRequestStart');
                if (args.set_enableAjax) {
                    args.set_enableAjax(false);
                }
            //}
        }



        // Forza la lettura lato server, ad ogni apertura della combo rcbTipologiaInterventoTicket, dei dati relativi agli articoli configurati per l'assistenza
        function rcbTipologiaInterventoTicket_OnClientDropDownOpening(sender, arg) {
            sender.clearItems();
            sender.requestItems("", true);
        }

        // Metodo di gestione dell'evento OnClientItemsRequesting relativo al combo che contiene gli articoli configurati per l'assistenza
        // viene utilizzato per passare parametri alla funzione lato server che seleziona gli articoli da proporre e che sono selezionati in base al cliente selezionato
        function rcbTipologiaInterventoTicket_OnClientItemsRequesting(sender, eventArgs) {
            if (eventArgs == null || !eventArgs.get_context) return;

            var comboSoggetti = $find('<%=rcbCliente.ClientID%>');
            var soggetto = comboSoggetti.get_value();
            var context = eventArgs.get_context();
            //window.alert(soggetto);
            if (context != null) {
                context["ClienteSelezionato"] = soggetto;
            }
        }
        function rcbTipologiaInterventoTicket_OnClientSelectedIndexChanged(sender, eventArgs) {
            var urg = eventArgs.get_item().get_attributes().getAttribute("Urgente");
            var chkUrgente = document.getElementById('<%=chkUrgente.ClientID%>');
            if (urg != null) {
                if (urg == 1) {
                    chkUrgente.checked = true;
                }
            //    else {
            //        chkUrgente.checked = false;
            //    }
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

    </script>

    <style type="text/css">
        .reToolBarWrapper
        { 
             /*Nasconde la toolbar dell'editor*/ 
            display: none; 
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
    <asp:Label ID="lblTitolo" runat="server" Text="Gestione Interventi" />
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


    <%--    <telerik:RadAjaxManagerProxy runat="server" ID="ramIntervento">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lrInfoOperatori">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lrInfoOperatori" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>--%>

    <telerik:RadAjaxManagerProxy runat="server" ID="ramTipo">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbTipologiaInterventoTicket">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pannelloTipologiaInterventoTicket" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                    <telerik:AjaxUpdatedControl ControlID="rcbTipologiaIntervento" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>

    <telerik:RadToolBar ID="RadToolBar1"
        runat="server"
        OnButtonClick="RadToolBar1_ButtonClick"
        OnClientButtonClicking="ToolBarButtonClicking">
        <Items>
            <telerik:RadToolBarButton runat="server" Value="TornaElenco" CommandName="TornaElenco" CausesValidation="False" NavigateUrl="Interventi.aspx" PostBack="False" ImageUrl="~/UI/Images/Toolbar/back.png" Text="Vai all'Elenco" ToolTip="Vai alla pagina di elenco Interventi" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/refresh.png" PostBack="False" Value="Aggiorna" CommandName="Aggiorna" Text="Aggiorna" ToolTip="Aggiorna i dati visualizzati" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" NavigateUrl="Intervento.aspx" ImageUrl="~/UI/Images/Toolbar/add.png" PostBack="False" CommandName="Nuovo" Value="Nuovo" Text="Nuovo Intervento" ToolTip="Permette la creazione di un nuovo Intervento" Target="_blank" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreSalva" />
            <telerik:RadToolBarButton runat="server" CommandName="Salva" ImageUrl="~/UI/Images/Toolbar/Save.png" Text="Salva" Value="Salva" ToolTip="Memorizza i dati visualizzati" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreChiudi" Visible="false" />
            <telerik:RadToolBarButton runat="server" CommandName="Chiudi" ImageUrl="~/UI/Images/Toolbar/basecircle.png" Text="Invia al gestionale" Value="Chiudi" ToolTip="Chiude definitivamente l'intervento." Visible="false" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreGeneraDocumento" />
            <telerik:RadToolBarButton runat="server" CommandName="GeneraDocumento" ImageUrl="~/UI/Images/Toolbar/24x24/print.png" Text="Genera Documento" Value="GeneraDocumento" ToolTip="Effettua la generazione del documento relativo all'intervento." Visible="false" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreRiapriIntervento" />
            <telerik:RadToolBarButton runat="server" CommandName="RiapriIntervento" ImageUrl="~/UI/Images/Toolbar/24x24/basecircle.png" Text="Riapri Intervento" Value="RiapriIntervento" ToolTip="Riapre l'intervento." Visible="false" />
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
                <asp:TextBox runat="server" ID="txtMotivazioneSostituzione" TextMode="MultiLine" Rows="5" Width="100%"></asp:TextBox>
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
                        <telerik:LayoutColumn Span="1" SpanMd="3" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblNumeroIntervento" runat="server" Text="Numero" AssociatedControlID="rntbNumeroIntervento" Font-Bold="true" />&nbsp;
                            <asp:RequiredFieldValidator ID="rfvNumeroIntervento" runat="server" Display="Dynamic"
                                ControlToValidate="rntbNumeroIntervento"
                                ErrorMessage="Il Numero di Intervento è obbligatorio."
                                ForeColor="Red">
                                <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                            </asp:RequiredFieldValidator>
                            <br />
                            <telerik:RadNumericTextBox runat="server" ID="rntbNumeroIntervento" MinValue="1" Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" Width="100%" Font-Bold="true" ReadOnly="true"></telerik:RadNumericTextBox>
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="1" SpanMd="3" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblNumeroCommessa" runat="server" Text="Commessa" AssociatedControlID="rtbNumeroCommessa" /><br />
                            <telerik:RadTextBox runat="server" ID="rtbNumeroCommessa" Width="100%"></telerik:RadTextBox>
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

                        <telerik:LayoutColumn Span="2" SpanMd="3" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Label ID="lblDataPrevistaIntervento" runat="server" Text="Data Intervento" AssociatedControlID="rdtpDataPrevistaIntervento" /><br />
                            <telerik:RadDateTimePicker ID="rdtpDataPrevistaIntervento" Width="100%" runat="server" DateInput-ReadOnly="true">
                                <DatePopupButton Visible="false" />
                                <TimePopupButton Visible="false" />
                            </telerik:RadDateTimePicker>
                        </telerik:LayoutColumn>


                        <telerik:LayoutColumn Span="2" SpanMd="4" SpanSm="6" SpanXs="6" CssClass="SpaziaturaSuperioreRiga" runat="server" ID="lcTicketProvenienza" Visible="false">
                            <asp:Label ID="lblLinkTicketProvenienza" runat="server" Text="Ticket di provenienza" AssociatedControlID="hlLinkTicketProvenienza"/>
                            <asp:HyperLink runat="server" ID="hlLinkTicketProvenienza" Target="_blank" ToolTip="Apri Ticket di provenienza"></asp:HyperLink>
                            <br />
                            <asp:TextBox runat="server" ID="txtMotivazioneSostituzioneTicketProvenienza" TextMode="MultiLine" Rows="3" Width="100%" ReadOnly="true"></asp:TextBox>
                        </telerik:LayoutColumn>
                        <telerik:LayoutColumn Span="2" SpanMd="4" SpanSm="6" SpanXs="6" CssClass="SpaziaturaSuperioreRiga" runat="server" ID="lcTicketSostitutivo" Visible="false">
                            <asp:Label ID="lblLinkTicketSostitutivo" runat="server" Text="Ticket sostitutivo" AssociatedControlID="hlLinkTicketSostitutivo"/>
                            <asp:HyperLink runat="server" ID="hlLinkTicketSostitutivo" Target="_blank" ToolTip="Apri Ticket sostitutivo"></asp:HyperLink>
                            <br />
                            <asp:TextBox runat="server" ID="txtMotivazioneSostituzioneTicketSostitutivo" TextMode="MultiLine" Rows="3" Width="100%" ReadOnly="true"></asp:TextBox>
                        </telerik:LayoutColumn>


                        <telerik:LayoutColumn Span="2" SpanMd="4" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                            Stato<br />
                            <telerik:RadComboBox runat="server" ID="rcbStatoIntervento" Width="100%"
                                DataTextField="Value"
                                DataValueField="Key"
                                HighlightTemplatedItems="true" />
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn>
                            <br />
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


                <%-- Caratteristiche Intervento (Tipologia Ticket) --%>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="12">

                            <div runat="server" id="pannelloTipologiaInterventoTicket">
<%--                            <telerik:RadAjaxPanel runat="server" LoadingPanelID="RadAjaxLoadingPanelMaster">--%>
                                <asp:Label runat="server" ID="lblTipologiaIntervento" Text="Tipologia Ticket" AssociatedControlID="rcbTipologiaInterventoTicket"></asp:Label><br />
                                <telerik:RadComboBox runat="server" ID="rcbTipologiaInterventoTicket" 
                                    EmptyMessage="Selezionare una Tipologia di Intervento fra quelle proposte"
                                    CausesValidation="false" Width="100%"
                                    DataValueField="Id" DataTextField="NomeEstesoTipologiaIntervento"
                                    AutoPostBack="true"
                                    MarkFirstMatch="false"
                                    HighlightTemplatedItems="true"
                                    ItemsPerRequest="200"
                                    ShowMoreResultsBox="true"
                                    EnableLoadOnDemand="true"
                                    EnableItemCaching="false"
                                    AllowCustomText="true"
                                    LoadingMessage="Caricamento in corso..."
                                    OnClientDropDownOpening="rcbTipologiaInterventoTicket_OnClientDropDownOpening"
                                    OnClientItemsRequesting="rcbTipologiaInterventoTicket_OnClientItemsRequesting"
                                    OnClientSelectedIndexChanged="rcbTipologiaInterventoTicket_OnClientSelectedIndexChanged"
                                    OnItemsRequested="rcbTipologiaInterventoTicket_ItemsRequested"
                                    OnSelectedIndexChanged="rcbTipologiaInterventoTicket_SelectedIndexChanged">
                                </telerik:RadComboBox>

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
                                            <strong>Data limite:  <asp:Label runat="server" ID="lblDataLimite"></asp:Label></strong> <asp:Label runat="server" ID="lblTempoNotifica" Text="(la notifica via email verrà inviata un ora prima)"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            <%--</telerik:RadAjaxPanel>--%>
                                </div>
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>


                <%-- Oggetto --%>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="6" SpanMd="6" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreGruppo">
                            <asp:Label ID="lblOggetto" runat="server" Text="Oggetto" AssociatedControlID="rtbOggetto" Font-Bold="true" />&nbsp;
                            <asp:RequiredFieldValidator ID="rfvOggetto" runat="server" Display="Static"
                                ControlToValidate="rtbOggetto"
                                ErrorMessage="L'Oggetto è obbligatorio."
                                ForeColor="Red">
                                <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                            </asp:RequiredFieldValidator>
                            <br />
                            <telerik:RadTextBox runat="server" ID="rtbOggetto" Width="100%"></telerik:RadTextBox>
                        </telerik:LayoutColumn>
                        <telerik:LayoutColumn Span="2" SpanMd="2" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreGruppo">
                            <asp:Label runat="server" Text="Tipologia" AssociatedControlID="rcbTipologiaIntervento" /><br />
                            <telerik:RadComboBox runat="server" ID="rcbTipologiaIntervento" Width="100%"
                                DataTextField="Nome"
                                DataValueField="Id"
                                HighlightTemplatedItems="true"
                                MarkFirstMatch="false"
                                EmptyMessage="Seleziona una tipologia di intervento" />
                        </telerik:LayoutColumn>
                        <telerik:LayoutColumn Span="1" SpanMd="1" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreGruppo">
                            <asp:Label runat="server" Text="Chiamata" AssociatedControlID="chkChiamata" /><br />
                            <asp:CheckBox runat="server" ID="chkChiamata" />
                        </telerik:LayoutColumn>
                        <telerik:CompositeLayoutColumn Span="3" SpanMd="3" SpanSm="12" SpanXs="12">
                            <Rows>
                                <telerik:LayoutRow>
                                    <Columns>
                                        <telerik:LayoutColumn Span="3" SpanMd="3" SpanSm="3" SpanXs="12" CssClass="SpaziaturaSuperioreGruppo">
                                            <asp:Label runat="server" Text="Urgente" ToolTip="Se settato, indica che l'intervento è urgente" AssociatedControlID="chkUrgente" /><br />
                                            <asp:CheckBox runat="server" ID="chkUrgente" />
                                        </telerik:LayoutColumn>
                                        <telerik:LayoutColumn Span="3" SpanMd="3" SpanSm="3" SpanXs="12" CssClass="SpaziaturaSuperioreGruppo">
                                            <asp:Label runat="server" Text="Interno" ToolTip="Se settato, indica che l'intervento è dovuto a delle operazioni interne" AssociatedControlID="chkInterventoInterno" /><br />
                                            <asp:CheckBox runat="server" ID="chkInterventoInterno" />
                                        </telerik:LayoutColumn>
                                        <telerik:LayoutColumn Span="6" SpanMd="6" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreGruppo">
                                            <asp:Label runat="server" Text="Visibile al Cliente" ToolTip="Se settato, indica che il cliente può visualizzare l'intervento nella propria area" AssociatedControlID="chkInterventoInterno" /><br />
                                            <asp:CheckBox runat="server" ID="chkVisibileAlCliente" />
                                        </telerik:LayoutColumn>
                                    </Columns>
                                </telerik:LayoutRow>
                            </Rows>
                        </telerik:CompositeLayoutColumn>
                    </Columns>
                </telerik:LayoutRow>


                <%-- Referente --%>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="6" SpanMd="6" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreGruppo">
                            <asp:Label ID="lblReferenteChiamata" runat="server" Text="Referente" AssociatedControlID="rtbReferenteChiamata" /><br />
                            <telerik:RadTextBox runat="server" ID="rtbReferenteChiamata" Width="100%"></telerik:RadTextBox>
                        </telerik:LayoutColumn>

                        <telerik:LayoutColumn Span="6" SpanMd="6" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreGruppo">
                            <table border="0" style="border: none;width: 100%;">
                                <tr>
                                    <td style="width: 200px;">
                                        <asp:Label ID="lblDataNotifica" runat="server" Text="Promemoria" AssociatedControlID="rdtpDataNotifica" /><br />
                                        <telerik:RadDateTimePicker ID="rdtpDataNotifica" Width="200px" runat="server">
                                            <TimeView runat="server"
                                                ShowHeader="false"
                                                StartTime="07:00:00"
                                                Interval="00:15:00"
                                                EndTime="19:00:00"
                                                Columns="4">
                                            </TimeView>
                                        </telerik:RadDateTimePicker>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTestoNotifica" runat="server" Text="Testo del Promemoria" AssociatedControlID="rdtpDataNotifica" /><br />
                                        <telerik:RadTextBox runat="server" ID="rtbTestoNotifica" Width="100%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>                            
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
                        <telerik:LayoutColumn Span="12">
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
                            <asp:Label ID="lblRichiesteProblematicheRiscontrate" runat="server" Text="Comunica con il cliente" Font-Bold="true" AssociatedControlID="txtRichiesteProblematicheRiscontrate" />&nbsp;
                            <asp:RequiredFieldValidator ID="rfvRichiesteProblematicheRiscontrate" runat="server" Display="Dynamic" ValidationGroup="Invio"
                                ControlToValidate="txtRichiesteProblematicheRiscontrate"
                                ErrorMessage="E' necessario scrivere del testo nel campo 'Comunica con il cliente'."
                                ForeColor="Red">
                                <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                            </asp:RequiredFieldValidator>
                            <telerik:RadTextBox runat="server" ID="txtRichiesteProblematicheRiscontrate" Width="100%" Height="130px" TextMode="MultiLine"></telerik:RadTextBox>
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
                                        <telerik:RadButton runat="server" ID="btSave" Text="Invia risposta" OnClick="btSave_Click" Height="35px" Width="150px" ToolTip="Invia la risposta al cliente." 
                                            CausesValidation="true" ValidateRequestMode="Enabled" ValidationGroup="Invio">
                                            <Icon PrimaryIconUrl="/UI/Images/Toolbar/24x24/send.png" PrimaryIconLeft="5px" PrimaryIconTop="5px" PrimaryIconHeight="24px" PrimaryIconWidth="24px" />
                                        </telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>






                                                




                <%-- Descrizione Intervento \ Voci Predefinite Intervento --%>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="12" style="margin-top: 40px;">
                            <asp:Label ID="lblDefinizione" runat="server" Text="Descrizione Interna Intervento" AssociatedControlID="rtbDefinizione" /><br />
                            <telerik:RadPanelBar runat="server" ID="rpbVociPredefiniteIntervento" Width="100%">
                                <Items>
                                    <telerik:RadPanelItem Text="Voci Predefinite Intervento" Value="RadPanelItemVociPredefiniteIntervento">
                                        <ContentTemplate>
                                            <telerik:RadGrid ID="rgVociPredefiniteIntervento" runat="server" AllowPaging="true"
                                                PageSize="10"
                                                GridLines="None"
                                                SortingSettings-SortToolTip="Clicca qui per ordinare i dati in base a questa colonna"
                                                AutoGenerateColumns="false"
                                                OnItemCommand="rgVociPredefiniteIntervento_ItemCommand"
                                                OnNeedDataSource="rgVociPredefiniteIntervento_NeedDataSource"
                                                OnItemCreated="rgVociPredefiniteIntervento_ItemCreated"
                                                PagerStyle-FirstPageToolTip="Prima Pagina"
                                                PagerStyle-LastPageToolTip="Ultima Pagina"
                                                PagerStyle-PrevPageToolTip="Pagina Precedente"
                                                PagerStyle-NextPageToolTip="Pagina Successiva"
                                                PagerStyle-PagerTextFormat="{4} Elementi da {2} a {3} su {5}, Pagina {0} di {1}">
                                                <MasterTableView DataKeyNames="Descrizione" Width="100%"
                                                    AllowFilteringByColumn="true"
                                                    AllowSorting="true"
                                                    GridLines="Both"
                                                    NoMasterRecordsText="Nessun dato da visualizzare">
                                                    <Columns>
                                                        <telerik:GridBoundColumn HeaderText="Categoria Principale"
                                                            HeaderStyle-Width="150px"
                                                            ItemStyle-Width="150px"
                                                            HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Left"
                                                            ItemStyle-Wrap="true"
                                                            DataField="Categoria"
                                                            Resizable="true"
                                                            FilterControlWidth="95"
                                                            FilterImageToolTip="Applica filtro" />

                                                        <telerik:GridBoundColumn HeaderText="Sottocategoria"
                                                            HeaderStyle-Width="150px"
                                                            ItemStyle-Width="150px"
                                                            HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Left"
                                                            ItemStyle-Wrap="true"
                                                            DataField="SottoCategoria"
                                                            Resizable="true"
                                                            FilterControlWidth="95"
                                                            FilterImageToolTip="Applica filtro" />

                                                        <telerik:GridBoundColumn HeaderText="Breve Descrizione"
                                                            HeaderStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="100%"
                                                            ItemStyle-HorizontalAlign="Left"
                                                            ItemStyle-Wrap="true"
                                                            DataField="Abstract"
                                                            Resizable="false"
                                                            FilterControlWidth="95%"
                                                            FilterImageToolTip="Applica filtro" />

                                                        <telerik:GridButtonColumn HeaderText="Inscrisci Prima" HeaderTooltip="Effettua l'inserimento della descrizione dell'intervento prima di un eventuale testo presente nel campo Descrizione Intervento"
                                                            ButtonType="ImageButton"
                                                            HeaderStyle-Width="50px"
                                                            ItemStyle-Width="50px"
                                                            HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center"
                                                            ImageUrl="/UI/Images/Toolbar/32x32/upload.png"
                                                            Text="Effettua l'inserimento della descrizione dell'intervento prima di un eventuale testo presente nel campo Descrizione Intervento"
                                                            Resizable="false"
                                                            CommandName="InserisciDescrizioneInterventoPrima"
                                                            UniqueName="InserisciDescrizioneInterventoPrima" />

                                                        <telerik:GridButtonColumn HeaderText="Inscrisci Dopo" HeaderTooltip="Effettua l'inserimento della descrizione dell'intervento successivamente ad un eventuale testo presente nel campo Descrizione Intervento"
                                                            ButtonType="ImageButton"
                                                            HeaderStyle-Width="50px"
                                                            ItemStyle-Width="50px"
                                                            HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center"
                                                            ImageUrl="/UI/Images/Toolbar/32x32/download.png"
                                                            Text="Effettua l'inserimento della descrizione dell'intervento successivamente ad un eventuale testo presente nel campo Descrizione Intervento"
                                                            Resizable="false"
                                                            CommandName="InserisciDescrizioneInterventoDopo"
                                                            UniqueName="InserisciDescrizioneInterventoDopo" />
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </ContentTemplate>
                                    </telerik:RadPanelItem>
                                </Items>
                            </telerik:RadPanelBar>
                            <telerik:RadTextBox runat="server" ID="rtbDefinizione" Width="100%" Height="200px" TextMode="MultiLine"></telerik:RadTextBox>
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>


                <%-- Articoli --%>
                <telerik:LayoutRow RowType="Region" runat="server" ID="lrArticoli">
                    <Columns>
                        <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreGruppo">
                            <asp:Label ID="lblArticoliIntervento" runat="server" Text="Articoli" AssociatedControlID="ucArticoliIntervento" /><br />
                            <uc1:ArticoliIntervento runat="server" ID="ucArticoliIntervento" />
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>

                <%-- Operatori --%>
                <telerik:LayoutRow RowType="Region" runat="server" ID="lrInfoOperatori">
                    <Columns>
                        <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreGruppo">
                            <telerik:RadAjaxPanel runat="server" LoadingPanelID="RadAjaxLoadingPanelMaster">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="vertical-align: bottom;">
                                            <asp:Label ID="lblOperatoriIntervento" runat="server" Text="Informazioni Operatore" AssociatedControlID="ucOperatoriIntervento" /><br />
                                        </td>
                                        <td runat="server" id="tdAggiungiOperatori" style="text-align: right;">
                                            <asp:Button runat="server" ID="btAddOperatore" OnClick="btAddOperatore_Click" text="Aggiungi Operatori"/>
                                            <telerik:RadNumericTextBox runat="server" id="rntbNumOperatoriDaAggiungere" NumberFormat-DecimalDigits="0" Width="60px" MinValue="1" MaxValue="20" Value="1" ShowSpinButtons="true" IncrementSettings-Step="1"></telerik:RadNumericTextBox>
                                        </td>
                                    </tr>
                                </table>
                                <uc1:OperatoriIntervento runat="server" ID="ucOperatoriIntervento" />
                            </telerik:RadAjaxPanel>
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>


                <%-- Note interne --%>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreGruppo">
                            <asp:Label ID="lblNote" runat="server" Text="Note interne" AssociatedControlID="rtbNote" /><br />
                            <telerik:RadTextBox runat="server" ID="rtbNote" Width="100%" Height="200px" TextMode="MultiLine"></telerik:RadTextBox>
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>

                <%-- Allegati --%>
                <telerik:LayoutRow RowType="Region" runat="server" id="lrAllegati">
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
                </telerik:LayoutRow>

            </Rows>
        </telerik:RadPageLayout>

        <br />
        <br />
        <br />
    </div>

</asp:Content>
