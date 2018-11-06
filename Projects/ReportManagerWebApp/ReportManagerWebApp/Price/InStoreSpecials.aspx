<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Price_InStoreSpecials" title="IRMA Report Manager" Codebehind="InStoreSpecials.aspx.vb" %>
<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="PriceReports.aspx?valuePath=Reports Home/Prices">Price Reports</a> &gt; In-Store Specials</h3>
    </div>
    <table border="0">
        <tr>
            <td style="text-align: right; width: 147px;">Store</td>
            <td>
                <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                    DataValueField="Store_No"
                    Width="265px" TabIndex="1">
                </asp:DropDownList>



<asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetAllStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td>
                <asp:RangeValidator ID="rngValid_cmbStore" runat="server" ControlToValidate="cmbStore"
                    ErrorMessage="* Store is a required field." MaximumValue="2147483647" MinimumValue="1"
                    SetFocusOnError="True" Type="Integer" Width="201px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="text-align: right; width: 147px;">Subteam</td>
            <td style="text-align: left">
                <asp:DropDownList ID="cmbSubteam" runat="server" DataSourceID="ICSubteam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" TabIndex="2"
                    Width="265px">
                </asp:DropDownList>
            <asp:SqlDataSource ID="ICSubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetAllSubTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="text-align: left">
                <asp:RequiredFieldValidator ID="reqfld_cmbSubteam" runat="server" ControlToValidate="cmbSubteam"
                    ErrorMessage="* Subteam is a required field." ></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="text-align: right; width: 147px;">Begin Date</td>
            <td style="text-align: left">
                <igsch:webdatechooser id="dteBeginDate" runat="server" tabindex="4" value="" width="112px">
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
            <td style="text-align: left">
                <asp:RequiredFieldValidator ID="reqfld_BeginDate" runat="server" ControlToValidate="dteBeginDate"
                    Display="Dynamic" ErrorMessage="* Begin Date is a required field." SetFocusOnError="True"></asp:RequiredFieldValidator><asp:RangeValidator
                        ID="rngValid_BeginDate" runat="server" ControlToValidate="dteBeginDate" Display="Dynamic"
                        ErrorMessage="* Begin Date - Value must be a valid date." Type="Date" Width="333px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="text-align: right; width: 147px;">End Date</td>
            <td style="text-align: left">
                <igsch:webdatechooser id="dteEndDate" runat="server" tabindex="5" width="112px">
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
            <td style="width: 318px; text-align: left">
                <asp:RequiredFieldValidator ID="reqfld_EndDate" runat="server" ControlToValidate="dteEndDate"
                    Display="Dynamic" ErrorMessage="* End Date is a required field." SetFocusOnError="True"></asp:RequiredFieldValidator><asp:RangeValidator
                        ID="rngValid_EndDate" runat="server" ControlToValidate="dteEndDate" Display="Dynamic"
                        ErrorMessage="* End Date - Value must be a valid date." SetFocusOnError="True"
                        Type="Date" Width="327px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="text-align: right; width: 147px;">Report Format</td>
            <td style="text-align: left;">
                <asp:DropDownList ID="cmbReportFormat" runat="server" TabIndex="3" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList><asp:Button ID="btnReport" runat="server" TabIndex="4"
                    Text="View Report" Width="100px" /></td>
            <td style="text-align: left;">
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: left">
                <asp:CompareValidator ID="comp_DateRange" runat="server" ControlToCompare="dteEndDate"
                    ControlToValidate="dteBeginDate" Display="Dynamic" ErrorMessage="* Invalid date range - End Date cannot be earlier than Begin Date."
                    Operator="LessThanEqual" Type="Date" Width="479px"></asp:CompareValidator></td>
            <td style="text-align: left">
            </td>
        </tr>
    </table>



</asp:Content>

