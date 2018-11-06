Imports System.Text
Imports WholeFoods.Utility
Friend Class frmGLSalesReport
    Inherits System.Windows.Forms.Form

    Private Sub cmbField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbField.KeyPress
        Dim KeyAscii As Short = CShort(Asc(eventArgs.KeyChar))
        Dim Index As Short = cmbField.GetIndex(CType(eventSender, ComboBox))

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
        Dim reportUrlBuilder As StringBuilder

        If dtpEndDate.Value < dtpStartDate.Value Then
            MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
            dtpEndDate.Focus()
            Exit Sub
        End If

        reportUrlBuilder = New StringBuilder()

        reportUrlBuilder.AppendFormat("{0}_GLSalesReport&rs:Command=Render&rc:Parameters=False&Begin_Date={1:d}&End_Date={2:d}", _
            ConfigurationServices.AppSettings("Region"), _
            dtpStartDate.Value, _
            dtpEndDate.Value)

        If cmbField(0).SelectedIndex > -1 Then
            reportUrlBuilder.AppendFormat("&Store_No={0}", VB6.GetItemData(cmbField(0), cmbField(0).SelectedIndex))
        End If

        ReportingServicesReport(reportUrlBuilder.ToString())
    End Sub

    Private Sub frmGLSalesReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        CenterForm(Me)

        LoadInventoryStore(cmbField(0))

        If glStore_Limit > 0 Then
            SetActive(cmbField(0), False)
            SetCombo(cmbField(0), glStore_Limit)
        Else
            cmbField(0).SelectedIndex = -1
        End If

        dtpStartDate.Value = DateAdd(DateInterval.Day, -1, SystemDateTime)
        dtpEndDate.Value = DateAdd(DateInterval.Day, -1, SystemDateTime)
    End Sub
End Class