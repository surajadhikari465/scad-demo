<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Shrink_ShrinkReport" Title="Shrink Report" Codebehind="Shrink.aspx.vb" %>

    <%@ register assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
        namespace="Infragistics.WebUI.WebSchedule" tagprefix="igsch" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <div id="navbar">
            <h3>
                <a href="..\Default.aspx">Home</a> &gt; <a href="ShrinkReports.aspx?valuePath=Reports Home/Waste">
                    Shrink/Spoilage Reports</a> &gt; Shrink Report</h3>
        </div>
        <br />
        <div style="text-align: left">
            <table style="width: 524px; table-layout: fixed;">
                <tr>
                    <td style="width: 117px" valign="top">
                        Store:</td>
                    <td style="width: 258px">
                    <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                       Width="250px"  DataValueField="Store_No"></asp:DropDownList>
                    <asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                        SelectCommand="GetAllStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                    </td>
                    <td valign="top">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                        ControlToValidate="cmbStore" ErrorMessage=" * Required">*Required</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 117px; height: 21px;" valign="top">
                        SubTeam:</td>
                    <td style="width: 258px; height: 21px;">
                        &nbsp;<asp:ListBox ID="listBoxSubTeams" runat="server" DataSourceID="ICSubTeams" DataTextField="SubTeam_Name"
                            DataValueField="SubTeam_No" SelectionMode="Multiple"></asp:ListBox>
                        <asp:SqlDataSource ID="ICSubTeams" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                        SelectCommand="GetAllSubTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                     </td>
                    <td valign="top">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="listBoxSubTeams"
                        ErrorMessage=" * Required" InitialValue='' Visible="True"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 117px; height: 9px;" valign="top">
                        Start Date:</td>
                    <td style="width: 258px; height: 9px;">
                    <igsch:WebDateChooser ID="dtStartDate" runat="server" NullDateLabel="" Value="">
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
                    <td style="height: 9px" valign="top">
                   <asp:RequiredFieldValidator ID="vaStartDate" runat="server" 
                    ControlToValidate="dtStartDate" 
                    ErrorMessage="Start date required."  >* Required</asp:RequiredFieldValidator>                
                    </td>
                </tr>
                <tr>
                    <td style="width: 117px; height: 27px;" valign="top">
                        End Date:</td>
                    <td style="width: 258px; height: 27px;">
                    <igsch:WebDateChooser ID="dtEndDate" runat="server" NullDateLabel="" Value="">
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
                    <td style="height: 27px" valign="top">
                   <asp:RequiredFieldValidator ID="vaEndDate" runat="server" 
                    ControlToValidate="dtendDate" 
                    ErrorMessage="Start date required."  >* Required</asp:RequiredFieldValidator>
                                    </td>
               
                </tr>
                <tr>
                    <td style="width: 117px" valign="top">
                        Report Format:</td>
                    <td style="width: 258px">
                    <asp:DropDownList ID="cmbReportFormat" runat="server" Width="94px">
                        <asp:ListItem>CSV</asp:ListItem>
                        <asp:ListItem>EXCEL</asp:ListItem>
                        <asp:ListItem Selected="True">HTML</asp:ListItem>
                        <asp:ListItem>PDF</asp:ListItem>
                        <asp:ListItem>XML</asp:ListItem>
                    </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 117px" valign="top">
                        </td>
                    <td style="width: 258px">
                    <asp:Button ID="btnReport" runat="server" Text="View Report" />
                    </td>
                </tr>
            </table>
        </div>
        <br />
    </asp:Content>
