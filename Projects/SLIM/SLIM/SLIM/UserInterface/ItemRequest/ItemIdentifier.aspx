<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_ItemIdentifier" title="New Item Request" Codebehind="ItemIdentifier.aspx.vb" %>

<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igmisc" Namespace="Infragistics.WebUI.Misc" Assembly="Infragistics2.WebUI.Misc.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ajax:ToolkitScriptManager runat="server" ID="ScriptManager1" />

    <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="7" BackColor="Green"
        BorderColor="White" BorderWidth="2px" Font-Names="Verdana" Font-Size="0.8em"
        HeaderText="New Item Entry" Height="257px" Style="position: static"
        Width="810px" BorderStyle="Solid" CancelDestinationPageUrl="~/UserInterface/Main.aspx" DisplayCancelButton="True">
        <StepStyle Font-Size="0.8em" ForeColor="#333333" BackColor="LightGray" />
        <SideBarStyle BackColor="#004000" Font-Size="0.9em" VerticalAlign="Top" />
        <NavigationButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid"
            BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" />
        <WizardSteps>
            <asp:WizardStep runat="server" StepType="Start" Title="Item Identifier">
                <table style="position: static">
                    <tr>
                        <td style="width: 75px; height: 26px;">
                        </td>
                        <td style="width: 156px; height: 26px;">
                            </td>
                        <td style="width: 100px; height: 26px;">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 75px">
                        </td>
                        <td style="width: 156px">
                        </td>
                        <td style="width: 100px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 75px">
                            <igmisc:webpanel id="webpanel1" runat="server" expanded="False" style="position: static"
                                width="70px" StyleSetName="">
                                <panelstyle customrules="filter:progid:dximagetransform.microsoft.gradient(startcolorstr=#7febe8d7, endcolorstr=#7fcfccbb, gradienttype=1) progid:dximagetransform.microsoft.gradient(startcolorstr=#7fcfccbb, endcolorstr=#7febe8d7, gradienttype=0);">
                                    <borderdetails colorbottom="Gray" colorleft="224, 224, 224" colorright="Gray" stylebottom="Solid"
                                        styleleft="Solid" styleright="Solid" widthbottom="1px" widthleft="1px" widthright="1px" />
                                </panelstyle>
                                <template>
                                    <table style="width: 1px; position: static; height: 1px">
                                        <tr>
                                            <td style="width: 100px">
                                            </td>
                                            <td style="width: 54px">
                                            </td>
                                            <td style="width: 100px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; height: 69px">
                                            </td>
                                            <td style="width: 54px; height: 69px">
                                                <asp:image id="image1" runat="server" height="80px" imageurl="~/app_themes/barcode.jpg"
                                                    style="position: static" width="90px" /></td>
                                            <td style="width: 100px; height: 69px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px; height: 14px">
                                            </td>
                                            <td style="width: 54px; height: 14px">
                                                </td>
                                            <td style="width: 100px; height: 14px">
                                            </td>
                                        </tr>
                                    </table>
                                </template>
                                <header text="Examples">
                                    <hoverappearance>
                                        <Styles ForeColor="Blue">
                                        </Styles>
                                    </hoverappearance>
                                </header>
                            </igmisc:webpanel>
                        </td>
                        <td style="width: 156px">
                            <asp:RadioButtonList ID="IdentifierRadio" runat="server" RepeatDirection="Horizontal"
                                Style="position: static" AutoPostBack="True" Height="16px" Width="168px">
                                <asp:ListItem Selected="True" Value="1">UPC</asp:ListItem>
                                <asp:ListItem Value="2">Scale </asp:ListItem>
                                <asp:ListItem Value="3">PLU</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td style="width: 100px">
                            </td>
                    </tr>
                    <tr>
                        <td style="width: 75px">
                        </td>
                        <td style="width: 156px">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="upcTxBx"
                                ErrorMessage="UPC in wrong format" Style="position: static" ValidationExpression="^(P|\d)\d{3,12}$" Width="152px"></asp:RegularExpressionValidator>
                        </td>
                        <td style="width: 100px">
                            </td>
                    </tr>
                    <tr>
                        <td style="width: 75px; height: 29px;">
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="X-Small" Font-Underline="True"
                                Style="position: static" Text="Identifier:" Width="32px"></asp:Label>
                            <font color="red">*</font></td>
                        <td style="width: 156px; height: 29px;">
                            <asp:TextBox ID="upcTxBx" runat="server" Style="position: static" CausesValidation="True" MaxLength="12" TabIndex="1" ToolTip="Please Enter 4-12 Digits -- No leading zeros -- No check digits" CssClass="Item Identifier"></asp:TextBox>
                        </td>
                        <td style="width: 100px; height: 29px;">
                            <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/App_Themes/ig_tblPlus.gif"
                                Style="position: static" ToolTip="Check if UPC exists already" />
                        UPC
                            Check</td>
                    </tr>
                    <tr>
                        <td style="width: 75px; height: 14px">
                            
                        </td>
                        <td style="width: 156px; height: 14px">
                            
                        <asp:Label ID="upcErrorLbl" runat="server" Font-Bold="True" Font-Size="X-Small"
                                ForeColor="Navy" Style="position: static" Width="177px"></asp:Label>
                        </td>
                        <td style="width: 100px; height: 14px">
                            </td>
                    </tr>
                    <tr>
                        <td style="width: 75px; height: 14px" valign="middle">
                        </td>
                        <td colspan="1" style="width: 156px; height: 14px" valign="middle">
                            </td>
                        <td style="width: 100px; height: 14px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 75px; height: 14px" valign="middle">
                            
                        </td>
                        <td style="width: 156px; height: 14px" colspan="1" valign="middle">
                            <asp:Label ID="IdentifierInfoLabel" runat="server" Font-Bold="True" Font-Size="X-Small" ForeColor="Navy"
                                Style="position: static" Width="144px"></asp:Label>
                        </td>
                        <td style="width: 100px; height: 14px">
                        </td>
                    </tr>
                </table>
                
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Vendor">
                <table style="position: static">
                    <tr>
                        <td style="width: 132px; height: 14px;" align="left">
                            
                        </td>
                        <td style="width: 83px; height: 14px;" align="center" valign="middle">
                            
                        </td>
                        <td style="width: 100px; height: 14px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 132px; height: 36px;">
                            <asp:Label ID="Label17" runat="server" Font-Bold="True" Font-Underline="True" Style="position: static"
                                Text="Existing Vendor:" Width="95px"></asp:Label>
                            <font color="red">*</font></td>
                        <td style="width: 83px; height: 36px;">
                            <asp:TextBox ID="txtVendor" runat="server" autocomplete="off" Font-Names="Tahoma" Font-Size="12px"
                                Style="position: static" TabIndex="1" Width="200px" ToolTip="Vendor Company Name (auto-completion)" CssClass="Vendor" />
                            <ajax:AutoCompleteExtender ID="acVendor" runat="server"
                                MinimumPrefixLength="2"
                                ServiceMethod="GetVendorCompletionList"
                                ServicePath="~/UserInterface/AutoComplete.asmx"
                                TargetControlID="txtVendor" DelimiterCharacters="" Enabled="True" />
                        </td>
                        <td style="width: 100px; height: 36px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 132px">
                            <asp:CheckBox ID="CkBx3" runat="server" AutoPostBack="True" Style="position: static"
                                TextAlign="Left" Width="136px" Font-Bold="True" Font-Underline="True" Text="Requested Vendor:" />
                        </td>
                        <td style="width: 83px">
                            
                            <asp:DropDownList ID="NewVendorDropDown" runat="server" Style="position: static" DataSourceID="SqlDataSource23" DataTextField="CompanyName" DataValueField="VendorRequest_ID" Visible="False" TabIndex="2">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                            
                        </td>
                        <td style="width: 100px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 132px">
                        <asp:Label ID="Label16" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="Vendor Item ID:"></asp:Label>
                            <font color="red">*</font></td>                          
                        <td style="width: 83px">
                        <asp:TextBox ID="vendorOrderTxBx" runat="server" Style="position: static" MaxLength="12" ToolTip="Warehouse #" Width="100px" TabIndex="3"></asp:TextBox>
                         
                        </td>
                        <td style="width: 100px">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Invalid Number"
                                Style="position: static" ControlToValidate="vendorOrderTxBx" ValidationExpression="\w{0,12}\s?\w{0,12}" ToolTip="12 Digit Number" Width="112px" SetFocusOnError="True"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 132px; height: 38px;">
                        </td>
                        <td style="width: 83px; height: 38px;">
                        </td>
                        <td style="width: 100px; height: 38px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 132px">
                        </td>
                        <td style="width: 83px">
                        </td>
                        <td style="width: 100px">
                            </td>
                    </tr>
                </table>

                <asp:SqlDataSource ID="SqlDataSource23" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    SelectCommand="SELECT TOP 100 CompanyName, VendorRequest_ID FROM VendorRequest WHERE (VendorStatus_ID = @VendorStatus_ID) and Ready_To_Apply = 1 ORDER BY Insert_Date DESC">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="1" Name="VendorStatus_ID" Type="Int16" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Item Description">
                <table style="position: static">
                    <tr>
                        <td style="width: 123px">
                            <asp:Label ID="SubTeamLabel" runat="server" Font-Bold="True" Style="position: static"
                                Text="SubTeam:"></asp:Label>
                            <font color="red">*</font></td>
                        <td style="width: 94px" align="left">
                            <asp:DropDownList ID="SubTeamDropDown" runat="server" DataSourceID="SqlDataSource3"
                                DataTextField="SubTeam_Name" DataValueField="SubTeam_No" Style="position: static" TabIndex="1" AppendDataBoundItems="True" CssClass="SubTeam" AutoPostBack="True">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 162px">
                        </td>
                        <td style="width: 108px">
                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    SelectCommand="GetAllSubTeams" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 123px">
                            <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="Subteam Class:"></asp:Label>
                            <font color="red">*</font></td>
                        <td align="left" colspan="2">
                            <asp:DropDownList ID="CatDropDown" runat="server" AppendDataBoundItems="True" CssClass="Item Category"
                                DataTextField="Category_Name" DataValueField="Category_ID" Style="position: static"
                                TabIndex="2" AutoPostBack="True">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 108px">
                            <asp:SqlDataSource ID="DataSource_Category" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                                SelectCommand="GetCategoriesBySubTeam" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                                    <asp:SessionParameter DefaultValue="0" Name="SubTeam_No" SessionField="SubTeam_No"
                                        Type="Int32" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 123px">
                            <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="National Class:"></asp:Label>
                            <font color="red">*</font></td>
                        <td align="left" colspan="3">
                            <asp:DropDownList ID="ClassDropDown" runat="server" AppendDataBoundItems="True" DataTextField="ClassName"
                                DataValueField="ClassID" Style="position: static" TabIndex="3" Enabled="False">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="Datasource_NatClass" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                                ProviderName="<%$ ConnectionStrings:SLIM_Conn.ProviderName %>" SelectCommand="SELECT NatItemClass.ClassID, NatItemFamily.NatFamilyName + '-' + NatItemCat.NatCatName + '-' + NatItemClass.ClassName AS ClassName FROM NatItemCat INNER JOIN NatItemClass ON NatItemCat.NatCatID = NatItemClass.NatCatID INNER JOIN NatItemFamily ON NatItemCat.NatFamilyID = NatItemFamily.NatFamilyID INNER JOIN SubTeam ON NatItemFamily.SubTeam_No = SubTeam.SubTeam_No">
                            </asp:SqlDataSource>
                            </td>
                    </tr>
                    <tr>
                        <td style="width: 123px; height: 26px;">
                            <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="Item Description:"></asp:Label>
                            <font color="red">*</font></td>
                        <td style="width: 94px; height: 26px;">
                            <asp:TextBox ID="itemDescTxBx" runat="server" Style="position: static" TabIndex="4" MaxLength="35" CssClass="Item Description"></asp:TextBox>
                        </td>
                        <td style="width: 162px; height: 26px;">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server"
                                ControlToValidate="itemDescTxBx" ErrorMessage="Invalid Description" Style="position: static"
                                ValidationExpression="\w.*(\s)?\w.*" SetFocusOnError="True"></asp:RegularExpressionValidator>
                        </td>
                        <td style="width: 108px; height: 26px">
                <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    SelectCommand="select distinct st.subteam_name,st.subteam_no from subteam st
