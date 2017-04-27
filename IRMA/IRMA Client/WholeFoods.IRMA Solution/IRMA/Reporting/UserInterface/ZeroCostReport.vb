Imports WholeFoods.Utility
Friend Class frmZeroCostReport
    Inherits System.Windows.Forms.Form

    Private Sub cmbSubTeam_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubTeam.KeyPress
        Dim KeyAscii As Short = CShort(Asc(eventArgs.KeyChar))

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
        If cmbSubTeam.SelectedIndex = -1 Then
            MsgBox("SubTeam is required.", CType(MsgBoxStyle.Information + MsgBoxStyle.OkOnly, MsgBoxStyle), Me.Text)
            cmbSubTeam.Focus()
            Exit Sub
        End If

        If cmbStore.SelectedIndex = -1 Then
            MsgBox("Store is required.", CType(MsgBoxStyle.Information + MsgBoxStyle.OkOnly, MsgBoxStyle), Me.Text)
            cmbStore.Focus()
            Exit Sub
        End If

        ReportingServicesReport(String.Format("{0}_ZeroCostReport&rs:Command=Render&rc:Parameters=False&CostPrice=Cost&SubTeam_No={1}&Store_No={2}&Store_Name={3}", _
            ConfigurationServices.AppSettings("Region"), _
            VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex), _
            VB6.GetItemData(cmbStore, cmbStore.SelectedIndex), _
            cmbStore.Text))
    End Sub

    Private Sub frmZeroCostReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        CenterForm(Me)

        '-- Load the combos
        LoadAllSubTeams(cmbSubTeam)

        LoadInventoryStore(cmbStore)

    End Sub
End Class