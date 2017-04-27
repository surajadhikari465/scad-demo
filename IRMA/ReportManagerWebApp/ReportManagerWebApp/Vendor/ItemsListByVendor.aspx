<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Item_ItemsListByVendor" title="IRMA Report Manager" Codebehind="ItemsListByVendor.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="VendorReports.aspx?valuePath=Reports Home/Vendor">Vendor Report</a> &gt; Items List by Vendor</h3>
    </div>
    <table border="0" >
        <tr>
            <td style="width: 150px; height: 2px; text-align: right">
                View By</td>
            <td style="width: 346px; height: 2px; text-align: left">
                <asp:RadioButtonList ID="radSelectBy" runat="server" AutoPostBack="True"
                    Width="224px" RepeatColumns="3">
                    <asp:ListItem>Region</asp:ListItem>
                    <asp:ListItem>Zone</asp:ListItem>
                    <asp:ListItem>Store</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 318px; height: 2px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 150px; height: 17px; text-align: right">
                <asp:Label ID="lblSelectionType" runat="server" Text="View Selection" Width="104px"></asp:Label></td>
            <td style="width: 346px; height: 17px; text-align: left">
                <asp:DropDownList ID="cmbSelection" runat="server" Width="225px">
                </asp:DropDownList></td>
            <td style="width: 318px; height: 17px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbSelection" runat="server" ControlToValidate="cmbSelection"
                    ErrorMessage="* Selection is a required field."  MaximumValue="2147483647" MinimumValue="1" SetFocusOnError="True"
                    Type="Integer" Width="224px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 150px; height: 17px; text-align: right">
                Vendor</td>
            <td style="width: 346px; height: 17px; text-align: left">
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
            <td style="width: 318px; height: 17px; text-align: left">
                </td>
        </tr>
        <tr>
            <td style="width: 150px; height: 17px; text-align: right">
                Brand</td>
            <td style="width: 346px; height: 17px; text-align: left">
                <asp:DropDownList ID="cmbBrand" runat="server" DataSourceID="ICBrands" DataTextField="Brand_Name"
                    DataValueField="Brand_ID" Width="225px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICBrands" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetBrandAndID" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 318px; height: 17px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 150px; height: 17px; text-align: right">
                Identifier</td>
            <td style="font-size: 8pt; width: 346px; height: 17px; text-align: left">
                <asp:TextBox ID="txtIdentifier" runat="server" MaxLength="13" Width="220px"></asp:TextBox>
                (optional)</td>
            <td style="width: 318px; height: 17px; text-align: left">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtIdentifier"
                    ErrorMessage="* Identifier must be numeric."
                    ValidationExpression="^[0-9(%)]*$"></asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td style="width: 150px; height: 10px; text-align: right">
                Team
            </td>
            <td style="width: 346px; height: 10px; text-align: left">
                <asp:DropDownList ID="cmbTeam" runat="server" DataSourceID="ICTeam" DataTextField="Team_Name"
                    DataValueField="Team_No" Width="225px" AutoPostBack="True">
                </asp:DropDownList><asp:SqlDataSource ID="ICTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 318px; height: 10px; text-align: left">
                </td>
        </tr>
        <tr>
            <td style="width: 150px; height: 10px; text-align: right">
                Subteam

            </td>
            <td style="width: 346px; height: 10px; text-align: left">
                <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="ICSubteam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" Width="225px" AutoPostBack="True">
                </asp:DropDownList><asp:SqlDataSource ID="ICSubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT ST.SubTeam_No, ST.SubTeam_Name FROM SubTeam ST (NOLOCK) WHERE ST.Team_No = ISNULL(@Team_No, ST.Team_No)">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="cmbTeam" DefaultValue="DBNull" Name="Team_No" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 318px; height: 10px; text-align: left">
                </td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right; height: 10px;">
                Category</td>
            <td style="width: 346px; text-align: left; height: 10px;">
                <asp:DropDownList ID="cmbCategory" runat="server" DataSourceID="ICCategory" DataTextField="Category_Name"
                    DataValueField="Category_ID" Width="225px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICCategory" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT C.Category_ID, C.Category_Name FROM ItemCategory C (NOLOCK) WHERE C.SubTeam_No = ISNULL(@SubTeam_No, C.SubTeam_No) AND @Team_No > 0">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="cmbSubTeam" DefaultValue="" Name="SubTeam_No" PropertyName="SelectedValue" Type="Int32" />
                        <asp:ControlParameter ControlID="cmbTeam" Name="Team_No" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 318px; text-align: left; height: 10px;">
            </td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right">
                Report Format</td>
             <td style="width: 346px; text-align: left"><asp:DropDownList ID="cmbReportFormat" runat="server" Font-Names="Verdana" Font-Size="10pt" Width="120px">
                <asp:ListItem>CSV</asp:ListItem>
                <asp:ListItem>EXCEL</asp:ListItem>
                <asp:ListItem Selected="True">HTML</asp:ListItem>
                <asp:ListItem>PDF</asp:ListItem>
                <asp:ListItem>XML</asp:ListItem>
            </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" Text="View Report"
                    Width="96px" /></td>
            <td style="width: 318px; text-align: left">
                <asp:SqlDataSource ID="ICRegions" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="GetRegions" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="ICZones" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="GetZones" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="GetAllStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
        </tr>
    </table>
</asp:Content>

