<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Specials" title="Report Manager - Specials" Codebehind="Specials.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="PriceReports.aspx?valuePath=Reports Home/Prices">Price Reports</a> &gt; All Specials</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 192px; text-align: right;">
                Store</td>
            <td style="width: 249px; text-align: left;"><asp:DropDownList ID="cmbStore" runat="server" Width="225px" DataSourceID="ICStores" DataTextField="Store_Name" DataValueField="Store_No" Font-Names="Verdana" Font-Size="10pt" TabIndex="1">
                </asp:DropDownList><asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="GetAllStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource></td>
            <td style="width: 318px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbStore" runat="server" ControlToValidate="cmbStore"
                    ErrorMessage="* Store is a required field." MaximumValue="2147483647" 
                    MinimumValue="1" SetFocusOnError="True"
                    Type="Integer" Width="201px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
                Team

            </td>
            <td style="width: 249px; text-align: left">
                <asp:DropDownList ID="cmbTeam" runat="server" AutoPostBack="True" DataSourceID="ICTeam"
                    DataTextField="Team_Name" DataValueField="Team_No" 
                    Width="225px" TabIndex="2">
                </asp:DropDownList><asp:SqlDataSource ID="ICTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
                Subteam

            </td>
            <td style="width: 249px; text-align: left">
                <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="ICSubteam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" Width="225px" TabIndex="3">
                </asp:DropDownList><asp:SqlDataSource ID="ICSubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT ST.SubTeam_No, ST.SubTeam_Name FROM SubTeam ST (NOLOCK) WHERE ST.Team_No = ISNULL(@Team_No, ST.Team_No)">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="cmbTeam" DefaultValue="DBNull" Name="Team_No" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right;">
                Begin Date</td>
            <td style="width: 249px; text-align: left">
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
                End Date</td>
            <td style="width: 249px; text-align: left;">
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
            <td style="width: 192px; text-align: right; height: 25px;">
                Use End Date</td>
            <td style="width: 249px; text-align: left; height: 25px;"><asp:CheckBox ID="chkEndDate" runat="server" Width="187px" TabIndex="6" /></td>
            <td style="width: 318px; text-align: left; height: 25px;">
            </td>
        </tr>        
        <tr>
            <td style="width: 192px; text-align: right; height: 25px;">
                Include Ongoing Specials</td>
            <td style="width: 249px; text-align: left; height: 25px;"><asp:CheckBox ID="chkIncludeOngoing" runat="server" Width="187px" TabIndex="7" /></td>
            <td style="width: 318px; text-align: left; height: 25px;">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
                Report Format</td>
            <td style="width: 346px; text-align: left">
                <asp:DropDownList ID="cmbReportFormat" runat="server" Width="120px" TabIndex="8">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" Text="View Report"
                    Width="96px" TabIndex="9" /></td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>      
    </table>
                <asp:CompareValidator ID="comp_DateRange" runat="server" ControlToCompare="dteEndDate"
                    ControlToValidate="dteBeginDate" Display="Dynamic" ErrorMessage="* Invalid date range - End Date cannot be earlier than Begin Date."
                    Operator="LessThanEqual"
                    Type="Date" Width="479px"></asp:CompareValidator>
    </asp:Content>

