<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Distribution_FacilityOrderReport" title="Report Manager - Facility Order Report" Codebehind="FacilityOrderReport.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="DistributionReports.aspx?valuePath=Reports Home/Distribution">Distribution Reports</a> &gt; Facility Order Report</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 192px; text-align: right;">
                Facility:&nbsp;
            </td>
            <td style="width: 510px; text-align: left;">
                <br />
                <asp:DropDownList ID="cmbFacility" runat="server" Width="225px" DataSourceID="IC_Facility" DataTextField="CompanyName" DataValueField="Vendor_Id" Font-Names="Verdana" Font-Size="10pt" TabIndex="1">
                </asp:DropDownList><asp:SqlDataSource ID="IC_Facility" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="SELECT V.Vendor_ID, V.CompanyName&#13;&#10;FROM Vendor V&#13;&#10;JOIN Store S ON S.Store_No = V.Store_No&#13;&#10;WHERE (S.Distribution_Center = 1 OR S.Manufacturer = 1) AND WFM_Store = 0"></asp:SqlDataSource></td>
            <td style="width: 383px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbStore" runat="server" ControlToValidate="cmbFacility"
                    ErrorMessage="* Facility is a required field." MaximumValue="2147483647" 
                    MinimumValue="1" SetFocusOnError="True"
                    Type="Integer" Width="201px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right;">
                Expected Date Begin:&nbsp;
            </td>
            <td style="width: 510px; text-align: left">
                <br />
                <igsch:webdatechooser id="dteBeginDate" runat="server" value="" width="112px" TabIndex="4">
                <CalendarLayout DayNameFormat="FirstLetter" FooterFormat="" ShowFooter="False" ShowNextPrevMonth="False"
                    ShowTitle="False">
                    <SelectedDayStyle BackColor="#0054E3" ></SelectedDayStyle>
                    <DayStyle BackColor="White" Font-Names="Arial" Font-Size="9pt" ></DayStyle>
                    <OtherMonthDayStyle ForeColor="#ACA899" ></OtherMonthDayStyle>
                    <DayHeaderStyle BackColor="#7A96DF" ForeColor="White" ></DayHeaderStyle>
                    <TitleStyle BackColor="#9EBEF5" ></TitleStyle>
                    <CalendarStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False">
                    </CalendarStyle>
                </CalendarLayout>
            </igsch:webdatechooser>
            </td>
            <td style="width: 383px; text-align: left">
                <asp:RequiredFieldValidator ID="reqfld_BeginDate" runat="server" ControlToValidate="dteBeginDate"
                    Display="Dynamic" ErrorMessage="* Expected Date Begin is a required field."
                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:RangeValidator
                        ID="rngValid_BeginDate" runat="server" ControlToValidate="dteBeginDate" Display="Dynamic"
                        ErrorMessage="* Expected Date Begin - Value must be a valid date." 
                        Type="Date" Width="376px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right;">
                Expected Date End:&nbsp;
            </td>
            <td style="width: 510px; text-align: left;">
                <br />
                <igsch:WebDateChooser ID="dteEndDate" runat="server" Width="112px" TabIndex="5">
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
            <td style="width: 383px; text-align: left">
                <asp:RequiredFieldValidator ID="reqfld_EndDate" runat="server" ControlToValidate="dteEndDate"
                    Display="Dynamic" ErrorMessage="* Expected Date End is a required field." 
                    SetFocusOnError="True"></asp:RequiredFieldValidator><asp:RangeValidator
                        ID="rngValid_EndDate" runat="server" ControlToValidate="dteEndDate" Display="Dynamic"
                        ErrorMessage="* Expected Date End - Value must be a valid date." 
                        SetFocusOnError="True" Type="Date" Width="368px"></asp:RangeValidator><br />
                <asp:CompareValidator ID="comp_DateRange" runat="server" ControlToCompare="dteEndDate"
                    ControlToValidate="dteBeginDate" Display="Dynamic" ErrorMessage="* Invalid date range - End Date cannot be earlier than Begin Date."
                    Operator="LessThanEqual"
                    Type="Date" Width="479px"></asp:CompareValidator></td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right;">
                PO Status:&nbsp;
            </td>
            <td style="width: 510px; text-align: left;">
                <br />
                <asp:DropDownList ID="cmbPOStatus" runat="server" Width="225px" Font-Names="Verdana" Font-Size="10pt" TabIndex="1">
                    <asp:ListItem Value="0">&lt; Select a Value &gt;</asp:ListItem>
                    <asp:ListItem Value="1">Sent</asp:ListItem>
                    <asp:ListItem Value="2">Warehouse Sent</asp:ListItem>
                    <asp:ListItem Value="3">Closed</asp:ListItem>
                    <asp:ListItem Value="4">Approved</asp:ListItem>
                    <asp:ListItem Value="5">Uploaded</asp:ListItem>
                </asp:DropDownList>
                <br />
            </td>
            <td style="width: 383px; text-align: left">
                </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right;">
                Store:&nbsp;
            </td>
            <td style="width: 510px; text-align: left;">
                <br />
                &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                <asp:CheckBox ID="chkAll_Store" runat="server" AutoPostBack="True" Checked="True"
                    Text="All" />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<br />
                <asp:ListBox ID="lbStoreList" runat="server" DataSourceID="IC_Store" DataTextField="Store_Name"
                    DataValueField="Store_No" Height="120px" SelectionMode="Multiple" Width="216px">
                </asp:ListBox>
                <asp:SqlDataSource ID="IC_Store" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="GetStores" SelectCommandType="StoredProcedure">
                </asp:SqlDataSource>
            </td>
            <td style="width: 383px; text-align: left">
                </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right;">
                SubTeam:&nbsp;
            </td>
            <td style="width: 510px; text-align: left;">
                <br />
                &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                <asp:CheckBox ID="chkAll_SubTeam" runat="server" AutoPostBack="True" Checked="True"
                    Text="All" /><br />
                <asp:ListBox ID="lbSubTeamList" runat="server" DataSourceID="IC_SubTeam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" Height="120px" SelectionMode="Multiple" Width="216px">
                </asp:ListBox><br />
                <asp:SqlDataSource ID="IC_SubTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="SELECT SubTeam_Name, SubTeam_No FROM SubTeam WHERE EXEDistributed = 1">
                </asp:SqlDataSource>
            </td>
            <td style="width: 383px; text-align: left">
                </td>
        </tr>
