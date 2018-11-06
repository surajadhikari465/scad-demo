<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_Administration_Other" title="Untitled Page" Codebehind="Other.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="position: static">
        <tr>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
                <asp:Label ID="Label1" runat="server" Style="position: static" Text="AAZ"></asp:Label></td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
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
        </tr>
    </table>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="EmailID"
        DataSourceID="ObjectDataSource1" EmptyDataText="Nothing!" Style="position: static">
        <Columns>
            <asp:CommandField ShowEditButton="True" />
            <asp:BoundField DataField="Store_No" HeaderText="Store_No" ReadOnly="True" SortExpression="Store_No" />
            <asp:BoundField DataField="Team_No" HeaderText="Team_No" ReadOnly="True" SortExpression="Team_No" />
            <asp:BoundField DataField="TeamLeader_email" HeaderText="TeamLeader" SortExpression="TeamLeader_email" />
            <asp:BoundField DataField="BA_email" HeaderText="BA" SortExpression="BA_email" />
            <asp:BoundField DataField="Other_email" HeaderText="Other" SortExpression="Other_email" />
            <asp:CommandField ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" InsertMethod="Insert"
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="SlimEmailTableAdapters.SlimEmailTableAdapter"
        UpdateMethod="UpdateEmail">
        <UpdateParameters>
            <asp:Parameter Name="TeamLeader_email" Type="String" />
            <asp:Parameter Name="BA_email" Type="String" />
            <asp:Parameter Name="Other_email" Type="String" />
            <asp:Parameter Name="EmailID" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="Store_No" Type="Int32" />
            <asp:Parameter Name="Team_No" Type="Int32" />
            <asp:Parameter Name="TeamLeader_email" Type="String" />
            <asp:Parameter Name="BA_email" Type="String" />
            <asp:Parameter Name="Other_email" Type="String" />
            <asp:Parameter Name="Insert_Date" Type="DateTime" />
        </InsertParameters>
    </asp:ObjectDataSource>
</asp:Content>

