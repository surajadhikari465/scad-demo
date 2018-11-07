<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false"  Inherits="SLIM.UserInterface_RefreshPOSPage_Main" title="Refresh POS Main" EnableSessionState="True" EnableViewState="false" EnableEventValidation="false" Codebehind="Main.aspx.vb" %>
<%@ Register Src="~/UserInterface/ItemSearch.ascx" TagName="RP_ItemSearch" TagPrefix="SLIM" %>
    <%@ MasterType VirtualPath="~/UserInterface/MasterPage.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <SLIM:RP_ItemSearch ID="RP_ItemSearchMod" runat="server" />
</asp:Content>