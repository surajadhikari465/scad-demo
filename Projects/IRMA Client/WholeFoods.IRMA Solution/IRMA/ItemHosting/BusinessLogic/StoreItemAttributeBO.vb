Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Class StoreItemAttributeBO
        Private _ID As Integer
        Private _exempt As Boolean
        Private _storeNo As Integer
        Private _itemKey As Integer

        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property

        Public Property StoreNo() As Integer
            Get
                Return _storeNo
            End Get
            Set(ByVal value As Integer)
                _storeNo = value
            End Set
        End Property

        Public Property ItemKey() As Integer
            Get
                Return _itemKey
            End Get
            Set(ByVal value As Integer)
                _itemKey = value
            End Set
        End Property

        Public Property Exempt() As Boolean
            Get
                Return _exempt
            End Get
            Set(ByVal value As Boolean)
                _exempt = value
            End Set
        End Property
    End Class
End Namespace
