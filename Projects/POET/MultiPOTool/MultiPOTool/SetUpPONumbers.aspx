<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MultiPOTool.Master"
    CodeBehind="SetUpPONumbers.aspx.vb" Inherits="MultiPOTool.SetUpPONumbers" Title="Set Up PO Numbers" %>

<%@ Register Assembly="Infragistics35.WebUI.UltraWebGrid.ExcelExport.v10.1, Version=10.1.20101.1011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid.ExcelExport" TagPrefix="igtblexp" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" language="javascript">
        function SelectAll(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%= gvPONumbers.ClientID %>");
            //variable to contain the cell of the grid
            var cell;

            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (i = 1; i < grid.rows.length; i++) {
                    //get the reference of first column
                    cell = grid.rows[i].cells[4];

                    //loop acccellording to the number of childNodes in the 
                    for (j = 0; j < cell.childNodes.length; j++) {
                        //if childNode type is CheckBox                 
                        if (cell.childNodes[j].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell checkbox within the grid
                            cell.childNodes[j].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }

    </script>
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td  align="center" colspan="2" style="padding:30px;">
                <asp:Label Font-Bold="True"  ID="lblCreatePONumbers" runat="server" CssClass="HeaderLable" Font-Italic="true" />
            </td>
        </tr>
        <tr>
            <td>
                <div class="tableDiv">
                    <table class="tablePOSetup" style="width: 93px">
                        <tr>
                            <td style="width: 52px">
                                <asp:Label ID="Label1" runat="server" Text="Region" Width="180px"></asp:Label>
                            </td>
                            <td style="width: 3px">
                                <asp:DropDownList ID="ddlRegion" runat="server" DataSourceID="dsGetRegions" DataTextField="RegionName"
                                    DataValueField="RegionID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 52px">
                                <asp:Label ID="Label4" runat="server" Text="PO Type" Width="181px"></asp:Label>
                            </td>
                            <td style="width: 3px">
                                <asp:DropDownList ID="ddlPOTypes" runat="server" DataSourceID="dsPOTypes" DataTextField="POTypeDescription"
                                    DataValueField="POTypeID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 52px">
                                <asp:Label ID="Label3" runat="server" Text="Count of POs to Create" Width="180px"></asp:Label>
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtPOCount" runat="server" Width="24px"></asp:TextBox>
                                <asp:Label ID="lblCountError" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 52px">
                            </td>
                            <td style="width: 3px">
                                <asp:Button ID="btnCreatePONumbers" runat="server" Text="Create PO Numbers" CssClass="button" />
                            </td>
                        </tr>
                       
                    </table>
                </div>
            </td>
        </tr>
          <tr>
            <td colspan="3">
                <hr />
            </td>
        </tr>
        <tr id="trColmnExport" runat="server">
            <td style="padding-top:30px;">
                <table>
                    <tr>
                        <td>
                            <asp:Label Font-Bold="True" Font-Size="Larger" ID="lblExport" runat="server" Text="Export Selected PO Numbers for" />
                        </td>
                    </tr>
                     <tr>
                            <td style="width: 52px">
                                <asp:Label ID="Label2" runat="server" Text="Count of columns to Export" Width="181px"></asp:Label>
                            </td>
                            <td style="width: 3px">
                                <asp:TextBox ID="txtColumnCount" runat="server" Width="24px" ValidationGroup="vgExport"
                                    Style="float: left"></asp:TextBox>
                            </td>
                              <td style="width: 3px">
                                <asp:Button ID="btnExport" runat="server" Text="Export Selected PO Numbers" Width="196px"
                                    ValidationGroup="vgCol"  CssClass="button" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 52px">
                            </td>
                          
                        </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ObjectDataSource ID="dsGetRegions" runat="server" SelectMethod="GetRegionsByUser"
                    TypeName="MultiPOTool.Utility">
                    <SelectParameters>
                        <asp:Parameter Name="UserId" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="dsPOTypes" runat="server" SelectMethod="GetPOTypes" TypeName="MultiPOTool.BOPONumbers">
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <hr />
            </td>
        </tr>
        <tr id="trGrid" runat="server">
            <td style="padding-top:30px;">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Label Font-Bold="True" Font-Size="Larger" ID="lblGirdText" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="ddlDiv" runat="server" style="width: 260px; height: 28px;">
                                <asp:Label ID="Label5" runat="server" Text="Display"></asp:Label>
                                <asp:DropDownList ID="ddlShowOption" runat="server" AutoPostBack="True">
                                    <asp:ListItem>25</asp:ListItem>
                                    <asp:ListItem>50</asp:ListItem>
                                    <asp:ListItem>100</asp:ListItem>
                                    <asp:ListItem>250</asp:ListItem>
                                    <asp:ListItem>500</asp:ListItem>
                                </asp:DropDownList>
                                <asp:Label ID="Label6" runat="server" Text="Per Page"></asp:Label></div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="padding-bottom:20px;">
                            <asp:Button ID="btnDelete" runat="server" Text="Delete Selected" CssClass="button" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="gridDiv">
                                <asp:GridView ID="gvPONumbers" HeaderStyle-BackColor="LightGray" BackColor="White" runat="server" AllowPaging="True"
                                    AllowSorting="True" AutoGenerateColumns="False" CssClass="mgrid" PageSize="30">
                                    <Columns>
                                       
                                        <asp:BoundField DataField="PONumber" HeaderText="PO Number" SortExpression="PONumber" />
                                        <asp:BoundField DataField="RegionName" HeaderText="Region" SortExpression="RegionName" />
                                        <asp:BoundField DataField="POTypeDescription" HeaderText="PO Type" SortExpression="POTypeDescription" />
                                        <asp:BoundField DataField="DateAssigned" HeaderText="Date Assigned" SortExpression="DateAssigned" />
                                        
                                         <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" runat="server" />
                                            </HeaderTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    
                </table>
            </td>
        </tr>
       
    </table>
</asp:Content>
