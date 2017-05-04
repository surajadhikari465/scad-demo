Namespace WholeFoods.IRMA.Pricing.BusinessLogic

    Public Class PriceBatchSearchBO

        Private _identifier As String
        Private _itemDescription As String
        Private _startDate As Date  'FROM DATE
        Private _endDate As Date    'TO DATE
        Private _storeList As String
        Private _storeListSeparator As Char
        Private _itemChgTypeID As Integer
        Private _priceChgTypeID As Integer
        Private _subTeamNo As Integer
        Private _priceBatchStatusID As Integer
        Private _batchDescription As String
        Private _autoApplyFlag As String
        Private _autoApplyDate As Date
        Private _IncScaleItems As Boolean
        Private _IncNonRetailItems As Boolean


#Region "Property Access Methods"
        Property BatchDescription() As String
            Get
                Return _batchDescription
            End Get
            Set(ByVal value As String)
                _batchDescription = value
            End Set
        End Property

        Property AutoApplyFlag() As String
            Get
                Return _autoApplyFlag
            End Get
            Set(ByVal value As String)
                _autoApplyFlag = value
            End Set
        End Property

        Property AutoApplyDate() As Date
            Get
                Return _autoApplyDate
            End Get
            Set(ByVal value As Date)
                _autoApplyDate = value
            End Set
        End Property

        Property Identifier() As String
            Get
                Return _identifier
            End Get
            Set(ByVal value As String)
                _identifier = value
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

        Property StartDate() As Date
            Get
                Return _startDate
            End Get
            Set(ByVal value As Date)
                _startDate = value
            End Set
        End Property

        Property EndDate() As Date
            Get
                Return _endDate
            End Get
            Set(ByVal value As Date)
                _endDate = value
            End Set
        End Property

        Property StoreList() As String
            Get
                Return _storeList
            End Get
            Set(ByVal value As String)
                _storeList = value
            End Set
        End Property

        Property StoreListSeparator() As Char
            Get
                Return _storeListSeparator
            End Get
            Set(ByVal value As Char)
                _storeListSeparator = value
            End Set
        End Property

        Property ItemChgTypeID() As Integer
            Get
                Return _itemChgTypeID
            End Get
            Set(ByVal value As Integer)
                _itemChgTypeID = value
            End Set
        End Property

        Property PriceChgTypeID() As Integer
            Get
                Return _priceChgTypeID
            End Get
            Set(ByVal value As Integer)
                _priceChgTypeID = value
            End Set
        End Property

        Property SubTeamNo() As Integer
            Get
                Return _subTeamNo
            End Get
            Set(ByVal value As Integer)
                _subTeamNo = value
            End Set
        End Property

        Property PriceBatchStatusID() As Integer
            Get
                Return _priceBatchStatusID
            End Get
            Set(ByVal value As Integer)
                _priceBatchStatusID = value
            End Set
        End Property
        Property IncScaleItems() As Boolean
            Get
                Return _IncScaleItems
            End Get
            Set(ByVal value As Boolean)
                _IncScaleItems = value
            End Set
        End Property

        Property IncNonRetailItems() As Boolean
            Get
                Return _IncNonRetailItems
            End Get
            Set(value As Boolean)
                _IncNonRetailItems = value
            End Set
        End Property
#End Region

    End Class

End Namespace
