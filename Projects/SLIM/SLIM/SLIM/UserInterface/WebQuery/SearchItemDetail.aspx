<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_WebQuery_SearchItemDetail" title="Search Item Detail" Codebehind="SearchItemDetail.aspx.vb" %>
<%@ Register TagPrefix="igtblexp" Namespace="Infragistics.WebUI.UltraWebGrid.ExcelExport" Assembly="Infragistics2.WebUI.UltraWebGrid.ExcelExport.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" Style="position: static">
        <table border="0" cellpadding="3" cellspacing="0" style="width: 618px">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Style="position: static" Text="UPC:" Font-Bold="True" Font-Size="12px" Font-Underline="False" Font-Names="Tahoma"></asp:Label></td>
                <td  colspan="2" style="width: 142px">
                    <asp:Label ID="Label_UPC" runat="server" Style="position: static" Font-Italic="False" Width="88px" Font-Size="12px" Font-Bold="True" Font-Names="Tahoma"></asp:Label></td>
                <td colspan="1" style="font-weight: bold; font-size: 12px; width: 132px; font-family: tahoma">
                    Size:</td>
                <td colspan="1" style="width: 65px">
                    <asp:Label ID="Label_Size" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Tahoma"
                        Font-Size="12px" Style="position: static" Width="88px"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server"  Text="Desc:" Font-Bold="True" Font-Size="12px" Font-Underline="False" Font-Names="Tahoma"></asp:Label></td>
                <td colspan="2" style="width: 142px" >
                    <asp:Label ID="Label_Desc" runat="server"  Font-Size="12px" Font-Bold="True" Font-Names="Tahoma"></asp:Label>
                </td>
                <td colspan="1" style="font-weight: bold; font-size: 12px; width: 132px; font-family: tahoma">
                    Unit Of Measure:</td>
                <td colspan="1" style="width: 65px">
                    <asp:Label ID="Label_UnitOfMeasure" runat="server" Font-Bold="True" Font-Italic="False" Font-Names="Tahoma"
                        Font-Size="12px" Style="position: static" Width="88px"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/UserInterface/WebQuery/ScaleItemDetail.aspx" Font-Names="Tahoma" Font-Size="12px">View Scale Info</asp:HyperLink>
                    </td>
                    <td colspan="1" style="width: 132px">
                    </td>
                    <td colspan="1" style="width: 65px">
                    </td>
                </tr>
                <tr>
                
                <td colspan="3">
                
                <asp:Panel ID="Panel_Movement" runat="server">
                <br />
                    <table border="0" cellspacing="0" cellpadding="2">
                    <tr><td colspan="5" style="font-family:Tahoma; font-size:12px; font-weight: bold;">Movement</td></tr>
                        <tr>
                        <td><asp:Label ID="Label11" runat="server" Style="position: static" Text="From:" Font-Size="12px" Font-Bold="True" Font-Names="Tahoma"></asp:Label></td>
                            <td>
                                <asp:Calendar ID="calStartDate" runat="server" Visible="false" SelectionMode="day" />
                                <igsch:WebDateChooser ID="StartDate" runat="server" NullDateLabel="" Style="position: static" Font-Names="Tahoma" Font-Size="12px">
                                    <CalendarLayout>
                                        <CalendarStyle Font-Size="X-Small"></CalendarStyle>
                                    </CalendarLayout>
                                    <DropDownStyle Font-Names="Tahoma" Font-Size="12px"></DropDownStyle>
                                </igsch:WebDateChooser>
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Style="position: static" Text="To:" Font-Size="12px" Font-Bold="True" Font-Names="Tahoma"></asp:Label>
                            </td>
                            <td>
                                <asp:Calendar ID="calEndDate" runat="server" Visible="false" SelectionMode="day" />
                                <igsch:WebDateChooser ID="EndDate" runat="server" Font-Size="12px" NullDateLabel="" Style="position: static" Font-Names="Tahoma">
                                    <CalendarLayout>
                                        <CalendarStyle Font-Size="X-Small"></CalendarStyle>
                                    </CalendarLayout>
                                    <DropDownStyle Font-Names="Tahoma" Font-Size="12px"></DropDownStyle>
                                </igsch:WebDateChooser>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="5">
                                <asp:Button ID="Button1" runat="server" Font-Size="12px" Style="position: static" Text="Submit" Font-Names="Tahoma" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                </td>
                <td colspan="1" style="width: 132px">
                </td>
                <td colspan="1" style="width: 65px">
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Style="position: static" EnableViewState="False">
        <asp:MultiView ID="mvDetail" runat="server" ActiveViewIndex="0">
        <asp:View ID="vwDesktop" runat="Server">
            <script type="text/javascript" src="../../javascript/AutosizeWebGridColumns.js"></script>
            <script language="javascript">
            function UltraGrid1_InitializeLayoutHeader(gridName){
	            setColWidths(gridName);
            }

            </script>
            &nbsp;
            <asp:Image ID="ExcelLogo" runat="server" ImageUrl="../../images/page_excel.png" BorderStyle="None" BorderWidth="0"/>
            <asp:LinkButton ID="LinkButton1" runat="server" Font-Names="tahoma" Font-Size="Small">Excel</asp:LinkButton>
            <igtblexp:ultrawebgridexcelexporter id="UltraWebGridExcelExporter1" runat="server"
                downloadname="WebQuery.XLS" worksheetname="WebQuery"></igtblexp:ultrawebgridexcelexporter>
            <igtbl:ultrawebgrid id="UltraWebGrid1" runat="server" datasourceid="SqlDataSource3"><Bands>
                <igtbl:UltraGridBand>
                <AddNewRow View="NotSet" Visible="NotSet"></AddNewRow>
                    <Columns>
                        <igtbl:UltraGridColumn BaseColumnName="Store_Name" HeaderText="Store_Name" IsBound="True"
                            Key="Store_Name">
                            <Header Caption="Store_Name">
                            </Header>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="PriceChgTypeDesc" HeaderText="PriceChgTypeDesc"
                            IsBound="True" Key="PriceChgTypeDesc">
                            <Header Caption="PriceChgTypeDesc">
                                <RowLayoutColumnInfo OriginX="1" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="1" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="RegPrice" HeaderText="RegPrice" IsBound="True"
                            Key="RegPrice">
                            <Header Caption="RegPrice">
                                <RowLayoutColumnInfo OriginX="2" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="2" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="SalePrice" HeaderText="SalePrice" IsBound="True"
                            Key="SalePrice">
                            <Header Caption="SalePrice">
                                <RowLayoutColumnInfo OriginX="3" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="3" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="SaleUnits" DataType="System.Int32" HeaderText="SaleUnits"
                            IsBound="True" Key="SaleUnits">
                            <Header Caption="SaleUnits">
                                <RowLayoutColumnInfo OriginX="4" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="4" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="SaleDollars" DataType="System.Int32" Format="$####0.00"
                            HeaderText="SaleDollars" IsBound="True" Key="SaleDollars">
                            <Header Caption="SaleDollars">
                                <RowLayoutColumnInfo OriginX="5" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="5" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="POS_Price" DataType="System.Decimal" HeaderText="POS_Price"
                            IsBound="True" Key="POS_Price">
                            <Header Caption="POS_Price">
                                <RowLayoutColumnInfo OriginX="6" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="6" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="POS_Price_Date" HeaderText="POS Price Date"
                            IsBound="True" Key="POS_Price_Date">
                            <Header Caption="POS Price Date">
                                <RowLayoutColumnInfo OriginX="7" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="7" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="Vendor" HeaderText="Vendor" IsBound="True"
                            Key="Vendor">
                            <Header Caption="Vendor">
                                <RowLayoutColumnInfo OriginX="8" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="8" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="VendorItemID" HeaderText="VendorItemID" IsBound="True"
                            Key="VendorItemID">
                            <Header Caption="VendorItemID">
                                <RowLayoutColumnInfo OriginX="9" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="9" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="PackSize" DataType="System.Decimal" HeaderText="PackSize"
                            IsBound="True" Key="PackSize">
                            <Header Caption="PackSize">
                                <RowLayoutColumnInfo OriginX="10" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="10" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="UnitCost" DataType="System.Decimal" Format="$###0.0000"
                            HeaderText="UnitCost" IsBound="True" Key="UnitCost">
                            <Header Caption="UnitCost">
                                <RowLayoutColumnInfo OriginX="11" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="11" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="NetCost" DataType="System.Decimal" Format="$###0.0000"
                            HeaderText="Net Unit Cost" IsBound="True" Key="NetCost">
                            <Header Caption="Net Unit Cost">
                                <RowLayoutColumnInfo OriginX="12" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="12" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="Margin" DataType="System.Decimal" Format="####.00%"
                            HeaderText="Margin" IsBound="True" Key="Margin">
                            <Header Caption="Margin">
                                <RowLayoutColumnInfo OriginX="13" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="13" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="NetMargin" DataType="System.Decimal" Format="####.00%"
                            HeaderText="Net Margin" IsBound="True" Key="NetMargin">
                            <Header Caption="Net Margin">
                                <RowLayoutColumnInfo OriginX="14" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="14" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="Auth" HeaderText="Auth" IsBound="True" Key="Auth">
                            <Header Caption="Auth">
                                <RowLayoutColumnInfo OriginX="15" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="15" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="Brand_Name" HeaderText="Brand Name" IsBound="True"
                            Key="Brand_Name">
                            <Header Caption="Brand Name">
                                <RowLayoutColumnInfo OriginX="16" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="16" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="TaxClassDesc" HeaderText="Tax Class" IsBound="True"
                            Key="TaxClassDesc">
                            <Header Caption="Tax Class">
                                <RowLayoutColumnInfo OriginX="17" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="17" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="Food_Stamps" HeaderText="FoodStamps" IsBound="True"
                            Key="Food_Stamps">
                            <Header Caption="FoodStamps">
                                <RowLayoutColumnInfo OriginX="18" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="18" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="subteam_name" HeaderText="SubTeam" IsBound="True"
                            Key="subteam_name">
                            <Header Caption="SubTeam">
                                <RowLayoutColumnInfo OriginX="19" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="19" />
                            </Footer>
                        </igtbl:UltraGridColumn>
                        <igtbl:UltraGridColumn BaseColumnName="category_name" HeaderText="Class" IsBound="True"
                            Key="category_name">
                            <Header Caption="Class">
                                <RowLayoutColumnInfo OriginX="20" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="20" />
                            </Footer>
                        </igtbl:UltraGridColumn>

                        <igtbl:UltraGridColumn BaseColumnName="DiscontinueItem" 
                            DataType="System.Boolean" IsBound="True" Key="DiscontinueItem">
                            <Header Caption="Discontinued">
                                <RowLayoutColumnInfo OriginX="21" />
                            </Header>
                            <Footer>
                                <RowLayoutColumnInfo OriginX="21" />
                            </Footer>
                        </igtbl:UltraGridColumn>

                    </Columns>
                    <RowEditTemplate>
                        <br>
                        <p align="center">
                            <input id='igtbl_reOkBtn' onclick='igtbl_gRowEditButtonClick(event);' style='width: 50px;'
                                type="button" value="OK">&nbsp;
                            <input id='igtbl_reCancelBtn' onclick='igtbl_gRowEditButtonClick(event);' style='width: 50px;'
                                type="button" value="Cancel"></p>
                    </RowEditTemplate>
                    <RowTemplateStyle BackColor="Window" BorderColor="Window" BorderStyle="Ridge">
                        <BorderDetails WidthBottom="3px" WidthLeft="3px" WidthRight="3px" WidthTop="3px" />
                    </RowTemplateStyle>
                </igtbl:UltraGridBand>
                </Bands>

                <DisplayLayout ViewType="OutlookGroupBy" Version="4.00" AllowSortingDefault="OnClient" StationaryMargins="Header" AllowColSizingDefault="Free" StationaryMarginsOutlookGroupBy="True" HeaderClickActionDefault="SortSingle" Name="UltraWebGrid1" BorderCollapseDefault="Separate" TableLayout="Fixed" RowHeightDefault="20px" SelectTypeRowDefault="Extended" ColWidthDefault="">
                <GroupByBox>
                <Style BorderColor="Window" BackColor="ActiveBorder"></Style>
                    <BoxStyle BackColor="ActiveBorder" BorderColor="Window">
                    </BoxStyle>
                </GroupByBox>

                <GroupByRowStyleDefault BorderColor="Window" BackColor="Control"></GroupByRowStyleDefault>

                <ActivationObject BorderWidth="" BorderColor=""></ActivationObject>

                <FooterStyleDefault BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
                <BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
                </FooterStyleDefault>

                <RowStyleDefault BorderWidth="1px" BorderColor="Silver" BorderStyle="Solid" Font-Size="12px" Font-Names="Tahoma" BackColor="WhiteSmoke">
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

                <RowSelectorStyleDefault BackColor="#004000"></RowSelectorStyleDefault>

                <HeaderStyleDefault ForeColor="White" HorizontalAlign="Left" BorderStyle="Solid" Font-Size="12px" Font-Names="tahoma" BackColor="#004000">
                <BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
                </HeaderStyleDefault>

                <RowAlternateStyleDefault Font-Size="12px" Font-Names="Tahoma" BackColor="#C0FFC0"></RowAlternateStyleDefault>

                <EditCellStyleDefault BorderWidth="0px" BorderStyle="None"></EditCellStyleDefault>

                <FrameStyle BorderWidth="1px" BorderColor="InactiveCaption" BorderStyle="Solid" Font-Size="8.25pt" Font-Names="Microsoft Sans Serif" BackColor="Window"></FrameStyle>

                <Pager MinimumPagesForDisplay="2">
                <Style BorderWidth="1px" BorderStyle="Solid" BackColor="LightGray">
                <BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
                </Style>
                    <PagerStyle BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                        WidthTop="1px" />
                    </PagerStyle>
                </Pager>

                <AddNewBox>
                <Style BorderWidth="1px" BorderColor="InactiveCaption" BorderStyle="Solid" BackColor="Window">
                <BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
                </Style>
                    <BoxStyle BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid" 
                        BorderWidth="1px">
                        <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                            WidthTop="1px" />
                    </BoxStyle>
                </AddNewBox>
                    <ClientSideEvents InitializeLayoutHandler="UltraGrid1_InitializeLayoutHeader" />
                </DisplayLayout>
            </igtbl:ultrawebgrid>
        </asp:View>
        <asp:View ID="vwMobile" runat="Server">
            <asp:GridView ID="gvDetails" runat="server" datasourceid="SqlDataSource3" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="Store_Name" HeaderText="Store_Name" SortExpression="Store_Name" />
                    <asp:BoundField DataField="RegPrice" HeaderText="RegPrice" 
                        SortExpression="RegPrice" ReadOnly="True" />
                    <asp:BoundField DataField="SalePrice" HeaderText="SalePrice" 
                        SortExpression="SalePrice" ReadOnly="True" />
                    <asp:BoundField DataField="POS_Price" HeaderText="POS_Price" 
                        SortExpression="POS_Price" />
                    <asp:BoundField DataField="POS_Price_Date" HeaderText="POS_Price_Date" 
                        SortExpression="POS_Price_Date" ReadOnly="True" />
                    <asp:BoundField DataField="SaleUnits" HeaderText="SaleUnits" 
                        SortExpression="SaleUnits" ReadOnly="True" />
                    <asp:BoundField DataField="SaleDollars" HeaderText="SaleDollars" 
                        SortExpression="SaleDollars" ReadOnly="True" />
                    <asp:BoundField DataField="Vendor" HeaderText="Vendor" 
                        SortExpression="Vendor" />
                    <asp:BoundField DataField="VendorItemID" HeaderText="VendorItemID" 
                        SortExpression="VendorItemID" />
                    <asp:BoundField DataField="PackSize" HeaderText="PackSize" 
                        SortExpression="PackSize" ReadOnly="True" />
                    <asp:BoundField DataField="UnitCost" HeaderText="UnitCost" 
                        SortExpression="UnitCost" ReadOnly="True" />
                    <asp:BoundField DataField="NetCost" HeaderText="NetCost" 
                        SortExpression="NetCost" ReadOnly="True" />
                    <asp:BoundField DataField="Margin" HeaderText="Margin" SortExpression="Margin" 
                        ReadOnly="True" />
                    <asp:BoundField DataField="NetMargin" HeaderText="NetMargin" ReadOnly="True" 
                        SortExpression="NetMargin" />
                    <asp:BoundField DataField="Auth" HeaderText="Auth" ReadOnly="True" 
                        SortExpression="Auth" />
                    <asp:BoundField DataField="PriceChgTypeDesc" HeaderText="PriceChgTypeDesc" 
                        SortExpression="PriceChgTypeDesc" />
                    <asp:BoundField DataField="Brand_Name" HeaderText="Brand_Name" 
                        SortExpression="Brand_Name" />
                    <asp:BoundField DataField="subteam_name" HeaderText="subteam_name" 
                        SortExpression="subteam_name" />
                    <asp:BoundField DataField="category_name" HeaderText="category_name" 
                        SortExpression="category_name" />
                    <asp:BoundField DataField="TaxClassDesc" HeaderText="TaxClassDesc" 
                        SortExpression="TaxClassDesc" />
                    <asp:BoundField DataField="Food_Stamps" HeaderText="Food_Stamps" 
                        ReadOnly="True" SortExpression="Food_Stamps" />
                    <asp:CheckBoxField DataField="DiscontinueItem" HeaderText="DiscontinueItem" 
                        SortExpression="DiscontinueItem" />
                </Columns>
                
            </asp:GridView>
        </asp:View>
    </asp:MultiView>
    
        
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
            SelectCommand="ItemWebQueryDetailMovement" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter DefaultValue="0" Name="item_key" SessionField="ItemKey" Type="Int32" />
                <asp:SessionParameter Name="StartDate" SessionField="StartDate" Type="DateTime" DefaultValue="0" />
                <asp:SessionParameter Name="EndDate" SessionField="EndDate" Type="DateTime" DefaultValue="0" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
            SelectCommand="ItemWebQueryDetail" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter DefaultValue="0" Name="Item_Key" SessionField="ItemKey" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="12px" ForeColor="Navy"
            Style="position: static" EnableTheming="True" Font-Names="Tahoma"></asp:Label>
    </asp:Panel>
</asp:Content>

