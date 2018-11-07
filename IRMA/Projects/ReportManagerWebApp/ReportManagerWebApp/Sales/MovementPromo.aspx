<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Sales_MovementPromo" title="Report Manager - Promo Movement" Codebehind="MovementPromo.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="SalesReports.aspx?valuePath=Reports Home/Sales">Sales Reports</a> &gt; Movement - Promo</h3>
    </div>

    <table style="width: 808px; ">
        <tr>
            <td style="width: 113px; text-align: right">
                Team</td>
            <td colspan="2">
                <asp:DropDownList ID="cmbTeam" runat="server" DataSourceID="ICTeam"
                    DataTextField="Team_Name" DataValueField="Team_No" Font-Names="Verdana" Font-Size="10pt"
                    Width="200px" TabIndex="1">
                </asp:DropDownList><asp:SqlDataSource ID="ICTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetTeams" SelectCommandType="StoredProcedure" DataSourceMode="DataReader"></asp:SqlDataSource>
            </td>
            <td colspan="3" style="text-align: left; width: 304px;">
                </td>
        </tr>
        <tr>
            <td style="width: 111px; text-align: right;">
                Begin Date</td>
            <td colspan="2"><igsch:WebDateChooser ID="dteBeginDate"
                    runat="server" Width="112px" Value="" TabIndex="2">
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
            </igsch:WebDateChooser></td>
            <td colspan="3" style="text-align: left; width: 304px;">
                <asp:RequiredFieldValidator ID="reqfld_BeginDate" runat="server" ControlToValidate="dteBeginDate"
                    ErrorMessage="* Begin Date is a required field."
                    SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rngValid_BeginDate" runat="server" ControlToValidate="dteBeginDate"
                    ErrorMessage="* Begin Date - Value must be a valid date." 
                    Type="Date" Display="Dynamic" Width="280px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 111px; text-align: right;">
                End Date</td>
            <td colspan="2"><igsch:WebDateChooser ID="dteEndDate" runat="server" Width="112px" TabIndex="3">
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
            </igsch:WebDateChooser></td>
            <td colspan="3" style="text-align: left; width: 304px;">
                <asp:RequiredFieldValidator ID="reqfld_EndDate" runat="server" ControlToValidate="dteEndDate"
                    ErrorMessage="* End Date is a required field." 
                    SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rngValid_EndDate" runat="server" ControlToValidate="dteEndDate"
                    ErrorMessage="* End Date - Value must be a valid date." 
                    Type="Date" SetFocusOnError="True" Display="Dynamic" Width="272px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 111px;">
            </td>
            <td colspan="2" valign="top">
                <asp:Button ID="btnSetDate" runat="server" CausesValidation="False" Text="Last Week"
                    ToolTip="Set dates for previous week" Width="104px" TabIndex="4" /></td>
            <td colspan="3" style="width: 304px;"><asp:CompareValidator ID="comp_DateRange" runat="server" ControlToCompare="dteEndDate"
                    ControlToValidate="dteBeginDate" Display="Dynamic" ErrorMessage="* Invalid date range - End Date cannot be earlier than Begin Date."
                    Operator="LessThanEqual"
                    Type="Date" Width="280px"></asp:CompareValidator></td>
        </tr>
        <tr>
            <td style="width: 113px; text-align: right;">
                Report Format</td>
            <td colspan="2">
                <asp:DropDownList ID="cmbReportFormat" runat="server" 
                    Width="104px" TabIndex="5">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" TabIndex="6"
                    Text="View Report" Width="104px" /></td>
            <td style="width: 304px" colspan="3">
            </td>
        </tr>
    </table>
</asp:Content>

