<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp._Default" title="IRMA Report Manager" Codebehind="Default.aspx.vb" %>
<%@ Register Src="Controls/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div id="navbar"><h3>Reporting Categories</h3></div>
            <uc1:Menu ID="Menu1" runat="server" />
</asp:Content>

