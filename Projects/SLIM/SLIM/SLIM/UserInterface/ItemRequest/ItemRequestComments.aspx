<%@ Page Language="VB" AutoEventWireup="false" Inherits="SLIM.UserInterface_ItemRequest_ItemRequestComments" Codebehind="ItemRequestComments.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Rejection Comments</title>
    

</head>

  
<body style="background-color: #e3e3e3;" >
    <form id="form1" runat="server">
    <div>
       
        <table>
            <tr >
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Item:"></asp:Label></td>
                <td style="width: 120px">
                    <asp:Label ID="Label_Item_Desc" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="UPC:"></asp:Label></td>
                <td>
                    <asp:Label ID="Label_UPC" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Comments:"></asp:Label></td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="width: 250px">
                    <asp:TextBox ID="TextBox_Comments" runat="server" Height="139px" TextMode="MultiLine" ToolTip="Insert Rejection Comments here" MaxLength="255" Width="225px"
                        ></asp:TextBox></td>
            </tr>
            <tr>
                <td >
                    <asp:Button ID="Button_Save" runat="server" Text="Save" UseSubmitBehavior="False" /></td>
                <td >
                    <asp:Label ID="Label_Error" runat="server" Font-Size="12px" ForeColor="Red" TabIndex="26"
                        Text="Label" Visible="False"></asp:Label></td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
