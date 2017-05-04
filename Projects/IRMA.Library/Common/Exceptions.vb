Namespace Common.Exceptions
    Public Class UserCanceledException
        Inherits System.Exception
        Public Sub New()
            MyBase.New("User Canceled Operation")
        End Sub
    End Class
End Namespace
