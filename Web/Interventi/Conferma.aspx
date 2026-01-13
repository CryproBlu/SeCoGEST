<%@ Page Title="Conferma operazione" Language="C#" MasterPageFile="~/UI/Main.Master" AutoEventWireup="true" CodeBehind="Conferma.aspx.cs" Inherits="SeCoGEST.Web.Interventi.Conferma" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main" runat="server">
    <div style="padding: 120px; width: 100%; box-sizing: border-box; text-align: center; font-size: large;">
        <asp:Label runat="server" ID="lblMessaggio"></asp:Label>
        <br />
        <br />
        <br />
        <asp:HyperLink runat="server" ID="hlPageLink"></asp:HyperLink>
        <br />
        <br />
        <a href="/Interventi/Tickets.aspx">Mostra Elenco Tickets</a>
    </div>
</asp:Content>
