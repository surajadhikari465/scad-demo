<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Distribution_FillRateReport" title="Untitled Page" Codebehind="FillRateReport.aspx.vb" %>
<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="DistributionReports.aspx?valuePath=Reports Home/Distribution">Distribution Reports</a> &gt; Fill Rate Report</h3>
    </div>
    <br />
    <table style="width: 347px">
        <tr>
            <td style="width: 1081px; text-align: right;">
                <asp:Label ID="Label1" runat="server" Text="Facility:" Width="56px"></asp:Label>
                &nbsp;
                <br />
            </td>
            <td style="width: 146px">
                <asp:DropDownList ID="cmbFacility" runat="server" DataSourceID="IC_Facility" DataTextField="CompanyName"
                    DataValueField="Vendor_Id" Font-Names="Verdana" Font-Size="10pt" TabIndex="1"
                    Width="248px">
                </asp:DropDownList><asp:SqlDataSource ID="IC_Facility" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT V.Vendor_ID, V.CompanyName&#13;&#10;FROM Vendor V&#13;&#10;JOIN Store S ON S.Store_No = V.Store_No&#13;&#10;WHERE (S.Distribution_Center = 1 OR S.Manufacturer = 1) AND WFM_Store = 0">
                </asp:SqlDataSource>
            </td>
            <td style="width: 131px">
            </td>
            <td style="width: 141px">
                &nbsp; &nbsp;&nbsp;
            </td>
            <td style="width: 141px">
            </td>
            <td style="width: 141px">
            </td>
        </tr>
        <tr>
            <td style="width: 1081px; text-align: right;">
                <br />
                <asp:Label ID="Label2" runat="server" Text="Vendor:" Width="64px"></asp:Label>
                &nbsp;
            </td>
            <td style="width: 146px">
                <br />
                <asp:DropDownList ID="cmbVendor" runat="server" DataSourceID="ICVendors" DataTextField="CompanyName"
                    DataValueField="Vendor_ID" Width="248px">
                </asp:DropDownList><asp:SqlDataSource ID="ICVendors" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetVendors" SelectCommandType="StoredProcedure" DeleteCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 131px">
            </td>
            <td style="width: 141px">
            </td>
            <td style="width: 141px">
            </td>
            <td style="width: 141px">
            </td>
        </tr>
        <tr>
            <td style="width: 1081px; height: 21px; text-align: right;">
                <br />
                <asp:Label ID="Label3" runat="server" Text="SubTeam:" Width="72px"></asp:Label>
                &nbsp;
            </td>
            <td style="height: 21px; width: 146px;">
                <br />
                <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="IC_SubTeam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" Width="248px">
                </asp:DropDownList><asp:SqlDataSource ID="IC_SubTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT * FROM SubTeam WHERE EXEDistributed = 1"></asp:SqlDataSource>
            </td>
            <td style="width: 131px; height: 21px">
            </td>
            <td style="width: 141px; height: 21px">
            </td>
            <td style="width: 141px; height: 21px">
            </td>
            <td style="width: 141px; height: 21px">
            </td>
        </tr>
        <tr>
            <td style="width: 1081px; height: 36px; text-align: right;">
                <br />
                <asp:Label ID="Label4" runat="server" Text="Store:" Width="40px"></asp:Label>
                &nbsp;
            </td>
            <td style="width: 146px; height: 36px;">
                <br />
                <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="IC_Store" DataTextField="Store_Name"
                    DataValueField="Store_No" Width="248px" DataMember="DefaultView">
                </asp:DropDownList><br />
                <asp:SqlDataSource ID="IC_Store" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 131px; height: 36px">
            </td>
            <td style="width: 141px; height: 36px">
            </td>
            <td style="width: 141px; height: 36px">
            </td>
            <td style="width: 141px; height: 36px">
            </td>
        </tr>
        <tr>
            <td style="width: 1081px; text-align: right;">
                <asp:Label ID="Label5" runat="server" Text="Time Frame:" Width="96px"></asp:Label>
                &nbsp;
            </td>
            <td style="width: 146px">
                <br />
                <asp:RadioButton ID="rbCustomRange" runat="server" Checked="True" GroupName="gnTimeFrame"
                    Text="Custom Date Range" Width="176px" AutoPostBack="True" /><br />
                <asp:RadioButton ID="rbLast7Days" runat="server" GroupName="gnTimeFrame"
                    Text="Last 7 Days" AutoPostBack="True" /><br />
                <asp:RadioButton ID="rbLast4Weeks" runat="server" GroupName="gnTimeFrame" Text="Last 4 Weeks" AutoPostBack="True" /><br />
                <asp:RadioButton ID="rbFPtoDate" runat="server" GroupName="gnTimeFrame" Text="Fiscal Period To Date" Width="184px" AutoPostBack="True" /><br />
            </td>
            <td style="width: 131px; text-align: center">
                <asp:Label ID="lblStartDate" runat="server" Enabled="False" Text="Start Date:" Width="96px"></asp:Label>
                <igsch:WebDateChooser ID="dteStartDate" runat="server" TabIndex="4" Value="" Width="136px" Enabled="False">
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
                    <AutoPostBack ValueChanged="True" />
                </igsch:WebDateChooser>
            </td>
            <td style="width: 141px; text-align: center">
            </td>
            <td style="width: 141px; text-align: center">
                <asp:Label ID="lblEndDate" runat="server" Enabled="False" Text="End Date:" Width="96px"></asp:Label><br />
                <igsch:WebDateChooser ID="dteEndDate" runat="server" TabIndex="5" Width="128px" Enabled="False">
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
            <td style="width: 141px; text-align: center">
                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="dteStartDate"
                    ErrorMessage="RequiredFieldValidator" Width="168px">Start Date is Required</asp:RequiredFieldValidator><br />
                <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="dteEndDate"
                    ErrorMessage="RequiredFieldValidator" Width="168px">End Date is Required</asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 1081px; text-align: right;">
                <br />
                <asp:Label ID="Label6" runat="server" Text="Report Type:" Width="96px"></asp:Label>
                &nbsp;
            </td>
            <td style="width: 146px">
                <br />
                <asp:RadioButton ID="rbDetail" runat="server" Checked="True" GroupName="gnReportType"
                    Text="Detail" /><br />
                <asp:RadioButton ID="rbSummary" runat="server" GroupName="gnReportType" Text="Summary" /><br />
            </td>
            <td style="width: 131px">
            </td>
            <td style="width: 141px">
            </td>
            <td style="width: 141px">
            </td>
            <td style="width: 141px">
            </td>
        </tr>
        <tr>
            <td style="width: 1081px; height: 21px;">
                <br />
                <asp:Label ID="Label7" runat="server" Text="Report Format:" Width="120px"></asp:Label></td>
            <td style="height: 21px; width: 146px;">
                <br />
                <asp:DropDownList ID="cmbReportFormat" runat="server" TabIndex="6" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList><br />
            </td>
            <td style="width: 131px; height: 21px">
            </td>
            <td style="width: 141px; height: 21px">
            </td>
            <td style="width: 141px; height: 21px">
            </td>
            <td style="width: 141px; height: 21px">
            </td>
        </tr>
        <tr>
            <td style="width: 1081px">
            </td>
            <td style="width: 146px">
                <asp:Button ID="btnReport" runat="server" TabIndex="9" Text="View Report" Width="96px" /></td>
            <td style="width: 131px">
            </td>
            <td style="width: 141px">
            </td>
            <td style="width: 141px">
            </td>
            <td style="width: 141px">
            </td>
        </tr>
    </table>
</asp:Content>

