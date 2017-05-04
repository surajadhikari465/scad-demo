<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_RefreshPOS_RefreshPending" title="Refresh Requests Pending" Codebehind="RefreshPending.aspx.vb" %>
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
    <asp:GridView ID="GridView1" runat="server" Style="position: static" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="True" CellPadding="2" CellSpacing="2" DataSourceID="SqlDataSource1" EmptyDataText="No Items Found" PageSize="50">
        <Columns>
            <asp:BoundField DataField="Item" HeaderText="Item" ReadOnly="True" SortExpression="Item" />
            <asp:BoundField DataField="Store" HeaderText="Store" ReadOnly="True" SortExpression="Store" />
            <asp:BoundField DataField="User" HeaderText="User" ReadOnly="True" SortExpression="User" />
            <asp:BoundField DataField="Date" HeaderText="Date" ReadOnly="True" SortExpression="Date" />
            <asp:BoundField DataField="Reason" HeaderText="Reason" ReadOnly="True" SortExpression="Reason" />            
        </Columns>
    </asp:GridView>      
    
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="
                    SELECT 
	                    [Item]		=	i.Item_Description,
	                    [Store]		=	s.Store_Name,
	                    [User]		=	u.UserName,
	                    [Date]		=	CONVERT(varchar(10), sir.InsertDate, 1),
	                    [Reason]	=	sir.Reason
                    FROM 
	                    StoreItem				(nolock) si
	                    JOIN StoreItemRefresh	(nolock) sir	ON si.StoreItemAuthorizationID	= sir.StoreItemAuthorizationID
	                    JOIN Users				(nolock) u		ON sir.UserID					= u.User_ID
	                    JOIN Store				(nolock) s		ON si.Store_No					= s.Store_No
	                    JOIN Item				(nolock) i		ON si.Item_Key					= i.Item_Key
	                WHERE
						si.Refresh = 1
                    ">
    </asp:SqlDataSource>
    
    <asp:Label ID="MsgLabel" runat="server" Font-Size="Small" SkinID="SumLabel" Style="position: static"></asp:Label>
</asp:Content>

