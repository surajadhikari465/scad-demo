Imports System.Text

Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Class ScaleDetailsBO

#Region "Declarations"

        ' lists
        Private _scaleEatByList As New ArrayList
        Private _scaleGradeList As New ArrayList
        Private _scaleLabelStyleList As New ArrayList
        Private _scaleNutrifactList As New ArrayList
        Private _scaleExtraTextList As New ArrayList
        Private _scaleRandomWeightTypeList As New ArrayList
        Private _scaleTareAlternateList As New ArrayList
        Private _scaleTareList As New ArrayList
        Private _scaleUOMList As New ArrayList

        ' properties
        Private _itemScaleID As Integer = 0
        Private _itemKey As Integer
        Private _storeJurisdictionID As String
        Private _storeJurisdictionDesc As String
        Private _itemIdentifier As String
        Private _eatBy As Integer
        Private _grade As Integer
        Private _labelStyle As Integer
        Private _nutrifact As Integer
        Private _extraTextID As Integer
        Private _extraText As String
        Private _description As String
        Private _randomWeightType As Integer
        Private _tareAlternate As Integer
        Private _tare As Integer
        Private _UOM As Integer
        Private _fixedWeight As String
        Private _byCount As Integer
        Private _forceTare As Boolean
        Private _printBlankShelfLife As Boolean
        Private _printBlankPackDate As Boolean
        Private _printBlankUnitPrice As Boolean
        Private _printBlankShelfEatBy As Boolean
        Private _printBlankWeight As Boolean
        Private _printBlankTotalPrice As Boolean
        Private _scaleDescription1 As String
        Private _scaleDescription2 As String
        Private _scaleDescription3 As String
        Private _scaleDescription4 As String
        Private _shelfLifeLength As Integer
        Private _customerFacingScaleDepartment As Boolean?
        Private _sendToScale As Boolean?

#End Region

#Region "List Property Access Methods"

        Public ReadOnly Property ScaleEatByList() As ArrayList
            Get
                Return _scaleEatByList
            End Get
        End Property

        Public Sub Add_ScaleEatBy(ByVal scaleEatBy As ScaleEatByBO)
            _scaleEatByList.Add(scaleEatBy)
        End Sub

        Public ReadOnly Property ScaleGradeList() As ArrayList
            Get
                Return _scaleGradeList
            End Get
        End Property

        Public Sub Add_ScaleGrade(ByVal scaleGrade As ScaleGradeBO)
            _scaleGradeList.Add(scaleGrade)
        End Sub

        Public ReadOnly Property ScaleLabelStyleList() As ArrayList
            Get
                Return _scaleLabelStyleList
            End Get
        End Property

        Public Sub Add_ScaleLabelStyle(ByVal scaleLabelStyle As ScaleLabelStyleBO)
            _scaleLabelStyleList.Add(scaleLabelStyle)
        End Sub

        Public ReadOnly Property ScaleNutrifactList() As ArrayList
            Get
                Return _scaleNutrifactList
            End Get
        End Property

        Public Sub Add_ScaleNutrifact(ByVal scaleNutrifact As ScaleNutrifactBO)
            _scaleNutrifactList.Add(scaleNutrifact)
        End Sub

        Public ReadOnly Property ScaleExtraTextList() As ArrayList
            Get
                Return _scaleExtraTextList
            End Get
        End Property

        Public Sub Add_ScaleExtraText(ByVal scaleExtraText As ScaleExtraTextBO)
            _scaleExtraTextList.Add(scaleExtraText)
        End Sub

        Public ReadOnly Property ScaleRandomWeightTypeList() As ArrayList
            Get
                Return _scaleRandomWeightTypeList
            End Get
        End Property

        Public Sub Add_ScaleRandomWeightType(ByVal scaleRandomWeightType As ScaleRandomWeightTypeBO)
            _scaleRandomWeightTypeList.Add(scaleRandomWeightType)
        End Sub

        Public ReadOnly Property ScaleTareAlternateList() As ArrayList
            Get
                Return _scaleTareAlternateList
            End Get
        End Property

        Public Sub Add_ScaleTareAlternate(ByVal scaleTare As ScaleTareBO)
            _scaleTareAlternateList.Add(scaleTare)
        End Sub

        Public ReadOnly Property ScaleTareList() As ArrayList
            Get
                Return _scaleTareList
            End Get
        End Property

        Public Sub Add_ScaleTare(ByVal scaleTare As ScaleTareBO)
            _scaleTareList.Add(scaleTare)
        End Sub

        Public ReadOnly Property ScaleUOMList() As ArrayList
            Get
                Return _scaleUOMList
            End Get
        End Property

        Public Sub Add_ScaleUOM(ByVal scaleUOM As ScaleUOMsBO)
            _scaleUOMList.Add(scaleUOM)
        End Sub

#End Region

