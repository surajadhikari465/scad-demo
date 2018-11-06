
Partial Class PriceChangeDetail
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'cmbSubteam.Items.Add("ALL")
        'add the default item
        'cmbSubteam.Items.Insert(0, oListItemAll)
        'cmbSubteam.AppendDataBoundItems = True
        'cmbSubteam.DataSourceID = "ICSubTeam"
        'cmbSubteam.DataTextField = "Subteam_Name"
        'cmbSubteam.DataValueField = "Subteam_No"

        'cmbClass.Items.Add("ALL")
        'add the default item
        'cmbClass.Items.Insert(0, oListItemAll)
        'cmbClass.AppendDataBoundItems = True
        'cmbClass.DataSourceID = "ICCategoriesBySubteam"
        'cmbClass.DataTextField = "Category_Name"
        'cmbClass.DataValueField = "Category_ID"

        cvSearch.Enabled = (rdbtn_SearchType.SelectedIndex = 2)
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder

        Dim sDateFormat As String = System.Globalization.DateTimeFormatInfo.CurrentInfo().ShortDatePattern



        'report name
        sReportURL.Append("PriceChangeEventReport")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&Vendor_ID=" & lstVendors.SelectedValue)

        If cmbSubteam.SelectedValue = 0 Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & cmbSubteam.SelectedValue)
        End If

        If cmbClass.SelectedValue = 0 Then
            sReportURL.Append("&Class:isnull=true")
        Else
            sReportURL.Append("&Class=" & cmbClass.SelectedValue)
        End If

        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub btnGetVendors_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetVendors.Click
        ' Clear out anything in the current search list
        lstVendors.ClearSelection()

        If Me.IsValid Then
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
        Else
            lstVendors.DataSourceID = Nothing
        End If
    End Sub

    Protected Sub ICVendors_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles ICVendors_CompanyName.Selected, ICVendors_PSVendorID.Selected, ICVendors_VendorID.Selected
        'lblVendorCount.Text = e.AffectedRows.ToString()

        If e.AffectedRows > 0 Then  ' if results are returned display nothing
            lblVendorCount.Text = ""
        Else ' if NO results are returned display a message
            lblVendorCount.Text = "No results found."
        End If

    End Sub

    Protected Sub cmbClass_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbClass.DataBound
        cmbClass.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbSubteam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubteam.DataBound
        cmbSubteam.Items.Insert(0, oListItemAll)
    End Sub
End Class
