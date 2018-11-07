<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MultiPOTool.Master"
    CodeBehind="Exceptions.aspx.vb" Inherits="MultiPOTool.Exceptions" Title="Exceptions" %>

<%@ Register Assembly="Infragistics35.WebUI.UltraWebGrid.v10.1, Version=10.1.20101.1011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid" TagPrefix="igtbl" %>

<%@ Register Assembly="Infragistics35.WebUI.UltraWebGrid.DocumentExport.v10.1, Version=10.1.20101.1011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid.DocumentExport" TagPrefix="igtbldocexp" %>

<%@ Register Assembly="Infragistics35.WebUI.UltraWebGrid.ExcelExport.v10.1, Version=10.1.20101.1011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid.ExcelExport" TagPrefix="igtblexp" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script language="javascript" type="text/javascript">
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
    <asp:Label ID="lblExceptions" runat="server" Text="Label" Font-Bold="True" Font-Size="Larger"></asp:Label><br />
    &nbsp;&nbsp;<br />
    <table style="text-align: left; width: 100%;">
        <tr>
            <td style="width: 18%; text-align: left; height: 24px;">
                Session
            </td>
            <td style="text-align: left; height: 24px;">
                <asp:DropDownList ID="ddlExceptionSessions" runat="server" DataSourceID="dsExceptionSessions"
                    AutoPostBack="True" DataTextField="SessionDescription" DataValueField="UploadSessionHistoryID">
                </asp:DropDownList>
              
            </td>
        </tr>
        <tr>
            <td style="width: 18%; height: 24px; text-align: left;">
                Exceptions Per Page
            </td>
            <td style="text-align: left; height: 24px;">
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
            <td style="width: 18%; text-align: left; height: 24px;">
                Export Format
            </td>
            <td  style="text-align: left; height: 24px;">
                <table>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlExportFormat" runat="server">
                                <asp:ListItem>Excel</asp:ListItem>
                                <asp:ListItem>PDF</asp:ListItem>
                                <asp:ListItem>Text</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="left">
                            <asp:Button ID="btnExport" runat="server" Text="Export Exceptions" Width="128px"
                                CssClass="button" />
                        </td>
                    </tr>
                </table>
            </td>
           
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnClearFilters" runat="server" Text="Clear Filters" CssClass="button" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <igtbl:UltraWebGrid ID="uwgExceptions" runat="server" Style="position: static" DataSourceID="dsExceptions"
                    Width="100%">
                  
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
                            <AddNewRow View="NotSet" Visible="NotSet">
                            </AddNewRow>
                        </igtbl:UltraGridBand>
                    </Bands>
                    <DisplayLayout Version="4.00" SelectTypeRowDefault="Extended" Name="uwgExceptions"
                        AllowSortingDefault="OnClient" AllowColSizingDefault="Free" RowHeightDefault="20px"
                        TableLayout="Fixed" AllowColumnMovingDefault="OnServer" HeaderClickActionDefault="SortMulti"
                        StationaryMargins="Header" BorderCollapseDefault="Separate" StationaryMarginsOutlookGroupBy="True"
                        SelectTypeCellDefault="Extended" SelectTypeColDefault="Extended">
                        <FrameStyle BackColor="Window" BorderColor="InactiveCaption" BorderWidth="1px" BorderStyle="Solid"
                            Font-Names="Microsoft Sans Serif" Font-Size="8.25pt" Width="100%">
                        </FrameStyle>
                        <Pager MinimumPagesForDisplay="2">
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
                        <GroupByBox>
                            <BoxStyle BackColor="ActiveBorder" BorderColor="Window">
                            </BoxStyle>
                        </GroupByBox>
                        <AddNewBox>
                            <BoxStyle BackColor="Window" BorderColor="InactiveCaption" BorderWidth="1px" BorderStyle="Solid">
                                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px">
                                </BorderDetails>
                            </BoxStyle>
                        </AddNewBox>
                        <ClientSideEvents MouseOverHandler="MouseOverHandler" MouseOutHandler="MouseOutHandler"></ClientSideEvents>

                        <ActivationObject BorderColor="" BorderWidth="">
                        </ActivationObject>
                        <FilterOptionsDefault FilterUIType="HeaderIcons">
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
                <igtblexp:UltraWebGridExcelExporter ID="udeExcelExportExceptions" runat="server">
                </igtblexp:UltraWebGridExcelExporter>
                <igtbldocexp:UltraWebGridDocumentExporter ID="udeExportExceptions" runat="server">
                </igtbldocexp:UltraWebGridDocumentExporter>
                <asp:ObjectDataSource ID="dsExceptionSessions" runat="server" SelectMethod="GetSessionsWithExceptionsByUserID"
                    TypeName="MultiPOTool.BOExceptions">
                    <SelectParameters>
                        <asp:Parameter Name="UserID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <br />
                <asp:ObjectDataSource ID="dsExceptions" runat="server" SelectMethod="GetExceptionsByUploadSession"
                    TypeName="MultiPOTool.BOExceptions" OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:Parameter Name="UploadSessionHistoryID" Type="Int32" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
    </table>
</asp:Content>
