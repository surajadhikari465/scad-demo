<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_RetailCost_Main" title="Web Query Main" EnableSessionState="True" Codebehind="Main.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="width: 344px; position: static; height: 160px">
        <tr>
            <td style="width: 100px">
                <asp:Label ID="Label1" runat="server" Style="position: static" Text="UPC:" Font-Bold="True" Font-Size="Medium" Font-Underline="True"></asp:Label></td>
            <td style="width: 100px">
                <asp:TextBox ID="upcTxBx" runat="server" Style="position: static" MaxLength="13" TabIndex="1" ToolTip="UPC - 4-13 Digits"></asp:TextBox></td>
            <td style="width: 100px">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="upcTxBx"
                    ErrorMessage="Invalid UPC" Font-Size="Small" Style="position: static" ValidationExpression="[0-9]{4,13}"></asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td style="width: 100px">
                <asp:Label ID="Label2" runat="server" Style="position: static" Text="Description:" Font-Bold="True" Font-Underline="True"></asp:Label></td>
            <td style="width: 100px">
                <asp:TextBox ID="descTxBx" runat="server" Style="position: static" MaxLength="25" TabIndex="2" ToolTip="Item Description 4-20 Characters"></asp:TextBox></td>
            <td style="width: 100px">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="descTxBx"
                    ErrorMessage="Invalid Description" Font-Size="Small" Style="position: static"
                    ValidationExpression="\w{2,10}(\s)?\w{2,20}" Width="104px"></asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td style="width: 100px">
                <asp:Label ID="Label4" runat="server" Style="position: static" Text="Department:" Font-Bold="True" Font-Underline="True"></asp:Label></td>
            <td style="width: 100px">
                <asp:DropDownList ID="depDropDown" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1"
                    DataTextField="SubTeam_Name" DataValueField="SubTeam_No" Style="position: static" TabIndex="3" AppendDataBoundItems="True">
                    <asp:ListItem Value="0">----Select----</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td style="width: 100px">
                <asp:Label ID="Label3" runat="server" Style="position: static" Text="Brand:" Font-Bold="True" Font-Underline="True"></asp:Label></td>
            <td style="width: 100px">
                <asp:DropDownList ID="brandDropDown" runat="server" DataSourceID="SqlDataSource3"
                    DataTextField="Brand_Name" DataValueField="Brand_ID" Style="position: static" TabIndex="4" AppendDataBoundItems="True">
                    <asp:ListItem Value="0">----Select----</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
                <asp:Button ID="SearchItem" runat="server" Style="position: static" Text="Search" /></td>
            <td style="width: 100px">
                <asp:Label ID="ErrorLabel" runat="server" Font-Bold="True" Font-Size="Small" ForeColor="#0000C0"
                    Style="position: static" Width="104px"></asp:Label></td>
        </tr>
    </table>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="GetAllSubTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="select distinct st.subteam_name,st.subteam_no from subteam st&#13;&#10;inner join userstoreteamtitle ust&#13;&#10;on st.team_no = ust.team_no&#13;&#10;where ust.user_id = @User_ID&#13;&#10;order by st.Subteam_Name&#13;&#10;">
        <SelectParameters>
            <asp:SessionParameter Name="User_ID" SessionField="UserID" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="GetSubTeamBrand" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="depDropDown" DefaultValue="0" Name="SubTeam_No"
                PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="GetBrandAndID" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
</asp:Content>

