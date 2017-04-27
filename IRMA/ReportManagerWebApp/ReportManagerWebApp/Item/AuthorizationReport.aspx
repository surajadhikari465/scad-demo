<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Item_AuthorizationReport" Title="Authorization Report" Codebehind="AuthorizationReport.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="navbar">
        <h3>
            <a href="..\Default.aspx">Home</a> &gt; <a href="ItemReports.aspx?valuePath=Reports Home/Items">
                Item Reports</a> &gt; Authorization Report</h3>
    </div>
    <asp:Table runat="server" ID="table1">
        <asp:TableRow>
            <asp:TableCell>
                Price Type:
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="cmbPriceType" runat="server" DataSourceID="ICPriceType" DataTextField="PriceChgTypeDesc"
                    DataValueField="PriceChgTypeId">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICPriceType" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetPriceTypes" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Name="IncludeReg" DefaultValue="True" Type="boolean" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                Price Start Date:
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
                Price End Date (leave as 'Null' if searching for REG price items):
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
                SubTeam (hold down ctrl key to select multiple values):
            </asp:TableCell>
            <asp:TableCell>
                <asp:ListBox ID="cmbSubTeam" runat="server" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" DataSourceID="ICSubTeams" SelectionMode="multiple"></asp:ListBox>
                <asp:SqlDataSource ID="ICSubTeams" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetSubTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
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
