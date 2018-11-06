<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Item_ItemList" title="IRMA Report Manager" Codebehind="ItemList.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar">
        <h3><a href="..\Default.aspx">Home</a> &gt; <a href="ItemReports.aspx?valuePath=Reports Home/Items">Item Reports</a> &gt; Item List</h3>
    </div>
    <table id="tblParameters" border="0">
        <tr>
            <td style="width: 218px; text-align: right">Store</td>
            <td style="width: 249px; text-align: left">
                <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                    DataValueField="Store_No" Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetAllStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 300px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbStore" runat="server" ControlToValidate="cmbStore"
                    ErrorMessage="* Store is a required field." 
                    MaximumValue="2147483647" MinimumValue="1" SetFocusOnError="True"
                    Type="Integer" Width="220px"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 218px; text-align: right">Vendor Match</td>
            <td style="width: 346px; text-align: left">
                <asp:RadioButtonList ID="radSelectBy" runat="server" AutoPostBack="True" Height="24px"
                    RepeatColumns="3" Width="224px">
                    <asp:ListItem>Partial</asp:ListItem>
                    <asp:ListItem>Exact</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 300px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 218px; text-align: right">
                <span>
                    <asp:Label ID="lblSelectionType" runat="server" Text="Vendor Selection" Width="128px"></asp:Label></span></td>
            <td style="width: 346px; text-align: left">
                <asp:TextBox ID="txtVendor" runat="server" MaxLength="50" Width="220px"></asp:TextBox><asp:DropDownList
                    ID="cmbVendor" runat="server" DataSourceID="ICVendors" DataTextField="CompanyName"
                    DataValueField="Vendor_ID" Width="225px">
                </asp:DropDownList>
                (optional)<asp:SqlDataSource ID="ICVendors" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetVendors" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 300px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 218px; text-align: right">
                <span>Vendor Item ID</span></td>
            <td style="width: 346px; text-align: left">
                <asp:TextBox ID="txtVendorItemID" runat="server" MaxLength="20" Width="220px"></asp:TextBox>
                (optional)</td>
            <td style="width: 300px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 218px; text-align: right">
                <span>Item Description</span></td>
            <td style="width: 346px; text-align: left">
                <asp:TextBox ID="txtItemDescription" runat="server" MaxLength="60" Width="220px"></asp:TextBox>
                (optional)</td>
            <td style="width: 300px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 218px; text-align: right">
                <span>Identifier</span></td>
            <td style="width: 346px; text-align: left">
                <asp:TextBox ID="txtIdentifier" runat="server" MaxLength="13" Width="220px"></asp:TextBox>
                (optional)</td>
            <td style="width: 300px; text-align: left">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtIdentifier"
                    ErrorMessage="* Identifier must be numeric." ValidationExpression="^[0-9(%)]*$">
                </asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td style="width: 218px; text-align: right">
            </td>
            <td style="width: 346px; text-align: left">
            </td>
            <td style="width: 300px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 218px; text-align: right">
               Team
            </td>
            <td style="width: 346px; text-align: left">
                <asp:DropDownList ID="cmbTeam" runat="server" AutoPostBack="True" DataSourceID="ICTeam"
                    DataTextField="Team_Name" DataValueField="Team_No" Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="ICTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 300px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 218px; text-align: right">
                Subteam
            </td>
            <td style="width: 346px; text-align: left">
                <asp:DropDownList ID="cmbSubTeam" runat="server" AutoPostBack="True" DataSourceID="ICSubteam"
                    DataTextField="SubTeam_Name" DataValueField="SubTeam_No" Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="ICSubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT ST.SubTeam_No, ST.SubTeam_Name FROM SubTeam ST (NOLOCK) WHERE ST.Team_No = ISNULL(@Team_No, ST.Team_No)">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="cmbTeam" DefaultValue="DBNull" Name="Team_No" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 300px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 218px; text-align: right">
                Include Discontinued Items?</td>
            <td style="width: 346px; text-align: left">
                <asp:CheckBox ID="chkIncludeDiscontinued" runat="server" /></td>
            <td style="width: 300px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 218px; text-align: right">
                WFM Items Only?</td>
            <td style="width: 346px; text-align: left">
                <asp:CheckBox ID="chkWFMOnly" runat="server" /></td>
            <td style="width: 300px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 218px; text-align: right">
                National Identifiers Only?</td>
            <td style="width: 346px; text-align: left">
                <asp:CheckBox ID="chkNatItems" runat="server" /></td>
            <td style="width: 300px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 218px; text-align: right">
                <span>Report Format</span></td>
            <td style="width: 346px; text-align: left">
                <asp:DropDownList ID="cmbReportFormat" runat="server" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" Text="View Report" Width="96px" /></td>
            <td style="width: 300px; text-align: left">
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

