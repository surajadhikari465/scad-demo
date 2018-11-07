<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Vendor_LeadTimeVendors" title="IRMA Report Manager" Codebehind="LeadTimeVendors.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar">
        <h3><a href="..\Default.aspx">Home</a> &gt; 
            <a href="VendorReports.aspx?valuePath=Reports Home/Vendor">Vendor Reports</a> &gt; 
            Lead Time Vendors</h3>
    </div>
    <table id="tblParameters" border="0">
        <tr>
            <td colspan="2" style="text-align: center">
                &lt; No Options Available for Report &gt; </td>
            <td style="width: 102px; text-align: left">
                <asp:Button ID="btnReport" runat="server" Text="View Report" Width="96px" /></td>
        </tr>
    </table>
</asp:Content>

