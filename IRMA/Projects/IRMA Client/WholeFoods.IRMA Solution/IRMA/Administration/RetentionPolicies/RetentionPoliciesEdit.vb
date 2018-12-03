Option Strict Off
Option Explicit On
Imports WholeFoods.Utility.DataAccess

Friend Class frmRetentionPolicies
  Inherits System.Windows.Forms.Form
  Implements IDisposable

  Private action As ActionName
  Private bUpdated As Boolean = False
  Private mbChanged As Boolean = False
  Private mbLoading As Boolean = True
  Private policyDataRow As DataRow
  Private dataSource As DataTable
  Private factory As DataFactory

  Const VALIDATION_HEADER As String = "Cannot save retention policy!"

  Public ReadOnly Property IsUpdated() As Boolean
    Get
      Return bUpdated
    End Get
  End Property

  Enum ActionName
    Add
    Update
  End Enum

  Public Sub New(ByRef sourceDataTable As DataTable, ByVal retentionID As Integer)
    If sourceDataTable Is Nothing Then Throw New ArgumentNullException("Required parameter <sourceDataTable> is missing.")

    dataSource = sourceDataTable
    Dim result As DataRow() = dataSource.Select(String.Format("RetentionPolicyId = {0}", retentionID.ToString()))

    action = IIf(result.Length = 0, ActionName.Add, ActionName.Update)

    Select Case action
      Case ActionName.Add : policyDataRow = dataSource.NewRow
      Case ActionName.Update : policyDataRow = result(0)
      Case Else : Throw New Exception("Unsupported action")
    End Select

    factory = New DataFactory(DataFactory.ItemCatalog)
    InitializeComponent()
  End Sub

  Private Sub form_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
    mbLoading = True

    If action = ActionName.Add Then
      cboJobName.SelectedIndex = 0
    Else
      LoadJobs()
      cboJobName.Text = policyDataRow!PurgeJobName
    End If

    txtSchema.Text = IIf(IsDBNull(policyDataRow!Schema), "dbo", policyDataRow!Schema)
    txtTable.Text = IIf(IsDBNull(policyDataRow!Table), String.Empty, policyDataRow!Table)
    If Not IsDBNull(policyDataRow!DaysToKeep) Then NumericUpDownDaysToKeep.Value = CInt(policyDataRow!DaysToKeep)
    If Not IsDBNull(policyDataRow!TimeToStart) Then NumericUpDownTimeToStart.Value = CInt(policyDataRow!TimeToStart)
    If Not IsDBNull(policyDataRow!TimeToEnd) Then NumericUpDownTimeToEnd.Value = CInt(policyDataRow!TimeToEnd)
    If Not IsDBNull(policyDataRow!IncludedInDailyPurge) Then checkIncludedInDailyPurge.Checked = CBool(policyDataRow!IncludedInDailyPurge)
    If Not IsDBNull(policyDataRow!DailyPurgeCompleted) Then checkDailyPurgeCompleted.Checked = CBool(policyDataRow!DailyPurgeCompleted)
    If Not IsDBNull(policyDataRow!LastPurgedDateTime) Then lblLastPurgedDate.Text = policyDataRow!LastPurgedDateTime.ToString()

    LoadColumns()
    If IsDBNull(policyDataRow!ReferenceColumn) Then
      cboColumn.SelectedIndex = -1
    Else
      cboColumn.Text = policyDataRow!ReferenceColumn
    End If

    cboJobName.Enabled = (action = ActionName.Update)
    txtSchema.Enabled = (action = ActionName.Add)
    txtTable.Enabled = (action = ActionName.Add)

    mbLoading = False
  End Sub

  Public Overloads Sub Dispose() Implements IDisposable.Dispose
    factory = Nothing
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub

  Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
    If mbChanged Then
      Select Case MsgBox("Save changes before closing?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Data changed!")
        Case MsgBoxResult.Yes : If Not Save() Then Exit Sub
        Case MsgBoxResult.Cancel : Exit Sub
      End Select
    End If

    Close()
  End Sub

  Private Sub LoadJobs()
    Const PURGE_JOB_NAME As String = "PurgeJobName"
    cboJobName.DataSource = IIf(dataSource Is Nothing, Nothing, dataSource.DefaultView.ToTable(True, PURGE_JOB_NAME))
    cboJobName.DisplayMember = PURGE_JOB_NAME
  End Sub

  Private Sub LoadColumns()
    cboColumn.Items.Clear()
    LoadColumnsForTable(cboColumn, txtSchema.Text, txtTable.Text)
  End Sub

  Private Sub cmdApply_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdApply.Click
    If Save() Then Close()
  End Sub

  Private Function Save() As Boolean
    If Not mbChanged OrElse Not ValidateData() Then Return False

    ' no need to check for duplicate, we will let them enter duplicate records
    ' if they want to enter from 22-2 then they can enter from 22-24 and 0-2
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

    Dim paramList As New DBParamList From
    {
      New DBParam("@RetentionPolicyId", DBParamType.Int, policyDataRow!RetentionPolicyId),
      New DBParam("@Schema", DBParamType.String, IIf(IsDBNull(policyDataRow!Schema), txtSchema.Text, policyDataRow!Schema)),
      New DBParam("@Table", DBParamType.String, IIf(IsDBNull(policyDataRow!Table), txtTable.Text, policyDataRow!Table)),
      New DBParam("@PurgeJobName", DBParamType.String, cboJobName.Text),
      New DBParam("@ReferenceColumn", DBParamType.String, cboColumn.Text),
      New DBParam("@DaysToKeep", DBParamType.Int, NumericUpDownDaysToKeep.Value),
      New DBParam("@TimeToStart", DBParamType.Int, NumericUpDownTimeToStart.Value),
      New DBParam("@TimeToEnd", DBParamType.Int, NumericUpDownTimeToEnd.Value),
      New DBParam("@IncludedInDailyPurge", DBParamType.Bit, checkIncludedInDailyPurge.Checked),
      New DBParam("@DailyPurgeCompleted", DBParamType.Bit, checkDailyPurgeCompleted.Checked)
    }

    Try
      factory.GetStoredProcedureDataTable("dbo.UpdateRetentionPolicy", paramList)
      bUpdated = True
    Catch ex As Exception
      MessageBox.Show(ex.InnerException.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
      Return False
    End Try

    Changed(False)
    Return True
  End Function

  Private Function ValidateData(Optional ByRef bMsg As Boolean = True) As Boolean
    Dim warning As String = String.Empty

    If Trim(txtSchema.Text) = "" Then
      warning = "Please enter value in Schema"
      txtSchema.Focus()
    ElseIf Trim(txtTable.Text) = "" Then
      warning = "Please enter value in Table"
      txtTable.Focus()
    ElseIf cboColumn.SelectedIndex = -1 Then
      warning = "Please select Column"
      cboColumn.Focus()
    ElseIf NumericUpDownTimeToStart.Value > NumericUpDownTimeToEnd.Value Then
      warning = "Time To Start must be less than or equal to Time to End."
      NumericUpDownTimeToStart.Focus()
    ElseIf cboJobName.SelectedIndex = -1 Then
      warning = "Please select Job Name"
      cboJobName.Focus()
    End If

    If Not String.IsNullOrEmpty(warning) Then MsgBox(warning, MsgBoxStyle.Exclamation, VALIDATION_HEADER)
    Return String.IsNullOrEmpty(warning)
  End Function

  ' this method will enable the aply (save) button
  Private Sub Changed(Optional ByRef bChanged As Boolean = True)
    If mbLoading Then Exit Sub

    mbChanged = bChanged
    cmdApply.Enabled = mbChanged
  End Sub

#Region "CheckForInputchange"
  Private Sub txtBox_TextChanged(sender As Object, e As EventArgs) Handles txtTable.TextChanged, txtSchema.TextChanged
    Changed()
  End Sub

  Private Sub CheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles checkIncludedInDailyPurge.CheckedChanged, checkDailyPurgeCompleted.CheckedChanged
    Changed()
  End Sub

  Private Sub cbo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboJobName.SelectedIndexChanged, cboColumn.SelectedIndexChanged
    If mbLoading Then Return

    If sender Is cboColumn Then policyDataRow!ReferenceColumn = cboColumn.Text
    Changed()
  End Sub

  Private Sub txtBox_Leave(sender As Object, e As EventArgs) Handles txtSchema.Leave, txtTable.Leave
    If (Not String.IsNullOrWhiteSpace(txtSchema.Text) AndAlso Not String.IsNullOrWhiteSpace(txtTable.Text)) Then
      LoadColumns()
    End If
  End Sub

  Private Sub NumericUpDown_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDownTimeToStart.ValueChanged, NumericUpDownTimeToEnd.ValueChanged, NumericUpDownDaysToKeep.ValueChanged
    Changed()
  End Sub
#End Region
End Class