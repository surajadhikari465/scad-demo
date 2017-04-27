<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_Administration_UserAccess" title="SLIM User Access" Codebehind="UserAccess.aspx.vb" %>
<%@ MasterType VirtualPath="~/UserInterface/MasterPage.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="position: static">
        <tr>
            <td align="right" style="width: 100px">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Style="position: static" Text="Users:"
                    Width="56px"></asp:Label></td>
            <td style="width: 100px">
                <asp:DropDownList ID="UserDropDown" runat="server" AppendDataBoundItems="True" DataSourceID="ObjectDataSource1"
                    DataTextField="UserName" DataValueField="User_ID" Style="position: static">
                    <asp:ListItem Selected="True" Value="0">-- Select --</asp:ListItem>
                </asp:DropDownList></td>
            <td style="width: 100px">
                <asp:Button ID="Button1" runat="server" Style="position: static" Text="Add New" /></td>
        </tr>
        <tr>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
        </tr>
    </table>
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" DataKeyNames="SlimAccess_ID" DataSourceID="SqlDataSource1"
                    EmptyDataText="No Users Found" Style="position: static" EnableViewState="False" PageSize="50">
                    <Columns>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:BoundField DataField="UserName" HeaderText="Name" ReadOnly="True" SortExpression="UserName">
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:CheckBoxField DataField="UserAdmin" HeaderText="Admin" SortExpression="UserAdmin" />
                        <asp:CheckBoxField DataField="ItemRequest" HeaderText="ItemRequest" SortExpression="ItemRequest" />
                        <asp:CheckBoxField DataField="VendorRequest" HeaderText="VendorRequest" SortExpression="VendorRequest" />
                        <asp:CheckBoxField DataField="IRMAPush" HeaderText="IRMAPush" SortExpression="IRMAPush" />
                        <asp:CheckBoxField DataField="ScaleInfo" HeaderText="ScaleInfo" SortExpression="ScaleInfo" />
                        <asp:CheckBoxField DataField="StoreSpecials" HeaderText="StoreSpecials" SortExpression="StoreSpecials" />
                        <asp:CheckBoxField DataField="RetailCost" HeaderText="RetailCost" SortExpression="RetailCost" />
                        <asp:CheckBoxField DataField="Authorizations" HeaderText="Auth." SortExpression="Authorizations" />
                        <asp:CheckBoxField DataField="WebQuery" HeaderText="WebQuery" SortExpression="WebQuery" />
                        <asp:CommandField ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>
                <asp:Label ID="MsgLabel" runat="server" Font-Size="Medium" SkinID="SumLabel" Style="position: static"
                    Width="336px"></asp:Label>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="GetData" TypeName="SLIM.UsersTableAdapters.UsersTableAdapter"></asp:ObjectDataSource>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    DeleteCommand="DELETE FROM SlimAccess WHERE (SlimAccess_ID = @SlimAccess_ID)"
                    SelectCommand="SELECT Users.UserName, SlimAccess.UserAdmin, SlimAccess.ItemRequest, SlimAccess.VendorRequest, SlimAccess.ScaleInfo, SlimAccess.IRMAPush, SlimAccess.StoreSpecials, SlimAccess.RetailCost, SlimAccess.Authorizations, SlimAccess.WebQuery, SlimAccess.Insert_Date, SlimAccess.SlimAccess_ID FROM SlimAccess INNER JOIN Users ON SlimAccess.User_ID = Users.User_ID ORDER BY SlimAccess.Insert_Date DESC"
                    UpdateCommand="UPDATE SlimAccess SET UserAdmin = @UserAdmin, ItemRequest = @ItemRequest, VendorRequest = @VendorRequest, StoreSpecials = @StoreSpecials, RetailCost = @RetailCost, Authorizations = @Authorizations, WebQuery = @WebQuery, ScaleInfo = @ScaleInfo, IRMAPush = @IRMAPush&#13;&#10;where SlimAccess_ID = @SlimAccess_ID">
                    <DeleteParameters>
                        <asp:Parameter Name="SlimAccess_ID" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="UserAdmin" />
                        <asp:Parameter Name="ItemRequest" />
                        <asp:Parameter Name="VendorRequest" />
                        <asp:Parameter Name="StoreSpecials" />
                        <asp:Parameter Name="RetailCost" />
                        <asp:Parameter Name="Authorizations" />
                        <asp:Parameter Name="WebQuery" />
                        <asp:Parameter Name="IRMAPush" />
                        <asp:Parameter Name="SlimAccess_ID" />
                    </UpdateParameters>
                </asp:SqlDataSource>
    &nbsp;&nbsp;
</asp:Content>

