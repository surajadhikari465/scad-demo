<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Sales_ZeroMovement" title="Zero Movement Report" Codebehind="ZeroMovement.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="SalesReports.aspx?valuePath=Reports Home/Sales">Sales Reports</a> &gt; Zero Movement</h3>
    </div>
    <table style="width: 735px;">
        <tr>
            <td style="width: 129px; text-align: right">
                Store</td>
            <td colspan="2">
                <asp:DropDownList ID="cmbStore" runat="server" Height="24px"
                    Width="170px" TabIndex="1">
                </asp:DropDownList><asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT (CAST(Store_No As varchar(5)) + '|' + CAST(Regional As varchar(1))) As Store_No,Store_Name FROM Store WHERE Regional = 0"></asp:SqlDataSource>
            </td>
            <td colspan="3" style="text-align: left">
                <asp:RequiredFieldValidator ID="reqfld_cmbStore" runat="server" ControlToValidate="cmbStore"
                    ErrorMessage="* Store is a required field."></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 129px; text-align: right">
                Subteam</td>
            <td colspan="2">
                <asp:DropDownList ID="cmbSubteam" runat="server" DataSourceID="ICSubteams" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" TabIndex="2"
                    Width="137px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICSubteams" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetAllSubTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td colspan="3" style="text-align: left">
                <asp:RequiredFieldValidator ID="reqfld_cmbSubteam" runat="server" ControlToValidate="cmbSubteam"
                    ErrorMessage="* Subteam is a required field."></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 129px; text-align: right">
                Begin Date</td>
            <td colspan="2">
                <igsch:webdatechooser id="dteBegin" runat="server" tabindex="3"></igsch:webdatechooser>
            </td>
            <td colspan="3" style="text-align: left">
                <asp:RequiredFieldValidator ID="reqfld_dteBegin" runat="server" ControlToValidate="dteBegin"
                    ErrorMessage="* Begin Date is a required field." ></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 129px; text-align: right">
                End Date</td>
            <td colspan="2">
                <igsch:webdatechooser id="dteEnd" runat="server" tabindex="4"></igsch:webdatechooser>
            </td>
            <td colspan="3" style="text-align: left">
                <asp:RequiredFieldValidator ID="reqfld_dteEnd" runat="server" ControlToValidate="dteEnd"
                    ErrorMessage="* End Date is a required field." ></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 129px; text-align: right">
                Report Format</td>
            <td colspan="2">
                
                <%--<%=System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern %>--%>
                <asp:DropDownList ID="cmbReportFormat" runat="server" TabIndex="5" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" TabIndex="6"
                    Text="View Report" Width="100px" /></td>
            <td colspan="3" style="text-align: left">
            </td>
        </tr>
    </table>
    <br />
    <asp:CompareValidator id="cmp_BeginDateValidator" runat="server" Operator="DataTypeCheck" Type="Date" 
        ControlToValidate="dteBegin" ErrorMessage="* Begin Date - Value must be a valid date."></asp:CompareValidator><br />    
    <asp:CompareValidator id="cmp_EndDateValidator" runat="server" Operator="DataTypeCheck" Type="Date" 
        ControlToValidate="dteEnd" ErrorMessage="* End Date - Value must be a valid date." ></asp:CompareValidator><br />  
    <asp:CompareValidator ID="cmp_txtBeginDate" runat="server" ControlToCompare="dteEnd"
        ControlToValidate="dteBegin" ErrorMessage="* Begin Date must be prior to End Date" Operator="LessThan" Type="Date"></asp:CompareValidator><br />
    <asp:CompareValidator ID="cmp_txtEndDate" runat="server" ControlToCompare="dteBegin"
        ControlToValidate="dteEnd" ErrorMessage="* End Date must be after Begin Date" Operator="GreaterThan"
        Type="Date"></asp:CompareValidator>
</asp:Content>


