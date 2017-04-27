<%@ Control Language="VB" AutoEventWireup="false" Inherits="SLIM.UserInterface_ItemSearch" Codebehind="ItemSearch.ascx.vb" Explicit="false" Strict="false" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<ajax:ToolkitScriptManager runat="server" ID="ScriptManager1" EnablePartialRendering="true"  />

<table  cellpadding="2" cellspacing="0" border="0">
    <tr>
        <td align="right">
            <asp:Label ID="Label1" runat="server" Style="position: static" Text="UPC:" Font-Bold="True" Font-Size="12px" Font-Underline="False" Font-Names="Tahoma"></asp:Label></td>
        <td>
            <asp:TextBox ID="upcTxBx" runat="server" Style="position: static" MaxLength="13" TabIndex="1" ToolTip="UPC - 2-13 Digits" Font-Names="Tahoma" Font-Size="12px" Width="200px"></asp:TextBox></td>
        <td>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="upcTxBx"
                ErrorMessage="Invalid UPC" Font-Size="Small" Style="position: static" ValidationExpression="[0-9]{2,13}"></asp:RegularExpressionValidator></td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="Label2" runat="server" Style="position: static" Text="Description:" Font-Bold="True" Font-Underline="False" Font-Names="Tahoma" Font-Size="12px"></asp:Label></td>
        <td>
            <asp:TextBox ID="descTxBx" runat="server" Style="position: static" MaxLength="25" TabIndex="2" ToolTip="Item Description 4-20 Characters" Font-Names="Tahoma" Font-Size="12px" Width="200px"></asp:TextBox></td>
        <td>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="descTxBx"
                ErrorMessage="Invalid Description" Font-Size="Small" Style="position: static"
                ValidationExpression="\w{2,10}(\s)?\w{2,20}" Width="104px" Enabled="False"></asp:RegularExpressionValidator></td>
    </tr>
    <tr>
        <td align="right" style="height: 28px">
            <asp:Label ID="Label5" runat="server"  Text="Team:" Font-Bold="True" Font-Underline="False" Font-Names="Tahoma" Font-Size="12px"></asp:Label></td>
        <td style="height: 28px">
            <asp:DropDownList ID="ddlTeam" runat="server" 
                DataSourceID="DataSource_Teams"
                DataTextField="Team_Name" 
                DataValueField="Team_No" 
                Style="position: static" 
                TabIndex="3" 
                AppendDataBoundItems="True" Font-Names="Tahoma" Font-Size="12px" Width="200px">
                <asp:ListItem Value="0">----Select----</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="txtTeam" runat="server" Width="200px" MaxLength="100" Visible="False" />
        </td>
        <td style="height: 28px">
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Tahoma" Font-Size="12px" Font-Underline="False" Text="SubTeam:"></asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="DropDown_SubTeam" runat="server" Style="position: static" TabIndex="3" Font-Names="Tahoma" Font-Size="12px" Width="200px" />
            <asp:TextBox ID="txtSubTeam" runat="server" Width="200px" MaxLength="100" Visible="False" />
        </td>
        <td>
            <ajax:CascadingDropDown
                ID="cddSubTeam"
                runat="server"  
                Category="SubTeam"
                ContextKey=""
                LoadingText="[Loading SubTeams...]"
                PromptText="----Select----"
                ServicePath="Hierarchy.asmx"
                ServiceMethod="GetHierarchyValues"
                TargetControlID="DropDown_SubTeam"
                UseContextKey="true" />
        </td>
    </tr>
    <tr>
        <td align="right" style="height: 24px" >
            <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Names="Tahoma" Font-Size="12px"
                Font-Underline="False" Text="Class:"></asp:Label></td>
        <td style="height: 24px" >
            <asp:DropDownList ID="DropDown_Category" runat="server" Style="position: static" TabIndex="4" Font-Names="Tahoma" Font-Size="12px" Width="200px" />
            <asp:TextBox ID="txtCategory" runat="server" Width="200px" MaxLength="35" Visible="False" />
        </td>
        <td style="height: 24px">
            <ajax:CascadingDropDown
                ID="cddCategory"
                runat="server"  
                Category="Category"
                ContextKey=""
                LoadingText="[Loading categories...]"
                ParentControlID="DropDown_SubTeam" 
                PromptText="----Select----"
                ServicePath="Hierarchy.asmx"
                ServiceMethod="GetHierarchyValues"
                TargetControlID="DropDown_Category"
                UseContextKey="true"/>
            </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Names="Tahoma" Font-Size="12px"
                Font-Underline="False" Text="Level 3:"></asp:Label></td>
        <td>
            <asp:DropDownList ID="DropDown_Level3" runat="server" Style="position: static" TabIndex="4" Font-Names="Tahoma" Font-Size="12px" Width="200px" Enabled="False" />
            <asp:TextBox ID="txtLevel3" runat="server" Width="200px" MaxLength="50" Visible="False" />
        </td>
        <td>
            <ajax:CascadingDropDown
                ID="cddLevel3"
                runat="server"  
                Category="Level 3"
                ContextKey=""
                LoadingText="[Loading Level 3...]"
                ParentControlID="DropDown_Category"
                PromptText="----Select----"
                ServicePath="Hierarchy.asmx"
                ServiceMethod="GetHierarchyValues"
                TargetControlID="DropDown_Level3"
                UseContextKey="true" />
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Names="Tahoma" Font-Size="12px"
                Font-Underline="False" Text="Level 4:"></asp:Label></td>
        <td>
            <asp:DropDownList ID="DropDown_Level4" runat="server" Style="position: static" TabIndex="4" Font-Names="Tahoma" Font-Size="12px" Width="200px" Enabled="False" />
            <asp:TextBox ID="txtLevel4" runat="server" Width="200px" MaxLength="50" Visible="False" />
        </td>
        <td>
            <ajax:CascadingDropDown
                ID="cddLevel4"
                runat="server"  
                Category="Level 4"
                ContextKey=""
                LoadingText="[Loading Level 4...]"
                ParentControlID="DropDown_Level3" 
                PromptText="----Select----"
                ServicePath="Hierarchy.asmx"
                ServiceMethod="GetHierarchyValues"
                TargetControlID="DropDown_Level4"
                UseContextKey="true"/>
        </td>
    </tr>
    <tr>
        <td align="right" style="height: 24px">
            <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Names="Tahoma" Font-Size="12px"
                Font-Underline="False" Style="position: static" Text="Jurisdiction:"></asp:Label></td>
        <td style="height: 24px">
            <asp:DropDownList ID="DropDown_Jurisdiction" runat="server" Style="position: static" TabIndex="5" Font-Names="Tahoma" Font-Size="12px" Width="200px" AppendDataBoundItems="True" DataSourceID="DataSource_Jurisdiction" DataTextField="StoreJurisdictionDesc" DataValueField="StoreJurisdictionID" >
                <asp:ListItem Value="0">--- Select ---</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="txtJurisdiction" runat="server" Width="200px" MaxLength="50" Visible="False" style="position: static" /></td>
        <td style="height: 24px">
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Tahoma" Font-Size="12px"
                Font-Underline="False" Style="position: static" Text="Brand:"></asp:Label></td>
        <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="inline">
                <ContentTemplate>
                    <asp:TextBox ID="txtBrand" runat="server" autocomplete="off" Font-Names="Tahoma" Font-Size="12px" Style="position: static" TabIndex="2" Width="200px" MaxLength="25" />
                    <ajax:AutoCompleteExtender ID="acBrand" runat="server" 
                        CompletionInterval="1000"
                        EnableCaching="true"
                        MinimumPrefixLength="2"
                        ServiceMethod="GetBrandCompletionList"
                        ServicePath="AutoComplete.asmx"
                        TargetControlID="txtBrand"
                        UseContextKey="true" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="DropDown_SubTeam" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
            
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Names="Tahoma" Font-Size="12px"
                Font-Underline="False" Style="position: static" Text="Vendor:"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtVendor" runat="server" autocomplete="off" Font-Names="Tahoma" Font-Size="12px"
                Style="position: static" TabIndex="2" Width="200px" MaxLength="50" />
            <ajax:AutoCompleteExtender ID="acVendor" runat="server" 
                CompletionInterval="1000"
                EnableCaching="true"
                MinimumPrefixLength="2"
                ServiceMethod="GetVendorCompletionList"
                ServicePath="AutoComplete.asmx"
                TargetControlID="txtVendor" />
        </td>
        <td></td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="Tahoma" Font-Size="12px"
                Font-Underline="False" Style="position: static" Text="Scale Extra Text:"></asp:Label></td>
        <td>
            <asp:TextBox ID="TextBox_ExtraText" runat="server" Font-Names="Tahoma" Font-Size="12px"
                MaxLength="25" Style="position: static" TabIndex="2" Width="200px"></asp:TextBox></td>
        <td></td>
    </tr>
    <tr>
        <td align="right">&nbsp;</td>
        <td align="center">
            <asp:Button ID="SearchItem" runat="server" Style="position: static" Text="Search" Font-Names="Tahoma" Font-Size="12px" Width="150px" /></td>
        <td>&nbsp;<asp:Label ID="ErrorLabel" runat="server" Font-Bold="True" Font-Size="12px" ForeColor="#0000C0"
                Style="position: static" Width="104px" Font-Names="Tahoma"></asp:Label></td>
    </tr>
</table>
<asp:SqlDataSource ID="DataSource_Teams" runat="server" 
    ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
    DataSourceMode="DataReader"
    SelectCommand="GetTeams" 
    SelectCommandType="StoredProcedure"/>
<asp:SqlDataSource ID="DataSource_Jurisdiction" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
    SelectCommand="SELECT [StoreJurisdictionID], [StoreJurisdictionDesc] FROM [StoreJurisdiction]">
</asp:SqlDataSource>
