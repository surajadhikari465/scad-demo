Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Class ScaleNutrifactBO

        Private _ID As Integer
        Private _Description As String
        Private _Scale_LabelFormat_ID As Integer
        Private _ServingUnits As Integer
        Private _ServingsPerPortion As Double
        Private _SizeWeight As Integer
        Private _Calories As Integer
        Private _CaloriesFat As Integer
        Private _CaloriesSaturatedFat As Integer
        Private _ServingPerContainer As String
        Private _TotalFatWeight As Decimal
        Private _TotalFatPercentage As Integer
        Private _SaturatedFatWeight As Decimal
        Private _SaturatedFatPercent As Integer
        Private _PolyunsaturatedFat As Integer
        Private _MonounsaturatedFat As Integer
        Private _CholesterolWeight As Decimal
        Private _CholesterolPercent As Integer
        Private _SodiumWeight As Decimal
        Private _SodiumPercent As Integer
        Private _PotassiumWeight As Decimal
        Private _PotassiumPercent As Integer
        Private _TotalCarbohydrateWeight As Decimal
        Private _TotalCarbohydratePercent As Integer
        Private _DietaryFiberWeight As Decimal
        Private _DietaryFiberPercent As Integer
        Private _SolubleFiber As Decimal
        Private _InsolubleFiber As Decimal
        Private _Sugar As Decimal
        Private _SugarAlcohol As Decimal
        Private _OtherCarbohydrates As Decimal
        Private _ProteinWeight As Decimal
        Private _ProteinPercent As Integer
        Private _VitaminA As Integer
        Private _Betacarotene As Integer
        Private _VitaminC As Integer
        Private _Calcium As Integer
        Private _Iron As Integer
        Private _VitaminD As Integer
        Private _VitaminE As Integer
        Private _Thiamin As Integer
        Private _Riboflavin As Integer
        Private _Niacin As Integer
        Private _VitaminB6 As Integer
        Private _Folate As Integer
        Private _VitaminB12 As Integer
        Private _Biotin As Integer
        Private _PantothenicAcid As Integer
        Private _Phosphorous As Integer
        Private _Iodine As Integer
        Private _Magnesium As Integer
        Private _Zinc As Integer
        Private _Copper As Integer
        Private _Transfat As Integer
        Private _TransfatWeight As Decimal
        Private _CaloriesFromTransFat As Integer
        Private _Om6Fatty As Decimal
        Private _Om3Fatty As Decimal
        Private _Starch As Decimal
        Private _Chloride As Integer
        Private _Chromium As Integer
        Private _VitaminK As Integer
        Private _Manganese As Integer
        Private _Molybdenum As Integer
        Private _Selenium As Integer
        Private _ServingSizeDesc As String


