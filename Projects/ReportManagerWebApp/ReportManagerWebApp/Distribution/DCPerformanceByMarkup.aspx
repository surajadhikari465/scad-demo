<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"  Inherits="ReportManagerWebApp.Distribution_DCPerformanceByMarkup" Codebehind="DCPerformanceByMarkup.aspx.vb" %>

<%@ register assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB" namespace="Infragistics.WebUI.WebSchedule" tagprefix="igsch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="DistributionReports.aspx?valuePath=Reports Home/Distribution">Distrubution Reports</a> &gt; DC Performance By Markup Dollars</h3>
</div>
    <table border="0">
        <tr>
            <td style="width: 152px; height: 1px; text-align: right">Facility</td>
            <td style="width: 458px; height: 1px; text-align: left">
                <asp:DropDownList ID="cmbFacility" runat="server" DataSourceID="dsFacility" DataTextField="Label" DataValueField="Value" Width="225px"></asp:DropDownList>
                <asp:SqlDataSource ID="dsFacility" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="
                                   SELECT 
	                                vendor_ID AS 'Value', 
	                                companyname AS Label
                                    FROM 
	                                vendor v (nolock)
                                    JOIN  STORE s (nolock) ON s.store_no = v.store_no
                                    WHERE 
	                                distribution_center = 1
                                    ORDER BY 
	                                companyname
                                  ">
                </asp:SqlDataSource>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
        <tr>
            <td style="width: 152px; height: 1px; text-align: right">Subteam</td>
            <td style="width: 458px; height: 1px; text-align: left">
                 <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="dsSubteam" DataTextField="Label" DataValueField="Value" Width="225px"></asp:DropDownList>
                <asp:SqlDataSource ID="dsSubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="
                                 SELECT SubTeam_Name AS Label, SubTeam_No AS 'Value'
                                   FROM SubTeam (NOLOCK)
                                   UNION
                                   SELECT '*All SubTeams*' AS Label, NULL AS 'Value'
                                   ORDER BY SubTeam_No

                                  ">
                </asp:SqlDataSource>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
    
        <tr>
            <td style="width: 152px; height: 1px; text-align: right"><a title="Order Close Date - Start" style="cursor:help">Start Date</a></td>
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
            <td style="width: 152px; height: 1px; text-align: right"><a title="Order Close Date - End" style="cursor:help">End Date</a></td>
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