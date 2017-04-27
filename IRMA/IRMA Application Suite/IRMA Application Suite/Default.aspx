<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="IRMA_Test_Suite._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>IRMA Application Suite</title>
    <script language="javascript" type="text/javascript">
        function InvalidateLinks() {
            pnl =  document.getElementById("pnlLinks");
            if (pnl) {
                pnl.style.visibility = "hidden";
            }
            return true;
        }
        
        function GenerateLinks() {
            var r = document.getElementById("cboRegion");
            var e = document.getElementById("cboEnvironment");
            var v = document.getElementById("cboVersion");
            
            if ((r.selectedIndex < 1) || (e.selectedIndex < 1) || (v.selectedIndex < 1)) {
                alert ("You must select an Environemnt, Region, and Version to generate links.");
                return false;
            } else {
                return true;
            }
            
        }
        
    </script>
</head>
<body bgcolor="#99CC99">
    <center>
    <form id="form1" runat="server">
    <div>
        &nbsp;<asp:Panel ID="Panel1" runat="server" BackColor="Transparent" Height="568px" Width="872px" BorderStyle="None">
            <br /><asp:Image ID="Image2" runat="server" ImageAlign="Middle" ImageUrl="~/Images/IRMA.jpg" />&nbsp;<br />
            <br />
            <!--<div style="width: 660px; font-size: small; background-color:#f5f5f5; border: solid 1 black;"> This webpage has been updated to support new database and deployment environments. <br /> <a href="docs/IRMAsuite.docx">This</a> document contains a short tutorial on how to use the new IRMASuite</div>-->
        <br />
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="X-Large"
            Font-Underline="True" Text="IRMA Application Suite" ForeColor="Black"></asp:Label><br />
        <br />
        <center>
        <table bgcolor="WhiteSmoke" style="width: 660px" cellpadding="0" cellspacing="0">
            <tr>
                <td align="left" style="width: 216px; height: 31px">
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                        Text="Environment :"></asp:Label>&nbsp; &nbsp;<asp:DropDownList ID="cboEnvironment"
                            runat="server" AutoPostBack="True" Width="96px" >
                        </asp:DropDownList></td>
                <td align="left" style="width: 191px; height: 31px">
                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                        Text="Region : "></asp:Label>
                    &nbsp;
                    <asp:DropDownList ID="cboRegion" runat="server" AutoPostBack="True" Width="72px">
                    </asp:DropDownList>
                    &nbsp;
                </td>
                <td align="left" style="width: 191px; height: 31px">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                        Text="Version : "></asp:Label>
                    &nbsp;&nbsp;
                    <asp:DropDownList ID="cboVersion" runat="server" AutoPostBack="True" Width="88px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td align="center" colspan="3" style="height: 30px">
                    <asp:Button Text= "Generate Links" runat="server"  ID="button_generagelinks"/>
                </td>
            </tr>
        </table>
        </center>
            <br />
            <hr style="width: 736px" />
            <asp:Label ID="lblIRMASystemVersion" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="Large"
                Font-Underline="False" ForeColor="Black" Text="IRMA System Version"></asp:Label><br />
        <br />
        <center>
        <asp:Panel ID="pnlLinks" runat="server" Height="1px" Width="660px" BackColor="Transparent">
            <table style="width: 660px" bgcolor="WhiteSmoke" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 1149px; height: 20px">
            <asp:HyperLink ID="lnkClient" runat="server" Font-Bold="True" Font-Names="Arial"
                ForeColor="Blue" Width="304px" Font-Size="11pt" style="text-align: left" BackColor="Transparent">IRMA Client</asp:HyperLink></td>
                    <td style="width: 1260px; height: 20px">
                        <asp:Label ID="lblClientInfo" runat="server" Font-Names="Arial" Text="Last updated" Font-Size="11pt" Width="352px" style="text-align: left"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 1149px; height: 20px">
                        <asp:HyperLink ID="lnkDVO" runat="server" BackColor="Transparent" Font-Bold="True"
                            Font-Names="Arial" Font-Size="11pt" ForeColor="Blue" Style="text-align: left"
                            Width="304px">DVO</asp:HyperLink></td>
                    <td style="width: 1260px; height: 20px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 1149px; height: 19px;">
                        <asp:HyperLink ID="lnkPOET" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="11pt"
                            ForeColor="Blue" Style="text-align: left" Width="304px" Target="_blank">POET</asp:HyperLink></td>
                    <td style="width: 1260px; text-align: right; height: 19px;">
                    </td>
                </tr>  
                <tr>
                    <td style="width: 1149px; height: 21px;">
                        <asp:HyperLink ID="lnkPromoPlanner" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="11pt" ForeColor="Blue" Style="text-align: left" Width="304px" Target="_blank">Promo Planner</asp:HyperLink></td>
                    <td style="width: 1260px; text-align: right; height: 21px;">
                    </td>
                </tr>
                <tr>
                    <td style="width: 1149px">
                        <asp:HyperLink ID="lnkReportManager" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="11pt" ForeColor="Blue" Style="text-align: left" Width="304px" Target="_blank">Report Manager</asp:HyperLink></td>
                    <td style="width: 1260px; text-align: right">
                    </td>
                </tr>
                <tr>
                    <td style="width: 1149px">
                        <asp:HyperLink ID="lnkSLIM" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="11pt"
                            ForeColor="Blue" Style="text-align: left" Width="304px" Target="_blank">SLIM</asp:HyperLink></td>
                    <td style="width: 1260px; text-align: right">
                    </td>
                </tr>
                <tr>
                    <td style="width: 1149px">
                        <asp:HyperLink ID="lnkStoreOps" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="11pt" ForeColor="Blue" Style="text-align: left" Width="304px" Target="_blank">Store Ops</asp:HyperLink></td>
                    <td style="width: 1260px; text-align: right">
                    </td>
                </tr>
                <tr>
                    <td style="width: 1149px">
                        <asp:HyperLink ID="lnkStoreOrderGuide" runat="server" Font-Bold="True" Font-Names="Arial"
                            Font-Size="11pt" ForeColor="Blue" Style="text-align: left" Width="304px" Target="_blank">Store Order Guide</asp:HyperLink></td>
                    <td style="width: 1260px; text-align: right">
                    </td>
                </tr>
                    <tr>
                        <td colspan="2" style="height: 20px">
            <asp:label ID="lblDatabase" runat="server" Font-Bold="True" Font-Names="Arial"
                ForeColor="Blue"  Font-Size="9pt" style="text-align: left" BackColor="Transparent"></asp:label></td>
                </tr>
            </table>
            <br />
            <asp:HyperLink ID="lnkFramework" Font-Size="Small" runat="server" Text="Click here to download the .NET 4.0 Framework" NavigateUrl="http://www.microsoft.com/downloads/en/details.aspx?FamilyID=9cfb2d51-5ff4-4491-b0e5-b386f32c0992&displaylang=en"></asp:HyperLink>
            <br />
            <asp:Label ID="lblServer" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                Text="Server:"></asp:Label><br />
            &nbsp; &nbsp;&nbsp;
        </asp:Panel>
        <asp:panel id="pnlNoConfig"  runat="server" BackColor="Transparent" Width="660px">
        <table style="width: 660px" bgcolor="whitesmoke">
            <tr><td style="width: 606px">No Configuration Found</td></tr>
        </table>
        </asp:panel>
        </center>
            <asp:Label ID="lblClientPath" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="lblAdminPath" runat="server" Visible="False"></asp:Label></asp:Panel>
        &nbsp;<br />
        <br />
        <br />
        <br />
        <asp:Image ID="Image1" runat="server" ImageAlign="Middle" ImageUrl="~/Images/IRMA.jpg" Visible="False" /><br />
        &nbsp;</div>
    </form>
    </center>
</body>
</html>
