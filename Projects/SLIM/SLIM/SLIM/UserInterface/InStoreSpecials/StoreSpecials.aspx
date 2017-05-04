<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_InStoreSpecials_StoreSpecials" title="Store Specials" Codebehind="StoreSpecials.aspx.vb" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table  cellspacing="0" cellpadding="2" width="500" class="SlimTable" >
       
        <tr class="header">
            <td >
                <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Size="12px"  Font-Names="Tahoma" Text="Sale Multiple:" Width="88px"></asp:Label>
            </td>
            <td style="width: 152px" >
                <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Size="12px"  Font-Names="Tahoma"
                     Text="Sale Price:" Width="88px"></asp:Label></td>
            <td >
                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="12px"  Font-Names="Tahoma"
                     Text="Start Date:"></asp:Label></td>
            <td >
                <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="12px"  Font-Names="Tahoma"
                     Text="End Date:"></asp:Label></td>
            <td>
            </td>
                     <td>&nbsp;</td>
        </tr>

        <tr>
            <td style="height: 47px" >
                <center><asp:TextBox ID="SaleMultiple" runat="server"  MaxLength="3" Width="40px"></asp:TextBox></center></td>
            <td style="width: 152px; height: 47px" >
                &nbsp;<asp:TextBox ID="SalePrice" runat="server"  MaxLength="7" Width="88px"></asp:TextBox></td>
            <td style="height: 47px" >
                <igsch:WebDateChooser ID="StartDate" runat="server"  Font-Names="Tahoma" Font-Size="12px" NullDateLabel="">
                    <CalendarLayout HideOtherMonthDays="True">
                        <CalendarStyle Font-Size="X-Small">
                        </CalendarStyle>
                    </CalendarLayout>
                    <ExpandEffects Duration="150" Type="Fade" />
                </igsch:WebDateChooser>
            </td>
            <td style="height: 47px" >
                <igsch:WebDateChooser ID="EndDate" runat="server" NullDateLabel="" >
                    <CalendarLayout>
                        <CalendarStyle Font-Size="X-Small">
                        </CalendarStyle>
                    </CalendarLayout>
                </igsch:WebDateChooser>
            </td>
            <td style="height: 47px">
                &nbsp;<asp:Button ID="Button_Apply" runat="server" Text="Apply" /></td>
            <td style="height: 47px">
                <asp:CheckBox ID="CheckBox_All" runat="server" AutoPostBack="True" Text="All" /></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="SalePrice"
                    ErrorMessage="Invalid Price" Font-Size="Small"  ValidationExpression="\d{0,4}(\.)?\d{0,2}"
                    Width="80px" ValidationGroup="All"></asp:RegularExpressionValidator></td>
            <td >
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="StartDate"
                    ErrorMessage="StartDate Required" Font-Size="Small" ValidationGroup="All" 
                    ></asp:RequiredFieldValidator></td>
            <td >
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="EndDate"
                    ErrorMessage="EndDate Required" Font-Size="Small"  Width="98px" ValidationGroup="All"></asp:RequiredFieldValidator></td>
            <td>
            </td>
                    <td>&nbsp;</td>
        </tr>

    </table>
    <asp:Literal ID="Literal_Message" runat="server"></asp:Literal><br />
    <asp:Label ID="Label_Message" runat="server"></asp:Label>
    <br />
    <asp:Button runat="server" ID="Button_Submit" Text="Submit" Font-Names="Tahoma" Font-Size="12px" /><br />
    <table cellpadding="3" cellspacing="0" border="0">
    <tr style="font-family:Tahoma; font-size:12px; font-weight:bold;"><td>Current Price Information</td></tr>
    <tr><td>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" RowStyle-Font-Names="Tahoma" HeaderStyle-Font-Names="Tahoma"  HeaderStyle-HorizontalAlign="Center">
            <Columns>
                <asp:CommandField ShowDeleteButton="True" />
                <asp:BoundField DataField="item_key" HeaderText="item_key" SortExpression="item_key" />
                <asp:BoundField DataField="Identifier" HeaderText="Identifier" SortExpression="Identifier" />
                <asp:BoundField DataField="Item_Description" HeaderText="Description" SortExpression="Item_Description" />
                <asp:BoundField DataField="Multiple" HeaderText="Multiple" SortExpression="Multiple" />
                <asp:BoundField DataField="price" DataFormatString="{0:c}" HeaderText="Price" SortExpression="price" HtmlEncode="False" />
                <asp:BoundField DataField="Vendor" HeaderText="Vendor" SortExpression="Vendor" />
                <asp:BoundField DataField="PackSize" HeaderText="Case Size" SortExpression="PackSize" />
                <asp:BoundField DataField="UnitCost" HeaderText="Unit Cost" SortExpression="UnitCost" />
                <asp:BoundField DataField="Margin" HeaderText="Margin" ReadOnly="True" SortExpression="Margin" />
                <asp:BoundField DataField="Allowances" HeaderText="Allowances" ReadOnly="True" SortExpression="Allowances" Visible="False" />
                <asp:BoundField DataField="Discounts" HeaderText="Discounts" ReadOnly="True" SortExpression="Discounts" Visible="False" />
                <asp:BoundField DataField="PriceChgTypeDesc" HeaderText="Price Type" SortExpression="PriceChgTypeDesc" Visible="False" />
                <asp:BoundField DataField="SubTeam_Name" HeaderText="SubTeam_Name" SortExpression="SubTeam_Name" />
                
                <asp:TemplateField HeaderText="Sale Multiple">
                    <ItemTemplate>
                        <center><asp:TextBox ID="txtSaleMultiple" runat="server" Width="20" MaxLength="3"></asp:TextBox></center>
                    </ItemTemplate>                
                    <HeaderStyle HorizontalAlign="Center" Width="20px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sale Price">
                    <ItemTemplate>
                        <asp:TextBox ID="txtSalePrice" runat="server" Width="70" MaxLength="7"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Start Date">
                    <ItemTemplate>
                        <igsch:WebDateChooser ID="dtpStartDate" runat="server"  Font-Names="Tahoma" Font-Size="12px" NullDateLabel="">
                            <CalendarLayout HideOtherMonthDays="True">
                                <CalendarStyle Font-Size="X-Small">
                                </CalendarStyle>
                            </CalendarLayout>
                            <ExpandEffects Duration="150" Type="Fade" />
                        </igsch:WebDateChooser>
                    </ItemTemplate>                 
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="End Date">
                    <ItemTemplate>
                        <igsch:WebDateChooser ID="dtpEndDate" runat="server"  Font-Names="Tahoma" Font-Size="12px" NullDateLabel="">
                            <CalendarLayout HideOtherMonthDays="True">
                                <CalendarStyle Font-Size="X-Small">
                                </CalendarStyle>
                            </CalendarLayout>
                            <ExpandEffects Duration="150" Type="Fade" />
                        </igsch:WebDateChooser>
                    </ItemTemplate>            
                </asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="#004000" Font-Names="Tahoma" HorizontalAlign="Center" />
            <RowStyle Font-Names="Tahoma" />
        </asp:GridView>
</td></tr>
</table>
         
             <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="ItemWebQueryStoreDetail" SelectCommandType="StoredProcedure" DeleteCommand="--NOTE: This is only here because it is required that something goes here.  The actual deletion is in --the code, it removes an item_key from a list of all the item keys.&#13;&#10;select top 1 * from item">
        <SelectParameters>
            <asp:SessionParameter DefaultValue="" Name="ItemKeyList" SessionField="ISSItemKeyList"
                Type="String" />
            <asp:SessionParameter DefaultValue="" Name="IdentifierIDList" SessionField="ISSIdentifierIDList"
                Type="String" />
            <asp:SessionParameter DefaultValue="0" Name="Store_no" SessionField="Store_No" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>

</asp:Content>

