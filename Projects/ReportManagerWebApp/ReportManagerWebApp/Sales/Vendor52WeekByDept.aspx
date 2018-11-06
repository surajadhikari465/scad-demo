<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Sales_Vendor52WeekByDept" title="Vendor 52-Week By Dept" Codebehind="Vendor52WeekByDept.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="SalesReports.aspx?valuePath=Reports Home/Sales">Sales Reports</a> &gt; Vendor 52-Week By Dept</h3>
    </div>
    <table style="width: 842px;">
        <tr>
            <td style="width: 259px; text-align: right;">
                Report Format</td>
            <td colspan="5">
                <asp:DropDownList ID="cmbReportFormat" runat="server" Width="120px" TabIndex="13">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" TabIndex="14"
                    Text="View Report" Width="100px" ValidationGroup="submit" /></td>
        </tr>
    </table>
    <br />     
</asp:Content>

