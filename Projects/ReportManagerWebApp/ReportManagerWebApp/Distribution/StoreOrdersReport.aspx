<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="ReportManagerWebApp.Distribution_StoreOrdersReport" Title="Store Orders Report" Codebehind="StoreOrdersReport.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="navbar"><h3><a href="..\Default.aspx">Home</a> &gt; <a href="DistributionReports.aspx?valuePath=Reports Home/Distribution">Distribution Reports</a> &gt; Store Orders Report</h3>
</div>
    <table border="0">
        <tr>
            <td style="width: 152px; height: 1px; text-align: right">Store</td>
            <td style="width: 458px; height: 1px; text-align: left">
                <asp:DropDownList ID="cmbStore" runat="server" DataSourceID="ICStore" DataTextField="Store_Name" DataValueField="Store_No" Width="225px"></asp:DropDownList>
                <asp:SqlDataSource ID="ICStore" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="
                                    SELECT
	                                    Store_No,
	                                    Store_Name
                                    FROM
	                                    Store
                                    WHERE
	                                    WFM_Store = 1
	                                    OR Mega_Store = 1
                                    ORDER BY Store_Name
                                  ">
                </asp:SqlDataSource>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
        <tr>
            <td style="width: 152px; height: 1px; text-align: right">SubTeam</td>
            <td style="width: 458px; height: 1px; text-align: left">
                <asp:DropDownList ID="cmbSubTeam" runat="server" DataSourceID="ICSubTeam" DataTextField="SubTeamName" DataValueField="SubTeamNo" Width="225px"></asp:DropDownList>
                <asp:SqlDataSource ID="ICSubTeam" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>"
                    SelectCommand="
                                    SELECT 
                                        [SubTeamNo]	    =	st.SubTeam_No, 
                                        [SubTeamName]	=	st.SubTeam_Name
                                    FROM
                                        SubTeam			        (nolock) st
                                        INNER JOIN StoreSubTeam	(nolock) sst	ON	st.SubTeam_No = sst.SubTeam_No
                                    WHERE
                                        sst.PS_SubTeam_No IS NOT NULL
                                    GROUP BY
                                        st.SubTeam_No,
                                        st.SubTeam_Name	
                                    ORDER BY
                                        st.SubTeam_No
                                  ">
                </asp:SqlDataSource>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
        <tr>
            <td style="width: 152px; height: 1px; text-align: right">Start Date</td>
            <td style="width: 458px; height: 1px; text-align: left">
                <asp:DropDownList ID="cmbStartDate" runat="server" DataSourceID="ICStartDate" DataTextField="Date" DataValueField="Date" Width="225px"></asp:DropDownList>
                <asp:SqlDataSource ID="ICStartDate" runat="server" ConnectionString="<%$ ConnectionStrings:ReportManager_Conn %>" 
                    SelectCommand="
			                        SELECT TOP 20 
				                        [Date]		= CONVERT( char(10), d.Date_Key, 101),
				                        [Year]		= d.Year,
				                        [Period]	= d.Period,
				                        [Week]		= d.Week
			                        FROM 
				                        Date (nolock) d
			                        WHERE 
				                        Date_Key		>=	DATEADD(d, -6, GETDATE())
				                        AND Day_Of_Week =	1
                                  ">
                </asp:SqlDataSource>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
        <tr>
            <td style="width: 152px; height: 1px; text-align: right">Identifier</td>
            <td style="width: 458px; height: 1px; text-align: left">
                <asp:TextBox ID="txtIdentifier" runat="server" Width="136px"></asp:TextBox>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
        <tr>
            <td style="width: 152px; height: 1px; text-align: right">Compiled</td>
            <td style="width: 458px; height: 1px; text-align: left">
                <asp:RadioButtonList ID="rblCompiled" runat="server">
                    <asp:ListItem Text="False"  Enabled="true" Value="False"    Selected="True" />
                    <asp:ListItem Text="True"   Enabled="true" Value="True"     Selected="False" />                    
                </asp:RadioButtonList>
            </td>
            <td style="width: 147px; height: 1px; text-align: left"></td>
        </tr>
         <tr>
            <td style="width: 150px; text-align: right">Report Format</td>
             <td style="width: 346px; text-align: left">
                <asp:DropDownList ID="cmbReportFormat" runat="server" Font-Names="Verdana" Font-Size="10pt" Width="120px">
                    <asp:ListItem>CSV</asp:ListItem>
                    <asp:ListItem>EXCEL</asp:ListItem>
                    <asp:ListItem Selected="True">HTML</asp:ListItem>
                    <asp:ListItem>PDF</asp:ListItem>
                    <asp:ListItem>XML</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnReport" runat="server" Text="View Report" Width="96px" />
            </td>
            <td style="width: 318px; text-align: left"></td>
        </tr>
        </table>
</asp:Content>

