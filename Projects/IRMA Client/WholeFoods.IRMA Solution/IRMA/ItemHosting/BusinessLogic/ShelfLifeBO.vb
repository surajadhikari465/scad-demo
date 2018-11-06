Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Class ShelfLifeBO

        Private _shelfLifeID As Integer
        Private _shelfLifeName As String

#Region "Property Access Methods"

        Public Property ShelfLifeID() As Integer
            Get
                Return _shelfLifeID
            End Get
            Set(ByVal value As Integer)
                _shelfLifeID = value
            End Set
        End Property

        Public Property ShelfLifeName() As String
            Get
                Return _shelfLifeName
            End Get
            Set(ByVal value As String)
                _shelfLifeName = value
            End Set
        End Property

#End Region

    End Class

End Namespace
