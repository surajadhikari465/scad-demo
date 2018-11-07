<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.PriceChangeDetail" title="IRMA Report Manager" Codebehind="PriceChangeDetail.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar">
        <a href="..\Default.aspx"></a> 
        <h3>
            <a href="..\Default.aspx">Home</a> &gt; <a href="PriceReports.aspx?valuePath=Reports Home/Prices">Price Reports</a>
        &gt; Price Change History Detail</h3>
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
                &nbsp;<asp:TextBox ID="txtSearch" runat="server" MaxLength="60" TabIndex="2" Width="175px"></asp:TextBox></td>
            <td style="width: 401px; height: 20px">
                <asp:Button ID="btnGetVendors" runat="server" TabIndex="3" Text="Search" Width="100px" /></td>
        </tr>
        <tr>
            <td style="width: 28px; height: 86px">
            </td>
            <td colspan="2" style="height: 86px">
                &nbsp;<asp:RequiredFieldValidator ID="reqfld_lstVendors" runat="server" ControlToValidate="lstVendors"
                    ErrorMessage="* A vendor must be selected." ValidationGroup="submit" Width="300px" Display="dynamic"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvSearch" runat="server" Enabled="false" EnableClientScript="False" ControlToValidate="txtSearch" Type="integer" Operator="dataTypeCheck" ErrorMessage="* Must be an integer" />
                <br />
                <asp:ListBox ID="lstVendors" runat="server" Height="149px" TabIndex="4" Width="393px">
                </asp:ListBox><br />
                <asp:Label ID="lblVendorCount" runat="server" ForeColor="Red"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 28px; height: 30px">
            </td>
            <td colspan="2" style="height: 30px">
                <strong>Sub-Team:</strong></td>
        </tr>
        <tr>
            <td style="width: 28px">
            </td>
            <td colspan="2">
                <asp:DropDownList ID="cmbSubteam" runat="server" AutoPostBack="True" DataSourceID="ICSubteam"
                    DataTextField="SubTeam_Name" DataValueField="SubTeam_No" TabIndex="2" Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="ICSubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetAllSubTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 28px; height: 30px">
            </td>
            <td colspan="2" style="height: 30px">
                <strong>Class:</strong></td>
        </tr>
        <tr>
            <td style="width: 28px">
            </td>
            <td colspan="2">
                <asp:DropDownList ID="cmbClass" runat="server" DataSourceID="ICCategoriesBySubteam" DataTextField="Category_Name"
                    DataValueField="Category_ID" TabIndex="3" Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="ICCategoriesBySubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT Category_ID, Category_Name&#13;&#10;FROM  ItemCategory (nolock) &#13;&#10;WHERE SubTeam_No = @SubTeam_No&#13;&#10;ORDER BY Category_Name">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="cmbSubTeam" DefaultValue="0" Name="SubTeam_No" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                &nbsp;
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

