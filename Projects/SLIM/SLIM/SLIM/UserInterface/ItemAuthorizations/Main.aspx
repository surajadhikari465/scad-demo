<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false"  Inherits="SLIM.UserInterface_ItemAuthorizations_Main" title="Item Authorizations Main" EnableSessionState="True" EnableViewState="false" EnableEventValidation="false" Codebehind="Main.aspx.vb" %>
<%@ Register Src="~/UserInterface/ItemSearch.ascx" TagName="IA_ItemSearch" TagPrefix="SLIM" %>
    <%@ MasterType VirtualPath="~/UserInterface/MasterPage.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <SLIM:IA_ItemSearch ID="IA_ItemSearchMod" runat="server" />
</asp:Content>

