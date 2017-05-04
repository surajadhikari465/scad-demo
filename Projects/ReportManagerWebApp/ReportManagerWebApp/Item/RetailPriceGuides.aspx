<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Item_RetailPriceGuides" title="Untitled Page" Codebehind="RetailPriceGuides.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar">
        <h3>
            <a href="..\Default.aspx">Home</a> &gt; <a href="ItemReports.aspx?valuePath=Reports Home/Item">
                Item Reports</a> &gt; Retail Price Guides</h3>
    </div>
    <table>
        <tr>
            <td style="width: 154px; height: 21px; text-align: right">
                Store</td>
            <td style="height: 21px; text-align: left">
                <asp:DropDownList ID="cmbStore" runat="server" AutoPostBack="True" DataSourceID="ICStores"
                    DataTextField="Store_Name" DataValueField="Store_No" TabIndex="5" Width="250px">
                </asp:DropDownList><asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 3px; height: 21px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 154px; text-align: right">
                SubTeam</td>
            <td style="text-align: left">
                <asp:DropDownList ID="cmbSubTeam" runat="server" AutoPostBack="True" DataSourceID="ICSubteams"
                    DataTextField="SubTeam_Name" DataValueField="SubTeam_No" TabIndex="5" Width="250px" Enabled="False">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICSubTeams" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetAllSubTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 3px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 154px; text-align: right">
            </td>
            <td style="text-align: left">
                &nbsp;<asp:Button ID="btnReport" runat="server" TabIndex="9" Text="View Report" Width="100px" /></td>
            <td style="width: 3px; text-align: left">
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

