<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" ClassName="WQ_Master" AutoEventWireup="false" Inherits="SLIM.UserInterface_WebQuery_Main" title="Web Query Main" EnableEventValidation="false" Codebehind="Main.aspx.vb" %>
<%@ Register Src="~/UserInterface/ItemSearch.ascx" TagName="WQ_ItemSearch" TagPrefix="SLIM" %>
    <%@ MasterType VirtualPath="~/UserInterface/MasterPage.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <SLIM:WQ_ItemSearch ID="WQ_ItemSearchMod" runat="server" />
</asp:Content>

