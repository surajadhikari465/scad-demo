<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Item_ItemsBySustainabilityRanking" title="IRMA Report Manager" Codebehind="ItemsBySustainabilityRanking.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="navbar">
        <h3><a href="..\Default.aspx">Home</a> &gt; <a href="ItemReports.aspx?valuePath=Reports Home/Items">Item Reports</a> &gt; Items by Sustainability Ranking</h3>
    </div>
    <table id="tblParameters" border="0">
        <tr>
            <td style="width: 218px; text-align: right">
                Sustainability Ranking</td>
            <td style="width: 249px; text-align: left">
                <asp:DropDownList ID="cmbSustainabilityRanking" runat="server" DataSourceID="SDS_SustainabilityRankings" DataTextField="Label"
                    DataValueField="ID" Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="SDS_SustainabilityRankings" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT '<All>' As Label, NULL As ID&#13;&#10;UNION&#13;&#10;SELECT RankingDescription As Label, ID&#13;&#10;FROM SustainabilityRanking (nolock)&#13;&#10;ORDER BY ID"></asp:SqlDataSource>
            </td>
            <td style="width: 300px; text-align: left">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="width: 218px; text-align: right">
                Subteam
            </td>
            <td style="width: 346px; text-align: left">
                <asp:DropDownList ID="cmbSubTeam" runat="server" AutoPostBack="True" DataSourceID="SDS_Subteams"
                    DataTextField="Label" DataValueField="ID" Width="225px">
                </asp:DropDownList><asp:SqlDataSource ID="SDS_Subteams" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT '<All>' As Label, NULL As ID&#13;&#10;UNION&#13;&#10;SELECT Subteam_Name As Label, Subteam_No as ID &#13;&#10;FROM Subteam (nolock) &#13;&#10;ORDER BY Label">
                </asp:SqlDataSource>
            </td>
            <td style="width: 300px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 218px; text-align: right">
                Category</td>
            <td style="width: 346px; text-align: left"><asp:DropDownList ID="cmbCategory" runat="server" DataSourceID="SDS_Categories"
                    DataTextField="Label" DataValueField="ID" Width="225px">
            </asp:DropDownList><asp:SqlDataSource ID="SDS_Categories" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT '<All>' As Label, NULL As ID&#13;&#10;UNION&#13;&#10;SELECT Category_Name As Label, Category_ID As ID&#13;&#10;FROM ItemCategory (nolock)&#13;&#10;WHERE Subteam_No = @Subteam_No&#13;&#10;ORDER BY Label">
                <SelectParameters>
                    <asp:ControlParameter ControlID="cmbSubTeam" DefaultValue="0" Name="Subteam_No" PropertyName="SelectedValue" />
                </SelectParameters>
                </asp:SqlDataSource>
            </td>
            <td style="width: 300px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 218px; text-align: right">
                Country of Origin</td>
            <td style="width: 346px; text-align: left"><asp:DropDownList ID="cmbCountryOfOrigin" runat="server" DataSourceID="SDS_CoO"
                    DataTextField="Label" DataValueField="ID" Width="225px">
            </asp:DropDownList><asp:SqlDataSource ID="SDS_CoO" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="SELECT '<All>' As Label, NULL As ID&#13;&#10;UNION&#13;&#10;SELECT Origin_Name As Label, Origin_ID As ID&#13;&#10;FROM ItemOrigin (nolock)&#13;&#10;ORDER BY Label">
            </asp:SqlDataSource>
            </td>
            <td style="width: 300px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 218px; text-align: right">
                Country of Processing</td>
            <td style="width: 346px; text-align: left"><asp:DropDownList ID="cmbCountryOfProcessing" runat="server" DataSourceID="SDS_CoO"
                    DataTextField="Label" DataValueField="ID" Width="225px">
            </asp:DropDownList></td>
            <td style="width: 300px; text-align: left">
            </td>
        </tr>
        <tr>
            <td style="width: 218px; text-align: right">
                <span>Report Format</span></td>
            <td style="width: 346px; text-align: left">
                <asp:DropDownList ID="cmbReportFormat" runat="server" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" Text="View Report" Width="96px" /></td>
            <td style="width: 300px; text-align: left">
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

