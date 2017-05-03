<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Distribution_IRMAEXEChecklist" title="IRMA Report Manager" Codebehind="IRMAEXEChecklist.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar">
        <h3><a href="..\Default.aspx">Home</a> &gt; <a href="DistributionReports.aspx?valuePath=Reports Home/Distribution">Distribution Reports</a> &gt; IRMA/EXE Checklist</h3>
    </div>
    <table id="tblParameters" border="0">
        <tr>
            <td style="width: 225px; text-align: right">
                SubTeam: &nbsp;<br />
            </td>
            <td style="width: 249px; text-align: left">
                <br />
                <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="IC_SubTeam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" Width="225px" TabIndex="1">
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
                    DataValueField="Store_No" Width="225px" TabIndex="2">
                </asp:DropDownList><asp:SqlDataSource ID="IC_DCStore" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT * FROM Store WHERE Distribution_Center = 1"></asp:SqlDataSource>
            </td>
            <td style="width: 300px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbDCStore" runat="server" ControlToValidate="cmbDCStore"
                    ErrorMessage="* DC Store is a required field." MaximumValue="2147483647" MinimumValue="1"
                    SetFocusOnError="True" Type="Integer" Width="220px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 225px; text-align: right">
                <span>DC Vendor:&nbsp; </span></td>
            <td style="width: 346px; text-align: left">
                <br />
                <asp:DropDownList
                    ID="cmbVendor" runat="server" DataSourceID="ICVendors" DataTextField="CompanyName"
                    DataValueField="Vendor_ID" Width="225px" TabIndex="3">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICVendors" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT * FROM Vendor WHERE EXEWarehouseVendSent = 1 AND EXEWarehouseCustSent = 1">
                    <SelectParameters>
                        <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 300px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbDCVendor" runat="server" ControlToValidate="cmbVendor"
                    ErrorMessage="* DC Vendor is a required field." MaximumValue="2147483647" MinimumValue="1"
                    SetFocusOnError="True" Type="Integer" Width="232px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 225px; text-align: right; height: 86px;">
                <span>Store:&nbsp;
                    <br />
                    <span style="font-size: 10pt">(used to compare cost records) &nbsp; </span></span></td>
            <td style="width: 346px; text-align: left; height: 86px;">
                <br />
                <asp:DropDownList
                    ID="cmbStore" runat="server" DataSourceID="IC_Store" DataTextField="Store_Name"
                    DataValueField="Store_No" Width="225px" TabIndex="4">
                </asp:DropDownList><asp:SqlDataSource ID="IC_Store" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT * FROM Store WHERE WFM_Store = 1">
                    <SelectParameters>
                        <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <br />
            </td>
            <td style="width: 300px; text-align: left; height: 86px;">
                <asp:RangeValidator ID="rngValid_cmbStore" runat="server" ControlToValidate="cmbStore"
                    ErrorMessage="* Store is a required field." MaximumValue="2147483647" MinimumValue="1"
                    SetFocusOnError="True" Type="Integer" Width="232px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 163px; height: 24px; text-align: right">
                Sort By:&nbsp;<br />
            </td>
            <td colspan="2" style="height: 24px">
                <asp:DropDownList ID="cmbSortBy" runat="server" TabIndex="5" Width="224px">
                    <asp:ListItem Value="1">Identifier</asp:ListItem>
                    <asp:ListItem Value="2">Error Message</asp:ListItem>
                </asp:DropDownList><br />
            </td>
            <td colspan="3" style="height: 24px">
            </td>
        </tr>
        <tr>
            <td style="width: 163px; text-align: right">
                <br />
                Report Format:&nbsp;
            </td>
            <td colspan="2">
                <br />
                <asp:DropDownList ID="cmbReportFormat" runat="server" TabIndex="6" Width="120px">
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
                <asp:Button ID="btnReport" runat="server" Font-Names="Verdana" Font-Size="10pt" TabIndex="7"
                    Text="View Report" Width="100px" /></td>
            <td colspan="3">
            </td>
        </tr>
    </table>
</asp:Content>

