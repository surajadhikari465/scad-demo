<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Schedule.aspx.vb" Inherits="StoreOrderGuide.ScheduleInterface" MasterPageFile="~/StoreOrderGuide.Master" Title="Schedule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" Runat="Server">

<!--########## DATA SOURCES ########## -->
<asp:ObjectDataSource ID="dsCatalogSchedule" runat="server" SelectMethod="GetCatalogSchedules" TypeName="StoreOrderGuide.CatalogSchedule" DeleteMethod="DelCatalogSchedule" InsertMethod="AddCatalogSchedule" UpdateMethod="SetCatalogSchedule" >
    <SelectParameters>
        <asp:Parameter Name="CatalogScheduleID" Type="Int32" />
        <asp:ControlParameter ControlID="ddlManagedByFilter" Name="ManagedByID" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter ControlID="ddlStoreFilter" Name="StoreNo" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter ControlID="ddlSubTeamFilter" Name="SubTeamNo" PropertyName="SelectedValue" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="dsSubTeam" runat="server" SelectMethod="GetSubTeamList" TypeName="StoreOrderGuide.Dal">
    <SelectParameters>
        <asp:Parameter Name="Catalog" DefaultValue="True" Type="Boolean" />
    </SelectParameters>
</asp:ObjectDataSource>

<asp:ObjectDataSource ID="dsStore" runat="server" SelectMethod="GetStoreList" TypeName="StoreOrderGuide.CatalogStore" />

<asp:ObjectDataSource ID="dsManagedBy" runat="server" SelectMethod="GetManagedByList" TypeName="StoreOrderGuide.Dal" />

<!--########## FILTERS ########## -->
<div id="filters">
<asp:Panel ID="pnlFilter" runat="server" Visible="True">
    <div class="subtitle">Schedule Filters</div>
    <table cellpadding="2" cellspacing="0">
      <tr>
        <td>Managed By*</td>
        <td>Store</td>
        <td>SubTeam</td>
      </tr>
      <tr>
        <td><asp:DropDownList ID="ddlManagedByFilter" runat="server" SkinID="SOGDropDownList" DataTextField="ManagedBy" DataValueField="ManagedByID" AutoPostBack="True" /></td>
        <td><asp:DropDownList ID="ddlStoreFilter" runat="server" SkinID="SOGDropDownList" DataTextField="StoreName" DataValueField="StoreID" /></td>
        <td><asp:DropDownList ID="ddlSubTeamFilter" runat="server" SkinID="SOGDropDownList" DataTextField="SubTeamName" DataValueField="SubTeamID" /></td>
      </tr>
    </table>
    
    <asp:Button ID="btnFilter" runat="server" SkinID="SOGButton" Text="Filter" />
</asp:Panel>
</div>

<!--########## CATALOG SCHEDULES ########## -->
<div id="catalogschedulelist">
<asp:Panel ID="pnlCatalogScheduleList" runat="server" Visible="True">
    <div class="subtitle">Catalog Schedules</div>
    <asp:GridView ID="gvCatalogSchedule" runat="server" EmptyDataText="No catalog schedules were found." EnableViewState="False" SkinID="SOGGridView" AllowPaging="True" DataSourceID="dsCatalogSchedule" AllowSorting="True" DataKeyNames="CatalogScheduleID">
        <Columns>
            <asp:CommandField ShowEditButton="True" ItemStyle-CssClass="cmdField" ItemStyle-ForeColor="#284775" />
            <asp:CommandField ShowDeleteButton="True" ItemStyle-CssClass="cmdField" ItemStyle-ForeColor="#284775" />
            <asp:TemplateField SortExpression="ManagedByID" HeaderText="ManagedByID">
                <EditItemTemplate>
                    <asp:DropDownList DataTextField="ManagedBy" DataValueField="ManagedByID" ID="ddlManagerID" Runat="server" SelectedValue='<%# Bind("ManagedByID") %>' DataSourceID="dsManagedBy" Width="180px" Font-Names="Calibri" />
                </EditItemTemplate>
                <ItemTemplate >
                    <asp:Label Runat="server" Text='<%# Bind("Value") %>' ID="lblManagerID"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField SortExpression="StoreNo" HeaderText="StoreNo">
                <EditItemTemplate>
                    <asp:DropDownList DataTextField="StoreName" DataValueField="StoreID" ID="ddlStoreNo" Runat="server" SelectedValue='<%# Bind("StoreNo") %>' DataSourceID="dsStore" Width="180px" Font-Names="Calibri" />
                </EditItemTemplate>
                <ItemTemplate >
                    <asp:Label Runat="server" Text='<%# Bind("Store_Name") %>' ID="lblStoreName"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField SortExpression="SubTeamNo" HeaderText="SubTeamNo">
                <EditItemTemplate>
                    <asp:DropDownList DataTextField="SubTeamName" DataValueField="SubTeamID" ID="ddlSubTeamNo" Runat="server" SelectedValue='<%# Bind("SubTeamNo") %>' DataSourceID="dsSubTeam" Width="180px" Font-Names="Calibri" />
                </EditItemTemplate>
                <ItemTemplate >
                    <asp:Label Runat="server" Text='<%# Bind("SubTeam_Name") %>' ID="lblSubTeamName"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CheckBoxField DataField="Mon" HeaderText="Mon" SortExpression="Mon" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />            
            <asp:CheckBoxField DataField="Tue" HeaderText="Tue" SortExpression="Tue" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:CheckBoxField DataField="Wed" HeaderText="Wed" SortExpression="Wed" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:CheckBoxField DataField="Thu" HeaderText="Thu" SortExpression="Thu" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:CheckBoxField DataField="Fri" HeaderText="Fri" SortExpression="Fri" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:CheckBoxField DataField="Sat" HeaderText="Sat" SortExpression="Sat" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:CheckBoxField DataField="Sun" HeaderText="Sun" SortExpression="Sun" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
        </Columns>
    </asp:GridView>

    <asp:Button ID="btnAddCatalogSchedule" runat="server" SkinID="SOGButton" Text="Add" />
