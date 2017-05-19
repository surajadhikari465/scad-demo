<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MultiPOTool.Master"
    CodeBehind="PushedToIRMA.aspx.vb" Inherits="MultiPOTool.PushedToIRMA" Title="Pushed To IRMA" %>

<%@ Register Assembly="Infragistics35.WebUI.UltraWebGrid.v10.1, Version=10.1.20101.1011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid" TagPrefix="igtbl" %>

<%@ Register Assembly="Infragistics35.WebUI.UltraWebGrid.DocumentExport.v10.1, Version=10.1.20101.1011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid.DocumentExport" TagPrefix="igtbldocexp" %>

<%@ Register Assembly="Infragistics35.WebUI.UltraWebGrid.ExcelExport.v10.1, Version=10.1.20101.1011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid.ExcelExport" TagPrefix="igtblexp" %>
    


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

  <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <script language="javascript" type="text/javascript">

        var txtStartDate = '<%= txtStartDate.ClientID %>';
        var Image8 = '<%= Image8.ClientID %>';
        function getCalendarValue() {
            document.getElementById(txtStartDate).value = document.getElementById(Image8).value;
        }
        // getCalendarValue()

        function MouseOverHandler(gridName, id, objectType) {

            if (objectType == 0) {

                igtbl_getCellById(id).getRow().Element.className += " over";

            }

        }



        function MouseOutHandler(gridName, id, objectType) {

            if (objectType == 0) {

                igtbl_getCellById(id).getRow().Element.className = igtbl_getCellById(id).getRow().Element.className.replace(" over", "")

            }

        } 

    </script>
      
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <table cellpadding="0" cellspacing="0" width="100%">
         <tr>
           <td>
               <%--      <asp:UpdatePanel ID="UpdMain" runat="server" UpdateMode="Always">
            <ContentTemplate>--%>
                <table>
                    <tr>
                        <td align="center" colspan="2" style="padding: 30px;">
                            <asp:Label ID="lblPushedToIRMA" runat="server" Text="Label" Font-Bold="True" CssClass="HeaderLable"
                                Font-Italic="true"></asp:Label>
                               
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-bottom: 20px;" align="left">
                            <table style="width: 100%">
                                <tr>
                                    <td style="height: 26px;" align="right">
                                        POs Per Page
                                    </td>
                                    <td style="text-align: left">
                                        <asp:DropDownList ID="ddlPaging" runat="server" AutoPostBack="True" CssClass="slim">
                                            <asp:ListItem Selected="True">25</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>75</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="padding-bottom: 20px;">
                            <table>
                                <tr>
                                    <td style="height: 26px;" align="right">
                                        Export Format
                                    </td>
                                    <td style="height: 26px; text-align: left" align="right">
                                        <asp:DropDownList ID="ddlExportFormat" runat="server" CssClass="slim">
                                            <asp:ListItem>Excel</asp:ListItem>
                                            <asp:ListItem>PDF</asp:ListItem>
                                            <asp:ListItem>Text</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="excelExport" runat="server" Style="position: static" Text="Export Pushed PO List"
                                            CssClass="button" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red" Style="position: static"
                                Width="648px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                        <table>
                            <tr id="trRegionFilter" runat="server" visible="false">    
                                <td valign="top">
                                      Select a Region to refresh Filters :
                                    <asp:DropDownList ID="drpRegions" runat="server" AutoPostBack="True">
                                        <asp:ListItem Text="Select Region" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                          
                        </td>
                        <td>
                            <asp:Button ID="btnClear" runat="server" Text="Clear Filters" CssClass="button" />   
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right" style="padding-bottom: 10px;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <div class="main-announcements">
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                <table>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:Label ID="Label1" runat="server" Text="Filters:" Font-Bold="True" CssClass="HeaderLable"
                                                                                Font-Italic="true"></asp:Label>
                                                                            <asp:DropDownList ID="drpPOType" runat="server" AutoPostBack="True" 
                                                                                Height="23px" Width="231px" Visible="False">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3">
                                                                            PO Type:&nbsp;&nbsp;&nbsp; &nbsp;<asp:RadioButtonList ID="rdoPOType" runat="server" 
                                                                                AutoPostBack="True" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                                <asp:ListItem Selected="True" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Value="2"></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        Sub-Team:
                                                                                        <asp:DropDownList ID="drpSubTeam" runat="server" CssClass="slim" 
                                                                                            DataTextField="SubTeam_Name" DataValueField="SubTeam_No">
                                                                                            <asp:ListItem Text="Select One" Value=""></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        Store:
                                                                                        <asp:DropDownList ID="drpStore" runat="server" CssClass="slim" 
                                                                                            DataTextField="Store_Name" DataValueField="StoreAbbr">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        Vendor:
                                                                                        <asp:DropDownList ID="drpVendor" runat="server" CssClass="slim" 
                                                                                            DataTextField="CompanyName" DataValueField="Vendor_ID">
                                                                                            <asp:ListItem Text="Select One" Value=""></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            Start Date :<asp:TextBox ID="txtStartDate" Text="" runat="server" size="15" MaxLength="10"
                                                                                CssClass="slim"></asp:TextBox>
                                                                            <asp:Image ID="Image8" runat="server" ImageUrl="~/App_Themes/Theme1/Images/date.png" CausesValidation="False" />
                                                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtStartDate" PopupButtonID="Image8"
                                                                                Format="MM/dd/yyyy" runat="server" Enabled="True">
                                                                            </asp:CalendarExtender>
                                                                        </td>
                                                                        <td align="left">
                                                                            End Date :<asp:TextBox ID="txtEndDate" Text="" runat="server" onFocus="javascript:this.blur();"
                                                                                autocomplete="off" size="15" MaxLength="10" CssClass="slim"></asp:TextBox>
                                                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Theme1/Images/date.png" />
                                                                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtEndDate" PopupButtonID="Image1"
                                                                                Format="MM/dd/yyyy" runat="server" Enabled="True">
                                                                            </asp:CalendarExtender>
                                                                        </td>
                                                                        <td align="left">
                                                                            <table>
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        Show last :
                                                                                        <asp:TextBox ID="txtNumber" runat="server" CssClass="slim"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        (Numbers Only)
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="center" style="width: 200px;" valign="bottom">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnGet" runat="server" Text="Get PO's" CssClass="button" Width="120" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnClear" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        <asp:updatepanel id="updGrid" runat="server" updatemode="always">
                        <ContentTemplate>
                            <igtbl:UltraWebGrid ID="ult" runat="server" Style="position: static" Width="100%"
                                DataSourceID="ObjectDataSource1">
                                <Bands>
                                    <igtbl:UltraGridBand>
                                        <AddNewRow Visible="NotSet" View="NotSet">
                                        </AddNewRow>
                                        <Columns>
                                            <igtbl:UltraGridColumn BaseColumnName="Databound Col0" IsBound="True" Key="Databound Col0">
                                                <Header Caption="Databound Col0">
                                                </Header>
                                            </igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn BaseColumnName="Databound Col1" DataType="System.Int32" IsBound="True"
                                                Key="Databound Col1">
                                                <Header Caption="Databound Col1">
                                                    <RowLayoutColumnInfo OriginX="1" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="1" />
                                                </Footer>
                                            </igtbl:UltraGridColumn>
                                            <igtbl:UltraGridColumn BaseColumnName="Databound Col2" IsBound="True" Key="Databound Col2">
                                                <Header Caption="Databound Col2">
                                                    <RowLayoutColumnInfo OriginX="2" />
                                                </Header>
                                                <Footer>
                                                    <RowLayoutColumnInfo OriginX="2" />
                                                </Footer>
                                            </igtbl:UltraGridColumn>
                                        </Columns>
                                    </igtbl:UltraGridBand>
                                </Bands>
                                <DisplayLayout Version="4.00" SelectTypeRowDefault="Extended" Name="ult"
                                    AllowSortingDefault="Yes" AllowColSizingDefault="Free" RowHeightDefault="20px"
                                    TableLayout="Fixed" ViewType="OutlookGroupBy" RowSelectorsDefault="No" AllowColumnMovingDefault="OnServer"
                                    HeaderClickActionDefault="SortMulti" StationaryMargins="Header" BorderCollapseDefault="Separate"
                                    StationaryMarginsOutlookGroupBy="True">
                                    <FrameStyle BackColor="Window" BorderColor="InactiveCaption" BorderWidth="1px" BorderStyle="Solid"
                                        Font-Names="Microsoft Sans Serif" Font-Size="8.25pt" Width="100%">
                                    </FrameStyle>
                                    <%--<Pager MinimumPagesForDisplay="2">
                            <PagerStyle BackColor="LightGray" BorderWidth="1px" BorderStyle="Solid">
                                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px">
                                </BorderDetails>
                            </PagerStyle>
                        </Pager>--%>
                                    <Pager PagerAppearance="Bottom" AllowPaging="True" PageSize="25">
                                        <PagerStyle BackColor="LightGrey" BorderStyle="Solid" BorderWidth="1px">
                                            <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
                                        </PagerStyle>
                                    </Pager>
                                    <EditCellStyleDefault BorderWidth="0px" BorderStyle="None">
                                    </EditCellStyleDefault>
                                    <FooterStyleDefault BackColor="LightGrey" BorderWidth="1px" BorderStyle="Solid">
                                        <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px">
                                        </BorderDetails>
                                    </FooterStyleDefault>
                                    <HeaderStyleDefault HorizontalAlign="Left" BackColor="LightGrey" BorderStyle="Solid">
                                        <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px">
                                        </BorderDetails>
                                    </HeaderStyleDefault>
                                    <RowStyleDefault BackColor="Window" BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"
                                        Font-Names="Microsoft Sans Serif" Font-Size="8.25pt">
                                        <Padding Left="3px"></Padding>
                                        <BorderDetails ColorLeft="Window" ColorTop="Window"></BorderDetails>
                                    </RowStyleDefault>
                                    <GroupByRowStyleDefault BackColor="Control" BorderColor="Window">
                                    </GroupByRowStyleDefault>
                                    <GroupByBox>
                                        <BoxStyle BackColor="ActiveBorder" BorderColor="Window">
                                        </BoxStyle>
                                    </GroupByBox>
                                    <AddNewBox Hidden="False">
                                        <BoxStyle BackColor="Window" BorderColor="InactiveCaption" BorderWidth="1px" BorderStyle="Solid">
                                            <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px">
                                            </BorderDetails>
                                        </BoxStyle>
                                    </AddNewBox>
                                    <ClientSideEvents MouseOverHandler="MouseOverHandler" MouseOutHandler="MouseOutHandler">
                                    </ClientSideEvents>
                                    <ActivationObject BorderColor="" BorderWidth="">
                                    </ActivationObject>
                                    <FilterOptionsDefault AllowRowFiltering="OnClient" FilterUIType="HeaderIcons">
                                        <FilterDropDownStyle CustomRules="overflow:auto;" BackColor="White" BorderColor="Silver"
                                            BorderWidth="1px" BorderStyle="Solid" Font-Names="Verdana,Arial,Helvetica,sans-serif"
                                            Font-Size="11px" Height="300px" Width="200px">
                                            <Padding Left="2px"></Padding>
                                        </FilterDropDownStyle>
                                        <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                                        </FilterHighlightRowStyle>
                                        <FilterOperandDropDownStyle CustomRules="overflow:auto;" BackColor="White" BorderColor="Silver"
                                            BorderWidth="1px" BorderStyle="Solid" Font-Names="Verdana,Arial,Helvetica,sans-serif"
                                            Font-Size="11px">
                                            <Padding Left="2px"></Padding>
                                        </FilterOperandDropDownStyle>
                                    </FilterOptionsDefault>
                                </DisplayLayout>
                            </igtbl:UltraWebGrid>
                                </ContentTemplate>
                        </asp:updatepanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           
                            <igtbldocexp:UltraWebGridDocumentExporter ID="ULTWDE" runat="server">
                            </igtbldocexp:UltraWebGridDocumentExporter>
                            <igtblexp:UltraWebGridExcelExporter ID="ULTWGEE" runat='server'>
                            </igtblexp:UltraWebGridExcelExporter>
                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetPOsPushedToIRMA"
                                TypeName="MultiPOTool.DAOValidatedPOs" 
                                OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:Parameter Name="UserID" Type="Int32" />
                                    <asp:Parameter Name="Top" Type="Int32" DefaultValue="100" />
                                    <asp:Parameter Name="StartDate" Type="DateTime" DefaultValue="01/01/1900" />
                                    <asp:Parameter Name="EndDate" Type="DateTime" DefaultValue="01/01/1900" />
                                    <asp:Parameter Name="Store" Type="String" DefaultValue="0" />
                                    <asp:Parameter Name="Vendor" Type="Int32" DefaultValue="0" />
                                    <asp:Parameter Name="Subteam" Type="Int32" DefaultValue="0" />
                                    <asp:SessionParameter DefaultValue="1" Name="POType" SessionField="POType" 
                                        Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                </table>
               <%--<Pager MinimumPagesForDisplay="2">
                            <PagerStyle BackColor="LightGray" BorderWidth="1px" BorderStyle="Solid">
                                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px">
                                </BorderDetails>
                            </PagerStyle>
                        </Pager>--%>
            </td>
        </tr>
    </table>
</asp:Content>

