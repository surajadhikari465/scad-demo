<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdminSettings.aspx.vb" Inherits="StoreOrderGuide.AdminSettingsInterface" MasterPageFile="~/StoreOrderGuide.Master" Title="Admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" Runat="Server">

<!--########## DATA SOURCES ########## -->
<asp:ObjectDataSource ID="dsAdminSettings" runat="server" SelectMethod="GetAdminSettings" TypeName="StoreOrderGuide.AdminSetting" DeleteMethod="DelAdminSetting" InsertMethod="AddAdminSetting" UpdateMethod="SetAdminSetting" >
    <UpdateParameters>
        <asp:ControlParameter ControlID="gvAdminSettingsList" Name="AdminKey" PropertyName="SelectedValue" Type="String" />
        <asp:ControlParameter ControlID="gvAdminSettingsList" Name="AdminValue" PropertyName="SelectedValue" Type="String" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="AdminKey" Type="String" />
        <asp:Parameter Name="AdminValue" Type="String" />
    </InsertParameters>
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="dsAdminSetting" runat="server" InsertMethod="AddAdminSetting" TypeName="StoreOrderGuide.AdminSetting">
    <InsertParameters>
        <asp:Parameter Name="AdminKey" Type="String" />
        <asp:Parameter Name="AdminValue" Type="String" />
    </InsertParameters>
</asp:ObjectDataSource>


<!--########## ADMIN SETTINGS ########## -->
<div id="adminsettinglist">
<asp:Panel ID="pnlAdminSettingList" runat="server" Visible="True">
    <div class="subtitle">Admin Settings</div>
    <asp:GridView ID="gvAdminSettingsList" runat="server" EmptyDataText="No administration settings were found." EnableViewState="False" SkinID="SOGGridView" AllowPaging="True" DataSourceID="dsAdminSettings" AllowSorting="True" DataKeyNames="AdminID">
        <Columns>
            <asp:CommandField ShowEditButton="True" ItemStyle-CssClass="cmdField" ItemStyle-ForeColor="#284775" />
            <asp:CommandField ShowDeleteButton="True" ItemStyle-CssClass="cmdField" ItemStyle-ForeColor="#284775" />
            <asp:BoundField DataField="AdminKey" HeaderText="AdminKey" SortExpression="AdminKey" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:BoundField DataField="AdminValue" HeaderText="AdminValue" SortExpression="AdminValue" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
        </Columns>
    </asp:GridView>

    <asp:Button ID="btnAddAdminSetting" runat="server" SkinID="SOGButton" Text="Add" />
</asp:Panel>
</div>


<!--########## ADMIN SETTING DETAILS ########## -->
<div id="adminsettingdetails">
<asp:Panel ID="pnlAdminSettingDetails" runat="server" Visible="False">
    <div class="subtitle">Admin Details</div>
    <asp:DetailsView ID="dvAdminSettingDetail" runat="server" EnableViewState="false" SkinID="SOGDetailView" AutoGenerateRows="False" DefaultMode="Edit" DataSourceID="dsAdminSettings" DataKeyNames="AdminID">
        <Fields>
            <asp:BoundField DataField="AdminID" HeaderText="AdminID" ReadOnly="True" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:BoundField DataField="AdminKey" HeaderText="AdminKey" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:BoundField DataField="AdminValue" HeaderText="AdminValue" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:CommandField ShowInsertButton="True" />
        </Fields>
    </asp:DetailsView>
</asp:Panel>
</div>

</asp:Content>