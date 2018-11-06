<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.InventoryValueReport" title="InventoryValueReport" Codebehind="InventoryValueReport.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar">
        <h3><a href="..\Default.aspx">Home</a> &gt; <a href="ItemReports.aspx?valuePath=Reports Home/Items">Item Reports</a> &gt; Inventory Value Report</h3>
    </div>
    <table border="0">
        <tr>
            <td style="width: 160px; text-align: right; height: 56px;">Business Unit:</td>
            <td style="width: 249px; text-align: left; height: 56px;">
                <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="ICStores" DataTextField="Store_Name"
                    DataValueField="Store_No"
                    Width="265px" TabIndex="1">
                </asp:DropDownList>
                
                <asp:RequiredFieldValidator ID="reqfld_cmbStore" runat="server" ControlToValidate="cmbStore" 
                    ErrorMessage="* Business Unit is a required field." Height="18px" Display="Dynamic">
                </asp:RequiredFieldValidator>
            </td>
            <td style="width: 343px;"></td>
        </tr>
        <tr>
            <td style="width: 160px; text-align: right; height: 56px;">Team:</td>
            <td style="width: 249px; text-align: left; height: 56px;">
                <asp:DropDownList ID="cmbTeam" runat="server" AutoPostBack="True" DataSourceID="ICTeam"
                    DataTextField="Team_Name" DataValueField="Team_No" Width="265px">
                </asp:DropDownList>
            </td>
            <td style="width: 343px;"></td>
        </tr>
        <tr>
            <td style="width: 160px; text-align: right; height: 56px;">SubTeam:</td>
            <td style="width: 160px; text-align: right; height: 56px;">
                <asp:DropDownList ID="cmbSubteam" runat="server" DataSourceID="ICSubteam" DataTextField="SubTeam_Name"
                    DataValueField="SubTeam_No" TabIndex="2" Width="265px" AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <td style="width: 343px;"></td>
        </tr>
        <tr>
            <td style="width: 160px; text-align: right; height: 56px;">Identifier:</td>
            <td style="width: 160px; text-align: right; height: 56px;">
                <asp:TextBox ID="txtIdentifier" TabIndex="2" Width="260px" runat="server"></asp:TextBox>
            </td>
            <td style="width: 343px;"></td>
        </tr>
        <tr>
            <td style="width: 160px; text-align: right; height: 56px;">Report Format:</td>
            <td style="width: 160px; text-align: left; height: 56px;">
                <asp:DropDownList ID="cmbReportFormat" runat="server" TabIndex="3" Width="265px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 343px;"></td>
        </tr>
        <tr>
            <td style="width: 160px; text-align: right; height: 56px;"></td>
            <td style="width: 160px; text-align: left; height: 56px;"></td>
            <td style="width: 343px; text-align: left;">    <asp:Button ID="btnReport" runat="server" TabIndex="4" Text="View Report" Width="100px" /></td>
        </tr>
    </table>


<asp:SqlDataSource ID="ICSubteam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
    SelectCommand="SELECT ST.SubTeam_No, ST.SubTeam_Name FROM SubTeam ST (NOLOCK) WHERE ST.Team_No = ISNULL(@Team_No, ST.Team_No)">
    <SelectParameters>
        <asp:ControlParameter ControlID="cmbTeam" Name="Team_No" PropertyName="SelectedValue" />
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="ICTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
    SelectCommand="GetTeams" SelectCommandType="StoredProcedure">
</asp:SqlDataSource>

<asp:SqlDataSource ID="ICStores" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>" 
    SelectCommand="GetAllDistributionCenters" SelectCommandType="StoredProcedure">
</asp:SqlDataSource>
    
</asp:Content>

