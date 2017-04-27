
Partial Class Purchases_SuspendedOrderReport
    Inherits System.Web.UI.Page


    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete
        Dim dtMinDate As Date = "01/01/1990"
        Dim dtMaxDate As Date = "01/01/2050"

        dteStartDate.MinDate = dtMinDate
        dteEndDate.MinDate = dtMinDate

        dteStartDate.MaxDate = dtMaxDate
        dteEndDate.MaxDate = dtMaxDate

        'dteStartDate.NullDateLabel = "< Enter Date >"
        'dteEndDate.NullDateLabel = "< Enter Date >"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            optCurrentYes.Checked = True
            optApprovedNo.Checked = True
        End If
    End Sub


    Private Sub SetDateControls(ByVal blnDateValue As Boolean)

        dteStartDate.Enabled = blnDateValue
        dteEndDate.Enabled = blnDateValue

    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder

        sReportURL.Append("SuspendedOrderReport")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&IncludeCurrentSuspensions=")
        If optCurrentYes.Checked Then
            sReportURL.Append("1")
        Else
            sReportURL.Append("0")
        End If

        sReportURL.Append("&IncludeApprovedSuspensions=")
        If optApprovedYes.Checked Then
            sReportURL.Append("1")
        Else
            sReportURL.Append("0")
        End If

        If cmbStore.SelectedValue = "" Then
            sReportURL.Append("&Store_No:isnull=true")
        Else
            sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)
        End If

        If cmbSubTeam.SelectedValue = "" Then
            sReportURL.Append("&SubTeamNo:isnull=true")
        Else
            sReportURL.Append("&SubTeamNo=" & cmbSubTeam.SelectedValue)
        End If

        If dteStartDate.Value Is Nothing Then
            sReportURL.Append("&BeginDate:isnull=true")
        Else
            sReportURL.Append("&BeginDate=" & dteStartDate.Value)
        End If

        If dteEndDate.Value Is Nothing Then
            sReportURL.Append("&EndDate:isnull=true")
        Else
            sReportURL.Append("&EndDate=" & dteEndDate.Value)
        End If

        ' Show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub optApprovedYes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optApprovedYes.CheckedChanged
        SetDateControls(True)
    End Sub

    Protected Sub optApprovedNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optApprovedNo.CheckedChanged
        SetDateControls(False)
    End Sub

End Class
