
Partial Class Price_MarginByVendor
    Inherits System.Web.UI.Page

    Protected Sub ValidateNumber(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custValidVendorID.ServerValidate
        Try
            If rdbtn_SearchType.SelectedValue = 1 Then
                args.IsValid = True
            Else
                If IsNumeric(args.Value) Then
                    If CInt(args.Value) = CSng(args.Value) Then
                        If args.Value > 0 Then
                            args.IsValid = True
                        Else
                            args.IsValid = False
                            'custValidVendorID.ErrorMessage = "Vendor ID must be an integer greater than zero."
                        End If
                    Else
                        args.IsValid = False
                        'custValidVendorID.ErrorMessage = "Vendor ID must be an integer value."
                    End If
                Else
                    args.IsValid = False
                    'custValidVendorID.ErrorMessage = "Vendor ID must be an integer value."
                End If
            End If
        Catch ex As Exception
            args.IsValid = False
        End Try
    End Sub

    Protected Sub btnGetVendors_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetVendors.Click
        If Page.IsValid Then

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
        Else
            ' Clear out anything in the current search list
            lstVendors.ClearSelection()
            lstVendors.DataSourceID() = ""
        End If
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder




        'report name
        sReportURL.Append(Application.Get("region") + "_" + "MarginByVendor")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)
        sReportURL.Append("&Vendor_ID=" & lstVendors.SelectedValue)
        sReportURL.Append("&Minval=" & txtMinval.Text)
        sReportURL.Append("&Maxval=" & txtMaxval.Text)
        sReportURL.Append("&Range=" & IIf(rdbtn_InRange.Checked, "true", "false"))

        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
