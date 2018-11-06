Public Class UsersSubTeamBO

    Private _userID As Integer
    Private _subTeamNo As Integer
    Private _coordinator As Boolean

    Public Property UserID() As Integer
        Get
            Return Me._userID
        End Get
        Set(ByVal value As Integer)
            Me._userID = value
        End Set
    End Property

    Public Property SubTeamNo() As Integer
        Get
            Return Me._subTeamNo
        End Get
        Set(ByVal value As Integer)
            Me._subTeamNo = value
        End Set
    End Property

    Public Property Coordinator() As Boolean
        Get
            Return Me._coordinator
        End Get
        Set(ByVal value As Boolean)
            Me._coordinator = False
        End Set
    End Property

    Public Sub New()
    End Sub

    Public Sub New(ByVal UserID As Integer, ByVal SubTeamNo As Integer, ByVal IsCoordinator As Boolean)
        Me._userID = UserID
        Me._subTeamNo = SubTeamNo
        Me._coordinator = IsCoordinator
    End Sub

    Public Shared Function Add(ByVal Entry As UsersSubTeamBO) As Boolean

        Try
            Try
                Return UsersSubTeamDAO.Add(Entry)
            Catch ex As Exception
                Throw ex
            End Try
        Catch ex As Exception

        End Try

    End Function

    Public Shared Function Remove(ByVal Entry As UsersSubTeamBO) As Boolean

        Try
            Return UsersSubTeamDAO.Remove(Entry)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function Update(ByVal Entry As UsersSubTeamBO) As Boolean

        Try
            Return UsersSubTeamDAO.Update(Entry)
        Catch ex As Exception
            Throw ex
        End Try

    End Function

End Class
