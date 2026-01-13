<%@ Page Title="Password Dimenticata" Language="C#" MasterPageFile="~/UI/Disconnected.Master" AutoEventWireup="true" CodeBehind="PasswordDimenticata.aspx.cs" Inherits="SeCoGEST.Web.Sicurezza.Login.PasswordDimenticata" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><%= SeCoGEST.Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE %> - Nome Utente o Password dimenticati</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    Username o Password dimenticati
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="subtitle" runat="server">
    In questa pagina è possibile recuperare le informazioni relative allo username o reimpostare la password di accesso
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="main" runat="server">

    <table style="margin-top: 150px;margin-left:10px;" class="PannelloTestoGuida" cellpadding="10">
		<tr>
			<td class="ColoreSfondoEvidenziato">
				<img alt="Guida"src="/UI/Images/help.png" />
			</td>
			<td style="background-color: #FFFF9F;">
				<span class="skinLabelMedium">Hai dimenticato il tuo username o la password?</span>
                <br />
				<span class="skinLabelSmall">
					Inserisci l'indirizzo e-mail che hai indicato al momento della registrazione e clicca sul pulsante 'Inviami i dati di accesso'. 
				    <br />
					Se l'e-mail è presente nel nostro database ti sarà inviato un messaggio contenente il tuo Username ed un link che ti permetterà di
					reimpostare la tua password. 
				</span>
			</td>
		</tr>
	</table>

    <table style="width:100%; margin-top:15px; padding: 15px;" border="0">
        <tr>
            <td style="width:350px;vertical-align:top;">
	            <span class="skinLabelSmall">Indirizzo e-mail di registrazione:</span><br />
                <asp:TextBox ID="txtEmail" width="350px" runat="server"></asp:TextBox>
	            <br />
	            <br />

                <telerik:RadButton runat="server" ID="rbReimposta" Width="250px" Height="60px" OnClick="rbReimposta_Click" CssClass="tasks">
                    <ContentTemplate>
                        <table border="0" style="vertical-align:middle;">
                            <tr>
                                <td><img alt="Inviami i dati di accesso" src="/UI/Images/Toolbar/send.png" /></td>
                                <td><span style="margin-left:5px;" class="btnText">Inviami i dati di accesso</span></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadButton>
            </td>
            <td style="vertical-align:middle; padding-left:15px;">
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
            </td>
        </tr>
    </table>

</asp:Content>