<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_UserInterface_RetailCost_RetailCost" title="Retail Cost Detail" Codebehind="RetailCost.aspx.vb" %>

<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="position: static">
        <tr>
            <td style="width: 2px">
            </td>
            <td style="width: 100px">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Small" Font-Underline="True"
                    Style="position: static" Text="UPC:"></asp:Label></td>
            <td style="width: 100px">
                </td>
            <td align="right" style="width: 8px">
            </td>
            <td style="width: 100px" align="left">
                <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Small" Font-Underline="True"
                    Style="position: static" Text="Description:"></asp:Label></td>
            <td style="width: 5px">
            </td>
            <td style="width: 100px">
                </td>
            <td style="width: 100px">
                <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="Small" Font-Underline="True"
                    Style="position: static" Text="Store:"></asp:Label></td>
            <td style="width: 111px">
                </td>
            <td style="width: 111px">
            </td>
        </tr>
        <tr>
            <td style="width: 2px; height: 22px">
            </td>
            <td style="width: 100px; height: 22px;">
                <asp:Label ID="Label5" runat="server" Style="position: static" Text="Label" Font-Size="Small" Font-Bold="True"></asp:Label></td>
            <td style="width: 100px; height: 22px;">
            </td>
            <td style="width: 8px; height: 22px">
            </td>
            <td style="width: 100px; height: 22px;">
                <asp:Label ID="Label6" runat="server" Font-Size="Small" Style="position: static"
                    Text="Label" Font-Bold="True"></asp:Label></td>
            <td style="width: 5px; height: 22px">
            </td>
            <td style="width: 100px; height: 22px;">
                </td>
            <td style="width: 100px; height: 22px">
                <asp:Label ID="Label8" runat="server" Font-Size="Small" Style="position: static"
                    Text="Label" Font-Bold="True"></asp:Label></td>
            <td style="width: 111px; height: 22px;">
                </td>
            <td style="width: 111px; height: 22px">
            </td>
        </tr>
        <tr>
            <td style="width: 2px; height: 22px">
            </td>
            <td style="width: 100px; height: 22px">
            </td>
            <td style="width: 100px; height: 22px">
            </td>
            <td style="width: 8px; height: 22px">
            </td>
            <td style="width: 100px; height: 22px">
            </td>
            <td style="width: 5px; height: 22px">
            </td>
            <td style="width: 100px; height: 22px">
            </td>
            <td style="width: 100px; height: 22px">
            </td>
            <td style="width: 111px; height: 22px">
            </td>
            <td style="width: 111px; height: 22px">
            </td>
        </tr>
        <tr>
            <td style="width: 2px">
            </td>
            <td style="width: 100px">
                <asp:Label ID="NewPriceLbl" runat="server" Font-Bold="True" Font-Size="Small" Font-Underline="True"
                    Style="position: static" Text="New Price:"></asp:Label></td>
            <td style="width: 100px">
                <asp:Label ID="PriceMultLbl" runat="server" Font-Bold="True" Font-Size="Small" Font-Underline="True"
                    Style="position: static" Text="Price Multiple:"></asp:Label></td>
            <td style="width: 8px">
            </td>
            <td style="width: 100px">
                <asp:Label ID="NewUnitCostLbl" runat="server" Font-Bold="True" Font-Size="Small" Font-Underline="True"
                    Style="position: static" Text="New Cost:" Visible="False"></asp:Label></td>
            <td style="width: 5px">
            </td>
            <td style="width: 100px">
                </td>
            <td style="width: 100px">
                <asp:Label ID="NewCaseCost" runat="server" Font-Bold="True" Font-Size="Small" Font-Underline="True"
                    Style="position: static" Text="New Case Cost:"></asp:Label></td>
            <td style="width: 111px">
                <asp:Label ID="NewMarginLbl" runat="server" Font-Bold="True" Font-Size="Small" Font-Underline="True"
                    Style="position: static" Text="New CaseSize:"></asp:Label>
            </td>
            <td style="width: 111px">
            </td>
        </tr>
        <tr>
            <td style="width: 2px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 8px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 5px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 111px">
                </td>
            <td style="width: 111px">
            </td>
        </tr>
        <tr>
            <td align="right" style="width: 2px">
                $</td>
            <td style="width: 100px">
                <asp:TextBox ID="NewPriceTxBx" runat="server" Style="position: static" Width="40px" TabIndex="1" MaxLength="8"></asp:TextBox></td>
            <td style="width: 100px">
                <asp:TextBox ID="PriceMultipleTxBx" runat="server" Style="position: static" Width="25px" TabIndex="2" MaxLength="3"></asp:TextBox></td>
            <td align="right" style="width: 2px">
                </td>
            <td style="width: 100px">
                &nbsp;<asp:TextBox ID="NewUnitCostTxBx" runat="server" Style="position: static" TabIndex="3"
                    Width="40px" MaxLength="8" Visible="False"></asp:TextBox></td>
            <td align="right" style="width: 2px">
                </td>
            <td style="width: 100px" align="right">
                $&nbsp;</td>
            <td style="width: 100px">
            <asp:TextBox ID="NewCaseCostTxBx" runat="server" Style="position: static" TabIndex="4"
                    Width="40px" MaxLength="8"></asp:TextBox></td>
            <td style="width: 111px">
                <asp:TextBox ID="NewCaseSizeTxBx" runat="server" Style="position: static" TabIndex="5"
                    Width="40px" MaxLength="4" ToolTip="1-4 Digits"></asp:TextBox>
                </td>
            <td style="width: 111px">
            </td>
        </tr>
        <tr>
            <td style="width: 2px">
            </td>
            <td style="width: 100px">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="NewPriceTxBx"
                    ErrorMessage="Invalid Price" Font-Size="Small" Style="position: static" ValidationExpression="^\d{0,4}(\.)?\d{1,2}$"
                    Width="80px"></asp:RegularExpressionValidator></td>
            <td style="width: 100px">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="PriceMultipleTxBx"
                    ErrorMessage="Invalid Multiple:" Font-Size="Small" Style="position: static" ValidationExpression="\d{1,3}"></asp:RegularExpressionValidator></td>
            <td style="width: 8px">
            </td>
            <td style="width: 100px">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="NewUnitCostTxBx"
                    ErrorMessage="Invalid Cost" Font-Size="Small" Style="position: static" ValidationExpression="^\d{0,5}(\.)?\d{1,2}$"
                    Width="96px" Visible="False"></asp:RegularExpressionValidator></td>
            <td style="width: 5px">
            </td>
            <td style="width: 100px">
                </td>
            <td style="width: 100px">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="NewCaseCostTxBx"
                    ErrorMessage="Invalid Cost" Font-Size="Small" Style="position: static" ValidationExpression="^\d{0,4}(\.)?\d{1,2}$"
                    Width="96px"></asp:RegularExpressionValidator></td>
            <td style="width: 111px">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="NewCaseSizeTxBx"
                    ErrorMessage="Invalid CaseSize" Font-Size="Small" Style="position: static" ValidationExpression="\d{1,5}"
                    Width="96px"></asp:RegularExpressionValidator></td>
            <td style="width: 111px">
            </td>
        </tr>
        <tr>
            <td style="width: 2px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 8px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 5px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 100px">
            </td>
            <td style="width: 111px">
            </td>
            <td style="width: 111px">
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" runat="server" CellPadding="2" CellSpacing="2" Style="position: static" AutoGenerateColumns="False" DataKeyNames="item_key" DataSourceID="SqlDataSource1" EmptyDataText="No Items Found" EnableViewState="False">
        <Columns>
            <asp:BoundField DataField="Store_Name" HeaderText="StoreName" SortExpression="Store_Name" >
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="Multiple" HeaderText="Multi" SortExpression="Multiple" />
            <asp:BoundField DataField="price" DataFormatString="{0:c}" HeaderText="Price" HtmlEncode="False"
                SortExpression="price">
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>
            <asp:BoundField DataField="PriceChgTypeDesc" HeaderText="PType" SortExpression="PriceChgTypeDesc" />
            <asp:BoundField DataField="Vendor" HeaderText="Vendor" SortExpression="Vendor" />
            <asp:BoundField DataField="PackSize" HeaderText="CaseSize" SortExpression="PackSize" DataFormatString="{0:N}" HtmlEncode="False" />
            <asp:BoundField DataField="UnitCost" HeaderText="Cost" SortExpression="UnitCost" DataFormatString="{0:c}" HtmlEncode="False" >
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="Margin" HeaderText="Margin" SortExpression="Margin" DataFormatString="{0:p}" HtmlEncode="False" >
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="Allowances" DataFormatString="{0:c}" HeaderText="Allowances"
                HtmlEncode="False" SortExpression="Allowances" />
            <asp:BoundField DataField="Discounts" DataFormatString="{0:p}" HeaderText="Discounts"
                HtmlEncode="False" SortExpression="Discounts" />
            <asp:ButtonField CommandName="Select" Text="Submit" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="ItemWebQueryStoreDetail" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="0" Name="item_key" QueryStringField="ItemKey"
                Type="Int32" />
            <asp:SessionParameter DefaultValue="0" Name="Store_no" SessionField="Store_No" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="Navy"
        Style="position: static" Width="208px"></asp:Label>
    <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="Navy"
        Style="position: static" Width="208px"></asp:Label>
</asp:Content>

