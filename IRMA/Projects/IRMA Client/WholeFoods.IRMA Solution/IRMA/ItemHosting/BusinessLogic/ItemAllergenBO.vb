Public Class ItemAllergenBO
    Private _itemNutritionId As Integer
    Private _scale_Allergen_ID As Integer
    Private _description As String
    Private _allergens As String

#Region "Property Access Methods"
    Public Property ItemNutritionId As Integer
        Get
            Return _itemNutritionId
        End Get
        Set(value As Integer)
            _itemNutritionId = value
        End Set
    End Property
    Public Property ScaleAllergenID() As Integer
        Get
            Return _scale_Allergen_ID
        End Get
        Set(ByVal value As Integer)
            _scale_Allergen_ID = value
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
    Public Property Allergens() As String
        Get
            Return _allergens
        End Get
        Set(ByVal value As String)
            _allergens = value
        End Set
    End Property
#End Region
End Class
