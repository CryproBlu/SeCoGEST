<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArticoliInterventoEdit.ascx.cs" Inherits="SeCoGEST.Web.Interventi.ArticoliInterventoEdit" %>

<style type="text/css">
    /*.WindowFooter {
        position: absolute;
        left: 18px;
        right: 18px;
        bottom: 40px;
    }*/
    .WindowBorders {
        border:solid 1px black;
        background-color:#E5ECF5;
        width:95%;
        height:95%;

        margin-right:10px;
        padding-right:30px;

    }
</style>

<telerik:RadScriptBlock runat="server">
    <script type="text/javascript">
                
        function rcbProvenienzaArticolo_OnClientSelectedIndexChanged(sender, eventArgs) {
            var valoreCombo = sender.get_value();
            if (valoreCombo.length > 0)
            {
                var provenienza = valoreCombo.charAt(0);
                var lcTempo = $get("<%=lcTempo.ClientID%>");
                var lcQuantita = $get("<%=lcQuantita.ClientID%>");

                // Per gli Addebiti deve essere richiesta una quantità per tutti gli altri una tempistica
                if (provenienza == "4") 
                {
                    lcTempo.style.display = 'none';
                    lcQuantita.style.display = 'block';
                }
                else
                {
                    lcTempo.style.display = 'block';
                    lcQuantita.style.display = 'none';
                }
            }
        }

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

            var note = eventArgs.get_item().get_attributes().getAttribute("NoteArticolo");
            var rtbNote = $find('<%=rtbNote.ClientID%>');
            rtbNote.set_value(note);
        }

        function validateComborcbArticoli(source, args) {
            args.IsValid = false;
            var combo = $find("<%= rcbArticoli.ClientID %>");
            if (combo)
            {
                if (combo.get_selectedItem())
                {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }

        function gestisciModificaValoreTempoImpiegato(rmtbTempoImpiegato) {
            try {
                
                var rntbQuantita = $find('<%=rntbQuantita.ClientID%>');
                if (rntbQuantita != null) {

                    var tempoImpiegatoValue = rmtbTempoImpiegato.get_valueWithLiterals();

                    if (tempoImpiegatoValue != null && $.trim(tempoImpiegatoValue) != "" && $.trim(tempoImpiegatoValue) != ":0") {
                        rntbQuantita.set_value("");
                        rntbQuantita.disable();
                    }
                    else {
                        rntbQuantita.enable();
                    }
                }
            } catch (e) {
                showMessage("Attenzione", e.message);
            }
        }

        function gestisciModificaValoreQuantita(rntbQuantita) {
            try {
                
                var rmtbTempoImpiegato = $find('<%=rmtbTempoImpiegato.ClientID %>');
                if (rmtbTempoImpiegato != null) {

                    var valore = rntbQuantita.get_value();

                    if ($.trim(valore) != "") {
                        rmtbTempoImpiegato.disable();
                        rmtbTempoImpiegato.set_value("");
                        rmtbTempoImpiegato.clear();
                    }
                    else {
                        rmtbTempoImpiegato.enable();
                    }
                }
            } catch (e) {
                showMessage("Attenzione", e.message);
            }
        }
        
        function rntbQuantita_ClientEvents_OnValueChanged(sender, args) {            
            gestisciModificaValoreQuantita(sender);
        }

        function rntbQuantita_ClientEvents_OnLoad(sender, args) {
            gestisciModificaValoreQuantita(sender);
        }

        function rmtbTempoImpiegato_ClientEvents_OnValueChanged(sender, args) {
            gestisciModificaValoreTempoImpiegato(sender);
        }

        function rmtbTempoImpiegato_ClientEvents_OnLoad(sender, args) {
            gestisciModificaValoreTempoImpiegato(sender);
        }
                
    </script>
</telerik:RadScriptBlock>

<telerik:RadPageLayout runat="server" GridType="Fluid" CssClass="WindowBorders">
    <Rows>
        <telerik:LayoutRow RowType="Region">
            <Columns>
                <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="12">
                    <asp:Label runat="server" ID="lblProvenienzaArticolo" Text="Provenienza articolo" AssociatedControlID="rcbProvenienzaArticolo"></asp:Label><br />
                    <telerik:RadComboBox ID="rcbProvenienzaArticolo" runat="server" 
                        OnClientSelectedIndexChanged="rcbProvenienzaArticolo_OnClientSelectedIndexChanged"
                        HighlightTemplatedItems="true"
                        Width="100%" ZIndex="10000">
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
                    <asp:CustomValidator ID="cvArticoli" runat="server"  
                        Text="Obbligatorio" ErrorMessage="E' obbligatorio selezionare un articolo dall'elenco proposto."  
                        ControlToValidate="rcbArticoli"
                        ForeColor="Red" Font-Bold="true"
                        ClientValidationFunction="validateComborcbArticoli" ValidateEmptyText="True"  
                        ValidationGroup="FinestraArticoli">  
                    </asp:CustomValidator> 
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
                                            <telerik:LayoutColumn Span="6">
                                                <asp:Label runat="server" Text="Descrizione" Font-Bold="true" />
                                            </telerik:LayoutColumn>
                                            <telerik:LayoutColumn Span="1" CssClass="AllineaCentro">
                                                <asp:Label runat="server" Text="Ore Fatte" Font-Bold="true" />
                                            </telerik:LayoutColumn>
                                            <telerik:LayoutColumn Span="1" CssClass="AllineaCentro">
                                                <asp:Label runat="server" Text="Ore Incluse" Font-Bold="true" />
                                            </telerik:LayoutColumn>
                                            <telerik:LayoutColumn Span="1" CssClass="AllineaCentro">
                                                <asp:Label runat="server" Text="Ore<br>da Fare" Font-Bold="true" />
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
                                            <telerik:LayoutColumn Span="6">
                                                <%# DataBinder.Eval(Container.DataItem, "Descrizione") %>
                                            </telerik:LayoutColumn>
                                            <telerik:LayoutColumn Span="1" CssClass="AllineaCentro">
                                                <%# DataBinder.Eval(Container.DataItem, "OreFatte") %>
                                            </telerik:LayoutColumn>
                                            <telerik:LayoutColumn Span="1" CssClass="AllineaCentro">
                                                <%# DataBinder.Eval(Container.DataItem, "OreIncluse") %>
                                            </telerik:LayoutColumn>
                                            <telerik:LayoutColumn Span="1" CssClass="AllineaCentro">
                                                <%# DataBinder.Eval(Container.DataItem, "OreDaFare") %>
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
                <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="6" runat="server" ID="lcTempo">
                    <asp:Label runat="server" ID="lblTempo" Text="Tempo impiegato in relazione all'articolo:" AssociatedControlID="rmtbTempoImpiegato"></asp:Label>&nbsp;&nbsp;
                    <telerik:RadMaskedTextBox ID="rmtbTempoImpiegato" runat="server" 
                        Mask="####:<0..59>"
                        ClientEvents-OnValueChanged="rmtbTempoImpiegato_ClientEvents_OnValueChanged"
                        ClientEvents-OnLoad="rmtbTempoImpiegato_ClientEvents_OnLoad">
                    </telerik:RadMaskedTextBox>
                </telerik:LayoutColumn>

                <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="6" runat="server" ID="lcQuantita" >
                    <asp:Label runat="server" ID="lblQuantita" Text="Quantita:" AssociatedControlID="rntbQuantita"></asp:Label>&nbsp;&nbsp;
                    <telerik:RadNumericTextBox ID="rntbQuantita" Width="100px" runat="server" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" 
                        ClientEvents-OnValueChanged="rntbQuantita_ClientEvents_OnValueChanged"
                        ClientEvents-OnLoad="rntbQuantita_ClientEvents_OnLoad"></telerik:RadNumericTextBox>
                </telerik:LayoutColumn>

                <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="6">
                    <asp:CheckBox runat="server" ID="chkDaFatturare" Text="Da Fatturare" />
                </telerik:LayoutColumn>
            </Columns>
        </telerik:LayoutRow>
                            


        <telerik:LayoutRow RowType="Region">
            <Columns>
                <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="12">
                    <asp:Label runat="server" ID="lblNote" Text="Note" AssociatedControlID="rtbNote"></asp:Label>&nbsp;&nbsp;
                    <telerik:RadTextBox runat="server" ID="rtbNote" Width="100%" Height="100px" TextMode="MultiLine"></telerik:RadTextBox>
                </telerik:LayoutColumn>
            </Columns>
        </telerik:LayoutRow>


        <telerik:LayoutRow RowType="Region" CssClass="SpaziaturaSuperioreRiga">
            <Columns>
                <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="2" SpanXs="6">
                    <telerik:RadButton runat="server" ID="rbSalvaValore" Width="110px" Height="45" CssClass="tasks rounded-corners" OnClick="rbSalvaValore_Click" ValidationGroup="FinestraArticoli">
                        <ContentTemplate>
                            <table style="vertical-align:middle; height:100%;">
                                <tr>
                                    <td><img alt="Salva" src="/UI/Images/Toolbar/32x32/save.png" /></td>
                                    <td><span style="margin-left:5px;" class="btnText">Salva</span></td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </telerik:RadButton>
                </telerik:LayoutColumn>

                <telerik:LayoutColumn CssClass="SpaziaturaSuperioreRiga" Span="2" SpanXs="6">
                    <telerik:RadButton runat="server" ID="rbAnnullaModificaValore" Width="110px" Height="45" CssClass="tasks rounded-corners" CausesValidation="false" OnClick="rbAnnullaModificaValore_Click">
                        <ContentTemplate>
                            <table style="vertical-align:middle; height:100%;">
                                <tr>
                                    <td><img alt="Annulla" src="/UI/Images/Toolbar/32x32/cancel.png" /></td>
                                    <td><span style="margin-left:5px;" class="btnText">Annulla</span></td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </telerik:RadButton>

                </telerik:LayoutColumn>
            </Columns>
        </telerik:LayoutRow>

        <telerik:LayoutRow RowType="Region" CssClass="SpaziaturaSuperioreRiga">
        </telerik:LayoutRow>

    </Rows>
</telerik:RadPageLayout>
