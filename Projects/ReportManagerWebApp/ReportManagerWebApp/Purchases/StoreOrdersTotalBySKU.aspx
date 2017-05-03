<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Purchases_StoreOrdersTotalBySKU" title="IRMA Report Manager" Codebehind="StoreOrdersTotalBySKU.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="PurchaseReports.aspx?valuePath=Reports Home/Purchases">Purchase Reports</a> 
        &gt; Store Orders Total By SKU</h3>
    </div>
    <table border="0" >

        <tr>
            <td style="width: 205px; height: 17px; text-align: right">
                Warehouse</td>
            <td style="width: 346px; height: 17px; text-align: left">
                <asp:DropDownList ID="cmbVendor" runat="server" DataSourceID="ICVendors" DataTextField="CompanyName"
                    DataValueField="Vendor_ID" Width="225px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICVendors" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT
                            Vendor.Vendor_ID, Vendor.CompanyName
                            FROM            Vendor WITH (nolock) 
                            LEFT OUTER JOIN  Store WITH (nolock) ON Vendor.Store_no = Store.Store_No
                            WHERE        (dbo.fn_GetCustomerType(Vendor.Store_no, Store.Internal, Store.BusinessUnit_ID) IN (2, 3)) AND (Store.Manufacturer = 1)">
                </asp:SqlDataSource>
            </td>
            <td style="width: 318px; height: 17px; text-align: left">
                </td>
        </tr>
        <tr>
            <td style="width: 205px; height: 10px; text-align: right">
                Team
            </td>
            <td style="width: 346px; height: 10px; text-align: left">
                <asp:DropDownList ID="cmbTeam" runat="server" DataSourceID="ICTeam" DataTextField="Team_Name"
                    DataValueField="Team_No" Width="225px" AutoPostBack="True">
                </asp:DropDownList><asp:SqlDataSource ID="ICTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                   SelectCommand="select Team_Name, Team_No from Team order by Team_Name">
