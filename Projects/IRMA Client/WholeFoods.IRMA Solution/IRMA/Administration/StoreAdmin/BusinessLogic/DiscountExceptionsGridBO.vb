Public Class DiscountExceptionsGridBO
    Public Property DiscountException As Boolean
    Public Property SubTeam_Name As String
    Public Property SubTeam_No As Integer
    Public Property OriginalExceptionState As Boolean

    Public Sub New(SubTeamNo As Integer, SubTeamName As String, IsDiscountException As Boolean)
        SubTeam_No = SubTeamNo
        SubTeam_Name = SubTeamName
        DiscountException = IsDiscountException
        OriginalExceptionState = IsDiscountException
    End Sub
End Class
