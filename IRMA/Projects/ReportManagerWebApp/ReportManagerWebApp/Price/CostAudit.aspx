<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.CostAudit_1" title="Report Manager - Cost Audit" Codebehind="CostAudit.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="PriceReports.aspx?valuePath=Reports Home/Prices">Price Reports</a> &gt; Cost Audit Report</h3>
    </div>
    
    <table border="0">
        <tr>
            <td style="width: 150px; height: 1px; text-align: right">
                Vendor</td>
            <td style="width: 346px; height: 1px; text-align: left">
                <asp:DropDownList ID="cmbVendor" runat="server" DataSourceID="ICVendors" DataTextField="CompanyName"
                    DataValueField="Vendor_ID" Width="225px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICVendors" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetVendors" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 318px; height: 1px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbVendor" runat="server" ControlToValidate="cmbVendor"
                    ErrorMessage="* Vendor is a required field." 
                    MaximumValue="2147483647" MinimumValue="1" SetFocusOnError="True"
                    Type="Integer" Width="176px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right">
                Team
            </td>
            <td style="width: 249px; text-align: left">
                <asp:DropDownList ID="cmbTeam" runat="server" DataSourceID="ICTeam" DataTextField="Team_Name"
                    DataValueField="Team_No"  Width="225px" AutoPostBack="True">
                </asp:DropDownList><asp:SqlDataSource ID="ICTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 318px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbTeam" runat="server" ControlToValidate="cmbTeam"
                    ErrorMessage="* Team is a required field." 
                    MaximumValue="2147483647" MinimumValue="1" SetFocusOnError="True"
                    Type="Integer" Width="176px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right">
               Subteam

            </td>
            <td style="width: 249px; text-align: left">
                <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="ICSubteam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="ICSubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT ST.SubTeam_No, ST.SubTeam_Name FROM SubTeam ST (NOLOCK) WHERE ST.Team_No = ISNULL(@Team_No, ST.Team_No)">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="cmbTeam" DefaultValue="DBNull" Name="Team_No" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
           <td style="width: 318px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbSubTeam" runat="server" ControlToValidate="cmbSubTeam"
                    ErrorMessage="* SubTeam is a required field." 
                    MaximumValue="2147483647" MinimumValue="1" SetFocusOnError="True"
                    Type="Integer" Width="176px"></asp:RangeValidator></td>
        </tr> 
        <tr>
            <td style="width: 150px; text-align: right;">
            </td>
            <td style="width: 249px; text-align: left;">
            </td>
            <td style="width: 318px; text-align: left;">
            </td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right">
                Report Format</td>
             <td style="width: 249px; text-align: left"><asp:DropDownList ID="cmbReportFormat" runat="server" Width="120px">
                <asp:ListItem>CSV</asp:ListItem>
                <asp:ListItem>EXCEL</asp:ListItem>
                <asp:ListItem Selected="True">HTML</asp:ListItem>
                <asp:ListItem>PDF</asp:ListItem>
                <asp:ListItem>XML</asp:ListItem>
            </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" Text="View Report"
                    Width="96px" /></td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
    </table>

</asp:Content>

