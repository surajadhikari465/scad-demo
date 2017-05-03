<%@ Reference Page="~/buildpromo.aspx" %>
<%@ Page Language="vb" MasterPageFile="~/MasterPage.master"  AutoEventWireup="false" Inherits="PromoPlanner.pp_irma.index" Codebehind="index.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
			<table class="regtable">
				<tr>
					<td><hr>
					</td>
				</tr>
				<tr>
					<td>Dept(s) are required for all options.
					</td>
				</tr>
				<tr>
					<td>Select Dept(s):&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="dept" Runat="server"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td><asp:LinkButton ID="buildPromo" Runat="server">Build New Promo</asp:LinkButton>
                        <br />
                        <asp:LinkButton ID="lnkUploadPromos" runat="server">Upload Promos</asp:LinkButton></td>
				</tr>
				<tr>
					<td><hr>
					</td>
				</tr>
				<tr>
					<td>Dates are only required for "Review/Edit", "Posted Review" and "Pull Orders".
					</td>
				</tr>
				<tr>
					<td>Select Dates:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="dates" Runat="server"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td><asp:Linkbutton ID="revPromos" Runat="server">Review/Edit Promos</asp:Linkbutton></td>
				</tr>
				<tr>
					<td><hr>
					</td>
				</tr>
				<tr>
					<td>Select Dates:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="orddates" Runat="server"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td><asp:LinkButton ID="getOrders" Runat="server">Pull Store_No Orders</asp:LinkButton></td>
				</tr>
				<tr>
					<td><hr>
					</td>
				</tr>
				<tr>
					<td>Select Dates:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:dropdownlist id="dates2" Runat="server"></asp:dropdownlist></td>
				</tr>
				<tr>
					<td><font color="red">When doing a Final Review/Edit please be sure to choose your full 
							department from the dropdown.</font></td>
				</tr>
				<tr>
					<td><asp:Linkbutton ID="revPromos2" Runat="server">Final Review/Edit Promos</asp:Linkbutton></td>
				</tr>
				<tr>
					<td><hr>
					</td>
				</tr>
			</table>
</asp:Content>