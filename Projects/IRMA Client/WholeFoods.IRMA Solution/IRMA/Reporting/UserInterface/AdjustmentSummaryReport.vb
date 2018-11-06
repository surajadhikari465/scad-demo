Imports System.Text
Imports WholeFoods.Utility

Friend Class frmAdjustmentSummaryReport
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

        If cmbField(0).SelectedIndex = -1 Then
            MsgBox(ResourcesIRMA.GetString("SelectStore"), MsgBoxStyle.Exclamation, Me.Text)
            cmbField(0).Focus()
            Exit Sub
        End If

        reportUrlBuilder = New StringBuilder()

        With reportUrlBuilder
            .AppendFormat("AdjustmentSummaryReport")
            '.Append("&rs:Command=Render")       'optional
            .Append("&rc:Parameters=False")

            If cmbField(0).SelectedIndex > -1 Then
                .AppendFormat("&Store_No={0}", VB6.GetItemData(cmbField(0), cmbField(0).SelectedIndex))
            End If

            If cmbField(1).SelectedIndex > -1 Then
                .AppendFormat("&SubTeam_No={0}", VB6.GetItemData(cmbField(1), cmbField(1).SelectedIndex))
            Else
                .Append("&SubTeam_No:isnull=true")
            End If

            .AppendFormat("&BeginDate={0}", dtpStartDate.Value.ToString("yyyy-MM-dd"))
            .AppendFormat("&EndDate={0}", dtpEndDate.Value.ToString("yyyy-MM-dd"))

            Call ReportingServicesReport(.ToString())
        End With

    End Sub

    Private Sub frmAdjustmentSummaryReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Dim currentDate As Date = SystemDateTime()

        CenterForm(Me)

        LoadInventoryStore(cmbField(0))
        LoadAllSubTeams(cmbField(1))

        If glStore_Limit > 0 Then
            SetActive(cmbField(0), False)
            SetCombo(cmbField(0), glStore_Limit)
        Else
            cmbField(0).SelectedIndex = -1
        End If

        dtpStartDate.Value = DateAdd(DateInterval.Day, -1, currentDate)
        dtpEndDate.Value = DateAdd(DateInterval.Day, -1, currentDate)
    End Sub
End Class