inner join userstoreteamtitle ust
on st.team_no = ust.team_no
where ust.user_id = @User_ID
order by st.Subteam_Name
">
                    <SelectParameters>
                        <asp:SessionParameter Name="User_ID" SessionField="UserID" />
                    </SelectParameters>
                </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 123px">
                            <asp:Label ID="Label4" runat="server" Font-Bold="True" Style="position: static" Text="POS Description:"></asp:Label>
                            <font color="red">*</font></td>
                        <td style="width: 94px">
                            <asp:TextBox ID="posDescTxBx" runat="server" Style="position: static" TabIndex="5" MaxLength="18" CssClass="POS Description"></asp:TextBox>
                        </td>
                        <td style="width: 162px">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server"
                                ControlToValidate="posDescTxBx" ErrorMessage="Invalid POS Desc." Style="position: static"
                                ValidationExpression="\w.*(\s)?\w.*" SetFocusOnError="True"></asp:RegularExpressionValidator>
                        </td>
                        <td style="width: 108px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 123px; height: 24px;">
                            <asp:Label ID="Label6" runat="server" Font-Bold="True" Style="position: static" Text="Item Brand:"></asp:Label>
                            <font color="red">*</font></td>
                        <td style="width: 94px; height: 24px;" align="left">
                            <asp:TextBox ID="txtBrand" runat="server" autocomplete="off" Font-Names="Tahoma" Font-Size="12px"
                                Style="position: static" TabIndex="6" Width="200px" ToolTip="Brand Name (auto-completion)" CssClass="Item Brand" />
                            <ajax:AutoCompleteExtender ID="acBrand" runat="server"
                                MinimumPrefixLength="2"
                                ServiceMethod="GetBrandCompletionList"
                                ServicePath="~/UserInterface/AutoComplete.asmx"
                                TargetControlID="txtBrand"
                                UseContextKey="True" DelimiterCharacters="" Enabled="True" />
                        </td>
                        <td style="width: 162px; height: 24px;">
                            </td>
                        <td style="width: 108px; height: 24px">
                            </td>
                    </tr>
                    <tr>
                        <td style="width: 123px; height: 36px;">
                            <asp:Label ID="Label7" runat="server" Font-Bold="True" Style="position: static" Text="New Brand:"></asp:Label>
                            <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="True" Style="position: static"
                                Width="16px" />
                             
                        </td>
                        <td style="width: 94px; height: 36px;">
                            <asp:TextBox ID="brandTxBx" runat="server" Style="position: static" Visible="False" TabIndex="7" MaxLength="25" ToolTip="Max. 20 alphanumeric characters" CssClass="New Brand"></asp:TextBox>
                        </td>
                        <td style="width: 162px; height: 36px">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server"
                                ControlToValidate="brandTxBx" ErrorMessage="Invalid Brand" Style="position: static"
                                ValidationExpression="(\w(\s)?){2,25}" SetFocusOnError="True"></asp:RegularExpressionValidator>
                        </td>
                        <td style="width: 108px; height: 36px">
                <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    SelectCommand="GetSubTeamBrand" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter DefaultValue="0" Name="SubTeam_No" SessionField="Subteam_No"
                            Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 123px; height: 36px">
                            <asp:Label ID="Label49" runat="server" Font-Bold="True" Style="position: static"
                                Text="Origin:"></asp:Label>
                        </td>
                        <td style="width: 94px; height: 36px">
                            <asp:DropDownList ID="OriginDropDown" runat="server" Style="position: static" AppendDataBoundItems="True" DataSourceID="SqlDataSource11" DataTextField="Origin_Name" DataValueField="Origin_ID" TabIndex="8">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 162px; height: 36px">
                            <asp:Label ID="Label50" runat="server" Font-Bold="True" Style="position: static"
                                Text="CountryOfProc:"></asp:Label>
                        </td>
                        <td style="width: 108px; height: 36px">
                            <asp:DropDownList ID="CountryDropDown" runat="server" Style="position: static" AppendDataBoundItems="True" DataSourceID="SqlDataSource11" DataTextField="Origin_Name" DataValueField="Origin_ID" TabIndex="9">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trShelfLabelType" runat="server">
                        <td style="width: 123px; height: 36px" runat="server">
                            <asp:Label ID="Label51" runat="server" Font-Bold="True" Style="position: static"
                                Text="ShelfLabelType:"></asp:Label>
                            <span style="color: #ff0000">*</span></td>
                        <td style="width: 94px; height: 36px" runat="server">
                            <asp:DropDownList ID="ShelfLabelDropDown" runat="server" Style="position: static" AppendDataBoundItems="True" DataSourceID="SqlDataSource12" DataTextField="LabelTypeDesc" DataValueField="LabelType_ID" TabIndex="10" CssClass="Shelf Label Type">
                                <asp:ListItem Value="-1">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 162px; height: 36px" runat="server">
                            <asp:Label ID="Label47" runat="server" Font-Bold="True" Style="position: static"
                                Text="DistributionSubTeam:"></asp:Label>
                            <span style="color: #ff0000">*</span></td>
                        <td style="width: 108px; height: 36px" runat="server">
                            <asp:DropDownList ID="DistSubDropDown" runat="server" Style="position: static" AppendDataBoundItems="True" DataSourceID="SqlDataSource15" DataTextField="SubTeam_Name" DataValueField="DistSubTeam_No" TabIndex="11" CssClass="Distribution Subteam">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 123px; height: 36px" align="left">
                            <asp:Label ID="Label69" runat="server" Font-Bold="True" Style="position: static"
                                Text="RetailUnits:"></asp:Label>
                            <span style="color: #ff0000">*</span></td>
                        <td style="width: 94px; height: 36px">
                            <asp:DropDownList ID="RetailUnitsDropDown" runat="server" Style="position: static" AppendDataBoundItems="True" DataSourceID="SqlDataSource10" DataTextField="Unit_Name" DataValueField="Unit_ID" TabIndex="12" CssClass="Retail Units" AutoPostBack="True">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 162px; height: 36px">
                            <asp:Label ID="lblDistributionUnits" runat="server" Font-Bold="True" Style="position: static">DistributionUnits:<span style="color: #ff0000">*</span></asp:Label>
                            </td>
                        <td style="width: 108px; height: 36px">
                            <asp:DropDownList ID="DistUnitsDropDown" runat="server" Style="position: static" AppendDataBoundItems="True" DataSourceID="SqlDataSource10" DataTextField="Unit_Name" DataValueField="Unit_ID" TabIndex="13" CssClass="Distribution Units">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 123px; height: 36px">
                            <asp:Label ID="Label70" runat="server" Font-Bold="True" Style="position: static"
                                Text="ManufacturingUnits:"></asp:Label>
                        <font color="red">*</font></td>
                        <td style="width: 94px; height: 36px">
                            <asp:DropDownList ID="ManufUnitsDropDown" runat="server" Style="position: static" AppendDataBoundItems="True" DataSourceID="SqlDataSource10" DataTextField="Unit_Name" DataValueField="Unit_ID" TabIndex="14" CssClass="Manufacturing Units">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 162px; height: 36px">
                            &nbsp;<asp:Label ID="lblAgeCode" runat="server" Font-Bold="True" Style="position: static"
                                Text="Age Code:"></asp:Label>
                        </td>
                        <td style="width: 108px; height: 36px">
                            &nbsp;<asp:TextBox ID="Textbox_AgeCode" runat="server" CssClass="AgeCode" MaxLength="2"
                                TabIndex="15" ToolTip="Values 0-99" Width="27px">0</asp:TextBox>
                            <asp:RegularExpressionValidator ID="regAgeCode" runat="server"
                                ControlToValidate="Textbox_AgeCode" ErrorMessage="Invalid Age Code" SetFocusOnError="True"
                                Style="position: static" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr id="trHasIngredients" runat="server">
                        <td style="width: 123px; height: 20px" runat="server">
                            <asp:Label ID="Label5" runat="server" Font-Bold="True" Style="position: static" Text="Has Ingredients:"></asp:Label>
                        </td>
                        <td style="width: 94px; height: 20px" runat="server">
                            <asp:CheckBox ID="IngredientsCK" runat="server" Style="position: static" Width="112px" TabIndex="16" />
                        </td>
                        <td style="width: 162px; height: 20px" runat="server">
                            <asp:Label ID="Label37" runat="server" Font-Bold="True" Style="position: static"
                                Text="Line Discount:"></asp:Label>
                        </td>
                        <td style="width: 108px; height: 20px" runat="server">
                            <asp:CheckBox ID="LineDiscountCK" runat="server" Style="position: static" Width="128px" TabIndex="16" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 123px; height: 20px">
                            <asp:Label ID="Label40" runat="server" Font-Bold="True" Style="position: static"
                                Text="Organic:"></asp:Label>
                        </td>
                        <td style="width: 94px; height: 20px">
                            <asp:CheckBox ID="OrganicCK" runat="server" Style="position: static" Width="152px" TabIndex="22" />
                        </td>
                        <td style="width: 162px; height: 20px">
                            <asp:Label ID="Label38" runat="server" Font-Bold="True" Style="position: static"
                                Text="CostByWeight:"></asp:Label>
                        </td>
                        <td style="width: 108px; height: 20px">
                            <asp:CheckBox ID="CostByWeightCK" runat="server" Style="position: static" Width="144px" TabIndex="18" AutoPostBack="True" />
                        </td>
                    </tr>
                    <tr id="trFixedWeight" runat="server">
                        <td style="width: 123px; height: 20px" runat="server">
                            <asp:Label ID="Label33" runat="server" Font-Bold="True" Style="position: static"
                                Text="FixedWeight:"></asp:Label>
                        </td>
                        <td style="width: 94px; height: 20px" runat="server">
                            <asp:CheckBox ID="FixedWeightCK" runat="server" Style="position: static" Width="144px" TabIndex="19" />
                        </td>
                        <td style="width: 162px; height: 20px" runat="server">
                            <asp:Label ID="Label39" runat="server" Font-Bold="True" Style="position: static"
                                Text="FoodStamps:"></asp:Label>
                            </td>
                        <td style="width: 108px; height: 20px" runat="server">
                            <asp:CheckBox ID="FoodStampsCK" runat="server" Style="position: static" Width="152px" TabIndex="20" />
                        </td>
                    </tr>
                    <tr id="trKeepFrozen" runat="server">
                        <td style="width: 123px; height: 20px" runat="server">
                            <asp:Label ID="Label34" runat="server" Font-Bold="True" Style="position: static"
                                Text="KeepFrozen:"></asp:Label>
                        </td>
                        <td style="width: 94px; height: 20px" runat="server">
                            <asp:CheckBox ID="KeepFrozenCK" runat="server" Style="position: static" Width="144px" TabIndex="21" />
                        </td>
                        <td style="width: 162px; height: 20px" runat="server">
                            <asp:Label ID="Label32" runat="server" Font-Bold="True" Style="position: static"
                                Text="Emp Discount:"></asp:Label>
                            </td>
                        <td style="width: 108px; height: 20px" runat="server">
                            <asp:CheckBox ID="EmpDiscountCK" runat="server" Style="position: static" Width="120px" TabIndex="17" />
                        </td>
                    </tr>
                    <tr id="trQuantityProhibit" runat="server">
                        <td style="width: 123px; height: 20px" runat="server">
                            <asp:Label ID="Label35" runat="server" Font-Bold="True" Style="position: static"
                                Text="QuantityProhibit:"></asp:Label>
                        </td>
                        <td style="width: 94px; height: 20px" runat="server">
                            <asp:CheckBox ID="QuantityProhibitCK" runat="server" Style="position: static" Width="144px" TabIndex="23" />
                        </td>
                        <td style="width: 162px; height: 20px" runat="server">
                            <asp:Label ID="Label41" runat="server" Font-Bold="True" Style="position: static"
                                Text="QuantityRequired:"></asp:Label>
                        </td>
                        <td style="width: 108px; height: 20px" runat="server">
                            <asp:CheckBox ID="QuantityReqCK" runat="server" Style="position: static" Width="168px" TabIndex="23" />
                        </td>
                    </tr>
                    <tr id="tdRefrigerated" runat="server">
                        <td style="width: 123px; height: 20px" runat="server">
                            <asp:Label ID="Label36" runat="server" Font-Bold="True" Style="position: static"
                                Text="Refrigerated:"></asp:Label>
                        </td>
                        <td style="width: 94px; height: 20px" runat="server">
                            <asp:CheckBox ID="RefrigeratedCK" runat="server" Style="position: static" Width="152px" TabIndex="24" />
                        </td>
                        <td style="width: 162px; height: 20px" runat="server">
                            <asp:Label ID="Label42" runat="server" Font-Bold="True" Style="position: static"
                                Text="Restricted:"></asp:Label>
                        </td>
                        <td style="width: 108px; height: 20px" runat="server">
                            <asp:CheckBox ID="RestrictedCK" runat="server" Style="position: static" TabIndex="25" />
                        </td>
                    </tr>
                    <tr id="trPriceRequired" runat="server">
                        <td style="width: 123px; height: 20px" runat="server">
                            <asp:Label ID="Label43" runat="server" Font-Bold="True" Style="position: static"
                                Text="PriceRequired:"></asp:Label>
                            </td>
                        <td style="width: 94px; height: 20px" runat="server"><asp:CheckBox ID="PriceRequiredCK" runat="server" Style="position: static" Width="144px" TabIndex="26" />
                        </td>
                        <td style="width: 162px; height: 20px" runat="server">
                            <asp:Label ID="Label44" runat="server" Font-Bold="True" Style="position: static"
                                Text="VisualVerify:"></asp:Label>
                        </td>
                        <td style="width: 108px; height: 20px" runat="server"><asp:CheckBox ID="VisualVerifyCK" runat="server" Style="position: static" Width="136px" TabIndex="27" />
                        </td>
                    </tr>
                    <tr id="trDiscountTerms" runat="server">
                        <td style="width: 123px; height: 20px" runat="server">
                            <asp:Label ID="Label54" runat="server" Font-Bold="True" Style="position: static"
                                Text="DiscountTerms:"></asp:Label>
                        </td>
                        <td style="width: 94px; height: 20px" runat="server">
                            <asp:TextBox ID="DiscTermTxBx" runat="server" Style="position: static" MaxLength="1" Width="20px" TabIndex="28"></asp:TextBox>
                        </td>
                        <td style="width: 162px; height: 20px" runat="server">
                            <asp:Label ID="Label55" runat="server" Font-Bold="True" Style="position: static"
                                Text="GoLocal:"></asp:Label>
                        </td>
                        <td style="width: 108px; height: 20px" runat="server">
                            <asp:DropDownList ID="GoLocalDropDown" runat="server" Style="position: static" TabIndex="29">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trMisc" runat="server">
                        <td style="width: 123px; height: 20px" runat="server">
                            <asp:Label ID="Label56" runat="server" Font-Bold="True" Style="position: static"
                                Text="Misc:"></asp:Label>
                        </td>
                        <td style="width: 94px; height: 20px" runat="server">
                            <asp:TextBox ID="MiscTxBx" runat="server" Style="position: static" MaxLength="100" TabIndex="30"></asp:TextBox>
                        </td>
                        <td style="width: 162px; height: 20px" runat="server">
                            <asp:Label ID="Label57" runat="server" Font-Bold="True" Style="position: static"
                                Text="CommodityCode:"></asp:Label>
                            </td>
                        <td style="width: 108px; height: 20px" runat="server">
                            <asp:DropDownList ID="CommodityDropDown" runat="server" Style="position: static" AppendDataBoundItems="True" TabIndex="31" CssClass="Commodity Code">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trNotAvailable" runat="server">
                        <td style="width: 123px; height: 20px" runat="server">
                            <asp:Label ID="Label46" runat="server" Font-Bold="True" Style="position: static"
                                Text="NotAvailable:"></asp:Label>
                            </td>
                        <td style="width: 94px; height: 20px" runat="server"><asp:CheckBox ID="NotAvailableCK" runat="server" Style="position: static" Width="152px" AutoPostBack="True" TabIndex="32" />
                        </td>
                        <td style="width: 162px; height: 20px" runat="server">
                            <asp:Label ID="Label_NotAvail" runat="server" Font-Bold="True" Style="position: static"
                                Text="NotAvailableNote:"></asp:Label>
                            &nbsp;
                        </td>
                        <td style="width: 108px; height: 20px" runat="server">
                            <asp:TextBox ID="NotAvailTxBx" runat="server" Style="position: static" TextMode="MultiLine"
                                Width="128px" MaxLength="255" TabIndex="33" CssClass="Not Available Note"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trMixMatch" runat="server">
                        <td style="width: 123px; height: 20px" runat="server">
                            <asp:Label ID="Label45" runat="server" Font-Bold="True" Style="position: static"
                                Text="MixMatch:"></asp:Label>
                            </td>
                        <td style="width: 94px; height: 20px" runat="server">
                            <asp:TextBox ID="TextBox_MixMatch" runat="server" MaxLength="2" Width="27px" CssClass="MixMatch" ToolTip="Values 0-99">0</asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator21" runat="server"
                                ControlToValidate="TextBox_MixMatch" ErrorMessage="Invalid MixMatch" SetFocusOnError="True"
                                Style="position: static" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                        </td>
                        <td style="width: 162px; height: 20px" runat="server">
                            <asp:Label ID="Label71" runat="server" Font-Bold="True" Style="position: static"
                                Text="CookingInstructions:"></asp:Label>
                        </td>
                        <td style="width: 108px; height: 20px" runat="server">
                            <asp:TextBox ID="ESRSCKTxBx" runat="server" Style="position: static" TextMode="MultiLine"
                                Width="128px" MaxLength="100" TabIndex="35"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trPOSLinkCode" runat="server">
                        <td style="width: 123px; height: 20px" runat="server">
                           <asp:Label ID="Label58" runat="server" Font-Bold="True" Style="position: static"
                                Text="POS Link Code:"></asp:Label>
                        </td>
                        <td style="width: 94px; height: 20px" runat="server">
                          <asp:TextBox ID="POSLinkCodeTxBx" runat="server" TabIndex="36"></asp:TextBox>
                        </td>
                        <td style="width: 162px; height: 20px" runat="server">
                        <asp:Label ID="Label52" runat="server" Font-Bold="True" Style="position: static"
                                Text="POSTare:"></asp:Label>
                        </td>
                        <td style="width: 108px; height: 20px" runat="server">
                            <asp:TextBox ID="POSTareTxBx" runat="server" Style="position: static" Width="50px" TabIndex="37">0</asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trVenue" runat="server">
                        <td style="width: 123px; height: 20px" runat="server">
                            <asp:Label ID="Label72" runat="server" Font-Bold="True" Style="position: static"
                                Text="Venue:"></asp:Label>
                        </td>
                        <td style="width: 94px; height: 20px" runat="server">
                            <asp:TextBox ID="VenueTxBx" runat="server" Style="position: static" Width="128px" TabIndex="38"></asp:TextBox>
                        </td>
                        <td style="width: 162px; height: 20px" runat="server">
                        </td>
                        <td style="width: 108px; height: 20px" runat="server">
                        </td>
                    </tr>
                </table>
                  
                <asp:SqlDataSource ID="SqlDataSource12" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    SelectCommand="SELECT [LabelType_ID], [LabelTypeDesc] FROM [LabelType]"></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource15" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    SelectCommand="SELECT DistSubTeam.DistSubTeam_No, SubTeam.SubTeam_Name FROM DistSubTeam INNER JOIN SubTeam ON DistSubTeam.DistSubTeam_No = SubTeam.SubTeam_No AND DistSubTeam.RetailSubTeam_No = SubTeam.SubTeam_No">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource11" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    SelectCommand="SELECT [Origin_ID], [Origin_Name] FROM [ItemOrigin]"></asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceLocal" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    ProviderName="System.Data.SqlClient" SelectCommand="select field_Values from attributeIdentifier where screen_text like '%local%' and combo_box = 'true'">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DataSourceCommCode" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    SelectCommand="select field_Values from attributeIdentifier where screen_text like '%Commodity%' and combo_box = 'true'">
                </asp:SqlDataSource>
                <asp:TextBox ID="TextBoxTempHidden" runat="server" Visible="False"></asp:TextBox>
                 
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Item Size">
                <table style="position: static">
                    <tr>
                        <td style="width: 100px">
                            <asp:Label ID="Label1" runat="server" Style="position: static" Text="Item Size:" Font-Bold="True"></asp:Label>
                            <font color="red">*</font></td>
                        <td style="width: 100px">
                            <asp:TextBox ID="itemSizeTxBx" runat="server" Style="position: static" MaxLength="5" ToolTip="Item Size (package_desc2)" TabIndex="1" Width="40px" CssClass="Item Size"></asp:TextBox>
                        </td>
                        <td style="width: 100px">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="itemSizeTxBx"
                                ErrorMessage="Invalid Entry" Style="position: static" ValidationExpression="\d{1,3}((.)?\d{1,2})?" SetFocusOnError="True"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px; height: 24px;">
                            <asp:Label ID="Label8" runat="server" Style="position: static" Text="Item UOM:" Font-Bold="True"></asp:Label>
                            <font color="red">*</font></td>
                        <td style="width: 100px; height: 24px;">
                            <asp:DropDownList ID="UOMDropDown" runat="server" DataSourceID="SqlDataSource10"
                                DataTextField="Unit_Name" DataValueField="Unit_ID" Style="position: static" AppendDataBoundItems="True" TabIndex="2" CssClass="Item UOM">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 100px; height: 24px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            <asp:Label ID="Label9" runat="server" Style="position: static" Text="Pack Size:" Font-Bold="True"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            <asp:TextBox ID="packTxBx" runat="server" Style="position: static" MaxLength="3" TabIndex="3" Width="40px" CssClass="Pack Size" ToolTip="Item_Pack (Package_Desc1)">1</asp:TextBox>
                        </td>
                        <td style="width: 100px">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="packTxBx"
                                ErrorMessage="Invalid Entry" Style="position: static" ValidationExpression="\d{1,3}" SetFocusOnError="True"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    SelectCommand="SELECT DISTINCT ItemUnit.Unit_Name, ItemUnit.Unit_ID FROM Item INNER JOIN ItemUnit ON Item.Package_Unit_ID = ItemUnit.Unit_ID WHERE (Item.SubTeam_No = @SubTeam_No)" DataSourceMode="DataReader">
                    <SelectParameters>
                        <asp:SessionParameter DefaultValue="0" Name="SubTeam_No" SessionField="SubTeam_No" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Scale Information">
                <table style="position: static">
                    <tr>
                        <td style="width: 105px">
                            <asp:Label ID="Label111" runat="server" Font-Bold="True" Style="position: static" Text="ScaleDescription1:"
                                Width="104px"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            <asp:TextBox ID="ScaleDesc1" runat="server" Style="position: static" MaxLength="64" TabIndex="1" Enabled="False"></asp:TextBox>
                        </td>
                        <td style="width: 100px">
                        </td>
                        <td style="width: 733px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px">
                            <asp:Label ID="Label13" runat="server" Font-Bold="True" Style="position: static"
                                Text="ScaleDescription2:"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            <asp:TextBox ID="ScaleDesc2" runat="server" Style="position: static" MaxLength="64" TabIndex="2" Enabled="False"></asp:TextBox>
                        </td>
                        <td style="width: 100px">
                        </td>
                        <td style="width: 733px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px">
                            <asp:Label ID="Label18" runat="server" Font-Bold="True" Style="position: static"
                                Text="ScaleDescription3:"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            <asp:TextBox ID="ScaleDesc3" runat="server" Style="position: static" TabIndex="3" Enabled="False"></asp:TextBox>
                        </td>
                        <td style="width: 100px">
                        </td>
                        <td style="width: 733px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 60px;">
                            <asp:Label ID="Label24" runat="server" Font-Bold="True" Style="position: static"
                                Text="ScaleDescription4:"></asp:Label>
                        </td>
                        <td style="width: 100px; height: 60px;">
                            <asp:TextBox ID="ScaleDesc4" runat="server" Style="position: static" TabIndex="4" Enabled="False"></asp:TextBox>
                        </td>
                        <td style="width: 100px; height: 60px;">
                        </td>
                        <td style="width: 733px; height: 60px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px">
                            <asp:Label ID="Label27" runat="server" Font-Bold="True" Style="position: static"
                                Text="Extra Text:"></asp:Label>
                            <font color="red">*</font></td>
                        <td style="width: 100px">
                            <asp:TextBox ID="Ingredients" runat="server" Style="position: static" TextMode="MultiLine" MaxLength="4200" TabIndex="5" Enabled="False" CssClass="Extra Text"></asp:TextBox>
                        </td>
                        <td style="width: 100px">
                           </td>
                        <td style="width: 733px">
                           </td>
                    </tr>
                    <tr>
                        <td style="width: 105px">
                            <asp:Label ID="Label26" runat="server" Font-Bold="True" Style="position: static"
                                Text="Shelf Life:"></asp:Label>
                            <font color="red">*</font></td>
                        <td style="width: 100px">
                            <asp:TextBox ID="ShelfLife" runat="server" Style="position: static" MaxLength="3" Width="40px" TabIndex="6" Enabled="False" CssClass="Shelf Life"></asp:TextBox>
                            Day(s)</td>
                        <td style="width: 100px">
                            </td>
                        <td style="width: 733px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 24px;">
                            <asp:Label ID="Label28" runat="server" Font-Bold="True" Style="position: static"
                                Text="Tare:"></asp:Label>
                        </td>
                        <td style="width: 100px; height: 24px;">
                            <asp:DropDownList ID="TareDropDown" runat="server" AppendDataBoundItems="True"
                                DataSourceID="SqlDataSource8" DataTextField="Description" DataValueField="Scale_Tare_ID"
                                Style="position: static" TabIndex="7" Enabled="False">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 100px; height: 24px;">
                           <asp:Label ID="Label59" runat="server" Font-Bold="True" Style="position: static"
                                Text="ForceTare:"></asp:Label>
                        </td>
                        <td style="width: 733px; height: 24px">
                           <asp:CheckBox ID="ForceTareCK" runat="server" Style="position: static" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px">
                            <asp:Label ID="Label25" runat="server" Font-Bold="True" Style="position: static"
                                Text="Scale UOM:"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            
                            <asp:DropDownList ID="ScaleUOMDropDown" runat="server" Style="position: static" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="Unit_Name" DataValueField="Unit_ID" TabIndex="8" Enabled="False">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 100px">
                           
                        </td>
                        <td style="width: 733px">
                            </td>
                    </tr>
                    <tr>
                        <td style="width: 105px">
                            <asp:Label ID="Label29" runat="server" Font-Bold="True" Style="position: static"
                                Text="RandomWeight:"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            <asp:DropDownList ID="RandomWeightDropDown" runat="server" AppendDataBoundItems="True"
                                DataSourceID="SqlDataSource9" DataTextField="Description" DataValueField="Scale_RandomWeightType_ID"
                                Style="position: static" TabIndex="9" Enabled="False">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 100px">
                           
                        </td>
                        <td style="width: 733px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px">
                            <asp:Label ID="Label30" runat="server" Font-Bold="True" Style="position: static"
                                Text="FixedWeight:"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            <asp:TextBox ID="FixedWeightTxBx" runat="server" MaxLength="10" Style="position: static"
                                Width="64px" TabIndex="10" Enabled="False">0</asp:TextBox>
                        </td>
                        <td style="width: 100px">
                        </td>
                        <td style="width: 733px">
                            </td>
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 26px;">
                            <asp:Label ID="Label31" runat="server" Font-Bold="True" Style="position: static"
                                Text="ByCount:"></asp:Label>
                        </td>
                        <td style="width: 100px; height: 26px;">
                            <asp:TextBox ID="ByCountTxBx" runat="server" MaxLength="2" Style="position: static"
                                Width="40px" TabIndex="11" Enabled="False">0</asp:TextBox>
                        </td>
                        <td style="width: 100px; height: 26px;">
                        </td>
                        <td style="width: 733px; height: 26px">
                            </td>
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 26px">
                        <asp:Label ID="Label60" runat="server" Font-Bold="True" Style="position: static"
                                Text="LabelStyle:"></asp:Label>
                        </td>
                        <td style="width: 100px; height: 26px">
                            <asp:DropDownList ID="LabelStyleDropDown" runat="server" Style="position: static" DataSourceID="SqlDataSource16" DataTextField="Description" DataValueField="Scale_LabelStyle_ID" AppendDataBoundItems="True" Enabled="False">
                                <asp:ListItem Value="-1">--Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 100px; height: 26px">
                        </td>
                        <td style="width: 733px; height: 26px">
                            </td>
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 26px">
                            <asp:Label ID="Label61" runat="server" Font-Bold="True" Style="position: static"
                                Text="ExtraText Label Type:"></asp:Label>
                            <font color="red">*</font></td>
                        <td style="width: 100px; height: 26px">
                            <asp:DropDownList ID="ScaleLabelTypeDropDown" runat="server" Style="position: static" AppendDataBoundItems="True" DataSourceID="SqlDataSource18" DataTextField="Description" DataValueField="Scale_LabelType_ID" Enabled="False" CssClass="Extra Text Label Type">
                                <asp:ListItem Value="-1">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 100px; height: 26px">
                        </td>
                        <td style="width: 733px; height: 26px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 26px">
                            </td>
                        <td style="width: 100px; height: 26px">
                            </td>
                        <td style="width: 100px; height: 26px">
                        </td>
                        <td style="width: 733px; height: 26px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 105px; height: 26px">
                        </td>
                        <td style="width: 100px; height: 26px">
                        </td>
                        <td style="width: 100px; height: 26px">
                        </td>
                        <td style="width: 733px; height: 26px">
                        </td>
                    </tr>
                </table>
                
                
                <asp:Label ID="ScaleInfoLabel" runat="server" Font-Bold="True" Font-Size="X-Small"
                    ForeColor="Navy" Style="position: static" Width="280px">Not a Scale Item - Please skip this page</asp:Label>
                 
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Cost">
                <table style="position: static">
                    <tr>
                        <td style="width: 123px; height: 26px;">
                            <asp:Label ID="Label14" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="Case Cost:"></asp:Label>
                            <font color="red">*</font></td>
                        <td align="right" style="width: 14px; height: 26px;">
                            <asp:Label ID="Label75" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="$"></asp:Label>
                        </td>
                        <td style="width: 62px; height: 26px;">
                            <asp:TextBox ID="caseCostTxBx" runat="server" Style="position: static" MaxLength="7" ToolTip="Format 12.32" Width="70px" TabIndex="1" CssClass="CaseCost"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Invalid Cost"
                                Style="position: static" ControlToValidate="caseCostTxBx" ValidationExpression="(\.)?\d{1,3}((\.)?(\d{1,4})?)?" Width="80px" SetFocusOnError="True"></asp:RegularExpressionValidator>
                        </td>
                        <td style="width: 128px; height: 26px" align="right">
                            <asp:Label ID="Label74" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="Cost Unit:"></asp:Label>
                            <font color="red">*</font>
                        </td>
                        <td style="width: 100px; height: 26px;">
                            <asp:DropDownList ID="CostUnitDropDown" runat="server" AppendDataBoundItems="True"
                                Style="position: static" DataSourceID="SqlDataSource10" DataTextField="Unit_Name" DataValueField="Unit_ID" AutoPostBack="True" TabIndex="2" CssClass="Cost Unit">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 123px">
                            <asp:Label ID="Label73" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="Cost Date:"></asp:Label>
                            <font color="red">*</font></td>
                        <td style="width: 14px" align="right">
                            <asp:Label ID="Label65" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="Start:"></asp:Label>
                        </td>
                        <td style="width: 62px">
                            <igsch:WebDateChooser ID="WebDateChooser5" runat="server" NullDateLabel="" Style="position: static"
                                TabIndex="3" Font-Size="X-Small" Value="">
                            </igsch:WebDateChooser>
                            <asp:Label ID="Label_WebDateChooser5" runat="server" ForeColor="Red" Text="Invalid Date"
                                Visible="False" Width="80px"></asp:Label>
                        </td>
                        <td style="width: 128px" align="right">
                            <asp:Label ID="Label64" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="End:"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            <igsch:WebDateChooser ID="WebDateChooser6" runat="server" NullDateLabel="" Style="position: static"
                                TabIndex="4" Font-Size="X-Small" Value="">
                            </igsch:WebDateChooser>
                            <asp:Label ID="Label_WebDateChooser6" runat="server" ForeColor="Red" Text="Invalid Date" Visible="False"
                                Width="80px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 123px; height: 35px;">
                            <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="Vendor Pack:"></asp:Label>
                            <font color="red">*</font></td>
                        <td style="width: 14px; height: 35px;" align="right">
                        </td>
                        <td style="width: 62px; height: 35px;">
                            <asp:TextBox ID="caseSizeTxBx" runat="server" Style="position: static" MaxLength="3" Width="70px" TabIndex="5" CssClass="Vendor Pack" ToolTip="Package_Desc1 (VCH)"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Invalid Size"
                                Style="position: static" ControlToValidate="caseSizeTxBx" ValidationExpression="\d{1,3}" Width="76px" SetFocusOnError="True"></asp:RegularExpressionValidator>
                        </td>
                        <td style="width: 128px; height: 35px;">
                            <asp:Label ID="Label68" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="Vendor Units:"></asp:Label>
                            <font color="red">*</font></td>
                        <td style="width: 100px; height: 35px;">
                            <asp:DropDownList ID="VendorUnitsDropDown" runat="server" Style="position: static" AppendDataBoundItems="True" TabIndex="6" DataSourceID="SqlDataSource10" DataTextField="Unit_Name" DataValueField="Unit_ID" ToolTip="How Vendor Sells Units" CssClass="Vendor Unit">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                            
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 123px; height: 26px;">
                            <asp:Label ID="Label76" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="Freight:"></asp:Label>
                            </td>
                        <td style="width: 14px; height: 26px;" align="right">
                        </td>
                        <td style="width: 62px; height: 26px;">
                            <asp:TextBox ID="FreightTxBx" runat="server" Style="position: static" Width="64px" TabIndex="7" CssClass="Freight">0</asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator19" runat="server"
                                ControlToValidate="FreightTxBx" ErrorMessage="Invalid Price" SetFocusOnError="True"
                                Style="position: static" ValidationExpression="(\.)?\d{1,3}((\.)?(\d{1,5})?)?"
                                Width="79px"></asp:RegularExpressionValidator>
                        </td>
                        <td style="width: 128px; height: 26px;">
                            <asp:Label ID="Label63" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="Freight Unit:"></asp:Label>
                            </td>
                        <td style="width: 100px; height: 26px;">
                            <asp:DropDownList ID="FreightUnitDropDown" runat="server" AppendDataBoundItems="True" Style="position: static" DataSourceID="SqlDataSource10" DataTextField="Unit_Name" DataValueField="Unit_ID" TabIndex="8" CssClass="Freight Unit">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 123px">
                            </td>
                        <td align="right" style="width: 14px">
                            </td>
                        <td style="width: 62px">
                           
                        </td>
                        <td align="right" style="width: 128px">
                            </td>
                        <td style="width: 100px">
                            
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 123px; height: 14px;">
                            <asp:Label ID="Label62" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="Promotional:"></asp:Label>
                        </td>
                        <td style="width: 14px; height: 14px;" align="right">
                        </td>
                        <td style="width: 62px; height: 14px;">
                            <asp:CheckBox ID="PromotionalCK" runat="server" Style="position: static" TabIndex="9" Width="112px" AutoPostBack="True" ToolTip="Click here is the Item has an opening or ongoing discount." />
                        </td>
                        <td style="width: 128px; height: 14px">
                            </td>
                        <td style="width: 100px; height: 14px;">
                            </td>
                    </tr>
                    <tr>
                        <td style="width: 123px; height: 14px">
                            <asp:Label ID="Label_Allow" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="Allowance:"></asp:Label>
                        </td>
                        <td align="right" style="width: 14px; height: 14px">
                            <asp:Label ID="Label81" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="$"></asp:Label>
                        </td>
                        <td style="width: 62px; height: 14px">
                            <asp:TextBox ID="AllowanceTxBx" runat="server" Style="position: static" Width="64px" TabIndex="10" CssClass="Allowance Amount">0</asp:TextBox>
                        </td>
                        <td style="width: 128px; height: 14px">
                        </td>
                        <td style="width: 100px; height: 14px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 123px; height: 14px">
                        </td>
                        <td align="right" style="width: 14px; height: 14px">
                            <asp:Label ID="Label82" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="Start:"></asp:Label>
                        </td>
                        <td style="width: 62px; height: 14px">
                            <igsch:WebDateChooser ID="WebDateChooser1" runat="server" Style="position: static" NullDateLabel="" TabIndex="11" Font-Size="X-Small">
                            </igsch:WebDateChooser>
                            <asp:Label ID="Label_WebDateChooser1" runat="server" ForeColor="Red" Text="Invalid Date" Visible="False"
                                Width="80px"></asp:Label>
                        </td>
                        <td align="right" style="width: 128px; height: 14px">
                            <asp:Label ID="Label83" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="End:"></asp:Label>
                        </td>
                        <td style="width: 100px; height: 14px">
                            <igsch:WebDateChooser ID="WebDateChooser2" runat="server" Style="position: static" NullDateLabel="" TabIndex="12" Font-Size="X-Small">
                            </igsch:WebDateChooser>
                            <asp:Label ID="Label_WebDateChooser2" runat="server" ForeColor="Red" Text="Invalid Date" Visible="False"
                                Width="80px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 123px; height: 14px">
                            <asp:Label ID="Label_Disc" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="Discount:"></asp:Label>
                        </td>
                        <td align="right" style="width: 14px; height: 14px">
                            <asp:Label ID="Label86" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="%"></asp:Label>
                        </td>
                        <td style="width: 62px; height: 14px">
                            <asp:TextBox ID="DiscountsTxBx" runat="server" Style="position: static" Width="64px" MaxLength="11" TabIndex="13" CssClass="Discount Amount">0</asp:TextBox>
                        </td>
                        <td style="width: 128px; height: 14px">
                        </td>
                        <td style="width: 100px; height: 14px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 123px; height: 15px">
                        </td>
                        <td align="right" style="width: 14px; height: 15px">
                            <asp:Label ID="Label85" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="Start:"></asp:Label>
                        </td>
                        <td style="width: 62px; height: 15px">
                            <igsch:WebDateChooser ID="WebDateChooser3" runat="server" Style="position: static" NullDateLabel="" TabIndex="14" Font-Size="X-Small">
                            </igsch:WebDateChooser>
                            <asp:Label ID="Label_WebDateChooser3" runat="server" ForeColor="Red" Text="Invalid Date" Visible="False"
                                Width="80px"></asp:Label>
                        </td>
                        <td align="right" style="width: 128px; height: 15px">
                            <asp:Label ID="Label84" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="End:"></asp:Label>
                        </td>
                        <td style="width: 100px; height: 15px">
                            <igsch:WebDateChooser ID="WebDateChooser4" runat="server" Style="position: static" NullDateLabel="" TabIndex="15" Font-Size="X-Small">
                            </igsch:WebDateChooser>
                            <asp:Label ID="Label_WebDateChooser4" runat="server" ForeColor="Red" Text="Invalid Date" Visible="False"
                                Width="80px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 123px; height: 14px">
                            
                        </td>
                        <td align="right" style="width: 14px; height: 14px">
                        </td>
                        <td style="width: 62px; height: 14px">
                            
                        </td>
                        <td align="right" style="width: 128px; height: 14px">
                            </td>
                        <td style="width: 100px; height: 14px">
                            </td>
                    </tr>
                </table>
                <asp:SqlDataSource ID="SqlDataSource17" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    SelectCommand="SELECT [Unit_ID], [Unit_Name] FROM [ItemUnit]"></asp:SqlDataSource>
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Retail">
                <table style="position: static">
                    <tr>
                        <td align="left" style="width: 79px; height: 24px">
                            <asp:Label ID="Label23" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="Margin:"></asp:Label>
                        </td>
                        <td align="right" style="width: 11px; height: 24px">
                            %</td>
                        <td style="width: 90px; height: 24px">
                            <asp:TextBox ID="MarginTxBx" runat="server" MaxLength="2" Style="position: static"
                                TabIndex="1" Width="50px"></asp:TextBox>
                        </td>
                        <td style="width: 100px; height: 24px">
                            <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/App_Themes/ig_tblPlus.gif"
                                Style="position: static" />
                            Get Price</td>
                        <td style="width: 100px; height: 24px">
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="MarginTxBx"
                                ErrorMessage="Margin Invalid" Style="position: static" MaximumValue="1000" MinimumValue="-1000" Type="Integer"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 79px; height: 24px;" align="left">
                            <asp:Label ID="Label19" runat="server" Style="position: static" Text="Price:" Font-Bold="True" Font-Underline="False"></asp:Label>
                            <font color="red">*</font></td>
                        <td align="right" style="width: 11px; height: 24px">
                            $</td>
                        <td style="width: 90px; height: 24px;">
                            <asp:TextBox ID="priceTxBx" runat="server" Style="position: static" MaxLength="6" TabIndex="2" Width="50px" CssClass="Price"></asp:TextBox>
                        </td>
                        <td style="width: 100px; height: 24px;">
                            <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/App_Themes/ig_tblPlus.gif"
                                Style="position: static" />
                            Get Margin</td>
                        <td style="width: 100px; height: 24px">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="priceTxBx"
                                ErrorMessage="Invalid Price" Style="position: static" ValidationExpression="(\.)?\d{1,3}((\.)?(\d{1,5})?)?" SetFocusOnError="True"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 79px; height: 26px;">
                            <asp:Label ID="Label20" runat="server" Style="position: static" Text="Price Multiple:" Font-Bold="True" Font-Underline="False"></asp:Label>
                            <font color="red">*</font></td>
                        <td style="width: 11px; height: 26px">
                        </td>
                        <td style="width: 90px; height: 26px;">
                            <asp:TextBox ID="priceMultiTxBx" runat="server" Style="position: static" MaxLength="2" Width="20px" TabIndex="3" CssClass="Price Multiple">1</asp:TextBox>
                        </td>
                        <td style="width: 100px; height: 26px;">
                            </td>
                        <td style="width: 100px; height: 26px">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="priceMultiTxBx"
                                ErrorMessage="Invalid Multiple" Style="position: static" ValidationExpression="\d{1,3}" SetFocusOnError="True"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 79px; height: 26px">
                            <asp:Label ID="Label66" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="MSRPPrice:"></asp:Label>
                            </td>
                        <td style="width: 11px; height: 26px">
                        </td>
                        <td style="width: 90px; height: 26px">
                            <asp:TextBox ID="MSRPPriceTxBx" runat="server" CssClass="MSRP Price" MaxLength="6" Style="position: static"
                                TabIndex="4" Width="50px"></asp:TextBox>
                        </td>
                        <td style="width: 100px; height: 26px">
                        </td>
                        <td style="width: 100px; height: 26px">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator23" runat="server"
                                ControlToValidate="MSRPPriceTxBx" ErrorMessage="Invalid Price" SetFocusOnError="True"
                                Style="position: static" ValidationExpression="(\.)?\d{1,3}((\.)?(\d{1,5})?)?"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 79px; height: 26px">
                            <asp:Label ID="Label67" runat="server" Font-Bold="True" Font-Underline="False" Style="position: static"
                                Text="MSRPPrice Multiple:"></asp:Label>
                            </td>
                        <td style="width: 11px; height: 26px">
                        </td>
                        <td style="width: 90px; height: 26px">
                            <asp:TextBox ID="MSRPPriceMultTxBx" runat="server" CssClass="MSRP Price Multiple" MaxLength="2"
                                Style="position: static" TabIndex="5" Width="20px">1</asp:TextBox>
                        </td>
                        <td style="width: 100px; height: 26px">
                        </td>
                        <td style="width: 100px; height: 26px">
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator24" runat="server"
                                ControlToValidate="MSRPPriceMultTxBx" ErrorMessage="Invalid Multiple" SetFocusOnError="True"
                                Style="position: static" ValidationExpression="\d{1,3}"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 79px; height: 18px;">
                            &nbsp;</td>
                        <td style="width: 11px; height: 18px;" align="right">
                            </td>
                        <td style="width: 90px; height: 18px;">
                            &nbsp;</td>
                        <td style="width: 100px; height: 18px;">
                        </td>
                        <td style="width: 100px; height: 18px;">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 79px; height: 7px;">
                        </td>
                        <td style="width: 11px; height: 7px">
                        </td>
                        <td style="width: 90px; height: 7px;">
                            <asp:Label ID="MarginErrorLbl" runat="server" Font-Bold="True" Font-Size="X-Small"
                                ForeColor="Navy" Style="position: static" Width="112px"></asp:Label>
                        </td>
                        <td style="width: 100px; height: 7px;">
                        </td>
                        <td style="width: 100px; height: 7px">
                        </td>
                    </tr>
                </table>
            </asp:WizardStep>
            <asp:WizardStep runat="server" Title="Tax">
                <table style="position: static">
                    <tr>
                        <td style="width: 100px; height: 24px">
                            &nbsp;</td>
                        <td style="width: 100px; height: 24px">
                            &nbsp;</td>
                        <td style="width: 100px; height: 24px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            <asp:Label ID="Label21" runat="server" Font-Bold="True" Font-Underline="True" Style="position: static"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            <asp:DropDownList ID="CRVDropDown" runat="server" Style="position: static" DataSourceID="SqlDataSource33" DataTextField="Item_Description" DataValueField="Item_Key" AppendDataBoundItems="True" TabIndex="2">
                                <asp:ListItem Value="0">-- Select --</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 100px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px; height: 24px;">
                            <asp:Label ID="Label22" runat="server" Font-Bold="True" Font-Underline="True" Style="position: static"
                                Text="Tax Class:"></asp:Label>
                            <font color="red">*</font></td>
                        <td style="width: 100px; height: 24px;">
                            <asp:TextBox ID="txtTaxClass" runat="server" autocomplete="off" Font-Names="Tahoma" Font-Size="12px"
                                Style="position: static" TabIndex="3" Width="200px" ToolTip="Tax Class Name (auto-completion)" CssClass="Tax Class" />
                            <ajax:AutoCompleteExtender ID="acTaxClass" runat="server"
                                MinimumPrefixLength="2"
                                ServiceMethod="GetTaxClassCompletionList"
                                ServicePath="~/UserInterface/AutoComplete.asmx"
                                TargetControlID="txtTaxClass" DelimiterCharacters="" Enabled="True" />
                        </td>
                        <td style="width: 100px; height: 24px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px; height: 18px">
                        </td>
                        <td style="width: 100px; height: 18px">
                            <asp:Label ID="ErrorLabel1" runat="server" Font-Bold="True" ForeColor="Navy" Style="position: static"></asp:Label>
                        </td>
                        <td style="width: 100px; height: 18px">
                        </td>
                    </tr>
                </table>
                <asp:SqlDataSource ID="SqlDataSource33" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    SelectCommand="SELECT Item_Description, Item_Key, subteam_no
