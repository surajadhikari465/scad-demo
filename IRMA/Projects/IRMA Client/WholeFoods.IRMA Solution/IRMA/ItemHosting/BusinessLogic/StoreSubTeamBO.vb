Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic
    Public Class StoreSubTeamBO

        Private _Store_No As Integer
        Public Property Store_No() As Integer
            Get
                Return _Store_No
            End Get
            Set(ByVal value As Integer)
                _Store_No = value
            End Set
        End Property


        Private _SubTeam_No As Integer
        Public Property SubTeam_No() As Integer
            Get
                Return _SubTeam_No
            End Get
            Set(ByVal value As Integer)
                _SubTeam_No = value
            End Set
        End Property


        Private _SubTeamName As String
        Public Property SubTeamName() As String
            Get
                Return _SubTeamName
            End Get
            Set(ByVal value As String)
                _SubTeamName = value
            End Set
        End Property


        Private _Team_No As Integer
        Public Property Team_No() As Integer
            Get
                Return _Team_No
            End Get
            Set(ByVal value As Integer)
                _Team_No = value
            End Set
        End Property


        Private _TeamName As String
        Public Property TeamName() As String
            Get
                Return _TeamName
            End Get
            Set(ByVal value As String)
                _TeamName = value
            End Set
        End Property



    End Class
End Namespace