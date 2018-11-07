Imports System.Globalization
Imports ReportManagerWebApp.WFM_Common

Partial Class Distribution_WarehouseMovementReport
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete
        Dim dtMinDate As Date = DateAdd(DateInterval.Year, -5, Today())
        Dim dtMaxDate As Date = DateAdd(DateInterval.Year, 2, Today())

        ''set minimum date
        'rngValid_ExpectedDate.MinimumValue = dtMinDate
        'dteExpectedDate.MinDate = dtMinDate

        ''set maximum date
        'rngValid_ExpectedDate.MaximumValue = dtMaxDate
        'dteExpectedDate.MaxDate = dtMaxDate

        'dteExpectedDate.NullDateLabel = "<Enter Date>"
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oListItemAll = Nothing
        oListItemDefault = Nothing
    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDCStore.DataBound
        'add the default item
        cmbDCStore.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder
        Dim sDateFormat As String = System.Globalization.DateTimeFormatInfo.CurrentInfo().ShortDatePattern

        'report name
        sReportURL.Append("PerpetualMovementReport")

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append("&Store_No=" & Me.cmbDCStore.SelectedItem.Value)
        sReportURL.Append("&SubTeam_No=" & cmbSubTeam.SelectedItem.Value)

        If Me.Webdatechooser1.Text.ToString <> "Null" AndAlso Me.Webdatechooser1.Text.ToString <> "" Then
            sReportURL.Append("&Date1From=" & CDate(Me.Webdatechooser1.Value).ToString("MM/dd/yyyy"))
        End If
        If Me.Webdatechooser3.Text.ToString <> "Null" AndAlso Me.Webdatechooser3.Text.ToString <> "" Then
            sReportURL.Append("&Date2From=" & CDate(Me.Webdatechooser3.Value).ToString("MM/dd/yyyy"))
        End If
        If Me.Webdatechooser5.Text.ToString <> "Null" AndAlso Me.Webdatechooser5.Text.ToString <> "" Then
            sReportURL.Append("&Date3From=" & CDate(Me.Webdatechooser5.Value).ToString("MM/dd/yyyy"))
        End If
        If Me.Webdatechooser7.Text.ToString <> "Null" AndAlso Me.Webdatechooser7.Text.ToString <> "" Then
            sReportURL.Append("&Date4From=" & CDate(Me.Webdatechooser7.Value).ToString("MM/dd/yyyy"))
        End If
        If Me.Webdatechooser9.Text.ToString <> "Null" AndAlso Me.Webdatechooser9.Text.ToString <> "" Then
            sReportURL.Append("&Date5From=" & CDate(Me.Webdatechooser9.Value).ToString("MM/dd/yyyy"))
        End If
        If Me.Webdatechooser2.Text.ToString <> "Null" AndAlso Me.Webdatechooser2.Text.ToString <> "" Then
            sReportURL.Append("&Date1To=" & CDate(Me.Webdatechooser2.Value).ToString("MM/dd/yyyy"))
        End If
        If Me.Webdatechooser4.Text.ToString <> "Null" AndAlso Me.Webdatechooser4.Text.ToString <> "" Then
            sReportURL.Append("&Date2To=" & CDate(Me.Webdatechooser4.Value).ToString("MM/dd/yyyy"))
        End If
        If Me.Webdatechooser6.Text.ToString <> "Null" AndAlso Me.Webdatechooser6.Text.ToString <> "" Then
            sReportURL.Append("&Date3To=" & CDate(Me.Webdatechooser6.Value).ToString("MM/dd/yyyy"))
        End If
        If Me.Webdatechooser8.Text.ToString <> "Null" AndAlso Me.Webdatechooser8.Text.ToString <> "" Then
            sReportURL.Append("&Date4To=" & CDate(Me.Webdatechooser8.Value).ToString("MM/dd/yyyy"))
        End If
        If Me.Webdatechooser10.Text.ToString <> "Null" AndAlso Me.Webdatechooser10.Text.ToString <> "" Then
            sReportURL.Append("&Date5To=" & CDate(Me.Webdatechooser10.Value).ToString("MM/dd/yyyy"))
        End If

        ''MsgBox(sReportURL.ToString)
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub cmbSubTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound
        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemDefault)
    End Sub
End Class
