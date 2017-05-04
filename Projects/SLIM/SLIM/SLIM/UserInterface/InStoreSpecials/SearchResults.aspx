<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_InStoreSpecials_SearchResult" title="Search Results" Codebehind="../../UserInterface/InStoreSpecials/SearchResults.aspx.vb" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script type="text/javascript">
function UltraWebGrid1_InitializeLayoutHandler(gridName) {

}
</script>

    <asp:Button ID="Button_Submit" runat="server" Text="Submit"/>
    <br />
    <br />
    <asp:CheckBox ID="CheckBox_All" runat="server" AutoPostBack="True" Font-Names="Tahoma"
        Font-Size="10pt" Text="All" /><br />
    <igtbl:UltraWebGrid ID="UltraWebGrid1" runat="server" Browser="Xml" DataSourceID="SqlDataSource1">
        <Bands>
            <igtbl:UltraGridBand>
                <AddNewRow View="NotSet" Visible="NotSet">
                </AddNewRow>
                <Columns>
                    <igtbl:UltraGridColumn AllowUpdate="Yes" Key="Select" Type="CheckBox">
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="Item_Key" DataType="System.Int32" Hidden="True"
                        IsBound="True" Key="Item_Key">
                        <Header Caption="Item_Key">
                            <RowLayoutColumnInfo OriginX="1" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="1" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="Store_No" DataType="System.Int32" IsBound="True"
                        Key="Store_No">
                        <Header Caption="Store No">
                            <RowLayoutColumnInfo OriginX="2" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="2" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="Identifier" IsBound="True" Key="Identifier">
                        <Header Caption="UPC">
                            <RowLayoutColumnInfo OriginX="3" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="3" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="brand_name" IsBound="True" Key="brand_name">
                        <Header Caption="Brand">
                            <RowLayoutColumnInfo OriginX="4" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="4" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="item_description" IsBound="True" Key="item_description">
                        <Header Caption="Description">
                            <RowLayoutColumnInfo OriginX="5" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="5" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="subteam_abbreviation" IsBound="True" Key="subteam_abbreviation">
                        <Header Caption="Subteam">
                            <RowLayoutColumnInfo OriginX="6" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="6" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="package_desc1" DataType="System.Decimal" IsBound="True"
                        Key="package_desc1">
                        <Header Caption="Pack">
                            <RowLayoutColumnInfo OriginX="7" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="7" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="package_desc2" DataType="System.Decimal" IsBound="True"
                        Key="package_desc2">
                        <Header Caption="Item Size">
                            <RowLayoutColumnInfo OriginX="8" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="8" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="unit_abbreviation" IsBound="True" Key="unit_abbreviation">
                        <Header Caption="UOM">
                            <RowLayoutColumnInfo OriginX="9" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="9" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="price" DataType="System.Decimal" Format="$ ###,###,##0.00"
                        IsBound="True" Key="price">
                        <Header Caption="Price">
                            <RowLayoutColumnInfo OriginX="10" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="10" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="multiple" DataType="System.Byte" IsBound="True"
                        Key="multiple">
                        <Header Caption="Mult">
                            <RowLayoutColumnInfo OriginX="11" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="11" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="StoreJurisdictionDesc" IsBound="True" Key="StoreJurisdictionDesc">
                        <Header Caption="Jurisdiction">
                            <RowLayoutColumnInfo OriginX="12" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="12" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="primaryvendor" DataType="System.Boolean" IsBound="True"
                        Key="primaryvendor" Type="CheckBox">
                        <Header Caption="Primary">
                            <RowLayoutColumnInfo OriginX="13" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="13" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="Identifier_ID" Hidden="True" IsBound="True"
                        Key="Identifier_ID">
                        <Header>
                            <RowLayoutColumnInfo OriginX="14" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="14" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                </Columns>
            </igtbl:UltraGridBand>
        </Bands>
       <DisplayLayout Version="4.00" AllowColSizingDefault="Free" Name="UltraWebGrid1" BorderCollapseDefault="Separate" ColWidthDefault="" TableLayout="Fixed" RowHeightDefault="20px" AllowColumnMovingDefault="NotSet" SelectTypeRowDefault="Extended" AllowSortingDefault="Yes" HeaderClickActionDefault="SortMulti" LoadOnDemand="Xml">

<GroupByRowStyleDefault BorderColor="Window" BackColor="Control"></GroupByRowStyleDefault>

