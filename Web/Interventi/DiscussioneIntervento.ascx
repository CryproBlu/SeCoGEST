<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiscussioneIntervento.ascx.cs" Inherits="SeCoGEST.Web.Interventi.DiscussioneIntervento" %>

<table style="width: 100%; margin-top: 10px; box-sizing:border-box; border: none; padding: 0px; margin: 7px -7px 7px -7px;">
    <tr>
        <td style="vertical-align: top; padding-top: 15px;">
            <asp:Image runat="server" ID="imgOperator" Height="30px" Style="padding-right: 7px;" />
        </td>
        <td style="width: 100%; vertical-align: top;margin: 0px; padding: 0px; box-sizing: border-box;">
            <asp:Label runat="server" ID="lblEtichetta" Text="Segnalazione" />
            <telerik:RadLabel runat="server" ID="rlDiscussione"
                Width="100%" BorderStyle="Solid" BorderWidth="1" BorderColor="#687DC1" BackColor="White"
                Style="padding: 7px; box-sizing: border-box;"
                Text='<%#Eval("Commento") %>'>
            </telerik:RadLabel>
        </td>
    </tr>
    <tr runat="server" id="trAllegati">
        <td colspan="2" style="text-align: right;">
            <asp:Repeater runat="server" ID="repAllegati" OnItemCommand="repAllegati_ItemCommand">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="lbAllegato" CausesValidation="false"
                        CommandArgument='<%#Eval("ID") %>' CommandName="Download">
                            <img src="/UI/Images/Menu/download.png" width="16" style="margin-left: 10px;margin-right: 3px;" /><%#Eval("NomeFile")%>
                    </asp:LinkButton>&nbsp;
                </ItemTemplate>
            </asp:Repeater>
        </td>
    </tr>
</table>
