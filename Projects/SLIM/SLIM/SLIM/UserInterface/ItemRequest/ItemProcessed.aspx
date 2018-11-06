<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_ItemRequest_ItemProcessed" title="Item Requests Processed" Codebehind="ItemProcessed.aspx.vb" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtblexp" Namespace="Infragistics.WebUI.UltraWebGrid.ExcelExport" Assembly="Infragistics2.WebUI.UltraWebGrid.ExcelExport.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ MasterType VirtualPath="~/UserInterface/MasterPage.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    &nbsp;&nbsp;&nbsp;
    <img src="../../images/page_excel.png"  border="0" /> <asp:LinkButton ID="LinkButton1" runat="server" Font-Names="tahoma" Font-Size="Small">Excel</asp:LinkButton>
    <igtblexp:UltraWebGridExcelExporter ID="UltraWebGridExcelExporter1" runat="server"
        DownloadName="test.xls">
    </igtblexp:UltraWebGridExcelExporter>
    <igtbl:UltraWebGrid ID="UltraWebGrid1" runat="server" DataSourceID="SqlDataSource1">
        <Bands>
            <igtbl:UltraGridBand GridLines="Both">
                <AddNewRow View="NotSet" Visible="NotSet">
                </AddNewRow>
                <Columns>
                    <igtbl:UltraGridColumn BaseColumnName="Identifier" HeaderText="UPC" IsBound="True"
                        Key="Identifier">
                        <Header Caption="UPC">
                        </Header>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="Item_Description" HeaderText="Description"
                        IsBound="True" Key="Item_Description">
                        <Header Caption="Description">
                            <RowLayoutColumnInfo OriginX="1" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="1" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="ItemSize" DataType="System.Int16" HeaderText="ItemSize"
                        IsBound="True" Key="ItemSize">
                        <Header Caption="ItemSize">
                            <RowLayoutColumnInfo OriginX="2" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="2" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="Price" DataType="System.Decimal" HeaderText="Price"
                        IsBound="True" Key="Price">
                        <Header Caption="Price">
                            <RowLayoutColumnInfo OriginX="3" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="3" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="PriceMultiple" DataType="System.Decimal" HeaderText="Multiple"
                        IsBound="True" Key="PriceMultiple">
                        <Header Caption="Multiple">
                            <RowLayoutColumnInfo OriginX="4" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="4" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="CaseCost" DataType="System.Decimal" HeaderText="Cost"
                        IsBound="True" Key="CaseCost">
                        <Header Caption="Cost">
                            <RowLayoutColumnInfo OriginX="5" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="5" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="CaseSize" DataType="System.Int16" HeaderText="CaseSize"
                        IsBound="True" Key="CaseSize">
                        <Header Caption="CaseSize">
                            <RowLayoutColumnInfo OriginX="6" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="6" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                                        <igtbl:UltraGridColumn BaseColumnName="Store_Name" HeaderText="Store" IsBound="True"
                        Key="Store_Name">
                        <Header Caption="Store">
                            <RowLayoutColumnInfo OriginX="10" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="10" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="UserName" HeaderText="UserName" IsBound="True"
                        Key="UserName">
                        <Header Caption="UserName">
                            <RowLayoutColumnInfo OriginX="11" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="11" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="SubTeam_Name" HeaderText="SubTeam" IsBound="True"
                        Key="SubTeam_Name">
                        <Header Caption="SubTeam">
                            <RowLayoutColumnInfo OriginX="12" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="12" />
                        </Footer>
                    </igtbl:UltraGridColumn>

                    <igtbl:UltraGridColumn BaseColumnName="Warehouse" HeaderText="Warehouse" IsBound="True"
                        Key="Warehouse" Hidden="True">
                        <Header Caption="Warehouse">
                            <RowLayoutColumnInfo OriginX="7" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="7" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="Insert_Date" DataType="System.DateTime" HeaderText="InsertDate"
                        IsBound="True" Key="Insert_Date">
                        <Header Caption="InsertDate">
                            <RowLayoutColumnInfo OriginX="8" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="8" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="IRMA_Add_Date" DataType="System.DateTime"
                        HeaderText="IRMAAdd" IsBound="True" Key="IRMA_Add_Date">
                        <Header Caption="IRMAAdd">
                            <RowLayoutColumnInfo OriginX="9" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="9" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                </Columns>
            </igtbl:UltraGridBand>
        </Bands>
        <DisplayLayout Name="UltraWebGrid1" RowHeightDefault="20px"
            StationaryMarginsOutlookGroupBy="True" TableLayout="Fixed" Version="4.00" AllowColSizingDefault="Free" AllowSortingDefault="OnClient" BorderCollapseDefault="Separate" HeaderClickActionDefault="SortSingle">
            <ActivationObject BorderColor="Black" BorderWidth="">
            </ActivationObject>
            <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
            </FooterStyleDefault>
            <RowStyleDefault BackColor="White" BorderColor="Gray" BorderStyle="None" BorderWidth="1px"
                Font-Names="Verdana" Font-Size="8pt">
                <BorderDetails ColorLeft="Gray" ColorTop="Gray" />
                <Padding Left="3px" />
            </RowStyleDefault>
            <FilterOptionsDefault AllowRowFiltering="OnClient" FilterUIType="HeaderIcons">
                <FilterOperandDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid"
                    BorderWidth="1px" CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif"
                    Font-Size="11px">
                    <Padding Left="2px" />
                </FilterOperandDropDownStyle>
                <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                </FilterHighlightRowStyle>
                <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                    CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif"
                    Font-Size="11px" Width="200px">
                    <Padding Left="2px" />
                </FilterDropDownStyle>
            </FilterOptionsDefault>
            <SelectedRowStyleDefault BackColor="#3A7B9C" ForeColor="White">
            </SelectedRowStyleDefault>
            <HeaderStyleDefault BackColor="Green" BorderColor="Black" BorderStyle="Solid" ForeColor="White">
                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
            </HeaderStyleDefault>
            <FrameStyle BorderStyle="Double" BorderWidth="3px" Cursor="Default" Font-Names="Verdana"
                Font-Size="8pt" ForeColor="#A37171">
            </FrameStyle>
            <AddNewBox>
                <Style BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</Style>
            </AddNewBox>
            <RowSelectorStyleDefault BackColor="Green">
            </RowSelectorStyleDefault>
            <RowAlternateStyleDefault BackColor="#C0FFC0">
            </RowAlternateStyleDefault>
            <Pager AllowPaging="true" StyleMode="customlabels" Pattern="[currentpageindex] of [pagecount] : [page:first:First] [prev] [next] [page:last:Last]" PageSize="50" />
        </DisplayLayout>
    </igtbl:UltraWebGrid>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DeleteMethod="Delete"
        InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" SelectMethod="GetProcessedItemRequests"
        TypeName="NewItemRequestTableAdapters.ItemRequestTableAdapter" UpdateMethod="Update">
        <DeleteParameters>
            <asp:Parameter Name="Original_ItemRequest_ID" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Identifier" Type="String" />
            <asp:Parameter Name="ItemStatus_ID" Type="Int16" />
            <asp:Parameter Name="ItemType_ID" Type="Int16" />
            <asp:Parameter Name="ItemTemplate" Type="Boolean" />
            <asp:Parameter Name="User_ID" Type="Int32" />
            <asp:Parameter Name="User_Store" Type="Int32" />
            <asp:Parameter Name="UserAccessLevel_ID" Type="Int16" />
            <asp:Parameter Name="VendorRequest_ID" Type="Int32" />
            <asp:Parameter Name="Item_Description" Type="String" />
            <asp:Parameter Name="POS_Description" Type="String" />
            <asp:Parameter Name="ItemUnit" Type="Int16" />
            <asp:Parameter Name="ItemSize" Type="Int16" />
            <asp:Parameter Name="PackSize" Type="Int16" />
            <asp:Parameter Name="VendorNumber" Type="String" />
            <asp:Parameter Name="SubTeam_No" Type="Int32" />
            <asp:Parameter Name="Price" Type="Decimal" />
            <asp:Parameter Name="PriceMultiple" Type="Decimal" />
            <asp:Parameter Name="CaseCost" Type="Decimal" />
            <asp:Parameter Name="CaseSize" Type="Int16" />
            <asp:Parameter Name="Warehouse" Type="String" />
            <asp:Parameter Name="Brand_ID" Type="Int32" />
            <asp:Parameter Name="BrandName" Type="String" />
            <asp:Parameter Name="Category_ID" Type="Int32" />
            <asp:Parameter Name="Insert_Date" Type="DateTime" />
            <asp:Parameter Name="ClassID" Type="Int32" />
            <asp:Parameter Name="IRMA_Add_Date" Type="DateTime" />
            <asp:Parameter Name="Ready_To_Apply" Type="Boolean" />
            <asp:Parameter Name="Original_ItemRequest_ID" Type="Int32" />
            <asp:Parameter Name="ItemRequest_ID" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="Identifier" Type="String" />
            <asp:Parameter Name="ItemStatus_ID" Type="Int16" />
            <asp:Parameter Name="ItemType_ID" Type="Int16" />
            <asp:Parameter Name="ItemTemplate" Type="Boolean" />
            <asp:Parameter Name="User_ID" Type="Int32" />
            <asp:Parameter Name="User_Store" Type="Int32" />
            <asp:Parameter Name="UserAccessLevel_ID" Type="Int16" />
            <asp:Parameter Name="VendorRequest_ID" Type="Int32" />
            <asp:Parameter Name="Item_Description" Type="String" />
            <asp:Parameter Name="POS_Description" Type="String" />
            <asp:Parameter Name="ItemUnit" Type="Int16" />
            <asp:Parameter Name="ItemSize" Type="Int16" />
            <asp:Parameter Name="PackSize" Type="Int16" />
            <asp:Parameter Name="VendorNumber" Type="String" />
            <asp:Parameter Name="SubTeam_No" Type="Int32" />
            <asp:Parameter Name="Price" Type="Decimal" />
            <asp:Parameter Name="PriceMultiple" Type="Decimal" />
            <asp:Parameter Name="CaseCost" Type="Decimal" />
            <asp:Parameter Name="CaseSize" Type="Int16" />
            <asp:Parameter Name="Warehouse" Type="String" />
            <asp:Parameter Name="Brand_ID" Type="Int32" />
            <asp:Parameter Name="BrandName" Type="String" />
            <asp:Parameter Name="Category_ID" Type="Int32" />
            <asp:Parameter Name="Insert_Date" Type="DateTime" />
            <asp:Parameter Name="ClassID" Type="Int32" />
            <asp:Parameter Name="IRMA_Add_Date" Type="DateTime" />
            <asp:Parameter Name="Ready_To_Apply" Type="Boolean" />
        </InsertParameters>
    </asp:ObjectDataSource>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="SELECT ItemRequest.Identifier, ItemRequest.Item_Description, ItemRequest.ItemSize, ItemRequest.Price, ItemRequest.PriceMultiple, ItemRequest.CaseCost, ItemRequest.CaseSize, ItemRequest.Warehouse, ItemRequest.Insert_Date, ItemRequest.IRMA_Add_Date, Store.Store_Name, Users.UserName, SubTeam.SubTeam_Name FROM Store INNER JOIN Users INNER JOIN SubTeam INNER JOIN ItemRequest ON SubTeam.SubTeam_No = ItemRequest.SubTeam_No ON Users.User_ID = ItemRequest.User_ID ON Store.Store_No = ItemRequest.User_Store WHERE (ItemRequest.ItemStatus_ID = 2) ORDER BY ItemRequest.IRMA_Add_Date DESC">
    </asp:SqlDataSource>
</asp:Content>

