<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.CostEventDetail" title="IRMA Report Manager" Codebehind="CostEventDetail.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar">
        <a href="..\Default.aspx"></a> 
        <h3>
            <a href="..\Default.aspx">Home</a> &gt; <a href="PriceReports.aspx?valuePath=Reports Home/Prices">Price Reports</a>&gt; Cost Event Detail</h3>
    </div>
    <table>
        <tr>
            <td style="width: 28px; height: 30px">
            </td>
            <td colspan="2" style="height: 30px">
                <strong>
    Search by:</strong></td>
        </tr>
        <tr>
            <td style="width: 28px; height: 30px">
            </td>
            <td colspan="2" style="height: 30px">
                <asp:RadioButtonList ID="rdbtn_SearchType" runat="server" TabIndex="1" Width="250px">
                    <asp:ListItem Selected="True" Value="1">Company Name</asp:ListItem>
                    <asp:ListItem Value="2">PS Vendor ID</asp:ListItem>
                    <asp:ListItem Value="3">Vendor ID</asp:ListItem>
                </asp:RadioButtonList></td>
        </tr>
        <tr>
            <td style="width: 28px; height: 30px">
            </td>
            <td colspan="2" style="height: 30px">
                <strong>
    Vendor Selection, please enter a search string:&nbsp; </strong>
            </td>
        </tr>
        <tr>
            <td style="width: 28px; height: 20px">
            </td>
            <td style="width: 200px; height: 20px">
                <asp:TextBox ID="txtSearch" runat="server" MaxLength="60" TabIndex="2" Width="175px"></asp:TextBox></td>
            <td style="width: 401px; height: 20px">
                <asp:Button ID="btnGetVendors" runat="server" TabIndex="3" Text="Search" Width="100px" /></td>
        </tr>
        <tr>
            <td style="width: 28px; height: 86px">
            </td>
            <td colspan="2" style="height: 86px">
                <asp:RequiredFieldValidator ID="reqfld_lstVendors" runat="server" ControlToValidate="lstVendors"
                    ErrorMessage="* A vendor must be selected." ValidationGroup="submit" Width="300px"></asp:RequiredFieldValidator><br />
                <asp:ListBox ID="lstVendors" runat="server" Height="149px" TabIndex="4" Width="393px">
                </asp:ListBox><br />
                <asp:Label ID="lblVendorCount" runat="server" ForeColor="Red"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 28px; height: 30px">
            </td>
            <td colspan="2" style="height: 30px">
                <strong>Team:</strong></td>
        </tr>
        <tr>
            <td style="width: 28px">
            </td>
            <td colspan="2">
                <asp:DropDownList ID="cmbTeam" runat="server" AutoPostBack="True" DataSourceID="ICTeam"
                    DataTextField="Team_Name" DataValueField="Team_No" TabIndex="2" Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="ICTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td style="width: 28px; height: 30px">
            </td>
            <td colspan="2" style="height: 30px">
                <strong>Subteam:</strong></td>
        </tr>
        <tr>
            <td style="width: 28px">
            </td>
            <td colspan="2">
                <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="ICSubteam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" TabIndex="3" Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="ICSubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetSubTeams" SelectCommandType="StoredProcedure">
                </asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td style="width: 28px; height: 30px">
            </td>
            <td colspan="2" style="height: 30px">
                <strong>
                Report Format:</strong></td>
        </tr>
        <tr>
            <td style="width: 28px">
            </td>
            <td colspan="2">
                <asp:DropDownList ID="cmbReportFormat" runat="server" TabIndex="10" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" TabIndex="11" Text="View Report" ValidationGroup="submit"
                    Width="100px" /></td>
        </tr>
        <tr>
            <td style="width: 28px">
            </td>
            <td colspan="2">
            </td>
        </tr>
    </table>
    <asp:SqlDataSource ID="ICVendors_CompanyName" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
        SelectCommand="GetVendor_ByCompanyName" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtSearch" Name="Find" PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="ICVendors_PSVendorID" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
        SelectCommand="GetVendor_ByPSVendorID" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtSearch" Name="Find" PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="ICVendors_VendorID" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
        SelectCommand="GetVendor_ByVendorID" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtSearch" Name="Find" PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