#Region "Property Access Methods"

        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property
        Public Property Scale_LabelFormat_ID() As Integer
            Get
                Return _Scale_LabelFormat_ID
            End Get
            Set(ByVal value As Integer)
                _Scale_LabelFormat_ID = value
            End Set
        End Property
        Public Property ServingUnits() As Integer
            Get
                Return _ServingUnits
            End Get
            Set(ByVal value As Integer)
                _ServingUnits = value
            End Set
        End Property
        Public Property ServingsPerPortion() As Double
            Get
                Return _ServingsPerPortion
            End Get
            Set(ByVal value As Double)
                _ServingsPerPortion = value
            End Set
        End Property

        Public Property SizeWeight() As Integer
            Get
                Return _SizeWeight
            End Get
            Set(ByVal value As Integer)
                _SizeWeight = value
            End Set
        End Property
        Public Property Calories() As Integer
            Get
                Return _Calories
            End Get
            Set(ByVal value As Integer)
                _Calories = value
            End Set
        End Property
        Public Property CaloriesFat() As Integer
            Get
                Return _CaloriesFat
            End Get
            Set(ByVal value As Integer)
                _CaloriesFat = value
            End Set
        End Property
        Public Property CaloriesSaturatedFat() As Integer
            Get
                Return _CaloriesSaturatedFat
            End Get
            Set(ByVal value As Integer)
                _CaloriesSaturatedFat = value
            End Set
        End Property
        Public Property ServingPerContainer() As String
            Get
                Return _ServingPerContainer
            End Get
            Set(ByVal value As String)
                _ServingPerContainer = value
            End Set
        End Property
        Public Property TotalFatWeight() As Decimal
            Get
                Return _TotalFatWeight
            End Get
            Set(ByVal value As Decimal)
                _TotalFatWeight = value
            End Set
        End Property
        Public Property TotalFatPercentage() As Integer
            Get
                Return _TotalFatPercentage
            End Get
            Set(ByVal value As Integer)
                _TotalFatPercentage = value
            End Set
        End Property
        Public Property SaturatedFatWeight() As Decimal
            Get
                Return _SaturatedFatWeight
            End Get
            Set(ByVal value As Decimal)
                _SaturatedFatWeight = value
            End Set
        End Property
        Public Property SaturatedFatPercent() As Integer
            Get
                Return _SaturatedFatPercent
            End Get
            Set(ByVal value As Integer)
                _SaturatedFatPercent = value
            End Set
        End Property
        Public Property PolyunsaturatedFat() As Integer
            Get
                Return _PolyunsaturatedFat
            End Get
            Set(ByVal value As Integer)
                _PolyunsaturatedFat = value
            End Set
        End Property
        Public Property MonounsaturatedFat() As Integer
            Get
                Return _MonounsaturatedFat
            End Get
            Set(ByVal value As Integer)
                _MonounsaturatedFat = value
            End Set
        End Property
        Public Property CholesterolWeight() As Decimal
            Get
                Return _CholesterolWeight
            End Get
            Set(ByVal value As Decimal)
                _CholesterolWeight = value
            End Set
        End Property
        Public Property CholesterolPercent() As Integer
            Get
                Return _CholesterolPercent
            End Get
            Set(ByVal value As Integer)
                _CholesterolPercent = value
            End Set
        End Property
        Public Property SodiumWeight() As Decimal
            Get
                Return _SodiumWeight
            End Get
            Set(ByVal value As Decimal)
                _SodiumWeight = value
            End Set
        End Property
        Public Property SodiumPercent() As Integer
            Get
                Return _SodiumPercent
            End Get
            Set(ByVal value As Integer)
                _SodiumPercent = value
            End Set
        End Property
        Public Property PotassiumWeight() As Decimal
            Get
                Return _PotassiumWeight
            End Get
            Set(ByVal value As Decimal)
                _PotassiumWeight = value
            End Set
        End Property
        Public Property PotassiumPercent() As Integer
            Get
                Return _PotassiumPercent
            End Get
            Set(ByVal value As Integer)
                _PotassiumPercent = value
            End Set
        End Property
        Public Property TotalCarbohydrateWeight() As Decimal
            Get
                Return _TotalCarbohydrateWeight
            End Get
            Set(ByVal value As Decimal)
                _TotalCarbohydrateWeight = value
            End Set
        End Property
        Public Property TotalCarbohydratePercent() As Integer
            Get
                Return _TotalCarbohydratePercent
            End Get
            Set(ByVal value As Integer)
                _TotalCarbohydratePercent = value
            End Set
        End Property
        Public Property DietaryFiberWeight() As Decimal
            Get
                Return _DietaryFiberWeight
            End Get
            Set(ByVal value As Decimal)
                _DietaryFiberWeight = value
            End Set
        End Property
        Public Property DietaryFiberPercent() As Integer
            Get
                Return _DietaryFiberPercent
            End Get
            Set(ByVal value As Integer)
                _DietaryFiberPercent = value
            End Set
        End Property
        Public Property SolubleFiber() As Decimal
            Get
                Return _SolubleFiber
            End Get
            Set(ByVal value As Decimal)
                _SolubleFiber = value
            End Set
        End Property
        Public Property InsolubleFiber() As Decimal
            Get
                Return _InsolubleFiber
            End Get
            Set(ByVal value As Decimal)
                _InsolubleFiber = value
            End Set
        End Property
        Public Property Sugar() As Decimal
            Get
                Return _Sugar
            End Get
            Set(ByVal value As Decimal)
                _Sugar = value
            End Set
        End Property
        Public Property SugarAlcohol() As Decimal
            Get
                Return _SugarAlcohol
            End Get
            Set(ByVal value As Decimal)
                _SugarAlcohol = value
            End Set
        End Property
        Public Property OtherCarbohydrates() As Decimal
            Get
                Return _OtherCarbohydrates
            End Get
            Set(ByVal value As Decimal)
                _OtherCarbohydrates = value
            End Set
        End Property
        Public Property ProteinWeight() As Decimal
            Get
                Return _ProteinWeight
            End Get
            Set(ByVal value As Decimal)
                _ProteinWeight = value
            End Set
        End Property
        Public Property ProteinPercent() As Integer
            Get
                Return _ProteinPercent
            End Get
            Set(ByVal value As Integer)
                _ProteinPercent = value
            End Set
        End Property
        Public Property VitaminA() As Integer
            Get
                Return _VitaminA
            End Get
            Set(ByVal value As Integer)
                _VitaminA = value
            End Set
        End Property
        Public Property Betacarotene() As Integer
            Get
                Return _Betacarotene
            End Get
            Set(ByVal value As Integer)
                _Betacarotene = value
            End Set
        End Property
        Public Property VitaminC() As Integer
            Get
                Return _VitaminC
            End Get
            Set(ByVal value As Integer)
                _VitaminC = value
            End Set
        End Property
        Public Property Calcium() As Integer
            Get
                Return _Calcium
            End Get
            Set(ByVal value As Integer)
                _Calcium = value
            End Set
        End Property
        Public Property Iron() As Integer
            Get
                Return _Iron
            End Get
            Set(ByVal value As Integer)
                _Iron = value
            End Set
        End Property
        Public Property VitaminD() As Integer
            Get
                Return _VitaminD
            End Get
            Set(ByVal value As Integer)
                _VitaminD = value
            End Set
        End Property
        Public Property VitaminE() As Integer
            Get
                Return _VitaminE
            End Get
            Set(ByVal value As Integer)
                _VitaminE = value
            End Set
        End Property
        Public Property Thiamin() As Integer
            Get
                Return _Thiamin
            End Get
            Set(ByVal value As Integer)
                _Thiamin = value
            End Set
        End Property
        Public Property Riboflavin() As Integer
            Get
                Return _Riboflavin
            End Get
            Set(ByVal value As Integer)
                _Riboflavin = value
            End Set
        End Property
        Public Property Niacin() As Integer
            Get
                Return _Niacin
            End Get
            Set(ByVal value As Integer)
                _Niacin = value
            End Set
        End Property
        Public Property VitaminB6() As Integer
            Get
                Return _VitaminB6
            End Get
            Set(ByVal value As Integer)
                _VitaminB6 = value
            End Set
        End Property
        Public Property Folate() As Integer
            Get
                Return _Folate
            End Get
            Set(ByVal value As Integer)
                _Folate = value
            End Set
        End Property
        Public Property VitaminB12() As Integer
            Get
                Return _VitaminB12
            End Get
            Set(ByVal value As Integer)
                _VitaminB12 = value
            End Set
        End Property
        Public Property Biotin() As Integer
            Get
                Return _Biotin
            End Get
            Set(ByVal value As Integer)
                _Biotin = value
            End Set
        End Property
        Public Property PantothenicAcid() As Integer
            Get
                Return _PantothenicAcid
            End Get
            Set(ByVal value As Integer)
                _PantothenicAcid = value
            End Set
        End Property
        Public Property Phosphorous() As Integer
            Get
                Return _Phosphorous
            End Get
            Set(ByVal value As Integer)
                _Phosphorous = value
            End Set
        End Property
        Public Property Iodine() As Integer
            Get
                Return _Iodine
            End Get
            Set(ByVal value As Integer)
                _Iodine = value
            End Set
        End Property
        Public Property Magnesium() As Integer
            Get
                Return _Magnesium
            End Get
            Set(ByVal value As Integer)
                _Magnesium = value
            End Set
        End Property
        Public Property Zinc() As Integer
            Get
                Return _Zinc
            End Get
            Set(ByVal value As Integer)
                _Zinc = value
            End Set
        End Property
        Public Property Copper() As Integer
            Get
                Return _Copper
            End Get
            Set(ByVal value As Integer)
                _Copper = value
            End Set
        End Property
        Public Property Transfat() As Integer
            Get
                Return _Transfat
            End Get
            Set(ByVal value As Integer)
                _Transfat = value
            End Set
        End Property
        Public Property TransfatWeight() As Decimal
            Get
                Return _TransfatWeight
            End Get
            Set(ByVal value As Decimal)
                _TransfatWeight = value
            End Set
        End Property
        Public Property CaloriesFromTransFat() As Integer
            Get
                Return _CaloriesFromTransFat
            End Get
            Set(ByVal value As Integer)
                _CaloriesFromTransFat = value
            End Set
        End Property
        Public Property Om6Fatty() As Decimal
            Get
                Return _Om6Fatty
            End Get
            Set(ByVal value As Decimal)
                _Om6Fatty = value
            End Set
        End Property
        Public Property Om3Fatty() As Decimal
            Get
                Return _Om3Fatty
            End Get
            Set(ByVal value As Decimal)
                _Om3Fatty = value
            End Set
        End Property
        Public Property Starch() As Decimal
            Get
                Return _Starch
            End Get
            Set(ByVal value As Decimal)
                _Starch = value
            End Set
        End Property
        Public Property Chloride() As Integer
            Get
                Return _Chloride
            End Get
            Set(ByVal value As Integer)
                _Chloride = value
            End Set
        End Property
        Public Property Chromium() As Integer
            Get
                Return _Chromium
            End Get
            Set(ByVal value As Integer)
                _Chromium = value
            End Set
        End Property
        Public Property VitaminK() As Integer
            Get
                Return _VitaminK
            End Get
            Set(ByVal value As Integer)
                _VitaminK = value
            End Set
        End Property
        Public Property Manganese() As Integer
            Get
                Return _Manganese
            End Get
            Set(ByVal value As Integer)
                _Manganese = value
            End Set
        End Property
        Public Property Molybdenum() As Integer
            Get
                Return _Molybdenum
            End Get
            Set(ByVal value As Integer)
                _Molybdenum = value
            End Set
        End Property
        Public Property Selenium() As Integer
            Get
                Return _Selenium
            End Get
            Set(ByVal value As Integer)
                _Selenium = value
            End Set
        End Property
        Public Property ServingSizeDesc() As String
            Get
                Return _ServingSizeDesc
            End Get
            Set(ByVal value As String)
                _ServingSizeDesc = value
            End Set
        End Property


#End Region

        Public Sub New()
            ' initialize default value
            Me.ServingUnits = 1
            Me.ServingPerContainer = "VARIED"
            Me.ServingsPerPortion = 1
            Me.SizeWeight = 1
        End Sub
    End Class

End Namespace
