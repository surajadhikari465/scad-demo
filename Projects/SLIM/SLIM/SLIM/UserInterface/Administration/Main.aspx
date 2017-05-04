<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_Administration_Main" title="User Admin Page" Codebehind="Main.aspx.vb" %>
<%@ MasterType VirtualPath="~/UserInterface/MasterPage.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table cellpadding="3" cellspacing="3" border="0" class="SlimTable">
        <tr>
        <td style="font-family: Tahoma; font-size: 11px; font-weight: bold;">Administration Menu</td>
        </tr>
        <tr>
            <td >
                <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/UserInterface/Administration/UserAccess.aspx"
                    Style="position: static" Font-Names="Tahoma" Font-Size="12px">User Access</asp:LinkButton></td>
        </tr>
         <tr>
            <td>
                <asp:LinkButton ID="LinkButton6" runat="server" PostBackUrl="~/UserInterface/Administration/StoreSubTeam.aspx"
                    Style="position: static" Font-Names="Tahoma" Font-Size="12px">Store/SubTeam</asp:LinkButton></td>
        </tr>
        <tr>
            <td >
                <asp:LinkButton ID="LinkButton2" runat="server" PostBackUrl="~/UserInterface/Administration/EmailMessages.aspx"
                    Style="position: static" Font-Names="Tahoma" Font-Size="12px">Email Settings</asp:LinkButton></td>
        </tr>
        <tr>
            <td >
                <asp:LinkButton ID="LinkButton3" runat="server" Style="position: static" Font-Names="Tahoma" Font-Size="12px">Field Config</asp:LinkButton></td>
        </tr>
        <tr>
            <td >
                <asp:LinkButton ID="LinkButton4" runat="server" PostBackUrl="~/UserInterface/Administration/Other.aspx"
                    Style="position: static" Font-Names="Tahoma" Font-Size="12px">App Settings</asp:LinkButton></td>
        </tr>
        <tr>
            <td >
                <asp:LinkButton ID="LinkButton5" runat="server" PostBackUrl="~/UserInterface/Main.aspx"
                    Style="position: static" Font-Names="Tahoma" Font-Size="12px">Back to Main</asp:LinkButton></td>
        </tr>
    </table>
</asp:Content>

