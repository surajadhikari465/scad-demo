<%@ Control Language="VB" AutoEventWireup="false" Inherits="ReportManagerWebApp.MenuControl" Codebehind="Menu.ascx.vb" %>
<asp:TreeView ID="MenuTreeView" runat="server" DataSourceID="MenuSource" ImageSet="Arrows" ExpandDepth="1" BackColor="LightYellow" BorderStyle="Solid" BorderWidth="1px">
    <DataBindings>
        <asp:TreeNodeBinding DataMember="menu" NavigateUrlField="url" TextField="name" />
        <asp:TreeNodeBinding DataMember="item" NavigateUrlField="url" TextField="name" />
    </DataBindings>
    <ParentNodeStyle Font-Bold="False" />
    <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
    <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px"
        VerticalPadding="0px" />
    <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="5px"
        NodeSpacing="0px" VerticalPadding="0px" />
</asp:TreeView>
<asp:XmlDataSource ID="MenuSource" runat="server"
    XPath="./navigation/menu"></asp:XmlDataSource>
