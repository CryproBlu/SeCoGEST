<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SeCoGEST.Web.Sicurezza.Login.Login" Title="HDemia - Gestione Accademia - Pagina di Login"%>

<%@ Register src="ModificaPassword.ascx" tagname="ModificaPasswordMan" tagprefix="uc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/UI/css/Global.css" rel="stylesheet" />
</head>

<body style="background-color:white;">
    <form id="form2" runat="server">
        <telerik:RadScriptManager ID="LoginRadScriptManager" runat="server" ClientIDMode="AutoID"></telerik:RadScriptManager>
        <telerik:RadFormDecorator ID="QsfFromDecorator" runat="server" Enabled="true" DecoratedControls="All" EnableRoundedCorners="false" />
        <telerik:RadSkinManager ID="LoginRadSkinManager" runat="server" Enabled="true" ShowChooser="false" Skin="MetroTouch">
            <TargetControls>
                <telerik:TargetControl ControlID="QsfFromDecorator" />
            </TargetControls>
        </telerik:RadSkinManager>

        <span id="spanVersione" style="font-family: Verdana; font-size:10px; position:absolute; top:0px; right:10px; z-index:10; color:#fff;" ><asp:Label ID="lblVersione" runat="server" /></span>
        <div id="intestazione" class="ColoreSfondoEvidenziato" style="border:0px; position: absolute; top:0px; left:0px; right:0px; height:115px; 
            font-variant:small-caps; vertical-align:top; padding-top:5px; padding-left:5px; padding-bottom:10px;">
            <table style="width:100%; height:100px;" cellpadding="0" cellspacing="0" class="ColoreSfondoEvidenziato" >
                <tr>
                    <td style="width:170px; height:110px; padding:5px; vertical-align:central;" rowspan="2">
                        <img alt="" src="/UI/Images/Logo_Small.png" style="border: solid 1px black; background-color: white; padding:5px;" />
                    </td>
                    <td style="width:100%; padding-left:10px;">
                        <asp:Label runat="server" ID="lbPageTitle" CssClass="skinLabelBig"></asp:Label>
                        <br />
                        <span class="skinLabelMedium">Pagina di accesso</span>
                    </td>
                    <td>
                        <a runat="server" id="hlLogin" href="/Sicurezza/Login/Login.aspx"><img src="/UI/Images/Toolbar/Login.png" alt="Login" title="Login" style="border:0px; position:absolute; top:20px; right:10px;" /></a>
                    </td>
                </tr>
            </table>
        </div>

        <table cellpadding="10px" style="margin-top:140px;">
            <tr>
                <td valign="top">
                    <div id="PannelloMessaggioOffLine" runat="server" class="PannelloTestoGuida" visible="false">
                        Non è possibile effettuare l'accesso perchè l'applicazione è attualmente in fase di aggiornamento.
                        <br />
                        <br />
                        Si prega di riprovare più tardi.&nbsp;&nbsp;&nbsp;<a href="#" onclick="window.location.href=window.location.href;">Riprova</a>
                    </div>

                    <asp:Panel ID="LoginPanel" runat="server" DefaultButton="lbLogin">
                        <span class="skinLabelMedium">Inserire i propri nome utente e password di accesso e successivamente cliccare sul pulsante 'Entra' per accedere alle funzionalità dell'applicazione.</span>
                        <br />
                        <br />
                        <table cellspacing="0" cellpadding="3" width="100%" >
                            <tr>
                                <td>
                                    <table style="width:100%">
                                        <tr>
                                            <td style="width: 250px;">
                                                <span class="skinLabelSmall">Nome utente</span><br />
                                                <asp:TextBox ID="txtUsername" runat="server" MaxLength="50" Width="100%"></asp:TextBox>
                                            </td>
                                            <td style="padding-left:10px; width:90px;">
                                                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ForeColor="Red" CssClass="skinLabelSmall"
                                                    ControlToValidate="txtUsername" ErrorMessage="Obbligatorio">Obbligatorio</asp:RequiredFieldValidator>
                                            </td>
                                            <td rowspan="2" style="padding-left:10px;" valign="middle" align="left">
                                                <asp:Label ID="lblMessage" runat="server" EnableViewState="false" Text="" ForeColor="Red" Font-Bold="true" CssClass="skinLabelSmall"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 250px;padding-top:10px;">
                                                <span class="skinLabelSmall">Password</span><br />
                                                <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" TextMode="Password" Width="100%"></asp:TextBox>
                                            </td>
                                            <td style="padding-left:10px; width:90px;">
                                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ForeColor="Red" CssClass="skinLabelSmall"
                                                    ControlToValidate="txtPassword"
                                                    ErrorMessage="Obbligatoria" 
                                                    Text="Obbligatoria">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr> 
                                        <tr>
                                            <td style="width: 250px;">
                                                <br />
                                                <telerik:RadButton runat="server" ID="lbLogin" Width="250px" Height="60" OnClick="lbLogin_Click" CssClass="tasks">
                                                    <ContentTemplate>
                                                        <table border="0" style="vertical-align:middle;">
                                                            <tr>
                                                                <td><img alt="Login" src="/UI/Images/Toolbar/Login.png" /></td>
                                                                <td><span style="margin-left:5px;" class="btnText">Entra</span></td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                               </telerik:RadButton>
                                            </td>
                                            <td colspan="2" valign="middle" align="left">
                                                <table width="100%" cellpadding="10" style="margin-left:20px; border-top: 1px solid Silver">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chkRemember" runat="server" Text="Attiva riconoscimento automatico" Checked="false" />
                                                        </td>
                                                        <td>
                                                            <asp:HyperLink ID="hlPasswordDimenticata" runat="server" CssClass="skinLabelSmall" NavigateUrl="PasswordDimenticata.aspx">Nome Utente o Password dimenticata?</asp:HyperLink>
                                                            <br /><br />
                                                            <a href="https://assistenza.secoges.com" target="_blank">Strumenti di Assistenza Se.Co.Ges.</a>
                                                        </td>
                                                    </tr>
                                                </table>                                                
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

    
                    <asp:Panel ID="ModificaPasswordPanel" runat="server">
                        <div class="PannelloTestoGuida">
                            <asp:Label ID="lblTitoloModificaPassword" runat="server" CssClass="skinLabelMedium"></asp:Label>
                        </div>
                        <br />
                        <uc1:ModificaPasswordMan ID="ModificaPassword" runat="server" />
                    </asp:Panel>

                </td>
            </tr>
        </table>

        <div style="display:none;">
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableEmbeddedScripts="true" RegisterWithScriptManager="true"/>
            <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1" >
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="RadWindowManager1">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="RadWindowManager1" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
        </div>

    </form>
</body>

</html>
