<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Price_MarginByVendor" title="IRMA Report Manager" Codebehind="MarginByVendor.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="PriceReports.aspx?valuePath=Reports Home/Prices">Price Reports</a> &gt; Margin By Vendor</h3>
    </div>
    <table style="width: 691px">
        <tr>
            <td style="width: 39px">
            </td>
            <td colspan="2">
    Search by:</td>
            <td style="width: 241px">
            </td>
        </tr>
        <tr>
            <td style="width: 39px">
            </td>
            <td style="width: 147px; text-align: right">
            </td>
            <td style="width: 145px">
                <asp:RadioButtonList ID="rdbtn_SearchType" runat="server" Width="250px" TabIndex="1">
                    <asp:ListItem Selected="True" Value="1">Company Name</asp:ListItem>
                    <asp:ListItem Value="2">PS Vendor ID</asp:ListItem>
                    <asp:ListItem Value="3">Vendor ID</asp:ListItem>
                </asp:RadioButtonList></td>
            <td style="width: 241px">
            </td>
        </tr>
        <tr>
            <td style="width: 39px">
            </td>
            <td colspan="2" valign="bottom">
    Vendor Selection, please enter a search string:</td>
            <td style="width: 241px">
                <asp:CustomValidator ID="custValidVendorID" runat="server" 
                    ControlToValidate="txtSearch"
                    OnServerValidate="ValidateNumber"
                    ErrorMessage="Vendor ID must be an integer greater than zero.">
                </asp:CustomValidator></td>
        </tr>
        <tr>
            <td style="width: 39px">
            </td>
            <td style="width: 147px; text-align: right">
            </td>
            <td style="width: 145px">
                <asp:TextBox ID="txtSearch" runat="server" Width="175px" MaxLength="60" TabIndex="2"></asp:TextBox></td>
            <td style="width: 241px">
    <asp:Button ID="btnGetVendors" runat="server" Text="Search" Width="100px" TabIndex="3" /></td>
        </tr>
        <tr>
            <td style="width: 39px">
            </td>
            <td style="width: 147px; text-align: right">
            </td>
            <td colspan="2">
                <asp:RequiredFieldValidator ID="reqfld_lstVendors" runat="server" ControlToValidate="lstVendors"
                    ErrorMessage="* A vendor must be selected." ValidationGroup="submit" Width="300px"></asp:RequiredFieldValidator><br />
                <asp:ListBox ID="lstVendors" runat="server" Height="149px" Width="352px" TabIndex="4" ></asp:ListBox>
                <br />
                <asp:Label ID="lblVendorCount" runat="server" ForeColor="Red"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 39px">
            </td>
            <td style="width: 147px; text-align: right">
                Store</td>
            <td style="width: 145px">
                <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                    DataValueField="Store_No" Height="24px"
                    Width="160px" TabIndex="5">
                </asp:DropDownList><asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="GetAllStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
            <td style="width: 241px">
                <asp:RequiredFieldValidator ID="reqfld_cmbStore" runat="server" ControlToValidate="cmbStore"
                    ErrorMessage="* Store is a required field." ValidationGroup="submit"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 39px">
            </td>
            <td style="width: 147px; text-align: right">
                Minimum Value</td>
            <td style="width: 145px">
                <asp:TextBox ID="txtMinval" runat="server" Width="70px" TabIndex="6"></asp:TextBox></td>
            <td style="width: 241px">
                <asp:RequiredFieldValidator ID="reqfld_txtMinval" runat="server" ControlToValidate="txtMinval"
                    ErrorMessage="* Minimum value is a required field." ValidationGroup="submit" Width="266px"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 39px">
            </td>
            <td style="width: 147px; text-align: right">
                Maximum Value</td>
            <td style="width: 145px">
                <asp:TextBox ID="txtMaxval" runat="server" Width="70px" TabIndex="7"></asp:TextBox></td>
            <td style="width: 241px">
                <asp:RequiredFieldValidator ID="reqfld_txtMaxval" runat="server" ControlToValidate="txtMaxval"
                    ErrorMessage="* Maximum value is a required field." ValidationGroup="submit" Width="267px"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 39px">
            </td>
            <td style="width: 147px; text-align: right">
                In Range</td>
            <td style="width: 145px">
                <asp:RadioButton ID="rdbtn_InRange" runat="server" Checked="True" GroupName="range" TabIndex="8" /></td>
            <td style="width: 241px">
            </td>
        </tr>
        <tr>
            <td style="width: 39px">
            </td>
            <td style="width: 147px; text-align: right">
                Out of Range</td>
            <td style="width: 145px">
                <asp:RadioButton ID="rdbtn_OutRange" runat="server" GroupName="range" TabIndex="9" /></td>
            <td style="width: 241px">
            </td>
        </tr>
        <tr>
            <td style="width: 39px">
            </td>
            <td style="width: 147px; text-align: right">
                Report Format</td>
            <td style="width: 145px">
                <asp:DropDownList ID="cmbReportFormat" runat="server" Width="120px" TabIndex="10">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList><asp:Button ID="btnReport" runat="server" Text="View Report"
                    Width="100px" ValidationGroup="submit" TabIndex="11" /></td>
            <td style="width: 241px">
            </td>
        </tr>
    </table>
    
    <asp:RangeValidator ID="rngValid_txtMinval" runat="server" ControlToValidate="txtMinval"
        ErrorMessage="* Minimum value must be an integer." MaximumValue="999999999" MinimumValue="-999999999" Type="Integer"
        ValidationGroup="submit"></asp:RangeValidator><br />
    <asp:RangeValidator ID="rngValid_txtMaxval" runat="server" ControlToValidate="txtMaxval"
        ErrorMessage="* Maximum value must be an integer." MaximumValue="999999999" MinimumValue="-999999999" Type="Integer"
        ValidationGroup="submit"></asp:RangeValidator><br />
    <asp:SqlDataSource ID="ICVendors_CompanyName" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
        SelectCommand="GetVendor_ByCompanyName" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtSearch" Name="Find" PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource><asp:SqlDataSource ID="ICVendors_PSVendorID" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
        SelectCommand="GetVendor_ByPSVendorID" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtSearch" Name="Find" PropertyName="Text" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="ICVendors_VendorID" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
        SelectCommand="GetVendor_ByVendorID" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtSearch" Name="Find" PropertyName="Text" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

