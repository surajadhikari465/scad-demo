Imports System.Text
Imports WholeFoods.Utility
Friend Class frmTransferVendingAcctReport
    Inherits System.Windows.Forms.Form

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        Me.Close()

    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        Dim reportUrlBuilder As StringBuilder

        If dtpEndDate.Value < dtpStartDate.Value Then
            MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
            dtpEndDate.Focus()
            Exit Sub
        End If

        reportUrlBuilder = New StringBuilder()

        reportUrlBuilder.AppendFormat("{0}_GLTransfer{1}sReport&rs:Command=Render&rc:Parameters=False&BeginDate={2:d}&EndDate={3:d}", _
            ConfigurationServices.AppSettings("Region"), _
            IIf(chkShowDetail.Checked, "Detail", ""), _
            dtpStartDate.Value, _
            dtpEndDate.Value)

        If cmbStore.SelectedIndex > -1 Then
            reportUrlBuilder.AppendFormat("&Store_No={0}&Store_Name={1}", VB6.GetItemData(cmbStore, cmbStore.SelectedIndex), cmbStore.Text)
        End If

        If cmbSubTeam.SelectedIndex > -1 Then
            reportUrlBuilder.AppendFormat("&SubTeam_No={0}", VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        End If

        ReportingServicesReport(reportUrlBuilder.ToString())
    End Sub

    Private Sub frmTransferVendingAcctReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        CenterForm(Me)

        dtpStartDate.Value = DateAdd(DateInterval.Day, -7, SystemDateTime)
        dtpEndDate.Value = DateAdd(DateInterval.Day, -1, SystemDateTime)

        LoadStores(cmbStore)
        LoadAllSubTeams(cmbSubTeam)

        If glStore_Limit > 0 Then
            SetActive(cmbStore, False)
            SetCombo(cmbStore, glStore_Limit)
        Else
            cmbStore.SelectedIndex = -1
        End If
    End Sub
End Class