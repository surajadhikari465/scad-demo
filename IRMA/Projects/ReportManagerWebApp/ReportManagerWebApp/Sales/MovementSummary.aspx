<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Sales_MovementSummary" title="Movement Summary" Codebehind="MovementSummary.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="SalesReports.aspx?valuePath=Reports Home/Sales">Sales Reports</a> &gt; Movement Summary</h3>
    </div>
    <table style="width: 735px;">
        <tr>
            <td style="width: 160px; text-align: right;  height: 30px;">
                Top</td>
            <td colspan="2" style="height: 30px; text-align: left">
                <asp:RadioButton ID="rdbtnTop" runat="server" Checked="True" GroupName="order" TabIndex="1" /></td>
            <td colspan="3" style="height: 30px">
                <span></span>
            </td>
        </tr>
        <tr>
            <td style="width: 160px; text-align: right; height: 30px;">
                Bottom</td>
            <td colspan="2" style="height: 30px; text-align: left">
                <asp:RadioButton ID="rdbtnBottom" runat="server" Font-Bold="False" GroupName="order" TabIndex="2" /></td>
            <td colspan="3" style="height: 30px">
            </td>
        </tr>
        <tr>
            <td style="width: 160px; text-align: right; height: 27px;">
                Number of Results</td>
            <td colspan="2" style="height: 27px">
                <asp:TextBox ID="txtResults" runat="server" TabIndex="3"></asp:TextBox></td>
            <td colspan="3" style="text-align: left; height: 27px;">
                <asp:RequiredFieldValidator ID="reqfld_txtResults" runat="server" ControlToValidate="txtResults"
                    ErrorMessage="* Number of results is a required field." ></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 160px; height: 27px; text-align: right">
                Zones</td>
            <td colspan="2" style="height: 27px">
                <asp:DropDownList ID="cmbZone" runat="server" DataSourceID="ICZones" DataTextField="Zone_Name"
                    DataValueField="Zone_id" Height="24px"
                    TabIndex="4" Width="239px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICZones" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetZones" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td colspan="3" style="height: 27px; text-align: left">
                <asp:RequiredFieldValidator ID="reqfld_cmbZone" runat="server" ControlToValidate="cmbZone"
                    ErrorMessage="* Zone is a required field." ></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 160px; height: 27px; text-align: right">
                Subteam</td>
            <td colspan="2" style="height: 27px">
                <asp:DropDownList ID="cmbSubteam" runat="server" TabIndex="5"
                    Width="241px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICSubteams" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetAllSubTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td colspan="3" style="height: 27px; text-align: left">
                <asp:RequiredFieldValidator ID="reqfld_cmbSubteam" runat="server" ControlToValidate="cmbSubteam"
                    ErrorMessage="* Subteam is a required field." ></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 160px; text-align: right">
                Begin Date</td>
            <td colspan="2">
                <igsch:WebDateChooser ID="dteBegin"
                    runat="server" Width="112px" Value="" TabIndex="6">
                <CalendarLayout DayNameFormat="FirstLetter" FooterFormat="" ShowFooter="False" ShowNextPrevMonth="False"
                    ShowTitle="False">
                    <SelectedDayStyle BackColor="#0054E3" />
                    <DayStyle BackColor="White" Font-Names="Arial" Font-Size="9pt" />
                    <OtherMonthDayStyle ForeColor="#ACA899" />
                    <DayHeaderStyle BackColor="#7A96DF" ForeColor="White" />
                    <TitleStyle BackColor="#9EBEF5" />
                    <CalendarStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False">
                    </CalendarStyle>
                </CalendarLayout>
            </igsch:WebDateChooser>
            </td>
            <td colspan="3" style="text-align: left">
                <asp:RequiredFieldValidator ID="reqfld_dteBegin" runat="server" ControlToValidate="dteBegin"
                    ErrorMessage="* Begin Date is a required field." ></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 160px; text-align: right">
                End Date</td>
            <td colspan="2">
                <igsch:WebDateChooser ID="dteEnd" runat="server" Width="112px" TabIndex="7">
                <CalendarLayout DayNameFormat="FirstLetter" FooterFormat="" ShowFooter="False" ShowNextPrevMonth="False"
                    ShowTitle="False">
                    <SelectedDayStyle BackColor="#0054E3" />
                    <DayStyle BackColor="White" Font-Names="Arial" Font-Size="9pt" />
                    <OtherMonthDayStyle ForeColor="#ACA899" />
                    <DayHeaderStyle BackColor="#7A96DF" ForeColor="White" />
                    <TitleStyle BackColor="#9EBEF5" />
                    <CalendarStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False">
                    </CalendarStyle>
                </CalendarLayout>
            </igsch:WebDateChooser>
            </td>
            <td colspan="3" style="text-align: left">
                <asp:RequiredFieldValidator ID="reqfld_dteEnd" runat="server" ControlToValidate="dteEnd"
                    ErrorMessage="* End Date is a required field." ></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 160px; text-align: right;">
                Report Format</td>
            <td colspan="2">
                <asp:DropDownList ID="cmbReportFormat" runat="server" Width="120px" TabIndex="8">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" TabIndex="9"
                    Text="View Report" Width="100px" /></td>
            <td colspan="3">
            </td>
        </tr>
    </table>
    
    <asp:RangeValidator ID="rngValid_txtResults" runat="server" ControlToValidate="txtResults"
        ErrorMessage="* Number of Results - Must be a positive integer." MaximumValue="999999999" MinimumValue="1"
        Type="Integer"></asp:RangeValidator><br />    
    <asp:CompareValidator id="cmp_BeginDateValidator" runat="server" Operator="DataTypeCheck" Type="Date" 
        ControlToValidate="dteBegin" ErrorMessage="* Begin Date - Value must be a valid date." ></asp:CompareValidator>
        <br />    
    <asp:CompareValidator id="cmp_EndDateValidator" runat="server" Operator="DataTypeCheck" Type="Date" 
        ControlToValidate="dteEnd" ErrorMessage="* End Date - Value must be a valid date."></asp:CompareValidator>
        <br />  
    <asp:CompareValidator ID="cmp_txtBeginDate" runat="server" ControlToCompare="dteEnd"
        ControlToValidate="dteBegin" ErrorMessage="* Begin Date must be prior to End Date" 
        Operator="LessThan" Type="Date"></asp:CompareValidator><br />
    <asp:CompareValidator ID="cmp_txtEndDate" runat="server" ControlToCompare="dteBegin"
        ControlToValidate="dteEnd" ErrorMessage="* End Date must be after Begin Date"
         Operator="GreaterThan"
        Type="Date"></asp:CompareValidator>
</asp:Content>

