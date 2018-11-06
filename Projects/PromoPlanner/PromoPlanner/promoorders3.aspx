<%@ Page Language="vb" MasterPageFile="~/MasterPage.master"   AutoEventWireup="false" Inherits="PromoPlanner.pp_irma.promoorders3" Codebehind="promoorders3.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
			<table class="regtable">
				<tr>
					<td>The following order has been placed for
						<%=session("Store_No")%>
						-
						<%=session("deptno")%>
						-
						<%=session("startdate")%>
						to
						<%=session("enddate")%>
					</td>
				</tr>
				<tr>
					<td>If you need to make changes to this order click Return to Main to go back to 
						the main order page.</td>
				</tr>
			</table>
			<table class="regtable">
				<asp:Repeater Runat="server" ID="promo" Visible="True">
					<HeaderTemplate>
						<tr>
							<td align="left">| Vendor
							</td>
							<td align="left">| Brand
							</td>
							<td align="left">| UPC
							</td>
							<td align="left">| Description
							</td>
							<td align="left">| Size
							</td>
							<td align="left">| UOM
							</td>
							<td align="left">| Sale Price
							</td>
							<td align="left">| Sale Cost
							</td>
							<td align="left">| Margin
							</td>
							<td align="left">| Natl./Regl.
							</td>
							<td align="left">| Comments
							</td>
							<td align="left">| Reg. Price
							</td>
							<td align="left">| Qty
							</td>
						</tr>
						<tr>
							<td colspan="12"><hr>
							</td>
						</tr>
					</HeaderTemplate>
					<ItemTemplate>
						<tr>
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("CompanyName")%>' ID="Label18">
								</asp:Label>
							</td>
							
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("Brand_Name")%>' ID="Label1">
								</asp:Label>
							</td>
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("Item_Key")%>' ID="Label2">
								</asp:Label>
							</td>
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("Item_Description")%>' ID="Label3">
								</asp:Label>
							</td>
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("Package_Desc2")%>' ID="Label4">
								</asp:Label>
							</td>
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("Unit_Name")%>' ID="Label5">
								</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							</td>
							</td>
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("Sale_Price")%>' ID="Label6">
								</asp:Label>
							</td>
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("Sale_cost")%>' ID="Label7">
								</asp:Label>
							</td>
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("mgn")%>' ID="Label10">
								</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							</td>
							
							<td align="left">|
							</td>
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("comment2")%>' ID="Label8">
								</asp:Label>
							</td>
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("comment1")%>' ID="Label9">
								</asp:Label>
							</td>
							<td align="left">|
							</td>
							<td align="left">| &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							</td>
							
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("Price")%>' ID="Label11">
								</asp:Label></td>
						
						<td align="left">|
								<asp:Label Runat="server" Width="20" id="iQty" text='<%# Container.DataItem("OrderQty")%>'>
								</asp:Label>
							<asp:Label Runat="server" Visible="False" ID="iID" text='<%# Container.DataItem("Item_Key")%>'>
							</asp:Label>
							<asp:Label Runat="server" Visible="False" ID="ipbpID" text='<%# Container.DataItem("PriceBatchPromoID")%>'>
							</asp:Label>
							<asp:Label Runat="server" Visible="False" ID="ippoID" text='<%# Container.DataItem("PromoPreOrderID")%>'>
							</asp:Label>
							</td>
						</tr>
						<tr>
							<td colspan="11"><hr>
							</td>
						</tr>
					</ItemTemplate>
				</asp:Repeater>
			</table>
			
			<table class="regtable">
				<tr>
					<td><asp:linkButton ID="orderIt" Runat="server">Return to Main</asp:linkButton></td>
				</tr>
			</table>
</asp:Content>