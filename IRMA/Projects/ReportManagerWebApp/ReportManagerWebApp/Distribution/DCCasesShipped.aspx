<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Distribution_DCCasesShipped" title="Report Manager - DC Cases Shipped" Codebehind="DCCasesShipped.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="DistributionReports.aspx?valuePath=Reports Home/Distribution">Distribution Reports</a> &gt; DC Cases Shipped</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 192px; text-align: right;">
                <br />
                DC Vendor:&nbsp;
            </td>
            <td style="width: 249px; text-align: left;">
                <br />
                <asp:DropDownList ID="cmbDCVendor" runat="server" Width="225px" DataSourceID="IC_DC_Vendor" DataTextField="CompanyName" DataValueField="Vendor_Id" Font-Names="Verdana" Font-Size="10pt" TabIndex="1">
                </asp:DropDownList><asp:SqlDataSource ID="IC_DC_Vendor" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="SELECT * FROM Vendor WHERE EXEWarehouseVendSent = 1 AND EXEWarehouseCustSent = 1"></asp:SqlDataSource></td>
            <td style="width: 318px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbStore" runat="server" ControlToValidate="cmbDCVendor"
                    ErrorMessage="* Store is a required field." MaximumValue="2147483647" 
                    MinimumValue="1" SetFocusOnError="True"
                    Type="Integer" Width="201px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right;">
                <br />
                Begin Date:&nbsp;
            </td>
            <td style="width: 249px; text-align: left">
                <br />
                <igsch:webdatechooser id="dteBeginDate" runat="server" value="" width="112px" TabIndex="4">
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
                <asp:RequiredFieldValidator ID="reqfld_BeginDate" runat="server" ControlToValidate="dteBeginDate"
                    Display="Dynamic" ErrorMessage="* Begin Date is a required field."
                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:RangeValidator
                        ID="rngValid_BeginDate" runat="server" ControlToValidate="dteBeginDate" Display="Dynamic"
                        ErrorMessage="* Begin Date - Value must be a valid date." 
                        Type="Date" Width="333px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right;">
                <br />
                End Date:&nbsp;
            </td>
            <td style="width: 249px; text-align: left;">
                <br />
                <igsch:WebDateChooser ID="dteEndDate" runat="server" Width="112px" TabIndex="5">
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
            <td style="width: 318px; text-align: left">
                <asp:RequiredFieldValidator ID="reqfld_EndDate" runat="server" ControlToValidate="dteEndDate"
                    Display="Dynamic" ErrorMessage="* End Date is a required field." 
                    SetFocusOnError="True"></asp:RequiredFieldValidator><asp:RangeValidator
                        ID="rngValid_EndDate" runat="server" ControlToValidate="dteEndDate" Display="Dynamic"
                        ErrorMessage="* End Date - Value must be a valid date." 
                        SetFocusOnError="True" Type="Date" Width="327px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
                Group SubTeam By:&nbsp;
            </td>
            <td style="width: 346px; text-align: left">
                <br />
                <asp:RadioButton ID="optFromSubTeam" runat="server" GroupName="SubTeam"
                    Text="From SubTeam" Checked="True" /><br />
                <asp:RadioButton ID="optToSubTeam" runat="server" GroupName="SubTeam" Text="To SubTeam" /><br />
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
                <br />
                Report Format:&nbsp;
            </td>
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
                <asp:CompareValidator ID="comp_DateRange" runat="server" ControlToCompare="dteEndDate"
                    ControlToValidate="dteBeginDate" Display="Dynamic" ErrorMessage="* Invalid date range - End Date cannot be earlier than Begin Date."
                    Operator="LessThanEqual"
                    Type="Date" Width="479px"></asp:CompareValidator>
    </asp:Content>

