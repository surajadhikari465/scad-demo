Option Strict Off
Imports WholeFoods.Utility
Public Class PurchaseAccrualsReport

  Private Sub PurchaseAccrualsReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        CenterForm(Me)

        LoadStore(cboStore)

        If glStore_Limit > 0 Then
            SetActive(cboStore, False)
            SetCombo(cboStore, glStore_Limit)
        Else
            cboStore.SelectedIndex = -1
        End If

        LoadAllSubTeams(cboSubTeam)
        cboSubTeam.Items.Insert(0, "ALL")
        cboSubTeam.SelectedIndex = 0

  End Sub

  Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
    Me.Close()
  End Sub

  Private Sub cmdReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReport.Click
        Dim sReportURL As New System.Text.StringBuilder
        Dim filename As String
        '--------------------------
        ' Setup Report URL
        '--------------------------
        filename = ConfigurationServices.AppSettings("Region")
        ' filename = filename + "_PurchaseAccrualReport"
        ' Region name has been deleted from the fileName.
        filename = "PurchaseAccrualReport"
        sReportURL.Append(filename)

        ' This chooses the region and based on the results points to the correct report.

        ' Report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")


        If Not IsValidDate(dtpAsOfDate.Value) Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), "As Of date"), MsgBoxStyle.Critical, Me.Text)
            dtpAsOfDate.Focus()
            Exit Sub
        End If

        If cboStore.SelectedIndex = -1 Then
            MsgBox("Store must be selected.", MsgBoxStyle.Exclamation, "Invalid Store")
            cboStore.Focus()
            Exit Sub
        End If

        If cboSubTeam.SelectedIndex = -1 Then
            MsgBox("Subteam must be selected.", MsgBoxStyle.Exclamation, "Invalid Subteam")
            cboSubTeam.Focus()
            Exit Sub
        End If

        If cboStore.Text = "ALL" Or cboStore.Text = "" Then
            sReportURL.Append("&Store_No:isnull=true")
        Else
            sReportURL.Append("&Store_No=" & VB6.GetItemData(cboStore, cboStore.SelectedIndex))
        End If


        If cboSubTeam.Text = "ALL" Or cboSubTeam.Text = "" Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & VB6.GetItemData(cboSubTeam, cboSubTeam.SelectedIndex))
        End If

        sReportURL.Append("&AsOfDate=" & dtpAsOfDate.Value.ToShortDateString)

        Call ReportingServicesReport(sReportURL.ToString)

  End Sub


   

    Private Sub dtpAsOfDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
End Class