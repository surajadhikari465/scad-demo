<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Item_ItemMaster" title="Movement Summary" Codebehind="ItemMasterDownload.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="ItemReports.aspx?valuePath=Reports Home/Items">Item Reports</a> &gt; Item Master Download</h3>
    </div>
    <table>
        <tr>
            <td colspan="3" style="height: 60px; text-align: center">
                <asp:RadioButtonList ID="ReportType" runat="server" RepeatDirection="Horizontal" Width="480px">
                    <asp:ListItem Selected="True" Value="rbtnItemmaster">Item Master</asp:ListItem>
                    <asp:ListItem Value="rbtnToledoMaster">Toledo Master</asp:ListItem>
                    <asp:ListItem Value="rbtnBoth">Item &amp; Toledo Master</asp:ListItem>
                </asp:RadioButtonList></td>
        </tr>
        <tr>
            <td style="text-align: right; width: 154px;">
                Vendor</td>
            <td style="text-align: left">
                <asp:DropDownList ID="cmbVendor" runat="server" DataSourceID="ICVendors" DataTextField="CompanyName"
                    DataValueField="Vendor_ID"
                    TabIndex="4" Width="250px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICVendors" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetVendors" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 3px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 154px;">
                SubTeam</td>
            <td style="text-align: left">
                <asp:DropDownList ID="cmbSubTeam" runat="server" TabIndex="5"
                    Width="250px" DataSourceID="ICSubteams" DataTextField="SubTeam_Name" DataValueField="SubTeam_No" AutoPostBack="True">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICSubTeams" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetAllSubTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 3px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 154px; height: 21px; text-align: right">
                Store</td>
            <td style="height: 21px; text-align: left">
                <asp:DropDownList ID="cmbStore" runat="server" TabIndex="5"
                    Width="250px" DataSourceID="ICStores" DataTextField="Store_Name" DataValueField="Store_No" AutoPostBack="True">
                </asp:DropDownList><asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 3px; height: 21px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 154px;">
                Class</td>
            <td style="text-align: left">
                <asp:DropDownList ID="cmbSubDept" runat="server" TabIndex="5"
                    Width="250px" DataSourceID="ICCategories" DataTextField="Category_Name" DataValueField="Category_ID" AutoPostBack="True">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICCategories" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT IC.Category_ID, IC.Category_Name FROM ItemCategory IC (NOLOCK) WHERE IC.SubTeam_No = ISNULL(@SubTeam_No, IC.SubTeam_No) ORDER BY IC.Category_Name">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="cmbSubTeam" DefaultValue="DBNull" Name="SubTeam_No"
                            PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 3px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 154px;">
                Level 3</td>
            <td style="text-align: left">
                <asp:DropDownList ID="cmbClass" runat="server" TabIndex="5"
                    Width="250px" DataSourceID="ICProdLevel4" DataTextField="Description" DataValueField="ProdHierarchyLevel3_ID" AutoPostBack="True">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICProdLevel4" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT PL3.ProdHierarchyLevel3_ID, PL3.Description FROM ProdHierarchyLevel3 PL3 (NOLOCK) WHERE PL3.Category_ID = ISNULL(@Category_ID, PL3.Category_ID) ORDER BY PL3.Description">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="cmbSubDept" DefaultValue="DBNull" Name="Category_ID"
                            PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 3px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 154px;">
                Level 4</td>
            <td style="text-align: left">
                <asp:DropDownList ID="cmbSubClass" runat="server" TabIndex="5"
                    Width="250px" DataSourceID="ICProdLevel3" DataTextField="Description" DataValueField="ProdHierarchyLevel4_ID">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICProdLevel3" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT PL4.ProdHierarchyLevel4_ID, PL4.Description FROM ProdHierarchyLevel4 PL4 (NOLOCK) WHERE PL4.ProdHierarchyLevel3_ID = ISNULL(@ProdHierarchyLevel3_ID, PL4.ProdHierarchyLevel3_ID) ORDER BY PL4.Description">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="cmbClass" DefaultValue="DBNull" Name="ProdHierarchyLevel3_ID"
                            PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 3px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 154px;">
                SKU Status</td>
            <td style="text-align: left">
                <asp:DropDownList ID="cmbSKUStatus" runat="server" TabIndex="5"
                    Width="250px">
                    <asp:ListItem>Active</asp:ListItem>
                    <asp:ListItem>Discontinued</asp:ListItem>
                    <asp:ListItem>Purged</asp:ListItem>
                </asp:DropDownList></td>
            <td style="width: 3px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 154px;">
                Merchandise Group</td>
            <td style="text-align: left">
                <asp:DropDownList ID="cmbMerchGp" runat="server" TabIndex="5"
                    Width="250px" DataSourceID="ICBrands" DataTextField="Brand_Name" DataValueField="Brand_ID">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICBrands" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetBrandAndID" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 3px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 154px;">
                Supplier Type</td>
            <td style="text-align: left">
                <asp:DropDownList ID="cmbSupplierType" runat="server" TabIndex="5"
                    Width="250px" DataSourceID="ICItemManager" DataTextField="Value" DataValueField="Manager_ID">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICItemManager" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetItemManagers" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 3px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 154px;">
                PLU Type</td>
            <td style="text-align: left">
                <asp:DropDownList ID="cmbPLUType" runat="server" TabIndex="5"
                    Width="250px">
                    <asp:ListItem Value="None">NONE</asp:ListItem>
                    <asp:ListItem Value="FrontEnd">Front End</asp:ListItem>
                    <asp:ListItem>Toledo</asp:ListItem>
                </asp:DropDownList></td>
            <td style="width: 3px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 154px;">
                From Identifier</td>
            <td style="text-align: left">
                <asp:TextBox ID="txtFromId" runat="server" TabIndex="3"></asp:TextBox></td>
            <td style="width: 3px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 154px;">
                To Identifier</td>
            <td style="text-align: left">
                <asp:TextBox ID="txtToId" runat="server" TabIndex="3"></asp:TextBox></td>
            <td style="width: 3px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="text-align: right; width: 154px;">
                Report Format</td>
            <td style="text-align: left">
                <asp:DropDownList ID="cmbReportFormat" runat="server" Width="150px" TabIndex="8">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" TabIndex="9"
                    Text="View Report" Width="100px" /></td>
            <td style="width: 3px; text-align: left">
                &nbsp;</td>
        </tr>
    </table>

</asp:Content>

