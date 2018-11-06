<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_ItemVendor_SearchResults_old" title="Authorizations Search Results" Codebehind="SearchResults_old.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
        AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" DataSourceID="SqlDataSource1"
        PageSize="20" Style="position: static" DataKeyNames="Item_Key">
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="Identifier" HeaderText="UPC" ReadOnly="True" SortExpression="Identifier" />
            <asp:BoundField DataField="brand_name" HeaderText="Brand" SortExpression="brand_name">
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="item_description" HeaderText="Description" SortExpression="item_description">
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="package_desc2" DataFormatString="{0:n}" HeaderText="Size"
                HtmlEncode="False" SortExpression="package_desc2" />
            <asp:BoundField DataField="unit_name" HeaderText="UOM" SortExpression="unit_name" />
            <asp:BoundField DataField="subteam_name" HeaderText="Dept" SortExpression="subteam_name">
                <ItemStyle Wrap="False" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="GetItemWebQuery" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="*" Name="Identifier" QueryStringField="UPC"
                Type="String" />
            <asp:QueryStringParameter DefaultValue="*" Name="Item_Description" QueryStringField="Description"
                Type="String" />
            <asp:QueryStringParameter DefaultValue="0" Name="SubTeam_No" QueryStringField="Dept"
                Type="Int32" />
            <asp:QueryStringParameter DefaultValue="0" Name="Brand_ID" QueryStringField="Brand"
                Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:Label ID="Label1" runat="server" Font-Bold="False" Font-Size="Small" Font-Underline="True"
        Style="position: static" Width="256px"></asp:Label>
</asp:Content>

