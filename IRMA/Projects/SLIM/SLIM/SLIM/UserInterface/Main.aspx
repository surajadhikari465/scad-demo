<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_Main" Title="Store Level Item Maintenance" CodeBehind="Main.aspx.vb" %>

<%@ MasterType VirtualPath="~/UserInterface/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    &nbsp;<table border="0" style="width: 432px; position: static; height: 104px">
        <tr>
            <td style="width: 139px"></td>
        </tr>
        <tr>
            <td style="width: 139px; height: 21px;">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="18px" Font-Underline="False"
                    Style="position: static" Text="Welcome to Store Level Item Maintenance" Width="396px" Font-Names="tahoma"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 139px">
                <br />
                <asp:Panel ID="pnlUserInfo" runat="server">
                    <table border="0" cellpadding="1" style="width: 240px; position: static">
                        <tr>
                            <td style="width: 100px; font-family: Tahoma; font-size: 12px;">UserID:</td>
                            <td style="width: 100px; font-family: Tahoma; font-size: 12px;">
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="12px" Style="position: static">UserID</asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 100px; font-family: Tahoma; font-size: 12px;">User Name</td>
                            <td style="width: 100px; font-family: Tahoma; font-size: 12px;">
                                <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="12px" Style="position: static">TestUser</asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 100px; font-family: Tahoma; font-size: 12px;">Store</td>
                            <td style="width: 100px; font-family: Tahoma; font-size: 12px;">
                                <asp:DropDownList ID="StoreDropDown" runat="server" Font-Size="12px" Font-Names="Tahoma"
                                    DataTextField="Store_Name" DataValueField="Store_No" Style="position: static" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; font-family: Tahoma; font-size: 12px;">AccessLevel</td>
                            <td style="width: 100px; font-family: Tahoma; font-size: 12px;">&nbsp;<asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="12px" Font-Names="tahoma" Style="position: static"></asp:Label></td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="ErrorLabel" runat="server" Font-Size="12px" Font-Names="tahoma" SkinID="SumLabel" Style="position: static"></asp:Label>
                </asp:Panel>
                <br />
            </td>
        </tr>
    </table>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="GetStores" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="select Store.Store_Name, Store.Store_No from Store inner join Users on Users.Telxon_Store_Limit = Store.Store_No where Users.User_ID = @user_id">
        <SelectParameters>
            <asp:SessionParameter Name="user_id" SessionField="UserID" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

