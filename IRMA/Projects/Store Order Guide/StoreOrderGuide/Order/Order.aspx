<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Order.aspx.vb" Inherits="StoreOrderGuide.OrderInterface" MasterPageFile="~/StoreOrderGuide.Master" Title="Order" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" Runat="Server">

<script language="javascript" type="text/javascript">
//update the order totals when the page first loads
document.body.onload = function () {
    var txtOrderCount = document.getElementById('mainContent_dvCatalogDetail_txtOrderCount');
    var txtOrderTotal = document.getElementById('mainContent_dvCatalogDetail_txtOrderTotal');
    var gvOrderItems = document.getElementById('mainContent_gvCatalogItemList');

    if (gvOrderItems) {
        var iRowCount = gvOrderItems.rows.length;
    } else {
        var iRowCount = 0;
    }

    var iRowIndex = 1;
    var iOrderCount = 0;
    var iOrderTotal = 0;

    for (iRowIndex; iRowIndex <= iRowCount - 1; iRowIndex++) {
        var rowElement = gvOrderItems.rows[iRowIndex];
        rowElement.cells[10].firstChild.value = 0;
    }

    if (txtOrderCount) {
        txtOrderCount.innerText = iOrderCount;
    }

    if (txtOrderTotal) {
        txtOrderTotal.innerText = iOrderTotal;
    }    
}

function submitButtonClick(button) {
     if (typeof(Page_ClientValidate) == 'function') { 
        if (Page_ClientValidate() == false) { 
            return false;
        }
     }
     
     var button2 = document.getElementById('ctl00_mainContent_btnCancel');
     
     button.value = 'Pushing...';
     button.style.display = 'none'; 
     
     if (button2){
        button2.value = 'Pushing...';
        button2.style.display = 'none'; 
     }

     return true;
}

function disableEnterKey(e) {
    var key;
    
    if (window.event)
        key = window.event.keyCode; //IE
    else
        key = e.which; //FF

    return (key != 13);
}

function set_focus(sName,keyCode) {
    if (keyCode==38 || keyCode==40) 
    {
        var ctlArray = sName.split("$");
        var ctl = ctlArray[3]
        var iIndex = ctl.substring(3,ctl.length);

        if (keyCode==38)
            {
            iIndex = Number(iIndex) - 3;
            }
        else if (keyCode==40)
            {
            iIndex = Number(iIndex) - 1;
            }
            
        var sIndex = String(iIndex);
        if (sIndex.length == 1)
	        {
	        sIndex = sIndex;
	        }

	    var tempName = "mainContent_gvCatalogItemList_txtQuantity_" + iIndex;
        
        if (document.getElementById(tempName).disabled)
            {
            set_focus(tempName.replace(/_/g,"$"),keyCode);
            }
        else
            {
            document.getElementById(tempName).focus();
            }
    }
}

function isInteger(sText) {
    var sValidChars     = "0123456789";
    var isNumber        = true;
    var iChar;

    for (i = 0; i < sText.length && isNumber == true; i++) { 
        iChar = String(sText.charAt(i));
                
        if (sValidChars.indexOf(iChar) == -1) {
            isNumber = false;
            return false;
        }                
    }
    return true;
}

function updateOrderTotals() {
    var txtOrderCount   = document.getElementById('mainContent_dvCatalogDetail_txtOrderCount');
    var txtOrderTotal   = document.getElementById('mainContent_dvCatalogDetail_txtOrderTotal');
    var gvOrderItems    = document.getElementById("mainContent_gvCatalogItemList");

    if (gvOrderItems) {
        var iRowCount = gvOrderItems.rows.length;
    }else{
        var iRowCount = 0;
    }

    var iRowIndex       = 1; 
    var iOrderCount     = 0; 
    var iOrderTotal     = 0; 

    for (iRowIndex; iRowIndex <= iRowCount -1; iRowIndex++) {        
        var rowElement = gvOrderItems.rows[iRowIndex];
        var nQty = rowElement.cells[10].firstChild.value;
        var nCst = String(rowElement.cells[7].innerText).substr(1).replace(/,/, "");
                      
        if (isInteger(nQty) == true) {
            iOrderCount = Number(iOrderCount) + Number(nQty);
            
            if (nCst) {
                iOrderTotal = Number(iOrderTotal) + (Number(nQty) * Number(nCst));
            }  
        }else{
            rowElement.cells[10].firstChild.value = 0;
            alert("Value input is not a valid quantity and has been reset to 0.");
        }
    }
    
    txtOrderCount.innerText = iOrderCount;
    txtOrderTotal.innerText = iOrderTotal;
}
</script>


