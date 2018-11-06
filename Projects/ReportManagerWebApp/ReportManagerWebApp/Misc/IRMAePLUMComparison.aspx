<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Misc_IRMAePlumCompare" title="Report Manager - IRMA Scan Audit" Codebehind="IRMAePLUMComparison.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="MiscReports.aspx?valuePath=Reports Home/Misc">Miscellaneous Reports</a> &gt; IRMA/ePLUM Comparison Report</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 192px; text-align: right;">
                <br />
                Store:&nbsp;
            </td>
            <td style="width: 243px; text-align: left;">
                <br />
                <asp:DropDownList ID="cmbStore" runat="server" Width="225px" DataSourceID="IC_Store" DataTextField="Store_Name" DataValueField="Store_No" Font-Names="Verdana" Font-Size="10pt" TabIndex="1">
                </asp:DropDownList><asp:SqlDataSource ID="IC_Store" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="SELECT Store_Name, Store_No FROM Store WHERE WFM_Store = 1 ORDER BY Store_Name" ProviderName="<%$ ConnectionStrings:ReportManager_Conn.ProviderName %>"></asp:SqlDataSource></td>
            <td style="width: 318px; text-align: left">
                <br />
                <asp:RangeValidator ID="rngValid_cmbStore" runat="server" ControlToValidate="cmbStore"
                    ErrorMessage="Store is a required field." MaximumValue="2147483647" MinimumValue="1"
                    SetFocusOnError="True" Type="Integer" Width="201px" Enabled="False"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
                <br />
                Team:&nbsp;</td>
            <td style="width: 243px; text-align: left">
                <br />
                <asp:DropDownList ID="cmbTeam" runat="server" Width="225px" DataSourceID="IC_Team" DataTextField="Team_Name" DataValueField="Team_No" Font-Names="Verdana" Font-Size="10pt" TabIndex="1">
                </asp:DropDownList><asp:SqlDataSource ID="IC_Team" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="SELECT Team_No, Team_Name FROM Team ORDER BY Team_Name" ProviderName="<%$ ConnectionStrings:ReportManager_Conn.ProviderName %>"></asp:SqlDataSource>
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
                <br />
                Report Type:&nbsp;
            </td>
            <td style="width: 243px; text-align: left">
                <br />
                &nbsp;<asp:RadioButton ID="optSummary" runat="server" Checked="True" GroupName="ReportType"
                    Text="Summary" AutoPostBack="True" />
                <br />
                &nbsp;<asp:RadioButton ID="optDetail" runat="server" GroupName="ReportType" Text="Detail" AutoPostBack="True" /><br />
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
                <br />
                Report Format</td>
            <td style="width: 243px; text-align: left">
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
            <td style="width: 243px; text-align: left">
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
            </td>
            <td style="width: 243px; text-align: left">
                <br />
                <asp:Button ID="btnReport" runat="server" TabIndex="9" Text="View Report" Width="96px" /></td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
    </table>
    </asp:Content>

