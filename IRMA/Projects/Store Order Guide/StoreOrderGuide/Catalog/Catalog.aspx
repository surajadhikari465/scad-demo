<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Catalog.aspx.vb" Inherits="StoreOrderGuide.CatalogInterface" MasterPageFile="~/StoreOrderGuide.Master" Title="Catalog" StylesheetTheme="StoreOrderGuide" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" Runat="Server">

<!--########## DATA SOURCES ########## -->
<asp:ObjectDataSource ID="dsCatalog" runat="server" SelectMethod="GetCatalogs" DeleteMethod="DelCatalog" UpdateMethod="SetCatalog" InsertMethod="AddCatalog" TypeName="StoreOrderGuide.Catalog" >
    <SelectParameters>
        <asp:ControlParameter ControlID="ddlZoneFilter" Name="ZoneID" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter ControlID="ddlStoreFilter" Name="StoreID" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter ControlID="ddlSubTeamFilter" Name="SubTeamID" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter ControlID="chkPublishedFilter" Name="Published" PropertyName="Checked" Type="Boolean" />
        <asp:Parameter Name="CatalogCode" DefaultValue="0" Type="String" />
        <asp:Parameter Name="Order" DefaultValue="False" Type="Boolean" />
        <asp:Parameter Name="CatalogID" Type="Int32" />
    </SelectParameters>
    <DeleteParameters>
        <asp:ControlParameter ControlID="gvCatalogList" Name="CatalogID" PropertyName="SelectedValue" Type="Int32" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:ControlParameter ControlID="dvCatalogDetail" Name="CatalogID" PropertyName="SelectedValue" Type="Int32" />
        <asp:Parameter Name="ManagedByID" Type="Int32" />
        <asp:Parameter Name="CatalogCode" Type="String" />
        <asp:Parameter Name="Description" Type="String" />
        <asp:Parameter Name="Details" Type="String" />
        <asp:Parameter Name="Published" Type="Boolean" />
        <asp:Parameter Name="SubTeam" Type="Int32" />
        <asp:Parameter Name="ExpectedDate" Type="Boolean" />
        <asp:SessionParameter Name="UpdateUser" SessionField="UserName" Type="String" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="ManagedByID" Type="Int32" />
        <asp:Parameter Name="CatalogCode" Type="String" />
        <asp:Parameter Name="Description" Type="String" />
        <asp:Parameter Name="Published" Type="Boolean" />
        <asp:Parameter Name="SubTeam" Type="Int32" />
        <asp:Parameter Name="ExpectedDate" Type="Boolean" />
        <asp:SessionParameter Name="InsertUser" SessionField="UserName" Type="String" />
        <asp:Parameter Name="Copy" Type="Boolean" DefaultValue="False" />
        <asp:Parameter Name="CatalogID" Type="Int32" />
        <asp:Parameter Name="Details" Type="String" />
    </InsertParameters>
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="dsMassPublishCatalogs" runat="server" UpdateMethod="MassPublishCatalogs" TypeName="StoreOrderGuide.Dal">
    <UpdateParameters>
        <asp:Parameter Name="CatalogIDs" Type="String"/>
        <asp:Parameter Name="Published" Type="Boolean"/>
        <asp:SessionParameter Name="UpdateUser" SessionField="UserName" Type="String" />
    </UpdateParameters>
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="dsManagedBy" runat="server" SelectMethod="GetManagedByList" TypeName="StoreOrderGuide.Dal">

</asp:ObjectDataSource>

<asp:ObjectDataSource ID="dsSubTeam" runat="server" SelectMethod="GetSubTeamList" TypeName="StoreOrderGuide.Dal">
    <SelectParameters>
        <asp:Parameter Name="Catalog" DefaultValue="True" Type="Boolean" />
    </SelectParameters>
</asp:ObjectDataSource>