<!--########## DATA SOURCES ########## -->
<asp:ObjectDataSource ID="dsCatalog" runat="server" SelectMethod="GetCatalogs" TypeName="StoreOrderGuide.Catalog" >
    <SelectParameters>
        <asp:Parameter Name="StoreID" Type="Int32" />
        <asp:Parameter Name="SubTeamID" DefaultValue="0" Type="Int32" />
        <asp:Parameter Name="ZoneID" DefaultValue="0" Type="Int32" />
        <asp:Parameter Name="Published" DefaultValue="True" Type="Boolean" />
        <asp:ControlParameter ControlID="txtCatalogCode" Name="CatalogCode" PropertyName="Text" Type="String" />
        <asp:Parameter Name="Order" DefaultValue="True" Type="Boolean" />
        <asp:Parameter Name="CatalogID" Type="Int32" DefaultValue="0" />
    </SelectParameters>
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="dsCatalogItems" runat="server" SelectMethod="GetCatalogItems" TypeName="StoreOrderGuide.CatalogItem">
    <SelectParameters>
        <asp:ControlParameter ControlID="dvCatalogDetail" Name="CatalogID" PropertyName="SelectedValue" Type="Int32" />
        <asp:Parameter Name="StoreNo" Type="Int32" />
        <asp:Parameter Name="Order" DefaultValue="True" Type="Boolean" />
        <asp:ControlParameter ControlID="txtIdentifierSearch" Name="Identifier" PropertyName="Text"
            Type="String" />
        <asp:ControlParameter ControlID="txtItemDescriptionSearch" Name="Description" PropertyName="Text"
            Type="String" />
        <asp:ControlParameter ControlID="ddlSubTeamSearch" Name="SubTeamID" PropertyName="SelectedValue"
            Type="Int32" />
        <asp:ControlParameter ControlID="ddlClassSearch" Name="ClassID" PropertyName="SelectedValue"
            Type="Int32" />
        <asp:ControlParameter ControlID="ddlLevel3Search" Name="Level3ID" PropertyName="SelectedValue"
            Type="Int32" />
        <asp:ControlParameter ControlID="ddlBrandSearch" Name="BrandID" PropertyName="SelectedValue"
            Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="dsCatalogSchedule" runat="server" SelectMethod="GetCatalogSchedules" TypeName="StoreOrderGuide.CatalogSchedule">
    <SelectParameters>
        <asp:Parameter Name="CatalogScheduleID" Type="Int32" />
        <asp:Parameter Name="ManagedByID" Type="Int32" />
        <asp:Parameter Name="StoreNo" Type="Int32" />
        <asp:Parameter Name="SubTeamNo" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="dsOrder" runat="server" UpdateMethod="SetOrder" InsertMethod="AddOrder" TypeName="StoreOrderGuide.Order">
    <UpdateParameters>
        <asp:Parameter Name="CatalogOrderID" Type="Int32" />
    </UpdateParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="dvCatalogDetail" Name="CatalogID" PropertyName="SelectedValue" Type="Int32" />
        <asp:Parameter Name="VendorID" Type="Int32" />
        <asp:Parameter Name="StoreID" Type="Int32" />
        <asp:SessionParameter Name="UserID" SessionField="UserID" Type="Int32" />
        <asp:Parameter Name="FromSubTeamID" Type="Int32" />
        <asp:Parameter Name="ToSubTeamID" Type="Int32" />
        <asp:Parameter Name="CatalogOrderID" Direction="ReturnValue" Type="Int32" />
        <asp:Parameter Name="ExpectedDate" Type="DateTime" />
    </InsertParameters>
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="dsOrderItems" runat="server" InsertMethod="AddOrderItem" TypeName="StoreOrderGuide.OrderItem">
    <InsertParameters>
        <asp:Parameter Name="CatalogOrderID" Type="Int32" />
        <asp:ControlParameter ControlID="gvCatalogItemList" Name="CatalogItemID" PropertyName="SelectedValue" Type="Int32" />        
        <asp:Parameter Name="Quantity" Type="Int32" />
    </InsertParameters>
</asp:ObjectDataSource>


