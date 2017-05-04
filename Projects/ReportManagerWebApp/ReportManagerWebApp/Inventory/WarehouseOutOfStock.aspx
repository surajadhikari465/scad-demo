<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Inventory_WarehouseOutOfStockReport" Title="Warehouse Out Of Stock Report" Codebehind="WarehouseOutOfStock.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="InventoryReports.aspx?valuePath=Reports Home/Inventory">Inventory Reports</a> &gt; Warehouse Out Of Stock Report</h3>
</div>
    <table border="0">
        <tr>
            <td style="width: 152px; height: 1px; text-align: right">Warehouse</td>
            <td style="width: 458px; height: 1px; text-align: left">
                <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="ICStore" DataTextField="Store_Name" DataValueField="Store_No" Width="225px"></asp:DropDownList>
                <asp:SqlDataSource ID="ICStore" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="
                                    SELECT
	                                    Store_No,
	                                    Store_Name
                                    FROM
	                                    Store
                                    WHERE
                                        Distribution_Center = 1
                                  ">
                </asp:SqlDataSource>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
        <tr>
            <td style="width: 152px; height: 1px; text-align: right">Vendor</td>
            <td style="width: 458px; height: 1px; text-align: left">
                <asp:DropDownList ID="cmbVendor" runat="server" DataSourceID="ICVendors" DataTextField="CompanyName" DataValueField="Vendor_ID" Width="225px"></asp:DropDownList>
                <asp:SqlDataSource ID="ICVendors" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="
                                    SELECT 
                                        CompanyName, 
                                        Vendor_ID 
                                    FROM 
                                        Vendor 
                                    ORDER BY 
                                        CompanyName
                                  ">
                </asp:SqlDataSource>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
        <tr>
            <td style="width: 152px; height: 1px; text-align: right">SubTeam</td>
            <td style="width: 458px; height: 1px; text-align: left">
                <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="ICSubTeam" DataTextField="SubTeam_Name" DataValueField="SubTeam_No" Width="225px"></asp:DropDownList>
                <asp:SqlDataSource ID="ICSubTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="
                                    SELECT 
                                        st.SubTeam_No, 
                                        st.SubTeam_Name
                                    FROM
                                        SubTeam			        (nolock) st
                                    ORDER BY
                                        st.SubTeam_No
                                  ">
                </asp:SqlDataSource>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
         <tr>
            <td style="width: 150px; text-align: right">Report Format</td>
             <td style="width: 346px; text-align: left">
                <asp:DropDownList ID="cmbReportFormat" runat="server" Font-Names="Verdana" Font-Size="10pt" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" Text="View Report" Width="96px" />
            </td>
            <td style="width: 318px; text-align: left"></td>
        </tr>
        </table>
</asp:Content>

