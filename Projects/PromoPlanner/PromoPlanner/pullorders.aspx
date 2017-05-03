<%@ Page Language="vb" MasterPageFile="~/MasterPage.master"   AutoEventWireup="false" Inherits="PromoPlanner.pp_irma.pullorders" Codebehind="pullorders.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
			<table class="smallertable">
				<tr>
					<td>Promo Orders for , <b>
							<%=Session("startdate")%>
						</b>- <b>
							<%=session("enddate")%>
						</b>- Dept(s) <b>
							<%=session("deptno")%>
						</b>
					</td>
				</tr>
			</table>
			<table class="smallertable">
				<tr>
					<td>
						<table>
							<tr>
								<asp:datagrid id="promoItems" Width="1400" Runat="server" AlternatingItemStyle-BackColor="Silver"
									CssClass="smallertable" GridLines="Both" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
									EditItemStyle-HorizontalAlign="left" EnableViewState="false">
									<HeaderStyle HorizontalAlign="Left" VerticalAlign="Bottom" BackColor="#009900" Font-Bold="True"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Bottom"></ItemStyle>
									<EditItemStyle HorizontalAlign="Left" VerticalAlign="Bottom"></EditItemStyle>
									
								</asp:datagrid></tr>
						</table>
					</td>
				</tr>
			</table>
			<table class="smallertable">
				<tr>
					<td>
						<asp:LinkButton ID="expExcel" Runat="server" text="Export to Excel"></asp:LinkButton>
					</td>
				</tr>
			</table>
</asp:Content>
