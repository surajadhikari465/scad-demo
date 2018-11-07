<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_ItemRequest_ItemPending" title="Item Requests Pending" Codebehind="ItemPending.aspx.vb" %>
<%@ Register TagPrefix="igtblexp" Namespace="Infragistics.WebUI.UltraWebGrid.ExcelExport" Assembly="Infragistics2.WebUI.UltraWebGrid.ExcelExport.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
    <%@ MasterType VirtualPath="~/UserInterface/MasterPage.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" src="../../javascript/AutosizeWebGridColumns.js"></script>
<script type="text/javascript" >

function UltraWebGrid1_InitializeLayoutHandler(gridName){
	setColWidths(gridName);
}

function popitup(url) {
	newwindow=window.open(url,'name','height=320,width=250');
	if (window.focus) {newwindow.focus()}
	
}

</script>
    <asp:Label ID="MsgLabel" runat="server" Style="position: static" Font-Bold="True" Font-Size="Medium" Font-Names="Tahoma" SkinID="MessageLabelNormal"></asp:Label><br />
    <br />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        DeleteCommand="delete from ItemRequest where ItemRequest_ID = @ItemRequest_ID"
        SelectCommand="SELECT ItemRequest_ID, Identifier, Item_Description, ItemSize, Price, PriceMultiple, CaseCost, CaseSize, Insert_Date, Ready_To_Apply, Warehouse, SubTeam.SubTeam_Name, RequestedBy, ProcessedBy, Comments FROM ItemRequest INNER JOIN SubTeam ON ItemRequest.SubTeam_No = SubTeam.SubTeam_No WHERE (ItemStatus_ID = 1) ORDER BY Insert_Date DESC">
        <DeleteParameters>
            <asp:Parameter Name="ItemRequest_ID" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <img alt="" src="../../images/page_excel.png" style="border-right: 0px; border-top: 0px;
        border-left: 0px; border-bottom: 0px" /><asp:LinkButton ID="LinkButton1" runat="server"
            Font-Names="tahoma" Font-Size="Small">Excel</asp:LinkButton><br />
    <igtblexp:ultrawebgridexcelexporter id="UltraWebGridExcelExporter1" runat="server"
        downloadname="ItemsPending.XLS" worksheetname="ItemRequestPending"></igtblexp:ultrawebgridexcelexporter>
    <igtbl:ultrawebgrid ID="UltraWebGrid1" runat="server" DataMember="DefaultView" DataSourceID="SqlDataSource1" style="left: 73px; top: -2px" DataKeyField="ItemRequest_ID">
        <Bands>
            <igtbl:UltraGridBand>
                <Columns>
                    <igtbl:UltraGridColumn CellButtonDisplay="Always" Key="PushToEIM" Type="Button">
                        <CellButtonStyle Font-Bold="True" Font-Size="10px" Height="17px" Width="80px">
                        </CellButtonStyle>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn CellButtonDisplay="Always" Key="ItemDetail">
                        <Header>
                            <RowLayoutColumnInfo OriginX="1" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="1" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn CellButtonDisplay="Always" Key="ScaleDetail">
                        <Header>
                            <RowLayoutColumnInfo OriginX="2" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="2" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="ItemRequest_ID" DataType="System.Int32" HeaderText="ItemRequest_ID"
                        Hidden="True" IsBound="True" Key="ItemRequest_ID">
                        <Header Caption="ItemRequest_ID">
                            <RowLayoutColumnInfo OriginX="3" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="3" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="Item_Key" DataType="System.Int32" HeaderText="Item_Key"
                        Hidden="True" IsBound="True" Key="Item_Key">
                        <Header Caption="Item_Key">
                            <RowLayoutColumnInfo OriginX="4" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="4" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="Identifier" HeaderText="UPC" IsBound="True"
                        Key="Identifier">
                        <Header Caption="UPC">
                            <RowLayoutColumnInfo OriginX="5" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="5" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="Item_Description" HeaderText="Item Description"
                        IsBound="True" Key="Item_Description">
                        <Header Caption="Item Description">
                            <RowLayoutColumnInfo OriginX="6" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="6" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="ItemSize" HeaderText="ItemSize" IsBound="True"
                        Key="ItemSize">
                        <Header Caption="ItemSize">
                            <RowLayoutColumnInfo OriginX="7" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="7" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="Price" DataType="System.Decimal" Format="$#####0.00"
                        HeaderText="Price" IsBound="True" Key="Price">
                        <Header Caption="Price">
                            <RowLayoutColumnInfo OriginX="8" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="8" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="PriceMultiple" DataType="System.Int32" HeaderText="Multiple"
                        IsBound="True" Key="PriceMultiple">
                        <Header Caption="Multiple">
                            <RowLayoutColumnInfo OriginX="9" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="9" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="CaseCost" DataType="System.Decimal" Format="$#####0.00"
                        HeaderText="Cost" IsBound="True" Key="CaseCost">
                        <Header Caption="Cost">
                            <RowLayoutColumnInfo OriginX="10" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="10" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="CaseSize" DataType="System.Int32" HeaderText="CaseSize"
                        IsBound="True" Key="CaseSize">
                        <Header Caption="CaseSize">
                            <RowLayoutColumnInfo OriginX="11" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="11" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="Ready_To_Apply" DataType="System.Boolean"
                        HeaderText="Ready to Apply" IsBound="True" Key="Ready_To_Apply" Type="CheckBox">
                        <Header Caption="Ready to Apply">
                            <RowLayoutColumnInfo OriginX="12" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="12" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="Insert_Date" DataType="System.DateTime" Format="MM/dd/yyyy"
                        HeaderText="InsertDate" IsBound="True" Key="Insert_Date">
                        <Header Caption="InsertDate">
                            <RowLayoutColumnInfo OriginX="13" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="13" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="SubTeam_Name" HeaderText="SubTeam" IsBound="True"
                        Key="SubTeam_Name">
                        <Header Caption="SubTeam">
                            <RowLayoutColumnInfo OriginX="14" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="14" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="RequestedBy" HeaderText="Requested By" IsBound="True"
                        Key="RequestedBy">
                        <Header Caption="Requested By">
                            <RowLayoutColumnInfo OriginX="15" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="15" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="ProcessedBy" HeaderText="ProcessedBy" IsBound="True"
                        Key="ProcessedBy">
                        <Header Caption="ProcessedBy">
                            <RowLayoutColumnInfo OriginX="16" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="16" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="Comments" HeaderText="Comments" IsBound="True"
                        Key="Comments">
                        <Header Caption="Comments">
                            <RowLayoutColumnInfo OriginX="17" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="17" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn CellButtonDisplay="Always" Key="DeleteItem" Type="HyperLink">
                        <Header>
                            <RowLayoutColumnInfo OriginX="18" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="18" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn CellButtonDisplay="Always" Key="RejectItem" Type="HyperLink">
                        <Header>
                            <RowLayoutColumnInfo OriginX="19" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="19" />
                        </Footer>
                    </igtbl:UltraGridColumn>
            </Columns>
                <AddNewRow View="NotSet" Visible="NotSet">
                </AddNewRow>
            </igtbl:UltraGridBand>
        </Bands>
       <DisplayLayout ViewType="OutlookGroupBy" Version="4.00" AllowSortingDefault="OnClient" StationaryMargins="Header" AllowColSizingDefault="Free" StationaryMarginsOutlookGroupBy="True" HeaderClickActionDefault="SortMulti" Name="UltraWebGrid1" BorderCollapseDefault="Separate" RowSelectorsDefault="No" TableLayout="Fixed" RowHeightDefault="20px" AllowColumnMovingDefault="OnServer" SelectTypeRowDefault="Extended" ColWidthDefault="">
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

