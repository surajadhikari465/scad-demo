<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Sales_DynamicRankTopBtmMovers" title="Untitled Page" Codebehind="DynamicRankTopBtmMovers.aspx.vb" %>
    
<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>
        
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><a href="..\Default.aspx"></a> 
        <h3><a href="..\Default.aspx">Home</a>&gt; <a href="SalesReports.aspx?valuePath=Reports Home/Sales">Sales Reports</a>&gt; Top,Bottom Dynamic Ranking Movement Report</h3>
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
            <td style="text-align: right; width: 139px;">
            </td>
            <td>
                Select one or more from each of the lists below.&nbsp;
                <br />
                <br />
                Choose a Monday start date and Sunday end date for Weekly Level reports.
            </td>
        </tr>
        <tr>
            <td style="width: 28px">
            </td>
            <td style="text-align: right; width: 139px;">
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 28px">
            </td>
            <td style="text-align: right; width: 139px;">
                <asp:Label ID="lblMovement" Visible="true" runat="server" Font-Size="XX-Small" ForeColor="white"></asp:Label>
                Level</td>
            <td>
                <asp:RadioButtonList ID="radMovementTable" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                    <asp:ListItem Selected="True" Value="2">Daily</asp:ListItem>
                    <asp:ListItem Value="1">Weekly</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td style="width: 28px">
            </td>
            <td style="width: 139px; text-align: right">
            </td>
            <td>
                </td>
        </tr>
        <tr>
            <td style="width: 28px">
            </td>
            <td style="width: 139px; text-align: right" valign="top">
                Sort By</td>
            <td>
                &nbsp;<asp:DropDownList ID="cmbRankBy" runat="server" Width="120px">
                    <asp:ListItem Value="GMDollars">GM Dollars</asp:ListItem>
                    <asp:ListItem Value="GMNetDollars">GM Net Dollars</asp:ListItem>
                    <asp:ListItem>Margin</asp:ListItem>
                    <asp:ListItem Value="NetMargin">Net Margin</asp:ListItem>
                    <asp:ListItem Selected="True" Value="SalesAmount">Sales Amount</asp:ListItem>
                    <asp:ListItem Value="SalesCost">Sales Cost</asp:ListItem>
                    <asp:ListItem Value="SalesNetCost">Sales Net Cost</asp:ListItem>
                    <asp:ListItem Value="SalesQuantity">Sales Quantity</asp:ListItem>
                </asp:DropDownList>&nbsp;
                <asp:RadioButtonList ID="radSortBy" runat="server">
                    <asp:ListItem Selected="True" Value="desc">Top</asp:ListItem>           
                    <asp:ListItem Value="asc">Bottom</asp:ListItem>
                </asp:RadioButtonList></td>
        </tr>
        <tr>
            <td style="width: 28px">
            </td>
            <td style="width: 139px; text-align: right">
            </td>
            <td>
                </td>
        </tr>
        <tr>
            <td style="width: 28px;"></td>
            <td style="text-align: right; width: 139px;">
                Store(s)
            </td>
            <td>
                &nbsp;<asp:ListBox ID="lstStores" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                    DataValueField="Store_No" SelectionMode="Multiple" Width="400px" Rows = 8></asp:ListBox>
                <asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td style="width: 28px;"></td>
            <td style="text-align: right; width: 139px;">
                Commodity 
                <br />
                Code(s)
            </td>
            <td style="height: 20px">
                &nbsp;<asp:ListBox ID="lstCommodityCode" runat="server" DataSourceID="ICItemAttributeText1"
                    DataTextField="ItemAttributeText1" DataValueField="OrderBy" SelectionMode="Multiple"
                    Width="400px" Rows= 8></asp:ListBox><br />
                <asp:SqlDataSource ID="ICItemAttributeText1" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetItemAttributeText_1" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td style="width: 28px;"></td>
            <td style="text-align: right; width: 139px;">
                 Subteam(s)
            </td>
            <td>
                &nbsp;<asp:ListBox ID="lstSubTeam" runat="server" DataSourceID="ICSubteam"
                    DataTextField="SubTeam_Name" DataValueField="SubTeam_No" SelectionMode="Multiple"
                    Width="400px" Rows = 8></asp:ListBox>
                <asp:SqlDataSource ID="ICSubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetSubTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td style="width: 28px;"></td>
            <td style="text-align: right; width: 139px;">
                Venue(s)
            </td>
            <td>
                &nbsp;<asp:ListBox ID="lstVenue" runat="server" SelectionMode="Multiple" Width="400px" DataSourceID="ICSItemAttributeText5" DataTextField="ItemAttributeText5" DataValueField="OrderBy" Rows = 8></asp:ListBox>&nbsp;
                <asp:SqlDataSource ID="ICSItemAttributeText5" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="&#9;&#9;SELECT&#13;&#10;&#9;&#9;DISTINCT CASE &#13;&#10;&#9;&#9;&#9;WHEN Text_5 = '' THEN ''&#13;&#10;&#9;&#9;&#9;WHEN Text_5 IS NULL THEN ''&#13;&#10;&#9;&#9;&#9;ELSE Text_5&#13;&#10;&#9;&#9;END as OrderBy,&#9;&#9;&#13;&#10;&#9;&#9;CASE &#13;&#10;&#9;&#9;&#9;WHEN Text_5 = '' THEN 'Venue Not Assigned'&#13;&#10;&#9;&#9;&#9;WHEN Text_5 IS NULL THEN 'Venue Not Assigned'&#13;&#10;&#9;&#9;&#9;ELSE Text_5 &#13;&#10;&#9;&#9;END as ItemAttributeText5&#13;&#10;&#9;FROM &#13;&#10;&#9;&#9;ItemAttribute&#13;&#10;&#9;ORDER BY&#13;&#10;&#9;&#9;OrderBy"></asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td style="width: 28px">
            </td>
            <td style="width: 139px; text-align: right">
                Item(s)<br />
                <span style="font-size: 9pt"></span>
            </td>
            <td>
                &nbsp;<asp:TextBox ID="txtItemList" runat="server" Width="395px" MaxLength="1000" TextMode="MultiLine" Height="50"></asp:TextBox><br />
                &nbsp;<asp:RegularExpressionValidator ID="regExValid_txtItemListLength" runat="server"
                    ControlToValidate="txtItemList" Display="Dynamic" ErrorMessage="The item list must be less than 1000 characters"
                    ValidationExpression="^[\s\S]{0,1000}$"></asp:RegularExpressionValidator>
                &nbsp;<asp:RegularExpressionValidator ID="regExValid_txtItemListValues" runat="server"
                    ControlToValidate="txtItemList" ErrorMessage="The entered value is not valid" ValidationExpression="(\d{1,13}(\r|\n)*)*"></asp:RegularExpressionValidator>
            </td>
            </tr>
        <tr>
            <td style="width: 28px">
            </td>
            <td style="width: 139px; text-align: right">
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="width: 28px; height: 27px;">
            </td>
            <td style="width: 139px; text-align: right; height: 27px;">
                Number of Results</td>
            <td style="height: 27px">
                &nbsp;<asp:TextBox ID="txtTopNumber" runat="server" Width="115px"></asp:TextBox>
                &nbsp;<asp:RegularExpressionValidator ID="regExValid_txtTopNumberValues" runat="server"
                    ControlToValidate="txtTopNumber" ErrorMessage="The entered value is not valid" ValidationExpression="\d{1,5}"></asp:RegularExpressionValidator>
    
            </td>
        </tr>
                <tr>
            <td style="width: 28px">
            </td>
            <td style="width: 139px; text-align: right" valign="top">
                Break By</td>
            <td>
                &nbsp;<asp:DropDownList ID="radBreakBy" runat="server" Width="120px">
                    <asp:ListItem Value="Region">None</asp:ListItem>
                    <asp:ListItem Value="Subteam_No">Subteam</asp:ListItem>
                </asp:DropDownList>
        </tr>
        <tr>
        <tr>
            <td style="width: 28px"></td>
            <td style="width: 139px">
            </td>
            <td>
                &nbsp;</td>
        </tr>

        <tr>
            <td style="width: 28px;"></td>
            <td style="text-align: right;">
                Begin Date
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
                End Date
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
        <tr><td>&nbsp;</td></tr>
        
        <tr><td style="width: 28px;"></td>
            <td align="right" style="width: 139px">
                Report Format
            </td>
            <td>
                &nbsp;<asp:DropDownList ID="cmbReportFormat" runat="server" TabIndex="10" Width="120px">
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





