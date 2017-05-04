<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="UserInterface_ItemVendor_Main_old" title="Item Authorizations Main" Codebehind="Main_old.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <table style="width: 344px; position: static; height: 160px">
        <tr>
            <td style="width: 100px">
                <asp:Label ID="Label1" runat="server" Style="position: static" Text="UPC:" Font-Bold="True" Font-Underline="True"></asp:Label></td>
            <td style="width: 100px">
                <asp:TextBox ID="upcTxBx" runat="server" Style="position: static" ToolTip="UPC from 4 to 13 characters"></asp:TextBox></td>
            <td style="width: 100px">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="upcTxBx"
                    ErrorMessage="Invalid UPC" Font-Size="Small" Style="position: static" ValidationExpression="[0-9]{4,13}"></asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td style="width: 100px">
                <asp:Label ID="Label2" runat="server" Style="position: static" Text="Description:" Font-Bold="True" Font-Underline="True"></asp:Label></td>
            <td style="width: 100px">
                <asp:TextBox ID="descTxBx" runat="server" Style="position: static" ToolTip="Description must be at least 4 characters"></asp:TextBox></td>
            <td style="width: 100px">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="descTxBx"
                    ErrorMessage="Invalid Description" Font-Size="Small" Style="position: static"
                    ValidationExpression="\w{2,10}(\s)?\w{2,20}" Width="152px"></asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td style="width: 100px; height: 24px;">
                <asp:Label ID="Label4" runat="server" Style="position: static" Text="Department:" Font-Bold="True" Font-Underline="True"></asp:Label></td>
            <td style="width: 100px; height: 24px;">
                <asp:DropDownList ID="depDropDown" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                    DataSourceID="SqlDataSource8" DataTextField="SubTeam_Name" DataValueField="SubTeam_No"
                    Style="position: static">
                    <asp:ListItem Value="0">-- Select --</asp:ListItem>
                </asp:DropDownList>&nbsp;
            </td>
            <td style="width: 100px; height: 24px;">
            </td>
        </tr>
        <tr>
            <td style="width: 100px">
                <asp:Label ID="Label3" runat="server" Style="position: static" Text="Brand:" Font-Bold="True" Font-Underline="True"></asp:Label></td>
            <td style="width: 100px">
                <asp:DropDownList ID="brandDropDown" runat="server" DataSourceID="SqlDataSource3"
                    DataTextField="Brand_Name" DataValueField="Brand_ID" Style="position: static" AppendDataBoundItems="True">
                    <asp:ListItem Value="0">-- Select --</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td style="width: 100px; height: 64px;">
            </td>
            <td style="width: 100px; height: 64px;">
                <asp:Button ID="Button1" runat="server" Style="position: static" Text="Search" /></td>
            <td style="width: 100px; height: 64px;">
                <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    SelectCommand="GetSubTeamBrand" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="depDropDown" DefaultValue="0" Name="SubTeam_No"
                            PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    SelectCommand="GetAllSubTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    SelectCommand="select distinct st.subteam_name,st.subteam_no from subteam st&#13;&#10;inner join userstoreteamtitle ust&#13;&#10;on st.team_no = ust.team_no&#13;&#10;where ust.user_id = @User_ID&#13;&#10;order by st.Subteam_Name&#13;&#10;">
                    <SelectParameters>
                        <asp:SessionParameter Name="User_ID" SessionField="UserID" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    SelectCommand="GetBrandAndID" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </td>
        </tr>
    </table>
</asp:Content>

