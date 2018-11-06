<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Vendor_VendorQuery" title="Vendor Query" Codebehind="VendorQuery.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="VendorReports.aspx?valuePath=Reports Home/Vendor">Vendor Report</a> &gt; Vendor Query</h3>
    </div>
    <asp:Table runat=server ID="table1">
        <asp:TableRow>
            <asp:TableCell>
                Search Option:
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="cmbSeachOption" runat="server" AutoPostBack="True">
                    <asp:ListItem Value="True" Selected="True">All</asp:ListItem>
                    <asp:ListItem Value="False">Specific</asp:ListItem>
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow Id="trSearchFor" Visible="false">
            <asp:TableCell>
                Search For:
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txtSearchFor" runat="server" Enabled="False"></asp:TextBox>
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

