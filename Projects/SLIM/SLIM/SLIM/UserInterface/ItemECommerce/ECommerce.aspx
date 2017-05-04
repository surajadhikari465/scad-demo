<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_ItemECommerce_ECommerce" title="ECommerce" Codebehind="ECommerce.aspx.vb" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    &nbsp;<asp:Label ID="Label4" runat="server" Style="position: static" Font-Size="Large" Font-Bold="True" Width="539px"></asp:Label>
    <asp:Panel ID="Panel1" runat="server" Style="position: static">
        <table style="position: static; width: 566px;">
            <tr>
                <td style="width: 44px; height: 30px;">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Medium" Font-Underline="False"
                        Style="position: static" Text="UPC:"></asp:Label>
                        <br />
                        <br />
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="Medium" Font-Underline="False"
                        Style="position: static" Text="Store:"></asp:Label></td>
                <td style="width: 185px; height: 30px;">
                    <asp:Label ID="Label3" runat="server" Style="position: static" Font-Size="Medium" Font-Bold="True"></asp:Label>
                    <br />
                  <br />
                    <asp:DropDownList ID="Dropdown_Store_List" runat="server" DataSourceID="SqlDataSource2"
                        DataTextField="Store_Name" DataValueField="Store_No" AutoPostBack="True" Width="238px">
                    </asp:DropDownList></td>
                <td colspan="2" rowspan="2">
                    <asp:Panel ID="pnlDesktop" runat="server">
                        <script type="text/javascript" src="../../javascript/AutosizeWebGridColumns.js"></script>
                        <script type="text/javascript" language="javascript">

                        function UltraWebGrid2_InitializeLayoutHandler(gridName){
                            setColWidths(gridName);
                        }

                        </script>
                        <igtbl:UltraWebGrid ID="UltraWebGrid2" runat="server"
                            Height="176px" Style="left: 2px; top: -29px" Width="267px" Browser="Xml">
                            <Bands>
                                <igtbl:UltraGridBand>
                                    <AddNewRow View="NotSet" Visible="NotSet">
                                    </AddNewRow>
                                    <Columns>
                                        <igtbl:UltraGridColumn BaseColumnName="Store_No" DataType="System.Int32" Hidden="True"
                                            IsBound="True" Key="Store_No">
                                        </igtbl:UltraGridColumn>
                                        <igtbl:UltraGridColumn BaseColumnName="Store_Name" IsBound="True" Key="Store_Name"
                                            Width="175px">
                                            <Header Caption="Store">
                                                <RowLayoutColumnInfo OriginX="1" />
                                                <RowLayoutColumnInfo OriginX="1" />
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="1" />
                                                <RowLayoutColumnInfo OriginX="1" />
                                            </Footer>
                                        </igtbl:UltraGridColumn>
                                        <igtbl:UltraGridColumn AllowUpdate="Yes" BaseColumnName="ECommerce" CellButtonDisplay="Always"
                                            DataType="System.Boolean" IsBound="True" Key="ECommerce" Type="CheckBox">
                                            <Header>
                                                <RowLayoutColumnInfo OriginX="2" />
                                                <RowLayoutColumnInfo OriginX="2" />
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="2" />
                                                <RowLayoutColumnInfo OriginX="2" />
                                            </Footer>
                                        </igtbl:UltraGridColumn>
                                        <igtbl:UltraGridColumn DataType="System.Int32" Hidden="True" IsBound="True" Key="StoreItemVendorID">
                                            <Header>
                                                <RowLayoutColumnInfo OriginX="3" />
                                                <RowLayoutColumnInfo OriginX="3" />
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="3" />
                                                <RowLayoutColumnInfo OriginX="3" />
                                            </Footer>
                                        </igtbl:UltraGridColumn>
                                        <igtbl:UltraGridColumn BaseColumnName="StoreJurisdictionID" 
                                            DataType="System.Int32" Hidden="True" IsBound="True" Key="StoreJurisdictionID">
                                            <Header Caption="StoreJurisdictionID">
                                                <RowLayoutColumnInfo OriginX="4" />
                                                <RowLayoutColumnInfo OriginX="4" />
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="4" />
                                                <RowLayoutColumnInfo OriginX="4" />
                                            </Footer>
                                        </igtbl:UltraGridColumn>
                                    </Columns>
                                </igtbl:UltraGridBand>
                            </Bands>
                            <DisplayLayout AllowColSizingDefault="Free" AllowColumnMovingDefault="OnServer"
                                AllowSortingDefault="OnClient" AllowUpdateDefault="Yes" BorderCollapseDefault="Separate"
                                ColWidthDefault="" HeaderClickActionDefault="SortMulti" Name="UltraWebGrid2"
                                RowHeightDefault="20px" RowSelectorsDefault="No" ScrollBarView="Vertical" SelectTypeRowDefault="Extended"
                                StationaryMargins="Header" StationaryMarginsOutlookGroupBy="True" TableLayout="Fixed"
                                Version="4.00" ViewType="OutlookGroupBy" EnableInternalRowsManagement="True" LoadOnDemand="Xml">
                                <GroupByBox Hidden="True">
                                    <Style BackColor="ActiveBorder" BorderColor="Window"></Style>
                                    <BoxStyle BackColor="ActiveBorder" BorderColor="Window">
                                    </BoxStyle>
                                </GroupByBox>
                                <GroupByRowStyleDefault BackColor="Control" BorderColor="Window">
                                </GroupByRowStyleDefault>
                                <ActivationObject BorderColor="" BorderWidth="">
                                </ActivationObject>
                                <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
                                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                        WidthTop="1px" />
                                </FooterStyleDefault>
                                <RowStyleDefault BackColor="Window" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                                    Font-Names="Microsoft Sans Serif" Font-Size="8.25pt">
                                    <BorderDetails ColorLeft="Window" ColorTop="Window" />
                                    <Padding Left="3px" />
                                    <Padding Left="3px" />
                                    <BorderDetails ColorLeft="Window" ColorTop="Window" />
                                </RowStyleDefault>
                                <FilterOptionsDefault>
                                    <FilterOperandDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid"
                                        BorderWidth="1px" CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif"
                                        Font-Size="11px">
                                        <Padding Left="2px" />
                                        <Padding Left="2px" />
                                    </FilterOperandDropDownStyle>
                                    <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                                    </FilterHighlightRowStyle>
                                    <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                                        CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif"
                                        Font-Size="11px" Height="300px" Width="200px">
                                        <Padding Left="2px" />
                                        <Padding Left="2px" />
                                    </FilterDropDownStyle>
                                </FilterOptionsDefault>
                                <HeaderStyleDefault BackColor="LightGray" BorderStyle="Solid" HorizontalAlign="Left">
                                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
                                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                        WidthTop="1px" />
                                </HeaderStyleDefault>
                                <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
                                </EditCellStyleDefault>
                                <FrameStyle BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid"
                                    BorderWidth="1px" Font-Names="Microsoft Sans Serif" Font-Size="8.25pt" Height="176px"
                                    Width="267px">
                                </FrameStyle>
                                <ClientSideEvents ClickCellButtonHandler="UltraWebGrid2_ClickCellButtonHandler" InitializeLayoutHandler="UltraWebGrid2_InitializeLayoutHandler" />
                                <Pager MinimumPagesForDisplay="2">
                                    <style backcolor="LightGray" borderstyle="Solid" borderwidth="1px">

                                        <borderdetails colorleft="White" colortop="White" widthleft="1px" 
                                            widthtop="1px" />
                                    </style>
                                    <PagerStyle BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                        WidthTop="1px" />
                                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                        WidthTop="1px" />
                                    </PagerStyle>
                                </Pager>
                                <AddNewBox>
                                    <style backcolor="Window" bordercolor="InactiveCaption" borderstyle="Solid" 
                                        borderwidth="1px">

                                        <borderdetails colorleft="White" colortop="White" widthleft="1px" 
                                            widthtop="1px" />
                                    </style>
                                    <BoxStyle BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid" 
                                        BorderWidth="1px">
                                        <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                            WidthTop="1px" />
                                        <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                            WidthTop="1px" />
                                    </BoxStyle>
                                </AddNewBox>
                                <ClientSideEvents ClickCellButtonHandler="UltraWebGrid2_ClickCellButtonHandler" 
                                    InitializeLayoutHandler="UltraWebGrid2_InitializeLayoutHandler" />
                            </DisplayLayout>
                        </igtbl:UltraWebGrid>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="width: 44px; height: 30px;"></td>
                <td style="width: 185px">
                    Disabled stores have no vendor for this item. To authorize this item, contact regional
                    to add a vendor.</td>
            </tr>
            <tr>
                <td colspan="2" rowspan="2">
                </td>
            </tr>
            <tr>
                <td style="width: 146px; text-align: right">
                </td>
                <td style="width: 2px; text-align: right">
                    <asp:Button ID="Button_Submit" runat="server" Text="Submit" />
                </td>
            </tr>
        </table>
        <asp:Label ID="Label_Error" runat="server" Visible="False" Width="511px"></asp:Label>
        <br />
        
        <asp:Panel ID="pnlMobile" runat="server" Visible="false">
            <asp:GridView ID="gvAuthorizations" runat="server" AutoGenerateColumns="False" DataKeyNames="Store_No, StoreItemVendorID">
                <Columns>
                    <asp:BoundField DataField="Store_Name" HeaderText="Store_Name" SortExpression="Store_Name" />
                    <asp:TemplateField HeaderText="Authorized">
                        <ItemTemplate>
                            <asp:CheckBox ID="cbAuthorized" runat="server" Enabled="true" Checked='<%# DataBinder.Eval(Container.DataItem, "Authorized").ToString() %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:Button ID="btnMobileSubmit" runat="server" Text="Submit" />
        </asp:Panel>
    </asp:Panel>
    <br />
    <igtbl:UltraWebGrid ID="UltraWebGrid1" runat="server" DataSourceID="SqlDataSource3"
        Height="200px" Width="561px">
        <Bands>
            <igtbl:UltraGridBand>
                <AddNewRow View="NotSet" Visible="NotSet">
                </AddNewRow>
                <Columns>
                    <igtbl:UltraGridColumn BaseColumnName="Item_Key" DataType="System.Int32" HeaderText="Item_Key"
                        Hidden="True" IsBound="True" Key="Item_Key">
                        <Header Caption="Item_Key">
                        </Header>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="Store_No" DataType="System.Int32" HeaderText="Store_No"
                        Hidden="True" IsBound="True" Key="Store_No">
                        <Header Caption="Store_No">
                            <RowLayoutColumnInfo OriginX="1" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="1" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="Vendor_ID" DataType="System.Int32" HeaderText="Vendor_ID"
                        Hidden="True" IsBound="True" Key="Vendor_ID">
                        <Header Caption="Vendor_ID">
                            <RowLayoutColumnInfo OriginX="2" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="2" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="CompanyName" HeaderText="Vendor" IsBound="True"
                        Key="CompanyName">
                        <Header Caption="Vendor">
                            <RowLayoutColumnInfo OriginX="3" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="3" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="Warehouse" HeaderText="Warehouse" IsBound="True"
                        Key="Warehouse">
                        <Header Caption="Warehouse">
                            <RowLayoutColumnInfo OriginX="4" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="4" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="Store_Name" HeaderText="Store" IsBound="True"
                        Key="Store_Name">
                        <Header Caption="Store">
                            <RowLayoutColumnInfo OriginX="5" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="5" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="StoreItemVendorID" DataType="System.Int32"
                        HeaderText="StoreItemVendorID" Hidden="True" IsBound="True" Key="StoreItemVendorID">
                        <Header Caption="StoreItemVendorID">
                            <RowLayoutColumnInfo OriginX="6" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="6" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                    <igtbl:UltraGridColumn BaseColumnName="PrimaryVendor" DataType="System.Boolean" HeaderText="Primary Vendor"
                        IsBound="True" Key="PrimaryVendor" Type="CheckBox">
                        <Header Caption="Primary Vendor">
                            <RowLayoutColumnInfo OriginX="7" />
                        </Header>
                        <Footer>
                            <RowLayoutColumnInfo OriginX="7" />
                        </Footer>
                    </igtbl:UltraGridColumn>
                </Columns>
            </igtbl:UltraGridBand>
        </Bands>
        <DisplayLayout AllowColSizingDefault="Free" AllowColumnMovingDefault="OnServer" AllowDeleteDefault="Yes"
            AllowSortingDefault="OnClient" AllowUpdateDefault="Yes" BorderCollapseDefault="Separate"
            HeaderClickActionDefault="SortMulti" Name="UltraWebGrid1" RowHeightDefault="20px"
            RowSelectorsDefault="No" SelectTypeRowDefault="Extended" StationaryMargins="Header"
            StationaryMarginsOutlookGroupBy="True" TableLayout="Fixed" Version="4.00" ViewType="OutlookGroupBy">
            <ClientSideEvents InitializeLayoutHandler="UltraWebGrid2_InitializeLayoutHandler" />
            <GroupByBox Hidden="True">
                <Style BackColor="ActiveBorder" BorderColor="Window"></Style>
            </GroupByBox>
            <GroupByRowStyleDefault BackColor="Control" BorderColor="Window">
            </GroupByRowStyleDefault>
            <ActivationObject BorderColor="" BorderWidth="">
            </ActivationObject>
            <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
            </FooterStyleDefault>
            <RowStyleDefault BackColor="Window" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                Font-Names="Microsoft Sans Serif" Font-Size="8.25pt">
                <BorderDetails ColorLeft="Window" ColorTop="Window" />
                <Padding Left="3px" />
            </RowStyleDefault>
            <FilterOptionsDefault>
                <FilterOperandDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid"
                    BorderWidth="1px" CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif"
                    Font-Size="11px">
                    <Padding Left="2px" />
                </FilterOperandDropDownStyle>
                <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                </FilterHighlightRowStyle>
                <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                    CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif"
                    Font-Size="11px" Height="300px" Width="200px">
                    <Padding Left="2px" />
                </FilterDropDownStyle>
            </FilterOptionsDefault>
            <HeaderStyleDefault BackColor="LightGray" BorderStyle="Solid" HorizontalAlign="Left">
                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
            </HeaderStyleDefault>
            <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
            </EditCellStyleDefault>
            <FrameStyle BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid"
                BorderWidth="1px" Font-Names="Microsoft Sans Serif" Font-Size="8.25pt" Height="200px"
                Width="561px">
            </FrameStyle>
            <Pager MinimumPagesForDisplay="2">
                <Style BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</Style>
            </Pager>
            <AddNewBox Hidden="False">
                <Style BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid" BorderWidth="1px">
<BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
</Style>
            </AddNewBox>
        </DisplayLayout>
    </igtbl:UltraWebGrid><br />
    &nbsp;&nbsp;<br />
    &nbsp;
    &nbsp;&nbsp;
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="SELECT [Store_No], [Store_Name]&#13;&#10; FROM [Store] &#13;&#10;WHERE Distribution_Center = 0&#13;&#10;&#9;&#9;AND Regional = 0&#13;&#10;&#9;&#9;AND (EXEWarehouse = 0 or EXEWarehouse IS NULL )&#13;&#10;&#9;&#9;AND (WFM_Store = 1 OR Mega_Store = 1)&#13;&#10;ORDER BY [Store_Name]">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="GetItemPrimVend" SelectCommandType="StoredProcedure" UpdateCommand="UpdateStoreItem" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="0" Name="Item_Key" QueryStringField="i" Type="Int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="Item_Key" Type="Int32" />
            <asp:Parameter Name="Store_No" Type="Int32" />
            <asp:Parameter Name="AuthorizedItem" Type="Boolean" />
        </UpdateParameters>
    </asp:SqlDataSource>
    &nbsp;&nbsp;
        <asp:Label ID="Label9" runat="server" Font-Size="Medium" Style="position: static" Width="152px" Font-Bold="True" ForeColor="Navy"></asp:Label>
        <asp:Label ID="Label10" runat="server" Font-Size="Medium" Style="position: static"
            Width="168px" Font-Bold="True" ForeColor="Navy"></asp:Label>
</asp:Content>

