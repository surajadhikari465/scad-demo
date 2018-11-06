<%@ Page Language="vb" MasterPageFile="~/MasterPage.master"   AutoEventWireup="false" Inherits="PromoPlanner.pp_irma.promoorders2" Codebehind="promoorders2.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
			<table class="regtable">
				<tr>
					<td>Placing orders for
						<%=session("storeno")%>
						-
						<%=session("deptno")%>
						-
						<%=session("startdate")%>
						to
						<%=session("enddate")%>
					</td>
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
								<asp:Label Runat="server" text='<%# Container.DataItem("Brand_Name")%>'>
								</asp:Label>
							</td>
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("Identifier")%>'>
								</asp:Label>
							</td>
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("Item_Description")%>'>
								</asp:Label>
							</td>
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("Package_Desc2")%>'>
								</asp:Label>
							</td>
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("Unit_Name")%>'>
								</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							</td>
							</td>
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("Sale_Price")%>'>
								</asp:Label>
							</td>
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("Sale_cost")%>'>
								</asp:Label>
							</td>
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("mgn")%>' ID="Label10">
								</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							</td>
							
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("comment2")%>'>
								</asp:Label>
							</td>
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("comment1")%>'>
								</asp:Label>
							</td>
						
							
							<td align="left">|
								<asp:Label Runat="server" text='<%# Container.DataItem("Price")%>'>
								</asp:Label></td>
						
						<td align="left">|
							<asp:TextBox Runat="server" Width="20" id="iQty" text='<%# Container.DataItem("OrderQty")%>'>
							</asp:TextBox>				
							<asp:Label Runat="server" Visible="False" ID="iID" text='<%# Container.DataItem("Item_Key")%>'>
							</asp:Label>
							<asp:Label Runat="server" Visible="False" ID="pbpID" text='<%# Container.DataItem("PriceBatchPromoID")%>'>
							</asp:Label>
							<asp:Label Runat="server" Visible="False" ID="ppoID" text='<%# Container.DataItem("PromoPreOrderID")%>'>
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
					<td><asp:linkButton ID="orderIt" Runat="server">Submit Order</asp:linkButton></td>
				</tr>
			</table>
</asp:Content>