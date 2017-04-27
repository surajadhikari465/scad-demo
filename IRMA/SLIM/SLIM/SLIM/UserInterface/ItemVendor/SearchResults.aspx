<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_ItemVendor_SearchResults" title="Edit Item's Vendor Search Results" Codebehind="SearchResults.aspx.vb" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript" src="../../javascript/AutosizeWebGridColumns.js"></script>
<script language="javascript" >





function UltraWebGrid1_InitializeLayoutHandler(gridName){
	setColWidths(gridName);
}

</script>
    &nbsp;
    &nbsp;&nbsp;&nbsp;
    <igtbl:ultrawebgrid id="UltraWebGrid1" runat="server" datasourceid="SqlDataSource1" Browser="Xml"><Bands>
<igtbl:UltraGridBand>
<AddNewRow View="NotSet" Visible="NotSet"></AddNewRow>
<Columns>
    <igtbl:UltraGridColumn CellButtonDisplay="Always" Key="Select">
    </igtbl:UltraGridColumn>
    <igtbl:UltraGridColumn BaseColumnName="Item_Key" DataType="System.Int32" HeaderText="Item_Key"
        IsBound="True" Key="Item_Key">
        <Header Caption="Item_Key">
            <RowLayoutColumnInfo OriginX="1" />
        </Header>
        <Footer>
            <RowLayoutColumnInfo OriginX="1" />
        </Footer>
    </igtbl:UltraGridColumn>
    <igtbl:UltraGridColumn BaseColumnName="Identifier" HeaderText="Identifier" IsBound="True"
        Key="Identifier">
        <Header Caption="Identifier">
            <RowLayoutColumnInfo OriginX="2" />
        </Header>
        <Footer>
            <RowLayoutColumnInfo OriginX="2" />
        </Footer>
    </igtbl:UltraGridColumn>
    <igtbl:UltraGridColumn BaseColumnName="brand_name" HeaderText="brand_name" IsBound="True"
        Key="brand_name">
        <Header Caption="brand_name">
            <RowLayoutColumnInfo OriginX="3" />
        </Header>
        <Footer>
            <RowLayoutColumnInfo OriginX="3" />
        </Footer>
    </igtbl:UltraGridColumn>
    <igtbl:UltraGridColumn BaseColumnName="item_description" HeaderText="item_description"
        IsBound="True" Key="item_description">
        <Header Caption="item_description">
            <RowLayoutColumnInfo OriginX="4" />
        </Header>
        <Footer>
            <RowLayoutColumnInfo OriginX="4" />
        </Footer>
    </igtbl:UltraGridColumn>
    <igtbl:UltraGridColumn BaseColumnName="package_desc2" DataType="System.Decimal" HeaderText="package_desc2"
        IsBound="True" Key="package_desc2">
        <Header Caption="package_desc2">
            <RowLayoutColumnInfo OriginX="5" />
        </Header>
        <Footer>
            <RowLayoutColumnInfo OriginX="5" />
        </Footer>
    </igtbl:UltraGridColumn>
    <igtbl:UltraGridColumn BaseColumnName="unit_name" HeaderText="unit_name" IsBound="True"
        Key="unit_name">
        <Header Caption="unit_name">
            <RowLayoutColumnInfo OriginX="6" />
        </Header>
        <Footer>
            <RowLayoutColumnInfo OriginX="6" />
        </Footer>
    </igtbl:UltraGridColumn>
    <igtbl:UltraGridColumn BaseColumnName="subteam_name" HeaderText="subteam_name" IsBound="True"
        Key="subteam_name">
        <Header Caption="subteam_name">
            <RowLayoutColumnInfo OriginX="7" />
        </Header>
        <Footer>
            <RowLayoutColumnInfo OriginX="7" />
        </Footer>
    </igtbl:UltraGridColumn>
        <igtbl:UltraGridColumn BaseColumnName="Brand_Id" HeaderText="BrandId" IsBound="True" Hidden="True"
        Key="Brand_Id">
        <Header Caption="BrandId">
            <RowLayoutColumnInfo OriginX="8" />
        </Header>
        <Footer>
            <RowLayoutColumnInfo OriginX="8" />
        </Footer>
    </igtbl:UltraGridColumn>
