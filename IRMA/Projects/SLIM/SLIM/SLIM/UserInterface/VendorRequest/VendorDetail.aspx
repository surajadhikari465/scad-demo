<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_VendorRequest_VendorDetail" title="Vendor Detail" Codebehind="VendorDetail.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:DetailsView ID="DetailsView1" runat="server" 
        AutoGenerateRows="False" 
        CellPadding="2"
        CellSpacing="2" 
        DataKeyNames="VendorRequest_ID"
        DataSourceID="SqlDataSource1"
        EmptyDataText="No Items Found" 
        Font-Bold="True" 
        Font-Size="Small" 
        HeaderText="Details View" 
        Height="1px" 
        Style="position: static" 
        Width="125px" >
        <Fields>
            <asp:BoundField DataField="CompanyName" HeaderText="Vendor" SortExpression="CompanyName">
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="Address_Line_1" HeaderText="Address 1" SortExpression="Address_Line_1">
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="Address_Line_2" HeaderText="Address 2" SortExpression="Address_Line_2">
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="City" HeaderText="City" SortExpression="City">
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" />
            <asp:BoundField DataField="ZipCode" HeaderText="ZipCode" SortExpression="ZipCode" />
            <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone" />
            <asp:BoundField DataField="Fax" HeaderText="Fax" SortExpression="Fax" />
            <asp:BoundField DataField="InsuranceNumber" HeaderText="Insurance Number" SortExpression="InsuranceNumber">
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email">
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="Store_Name" HeaderText="Store_Name" ReadOnly="True" SortExpression="Store_Name" />
            <asp:BoundField DataField="UserName" HeaderText="UserName" ReadOnly="True" SortExpression="UserName" />
            <asp:CommandField ShowEditButton="True" />
        </Fields>
    </asp:DetailsView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="SELECT VendorRequest.CompanyName, VendorRequest.Address_Line_1, VendorRequest.Address_Line_2, VendorRequest.City, VendorRequest.State, VendorRequest.ZipCode, VendorRequest.Phone, VendorRequest.Fax, VendorRequest.InsuranceNumber, VendorRequest.Email, VendorRequest.VendorRequest_ID, Store.Store_Name, Users.UserName FROM VendorRequest INNER JOIN Store ON VendorRequest.User_Store = Store.Store_No INNER JOIN Users ON VendorRequest.User_ID = Users.User_ID WHERE (VendorRequest.VendorRequest_ID = @VendorRequest_ID)" 
        UpdateCommand="UPDATE VendorRequest SET CompanyName = @CompanyName, Address_Line_1 = @Address_Line_1, Address_Line_2 = @Address_Line_2, City = @City, State = @State, ZipCode = @ZipCode, Phone = @Phone, Fax=@Fax, InsuranceNumber = @InsuranceNumber, Email = @Email&#13;&#10;where VendorRequest_ID=@VendorRequest_ID">
        <SelectParameters>
            <asp:QueryStringParameter Name="VendorRequest_ID" QueryStringField="VendorRequest_ID" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="CompanyName" />
            <asp:Parameter Name="Address_Line_1" />
            <asp:Parameter Name="Address_Line_2" />
            <asp:Parameter Name="City" />
            <asp:Parameter Name="State" />
            <asp:Parameter Name="ZipCode" />
            <asp:Parameter Name="Phone" />
            <asp:Parameter Name="InsuranceNumber" />
            <asp:Parameter Name="Email" />
            <asp:Parameter Name="VendorRequest_ID" />
            <asp:Parameter Name="Fax" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>

