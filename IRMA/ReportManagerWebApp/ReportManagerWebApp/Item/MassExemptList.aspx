<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Item_MassExemptList" title="IRMA Report Manager" Codebehind="MassExemptList.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar">
        <h3><a href="..\Default.aspx">Home</a> &gt; <a href="ItemReports.aspx?valuePath=Reports Home/Items">Item Reports</a> &gt; Massachusetts Exempt List</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 150px; text-align: right">
                Store</td>
            <td style="width: 249px; text-align: left">
                <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                    DataValueField="Store_No" Font-Names="Verdana" Font-Size="10pt"
                    Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetAllStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 318px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbStore" runat="server" ControlToValidate="cmbStore"
                    ErrorMessage="* Store is a required field." Height="21px" MaximumValue="2147483647"
                    MinimumValue="1" SetFocusOnError="True" Type="Integer" Width="270px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right">
                Report Format</td>
            <td style="width: 249px; text-align: left">
                <asp:DropDownList ID="cmbReportFormat" runat="server" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" Text="View Report" Width="96px" /></td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
    </table>
</asp:Content>