</asp:Panel>
</div>


<!--########## CATALOG SCHEDULE DETAILS ########## -->
<div id="catalogscheduledetails">
<asp:Panel ID="pnlCatalogScheduleDetails" runat="server" Visible="False">
    <div class="subtitle">Catalog Schedule Details</div>
    <asp:DetailsView ID="dvCatalogSchedule" runat="server" EnableViewState="false" SkinID="SOGDetailView" AutoGenerateRows="False" DefaultMode="Edit" DataSourceID="dsCatalogSchedule" DataKeyNames="CatalogScheduleID">
        <Fields>
            <asp:BoundField DataField="CatalogScheduleID" HeaderText="AdminID" ReadOnly="True" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:TemplateField SortExpression="ManagedByID" HeaderText="ManagedByID">
                <EditItemTemplate>
                    <asp:DropDownList DataTextField="ManagedBy" DataValueField="ManagedByID" ID="ddlManagerID" Runat="server" SelectedValue='<%# Bind("ManagedByID") %>' DataSourceID="dsManagedBy" Width="180px" />
                </EditItemTemplate>
                <ItemTemplate >
                    <asp:Label Runat="server" Text='<%# Bind("ManagedByID") %>' ID="lblManagerID"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField SortExpression="StoreNo" HeaderText="StoreNo">
                <EditItemTemplate>
                    <asp:DropDownList DataTextField="StoreName" DataValueField="StoreID" ID="ddlStoreNo" Runat="server" SelectedValue='<%# Bind("StoreNo") %>' DataSourceID="dsStore" Width="180px" />
                </EditItemTemplate>
                <ItemTemplate >
                    <asp:Label Runat="server" Text='<%# Bind("StoreNo") %>' ID="lblStoreNo"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField SortExpression="SubTeamNo" HeaderText="SubTeamNo">
                <EditItemTemplate>
                    <asp:DropDownList DataTextField="SubTeamName" DataValueField="SubTeamID" ID="ddlSubTeamNo" Runat="server" SelectedValue='<%# Bind("SubTeamNo") %>' DataSourceID="dsSubTeam" Width="180px" />
                </EditItemTemplate>
                <ItemTemplate >
                    <asp:Label Runat="server" Text='<%# Bind("SubTeamNo") %>' ID="lblSubTeamNo"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CheckBoxField DataField="Mon" HeaderText="Mon" SortExpression="Mon" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />            
            <asp:CheckBoxField DataField="Tue" HeaderText="Tue" SortExpression="Tue" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:CheckBoxField DataField="Wed" HeaderText="Wed" SortExpression="Wed" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:CheckBoxField DataField="Thu" HeaderText="Thu" SortExpression="Thu" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:CheckBoxField DataField="Fri" HeaderText="Fri" SortExpression="Fri" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:CheckBoxField DataField="Sat" HeaderText="Sat" SortExpression="Sat" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:CheckBoxField DataField="Sun" HeaderText="Sun" SortExpression="Sun" ItemStyle-Font-Names="Calibri" ControlStyle-Font-Names="Calibri" />
            <asp:CommandField ShowInsertButton="True" />
        </Fields>
    </asp:DetailsView>
</asp:Panel>
</div>

</asp:Content>