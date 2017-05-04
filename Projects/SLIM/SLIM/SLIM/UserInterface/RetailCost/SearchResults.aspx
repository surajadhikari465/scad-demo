<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_RetailCost_SearchResults" title="Retail Cost Search Results" Codebehind="SearchResults.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="GridView1" runat="server" Style="position: static" DataSourceID="SqlDataSource1" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" EmptyDataText="No Items Found!" CellPadding="2" CellSpacing="2" DataKeyNames="Item_Key" PageSize="20">
        <EmptyDataRowStyle BackColor="White" />
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="Identifier" HeaderText="UPC" SortExpression="Identifier">
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="brand_name" HeaderText="Brand" SortExpression="brand_name">
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="item_description" HeaderText="Desc" SortExpression="item_description">
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="package_desc1" DataFormatString="{0:n}" HeaderText="Pack"
                HtmlEncode="False" SortExpression="package_desc1" />
            <asp:BoundField DataField="package_desc2" DataFormatString="{0:n}" HeaderText="ItemSize"
                HtmlEncode="False" SortExpression="package_desc2" />
            <asp:BoundField DataField="unit_abbreviation" HeaderText="UOM" SortExpression="unit_abbreviation" />
            <asp:BoundField DataField="price" DataFormatString="{0:c}" HeaderText="Price" HtmlEncode="False"
                SortExpression="price" />
            <asp:BoundField DataField="multiple" HeaderText="Mult" SortExpression="multiple" />
            <asp:CheckBoxField DataField="primaryvendor" HeaderText="Primary" SortExpression="primaryvendor" />
        </Columns>
    </asp:GridView>
    &nbsp;&nbsp;
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>" SelectCommand="GetItemWebQueryStore" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="*" Name="Identifier" QueryStringField="UPC"
                Type="String" />
            <asp:QueryStringParameter Name="Item_Description" QueryStringField="Description"
                Type="String" DefaultValue="*" />
            <asp:Parameter DefaultValue="0" Name="Team_no" />
            <asp:QueryStringParameter DefaultValue="0" Name="SubTeam_No" QueryStringField="Dept"
                Type="Int32" />
            <asp:Parameter DefaultValue="0" Name="Category_No" />
            <asp:Parameter DefaultValue="0" Name="Level3_Hierarchy_ID" />
            <asp:Parameter DefaultValue="0" Name="Level4_Hierarchy_ID" />
            <asp:Parameter DefaultValue="0" Name="Vendor_Id" />
            <asp:Parameter DefaultValue="*" Name="ExtraText" />
            <asp:QueryStringParameter DefaultValue="0" Name="Brand_ID" QueryStringField="Brand"
                Type="Int32" />
            <asp:SessionParameter DefaultValue="0" Name="Store_No" SessionField="Store_no" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:Label ID="Label1" runat="server" Font-Bold="False" Font-Size="X-Small" Style="position: static"
        Width="256px" Font-Underline="True">7</asp:Label>
</asp:Content>

