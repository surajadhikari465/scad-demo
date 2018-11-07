Option Strict Off
Option Explicit On
Imports WholeFoods.Utility
Friend Class frmSpecialsByEndDate
    Inherits System.Windows.Forms.Form

    Private Sub cmbField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbField.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        Dim Index As Short = cmbField.GetIndex(eventSender)

        If KeyAscii = 8 Then cmbField(Index).SelectedIndex = -1

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        Me.Close()

    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        Dim sReportURL As New System.Text.StringBuilder
        Dim filename As String

        '--------------------------
        ' Setup Report URL
        '--------------------------
        filename = ConfigurationServices.AppSettings("Region")
        filename = filename + "_Specials"
        sReportURL.Append(filename)
        'This chooses the region and based on the results points to the correct report.'report name

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        If Not IsValidDate(dtpStartDate.Value) Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), "Start Date"), MsgBoxStyle.Critical, Me.Text)
            dtpStartDate.Focus()
            Exit Sub
        End If

        If Not IsValidDate(dtpEndDate.Value) Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), "End Date"), MsgBoxStyle.Critical, Me.Text)
            dtpEndDate.Focus()
            Exit Sub
        End If

        If dtpEndDate.Value < dtpStartDate.Value Then
            MsgBox("End date is older than begin date.", MsgBoxStyle.Exclamation, "Invalid Range!")
            dtpEndDate.Focus()
            Exit Sub
        End If

        If cmbField(0).SelectedIndex = -1 Then
            MsgBox("Store must be selected.", MsgBoxStyle.Exclamation, "Invalid Store")
            cmbField(0).Focus()
            Exit Sub
        End If

        If cmbField(1).SelectedIndex = -1 Then
            MsgBox("Subteam must be selected.", MsgBoxStyle.Exclamation, "Invalid Subteam")
            cmbField(1).Focus()
            Exit Sub
        End If

        sReportURL.Append("&StoreNo=" & VB6.GetItemData(cmbField(0), cmbField(0).SelectedIndex))
        sReportURL.Append("&SubTeamNo=" & VB6.GetItemData(cmbField(1), cmbField(1).SelectedIndex))
        sReportURL.Append("&StartDate=" & dtpStartDate.Value)
        sReportURL.Append("&EndDate=" & dtpEndDate.Value)
        sReportURL.Append("&UseEndDate=" & IIf(optDateType(1).Checked, "true", "false"))

        If gsRegionCode = "EU" Then
            ' Pass in the extra date parameter for the UK report
            sReportURL.Append("&BeginDateUS=" & FormatDateAsUS(dtpStartDate.DateTime))
            sReportURL.Append("&EndDateUS=" & FormatDateAsUS(dtpEndDate.DateTime))
        End If

        Call ReportingServicesReport(sReportURL.ToString)

    End Sub

    Private Function FormatDateAsUS(ByVal inDate As Date) As String
        Dim outDate As New System.Text.StringBuilder
        outDate.Append(String.Format("{0:##}", inDate.Month))
        outDate.Append("/")
        outDate.Append(String.Format("{0:##}", inDate.Day))
        outDate.Append("/")
        outDate.Append(String.Format("{0:####}", inDate.Year))
        Return outDate.ToString
    End Function

    Private Sub frmSpecialsByEndDate_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Dim dtDefaultDate As Date

        CenterForm(Me)

        LoadStore(cmbField(0))
        LoadAllSubTeams(cmbField(1))

        If glStore_Limit > 0 Then
            SetActive(cmbField(0), False)
            SetCombo(cmbField(0), glStore_Limit)
        Else
            cmbField(0).SelectedIndex = -1
        End If

        dtDefaultDate = DateAdd(DateInterval.Day, -1, SystemDateTime(True))

        dtpStartDate.Value = dtDefaultDate
        dtpEndDate.Value = dtDefaultDate

    End Sub
End Class