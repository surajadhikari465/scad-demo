<%@ Page Language="VB" AutoEventWireup="false" Inherits="SLIM._Default" EnableSessionState="True" Codebehind="Default.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SLIM Login Screen</title>
</head>
<!-- body bgcolor="FFF0CB"-->
 <body style="background-color: #eeeeee;">
    <form id="form1" runat="server">
    <div align="center">
        
        <asp:Login ID="Login1" runat="server" BackColor="Green" BorderColor="#E6E2D8"
            BorderPadding="4" BorderStyle="Double" BorderWidth="1px" Font-Names="Tahoma"
            Font-Size="0.8em" ForeColor="#333333" Height="200px"  TextLayout="TextOnTop" Width="200px" DisplayRememberMe="False" TitleText="SLIM Log In">
            <TitleTextStyle BackColor="DarkGreen" Font-Bold="True" Font-Size="Medium" ForeColor="White" />
            <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
            <LabelStyle BackColor="Green" Font-Bold="True" Font-Overline="False" Font-Underline="False"
                ForeColor="White" />
                <LoginButtonStyle Font-Names="Tahoma"  BorderStyle="Outset" BorderWidth="2px" BackColor="White" BorderColor="#C5BBAF" Font-Bold="True" Font-Size="Small" ForeColor="#000040"/>
                <TextBoxStyle Font-Names="Tahoma" Width="200px" Font-Size="0.8em" />
            <FailureTextStyle BackColor="White" Font-Size="Medium" />
            <LayoutTemplate>
                <table border="0" cellpadding="4" cellspacing="0" style="border-collapse: collapse">
                    <tr>
                        <td>
                            <table border="0" cellpadding="0" style="width: 200px; height: 200px">
                                <tr>
                                    <td align="center" style="font-weight: bold; font-size: medium; color: white; background-color: Green">
                                        SLIM Log In</td>
                                </tr>
                                <tr align="left">
                                    <td style="font-weight: bold; color: white; background-color: green; text-decoration: none">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                       <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                            ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>

                                        </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="UserName" runat="server" Font-Names="Tahoma" Font-Size="0.8em" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td style="font-weight: bold; color: white; background-color: green; text-decoration: none">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="Password" runat="server" Font-Names="Tahoma" Font-Size="0.8em" TextMode="Password"
                                            Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="font-size: small; color: white; background-color: green; font-weight:bold;">
                                        <asp:Literal ID="FailureText" runat="server"   EnableViewState="False"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="LoginButton" runat="server" BackColor="White" BorderColor="#C5BBAF"
                                            BorderStyle="Outset" BorderWidth="2px" CommandName="Login" Font-Bold="True" Font-Names="Tahoma"
                                            Font-Size="Small" ForeColor="#000040" Text="Log In" ValidationGroup="Login1" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
        </asp:Login><br />
        &nbsp;<asp:Button ID="Button_UnsecureWebQuery" runat="server" Font-Bold="True" Font-Names="Tahoma"
            Font-Size="12px" Text="Unsecure WebQuery" /><br />
        <br />
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="Green"
            Height="64px" 
            Width="732px"></asp:Label>
        <br />
        <br />
        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Calibri" Font-Size="Small"></asp:Label></div>
    </form>
</body>
</html>
