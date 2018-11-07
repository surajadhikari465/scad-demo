
Public Class Purchases_PurchaseAccrualReport
    Inherits System.Web.UI.Page
    Private oListItemAll As New System.Web.UI.WebControls.ListItem("ALL", CType(0, String))

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder

        ' Report name
        ' sReportURL.Append(Application.Get("region") + "_" + "PurchaseAccrualReport")
        ' Region Name has been removed from the file name.
        sReportURL.Append("PurchaseAccrualReport")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If cmbStore.SelectedValue = 0 Or cmbStore.Text = "ALL" Then
            sReportURL.Append("&Store_No:isnull=true")
        Else
            sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)
        End If

        If cmbSubteam.SelectedValue = 0 Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & cmbSubteam.SelectedValue)
        End If


        If dteAsOfdate.Text = "Null" Then
            sReportURL.Append("&AsOfDate:isnull=true")
        Else
            sReportURL.Append("&AsOfDate=" & dteAsOfdate.Value)
        End If

        ' Show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub cmbSubteam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubteam.DataBound
        cmbSubteam.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbSubteam_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubteam.SelectedIndexChanged

    End Sub

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete

        Dim dtMinDate As Date = DateAdd(DateInterval.Year, -5, Today())
        Dim dtMaxDate As Date = DateAdd(DateInterval.Year, 2, Today())

        'set minimum date
        rng_AsOfDateValue.MinimumValue = dtMinDate
        rng_AsOfDateValue.MinimumValue = dtMinDate

        dteAsOfdate.MinDate = dtMinDate
        dteAsOfdate.MinDate = dtMinDate

        'set maximum date
        rng_AsOfDateValue.MaximumValue = dtMaxDate
        rng_AsOfDateValue.MaximumValue = dtMaxDate

        dteAsOfdate.MaxDate = dtMaxDate
        dteAsOfdate.MaxDate = dtMaxDate

        dteAsOfdate.NullDateLabel = "< Enter Date >"

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oListItemAll = Nothing
    End Sub


End Class
