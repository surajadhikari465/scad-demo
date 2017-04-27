<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_Administration_EmailMessages" title="Email Notifications" Codebehind="EmailMessages.aspx.vb" %>
<%@ MasterType VirtualPath="~/UserInterface/MasterPage.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table cellpadding="3" cellspacing="1" style="width: 360px; position: static; height: 48px">
        <tr>
            <td align="left" style="width: 100px">
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Style="position: static" Text="Teams"></asp:Label></td>
            <td style="width: 100px">
            </td>
            <td align="left" style="width: 100px">
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Style="position: static" Text="Store"></asp:Label></td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
                            </td>
        </tr>
        <tr>
            <td style="width: 100px">
                            <asp:DropDownList ID="TeamDropDown" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSource1"
                                DataTextField="Team_Name" DataValueField="Team_No" Style="position: static">
                                <asp:ListItem Selected="True" Value="0">--Select--</asp:ListItem>
                            </asp:DropDownList></td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
                            <asp:DropDownList ID="StoreDropDown" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSource3"
                                DataTextField="Store_Name" DataValueField="Store_No" Style="position: static">
                                <asp:ListItem Selected="True" Value="0">--Select--</asp:ListItem>
                            </asp:DropDownList></td>
            <td style="width: 100px">
                            <asp:Button ID="Button1" runat="server" Style="position: static" Text="New Entry" /></td>
            <td style="width: 100px">
            </td>
        </tr>
    </table>
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" DataKeyNames="EmailID" DataSourceID="SqlDataSource2"
                    EmptyDataText="No Items Found" Style="position: static" EnableViewState="False">
                    <Columns>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:BoundField DataField="Team_Name" HeaderText="Team" SortExpression="Team_Name" />
                        <asp:BoundField DataField="Store_Name" HeaderText="Store" ReadOnly="True" SortExpression="Store_Name">
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TeamLeader_email" HeaderText="TeamLeader" SortExpression="TeamLeader_email">
                            <ItemStyle Wrap="True" />
                        </asp:BoundField>
                        <asp:BoundField DataField="BA_email" HeaderText="BuyerAssistant" SortExpression="BA_email">
                            <ItemStyle Wrap="True" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Other_email" HeaderText="Other" SortExpression="Other_email">
                            <ItemStyle Wrap="True" />
                        </asp:BoundField>
                        <asp:CommandField ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>
                &nbsp;
                <asp:Label ID="MsgLabel" runat="server" Font-Size="Medium" SkinID="SumLabel" Style="position: static"
                    Width="264px"></asp:Label>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    SelectCommand="SELECT Team_No, Team_Name FROM Team ORDER BY Team_Name"></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    SelectCommand="SELECT Store_No, Store_Name FROM Store ORDER BY Store_Name">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    DeleteCommand="DELETE FROM SlimEmail WHERE (EmailID = @EmailID)" SelectCommand="SELECT SlimEmail.TeamLeader_email, SlimEmail.BA_email, SlimEmail.Other_email, SlimEmail.EmailID, Store.Store_Name, Team.Team_Name FROM SlimEmail INNER JOIN Store ON SlimEmail.Store_No = Store.Store_No INNER JOIN Team ON Team.Team_No = SlimEmail.Team_No ORDER BY SlimEmail.Insert_Date DESC"
                    UpdateCommand="UPDATE SlimEmail SET TeamLeader_email = @TeamLeader_email, BA_email = @BA_email, Other_email = @Other_email&#13;&#10;where EmailID = @EmailID">
                    <UpdateParameters>
                        <asp:Parameter Name="TeamLeader_email" />
                        <asp:Parameter Name="BA_email" />
                        <asp:Parameter Name="Other_email" />
                        <asp:Parameter Name="EmailID" />
                    </UpdateParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="EmailID" />
                    </DeleteParameters>
                </asp:SqlDataSource>
</asp:Content>