<%--<asp:ObjectDataSource ID="dsItems" runat="server" SelectMethod="GetItemList" InsertMethod="AddCatalogItem" TypeName="StoreOrderGuide.CatalogItem">
    <SelectParameters>
        <asp:ControlParameter ControlID="dvCatalogDetail" Name="CatalogID" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter ControlID="txtIdentifierSearch" Name="Identifier" PropertyName="Text" Type="String" />
        <asp:ControlParameter ControlID="txtItemDescriptionSearch" Name="Description" PropertyName="Text" Type="String" />
        <asp:ControlParameter ControlID="ddlSubTeamSearch" Name="SubTeamID" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter ControlID="ddlClassSearch" Name="ClassID" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter ControlID="ddlLevel3Search" Name="Level3ID" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter ControlID="ddlBrandSearch" Name="BrandID" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="dvCatalogDetail" Name="CatalogID" PropertyName="SelectedValue" Type="Int32" />
        <asp:Parameter Name="ItemKey" Type="Int32" />
         <asp:SessionParameter Name="UserName" SessionField="UserName" Type="String" />
    </InsertParameters>
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="dsStores" runat="server" SelectMethod="GetStoreList" InsertMethod="AddCatalogStore" TypeName="StoreOrderGuide.CatalogStore">
    <InsertParameters>
        <asp:ControlParameter ControlID="dvCatalogDetail" Name="CatalogID" PropertyName="SelectedValue" Type="Int32" />
        <asp:Parameter Name="StoreID" Type="Int32" />
        <asp:SessionParameter Name="UserName" SessionField="UserName" Type="String" />
    </InsertParameters>
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="dsCatalogItems" runat="server" SelectMethod="GetCatalogItems" DeleteMethod="DelCatalogItem" UpdateMethod="SetCatalogItem" TypeName="StoreOrderGuide.CatalogItem">
    <SelectParameters>
        <asp:ControlParameter ControlID="dvCatalogDetail" Name="CatalogID" PropertyName="SelectedValue" Type="Int32" />
        <asp:Parameter Name="StoreNo" DefaultValue="0" Type="Int32" />
        <asp:Parameter Name="Order" DefaultValue="False" Type="Boolean" />
        <asp:ControlParameter ControlID="txtIdentifierSearch_Cat" DefaultValue="" Name="Identifier" PropertyName="Text" />
        <asp:ControlParameter ControlID="txtItemDescriptionSearch_Cat" Name="Description" PropertyName="Text" />
        <asp:ControlParameter ControlID="ddlSubTeamSearch_Cat" Name="SubTeamID" PropertyName="SelectedValue" />
        <asp:ControlParameter ControlID="ddlClassSearch_Cat" Name="ClassID" PropertyName="SelectedValue" />
        <asp:ControlParameter ControlID="ddlLevel3Search_Cat" Name="Level3ID" PropertyName="SelectedValue" />
        <asp:ControlParameter ControlID="ddlBrandSearch_Cat" Name="BrandID" PropertyName="SelectedValue" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="ItemNote" Type="String" />
        <asp:ControlParameter ControlID="gvCatalogItemList" Name="CatalogItemID" PropertyName="SelectedValue" Type="Int32" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:ControlParameter ControlID="gvCatalogItemList" Name="CatalogItemID" PropertyName="SelectedValue" Type="Int32" />
    </DeleteParameters>
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="dsCatalogStores" runat="server" SelectMethod="GetCatalogStores" DeleteMethod="DelCatalogStore" TypeName="StoreOrderGuide.CatalogStore">
    <SelectParameters>
        <asp:ControlParameter ControlID="dvCatalogDetail" Name="CatalogID" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
    <DeleteParameters>
        <asp:ControlParameter ControlID="gvCatalogStoreList" Name="CatalogStoreID" PropertyName="SelectedValue" Type="Int32" />
    </DeleteParameters> 
</asp:ObjectDataSource>--%>


