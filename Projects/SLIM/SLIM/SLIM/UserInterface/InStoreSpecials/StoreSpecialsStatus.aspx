<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_InStoreSpecials_StoreSpecialsStatus" EnableEventValidation ="False" Codebehind="StoreSpecialsStatus.aspx.vb" %>

<%@ Register TagPrefix="igtblexp" Namespace="Infragistics.WebUI.UltraWebGrid.ExcelExport" Assembly="Infragistics2.WebUI.UltraWebGrid.ExcelExport.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register TagPrefix="igsch" Namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics2.WebUI.WebDateChooser.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register TagPrefix="igtbl" Namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics2.WebUI.UltraWebGrid.v9.2, Version=9.2.20092.1003, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
    <%@ MasterType VirtualPath="~/UserInterface/MasterPage.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<script type="text/javascript" src="../../javascript/AutosizeWebGridColumns.js"></script>
<script language="javascript" >

function UltraWebGrid1_InitializeLayoutHandler(gridName){
	setColWidths(gridName);
}

function popitup(url) {
	newwindow=window.open(url,'name','height=320,width=250');
	if (window.focus) {newwindow.focus()}
	
}

</script>

    <img border="0" src="../../images/page_excel.png" />
    <asp:LinkButton ID="LinkButton1" runat="server" Font-Names="tahoma" Font-Size="Small">Excel</asp:LinkButton>
    &nbsp; &nbsp; &nbsp;&nbsp;
    <asp:Table ID="tblOptions" runat="server" Width="205px" BackColor="#004000">
        <asp:TableRow runat="server" HorizontalAlign="Center" VerticalAlign="Middle">
            <asp:TableCell runat="server"><asp:RadioButton ID="optApprove" runat="server" Text="Approve" Font-Bold="True" Font-Names="Tahoma" Font-Size="10pt" ForeColor="White" GroupName="ISSRequest" AutoPostBack="True" Checked="True" Width="118px" /><asp:RadioButton ID="optReject" runat="server" Text="Reject" Font-Bold="True" Font-Names="Tahoma" Font-Size="10pt" ForeColor="White" GroupName="ISSRequest" AutoPostBack="True" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Center" VerticalAlign="Middle">
            <asp:TableCell runat="server"><asp:RadioButton ID="optReprocess" runat="server" Text="Reprocess" Font-Bold="True" Font-Names="Tahoma" Font-Size="10pt" ForeColor="White" GroupName="ISSRequest" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" HorizontalAlign="Center" VerticalAlign="Middle">
            <asp:TableCell runat="server"><asp:RadioButton ID="optEndISS" runat="server" Text="End ISS" Font-Bold="True" Font-Names="Tahoma" Font-Size="10pt" ForeColor="White" GroupName="ISSRequest" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow1" runat="server" HorizontalAlign="Center" VerticalAlign="Middle">
            <asp:TableCell ID="TableCell1" runat="server"><asp:Button ID="Button_Submit" runat="server" Text="Submit" /></asp:TableCell>
        </asp:TableRow>        
    </asp:Table>
    <igtblexp:ultrawebgridexcelexporter id="UltraWebGridExcelExporter1" runat="server"
        downloadname="ISS.XLS" worksheetname="InStoreSpecials"></igtblexp:ultrawebgridexcelexporter>
    <asp:Label ID="Label_RejectionReason" runat="server" Font-Names="Tahoma" Font-Size="10pt"
        Text="Rejection Reason:" Width="112px"></asp:Label><br />
    <asp:TextBox ID="Textbox_RejectionReason" runat="server" Height="120px" TextMode="MultiLine"
        Width="496px"></asp:TextBox>&nbsp;&nbsp;
    <asp:RequiredFieldValidator ID="rfvRejectionReason" runat="server" ControlToValidate="Textbox_RejectionReason"
        ErrorMessage="RequiredFieldValidator">A rejection reason is required</asp:RequiredFieldValidator><br />
    &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;<br />
    <asp:Panel ID="pnlFilters" runat="server" Height="50px" Visible="False" Width="125px">
    <table cellpadding="2" cellspacing="0" class="SlimTable" style="width: 736px">
        <tr bgcolor="#004000" class="header">
            <td style="width: 185px; height: 20px">
                <asp:Label ID="Label1" runat="server" BackColor="#004000" Font-Bold="True" Font-Names="Tahoma"
                    Font-Size="10pt" ForeColor="White" Text="Store:"></asp:Label></td>
            <td style="height: 20px; width: 196px;">
                <asp:Label ID="Label7" runat="server" BackColor="#004000" Font-Bold="True" Font-Names="Tahoma"
                    Font-Size="10pt" ForeColor="White" Text="SubTeam:"></asp:Label></td>
            <td style="height: 20px; width: 147px;">
                <asp:Label ID="Label9" runat="server" BackColor="#004000" Font-Bold="True" Font-Names="Tahoma"
                    Font-Size="12px" ForeColor="White" Text="Start Date:"></asp:Label></td>
            <td style="height: 20px; width: 133px;">
                <asp:Label ID="Label8" runat="server" BackColor="#004000" Font-Bold="True" Font-Names="Tahoma"
                    Font-Size="12px" ForeColor="White" Text="End Date:"></asp:Label></td>
        </tr>
        <tr bgcolor="#004000">
            <td style="width: 185px; height: 47px">
                <asp:DropDownList
                ID="cmbStore" runat="server" Width="184px" DataSourceID="dsStore" DataTextField="Store_Name" DataValueField="Store_No" AutoPostBack="True">
                </asp:DropDownList></td>
            <td style="width: 196px; height: 47px">
                &nbsp;<asp:DropDownList
                ID="cmbSubTeam" runat="server" Width="184px" DataSourceID="dsSubTeam" DataTextField="SubTeam_Name" DataValueField="SubTeam_No" AutoPostBack="True">
                </asp:DropDownList></td>
            <td style="width: 147px; height: 47px">
                <igsch:WebDateChooser ID="dtpStartDate" runat="server" Font-Names="Tahoma" Font-Size="12px"
                    NullDateLabel="" Width="128px">
                    <CalendarLayout HideOtherMonthDays="True">
                        <CalendarStyle Font-Size="X-Small">
                        </CalendarStyle>
                    </CalendarLayout>
                    <ExpandEffects Duration="150" Type="Fade" />
                    <AutoPostBack ValueChanged="True" />
                </igsch:WebDateChooser>
            </td>
            <td style="height: 47px; width: 133px;">
                <igsch:WebDateChooser ID="dtpEndDate" runat="server" NullDateLabel="">
                    <CalendarLayout>
                        <CalendarStyle Font-Size="X-Small">
                        </CalendarStyle>
                    </CalendarLayout>
                    <AutoPostBack ValueChanged="True" />
                </igsch:WebDateChooser>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <br />
    <br />
    <asp:CheckBox ID="CheckBox_All" runat="server" Text="Select All" AutoPostBack="True" /><br />
    <asp:GridView ID="grdISS" runat="server" AutoGenerateColumns="False" DataSourceID="dsISS"
        HeaderStyle-Font-Names="Tahoma" HeaderStyle-HorizontalAlign="Center" RowStyle-Font-Names="Tahoma">
        <RowStyle Font-Names="Tahoma" />
        <Columns>    
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="RequestId" HeaderText="RequestId" InsertVisible="False"
                ReadOnly="True" SortExpression="RequestId" />
            <asp:BoundField DataField="Item_Key" HeaderText="Item_Key" SortExpression="Item_Key" />
            <asp:BoundField DataField="Store_No" HeaderText="Store_No" SortExpression="Store_No" />
            <asp:BoundField DataField="SubTeam_No" HeaderText="SubTeam_No" SortExpression="SubTeam_No" />
            <asp:BoundField DataField="Identifier" HeaderText="Identifier" SortExpression="Identifier" />
            <asp:BoundField DataField="Item_Description" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Description"
                SortExpression="Item_Description" />
            <asp:BoundField DataField="SubTeam_Name" HeaderText="SubTeam" SortExpression="SubTeam_Name" />
            <asp:BoundField DataField="Store_Name" HeaderText="Store" SortExpression="Store_Name" />
            <asp:BoundField DataField="Multiple" HeaderText="Multiple" SortExpression="Multiple" />
            <asp:BoundField DataField="POSPrice" HeaderText="Price" SortExpression="POSPrice" />
            <asp:BoundField DataField="SaleMultiple" HeaderText="Sale Multiple" SortExpression="SaleMultiple" />
            <asp:BoundField DataField="POSSalePrice" HeaderText="Sale Price" SortExpression="POSSalePrice" />
            <asp:BoundField DataField="StartDate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Start Date"
                SortExpression="StartDate" />
            <asp:BoundField DataField="EndDate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="End Date"
                SortExpression="EndDate" />
            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
            <asp:BoundField DataField="RequestedBy" HeaderText="Requested By" SortExpression="RequestedBy" />
            <asp:BoundField DataField="Comments" HeaderText="Comments" SortExpression="Comments" />
        </Columns>
        <HeaderStyle BackColor="#004000" Font-Names="Tahoma" HorizontalAlign="Center" />
    </asp:GridView>
    <br />
    <br />
    <asp:Label ID="Label_Message" runat="server" Font-Names="Verdana" Font-Size="12px"></asp:Label>
    <asp:SqlDataSource ID="dsISS" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="SLIM_StoreSpecialsStatus" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="0" Name="status" QueryStringField="Status"
                Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource><asp:SqlDataSource ID="dsStore" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="SELECT    Store_no, Store_Name&#13;&#10;FROM       dbo.Store&#13;&#10;WHERE     Distribution_Center = 0&#13;&#10;&#9;AND Regional = 0&#13;&#10;&#9;AND (EXEWarehouse = 0 or EXEWarehouse IS NULL )&#13;&#10;&#9;AND (WFM_Store = 1 OR Mega_Store = 1)&#13;&#10;ORDER BY Store_Name&#13;&#10;">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="dsSubTeam" runat="server" ConnectionString="<%$ ConnectionStrings:SLIM_Conn %>"
        SelectCommand="SELECT    SubTeam_no, SubTeam_Name&#13;&#10;FROM       dbo.SubTeam&#13;&#10;ORDER BY SubTeam_Name">
    </asp:SqlDataSource>
    <br />
    <br />


</asp:Content>

