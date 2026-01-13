<%@ Page Title="Dettagli Offerta" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="DettagliOfferta.aspx.cs" Inherits="SeCoGEST.Web.Offerte.DettagliOfferta" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Offerte/OffertaRaggruppamentoHeader.ascx" TagPrefix="uc1" TagName="OffertaRaggruppamentoHeader" %>
<%@ Register Src="~/Offerte/OffertaRaggruppamentoEditItem.ascx" TagPrefix="uc1" TagName="OffertaRaggruppamentoEditItem" %>
<%@ Register Src="~/UI/DocumentazioneAllegata.ascx" TagPrefix="uc1" TagName="DocumentazioneAllegata" %>
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
                    else if (button.get_commandName() == "RichiediValidazione") {
                        messaggioConfermato = confirm('Si desidera richiedere la validazione dell\'offerta corrente?\n\nPrima di confermare questo messaggio verifica che le eventuali modifiche apportate ai dati siano state salvate.');
                    }
                    else if (button.get_commandName() == "ConfermaOfferta") {
                        messaggioConfermato = confirm('Si desidera confermare l\'offerta corrente?');
                    }
                    else if (button.get_commandName() == "RifiutoOfferta") {
                        messaggioConfermato = confirm('Si desidera rifiutare l\'offerta corrente?');
                    }
                    else if (button.get_commandName() == "InviaOffertaAlCliente") {
                        messaggioConfermato = confirm('Si desidera inviare al cliente l\'offerta corrente?');
                    }
                    else if (button.get_commandName() == "ConfermaOffertaDaParteDelCliente") {
                        messaggioConfermato = confirm('Si desidera memorizzare la conferma dell\'offerta corrente da parte del cliente?');
                    }
                    else if (button.get_commandName() == "RifiutoOffertaDaParteDelCliente") {
                        messaggioConfermato = confirm('Si desidera memorizzare il rifiuto dell\'offerta corrente da parte del cliente?');
                    }
                    else if (button.get_commandName() == "CreaRevisione") {
                        messaggioConfermato = confirm('Si desidera creare una revisione dell\'offerta corrente?\n\nPrima di confermare questo messaggio verifica che le eventuali modifiche apportate ai dati siano state salvate.');
                    }
                    else if (button.get_commandName() == "ClonaOfferta") {
                        messaggioConfermato = confirm('Si desidera clonare l\'offerta corrente?\n\nPrima di confermare questo messaggio verifica che le eventuali modifiche apportate ai dati siano state salvate.');
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
            ScriviIndirizzo(Ind, CAP, Loc, Prov, Tel, State, Note);

            var rcbIndirizzi = $find('<%=rcbIndirizzi.ClientID%>');
            rcbIndirizzi.clearSelection();

            var rapMetodiDiPagamento = $find("<%=rapMetodiDiPagamento.ClientID%>");
            if (rapMetodiDiPagamento != null) {
                rapMetodiDiPagamento.ajaxRequest("<%=COMANDO_AGGIORNAMENTO_METODI_PAGAMENTO%>");
            }
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
            ScriviIndirizzo(Ind, CAP, Loc, Prov, Tel, "X");
        }

        function ScriviIndirizzo(Ind, CAP, Loc, Prov, Tel, State, Note) {
            var rtbIndirizzo = $find('<%=rtbIndirizzo.ClientID%>');
            var rtbCAP = $find('<%=rtbCAP.ClientID%>');
            var rtbLocalita = $find('<%=rtbLocalita.ClientID%>');
            var rtbProvincia = $find('<%=rtbProvincia.ClientID%>');
            var rtbTelefono = $find('<%=rtbTelefono.ClientID%>');
            var lblStatoCliente = document.getElementById('<%=lblStatoCliente.ClientID%>');
            var lblNoteCliente = document.getElementById('<%=lblNoteCliente.ClientID%>');

            if (Ind != null && Ind != "") rtbIndirizzo.set_value(Ind);
            if (CAP != null && CAP != "") rtbCAP.set_value(CAP);
            if (Loc != null && Loc != "") rtbLocalita.set_value(Loc);
            if (Prov != null && Prov != "") rtbProvincia.set_value(Prov);
            if (Tel != null && Tel != "") rtbTelefono.set_value(Tel);
            if (Note != null && Note != "") lblNoteCliente.innerHTML = Note;

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

        function rntbTotaleGruppo_OnValueChanged(sender, args) {
            try {

                var contenitoreTastiHtml = sender.get_element().parentNode.parentNode.parentNode;
                var contenitoreTasti = $(contenitoreTastiHtml);
                var elementoTotaleGruppoCostoId = contenitoreTasti.find("input.TotaleGruppoCosto").attr("id");
                var elementoTotaleGruppoVenditaId = contenitoreTasti.find("input.TotaleGruppoVendita").attr("id");
                var elementoTotaleGruppoRedditivitaId = contenitoreTasti.find("input.TotaleGruppoRedditivita").attr("id");

                var controlloTotaleGruppoCosto = $telerik.findControl(contenitoreTastiHtml, elementoTotaleGruppoCostoId);
                var controlloTotaleGruppoVendita = $telerik.findControl(contenitoreTastiHtml, elementoTotaleGruppoVenditaId);
                var controlloTotaleGruppoReddivita = $telerik.findControl(contenitoreTastiHtml, elementoTotaleGruppoRedditivitaId);

                CalcolaTotaliTotaleGruppo(controlloTotaleGruppoCosto, controlloTotaleGruppoVendita, controlloTotaleGruppoReddivita);

            } catch (e) {
                alert(e.message);
            }
        }

        function CalcolaTotaliTotaleGruppo(rntbTotaleCosto, rntbTotaleVendita, rntbTotaleRedditivita) {

            if (rntbTotaleCosto != null && rntbTotaleVendita != null && rntbTotaleRedditivita != null) {
                var totaleCosto = rntbTotaleCosto.get_value();
                var totaleVendite = rntbTotaleVendita.get_value();

                var incremento = 0;
                var aumento = 0;

                if (totaleCosto != null && totaleCosto > 0 &&
                    totaleVendite != null && totaleVendite > 0) {

                    incremento = totaleVendite - totaleCosto;
                    aumento = (incremento / totaleCosto) * 100;
                }

                rntbTotaleRedditivita.set_value(incremento);
                //rntbTotaleRedditivita.set_textBoxValue(incremento.toString() + " € (" + aumento.toFixed(2) + "%)");                
            }
        }

        function RichiestaConfermaAggiungiNuovoRaggruppamento() {
            return confirm('Si desidera aggiungere un nuovo gruppo?');
        }

        function RichiestaConfermaClonazioneRaggruppamento() {
            return confirm('Si desidera effettuare la clonazione del gruppo?');
        }

        function rntbTotaleGlobale_OnValueChanged() {
            var controlloTotaleGruppoCosto = $find('<%=rntbTotaleCostoGlobale.ClientID%>');
            var controlloTotaleGruppoVendita = $find('<%=rntbTotaleVenditaGlobale.ClientID%>');
            var controlloTotaleGruppoReddivita = $find('<%=rntbTotaleRedditivitaGlobale.ClientID%>');

            CalcolaTotaliTotaleGruppo(controlloTotaleGruppoCosto, controlloTotaleGruppoVendita, controlloTotaleGruppoReddivita);
        }

        function rwInvioOffertaAlCliente_OnClientShow(sender, args) {
            // Codice necessario per far funzionare il RadEditor in una finestra RadWindow
            $find("<%=reTestoEmailDaInviareAlCliente.ClientID %>").onParentNodeChanged();
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
    <asp:Label ID="lblTitolo" runat="server" Text="Dettagli Offerta" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">

    <telerik:RadToolBar ID="RadToolBar1"
        runat="server"
        OnButtonClick="RadToolBar1_ButtonClick"
        OnClientButtonClicking="ToolBarButtonClicking">
        <Items>
            <telerik:RadToolBarButton runat="server" Value="TornaElenco" CommandName="TornaElenco" CausesValidation="False" NavigateUrl="ElencoOfferte.aspx" PostBack="False" ImageUrl="~/UI/Images/Toolbar/back.png" Text="Vai all'Elenco" ToolTip="Vai alla pagina di elenco Offerte" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" ImageUrl="~/UI/Images/Toolbar/refresh.png" PostBack="False" Value="Aggiorna" CommandName="Aggiorna" Text="Aggiorna" ToolTip="Aggiorna i dati visualizzati" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreNuovo" />
            <telerik:RadToolBarButton runat="server" CausesValidation="False" NavigateUrl="DettagliOfferta.aspx" ImageUrl="~/UI/Images/Toolbar/add.png" PostBack="False" CommandName="Nuovo" Value="Nuovo" Text="Nuova Offerta" ToolTip="Permette la creazione di una nuova Offerta" Target="_blank" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreSalva" />
            <telerik:RadToolBarButton runat="server" CommandName="Salva" ImageUrl="~/UI/Images/Toolbar/Save.png" Text="Salva" Value="Salva" ToolTip="Memorizza i dati visualizzati" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreRichiestaConferma" Visible="false" />
            <telerik:RadToolBarButton runat="server" CommandName="RichiediValidazione" ImageUrl="~/UI/Images/Toolbar/Send.png" Text="Richiedi Validazione" Value="RichiediValidazione" ToolTip="Effettua la richiesta di validazione dell'offerta" PostBack="true" Visible="false" />
            <telerik:RadToolBarButton runat="server" CommandName="ConfermaOfferta" ImageUrl="~/UI/Images/Toolbar/check.png" Text="Conferma Offerta" Value="ConfermaOfferta" ToolTip="Effettua la conferma dell'offerta" PostBack="true" Visible="false" />
            <telerik:RadToolBarButton runat="server" CommandName="RifiutoOfferta" ImageUrl="~/UI/Images/Toolbar/cancel.png" Text="Rifiuta Offerta" Value="RifiutoOfferta" ToolTip="Effettua il rifiuto dell'offerta" PostBack="true" Visible="false" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreEsportaArticoli" />
            <telerik:RadToolBarButton runat="server" CommandName="EsportaArticoli" ImageUrl="~/UI/Images/Toolbar/excel.png" Value="EsportaArticoli" Text="Esporta Articoli" ToolTip="Effettua l'esportazione degli articoli relativi all'offerta" PostBack="true" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreGeneraDocumenti" Visible="false" />
            <telerik:RadToolBarDropDown runat="server" CommandName="GeneraDocumento" ImageUrl="~/UI/Images/Toolbar/download.png" Text="Genera Documento" ToolTip="Effettua la generazione del documento relativo all'offerta" PostBack="true" Visible="false" />
            <telerik:RadToolBarButton runat="server" CommandName="CaricaDocumento" ImageUrl="~/UI/Images/Toolbar/upload.png" Text="Carica Documento" Value="CaricaDocumento" ToolTip="Permette di caricare il documento dell'offerta corretto" PostBack="true" Visible="false" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreInvioOffertaAlCliente" Visible="false" />
            <telerik:RadToolBarButton runat="server" CommandName="InviaOffertaAlCliente" ImageUrl="~/UI/Images/Toolbar/Send.png" Text="Invia offerta al cliente" Value="InviaOffertaAlCliente" ToolTip="Permette la selezione e l'invio di una offerta caricata" PostBack="true" Visible="false" />
            <telerik:RadToolBarButton runat="server" CommandName="ConfermaOffertaDaParteDelCliente" ImageUrl="~/UI/Images/Toolbar/check.png" Text="Offerta Confermata dal cliente" Value="ConfermaOffertaDaParteDelCliente" ToolTip="Effettua il settaggio dell'offerta come accettata da parte del cliente" PostBack="true" Visible="false" />
            <telerik:RadToolBarButton runat="server" CommandName="RifiutoOffertaDaParteDelCliente" ImageUrl="~/UI/Images/Toolbar/cancel.png" Text="Offerta Rifiuta dal cliente" Value="RifiutoOffertaDaParteDelCliente" ToolTip="Effettua il settaggio dell'offerta come rifiutata da parte del cliente" PostBack="true" Visible="false" />
            <telerik:RadToolBarButton runat="server" IsSeparator="True" Value="SeparatoreCreaRevisione" Visible="false" />
            <telerik:RadToolBarButton runat="server" CommandName="CreaRevisione" ImageUrl="~/UI/Images/Toolbar/CheckBoxes.png" Text="Crea Revisione" Value="CreaRevisione" ToolTip="Permette la creazione di una revisione dell'offerta" PostBack="true" Visible="false" CausesValidation="false" />
            <telerik:RadToolBarButton runat="server" CommandName="ClonaOfferta" ImageUrl="~/UI/Images/Toolbar/clone.png" Text="Clona Offerta" Value="ClonaOfferta" ToolTip="Permette di clonare l'offerta" PostBack="true" Visible="false" CausesValidation="false" />
        </Items>
    </telerik:RadToolBar>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server"
        CssClass="ValidationSummaryStyle"
        HeaderText="&nbsp;Errori di validazione dei dati:"
        DisplayMode="BulletList"
        ShowMessageBox="false"
        ShowSummary="true" />

    <div class="pageBody">

        <div style="margin-bottom: 10px; border: 1px solid;">
            <uc1:PageMessage runat="server" ID="PageMessage" FrameStyle="Note" Visible="false" />
        </div>

        <telerik:RadTabStrip runat="server" ID="rtsOfferta" MultiPageID="rmpOfferta" SelectedIndex="0" CausesValidation="false">
            <Tabs>
                <telerik:RadTab Text="Dati Offerta" Value="DatiOfferta"></telerik:RadTab>
                <telerik:RadTab Text="Testi per generazione documenti" Value="TestiDocumenti"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage runat="server" ID="rmpOfferta" SelectedIndex="0">
            <telerik:RadPageView runat="server" ID="rpvDatiOfferta">
                <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                    <Rows>
                        <telerik:LayoutRow RowType="Region">
                            <Columns>
                                <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="1" SpanMd="1" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:Label ID="lblNumero" runat="server" Text="Numero" AssociatedControlID="rntbNumero" Font-Bold="true" /><br />
                                    <telerik:RadNumericTextBox runat="server" ID="rntbNumero" MinValue="1" Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" Width="100%" Font-Bold="true" ReadOnly="true"></telerik:RadNumericTextBox>
                                </telerik:LayoutColumn>

                                <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="1" SpanMd="1" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:Label ID="lblNumeroRevisione" runat="server" Text="Revisione" AssociatedControlID="rtbNumeroRevisione" /><br />
                                    <telerik:RadTextBox runat="server" ID="rtbNumeroRevisione" Width="100%" ReadOnly="true"></telerik:RadTextBox>
                                </telerik:LayoutColumn>

                                <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
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

                                <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="1" SpanMd="1" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <telerik:RadLabel ID="lblGiorniValiditaOfferta" runat="server" Text="Giorni Validità" AssociatedControlID="rntbGrioniValidita" />
                                    <br />
                                    <telerik:RadNumericTextBox runat="server" ID="rntbGrioniValidita" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                                </telerik:LayoutColumn>      
                                
                                <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <telerik:RadLabel ID="lblTipologiaGiorniValidita" runat="server" Text="Tipologia Giorni Validità" AssociatedControlID="rcbTipologiaGiorniValidita" />
                                    <br />
                                    <telerik:RadComboBox ID="rcbTipologiaGiorniValidita" runat="server" Width="100%" AllowCustomText="false" DataTextField="Value" DataValueField="Key"/>
                                </telerik:LayoutColumn>

                                <telerik:LayoutColumn Span="1" SpanXl="1" SpanLg="1" SpanMd="1" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <telerik:RadLabel ID="lblTempiDiConsegna" runat="server" Text="Tempi di consegna" AssociatedControlID="rntbTempiDiConsegna" />
                                    <br />
                                    <telerik:RadNumericTextBox runat="server" ID="rntbTempiDiConsegna" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                                </telerik:LayoutColumn>

                                <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <telerik:RadLabel ID="lblTipologiaTempiDiConsegna" runat="server" Text="Tipologia Tempi di consegna" AssociatedControlID="rcbTipologiaTempiDiConsegna" />
                                    <br />
                                    <telerik:RadComboBox ID="rcbTipologiaTempiDiConsegna" runat="server" Width="100%" AllowCustomText="false" DataTextField="Value" DataValueField="Key"/>
                                </telerik:LayoutColumn>

                                <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:Label ID="lblCodiceCommessa" runat="server" Text="Codice Commessa" AssociatedControlID="rtbCodiceCommessa" />
                                    <br />
                                    <telerik:RadTextBox runat="server" ID="rtbCodiceCommessa" Width="100%" MaxLength="50"></telerik:RadTextBox>
                                </telerik:LayoutColumn>
                            </Columns>
                        </telerik:LayoutRow>

                        <telerik:LayoutRow RowType="Region">
                            <Columns>
                                <telerik:LayoutColumn Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:Label ID="lblTitoloOfferta" runat="server" Text="Titolo" Font-Bold="true" AssociatedControlID="rtbTitoloOfferta" />&nbsp;
                                    <asp:RequiredFieldValidator ID="rfvTitoloOfferta" runat="server" Display="Dynamic"
                                        ControlToValidate="rtbTitoloOfferta"
                                        ErrorMessage="Il Titolo del documento è obbligatorio."
                                        ForeColor="Red">
                                        <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                    </asp:RequiredFieldValidator>
                                    <br />
                                    <telerik:RadTextBox runat="server" ID="rtbTitoloOfferta" Width="100%" MaxLength="50"></telerik:RadTextBox>
                                </telerik:LayoutColumn>
                            </Columns>
                        </telerik:LayoutRow>

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
                                    <asp:Label ID="lblDestinazioniMerce" runat="server" Text="Destinazione Merce" AssociatedControlID="rcbIndirizzi" /><br />
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

                        <telerik:LayoutRow RowType="Region">
                            <Columns>
                                <telerik:LayoutColumn Span="4" SpanSm="6" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:Label ID="lblIndirizzo" runat="server" Text="Indirizzo" AssociatedControlID="rtbIndirizzo" /><br />
                                    <telerik:RadTextBox runat="server" ID="rtbIndirizzo" Width="100%" MaxLength="80"></telerik:RadTextBox>
                                </telerik:LayoutColumn>

                                <telerik:LayoutColumn Span="1" SpanSm="1" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:Label ID="lblCAP" runat="server" Text="CAP" AssociatedControlID="rtbCAP" /><br />
                                    <telerik:RadTextBox runat="server" ID="rtbCAP" Width="100%" MaxLength="8"></telerik:RadTextBox>
                                </telerik:LayoutColumn>

                                <telerik:LayoutColumn Span="3" SpanSm="4" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:Label ID="lblLocalita" runat="server" Text="Localita" AssociatedControlID="rtbLocalita" /><br />
                                    <telerik:RadTextBox runat="server" ID="rtbLocalita" Width="100%" MaxLength="80"></telerik:RadTextBox>
                                </telerik:LayoutColumn>

                                <telerik:LayoutColumn Span="1" SpanSm="1" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:Label ID="lblProvincia" runat="server" Text="Prov." AssociatedControlID="rtbProvincia" /><br />
                                    <telerik:RadTextBox runat="server" ID="rtbProvincia" Width="100%" MaxLength="4"></telerik:RadTextBox>
                                </telerik:LayoutColumn>

                                <telerik:LayoutColumn Span="3" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:Label ID="lblTelefono" runat="server" Text="Telefono" AssociatedControlID="rtbTelefono" /><br />
                                    <telerik:RadTextBox runat="server" ID="rtbTelefono" Width="100%" MaxLength="24"></telerik:RadTextBox>
                                </telerik:LayoutColumn>

                            </Columns>
                        </telerik:LayoutRow>

                        <telerik:LayoutRow RowType="Region">
                            <Columns>
                                <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:Label ID="lblNoteInterne" runat="server" Text="Note Interne" AssociatedControlID="rtbNoteInterne" /><br />
                                    <telerik:RadTextBox runat="server" ID="rtbNoteInterne" Width="100%" TextMode="MultiLine" Rows="4"></telerik:RadTextBox>
                                </telerik:LayoutColumn>
                            </Columns>
                        </telerik:LayoutRow>

                        <telerik:LayoutRow RowType="Region">
                            <Content>
                                <telerik:RadAjaxPanel ID="rapMetodiDiPagamento" runat="server" OnAjaxRequest="rapMetodiDiPagamento_AjaxRequest">
                                    <telerik:LayoutRow ID="RigaMetodiPagamento" runat="server" RowType="Region">
                                        <Columns>
                                            <telerik:LayoutColumn Span="6" SpanXl="6" SpanLg="6" SpanMd="6" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                                <asp:Label ID="lblMetodiDiPagamento" runat="server" Text="Metodologie di pagamento" Font-Bold="true" AssociatedControlID="rcbMetodiDiPagamento" />&nbsp;
                                        <asp:RequiredFieldValidator ID="rfvMetodiDiPagamento" runat="server" Display="Dynamic" Enabled="false"
                                            ControlToValidate="rcbMetodiDiPagamento"
                                            ErrorMessage="Il metodo di pagamento è obbligatorio."
                                            ForeColor="Red">
                                            <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                        </asp:RequiredFieldValidator>
                                                <br />
                                                <telerik:RadComboBox runat="server" ID="rcbMetodiDiPagamento" Width="100%" DataTextField="DESCRIZIONE" DataValueField="CODICE" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="rcbMetodiDiPagamento_SelectedIndexChanged" />
                                            </telerik:LayoutColumn>
                                            <telerik:LayoutColumn ID="ColonnaIban" runat="server" Span="6" SpanXl="6" SpanLg="6" SpanMd="6" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                                <asp:Label ID="lblIban" runat="server" Text="Codice IBAN" AssociatedControlID="rcbCodiceIban" />&nbsp;
                                        <br />
                                                <telerik:RadComboBox runat="server" ID="rcbCodiceIban" Width="100%" DataTextField="CODICEIBAN" DataValueField="CODICEIBAN" AllowCustomText="true" Filter="Contains" MaxLength="34" />
                                            </telerik:LayoutColumn>
                                        </Columns>
                                    </telerik:LayoutRow>
                                </telerik:RadAjaxPanel>
                            </Content>

                        </telerik:LayoutRow>

                        <telerik:LayoutRow ID="rigaTotaliGlobali" runat="server" RowType="Region">
                            <Rows>
                                <telerik:LayoutRow RowType="Region">
                                    <Columns>
                                        <telerik:LayoutColumn CssClass="SpaziaturaSuperioreGruppo" Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12">
                                            <hr />
                                            <h2>TOTALI GLOBALI</h2>
                                        </telerik:LayoutColumn>
                                    </Columns>
                                </telerik:LayoutRow>
                                <telerik:LayoutRow RowType="Region">
                                    <Columns>
                                        <telerik:LayoutColumn Span="6" SpanXl="6" SpanLg="6" SpanMd="6" SpanSm="12" SpanXs="12" CssClass="">
                                            &nbsp;
                                        </telerik:LayoutColumn>
                                        <telerik:LayoutColumn runat="server" CssClass="" Span="2" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12">
                                            <telerik:RadLabel ID="lblTotaleCostoGlobaleCalcolato" runat="server" Text="Totale Costo Calcolato" AssociatedControlID="rntbTotaleCostoGlobaleCalcolato" />
                                            <br />
                                            <telerik:RadNumericTextBox runat="server" ID="rntbTotaleCostoGlobaleCalcolato" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" DisplayText='<%#String.Format("{0:c}", Eval("TotaleCosto")) %>' Value='<%# (double?)(decimal?)Eval("TotaleCosto") %>' ReadOnly="true" />
                                        </telerik:LayoutColumn>
                                        <telerik:LayoutColumn runat="server" CssClass="" Span="2" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12">
                                            <telerik:RadLabel ID="lblTotaleVenditaGlobaleCalcolato" runat="server" Text="Totale Vendita Calcolato" AssociatedControlID="rntbTotaleVenditaGlobaleCalcolato" />
                                            <br />
                                            <telerik:RadNumericTextBox runat="server" ID="rntbTotaleVenditaGlobaleCalcolato" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" ReadOnly="true" />
                                        </telerik:LayoutColumn>
                                        <telerik:LayoutColumn runat="server" CssClass="" Span="2" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12">
                                            <telerik:RadLabel ID="lblTotaleRedditivitaGlobaleCalcolato" runat="server" Text="Totale Redditività Calcolato" AssociatedControlID="rntbTotaleRedditivitaGlobaleCalcolato" />
                                            <br />
                                            <telerik:RadNumericTextBox runat="server" ID="rntbTotaleRedditivitaGlobaleCalcolato" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" ReadOnly="true" />
                                        </telerik:LayoutColumn>
                                    </Columns>
                                </telerik:LayoutRow>
                                <telerik:LayoutRow RowType="Region">
                                    <Columns>
                                        <telerik:LayoutColumn Span="6" SpanXl="6" SpanLg="6" SpanMd="6" SpanSm="12" SpanXs="12" CssClass="SpaziaturaSuperioreRiga">
                                            &nbsp;
                                        </telerik:LayoutColumn>
                                        <telerik:LayoutColumn ID="ColonnaTotaleCostoGlobale" runat="server" CssClass="SpaziaturaSuperioreRiga" Span="2" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12">
                                            <telerik:RadLabel ID="lblTotaleCostoGlobale" runat="server" Text="Totale Costo" AssociatedControlID="rntbTotaleCostoGlobale" Font-Bold="true" />
                                            <asp:RequiredFieldValidator ID="rfvTotaleCostoGlobale" runat="server" Display="Dynamic"
                                                ControlToValidate="rntbTotaleCostoGlobale"
                                                ErrorMessage="Il totale globale dei costi è obbligatorio."
                                                ForeColor="Red">
                                        <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                            </asp:RequiredFieldValidator>
                                            <br />
                                            <telerik:RadNumericTextBox runat="server" ID="rntbTotaleCostoGlobale" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" DisplayText='<%#String.Format("{0:c}", Eval("TotaleCosto")) %>' Value='<%# (double?)(decimal?)Eval("TotaleCosto") %>'>
                                                <ClientEvents OnValueChanged="rntbTotaleGlobale_OnValueChanged" />
                                            </telerik:RadNumericTextBox>
                                        </telerik:LayoutColumn>
                                        <telerik:LayoutColumn ID="ColonnaTotaleVenditaGlobale" runat="server" CssClass="SpaziaturaSuperioreRiga" Span="2" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12">
                                            <telerik:RadLabel ID="lblTotaleVenditaGlobale" runat="server" Text="Totale Vendita" AssociatedControlID="rntbTotaleVenditaGlobale" Font-Bold="true" />
                                            <asp:RequiredFieldValidator ID="rfvTotaleVenditaGlobale" runat="server" Display="Dynamic"
                                                ControlToValidate="rntbTotaleVenditaGlobale"
                                                ErrorMessage="Il totale globale della vendita è obbligatorio."
                                                ForeColor="Red">
                                        <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                            </asp:RequiredFieldValidator>
                                            <br />
                                            <telerik:RadNumericTextBox runat="server" ID="rntbTotaleVenditaGlobale" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2">
                                                <ClientEvents OnValueChanged="rntbTotaleGlobale_OnValueChanged" />
                                            </telerik:RadNumericTextBox>
                                        </telerik:LayoutColumn>
                                        <telerik:LayoutColumn ID="ColonnaTotaleRedditivitaGlobale" runat="server" CssClass="SpaziaturaSuperioreRiga" Span="2" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12">
                                            <telerik:RadLabel ID="lblTotaleRedditivitaGlobale" runat="server" Text="Totale Redditività" AssociatedControlID="rntbTotaleRedditivitaGlobale" Font-Bold="true" />
                                            <br />
                                            <telerik:RadNumericTextBox runat="server" ID="rntbTotaleRedditivitaGlobale" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" ReadOnly="true"></telerik:RadNumericTextBox>
                                        </telerik:LayoutColumn>
                                    </Columns>
                                </telerik:LayoutRow>
                            </Rows>
                        </telerik:LayoutRow>

                        <telerik:LayoutRow ID="rowDocumentiCaricati" runat="server" RowType="Region" Visible="false">
                            <Columns>
                                <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                                    <asp:Label runat="server" Text="Documenti Caricati" /><br />
                                    <uc1:DocumentazioneAllegata runat="server" ID="ucDocumentazioneCaricata"
                                        OnNeedDataSource="ucDocumentazioneCaricata_NeedDataSource"
                                        OnNeedDeleteAllegato="ucDocumentazioneCaricata_NeedDeleteAllegato" />
                                </telerik:LayoutColumn>
                            </Columns>
                        </telerik:LayoutRow>

                        <telerik:LayoutRow runat="server" ID="rowRaggruppamenti" RowType="Region" Visible="false">
                            <Columns>
                                <telerik:LayoutColumn CssClass="SpaziaturaSuperioreGruppo">

                                    <p style="margin-bottom: 15px;">
                                        <asp:LinkButton ID="lbAggiungiNuovoRaggruppamento" runat="server" Text="Aggiungi Nuovo Gruppo" CausesValidation="false" OnClientClick="if(!RichiestaConfermaAggiungiNuovoRaggruppamento()) return false;" OnClick="lbAggiungiNuovoRaggruppamento_Click" />
                                    </p>

                                    <asp:Repeater runat="server" ID="repRaggruppamenti" OnItemCommand="repRaggruppamenti_ItemCommand" OnItemDataBound="repRaggruppamenti_ItemDataBound">
                                        <ItemTemplate>

                                            <asp:HiddenField runat="server" ID="hdIdArticoloInEdit" />
                                            <asp:HiddenField runat="server" ID="hdIdRaggruppamento" />

                                            <asp:Panel runat="server" ID="panelHeader" BorderStyle="None">
                                                <uc1:OffertaRaggruppamentoHeader runat="server" ID="HeaderRaggruppamento" OnGruppoAdded="HeaderRaggruppamento_Command" OnGruppoDeleted="HeaderRaggruppamento_Command"></uc1:OffertaRaggruppamentoHeader>
                                            </asp:Panel>
                                            <asp:Panel runat="server" ID="panelContent" BackColor="white" Style="padding-left: 10px; padding-right: 10px; padding-top: 10px; padding-bottom: 00px; overflow-x: hidden;">

                                                <asp:Button runat="server" ID="btAggiungiArticolo" Text="Aggiungi articolo..." Visible="true" CausesValidation="false" CommandName="AggiungiArticolo" />
                                                <asp:Button runat="server" ID="btSalvaArticolo" Text="Salva" Visible="false" CommandName="SalvaArticolo" />
                                                <asp:Button runat="server" ID="btAggiornaArticolo" Text="Salva" Visible="false" CommandName="AggiornaArticolo" />
                                                <asp:Button runat="server" ID="btAnnullaEdit" Text="Annulla" Visible="false" CausesValidation="false" CommandName="AnnullaEdit" />
                                                <asp:Button runat="server" ID="btClonaGruppo" Text="Clona gruppo..." Visible="true" CausesValidation="false" CommandName="ClonaGruppo" OnClientClick="if (!RichiestaConfermaClonazioneRaggruppamento()) return false;" OnClick="btClonaGruppo_Click" />

                                                <br />

                                                <uc1:OffertaRaggruppamentoEditItem runat="server" ID="EditItemRaggruppamento" Visible="false"></uc1:OffertaRaggruppamentoEditItem>

                                                <asp:Panel ID="pnlContenitoreMessaggi" runat="server" Style="padding: 15px; margin-top: 25px; background-color: #D9F0F7;">
                                                    <h3 style="text-align: center;">AVVISI AUTOMATICI</h3>
                                                    <asp:Repeater ID="rptMessaggiAutomatici" runat="server">
                                                        <ItemTemplate>
                                                            <div style="padding-bottom: 5px; padding-top: 5px; border-bottom: 1px solid #666;"><%# Container.DataItem %></div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                    <p>
                                                        <asp:Button ID="btnChiudiMessaggioAvvisiAutomatici" runat="server" Text="Proseguire chiudendo il messaggio" OnClick="btnChiudiMessaggio_Click" CausesValidation="false" />&nbsp;
                                                    </p>
                                                </asp:Panel>

                                                <asp:Panel ID="pnlContenitoreArticoliAutomatici" runat="server" Style="padding: 15px; margin-top: 25px; background-color: #D9F0F7;">
                                                    <h3 style="text-align: center;">ARTICOLI CHE VERRANNO AGGIUNTI AUTOMATICAMENTE</h3>
                                                    <p>
                                                        Di seguito sono riportati gli articoli che verranno aggiunti automaticamente al gruppo di articoli corrente alla pressione del tasto "Inserisci Articoli"<br />
                                                        Nel caso in cui di decidesse di effettuare l'inserimento degli articoli proposti, premere il pulsante "Proseguire senza inserire gli articoli".
                                                    </p>
                                                    <asp:Repeater ID="rptArticoliAutomatici" runat="server">
                                                        <HeaderTemplate>
                                                            <ul>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <li><%# Container.DataItem %></li>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </ul>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                    <p>
                                                        <asp:Button ID="btnAggiungiArticoliAutomaticiAlGruppo" runat="server" Text="Inserisci Articoli" OnClick="btnAggiungiArticoliAutomaticiAlGruppo_Click" ValidationGroup="ArticoliAutomatici" CausesValidation="false" />&nbsp;
                                                        <asp:Button ID="btnProcediSenzaInserireArticoliAutomaticiAlGruppo" runat="server" Text="Proseguire senza inserire gli articoli" OnClick="btnProcediSenzaInserireArticoliAutomaticiAlGruppo_Click" ValidationGroup="ArticoliAutomatici" CausesValidation="false" />&nbsp;
                                                        <asp:Button ID="btnChiudiMessaggio" runat="server" Text="Proseguire chiudendo il messaggio" OnClick="btnChiudiMessaggio_Click" ValidationGroup="ArticoliAutomatici" CausesValidation="false" />&nbsp;
                                                    </p>
                                                </asp:Panel>

                                                <br />
                                                <telerik:RadGrid runat="server"
                                                    ID="rgGrigliaArticoli"
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

                                                            <telerik:GridBoundColumn SortExpression="CodiceGruppo" HeaderText="Gruppo" HeaderButtonType="TextButton"
                                                                DataField="TABGRUPPI.DESCRIZIONE">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn SortExpression="CodiceCategoria" HeaderText="Categoria" HeaderButtonType="TextButton"
                                                                DataField="TABCATEGORIE.DESCRIZIONE">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn SortExpression="CodiceCategoriaStatistica" HeaderText="Categoria Statistica" HeaderButtonType="TextButton"
                                                                DataField="TABCATEGORIESTAT.DESCRIZIONE">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn SortExpression="Descrizione" HeaderText="Descrizione" HeaderButtonType="TextButton"
                                                                DataField="Descrizione">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn SortExpression="UnitaMisura" HeaderText="UM" HeaderButtonType="TextButton"
                                                                DataField="UnitaMisura" HeaderStyle-Width="50px">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn SortExpression="Quantita" HeaderText="Qta" HeaderButtonType="TextButton"
                                                                DataField="Quantita" DataFormatString="{0:N0}" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn SortExpression="TotaleCosto" HeaderText="Tot. Costo" HeaderButtonType="TextButton"
                                                                DataField="TotaleCosto" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                                                            </telerik:GridBoundColumn>                                                            
                                                            <telerik:GridBoundColumn SortExpression="TotaleRicarico" HeaderText="Tot. Ricarico" HeaderButtonType="TextButton"
                                                                DataField="TotaleRicarico" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn SortExpression="RicaricoPercentuale" HeaderText="% Ricarico" HeaderButtonType="TextButton"
                                                                DataField="RicaricoPercentuale" DataFormatString="{0:F2} %" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn SortExpression="TotaleSpesa" HeaderText="Tot. Spese" HeaderButtonType="TextButton"
                                                                DataField="TotaleSpesa" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                                                            </telerik:GridBoundColumn>
                                                            <telerik:GridBoundColumn SortExpression="TotaleVenditaConSpesa" HeaderText="Tot. Vendita" HeaderButtonType="TextButton"
                                                                DataField="TotaleVenditaConSpesa" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px">
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridButtonColumn HeaderStyle-Width="40" UniqueName="ColonnaOrdinamentoSpostaSopra"
                                                                Resizable="false"
                                                                HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-Wrap="true"
                                                                ItemStyle-HorizontalAlign="Center"
                                                                ButtonType="ImageButton"
                                                                ImageUrl="/UI/Images/Toolbar/16x16/ArrowUp.png"
                                                                Text="Sposta verso l'alto..."
                                                                CommandName="MoveUp" />

                                                            <telerik:GridButtonColumn HeaderStyle-Width="40" UniqueName="ColonnaOrdinamentoSpostaSotto"
                                                                Resizable="false"
                                                                HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-Wrap="true"
                                                                ItemStyle-HorizontalAlign="Center"
                                                                ButtonType="ImageButton"
                                                                ImageUrl="/UI/Images/Toolbar/16x16/ArrowDown.png"
                                                                Text="Sposta verso il basso..."
                                                                CommandName="MoveDown" />

                                                            <telerik:GridButtonColumn HeaderStyle-Width="40" UniqueName="ColonnaCopia"
                                                                Resizable="false"
                                                                HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-Wrap="true"
                                                                ItemStyle-HorizontalAlign="Center"
                                                                ConfirmText="Clona l'articolo selezionato?"
                                                                ConfirmTextFields="Descrizione"
                                                                ConfirmTextFormatString="Clonare l'articolo '{0}'?"
                                                                ConfirmDialogType="RadWindow"
                                                                ConfirmTitle=""
                                                                ButtonType="ImageButton"
                                                                ImageUrl="/UI/Images/Toolbar/16x16/clone.png"
                                                                Text="Clona..."
                                                                CommandName="Clona" />

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
                                                
                                                <telerik:LayoutRow ID="rigaTotaliOpzioni" runat="server" RowType="Region" Visible="True">
                                                    <Columns>
                                                        <telerik:LayoutColumn Span="6" SpanXl="6" SpanLg="6" SpanMd="6" SpanSm="12" SpanXs="12">
                                                            <telerik:LayoutRow ID="rigaOpzioniStampa" runat="server" RowType="Region" Visible="True">
                                                                <Rows>
                                                                    <telerik:LayoutRow RowType="Region">
                                                                        <Columns>
                                                                            <telerik:LayoutColumn CssClass="SpaziaturaSuperioreGruppo" Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12">
                                                                                <hr />
                                                                                <h2>OPZIONI STAMPA NELLA OFFERTA</h2>
                                                                            </telerik:LayoutColumn>
                                                                        </Columns>
                                                                    </telerik:LayoutRow>

                                                                    <telerik:LayoutRow RowType="Region">
                                                                        <Columns>
                                                                            <telerik:LayoutColumn CssClass="SpaziaturaSuperioreGruppo" Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12">
                                                                                <telerik:RadCheckBoxList ID="rcblOpzioniGruppo" runat="server" Columns="2" Direction="Vertical" Layout="Flow" ValidationGroup="OpzioniGruppo" AutoPostBack="false">
                                                                                    <DataBindings DataSelectedField="Selected" DataTextField="Text" DataValueField="Value" />
                                                                                </telerik:RadCheckBoxList>
                                                                            </telerik:LayoutColumn>
                                                                        </Columns>
                                                                    </telerik:LayoutRow>

                                                                    <telerik:LayoutRow RowType="Region">
                                                                        <Columns>
                                                                            <telerik:LayoutColumn CssClass="SpaziaturaSuperioreGruppo" Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12">
                                                                                <br />
                                                                                <asp:Button runat="server" ID="btSalvaModificheOpzioniGruppo" Text="Salva" Visible="true" CommandName="SalvaModificheOpzioniGruppo" ValidationGroup="OpzioniGruppo" OnClick="btSalvaModificheOpzioniGruppo_Click" />
                                                                                <asp:Button runat="server" ID="btAnnullaModificheOpzioniGruppo" Text="Ricarica Valori" Visible="true" CommandName="AnnullaModificheOpzioniGruppo" ValidationGroup="OpzioniGruppo" CausesValidation="false" OnClick="btAnnullaModificheOpzioniGruppo_Click" />
                                                                            </telerik:LayoutColumn>
                                                                        </Columns>
                                                                    </telerik:LayoutRow>
                                                                </Rows>
                                                            </telerik:LayoutRow>
                                                        </telerik:LayoutColumn>

                                                        <telerik:LayoutColumn Span="6" SpanXl="6" SpanLg="6" SpanMd="6" SpanSm="12" SpanXs="12">
                                                            <telerik:LayoutRow ID="rigaTotaliGruppo" runat="server" RowType="Region" Visible="True">
                                                                <Rows>
                                                                    <telerik:LayoutRow RowType="Region">
                                                                        <Columns>
                                                                            <telerik:LayoutColumn CssClass="SpaziaturaSuperioreGruppo" Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12">
                                                                                <hr />
                                                                                <h2>TOTALI GRUPPO</h2>
                                                                            </telerik:LayoutColumn>
                                                                        </Columns>
                                                                    </telerik:LayoutRow>
                                                                    <telerik:LayoutRow RowType="Region">
                                                                        <Columns>
                                                                            <telerik:LayoutColumn CssClass="" Span="4" SpanXl="4" SpanLg="4" SpanMd="4" SpanSm="12" SpanXs="12">
                                                                                <telerik:RadLabel ID="lblTotaleCostoCalcolato" runat="server" Text="Totale Costo Calcolato" AssociatedControlID="rntbTotaleCostoCalcolato" />
                                                                                <br />
                                                                                <telerik:RadNumericTextBox runat="server" ID="rntbTotaleCostoCalcolato" CssClass="TotaleGruppoCosto" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" ValidationGroup="TotaliGruppo" ReadOnly="true" />
                                                                            </telerik:LayoutColumn>
                                                                            <telerik:LayoutColumn CssClass="" Span="4" SpanXl="4" SpanLg="4" SpanMd="4" SpanSm="12" SpanXs="12">
                                                                                <telerik:RadLabel ID="lblTotaleVenditaCalcolato" runat="server" Text="Totale Vendita Calcolato" AssociatedControlID="rntbTotaleVenditaCalcolato" />
                                                                                <br />
                                                                                <telerik:RadNumericTextBox runat="server" ID="rntbTotaleVenditaCalcolato" Width="100%" CssClass="TotaleGruppoVendita" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" ValidationGroup="TotaliGruppo" ReadOnly="true" />
                                                                            </telerik:LayoutColumn>
                                                                            <telerik:LayoutColumn CssClass="" Span="4" SpanXl="4" SpanLg="4" SpanMd="4" SpanSm="12" SpanXs="12">
                                                                                <telerik:RadLabel ID="lblTotaleRedditivitaCalcolato" runat="server" Text="Totale Redditività Calcolato" AssociatedControlID="rntbTotaleRedditivitaCalcolato" />
                                                                                <br />
                                                                                <telerik:RadNumericTextBox runat="server" ID="rntbTotaleRedditivitaCalcolato" Width="100%" CssClass="TotaleGruppoRedditivita" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" ValidationGroup="TotaliGruppo" ReadOnly="true" />
                                                                            </telerik:LayoutColumn>
                                                                        </Columns>
                                                                    </telerik:LayoutRow>
                                                                    <telerik:LayoutRow RowType="Region">
                                                                        <Columns>                                                                            
                                                                            <telerik:LayoutColumn CssClass="SpaziaturaSuperioreGruppo" Span="4" SpanXl="4" SpanLg="4" SpanMd="4" SpanSm="12" SpanXs="12">
                                                                                <telerik:RadLabel ID="lblTotaleCosto" runat="server" Text="Totale Costo" Font-Bold="true" AssociatedControlID="rntbTotaleCosto" />
                                                                                <asp:RequiredFieldValidator ID="rfvTotaleCosto" runat="server" Display="Dynamic" ValidationGroup="TotaliGruppo"
                                                                                    ControlToValidate="rntbTotaleCosto"
                                                                                    ErrorMessage="Il totale dei costi è obbligatorio."
                                                                                    ForeColor="Red">
                                                                            <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                                                                </asp:RequiredFieldValidator>
                                                                                <br />
                                                                                <telerik:RadNumericTextBox runat="server" ID="rntbTotaleCosto" CssClass="TotaleGruppoCosto" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" ValidationGroup="TotaliGruppo">
                                                                                    <ClientEvents OnValueChanged="rntbTotaleGruppo_OnValueChanged" />
                                                                                </telerik:RadNumericTextBox>
                                                                            </telerik:LayoutColumn>
                                                                            <telerik:LayoutColumn CssClass="SpaziaturaSuperioreGruppo" Span="4" SpanXl="4" SpanLg="4" SpanMd="4" SpanSm="12" SpanXs="12">
                                                                                <telerik:RadLabel ID="lblTotaleVendita" runat="server" Text="Totale Vendita" Font-Bold="true" AssociatedControlID="rntbTotaleVendita" />
                                                                                <asp:RequiredFieldValidator ID="rfvTotaleVendite" runat="server" Display="Dynamic" ValidationGroup="TotaliGruppo"
                                                                                    ControlToValidate="rntbTotaleVendita"
                                                                                    ErrorMessage="Il totale della vendita è obbligatorio."
                                                                                    ForeColor="Red">
                                                                            <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                                                                </asp:RequiredFieldValidator>
                                                                                <br />
                                                                                <telerik:RadNumericTextBox runat="server" ID="rntbTotaleVendita" Width="100%" CssClass="TotaleGruppoVendita" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" ValidationGroup="TotaliGruppo">
                                                                                    <ClientEvents OnValueChanged="rntbTotaleGruppo_OnValueChanged" />
                                                                                </telerik:RadNumericTextBox>
                                                                            </telerik:LayoutColumn>
                                                                            <telerik:LayoutColumn CssClass="SpaziaturaSuperioreGruppo" Span="4" SpanXl="4" SpanLg="4" SpanMd="4" SpanSm="12" SpanXs="12">
                                                                                <telerik:RadLabel ID="lblTotaleRedditivita" runat="server" Text="Totale Redditività" Font-Bold="true" AssociatedControlID="rntbTotaleRedditivita" />
                                                                                <br />
                                                                                <telerik:RadNumericTextBox runat="server" ID="rntbTotaleRedditivita" Width="100%" CssClass="TotaleGruppoRedditivita" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2" ValidationGroup="TotaliGruppo" ReadOnly="true" />
                                                                            </telerik:LayoutColumn>
                                                                        </Columns>
                                                                    </telerik:LayoutRow>
                                                                    <telerik:LayoutRow RowType="Region">
                                                                        <Columns>
                                                                            <telerik:LayoutColumn CssClass="SpaziaturaSuperioreGruppo AllineaDestra" Span="12" SpanXl="12" SpanLg="12" SpanMd="12" SpanSm="12" SpanXs="12">
                                                                                <br />
                                                                                <asp:Button runat="server" ID="btSalvaModificheGruppo" Text="Salva" Visible="true" CommandName="SalvaModificheGruppo" ValidationGroup="TotaliGruppo" OnClick="btSalvaModificheGruppo_Click" />
                                                                                <asp:Button runat="server" ID="btAnnullaModificheGruppo" Text="Ricarica Valori" Visible="true" CommandName="AnnullaModificheGruppo" ValidationGroup="TotaliGruppo" CausesValidation="false" OnClick="btAnnullaModificheGruppo_Click" />
                                                                            </telerik:LayoutColumn>
                                                                        </Columns>
                                                                    </telerik:LayoutRow>
                                                                </Rows>
                                                            </telerik:LayoutRow>
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
            </telerik:RadPageView>
            <telerik:RadPageView runat="server" ID="rpvTestiDocumento">
                <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                    <telerik:LayoutRow RowType="Region">
                        <Columns>
                            <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                                <asp:Label ID="lblIntestazioni" runat="server" Text="Testo Intestazioni" AssociatedControlID="reTestoIntestazioni" /><br />
                                <telerik:RadEditor runat="server" ID="reTestoIntestazioni" ToolsFile="~/App_Data/Editor/Settings/Tools.xml" ToolbarMode="Default" Width="100%" BorderWidth="1" BorderStyle="Solid" AutoResizeHeight="true" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>

                    <telerik:LayoutRow RowType="Region">
                        <Columns>
                            <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                                <asp:Label ID="lblTestoPieDiPagina" runat="server" Text="Testo Pie di pagina" AssociatedControlID="reTestoPieDiPagina" /><br />
                                <telerik:RadEditor runat="server" ID="reTestoPieDiPagina" ToolsFile="~/App_Data/Editor/Settings/Tools.xml" ToolbarMode="Default" Width="100%" BorderWidth="1" BorderStyle="Solid" AutoResizeHeight="true" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>

                    <telerik:LayoutRow RowType="Region">
                        <Columns>
                            <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                                <asp:Label ID="lblTestoSezionePagamenti" runat="server" Text="Testo Sezione Pagamenti" AssociatedControlID="reTestoSezionePagamenti" /><br />
                                <telerik:RadEditor runat="server" ID="reTestoSezionePagamenti" ToolsFile="~/App_Data/Editor/Settings/Tools.xml" ToolbarMode="Default" Width="100%" BorderWidth="1" BorderStyle="Solid" AutoResizeHeight="true" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                </telerik:RadPageLayout>
            </telerik:RadPageView>
        </telerik:RadMultiPage>



        <br />
        <br />
        <br />
    </div>

    <telerik:RadWindow ID="rwRichiestaValidazione" runat="server" AutoSize="false" Behaviors="None" Width="800px" Height="150px" Modal="true" VisibleStatusbar="false" Title="Richiesta di validazione offerta" Center="true">
        <ContentTemplate>
            <script type="text/javascript">

                function RichiestaProsegumentoConValidatori() {
                    return confirm('Si desidera procedere con l\'invio ai valodatori selezionati?');
                }

            </script>
            <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                <Rows>
                    <telerik:LayoutRow RowType="Region">
                        <Columns>
                            <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                                <asp:Label ID="lblValidatori" runat="server" Text="Seleziona uno o più valori tra quelli riportati di seguito e premi il pulsante 'Prosegui' per inviare la richiesta" AssociatedControlID="rbElencoValidatori" />
                                <asp:RequiredFieldValidator ID="rfvValidatori" runat="server" Display="Dynamic" ValidationGroup="Validatori" Enabled="false"
                                    ControlToValidate="rbElencoValidatori"
                                    ErrorMessage="La selezione del validatore è obbligatoria."
                                    ForeColor="Red">
                                    <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                </asp:RequiredFieldValidator>
                                <br />
                                <telerik:RadComboBox runat="server" ID="rbElencoValidatori" Width="100%" EnableCheckAllItemsCheckBox="true" CheckBoxes="true" Style="z-index: 10000" ValidationGroup="Validatori"
                                    DataValueField="ID" DataTextField="NominativoConEmail">
                                    <Localization AllItemsCheckedString="Selezionato tutti i validatori" CheckAllString="Seleziona tutti i validatori" ItemsCheckedString="Validatori selezionati" />
                                </telerik:RadComboBox>
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow RowType="Region">
                        <Columns>
                            <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                                <asp:Button ID="btnProsegui" runat="server" Text="Prosegui" ValidationGroup="Validatori" OnClientClick="if(!RichiestaProsegumentoConValidatori()) {return false;}" OnClick="btnProsegui_Click" />&nbsp;
                                <asp:Button ID="btnAnnulla" runat="server" Text="Annulla" ValidationGroup="Validatori" CausesValidation="false" OnClick="btnAnnulla_Click" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                </Rows>
            </telerik:RadPageLayout>
        </ContentTemplate>
    </telerik:RadWindow>

    <telerik:RadWindow ID="rwCaricaDocumento" runat="server" AutoSize="false" Behaviors="None" Width="800px" Height="250px" Modal="true" VisibleStatusbar="false" Title="Caricamento Documento" Center="true">
        <ContentTemplate>
            <script type="text/javascript">

                function RichiestaProsegumentoConCaricamentoDocumento() {
                    if (!confirm('Si desidera procedere con il caricamento del documento indicato?')) {
                        return false;
                    }
                }

            </script>
            <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="12">
                            <asp:Label runat="server" Text="Documento da caricare" Font-Bold="true" AssociatedControlID="ruCaricamentoDocumento" />
                            <br />
                            <telerik:RadUpload ID="ruCaricamentoDocumento" runat="server" ValidationGroup="DocumentoDaCaricare" ControlObjectsVisibility="None" InitialFileInputsCount="1" MaxFileInputsCount="1" Visible="true" Width="250px" />
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="4">
                            <asp:Label runat="server" Text="Tipologia Documento" AssociatedControlID="rcbTipologiaDocumento" Font-Bold="true" />
                            <asp:RequiredFieldValidator ID="rfvTipologiaDocumento" runat="server" Display="Dynamic" ValidationGroup="DocumentoDaCaricare" Enabled="false"
                                ControlToValidate="rcbTipologiaDocumento"
                                ErrorMessage="La selezione la tiplogia di documento."
                                ForeColor="Red">
                                <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                            </asp:RequiredFieldValidator>
                            <br />
                            <telerik:RadComboBox ID="rcbTipologiaDocumento" runat="server"
                                Style="z-index: 10000"
                                Width="100%"
                                DataValueField="Key"
                                DataTextField="Value"
                                HighlightTemplatedItems="true"
                                MarkFirstMatch="true"
                                EmptyMessage="Seleziona una tipologia di documento"
                                AllowCustomText="false"
                                EnableItemCaching="true"
                                ValidationGroup="DocumentoDaCaricare" />
                        </telerik:LayoutColumn>
                        <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="8">
                            <asp:Label runat="server" Text="Note" AssociatedControlID="rtbNoteDocumentoDaCaricare" /><br />
                            <telerik:RadTextBox ID="rtbNoteDocumentoDaCaricare" runat="server" Width="100%" TextMode="MultiLine" Rows="3" ValidationGroup="DocumentoDaCaricare" />
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>
                <telerik:LayoutRow RowType="Region">
                    <Columns>
                        <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                            <asp:Button ID="btnCaricaDocumento" runat="server" Text="Carica Documento" ValidationGroup="DocumentoDaCaricare" OnClientClick="return RichiestaProsegumentoConCaricamentoDocumento()" OnClick="btnCaricaDocumento_Click" />&nbsp;
                            <asp:Button ID="btnAnnullaCaricamentoDocumento" runat="server" Text="Annulla" ValidationGroup="DocumentoDaCaricare" CausesValidation="false" OnClick="btnAnnullaCaricamentoDocumento_Click" />
                        </telerik:LayoutColumn>
                    </Columns>
                </telerik:LayoutRow>
            </telerik:RadPageLayout>

            <telerik:RadProgressArea ID="rpaCarcamentoAllegati" runat="server" Style="position: absolute; top: 150px; left: 150px; width: 700px; z-index: 9999" OnClientProgressBarUpdating="LockPage" />
            <telerik:RadProgressManager ID="rpmCaricamentoAllegati" runat="server" />
        </ContentTemplate>
    </telerik:RadWindow>

    <telerik:RadWindow ID="rwInvioOffertaAlCliente" runat="server" AutoSize="false" Behaviors="None" Width="800px" Height="675px" Modal="true" VisibleStatusbar="false" Title="Invio offerta al cliente" Center="true" OnClientShow="rwInvioOffertaAlCliente_OnClientShow">
        <ContentTemplate>
            <script type="text/javascript">

                function RichiestaProsegumentoConInvioOffertaAlCliente() {

                    var errori = [];

                    var rcbDocumentiOffertaCaricati = $find("<%=rcbDocumentiOffertaCaricati.ClientID%>");
                    if (rcbDocumentiOffertaCaricati != null) {
                        var checkedItems = rcbDocumentiOffertaCaricati.get_checkedItems();
                        if (checkedItems == null || checkedItems.length == 0) {
                            errori.push("- E'necessario selezionare almeno un documento tra quelli caricati nella offerta");
                        }
                    }

                    var rtbIndirizzoEmailInvioOfferta = $get("<%=tbIndirizzoEmailInvioOfferta.ClientID%>");
                    if (rtbIndirizzoEmailInvioOfferta != null) {
                        var email = rtbIndirizzoEmailInvioOfferta.value;
                        if (email == null || $.trim(email) == "") {
                            errori.push("- Indirizzo email a cui inviare l'offerta è obbligatorio");
                        }
                        else if (!EmailAddressIsValid(email)) {
                            errori.push("- L'indirizzo email inserito non è valido");
                        }
                    }

                    var reTestoEmailDaInviareAlCliente = $find("<%=reTestoEmailDaInviareAlCliente.ClientID%>");
                    if (reTestoEmailDaInviareAlCliente != null) {
                        var text = reTestoEmailDaInviareAlCliente.get_text();
                        if (text == null || $.trim(text) == "") {
                            errori.push("Per procedere è necessario inserire il testo del messaggio da inviare al cliente");
                        }
                    }

                    if (errori.length > 0) {
                        alert("Attenzione!\nPer proseguie è necessario risolvere i problemi riportati di seguito:\n\n" + errori.join("\n\n"));
                        return false;
                    }
                    else {
                        return confirm('Si desidera procedere con l\'invio dell\'offerta selezionata?');
                    }
                }

                function validateIndirizzoEmailInvioOfferta(source, args) {
                    args.IsValid = true;
                    var tbIndirizzoEmailInvioOfferta = $get("<%=tbIndirizzoEmailInvioOfferta.ClientID %>");
                    if (tbIndirizzoEmailInvioOfferta) {
                        if (tbIndirizzoEmailInvioOfferta.value) {
                            args.IsValid = EmailAddressIsValid(tbIndirizzoEmailInvioOfferta.value);
                        }
                        else {
                            args.IsValid = false;
                        }
                    }
                    else {
                        args.IsValid = false;
                    }
                }

            </script>
            <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                <Rows>
                    <telerik:LayoutRow RowType="Region">
                        <Columns>
                            <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                                <asp:Label ID="lblInvioOffertaAlCliente" runat="server" Font-Bold="true" Text="Seleziona l'offerta da inviare al cliente e premi il pulsante 'Invia' per inviare l'offerta" AssociatedControlID="rcbDocumentiOffertaCaricati" />
                                <asp:RequiredFieldValidator ID="rfvElencoDocumentiOffertaCaricati" runat="server" Display="Dynamic" ValidationGroup="InvioOffertaAlCliente" Enabled="false"
                                    ControlToValidate="rcbDocumentiOffertaCaricati"
                                    ErrorMessage="La selezione dell'offerta da inviare è obbligatoria."
                                    ForeColor="Red">
                                    <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                </asp:RequiredFieldValidator>
                                <br />
                                <telerik:RadComboBox runat="server" ID="rcbDocumentiOffertaCaricati" Width="100%" Style="z-index: 10000" ValidationGroup="InvioOffertaAlCliente"
                                    DataValueField="ID" DataTextField="NomeFile" EnableCheckAllItemsCheckBox="true" CheckBoxes="true">
                                    <Localization AllItemsCheckedString="Selezionato tutti i documenti" CheckAllString="Seleziona tutti i documenti" ItemsCheckedString="Documenti selezionati" />
                                    <HeaderTemplate>
                                        <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                            <Rows>
                                                <telerik:LayoutRow RowType="Region">
                                                    <Columns>
                                                        <telerik:LayoutColumn Span="5">
                                                            <strong>Data Archiviazione</strong>
                                                        </telerik:LayoutColumn>
                                                        <telerik:LayoutColumn Span="2">
                                                            <strong>Tipologia</strong>
                                                        </telerik:LayoutColumn>
                                                        <telerik:LayoutColumn Span="5">
                                                            <strong>Nome File</strong>
                                                        </telerik:LayoutColumn>
                                                    </Columns>
                                                </telerik:LayoutRow>
                                            </Rows>
                                        </telerik:RadPageLayout>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
                                            <Rows>
                                                <telerik:LayoutRow RowType="Region" CssClass="RadPageLayoutOnRadComboBox">
                                                    <Columns>
                                                        <telerik:LayoutColumn Span="5">
                                                            <%# Eval("DataArchiviazione") %>
                                                        </telerik:LayoutColumn>
                                                        <telerik:LayoutColumn Span="2">
                                                            <%# Eval("TipologiaAllegatoString") %>
                                                        </telerik:LayoutColumn>
                                                        <telerik:LayoutColumn Span="5">
                                                            <%# Eval("NomeFile") %>
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
                            <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                                <asp:Label ID="lblIndirizzoEmailInvioOfferta" runat="server" Font-Bold="true" Text="Indirizzo email a cui inviare l'offerta" AssociatedControlID="tbIndirizzoEmailInvioOfferta" />
                                <asp:RequiredFieldValidator ID="rfvIndirizzoEmailInvioOfferta" runat="server" Display="Dynamic" ValidationGroup="InvioOffertaAlCliente" Enabled="false"
                                    ControlToValidate="tbIndirizzoEmailInvioOfferta"
                                    ErrorMessage="L'indirizzo email a cui inviare è obbligatoria."
                                    ForeColor="Red">
                                    <img src="/UI/Images/Markers/marker_rounded_red.png" alt="Obbligatorio" title="Dato obbligatorio" style="height: 12px" />
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvIndirizzoEmailInvioOfferta" runat="server" Display="Dynamic" Enabled="false"
                                    Text="Formato Errato" ErrorMessage="L'indirizzo email inserito non è valido"
                                    ControlToValidate="tbIndirizzoEmailInvioOfferta"
                                    ForeColor="Red"
                                    Font-Bold="true"
                                    ClientValidationFunction="validateIndirizzoEmailInvioOfferta" ValidateEmptyText="false"
                                    ValidationGroup="InvioOffertaAlCliente">  
                                </asp:CustomValidator><br />
                                <asp:TextBox ID="tbIndirizzoEmailInvioOfferta" runat="server" Width="100%" InputType="Email" EmptyMessage="Indirizzo email a cui inviare l'offerta" MaxLength="255" ValidationGroup="InvioOffertaAlCliente" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow RowType="Region">
                        <Columns>
                            <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                                <asp:Label ID="lblTestoEmailDaInviareAlCliente" runat="server" Font-Bold="true" Text="Testo email da inviare al cliente" AssociatedControlID="reTestoEmailDaInviareAlCliente" /><br />
                                <telerik:RadEditor runat="server" ID="reTestoEmailDaInviareAlCliente" ToolsFile="~/App_Data/Editor/Settings/Tools.xml" ToolbarMode="Default" Width="100%" Height="430px" BorderWidth="1" BorderStyle="Solid" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow RowType="Region">
                        <Columns>
                            <telerik:LayoutColumn Span="12" CssClass="SpaziaturaSuperioreRiga">
                                <asp:Button ID="btnInviaOffertaAlCliente" runat="server" Text="Prosegui" ValidationGroup="InvioOffertaAlCliente" OnClientClick="if(!RichiestaProsegumentoConInvioOffertaAlCliente()) {return false;}" OnClick="btnInviaOffertaAlCliente_Click" />&nbsp;
                                <asp:Button ID="btnAnnullaInvioOffertaAlCliente" runat="server" Text="Annulla" ValidationGroup="InvioOffertaAlCliente" CausesValidation="false" OnClick="btnAnnullaInvioOffertaAlCliente_Click" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                </Rows>
            </telerik:RadPageLayout>
        </ContentTemplate>
    </telerik:RadWindow>


    <div style="display: none;">

        <%--        <telerik:RadAjaxManagerProxy runat="server" ID="ramOfferta">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgGrigliaArticoli">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgGrigliaArticoli" LoadingPanelID="RadAjaxLoadingPanelMaster" />
                        <telerik:AjaxUpdatedControl ControlID="RadWindowManagerMaster" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManagerProxy>--%>
        <telerik:RadPersistenceManagerProxy ID="rpmpOfferta" runat="server">
            <PersistenceSettings>
                <telerik:PersistenceSetting ControlID="rtsOfferta" />
            </PersistenceSettings>
        </telerik:RadPersistenceManagerProxy>
    </div>

</asp:Content>