<!--########## FILTERS ########## -->
<div id="filters">
<asp:Panel ID="pnlFilter" runat="server" Visible="True">
    <div class="subtitle">Catalog Filters</div>
    <table cellpadding="2" cellspacing="0">
      <tr>
        <td>Zone</td>
        <td>Store</td>
        <td>SubTeam</td>
        <td>Published</td>
      </tr>
      <tr>
        <td><asp:DropDownList ID="ddlZoneFilter" runat="server" SkinID="SOGDropDownList" DataTextField="ZoneName" DataValueField="ZoneID" /></td>
        <td><asp:DropDownList ID="ddlStoreFilter" runat="server" SkinID="SOGDropDownList" DataTextField="StoreName" DataValueField="StoreID" /></td>
        <td><asp:DropDownList ID="ddlSubTeamFilter" runat="server" SkinID="SOGDropDownList" DataTextField="SubTeamName" DataValueField="SubTeamID" /></td>
        <td><asp:CheckBox ID="chkPublishedFilter" runat="server" SkinID="SOGCheckBox" Text="" Checked="True" /></td>
      </tr>
    </table>
    
    <asp:Button ID="btnFilter" runat="server" SkinID="SOGButton" Text="Filter" />
</asp:Panel>
</div>


<!--########## CATALOG ########## -->
<div id="catalog">
<asp:Panel ID="pnlCatalog" runat="server" Visible="True">
    <div class="subtitle">Catalogs</div>
    <asp:GridView ID="gvCatalogList" runat="server" EmptyDataText="No catalogs were found." EnableViewState="true" SkinID="SOGGridView" AllowPaging="True" AllowSorting="True" DataKeyNames="CatalogID" DataSourceID="dsCatalog" PageSize="20">
         <Columns>
            <asp:TemplateField ShowHeader="false" ItemStyle-CssClass="cmdField">
                <ItemTemplate>
                    <nobr>
                        <asp:LinkButton ID="btnCopy" runat="server" ForeColor="#0065A5" CommandName="btnCopy" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" Text="Copy" ToolTip="Copy this Catalog" />
                    </nobr>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowSelectButton="True" SelectText="Edit" ItemStyle-CssClass="cmdField" ItemStyle-ForeColor="#0065A5" />
            <asp:CommandField ShowDeleteButton="True" ItemStyle-CssClass="cmdField" ItemStyle-ForeColor="#0065A5" />
            <asp:TemplateField ShowHeader="false" ItemStyle-CssClass="cmdField">
                <ItemTemplate>
                    <nobr>
                        <asp:LinkButton ID="btnPrint" runat="server" ForeColor="#0065A5" CommandName="btnPrint" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" Text="Print" ToolTip="Print this Catalog" />
                    </nobr>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Mass Publish</br>/ Unpublish" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri">
                <ItemTemplate>
                    <asp:CheckBox ID="chkMasskPublish" runat="server" SkinID="SOGCheckBox" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CatalogID" SortExpression="CatalogID" HeaderText="CatalogID" ReadOnly="True" />
            <asp:BoundField DataField="CatalogCode" HeaderText="CatalogCode" SortExpression="CatalogCode" />
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
            <asp:BoundField DataField="ManagedBy" HeaderText="ManagedBy" SortExpression="ManagedBy" />
            <asp:BoundField DataField="ExpectedDate" HeaderText="ExpectedDate" SortExpression="ExpectedDate" />
            <asp:BoundField DataField="SubTeamName" HeaderText="SubTeam" SortExpression="SubTeamName" />
            <asp:BoundField DataField="Published" HeaderText="Published" SortExpression="Published" />
        </Columns>
    </asp:GridView>

    <asp:Button ID="btnAddCatalog" runat="server" SkinID="SOGButton" Text="Add" />
    <asp:Button ID="btnPrint" runat="server" SkinID="SOGButton" Text="Print" />
    <asp:Button ID="btnPublishAll" runat="server" SkinID="SOGButton" Text="Unpublish All" />
</asp:Panel>
</div>


