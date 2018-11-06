<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Accounting_GLUploadReport" title="GL Upload Report" Codebehind="GLUploadReport.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="AccountingReports.aspx?valuePath=Reports Home/Accounting">Accounting Reports</a> &gt; GL Upload Report</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 164px; text-align: right">
                Report Type: &nbsp;<br />
            </td>
            <td style="width: 329px">
                <asp:RadioButton ID="optPreUpload" runat="server" Checked="True" GroupName="ReportType"
                    Text="Pre Upload" />
                &nbsp;&nbsp;
                <asp:RadioButton ID="optPostUpload" runat="server" GroupName="ReportType" Text="Post Upload" /><br />
            </td>
            <td style="width: 343px">
            </td>
        </tr>
        <tr>
            <td style="width: 164px; text-align: right">
                <br />
                Order Type: &nbsp;
                <br />
            </td>
            <td style="width: 329px">
                <br />
                <asp:DropDownList ID="cmbOrderType" runat="server"
                    Width="265px" TabIndex="1" AutoPostBack="True">
                </asp:DropDownList><br />
            </td>
            <td style="width: 343px">
                <br />
            </td>
        </tr>
        <tr>
            <td style="width: 164px; text-align: right" valign="top">
                <br />
                Store: &nbsp;
            </td>
            <td style="width: 329px;">
                <br />
                <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                    DataValueField="Store_No"
                    Width="265px" TabIndex="1">
                </asp:DropDownList>

<asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetAllStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 343px;">
                </td>
        </tr>
        <tr>
            <td style="width: 164px; text-align: right; height: 122px;" valign="top">
                <br />
                Date Range: &nbsp;
            </td>
            <td style="width: 249px; height: 122px;">
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                <br />
                <table>
                    <tr>
                        <td style="width: 117px">
                            <asp:RadioButton ID="optPreviousFP" runat="server" Checked="True" GroupName="DateType"
                                Text="Previous Week" AutoPostBack="True" Width="129px" /></td>
                        <td style="width: 145px">
                            <asp:RadioButton ID="optCustomDate" runat="server" GroupName="DateType" Text="Custom Date" AutoPostBack="True" /></td>
                    </tr>
                    <tr>
                        <td style="width: 117px">
                        </td>
                        <td style="width: 145px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 117px; height: 53px">
                            <asp:Label ID="lblStartDate" runat="server" Enabled="False" Text="Start Date:"></asp:Label><igsch:WebDateChooser id="dteStartDate" runat="server" tabindex="7" value="" width="128px" Enabled="False">
                                <CalendarLayout DayNameFormat="FirstLetter" FooterFormat="" ShowFooter="False" ShowNextPrevMonth="False"
                            ShowTitle="False">
                                    <CalendarStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False">
                                    </CalendarStyle>
                                    <DayHeaderStyle BackColor="#7A96DF" ForeColor="White" />
                                    <DayStyle BackColor="White" Font-Names="Arial" Font-Size="9pt" />
                                    <OtherMonthDayStyle ForeColor="#ACA899" />
                                    <SelectedDayStyle BackColor="#0054E3" />
                                    <TitleStyle BackColor="#9EBEF5" />
                                </CalendarLayout>
                            </igsch:WebDateChooser>
                        </td>
                        <td style="width: 145px; height: 53px">
                            &nbsp;&nbsp;
                            <asp:Label ID="lblEndDate" runat="server" Enabled="False" Text="End Date:"></asp:Label><igsch:WebDateChooser id="dteEndDate" runat="server" tabindex="7" value="" width="128px" Enabled="False">
                                <CalendarLayout DayNameFormat="FirstLetter" FooterFormat="" ShowFooter="False" ShowNextPrevMonth="False"
                            ShowTitle="False">
                                    <CalendarStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False">
                                    </CalendarStyle>
                                    <DayHeaderStyle BackColor="#7A96DF" ForeColor="White" />
                                    <DayStyle BackColor="White" Font-Names="Arial" Font-Size="9pt" />
                                    <OtherMonthDayStyle ForeColor="#ACA899" />
                                    <SelectedDayStyle BackColor="#0054E3" />
                                    <TitleStyle BackColor="#9EBEF5" />
                                </CalendarLayout>
                            </igsch:WebDateChooser>
                        </td>
                    </tr>
                </table>
                &nbsp;</td>
            <td colspan="4" style="width: 332px; height: 122px;">
                <br />
                </td>
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
            <td style="width: 164px; text-align: right">
            </td>
            <td style="width: 329px; text-align: left">
                <br />
                <asp:Button ID="btnReport" runat="server" TabIndex="4"
                    Text="View Report" Width="100px" /></td>
            <td style="width: 343px; text-align: left">
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

