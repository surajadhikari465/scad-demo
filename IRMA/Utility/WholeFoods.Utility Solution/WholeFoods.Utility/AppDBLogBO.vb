Imports log4net
Imports System.Configuration
Imports system.Data.SqlClient
Namespace WholeFoods.Utility
    Public Class AppDBLogBO

        Private _appName As String
        Private _envShortName As String
        Private _logEntryId As Integer
        Private _logDate As Date
        Private _appGUID As String
        Private _hostName As String
        Private _userName As String
        Private _thread As String
        Private _level As String
        Private _logger As String
        Private _message As String
        Private _exception As String
        Private _insertDate As Date

        ' Define the log4net logger for this class.  (This is for instances of this class, not shared references.)
        Dim ilogger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Properties"
        Public Property appName() As String
            Get
                Return _appName
            End Get
            Set(ByVal value As String)
                _appName = value
            End Set
        End Property
        Public Property envShortName() As String
            Get
                Return _envShortName
            End Get
            Set(ByVal value As String)
                _envShortName = value
            End Set
        End Property
        Public Property logEntryId() As Integer
            Get
                Return _logEntryId
            End Get
            Set(ByVal value As Integer)
                _logEntryId = value
            End Set
        End Property
        Public Property logDate() As Date
            Get
                Return _logDate
            End Get
            Set(ByVal value As Date)
                _logDate = value
            End Set
        End Property
        Public Property appGUID() As String
            Get
                Return _appGUID
            End Get
            Set(ByVal value As String)
                _appGUID = value
            End Set
        End Property
        Public Property hostName() As String
            Get
                Return _hostName
            End Get
            Set(ByVal value As String)
                _hostName = value
            End Set
        End Property
        Public Property userName() As String
            Get
                Return _userName
            End Get
            Set(ByVal value As String)
                _userName = value
            End Set
        End Property
        Public Property thread() As String
            Get
                Return _thread
            End Get
            Set(ByVal value As String)
                _thread = value
            End Set
        End Property
        Public Property level() As String
            Get
                Return _level
            End Get
            Set(ByVal value As String)
                _level = value
            End Set
        End Property
        Public Property logger() As String
            Get
                Return _logger
            End Get
            Set(ByVal value As String)
                _logger = value
            End Set
        End Property
        Public Property message() As String
            Get
                Return _message
            End Get
            Set(ByVal value As String)
                _message = value
            End Set
        End Property
        Public Property exception() As String
            Get
                Return _exception
            End Get
            Set(ByVal value As String)
                _exception = value
            End Set
        End Property
        Public Property insertDate() As Date
            Get
                Return _insertDate
            End Get
            Set(ByVal value As Date)
                _insertDate = value
            End Set
        End Property
#End Region

        Public Sub New(ByRef results As SqlDataReader)

            If (Not results.IsDBNull(results.GetOrdinal("AppName"))) Then
                _appName = results.GetString(results.GetOrdinal("AppName"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("EnvShortName"))) Then
                _envShortName = results.GetString(results.GetOrdinal("EnvShortName"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ID"))) Then
                _logEntryId = results.GetInt32(results.GetOrdinal("ID"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("LogDate"))) Then
                _logDate = results.GetDateTime(results.GetOrdinal("LogDate"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ApplicationID"))) Then
                _appGUID = results.GetGuid(results.GetOrdinal("ApplicationID")).ToString
            End If
            If (Not results.IsDBNull(results.GetOrdinal("HostName"))) Then
                _hostName = results.GetString(results.GetOrdinal("HostName"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("UserName"))) Then
                _userName = results.GetString(results.GetOrdinal("UserName"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Thread"))) Then
                _thread = results.GetString(results.GetOrdinal("Thread"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Level"))) Then
                _level = results.GetString(results.GetOrdinal("Level"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Logger"))) Then
                _logger = results.GetString(results.GetOrdinal("Logger"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Message"))) Then
                _message = results.GetString(results.GetOrdinal("Message"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Exception"))) Then
                _exception = results.GetString(results.GetOrdinal("Exception"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("InsertDate"))) Then
                _insertDate = results.GetDateTime(results.GetOrdinal("InsertDate"))
            End If
        End Sub

        Public Shared Sub purgeHistory()
            ' Shared methods in this class get a local log4net logger.
            Dim sharedLogger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
            ' Get app GUID.
            Dim appID As String = ConfigurationManager.AppSettings("ApplicationGUID").ToString
            Dim daysToKeep As Integer
            Try
                daysToKeep = CInt(ConfigurationServices.AppSettings("AppDBLogDaysToKeep"))
            Catch ex As Exception
                sharedLogger.Warn("Default value of 30 days will be used for app setting AppDBLogDaysToKeep.")
                ' Purge DB-log entries older than 30 days if app setting is not defined or is not integer.
                daysToKeep = 30
            End Try
            Dim purgeDAO As New AppDBLogDAO()
            Try
                purgeDAO.PurgeHistory(appID, daysToKeep)
            Catch ex As Exception
                If Not sharedLogger Is Nothing Then
                    sharedLogger.Error("Error purging history older than '" + CStr(daysToKeep) + "' day(s) from app DB log.")
                    Throw ex
                End If
            End Try
        End Sub

    End Class
End Namespace
