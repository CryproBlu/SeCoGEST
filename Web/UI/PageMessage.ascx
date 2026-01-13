<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageMessage.ascx.cs" Inherits="SeCoGEST.Web.UI.PageMessage" %>

<style>
    .pageMessagePanel{
        /*margin: 10px;*/
        /*margin-top: 10px;
        margin-bottom: 10px;*/
        padding-left: 10px;
        padding-right: 10px;
        padding-bottom: 10px;
    }

    .pageMessageColorNote{
        background-color: #D9F0F7;
        color: #1999CE;
    }

    .pageMessageColorCaution{
        background-color: #EDDEE1;
        color: #881F33; 
    }

    .pageMessageColorImportant{
        background-color: #F3F0EB;
        color: #D2883F; 
    }

    .pageMessageColorTip{
        background-color: #DEF4E4;
        color: #24B24B; 
    }
    
    .pageMessageIcon{
        padding:0px;
        vertical-align:bottom;
    }

    .pageMessageText{
        vertical-align:middle;
    }

</style>

<telerik:RadPageLayout runat="server" Width="100%" HtmlTag="None" GridType="Fluid"> 
    <Rows>
        <telerik:LayoutRow runat="server" ID="lrOpenClose" RowType="Region" CssClass="pageMessageColorNote">
            <Columns>
                <telerik:LayoutColumn>
                    <div runat="server" id="divOpenClose" style="width:100%; border:none; font-size:xx-small;" title="Mostra/Nascondi messaggio" >&nbsp;</div>
                </telerik:LayoutColumn>
            </Columns>
        </telerik:LayoutRow>
        <telerik:LayoutRow runat="server" ID="lrPanel" RowType="Region" CssClass="pageMessagePanel pageMessageColorNote">
            <Columns>
                <telerik:LayoutColumn Span="0" HiddenXs="true" CssClass="pageMessageIcon">
                    <asp:Image runat="server" ID="imgIcon" ImageUrl="~/UI/Images/Messages/Note.png" ImageAlign="AbsMiddle" />
                </telerik:LayoutColumn>
                <telerik:LayoutColumn Span="10" CssClass="pageMessageText" >
                    <asp:Label runat="server" ID="lblTitle" CssClass="pageMessageText"></asp:Label>
                    <asp:Label runat="server" ID="lblMessage" CssClass="pageMessageText"></asp:Label>
                </telerik:LayoutColumn>
            </Columns>
        </telerik:LayoutRow>
    </Rows>
</telerik:RadPageLayout>