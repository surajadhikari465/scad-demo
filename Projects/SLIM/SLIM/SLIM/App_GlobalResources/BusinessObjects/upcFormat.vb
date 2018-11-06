Imports Microsoft.VisualBasic

Public Class upcFormat

    Public Shared Function GetPLUstring() As String
        Return String.Format("P{0:yy}{0:MM}{0:dd}{0:HH}{0:mm}", Date.Now)
    End Function

End Class
