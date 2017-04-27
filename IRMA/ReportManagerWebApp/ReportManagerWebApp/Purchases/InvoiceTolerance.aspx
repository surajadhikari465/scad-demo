<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Purchases_InvoiceToleranceReport" Title="Invoice Tolerance Report" Codebehind="InvoiceTolerance.aspx.vb" %>

    <%@ register assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
        namespace="Infragistics.WebUI.WebSchedule" tagprefix="igsch" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <div id="navbar">
            <h3>
                <a href="..\Default.aspx">Home</a> &gt; Purchases &gt; Invoice Tolerance Report</h3>
        </div>
        <br />
        <div style="text-align: left">
            <table style="width: 524px; table-layout: fixed;">
            <!--
                <tr>
                    <td style="width: 117px" valign="top">
                        </td>
                    <td style="width: 258px">
                    </td>
                    <td valign="top">
                    </td>
                </tr>
                <tr>
                    <td style="width: 117px; height: 21px;" valign="top">
                        </td>
                    <td style="width: 258px; height: 21px;">
                     </td>
                    <td valign="top">
                    </td>
                </tr>-->
                <tr>
                    <td style="width: 117px; height: 9px;" valign="top">
                        Start Date:</td>
                    <td style="width: 258px; height: 9px;">
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
                    <td style="height: 9px" valign="top">
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage=" * Required" ControlToValidate="dtStartDate" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator></td>
                </tr>
                <tr>
                    <td style="width: 117px; height: 27px;" valign="top">
                        End Date:</td>
                    <td style="width: 258px; height: 27px;">
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
                    <td style="height: 27px" valign="top">
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="dtEndDate"
                            ErrorMessage=" * Required" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator></td>
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