<%--        <tr>
            <td style="width: 192px; text-align: right">
                Sorting: &nbsp;</td>
            <td style="width: 510px; text-align: left">
                <br />
                <asp:RadioButton ID="optSortLevel1_Store" runat="server" Checked="True" Text="Store" AutoPostBack="True" GroupName="SortLevel1" />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:RadioButton ID="optSortLevel2_Store"
                    runat="server" Text="Store" AutoPostBack="True" GroupName="SortLevel2" />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                <asp:RadioButton ID="optSortLevel3_Store" runat="server" Text="Store" AutoPostBack="True" GroupName="SortLevel3" /><br />
                <asp:RadioButton ID="optSortLevel1_SubTeam" runat="server" Text="SubTeam" AutoPostBack="True" GroupName="SortLevel1" />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:RadioButton ID="optSortLevel2_SubTeam" runat="server"
                    Checked="True" Text="SubTeam" AutoPostBack="True" GroupName="SortLevel2" />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                <asp:RadioButton ID="optSortLevel3_SubTeam" runat="server" Text="SubTeam" AutoPostBack="True" GroupName="SortLevel3" /><br />
                <asp:RadioButton ID="optSortLevel1_ExpectedDate" runat="server" Text="Expected Date" AutoPostBack="True" GroupName="SortLevel1" />
                &nbsp; &nbsp;
                <asp:RadioButton ID="optSortLevel2_ExpectedDate" runat="server" Text="Expected Date" AutoPostBack="True" GroupName="SortLevel2" />
                &nbsp; &nbsp;
                <asp:RadioButton ID="optSortLevel3_ExpectedDate" runat="server" Checked="True" Text="Expected Date" AutoPostBack="True" GroupName="SortLevel3" /><br />
            </td>
            <td style="width: 383px; text-align: left">
                <asp:Label ID="lblSortingError" runat="server" ForeColor="Red" Text="* Each sorting option can only be selected one time."
                    Visible="False" Width="472px"></asp:Label><br />
            </td>
        </tr>--%>
        <tr>
            <td style="width: 192px; text-align: right">
                Sorting: &nbsp;</td>
            <td style="width: 510px; text-align: left">
                <br />
                   <table> 
                    <tr><td style="width: 170px; text-align: left">
                    <asp:RadioButton ID="optSortLevel1_Store" runat="server" Checked="True" Text="Store" AutoPostBack="True" GroupName="SortLevel1" />
                    </td>
                    <td style="width: 170px; text-align: left">
                        <asp:RadioButton ID="optSortLevel2_Store" runat="server" Text="Store" AutoPostBack="True" GroupName="SortLevel2" />
                    </td>
                    <td style="width: 170px; text-align: left">
                    <asp:RadioButton ID="optSortLevel3_Store" runat="server" Text="Store" AutoPostBack="True" GroupName="SortLevel3" />
                    </td>
                    </tr>
                    <tr>
                    <td style="width: 170px; text-align: left">
                    <asp:RadioButton ID="optSortLevel1_SubTeam" runat="server" Text="SubTeam" AutoPostBack="True" GroupName="SortLevel1" />
                    </td>
                    <td style="width: 170px; text-align: left">
                        <asp:RadioButton ID="optSortLevel2_SubTeam" runat="server" Checked="True" Text="SubTeam" AutoPostBack="True" GroupName="SortLevel2" />
                    </td>
                    <td style="width: 170px; text-align: left">
                        <asp:RadioButton ID="optSortLevel3_SubTeam" runat="server" Text="SubTeam" AutoPostBack="True" GroupName="SortLevel3" />
                    </td>
                    </tr>
                    <tr>
                    <td style="width: 170px; text-align: left">
                        <asp:RadioButton ID="optSortLevel1_ExpectedDate" runat="server" Text="Expected Date" AutoPostBack="True" GroupName="SortLevel1" />
                    </td>
                    <td style="width: 170px; text-align: left">
                        <asp:RadioButton ID="optSortLevel2_ExpectedDate" runat="server" Text="Expected Date" AutoPostBack="True" GroupName="SortLevel2" />
                   </td>
                   <td style="width: 170px; text-align: left">
                        <asp:RadioButton ID="optSortLevel3_ExpectedDate" runat="server" Checked="True" Text="Expected Date" AutoPostBack="True" GroupName="SortLevel3" />
                   </td> 
                  </tr>
                </table>
            </td>
            <td style="width: 383px; text-align: left">
                <asp:Label ID="lblSortingError" runat="server" ForeColor="Red" Text="* Each sorting option can only be selected one time."
                    Visible="False" Width="472px"></asp:Label><br />
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
                <br />
                Report Format</td>
            <td style="width: 510px; text-align: left">
                <br />
                <asp:DropDownList ID="cmbReportFormat" runat="server" TabIndex="8" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 383px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
            </td>
            <td style="width: 510px; text-align: left">
            </td>
            <td style="width: 383px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
            </td>
            <td style="width: 510px; text-align: left">
                <br />
                <asp:Button ID="btnReport" runat="server" TabIndex="9" Text="View Report" Width="96px" /></td>
            <td style="width: 383px; text-align: left">
            </td>
        </tr>
    </table>
    </asp:Content>

