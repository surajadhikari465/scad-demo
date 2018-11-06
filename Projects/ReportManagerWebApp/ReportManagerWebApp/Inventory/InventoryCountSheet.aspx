<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Inventory_InventoryCountSheet" title="Inventory Count Sheet Report" Codebehind="InventoryCountSheet.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="InventoryReports.aspx?valuePath=Reports Home/Inventory">Inventory Reports</a> &gt; Inventory Count Sheet</h3>
</div>
    <table border="0">
        <tr>
            <td style="width: 192px; text-align: right">
                <br />
                <br />
                Store: &nbsp;
            </td>
            <td style="width: 249px; text-align: left">
                <br />
                <br />
                <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="IC_Store" DataTextField="Store_Name"
                    DataValueField="Store_No" Font-Names="Verdana" Font-Size="10pt" TabIndex="1"
                    Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="IC_Store" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT Store_No, Store_Name FROM Store WHERE (WFM_Store = 1 or Mega_Store = 1 or Manufacturer = 1) ORDER BY Store_Name">
                </asp:SqlDataSource>
            </td>
            <td style="width: 318px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbStore" runat="server" ControlToValidate="cmbStore"
                    ErrorMessage="* Store is a required field." MaximumValue="2147483647" MinimumValue="1"
                    SetFocusOnError="True" Type="Integer" Width="201px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
                <br />
                SubTeam: &nbsp;
            </td>
            <td style="width: 249px; text-align: left">
                <br />
                <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="IC_SubTeam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" Font-Names="Verdana" Font-Size="10pt" TabIndex="1"
                    Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="IC_SubTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT * FROM [SubTeam]">
                </asp:SqlDataSource>
            </td>
            <td style="width: 318px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbSubTeam" runat="server" ControlToValidate="cmbSubTeam"
                    ErrorMessage="* SubTeam is a required field." MaximumValue="2147483647" MinimumValue="1"
                    SetFocusOnError="True" Type="Integer" Width="227px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
                <br />
                Order History Days: &nbsp;
            </td>
            <td style="width: 249px; text-align: left">
                <br />
                <asp:TextBox ID="txtOrderHistoryDays" runat="server"></asp:TextBox><br />
            </td>
            <td style="width: 318px; text-align: left">
                <asp:RangeValidator ID="rngValid_txtOrderHistoryDays" runat="server" ControlToValidate="txtOrderHistoryDays"
                    ErrorMessage="* The valid range for Order History Days is 0 - 2557 (~7 years)."
                    MaximumValue="2557" MinimumValue="0" SetFocusOnError="True" Type="Integer"
                    Width="276px"></asp:RangeValidator></td>
        </tr>
<tr>
            <td style="width: 192px; text-align: right">
                <br />
                Report Format:&nbsp;
            </td>
            <td style="width: 346px; text-align: left">
                <br />
                <asp:DropDownList ID="cmbReportFormat" runat="server" TabIndex="8" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
            </td>
            <td style="width: 249px; text-align: left">
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
            </td>
            <td style="width: 249px; text-align: left">
                <br />
                <asp:Button ID="btnReport" runat="server" TabIndex="9" Text="View Report" Width="96px" /></td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>        
    </table>
</asp:Content>

