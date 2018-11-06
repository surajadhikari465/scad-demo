<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_RefreshPOS_Refresh" title="Refresh" Codebehind="Refresh.aspx.vb" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    &nbsp;<asp:Label ID="Label4" runat="server" Style="position: static" Font-Size="Large" Font-Bold="True" Width="539px"></asp:Label>
    <asp:Panel ID="Panel1" runat="server" Style="position: static">
        <table style="position: static; width: 566px;">
            <tr>
                <td style="width: 44px; height: 14px;">
                    &nbsp;<br />
                        <br />
                                          <br />
                        </td>
                <td style="width: 185px; height: 14px;">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Medium" Font-Underline="False"
                        Style="position: static" Text="UPC:"></asp:Label>
                    <asp:Label ID="Label3" runat="server" Style="position: static" Font-Size="Medium" Font-Bold="False"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="Medium" Font-Underline="False"
                        Style="position: static" Text="Store:"></asp:Label><br />
                    <asp:DropDownList ID="Dropdown_Store_List" runat="server" DataSourceID="SqlDataSource2"
                        DataTextField="Store_Name" DataValueField="Store_No" AutoPostBack="True" Width="238px">
                    </asp:DropDownList><br />
                    <br />
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Medium" Font-Underline="False"
                        Style="position: static" Text="Reason:"></asp:Label><br />
                    <asp:DropDownList ID="ddlReason" runat="server" DataSourceID="SqlDataSource1"
                        DataTextField="Reason" DataValueField="Reason" Width="238px">
                    </asp:DropDownList>
                    <br />
                    <asp:Button ID="Button_Submit" runat="server" Text="Submit" /><br />
                    <br />
                    <span style="font-size: 9pt; font-family: Calibri">If the Refresh option is <em><strong>
                        unchecked</strong></em> and disabled, item is not authorized for that store.<br />
                    <br />
                    If the Refresh option is <em><strong>checked</strong></em> and disabled, there is a refresh request pending.<br />
                    </span>
                 <br />
                 <asp:Label ID="lblRefreshMessage" runat="server" Visible="False" Font-Bold="True" ForeColor="#009900" />   </td>
                <td colspan="1" rowspan="3">
                    <asp:Panel ID="pnlDesktop" runat="server">
                        <script type="text/javascript" src="../../javascript/AutosizeWebGridColumns.js"></script>
                        <br />
                        <igtbl:UltraWebGrid ID="UltraWebGrid2" runat="server"
                            Height="589px" Style="left: 2px; top: -29px" Width="267px" Browser="Xml">
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
                                            <Header>
                                                <RowLayoutColumnInfo OriginX="1" />
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="1" />
                                            </Footer>
                                        </igtbl:UltraGridColumn>
                                        <igtbl:UltraGridColumn AllowUpdate="Yes" BaseColumnName="Refresh" CellButtonDisplay="Always"
                                            DataType="System.Boolean" IsBound="True" Key="Refresh" Type="CheckBox">
                                            <Header>
                                                <RowLayoutColumnInfo OriginX="2" />
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="2" />
                                            </Footer>
                                        </igtbl:UltraGridColumn>
                                        <igtbl:UltraGridColumn DataType="System.Int32" Hidden="True" IsBound="True" Key="StoreItemVendorID">
                                            <Header>
                                                <RowLayoutColumnInfo OriginX="3" />
                                            </Header>
                                            <Footer>
                                                <RowLayoutColumnInfo OriginX="3" />
                                            </Footer>
                                        </igtbl:UltraGridColumn>
                                        <igtbl:UltraGridColumn BaseColumnName="StoreJurisdictionID" 
                                            DataType="System.Int32" Hidden="True" IsBound="True" Key="StoreJurisdictionID">
                                            <Header Caption="StoreJurisdictionID">
                                                <RowLayoutColumnInfo OriginX="4" />
                                            </Header>
                                            <Footer>
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
                                    <BoxStyle BackColor="ActiveBorder" BorderColor="Window">
                                    </BoxStyle>
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
                                    BorderWidth="1px" Font-Names="Microsoft Sans Serif" Font-Size="8.25pt" Height="589px"
                                    Width="267px">
                                </FrameStyle>
                                <Pager MinimumPagesForDisplay="2" >
                                    <PagerStyle BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px" />
                                </Pager>
                                <AddNewBox>
                                    <BoxStyle BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid" BorderWidth="1px">
                                    </BoxStyle>
                                </AddNewBox>
                                <ClientSideEvents ClickCellButtonHandler="UltraWebGrid2_ClickCellButtonHandler" InitializeLayoutHandler="UltraWebGrid2_InitializeLayoutHandler" />
                            </DisplayLayout>
                        </igtbl:UltraWebGrid>
                        <script type="text/javascript" language="javascript">

                        function UltraWebGrid2_InitializeLayoutHandler(gridName){
                            setColWidths(gridName);
                        }

                        </script>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td style="width: 44px; height: 248px;"></td>
                <td style="width: 185px; height: 248px;">
                 </td>
            </tr>
            <tr>
                <td colspan="2" rowspan="2">
                </td>
            </tr>
        </table>
        <asp:Label ID="Label_Error" runat="server" Visible="False" Width="511px"></asp:Label>
        
        <asp:Panel ID="pnlMobile" runat="server" Visible="false">
            <asp:GridView ID="gvRefresh" runat="server" AutoGenerateColumns="False" DataKeyNames="Store_No, StoreItemVendorID">
                <Columns>
                    <asp:BoundField DataField="Store_Name" HeaderText="Store_Name" SortExpression="Store_Name" />
                    <asp:TemplateField HeaderText="Refresh">
                        <ItemTemplate>
                            <asp:CheckBox ID="cbRefresh" runat="server" Enabled="true" Checked='<%# DataBinder.Eval(Container.DataItem, "Refresh").ToString() %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:Button ID="btnMobileSubmit" runat="server" Text="Submit" />
        </asp:Panel>
    </asp:Panel>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="SELECT [Reason] FROM [StoreItemRefreshReason]">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="SELECT [Store_No], [Store_Name] FROM [Store] WHERE Distribution_Center = 0 AND Regional = 0 AND (EXEWarehouse = 0 or EXEWarehouse IS NULL ) AND (WFM_Store = 1 OR Mega_Store = 1) ORDER BY [Store_Name]">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        UpdateCommand="UpdateStoreItem" UpdateCommandType="StoredProcedure">
        <UpdateParameters>
            <asp:Parameter Name="Item_Key" Type="Int32" />
            <asp:Parameter Name="Store_No" Type="Int32" />
            <asp:Parameter Name="Refresh" Type="Boolean" />
        </UpdateParameters>
    </asp:SqlDataSource>
    &nbsp;&nbsp;
        <asp:Label ID="Label9" runat="server" Font-Size="Medium" Style="position: static" Width="152px" Font-Bold="True" ForeColor="Navy"></asp:Label>
        <asp:Label ID="Label10" runat="server" Font-Size="Medium" Style="position: static"
            Width="168px" Font-Bold="True" ForeColor="Navy"></asp:Label>
</asp:Content>