<ActivationObject BorderWidth="" BorderColor=""></ActivationObject>

<FooterStyleDefault BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</FooterStyleDefault>

<RowStyleDefault BorderWidth="1px" BorderColor="Silver" BorderStyle="Solid" Font-Size="8.25pt" Font-Names="Microsoft Sans Serif" BackColor="Window">
<BorderDetails ColorTop="Window" ColorLeft="Window"></BorderDetails>

<Padding Left="3px"></Padding>
</RowStyleDefault>

<FilterOptionsDefault FilterUIType="FilterRow" AllowRowFiltering="OnClient">
<FilterOperandDropDownStyle BorderWidth="1px" BorderColor="Silver" BorderStyle="Solid" Font-Size="11px" Font-Names="Verdana,Arial,Helvetica,sans-serif" BackColor="White" CustomRules="overflow:auto;">
<Padding Left="2px"></Padding>
</FilterOperandDropDownStyle>

<FilterHighlightRowStyle ForeColor="White" BackColor="#151C55"></FilterHighlightRowStyle>

<FilterDropDownStyle BorderWidth="1px" BorderColor="Silver" BorderStyle="Solid" Font-Size="11px" Font-Names="Verdana,Arial,Helvetica,sans-serif" BackColor="White" Width="200px" Height="300px" CustomRules="overflow:auto;">
<Padding Left="2px"></Padding>
</FilterDropDownStyle>
</FilterOptionsDefault>

<HeaderStyleDefault HorizontalAlign="Left" BorderStyle="Solid" BackColor="#004000" ForeColor="White">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</HeaderStyleDefault>
<RowAlternateStyleDefault BackColor="#C0FFC0"></RowAlternateStyleDefault>

<EditCellStyleDefault BorderWidth="0px" BorderStyle="None"></EditCellStyleDefault>

<FrameStyle BorderWidth="1px" BorderColor="InactiveCaption" BorderStyle="Solid" Font-Size="12px" Font-Names="tahoma" BackColor="Window"></FrameStyle>


<Pager MinimumPagesForDisplay="2" AllowPaging="True" PagerAppearance="Both" PageSize="50" QuickPages="5">
</Pager>
    <ClientSideEvents InitializeLayoutHandler="UltraWebGrid1_InitializeLayoutHandler" />
    <SelectedRowStyleDefault BackColor="#FFFFC0">
    </SelectedRowStyleDefault>
    <RowSelectorStyleDefault BackColor="#004000" Cursor="Hand" ForeColor="White">
    </RowSelectorStyleDefault>
</DisplayLayout>
    </igtbl:UltraWebGrid>
    &nbsp;&nbsp;
    <asp:Label ID="Label_RowCount" runat="server" Font-Bold="True" Font-Size="X-Small"
        Text="Label" Visible="False"></asp:Label>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="GetItemWebQueryStore" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="*" Name="Identifier" QueryStringField="u"
                Type="String" />
            <asp:QueryStringParameter DefaultValue="*" Name="Item_Description" QueryStringField="d"
                Type="String" />
            <asp:QueryStringParameter DefaultValue="0" Name="Team_No" QueryStringField="t" Type="Int32" />
            <asp:QueryStringParameter DefaultValue="0" Name="SubTeam_No" QueryStringField="s"
                Type="Int32" />
            <asp:QueryStringParameter DefaultValue="0" Name="Category_No" QueryStringField="c"
                Type="Int32" />
            <asp:QueryStringParameter DefaultValue="0" Name="Level3_Hierarchy_ID" QueryStringField="3"
                Type="Int32" />
            <asp:QueryStringParameter DefaultValue="0" Name="Level4_Hierarchy_ID" QueryStringField="4"
                Type="Int32" />
            <asp:QueryStringParameter DefaultValue="0" Name="Vendor_Id" QueryStringField="v"
                Type="Int32" />
            <asp:QueryStringParameter DefaultValue="*" Name="ExtraText" QueryStringField="x"
                Type="String" />
            <asp:QueryStringParameter DefaultValue="0" Name="Brand_ID" QueryStringField="b"
                Type="Int32" />
            <asp:SessionParameter DefaultValue="0" Name="Store_No" SessionField="Store_No" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <br />
    <asp:Label ID="Label_Message" runat="server" Font-Bold="False" Font-Size="12px" Font-Underline="True"
        Style="position: static" Width="433px" Font-Names="Tahoma">No results can be found that match your search criteria.</asp:Label>
</asp:Content>

