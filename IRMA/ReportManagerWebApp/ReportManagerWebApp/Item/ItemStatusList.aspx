<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Item_ItemStatusList" title="Item Status List" Codebehind="ItemStatusList.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="ItemReports.aspx?valuePath=Reports Home/Items">Item Reports</a> &gt; Items Status List</h3>
    </div>
    &nbsp; 
    
    <table align="left">
        <tr>
            <td valign="middle">
            </td>
            <td style="width: 150px" valign="middle">
            </td>
            <td valign="middle">
            </td>
            <td style="width: 172px" valign="middle">
            </td>
            <td style="width: 172px" valign="middle">
                Vendor Item Status</td>
        </tr>
        <tr>
            <td valign="middle">
                <asp:RadioButtonList ID="Add_Identifier" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 150px" valign="middle">
                -
                Add Identifier</td>
            <td valign="middle">
                <asp:RadioButtonList ID="Organic" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 172px" valign="middle">
                -
                Organic</td>
            <td style="width: 172px" valign="middle" rowspan="5">
                <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Solid" 
                    BorderWidth="2px">
                    <asp:CheckBoxList ID="cbListVendorItemStatus" runat="server" 
                        DataSourceID="SqlDataSource1" DataTextField="StatusName" 
    DataValueField="StatusCode">
                    </asp:CheckBoxList>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td valign="middle">
                <asp:RadioButtonList ID="Default_Identifier" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 150px" valign="middle">
                -
                Default Identifier</td>
            <td valign="middle">
                <asp:RadioButtonList ID="Pre_Order" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 172px" valign="middle">
                -
                Pre Order</td>
        </tr>
        <tr>
            <td valign="middle">
                <asp:RadioButtonList ID="Deleted_Identifier" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 150px" valign="middle">
                -
                Deleted Item</td>
            <td valign="middle">
                <asp:RadioButtonList ID="Recall_Flag" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 172px" valign="middle">
                -
                Recall</td>
        </tr>
        <tr>
            <td valign="middle">
                <asp:RadioButtonList ID="Discontinue_Item" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 150px" valign="middle">
                -
                Discontinued</td>
            <td valign="middle">
                <asp:RadioButtonList ID="Refrigerated" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 172px" valign="middle">
                -
                Refrigerated</td>
        </tr>
        <tr>
            <td valign="middle">
                <asp:RadioButtonList ID="EXEDistributed" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 150px" valign="middle">
                -
                EXE Distributed</td>
            <td valign="middle">
                <asp:RadioButtonList ID="Remove_Identifier" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 172px" valign="middle">
                -
                Remove Identifier</td>
        </tr>
        <tr>
            <td valign="middle">
                <asp:RadioButtonList ID="Full_Pallet_Only" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 150px" valign="middle">
                -
                Full Pallet</td>
            <td valign="middle">
                <asp:RadioButtonList ID="Retail_Sale" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True" Selected="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 172px" valign="middle">
                -
                Retail Sale</td>
            <td style="width: 172px" valign="middle">
                &nbsp;</td>
        </tr>
        <tr>
            <td valign="middle">
                <asp:RadioButtonList ID="Keep_Frozen" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>                   
                </asp:RadioButtonList></td>
            <td style="width: 150px" valign="middle">
                -
                Keep Frozen</td>
            <td valign="middle">
                <asp:RadioButtonList ID="Scale_Identifier" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 172px" valign="middle">
                -
                Scale Identifier</td>
            <td style="width: 172px" valign="middle">
                &nbsp;</td>
        </tr>
        <tr>
            <td valign="middle">
                <asp:RadioButtonList ID="LockAuth" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 150px" valign="middle">
                -
                Lock Auth</td>
            <td valign="middle">
                <asp:RadioButtonList ID="Shipper_Item" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 172px" valign="middle">
                -
                Shipper</td>
            <td style="width: 172px" valign="middle">
                &nbsp;</td>
        </tr>
        <tr>
            <td valign="middle">
                <asp:RadioButtonList ID="National_Identifier" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 150px" valign="middle">
                -
                National Identifier</td>
            <td valign="middle">
                <asp:RadioButtonList ID="HFM_Item" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True" Selected="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 172px" valign="middle">
                -
                Sold at HFM</td>
            <td style="width: 172px" valign="middle">
                &nbsp;</td>
        </tr>
        <tr>
            <td valign="middle">
                <asp:RadioButtonList ID="NoDistMarkup" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 150px" valign="middle">
                -
                No Dist Markup</td>
            <td valign="middle">
                <asp:RadioButtonList ID="WFM_Item" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True" Selected="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 172px" valign="middle">
                -
                Sold at WFM</td>
            <td style="width: 172px" valign="middle">
                &nbsp;</td>
        </tr>
        <tr>
            <td valign="middle">
                <asp:RadioButtonList ID="Not_Available" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="True">Checked</asp:ListItem>
                    <asp:ListItem Value="False">Unchecked</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 150px" valign="middle">
                -
                Not Available</td>
            <td valign="middle">
                &nbsp;</td>
            <td style="width: 172px" valign="middle">
                &nbsp;</td>
            <td style="width: 172px" valign="middle">
                &nbsp;</td>
        </tr>
        <tr>
            <td valign="middle">
                &nbsp;</td>
            <td style="width: 150px" valign="middle">
                &nbsp;</td>
            <td valign="middle">
                &nbsp;</td>
            <td style="width: 172px" valign="middle">
                &nbsp;</td>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>" 
                SelectCommand="SELECT * FROM [VendorItemStatuses]"></asp:SqlDataSource>
        </tr>
        <tr>
            <td valign="middle">
                &nbsp;</td>
            <td style="width: 150px" valign="middle">
                &nbsp;</td>
            <td valign="middle" align="left">
                &nbsp;</td>
            <td style="width: 172px" valign="middle">
                &nbsp;</td>
            <td style="width: 172px" valign="middle">
                &nbsp;</td>
        </tr>
        <tr>
            <td valign="middle">
                &nbsp;</td>
            <td style="width: 150px" valign="middle">
                &nbsp;</td>
            <td valign="middle" align="left">
                <asp:Button ID="btnClearForm" runat="server" Text="Reset Defaults" Width="100px" /></td>
            <td style="width: 172px" valign="middle">
                </td>
            <td style="width: 172px" valign="middle">
                &nbsp;</td>
        </tr>
        <tr>
            <td valign="middle">
            </td>
            <td style="width: 150px" valign="middle">
            </td>
            <td valign="middle">
            </td>
            <td style="width: 172px" valign="middle">
            </td>
            <td style="width: 172px" valign="middle">
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right" valign="middle">
                Report Format &nbsp;&nbsp;</td>
            <td align="left" style="width: 150px" valign="middle">
                <asp:DropDownList ID="cmbReportFormat" runat="server" Width="149px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList></td>
            <td valign="middle">
                <asp:Button ID="btnReport" runat="server" Text="View Report" Width="100px" ToolTip="Results sets of 65k or more should be viewed in a report format other than excel" /></td>
            <td style="width: 172px" valign="middle">
                </td>
            <td style="width: 172px" valign="middle">
                &nbsp;</td>
        </tr>
    </table>
    <br />
</asp:Content>


