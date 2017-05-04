Imports WholeFoods.IRMA.Replenishment.AverageCostUpdate.DataAccess
Imports WholeFoods.IRMA.Replenishment.Jobs
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.Utility
Imports log4net

Public Class Form_AverageCostUpdateController


    ''' <summary>
    ''' Private Variables: Define the log4net logger for this class.
    ''' </summary>
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    ''' <summary>
    ''' Loading Form and populate Drop Downs
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_AverageCostUpdateController_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cmbStores.DataSource = AverageCostUpdateDAO.GetAllStores()
        cmbStores.DisplayMember = "Store_Name"
        cmbStores.ValueMember = "Store_No"
        cmbSubteam.DataSource = AverageCostUpdateDAO.GetAllSubteams()
        cmbSubteam.DisplayMember = "Subteam_Name"
        cmbSubteam.ValueMember = "Subteam_No"
        cmbSubteam.SelectedIndex = -1

        Dim RegionUseAvgCost As Boolean = CBool(ConfigurationServices.AppSettings("UseAvgCostforCostAndMargin"))

        If RegionUseAvgCost = False Then
            Label_MessageExecutionText.Visible = True
            Label_MessageExecutionText.Text = "The Average Cost Update is not available " & vbCrLf & "App Key UseAvgCostforCostAndMargin is set to zero "
            btnUpdate.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' The user selected the button to kick-off the process.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        Label_MessageExecutionText.Visible = False
        Try
            ' Disable the button while the process is running and update the status on the UI
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            ' Reset the status messages
            ResetUIStatus()

            ' Setup the job
            Dim averageCostJob As New AverageCostUpdateJob()
            ' Get Selected Values
            If Not cmbStores.SelectedIndex = -1 Then
                averageCostJob.Store = cmbStores.SelectedValue
            End If
            If Not cmbSubteam.SelectedIndex = -1 Then
                averageCostJob.Subteam = cmbSubteam.SelectedValue
            End If
            ' Run the Job
            
            averageCostJob.Main()

            Label_JobStatus.Visible = True
            ' The job finished executing - update the status and enable the button
            ' Display the job status to the user.
            Label_JobStatus.Text = ScheduledJobBO.GetJobCompletionStatusForUI(CType(averageCostJob, ScheduledJob))
        Catch e1 As Exception
            ' An error occurred during processing - display a message and enable the button
            logger.Error("Error during processing of the AP Upload job", e1)
            Label_JobStatus.Text = "Error during AP Upload process: " & e1.Message()
            Label_MessageExecutionText.Visible = True
            Label_MessageExecutionText.Text = e1.StackTrace()
        Finally
            btnUpdate.Enabled = True
            Windows.Forms.Cursor.Current = Cursors.Default
            Me.Refresh()
        End Try

    End Sub

    ''' <summary>
    ''' Close Form.
    ''' </summary>
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' Reset the values on the UI screen to let the user know the jos is now processing.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ResetUIStatus()
        logger.Debug("ResetUIStatus entry")
        btnUpdate.Enabled = False
        Label_JobStatus.Text = "AverageCostUpdate process is executing."
        Label_MessageExecutionText.Visible = False
        Me.Refresh()
        logger.Debug("ResetUIStatus exit")
    End Sub

    ''' <summary>
    ''' Allow Subteam DropDown to reset selection.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub cmbSubteam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubteam.KeyPress
        logger.Debug("cmbSubTeam_KeyPress Entry")

        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 8 Then
            cmbSubteam.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If

        logger.Debug("cmbSubTeam_KeyPress Exit")
    End Sub

End Class