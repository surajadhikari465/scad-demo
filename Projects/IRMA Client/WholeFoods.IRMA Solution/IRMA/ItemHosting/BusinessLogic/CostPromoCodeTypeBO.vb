Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Class CostPromoCodeTypeBO

        Private _costPromoCodeTypeID As Integer
        Private _costPromoCode As Integer
        Private _costPromoDesc As String

        Public Property CostPromoCodeTypeID() As Integer
            Get
                Return _costPromoCodeTypeID
            End Get
            Set(ByVal value As Integer)
                _costPromoCodeTypeID = value
            End Set
        End Property

        Public Property CostPromoCode() As Integer
            Get
                Return _costPromoCode
            End Get
            Set(ByVal value As Integer)
                _costPromoCode = value
            End Set
        End Property

        Public Property CostPromoDesc() As String
            Get
                Return _costPromoDesc
            End Get
            Set(ByVal value As String)
                _costPromoDesc = value
            End Set
        End Property

    End Class

End Namespace