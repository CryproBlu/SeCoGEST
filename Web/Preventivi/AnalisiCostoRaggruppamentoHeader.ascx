<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AnalisiCostoRaggruppamentoHeader.ascx.cs" Inherits="SeCoGEST.Web.Preventivi.AnalisiCostoRaggruppamentoHeader" %>

<telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid">
    <Rows>
        <telerik:LayoutRow RowType="Row" CssClass="AnalisiCostoRaggruppamentoExpanderHeader">
            <Columns>
                <telerik:LayoutColumn Span="6" SpanXl="6" SpanLg="6" SpanMd="3" SpanSm="12" SpanXs="12">
                    <asp:Label ID="lblDenominazione" runat="server" Font-Bold="true" Text='<%#Eval("Denominazione") %>' Visible="false" />
                    <asp:ImageButton runat="server" ID="ibNuovoGruppo" ImageUrl="~/UI/Images/Toolbar/16x16/add.png" ToolTip="Crea nuovo gruppo" 
                        CommandArgument='<%#Eval("ID") %>' OnCommand="ibNuovoGruppo_Command"
                        OnClientClick="return confirm('Creare un nuovo gruppo?')"  />
                    <asp:TextBox runat="server" ID="txtDenominazione" Width="90%" Font-Bold="true" BackColor="#5467AA" ForeColor="White" Text='<%#Eval("Denominazione") %>' />
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="3" SpanSm="3" SpanXs="12" CssClass="AllineaDestra">
                    Costo: <asp:Label ID="lblTotaleCosto" runat="server" Text='<%#Eval("TotaleCosto", "{0:c}") %>' />
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="3" SpanSm="3" SpanXs="12" CssClass="AllineaDestra">
                    Vendita: <asp:Label ID="lblTotaleVendita" runat="server" Text='<%#Eval("TotaleVendita", "{0:c}") %>' />
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="2" SpanXl="2" SpanLg="2" SpanMd="3" SpanSm="3" SpanXs="12" CssClass="AllineaDestra">
                    Redditività: <asp:Label ID="lblRicarico" runat="server" Text='<%#String.Format("{0:c} ({1}%)", Eval("TotaleRicaricoValuta"), Eval("TotaleRicaricoPercentuale")) %>'  />
                    <asp:ImageButton runat="server" ID="ibEliminaGruppo" ImageUrl="~/UI/Images/Toolbar/16x16/delete.png" ToolTip="Elimina gruppo" 
                        CommandArgument='<%#Eval("ID") %>' OnCommand="ibEliminaGruppo_Command"
                        OnClientClick="return confirm('Eliminare il gruppo indicato e tutto il suo contenuto?')"  />
                </telerik:LayoutColumn>            
            </Columns>
        </telerik:LayoutRow>
    </Rows>
</telerik:RadPageLayout>