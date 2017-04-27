<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_ItemAuthorizations_SearchResults" title="Authorizations Search Results" Codebehind="SearchResults.aspx.vb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:MultiView ID="mvResults" runat="Server" ActiveViewIndex="0">
        <asp:View ID="vwDesktop" runat="server">
            <script src="../../javascript/AutosizeWebGridColumns.js" type="text/javascript"></script>
            <script language="javascript">
                function UltraWebGrid1_InitializeLayoutHandler(gridName){
	                setColWidths(gridName);
                }
            </script>

            &nbsp; &nbsp;&nbsp;&nbsp;
            <igtbl:UltraWebGrid ID="UltraWebGrid1" runat="server">
                <Bands>
                    <igtbl:UltraGridBand>
                        <RowEditTemplate>
                            <br />
                            <p align="center">
                                <input id="igtbl_reOkBtn" onclick="igtbl_gRowEditButtonClick(event);" style="width: 50px"
                                    type="button" value="OK" />&nbsp;
                                <input id="igtbl_reCancelBtn" onclick="igtbl_gRowEditButtonClick(event);" style="width: 50px"
                                    type="button" value="Cancel" /></p>
                        </RowEditTemplate>
                        <AddNewRow View="NotSet" Visible="NotSet">
                        </AddNewRow>
                        <Columns>
                            <igtbl:UltraGridColumn CellButtonDisplay="Always" Key="Select">
                            </igtbl:UltraGridColumn>
                        </Columns>
                        <RowTemplateStyle BackColor="Window" BorderColor="Window" BorderStyle="Ridge">
                            <BorderDetails WidthBottom="3px" WidthLeft="3px" WidthRight="3px" WidthTop="3px" />
                        </RowTemplateStyle>
                    </igtbl:UltraGridBand>
                    <igtbl:UltraGridBand>
                        <AddNewRow View="NotSet" Visible="NotSet">
                        </AddNewRow>
                        <Columns>
                            <igtbl:UltraGridColumn CellButtonDisplay="Always" Key="Select">
                            </igtbl:UltraGridColumn>
                        </Columns>
                    </igtbl:UltraGridBand>
                </Bands>
                <DisplayLayout AllowColSizingDefault="Free" AllowColumnMovingDefault="NotSet" BorderCollapseDefault="Separate"
                    ColWidthDefault="" AllowSortingDefault="Yes" HeaderClickActionDefault="SortMulti" Name="UltraWebGrid1" RowHeightDefault="20px"
                    SelectTypeRowDefault="Extended" TableLayout="Fixed" Version="4.00" ViewType="Hierarchical">
                    <GroupByBox>
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
                    <FilterOptionsDefault AllowRowFiltering="OnClient" FilterUIType="FilterRow">
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
                    <HeaderStyleDefault BackColor="#004000" BorderStyle="Solid" ForeColor="White" HorizontalAlign="Left">
                        <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
                    </HeaderStyleDefault>
                    <RowAlternateStyleDefault BackColor="#C0FFC0">
                    </RowAlternateStyleDefault>
                    <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
                    </EditCellStyleDefault>
                    <FrameStyle BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid"
                        BorderWidth="1px" Font-Names="tahoma" Font-Size="12px">
                    </FrameStyle>
                    <Pager AllowPaging="True" MinimumPagesForDisplay="2">
                        <Style BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
        <BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
        </Style>
                    </Pager>
                    <AddNewBox>
                        <Style BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid" BorderWidth="1px">
                            <BorderDetails ColorTop="White" WidthLeft="1px" WidthTop="1px" ColorLeft="White"></BorderDetails>
                        </Style>
                    </AddNewBox>
                    <ClientSideEvents InitializeLayoutHandler="UltraWebGrid1_InitializeLayoutHandler" />
                    <SelectedRowStyleDefault BackColor="#FFFFC0">
                    </SelectedRowStyleDefault>
                    <RowSelectorStyleDefault BackColor="#004000">
                    </RowSelectorStyleDefault>
                </DisplayLayout>
            </igtbl:UltraWebGrid>
        </asp:View>
        <asp:View ID="vwMobile" runat="server">
            <asp:GridView ID="gvResults" runat="server" DataSourceID="SqlDataSource1" AutoGenerateColumns="False" DataKeyNames="Item_Key,brand_id">
                <Columns>
                    <asp:HyperLinkField Text="Select" DataNavigateUrlFields="Item_Key, Identifier, item_description" DataNavigateUrlFormatString="Authorizations.aspx?i={0}&u={1}&d={2}" />
                    <asp:BoundField DataField="Item_Key" HeaderText="Item_Key" InsertVisible="False"
                        ReadOnly="True" SortExpression="Item_Key" />
                    <asp:BoundField DataField="Identifier" HeaderText="Identifier" SortExpression="Identifier" />
                    <asp:BoundField DataField="Identifier1" HeaderText="Identifier1" ReadOnly="True"
                        SortExpression="Identifier1" Visible="False" />
                    <asp:BoundField DataField="brand_id" HeaderText="brand_id" InsertVisible="False"
                        ReadOnly="True" SortExpression="brand_id" Visible="False" />
                    <asp:BoundField DataField="brand_name" HeaderText="brand_name" SortExpression="brand_name" />
                    <asp:BoundField DataField="item_description" HeaderText="item_description" SortExpression="item_description" />
                    <asp:BoundField DataField="package_desc1" HeaderText="package_desc1" SortExpression="package_desc1"
                        Visible="False" />
                    <asp:BoundField DataField="package_desc2" HeaderText="package_desc2" SortExpression="package_desc2" />
                    <asp:BoundField DataField="unit_name" HeaderText="unit_name" SortExpression="unit_name" />
                    <asp:BoundField DataField="subteam_name" HeaderText="subteam_name" SortExpression="subteam_name" />
                    <asp:BoundField DataField="category_name" HeaderText="category_name" SortExpression="category_name"
                        Visible="False" />
                </Columns>
            </asp:GridView>
        </asp:View>
    </asp:MultiView>
    
    <asp:Label ID="Label_RowCount" runat="server" Font-Bold="True" Font-Size="X-Small"
        Text="Label" Visible="False"></asp:Label><br />
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
            <asp:QueryStringParameter Name="Team_Name" QueryStringField="tn" Type="String" ConvertEmptyStringToNull="false" />
            <asp:QueryStringParameter Name="SubTeam_Name" QueryStringField="stn" Type="string" ConvertEmptyStringToNull="false" />
            <asp:QueryStringParameter Name="Category_Name" QueryStringField="cn" Type="string" ConvertEmptyStringToNull="false" />
            <asp:QueryStringParameter Name="Level3_Name" QueryStringField="l3n" Type="string" ConvertEmptyStringToNull="false" />
            <asp:QueryStringParameter Name="Level4_Name" QueryStringField="l4n" Type="string" ConvertEmptyStringToNull="false" />
            <asp:QueryStringParameter Name="Brand_Name" QueryStringField="bn" Type="string" ConvertEmptyStringToNull="false" />
            <asp:QueryStringParameter Name="Vendor_Name" QueryStringField="vn" Type="string" ConvertEmptyStringToNull="false" />
            <asp:QueryStringParameter Name="StoreJurisdictionID" QueryStringField="j" Type="Int32" ConvertEmptyStringToNull="false" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:Label ID="Label_Message" runat="server" Font-Bold="False" Font-Names="Tahoma"
        Font-Size="12px" Font-Underline="True" Style="position: static" Width="433px">No results can be found that match your search criteria.</asp:Label>
</asp:Content>