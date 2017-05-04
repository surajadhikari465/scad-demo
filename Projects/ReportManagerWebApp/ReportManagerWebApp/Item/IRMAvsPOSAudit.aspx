<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Item_IRMAvsPOSAudit" Codebehind="IRMAvsPOSAudit.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar">
        <h3><a href="..\Default.aspx">Home</a> &gt; <a href="ItemReports.aspx?valuePath=Reports Home/Items">Item Reports</a> &gt; IRMA vs. POS Audit</h3>
    </div>
    <table id="tblParameters" border="0">
        <tr>
            <td colspan="2" style="text-align: center" title="IRMA Report Manager">
                The parameters for this report are entered in the report.</td>
            <td style="width: 105px; text-align: left">
                <asp:Button ID="btnReport" runat="server" Text="View Report" Width="96px" /></td>
        </tr>
    </table>
</asp:Content>

