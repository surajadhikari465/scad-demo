<%@ Page Language="vb" MasterPageFile="~/MasterPage.master"   AutoEventWireup="false" Inherits="PromoPlanner.pp_irma.reviewpromo2" Codebehind="reviewpromo2.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
			<table class="regtable">
				<tr>
					<td>Reviewing Promo for <b>
							<%=session("dept")%>
							-
							<%=session("startdate")%>
							to
							<%=session("enddate")%>
						</b>
					</td>
				</tr>
			</table>
			<table>
				<tr>
					<td>
						<table>
							<tr>
								<asp:datagrid id="promoItems" Runat="server" EditItemStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="Left"
									HeaderStyle-HorizontalAlign="Left" GridLines="Both" Width="1400" CssClass="smallertable" OnUpdateCommand="promoItems_Update"
									OnEditCommand="promoItems_Edit" OnDeleteCommand="promoItems_Delete" OnCancelCommand="promoItems_Cancel"
									AlternatingItemStyle-BackColor="Silver" AutoGenerateColumns="False">
									<HeaderStyle HorizontalAlign="Left" VerticalAlign="Bottom" BackColor="#009900" Font-Bold="True"></HeaderStyle>
									<ItemStyle HorizontalAlign="Left" VerticalAlign="Bottom"></ItemStyle>
									<EditItemStyle HorizontalAlign="Left" VerticalAlign="Bottom"></EditItemStyle>
									<Columns>
										<asp:EditCommandColumn EditText="Edit" CancelText="Cancel" UpdateText="Update"></asp:EditCommandColumn>
										<asp:TemplateColumn>
											<HeaderTemplate>
												<td>
													<asp:label Runat="server" text="Store" ID="Label10"></asp:label></td>
												<td>
													<asp:label Runat="server" text="UPC" ID="Label1"></asp:label></td>
												<td>
													<asp:label Runat="server" text="Description" ID="Label2"></asp:label></td>
												<td>
													<asp:label Runat="server" text="Size" ID="Label3"></asp:label></td>
												<td>
													<asp:label Runat="server" text="UOM" ID="Label4"></asp:label></td>
												<td>
													<asp:label Runat="server" text="Manuf" ID="Label5"></asp:label></td>
												<td>
													<asp:label Runat="server" text="Vendor" ID="Label6"></asp:label></td>
												<td>
													<asp:label Runat="server" text="Current Price" ID="Label20"></asp:label></td>
												<td>
													<asp:label Runat="server" text="Sale Price" ID="Label8"></asp:label></td>
												<td>
													<asp:label Runat="server" text="Sale Cost" ID="Label9"></asp:label></td>
												<td>
													<asp:label Runat="server" text="Dept" ID="Label11"></asp:label></td>
												<td>
													<asp:label Runat="server" text="Nat'l/Regn'l" ID="Label12"></asp:label></td>
												<td>
													<asp:label Runat="server" text="Comments" ID="Label13"></asp:label></td>
												<td>
													<asp:label Runat="server" text="Default Qty" ID="Label15"></asp:label></td>
											</HeaderTemplate>
											<ItemTemplate>
												<td>
													<asp:label Runat="server" Visible="False" ID="lbl_ID" text='<%# Container.DataItem("PriceBatchPromoID")%>'>
													</asp:label>
													<asp:label Runat="server" ID="Label14" text='<%# Container.DataItem("Store_No")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" ID="Label23" text='<%# Container.DataItem("Identifier")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Item_Description")%>' ID="Label7">
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Package_Desc2")%>' ID="Label17">
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Unit_Name")%>' ID="Label18">
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Brand_Name")%>' ID="Label19">
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("CompanyName")%>' ID="Label21">
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Price")%>' ID="Label22">
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Sale_Multiple")%>' ID="Label24">
													</asp:label>/
													<asp:label Runat="server" text='<%# Container.DataItem("Sale_Price")%>' ID="Label25">
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Sale_Cost")%>' ID="Label26">
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Dept_No")%>' ID="Label27">
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("comment2")%>' ID="Label28">
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("comment1")%>' ID="Label29">
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("projunits")%>' ID="Label30">
													</asp:label></td>
											</ItemTemplate>
											<EditItemTemplate>
												<td>
													<asp:label Runat="server" Visible="False" ID="lbl_ID2" text='<%# Container.DataItem("PriceBatchPromoID")%>'>
													</asp:label>
													<asp:label Runat="server" Visible="False" ID="lbl_Item_Key2" text='<%# Container.DataItem("Item_Key")%>'>
													</asp:label>
													<asp:label Runat="server" ID="lbl_store2" text='<%# Container.DataItem("Store_No")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" ID="Label32" text='<%# Container.DataItem("Identifier")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Item_Description")%>' ID="Label33">
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Package_Desc2")%>' ID="Label34">
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Unit_Name")%>' ID="Label35">
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Brand_Name")%>' ID="Label36">
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("CompanyName")%>' ID="Label37">
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Price")%>' ID="Label38">
													</asp:label></td>
												<td>
													<asp:textbox Width="60" Runat="server" ID="txt_price" text='<%# Container.DataItem("Sale_Price")%>'>
													</asp:textbox></td>
												<td>
													<asp:textbox Width="60" Runat="server" id="txt_cost" text='<%# Container.DataItem("Sale_Cost")%>'>
													</asp:textbox></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Dept_No")%>' ID="Label39">
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("comment2")%>' ID="Label40">
													</asp:label></td>
												<td>
													<asp:textbox Width="100" Runat="server" id="txt_com1" text='<%# Container.DataItem("comment1")%>'>
													</asp:textbox></td>
												<td>
													<asp:textbox Runat="server" Width="20" text='<%# Container.DataItem("projunits")%>' ID="qty">
													</asp:textbox></td>
											</EditItemTemplate>
										</asp:TemplateColumn>
										<asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete"></asp:ButtonColumn>
									</Columns>
								</asp:datagrid></tr>
						</table>
					</td>
				</tr>
			</table>
			<table class="regtable">
				<TR>
					<td><asp:linkbutton id="main" Runat="server">Return to Main</asp:linkbutton></td>
				</TR>
			</table>
</asp:Content>