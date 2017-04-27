<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Vendor_VendorListing" title="Vendor Listing" Codebehind="VendorListing.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="VendorReports.aspx?valuePath=Reports Home/Vendor">Vendor Report</a> &gt; Vendor Listing</h3>
    </div>
    <asp:Table runat="server" ID="table1">
        <asp:TableRow>
            <asp:TableCell>
                Vendor:
            </asp:TableCell>
            <asp:TableCell>
                <asp:ListBox SelectionMode="multiple" ID="cmbVendors" runat="server" DataSourceID="ICVendors" DataTextField="CompanyName"
                    DataValueField="Vendor_ID"></asp:ListBox>
                <asp:SqlDataSource ID="ICVendors" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="Reporting_GetVendors" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Name="blnAll" DefaultValue="True" Type="boolean" />
                    </SelectParameters>
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

