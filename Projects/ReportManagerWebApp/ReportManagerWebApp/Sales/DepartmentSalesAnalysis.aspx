<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.DepartmentSalesAnalysis" title="Department Sales Analysis" Codebehind="DepartmentSalesAnalysis.aspx.vb" %>

<%@ register assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB" namespace="Infragistics.WebUI.WebSchedule" tagprefix="igsch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="SalesReports.aspx?valuePath=Reports Home/Sales">Sales Reports</a> &gt; Department Sales Analysis</h3>
</div>
    <table border="0">
         <tr>
            <td style="width: 150px; text-align: right"><a title="Result Sort" style="cursor:help">Sort By</a></td>
             <td style="width: 346px; text-align: left">
                <asp:DropDownList ID="cmbSortBy" runat="server" Font-Names="Verdana" Font-Size="10pt" Width="120px">
                    <asp:ListItem Selected="True">TopSales</asp:ListItem>
                    <asp:ListItem>TopUnits</asp:ListItem>
                    <asp:ListItem>TopMargin</asp:ListItem>
                    <asp:ListItem>BottomSales</asp:ListItem>
                    <asp:ListItem>BottomUnits</asp:ListItem>
                    <asp:ListItem>BottomMargin</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 318px; text-align: left"></td>
        </tr>
        <tr>
            <td style="width: 138px; height: 1px; text-align: right">
                Zone&nbsp;</td>
            <td style="width: 443px; height: 1px; text-align: left">
                <asp:DropDownList ID="cmbZone" runat="server" AutoPostBack="True" DataSourceID="ICZone"
                    DataTextField="Zone_Name" DataValueField="Zone_ID" Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="ICZone" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT Zone_ID, Zone_Name FROM Zone ORDER BY Zone_Name "></asp:SqlDataSource>
                &nbsp;
            </td>
            <td style="width: 147px; height: 1px; text-align: left">
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="cmbZone"
                    ErrorMessage="*A Zone selection is required" Operator="GreaterThan" Type="Integer"
                    ValueToCompare="0" Width="251px"></asp:CompareValidator></td>
        </tr>
        <tr>
            <td style="width: 138px; height: 1px; text-align: right">
                Store&nbsp;
            </td>
            <td style="width: 443px; height: 1px; text-align: left">
                <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="ICStore" DataTextField="Store_Name"
                    DataValueField="Store_No" Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="ICStore" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT [Store_No], [Store_Name] FROM [Store] WHERE ([Regional] = @Regional) ORDER BY [Store_Name]">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="False" Name="Regional" Type="Boolean" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 147px; height: 1px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 152px; height: 1px; text-align: right">SubTeam</td>
            <td style="width: 458px; height: 1px; text-align: left">
                <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="ICSubTeam" DataTextField="SubTeamName" DataValueField="SubTeamNo" Width="225px"></asp:DropDownList>
                <asp:SqlDataSource ID="ICSubTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="
                                    SELECT DISTINCT
	                                    [SubTeamName]  = st.SubTeam_Name,
	                                    [SubTeamNo]    = st.SubTeam_No
                                    FROM
	                                    SubTeam	st  (nolock)

                                    UNION

                                    SELECT
                                        [SubTeamName]  = 'All SubTeams',
                                        [SubTeamNo]    = 0

                                    ORDER BY SubTeamNo ASC
                                  ">
                </asp:SqlDataSource>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
        <tr>
            <td style="width: 152px; height: 1px; text-align: right"><a title="Maxiumum Results Returned" style="cursor:help">Results</a></td>
            <td style="width: 458px; height: 1px; text-align: left">
                <asp:TextBox ID="txtResults" runat="server" Width="136px" Text="100"></asp:TextBox>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
        <tr>
            <td style="width: 152px; height: 1px; text-align: right"><a title="Item Identifier" style="cursor:help">Identifier</a></td>
            <td style="width: 458px; height: 1px; text-align: left">
                <asp:TextBox ID="txtIdentifier" runat="server" Width="136px"></asp:TextBox>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
        <tr>
            <td style="width: 152px; height: 1px; text-align: right"><a title="Sales Date - Start" style="cursor:help">Start Date</a></td>
            <td style="width: 458px; height: 1px; text-align: left">
                <igsch:WebDateChooser ID="dtStartDate" runat="server" NullDateLabel="< Enter Date >" Value="">
                    <CalendarLayout NextMonthImageUrl="~/images/ig_cal_blueN0.gif" PrevMonthImageUrl="~/images/ig_cal_blueP0.gif"
                        ShowMonthDropDown="False" ShowYearDropDown="False" TitleFormat="Month">
                        <TodayDayStyle BackColor="#E0EEFF" />
                        <FooterStyle BackgroundImage="~/images/ig_cal_blue1.gif" Font-Size="8pt" ForeColor="#505080"
                            Height="16pt">
                            <BorderDetails ColorTop="LightSteelBlue" StyleTop="Solid" WidthTop="1px" />
                        </FooterStyle>
                        <SelectedDayStyle BackColor="SteelBlue" />
                        <NextPrevStyle BackgroundImage="~/images/ig_cal_blue2.gif" />
                        <OtherMonthDayStyle ForeColor="SlateGray" />
                        <DayHeaderStyle BackColor="#E0EEFF" Font-Bold="True" Font-Size="8pt" ForeColor="#8080A0"
                            Height="1pt">
                            <BorderDetails ColorBottom="LightSteelBlue" StyleBottom="Solid" WidthBottom="1px" />
                        </DayHeaderStyle>
                        <TitleStyle BackColor="#CCDDFF" BackgroundImage="~/images/ig_cal_blue2.gif" Font-Bold="True"
                            Font-Size="10pt" ForeColor="#505080" Height="18pt" />
                        <CalendarStyle BackColor="#CCDDFF" BorderColor="SteelBlue" BorderStyle="Solid" BorderWidth="1px"
                            Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False"
                            Font-Size="9pt" Font-Strikeout="False" Font-Underline="False">
                        </CalendarStyle>
                    </CalendarLayout>
                </igsch:WebDateChooser>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
        <tr>
            <td style="width: 152px; height: 1px; text-align: right"><a title="Sales Date - End" style="cursor:help">End Date</a></td>
            <td style="width: 458px; height: 1px; text-align: left">
                <igsch:WebDateChooser ID="dtEndDate" runat="server" NullDateLabel="< Enter Date >" Value="">
                    <CalendarLayout NextMonthImageUrl="~/images/ig_cal_blueN0.gif" PrevMonthImageUrl="~/images/ig_cal_blueP0.gif"
                        ShowMonthDropDown="False" ShowYearDropDown="False" TitleFormat="Month">
                        <TodayDayStyle BackColor="#E0EEFF" />
                        <FooterStyle BackgroundImage="~/images/ig_cal_blue1.gif" Font-Size="8pt" ForeColor="#505080"
                            Height="16pt">
                            <BorderDetails ColorTop="LightSteelBlue" StyleTop="Solid" WidthTop="1px" />
                        </FooterStyle>
                        <SelectedDayStyle BackColor="SteelBlue" />
                        <NextPrevStyle BackgroundImage="~/images/ig_cal_blue2.gif" />
                        <OtherMonthDayStyle ForeColor="SlateGray" />
                        <DayHeaderStyle BackColor="#E0EEFF" Font-Bold="True" Font-Size="8pt" ForeColor="#8080A0"
                            Height="1pt">
                            <BorderDetails ColorBottom="LightSteelBlue" StyleBottom="Solid" WidthBottom="1px" />
                        </DayHeaderStyle>
                        <TitleStyle BackColor="#CCDDFF" BackgroundImage="~/images/ig_cal_blue2.gif" Font-Bold="True"
                            Font-Size="10pt" ForeColor="#505080" Height="18pt" />
                        <CalendarStyle BackColor="#CCDDFF" BorderColor="SteelBlue" BorderStyle="Solid" BorderWidth="1px"
                            Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False"
                            Font-Size="9pt" Font-Strikeout="False" Font-Underline="False">
                        </CalendarStyle>
                    </CalendarLayout>
                </igsch:WebDateChooser>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
         <tr>
            <td style="width: 150px; text-align: right">Report Format</td>
             <td style="width: 346px; text-align: left">
                <asp:DropDownList ID="cmbReportFormat" runat="server" Font-Names="Verdana" Font-Size="10pt" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" Text="View Report" Width="96px" />
            </td>
            <td style="width: 318px; text-align: left"></td>
        </tr>
        </table>
</asp:Content>

