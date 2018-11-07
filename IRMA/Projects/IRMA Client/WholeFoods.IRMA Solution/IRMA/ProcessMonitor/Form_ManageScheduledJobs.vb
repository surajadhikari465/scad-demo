Imports log4net
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.IRMA.Administration.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.Jobs
Imports System.DirectoryServices.AccountManagement
Imports System.Linq

Public Class Form_ManageScheduledJobs
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private currentSelectedRow As Integer
    Private IsRefresh As Boolean = False

    ''' <summary>
    ''' Loads the form, populating the data grid with the current db status for all jobs.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_ManageScheduledJobs_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        logger.Debug("Form_ManageScheduledJobs_Load entry")

        ' Populate the data
        PopulateJobStatusGrid()

        Dim username As String = gsUserName
        Dim principalContext As PrincipalContext = New PrincipalContext(ContextType.Domain)
        Dim userPrincipal As UserPrincipal = userPrincipal.FindByIdentity(principalContext, username)
        Dim groups As PrincipalSearchResult(Of Principal) = userPrincipal.GetAuthorizationGroups()

        ' Reset button only available to members of IRMA Applications.
        Me.Button_Reset.Visible = groups.Any(Function(g) g.Name = "IRMA Applications" Or g.Name = "IRMA Developers")

        Me.DataGridView_Jobs.ClearSelection()
        Me.Button_Reset.Enabled = False

        logger.Debug("Form_ManageScheduledJobs_Load exit")
    End Sub

    ''' <summary>
    ''' Queries the database to populate the data grid.  Also formats the display of the grid.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateJobStatusGrid()
        logger.Debug("PopulateJobStatusGrid entry")

        ' Read the job status data
        Dim jobList As ArrayList = JobStatusDAO.GetScheduledJobClassList()

        ' Bind the data to the grid
        DataGridView_Jobs.DataSource = jobList
        DataGridView_Jobs.MultiSelect = False

        ' Format the view
        ' Make sure at least one entry was returned before configuring the columns
        If (DataGridView_Jobs.Columns.Count > 0) Then
            DataGridView_Jobs.Columns("Status").Visible = False

            DataGridView_Jobs.Columns("Classname").DisplayIndex = 0
            DataGridView_Jobs.Columns("Classname").HeaderText = ResourcesAdministration.GetString("label_jobClassname")
            DataGridView_Jobs.Columns("Classname").ReadOnly = True

            DataGridView_Jobs.Columns("StatusDescription").DisplayIndex = 1
            DataGridView_Jobs.Columns("StatusDescription").HeaderText = ResourcesAdministration.GetString("label_jobStatus")
            DataGridView_Jobs.Columns("StatusDescription").ReadOnly = True

            DataGridView_Jobs.Columns("LastRun").DisplayIndex = 2
            DataGridView_Jobs.Columns("LastRun").HeaderText = ResourcesAdministration.GetString("label_jobLastRun")
            DataGridView_Jobs.Columns("LastRun").ReadOnly = True

            DataGridView_Jobs.Columns("ServerName").DisplayIndex = 3
            DataGridView_Jobs.Columns("ServerName").HeaderText = ResourcesAdministration.GetString("label_jobServer")
            DataGridView_Jobs.Columns("ServerName").ReadOnly = True

            DataGridView_Jobs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        End If

        Dim currentStatus As New JobStatusBO

        For Each row As DataGridViewRow In DataGridView_Jobs.Rows

            currentStatus.PopulateFromAdminDataGrid(row)

            Select Case currentStatus.Status
                Case DBJobStatus.Failed
                    row.DefaultCellStyle.BackColor = Color.MistyRose
                Case DBJobStatus.Running
                    row.DefaultCellStyle.BackColor = Color.LightGreen
                Case Else
                    row.DefaultCellStyle.BackColor = Color.White
            End Select

        Next

        logger.Debug("PopulateJobStatusGrid exit")
    End Sub

    ''' <summary>
    ''' This function reads the selected row from a data grid.
    ''' The row can be selected by highlighting the entire row or a single cell
    ''' within the row.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getSelectedRow(ByRef dataGrid As DataGridView) As DataGridViewRow

        ' Get the selected row
        Dim selectedRow As DataGridViewRow = Nothing

        If (dataGrid.SelectedRows.Count = 1) Then

            Dim rowEnum As IEnumerator = dataGrid.SelectedRows.GetEnumerator

            rowEnum.MoveNext()

            selectedRow = CType(rowEnum.Current, DataGridViewRow)

        ElseIf (dataGrid.SelectedCells.Count = 1) Then

            Dim cellEnum As IEnumerator = dataGrid.SelectedCells.GetEnumerator

            cellEnum.MoveNext()

            Dim selectedCell As DataGridViewCell = CType(cellEnum.Current, DataGridViewCell)

            selectedRow = selectedCell.OwningRow

        Else
            ' Error condition
            MessageBox.Show("A row must be selected to perform this action.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        Return selectedRow

    End Function

    ''' <summary>
    ''' Allows the user to reset a failed job so that it can be re-executed.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Reset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Reset.Click
        logger.Debug("Button_Reset_Click entry")

        ' Get the selected row
        Dim selectedRow As DataGridViewRow = getSelectedRow(Me.DataGridView_Jobs)
        Dim currentStatus As New JobStatusBO

        If selectedRow IsNot Nothing Then

            currentStatus.PopulateFromAdminDataGrid(selectedRow)

            ' Confirm reset.
            If MessageBox.Show(String.Format(ResourcesAdministration.GetString("msg_confirmJobReset"), currentStatus.Classname, currentStatus.StatusDescription), Me.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.OK Then

                ' Reset the status
                Dim saveStatus As Integer = JobStatusDAO.ResetFailedJob(currentStatus.Classname)

                If saveStatus <> 0 Then ' 0 is the VALID code

                    If ValidationDAO.IsErrorCode(saveStatus) Then

                        ' A validation error was encountered during the save.  
                        Dim validationCode As ValidationBO = ValidationDAO.GetValidationCodeDetails(saveStatus)

                        ' Currently there is only one known error code to be processed.
                        If saveStatus = 300 Then

                            ' Refresh the data grid - it must be stale because the DB status no longer matches the client status.
                            PopulateJobStatusGrid()

                            Me.Refresh()

                            MessageBox.Show(String.Format(ResourcesAdministration.GetString("msg_validation_wrongResetStatusInDB"), currentStatus.Classname), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)

                        Else

                            MessageBox.Show(String.Format(ResourcesAdministration.GetString("msg_unknownJobResetError"), vbCrLf, saveStatus, validationCode.ValidationCodeDesc), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)

                        End If

                    Else

                        ' Just a warning was returned, so the update succeeded - refresh the data in the grid
                        PopulateJobStatusGrid()

                        Me.Refresh()

                    End If

                Else

                    ' The update succeeded - refresh the data in the grid

                    PopulateJobStatusGrid()

                    Me.Refresh()

                End If

            End If

        End If

        logger.Debug("Button_Reset_Click exit")
    End Sub

    ''' <summary>
    ''' Allows the user to view the log history for a job.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_ErrorLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_ErrorLog.Click
        logger.Debug("Button_ErrorLog_Click entry")

        ' Get the selected row
        Dim selectedRow As DataGridViewRow = getSelectedRow(Me.DataGridView_Jobs)
        Dim currentStatus As New JobStatusBO

        If selectedRow IsNot Nothing Then

            currentStatus.PopulateFromAdminDataGrid(selectedRow)

            Dim form As Form_ViewScheduledJobErrors = New Form_ViewScheduledJobErrors()

            form.selectedJob = currentStatus
            form.ShowDialog(Me)
            form.Dispose()

        End If

        logger.Debug("Button_ErrorLog_Click exit")
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click

        Cursor = Cursors.WaitCursor

        Me.IsRefresh = True

        PopulateJobStatusGrid()

        If Me.currentSelectedRow >= 0 Then

            Dim currentStatus As New JobStatusBO

            currentStatus.PopulateFromAdminDataGrid(Me.DataGridView_Jobs.Rows(Me.currentSelectedRow))

            If currentStatus.Status = DBJobStatus.Running Or currentStatus.Status = DBJobStatus.Failed Then

                Me.Button_Reset.Enabled = True

            Else

                Me.Button_Reset.Enabled = False

            End If

        End If

        Me.DataGridView_Jobs.Rows(Me.currentSelectedRow).Selected = True

        Me.IsRefresh = False

        Cursor = Cursors.Default

    End Sub

    Private Sub DataGridView_Jobs_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView_Jobs.SelectionChanged

        If Me.IsRefresh Then Exit Sub

        If Me.DataGridView_Jobs.CurrentRow.Index >= 0 Then

            Dim currentStatus As New JobStatusBO

            currentStatus.PopulateFromAdminDataGrid(Me.DataGridView_Jobs.CurrentRow)

            If currentStatus.Status = DBJobStatus.Running Or currentStatus.Status = DBJobStatus.Failed Then

                Me.Button_Reset.Enabled = True

            Else

                Me.Button_Reset.Enabled = False

            End If

            Me.currentSelectedRow = Me.DataGridView_Jobs.CurrentRow.Index

        End If

    End Sub
End Class
