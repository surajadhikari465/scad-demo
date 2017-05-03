<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_ItemRequest_ItemScaleDetail" title="Untitled Page" Codebehind="ItemScaleDetail.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    &nbsp;<asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False"
        CellPadding="2" CellSpacing="2" DataKeyNames="ItemScaleRequest_ID" DataSourceID="SqlDataSource1"
        EmptyDataText="No Items Found" EnableViewState="False" Font-Bold="True" Font-Size="Small"
        HeaderText="Details View" Style="position: static" Width="125px">
        <Fields>
            <asp:BoundField DataField="ScaleDescription1" HeaderText="ScaleDescription1" SortExpression="ScaleDescription1" />
            <asp:BoundField DataField="ScaleDescription2" HeaderText="ScaleDescription2" SortExpression="ScaleDescription2" />
            <asp:BoundField DataField="ScaleDescription3" HeaderText="ScaleDescription3" SortExpression="ScaleDescription3" />
            <asp:BoundField DataField="ScaleDescription4" HeaderText="ScaleDescription4" SortExpression="ScaleDescription4" />
            <asp:BoundField DataField="ShelfLife" HeaderText="ShelfLife" SortExpression="ShelfLife" />
            <asp:BoundField DataField="ScaleUomUnit_ID" HeaderText="ScaleUomUnit_ID" SortExpression="ScaleUomUnit_ID" />
            <asp:BoundField DataField="ScaleRandomWeightType_ID" HeaderText="ScaleRandomWeightType_ID"
                SortExpression="ScaleRandomWeightType_ID" />
            <asp:BoundField DataField="Scale_Tare_ID" HeaderText="Scale_Tare_ID" SortExpression="Scale_Tare_ID" />
            <asp:BoundField DataField="Ingredients" HeaderText="Ingredients" SortExpression="Ingredients" />
            <asp:BoundField DataField="FixedWeight" HeaderText="FixedWeight" SortExpression="FixedWeight" />
            <asp:BoundField DataField="ByCount" HeaderText="ByCount" SortExpression="ByCount" />
        </Fields>
    </asp:DetailsView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="SELECT * FROM [ItemScaleRequest] WHERE ([ItemRequest_ID] = @ItemRequest_ID)">
        <SelectParameters>
            <asp:QueryStringParameter Name="ItemRequest_ID" QueryStringField="ItemRequest_ID"
                Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <br />
</asp:Content>