<!--########## CATALOG DETAILS ########## -->
<div id="catalogdetails">
<asp:Panel ID="pnlCatalogDetails" runat="server" Visible="False">
    <div class="subtitle">Catalog Details</div>
    <asp:DetailsView ID="dvCatalogDetail" runat="server" EnableViewState="true" SkinID="SOGDetailView" DataSourceID="dsCatalog" AutoGenerateRows="False" DefaultMode="Edit" DataKeyNames="CatalogID">
        <Fields>
            <asp:BoundField DataField="CatalogID" HeaderText="CatalogID" ReadOnly="True" />
            <asp:TemplateField SortExpression="ManagedBy" HeaderText="ManagedBy" >
                <EditItemTemplate>
                    <asp:DropDownList DataTextField="ManagedBy" DataValueField="ManagedByID" ID="ddlManagedByID" Runat="server" SelectedValue='<%# Bind("ManagedByID") %>' DataSourceID="dsManagedBy" />
                </EditItemTemplate>
                <ItemTemplate >
                    <asp:Label Runat="server" Text='<%# Bind("ManagedBy") %>' ID="ManagedBy"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField SortExpression="SubTeam" HeaderText="SubTeam">
                <EditItemTemplate>
                    <asp:DropDownList DataTextField="SubTeamName" DataValueField="SubTeamID" ID="ddlSubTeamID" Runat="server" SelectedValue='<%# Bind("SubTeam") %>' DataSourceID="dsSubTeam" Width="180px" />
                </EditItemTemplate>
                <ItemTemplate >
                    <asp:Label Runat="server" Text='<%# Bind("SubTeam") %>' ID="SubTeam"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CatalogCode" HeaderText="CatalogCode" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:BoundField DataField="Details" HeaderText="Details" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:CheckBoxField DataField="ExpectedDate" HeaderText="ExpectedDate" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:CheckBoxField DataField="Published" HeaderText="Published" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:CommandField ShowCancelButton="True" ShowEditButton="True" CancelText="Exit" />
            <asp:CommandField ShowInsertButton="True" />
        </Fields>
    </asp:DetailsView>
</asp:Panel>
</div>


<!--########## CATALOGITEM ########## -->
<div id="catalogitem">

<asp:UpdatePanel runat="server" ID="updItem">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnAddItem" />
    </Triggers>
<ContentTemplate>

<asp:ObjectDataSource ID="dsItems" runat="server" SelectMethod="GetItemList" InsertMethod="AddCatalogItem" TypeName="StoreOrderGuide.CatalogItem">
    <SelectParameters>
        <asp:ControlParameter ControlID="dvCatalogDetail" Name="CatalogID" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter ControlID="txtIdentifierSearch" Name="Identifier" PropertyName="Text" Type="String" />
        <asp:ControlParameter ControlID="txtItemDescriptionSearch" Name="Description" PropertyName="Text" Type="String" />
        <asp:ControlParameter ControlID="ddlSubTeamSearch" Name="SubTeamID" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter ControlID="ddlClassSearch" Name="ClassID" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter ControlID="ddlLevel3Search" Name="Level3ID" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter ControlID="ddlBrandSearch" Name="BrandID" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="dvCatalogDetail" Name="CatalogID" PropertyName="SelectedValue" Type="Int32" />
        <asp:Parameter Name="ItemKey" Type="Int32" />
         <asp:SessionParameter Name="UserName" SessionField="UserName" Type="String" />
    </InsertParameters>
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="dsCatalogItems" runat="server" SelectMethod="GetCatalogItems" DeleteMethod="DelCatalogItem" UpdateMethod="SetCatalogItem" TypeName="StoreOrderGuide.CatalogItem">
    <SelectParameters>
        <asp:ControlParameter ControlID="dvCatalogDetail" Name="CatalogID" PropertyName="SelectedValue" Type="Int32" />
        <asp:Parameter Name="StoreNo" DefaultValue="0" Type="Int32" />
        <asp:Parameter Name="Order" DefaultValue="False" Type="Boolean" />
        <asp:ControlParameter ControlID="txtIdentifierSearch_Cat" DefaultValue="" Name="Identifier" PropertyName="Text" />
        <asp:ControlParameter ControlID="txtItemDescriptionSearch_Cat" Name="Description" PropertyName="Text" />
        <asp:ControlParameter ControlID="ddlSubTeamSearch_Cat" Name="SubTeamID" PropertyName="SelectedValue" />
        <asp:ControlParameter ControlID="ddlClassSearch_Cat" Name="ClassID" PropertyName="SelectedValue" />
        <asp:ControlParameter ControlID="ddlLevel3Search_Cat" Name="Level3ID" PropertyName="SelectedValue" />
        <asp:ControlParameter ControlID="ddlBrandSearch_Cat" Name="BrandID" PropertyName="SelectedValue" />
    </SelectParameters>
    <UpdateParameters>
        <asp:Parameter Name="ItemNote" Type="String" />
        <asp:ControlParameter ControlID="gvCatalogItemList" Name="CatalogItemID" PropertyName="SelectedValue" Type="Int32" />
    </UpdateParameters>
    <DeleteParameters>
        <asp:ControlParameter ControlID="gvCatalogItemList" Name="CatalogItemID" PropertyName="SelectedValue" Type="Int32" />
    </DeleteParameters>
