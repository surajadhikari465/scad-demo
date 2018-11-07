<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Misc_SoxConflict" title="Report Manager - PO Date Compare Report" Codebehind="IRMASOXConflictReport.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="MiscReports.aspx?valuePath=Reports Home/Misc">Miscellaneous Reports</a> &gt; IRMA SOX Conflict Report</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 221px; text-align: right;">
                <br />
                Conflict Type: &nbsp;</td>
            <td style="width: 290px; text-align: left;">
                <br />
                <asp:DropDownList ID="cmbConflictType" runat="server" Width="225px" Font-Names="Verdana" Font-Size="10pt" TabIndex="1">
                </asp:DropDownList><br />
            </td>
            <td style="width: 318px; text-align: left">
                </td>
        </tr>
        <tr>
            <td style="width: 221px; text-align: right">
                <br />
                Start Date: &nbsp;
            </td>
            <td style="width: 290px; text-align: left">
                <br />
                <igsch:WebDateChooser ID="dteStartDate" runat="server" TabIndex="4" Value="" Width="112px">
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
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 221px; text-align: right">
                <br />
                End Date: &nbsp;
            </td>
            <td style="width: 290px; text-align: left">
                &nbsp;<igsch:WebDateChooser ID="dteEndDate" runat="server" TabIndex="4" Value=""
                    Width="112px">
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
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 221px; text-align: right">
                <br />
                Inserted By
                User:&nbsp;
                <br />
            </td>
            <td style="width: 290px; text-align: left">
                <br />
                <asp:DropDownList ID="cmbInsertedByUser" runat="server" Width="225px" DataSourceID="IC_InsertedByUser" DataTextField="FullName" DataValueField="User_Id" Font-Names="Verdana" Font-Size="10pt" TabIndex="1">
                </asp:DropDownList><asp:SqlDataSource ID="IC_InsertedByUser" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="SELECT User_Id, &#13;&#10;             CASE WHEN CHARINDEX('(',FullName) - 1 > 0 THEN SUBSTRING(FullName,1,CHARINDEX('(',FullName) - 1)&#13;&#10;             ELSE FullName END AS FullName&#13;&#10;FROM Users&#13;&#10;WHERE AccountEnabled = 1&#13;&#10;ORDER BY FullName"></asp:SqlDataSource>
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 221px; text-align: right">
                <br />
                Report Format: &nbsp;
            </td>
            <td style="width: 290px; text-align: left">
                <br />
                <asp:DropDownList ID="cmbReportFormat" runat="server" TabIndex="8" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 221px; text-align: right">
            </td>
            <td style="width: 290px; text-align: left">
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 221px; text-align: right">
            </td>
            <td style="width: 290px; text-align: left">
                <br />
                <asp:Button ID="btnReport" runat="server" TabIndex="9" Text="View Report" Width="96px" /></td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
    </table>
    </asp:Content>

