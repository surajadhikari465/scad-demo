<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_ItemVendor_Main" title="Edit Item's Vendor Main" EnableSessionState="True" EnableEventValidation="false" Codebehind="Main.aspx.vb" %>
<%@ Register Src="~/UserInterface/ItemSearch.ascx" TagName="IV_ItemSearch" TagPrefix="SLIM" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <SLIM:IV_ItemSearch ID="IV_ItemSearchMod" runat="server" FilterByStore="true" />
</asp:Content>

