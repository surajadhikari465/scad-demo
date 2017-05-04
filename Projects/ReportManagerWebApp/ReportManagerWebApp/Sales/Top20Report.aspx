<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Sales_Top20Report" title="Untitled Page" Codebehind="Top20Report.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="SalesReports.aspx?valuePath=Reports Home/Sales">Sales Report</a> &gt; Top 20 Report</h3>
    </div>
    
    <asp:Table runat=server ID="table1">
        <asp:TableRow>
            <asp:TableCell>
                Sub Team:
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="ICSubTeam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICSubTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="Reporting_GetSubTeams" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Name="blnAll" DefaultValue="True" Type="boolean" />
                    </SelectParameters>
                    </asp:SqlDataSource>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                Store:
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="cmbStores" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                    DataValueField="Store_No">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="Reporting_GetStores" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Name="blnAll" DefaultValue="True" Type="boolean" />
                    </SelectParameters>
                    </asp:SqlDataSource>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                Sale Start Date:
            </asp:TableCell>
            <asp:TableCell>
                <igsch:WebDateChooser ID="dtStartDate" runat="server">
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
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                Sale End Date:
            </asp:TableCell>
            <asp:TableCell>
                <igsch:WebDateChooser ID="dtEndDate" runat="server">
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
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                Report Format:
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="cmbReportFormat" runat="server" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                &nbsp;
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button ID="btnReport" runat="server" Text="View Report" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    </asp:Content>

