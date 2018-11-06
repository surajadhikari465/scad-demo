Partial Class Accounting_GLUploadReport
    Inherits System.Web.UI.Page
    Private oListItemSelect As New System.Web.UI.WebControls.ListItem("<Select a Value>", CType(0, String))
    Private oListItemAll As New System.Web.UI.WebControls.ListItem("ALL", CType(1, String))

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete
        Dim dtMinDate As Date = "01/01/1990"
        Dim dtMaxDate As Date = "01/01/2050"

        'set minimum date
        'rng_StartDate.MinimumValue = dtMinDate
        'rng_EndDate.MinimumValue = dtMinDate

        dteStartDate.MinDate = dtMinDate
        dteEndDate.MinDate = dtMinDate

        'set maximum date
        'rng_StartDate.MaximumValue = dtMaxDate
        'rng_EndDate.MaximumValue = dtMaxDate

        dteStartDate.MaxDate = dtMaxDate
        dteEndDate.MaxDate = dtMaxDate

        dteStartDate.NullDateLabel = "< Enter Date >"
        dteEndDate.NullDateLabel = "< Enter Date >"
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oListItemSelect = Nothing
        oListItemAll = Nothing
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            cmbOrderType.Items.Add(oListItemSelect)
            cmbOrderType.Items.Add(New System.Web.UI.WebControls.ListItem("Distribution", "D"))
            cmbOrderType.Items.Add(New System.Web.UI.WebControls.ListItem("Transfers", "T"))

            optPreviousFP.Checked = True
        End If
    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound
        If cmbStore.Items.Count > 0 Then
            cmbStore.Items.Insert(0, oListItemSelect)
        Else
            cmbStore.Items.Add(oListItemSelect)
        End If
    End Sub

    Protected Sub cmbOrderType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOrderType.SelectedIndexChanged
        If cmbOrderType.SelectedValue.ToString = "D" Then
            ICStores.SelectCommand = "GetDistAndMfg"
            optPreviousFP.Enabled = True
            optPreviousFP.Checked = True
            optCustomDate.Checked = False
            SetDateControls(False)
        ElseIf cmbOrderType.SelectedValue.ToString = "T" Then
            ICStores.SelectCommand = "GetStores"
            'optPreviousFP.Enabled = False
            optPreviousFP.Checked = False
            optCustomDate.Checked = True
            SetDateControls(True)
        Else
            ICStores.SelectCommand = "GetAllStores"
            optPreviousFP.Enabled = True
            optPreviousFP.Checked = True
            optCustomDate.Checked = False
            SetDateControls(False)
        End If
    End Sub

    Protected Sub optCustomDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optCustomDate.CheckedChanged
        SetDateControls(True)
        'rng_StartDate.Enabled = True
        'rng_EndDate.Enabled = True
    End Sub

    Protected Sub optPreviousFP_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optPreviousFP.CheckedChanged
        SetDateControls(False)
        'rng_StartDate.Enabled = False
        'rng_EndDate.Enabled = False
    End Sub

    Private Sub SetDateControls(ByVal blnDateValue As Boolean)
        lblStartDate.Enabled = blnDateValue
        dteStartDate.Enabled = blnDateValue

        lblEndDate.Enabled = blnDateValue
        dteEndDate.Enabled = blnDateValue
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder

        sReportURL.Append("GLUploadReport")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If optPreUpload.Checked Then
            sReportURL.Append("&IsUploaded=false")
        Else
            sReportURL.Append("&IsUploaded=true")
        End If

        If cmbOrderType.SelectedValue = CStr(0) Then
            sReportURL.Append("&OrderType:isnull=true")
        Else
            sReportURL.Append("&OrderType=" & cmbOrderType.SelectedValue)
        End If

        If cmbStore.SelectedValue = 0 Then
            sReportURL.Append("&Store_No:isnull=true")
        Else
            sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)
        End If

        If optPreviousFP.Checked Then
            sReportURL.Append("&StartDate:isnull=true")
            sReportURL.Append("&EndDate:isnull=true")
        Else
            sReportURL.Append("&StartDate=" & dteStartDate.Value)
            sReportURL.Append("&EndDate=" & dteEndDate.Value)
        End If

        ' Show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub
End Class
