<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.GrossMarginExceptionVsMvmt" title="Gross Margin Exception Vs. Movement" Codebehind="GrossMarginExceptionVsMvmt.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="PriceReports.aspx?valuePath=Reports Home/Prices">Price Reports</a> &gt; Gross Margin Exception Vs. Movement</h3>
    </div>
        <table style="width: 827px;">
            <tr>
                <td style="width: 163px; text-align: right;">
                    Min Team</td>
                <td style="width: 249px" >
                    <asp:DropDownList ID="cmbMinTeam" runat="server" DataSourceID="ICTeams" DataTextField="Team_Name"
                        DataValueField="Team_No" Height="24px" Width="160px" TabIndex="1">
                    </asp:DropDownList><asp:SqlDataSource ID="ICTeams" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                        SelectCommand="GetTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                </td>
                <td colspan="4" style="width: 332px">
                    </td>
            </tr>
            <tr>
                <td style="width: 163px; text-align: right;">
                    Max Team</td>
                <td style="width: 249px" >
                    <asp:DropDownList ID="cmbMaxTeam" runat="server" DataSourceID="ICTeams" DataTextField="Team_Name"
                        DataValueField="Team_No" Height="24px" Width="160px" TabIndex="1">
                    </asp:DropDownList>
                </td>
                <td colspan="4" style="width: 332px">
                </td>
            </tr>
            <tr>
                <td style="width: 163px; text-align: right;">
                    Min Subteam</td>
                <td style="width: 249px">
                    <asp:DropDownList ID="cmbMinSubteam" runat="server" DataSourceID="ICSubteams" DataTextField="SubTeam_Name"
                        DataValueField="SubTeam_No" TabIndex="2"
                        Width="160px">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="ICSubteams" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                        SelectCommand="GetAllSubTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                </td>
                <td colspan="4" style="width: 332px"></td>
            </tr>
            <tr>
                <td style="width: 163px; text-align: right;">
                    Max Subteam</td>
                <td style="width: 249px">
                    <asp:DropDownList ID="cmbMaxSubteam" runat="server" DataSourceID="ICSubteams" DataTextField="SubTeam_Name"
                        DataValueField="SubTeam_No" TabIndex="2"
                        Width="276px">
                    </asp:DropDownList>&nbsp;
                </td>
                <td colspan="4" style="width: 332px">
                </td>
            </tr>
            <tr>
                <td style="width: 163px; text-align: right;">
                    Vendor</td>
                <td style="width: 249px">
                    <asp:DropDownList ID="cmbVendor" runat="server" DataSourceID="ICVendors" DataTextField="CompanyName"
                        DataValueField="Vendor_ID" TabIndex="2"
                        Width="275px">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="ICVendors" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                        SelectCommand="GetVendors" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                </td>
                <td colspan="4" style="width: 332px">
                </td>
            </tr>
            <tr>
                <td style="width: 163px; text-align: right">
                    Min Gross Margin</td>
                <td style="width: 249px">
                    <asp:TextBox ID="txtMinGM" runat="server" Width="88px" TabIndex="3"></asp:TextBox></td>
                <td colspan="4" style="width: 332px">
                    <asp:RequiredFieldValidator ID="reqfld_txtMinval" runat="server" ControlToValidate="txtMinGM"
                        ErrorMessage="* Minimum Gross Margin is a required field." Width="324px"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 163px; text-align: right">
                    Max Gross Margin</td>
                <td style="width: 249px">
                    <asp:TextBox ID="txtMaxGM" runat="server" Width="88px" TabIndex="4"></asp:TextBox></td>
                <td colspan="4" style="width: 332px">
                    <asp:RequiredFieldValidator ID="reqfld_txtMaxval" runat="server" ControlToValidate="txtMaxGM"
                        ErrorMessage="* Maximum Gross Margin is a required field." Width="325px"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 163px; text-align: right">
                    Store</td>
                <td style="width: 249px">
                    <asp:DropDownList ID="cmbStores" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                        DataValueField="Store_No" TabIndex="2"
                        Width="277px">
                    </asp:DropDownList><asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                        SelectCommand="GetStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                </td>
                <td colspan="4" style="width: 332px">
                </td>
            </tr>
            <tr>
                <td style="width: 163px; text-align: right;">
                    Start Date</td>
                <td style="width: 249px">
                    <igsch:WebDateChooser ID="dteBegin" runat="server" TabIndex="7" Value="" Width="128px">
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
                    <asp:RequiredFieldValidator ID="reqfld_dteBegin" runat="server" ControlToValidate="dteBegin"
                        ErrorMessage="* Begin Date is a required field."></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 163px; text-align: right">
                    End Date</td>
                <td style="width: 249px">
                    <igsch:WebDateChooser ID="dteEnd" runat="server" TabIndex="8" Width="128px">
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
                    <asp:RequiredFieldValidator ID="reqfld_dteEnd" runat="server" ControlToValidate="dteEnd"
                        ErrorMessage="* End Date is a required field."></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 163px; text-align: right;">
                    Report Format</td>
                <td style="width: 249px">
                    <asp:DropDownList ID="cmbReportFormat" runat="server" Width="120px" TabIndex="7">
                        <asp:ListItem>CSV</asp:ListItem>
                        <asp:ListItem>EXCEL</asp:ListItem>
                        <asp:ListItem Selected="True">HTML</asp:ListItem>
                        <asp:ListItem>PDF</asp:ListItem>
                        <asp:ListItem>XML</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="btnReport" runat="server" Text="View Report"
                        Width="100px" TabIndex="8" /></td>
                <td colspan="4" style="width: 332px">
                </td>
            </tr>
        </table>
    
    <asp:CompareValidator ID="cmp_BeginDateValidator" runat="server" ControlToValidate="dteBegin"
        ErrorMessage="* Begin Date - Value must be a valid date." Operator="DataTypeCheck"
        Type="Date"></asp:CompareValidator><br />
    <asp:CompareValidator ID="cmp_EndDateValidator" runat="server" ControlToValidate="dteEnd"
        ErrorMessage="* End Date - Value must be a valid date." Operator="DataTypeCheck"
        Type="Date"></asp:CompareValidator><br />
    <asp:CompareValidator ID="cmp_txtBeginDate" runat="server" ControlToCompare="dteEnd"
        ControlToValidate="dteBegin" ErrorMessage="* Begin Date must be prior to End Date"
        Operator="LessThan" Type="Date"></asp:CompareValidator><br />
    <asp:CompareValidator ID="cmp_txtEndDate" runat="server" ControlToCompare="dteBegin"
        ControlToValidate="dteEnd" ErrorMessage="* End Date must be after Begin Date"
        Operator="GreaterThan" Type="Date"></asp:CompareValidator>
    
</asp:Content>

