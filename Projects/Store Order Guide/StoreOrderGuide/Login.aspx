<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/StoreOrderGuide.Master" CodeBehind="Login.aspx.vb" Inherits="StoreOrderGuide.Login" title="Login" %>
<asp:Content id="Content1" contentplaceholderid="mainContent" runat="server">
    <asp:Login ID="Login1" runat="server" SkinID="SOGLogin" DisplayRememberMe="false" DestinationPageUrl="~/Default.aspx" />
</asp:Content>