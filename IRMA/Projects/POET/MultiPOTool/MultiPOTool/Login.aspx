<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MainMaster.Master" CodeBehind="Login.aspx.vb" Inherits="MultiPOTool.Login" 
    title="Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%" >
    <tr>
        <td style="padding-left:100px;">
          <div class="announcementsFrame">
        <div class="main-announcements">
            <asp:Login ID="Login1" runat="server" BackColor="#F7F6F3" BorderColor="#E6E2D8" BorderPadding="0"
        BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="1.0em"
        ForeColor="#333333" Style="position: static" Width="50px" DestinationPageUrl="~/Upload.aspx">
        <TextBoxStyle Font-Size="1.0em" Font-Bold="true" />
        <LoginButtonStyle  CssClass="button" />
        <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
        <TitleTextStyle BackColor="#cde199" Font-Bold="True" Font-Size="1.0em" ForeColor="black" />
    </asp:Login>
            <br />
            <asp:Label ID="SystemMessage" runat="server" Font-Bold="True" Font-Size="Small" 
                ForeColor="#CC0000"></asp:Label>
    </div>
    </div>
        </td>
    </tr>
</table>
    
</asp:Content>
