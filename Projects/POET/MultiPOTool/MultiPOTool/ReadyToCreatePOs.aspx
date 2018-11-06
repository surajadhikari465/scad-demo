<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MultiPOTool.Master"
    CodeBehind="ReadyToCreatePOs.aspx.vb" Inherits="MultiPOTool.ReadyToCreatePOs"
    Title="Ready To Create POs" %>

<%@ Register Assembly="Infragistics35.WebUI.UltraWebGrid.v10.1, Version=10.1.20101.1011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid" TagPrefix="igtbl" %>

<%@ Register Assembly="Infragistics35.WebUI.UltraWebGrid.ExcelExport.v10.1, Version=10.1.20101.1011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid.ExcelExport" TagPrefix="igtblexp" %>

<%@ Register Assembly="Infragistics35.WebUI.UltraWebGrid.DocumentExport.v10.1, Version=10.1.20101.1011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid.DocumentExport" TagPrefix="igtbldocexp" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<span style="font-size: large; color: #FF0000; font-family: 'Times New Roman', Times, serif; font-style: italic">
    <br />
    </span> 

<span style="font-size: medium; color: #FF0000; font-family: 'Times New Roman', Times, serif; font-style: italic">
    Note: POs that are scheduled for Auto Push today will be pushed to IRMA immediately. Please look at the ‘Pushed To IRMA’ page if you are not seeing your POs (scheduled for auto push today) displayed here.<br />
    </span> 

<span style="font-size: large; color: #FF0000; font-family: 'Times New Roman', Times, serif; font-style: italic">
    <br />
    </span> 

    &nbsp;<table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td  align="left">
                            Regional User</td>
            <td  align="left">
                            <asp:DropDownList ID="ddlUsers" runat="server" Height="20px" 
                    Width="238px" DataSourceID="dsRegionalUsers" DataTextField="UserName" 
                                DataValueField="UserID" AutoPostBack="True">
                            </asp:DropDownList>
                            <asp:Label ID="lblUserID" runat="server" Text="Label" Visible="False"></asp:Label>
                            <asp:SqlDataSource ID="dsRegionalUsers" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:MultiPOToolConnectionString %>" 
                                SelectCommand="GetRegionalUsers" SelectCommandType="StoredProcedure" 
                                ProviderName="<%$ ConnectionStrings:MultiPOToolConnectionString.ProviderName %>">
                                <SelectParameters>
                                        <asp:SessionParameter Name="UserID" SessionField="UserID" Type="Int32" />
                                </SelectParameters>
                            </asp:SqlDataSource>
            </td>
        </tr>
        <tr>
            <td  align="center" colspan="2" style="padding:30px;">
                <br />
                <asp:Label ID="lblPOsReadyToPush" runat="server" Text="Label" Font-Bold="True" CssClass="HeaderLable" Font-Italic="true"></asp:Label><br />
            </td>
        </tr>
        <tr id="trExport" runat="server">
            <td colspan="2">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 18%">
                            POs Per Page
                        </td>
                        <td style="text-align: left">
                            <asp:DropDownList ID="ddlPaging" runat="server" AutoPostBack="True">
                                <asp:ListItem Selected="True">25</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>75</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                                <asp:ListItem>all</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                      
                        <td style="width: 18%; height: 26px;">
                            Export Format
                        </td>
                        <td style="text-align: left; height: 26px;">
                            <asp:DropDownList ID="ddlExportFormat" runat="server">
                                <asp:ListItem>Excel</asp:ListItem>
                                <asp:ListItem>PDF</asp:ListItem>
                                <asp:ListItem>Text</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;
                            <asp:Button ID="btnExport" runat="server" Text="Export PO List" Width="128px" CssClass="button"  />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Label ID="ErrorLabel" runat="server" Style="position: static" Width="352px"></asp:Label><br />
            </td>
        </tr>
        <tr>
            <td align="center" style="width: 170px; text-align: left">
                            <br />
            </td>
            <td align="center" style="text-align: left">
                            &nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                <igtbl:UltraWebGrid ID="uwgPOsReadyToPush" runat="server" DataSourceID="dsPOsReadyToPush"
                    Width="100%" >
                    <Bands>
                        <igtbl:UltraGridBand>
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
                            <AddNewRow Visible="NotSet" View="NotSet">
                            </AddNewRow>
                        </igtbl:UltraGridBand>
                    </Bands>
                    <DisplayLayout Version="4.00" SelectTypeRowDefault="Extended" Name="uwgPOsReadyToPush"
                        AllowDeleteDefault="Yes" AllowUpdateDefault="Yes" AllowColSizingDefault="Free"
                        RowHeightDefault="20px" TableLayout="Fixed" ViewType="OutlookGroupBy" RowSelectorsDefault="No"
                        AllowColumnMovingDefault="OnServer" StationaryMargins="Header" BorderCollapseDefault="Separate"
                        StationaryMarginsOutlookGroupBy="True" AllowSortingDefault="Yes" HeaderClickActionDefault="SortMulti">
                        <FrameStyle BackColor="Window" BorderColor="InactiveCaption" BorderWidth="1px" BorderStyle="Solid"
                            Font-Names="Microsoft Sans Serif" Font-Size="8.25pt" Width="100%">
                        </FrameStyle>
                        <Pager MinimumPagesForDisplay="2" AllowPaging="True">
                            <PagerStyle BackColor="LightGray" BorderWidth="1px" BorderStyle="Solid">
                                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px">
                                </BorderDetails>
                            </PagerStyle>
                        </Pager>
                        <EditCellStyleDefault BorderWidth="0px" BorderStyle="None">
                        </EditCellStyleDefault>
                        <FooterStyleDefault BackColor="LightGray" BorderWidth="1px" BorderStyle="Solid">
                            <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px">
                            </BorderDetails>
                        </FooterStyleDefault>
                        <HeaderStyleDefault HorizontalAlign="Left" BackColor="LightGray" BorderStyle="Solid">
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
                        <GroupByBox Prompt="Drag a column header here to group by that column - Double Click on Header To Select All CheckBoxes">
                            <BoxStyle BackColor="ActiveBorder" BorderColor="Window">
                            </BoxStyle>
                        </GroupByBox>
                        <AddNewBox Hidden="False">
                            <BoxStyle BackColor="Window" BorderColor="InactiveCaption" BorderWidth="1px" BorderStyle="Solid">
                                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px">
                                </BorderDetails>
                            </BoxStyle>
                        </AddNewBox>
                        <ActivationObject BorderColor="" BorderWidth="">
                        </ActivationObject>
                        <FilterOptionsDefault>
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
 
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:ObjectDataSource ID="dsPOsReadyToPush" runat="server" SelectMethod="GetPOsReadyToPushByUser"
                    TypeName="MultiPOTool.DAOValidatedPOs" DeleteMethod="DeletePOs" OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:Parameter Name="UserID" Type="Int32" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="UserID" Type="Int32" />
                        <asp:Parameter Name="POHeaderID" Type="Int32" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
                <igtbldocexp:UltraWebGridDocumentExporter ID="udeExportReadyToCreate" runat="server">
                </igtbldocexp:UltraWebGridDocumentExporter>
                <igtblexp:UltraWebGridExcelExporter ID="udeExcelExportReadyToCreate" runat="server">
                </igtblexp:UltraWebGridExcelExporter>
            </td>
        </tr>
        <tr id="trPushDelete" runat="server">
            <td colspan="2">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnPushToIRMA" runat="server" Text="Push Selected POs to IRMA" CssClass="button" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnDeletePOs" runat="server" Text="Delete Selected POs" CssClass="button"  />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