</asp:ObjectDataSource>

<asp:UpdateProgress runat="server" ID="udpLoading" class="loading">
        <ProgressTemplate>
        
            <img src="../Images/ajax-loader.gif" />
            Loading...
        
        </ProgressTemplate>
</asp:UpdateProgress>

<asp:Panel ID="pnlCatalogItem" runat="server" Visible="False">
    <div id="catalogitemlist">
        <div id="catalogitemlistsearchfilters">
        <div class="subtitle">
            Catalog Item Filters</div>
        <table cellpadding="2" cellspacing="0">
          <tr>
            <td>Identifier</td>
            <td><asp:TextBox ID="txtIdentifierSearch_Cat" runat="server" SkinID="SOGTextBox" /></td>
          </tr>
          <tr>
            <td>Description</td>
            <td><asp:TextBox ID="txtItemDescriptionSearch_Cat" runat="server" SkinID="SOGTextBox" /></td>
          </tr>
          <tr>
            <td>SubTeam*</td>
            <td><asp:DropDownList ID="ddlSubTeamSearch_Cat" runat="server" SkinID="SOGDropDownList" DataTextField="SubTeamName" DataValueField="SubTeamID" Width="200px" AutoPostBack="True" /></td>
          </tr>
          <tr>
            <td>Class*</td>
            <td><asp:DropDownList ID="ddlClassSearch_Cat" runat="server" SkinID="SOGDropDownList" DataTextField="ClassName" DataValueField="ClassID" Width="200px" AutoPostBack="True" /></td>
          </tr>
          <tr>
            <td>Level 3</td>
            <td><asp:DropDownList ID="ddlLevel3Search_Cat" runat="server" SkinID="SOGDropDownList" DataTextField="Level3Name" DataValueField="Level3ID" Width="200px" /></td>
          </tr>
          <tr>
            <td>Brand</td>
            <td><asp:DropDownList ID="ddlBrandSearch_Cat" runat="server" SkinID="SOGDropDownList" DataTextField="BrandName" DataValueField="BrandID" Width="200px" /></td>
          </tr>
        </table>    

        <asp:Button ID="btnSearch_Cat" runat="server" SkinID="SOGButton" Text="Search" />
        </div>
    
        <div id="catalogitemlistsearchresults">
        <div class="subtitle">Catalog Items</div>
        <asp:GridView ID="gvCatalogItemList" runat="server" EmptyDataText="No items were found." EnableViewState="true" SkinID="SOGGridView" DataSourceID="dsCatalogItems" DataKeyNames="CatalogItemID" PageSize="20" Width="100%" AllowPaging="True" AllowSorting="True"> 
             <Columns>
                <asp:CommandField ShowEditButton="True" ItemStyle-CssClass="cmdField" ItemStyle-ForeColor="#284775" />
                <asp:TemplateField HeaderText = "Delete">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkItemDelete" runat="server" OnCheckedChanged="ItemDeleteChecked"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CatalogItemID" Visible="False" />
                <asp:BoundField DataField="ItemKey" HeaderText="ItemKey" SortExpression="ItemKey" ReadOnly="True" />
                <asp:BoundField DataField="Identifier" HeaderText="Identifier" SortExpression="Identifier" ReadOnly="True" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" ReadOnly="True" />
                <asp:BoundField DataField="Class" HeaderText="Class" SortExpression="Class" ReadOnly="True" />
                <asp:BoundField DataField="RetailUnit" HeaderText="RetailUnit" SortExpression="RetailUnit" ReadOnly="True" />
                <asp:BoundField DataField="DistributionUnit" HeaderText="DistributionUnit" SortExpression="DistributionUnit" ReadOnly="True" />
                <asp:BoundField DataField="Discontinued" HeaderText="Discontinued" SortExpression="Discontinued" ReadOnly="True" />
                <asp:BoundField DataField="NotAvailable" HeaderText="N/A" SortExpression="NotAvailable" ReadOnly="True" />
                <asp:BoundField DataField="ItemNote" HeaderText="ItemNote" SortExpression="ItemNote" />
                <asp:BoundField DataField="Cost" HeaderText="Cost" SortExpression="Cost" ReadOnly="True" DataFormatString="{0:C}" />
            </Columns>
        </asp:GridView>
        <asp:Button ID="btnDeleteItem" runat="server" Text="Delete Items" />
        </div>
