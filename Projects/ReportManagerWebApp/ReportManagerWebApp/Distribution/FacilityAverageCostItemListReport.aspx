<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Distribution_FacilityAvgCost" title="Report Manager - Facility Average Cost" Codebehind="FacilityAverageCostItemListReport.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="DistributionReports.aspx?valuePath=Reports Home/Distribution">Distribution Reports</a> &gt; Facility Average Cost</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 137px; text-align: right; height: 56px;">
                Facility:&nbsp;
            </td>
            <td style="width: 260px; text-align: left; height: 56px;">
                <asp:DropDownList ID="cmbDCStore" runat="server" Width="225px" DataSourceID="SqlDataSource1" DataTextField="Store_Name" DataValueField="Store_No" Font-Names="Verdana" Font-Size="10pt" TabIndex="1">
                </asp:DropDownList><asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="SELECT * FROM Store WHERE Distribution_Center = 1"></asp:SqlDataSource></td>
            <td style="width: 314px; text-align: left; height: 56px;">
                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="cmbDCStore"
                    ErrorMessage="* Facility is a required field." MaximumValue="2147483647" 
                    MinimumValue="1" SetFocusOnError="True"
                    Type="Integer" Width="241px"></asp:RangeValidator></td>
            <td style="width: 552px; height: 56px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 137px; text-align: right;">
                Sub-Team:&nbsp;
            </td>
            <td style="width: 260px; text-align: left;">
                <asp:DropDownList ID="cmbSubTeam" runat="server" Width="225px" DataSourceID="IC_SubTeam" DataTextField="SubTeam_Name" DataValueField="SubTeam_No" Font-Names="Verdana" Font-Size="10pt" TabIndex="1" AutoPostBack="True">
                </asp:DropDownList><asp:SqlDataSource ID="IC_SubTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="SELECT * FROM SubTeam WHERE EXEDistributed = 1">
                </asp:SqlDataSource>
            </td>
            <td style="width: 314px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbSubTeam" runat="server" ControlToValidate="cmbSubTeam"
                    ErrorMessage="* Sub-Team is a required field." MaximumValue="2147483647" MinimumValue="1"
                    SetFocusOnError="True" Type="Integer" Width="232px"></asp:RangeValidator></td>
            <td style="width: 552px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 137px; height: 21px; text-align: right">
                Category:&nbsp;
            </td>
            <td style="width: 260px; height: 21px; text-align: left">
                <asp:DropDownList ID="cmbCategory" runat="server" DataSourceID="ICCategory" DataTextField="Category_Name"
                    DataValueField="Category_ID" Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="ICCategory" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT C.Category_ID, C.Category_Name FROM ItemCategory C (NOLOCK) WHERE C.SubTeam_No = @SubTeam_No">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="cmbSubTeam" DefaultValue="" Name="SubTeam_No" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 314px; height: 21px; text-align: left">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="cmbCategory"
                    ErrorMessage="* Category is a requiered field."></asp:RequiredFieldValidator></td>
            <td style="width: 552px; height: 21px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 137px; text-align: right; height: 21px;">
                Date: &nbsp;</td>
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
            <td style="width: 314px; text-align: left; height: 21px;">
                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Webdatechooser1"
                    ErrorMessage="* Date is a requiered field."></asp:RequiredFieldValidator></td>
            <td style="width: 552px; height: 21px; text-align: left">
                </td>
        </tr>
        <tr>
            <td style="width: 137px; text-align: right">
            </td>
            <td style="width: 260px; text-align: left">
                <br />
                <asp:Button ID="btnReport" runat="server" TabIndex="9" Text="View Report" Width="96px" /></td>
            <td style="width: 314px; text-align: left">
            </td>
            <td style="width: 552px; text-align: left">
            </td>
        </tr>
    </table>
    </asp:Content>

