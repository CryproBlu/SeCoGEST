<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModificaPassword.ascx.cs" Inherits="SeCoGEST.Web.Sicurezza.Login.ModificaPassword" %>
<asp:Panel ID="ChangePasswordPanel" runat="server" DefaultButton="lbChange">
    <table cellspacing="0" cellpadding="3" width="100%" >
        <tr>
            <td>
                <table >
                    <tr>
                        <td style="width: 250px; padding-bottom:15px;">
                            <span class="skinLabelSmall">Nome utente inserito</span><br />
                            <asp:TextBox ID="txtUsername" runat="server" ReadOnly="true" Width="100%" BackColor="Silver"></asp:TextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="width: 250px; padding-bottom:15px;">
                            <span class="skinLabelSmall">Nuova Password</span><br />
                            <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" TextMode="Password" Width="100%"></asp:TextBox>
                        </td>
                        <td style="padding-left:10px;">
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" CssClass="skinLabelSmall"
                                Display="Dynamic" ForeColor="Red"
                                ControlToValidate="txtPassword"
                                ErrorMessage="Obbligatorio" 
                                Text="Obbligatorio">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revPassword" Display="Dynamic" runat="server" CssClass="skinLabelSmall"
                                ControlToValidate="txtPassword" ForeColor="Red"
                                Text="La password deve contenere almeno una lettera ed un numero." 
                                ErrorMessage="La password deve contenere almeno una lettera ed un numero." 
                                ValidationExpression="(.*[0-9]{1,}.*[a-zA-Z]{1,}.*)|(.*[a-zA-Z]{1,}.*[0-9]{1,}.*)">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr> 
                    <tr>
                        <td style="width: 250px;" valign="top">
                            <span class="skinLabelSmall">Password di conferma</span><br />
                            <asp:TextBox ID="txtConfermaPassword" runat="server" MaxLength="50" TextMode="Password" Width="100%"></asp:TextBox>
                        </td>
                        <td style="padding-left:10px;">
                            <asp:RequiredFieldValidator ID="rfvConfermaPassword" runat="server" ForeColor="Red" Display="Dynamic" CssClass="skinLabelSmall"
                                ControlToValidate="txtConfermaPassword"
                                ErrorMessage="Obbligatorio" 
                                Text="Obbligatorio">
                            </asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvPassword" runat="server" ForeColor="Red" Display="Dynamic" CssClass="skinLabelSmall"
                                ControlToValidate="txtConfermaPassword"
                                ControlToCompare="txtPassword"
                                ErrorMessage="Le due password non coincidono" 
                                Text="Le due password non coincidono">
                            </asp:CompareValidator>
                        </td>
                    </tr> 
                    <tr>
                        <td style="width: 250px;">
                            <br />
                            <telerik:RadButton runat="server" ID="lbChange" Width="250px" Height="60" OnClick="lbChange_Click" CssClass="tasks">
                                <ContentTemplate>
                                    <table border="0" valign="middle" height="100%">
                                        <tr>
                                            <td><img alt="cog" src="/UI/Images/Buttons/Password.png" /></td>
                                            <td><span style="margin-left:5px;" class="btnText">Modifica Password</span></td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </telerik:RadButton>
                        </td>
                        <td></td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false" Text="" ForeColor="Red" Font-Bold="true" CssClass="skinLabelSmall"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
