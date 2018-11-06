<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MultiPOTool.Master"
    CodeBehind="SetUpUsers.aspx.vb" Inherits="MultiPOTool.SetUpUsers" Title="Set Up Users" %>

<%@ Register Assembly="Infragistics35.WebUI.UltraWebGrid.v10.1, Version=10.1.20101.1011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid" TagPrefix="igtbl" %>
    
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
    <table>
        <tr>
            <td>
                <asp:Label ID="errorlabel" runat="server" Style="position: static" Width="600px"
                    ForeColor="Red"></asp:Label><br />
               
            </td>
        </tr>
        <tr>
            <td>
                <table style="position: static; width: 100%; height: 24px;" width="100%">
                    <tr>
                        <td style="width: 18%; height: 24px">
                            Users Per Page
                        </td>
                        <td style="height: 24px; text-align: left">
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
                        <td colspan="2">
                            <igtbl:UltraWebGrid ID="UltraWebGrid1" runat="server" DataSourceID="ObjectDataSource1"
                                Height="528px" Style="position: static" Width="1500px">
                                <Bands>
                                    <igtbl:UltraGridBand AllowAdd="Yes" AllowDelete="Yes" AllowSorting="Yes" AllowUpdate="Yes">
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
                                <DisplayLayout AllowAddNewDefault="Yes" AllowColSizingDefault="Free" AllowColumnMovingDefault="OnServer"
                                    AllowDeleteDefault="Yes" AllowSortingDefault="OnClient" AllowUpdateDefault="Yes"
                                    BorderCollapseDefault="Separate" HeaderClickActionDefault="SortSingle" Name="UltraWebGrid1"
                                    RowHeightDefault="20px" SelectTypeRowDefault="Single" StationaryMargins="Header"
                                    StationaryMarginsOutlookGroupBy="True" TableLayout="Fixed" Version="4.00" ViewType="OutlookGroupBy">
                                    <FrameStyle BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid"
                                        BorderWidth="1px" Font-Names="Microsoft Sans Serif" Font-Size="8.25pt" Height="528px"
                                        Width="1500px">
                                    </FrameStyle>
                                    <Pager AllowPaging="True" MinimumPagesForDisplay="2" ChangeLinksColor="true" StyleMode="CustomLabels">
                                    </Pager>
                                    <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
                                    </EditCellStyleDefault>
                                    <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                        <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
                                    </FooterStyleDefault>
                                    <HeaderStyleDefault BackColor="LightGray" BorderStyle="Solid" CustomRules="font-weight:normal;"
                                        HorizontalAlign="Left">
                                        <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
                                    </HeaderStyleDefault>
                                    <RowStyleDefault BackColor="Window" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                                        Font-Names="Microsoft Sans Serif" Font-Size="8.25pt">
                                        <Padding Left="3px" />
                                        <BorderDetails ColorLeft="Window" ColorTop="Window" />
                                    </RowStyleDefault>
                                    <GroupByRowStyleDefault BackColor="Control" BorderColor="Window">
                                    </GroupByRowStyleDefault>
                                    <GroupByBox>
                                        <BoxStyle BackColor="ActiveBorder" BorderColor="Window">
                                        </BoxStyle>
                                    </GroupByBox>
                                   <ClientSideEvents MouseOverHandler="MouseOverHandler" MouseOutHandler="MouseOutHandler"></ClientSideEvents>
                                    <AddNewBox View="Compact" Location="Top">
                                        <BoxStyle BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid" BorderWidth="1px">
                                            <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" WidthTop="1px" />
                                        </BoxStyle>
                                    </AddNewBox>
                                    <ActivationObject BorderColor="" BorderWidth="">
                                    </ActivationObject>
                                    <FilterOptionsDefault AllowRowFiltering="OnClient" FilterUIType="FilterRow">
                                        <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                                            CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif"
                                            Font-Size="11px" Height="300px" Width="200px">
                                            <Padding Left="2px" />
                                        </FilterDropDownStyle>
                                        <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                                        </FilterHighlightRowStyle>
                                        <FilterOperandDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid"
                                            BorderWidth="1px" CustomRules="overflow:auto;" Font-Names="Verdana,Arial,Helvetica,sans-serif"
                                            Font-Size="11px">
                                            <Padding Left="2px" />
                                        </FilterOperandDropDownStyle>
                                    </FilterOptionsDefault>
                                    <AddNewRowDefault Visible="Yes" View="Top">
                                    </AddNewRowDefault>
                                </DisplayLayout>
                            </igtbl:UltraWebGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="SelectAllUsers"
        TypeName="MultiPOTool.BOManageUsers" OldValuesParameterFormatString="original_{0}">
    </asp:ObjectDataSource>
</asp:Content>
