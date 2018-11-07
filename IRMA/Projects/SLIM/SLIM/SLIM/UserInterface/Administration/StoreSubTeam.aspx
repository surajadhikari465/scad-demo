<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_Administration_StoreSubTeam" title="User Store/SubTeam" Codebehind="StoreSubTeam.aspx.vb" %>
<%@ MasterType VirtualPath="~/UserInterface/MasterPage.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="position: static">
        <tr>
            <td style="width: 100px">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Small" Style="position: static"
                    Text="Users"></asp:Label></td>
            <td style="width: 100px">
                <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Small" Style="position: static"
                    Text="Stores"></asp:Label></td>
            <td style="width: 100px">
                <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="Small" Style="position: static"
                    Text="Team"></asp:Label></td>
            <td style="width: 100px">
                <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="Small" Style="position: static"
                    Text="Title"></asp:Label></td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td style="width: 100px">
                <asp:DropDownList ID="UserList" runat="server" DataSourceID="SqlDataSource1"
                    DataTextField="UserName" DataValueField="User_ID" Style="position: static" AppendDataBoundItems="True">
                    <asp:ListItem Value="0">-- Select --</asp:ListItem>
                </asp:DropDownList></td>
            <td style="width: 100px">
                <asp:DropDownList ID="StoreList" runat="server" DataSourceID="SqlDataSource2"
                    DataTextField="Store_Name" DataValueField="Store_No" Style="position: static" AppendDataBoundItems="True">
                    <asp:ListItem Value="0">-- Select --</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 100px">
                <asp:DropDownList ID="TeamList" runat="server" DataSourceID="SqlDataSource3"
                    DataTextField="Team_Name" DataValueField="Team_No" Style="position: static" AppendDataBoundItems="True">
                    <asp:ListItem Value="0">-- Select --</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 100px">
                <asp:DropDownList ID="TitleList" runat="server" DataSourceID="SqlDataSource5"
                    DataTextField="Title_Desc" DataValueField="Title_ID" Style="position: static" AppendDataBoundItems="True">
                    <asp:ListItem Value="0">-- Select --</asp:ListItem>
                </asp:DropDownList></td>
            <td style="width: 100px">
                <asp:Button ID="AddUser" runat="server" Style="position: static" Text="Add User" /></td>
        </tr>
        <tr>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
        </tr>
    </table>
    &nbsp;&nbsp;
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource4"
        Style="position: static" AllowPaging="True" EmptyDataText="No Users Found" EnableViewState="False" AllowSorting="True" DataKeyNames="User_ID,Store_No,Team_No,Title_ID">
        <Columns>
            <asp:BoundField DataField="UserName" HeaderText="User_Name" SortExpression="UserName" />
            <asp:BoundField DataField="Store_Name" HeaderText="Store_Name" SortExpression="Store_Name" />
            <asp:BoundField DataField="Team_Name" HeaderText="Team_Name" SortExpression="Team_Name" />
            <asp:BoundField DataField="Title_Desc" HeaderText="Title_Desc" SortExpression="Title_Desc" />
            <asp:CommandField ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
    &nbsp;&nbsp;
    <asp:Label ID="MsgLabel" runat="server" Font-Bold="True" Font-Size="Medium" SkinID="SumLabel"
        Style="position: static" Width="304px"></asp:Label>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="SELECT [Store_Name], [Store_No] FROM [Store] WHERE ([WFM_Store] = 1) ORDER BY [Store_Name]">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="SELECT [Team_No], [Team_Name] FROM [Team] ORDER BY [Team_Name]">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="SELECT Users.UserName, SlimAccess.User_ID FROM SlimAccess INNER JOIN Users ON SlimAccess.User_ID = Users.User_ID ORDER BY SlimAccess.Insert_Date DESC">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="SELECT Users.UserName, Store.Store_Name, Team.Team_Name, Title.Title_Desc, UserStoreTeamTitle.User_ID, UserStoreTeamTitle.Store_No, UserStoreTeamTitle.Team_No, UserStoreTeamTitle.Title_ID FROM Users INNER JOIN SlimAccess ON Users.User_ID = SlimAccess.User_ID INNER JOIN Store INNER JOIN UserStoreTeamTitle ON Store.Store_No = UserStoreTeamTitle.Store_No ON SlimAccess.User_ID = UserStoreTeamTitle.User_ID AND Users.User_ID = UserStoreTeamTitle.User_ID INNER JOIN Team ON UserStoreTeamTitle.Team_No = Team.Team_No INNER JOIN Title ON UserStoreTeamTitle.Title_ID = Title.Title_ID" DeleteCommand="DELETE FROM UserStoreTeamTitle WHERE (User_ID = @User_ID) AND (Store_No = @Store_No) AND (Team_No = @Team_No) AND (Title_ID = @Title_ID)">
        <DeleteParameters>
            <asp:Parameter Name="User_ID" />
            <asp:Parameter Name="Store_No" />
            <asp:Parameter Name="Team_No" />
            <asp:Parameter Name="Title_ID" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="SELECT [Title_ID], [Title_Desc] FROM [Title] ORDER BY [Title_ID]">
    </asp:SqlDataSource>
</asp:Content>