<%--                    <SelectParameters>
                        <asp:ControlParameter ControlID="cmbTeam" DefaultValue="DBNull" Name="Team_No" PropertyName="SelectedValue" />
                    </SelectParameters>--%>
                    </asp:SqlDataSource>
            </td>
            <td style="width: 318px; height: 10px; text-align: left">
                </td>
        </tr>
        <tr>
            <td style="width: 205px; height: 10px; text-align: right">
                Subteam

            </td>
            <td style="width: 346px; height: 10px; text-align: left">
                <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="ICSubteam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" Width="225px" AutoPostBack="False">
                </asp:DropDownList><asp:SqlDataSource ID="ICSubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                     SelectCommand="select SubTeam_Name, SubTeam_No from subteam where team_no = ISNULL(@Team_No, team_no) order by subteam_name">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="cmbTeam" DefaultValue="DBNull" Name="Team_No" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 318px; height: 10px; text-align: left">
                </td>
        </tr>

        <tr>
            <td style="width: 205px; text-align: right">
                Starting Expected Date</td>
            <td>
                <igsch:WebDateChooser ID="dStartDate" runat="server" Value="" NullDateLabel="" >
                     <CalendarLayout ShowMonthDropDown="False" 
                        ShowYearDropDown="False" TitleFormat="Month">
                        <CalendarStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                            Font-Strikeout="False" Font-Underline="False" BackColor="#EFF6F8" 
                            BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" 
                            Font-Size="9pt" ForeColor="#404050">
                        </CalendarStyle>
                        <DayHeaderStyle BackColor="#9A98AE" Font-Bold="True" Font-Size="8pt" 
                            ForeColor="White" Height="1pt" />
                        <NextPrevStyle />
                        <OtherMonthDayStyle ForeColor="#888B90" />
                        <SelectedDayStyle BackColor="#888990" ForeColor="White" />
                        <TitleStyle BackColor="#D8E0E2"  
                            Font-Bold="True" Font-Size="10pt" ForeColor="#303040" Height="18pt" />
                        <TodayDayStyle BackColor="#D0D2D6" ForeColor="Black" />
                        <FooterStyle Font-Size="8pt" 
                            ForeColor="#707377" Height="16pt" />
                    </CalendarLayout>
                </igsch:WebDateChooser>
            </td>
            <td style="text-align: left">
                <asp:RequiredFieldValidator ID="reqfld_dStartDate" runat="server" ControlToValidate="dStartDate"
                    ErrorMessage="* Start Date is a required field." ></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 205px; text-align: right">
                Ending Expected Date</td>
            <td>
                <igsch:WebDateChooser ID="dEndDate" runat="server" NullDateLabel="">
                    <CalendarLayout ShowMonthDropDown="False" 
                        ShowYearDropDown="False" TitleFormat="Month">
                        <CalendarStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" 
                            Font-Strikeout="False" Font-Underline="False" BackColor="#EFF6F8" 
                            BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" 
                            Font-Size="9pt" ForeColor="#404050">
                        </CalendarStyle>
                        <DayHeaderStyle BackColor="#9A98AE" Font-Bold="True" Font-Size="8pt" 
                            ForeColor="White" Height="1pt" />
                        <NextPrevStyle />
                        <OtherMonthDayStyle ForeColor="#888B90" />
                        <SelectedDayStyle BackColor="#888990" ForeColor="White" />
                        <TitleStyle BackColor="#D8E0E2" 
                            Font-Bold="True" Font-Size="10pt" ForeColor="#303040" Height="18pt" />
                        <TodayDayStyle BackColor="#D0D2D6" ForeColor="Black" />
                        <FooterStyle Font-Size="8pt" 
                            ForeColor="#707377" Height="16pt" />
                    </CalendarLayout>
                </igsch:WebDateChooser>
            </td>
            <td colspan="3" style="text-align: left">
                <asp:RequiredFieldValidator ID="reqfld_dEndDate" runat="server" ControlToValidate="dEndDate"
                    ErrorMessage="* End Date is a required field." ></asp:RequiredFieldValidator>
             </td>
        </tr>
        <tr>
            <td style="width: 205px; text-align: right">
                Report Format</td>
             <td style="width: 346px; text-align: left"><asp:DropDownList ID="cmbReportFormat" runat="server" Font-Names="Verdana" Font-Size="10pt" Width="120px">
                <asp:ListItem>CSV</asp:ListItem>
                <asp:ListItem>EXCEL</asp:ListItem>
                <asp:ListItem Selected="True">HTML</asp:ListItem>
                <asp:ListItem>PDF</asp:ListItem>
                <asp:ListItem>XML</asp:ListItem>
            </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" Text="View Report"
                    Width="96px" /></td>
            <td style="width: 318px; text-align: left">
                &nbsp;</td>
        </tr>
    </table>
    <asp:CompareValidator id="cmp_BeginDateValidator" runat="server" Operator="DataTypeCheck" Type="Date" 
        ControlToValidate="dStartDate" ErrorMessage="* Start Date - Value must be a valid date."></asp:CompareValidator><br />    
    <asp:CompareValidator id="cmp_EndDateValidator" runat="server" Operator="DataTypeCheck" Type="Date" 
        ControlToValidate="dEndDate" ErrorMessage="* End Date - Value must be a valid date." ></asp:CompareValidator><br />  
    <asp:CompareValidator ID="cmp_txtStartDate" runat="server" ControlToCompare="dEndDate"
        ControlToValidate="dStartDate" 
        ErrorMessage="* The Start Date must be the same or earlier than the End Date" 
        Operator="LessThanEqual" Type="Date"></asp:CompareValidator><br />
    <asp:CompareValidator ID="cmp_txtEndDate" runat="server" ControlToCompare="dStartDate"
        ControlToValidate="dEndDate" 
        ErrorMessage="* The End Date must be the same or later than the Start Date" Operator="GreaterThanEqual"
        Type="Date"></asp:CompareValidator>
</asp:Content>

