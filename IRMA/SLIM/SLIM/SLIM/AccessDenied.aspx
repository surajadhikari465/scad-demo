<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.AccessDenied" title="Untitled Page" Codebehind="AccessDenied.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label1" runat="server" SkinID="MessageLabelLarge" Style="position: static"
        Text="User Permission do not allow access to this Page or Functionality!"></asp:Label>
    <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/UserInterface/Main.aspx"
        Style="position: static">Back To Main</asp:LinkButton>
</asp:Content>

