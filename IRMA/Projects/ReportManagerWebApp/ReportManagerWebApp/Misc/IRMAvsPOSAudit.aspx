<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.IRMAvsPOSAudit" title="Report Manager - IRMA vs. POS Audit" Codebehind="IRMAvsPOSAudit.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="MiscReports.aspx?valuePath=Reports Home/Misc">Price Reports</a> &gt; IRMA vs. POS Audit</h3>
    </div>
    
    <table border="0">
        <tr>
            <td style="width: 150px; height: 1px; text-align: right">
                <br />
                Audit Category:&nbsp;
                <br />
            </td>
            <td style="width: 346px; height: 1px; text-align: left">
                <br />
                <asp:DropDownList ID="cmbAuditCategory" runat="server" DataSourceID="ICAuditCategory" DataTextField="POSAuditExceptionTypeDesc"
                    DataValueField="POSAuditExceptionTypeID" Width="225px">
                </asp:DropDownList>
                <asp:SqlDataSource ID="ICAuditCategory" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT * FROM POSAuditExceptionType WHERE POSAuditExceptionTypeId NOT IN (2)">
                </asp:SqlDataSource>
            </td>
            <td style="width: 318px; height: 1px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbVendor" runat="server" ControlToValidate="cmbAuditCategory"
                    ErrorMessage="*Audit Category is a required field." 
                    MaximumValue="2147483647" MinimumValue="1" SetFocusOnError="True"
                    Type="Integer" Width="256px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right">
                <br />
                Store: &nbsp;<br />
            </td>
            <td style="width: 249px; text-align: left">
                <br />
                <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="ICStore" DataTextField="Store_Name"
                    DataValueField="Store_No"  Width="225px" AutoPostBack="True">
                </asp:DropDownList><asp:SqlDataSource ID="ICStore" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT Store.Store_No, Store_Name&#9;&#9;&#13;&#10;FROM Store (nolock)&#13;&#10;WHERE (Mega_Store = 1 OR WFM_Store = 1) AND dbo.fn_getCustomerType(Store.Store_No, Store.Internal, Store.BusinessUnit_ID) = 3"></asp:SqlDataSource>
            </td>
            <td style="width: 318px; text-align: left">
                <asp:RangeValidator ID="rngValid_cmbTeam" runat="server" ControlToValidate="cmbStore"
                    ErrorMessage="* Store is a required field." 
                    MaximumValue="2147483647" MinimumValue="1" SetFocusOnError="True"
                    Type="Integer" Width="216px"></asp:RangeValidator></td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right">
                <br />
                Price Type:&nbsp;
                <br />
            </td>
            <td style="width: 249px; text-align: left">
                <br />
                <asp:DropDownList ID="cmbPriceType" runat="server" DataSourceID="ICPriceType" DataTextField="PriceChgTypeDesc"
                    DataValueField="PriceChgTypeId" Width="224px">
                </asp:DropDownList><asp:SqlDataSource ID="ICPriceType" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT PriceChgTypeId, PriceChgTypeDesc FROM PriceChgType&#13;&#10;ORDER BY PriceChgTypeId">
                </asp:SqlDataSource>
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right">
                <br />
                Subteam: &nbsp;<br />

            </td>
            <td style="width: 249px; text-align: left">
                <br />
                <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="ICSubteam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="ICSubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT SubTeam_No, SubTeam_Name FROM SubTeam ORDER BY SubTeam_Name">
                </asp:SqlDataSource>
            </td>
           <td style="width: 318px; text-align: left">
                </td>
        </tr> 
        <tr>
            <td style="width: 150px; text-align: right;">
                <br />
                Show filtered items:&nbsp;
            </td>
            <td style="width: 249px; text-align: left;">
                <br />
                <asp:CheckBox ID="chkShowFilteredItems" runat="server" /><br />
            </td>
            <td style="width: 318px; text-align: left;">
            </td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right">
                <br />
                Report Format:&nbsp;
                <br />
            </td>
             <td style="width: 249px; text-align: left">
                 <br />
                 <asp:DropDownList ID="cmbReportFormat" runat="server" Width="120px">
                <asp:ListItem>CSV</asp:ListItem>
                <asp:ListItem>EXCEL</asp:ListItem>
                <asp:ListItem Selected="True">HTML</asp:ListItem>
                <asp:ListItem>PDF</asp:ListItem>
                <asp:ListItem>XML</asp:ListItem>
            </asp:DropDownList>
                 <br />
             </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right">
            </td>
            <td style="width: 249px; text-align: left">
                <br />
            </td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 150px; text-align: right">
            </td>
            <td style="width: 249px; text-align: left">
                <asp:Button ID="btnReport" runat="server" Text="View Report"
                    Width="96px" /></td>
            <td style="width: 318px; text-align: left">
            </td>
        </tr>
    </table>

</asp:Content>

