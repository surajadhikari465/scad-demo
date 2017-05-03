<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" 
    Inherits="ReportManagerWebApp.Price_MarginAudit" title="Gross Margin Audit" Codebehind="MarginAudit.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar">
    <h3>
        <a href="..\Default.aspx">Home</a> &gt; 
        <a href="PriceReports.aspx?valuePath=Reports Home/Prices">Price Reports</a> &gt; 
        Gross Margin Audit
    </h3>
    </div>
    &nbsp;<table style="width: 836px">
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
                GM Less Than</td>
            <td style="width: 326px" valign="top">
                <asp:TextBox ID="txtMinGM" runat="server" TabIndex="3" Width="88px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqfld_txtMinval" runat="server" ControlToValidate="txtMinGM"
                    ErrorMessage="This is a required field" Width="13px" Display="Dynamic">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator
                        ID="revPosNegTxtMinGM" runat="server" ControlToValidate="txtMinGM" Display="Dynamic"
                        ErrorMessage="Must be a positive or negative number" ValidationExpression="^(-)?\d{1,4}$">*</asp:RegularExpressionValidator></td>
            <td colspan="4" style="width: 306px" rowspan="4" valign="top">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" />
            </td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
                GM Greater Than</td>
            <td style="width: 326px" valign="top">
                <asp:TextBox ID="txtMaxGM" runat="server" TabIndex="4" Width="88px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqfld_txtMaxval" runat="server" ControlToValidate="txtMaxGM"
                    ErrorMessage="This is a required field" Width="6px" Display="Dynamic">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator
                        ID="revPosNegTxtMaxGM" runat="server" ControlToValidate="txtMaxGM" Display="Dynamic"
                        ErrorMessage="Must be a positive or negative number" ValidationExpression="^(-)?\d{1,4}$">*</asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
            </td>
            <td style="width: 326px" valign="top">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
                Zone</td>
            <td style="width: 326px" valign="top">
                <asp:DropDownList ID="cmbZone" runat="server" Width="320px" DataSourceID="ICZones" DataTextField="Zone_Name" DataValueField="Zone_id">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICZones" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetZones" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
            </td>
            <td style="width: 326px" valign="top">
                &nbsp;</td>
            <td colspan="4" style="width: 306px" valign="top">
            </td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
                Store #1</td>
            <td style="width: 326px" valign="top">
                <asp:DropDownList ID="cmbStore1" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                    DataValueField="Store_No" Width="320px">
                </asp:DropDownList></td>
            <td colspan="4" style="width: 306px" valign="top">
            </td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
                Store #2</td>
            <td style="width: 326px" valign="top">
                <asp:DropDownList ID="cmbStore2" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                    DataValueField="Store_No" Width="320px">
                </asp:DropDownList></td>
            <td colspan="4" style="width: 306px" valign="top">
            </td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
                Store #3</td>
            <td style="width: 326px" valign="top">
                <asp:DropDownList ID="cmbStore3" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                    DataValueField="Store_No" Width="320px">
                </asp:DropDownList></td>
            <td colspan="4" style="width: 306px" valign="top">
            </td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
                Store #4</td>
            <td style="width: 326px" valign="top">
                <asp:DropDownList ID="cmbStore4" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                    DataValueField="Store_No" Width="320px">
                </asp:DropDownList></td>
            <td colspan="4" style="width: 306px" valign="top">
            </td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
                Store #5</td>
            <td style="width: 326px" valign="top">
                <asp:DropDownList ID="cmbStore5" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                    DataValueField="Store_No" Width="320px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td colspan="4" style="width: 306px" valign="top">
            </td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
            </td>
            <td style="width: 326px" valign="top">
                &nbsp;
            </td>
            <td colspan="4" style="width: 306px" valign="top">
            </td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
                Subteam #1</td>
            <td style="width: 326px" valign="top">
                <asp:DropDownList ID="cmbSubTeam1" runat="server" DataSourceID="ICSubteams" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" TabIndex="2" Width="320px">
                </asp:DropDownList></td>
            <td colspan="4" style="width: 306px" valign="top">
            </td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
                Subteam #2</td>
            <td style="width: 326px" valign="top">
                <asp:DropDownList ID="cmbSubteam2" runat="server" DataSourceID="ICSubteams" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" TabIndex="2" Width="320px">
                </asp:DropDownList></td>
            <td colspan="4" style="width: 306px" valign="top">
            </td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
                Subteam #3</td>
            <td style="width: 326px" valign="top">
                <asp:DropDownList ID="cmbSubteam3" runat="server" DataSourceID="ICSubteams" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" TabIndex="2" Width="320px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICSubteams" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetAllSubTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td colspan="4" style="width: 306px" valign="top">
            </td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
            </td>
            <td style="width: 326px" valign="top">
                &nbsp;
            </td>
            <td colspan="4" style="width: 306px" valign="top">
            </td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
                Category</td>
            <td style="width: 326px" valign="top">
                <asp:DropDownList ID="cmbCategory" runat="server" DataSourceID="ICCategory" DataTextField="Category_Name"
                    DataValueField="Category_ID" TabIndex="2" Width="320px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICCategory" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetCategoryAndID" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td colspan="4" style="width: 306px" valign="top">
            </td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
                Vendor</td>
            <td style="width: 326px" valign="top">
                <asp:DropDownList ID="cmbVendor" runat="server" DataSourceID="ICVendors" DataTextField="CompanyName"
                    DataValueField="Vendor_ID" TabIndex="2" Width="320px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICVendors" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetVendors" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td colspan="4" style="width: 306px" valign="top">
            </td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
                </td>
            <td style="width: 326px" valign="top">
                </td>
            <td colspan="4" style="width: 306px" valign="top">
                </td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
                </td>
            <td style="width: 326px" valign="top">
                </td>
            <td colspan="4" style="width: 306px" valign="top">
                </td>
        </tr>
        <tr>
            <td style="width: 139px; text-align: right" valign="top">
                Report Format</td>
            <td style="width: 326px" valign="top">
                <asp:DropDownList ID="cmbReportFormat" runat="server" TabIndex="7" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" TabIndex="8" Text="View Report" Width="100px" /></td>
            <td colspan="4" style="width: 306px" valign="top">
            </td>
        </tr>
    </table>
</asp:Content>

