<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.eInvoiceItemExceptions" title="EInvoiceing Item Exceptions - Items Not On PO" Codebehind="eInvoiceItemExceptions.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar">
        <h3><a href="..\Default.aspx">Home</a> &gt; Item Reports</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 150px; text-align: right;">
                Store</td>
            <td style="width: 249px; text-align: left;"><asp:DropDownList ID="cmbStore" runat="server" Width="225px" DataSourceID="ICStores" DataTextField="Store_Name" DataValueField="Store_No" Font-Names="Verdana" Font-Size="10pt">
                </asp:DropDownList><asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="GetAllStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource></td>
            <td style="width: 318px; text-align: left">
                </td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right">
               Subteam

            </td>
            <td style="width: 249px; text-align: left">
                <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="ICSubteam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="ICSubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT ST.SubTeam_No, ST.SubTeam_Name FROM SubTeam ST (NOLOCK) ">
                    <SelectParameters>
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 318px; text-align: left">
                </td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right">Vendor</td>
            <td style="width: 346px; text-align: left">
                <asp:DropDownList ID="cmbVendor" runat="server" DataSourceID="ICVendors" DataTextField="CompanyName"
                    DataValueField="Vendor_ID" Width="225px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICVendors" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetVendors" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right">
                Order Date</td>
            <td style="width: 249px; text-align: left">
                <span style="font-size: 10pt">
                from</span>&nbsp; 
                <igsch:WebDateChooser ID="OrderDateFromChooser" runat="server">
                </igsch:WebDateChooser>
                <span style="font-size: 10pt">
                to</span>&nbsp;
                <igsch:WebDateChooser ID="OrderDateToChooser" runat="server">
                </igsch:WebDateChooser>
            </td>
            <td style="width: 318px; text-align: left">
                <asp:CompareValidator ID="OrderDateCompare" runat="server" ControlToCompare="OrderDateFromChooser"
                    ControlToValidate="OrderDateToChooser" ErrorMessage="* The Order From Date must be earlier than the Order To Date."
                    Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator></td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right;">
                &nbsp;</td>
            <td style="width: 249px; text-align: left;">
                &nbsp;<asp:CheckBox ID="DetailedCheckBox" runat="server" Text="Detailed" 
                    Visible="False" /></td>
            <td style="width: 318px; text-align: left;">
            </td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right">
                </td>
             <td style="width: 249px; text-align: left">
                 &nbsp;<asp:Button ID="btnReport" runat="server" Text="View Report"
                    Width="96px" /></td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
    </table>
    
    

</asp:Content>

