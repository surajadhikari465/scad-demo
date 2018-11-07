Imports log4net
Imports System.Windows.Forms
Imports WholeFoods.IRMA.Replenishment.Jobs

Namespace WholeFoods.IRMA.Replenishment.Common.BusinessLogic
    Public Class JobStatusBO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Private _classname As String
        Private _status As DBJobStatus
        Private _lastRun As Date
        Private _serverName As String

        Public Property Classname() As String
            Get
                Return _classname
            End Get
            Set(ByVal value As String)
                _classname = value
            End Set
        End Property

        Public Property Status() As DBJobStatus
            Get
                Return _status
            End Get
            Set(ByVal value As DBJobStatus)
                _status = value
            End Set
        End Property

        Public ReadOnly Property StatusDescription() As String
            Get
                Return JobStatusBO.GetCurrentDBStatusDescription(_status)
            End Get
        End Property

        Public Property LastRun() As Date
            Get
                Return _lastRun
            End Get
            Set(ByVal value As Date)
                _lastRun = value
            End Set
        End Property

        Public Property ServerName() As String
            Get
                Return _serverName
            End Get
            Set(ByVal value As String)
                _serverName = value
            End Set
        End Property

        ''' <summary>
        ''' Translate the database job status from an enum value to a string that is meaningful in log and
        ''' error messages.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetCurrentDBStatusDescription(ByVal currentDBStatus As DBJobStatus) As String
            logger.Debug("GetCurrentDBStatusDescription entry")
            Dim statusDesc As String
            Select Case currentDBStatus
                Case DBJobStatus.Running
                    statusDesc = "Running"
                Case DBJobStatus.Complete
                    statusDesc = "Complete"
                Case DBJobStatus.Failed
                    statusDesc = "Failed"
                Case DBJobStatus.JobError
                    statusDesc = "**DB Communication Error**"
                Case DBJobStatus.Unitialized
                    statusDesc = "Job not in DB table"
                Case Else
                    logger.Warn("GetCurrentDBStatusDescription requested for an unhandled CASE;  Enum Value = " + currentDBStatus.ToString())
                    statusDesc = "DBJobStatus Enum Value = " + currentDBStatus.ToString
            End Select
            logger.Debug("GetCurrentDBStatusDescription exit")
            Return statusDesc
        End Function

        ''' <summary>
        ''' Translates the application job status from an enum value to a string that is meaningful in log and
        ''' error messages.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetCurrentAppStatusDescription(ByVal currentAppStatus As AppJobStatus) As String
            logger.Debug("GetCurrentAppStatusDescription entry")
            Dim statusDesc As String
            Select Case currentAppStatus
                Case AppJobStatus.NotRunning
                    statusDesc = "Not Running"
                Case AppJobStatus.OK
                    statusDesc = "OK"
                Case AppJobStatus.Processing
                    statusDesc = "Processing"
                Case Else
                    logger.Warn("GetCurrentAppStatusDescription requested for an unhandled CASE;  Enum Value = " + currentAppStatus.ToString())
                    statusDesc = "AppStatus Enum Value = " + currentAppStatus.ToString
            End Select
            logger.Debug("GetCurrentAppStatusDescription exit")
            Return statusDesc
        End Function

        ''' <summary>
        ''' Populates this BO with the values from a selected row on the Admin UI.
        ''' </summary>
        ''' <param name="selectedRow"></param>
        ''' <remarks></remarks>
        Public Sub PopulateFromAdminDataGrid(ByVal selectedRow As DataGridViewRow)
            logger.Debug("PopulateFromAdminDataGrid entry")
            _status = CType(selectedRow.Cells("Status").Value, DBJobStatus)
            _classname = CType(selectedRow.Cells("Classname").Value, String)
            _lastRun = CType(selectedRow.Cells("LastRun").Value, Date)
            _serverName = CType(selectedRow.Cells("ServerName").Value, String)
            logger.Debug("PopulateFromAdminDataGrid exit")
        End Sub
    End Class
End Namespace
