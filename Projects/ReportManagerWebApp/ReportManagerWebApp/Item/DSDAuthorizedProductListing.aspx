<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Item_DSD_Authorized_Product_Listing" title="DSD Authorized Product Listing" Codebehind="DSDAuthorizedProductListing.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="navbar">
        <h3>
            <a href="..\Default.aspx">Home</a> &gt; <a href="ItemReports.aspx?valuePath=Reports Home/Items">
                Item Reports</a> &gt; DSD Authorized Product Listing</h3>
    </div>
    <asp:Table runat="server" ID="table1">
        <asp:TableRow>
            <asp:TableCell>
                Vendor:
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="cmbVendor" runat="server" DataSourceID="ICVendor" DataTextField="CompanyName"
                    DataValueField="Vendor_ID">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICVendor" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetVendors" SelectCommandType="StoredProcedure">
                </asp:SqlDataSource>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                Report Format:
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="cmbReportFormat" runat="server" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                &nbsp;
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button ID="btnReport" runat="server" Text="View Report" />
            </asp:TableCell>
        </asp:TableRow>
        </asp:Table>
</asp:Content>