<!--########## FILTERS ########## -->
<div id="filters">
<asp:Panel ID="pnlFilter" runat="server" Visible="True">
    <div class="subtitle">Catalog Filters</div>
    <table cellpadding="2" cellspacing="0">
      <tr>
        <td>Store*</td>
        <td>Catalog Code</td>
      </tr>
      <tr>
        <td>
            <asp:Label ID="lblStore" runat="server" Visible="True" />
            <asp:DropDownList ID="ddlStoreFilter" runat="server" DataTextField="StoreName" DataValueField="StoreID" Visible="False" AutoPostBack="True"/>
        </td>
        <td><asp:TextBox ID="txtCatalogCode" runat="server" SkinID="SOGTextBox" /></td>
      </tr>
    </table>
    
    <asp:Button ID="btnFilter" runat="server" SkinID="SOGButton" Text="Filter" />
</asp:Panel>
</div>


<!--########## CATALOG ########## -->
<div id="orderlist">
<asp:Panel ID="pnlCatalog" runat="server" Visible="True">
    <div class="subtitle">Catalogs</div>
    <asp:GridView ID="gvCatalogList" runat="server" EmptyDataText="No catalogs were found." EnableViewState="False" SkinID="SOGGridView" DataKeyNames="CatalogID" DataSourceID="dsCatalog" AllowPaging="True" AllowSorting="True">
         <Columns>
            <asp:CommandField ShowSelectButton="True" SelectText="Order" ItemStyle-CssClass="cmdField" ItemStyle-ForeColor="#284775" />
            <asp:TemplateField ShowHeader="false" ItemStyle-CssClass="cmdField">
                <ItemTemplate>
                    <nobr>
                        <asp:LinkButton ID="btnPrint" runat="server" ForeColor="#0065A5" CommandName="btnPrint" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" Text="Print" ToolTip="Print this Catalog" />
                    </nobr>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CatalogID" SortExpression="CatalogID" HeaderText="CatalogID" ReadOnly="True" />
            <asp:BoundField DataField="CatalogCode" HeaderText="CatalogCode" SortExpression="CatalogCode" />
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
            <asp:BoundField DataField="Details" HeaderText="Details" SortExpression="Details" />
            <asp:BoundField DataField="ManagedBy" HeaderText="ManagedBy" SortExpression="ManagedBy" />
            <asp:BoundField DataField="ExpectedDate" HeaderText="ExpectedDate" SortExpression="ExpectedDate" />
            <asp:BoundField DataField="SubTeamName" HeaderText="SubTeam" SortExpression="SubTeamName" />
        </Columns>
    </asp:GridView>
</asp:Panel>
</div>


<!--########## ORDERITEM ########## -->
<div id="orderdetails">
<asp:Panel ID="pnlOrderDetails" runat="server" Visible="False">
    <div id="orderdetailsbuttons">
    <asp:Button ID="btnPush" runat="server" SkinID="SOGButton" Text="Push" />
    <asp:Button ID="btnCancel" runat="server" SkinID="SOGButton" Text="Cancel" />
    </div>   

    <div id="divdetailsdata">
    <div class="subtitle">Order Details</div>   
    <asp:DetailsView ID="dvCatalogDetail" runat="server" EnableViewState="false" SkinID="SOGDetailView" AutoGenerateRows="False" DefaultMode="ReadOnly" DataSourceID="dsCatalog" DataKeyNames="CatalogID">
        <Fields>
            <asp:BoundField DataField="CatalogID" HeaderText="CatalogID" ReadOnly="True" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:BoundField DataField="ManagedByID" HeaderText="ManagedByID" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:BoundField DataField="SubTeam" HeaderText="SubTeam" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:BoundField DataField="CatalogCode" HeaderText="CatalogCode" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:BoundField HeaderText="Store" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:TemplateField HeaderText="Items Ordered">
                <ItemTemplate>
                    <asp:TextBox ID="txtOrderCount" runat="server" Text="0" Enabled="False" Width="75px"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Order Total $">
                <ItemTemplate>
                    <asp:TextBox ID="txtOrderTotal" runat="server" Text="0" Enabled="False" Width="75px" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ExpectedDate" HeaderText="ExpectedDate"/>
        </Fields>
    </asp:DetailsView>
    </div>
    
    <div id="orderdetailscalendar">
    <div class="subtitle">Expected Date</div>
    <asp:Label ID="lblExpectedDate" runat="server" Text="0" Visible="False" />
    <asp:Calendar ID="calExpectedDate" runat="server" ToolTip="Enter the expected date for the Order." SkinID="SOGCalendar" Visible="False" Width="200px" />
    </div>
    
    <div id="orderdetailsitems">
