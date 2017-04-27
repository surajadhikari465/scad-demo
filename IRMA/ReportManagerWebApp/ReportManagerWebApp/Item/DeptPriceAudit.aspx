<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Price_DeptPriceAudit" title="Untitled Page" Codebehind="DeptPriceAudit.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="ItemReports.aspx?valuePath=Reports Home/Items">Item Reports</a> &gt; Department Price Audit</h3>
    </div>
    <asp:Table runat="server" ID="table1">
        <asp:TableRow ID="trStores">
            <asp:TableCell>
                Stores:
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="cmbStores" runat="server" DataSourceID="SqlDataSource1" DataTextField="Store_Name"
                    DataValueField="Store_No">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="trSearchFor" Visible="false">
            <asp:TableCell>
                Search For:
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txtSearchFor" runat="server"></asp:TextBox>
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

