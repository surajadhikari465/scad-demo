Option Strict Off
Option Explicit On

Imports WholeFoods.Utility.DataAccess

Friend Class frmRetentionPolicyList
  Inherits System.Windows.Forms.Form

  Private RetentionPolicyTable As DataTable
  Private Const StraightPugeJob As String = "StraightPurge"

  Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
    Me.Close()
  End Sub

  Private Sub frmRetentionPolicyList_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
    Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

    If KeyAscii = 13 Then 'Shift+Enter.
      ClearAndRefresh()
    End If

    eventArgs.KeyChar = Chr(KeyAscii)
    eventArgs.Handled = (KeyAscii = 0)
  End Sub

  Private Sub ClearAndRefresh() 'Clear Search Criteria
    cboTable.SelectedIndex = -1
    CheckBox_IncludedInDailyPurge.Checked = False
    RefreshGrid()
  End Sub

  Private Sub frmRetentionPolicyList_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
    ClearAndRefresh()
  End Sub

  Private Sub RefreshGrid()
    Try
      If RetentionPolicyTable Is Nothing Then
        cboTable.DataSource = Nothing
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        RetentionPolicyTable = factory.GetStoredProcedureDataTable("dbo.GetRetentionPoliciesByTableDailyPurge")

        If RetentionPolicyTable IsNot Nothing Then
          cboTable.DataSource = RetentionPolicyTable.DefaultView.ToTable(True, "Table")
          cboTable.DisplayMember = "Table"
          cboTable.SelectedIndex = -1
        End If
      End If

      RetentionPolicyTable.DefaultView.RowFilter = IIf(cboTable.SelectedIndex < 0, Nothing, String.Format("Table = '{0}' and IncludedInDailyPurge = {1}", cboTable.Text, IIf(CheckBox_IncludedInDailyPurge.Checked, "1", "0")))
      ugrdRetentionPolicy.DataSource = RetentionPolicyTable.DefaultView

    Catch ex As Exception
      RetentionPolicyTable = Nothing
      MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK)
    End Try

    If ugrdRetentionPolicy.Rows.Count > 0 Then ugrdRetentionPolicy.Rows(0).Selected = True

    SetButtons()
  End Sub

  Private Sub cboTable_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cboTable.KeyPress
    Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

    If KeyAscii = 8 Then
      cboTable.SelectedIndex = -1
    End If

    eventArgs.KeyChar = Chr(KeyAscii)
    eventArgs.Handled = (KeyAscii = 0)
  End Sub

  Private Sub SetButtons()
    cmdDelete.Enabled = ugrdRetentionPolicy.Selected.Rows.Count > 0
    cmdEdit.Enabled = ugrdRetentionPolicy.Selected.Rows.Count = 1
  End Sub

  Private Sub ugrdLocationList_CellChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles ugrdRetentionPolicy.CellChange
    SetButtons()
  End Sub

  Private Sub ugrdLocationList_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdRetentionPolicy.Click
    SetButtons()
  End Sub

  Private Sub ugrdLocationList_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdRetentionPolicy.DoubleClickRow
    If cmdEdit.Enabled Then cmdEdit.PerformClick()
  End Sub

  Private Sub cmdDeleteRetentionPolicy_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click
    Dim iDelCnt As Short
    Dim hasJobsOtherThanPurgejob As Boolean = False
    If ugrdRetentionPolicy.Selected.Rows.Count > 0 Then

      '-- Make sure they really want to delete.
      If MsgBox("Delete the selected Retention Policies?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Delete Retention Policies") = MsgBoxResult.No Then
        Exit Sub
      End If

      For iDelCnt = 0 To ugrdRetentionPolicy.Selected.Rows.Count - 1
        '  only jobs with purge name as StraightPurge can be deleted at this point
        If (ugrdRetentionPolicy.Selected.Rows(iDelCnt).Cells("PurgeJobName").Value.ToString <> StraightPugeJob) Then
          hasJobsOtherThanPurgejob = True

        Else
          '   SQLExecute("EXEC DeleteInventoryLocations " & ugrdLocationList.Columns(0).CellValue(vBook).ToString, DAO.RecordsetOptionEnum.dbSQLPassThrough)
          SQLExecute("EXEC DeleteRetentionPolicy " & ugrdRetentionPolicy.Selected.Rows(iDelCnt).Cells("RetentionPolicyId").Value.ToString, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End If
      Next iDelCnt
      ' IF any job with purge name other than StraightPurge has been marked for deletion show the message
      If (hasJobsOtherThanPurgejob And ugrdRetentionPolicy.Selected.Rows.Count > 1) Then
        MsgBox("Retention Policies with Purge Job Name other than ""StraightPurge"" cannot be deleted.", MsgBoxStyle.Exclamation, "Notice!")
      ElseIf (hasJobsOtherThanPurgejob And ugrdRetentionPolicy.Selected.Rows.Count = 1) Then
        MsgBox("Retention Policy with Purge Job Name other than ""StraightPurge"" cannot be deleted.", MsgBoxStyle.Exclamation, "Notice!")
      End If

      RefreshGrid()
    Else
      'Shouldn't happen, but just in case.
      MsgBox("You must first select a Retention Policy to delete.", MsgBoxStyle.Exclamation, "Notice!")
    End If
  End Sub

  Private Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
    RefreshGrid()
  End Sub

  Private Sub cmdReset_Click(sender As Object, e As EventArgs) Handles cmdReset.Click
    ClearAndRefresh()
  End Sub

  Private Sub cmdAddEdit_Click(sender As Object, e As EventArgs) Handles cmdEdit.Click, cmdAdd.Click
    If RetentionPolicyTable Is Nothing Then Return

    Dim id As Integer = -1

    If sender Is cmdEdit AndAlso ugrdRetentionPolicy.Selected.Rows.Count > 0 Then
      id = CInt(ugrdRetentionPolicy.Selected.Rows(0).Cells("RetentionPolicyId").Value)
    End If

    frmRetentionPolicies.Load_Form(RetentionPolicyTable, id)
    Dim bRefresh As Boolean = frmRetentionPolicies.IsUpdated
    frmRetentionPolicies.Dispose()

    If Not bRefresh Then Exit Sub 'No changes ha been made

    RetentionPolicyTable = Nothing
    RefreshGrid() 'Refresh grid when actual changes has been done.
  End Sub
End Class