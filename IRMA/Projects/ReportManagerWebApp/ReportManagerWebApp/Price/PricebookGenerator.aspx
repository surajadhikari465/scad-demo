<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.PriceBookGenerator" title="Report Manager - Pricebook Generator" Codebehind="PricebookGenerator.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 
    <div id="navbar">
        <h3><a href="..\Default.aspx">Home</a> &gt; <a href="PriceReports.aspx?valuePath=Reports Home/Price">Price</a> &gt; Pricebook Generator</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 138px; height: 1px; text-align: right">
                Zone&nbsp;</td>
            <td style="width: 443px; height: 1px; text-align: left">
                <asp:DropDownList ID="cmbZone" runat="server" DataSourceID="ICZone" DataTextField="Zone_Name"
                    DataValueField="Zone_ID" Width="225px" AutoPostBack="True">
                </asp:DropDownList><asp:SqlDataSource ID="ICZone" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT Zone_ID, Zone_Name FROM Zone ORDER BY Zone_Name "></asp:SqlDataSource>
                &nbsp;
            </td>
            <td style="width: 147px; height: 1px; text-align: left">
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="cmbZone"
                    ErrorMessage="*A Zone selection is required" Operator="GreaterThan" Type="Integer"
                    ValueToCompare="0" Width="251px"></asp:CompareValidator></td>
        </tr>
        <tr>
            <td style="width: 138px; height: 1px; text-align: right">
                Store&nbsp;
            </td>
            <td style="width: 443px; height: 1px; text-align: left">
                <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="ICStore" DataTextField="Store_Name"
                    DataValueField="Store_No" Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="ICStore" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT (CAST(Store_No As varchar(5)) + '|' + CAST(Regional As varchar(1))) As Store_No,Store_Name FROM Store WHERE Zone_ID = @ZoneID AND Regional = 0">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="cmbZone" DefaultValue="0" Name="ZoneID" PropertyName="SelectedValue" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 147px; height: 1px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 138px; height: 1px; text-align: right">
                Vendor&nbsp;
            </td>
            <td style="width: 443px; height: 1px; text-align: left">
                <asp:DropDownList ID="cmbVendor" runat="server" DataSourceID="ICVendors" DataTextField="CompanyName"
                    DataValueField="Vendor_Key" Width="225px">
                </asp:DropDownList>&nbsp; or &nbsp;<asp:TextBox ID="txtVendor" runat="server" Width="136px"></asp:TextBox>
                <asp:SqlDataSource ID="ICVendors" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT CompanyName,Vendor_Key FROM VENDOR ORDER BY CompanyName">
                </asp:SqlDataSource>
            </td>
            <td style="width: 147px; height: 1px; text-align: left">
                </td>
        </tr>
        <tr>
            <td style="width: 138px; text-align: right">
               Subteam&nbsp;

            </td>
            <td style="width: 443px; text-align: left">
                <asp:ListBox ID="lbSubTeam" runat="server" DataSourceID="ICSubteam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" Height="168px" SelectionMode="Multiple" Width="200px">
                </asp:ListBox>
                <asp:SqlDataSource ID="ICSubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetSubTeams" SelectCommandType="StoredProcedure">
                </asp:SqlDataSource>
            </td>
           <td style="width: 147px; text-align: left">
               <br />
               <asp:DropDownList ID="cmbPTD_Date" runat="server" DataSourceID="ICPTD" DataTextField="PeriodBeginDate" Width="40px">
               </asp:DropDownList><asp:SqlDataSource ID="ICPTD" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetBeginPeriodDateRS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
               <asp:DropDownList ID="cmbQTD_Date" runat="server" DataSourceID="ICQTD" DataTextField="QuarterBeginDate" Width="40px">
               </asp:DropDownList><asp:SqlDataSource ID="ICQTD" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetBeginQuarterDateRS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
               <asp:DropDownList ID="cmbYTD_Date" runat="server" DataSourceID="ICYTD" DataTextField="FiscalYearBeginDate" Width="40px">
               </asp:DropDownList><asp:SqlDataSource ID="ICYTD" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetBeginFiscalYearDateRS" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
           </td>
        </tr> 
        <tr>
            <td style="width: 138px; text-align: right">
                Narrow Criteria&nbsp;
            </td>
            <td style="width: 443px; text-align: left">
                <asp:DropDownList ID="cmbNarrowCriteria" runat="server" Width="192px">
                    <asp:ListItem>None</asp:ListItem>
                    <asp:ListItem Value="1">By Brand Name</asp:ListItem>
                    <asp:ListItem Value="2">By Manufacturer Number</asp:ListItem>
                    <asp:ListItem Value="3">By Item Description</asp:ListItem>
                    <asp:ListItem Value="4">By Price Type</asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp; = &nbsp;&nbsp;
                <asp:TextBox ID="txtNarrowCriteria" runat="server" Width="136px"></asp:TextBox></td>
            <td style="width: 147px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 138px; text-align: right">
                Include ISS's&nbsp;
            </td>
            <td style="width: 443px; text-align: left">
                <asp:CheckBox ID="chkIncludeISS" runat="server" /></td>
            <td style="width: 147px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 138px; text-align: right;">
                Movement Type&nbsp;
            </td>
            <td style="width: 443px; text-align: left;">
                &nbsp; &nbsp;<br />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                <asp:RadioButton ID="optPTD" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="MovementOptions"
                    Height="24px" Text="PTD" Width="136px" />
                &nbsp; &nbsp;&nbsp;<asp:RadioButton ID="optQTD" runat="server" Font-Names="Arial"
                    Font-Size="10pt" GroupName="MovementOptions" Height="24px" Text="QTD" Width="136px" /><br />
                <asp:RadioButton ID="optNone" runat="server" Checked="True" Font-Names="Arial" Font-Size="10pt"
                    GroupName="MovementOptions" Height="24px" Text="No Movement" Width="104px" /><br />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                <asp:RadioButton ID="optYTD" runat="server" Font-Names="Arial" Font-Size="10pt" GroupName="MovementOptions"
                    Height="24px" Text="YTD" Width="136px" />
                &nbsp; &nbsp;
                <asp:RadioButton ID="opt52Wk" runat="server" Font-Names="Arial" Font-Size="10pt"
                    GroupName="MovementOptions" Height="24px" Text="52 Weeks" Width="120px" /><br />
                <br />
            </td>
            <td style="width: 147px; text-align: left;">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 138px; text-align: right;">
                Pricebook Format&nbsp;
            </td>
            <td style="width: 443px; text-align: left;">
                <asp:DropDownList ID="cmbPricebookFormat" runat="server" Width="160px">
                    <asp:ListItem Selected="True" Value="1">Pricebook</asp:ListItem>
                    <asp:ListItem Value="2">Grocery Orderboard</asp:ListItem>
                    <asp:ListItem Value="3">Specialty Orderboard</asp:ListItem>
                </asp:DropDownList>&nbsp;
            </td>
            <td style="width: 147px; text-align: left;">
            </td>
        </tr>
        <tr>
            <td style="width: 138px; text-align: right;">
            </td>
            <td style="width: 443px; text-align: left;">
            </td>
            <td style="width: 147px; text-align: left;">
            </td>
        </tr>
        <tr>
            <td style="width: 138px; text-align: right">
                Report Format&nbsp;
            </td>
             <td style="width: 443px; text-align: left"><asp:DropDownList ID="cmbReportFormat" runat="server" Width="120px">
                <asp:ListItem Selected="True">EXCEL</asp:ListItem>
                <asp:ListItem>CSV</asp:ListItem>
                <asp:ListItem>HTML</asp:ListItem>
                <asp:ListItem>PDF</asp:ListItem>
                <asp:ListItem>XML</asp:ListItem>
            </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" Text="View Report"
                    Width="96px" /></td>
            <td style="width: 147px; text-align: left">
            </td>
        </tr>
    </table>
    <br />

</asp:Content>

