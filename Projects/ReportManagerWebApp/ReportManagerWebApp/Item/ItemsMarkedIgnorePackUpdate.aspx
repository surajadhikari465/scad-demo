<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Item_ItemsMarkedIgnorePackUpdate" title="IRMA Report Manager" Codebehind="ItemsMarkedIgnorePackUpdate.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="ItemReports.aspx?valuePath=Reports Home/Item">
            Item Reports</a> &gt; Items Marked Ignore Pack Update</h3>
    </div>

    <table id="tblParameters" border="0">
        <tr>
            <td style="text-align: right" title="IRMA Report Manager">
                Subteam</td>
            <td style="width: 105px; text-align: left">
                <asp:DropDownList ID="ddlSubteam" runat="server" DataSourceID="Subteams" 
                    DataTextField="SubTeam_Name" DataValueField="SubTeam_No">
                </asp:DropDownList>
                <asp:SqlDataSource ID="Subteams" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>" SelectCommand="select '&lt;All&gt;' as SubTeam_Name, null as SubTeam_No
UNION
select SubTeam_Name, SubTeam_No from SubTeam"></asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td style="text-align: right" title="IRMA Report Manager">
                Vendor</td>
            <td style="width: 105px; text-align: left">
                <asp:DropDownList ID="ddlVendor" runat="server" DataSourceID="GetVendors" 
                    DataTextField="CompanyName" DataValueField="Vendor_ID">
                </asp:DropDownList>
                <asp:SqlDataSource ID="GetVendors" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>" SelectCommand="select '&lt;All&gt;' as CompanyName, null as Vendor_ID
UNION
select CompanyName, Vendor_ID from Vendor"></asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td style="text-align: right" title="IRMA Report Manager">
                Vendor Item Status</td>
            <td style="width: 105px; text-align: left">
                <asp:CheckBoxList ID="cblVendorItemStatus" runat="server" 
                    DataSourceID="GetVendorItemStatuses" DataTextField="StatusName" 
                    DataValueField="StatusID">
                </asp:CheckBoxList>
                <asp:SqlDataSource ID="GetVendorItemStatuses" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>" SelectCommand="    SELECT StatusID, StatusName
    FROM VendorItemStatuses"></asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td style="text-align: right" title="IRMA Report Manager">
                Default Identifiers Only</td>
            <td style="width: 105px; text-align: left">
                <asp:CheckBox ID="chkDefaultIdentifier" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="text-align: center" title="IRMA Report Manager">
                &nbsp;</td>
            <td style="width: 105px; text-align: left">
                <asp:Button ID="btnReport" runat="server" Text="View Report" Width="96px" /></td>
        </tr>
    </table>
</asp:Content>

