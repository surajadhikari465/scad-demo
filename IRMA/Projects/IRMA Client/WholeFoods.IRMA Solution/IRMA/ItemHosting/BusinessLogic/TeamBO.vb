Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic
    Public Class TeamBO

        Private _TeamNo As Integer
        Public Property TeamNo() As Integer
            Get
                Return _TeamNo
            End Get
            Set(ByVal value As Integer)
                _TeamNo = value
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


        Private _TeamAbbr As String
        Public Property TeamAbbr() As String
            Get
                Return _TeamAbbr
            End Get
            Set(ByVal value As String)
                _TeamAbbr = value
            End Set
        End Property

        Sub New(ByVal TeamNo As Integer, ByVal TeamName As String, ByVal TeamAbbr As String)
            Me._TeamAbbr = TeamAbbr
            Me._TeamNo = TeamNo
            Me._TeamName = TeamName
        End Sub

        Sub New()

        End Sub
    End Class
End Namespace
