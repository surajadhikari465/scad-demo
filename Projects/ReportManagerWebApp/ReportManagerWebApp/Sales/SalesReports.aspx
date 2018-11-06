<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Sales_SalesReports" title="Report Manager - Sales Reports" Codebehind="SalesReports.aspx.vb" %>
<%@ Register Src="../Controls/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; Sales Reports</h3>
    </div>
        <uc1:Menu ID="Menu1" runat="server" />
</asp:Content>

