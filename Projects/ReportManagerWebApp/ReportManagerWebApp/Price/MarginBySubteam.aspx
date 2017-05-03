<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Price_MarginBySubTeam" title="IRMA Report Manager" Codebehind="MarginBySubteam.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="PriceReports.aspx?valuePath=Reports Home/Prices">Price Reports</a> &gt; Margin By Subteam</h3>
    </div>
        <table style="width: 725px;">
            <tr>
                <td style="width: 259px; text-align: right; height: 20px;">
                    Store</td>
                <td style="width: 487px; height: 20px;" >
                    <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                        DataValueField="Store_No" Height="24px" Width="160px" TabIndex="1">
                    </asp:DropDownList><asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                        SelectCommand="GetAllStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                </td>
                <td colspan="4" style="height: 20px">
                    <asp:RequiredFieldValidator ID="reqfld_cmbStore" runat="server" ControlToValidate="cmbStore"
                        ErrorMessage="* Store is a required field."></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 259px; text-align: right;">
                    Subteam</td>
                <td style="width: 487px">
                    <asp:DropDownList ID="cmbSubteam" runat="server" DataSourceID="ICSubteams" DataTextField="SubTeam_Name"
                        DataValueField="SubTeam_No" TabIndex="2"
                        Width="160px">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="ICSubteams" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                        SelectCommand="GetAllSubTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                </td>
                <td colspan="4">
                    <asp:RequiredFieldValidator ID="reqfld_cmbSubteam" runat="server" ControlToValidate="cmbSubteam"
                        ErrorMessage="* Subteam is a required field." ></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 259px; text-align: right">
                    Minimum Value</td>
                <td style="width: 487px">
                    <asp:TextBox ID="txtMinval" runat="server" Width="70px" TabIndex="3"></asp:TextBox></td>
                <td colspan="4">
                    <asp:RequiredFieldValidator ID="reqfld_txtMinval" runat="server" ControlToValidate="txtMinval"
                        ErrorMessage="* Minimum value is a required field." Width="270px"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 259px; text-align: right">
                    Maximum Value</td>
                <td style="width: 487px">
                    <asp:TextBox ID="txtMaxval" runat="server" Width="70px" TabIndex="4"></asp:TextBox></td>
                <td colspan="4">
                    <asp:RequiredFieldValidator ID="reqfld_txtMaxval" runat="server" ControlToValidate="txtMaxval"
                        ErrorMessage="* Maximum value is a required field." Width="270px"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 259px; text-align: right">
                    In Range</td>
                <td style="width: 487px">
                    <asp:RadioButton ID="rdbtn_InRange" runat="server" Checked="True" GroupName="range" TabIndex="5" /></td>
                <td colspan="4">
                </td>
            </tr>
            <tr>
                <td style="width: 259px; text-align: right; height: 1px;">
                    Out of Range</td>
                <td style="width: 487px; height: 1px;">
                    <asp:RadioButton ID="rdbtn_OutRange" runat="server" GroupName="range" TabIndex="6" /></td>
                <td colspan="4" style="height: 1px">
                </td>
            </tr>
            <tr>
                <td style="width: 259px; text-align: right;">
                    Report Format</td>
                <td style="width: 487px">
                    <asp:DropDownList ID="cmbReportFormat" runat="server" Width="120px" TabIndex="7">
                        <asp:ListItem>CSV</asp:ListItem>
                        <asp:ListItem>EXCEL</asp:ListItem>
                        <asp:ListItem Selected="True">HTML</asp:ListItem>
                        <asp:ListItem>PDF</asp:ListItem>
                        <asp:ListItem>XML</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="btnReport" runat="server" Text="View Report"
                        Width="100px" TabIndex="8" /></td>
                <td colspan="4">
                </td>
            </tr>
        </table>
    
    <asp:RangeValidator ID="rngValid_txtMinval" runat="server" ControlToValidate="txtMinval"
        ErrorMessage="* Minimum value must be an integer." 
         MaximumValue="999999999" MinimumValue="-999999999" Type="Integer"></asp:RangeValidator><br />
    <asp:RangeValidator ID="rngValid_txtMaxval" runat="server" ErrorMessage="* Maximum value must be an integer."
        MaximumValue="999999999"
        MinimumValue="-999999999" Type="Integer" ControlToValidate="txtMaxval"></asp:RangeValidator>
    
</asp:Content>

