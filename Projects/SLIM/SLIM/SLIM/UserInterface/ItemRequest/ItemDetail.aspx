<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_ItemRequest_ItemDetail" title="Item Detail" Codebehind="ItemDetail.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" CellPadding="2"
        CellSpacing="2" DataSourceID="SqlDataSource1"
        EmptyDataText="No Items Found" Font-Bold="True" Font-Size="Small"
        Style="position: static" Width="125px" HeaderText="Details View" DataKeyNames="ItemRequest_ID" EnableViewState="False">
        <Fields>
            <asp:BoundField DataField="Identifier" HeaderText="Identifier" SortExpression="Identifier" >
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="Item_Description" HeaderText="Item_Description" SortExpression="Item_Description">
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="POS_Description" HeaderText="POS_Description" SortExpression="POS_Description" />
            <asp:BoundField DataField="ItemSize" HeaderText="ItemSize" SortExpression="ItemSize" />
            <asp:BoundField DataField="Unit_Name" HeaderText="Unit_Name" ReadOnly="True" SortExpression="Unit_Name" />
            <asp:BoundField DataField="PackSize" HeaderText="PackSize" SortExpression="PackSize" />
            <asp:BoundField DataField="Price" DataFormatString="{0:c}" HeaderText="Price" HtmlEncode="False"
                SortExpression="Price" />
            <asp:BoundField DataField="PriceMultiple" HeaderText="PriceMultiple" SortExpression="PriceMultiple" DataFormatString="{0:n}" HtmlEncode="False" ReadOnly="True" >
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="CaseCost" DataFormatString="{0:c}" HeaderText="CaseCost"
                HtmlEncode="False" SortExpression="CaseCost" />
            <asp:BoundField DataField="CaseSize" HeaderText="CaseSize" SortExpression="CaseSize" />
            <asp:BoundField DataField="Category_Name" HeaderText="CategoryName" ReadOnly="True"
                SortExpression="Category_Name" />
            <asp:BoundField DataField="Brand_Name" HeaderText="BrandName" SortExpression="Brand_Name" ReadOnly="True" />
            <asp:BoundField DataField="BrandName" HeaderText="NewBrand" SortExpression="BrandName" />
            <asp:BoundField DataField="SubTeam_Name" HeaderText="SubTeam" ReadOnly="True" SortExpression="SubTeam_Name" />
            <asp:BoundField DataField="Vendor" HeaderText="Vendor" ReadOnly="True" SortExpression="Vendor" />
            <asp:BoundField DataField="CompanyName" HeaderText="NewVendor" ReadOnly="True" SortExpression="CompanyName" />
            <asp:BoundField DataField="Warehouse" HeaderText="VendorOrder" SortExpression="Warehouse" />
            <asp:BoundField DataField="Store_Name" HeaderText="Store_Name" ReadOnly="True" SortExpression="Store_Name" />
            <asp:BoundField DataField="UserName" HeaderText="UserName" ReadOnly="True" SortExpression="UserName" />
            <asp:CommandField ShowEditButton="True" />
        </Fields>
    </asp:DetailsView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="SELECT ItemRequest.Identifier, ItemRequest.Item_Description, ItemRequest.ItemSize, ItemRequest.PackSize, ItemRequest.Price, ItemRequest.PriceMultiple, ItemRequest.CaseCost, ItemRequest.CaseSize, Users.UserName, Store.Store_Name, VendorRequest.CompanyName, ItemRequest.POS_Description, ItemUnit.Unit_Name, Vendor.CompanyName AS Vendor, SubTeam.SubTeam_Name, ItemBrand.Brand_Name, ItemRequest.BrandName, ItemCategory.Category_Name, ItemRequest.ItemRequest_ID, ItemRequest.Warehouse FROM ItemRequest LEFT OUTER JOIN Users ON ItemRequest.User_ID = Users.User_ID LEFT OUTER JOIN Store ON ItemRequest.User_Store = Store.Store_No LEFT OUTER JOIN VendorRequest ON ItemRequest.VendorRequest_ID = VendorRequest.VendorRequest_ID LEFT OUTER JOIN ItemUnit ON ItemRequest.ItemUnit = ItemUnit.Unit_ID LEFT OUTER JOIN Vendor ON ItemRequest.VendorNumber = Vendor.Vendor_ID LEFT OUTER JOIN SubTeam ON ItemRequest.SubTeam_No = SubTeam.SubTeam_No  LEFT OUTER JOIN ItemBrand ON ItemRequest.Brand_ID = ItemBrand.Brand_ID LEFT OUTER JOIN ItemCategory ON ItemRequest.Category_ID = ItemCategory.Category_ID WHERE (ItemRequest.ItemRequest_ID = @ItemRequest_ID)" UpdateCommand="UPDATE ItemRequest SET Identifier = @Identifier,&#13;&#10;Item_Description = @Item_Description,&#13;&#10;POS_Description = UPPER(@POS_Description),&#13;&#10;ItemSize = @ItemSize,&#13;&#10;PackSize = @PackSize,&#13;&#10;Price =convert(smallmoney,@Price),&#13;&#10;CaseCost = convert(smallmoney,@CaseCost),&#13;&#10;CaseSize = @CaseSize,&#13;&#10;BrandName = @BrandName,&#13;&#10;Warehouse = @Warehouse&#13;&#10;WHERE (ItemRequest_ID = @ItemRequest_ID)">
        <SelectParameters>
            <asp:QueryStringParameter Name="ItemRequest_ID" QueryStringField="ItemRequest_ID" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="Identifier" />
            <asp:Parameter Name="Item_Description" />
            <asp:Parameter Name="POS_Description" />
            <asp:Parameter Name="ItemSize" />
            <asp:Parameter Name="PackSize" />
            <asp:Parameter Name="Price" />
            <asp:Parameter Name="CaseCost" />
            <asp:Parameter Name="CaseSize" />
            <asp:Parameter Name="BrandName" />
            <asp:Parameter Name="Warehouse" />
            <asp:Parameter Name="ItemRequest_ID" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>

