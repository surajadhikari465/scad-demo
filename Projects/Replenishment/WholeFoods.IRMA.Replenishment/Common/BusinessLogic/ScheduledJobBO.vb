Imports log4net
Imports WholeFoods.IRMA.Replenishment.Jobs

Namespace WholeFoods.IRMA.Replenishment.Common.BusinessLogic
    Public Class ScheduledJobBO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        ''' <summary>
        ''' Translates the job status results for the jobs that implement the WholeFoods.IRMA.Replenishment.ScheduledJob
        ''' class to a UI message that is meaningful to the user.
        ''' </summary>
        ''' <param name="currentJob"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetJobCompletionStatusForUI(ByVal currentJob As ScheduledJob) As String
            logger.Debug("GetJobCompletionStatusForUI entry")
            Dim statusMsg As String
            Select Case currentJob.MyStatus
                Case AppJobStatus.NotRunning
                    ' Check the DB status to display the appropriate message to the user.
                    Select Case currentJob.CurrentDBStatus
                        Case DBJobStatus.Running
                            statusMsg = "This instance of the " + currentJob.Classname() + " job did not execute because it could not obtain the lock." + Environment.NewLine + _
                                        "The job is currently running on the machine named " + currentJob.ServerName() + ", and" + Environment.NewLine + _
                                        "it started running at " + Format(currentJob.LastRunDate(), "yyyy/MM/dd HH:mm:ss") + Environment.NewLine + _
                                        "You must wait until the job completes before starting a new instance of the job."
                        Case DBJobStatus.Complete
                            statusMsg = "This instance of the " + currentJob.Classname() + " process completed and the job lock has been released." + Environment.NewLine + Environment.NewLine + _
                                        "Job Success: " + currentJob.JobExecutionStatus.ToString() + Environment.NewLine + Environment.NewLine
                            If Not currentJob.JobExecutionMessage() Is Nothing Then
                                statusMsg = statusMsg + "Job Message: " + currentJob.JobExecutionMessage()
                            End If
                        Case DBJobStatus.Failed
                            statusMsg = "Error during " + currentJob.Classname() + " process:" + Environment.NewLine + _
                                        "The last run of the job FAILED on the machine named " + currentJob.ServerName() + " at " + Format(currentJob.LastRunDate(), "yyyy/MM/dd HH:mm:ss") + "." + Environment.NewLine + _
                                        "The database status must be reset before a new instance of the job can be started." + Environment.NewLine + Environment.NewLine + _
                                        "The error log and reset of the job status table can be managed using the Admin Client." + Environment.NewLine + _
                                        "Select the Scheduled Jobs > Manage Scheduled Jobs menu option."
                        Case DBJobStatus.JobError
                            statusMsg = "This instance of the " + currentJob.Classname() + " job did not execute because there was an error when communicating with the database to" + Environment.NewLine + _
                                        "determine the current state of the job and obtain a lock for execution."
                        Case Else
                            logger.Warn("GetJobCompletionStatusForUI requested for an unhandled CASE;  Application Status = " + JobStatusBO.GetCurrentAppStatusDescription(currentJob.MyStatus()) + ", DB Status = " + JobStatusBO.GetCurrentDBStatusDescription(currentJob.CurrentDBStatus()))
                            statusMsg = "Application Status: " + JobStatusBO.GetCurrentAppStatusDescription(currentJob.MyStatus()) + Environment.NewLine + _
                                        "Database Status: " + JobStatusBO.GetCurrentDBStatusDescription(currentJob.CurrentDBStatus())
                    End Select
                Case AppJobStatus.OK
                    ' This is unexpected after the job has returned.  The application has the DB lock
                    ' but it has not started processing yet.
                    statusMsg = "Unexpected job return status: " + Environment.NewLine + _
                                "This instance of the " + currentJob.Classname() + " job has obtained the lock to start executing, but it is not currently running."
                Case AppJobStatus.Processing
                    ' This is unexpected after the job has returned.  The application has the DB lock
                    ' and it is currently processing.
                    statusMsg = "Unexpected job return status: " + Environment.NewLine + _
                                "This instance of the " + currentJob.Classname() + " job has obtained the lock and it is still running."
                Case Else
                    logger.Warn("GetJobCompletionStatusForUI requested for an unhandled CASE;  Application Status = " + JobStatusBO.GetCurrentAppStatusDescription(currentJob.MyStatus()) + ", DB Status = " + JobStatusBO.GetCurrentDBStatusDescription(currentJob.CurrentDBStatus()))
                    statusMsg = "Application Status: " + JobStatusBO.GetCurrentAppStatusDescription(currentJob.MyStatus()) + Environment.NewLine + _
                                "Database Status: " + JobStatusBO.GetCurrentDBStatusDescription(currentJob.CurrentDBStatus())
            End Select
            logger.Debug("GetJobCompletionStatusForUI exit")
            Return statusMsg
        End Function

    End Class
End Namespace

