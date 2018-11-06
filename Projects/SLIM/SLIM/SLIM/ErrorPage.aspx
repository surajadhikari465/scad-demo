<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.ErrorPage" title="Error Page" Codebehind="ErrorPage.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    &nbsp;<div style="width: 336px; position: static; height: 100px">
        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Medium" Font-Underline="True"
            ForeColor="Navy" Height="48px" Style="position: static" Text="Error - Error - Error"
            Width="288px"></asp:Label><br />
        <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="Medium" Font-Underline="True"
            ForeColor="Navy" Height="64px" Style="position: static" Text="The Requested Functionality is not available"
            Width="336px"></asp:Label></div>
    <div style="width: 448px; position: static; height: 112px">
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Medium" Font-Underline="True"
            ForeColor="Navy" Height="64px" Style="position: static" Text="Please report this Error to your HelpDesk"
            Width="336px"></asp:Label><br />
        <asp:Image ID="Image1" runat="server" Height="48px" ImageUrl="~/App_Themes/in00483_.gif"
            Style="position: static" Width="80px" />
        <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/UserInterface/Main.aspx"
            Style="position: static">Back to Main</asp:LinkButton></div>
</asp:Content>

