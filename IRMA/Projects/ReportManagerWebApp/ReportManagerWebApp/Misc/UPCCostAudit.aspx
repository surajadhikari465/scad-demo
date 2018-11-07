<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Misc_UPCCostAudit" title="Report Manager - UPC Cost Audit" Codebehind="UPCCostAudit.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>    

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="MiscReports.aspx?valuePath=Reports Home/Misc">Miscellaneous Reports</a> &gt; UPC Cost Audit</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 192px; text-align: right;">
                <br />
                Begin Date:&nbsp;
            </td>
            <td style="width: 260px; text-align: left">
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
            <td style="width: 260px; text-align: left;">
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
                <br />
                Team:&nbsp;
            </td>
            <td style="width: 260px; text-align: left">
                <br />
                <asp:DropDownList ID="cmbTeam" runat="server" Width="225px" DataSourceID="IC_Team" DataTextField="Team_Name" DataValueField="Team_No" Font-Names="Verdana" Font-Size="10pt" TabIndex="1" AutoPostBack="True">
                </asp:DropDownList><br />
                <asp:SqlDataSource ID="IC_Team" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="SELECT Team_Name, Team_No FROM Team ORDER BY Team_Name" ProviderName="<%$ ConnectionStrings:ReportManager_Conn.ProviderName %>"></asp:SqlDataSource>
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
                <br />
                SubTeam:&nbsp;
            </td>
            <td style="width: 260px; text-align: left">
                <br />
                <asp:DropDownList ID="cmbSubTeam" runat="server" Width="225px" DataSourceID="IC_SubTeam" DataTextField="SubTeam_Name" DataValueField="SubTeam_No" Font-Names="Verdana" Font-Size="10pt" TabIndex="1">
                </asp:DropDownList><br />
                <asp:SqlDataSource ID="IC_SubTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="SELECT SubTeam_Name, SubTeam_No FROM SubTeam WHERE Team_No = @TeamNo ORDER BY SubTeam_Name" ProviderName="<%$ ConnectionStrings:ReportManager_Conn.ProviderName %>">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="cmbTeam" Name="TeamNo" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
                <br />
                Vendor:&nbsp;
            </td>
            <td style="width: 260px; text-align: left">
                <br />
                <asp:DropDownList ID="cmbVendor" runat="server" Width="225px" DataSourceID="IC_Vendor" DataTextField="CompanyName" DataValueField="Vendor_Id" Font-Names="Verdana" Font-Size="10pt" TabIndex="1">
                </asp:DropDownList><br />
                <asp:SqlDataSource ID="IC_Vendor" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="SELECT CompanyName, Vendor_Id FROM Vendor ORDER BY CompanyName" ProviderName="<%$ ConnectionStrings:ReportManager_Conn.ProviderName %>"></asp:SqlDataSource>
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
                <br />
                Identifier List:&nbsp;
            </td>
            <td style="width: 260px; text-align: left">
                <span style="font-size: 10pt">
                    <br />
                    (Enter 1 per line, leave blank for all)<br />
                </span>
                <asp:TextBox ID="txtIdentifiers" runat="server" Height="194px" TextMode="MultiLine"
                    Width="218px"></asp:TextBox><br />
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
                <br />
                Report Format</td>
            <td style="width: 260px; text-align: left">
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
            <td style="width: 260px; text-align: left">
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
            </td>
            <td style="width: 260px; text-align: left">
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

