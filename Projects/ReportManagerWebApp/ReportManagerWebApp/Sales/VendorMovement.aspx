<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Sales_StoreVendorMovement" title="Store Vendor Movement" Codebehind="VendorMovement.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="SalesReports.aspx?valuePath=Reports Home/Sales">Sales Reports</a> &gt; Vendor Movement</h3>
    </div>
    <table style="width: 842px;">
        <tr>
            <td style="width: 259px; text-align: right">
                Top</td>
            <td colspan="2">
                <asp:RadioButton ID="rdbtnTop" runat="server" Checked="True" GroupName="order" TabIndex="1" /></td>
            <td colspan="3" style="width: 280px">
            </td>
        </tr>
        <tr>
            <td style="width: 259px; text-align: right">
                Bottom</td>
            <td colspan="2">
                <asp:RadioButton ID="rdbtnBottom" runat="server" GroupName="order" TabIndex="2" /></td>
            <td colspan="3" style="width: 280px">
            </td>
        </tr>
        <tr>
            <td style="width: 259px; text-align: right">
                Number of Results</td>
            <td colspan="2">
                <asp:TextBox ID="txtResults" runat="server" TabIndex="3"></asp:TextBox></td>
            <td colspan="3" style="width: 280px">
                <asp:RequiredFieldValidator ID="reqfld_txtResults" runat="server" ControlToValidate="txtResults"
                    ErrorMessage="* Number of results is a required field." ValidationGroup="submit" Width="282px"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 259px; text-align: right">
                Zone</td>
            <td colspan="2">
                <asp:DropDownList ID="cmbZone" runat="server" DataSourceID="ICZones" DataTextField="Zone_Name"
                    DataValueField="Zone_id" Height="24px"
                    TabIndex="4" Width="222px">
                </asp:DropDownList><asp:SqlDataSource ID="ICZones" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetZones" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td colspan="3" style="width: 280px">
                <asp:RequiredFieldValidator ID="reqfld_cmbZone" runat="server" ControlToValidate="cmbZone"
                    ErrorMessage="* Zone is a required field." ValidationGroup="submit"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 259px; text-align: right">
                Store</td>
            <td colspan="2">
                <asp:DropDownList ID="cmbStore" runat="server" Height="24px"
                    Width="220px" TabIndex="5">
                </asp:DropDownList><asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetAllStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td colspan="3" style="width: 280px">
                <asp:RequiredFieldValidator ID="reqfld_cmbStore" runat="server" ControlToValidate="cmbStore"
                    ErrorMessage="* Store is a required field." ></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 259px; height: 35px; text-align: right">
    Search by</td>
            <td colspan="2" style="height: 35px">
                <asp:RadioButtonList ID="rdbtn_SearchType" runat="server" Width="250px" TabIndex="6">
                    <asp:ListItem Selected="True" Value="1">Company Name</asp:ListItem>
                    <asp:ListItem Value="2">PS Vendor ID</asp:ListItem>
                    <asp:ListItem Value="3">Vendor ID</asp:ListItem>
                </asp:RadioButtonList></td>
            <td colspan="3" style="width: 280px; height: 35px">
            </td>
        </tr>
        <tr>
            <td style="width: 259px; height: 17px; text-align: right">
                Vendor Selection</td>
            <td colspan="2" style="height: 17px">
                <asp:TextBox ID="txtSearch" runat="server" MaxLength="60" Width="175px" TabIndex="7"></asp:TextBox>
                <asp:Button ID="btnGetVendors" runat="server" Text="Search" Width="100px" TabIndex="8" /><br />
                please enter a search string</td>
            <td colspan="3" style="width: 280px; height: 17px">
            </td>
        </tr>
        <tr>
            <td style="width: 259px; text-align: right">
            </td>
            <td colspan="2">
                <asp:ListBox ID="lstVendors" runat="server" Height="149px" Width="393px" TabIndex="9"></asp:ListBox><br />
                <asp:Label ID="lblVendorCount" runat="server" ForeColor="Red"></asp:Label></td>
            <td colspan="3" style="width: 280px">
            </td>
        </tr>
        <tr>
            <td style="width: 259px; text-align: right">
                Subteam</td>
            <td colspan="2">
                <asp:DropDownList ID="cmbSubteam" runat="server" TabIndex="10"
                    Width="279px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICSubteams" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetAllSubTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td colspan="3" style="width: 280px">
                <asp:RequiredFieldValidator ID="reqfld_cmbSubteam" runat="server" ControlToValidate="cmbSubteam"
                    ErrorMessage="* Subteam is a required field." ></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 259px; text-align: right;">
                Begin Date</td>
            <td colspan="2">
                <igsch:WebDateChooser ID="dteBegin"
                    runat="server" Width="112px" Value="" TabIndex="11">
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
            <td colspan="3" style="width: 280px;">
                <asp:RequiredFieldValidator ID="reqfld_dteBegin" runat="server" ControlToValidate="dteBegin"
                    ErrorMessage="* Begin Date is a required field." ValidationGroup="submit"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>

            <td style="width: 259px; text-align: right;">
                End Date</td>
            <td colspan="2">
                <igsch:WebDateChooser ID="dteEnd" runat="server" Width="112px" TabIndex="12">
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
            <td colspan="3" style="width: 280px">
                <asp:RequiredFieldValidator ID="reqfld_dteEnd" runat="server" ControlToValidate="dteEnd"
                    ErrorMessage="* End Date is a required field." ValidationGroup="submit"></asp:RequiredFieldValidator>
                    </td>

        </tr>
        <tr>

            <td style="width: 259px; text-align: right;">
                Report Format</td>
            <td colspan="5">
                <asp:DropDownList ID="cmbReportFormat" runat="server" Width="120px" TabIndex="13">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" TabIndex="14"
                    Text="View Report" Width="100px" ValidationGroup="submit" /></td>

        </tr>
    </table>
    <asp:RangeValidator ID="rngValid_txtResults" runat="server" ControlToValidate="txtResults"
        ErrorMessage="* Number of Results - Must be a positive integer." MaximumValue="999999999" MinimumValue="1"
        Type="Integer" ValidationGroup="submit"></asp:RangeValidator><br />
    <asp:CompareValidator id="cmp_BeginDateValidator" runat="server" Operator="DataTypeCheck" Type="Date" 
        ControlToValidate="dteBegin" ErrorMessage="* Begin Date - Value must be a valid date."  
        ValidationGroup="submit"></asp:CompareValidator><br />    
    <asp:CompareValidator id="cmp_EndDateValidator" runat="server" Operator="DataTypeCheck" Type="Date" 
        ControlToValidate="dteEnd" ErrorMessage="* End Date - Value must be a valid date."  
        ValidationGroup="submit"></asp:CompareValidator><br />  
    <asp:CompareValidator ID="cmp_txtBeginDate" runat="server" ControlToCompare="dteEnd"
        ControlToValidate="dteBegin" ErrorMessage="* Begin Date must be prior to End Date"
        Operator="LessThan" Type="Date" ValidationGroup="submit"></asp:CompareValidator><br />
    <asp:CompareValidator ID="cmp_txtEndDate" runat="server" ControlToCompare="dteBegin"
        ControlToValidate="dteEnd" ErrorMessage="* End Date must be after Begin Date"
        Operator="GreaterThan"
        Type="Date" ValidationGroup="submit"></asp:CompareValidator><br />
    <br />
    <asp:SqlDataSource ID="ICVendors_CompanyName" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
        SelectCommand="GetVendor_ByCompanyName" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtSearch" Name="Find" PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="ICVendors_PSVendorID" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
        SelectCommand="GetVendor_ByPSVendorID" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtSearch" Name="Find" PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="ICVendors_VendorID" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
        SelectCommand="GetVendor_ByVendorID" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtSearch" Name="Find" PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>        
</asp:Content>

