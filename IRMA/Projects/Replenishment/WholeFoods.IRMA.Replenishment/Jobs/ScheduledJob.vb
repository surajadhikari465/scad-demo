Imports log4net
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Replenishment.Jobs
    Public Enum AppJobStatus
        NotRunning  ' Initial status; the application is not currently executing the job and it does not have the db lock
        OK          ' The application has obtained the db lock for the job and it is safe to start Processing  
        Processing  ' The application is currently running the job
    End Enum

    Public Enum DBJobStatus
        Running     ' Job is currently running
        Complete    ' Job has completed successfully; no longer running 
        Failed      ' Job has failed
        JobError    ' If the application is unable to query the db, this is the error status to indicate that
        Unitialized ' This job does not yet exist in the database
        Queueing    ' Receiving is going to Queue table 
    End Enum


    Public MustInherit Class ScheduledJob
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        ''' <summary>
        ''' Current application status for this job
        ''' </summary>
        ''' <remarks></remarks>
        Private _myStatus As AppJobStatus = AppJobStatus.NotRunning

        ''' <summary>
        ''' Current database status for this job
        ''' </summary>
        ''' <remarks></remarks>
        Private _currentDBStatus As DBJobStatus = DBJobStatus.JobError

        ''' <summary>
        ''' Classname (without the namespace) for this job
        ''' </summary>
        ''' <remarks></remarks>
        Private _classname As String
        ''' <summary>
        ''' _pushAlerts
        ''' </summary>
        Private _stopAlerts As Boolean
        ''' <summary>
        ''' The name of the server for a job that is currently running, or the last server that the job was run on.
        ''' </summary>
        ''' <remarks></remarks>
        Private _serverName As String

        ''' <summary>
        ''' The date for the last status change for a job.
        ''' </summary>
        ''' <remarks></remarks>
        Private _lastRunDate As Date

        ''' <summary>
        ''' This is the a status set by the job.  There are jobs that might run to completion, 
        ''' so the db status will be set to completed.  However, there were errors encountered 
        ''' that the job handled.  If there is a UI tool that is running the job, it should notify 
        ''' the user of these errors.
        ''' </summary>
        ''' <remarks></remarks>
        Private _jobExecutionStatus As Boolean

        ''' <summary>
        ''' Message text that can be displayed to the user if there is a UI tool that is running 
        ''' the job.  This can include job log output, job error messages for errors that were encountered
        ''' but successfully handled by the job, etc.
        ''' </summary>
        ''' <remarks></remarks>
        Protected _jobExecutionMessage As String


#Region "Property Accessors"
        Public ReadOnly Property CurrentDBStatus() As DBJobStatus
            Get
                Return _currentDBStatus
            End Get
        End Property

        Public ReadOnly Property MyStatus() As AppJobStatus
            Get
                Return _myStatus
            End Get
        End Property

        Public ReadOnly Property ServerName() As String
            Get
                Return _serverName
            End Get
        End Property

        Public ReadOnly Property LastRunDate() As Date
            Get
                Return _lastRunDate
            End Get
        End Property

        Public ReadOnly Property Classname() As String
            Get
                Return _classname
            End Get
        End Property

        Public ReadOnly Property JobExecutionStatus() As Boolean
            Get
                Return _jobExecutionStatus
            End Get
        End Property

        Public ReadOnly Property JobExecutionMessage() As String
            Get
                Return _jobExecutionMessage
            End Get
        End Property
        Public Property StopAlerts() As Boolean
            Get
                Return _stopAlerts
            End Get
            Set(value As Boolean)
                _stopAlerts = value
            End Set
        End Property



