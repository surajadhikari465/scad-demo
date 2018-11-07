Imports ReportManagerWebApp.WFM_Common

Partial Class Price_MarginAudit
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< ALL >", CType(0, String))

    Protected Sub cmbZone_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbZone.DataBound
        cmbZone.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbStore1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore1.DataBound
        cmbStore1.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbStore2_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore2.DataBound
        cmbStore2.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbStore3_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore3.DataBound
        cmbStore3.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbStore4_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore4.DataBound
        cmbStore4.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbStore5_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore5.DataBound
        cmbStore5.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbSubTeam1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam1.DataBound
        cmbSubTeam1.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbSubteam2_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubteam2.DataBound
        cmbSubteam2.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbSubteam3_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubteam3.DataBound
        cmbSubteam3.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbCategory_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCategory.DataBound
        cmbCategory.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbVendor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbVendor.DataBound
        cmbVendor.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder




        'report name
        sReportURL.Append(Application.Get("region") & "_GrossMarginAudit")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If cmbZone.SelectedValue = "0" Then
            sReportURL.Append("&Zone_ID:isnull=true")
        Else
            sReportURL.Append("&Zone_ID=" & cmbZone.SelectedValue.ToString())
        End If

        If cmbStore1.SelectedValue = "0" Then
            sReportURL.Append("&Store_No:isnull=true")
        Else
            sReportURL.Append("&Store_No=" & cmbStore1.SelectedValue.ToString())
        End If

        If cmbStore2.SelectedValue = "0" Then
            sReportURL.Append("&Store_No2:isnull=true")
        Else
            sReportURL.Append("&Store_No2=" & cmbStore2.SelectedValue.ToString())
        End If

        If cmbStore3.SelectedValue = "0" Then
            sReportURL.Append("&Store_No3:isnull=true")
        Else
            sReportURL.Append("&Store_No3=" & cmbStore3.SelectedValue.ToString())
        End If

        If cmbStore4.SelectedValue = "0" Then
            sReportURL.Append("&Store_No4:isnull=true")
        Else
            sReportURL.Append("&Store_No4=" & cmbStore4.SelectedValue.ToString())
        End If

        If cmbStore5.SelectedValue = "0" Then
            sReportURL.Append("&Store_No5:isnull=true")
        Else
            sReportURL.Append("&Store_No5=" & cmbStore5.SelectedValue.ToString())
        End If

        If cmbSubTeam1.SelectedValue = "0" Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & cmbSubTeam1.SelectedValue.ToString())
        End If

        If cmbSubteam2.SelectedValue = "0" Then
            sReportURL.Append("&SubTeam_No2:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No2=" & cmbSubteam2.SelectedValue.ToString())
        End If

        If cmbSubteam3.SelectedValue = "0" Then
            sReportURL.Append("&SubTeam_No3:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No3=" & cmbSubteam3.SelectedValue.ToString())
        End If

        If cmbCategory.SelectedValue = "0" Then
            sReportURL.Append("&Category_ID:isnull=true")
        Else
            sReportURL.Append("&Category_ID=" & cmbCategory.SelectedValue.ToString())
        End If

        If cmbVendor.SelectedValue = "0" Then
            sReportURL.Append("&Vendor_ID:isnull=true")
        Else
            sReportURL.Append("&Vendor_ID=" & cmbVendor.SelectedValue.ToString())
        End If

        sReportURL.Append("&Region_ID:isnull=true")
        sReportURL.Append("&MinGM=" & txtMinGM.Text.ToString())
        sReportURL.Append("&MaxGM=" & txtMaxGM.Text.ToString())

        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
        'Response.Write(sReportServer + sReportURL.ToString())
    End Sub
End Class
