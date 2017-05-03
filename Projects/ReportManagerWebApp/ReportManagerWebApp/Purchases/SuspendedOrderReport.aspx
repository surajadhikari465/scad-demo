<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Purchases_SuspendedOrderReport" title="Suspended Order Report" Codebehind="SuspendedOrderReport.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar">
        <h3><a href="..\Default.aspx">Home</a> &gt; <a href="PurchaseReports.aspx?valuePath=Reports Home/Purchases">Purchasing Reports</a>&gt; Suspended Order Report</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 150px; text-align: right;">
                Store</td>
            <td style="width: 249px; text-align: left;"><asp:DropDownList ID="cmbStore" runat="server" Width="225px" DataSourceID="ICStores" DataTextField="Store_Name" DataValueField="Store_No" Font-Names="Verdana" Font-Size="10pt">
                </asp:DropDownList><asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="SELECT Store_No, Store_Name
                                FROM Store s (NOLOCK)
                                WHERE WFM_Store = 1 OR Mega_Store = 1
                                UNION
                                SELECT NULL as Store_No, '&lt;All Stores&gt;' Store_Name Order By 2 ASC" ProviderName="<%$ ConnectionStrings:ReportManager_Conn.ProviderName %>"></asp:SqlDataSource></td>
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
                    SelectCommand="Select Subteam_No, Subteam_Name 
                                    FROM Subteam (NOLOCK)
                                    UNION 
                                    SELECT NULL Subteam_No, '&lt;All Subteams&gt;' Subteam_Name 
                                    ORDER BY Subteam_Name ASC" ProviderName="<%$ ConnectionStrings:ReportManager_Conn.ProviderName %>">
                </asp:SqlDataSource>
            </td>
            <td style="width: 318px; text-align: left">
                </td>
        </tr>
        <tr>
            <td style="width: 164px; text-align: right">
                Include Current Suspensions: &nbsp;<br />
            </td>
            <td style="width: 329px">
                <asp:RadioButton ID="optCurrentYes" runat="server" Checked="True" GroupName="CurrentSuspensions"
                    Text="Yes" />
                &nbsp;&nbsp;
                <asp:RadioButton ID="optCurrentNo" runat="server" GroupName="CurrentSuspensions" Text="No" /><br />
            </td>
            <td style="width: 343px">
            </td>
        </tr>
        <tr>
            <td style="width: 164px; text-align: right">
                Include Approved Suspensions: &nbsp;<br />
            </td>
            <td style="width: 329px">
                <asp:RadioButton ID="optApprovedYes" runat="server" GroupName="ApprovedSuspensions"
                    Text="Yes" />
                &nbsp;&nbsp;
                <asp:RadioButton ID="optApprovedNo" runat="server" Checked="True" GroupName="ApprovedSuspensions" Text="No" /><br />
            </td>
            <td style="width: 343px">
            </td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right">
                Approved Order Receiving Log Dates</td>
            <td style="width: 249px; text-align: left">
                <span style="font-size: 10pt">
                from</span>&nbsp; 
                <igsch:WebDateChooser ID="dteStartDate" runat="server">
                </igsch:WebDateChooser>
                <span style="font-size: 10pt">
                to</span>&nbsp;
                <igsch:WebDateChooser ID="dteEndDate" runat="server" AutoCloseUp="False">
                </igsch:WebDateChooser>
            </td>
            <td style="width: 318px; text-align: left">
                <asp:CompareValidator ID="OrderDateCompare" runat="server" ControlToCompare="dteStartDate"
                    ControlToValidate="dteEndDate" ErrorMessage="* The Start Date must be earlier than the End Date."
                    Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator></td>
        </tr>
        <tr>
            <td style="width: 164px; text-align: right;">
                Report Format: &nbsp;
            </td>
            <td style="width: 329px; text-align: left;">
                <br />
                <asp:DropDownList ID="cmbReportFormat" runat="server" TabIndex="3" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList></td>
            <td style="width: 343px; text-align: left;">
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

