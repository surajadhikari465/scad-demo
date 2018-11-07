<%@ Page Language="VB" MasterPageFile="~/UserInterface/MasterPage.master" AutoEventWireup="false" Inherits="SLIM.UserInterface_VendorRequest_VendorForm" title="Vendor Form" Codebehind="VendorForm.aspx.vb" %>
    <%@ MasterType VirtualPath="~/UserInterface/MasterPage.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h3>New IRMA Vendor Request</h3><br />
    <table style="position: static; border-left-color: black; border-bottom-color: black; border-top-style: solid; border-top-color: black; border-right-style: solid; border-left-style: solid; border-right-color: black; border-bottom-style: solid;" border="1">
        <tr>
            <td style="width: 100px;border-width:0" align="right">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="12px" Font-Underline="False"
                    Style="position: static" Text="Vendor Name:" Width="96px" Font-Names="Tahoma"></asp:Label></td>
            <td style="width: 100px;border-width:0">
                <asp:TextBox ID="VendorName" runat="server" Style="position: static" MaxLength="33" TabIndex="1" ToolTip="Company Name" Width="238px"></asp:TextBox>*</td>
            <td style="width: 514px;border-width:0">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="VendorName"
                    ErrorMessage="* Required Field" Font-Size="X-Small" Style="position: static"
                    Width="104px"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 100px;border-width:0" align="right">
                <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="12px" Font-Underline="False"
                    Style="position: static" Text="Address Line 1:" Width="96px" Font-Names="Tahoma"></asp:Label></td>
            <td style="width: 100px;border-width:0">
                <asp:TextBox ID="Address1" runat="server" Style="position: static" MaxLength="50" TabIndex="2" ToolTip="max 50 characters" Width="238px"></asp:TextBox>*</td>
            <td style="width: 514px;border-width:0">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Address1"
                    ErrorMessage="* Required Field" Font-Size="X-Small" Style="position: static"
                    Width="104px"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 100px;border-width:0" align="right">
                <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Size="12px" Font-Underline="False"
                    Style="position: static" Text="Address Line 2:" Width="96px" Font-Names="Tahoma"></asp:Label></td>
            <td style="width: 100px; border-width:0">
                <asp:TextBox ID="Address2" runat="server" Style="position: static" MaxLength="50" TabIndex="2" ToolTip="max 50 characters" Width="237px"></asp:TextBox></td>
            <td style="width: 514px; border-width:0">
                </td>
        </tr>
        <tr>
            <td style="width: 100px; border-width:0" align="right">
                <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="12px" Font-Underline="False"
                    Style="position: static" Text="City:" Width="96px" Font-Names="Tahoma"></asp:Label></td>
            <td style="width: 100px; border-width:0">
                <asp:TextBox ID="City" runat="server" Style="position: static" MaxLength="33" TabIndex="3" ToolTip="max. 33 characters" Width="237px"></asp:TextBox>*</td>
            <td style="width: 514px; border-width:0">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="City"
                    ErrorMessage="* Required Field" Font-Size="X-Small" Style="position: static"
                    Width="104px"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 100px;border-width:0" align="right">
                <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="12px" Font-Underline="False"
                    Style="position: static" Text="State:" Width="96px" Font-Names="Tahoma"></asp:Label></td>
            <td style="width: 100px; border-width:0">
                <asp:TextBox ID="State" runat="server" Style="position: static" MaxLength="2" TabIndex="4" ToolTip="ex. CA" Width="21px"></asp:TextBox>*</td>
            <td style="width: 514px;border-width:0">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="State"
                    ErrorMessage="* Required Field" Font-Size="X-Small" Style="position: static"
                    Width="104px"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 100px; border-width:0" align="right">
                <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="12px" Font-Underline="False"
                    Style="position: static" Text="Zip Code:" Width="96px" Font-Names="Tahoma"></asp:Label></td>
            <td style="width: 100px; border-width:0">
                <asp:TextBox ID="ZipCode" runat="server" Style="position: static" MaxLength="10" TabIndex="5" ToolTip="ex. 91423" Width="76px"></asp:TextBox>*</td>
            <td style="width: 514px; border-width:0">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ZipCode"
                    ErrorMessage="* Required Field" Font-Size="X-Small"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td style="width: 100px; border-width:0" align="right">
                <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Size="12px" Font-Underline="False"
                    Style="position: static" Text="Phone:" Width="96px" Font-Names="Tahoma"></asp:Label></td>
            <td style="width: 100px; border-width:0">
                <asp:TextBox ID="Phone" runat="server" Style="position: static" MaxLength="20" TabIndex="6" ToolTip="ex. 818-234-2345"></asp:TextBox></td>
            <td style="width: 514px; border-width:0">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="Phone"
                    ErrorMessage="Invalid Phone Number" Font-Size="X-Small" Style="position: static"
                    ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"></asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td align="right" style="border-top-width: 0px; border-left-width: 0px; border-bottom-width: 0px;
                width: 100px; border-right-width: 0px">
                <asp:Label ID="Label14" runat="server" Font-Bold="True" Font-Names="Tahoma" Font-Size="12px"
                    Font-Underline="False" Style="position: static" Text="Fax:" Width="96px"></asp:Label></td>
            <td style="border-top-width: 0px; border-left-width: 0px; border-bottom-width: 0px;
                width: 100px; border-right-width: 0px">
                <asp:TextBox ID="Fax" runat="server" MaxLength="20" Style="position: static"
                    TabIndex="7" ToolTip="ex. 818-234-2345"></asp:TextBox></td>
            <td style="border-top-width: 0px; border-left-width: 0px; border-bottom-width: 0px;
                width: 514px; border-right-width: 0px">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="Fax"
                    ErrorMessage="Invalid Fax Number" Font-Size="X-Small" Style="position: static"
                    ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"></asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td style="width: 100px; border-width:0" align="right">
                <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Size="12px" Font-Underline="False"
                    Style="position: static" Text="Contact:" Width="96px" Font-Names="Tahoma"></asp:Label></td>
            <td style="width: 100px; border-width:0">
                <asp:TextBox ID="Contact" runat="server" Style="position: static" MaxLength="50" TabIndex="8" ToolTip="max 50 characters" Width="237px"></asp:TextBox></td>
            <td style="width: 514px; border-width:0">
            </td>
        </tr>
        <tr>
            <td style="width: 100px; border-width:0" align="right">
                <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="12px" Font-Underline="False"
                    Style="position: static" Text="Insurance #:" Width="96px" Font-Names="Tahoma"></asp:Label></td>
            <td style="width: 100px; border-width:0">
                <asp:TextBox ID="Liability" runat="server" Style="position: static" MaxLength="20" TabIndex="9" ToolTip="Vendor's Insurance Number"></asp:TextBox></td>
            <td style="width: 514px; border-width:0">
                </td>
        </tr>
        <tr>
            <td style="width: 100px; border-width:0" align="right">
                <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Size="12px" Font-Underline="False"
                    Style="position: static" Text="Key:" Width="96px" Font-Names="Tahoma"></asp:Label></td>
            <td style="width: 100px; border-width:0">
                <asp:TextBox ID="VendorKey" runat="server" Style="position: static" MaxLength="20" TabIndex="10" ToolTip="Vendor Key"></asp:TextBox></td>
            <td style="width: 514px; border-width:0">
                </td>
        </tr>
        <tr>
            <td style="width: 100px; border-width:0" align="right">
                <asp:Label ID="Label13" runat="server" Font-Bold="True" Font-Size="12px" Font-Underline="False"
                    Style="position: static" Text="PS Vendor Number:" Width="96px" Font-Names="Tahoma"></asp:Label></td>
            <td style="width: 100px; border-width:0">
                <asp:TextBox ID="PSVendor" runat="server" Style="position: static" MaxLength="20" TabIndex="11" ToolTip="PeopleSoft Vendor #"></asp:TextBox></td>
            <td style="width: 514px; border-width:0">
                </td>
        </tr>
        <tr>
            <td style="width: 100px; border-width:0" align="right">
                <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Size="12px" Font-Underline="False"
                    Style="position: static" Text="PS Vendor Export:" Width="96px" Font-Names="Tahoma"></asp:Label></td>
            <td style="width: 100px; border-width:0">
                <asp:TextBox ID="txtPSVendorExport" runat="server" Style="position: static" MaxLength="20" TabIndex="11" ToolTip="PeopleSoft Vendor Export #"></asp:TextBox></td>
            <td style="width: 514px; border-width:0">
                </td>
        </tr>
        <tr>
            <td style="width: 100px; border-width:0" align="right">
                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="12px" Font-Underline="False"
                    Style="position: static" Text="Email:" Width="96px" Font-Names="Tahoma"></asp:Label></td>
            <td style="width: 100px; border-width:0">
                <asp:TextBox ID="Email" runat="server" Style="position: static" MaxLength="50" TabIndex="12" ToolTip="ex. slimuser@wholefoods.com" Width="238px"></asp:TextBox></td>
            <td style="width: 514px; border-width:0">
                </td>
        </tr>
                <tr>
            <td style="width: 100px; border-width:0; vertical-align: top" align="right">
                <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Size="12px" Font-Underline="False"
                    Style="position: relative" Text="Notes:" Width="96px" Font-Names="Tahoma"></asp:Label></td>
            <td style="width: 100px; border-width:0">
                <asp:TextBox ID="Notes" runat="server" Style="position: static" MaxLength="250" TabIndex="13" ToolTip="Input for any additional Vendor info." TextMode="MultiLine" Height="80px" Width="285px"></asp:TextBox></td>
            <td style="width: 514px; border-width:0">
                </td>
        </tr>
        <tr>
        </tr>
        <tr>
            <td style="width: 100px; border-width:0; height: 14px;" align="right">
                </td>
            <td colspan="2" style=" border-width:0">
                <table style="width: 366px;" border="0">
                    <tr>
                        <td colspan="2" style="width: 118px; border-width:0; height: 26px;">
                        </td>
                        <td style="height: 26px; width: 157px; border-width:0;">
                         <asp:Button ID="SubmitVendor" runat="server" Style="position: static" Text="Submit" Width="88px" TabIndex="14" ToolTip="Submit Vendor Request" />
                       </td>
                    </tr>
                </table>
                * denotes a required Field</td>
        </tr>
    </table>
    <asp:Label ID="MsgLabel" runat="server" Font-Bold="True" Font-Size="Medium" Height="48px"
        SkinID="MessageLabelLarge" Style="position: static" Width="352px"></asp:Label>
</asp:Content>

