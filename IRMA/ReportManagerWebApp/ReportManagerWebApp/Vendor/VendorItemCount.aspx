<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Vendor_VendorItemCount" title="Vendor Item Count" Codebehind="VendorItemCount.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="VendorReports.aspx?valuePath=Reports Home/Vendor">Vendor Report</a> &gt; Vendor Item Count</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 8.83em; height: 1em; text-align: right">
                Report Format</td>
            <td style="width: 23.43em; height: 1em; text-align: left">
                <asp:DropDownList ID="cmbReportFormat" runat="server" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" Text="View Report" Width="100px" /></td>
            <td style="width: 12.84em; height: 1em; text-align: right">
            </td>
        </tr>
    </table>
</asp:Content>