<FilterOptionsDefault>
<FilterOperandDropDownStyle BorderWidth="1px" BorderColor="Silver" BorderStyle="Solid" Font-Size="11px" Font-Names="Verdana,Arial,Helvetica,sans-serif" BackColor="White" CustomRules="overflow:auto;">
<Padding Left="2px"></Padding>
</FilterOperandDropDownStyle>

<FilterHighlightRowStyle ForeColor="White" BackColor="#151C55"></FilterHighlightRowStyle>

<FilterDropDownStyle BorderWidth="1px" BorderColor="Silver" BorderStyle="Solid" Font-Size="11px" Font-Names="Verdana,Arial,Helvetica,sans-serif" BackColor="White" Width="200px" Height="300px" CustomRules="overflow:auto;">
<Padding Left="2px"></Padding>
</FilterDropDownStyle>
</FilterOptionsDefault>

<HeaderStyleDefault HorizontalAlign="Left" BorderStyle="Solid" BackColor="#004000" Font-Bold="True" Font-Names="Tahoma" ForeColor="White">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</HeaderStyleDefault>

<EditCellStyleDefault BorderWidth="0px" BorderStyle="None"></EditCellStyleDefault>

<FrameStyle BorderWidth="1px" BorderColor="InactiveCaption" BorderStyle="Solid" Font-Size="8.25pt" Font-Names="Microsoft Sans Serif" BackColor="Window"></FrameStyle>

<Pager AllowPaging="True" MinimumPagesForDisplay="2">
<Style BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</Style>
</Pager>

<AddNewBox Hidden="False">
<Style BorderWidth="1px" BorderColor="InactiveCaption" BorderStyle="Solid" BackColor="Window">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</Style>
</AddNewBox>
    <ClientSideEvents InitializeLayoutHandler="UltraWebGrid1_InitializeLayoutHandler" />
    <RowAlternateStyleDefault BackColor="#C0FFC0">
    </RowAlternateStyleDefault>
</DisplayLayout>
</igtbl:ultrawebgrid>
</asp:Content>

