<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_ItemWiz" title="Untitled Page" Codebehind="ItemWiz.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
        AutoGenerateColumns="False" DataKeyNames="User_ID,UserName" DataSourceID="ObjectDataSource1"
        Style="position: static" EnableViewState="False">
        <Columns>
            <asp:CommandField ShowEditButton="True" />
            <asp:BoundField DataField="User_ID" HeaderText="User_ID" SortExpression="User_ID" />
            <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="UserName" />
            <asp:CommandField ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="GetData" TypeName="SLIM.UsersTableAdapters.UsersTableAdapter" UpdateMethod="UpdateUserAZ">
        <UpdateParameters>
            <asp:Parameter Name="User_ID" Type="Int32" />
        </UpdateParameters>
    </asp:ObjectDataSource>
    <asp:Label ID="Label1" runat="server" Font-Size="Small" SkinID="SumLabel" Style="position: static"
        Text="Label"></asp:Label>
</asp:Content>

