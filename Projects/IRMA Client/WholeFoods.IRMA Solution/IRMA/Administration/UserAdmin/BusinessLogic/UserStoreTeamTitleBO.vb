Public Class UserStoreTeamTitleBO

    Private _userID As Integer
    Private _titleID As Integer
    Private _storeNo As Integer
    Private _teamNo As Integer

    Public Property UserID() As Integer
        Get
            Return Me._userID
        End Get
        Set(ByVal value As Integer)
            Me._userID = value
        End Set
    End Property

    Public Property TitleID() As Integer
        Get
            Return Me._titleID
        End Get
        Set(ByVal value As Integer)
            Me._titleID = value
        End Set
    End Property

    Public Property StoreNo() As Integer
        Get
            Return Me._storeNo
        End Get
        Set(ByVal value As Integer)
            Me._storeNo = value
        End Set
    End Property

    Public Property TeamNo() As Integer
        Get
            Return Me._teamNo
        End Get
        Set(ByVal value As Integer)
            Me._teamNo = value
        End Set
    End Property

    Public Sub New()
    End Sub

    Public Sub New(ByVal User As Integer, ByVal Store As Integer, ByVal Team As Integer, ByVal Title As Integer)
        Me._userID = User
        Me._storeNo = Store
        Me._teamNo = Team
        Me._titleID = Title
    End Sub

    Public Shared Function Add(ByVal Entry As UserStoreTeamTitleBO) As Boolean
        Try
            Return UserStoreTeamTitleDAO.Add(Entry)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function Remove(ByVal Entry As UserStoreTeamTitleBO) As Boolean

        Try
            Return UserStoreTeamTitleDAO.Remove(Entry)
        Catch ex As Exception
            Throw ex
        End Try

    End Function

End Class
