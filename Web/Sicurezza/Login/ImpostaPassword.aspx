<%@ Page Title="Reimpostazione della Password" Language="C#" MasterPageFile="~/UI/Disconnected.Master" AutoEventWireup="true" CodeBehind="ImpostaPassword.aspx.cs" Inherits="SeCoGEST.Web.Sicurezza.Login.ImpostaPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><%= SeCoGEST.Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE %> - Reimpostazione della Password</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    Reimpostazione della Password
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="subtitle" runat="server">
        In questa pagina è possibile richiedere la reimpostazione della password di accesso
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="main" runat="server">

    <table runat="server" id="tableMessaggio" style="margin-top: 150px; margin-left:10px;" class="PannelloTestoGuida" cellpadding="10">
        <tr>
            <td class="ColoreSfondoEvidenziato">
                 <img alt="Guida"src="/UI/Images/help.png" />
            </td>
            <td style="background-color: #FFFF9F;">
                <span class="skinLabelMedium">
                    Gentile <asp:Label runat="server" ID="lblNomeUtente"></asp:Label>,<br />
                    completando questa procedura la tua password verrà reimpostata e il suo nuovo valore ti sarà inviato all'indirizzo e-mail '<asp:Label runat="server" ID="lblIndirizzoEmail"></asp:Label>'.
                    <br />
                    <br />
                    Per eseguire l'operazione clicca sul pulsante <b>'Reimposta Password'</b>.
                </span>
            </td>
        </tr>
    </table>

    <div runat="server" id="divReimposta" style="padding:20px;">

        <telerik:RadButton runat="server" ID="rbReimposta" Width="250px" Height="60px" OnClick="rbReimposta_Click" CssClass="tasks" Skin="MetroTouch">
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" style="vertical-align:middle;">
                    <tr>
                        <td><img alt="Reimposta Password" src="/UI/Images/Buttons/Password.png" /></td>
                        <td><span style="margin-left:5px;" class="btnText">Reimposta Password</span></td>
                    </tr>
                </table>
            </ContentTemplate>
        </telerik:RadButton>

        <table border="0">
            <tr>
                <td>
                    <asp:Image ID="AlertImage" runat="server" Visible="false" ImageUrl="~/UI/Images/Toolbar/problem.png" hspace="10px" />
                </td>
                <td>
                    <asp:Label ID="lblResult" runat="server" Font-Bold="true" CssClass="skinLabelSmall"></asp:Label>
                </td>
            </tr>
        </table>
    </div>

    <br />
    <br />
    <br />

</asp:Content>
