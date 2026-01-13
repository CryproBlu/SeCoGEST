<%@ Page Title="Pagina principale" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SeCoGEST.Web.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #imageContainer {
            text-align:center;
        }

        #imageContainer img {
            border:none;
            margin-top:10%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    <%=SeCoGEST.Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">
    <div id="imageContainer">
        <img runat="server" id="imgWall" style="border:none;" 
            alt="<%#SeCoGEST.Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE %>" 
            title="<%#SeCoGEST.Infrastructure.ConfigurationKeys.TITOLO_APPLICAZIONE %>" 
            src="UI/Images/Wallpaper.png"/>
    </div>
</asp:Content>