<br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
<br />
<br />        
    </div>
    


    <div id="catalogitemsearch">
    <asp:Panel ID="pnlCatalogItemSearch" runat="server" Visible="True">
        <div id="catalogitemsearchfilters">
        <div class="subtitle">Item Search Filters</div>
        <table cellpadding="2" cellspacing="0">
          <tr>
            <td>Identifier</td>
            <td><asp:TextBox ID="txtIdentifierSearch" runat="server" SkinID="SOGTextBox" /></td>
          </tr>
          <tr>
            <td>Description</td>
            <td><asp:TextBox ID="txtItemDescriptionSearch" runat="server" SkinID="SOGTextBox" /></td>
          </tr>
          <tr>
            <td>SubTeam*</td>
            <td><asp:DropDownList ID="ddlSubTeamSearch" runat="server" SkinID="SOGDropDownList" DataTextField="SubTeamName" DataValueField="SubTeamID" Width="200px" AutoPostBack="True" /></td>
          </tr>
          <tr>
            <td>Class*</td>
            <td><asp:DropDownList ID="ddlClassSearch" runat="server" SkinID="SOGDropDownList" DataTextField="ClassName" DataValueField="ClassID" Width="200px" AutoPostBack="True" /></td>
          </tr>
          <tr>
            <td>Level 3</td>
            <td><asp:DropDownList ID="ddlLevel3Search" runat="server" SkinID="SOGDropDownList" DataTextField="Level3Name" DataValueField="Level3ID" Width="200px" /></td>
          </tr>
          <tr>
            <td>Brand</td>
            <td><asp:DropDownList ID="ddlBrandSearch" runat="server" SkinID="SOGDropDownList" DataTextField="BrandName" DataValueField="BrandID" Width="200px" /></td>
          </tr>
        </table>    

        <asp:Button ID="btnSearch" runat="server" SkinID="SOGButton" Text="Search" />
        </div>
    
        <div id="catalogitemsearchresults">
        <div class="subtitle">Search Items</div>
        <asp:GridView ID="gvItemList" runat="server" EmptyDataText="No items were found." EnableViewState="true" SkinID="SOGGridView" DataSourceID="dsItems" DataKeyNames="ItemKey" PageSize="20" AllowPaging="True" AllowSorting="True">
            <Columns>
                <asp:TemplateField HeaderText = "Add">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkItemAdd" runat="server" OnCheckedChanged="ItemAddChecked"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ItemKey" HeaderText="ItemKey" SortExpression="ItemKey" />
                <asp:BoundField DataField="Identifier" HeaderText="Identifier" SortExpression="Identifier" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                <asp:BoundField DataField="Class" HeaderText="Class" SortExpression="Class" />
                <asp:BoundField DataField="Level3" HeaderText="Level3" SortExpression="Level3" />
                <asp:BoundField DataField="RetailUnit" HeaderText="Retail" SortExpression="RetailUnit" />
                <asp:BoundField DataField="DistributionUnit" HeaderText="Distribution" SortExpression="DistributionUnit" />
                <asp:BoundField DataField="Discontinued" HeaderText="Discontinued" SortExpression="Discontinued" />
                <asp:BoundField DataField="NotAvailable" HeaderText="N/A" SortExpression="NotAvailable" />
                <asp:BoundField DataField="Cost" HeaderText="Cost" ReadOnly="True" SortExpression="Cost" DataFormatString="{0:C}" />
            </Columns>
        </asp:GridView>
        <asp:Button ID="btnAddItem" runat="server" text="Add Items" />
        </div>
    </asp:Panel>
    </div>