</Columns>
    <RowEditTemplate>
        <br />
        <p align="center">
            <input id="igtbl_reOkBtn" onclick="igtbl_gRowEditButtonClick(event);" style="width: 50px"
                type="button" value="OK" />&nbsp;
            <input id="igtbl_reCancelBtn" onclick="igtbl_gRowEditButtonClick(event);" style="width: 50px"
                type="button" value="Cancel" /></p>
    </RowEditTemplate>
    <RowTemplateStyle BackColor="Window" BorderColor="Window" BorderStyle="Ridge">
        <BorderDetails WidthBottom="3px" WidthLeft="3px" WidthRight="3px" WidthTop="3px" />
    </RowTemplateStyle>
</igtbl:UltraGridBand>
</Bands>

<DisplayLayout ViewType="OutlookGroupBy" Version="4.00" AllowColSizingDefault="Free" Name="UltraWebGrid1" BorderCollapseDefault="Separate" ColWidthDefault="" TableLayout="Fixed" RowHeightDefault="20px" AllowColumnMovingDefault="NotSet" SelectTypeRowDefault="Extended" AllowSortingDefault="Yes" HeaderClickActionDefault="SortMulti" LoadOnDemand="Xml">
<GroupByBox>
<Style BorderColor="Window" BackColor="ActiveBorder"></Style>
</GroupByBox>

<GroupByRowStyleDefault BorderColor="Window" BackColor="Control"></GroupByRowStyleDefault>

<ActivationObject BorderWidth="" BorderColor=""></ActivationObject>

<FooterStyleDefault BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</FooterStyleDefault>

<RowStyleDefault BorderWidth="1px" BorderColor="Silver" BorderStyle="Solid" Font-Size="8.25pt" Font-Names="Microsoft Sans Serif" BackColor="Window">
<BorderDetails ColorTop="Window" ColorLeft="Window"></BorderDetails>

<Padding Left="3px"></Padding>
</RowStyleDefault>

<FilterOptionsDefault AllowRowFiltering="OnClient" FilterUIType="FilterRow">
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

<Pager MinimumPagesForDisplay="2" AllowPaging="True">
<Style BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</Style>
</Pager>

<AddNewBox>
<Style BorderWidth="1px" BorderColor="InactiveCaption" BorderStyle="Solid" BackColor="Window">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</Style>
</AddNewBox>
    <ClientSideEvents InitializeLayoutHandler="UltraWebGrid1_InitializeLayoutHandler" />
    <SelectedRowStyleDefault BackColor="#FFFFC0">
    </SelectedRowStyleDefault>
    <RowSelectorStyleDefault BackColor="#004000">
    </RowSelectorStyleDefault>
</DisplayLayout>
</igtbl:ultrawebgrid>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>" SelectCommand="GetItemWebQuery" SelectCommandType="StoredProcedure">
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
            <asp:QueryStringParameter DefaultValue="0" Name="Brand_ID" QueryStringField="b" Type="Int32" />
            <asp:QueryStringParameter DefaultValue="0" Name="Vendor_Id" QueryStringField="v"
                Type="Int32" />
            <asp:QueryStringParameter DefaultValue="*" Name="ExtraText" QueryStringField="x"
                Type="String" />
            <asp:Parameter Name="Team_Name" Type="String" />
            <asp:Parameter Name="SubTeam_Name" Type="String" />
            <asp:Parameter Name="Category_Name" Type="String" />
            <asp:Parameter Name="Level3_Name" Type="String" />
            <asp:Parameter Name="Level4_Name" Type="String" />
            <asp:Parameter Name="Brand_Name" Type="String" />
            <asp:Parameter Name="Vendor_Name" Type="String" />
            <asp:QueryStringParameter DefaultValue="" Name="StoreJurisdictionID" QueryStringField="j"
                Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

