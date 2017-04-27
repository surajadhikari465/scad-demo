Imports log4net
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess

Public Class Form_ViewScheduledJobErrors
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    ''' <summary>
    ''' The selected job for viewing error messages.
    ''' </summary>
    ''' <remarks></remarks>
    Dim _selectedJob As New JobStatusBO

#Region "Property Accessors"
    Public Property selectedJob() As JobStatusBO
        Get
            Return _selectedJob
        End Get
        Set(ByVal value As JobStatusBO)
            _selectedJob = value
        End Set
    End Property
#End Region

    ''' <summary>
    ''' Loads the job error form with the error log details for the selected job.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_ViewScheduledJobErrors_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If _selectedJob Is Nothing Then
            Me.Close()
        End If

        Me.CenterToParent()
        Me.GroupBox_Log.Text = "Error Log for " + _selectedJob.Classname() + " Job"
        ' Populate the data
        PopulateJobErrorLogGrid()
    End Sub

    ''' <summary>
    ''' Queries the database to populate the data grid.  Also formats the display of the grid.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PopulateJobErrorLogGrid()
        logger.Debug("PopulateJobErrorLogGrid entry")
        ' Read the job status data
        Dim errorList As ArrayList = JobStatusDAO.GetScheduledJobErrorList(_selectedJob.Classname)

        ' Bind the data to the grid
        DataGridView_Errors.DataSource = errorList
        DataGridView_Errors.MultiSelect = False

        ' Format the view
        ' Make sure at least one entry was returned before configuring the columns
        If (DataGridView_Errors.Columns.Count > 0) Then
            DataGridView_Errors.Columns("Classname").Visible = False

            DataGridView_Errors.Columns("LastRun").DisplayIndex = 0
            DataGridView_Errors.Columns("LastRun").HeaderText = ResourcesAdministration.GetString("label_jobRunDate")
            DataGridView_Errors.Columns("LastRun").ReadOnly = True
            DataGridView_Errors.Columns("LastRun").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            DataGridView_Errors.Columns("ExceptionText").DisplayIndex = 1
            DataGridView_Errors.Columns("ExceptionText").HeaderText = ResourcesAdministration.GetString("label_jobException")
            DataGridView_Errors.Columns("ExceptionText").ReadOnly = True
            DataGridView_Errors.Columns("ExceptionText").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            DataGridView_Errors.Columns("ServerName").DisplayIndex = 2
            DataGridView_Errors.Columns("ServerName").HeaderText = ResourcesAdministration.GetString("label_jobServer")
            DataGridView_Errors.Columns("ServerName").ReadOnly = True
            DataGridView_Errors.Columns("ServerName").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            DataGridView_Errors.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        End If
        logger.Debug("PopulateJobErrorLogGrid exit")
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
            'MessageBox.Show("A row must be selected to perform this action.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        Return selectedRow
    End Function

    Private Sub cmdCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCopy.Click

        Dim selectedRow As DataGridViewRow = getSelectedRow(Me.DataGridView_Errors)

        If selectedRow IsNot Nothing Then
            Clipboard.SetText(selectedRow.Cells("ExceptionText").Value)
        End If

    End Sub

    Private Sub DataGridView_Errors_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView_Errors.RowEnter
        Me.DataGridView_Errors.Rows(e.RowIndex).ErrorText = Me.DataGridView_Errors.Rows(e.RowIndex).Cells("ExceptionText").Value
    End Sub
End Class