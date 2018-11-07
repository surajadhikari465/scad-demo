Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Class ItemExtraTextBO
        Private _itemNutritionId As Integer
        Private _extraTextID As Integer
        Private _description As String
        Private _scale_LabelType_ID As Integer
        Private _extraText As String

#Region "Property Access Methods"
        Public Property ItemNutritionId As Integer
            Get
                Return _itemNutritionId
            End Get
            Set(value As Integer)
                _itemNutritionId = value
            End Set
        End Property
        Public Property ExtraTextID() As Integer
            Get
                Return _extraTextID
            End Get
            Set(ByVal value As Integer)
                _extraTextID = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

        Public Property Scale_LabelType_ID() As Integer
            Get
                Return _scale_LabelType_ID
            End Get
            Set(ByVal value As Integer)
                _scale_LabelType_ID = value
            End Set
        End Property

        Public Property ExtraText() As String
            Get
                Return _extraText
            End Get
            Set(ByVal value As String)
                _extraText = value
            End Set
        End Property
#End Region
    End Class
End Namespace
