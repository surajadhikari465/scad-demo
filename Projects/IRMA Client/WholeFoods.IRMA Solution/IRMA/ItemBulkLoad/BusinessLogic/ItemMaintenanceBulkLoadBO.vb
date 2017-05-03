
Namespace WholeFoods.IRMA.ItemBulkLoad.BusinessLogic

    Public Class ItemMaintenanceBulkLoadBO

        Private _itemIdentifier As String
        Private _posDescription As String
        Private _itemDescription As String
        Private _discontinueItem As Integer
        Private _discountable As Integer
        Private _foodStamps As Integer
        Private _nationalClassID As Integer
        Private _taxClassId As Integer
        Private _restrictedHours As Integer

        Property ItemIdentifier() As String
            Get
                Return _itemIdentifier
            End Get
            Set(ByVal value As String)
                _itemIdentifier = value
            End Set
        End Property

        Property PosDescription() As String
            Get
                Return _posDescription
            End Get
            Set(ByVal value As String)
                _posDescription = value
            End Set
        End Property

        Property ItemDescription() As String
            Get
                Return _itemDescription
            End Get
            Set(ByVal value As String)
                _itemDescription = value
            End Set
        End Property

        Property DiscontinueItem() As Integer
            Get
                Return _discontinueItem
            End Get
            Set(ByVal value As Integer)
                _discontinueItem = value
            End Set
        End Property

        Property Discountable() As Integer
            Get
                Return _discountable
            End Get
            Set(ByVal value As Integer)
                _discountable = value
            End Set
        End Property

        Property FoodStamps() As Integer
            Get
                Return _foodStamps
            End Get
            Set(ByVal value As Integer)
                _foodStamps = value
            End Set
        End Property

        Property NationalClassID() As Integer
            Get
                Return _nationalClassID
            End Get
            Set(ByVal value As Integer)
                _nationalClassID = value
            End Set
        End Property

        Property TaxClassId() As Integer
            Get
                Return _taxClassId
            End Get
            Set(ByVal value As Integer)
                _taxClassId = value
            End Set
        End Property

        Property RestrictedHours() As Integer
            Get
                Return _restrictedHours
            End Get
            Set(ByVal value As Integer)
                _restrictedHours = value
            End Set
        End Property
    End Class

End Namespace