</asp:Panel>

</ContentTemplate>
</asp:UpdatePanel>
</div>


<!--########## CATALOGSTORE ########## -->
<div id="catalogstore">
<asp:UpdatePanel ID="updStores" runat="server" >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnAddStores" />
    </Triggers>
<ContentTemplate>

<asp:ObjectDataSource ID="dsStores" runat="server" SelectMethod="GetStoreList" InsertMethod="AddCatalogStore" TypeName="StoreOrderGuide.CatalogStore">
    <InsertParameters>
        <asp:ControlParameter ControlID="dvCatalogDetail" Name="CatalogID" PropertyName="SelectedValue" Type="Int32" />
        <asp:Parameter Name="StoreID" Type="Int32" />
        <asp:SessionParameter Name="UserName" SessionField="UserName" Type="String" />
    </InsertParameters>
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="dsCatalogStores" runat="server" SelectMethod="GetCatalogStores" DeleteMethod="DelCatalogStore" TypeName="StoreOrderGuide.CatalogStore">
    <SelectParameters>
        <asp:ControlParameter ControlID="dvCatalogDetail" Name="CatalogID" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
    <DeleteParameters>
        <asp:ControlParameter ControlID="gvCatalogStoreList" Name="CatalogStoreID" PropertyName="SelectedValue" Type="Int32" />
    </DeleteParameters> 
</asp:ObjectDataSource>

<asp:UpdateProgress runat="server" ID="UpdateProgress1" class="loading">
        <ProgressTemplate>
        
            <img src="../Images/ajax-loader.gif" />
            Loading...
        
        </ProgressTemplate>
</asp:UpdateProgress>

<asp:Panel ID="pnlCatalogStore" runat="server" Visible="False">
    <div id="catalogstorelist">
    <div class="subtitle">Catalog Stores</div><!--DataSourceID="dsCatalogStores" -->
    <asp:GridView ID="gvCatalogStoreList" runat="server" EmptyDataText="No stores were found." EnableViewState="true" SkinID="SOGGridView" DataSourceID="dsCatalogStores" DataKeyNames="CatalogStoreID" PageSize="7" AllowPaging="True" AllowSorting="True">
         <Columns>
            <asp:TemplateField HeaderText = "Delete">
                <ItemTemplate>
                    <asp:CheckBox ID="chkDelete" runat="server" OnCheckedChanged="DeleteStoreChecked"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CatalogStoreID" Visible="False" />
            <asp:BoundField DataField="StoreNo" HeaderText="StoreNo" SortExpression="StoreNo" />
            <asp:BoundField DataField="StoreName" HeaderText="StoreName" SortExpression="StoreName" />
            <asp:BoundField DataField="StoreAbbr" HeaderText="StoreAbbr" SortExpression="StoreAbbr" />
        </Columns>
    </asp:GridView>
    <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClientClick="return confirm('Are You Sure You Want To Delete Stores?')" />
    </div>

    <div id="catalogstoresearch">
    <div class="subtitle">Store List</div>
    <asp:GridView ID="gvStoreList" runat="server" EmptyDataText="No stores were found." EnableViewState="true" SkinID="SOGGridView" DataSourceID="dsStores" DataKeyNames="StoreID" PageSize="7" AllowPaging="True" AllowSorting="True">
         <Columns>
            <asp:TemplateField HeaderText="Add">
                <ItemTemplate>
                    <asp:CheckBox ID="chkAdd" runat="server" OnCheckedChanged="StoreChecked" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="StoreID" HeaderText="StoreID" SortExpression="StoreID" />
            <asp:BoundField DataField="StoreName" HeaderText="StoreName" SortExpression="StoreName" />
            <asp:BoundField DataField="StoreAbbr" HeaderText="StoreAbbr" SortExpression="StoreAbbr" />
        </Columns>
    </asp:GridView>
    <asp:Button ID="btnAddStores" runat="server" text="Add Stores"/>
    </div>
</asp:Panel>

</ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>