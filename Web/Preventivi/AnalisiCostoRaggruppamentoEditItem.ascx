<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnalisiCostoRaggruppamentoEditItem.ascx.cs" Inherits="SeCoGEST.Web.Preventivi.AnalisiCostoRaggruppamentoEditItem" %>

<telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
    <Rows>
        <telerik:LayoutRow RowType="Row" CssClass="AnalisiCostoRaggruppamentoExpanderItem SpaziaturaSuperioreGruppo">
            <Columns>
                <telerik:LayoutColumn Span="4" SpanXl="4" SpanLg="4" SpanMd="4" SpanSm="12" SpanXs="12">
                    <asp:Label ID="lblGruppo" runat="server" Font-Bold="true" Text="Gruppo" AssociatedControlID="rcbGruppo" /><br />
                    <telerik:RadComboBox runat="server" ID="rcbGruppo" DataValueField="CODICE" DataTextField="DESCRIZIONE" Filter="Contains" Width="100%"></telerik:RadComboBox>
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="4" SpanXl="4" SpanLg="4" SpanMd="4" SpanSm="12" SpanXs="12">
                    <asp:Label ID="lblCategoria" runat="server" Font-Bold="true" Text="Categoria" AssociatedControlID="rcbCategoria" /><br />
                    <telerik:RadComboBox runat="server" ID="rcbCategoria" DataValueField="CODICE" DataTextField="DESCRIZIONE" Filter="Contains" Width="100%"></telerik:RadComboBox>
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="4" SpanXl="4" SpanLg="4" SpanMd="4" SpanSm="12" SpanXs="12">
                    <asp:Label ID="lblCategoriaStatistica" runat="server" Font-Bold="true" Text="Categoria statistica" AssociatedControlID="rcbCategoriaStatistica" /><br />
                    <telerik:RadComboBox runat="server" ID="rcbCategoriaStatistica" DataValueField="CODICE" DataTextField="DESCRIZIONE" Filter="Contains" Width="100%"></telerik:RadComboBox>
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="4" SpanXl="4" SpanLg="4" SpanMd="4" SpanSm="12" SpanXs="12">
                    <br />
                    <asp:LinkButton runat="server" ID="lbContinua" Text="Continua..." OnClick="lbContinua_Click"></asp:LinkButton>
                </telerik:LayoutColumn>
            </Columns>
        </telerik:LayoutRow>

        <telerik:LayoutRow runat="server" ID="rowAddArticoloCodiciSelezione" RowType="Row" Visible="false" CssClass="AnalisiCostoRaggruppamentoExpanderItem SpaziaturaSuperioreRiga">
            <Columns>
                <telerik:LayoutColumn Span="3" SpanXl="2" SpanLg="2" SpanMd="3" SpanSm="12" SpanXs="12">
                    <asp:Label ID="Label1" runat="server" Text="Codice Articolo" AssociatedControlID="rcbCodiceArticolo" /><br />
                    <telerik:RadComboBox ID="rcbCodiceArticolo" runat="server"
                        Width="100%"
                        DropDownAutoWidth="Enabled"
                        MarkFirstMatch="false"
                        HighlightTemplatedItems="true"
                        EmptyMessage="Selezionare un Articolo"
                        ItemsPerRequest="20"
                        ShowMoreResultsBox="true"
                        EnableLoadOnDemand="true"
                        EnableItemCaching="true"
                        AllowCustomText="true"
                        LoadingMessage="Caricamento in corso..."
                        DataValueField="CodiceArticolo"
                        DataTextField="Descrizione"
                        OnItemsRequested="rcbCodiceArticolo_ItemsRequested"
                        EnableVirtualScrolling="true">
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
                                                <%# DataBinder.Eval(Container.DataItem, "CodiceArticolo") %>
                                            </telerik:LayoutColumn>
                                            <telerik:LayoutColumn Span="8">
                                                <%# DataBinder.Eval(Container.DataItem, "Descrizione") %>
                                            </telerik:LayoutColumn>
                                        </Columns>
                                    </telerik:LayoutRow>
                                </Rows>
                            </telerik:RadPageLayout>
                        </ItemTemplate>
                    </telerik:RadComboBox>
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="3" SpanXl="6" SpanLg="6" SpanMd="5" SpanSm="12" SpanXs="12">
                    <asp:Label ID="Label2" runat="server" Text="Descrizione" AssociatedControlID="rtbDescrizione" /><br />
                    <telerik:RadTextBox runat="server" ID="rtbDescrizione" Width="100%"></telerik:RadTextBox>
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="3" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12">
                    <asp:Label ID="Label3" runat="server" Text="UM" AssociatedControlID="rtbUM" /><br />
                    <telerik:RadTextBox runat="server" ID="rtbUM" Width="100%"></telerik:RadTextBox>
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="3" SpanXl="2" SpanLg="2" SpanMd="2" SpanSm="12" SpanXs="12">
                    <asp:Label ID="Label4" runat="server" Text="Q.tà" AssociatedControlID="rntbQuantità" /><br />
                    <telerik:RadNumericTextBox runat="server" ID="rntbQuantità" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                </telerik:LayoutColumn>
            </Columns>
        </telerik:LayoutRow>

        <telerik:LayoutRow runat="server" ID="rowAddArticolo" RowType="Row" Visible="false" CssClass="AnalisiCostoRaggruppamentoExpanderItem">
            <Columns>
                <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="12">
                    <asp:Label ID="Label5" runat="server" Text="Costo unitario" AssociatedControlID="rntbCosto" /><br />
                    <telerik:RadNumericTextBox runat="server" ID="rntbCosto" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="12">
                    <asp:Label ID="Label6" runat="server" Text="Prezzo unitario" AssociatedControlID="rntbVendita" /><br />
                    <telerik:RadNumericTextBox runat="server" ID="rntbVendita" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="12">
                    <asp:Label ID="Label8" runat="server" Text="Ricarico €" AssociatedControlID="rntbRicaricoValore" /><br />
                    <telerik:RadNumericTextBox runat="server" ID="rntbRicaricoValore" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="12">
                    <asp:Label ID="Label9" runat="server" Text="Ricarico %" AssociatedControlID="rntbRicaricoPercentuale" /><br />
                    <telerik:RadNumericTextBox runat="server" ID="rntbRicaricoPercentuale" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="12">
                    <asp:Label ID="Label7" runat="server" Text="Totale costo" AssociatedControlID="rntbTotaleCosto" /><br />
                    <telerik:RadNumericTextBox runat="server" ID="rntbTotaleCosto" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="12">
                    <asp:Label ID="Label10" runat="server" Text="Totale vendita" AssociatedControlID="rntbTotaleVendita" /><br />
                    <telerik:RadNumericTextBox runat="server" ID="rntbTotaleVendita" Width="100%" NumberFormat-DecimalSeparator="," NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                </telerik:LayoutColumn>
            </Columns>
        </telerik:LayoutRow>

    </Rows>
</telerik:RadPageLayout>

<asp:Repeater runat="server" ID="repCampiAggiuntivi">
    <HeaderTemplate>
        <table border="0" style="width:100%;padding-top:15px;">
            <thead>
                <tr class="AnalisiCostoRaggruppamentoExpanderHeader">
                    <th colspan="2" style="height:20px;">
                        Campi aggiuntivi
                    </th>
                </tr>
            </thead>
    </HeaderTemplate>
    <ItemTemplate>
            <tr>
                <td style="width:150px">
                    <asp:HiddenField runat="server" ID="hdIdCampoAggiuntivo" Value='<%#Eval("ID") %>' />
                    <asp:Label runat="server" ID="lblNomeCampoAggiuntivo" Text='<%#Eval("NomeCampo") %>'></asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtValoreCampoAggiuntivo" Text='<%#Eval("Valore") %>' Width="100%"></asp:TextBox>
                </td>
            </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>