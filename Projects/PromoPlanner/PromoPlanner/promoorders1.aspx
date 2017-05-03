<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="PromoPlanner.pp_irma.promoorders1" Codebehind="promoorders1.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
			<table class="regtable">
				<tr>
					<td>
						<hr>
					</td>
				</tr>
				<tr>
					<td>Select Store_No:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="Store_Nos" Runat="server"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td>Select Dept(s):&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="dept" Runat="server"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td>Select Dates:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="orddates" Runat="server"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td><asp:linkbutton id="getOrders" Runat="server">Place Order</asp:linkbutton></td>
				</tr>
				<tr>
					<td>
						<hr>
					</td>
				</tr>
			</table>
</asp:Content>