<!--    <div id="orderitemsfilter">
    <div class="subtitle">Order Item Filters</div>
    <table cellpadding="2" cellspacing="0">
      <tr>
        <td>Identifier</td>
        <td>Description</td>
        <td>SubTeam*</td>
        <td>Class*</td>
        <td>Level 3</td>
        <td>Brand</td>
        <td></td>
      </tr>
      <tr>
        <td style="width: 50px"><asp:TextBox ID="txtIdentifierSearch" runat="server" SkinID="SOGFilterTextBox_Identifier"/></td>
        <td style="width: 50px"><asp:TextBox ID="txtItemDescriptionSearch" runat="server" SkinID="SOGFilterTextBox_Desc"/></td>
        <td style="width: 50px"><asp:DropDownList ID="ddlSubTeamSearch" runat="server" SkinID="SOGDropDownList" DataTextField="SubTeamName" DataValueField="SubTeamID" Width="130px" AutoPostBack="True" /></td>
        <td style="width: 50px"><asp:DropDownList ID="ddlClassSearch" runat="server" SkinID="SOGDropDownList" DataTextField="ClassName" DataValueField="ClassID" Width="130px" AutoPostBack="True" /></td>
        <td style="width: 50px"><asp:DropDownList ID="ddlLevel3Search" runat="server" SkinID="SOGDropDownList" DataTextField="Level3Name" DataValueField="Level3ID" Width="130px" /></td>
        <td style="width: 50px"><asp:DropDownList ID="ddlBrandSearch" runat="server" SkinID="SOGDropDownList" DataTextField="BrandName" DataValueField="BrandID" Width="130px" /></td>
        <td style="width: 50px"><asp:Button ID="btnSearch" runat="server" SkinID="SOGButton" Text="Search" /></td>
      </tr>
    </table>        
    </div>        

    <br />
    <br />
    <br />
    <br />-->

    <div class="subtitle">Order Items</div>
    <asp:GridView ID="gvCatalogItemList" runat="server" EmptyDataText="No items were found." EnableViewState="false" SkinID="SOGGridView" DataSourceID="dsCatalogItems" DataKeyNames="CatalogItemID" ShowFooter="False" AllowPaging="False" AllowSorting="False" PageSize="2000">
         <Columns>
            <asp:BoundField DataField="CatalogItemID" Visible="False" />
            <asp:BoundField DataField="Identifier" HeaderText="Identifier" SortExpression="Identifier" ReadOnly="True" />
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" ReadOnly="True" />
            <asp:BoundField DataField="SubTeamName" HeaderText="SubTeam" SortExpression="SubTeamName" ReadOnly="True" />
            <asp:BoundField DataField="Class" HeaderText="Class" SortExpression="Class" ReadOnly="True" />
            <asp:BoundField DataField="Level3" HeaderText="Level3" SortExpression="Level3" ReadOnly="True" />
            <asp:BoundField DataField="CasePack" HeaderText="CasePack" SortExpression="CasePack" ReadOnly="True" DataFormatString="{0:N0}" />
            <asp:BoundField DataField="RetailUnit" HeaderText="Retail" SortExpression="RetailUnit" ReadOnly="True" />
            <asp:BoundField DataField="Cost" HeaderText="Cost" SortExpression="Cost" ReadOnly="True" DataFormatString="{0:C}" />
            <asp:BoundField DataField="NotAvailable" HeaderText="N/A" SortExpression="NotAvailable" ReadOnly="True" />
            <asp:BoundField DataField="ItemNote" HeaderText="Note" SortExpression="ItemNote" ReadOnly="True" />
            <asp:TemplateField HeaderText="Order Qty" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri">
                <ItemTemplate>
                    <asp:TextBox ID="txtQuantity" runat="server" Text="0" ToolTip="Enter the quantity to order" Width="35px" MaxLength="4" OnBlur="javascript:updateOrderTotals();" OnClick="this.value='';" OnKeyPress="return disableEnterKey(event);" OnKeyDown="set_focus(this.name,event.keyCode);" onFocus="this.select();"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Authorized" HeaderText="Auth" SortExpression="Authorized" Visible="True"/>
        </Columns>
    </asp:GridView>
    </div> 
</asp:Panel>
</div>

</asp:Content>