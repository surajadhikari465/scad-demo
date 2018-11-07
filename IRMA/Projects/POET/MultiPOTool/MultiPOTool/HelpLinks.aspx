<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MultiPOTool.Master" CodeBehind="HelpLinks.aspx.vb" Inherits="MultiPOTool.HelpLinks" 
    title="Help Links" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <script language="javascript" type="text/javascript">
     function MouseOverHandler(gridName, id, objectType) {

         if (objectType == 0) {

             igtbl_getCellById(id).getRow().Element.className += " over";

         }

     }



     function MouseOutHandler(gridName, id, objectType) {

         if (objectType == 0) {

             igtbl_getCellById(id).getRow().Element.className = igtbl_getCellById(id).getRow().Element.className.replace(" over", "")

         }

     } 

    </script>

    <table>
        <tr>    
            <td>
                <asp:BulletedList ID="blLinks" runat="server" BulletStyle="Circle" AppendDataBoundItems="true"  DisplayMode="HyperLink"
                DataSourceID="dsLinks" DataTextField="LinkDescription" DataValueField="LinkURL">
                </asp:BulletedList>
            </td>
        </tr>
    </table>
  
    <asp:ObjectDataSource ID="dsLinks" runat="server" SelectMethod="GetLinks" TypeName="MultiPOTool.BOHelpLinks" OldValuesParameterFormatString="original_{0}">
    </asp:ObjectDataSource>
</asp:Content>
