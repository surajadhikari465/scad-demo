<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_RetailCost_PendingPriceChanges" title="Untitled Page" Codebehind="PendingPriceChanges.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:DetailsView ID="DetailsView1" runat="server" AllowPaging="True" AutoGenerateRows="False"
        DataSourceID="SqlDataSource1" Height="50px" Style="position: static" Width="125px">
        <Fields>
            <asp:BoundField DataField="Store_Name" HeaderText="Store_Name" SortExpression="Store_Name">
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="StartDate" DataFormatString="{0:d}" HeaderText="StartDate"
                HtmlEncode="False" SortExpression="StartDate" />
            <asp:BoundField DataField="PriceChgTypeDesc" HeaderText="PriceChgTypeDesc" SortExpression="PriceChgTypeDesc" />
            <asp:BoundField DataField="PriceBatchStatusDesc" HeaderText="PriceBatchStatusDesc"
                SortExpression="PriceBatchStatusDesc" />
            <asp:BoundField DataField="Multiple" HeaderText="Multiple" SortExpression="Multiple" />
            <asp:BoundField DataField="Price" DataFormatString="{0:c}" HeaderText="Price" HtmlEncode="False"
                SortExpression="Price" />
            <asp:BoundField DataField="POSPrice" DataFormatString="{0:c}" HeaderText="POSPrice"
                HtmlEncode="False" SortExpression="POSPrice" />
            <asp:BoundField DataField="MSRPPrice" DataFormatString="{0:c}" HeaderText="MSRPPrice"
                HtmlEncode="False" SortExpression="MSRPPrice" />
            <asp:BoundField DataField="MSRPMultiple" HeaderText="MSRPMultiple" SortExpression="MSRPMultiple" />
        </Fields>
    </asp:DetailsView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="SELECT top 10 PBD.PriceBatchDetailID, PBD.Store_No, Store.Store_Name, PBD.StartDate, PBD.PriceChgTypeID, PriceChgType.PriceChgTypeDesc, ISNULL(PBH.PriceBatchStatusID, 0) AS PriceBatchStatusID, ISNULL(PBS.PriceBatchStatusDesc, '') AS PriceBatchStatusDesc, PBD.Multiple, PBD.Price, PBD.POSPrice, PBD.MSRPPrice, PBD.MSRPMultiple, PBD.PricingMethod_ID FROM PriceBatchDetail AS PBD WITH (nolock) INNER JOIN Store WITH (nolock) ON Store.Store_No = PBD.Store_No INNER JOIN PriceChgType WITH (nolock) ON PriceChgType.PriceChgTypeID = PBD.PriceChgTypeID LEFT OUTER JOIN PriceBatchHeader AS PBH WITH (nolock) ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID LEFT OUTER JOIN PriceBatchStatus AS PBS WITH (nolock) ON PBH.PriceBatchStatusID = PBS.PriceBatchStatusID WHERE (PBD.Item_Key = @Item_Key) AND (ISNULL(PBH.PriceBatchStatusID, 0) < 6) AND (Store.Store_No = @Store_No) AND (PBD.PriceChgTypeID = 1) ORDER BY PBD.StartDate, PBD.Insert_Date DESC">
        <SelectParameters>
            <asp:QueryStringParameter Name="Item_Key" QueryStringField="Item_Key" />
            <asp:SessionParameter Name="Store_No" SessionField="Store_No" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

