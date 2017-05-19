<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MultiPOTool.Master"
    CodeBehind="Upload.aspx.vb" Inherits="MultiPOTool.Upload" Title="Upload" %>

<%@ Register Assembly="Infragistics35.WebUI.UltraWebGrid.v10.1, Version=10.1.20101.1011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebGrid" TagPrefix="igtbl" %>

<%@ Register Assembly="Infragistics35.WebUI.UltraWebNavigator.v10.1, Version=10.1.20101.1011, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.UltraWebNavigator" TagPrefix="ignav" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <span style="font-size: large; color: #FF0000; font-family: 'Times New Roman', Times, serif; font-style: italic">
    Note: Starting August 19, 2013 a new spreadsheet template will be required for 
    uploading POET orders. This new  
    <asp:HyperLink ID="HyperLink1" runat="server" ForeColor="Red" 
        
        NavigateUrl="http://sites/global/IRMA/IRMA Training/POET 3.0 Spreadsheet Template.xlsx">template</asp:HyperLink>
    &nbsp;will be available in the POET Help Links page!</span> 

    <table class="tablePOSetup">
        <tr>
            <td style="width: 100px; padding-bottom: 40px;">
                <asp:Label ID="ResultLabel" runat="server" ForeColor="Green" Style="position: static"
                    Width="96px"></asp:Label>
            </td>
            <td align="right">
                <asp:Button ID="btnReset" runat="server" Text="Reset/StartOver"  CssClass="button" />
            </td>
        </tr>
        
        <tr>
            <td style="width: 100px; padding-bottom: 40px;" colspan="2">
                <asp:Label ID="lblsteps" runat="server" Text="Step : "></asp:Label><asp:Image ID="img1"
                    runat="server" />
            </td>
        </tr>
        <tr id="trUpload" runat="server">
            <td style="width: 54px">
            </td>
            <td style="width: 100px">
                <asp:FileUpload ID="FileUpload1" runat="server" Style="position: static" Width="344px"  />
            </td>
            <td style="width: 100px">
            </td>
        </tr>
        <tr>
            <td style="padding-bottom: 15px;" colspan="2">
                <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red" Style="position: static"></asp:Label>
            </td>
        </tr>
        <tr>
            <td  style="padding-bottom: 30px;" colspan="2" align="center">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnImport" runat="server" Text="Import" Visible="false" CssClass="button" />
                        </td>
                        <td>
                            <asp:Button ID="btnValidate" runat="server" Text="Validate" Visible="false"  CssClass="button" />
                        </td>
                        <td>
                            <asp:Button ID="btnUpload" runat="server" Text="Upload" Visible="false"  CssClass="button" />
                        </td>
                    </tr>
                </table>
              
            </td>
        </tr>
        <tr>
            <td colspan="2">
            <asp:UpdatePanel ID="upd2" runat="server" UpdateMode="conditional">
                <ContentTemplate>
                  <igtbl:UltraWebGrid ID="UltraWebGrid1" runat="server" Style="position: static" Width="100%"
                    Visible="False" Height="100%">
                     
              
                    <Bands>
                        <igtbl:UltraGridBand>
                            <AddNewRow View="NotSet" Visible="NotSet">
                            </AddNewRow>
                        </igtbl:UltraGridBand>
                    </Bands>
                    <DisplayLayout Version="4.00" Name="UltraWebGrid1" AllowColSizingDefault="Free" RowHeightDefault="20px"
                        TableLayout="Fixed" ViewType="OutlookGroupBy" RowSelectorsDefault="No" StationaryMargins="Header"
                        BorderCollapseDefault="Separate" StationaryMarginsOutlookGroupBy="True">
                        <FrameStyle BackColor="Window" BorderColor="InactiveCaption" BorderWidth="1px" BorderStyle="Solid"
                            Font-Names="Microsoft Sans Serif" Font-Size="8.25pt" Width="100%" Height="100%">
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
                      </ContentTemplate>
            </asp:UpdatePanel>
            </td>
        </tr>
    </table>

</asp:Content>
