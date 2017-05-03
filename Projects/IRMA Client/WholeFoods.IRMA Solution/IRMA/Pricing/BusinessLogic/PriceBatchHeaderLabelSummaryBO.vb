Namespace WholeFoods.IRMA.Pricing.BusinessLogic

    Public Class PriceBatchHeaderLabelSummaryBO

        Private _labelType_ID As Integer
        Private _labelTypeDesc As String
        Private _itemCount As Integer
        Private _priceBatchHeaderID As Integer

#Region "Property Access Methods"
        Property LabelTypeID() As Integer
            Get
                Return _labelType_ID
            End Get
            Set(ByVal value As Integer)
                _labelType_ID = value
            End Set
        End Property

        Property LabelTypeDesc() As String
            Get
                Return _labelTypeDesc
            End Get
            Set(ByVal value As String)
                _labelTypeDesc = value
            End Set
        End Property

        Property ItemCount() As Integer
            Get
                Return _itemCount
            End Get
            Set(ByVal value As Integer)
                _itemCount = value
            End Set
        End Property

        Property PriceBatchHeaderID() As Integer
            Get
                Return _priceBatchHeaderID
            End Get
            Set(ByVal value As Integer)
                _priceBatchHeaderID = value
            End Set
        End Property

#End Region

    End Class

End Namespace
