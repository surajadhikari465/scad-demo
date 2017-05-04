<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Distribution_WarehouseMovementReport" title="Report Manager - Warehouse Movement" Codebehind="WarehouseMovementReport.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="DistributionReports.aspx?valuePath=Reports Home/Distribution">Distribution Reports</a> &gt; Warehouse Movement</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 109px; text-align: right; height: 56px;">
                Warehouse:&nbsp;
            </td>
            <td style="width: 260px; text-align: left; height: 56px;">
                <asp:DropDownList ID="cmbDCStore" runat="server" Width="225px" DataSourceID="SqlDataSource1" DataTextField="Store_Name" DataValueField="Store_No" Font-Names="Verdana" Font-Size="10pt" TabIndex="1">
                </asp:DropDownList><asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="SELECT * FROM Store WHERE Distribution_Center = 1"></asp:SqlDataSource></td>
            <td style="width: 30px; height: 56px; text-align: left">
            </td>
            <td style="width: 314px; text-align: left; height: 56px;">
                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="cmbDCStore"
                    ErrorMessage="* Warehouse is a required field." MaximumValue="2147483647" 
                    MinimumValue="1" SetFocusOnError="True"
                    Type="Integer" Width="241px"></asp:RangeValidator></td>
            <td style="width: 552px; height: 56px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 109px; text-align: right;">
                Sub-Team:&nbsp;
            </td>
            <td style="width: 260px; text-align: left;">
                <asp:DropDownList ID="cmbSubTeam" runat="server" Width="225px" DataSourceID="IC_SubTeam" DataTextField="SubTeam_Name" DataValueField="SubTeam_No" Font-Names="Verdana" Font-Size="10pt" TabIndex="1">
                </asp:DropDownList><asp:SqlDataSource ID="IC_SubTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="SELECT * FROM SubTeam WHERE EXEDistributed = 1">
                </asp:SqlDataSource>
            </td>
            <td style="width: 30px; text-align: left">
            </td>
            <td style="width: 314px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbSubTeam" runat="server" ControlToValidate="cmbSubTeam"
                    ErrorMessage="* Sub-Team is a required field." MaximumValue="2147483647" MinimumValue="1"
                    SetFocusOnError="True" Type="Integer" Width="232px"></asp:RangeValidator></td>
            <td style="width: 552px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 109px; text-align: right; height: 21px;">
                From:
            </td>
            <td style="width: 260px; text-align: left; height: 21px;"><igsch:webdatechooser id="Webdatechooser1" runat="server" value="" width="112px" TabIndex="4">
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
            <td style="width: 30px; height: 21px; text-align: left">
                To:</td>
            <td style="width: 314px; text-align: left; height: 21px;"><igsch:webdatechooser id="Webdatechooser2" runat="server" value="" width="112px" TabIndex="4">
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
            <td style="width: 552px; height: 21px; text-align: left">
                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="Webdatechooser1"
                    ControlToValidate="Webdatechooser2" ErrorMessage="* From date must be earlier than To date."
                    Operator="GreaterThanEqual"></asp:CompareValidator></td>
        </tr>
        <tr>
            <td style="width: 109px; text-align: right">
                From:</td>
            <td style="width: 260px; text-align: left"><igsch:webdatechooser id="Webdatechooser3" runat="server" value="" width="112px" TabIndex="4">
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
            <td style="width: 30px; text-align: left">
                To:</td>
            <td style="width: 314px; text-align: left"><igsch:webdatechooser id="Webdatechooser4" runat="server" value="" width="112px" TabIndex="4">
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
            <td style="width: 552px; text-align: left">
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="Webdatechooser3"
                    ControlToValidate="Webdatechooser4" ErrorMessage="* From date must be earlier than To date."
                    Operator="GreaterThanEqual"></asp:CompareValidator></td>
        </tr>
        <tr>
            <td style="width: 109px; text-align: right">
                From:</td>
            <td style="width: 260px; text-align: left"><igsch:webdatechooser id="Webdatechooser5" runat="server" value="" width="112px" TabIndex="4">
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
            <td style="width: 30px; text-align: left">
                To:</td>
            <td style="width: 314px; text-align: left"><igsch:webdatechooser id="Webdatechooser6" runat="server" value="" width="112px" TabIndex="4">
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
            <td style="width: 552px; text-align: left">
                <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToCompare="Webdatechooser5"
                    ControlToValidate="Webdatechooser6" ErrorMessage="* From date must be earlier than To date."
                    Operator="GreaterThanEqual"></asp:CompareValidator></td>
        </tr>
        <tr>
            <td style="width: 109px; text-align: right">
                From:</td>
            <td style="width: 260px; text-align: left"><igsch:webdatechooser id="Webdatechooser7" runat="server" value="" width="112px" TabIndex="4">
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
            <td style="width: 30px; text-align: left">
                To:</td>
            <td style="width: 314px; text-align: left"><igsch:webdatechooser id="Webdatechooser8" runat="server" value="" width="112px" TabIndex="4">
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
            <td style="width: 552px; text-align: left">
                <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToCompare="Webdatechooser7"
                    ControlToValidate="Webdatechooser8" ErrorMessage="* From date must be earlier than To date."
                    Operator="GreaterThanEqual"></asp:CompareValidator></td>
        </tr>
        <tr>
            <td style="width: 109px; text-align: right; height: 30px;">
                From:</td>
            <td style="width: 260px; text-align: left; height: 30px;"><igsch:webdatechooser id="Webdatechooser9" runat="server" value="" width="112px" TabIndex="4">
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
            <td style="width: 30px; height: 30px; text-align: left">
                To:</td>
            <td style="width: 314px; text-align: left; height: 30px;"><igsch:webdatechooser id="Webdatechooser10" runat="server" value="" width="112px" TabIndex="4">
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
            <td style="width: 552px; height: 30px; text-align: left">
                <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToCompare="Webdatechooser9"
                    ControlToValidate="Webdatechooser10" ErrorMessage="* From date must be earlier than To date."
                    Operator="GreaterThanEqual"></asp:CompareValidator></td>
        </tr>
        <tr>
            <td style="width: 109px; text-align: right">
            </td>
            <td style="width: 260px; text-align: left">
                <br />
                <asp:Button ID="btnReport" runat="server" TabIndex="9" Text="View Report" Width="96px" /></td>
            <td style="width: 30px; text-align: left">
            </td>
            <td style="width: 314px; text-align: left">
            </td>
            <td style="width: 552px; text-align: left">
            </td>
        </tr>
    </table>
    </asp:Content>

