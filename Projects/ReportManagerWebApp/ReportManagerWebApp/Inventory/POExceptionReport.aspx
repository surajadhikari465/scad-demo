<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Inventory_POExceptionReport" Title="PO Exception Report" Codebehind="POExceptionReport.aspx.vb" %>

<%@ register assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB" namespace="Infragistics.WebUI.WebSchedule" tagprefix="igsch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="InventoryReports.aspx?valuePath=Reports Home/Inventory">Inventory Reports</a> &gt; PO Exception Report</h3>
</div>
    <table border="0">
        <tr>
            <td style="width: 152px; height: 1px; text-align: right">Store</td>
            <td style="width: 458px; height: 1px; text-align: left">
                <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="ICStore" DataTextField="Store_Name" DataValueField="BusinessUnit_ID" Width="225px"></asp:DropDownList>
                <asp:SqlDataSource ID="ICStore" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="
                                    SELECT
	                                    BusinessUnit_ID,
	                                    Store_Name
                                    FROM
	                                    Store
                                    WHERE
	                                    WFM_Store = 1
	                                    OR Mega_Store = 1
	                                    OR Distribution_Center = 1
                                  ">
                </asp:SqlDataSource>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
        <tr>
            <td style="width: 152px; height: 1px; text-align: right">Vendor</td>
            <td style="width: 458px; height: 1px; text-align: left">
                <asp:DropDownList ID="cmbVendor" runat="server" DataSourceID="ICVendors" DataTextField="CompanyName" DataValueField="PS_Export_Vendor_ID" Width="225px"></asp:DropDownList>
                <asp:SqlDataSource ID="ICVendors" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="
                                    SELECT 
                                        CompanyName, 
                                        PS_Export_Vendor_ID 
                                    FROM 
                                        Vendor 
                                    ORDER BY 
                                        CompanyName
                                  ">
                </asp:SqlDataSource>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
        <tr>
            <td style="width: 152px; height: 1px; text-align: right">SubTeam</td>
            <td style="width: 458px; height: 1px; text-align: left">
                <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="ICSubTeam" DataTextField="IRMASubTeam" DataValueField="PSSubTeam" Width="225px"></asp:DropDownList>
                <asp:SqlDataSource ID="ICSubTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="
                                    SELECT 
                                        [PSSubTeam]	    =	st.SubTeam_No, 
                                        [IRMASubTeam]	=	st.SubTeam_Name
                                    FROM
                                        SubTeam			        (nolock) st
                                        INNER JOIN StoreSubTeam	(nolock) sst	ON	st.SubTeam_No = sst.SubTeam_No
                                    WHERE
                                        sst.PS_SubTeam_No IS NOT NULL
                                    GROUP BY
                                        st.SubTeam_No,
                                        st.SubTeam_Name	
                                    ORDER BY
                                        st.SubTeam_No
                                  ">
                </asp:SqlDataSource>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
        <tr>
            <td style="width: 152px; height: 1px; text-align: right"><a title="Maxiumum System Amount Tolerance" style="cursor:help">Tolerance</a></td>
            <td style="width: 458px; height: 1px; text-align: left">
                <asp:TextBox ID="txtTolerance" runat="server" Width="136px"></asp:TextBox>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
        <tr>
            <td style="width: 152px; height: 1px; text-align: right"><a title="Minimum Invoice Amount" style="cursor:help">Minimum Amount</a></td>
            <td style="width: 458px; height: 1px; text-align: left">
                <asp:TextBox ID="txtMinAmount" runat="server" Width="136px"></asp:TextBox>
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

