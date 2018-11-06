<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_WebQuery_ScaleItemDetail" title="Scale Item Detail" Codebehind="ScaleItemDetail.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" CellPadding="2" DataSourceID="SqlDataSource1" EmptyDataText="No Scale information exists for this item."
        EnableViewState="False" Font-Bold="True" Font-Size="12px" HeaderText="Scale Details View"
        Style="position: static" Font-Names="Tahoma">
        <Fields>
            <asp:BoundField DataField="Scale_Description1" HeaderText="Scale_Description1" SortExpression="Scale_Description1" />
            <asp:BoundField DataField="Scale_Description2" HeaderText="Scale_Description2" SortExpression="Scale_Description2" />
            <asp:BoundField DataField="Scale_Description3" HeaderText="Scale_Description3" SortExpression="Scale_Description3" />
            <asp:BoundField DataField="Scale_Description4" HeaderText="Scale_Description4" SortExpression="Scale_Description4" />
            <asp:BoundField DataField="Scale_ByCount" HeaderText="ByCount" SortExpression="Scale_ByCount" />
            <asp:BoundField DataField="Unit_Name" HeaderText="ScaleUOM" SortExpression="Unit_Name" />
            <asp:BoundField DataField="Scale_FixedWeight" HeaderText="FixedWeight" SortExpression="Scale_FixedWeight" />
            <asp:BoundField DataField="ShelfLife_Length" HeaderText="ShelfLife_Length" SortExpression="ShelfLife_Length" />
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
            <asp:BoundField DataField="ExtraText" HeaderText="Ingredients" SortExpression="ExtraText" />
        </Fields>
        <RowStyle BackColor="#C0FFC0" />
        <AlternatingRowStyle Font-Names="tahoma" Font-Size="12px" />
        <HeaderStyle BackColor="#004000" />
        <EmptyDataRowStyle BorderWidth="0" Font-Names="Tahoma" Font-Size="12px" Font-Bold="True" />
    </asp:DetailsView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="ItemWebQueryScaleDetail" SelectCommandType="storedProcedure">
        <SelectParameters>
            <asp:SessionParameter Name="Item_Key" SessionField="ItemKey" />
            <asp:SessionParameter Name="StoreJurisdictionID" SessionField="StoreJurisdictionID" />
        </SelectParameters>
    </asp:SqlDataSource>
    &nbsp;&nbsp;
</asp:Content>

