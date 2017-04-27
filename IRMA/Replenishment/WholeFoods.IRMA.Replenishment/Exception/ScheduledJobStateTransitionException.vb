''' <summary>
''' This exception is thrown if a scheduled job tries to update the state
''' in the JobStatus table in a manner that is not allowed.
''' 
''' Allowed State Transitions:
'''      complete -> running 
'''      running -> waiting
'''      running -> failed
'''      running -> complete
'''      waiting -> running   
''' </summary>
''' <remarks></remarks>
Public Class ScheduledJobStateTransitionException
    Inherits System.ApplicationException

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal inner As Exception)
        MyBase.New(message, inner)
    End Sub

End Class
