Option Strict Off
Option Explicit On

Imports WholeFoods.Utility.DataAccess

Friend Class frmRetentionPolicyList
  Inherits System.Windows.Forms.Form
  Implements IDisposable

  Private RetentionPolicyTable As DataTable = Nothing
  Private Const StraightPugeJob As String = "StraightPurge"

  Private Sub form_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
    ClearAndRefresh()
  End Sub

  Public Overloads Sub Dispose() Implements IDisposable.Dispose
    RetentionPolicyTable = Nothing
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub

  Private Sub form_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
    Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

    If KeyAscii = 13 Then 'Shift+Enter.
      ClearAndRefresh()
    End If

    eventArgs.KeyChar = Chr(KeyAscii)
    eventArgs.Handled = (KeyAscii = 0)
  End Sub

  Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
    Me.Close()
  End Sub

  Private Sub ClearAndRefresh() 'Clear Search Criteria
    cboTable.SelectedIndex = -1
    CheckBox_IncludedInDailyPurge.Checked = False
    RefreshGrid()
  End Sub

  Private Sub RefreshGrid(Optional bForce As Boolean = False)
    Try
      If RetentionPolicyTable Is Nothing OrElse bForce Then
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
    If eventArgs.KeyChar = vbBack Then cboTable.SelectedIndex = -1
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
    Dim count As Short
    Dim hasJobsOtherThanPurgejob As Boolean = False

    If ugrdRetentionPolicy.Selected.Rows.Count > 0 Then
      '-- Make sure they really want to delete.
      If MsgBox("Delete the selected Retention Policies?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Delete Retention Policies") = MsgBoxResult.No Then
        Exit Sub
      End If

      For count = 0 To ugrdRetentionPolicy.Selected.Rows.Count - 1 'FYI: Only jobs with purge name as StraightPurge can be deleted at this point
        If String.Compare(ugrdRetentionPolicy.Selected.Rows(count).Cells("PurgeJobName").Value.ToString(), StraightPugeJob, True) <> 0 Then
          hasJobsOtherThanPurgejob = True
        Else
          SQLExecute("EXEC DeleteRetentionPolicy " & ugrdRetentionPolicy.Selected.Rows(count).Cells("RetentionPolicyId").Value.ToString, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End If
      Next count

      ' IF any job with purge name other than StraightPurge has been marked for deletion show the message
      If (hasJobsOtherThanPurgejob And ugrdRetentionPolicy.Selected.Rows.Count > 1) Then
        MsgBox("Retention Policies with Purge Job Name other than ""StraightPurge"" cannot be deleted.", MsgBoxStyle.Exclamation, "Notice!")
      ElseIf (hasJobsOtherThanPurgejob And ugrdRetentionPolicy.Selected.Rows.Count = 1) Then
        MsgBox("Retention Policy with Purge Job Name other than ""StraightPurge"" cannot be deleted.", MsgBoxStyle.Exclamation, "Notice!")
      End If

      RefreshGrid(True)
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
    Dim bRefresh As Boolean = False

    If sender Is cmdEdit AndAlso ugrdRetentionPolicy.Selected.Rows.Count > 0 Then
      id = CInt(ugrdRetentionPolicy.Selected.Rows(0).Cells("RetentionPolicyId").Value)
    End If

    Try
      Using form As frmRetentionPolicies = New frmRetentionPolicies(RetentionPolicyTable, id)
        form.ShowDialog()
        bRefresh = form.IsUpdated
      End Using
      If bRefresh Then RefreshGrid(True) 'Refresh grid when actual changes has been done.
    Catch ex As Exception
      MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Try
  End Sub
End Class