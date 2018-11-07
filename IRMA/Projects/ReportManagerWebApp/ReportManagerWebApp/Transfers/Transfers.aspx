<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Transfers_Transfers" title="Report Manager - Transfer Report" Codebehind="Transfers.aspx.vb" %>
<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="Transfers.aspx?valuePath=Reports Home/Distribution">
        Transfers</a> &gt; Transfer Orders Report</h3>
    </div>
    <table style="width: 528px">
        <tr>
            <td style="width: 173px; height: 45px;" valign="middle">
                Order Date Start:</td>
            <td style="width: 186px; height: 30px;" valign="middle">
                <igsch:WebDateChooser ID="dteOrderDateStart" runat="server" TabIndex="4" Value=""
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
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 173px; height: 45px" valign="middle">
                Order Date End:</td>
            <td style="width: 186px; height: 30px" valign="middle">
                <igsch:WebDateChooser ID="dteOrderDateEnd" runat="server" TabIndex="4" Value="" Width="112px">
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
        <tr>
            <td style="width: 173px; height: 45px;" valign="middle">
                Expected Date Start:</td>
            <td style="width: 186px; height: 30px;" valign="middle">
                <igsch:WebDateChooser ID="dteExpectedDateStart" runat="server" TabIndex="4" Value=""
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
        </tr>
        <tr>
            <td style="width: 173px; height: 45px;" valign="middle">
                Expected Date End:</td>
            <td style="width: 186px; height: 30px;" valign="middle">
                <igsch:WebDateChooser ID="dteExpectedDateEnd" runat="server" TabIndex="4" Value=""
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
        </tr>
        <tr>
            <td style="width: 173px; height: 36px" valign="middle">
                From
                SubTeam:</td>
            <td style="width: 186px; height: 36px" valign="middle">
                <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="IC_SubTeam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" Width="224px" Height="24px">
                </asp:DropDownList>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 173px; height: 36px" valign="middle">
                Receiving Store:</td>
            <td style="width: 186px; height: 36px" valign="middle">
                <asp:DropDownList ID="cmbReceivingStore" runat="server" DataMember="DefaultView" DataSourceID="IC_Store"
                    DataTextField="Store_Name" DataValueField="Vendor_ID" Width="225px">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td style="width: 173px; height: 36px" valign="middle">
                Transferring Store:</td>
            <td style="width: 186px; height: 36px" valign="middle">
                <asp:DropDownList ID="cmbTransferringStore" runat="server" DataMember="DefaultView" DataSourceID="IC_Store"
                    DataTextField="Store_Name" DataValueField="Vendor_ID" Width="225px">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td style="width: 173px; height: 42px;">
                Report Type:</td>
            <td style="width: 186px; height: 42px;">
                <br />
                <asp:RadioButton ID="rbSummary" runat="server" GroupName="gnReportType" Text="Summary" Checked="True" /><br />
                <asp:RadioButton ID="rbDetail" runat="server" GroupName="gnReportType"
                    Text="Detail" /><br />
            </td>
        </tr>
        <tr>
            <td style="width: 173px">
            </td>
            <td style="width: 186px">
            </td>
        </tr>
        <tr>
            <td style="width: 173px; height: 21px" valign="middle">
                <br />
                Report Format:</td>
            <td style="width: 186px; height: 21px">
                <br />
                <asp:DropDownList ID="cmbReportFormat" runat="server" TabIndex="6" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td style="width: 173px">
            </td>
            <td style="width: 186px">
                <br />
                <asp:Button ID="btnReport" runat="server" TabIndex="9" Text="View Report" Width="96px" /></td>
        </tr>
    </table>
    &nbsp;
                <asp:SqlDataSource ID="IC_Store" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT v.Vendor_ID, s.Store_Name &#13;&#10;FROM Vendor v&#13;&#10;JOIN Store s ON s.Store_No = v.Store_No&#13;&#10;WHERE s.WFM_Store = 1&#13;&#10;ORDER BY s.Store_Name"></asp:SqlDataSource>
                <asp:SqlDataSource ID="IC_SubTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT * FROM SubTeam WHERE EXEDistributed = 1"></asp:SqlDataSource>
</asp:Content>

