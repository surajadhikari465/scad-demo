Imports System
Imports log4net
Imports log4net.Appender
Imports log4net.Core


'
'
'  Custom Appender for log4net.  Raises a Public Event for each log entry written. 
'  Experimental. Do not use.
'
'

Namespace WholeFoods.Utility

    Public Delegate Sub MessageLoggedEventHander(ByVal sender As Object, ByVal e As MessageLoggedEventArgs)

    Public Class MessageLoggedEventArgs
        Inherits EventArgs


        Sub New(ByVal loggingEvent As LoggingEvent)
            _loggingEvent = loggingEvent
        End Sub


        Private _loggingEvent As LoggingEvent

        Public ReadOnly Property LoggingEvent() As LoggingEvent
            Get
                Return _loggingEvent
            End Get
        End Property



    End Class

    Public Class FireEventAppender
        Inherits AppenderSkeleton

        Private Shared _instance As FireEventAppender
        Public Shared Event MessageLoggedEvent As MessageLoggedEventHander


        Public Shared ReadOnly Property Instance() As FireEventAppender
            Get
                Return _instance
            End Get
        End Property



        Sub New()
            _instance = Me
        End Sub


        Private _fixFlags As FixFlags = FixFlags.All
        Public Overridable Property Fix() As FixFlags
            Get
                Return _fixFlags
            End Get
            Set(ByVal value As FixFlags)
                _fixFlags = value
            End Set
        End Property

        Protected Overloads Overrides Sub Append(ByVal loggingEvent As log4net.Core.LoggingEvent)
            loggingEvent.Fix = Me.Fix
            RaiseEvent MessageLoggedEvent(Me, New MessageLoggedEventArgs(loggingEvent))
        End Sub

    End Class
End Namespace
