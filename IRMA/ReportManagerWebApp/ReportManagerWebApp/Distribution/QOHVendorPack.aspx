<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Distribution_QOHVendorPack" title="IRMA Report Manager" Codebehind="QOHVendorPack.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar">
        <h3><a href="..\Default.aspx">Home</a> &gt; <a href="DistributionReports.aspx?valuePath=Reports Home/Distribution">Distribution Reports</a> &gt; QOH Vendor Pack</h3>
    </div>
    <table id="tblParameters" border="0">
        <tr>
            <td style="width: 225px; text-align: right">
                SubTeam: &nbsp;<br />
            </td>
            <td style="width: 249px; text-align: left">
                <br />
                <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="IC_SubTeam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="IC_SubTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT * FROM SubTeam WHERE EXEDistributed = 1"></asp:SqlDataSource>
            </td>
            <td style="width: 300px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbSubTeam" runat="server" ControlToValidate="cmbSubTeam"
                    ErrorMessage="* SubTeam is a required field." 
                    MaximumValue="2147483647" MinimumValue="1" SetFocusOnError="True"
                    Type="Integer" Width="220px"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 225px; text-align: right">
                DC Store: &nbsp;</td>
            <td style="width: 346px; text-align: left">
                <br />
                <asp:DropDownList ID="cmbDCStore" runat="server" DataSourceID="IC_DCStore" DataTextField="Store_Name"
                    DataValueField="Store_No" Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="IC_DCStore" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT * FROM Store WHERE Distribution_Center = 1"></asp:SqlDataSource>
            </td>
            <td style="width: 300px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbDCStore" runat="server" ControlToValidate="cmbDCStore"
                    ErrorMessage="* DC Store is a required field." MaximumValue="2147483647" MinimumValue="1"
                    SetFocusOnError="True" Type="Integer" Width="220px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 163px; text-align: right">
                <br />
                Report Format</td>
            <td colspan="2">
                <br />
                <asp:DropDownList ID="cmbReportFormat" runat="server" TabIndex="9" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td colspan="3">
            </td>
        </tr>
        <tr>
            <td style="width: 163px; text-align: right">
            </td>
            <td colspan="2">
            </td>
            <td colspan="3">
            </td>
        </tr>
        <tr>
            <td style="width: 163px; text-align: right">
            </td>
            <td colspan="2">
                <br />
                <asp:Button ID="btnReport" runat="server" Font-Names="Verdana" Font-Size="10pt" TabIndex="10"
                    Text="View Report" Width="100px" /></td>
            <td colspan="3">
            </td>
        </tr>
    </table>
</asp:Content>