#Region "Properties"
        Public Property ItemScaleID() As Integer
            Get
                Return _itemScaleID
            End Get
            Set(ByVal value As Integer)
                _itemScaleID = value
            End Set
        End Property
        Public Property StoreJurisdictionID() As String
            Get
                Return _storeJurisdictionID
            End Get
            Set(ByVal value As String)
                _storeJurisdictionID = value
            End Set
        End Property
        Public Property StoreJurisdictionDesc() As String
            Get
                Return _storeJurisdictionDesc
            End Get
            Set(ByVal value As String)
                _storeJurisdictionDesc = value
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
        Public Property ItemIdentifier() As String
            Get
                Return _itemIdentifier
            End Get
            Set(ByVal value As String)
                _itemIdentifier = value
            End Set
        End Property
        Public Property EatBy() As Integer
            Get
                Return _eatBy
            End Get
            Set(ByVal value As Integer)
                _eatBy = value
            End Set
        End Property
        Public Property Grade() As Integer
            Get
                Return _grade
            End Get
            Set(ByVal value As Integer)
                _grade = value
            End Set
        End Property
        Public Property LabelStyle() As Integer
            Get
                Return _labelStyle
            End Get
            Set(ByVal value As Integer)
                _labelStyle = value
            End Set
        End Property
        Public Property Nutrifact() As Integer
            Get
                Return _nutrifact
            End Get
            Set(ByVal value As Integer)
                _nutrifact = value
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
        Public Property ExtraText() As String
            Get
                Return _extraText
            End Get
            Set(ByVal value As String)
                _extraText = value
            End Set
        End Property
        Public Property IngredientID As Integer
        Public Property Ingredient As String
        Public Property AllergenID As Integer
        Public Property Allergen As String
        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

        Public Property RandomWeightType() As Integer
            Get
                Return _randomWeightType
            End Get
            Set(ByVal value As Integer)
                _randomWeightType = value
            End Set
        End Property
        Public Property TareAlternate() As Integer
            Get
                Return _tareAlternate
            End Get
            Set(ByVal value As Integer)
                _tareAlternate = value
            End Set
        End Property
        Public Property Tare() As Integer
            Get
                Return _tare
            End Get
            Set(ByVal value As Integer)
                _tare = value
            End Set
        End Property
        Public Property UOM() As Integer
            Get
                Return _UOM
            End Get
            Set(ByVal value As Integer)
                _UOM = value
            End Set
        End Property
        Public Property FixedWeight() As String
            Get
                Return _fixedWeight
            End Get
            Set(ByVal value As String)
                _fixedWeight = value
            End Set
        End Property
        Public Property ByCount() As Integer
            Get
                Return _byCount
            End Get
            Set(ByVal value As Integer)
                _byCount = value
            End Set
        End Property
        Public Property ForceTare() As Boolean
            Get
                Return _forceTare
            End Get
            Set(ByVal value As Boolean)
                _forceTare = value
            End Set
        End Property
        Public Property PrintBlankShelfLife() As Boolean
            Get
                Return _printBlankShelfLife
            End Get
            Set(ByVal value As Boolean)
                _printBlankShelfLife = value
            End Set
        End Property
        Public Property PrintBlankPackDate() As Boolean
            Get
                Return _printBlankPackDate
            End Get
            Set(ByVal value As Boolean)
                _printBlankPackDate = value
            End Set
        End Property
        Public Property PrintBlankUnitPrice() As Boolean
            Get
                Return _printBlankUnitPrice
            End Get
            Set(ByVal value As Boolean)
                _printBlankUnitPrice = value
            End Set
        End Property
        Public Property PrintBlankShelfEatBy() As Boolean
            Get
                Return _printBlankShelfEatBy
            End Get
            Set(ByVal value As Boolean)
                _printBlankShelfEatBy = value
            End Set
        End Property
        Public Property PrintBlankWeight() As Boolean
            Get
                Return _printBlankWeight
            End Get
            Set(ByVal value As Boolean)
                _printBlankWeight = value
            End Set
        End Property
        Public Property PrintBlankTotalPrice() As Boolean
            Get
                Return _printBlankTotalPrice
            End Get
            Set(ByVal value As Boolean)
                _printBlankTotalPrice = value
            End Set
        End Property
        Public Property ScaleDescription1() As String
            Get
                Return _scaleDescription1
            End Get
            Set(ByVal value As String)
                _scaleDescription1 = value
            End Set
        End Property
        Public Property ScaleDescription2() As String
            Get
                Return _scaleDescription2
            End Get
            Set(ByVal value As String)
                _scaleDescription2 = value
            End Set
        End Property
        Public Property ScaleDescription3() As String
            Get
                Return _scaleDescription3
            End Get
            Set(ByVal value As String)
                _scaleDescription3 = value
            End Set
        End Property
        Public Property ScaleDescription4() As String
            Get
                Return _scaleDescription4
            End Get
            Set(ByVal value As String)
                _scaleDescription4 = value
            End Set
        End Property
        Public Property ShelfLifeLength() As Integer
            Get
                Return _shelfLifeLength
            End Get
            Set(ByVal value As Integer)
                _shelfLifeLength = value
            End Set
        End Property
        Public Property CustomerFacingScaleDepartment() As Boolean?
            Get
                Return _customerFacingScaleDepartment
            End Get
            Set(ByVal value As Boolean?)
                _customerFacingScaleDepartment = value
            End Set
        End Property
        Public Property SendToScale() As Boolean?
            Get
                Return _sendToScale
            End Get
            Set(ByVal value As Boolean?)
                _sendToScale = value
            End Set
        End Property

        Public Property IsNewToScale As Boolean

#End Region

    End Class

End Namespace