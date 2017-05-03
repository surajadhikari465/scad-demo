<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Distribution_PreOrderQty" title="Report Manager - Pre Order Quantity" Codebehind="PreOrderQty.aspx.vb" %>

<%@ Register Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7DD5C3163F2CD0CB"
    Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="DistributionReports.aspx?valuePath=Reports Home/Distribution">Distribution Reports</a> &gt; PreOrder Quantity</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 192px; text-align: right;">
                <br />
                SubTeam No:&nbsp;
            </td>
            <td style="width: 249px; text-align: left;">
                <br />
                <asp:DropDownList ID="cmbSubTeam" runat="server" Width="225px" DataSourceID="IC_SubTeam" DataTextField="SubTeam_Name" DataValueField="SubTeam_No" Font-Names="Verdana" Font-Size="10pt" TabIndex="1">
                </asp:DropDownList><asp:SqlDataSource ID="IC_SubTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="SELECT * FROM SubTeam WHERE EXEDistributed = 1">
                </asp:SqlDataSource>
            </td>
            <td style="width: 318px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbSubTeam" runat="server" ControlToValidate="cmbSubTeam"
                    ErrorMessage="* SubTeam is a required field." MaximumValue="2147483647" MinimumValue="1"
                    SetFocusOnError="True" Type="Integer" Width="232px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right;">
                <br />
                DC Store:&nbsp;
            </td>
            <td style="width: 249px; text-align: left;">
                <br />
                <asp:DropDownList ID="cmbDCStore" runat="server" Width="225px" DataSourceID="IC_DC_Store" DataTextField="Store_Name" DataValueField="Store_No" Font-Names="Verdana" Font-Size="10pt" TabIndex="1">
                </asp:DropDownList><asp:SqlDataSource ID="IC_DC_Store" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                SelectCommand="SELECT * FROM Store WHERE Distribution_Center = 1"></asp:SqlDataSource></td>
            <td style="width: 318px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbDCStore" runat="server" ControlToValidate="cmbDCStore"
                    ErrorMessage="* DC Store is a required field." MaximumValue="2147483647" 
                    MinimumValue="1" SetFocusOnError="True"
                    Type="Integer" Width="224px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right; height: 64px;">
                <br />
                Expected Date:&nbsp;
            </td>
            <td style="width: 249px; text-align: left; height: 64px;">
                <br />
                <igsch:webdatechooser id="dteExpectedDate" runat="server" value="" width="112px" TabIndex="4">
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
            <td style="width: 318px; text-align: left; height: 64px;">
                <asp:RequiredFieldValidator ID="reqfld_ExpectedDate" runat="server" ControlToValidate="dteExpectedDate"
                    Display="Dynamic" ErrorMessage="* Expected Date is a required field."
                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:RangeValidator
                        ID="rngValid_ExpectedDate" runat="server" ControlToValidate="dteExpectedDate" Display="Dynamic"
                        ErrorMessage="* Expected Date must be a valid date." 
                        Type="Date" Width="333px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
                <br />
                Pre Order:&nbsp;
                <br />
            </td>
            <td style="width: 346px; text-align: left">
                <br />
                <asp:RadioButton ID="optPreOrderTrue" runat="server" Checked="True" GroupName="grpPreOrder"
                    Text="True" />
                <asp:RadioButton ID="optPreOrderFalse" runat="server" GroupName="grpPreOrder" Text="False" /><br />
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
                <br />
                Report Format:&nbsp;
            </td>
            <td style="width: 346px; text-align: left">
                <br />
                <asp:DropDownList ID="cmbReportFormat" runat="server" TabIndex="8" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
            </td>
            <td style="width: 249px; text-align: left">
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 192px; text-align: right">
            </td>
            <td style="width: 249px; text-align: left">
                <br />
                <asp:Button ID="btnReport" runat="server" TabIndex="9" Text="View Report" Width="96px" /></td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
    </table>
    </asp:Content>

