Option Strict Off
Option Explicit On
Imports WholeFoods.Utility
Public Class frmZeroCostItemsReport

    Private Sub cmbSubTeam_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubTeam.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 8 Then cmbSubTeam.SelectedIndex = -1

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        Me.Close()

    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click

        ' Parameters declaration to call Reporting Services.
        Dim sReportURL As New System.Text.StringBuilder
        Dim filename As String

        '--------------------------
        ' Setup Report URL
        '--------------------------

        ' Input Validation for Store.
        If cmbStore.SelectedIndex = -1 Then
            MsgBox("Store is required.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
            cmbStore.Focus()
            Exit Sub
        End If

        ' Input validation for SubTeam.
        If cmbSubTeam.SelectedIndex = -1 Then
            MsgBox("SubTeam is required.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
            cmbSubTeam.Focus()
            Exit Sub
        End If


        filename = ConfigurationServices.AppSettings("Region")

        ' Region Name has been removed from the report file name.
        ' filename = filename + "_ZeroCostItemsReport"
        filename = "ZeroCostItemsReport"
        sReportURL.Append(filename)

        ' This chooses the region and based on the results points to the correct report.

        ' Report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")


        If cmbStore.Text = "ALL" Or cmbStore.Text = "" Then
            sReportURL.Append("&Store_No:isnull=true")
        Else
            sReportURL.Append("&Store_No=" & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex))
        End If

        If cmbSubTeam.Text = "ALL" Or cmbSubTeam.Text = "" Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        End If

        Call ReportingServicesReport(sReportURL.ToString)

    End Sub

    Private Sub frmZeroCostReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        CenterForm(Me)

        LoadStore(cmbStore)
        cmbStore.Items.Insert(0, "ALL")
        If glStore_Limit > 0 Then
            SetActive(cmbStore, False)
            SetCombo(cmbStore, glStore_Limit)
        Else
            cmbStore.SelectedIndex = 0
        End If

        LoadAllSubTeams(cmbSubTeam)
        cmbSubTeam.Items.Insert(0, "ALL")
        cmbSubTeam.SelectedIndex = 0
    End Sub
End Class