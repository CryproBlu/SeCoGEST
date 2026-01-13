<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserInfo.ascx.cs" Inherits="SeCoGEST.Web.UC.UserInfo" %>

<telerik:RadButton runat="server" ID="rbRefreshUserCache" ButtonType="LinkButton" ToolTip="Aggiorna cache utente" OnClick="rbRefreshUserCache_Click">
    <ContentTemplate>
        <img alt="" src="/UI/Images/User.gif" style="vertical-align:middle;" />&nbsp;<%= Page.User.Identity.Name %>
    </ContentTemplate>
</telerik:RadButton>

