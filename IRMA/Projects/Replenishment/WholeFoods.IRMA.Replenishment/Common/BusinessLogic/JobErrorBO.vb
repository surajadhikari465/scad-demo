Imports log4net
Imports System.Windows.Forms
Imports WholeFoods.IRMA.Replenishment.Jobs

Namespace WholeFoods.IRMA.Replenishment.Common.BusinessLogic
    Public Class JobErrorBO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Private _classname As String
        Private _runDate As Date
        Private _serverName As String
        Private _exceptionText As String

        Public Property Classname() As String
            Get
                Return _classname
            End Get
            Set(ByVal value As String)
                _classname = value
            End Set
        End Property

        Public Property LastRun() As Date
            Get
                Return _runDate
            End Get
            Set(ByVal value As Date)
                _runDate = value
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

        Public Property ExceptionText() As String
            Get
                Return _exceptionText
            End Get
            Set(ByVal value As String)
                _exceptionText = value
            End Set
        End Property

    End Class
End Namespace
