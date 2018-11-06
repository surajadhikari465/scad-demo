<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_WebQuery_SearchResults" title="Web Query Search Results" Codebehind="SearchResults.aspx.vb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
	<%@ Register TagPrefix="igtblexp" Namespace="Infragistics.WebUI.UltraWebGrid.ExcelExport" Assembly="Infragistics2.WebUI.UltraWebGrid.ExcelExport.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
	
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:MultiView ID="mvGrids" runat="server" ActiveViewIndex="0">
	<asp:View ID="vwDesktop" runat="server">
		<script type="text/javascript" src="../../javascript/AutosizeWebGridColumns.js"></script>
		<script type="text/javascript" language="javascript">

		function UltraWebGrid1_InitializeLayoutHandler(gridName){
			setColWidths(gridName);
		}
		</script>
		<img alt="" style="border: 0;" src="../../images/page_excel.png" /><asp:LinkButton ID="LinkButton1"
			runat="server" Font-Names="tahoma" Font-Size="Small">Excel</asp:LinkButton>
		<br />
		<igtblexp:ultrawebgridexcelexporter id="UltraWebGridExcelExporter1" runat="server"
			downloadname="WebQuery.XLS" worksheetname="WebQuery"></igtblexp:ultrawebgridexcelexporter>
		<igtbl:ultrawebgrid id="UltraWebGrid1" runat="server">
			<Bands>
				<igtbl:UltraGridBand>
					<AddNewRow View="NotSet" Visible="NotSet">
					</AddNewRow>
					<Columns>
						<igtbl:UltraGridColumn HeaderText="" Width="33%">
							<Header Caption="">
							</Header>
						</igtbl:UltraGridColumn>

					</Columns>
				</igtbl:UltraGridBand>
				<igtbl:UltraGridBand>
					<AddNewRow View="NotSet" Visible="NotSet">
					</AddNewRow>
					<Columns>
						<igtbl:UltraGridColumn Width="33%">
						</igtbl:UltraGridColumn>
					</Columns>
				</igtbl:UltraGridBand>
			</Bands>

			<DisplayLayout ViewType="Hierarchical" Version="4.00" AllowColSizingDefault="Free" Name="UltraWebGrid1" BorderCollapseDefault="Separate" ColWidthDefault="" TableLayout="Fixed" RowHeightDefault="20px" AllowColumnMovingDefault="NotSet" SelectTypeRowDefault="Extended" AllowSortingDefault="Yes" HeaderClickActionDefault="SortMulti">
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

			<Pager MinimumPagesForDisplay="2" AllowPaging="True" PageSize="60" StyleMode="PrevNext">
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
		</igtbl:ultrawebgrid><asp:ObjectDataSource ID="ObjectDataSource1" runat="server"></asp:ObjectDataSource>
	</asp:View>
	<asp:View ID="vwMobile" runat="server">
		<asp:GridView ID="gvResults" runat="server" DataSourceID="SqlDataSource1" AutoGenerateColumns="False" DataKeyNames="Item_Key,brand_id">
			<Columns>
				<asp:HyperLinkField Text="Select" DataNavigateUrlFields="Item_Key, Identifier, item_description, package_desc2, unit_name" DataNavigateUrlFormatString="SearchItemDetail.aspx?i={0}&u={1}&d={2}&s={3}&m={4}" />
				<asp:BoundField DataField="Item_Key" HeaderText="Item_Key" InsertVisible="False"
					ReadOnly="True" SortExpression="Item_Key" Visible="False" />
				<asp:BoundField DataField="Identifier" HeaderText="Identifier" SortExpression="Identifier" />
				<asp:BoundField DataField="Identifier1" HeaderText="Identifier1" ReadOnly="True"
					SortExpression="Identifier1" Visible="False" />
				<asp:BoundField DataField="brand_id" HeaderText="brand_id" InsertVisible="False"
					ReadOnly="True" SortExpression="brand_id" Visible="False" />
				<asp:HyperLinkField HeaderText="brand_name" HeaderStyle-Width="50" DataNavigateUrlFields="brand_id" DataNavigateUrlFormatString="SearchResults.aspx?u=*&d=*&t=0&s=0&c=0&3=0&4=0&b={0}&v=0&x=*" DataTextField="brand_name" />
				<asp:BoundField DataField="item_description" HeaderText="item_description" SortExpression="item_description" />
				<asp:BoundField DataField="package_desc1" HeaderText="package_desc1" SortExpression="package_desc1" />
				<asp:BoundField DataField="package_desc2" HeaderText="package_desc2" SortExpression="package_desc2" />
				<asp:BoundField DataField="unit_name" HeaderText="unit_name" SortExpression="unit_name" />
				<asp:BoundField DataField="subteam_name" HeaderText="subteam_name" SortExpression="subteam_name" />
				<asp:BoundField DataField="category_name" HeaderText="category_name" SortExpression="category_name" />
			</Columns>
			
		</asp:GridView>
	</asp:View>
</asp:MultiView>


	<asp:Label ID="Label_RowCount" runat="server" Font-Bold="True" Font-Size="X-Small"
		Text="Label" Visible="False"></asp:Label>
	<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
		ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>" 
		SelectCommand="GetItemWebQuery" 
		SelectCommandType="StoredProcedure">
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
			<asp:QueryStringParameter Name="StoreJurisdictionID" QueryStringField="j" Type="String" ConvertEmptyStringToNull="false"/>
		</SelectParameters>
	</asp:SqlDataSource>
	<asp:Label ID="Label_Message" runat="server" Font-Names="Tahoma" Font-Size="12px"
		Text="No results can be found that match your search criteria." Width="574px"></asp:Label>
</asp:Content>

