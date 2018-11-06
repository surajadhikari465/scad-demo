Imports log4net
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Jobs
Public Class Form_PeopleSoftUploadJobController
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' Define the job being processed by this class.
    Dim WithEvents uploadJob As PeopleSoftUploadJob

    Private Sub Form_PeopleSoftUploadJobController_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' TODO: Any way to check to see if job is already running?
    End Sub

    ''' <summary>
    ''' Reset the values on the UI screen to let the user know the jos is now processing.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ResetUIStatus()
        logger.Debug("ResetUIStatus entry")
        Button_StartJob.Enabled = False
        Label_JobStatus.Text = "AP Upload process is executing."
        Label_MessageExecutionText.Visible = False

        Me.Refresh()
        logger.Debug("ResetUIStatus exit")
    End Sub

    ''' <summary>
    ''' The user selected the button to kick-off the process.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_StartJob_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_StartJob.Click
        logger.Debug("Button_StartJob_Click entry")
        Try
            ' Disable the button while the process is running and update the status on the UI
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            ' Reset the status messages
            ResetUIStatus()

            ' Start the job
            uploadJob = New PeopleSoftUploadJob()
            uploadJob.Main()

            ' The job finished executing - update the status and enable the button
            ' Display the job status to the user.
            Label_JobStatus.Text = ScheduledJobBO.GetJobCompletionStatusForUI(CType(uploadJob, ScheduledJob))
        Catch e1 As Exception
            ' An error occurred during processing - display a message and enable the button
            logger.Error("Error during processing of the AP Upload job", e1)
            Label_JobStatus.Text = "Error during AP Upload process: " & e1.Message()
            Label_MessageExecutionText.Text = e1.StackTrace()
            Label_MessageExecutionText.Visible = True
        Finally
            Button_StartJob.Enabled = True
            Windows.Forms.Cursor.Current = Cursors.Default
            Me.Refresh()
        End Try
        logger.Debug("Button_StartJob_Click exit")
    End Sub



End Class