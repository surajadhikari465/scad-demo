<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" codebehind="PurchaseAccrualReport.aspx.vb" Inherits="ReportManagerWebApp.Purchases_PurchaseAccrualReport" title="PurchaseAccrualReport" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="AccountingReports.aspx?valuePath=Reports Home/Accounting">Accounting Reports</a> &gt; Purchase Accrual Report</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 160px; text-align: right">
                Store:</td>
            <td style="width: 329px;">
                <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                    DataValueField="Store_No"
                    Width="265px" TabIndex="1">
                </asp:DropDownList>

<asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetAllStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 343px;">
                <asp:RequiredFieldValidator ID="reqfld_cmbStore" runat="server" ControlToValidate="cmbStore"
                    ErrorMessage="* Store is a required field." Height="18px"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 160px; text-align: right">
                Subteam:</td>
            <td style="width: 329px; text-align: left">
                <asp:DropDownList ID="cmbSubteam" runat="server" DataSourceID="ICSubteam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" TabIndex="2"
                    Width="265px" AutoPostBack="True">
                </asp:DropDownList>
            <asp:SqlDataSource ID="ICSubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT ST.SubTeam_No, ST.SubTeam_Name FROM SubTeam ST (NOLOCK) ">
            </asp:SqlDataSource>
            </td>
            <td style="width: 343px; text-align: left">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 163px; text-align: right">
                As of Date:</td>
            <td style="width: 249px">
                <igsch:webdatechooser id="dteAsOfdate" runat="server" tabindex="7" value="" width="128px">
                        <CalendarLayout DayNameFormat="FirstLetter" FooterFormat="" ShowFooter="False" ShowNextPrevMonth="False"
                            ShowTitle="False">
                            <SelectedDayStyle BackColor="#0054E3" ></SelectedDayStyle>
                            <DayStyle BackColor="White" Font-Names="Arial" Font-Size="9pt" ></DayStyle>
                            <OtherMonthDayStyle ForeColor="#ACA899" ></OtherMonthDayStyle>
                            <DayHeaderStyle BackColor="#7A96DF" ForeColor="White" ></DayHeaderStyle>
                            <TitleStyle BackColor="#9EBEF5" ></TitleStyle>
                            <CalendarStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False">
                            </CalendarStyle>
                        </CalendarLayout>
                    </igsch:webdatechooser>
            </td>
            <td colspan="4" style="width: 332px">
                <asp:RequiredFieldValidator ID="reqfld_dteAsOfDate" runat="server" ControlToValidate="dteAsOfdate"
                    ErrorMessage="* As of Date is a required field."></asp:RequiredFieldValidator><br />
                <asp:RangeValidator ID="rng_AsOfDateValue" runat="server" ControlToValidate="dteAsOfdate"
                    Display="Dynamic" ErrorMessage="*As of  Date - Value must be a valid date." Type="Date"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 160px; text-align: right;">
                Report Format</td>
            <td style="width: 329px; text-align: left;">
                <br />
                <asp:DropDownList ID="cmbReportFormat" runat="server" TabIndex="3" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList><asp:Button ID="btnReport" runat="server" TabIndex="4"
                    Text="View Report" Width="100px" /></td>
            <td style="width: 343px; text-align: left;">
            </td>
        </tr>
    </table>
</asp:Content>

