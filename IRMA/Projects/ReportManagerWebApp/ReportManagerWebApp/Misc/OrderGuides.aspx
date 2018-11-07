<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.OrderGuides" title="Report Manager - Order Guides" Codebehind="OrderGuides.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar">
        <h3><a href="..\Default.aspx">Home</a> &gt; <a href="MiscReports.aspx?valuePath=Reports Home/Misc">Miscellaneous</a> &gt; Product Order Guides</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 150px; text-align: right;">
                Store</td>
            <td style="width: 249px; text-align: left;"><asp:DropDownList ID="cmbStore" runat="server" Width="225px" DataSourceID="ICStores" DataTextField="Store_Name" DataValueField="Store_No" Font-Names="Verdana" Font-Size="10pt">
                </asp:DropDownList><asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="GetAllStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource></td>
            <td style="width: 318px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbStore" runat="server" ControlToValidate="cmbStore"
                    ErrorMessage="* Store is a required field." MaximumValue="2147483647" 
                    MinimumValue="1" SetFocusOnError="True"
                    Type="Integer" Width="255px"></asp:RangeValidator></td>
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
                    Type="Integer" Width="259px"></asp:RangeValidator></td>
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
                </td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right">Vendor</td>
            <td style="width: 346px; text-align: left">
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
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right;">
                Order Type</td>
            <td style="width: 249px; text-align: left;">
                <asp:RadioButton ID="optPerishable" runat="server" GroupName="OrderType" Text="Perishable"
                    Width="88px" />
                <asp:RadioButton ID="optNonPerishable" runat="server" Checked="True" GroupName="OrderType"
                    Text="Non-Perishable" Width="128px" /></td>
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

