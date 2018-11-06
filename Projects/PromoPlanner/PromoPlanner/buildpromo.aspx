<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="PromoPlanner.pp_irma.buildpromo" Codebehind="buildpromo.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
			<table class="regtable">
				<tr>
					<td colspan="4">
						<hr/>
					</td>
				</tr>
				<tr>
					<td>Start Date:</td>
					<td><asp:textbox id="startdate" MaxLength="10" EnableViewState="True" Width="80" Runat="server"></asp:textbox><asp:regularexpressionvalidator id="valStartdate" Runat="server" ErrorMessage="Dates must be formatted as mm/dd/yyyy"
							ControlToValidate="startdate" ValidationExpression="(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d" Display="Dynamic"></asp:regularexpressionvalidator></td>
					<td>End Date:</td>
					<td><asp:textbox id="enddate" MaxLength="10" EnableViewState="True" Width="80" Runat="server"></asp:textbox><asp:regularexpressionvalidator id="valEnddate" Runat="server" ErrorMessage="Dates must be formatted as mm/dd/yyyy"
							ControlToValidate="enddate" ValidationExpression="(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d"></asp:regularexpressionvalidator></td>
				</tr>
				<tr>
					<td colspan="4">
						<hr/>
					</td>
				</tr>
				<tr>
					<td>UPC:</td>
					<td><asp:textbox id="upc" MaxLength="13" EnableViewState="False" Width="150" Runat="server"></asp:textbox><asp:regularexpressionvalidator id="valUPC" Runat="server" ErrorMessage="UPC must be 13 digits" ControlToValidate="upc"
							ValidationExpression="\d{13}" Display="Dynamic"></asp:regularexpressionvalidator></td>
				</tr>
				<tr>
					<td>Sale Price:</td>
					<td><asp:textbox id="salPM" MaxLength="2" EnableViewState="False" Width="20" Runat="server"></asp:textbox>@<asp:textbox id="Sale_Price" MaxLength="6" Width="80" Runat="server"></asp:textbox><asp:requiredfieldvalidator id="valPM" Runat="server" ErrorMessage="Price Multiple must be entered." ControlToValidate="salPM"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="valSale_Price" Runat="server" ErrorMessage="Sale_Price is a required field. If %'s off are being used then please enter zero."
							ControlToValidate="Sale_Price"></asp:requiredfieldvalidator></td>
					<td>Sale Cost:</td>
					<td><asp:textbox id="salecost" MaxLength="6" EnableViewState="False" Width="80" Runat="server"></asp:textbox><asp:requiredfieldvalidator id="valSalecost" Runat="server" ErrorMessage="Salecost is a required field. If %'s off are being used then please enter zero."
							ControlToValidate="salecost"></asp:requiredfieldvalidator></td>
				</tr>
				<tr>
					<td>% Off Retail:</td>
					<td><asp:textbox id="pricePerc" MaxLength="4" EnableViewState="False" Width="80" Runat="server"></asp:textbox><asp:requiredfieldvalidator id="valPricePerc" Runat="server" ErrorMessage="Percentage off price is a required field. If you're entering actual retail then please enter zero."
							ControlToValidate="pricePerc"></asp:requiredfieldvalidator></td>
					<td>% Off Cost:</td>
					<td><asp:textbox id="costPerc" MaxLength="4" EnableViewState="False" Width="80" Runat="server"></asp:textbox><asp:requiredfieldvalidator id="valCostPerc" Runat="server" ErrorMessage="Percentage off price is a required field. If you're entering actual cost then please enter zero."
							ControlToValidate="costPerc"></asp:requiredfieldvalidator></td>
				</tr>
				<tr>
					<td>Billback:</td>
					<td><asp:textbox id="billBack" MaxLength="6" EnableViewState="False" Width="80" Runat="server"></asp:textbox><asp:requiredfieldvalidator id="valBillback" Runat="server" ErrorMessage="Billback is a required field. If you're not using billback then please enter zero."
							ControlToValidate="billBack"></asp:requiredfieldvalidator></td>
					<td>Vendor:</td>
					<td><asp:dropdownlist id="Vendor_ID" EnableViewState="True" Width="100" Runat="server">
							<asp:ListItem Value="0">No Vendor</asp:ListItem>
						</asp:dropdownlist></td>
				</tr>
				<tr>
					<td>Comments:</td>
					<td colspan="2"><asp:textbox id="comments" MaxLength="50" EnableViewState="False" Width="200" Runat="server"></asp:textbox><asp:requiredfieldvalidator id="valCom1" Runat="server" ErrorMessage="Comments is a required field. If you're not using billback then please enter zero."
							ControlToValidate="comments"></asp:requiredfieldvalidator></td>
					<td>Nat'l/Regn'l:&nbsp;<asp:textbox id="NorY" EnableViewState="False" Width="20" Runat="server"></asp:textbox><asp:requiredfieldvalidator id="valCom2" Runat="server" ErrorMessage="Regional/Natle is a required field. If you're not using billback then please enter zero."
							ControlToValidate="NorY"></asp:requiredfieldvalidator></td>
				</tr>
				<tr>
					<td>Default Qty:</td>
					<td><asp:textbox id="defQty" MaxLength="2" Runat="server" width="20"></asp:textbox><asp:requiredfieldvalidator id="valQty" Runat="server" ErrorMessage="Default Qty is a required field. If you're not requiring a minimum order then please enter zero."
							ControlToValidate="defQty"></asp:requiredfieldvalidator></td>
				</tr>
				<tr>
					<td><asp:button id="insertItem" Runat="server" Text="Add Item"></asp:button></td>
				</tr>
				<tr>
					<td colspan="4">
						<hr/>
					</td>
				</tr>
			</table>
			<table class="regtable">
				<tr>
					<td>
						<table>
							<tr>
								<asp:datagrid id="promoItems" Width="1400" Runat="server" AutoGenerateColumns="False" AlternatingItemStyle-BackColor="Silver"
									OnCancelCommand="promoItems_Cancel" OnDeleteCommand="promoItems_Delete" OnEditCommand="promoItems_Edit"
									OnUpdateCommand="promoItems_Update" CssClass="smallertable" GridLines="Both" HeaderStyle-HorizontalAlign="Left"
									ItemStyle-HorizontalAlign="Left" EditItemStyle-HorizontalAlign="left">
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
													<asp:label Runat="server" text='<%# Container.DataItem("Item_Description")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Package_Desc2")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Unit_Name")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Brand_Name")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("CompanyName")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Price")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Sale_Multiple")%>'>
													</asp:label>/
													<asp:label Runat="server" text='<%# Container.DataItem("Sale_Price")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Sale_Cost")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Dept_No")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("comment2")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("comment1")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("projunits")%>'>
													</asp:label></td>
											</ItemTemplate>
											<EditItemTemplate>
												<td>
													<asp:label Runat="server" Visible="False" ID="lbl_ID2" text='<%# Container.DataItem("PriceBatchPromoID")%>'>
													</asp:label>
													<asp:label Runat="server" ID="Label17" text='<%# Container.DataItem("Store_No")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" ID="Label7" text='<%# Container.DataItem("Identifier")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Item_Description")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Package_Desc2")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Unit_Name")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Brand_Name")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("CompanyName")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Price")%>' ID="Label18">
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Sale_Multiple")%>' ID="Label19">
													</asp:label>/
													<asp:textbox Width="60" Runat="server" ID="txt_price" text='<%# Container.DataItem("Sale_Price")%>'>
													</asp:textbox></td>
												<td>
													<asp:textbox Width="60" Runat="server" id="txt_cost" text='<%# Container.DataItem("Sale_Cost")%>'>
													</asp:textbox></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("Dept_No")%>'>
													</asp:label></td>
												<td>
													<asp:label Runat="server" text='<%# Container.DataItem("comment2")%>'>
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
				<tr>
					<td><asp:linkbutton id="main" Runat="server">Return to Main</asp:linkbutton></td>
				</tr>
			</table>
</asp:Content>
