
Partial Class CostEventDetail
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        cmbTeam.Items.Add("ALL")
        cmbTeam.AppendDataBoundItems = True
        cmbTeam.DataSourceID = "ICTeam"
        cmbTeam.DataTextField = "Team_Name"
        cmbTeam.DataValueField = "Team_No"

        cmbSubTeam.Items.Add("ALL")
        cmbSubTeam.AppendDataBoundItems = True
        cmbSubTeam.DataSourceID = "ICSubteam"
        cmbSubTeam.DataTextField = "SubTeam_Name"
        cmbSubTeam.DataValueField = "SubTeam_No"

    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder

        Dim sDateFormat As String = System.Globalization.DateTimeFormatInfo.CurrentInfo().ShortDatePattern



        'report name
        sReportURL.Append("CostChangeEventReport")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&Vendor_ID=" & lstVendors.SelectedValue)

        If cmbTeam.SelectedValue = "ALL" Then
            sReportURL.Append("&Team_No:isnull=true")
        Else
            sReportURL.Append("&Team_No=" & cmbTeam.SelectedValue)
        End If

        If cmbSubTeam.SelectedValue = "ALL" Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & cmbSubTeam.SelectedValue)
        End If

        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub btnGetVendors_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetVendors.Click
        ' Clear out anything in the current search list
        lstVendors.ClearSelection()

        ' note write something to clear list if rdbtn selection changes.
        If rdbtn_SearchType.SelectedValue = 1 Then
            lstVendors.DataSourceID() = "ICVendors_CompanyName"
        ElseIf rdbtn_SearchType.SelectedValue = 2 Then
            lstVendors.DataSourceID() = "ICVendors_PSVendorID"
        ElseIf rdbtn_SearchType.SelectedValue = 3 Then
            lstVendors.DataSourceID() = "ICVendors_VendorID"
        End If

        lstVendors.DataTextField() = "CompanyName"
        lstVendors.DataValueField() = "Vendor_ID"
    End Sub

    Protected Sub ICVendors_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles ICVendors_CompanyName.Selected, ICVendors_PSVendorID.Selected, ICVendors_VendorID.Selected
        'lblVendorCount.Text = e.AffectedRows.ToString()

        If e.AffectedRows > 0 Then  ' if results are returned display nothing
            lblVendorCount.Text = ""
        Else ' if NO results are returned display a message
            lblVendorCount.Text = "No results found."
        End If

    End Sub

End Class
