Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Class ItemSearchBO
        Private _itemKey As Integer
        Private _itemDesc As String
        Private _itemSubTeam As Integer
        Private _itemIdentifier As String

#Region "Property Access Methods"
        Public Property ItemKey() As Integer
            Get
                Return _itemKey
            End Get
            Set(ByVal value As Integer)
                _itemKey = value
            End Set
        End Property

        Public Property ItemDesc() As String
            Get
                Return _itemDesc
            End Get
            Set(ByVal value As String)
                _itemDesc = value
            End Set
        End Property

        Public Property ItemSubTeam() As Integer
            Get
                Return _itemSubTeam
            End Get
            Set(ByVal value As Integer)
                _itemSubTeam = value
            End Set
        End Property

        Public Property ItemIdentifier() As String
            Get
                Return _itemIdentifier
            End Get
            Set(ByVal value As String)
                _itemIdentifier = value
            End Set
        End Property
#End Region
    End Class

End Namespace