Imports System.Globalization
Imports ReportManagerWebApp.WFM_Common

Partial Class Misc_SoxConflict
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete
        Dim dtMinDate As Date = DateAdd(DateInterval.Year, -5, Today())
        Dim dtMaxDate As Date = DateAdd(DateInterval.Year, 2, Today())

        dteStartDate.MinDate = dtMinDate
        dteStartDate.MaxDate = dtMaxDate
        dteStartDate.Value = Now.Date

        dteEndDate.MinDate = dtMinDate
        dteEndDate.MaxDate = dtMaxDate
        dteEndDate.Value = Now.Date

        dteStartDate.NullDateLabel = "< Enter Date >"
        dteEndDate.NullDateLabel = "< Enter Date >"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim itm1 As New ListItem
        Dim itm2 As New ListItem

        If Not Page.IsPostBack Then
            itm1.Text = "Title"
            itm1.Value = "T"

            itm2.Text = "User"
            itm2.Value = "U"

            cmbConflictType.Items.Insert(0, oListItemDefault)
            cmbConflictType.Items.Insert(1, itm1)
            cmbConflictType.Items.Insert(1, itm2)
        End If
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oListItemAll = Nothing
        oListItemDefault = Nothing
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder

        Dim sDateFormat As String = System.Globalization.DateTimeFormatInfo.CurrentInfo().ShortDatePattern



        'report name
        sReportURL.Append("IRMASOXConflictReport")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If IsNumeric(cmbConflictType.SelectedValue) Then
            sReportURL.Append("&ConflictType:isnull=true")
        Else
            sReportURL.Append("&ConflictType=" & cmbConflictType.SelectedValue)
        End If

        If cmbInsertedByUser.SelectedValue = 0 Then
            sReportURL.Append("&InsertUserId:isnull=true")
        Else
            sReportURL.Append("&InsertUserId=" & cmbInsertedByUser.SelectedValue)
        End If

        If dteStartDate.Value Is DBNull.Value Then
            sReportURL.Append("&StartDate:isnull=true")
        Else
            sReportURL.Append("&StartDate=" & CDate(GetUniversalDateString(dteStartDate.Value)).ToString("MM/dd/yyyy"))
        End If

        If dteStartDate.Value Is DBNull.Value Then
            sReportURL.Append("&EndDate:isnull=true")
        Else
            sReportURL.Append("&EndDate=" & CDate(GetUniversalDateString(dteEndDate.Value)).ToString("MM/dd/yyyy"))
        End If

        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
    End Sub

    Protected Sub cmbInsertedByUser_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbInsertedByUser.DataBound
        'add the default item
        cmbInsertedByUser.Items.Insert(0, oListItemDefault)
    End Sub
End Class
