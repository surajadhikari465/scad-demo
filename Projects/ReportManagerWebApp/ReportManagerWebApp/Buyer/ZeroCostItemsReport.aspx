<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Price_ZeroCostItemsReport" title="Untitled Page" Codebehind="ZeroCostItemsReport.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="BuyerReports.aspx?valuePath=Reports Home/Buyer">Buyer Reports</a> &gt; Zero Cost Items Report</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 160px; text-align: right">
                Store:</td>
            <td style="width: 329px;">
                <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                    DataValueField="Store_No"
                    Width="265px" TabIndex="1">
                </asp:DropDownList>
                  <asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetAllStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 343px;">
                <asp:RequiredFieldValidator ID="reqfld_cmbStore" runat="server" ControlToValidate="cmbStore"
                    ErrorMessage="* Store is a required field." Height="18px"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 160px; text-align: right">
                Subteam:</td>
            <td style="width: 329px; text-align: left">
                <br />
                <asp:DropDownList ID="cmbSubteam" runat="server" DataSourceID="ICSubteam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" TabIndex="2"
                    Width="265px" AutoPostBack="True">
                </asp:DropDownList>
            <asp:SqlDataSource ID="ICSubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT ST.SubTeam_No, ST.SubTeam_Name FROM SubTeam ST (NOLOCK)">
            </asp:SqlDataSource>
            </td>
            <td style="width: 343px; text-align: left">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 160px; text-align: right;">
                Report Format</td>
            <td style="width: 329px; text-align: left;">
                <br />
                <asp:DropDownList ID="cmbReportFormat" runat="server" TabIndex="3" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList><asp:Button ID="btnReport" runat="server" TabIndex="4"
                    Text="View Report" Width="100px" /></td>
            <td style="width: 343px; text-align: left;">
            </td>
        </tr>
    </table>
</asp:Content>

