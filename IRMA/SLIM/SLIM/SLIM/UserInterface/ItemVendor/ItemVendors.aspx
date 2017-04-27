<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_ItemVendor_ItemVendors" title="Authorizations" Codebehind="ItemVendors.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    &nbsp;<asp:Panel ID="Panel1" runat="server" Style="position: static">
        <table style="position: static">
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="Small" Font-Underline="True"
                        Style="position: static" Text="Store:"></asp:Label></td>
                <td style="width: 100px">
                    <asp:Label ID="Label6" runat="server" Font-Size="Small" Style="position: static"
                        Width="80px" Font-Bold="True"></asp:Label></td>
                <td style="width: 100px">
                </td>
                <td style="width: 105px">
                    <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="Small" Font-Underline="True"
                        Style="position: static" Text="Vendor:"></asp:Label></td>
                <td style="width: 105px">
                    <asp:DropDownList ID="VendorDropDown" runat="server" Style="position: static" DataSourceID="SqlDataSource1" DataTextField="CompanyName" DataValueField="Vendor_ID">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Small" Font-Underline="True"
                        Style="position: static" Text="UPC:"></asp:Label></td>
                <td style="width: 100px">
                    <asp:Label ID="Label3" runat="server" Style="position: static" Font-Size="Small" Font-Bold="True"></asp:Label></td>
                <td style="width: 100px">
                </td>
                <td style="width: 105px">
                    <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Size="Small" Font-Underline="True"
                        Style="position: static" Width="80px">VendorItem ID</asp:Label></td>
                <td style="width: 105px">
                    <asp:TextBox ID="WarehouseTxBx" runat="server" MaxLength="12" Style="position: static"
                        Width="100px"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Small" Font-Underline="True"
                        Style="position: static" Text="Description:"></asp:Label></td>
                <td style="width: 100px">
                    <asp:Label ID="Label4" runat="server" Style="position: static" Font-Size="Small" Font-Bold="True"></asp:Label></td>
                <td style="width: 100px">
                </td>
                <td style="width: 105px">
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="WarehouseTxBx"
                        ErrorMessage="Invalid VendorItem ID" Style="position: static" ValidationExpression="\w{0,12}\s?\w{0,12}"
                        Width="112px"></asp:RegularExpressionValidator></td>
                <td style="width: 105px">
                    <asp:Button ID="Button1" runat="server" Style="position: static" Text="Add Vendor" /></td>
            </tr>
        </table>
    </asp:Panel>
        <asp:GridView ID="GridView1" runat="server" AllowSorting="True"
            AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" DataSourceID="ObjectDataSource1"
            Style="position: static" EmptyDataText="No Items Found" DataKeyNames="Item_Key,Vendor_ID" EnableViewState="False">
            <Columns>
                <asp:BoundField DataField="CompanyName" HeaderText="Vendor" ReadOnly="True" SortExpression="CompanyName" >
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="Warehouse" HeaderText="Vendor Item ID" ReadOnly="True" SortExpression="Warehouse" >
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="Store_Name" HeaderText="Store" ReadOnly="True" SortExpression="Store_Name" >
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="Authorized" HeaderText="Authorized" ReadOnly="True" SortExpression="Authorized" >
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="PrimVen" HeaderText="Primary Vendor" ReadOnly="True" SortExpression="PrimVen" >
                    <ItemStyle Wrap="False" />
                </asp:BoundField>
                <asp:ButtonField CommandName="Select" DataTextField="Action1" >
                    <ItemStyle Wrap="False" />
                </asp:ButtonField>
            </Columns>
            <EditRowStyle Font-Size="X-Small" Width="50px" />
        </asp:GridView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetData" TypeName="ItemAuthorizationsTableAdapters.GetItemAuthorizationTableAdapter">
            <SelectParameters>
                <asp:SessionParameter DefaultValue="0" Name="Store_No" SessionField="Store_No" Type="Int32" />
                <asp:QueryStringParameter DefaultValue="" Name="ItemKey" QueryStringField="i"
                    Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>" SelectCommand="SELECT [Vendor_ID], [CompanyName] FROM [Vendor] ORDER BY [CompanyName]"></asp:SqlDataSource>
    &nbsp;
        <asp:Label ID="Label9" runat="server" Font-Size="Medium" Style="position: static" Width="152px" Font-Bold="True" ForeColor="Navy"></asp:Label>
        <asp:Label ID="Label10" runat="server" Font-Size="Medium" Style="position: static"
            Width="168px" Font-Bold="True" ForeColor="Navy"></asp:Label>
</asp:Content>

