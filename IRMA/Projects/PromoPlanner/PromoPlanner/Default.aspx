<%@ Page Language="vb" MasterPageFile="~/MasterPage.master"  AutoEventWireup="false" Inherits="PromoPlanner.pp_irma._default" Codebehind="default.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
			<table class="regtable">
				<tr>
					<td>Username:</td>
					<td><asp:textbox id="username" Runat="server" Width="160px"></asp:textbox></td>
				</tr>
				<tr>
					<td>Password:</td>
					<td><asp:textbox id="password" Runat="server" TextMode="Password" Width="160px"></asp:textbox></td>
				</tr>
				<tr>
					<td><asp:Button ID="login1" Runat="server" Text="Login"></asp:Button></td>
				</tr>
			</table>
</asp:Content>
