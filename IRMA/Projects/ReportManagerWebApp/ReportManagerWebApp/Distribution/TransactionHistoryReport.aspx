<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Distribution_TransactionHistory" title="Report Manager - Transaction History Report" Codebehind="TransactionHistoryReport.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="DistributionReports.aspx?valuePath=Reports Home/Distribution">Distribution Reports</a> &gt; Transaction History Report</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 192px; text-align: right;">
                <br />
                DC Store:&nbsp;
            </td>
            <td style="width: 249px; text-align: left;">
                <br />
                <asp:DropDownList ID="cmbDCStore" runat="server" Width="225px" DataSourceID="IC_DC_Store" DataTextField="Store_Name" DataValueField="Store_No" Font-Names="Verdana" Font-Size="10pt" TabIndex="1">
                </asp:DropDownList><asp:SqlDataSource ID="IC_DC_Store" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="SELECT Store_Name, Store_No FROM Store WHERE Distribution_Center = 1 OR Manufacturer = 1"></asp:SqlDataSource></td>
            <td style="width: 318px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbStore" runat="server" ControlToValidate="cmbDCStore"
                    ErrorMessage="Store is a required field." MaximumValue="2147483647" 
                    MinimumValue="1" SetFocusOnError="True"
                    Type="Integer" Width="201px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right;">
                <br />
                Identifier:&nbsp;
            </td>
            <td style="width: 249px; text-align: left">
                <br />
                &nbsp;<asp:TextBox ID="txtIdentifier" runat="server" Width="216px"></asp:TextBox><br />
            </td>
            <td style="width: 318px; text-align: left">
                <asp:RequiredFieldValidator ID="reqfld_Identifier" runat="server" ControlToValidate="txtIdentifier"
                    Display="Dynamic" ErrorMessage="Identifier is a required field." SetFocusOnError="True"
                    Width="216px"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
                <br />
                Start Date :&nbsp;
                <br />
            </td>
            <td style="width: 346px; text-align: left">
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
            <td style="width: 192px; text-align: right">
                <br />
                End Date :&nbsp;
                <br />
            </td>
            <td style="width: 346px; text-align: left">
                <br />
                <igsch:WebDateChooser ID="dteEndDate" runat="server" TabIndex="4" Value="" Width="112px">
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
            <td style="width: 192px; text-align: right">
                <br />
                Report Format</td>
            <td style="width: 346px; text-align: left">
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
            <td style="width: 192px; text-align: right">
            </td>
            <td style="width: 249px; text-align: left">
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
            </td>
            <td style="width: 249px; text-align: left">
                <br />
                <asp:Button ID="btnReport" runat="server" TabIndex="9" Text="View Report" Width="96px" /></td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
    </table>
    </asp:Content>