FROM Item I 
WHERE subteam_No =  4820
ORDER BY Item_Description">
                </asp:SqlDataSource>
            </asp:WizardStep>
            <asp:WizardStep runat="server" StepType="Finish" Title="Summary">
                <table style="position: static" border="1" cellpadding="2" cellspacing="2">
                    <tr>
                        <td style="width: 100px; height: 14px">
                        </td>
                        <td colspan="1" style="width: 100px; height: 14px">
                        </td>
                        <td style="width: 100px; height: 14px">
                        </td>
                        <td style="width: 100px; height: 14px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            UPC:</td>
                        <td style="width: 100px">
                            <asp:Label ID="SLabelUPC" runat="server" Style="position: static" SkinID="SumLabel"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            Vendor:</td>
                        <td style="width: 100px">
                            <asp:Label ID="SLabelVend" runat="server" SkinID="SumLabel" Style="position: static"
                                Width="80px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            SubTeam:</td>
                        <td style="width: 100px">
                            <asp:Label ID="SLabelSub" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            New Vendor:</td>
                        <td style="width: 100px">
                            <asp:Label ID="SLabelNVend" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px; height: 14px;">
                            Description:</td>
                        <td style="width: 100px; height: 14px;">
                            <asp:Label ID="SLabelDesc" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                        <td style="width: 100px; height: 14px;">
                            CaseCost:</td>
                        <td style="width: 100px; height: 14px">
                            <asp:Label ID="SLabelCost" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px; height: 14px">
                            POSDescription:</td>
                        <td style="width: 100px; height: 14px">
                            <asp:Label ID="SLabelPOSDesc" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                        <td style="width: 100px; height: 14px">
                            CaseSize:</td>
                        <td style="width: 100px; height: 14px">
                            <asp:Label ID="SLabelCaseSize" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            Brand:</td>
                        <td style="width: 100px">
                            <asp:Label ID="SLabelBrand" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            VendorItem ID:</td>
                        <td style="width: 100px">
                            <asp:Label ID="SLabelVendorOrd" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            New Brand:</td>
                        <td style="width: 100px">
                            <asp:Label ID="SLabelNBrand" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            Price:  &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;
                            &nbsp; &nbsp;&nbsp;
                        </td>
                        <td style="width: 100px">
                            <asp:Label ID="SLabelPrice" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            Item Size:</td>
                        <td style="width: 100px">
                            <asp:Label ID="SLabelItemSize" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            Age Code:</td>
                        <td style="width: 100px">
                            <asp:Label ID="SLabelACode" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            Item UOM:</td>
                        <td style="width: 100px">
                            <asp:Label ID="SLabelUOM" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            FoodStamp:</td>
                        <td style="width: 100px">
                            <asp:Label ID="SLabelFS" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            Pack Size:</td>
                        <td style="width: 100px">
                            <asp:Label ID="SLabelPack" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            CRV:</td>
                        <td style="width: 100px">
                            <asp:Label ID="SLabelCRV" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            ItemCategory:</td>
                        <td style="width: 100px">
                            <asp:Label ID="SLabelItemCat" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            Tax:</td>
                        <td style="width: 100px">
                            <asp:Label ID="SLabelTax" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            Class:</td>
                        <td style="width: 100px">
                            <asp:Label ID="SLabelClass" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
                        </td>
                        <td style="width: 100px">
                            Store:</td>
                        <td style="width: 100px">
                            <asp:Label ID="SLabelStore" runat="server" Style="position: static" Width="80px" SkinID="SumLabel"></asp:Label>
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
                <asp:Label ID="FinishLabel" runat="server" Font-Bold="True" Font-Size="Small" Height="32px"
                    SkinID="SumLabel" Style="position: static" Width="296px"></asp:Label>
            </asp:WizardStep>
        </WizardSteps>
        <SideBarButtonStyle BackColor="Transparent" Font-Names="Verdana" ForeColor="White" />
        <HeaderStyle BackColor="Black" BorderColor="#EFF3FB" BorderStyle="Solid" BorderWidth="2px"
            Font-Bold="True" Font-Size="0.9em" ForeColor="White" HorizontalAlign="Center" />
        <FinishNavigationTemplate>
            <asp:Button ID="FinishPreviousButton" runat="server" BackColor="White" BorderColor="#507CD1"
                BorderStyle="Solid" BorderWidth="1px" CausesValidation="False" CommandName="MovePrevious"
                Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" Text="Previous" />
            <asp:Button ID="FinishButton" runat="server" BackColor="White" BorderColor="#507CD1"
                BorderStyle="Solid" BorderWidth="1px" CommandName="MoveComplete" Font-Names="Verdana"
                Font-Size="0.8em" ForeColor="#284E98" Text="Finish" />
        </FinishNavigationTemplate>
        <StartNavigationTemplate>
            <asp:Button ID="StartNextButton" runat="server" BackColor="White" BorderColor="#507CD1"
                BorderStyle="Solid" BorderWidth="1px" CommandName="MoveNext" Font-Names="Verdana"
                Font-Size="0.8em" ForeColor="#284E98" Text="Next" />
        </StartNavigationTemplate>
        <NavigationStyle BackColor="#E0E0E0" BorderColor="#E0E0E0" />
    </asp:Wizard>
                            <asp:SqlDataSource ID="SqlDataSource9" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                                SelectCommand="SELECT [Scale_RandomWeightType_ID], [Description] FROM [Scale_RandomWeightType]">
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="SqlDataSource10" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                                SelectCommand="GetItemUnitsCost" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="false" Name="WeightUnits" Type="Boolean" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource16" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                    SelectCommand="SELECT [Scale_LabelStyle_ID], [Description] FROM [Scale_LabelStyle]">
                </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource18" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="SELECT [Scale_LabelType_ID], [Description] FROM [Scale_LabelType]">
    </asp:SqlDataSource>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                                SelectCommand="SELECT [Unit_ID], [Unit_Name] FROM [ItemUnit] WHERE ([Weight_Unit] = @Weight_Unit)">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="True" Name="Weight_Unit" Type="Boolean" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
                                SelectCommand="SELECT [Scale_Tare_ID], [Description] FROM [Scale_Tare]"></asp:SqlDataSource>
</asp:Content>

