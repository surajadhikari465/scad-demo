<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Sales_MovementFull" title="Report Manager - Full Movement" Codebehind="MovementFull.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="SalesReports.aspx?valuePath=Reports Home/Sales">Sales Reports</a> &gt; Movement - Full</h3>
    </div>

    <table style="width: 995px;">
        <tr>
            <td style="width: 127px; text-align: right;">
                Team</td>
            <td colspan="2">
                <asp:DropDownList ID="cmbTeam" runat="server" AutoPostBack="True" DataSourceID="ICTeam"
                    DataTextField="Team_Name" DataValueField="Team_No" Width="300px" TabIndex="1">
                </asp:DropDownList><asp:SqlDataSource ID="ICTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetTeams" SelectCommandType="StoredProcedure" DataSourceMode="DataReader"></asp:SqlDataSource>
            </td>
            <td colspan="3" style="text-align: left; width: 276px;">
                </td>
        </tr>
        <tr>
            <td style="width: 127px; text-align: right; height: 10px;">
                Subteam</td>
            <td colspan="2" style="height: 10px;">
                <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="ICSubteam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" Width="298px" AutoPostBack="True" TabIndex="2">
                </asp:DropDownList><asp:SqlDataSource ID="ICSubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT ST.SubTeam_No, ST.SubTeam_Name FROM SubTeam ST (NOLOCK) WHERE ST.Team_No = ISNULL(@Team_No, ST.Team_No)">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="cmbTeam" DefaultValue="DBNull" Name="Team_No" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td colspan="3" style="text-align: left; width: 276px; height: 10px;">
                &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 127px; height: 10px; text-align: right">
                Category</td>
            <td colspan="2" style="height: 10px">
                <asp:DropDownList ID="cmbCategory" runat="server" DataSourceID="ICCategory" DataTextField="Category_Name"
                    DataValueField="Category_ID" Width="299px" TabIndex="3">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICCategory" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT C.Category_ID, C.Category_Name FROM ItemCategory C (NOLOCK) WHERE C.SubTeam_No = ISNULL(@SubTeam_No, C.SubTeam_No) AND @Team_No > 0">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="cmbSubTeam" DefaultValue="" Name="SubTeam_No" PropertyName="SelectedValue"
                            Type="Int32" />
                        <asp:ControlParameter ControlID="cmbTeam" Name="Team_No" PropertyName="SelectedValue"
                            Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td colspan="3" style="width: 276px; height: 10px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 127px; text-align: right;">
                Begin Date</td>
            <td colspan="2"><igsch:WebDateChooser ID="dteBeginDate"
                    runat="server" Width="112px" Value="" TabIndex="4">
                <CalendarLayout DayNameFormat="FirstLetter" FooterFormat="" ShowFooter="False" ShowNextPrevMonth="False"
                    ShowTitle="False" ChangeMonthToDateClicked="True">
                    <SelectedDayStyle BackColor="#0054E3" />
                    <DayStyle BackColor="White" Font-Names="Arial" Font-Size="9pt" />
                    <OtherMonthDayStyle ForeColor="#ACA899" />
                    <DayHeaderStyle BackColor="#7A96DF" ForeColor="White" />
                    <TitleStyle BackColor="#9EBEF5" />
                    <CalendarStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False">
                    </CalendarStyle>
                </CalendarLayout>
            </igsch:WebDateChooser></td>
            <td colspan="3" style="text-align: left; width: 276px;">
                <asp:RequiredFieldValidator ID="reqfld_BeginDate" runat="server" ControlToValidate="dteBeginDate"
                    ErrorMessage="* Begin Date is a required field." SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
    <asp:RangeValidator ID="rngValid_BeginDate" runat="server" ControlToValidate="dteBeginDate"
        ErrorMessage="* Begin Date - Value must be a valid date." Type="Date" Display="Dynamic" Width="317px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 127px; text-align: right">
                End Datenal
            </td>
            <td colspan="2"><igsch:WebDateChooser ID="dteEndDate" runat="server" Width="112px" Value="" TabIndex="5">
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
            </igsch:WebDateChooser></td>
            <td colspan="3" style="text-align: left; width: 276px;">
                <asp:RequiredFieldValidator ID="reqfld_EndDate" runat="server" ControlToValidate="dteEndDate"
                    ErrorMessage="* End Date is a required field." SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
    <asp:RangeValidator ID="rngValid_EndDate" runat="server" ControlToValidate="dteEndDate"
        ErrorMessage="* End Date - Value must be a valid date." Type="Date" SetFocusOnError="True" Display="Dynamic" Width="310px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 127px;">
            </td>
            <td colspan="2" valign="top">
                <asp:Button ID="btnSetDate" runat="server" Text="Last Week" Width="104px" CausesValidation="False" ToolTip="Set dates for previous week" TabIndex="6" /></td>
            <td style="width: 276px;" colspan="3">
                <asp:CompareValidator ID="comp_DateRange" runat="server" ControlToCompare="dteEndDate"
                    ControlToValidate="dteBeginDate" Display="Dynamic" ErrorMessage="* Invalid date range - End Date cannot be earlier than Begin Date."
                    Operator="LessThanEqual"
                    Type="Date" Width="494px"></asp:CompareValidator></td>
        </tr>
        <tr>
            <td style="width: 127px">
            </td>
            <td colspan="2" valign="top">
            </td>
            <td colspan="3" style="width: 276px">
            </td>
        </tr>
        <tr>
            <td style="width: 127px; height: 21px">
            </td>
            <td colspan="2" style="height: 21px" valign="top">
                <strong><span style="color: blue; font-style: normal; font-variant: normal; text-decoration: underline">
                    Optional Parameters below</span></strong></td>
            <td colspan="3" style="width: 276px; height: 21px">
            </td>
        </tr>
        <tr>
            <td style="width: 127px">
            </td>
            <td colspan="2" valign="top">
            </td>
            <td colspan="3" style="width: 276px">
            </td>
        </tr>
        <tr>
            <td style="width: 127px">
                Identifier</td>
            <td colspan="2" valign="top">
                <asp:TextBox ID="textBoxIdentifier" runat="server"></asp:TextBox></td>
            <td colspan="3" style="width: 276px">
            </td>
        </tr>
        <tr>
            <td style="width: 127px">
                Item Description</td>
            <td colspan="2" valign="top">
                <asp:TextBox ID="textBoxItemDescription" runat="server"></asp:TextBox></td>
            <td colspan="3" style="width: 276px">
            </td>
        </tr>
        <tr>
            <td style="width: 127px; height: 24px">
            </td>
            <td colspan="2" style="height: 24px" valign="top">
            </td>
            <td colspan="3" style="width: 276px; height: 24px">
            </td>
        </tr>
        <tr>
            <td style="width: 127px; height: 60px">
                Brand(s)</td>
            <td colspan="2" style="height: 60px" valign="top">
                <asp:ListBox ID="listBoxBrands" runat="server"
                    DataSourceID="ICBrands" DataTextField="Brand_Name" DataValueField="Brand_ID" 
                    SelectionMode="Multiple" CausesValidation="True" Width="225px" Height="104px">
                </asp:ListBox>
                <asp:SqlDataSource ID="ICBrands" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetBrandAndID" SelectCommandType="StoredProcedure">
                </asp:SqlDataSource>
            </td>
            <td colspan="3" style="width: 276px; height: 60px">
            </td>
        </tr>
        <tr>
            <td style="width: 127px; height: 27px">
            </td>
            <td colspan="2" style="height: 27px" valign="top">
            </td>
            <td colspan="3" style="width: 276px; height: 27px">
            </td>
        </tr>
        <tr>
            <td style="width: 127px; height: 20px">
                Vendor(s)</td>
            <td colspan="2" style="height: 20px" valign="top">
                <asp:ListBox ID="listBoxVendors" runat="server" DataSourceID="ICVendors" DataTextField="CompanyName"
                    DataValueField="Vendor_ID" SelectionMode="Multiple" CausesValidation="True" Width="225px" Height="104px">
                </asp:ListBox>
                <asp:SqlDataSource ID="ICVendors" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetVendors" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td colspan="3" style="width: 276px; height: 20px">
            </td>
        </tr>
        <tr>
            <td style="width: 127px; height: 21px">
                Vendor Item ID</td>
            <td colspan="2" style="height: 21px" valign="top">
                <asp:TextBox ID="textBoxVendorItemID" runat="server"></asp:TextBox></td>
            <td colspan="3" style="width: 276px; height: 21px">
            </td>
        </tr>
        <tr>
            <td style="width: 127px; height: 21px">
            </td>
            <td colspan="2" style="height: 21px" valign="top">
            </td>
            <td colspan="3" style="width: 276px; height: 21px">
            </td>
        </tr>
        <tr>
            <td style="width: 127px; text-align: right;">
                Report Format</td>
            <td colspan="2">
                <asp:DropDownList ID="cmbReportFormat" runat="server" 
                    Width="104px" TabIndex="7">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" TabIndex="8"
                    Text="View Report" Width="104px" /></td>
            <td style="width: 276px" colspan="3">
            </td>
        </tr>
    </table>
</asp:Content>

