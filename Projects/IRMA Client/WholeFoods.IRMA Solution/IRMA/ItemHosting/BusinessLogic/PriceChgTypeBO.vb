Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Class PriceChgTypeBO

        Private _priceChgTypeID As Integer
        Private _priceChgTypeDesc As String
        Private _priority As Integer
        Private _onSale As Boolean
        Private _msrpRequired As Boolean
        Private _lineDrive As Boolean
        Private _competitive As Boolean
        Private _lastUpdateTimestamp As DateTime

#Region "Property Access Methods"

        Public Property PriceChgTypeID() As Integer
            Get
                Return _priceChgTypeID
            End Get
            Set(ByVal value As Integer)
                _priceChgTypeID = value
            End Set
        End Property

        Public Property PriceChgTypeDesc() As String
            Get
                Return _priceChgTypeDesc
            End Get
            Set(ByVal value As String)
                _priceChgTypeDesc = value
            End Set
        End Property

        Public Property Priority() As Integer
            Get
                Return _priority
            End Get
            Set(ByVal value As Integer)
                _priority = value
            End Set
        End Property

        Public Property IsOnSale() As Boolean
            Get
                Return _onSale
            End Get
            Set(ByVal value As Boolean)
                _onSale = value
            End Set
        End Property

        Public Property IsMSRPRequired() As Boolean
            Get
                Return _msrpRequired
            End Get
            Set(ByVal value As Boolean)
                _msrpRequired = value
            End Set
        End Property

        Public Property IsLineDrive() As Boolean
            Get
                Return _lineDrive
            End Get
            Set(ByVal value As Boolean)
                _lineDrive = value
            End Set
        End Property

        Public Property IsCompetitive() As Boolean
            Get
                Return _competitive
            End Get
            Set(ByVal value As Boolean)
                _competitive = value
            End Set
        End Property

        Public Property LastUpdateTimestamp() As DateTime
            Get
                Return _lastUpdateTimestamp
            End Get
            Set(ByVal value As DateTime)
                _lastUpdateTimestamp = value
            End Set
        End Property
#End Region

    End Class

End Namespace
