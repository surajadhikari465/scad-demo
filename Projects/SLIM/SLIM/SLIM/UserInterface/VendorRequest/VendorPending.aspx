<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_VendorRequest_VendorPending" title="Vendor Requests Pending" Codebehind="VendorPending.aspx.vb" %>
<%@ Register TagPrefix="igtblexp" Namespace="Infragistics.WebUI.UltraWebGrid.ExcelExport" Assembly="Infragistics2.WebUI.UltraWebGrid.ExcelExport.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
     <%@ MasterType VirtualPath="~/UserInterface/MasterPage.master"%>   
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" src="../../javascript/AutosizeWebGridColumns.js"></script>
<script language="javascript" type="text/javascript" >

function UltraWebGrid1_InitializeLayoutHandler(gridName){
	setColWidths(gridName);
}

function popitup(url) {
	newwindow=window.open(url,'name','height=320,width=250');
	if (window.focus) {newwindow.focus()}
	
}

</script>
    <asp:GridView ID="GridView1" runat="server" Style="position: static" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="2" CellSpacing="2" DataKeyNames="VendorRequest_ID" DataSourceID="SqlDataSource1" EmptyDataText="No Items Found">
        <PagerSettings Mode="NextPreviousFirstLast" />
        <Columns>
            <asp:CommandField ShowEditButton="True" />
            <asp:BoundField DataField="CompanyName" HeaderText="Vendor" ReadOnly="True" SortExpression="CompanyName" />
            <asp:TemplateField HeaderText="Vendor_Key" SortExpression="Vendor_Key">
                <EditItemTemplate>
                    &nbsp;<asp:TextBox ID="TextBox1" runat="server" MaxLength="8" Text='<%# Bind("Vendor_Key") %>'
                        ToolTip="8 Alphanumeric Chars" Width="72px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TextBox1"
                        ErrorMessage="Invalid Vendor Key" Style="position: static" ValidationExpression="[a-zA-Z0-9\s]{2,8}"></asp:RegularExpressionValidator>
                    <asp:CustomValidator ID="requireVendorKey" runat="server" ErrorMessage="Required" OnServerValidate="ValidateVendorKey" ControlToValidate="TextBox1" ValidateEmptyText="True"
></asp:CustomValidator>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Vendor_Key") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="PS_Vendor_ID" SortExpression="PS_Vendor_ID">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("PS_Vendor_ID") %>' ToolTip="10 Numbers"
                        Width="72px" MaxLength="10"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TextBox2"
                        ErrorMessage="Invalid PS Vendor ID" Style="position: static" Text='<%# Eval("PS_Vendor_ID") %>'
                        ValidationExpression="\d{10}"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox2"
                        ErrorMessage="Required"></asp:RequiredFieldValidator><br />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("PS_Vendor_ID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="PS_Export_Vendor_ID" SortExpression="PS_Export_Vendor_ID">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("PS_Export_Vendor_ID") %>' ToolTip="10 Numbers"
                        Width="72px" MaxLength="10"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="TextBox3"
                        ErrorMessage="Invalid PS Export Vendor ID" Style="position: static" Text='<%# Eval("PS_Export_Vendor_ID") %>'
                        ValidationExpression="\d{10}"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBox3"
                        ErrorMessage="Required"></asp:RequiredFieldValidator><br />
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("PS_Export_Vendor_ID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Insert_Date" DataFormatString="{0:d}" HeaderText="Insert_Date"
                HtmlEncode="False" ReadOnly="True" SortExpression="Insert_Date">
                <ItemStyle Wrap="False" />
            </asp:BoundField>
            <asp:CheckBoxField DataField="Ready_To_Apply" HeaderText="ToApply" SortExpression="Ready_To_Apply" />
            <asp:HyperLinkField DataNavigateUrlFields="VendorRequest_ID" DataNavigateUrlFormatString="VendorDetail.aspx?VendorRequest_ID={0}"
                Text="Details" />
            <asp:CommandField ShowDeleteButton="True" />
            <asp:BoundField DataField="VendorRequest_ID" HeaderText="VendorRequest_ID" SortExpression="VendorRequest_ID"
                Visible="False" />
        </Columns>
    </asp:GridView>
      
    
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        DeleteCommand="DELETE FROM VendorRequest WHERE (VendorRequest_ID = @VendorRequest_ID)"
        SelectCommand="SELECT VendorRequest_ID, VendorStatus_ID, Vendor_Key, CompanyName, PS_Vendor_ID, PS_Export_Vendor_ID, Insert_Date, Ready_To_Apply FROM VendorRequest WHERE (VendorStatus_ID = @VendorStatus_ID) ORDER BY Insert_Date DESC"
        UpdateCommand="UPDATE VendorRequest SET Vendor_Key = upper(@Vendor_Key), PS_Vendor_ID = @PS_Vendor_ID, PS_Export_Vendor_ID = @PS_Export_Vendor_ID, &#13;&#10; Ready_To_Apply = @Ready_To_Apply&#13;&#10; WHERE (VendorRequest_ID = @VendorRequest_ID)">
        <DeleteParameters>
            <asp:Parameter Name="VendorRequest_ID" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Vendor_Key" />
            <asp:Parameter Name="PS_Vendor_ID" />
            <asp:Parameter Name="PS_Export_Vendor_ID" />
            <asp:Parameter Name="Ready_To_Apply" />
            <asp:Parameter Name="VendorRequest_ID" />
        </UpdateParameters>
        <SelectParameters>
            <asp:Parameter DefaultValue="1" Name="VendorStatus_ID" Type="Int16" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:Label ID="MsgLabel" runat="server" Font-Size="Small" SkinID="SumLabel" Style="position: static"></asp:Label>
</asp:Content>

