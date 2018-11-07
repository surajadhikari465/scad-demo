<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Sales_CommodityCodeDateRanges" title="Commodity Code Date Range Compare" Codebehind="CommodityCodeDateRanges.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><a href="..\Default.aspx"></a> 
        <h3><a href="..\Default.aspx">Home</a>&gt; <a href="SalesReports.aspx?valuePath=Reports Home/Sales">Sales Reports</a>&gt; Commodity Code Date Range Comparison</h3>
    </div>
    <script language="javascript">
    function valueChangingMonday(owner, date, evt)
    {
        var elem = ig_csom.getElementById('<%=lblMovement.ClientId%>').innerText;
        if( elem == "1" )
        {
            var dayOfWeek = date.getDay();
            if(dayOfWeek != 1) evt.cancel = true;
        }               
    }
    
    function valueChangingSunday(owner, date, evt)
    {
        var elem = ig_csom.getElementById('<%=lblMovement.ClientId%>').innerText;
        if( elem == "1" )
        {
            var dayOfWeek = date.getDay();
            if(dayOfWeek != 0) evt.cancel = true;
        }               
    }               
    </script>
    <table>
        <tr>
            <td style="width: 28px">
            </td>
            <td style="text-align: right">
            </td>
            <td>
                Select one or more from each of the lists below.&nbsp;
                <br />
                <br />
                The class list dynamically populates based on the SubTeam selection.
                <br />
                <br />
                Choose a Monday start date and Sunday end date for Weekly Level reports.
            </td>
        </tr>
        <tr>
            <td style="width: 28px">
            </td>
            <td style="text-align: right">
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 28px">
            </td>
            <td style="text-align: right">
            <asp:Label ID="lblMovement" Visible="true" runat="server" Font-Size="XX-Small" ForeColor="white"></asp:Label>
                Level</td>
            <td>
                <asp:RadioButtonList ID="radMovementTable" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                    <asp:ListItem Selected="True" Value="2">Daily</asp:ListItem>
                    <asp:ListItem Value="1">Weekly</asp:ListItem>
                </asp:RadioButtonList></td>
        </tr>
        <tr>
            <td style="width: 28px;"></td>
            <td style="text-align: right;">
                Store(s)
            </td>
            <td>
                <asp:ListBox ID="lstStores" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                    DataValueField="Store_No" SelectionMode="Multiple" Width="400px" Rows = 8></asp:ListBox>
                <asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td style="width: 28px;"></td>
            <td style="text-align: right;">
                Commodity Code(s)
            </td>
            <td style="width: 200px; height: 20px">
                <asp:ListBox ID="lstCommodityCode" runat="server" DataSourceID="ICItemAttributeText1"
                    DataTextField="ItemAttributeText1" DataValueField="OrderBy" SelectionMode="Multiple"
                    Width="400px" Rows = 8></asp:ListBox><br />
                <asp:SqlDataSource ID="ICItemAttributeText1" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetItemAttributeText_1" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td style="width: 28px;"></td>
            <td style="text-align: right;">
                 Subteam(s)
            </td>
            <td>
                <asp:ListBox ID="lstSubTeam" runat="server" AutoPostBack="True" DataSourceID="ICSubteam"
                    DataTextField="SubTeam_Name" DataValueField="SubTeam_No" SelectionMode="Multiple"
                    Width="400px" Rows = 8></asp:ListBox>
                <asp:SqlDataSource ID="ICSubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetSubTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td style="width: 28px;"></td>
            <td style="text-align: right;">
                Class(es)
            </td>
            <td>
                <asp:ListBox ID="lstClass" runat="server" DataSourceID="ICCategory" DataTextField="Category_Name"
                    DataValueField="Category_ID" SelectionMode="Multiple" Width="400px" Rows = 8></asp:ListBox>
                <asp:SqlDataSource ID="ICCategory" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetCategoriesByMultiSubTeam" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Name="SubteamList" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td style="width: 28px"></td>
            <td style="width: 28px">
            </td>
            <td style="width: 28px">
            </td>
        </tr>
