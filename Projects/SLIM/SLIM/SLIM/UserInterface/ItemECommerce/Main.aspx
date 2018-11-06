<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false"  Inherits="SLIM.UserInterface_ItemEcommerce_Main" title="Item ECommerce Main" EnableSessionState="True" EnableViewState="false" EnableEventValidation="false" Codebehind="Main.aspx.vb" %>
<%@ Register Src="~/UserInterface/ItemSearch.ascx" TagName="IEComm_ItemSearch" TagPrefix="SLIM" %>
    <%@ MasterType VirtualPath="~/UserInterface/MasterPage.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <SLIM:IEComm_ItemSearch ID="IE_ItemSearchMod" runat="server" />
</asp:Content>

