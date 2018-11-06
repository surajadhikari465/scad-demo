<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_VendorRequest_VendorProcessed" title="Processed Vendor Requests" Codebehind="VendorProcessed.aspx.vb" %>

<%@ Register TagPrefix="igtblexp" Namespace="Infragistics.WebUI.UltraWebGrid.ExcelExport" Assembly="Infragistics2.WebUI.UltraWebGrid.ExcelExport.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ MasterType VirtualPath="~/UserInterface/MasterPage.master"%>   
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script type="text/javascript" src="../../javascript/AutosizeWebGridColumns.js"></script>
<script type="text/javascript"language="javascript">
function UltraGrid1_InitializeLayoutHeader(gridName){
	setColWidths(gridName);
}


</script>

    <igtblexp:ultrawebgridexcelexporter id="UltraWebGridExcelExporter1" runat="server"
        downloadname="ProcessedVendors.XLS" worksheetname="ProcessedVendors"></igtblexp:ultrawebgridexcelexporter>
    <img border="0" src="../../images/page_excel.png" />
    <asp:LinkButton ID="LinkButton1" runat="server" Font-Names="tahoma" Font-Size="Small">Excel</asp:LinkButton>&nbsp;<br />
    &nbsp;<asp:Label ID="Label_Message" runat="server" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="12px"></asp:Label>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="SELECT [Vendor_Key], [CompanyName], [Address_Line_1],[Address_Line_2], [City], [State], [ZipCode], [Phone], [PS_Vendor_ID], [PS_Export_Vendor_ID], [IRMA_Add_Date] FROM [VendorRequest] WHERE ([VendorStatus_ID] = @VendorStatus_ID) ORDER BY [IRMA_Add_Date] DESC">
        <SelectParameters>
            <asp:Parameter DefaultValue="3" Name="VendorStatus_ID" Type="Int16" />
        </SelectParameters>
    </asp:SqlDataSource>
    <igtbl:UltraWebGrid ID="UltraWebGrid1" runat="server" Browser="Xml" DataSourceID="SqlDataSource1"><Bands>
<igtbl:UltraGridBand>
<AddNewRow View="NotSet" Visible="NotSet"></AddNewRow>
</igtbl:UltraGridBand>
</Bands>

<DisplayLayout ViewType="OutlookGroupBy" Version="4.00" AllowColSizingDefault="Free" Name="UltraWebGrid1" BorderCollapseDefault="Separate" ColWidthDefault="" TableLayout="Fixed" RowHeightDefault="20px" AllowColumnMovingDefault="NotSet" SelectTypeRowDefault="Extended" LoadOnDemand="Xml">
<GroupByBox Hidden="True">
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

<HeaderStyleDefault HorizontalAlign="Left" BorderStyle="Solid" BackColor="DarkGreen" ForeColor="White">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</HeaderStyleDefault>

<RowAlternateStyleDefault BackColor="#C0FFC0"></RowAlternateStyleDefault>

<EditCellStyleDefault BorderWidth="0px" BorderStyle="None"></EditCellStyleDefault>

<FrameStyle BorderWidth="1px" BorderColor="InactiveCaption" BorderStyle="Solid" Font-Size="12px" Font-Names="tahoma" BackColor="Window"></FrameStyle>

<Pager MinimumPagesForDisplay="2" AllowPaging="True" StyleMode="customlabels" Pattern="[currentpageindex] of [pagecount] : [page:first:First] [prev] [next] [page:last:Last]" PageSize="50" PagerAppearance="both">
<Style BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</Style>
</Pager>

<AddNewBox>
<Style BorderWidth="1px" BorderColor="InactiveCaption" BorderStyle="Solid" BackColor="Window">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</Style>
</AddNewBox>
    <ClientSideEvents InitializeLayoutHandler="UltraGrid1_InitializeLayoutHeader" />
    <SelectedRowStyleDefault BackColor="#FFFFC0">
    </SelectedRowStyleDefault>
    <RowSelectorStyleDefault BackColor="DarkGreen">
    </RowSelectorStyleDefault>
</DisplayLayout>
</igtbl:UltraWebGrid>
</asp:Content>