#End Region

        ''' <summary>
        ''' The constructor reads the current entry from the JobStatus table for this class:
        '''      - no entry = create an entry with a status of RUNNING and set the application status to OK
        '''      - entry with status of COMPLETE = update the entry to a status of RUNNING and set the application status to OK
        '''      - entry with status of other than COMPLETE = set the application status to NO_RUN
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            logger.Debug("ScheduledJob constructor entry")
            _myStatus = AppJobStatus.NotRunning

            ' Get the classname without the package - this is the format in the JobStatus table
            _classname = Me.GetType.Name()

            Try
                ' get the current status entry for this job 
                RefreshJobStatus()
            Catch ex As Exception
                logger.Error("ScheduledJob constructor exception when processing _classname=" + _classname, ex)
                _currentDBStatus = DBJobStatus.JobError
                '   sendErrorEmail(e);
            End Try
            logger.Debug("ScheduledJob constructor exit")
        End Sub

        ''' <summary>
        ''' This method is called to refresh the job status.  It allows the calling object
        ''' to see if it is now safe to start a job that would have been previously blocked because
        ''' another instance of the job was already running.
        ''' If it is safe, this process will obtain the database "running" lock.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RefreshJobStatus()
            logger.Debug("RefreshJobStatus entry")
            If (_myStatus = AppJobStatus.Processing) Then
                ' do nothing.  this job is currently running.
            Else
                ' get the current JobStatus entry for this job from the DB
                _currentDBStatus = GetJobStatus()
                If (_currentDBStatus <> DBJobStatus.Unitialized) Then
                    ' we have an entry
                    If (_currentDBStatus = DBJobStatus.Complete) Then
                        ' it's safe to run - so update the JobStatus to running to obtain the DB lock
                        UpdateJobStatus(DBJobStatus.Running)
                        ' set the application status to allow this job to start processing
                        _myStatus = AppJobStatus.OK
                    Else
                        ' it's already running or failed - let's stop
                        _myStatus = AppJobStatus.NotRunning
                    End If
                Else
                    ' we don't have a JobStatus entry - create one and initialize the status to running to obtain the DB lock
                    CreateJobStatus(DBJobStatus.Running)
                    ' set the application status
                    _myStatus = AppJobStatus.OK
                End If
            End If
            logger.Debug("RefreshJobStatus exit")
        End Sub

        ''' <summary>
        ''' This method actually executes the job.  It should be called by any subclass
        ''' performing work to keep the values up to date in the JobStatus table.
        ''' This method calls the runProcess method, which is implemented by the subclass, 
        ''' to actually execute the job logic.
        ''' </summary>
        ''' <returns>The application will return TRUE if the Main job is able to execute.  The
        ''' AppJobStatus and DBJobStatus properties should be examined to see if errors were
        ''' encountered.</returns>
        ''' <remarks></remarks>
        Public Function Main() As Boolean
            'logger.Debug("Main entry")
            ' check the job status to make sure it is safe to run the job
            If (_myStatus = AppJobStatus.OK) Then
                ' this application has the DB lock on the job - run it
                Try
                    _myStatus = AppJobStatus.Processing
                    _jobExecutionStatus = Me.RunProcess()
                    
                    ' after job runs, set status=complete to release the DB lock
                    Try
                        _myStatus = AppJobStatus.NotRunning  ' set to no run so the application can't try to run the same object again without getting the db lock
                        UpdateJobStatus(DBJobStatus.Complete)
                    Catch ignore As Exception
                        logger.Error("Exception when setting the JobStatus table to complete:", ignore)
                    End Try
                Catch e As Exception
                    ' there was an error during the processing of the job
                    ' log the exception
                    _myStatus = AppJobStatus.NotRunning  ' set to no run so the application can't try to run the same object again without getting the db lock
                    logger.Error("Exception caught during the execution of the runProcess method for classname=" + Me.Classname(), e)

                    ' log the error in the JobErrorLog table
                    Try
                        CreateJobError(e.ToString)
                    Catch ignore As Exception
                        logger.Error("Exception when inserting a record into the JobErrorLog table for classname=" + Me.Classname() + ":", ignore)
                    End Try

                    ' ... and set JobStatus status=failed
                    Try
                        UpdateJobStatus(DBJobStatus.Failed)
                    Catch ignore As Exception
                        logger.Error("Exception when setting the JobStatus table to failed for classname=" + Me.Classname() + ":", ignore)
                    End Try

                    ' ... and send out an email notification of the failure 
                    Dim args(1) As String
                    args(0) = Classname
                    ErrorHandler.ProcessError(ErrorType.ScheduledJob_ProcessingError, args, SeverityLevel.Fatal, e)

                End Try
            Else
                ' unable to run the job - current JobStatus entry for the class is not complete
                Dim msg As String = "The job " + Me.Classname + " did NOT run at " + Now.ToShortTimeString + " with a JobStatus.Status value = " + JobStatusBO.GetCurrentDBStatusDescription(_currentDBStatus)
                logger.Warn(msg)

                ' send out an email warning that the class was not run 
                Dim args(2) As String
                args(0) = Classname
                args(1) = JobStatusBO.GetCurrentDBStatusDescription(_currentDBStatus)
                ErrorHandler.ProcessError(ErrorType.ScheduledJob_DidNotRun, args, SeverityLevel.Warning)

                If (Not StopAlerts And (Me.Classname = "POSPushJob" Or Me.Classname = "ScalePushJob" Or Me.Classname = "PeopleSoftUploadJob" Or Me.Classname = "PeopleSoftTransferUploadJob")) Then
                    OpsGenieUtility.SendMail( Me.Classname + " Failure", msg)
                End If

            End If
            'logger.Debug("Main exit")
            Return True
        End Function

        ''' <summary>
        ''' This is the function that actually contains the job logic.  It must be implemented by the sub-class.
        ''' </summary>
        ''' <returns>TRUE if the job completed successfully;  FALSE if there were errors encountered during processing
        ''' that were handled by the job but should still be reported to the user</returns>
        ''' <remarks></remarks>
        Protected MustOverride Function RunProcess() As Boolean

        ''' <summary>
        ''' Read the current JobStatus value for this class from the database.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function GetJobStatus() As DBJobStatus
            Dim jobStatus As JobStatusBO = JobStatusDAO.GetJobStatus(Classname)
            If Not jobStatus Is Nothing Then
                ' Set the other properties from the status BO in case they are needed later
                _serverName = jobStatus.ServerName()
                _lastRunDate = jobStatus.LastRun()
            Else
                ' This job does not yet exist in the database
                jobStatus = New JobStatusBO()
                jobStatus.Status = DBJobStatus.Unitialized
            End If
            Return jobStatus.Status()
        End Function


        ''' <summary>
        ''' Create the status record in the JobStatus table for this class.
        ''' </summary>
        ''' <param name="setStatus"></param>
        ''' <remarks></remarks>
        Protected Sub CreateJobStatus(ByVal setStatus As DBJobStatus)
            ' Update the DB status
            JobStatusDAO.InsertJobStatus(Classname, setStatus)
            ' Keep the local app vars in sync with the DB
            _currentDBStatus = setStatus
            _lastRunDate = Now
        End Sub

        ''' <summary>
        ''' Update the status value in the JobStatus table for this class.
        ''' </summary>
        ''' <param name="setStatus"></param>
        ''' <remarks></remarks>
        Protected Sub UpdateJobStatus(ByVal setStatus As DBJobStatus)
            ' Update the DB status
            JobStatusDAO.UpdateJobStatus(Classname, setStatus)
            ' Keep the local app vars in sync with the DB
            _currentDBStatus = setStatus
            _lastRunDate = Now
        End Sub

         ''' <summary>
        ''' Create a record in the JobErrorLog table when an exception is caught during execution.
        ''' </summary>
        ''' <param name="exceptionMsg"></param>
        ''' <remarks></remarks>
        Protected Sub CreateJobError(ByVal exceptionMsg As String)
            JobStatusDAO.InsertJobError(classname, exceptionMsg)
        End Sub


    End Class
End Namespace

