<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_InStoreSpecials_Main" title="Store Specials Main" EnableEventValidation="false" Codebehind="Main.aspx.vb" %>
<%@ Register Src="~/UserInterface/ItemSearch.ascx" TagName="ISS_ItemSearch" TagPrefix="SLIM" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Literal ID="Literal_Message" runat="server" Text=""></asp:Literal>
    <SLIM:ISS_ItemSearch ID="ISS_ItemSearchMod" runat="server" FilterByStore="true" />
</asp:Content>

