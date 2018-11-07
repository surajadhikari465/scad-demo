<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Distribution_GetReceivingListForNOIDNORD" title="Report Manager - Item Exception Report" Codebehind="GetReceivingListForNOIDNORD.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="DistributionReports.aspx?valuePath=Reports Home/Distribution">Distribution Reports</a> &gt; Facility Average Cost</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 137px; text-align: right; height: 56px;">
                Order ID:&nbsp;
            </td>
            <td style="width: 221px; text-align: left; height: 56px;">
                &nbsp;<asp:TextBox ID="OrderHeaderIDTextBox" runat="server" Width="167px"></asp:TextBox>
            </td>
            <td style="width: 552px; height: 56px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 137px; text-align: right">
            </td>
            <td style="width: 221px; text-align: left">
                <br />
                <asp:Button ID="btnReport" runat="server" TabIndex="9" Text="View Report" Width="96px" /></td>
            <td style="width: 314px; text-align: left">
            </td>
            <td style="width: 552px; text-align: left">
            </td>
        </tr>
    </table>
    </asp:Content>

