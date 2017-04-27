Namespace WholeFoods.IRMA.Replenishment.TLog
    Public Class TlogLoginInfo

        Sub New()


        End Sub



        Private _Database As String
        Public Property Database() As String
            Get
                Return _Database
            End Get
            Set(ByVal value As String)
                _Database = value
            End Set
        End Property

        Private _DBUser As String
        Public Property DBUser() As String
            Get
                Return _DBUser
            End Get
            Set(ByVal value As String)
                _DBUser = value
            End Set
        End Property


        Private _DBPass As String
        Public Property DBPass() As String
            Get
                Return _DBPass
            End Get
            Set(ByVal value As String)
                _DBPass = value
            End Set
        End Property

        Private _ProcessUser As String
        Public Property ProcessUser() As String
            Get
                Return _ProcessUser
            End Get
            Set(ByVal value As String)
                _ProcessUser = value
            End Set
        End Property


        Private _ProcessPass As String
        Public Property ProcessPass() As String
            Get
                Return _ProcessPass
            End Get
            Set(ByVal value As String)
                _ProcessPass = value
            End Set
        End Property


        Private _DBServer As String
        Public Property DBServer() As String
            Get
                Return _DBServer
            End Get
            Set(ByVal value As String)
                _DBServer = value
            End Set
        End Property


        Private _RunFromClient As Boolean
        Public Property RunFromClient() As Boolean
            Get
                Return _RunFromClient
            End Get
            Set(ByVal value As Boolean)
                _RunFromClient = value
            End Set
        End Property




    End Class
End Namespace

