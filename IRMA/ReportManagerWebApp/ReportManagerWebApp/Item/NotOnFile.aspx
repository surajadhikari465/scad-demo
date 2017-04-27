<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Item_NotOnFile" title="IRMA Report Manager" Codebehind="NotOnFile.aspx.vb" %>
<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>
<asp:Content ID="conMain" runat="server" ContentPlaceHolderID="contentPlaceHolder1">
    <div id="navbar">
        <h3><a href="..\Default.aspx">Home</a> &gt; <a href="ItemReports.aspx?valuePath=Reports Home/Items">Item Reports</a> &gt; Not on File</h3>
    </div>
    <table id="tblParameters" border="0">
        <tr>
            <td style="width: 237px; text-align: right;">
                Selected Week End Date</td>
            <td style="width: 249px">
                    <igsch:WebDateChooser ID="wdcEndDate" runat="server" TabIndex="7" Value="" Width="128px" NullDateLabel="Please Select">
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
            <td colspan="4" style="width: 332px">
                <asp:RequiredFieldValidator ID="reqfld_wdcEndDate" runat="server" ControlToValidate="wdcEndDate"
                    ErrorMessage="* Selected Week End Date is a required field."></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 237px; text-align: right">
                <span>Report Format</span></td>
            <td style="width: 346px; text-align: left">
                <asp:DropDownList ID="cmbReportFormat" runat="server" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" Text="View Report" Width="96px" /></td>
            <td style="width: 300px; text-align: left">
            </td>
        </tr>
    </table>
</asp:Content>