Option Strict Off
Option Explicit On
Imports WholeFoods.Utility.DataAccess

Friend Class frmRetentionPolicies
    Inherits System.Windows.Forms.Form

    Private retentionPoliciesID As Integer
    Private mbChanged As Boolean
    Private mbLoading As Boolean
    Private schema As String
    Private operation As String
    Private table As String
    Private referenceColumn As String
    Private mbUserUpdates As Boolean 'Indicates that the user can update (save) the form.
    Const Add As String = "Add"
    Const StraightPugeJob As String = "StraightPurge"
    Private IsInitializing As Boolean


    Private Sub frmRetentionPoliciesEdit_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Dim rsRetentionPolicy As DAO.Recordset = Nothing
        mbChanged = False
        mbLoading = True
        '-- Center form
        CenterForm(Me)

        '-- Load rentention policy data if retentionPoliciesID has value
        If retentionPoliciesID <> 0 Then
            Try
                rsRetentionPolicy = SQLOpenRecordSet("EXEC GetRetentionPolicyById " & retentionPoliciesID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                txtSchema.Text = Convert.ToString(rsRetentionPolicy.Fields("Schema").Value)
                txtTable.Text = Convert.ToString(rsRetentionPolicy.Fields("Table").Value)

                LoadColumns()
                ' set the controls based on data from database
                cboColumn.Text = Convert.ToString(rsRetentionPolicy.Fields("ReferenceColumn").Value)
                schema = rsRetentionPolicy.Fields("Schema").Value
                table = rsRetentionPolicy.Fields("Table").Value
                referenceColumn = rsRetentionPolicy.Fields("ReferenceColumn").Value
                NumericUpDownDaysToKeep.Value = rsRetentionPolicy.Fields("DaysToKeep").Value
                NumericUpDownTimeToStart.Value = rsRetentionPolicy.Fields("TimeToStart").Value
                NumericUpDownTimeToEnd.Value = rsRetentionPolicy.Fields("TimeToEnd").Value
                CheckBox_IncludedInDailyPurge.Checked = rsRetentionPolicy.Fields("IncludedInDailyPurge").Value
                CheckBox_DailyPurgeCompleted.Checked = rsRetentionPolicy.Fields("DailyPurgeCompleted").Value

                If (Not IsDBNull(rsRetentionPolicy.Fields("LastPurgedDateTime").Value)) Then
                    txtLastPurgedDateTime.Text = rsRetentionPolicy.Fields("LastPurgedDateTime").Value
                Else

                    txtLastPurgedDateTime.Text = String.Empty
                End If

                LoadJobs()
                cboJobName.Text = Convert.ToString(rsRetentionPolicy.Fields("PurgeJobName").Value)

            Finally
                If rsRetentionPolicy IsNot Nothing Then
                    rsRetentionPolicy.Close()
                    rsRetentionPolicy = Nothing
                End If
            End Try

        End If
        If (operation = Add) Then

            LoadJobs()
            cboJobName.Text = StraightPugeJob
            txtLastPurgedDateTime.Visible = False
            lblPurgedDateTime.Visible = False
            cboJobName.Enabled = False
        Else
            txtSchema.Enabled = False
            txtTable.Enabled = False
            txtLastPurgedDateTime.Enabled = False
        End If

        mbLoading = False
        mbUserUpdates = True
    End Sub
    ''' <summary>
    ''' Load_Form
    ''' </summary>
    ''' <param name="RetentionPoliciesIDPassed"></param>
    ''' <param name="SchemaPassed"></param>
    ''' <param name="TablePassed"></param>
    ''' <param name="ReferenceColumnPassed"></param>
    ''' <param name="OperationPassed"></param>
    Public Sub Load_Form(Optional ByRef RetentionPoliciesIDPassed As Integer = 0, Optional ByRef SchemaPassed As String = "", Optional ByRef TablePassed As String = "", Optional ByRef ReferenceColumnPassed As String = "", Optional ByVal OperationPassed As String = "Add")

        retentionPoliciesID = RetentionPoliciesIDPassed
        schema = SchemaPassed
        table = TablePassed
        referenceColumn = ReferenceColumnPassed
        operation = OperationPassed
        Me.ShowDialog()

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        Dim iAns As Short

        If mbUserUpdates Then
            If mbChanged Then
                iAns = MsgBox("Save changes before closing?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Data changed!")
                Select Case iAns
                    Case MsgBoxResult.Yes
                        If Not Save() Then Exit Sub
                    Case MsgBoxResult.Cancel
                        Exit Sub
                End Select
            End If
        End If

        Me.Close()

    End Sub
    Private Sub LoadJobs()

        LoadAllJobsFromRetentionPolicy(cboJobName)


    End Sub
    Private Sub LoadColumns()
        '' make sure table name and schema are valis, if valid then load columns for that table
        If ValidateTableName(txtSchema.Text, txtTable.Text) Then
            LoadColumnsForTable(cboColumn, txtSchema.Text, txtTable.Text)
        Else
            cboColumn.Items.Clear()
        End If

    End Sub
    Private Function ValidateTableName(ByVal SchemaPassed As String, ByVal TablePassed As String) As Boolean

        Return ValidateTableByTableNameSchema(txtSchema.Text, txtTable.Text)

    End Function
    Private Sub cboColumn_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cboColumn.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 8 Then
            cboColumn.SelectedIndex = -1
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cboColumn_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboColumn.SelectedIndexChanged

        If IsInitializing Then Exit Sub

        referenceColumn = VB6.GetItemData(cboColumn, cboColumn.SelectedIndex)

        Changed()

    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdApply.Click

        Save()

    End Sub

    Private Function Save() As Boolean

        Dim rsRetenionPolicy As DAO.Recordset = Nothing

        Save = False

        If mbChanged Then
            If Not ValidateTableName(txtSchema.Text, txtTable.Text) Then
                MsgBox("Please enter valid values for Schema and Table.", MsgBoxStyle.Exclamation, "Cannot save retention policy!")
                Exit Function
            End If
            If Not ValidateData() Then Exit Function
            ' no need to check for duplicate, we will let them enter duplicate records
            ' i fthey want to enter from 22-2 then they can enter from 22-24 and 0-2
            'If (operation = Add) Then
            '    Try


            '        '-- Check to see if the retention policy already exists. this will be called only in add operation
            '        rsRetenionPolicy = SQLOpenRecordSet("EXEC CheckForDuplicateRetentionPolicyRecord " & txtSchema.Text & ", " & txtTable.Text, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            '        If rsRetenionPolicy.Fields("Found").Value > 0 Then
            '            MsgBox("A retention policy already exists for this table.", MsgBoxStyle.Exclamation, "Cannot save retention policy!")

            '            txtSchema.Focus()
            '            Exit Function
            '        End If
            '    Finally
            '        If rsRetenionPolicy IsNot Nothing Then
            '            rsRetenionPolicy.Close()
            '            rsRetenionPolicy = Nothing
            '        End If
            '    End Try
            'End If
            '--Update the record.
            SQLOpenRS(rsRetenionPolicy, "EXEC UpdateRetentionPolicy " & retentionPoliciesID & ", " & txtSchema.Text & ", " & txtTable.Text & ", " & cboColumn.Text & ",'" & NumericUpDownDaysToKeep.Value & "', '" & NumericUpDownTimeToStart.Value & "', '" & NumericUpDownTimeToEnd.Value & "', '" & CheckBox_IncludedInDailyPurge.Checked & "', '" & CheckBox_DailyPurgeCompleted.Checked & "', '" & cboJobName.Text & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

                retentionPoliciesID = rsRetenionPolicy.Fields(0).Value

                Save = True

            Changed(False)

        End If

    End Function

    Private Function ValidateData(Optional ByRef bMsg As Boolean = True) As Boolean

        ValidateData = True
        Dim errorMessage As String = ""
        ' validate data for different controls
        If Trim(txtSchema.Text) = "" Then
            errorMessage = MsgBox("Please enter value in Schema", MsgBoxStyle.Exclamation, "Cannot save retention policy!")
            ValidateData = False
            txtSchema.Focus()
        ElseIf Trim(txtTable.Text) = "" Then
            ValidateData = False
            errorMessage = MsgBox("Please enter value in Table", MsgBoxStyle.Exclamation, "Cannot save retention policy!")
            txtTable.Focus()
        ElseIf cboColumn.SelectedIndex = -1 Then
            ValidateData = False
            errorMessage = MsgBox("Please select Column", MsgBoxStyle.Exclamation, "Cannot save retention policy!")
            cboColumn.Focus()
        ElseIf NumericUpDownDaysToKeep.Value <= 0 Then
            errorMessage = MsgBox("Please enter value in Days To Keep greater than 0", MsgBoxStyle.Exclamation, "Cannot save retention policy!")
            ValidateData = False
            NumericUpDownDaysToKeep.Focus()
        ElseIf NumericUpDownTimeToStart.Value > NumericUpDownTimeToEnd.Value Then
            errorMessage = MsgBox("Time To Start must be less than or equal to Time to End.", MsgBoxStyle.Exclamation, "Cannot save retention policy!")
            ValidateData = False
            NumericUpDownTimeToStart.Focus()

        ElseIf cboJobName.SelectedIndex = -1 Then
            ValidateData = False
            errorMessage = MsgBox("Please select Job Name", MsgBoxStyle.Exclamation, "Cannot save retention policy!")
            cboJobName.Focus()
        End If


    End Function
    ' this method will enable the aply (save) button
    Private Sub Changed(Optional ByRef bChanged As Boolean = True)

        If mbLoading Or Not mbUserUpdates Then Exit Sub

        If bChanged = True Then
            cmdApply.Enabled = True
            mbChanged = True
        Else
            cmdApply.Enabled = False
            mbChanged = False
        End If

    End Sub

    Private Sub frmRetentionPolicies_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        mbChanged = False
        mbLoading = False
        retentionPoliciesID = 0
        schema = ""
        table = ""
        referenceColumn = False
        mbUserUpdates = False

    End Sub
#Region "CheckForInputchange"
    Private Sub txtSchema_TextChanged(sender As Object, e As EventArgs) Handles txtSchema.TextChanged

        If IsInitializing Then Exit Sub

        Changed()
    End Sub

    Private Sub txtTable_TextChanged(sender As Object, e As EventArgs) Handles txtTable.TextChanged

        If IsInitializing Then Exit Sub

        Changed()
    End Sub

    Private Sub NumericUpDownDaysToKeep_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDownDaysToKeep.ValueChanged

        If IsInitializing Then Exit Sub

        Changed()
    End Sub
    ' eneable save change button if user makes any changes in any of the controls
    Private Sub dtStartDate_ValueChanged(sender As Object, e As EventArgs)

        If IsInitializing Then Exit Sub

        Changed()
    End Sub

    Private Sub dtEndDate_ValueChanged(sender As Object, e As EventArgs)

        If IsInitializing Then Exit Sub

        Changed()
    End Sub

    Private Sub CheckBox_IncludedInDailyPurge_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_IncludedInDailyPurge.CheckedChanged

        If IsInitializing Then Exit Sub

        Changed()
    End Sub

    Private Sub CheckBox_DailyPurgeCompleted_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_DailyPurgeCompleted.CheckedChanged

        If IsInitializing Then Exit Sub

        Changed()
    End Sub

    Private Sub cboJobName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboJobName.SelectedIndexChanged
        If IsInitializing Then Exit Sub
        Changed()
    End Sub

    Private Sub txtSchema_Leave(sender As Object, e As EventArgs) Handles txtSchema.Leave
        If (txtSchema.Text <> "" And txtTable.Text <> "") Then
            LoadColumns()
        End If
    End Sub

    Private Sub txtTable_Leave(sender As Object, e As EventArgs) Handles txtTable.Leave
        If (txtSchema.Text <> "" And txtTable.Text <> "") Then
            LoadColumns()
        End If
    End Sub

    Private Sub NumericUpDownTimeToStart_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDownTimeToStart.ValueChanged
        If IsInitializing Then Exit Sub

        Changed()
    End Sub

    Private Sub NumericUpDownTimeToEnd_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDownTimeToEnd.ValueChanged
        If IsInitializing Then Exit Sub

        Changed()
    End Sub

#End Region
End Class