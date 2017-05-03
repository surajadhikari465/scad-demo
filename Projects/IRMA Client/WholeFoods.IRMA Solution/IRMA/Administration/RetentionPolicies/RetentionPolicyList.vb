Option Strict Off
Option Explicit On
Friend Class frmRetentionPolicyList
    Inherits System.Windows.Forms.Form

    Private RetentionPolicyTable As DataTable
    Private Const StraightPugeJob As String = "StraightPurge"
    Private Const SelectOne As String = "Select One"
    Private IsInitializing As Boolean
    Private Sub cmdAddRetentionPolicy_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAddRetentionPolicy.Click

        frmRetentionPolicies.Load_Form()

        frmRetentionPolicies.Dispose()

        RefreshGrid()

    End Sub


    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub
    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)

        RefreshGrid()

    End Sub
    Private Sub frmRetentionPolicyList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DoubleClick
        Dim s As String
        s = ugrdRetentionPolicy.Selected.Rows(0).Cells("RetentionPolicyId").Value
        MsgBox(s)
    End Sub

    Private Sub frmRetentionPolicyList_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 13 Then 'Shift+Enter.
            ClearAndRefresh()
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub
    Private Sub ClearAndRefresh()
        '-- Clear Search Criteria
        If cboTable.Enabled Then cboTable.SelectedIndex = -1
        CheckBox_IncludedInDailyPurge.Checked = False
        RefreshGrid()

    End Sub

    Private Sub frmRetentionPolicyList_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        '-- Center form
        CenterForm(Me)

        LoadTable()
        SetupDataTable()
        RefreshGrid()

    End Sub

    Private Sub LoadTable()

        LoadTableswithRetentionPolicy(cboTable)

    End Sub

    Private Sub RefreshGrid()

        Dim selectedTable As String = Nothing
        Dim includedInDailyPurge As String = Nothing

        'new grid
        Dim rowRetentionPolicyList As DAO.Recordset = Nothing
        Dim row As DataRow
        selectedTable = "Null"
        If cboTable.SelectedIndex > -1 Then
            selectedTable = cboTable.Text
        Else
            LoadTable()
        End If


        If CheckBox_IncludedInDailyPurge.Checked Then
            includedInDailyPurge = "True"
        Else
            includedInDailyPurge = "Null"
        End If

        Try

            rowRetentionPolicyList = SQLOpenRecordSet("EXEC GetRetentionPoliciesByTableDailyPurge " & selectedTable & ", " & includedInDailyPurge, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            RetentionPolicyTable.Rows.Clear()

            'loads new grid
            While Not rowRetentionPolicyList.EOF

                row = RetentionPolicyTable.NewRow
                row("RetentionPolicyId") = rowRetentionPolicyList.Fields("RetentionPolicyId").Value
                row("Schema") = rowRetentionPolicyList.Fields("Schema").Value
                row("Table") = rowRetentionPolicyList.Fields("Table").Value
                row("ReferenceColumn") = rowRetentionPolicyList.Fields("ReferenceColumn").Value
                row("DaysToKeep") = rowRetentionPolicyList.Fields("DaysToKeep").Value
                row("TimeToStart") = rowRetentionPolicyList.Fields("TimeToStart").Value
                row("TimeToEnd") = rowRetentionPolicyList.Fields("TimeToEnd").Value
                row("IncludedInDailyPurge") = rowRetentionPolicyList.Fields("IncludedInDailyPurge").Value
                row("DailyPurgeCompleted") = rowRetentionPolicyList.Fields("DailyPurgeCompleted").Value
                row("PurgeJobName") = rowRetentionPolicyList.Fields("PurgeJobName").Value
                row("LastPurgedDateTime") = rowRetentionPolicyList.Fields("LastPurgedDateTime").Value


                RetentionPolicyTable.Rows.Add(row)

                rowRetentionPolicyList.MoveNext()
            End While

            RetentionPolicyTable.AcceptChanges()
            ugrdRetentionPolicy.DataSource = RetentionPolicyTable
            'close down rs for new grid
        Finally
            If rowRetentionPolicyList IsNot Nothing Then
                rowRetentionPolicyList.Close()
                rowRetentionPolicyList = Nothing
            End If
        End Try

        If ugrdRetentionPolicy.Rows.Count > 0 Then
            ugrdRetentionPolicy.Rows(0).Selected = True
        End If

        SetButtons()

    End Sub
    Private Sub cboTable_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cboTable.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 8 Then
            cboTable.SelectedIndex = -1
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub
    Private Sub gridLoc_DblClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)

        If cmdEditRetentionPolicy.Enabled Then
            cmdEditRetentionPolicy.PerformClick()
        End If

    End Sub
    Private Sub SetButtons()

        If ugrdRetentionPolicy.Selected.Rows.Count > 0 Then
            Me.cmdDeleteRetentionPolicy.Enabled = True
        Else
            Me.cmdDeleteRetentionPolicy.Enabled = False
        End If

        If ugrdRetentionPolicy.Selected.Rows.Count = 1 Then
            Me.cmdEditRetentionPolicy.Enabled = True
        Else

            Me.cmdEditRetentionPolicy.Enabled = False
        End If

    End Sub
    Private Sub SetupDataTable()
        RetentionPolicyTable = New DataTable("RetentionPolicy")
        'Hidden.
        '--------------------
        RetentionPolicyTable.Columns.Add(New DataColumn("RetentionPolicyId", GetType(Integer)))
        'Visible.
        '--------------------
        RetentionPolicyTable.Columns.Add(New DataColumn("Schema", GetType(String)))
        RetentionPolicyTable.Columns.Add(New DataColumn("Table", GetType(String)))
        RetentionPolicyTable.Columns.Add(New DataColumn("ReferenceColumn", GetType(String)))
        RetentionPolicyTable.Columns.Add(New DataColumn("DaysToKeep", GetType(Integer)))
        RetentionPolicyTable.Columns.Add(New DataColumn("TimeToStart", GetType(Integer)))
        RetentionPolicyTable.Columns.Add(New DataColumn("TimeToEnd", GetType(Integer)))
        RetentionPolicyTable.Columns.Add(New DataColumn("IncludedInDailyPurge", GetType(Boolean)))
        RetentionPolicyTable.Columns.Add(New DataColumn("DailyPurgeCompleted", GetType(Boolean)))
        RetentionPolicyTable.Columns.Add(New DataColumn("PurgeJobName", GetType(String)))
        RetentionPolicyTable.Columns.Add(New DataColumn("LastPurgedDateTime", GetType(String)))

    End Sub
    Private Sub ugrdLocationList_CellChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles ugrdRetentionPolicy.CellChange
        SetButtons()
    End Sub

    Private Sub ugrdLocationList_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdRetentionPolicy.Click
        SetButtons()
    End Sub

    Private Sub ugrdLocationList_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdRetentionPolicy.DoubleClickRow

        If cmdEditRetentionPolicy.Enabled Then
            cmdEditRetentionPolicy.PerformClick()
        End If

    End Sub
    Private Sub cmdDeleteRetentionPolicy_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDeleteRetentionPolicy.Click

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
            '-- Refresh the grid
            RefreshGrid()

        Else
            'Shouldn't happen, but just in case.
            MsgBox("You must first select a Retention Policy to delete.", MsgBoxStyle.Exclamation, "Notice!")

        End If

    End Sub
    Private Sub cmdSearch_Click_1(sender As Object, e As EventArgs) Handles cmdSearch.Click
        RefreshGrid()
    End Sub

    Private Sub cmdReset_Click(sender As Object, e As EventArgs) Handles cmdReset.Click
        ClearAndRefresh()
    End Sub

    Private Sub cmdEditRetentionPolicy_Click(sender As Object, e As EventArgs) Handles cmdEditRetentionPolicy.Click
        If ugrdRetentionPolicy.Selected.Rows.Count = 1 Then

            '-- Show the edit screen
            frmRetentionPolicies.Load_Form((ugrdRetentionPolicy.Selected.Rows(0).Cells("RetentionPolicyId").Value), (ugrdRetentionPolicy.Selected.Rows(0).Cells("Schema").Value), (ugrdRetentionPolicy.Selected.Rows(0).Cells("Table").Value), (ugrdRetentionPolicy.Selected.Rows(0).Cells("ReferenceColumn").Value), "Update")
            frmRetentionPolicies.Dispose()
            RefreshGrid()

        Else
            MsgBox("Please select a line to edit.", MsgBoxStyle.Exclamation, "Notice!")

        End If

    End Sub
End Class