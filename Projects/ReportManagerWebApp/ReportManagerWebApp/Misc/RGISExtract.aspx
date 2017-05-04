<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Item_RGISExtract" title="IRMA Report Manager" Codebehind="RGISExtract.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar">
        <h3><a href="..\Default.aspx">Home</a> &gt; <a href="MiscReports.aspx?valuePath=Reports Home/Misc">Miscellaneous</a> &gt; RGIS Extract by Store</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 8.83em; text-align: right; height: 1em;">Store</td>
            <td style="width: 23.43em; text-align: left; height: 1em;">
                <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                    DataValueField="Store_No" Width="335px">
                </asp:DropDownList><asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetAllStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 12.84em; text-align: right; height: 1em;">
                <asp:RangeValidator ID="rngValid_cmbStore" runat="server" ControlToValidate="cmbStore"
                    ErrorMessage="* Store is a required field." 
                    MaximumValue="2147483647" MinimumValue="1" SetFocusOnError="True"
                    Type="Integer"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 8.83em; text-align: right; height: 1em;">Report Format</td>
            <td style="width: 23.43em; text-align: left; height: 1em;">
                <asp:DropDownList ID="cmbReportFormat" runat="server" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" Text="View Report" Width="100px" /></td>
            <td style="width: 12.84em; text-align: right; height: 1em;">
            </td>
        </tr>
    </table>
</asp:Content>