<tr>
            <td style="width: 28px;"></td>
            <td style="text-align: right;">
                Begin Date 1
            </td>
            <td>
                <igsch:WebDateChooser ID="dteBeginDate"
                    runat="server" Value="" TabIndex="2">
                <ClientSideEvents CalendarValueChanging="valueChangingMonday"></ClientSideEvents>
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
            <td style="text-align: left;">
                <asp:RequiredFieldValidator ID="reqfld_BeginDate" runat="server" ControlToValidate="dteBeginDate" ErrorMessage="* Begin Date is a required field." SetFocusOnError="True" Display="Dynamic">
                </asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rngValid_BeginDate" runat="server" ControlToValidate="dteBeginDate" ErrorMessage="* Begin Date - Value must be a valid date." Type="Date" Display="Dynamic" Width="280px">
                </asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 28px;"></td>
            <td style="text-align: right;">
                End Date 1
            </td>
            <td>
                <igsch:WebDateChooser ID="dteEndDate" runat="server" TabIndex="3">
                    <ClientSideEvents CalendarValueChanging="valueChangingSunday"></ClientSideEvents>                
                    <CalendarLayout DayNameFormat="FirstLetter" FooterFormat="" ShowFooter="False" ShowNextPrevMonth="False" ShowTitle="False">
                    <SelectedDayStyle BackColor="#0054E3" />
                    <DayStyle BackColor="White" Font-Names="Arial" Font-Size="9pt" />
                    <OtherMonthDayStyle ForeColor="#ACA899" />
                    <DayHeaderStyle BackColor="#7A96DF" ForeColor="White" />
                    <TitleStyle BackColor="#9EBEF5" />
                    <CalendarStyle  Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                    </CalendarStyle>
                    </CalendarLayout>
                </igsch:WebDateChooser>
            </td>
            <td style="text-align: left;">
                <asp:RequiredFieldValidator ID="reqfld_EndDate" runat="server" ControlToValidate="dteEndDate" ErrorMessage="* End Date is a required field." SetFocusOnError="True" Display="Dynamic">
                </asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rngValid_EndDate" runat="server" ControlToValidate="dteEndDate" ErrorMessage="* End Date - Value must be a valid date." Type="Date" SetFocusOnError="True" Display="Dynamic" Width="272px">
                </asp:RangeValidator>
                <asp:CompareValidator ID="cmpValid_StartEndDate" runat="server" ControlToCompare="dteEndDate" ControlToValidate="dteBeginDate" Operator="LessThanEqual" ErrorMessage="* End date cannot be less than the begin date" Type="Date"></asp:CompareValidator>
            </td>
        </tr>      
        <tr>
            <td style="width: 28px;"></td>
            <td style="text-align: right;">
                Begin Date 2
            </td>
            <td>
                <igsch:WebDateChooser ID="dteBeginDate2"
                    runat="server" Value="" TabIndex="2">
                <ClientSideEvents CalendarValueChanging="valueChangingMonday"></ClientSideEvents>
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
            <td style="text-align: left;">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="dteBeginDate2" ErrorMessage="* Begin Date is a required field." SetFocusOnError="True" Display="Dynamic">
                </asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rngValid_BeginDate2" runat="server" ControlToValidate="dteBeginDate2" ErrorMessage="* Begin Date - Value must be a valid date." Type="Date" Display="Dynamic" Width="280px">
                </asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 28px;"></td>
            <td style="text-align: right;">
                End Date 2
            </td>
            <td>
                <igsch:WebDateChooser ID="dteEndDate2" runat="server" TabIndex="3">
                    <ClientSideEvents CalendarValueChanging="valueChangingSunday"></ClientSideEvents>                
                    <CalendarLayout DayNameFormat="FirstLetter" FooterFormat="" ShowFooter="False" ShowNextPrevMonth="False" ShowTitle="False">
                    <SelectedDayStyle BackColor="#0054E3" />
                    <DayStyle BackColor="White" Font-Names="Arial" Font-Size="9pt" />
                    <OtherMonthDayStyle ForeColor="#ACA899" />
                    <DayHeaderStyle BackColor="#7A96DF" ForeColor="White" />
                    <TitleStyle BackColor="#9EBEF5" />
                    <CalendarStyle  Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                    </CalendarStyle>
                    </CalendarLayout>
                </igsch:WebDateChooser>
            </td>
            <td style="text-align: left;">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="dteEndDate2" ErrorMessage="* End Date is a required field." SetFocusOnError="True" Display="Dynamic">
                </asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rngValid_EndDate2" runat="server" ControlToValidate="dteEndDate2" ErrorMessage="* End Date - Value must be a valid date." Type="Date" SetFocusOnError="True" Display="Dynamic" Width="272px">
                </asp:RangeValidator>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="dteEndDate2" ControlToValidate="dteBeginDate2" Operator="LessThanEqual" ErrorMessage="* End date cannot be less than the begin date" Type="Date"></asp:CompareValidator>
            </td>
        </tr>      
        <tr><td>&nbsp;</td></tr>
        
        <tr><td style="width: 28px;"></td>
            <td align="right">
                Report Format
            </td>
            <td>
                <asp:DropDownList ID="cmbReportFormat" runat="server" TabIndex="10" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" TabIndex="11" Text="View Report" CausesValidation="true" Width="100px" />
            </td>
        </tr>
    </table>
</asp:Content>

