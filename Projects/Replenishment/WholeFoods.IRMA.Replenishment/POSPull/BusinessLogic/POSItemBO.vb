Imports System.Configuration
Imports System.Math

Namespace WholeFoods.IRMA.Replenishment.POSPull.BusinessLogic
    Public Class POSItemBO

        ' defaulting this to 1 was brought from old code - is this really necessary?
        Private _ItemKey As Integer = 1

        Private _Identifier As String
        Private _StoreNo As Integer
        Private _POSDescription As String
        Private _RetailSale As Boolean
        Private _FoodStamps As Boolean
        Private _Discountable As Boolean
        Private _IBMDiscount As Boolean
        Private _TaxFlag1 As Boolean
        Private _TaxFlag2 As Boolean
        Private _TaxFlag3 As Boolean
        Private _TaxFlag4 As Boolean
        Private _PriceRequired As Boolean
        Private _QuantityRequired As Boolean
        Private _RestrictedHours As Boolean
        Private _ItemTypeID As Integer
        Private _SubTeamNo As Integer
        Private _CasePrice As Decimal
        Private _PricingMethodID As Integer
        Private _UnitPrice As Decimal
        Private _DealQuantity As Integer
        Private _DealPrice As Decimal
        Private _ForPrice As Decimal
        Private _ForQuantity As Integer
        Private _LOADDate As Date
        Private _SoldByWeight As Boolean

#Region "Properties"
        Public Property ForPrice() As Decimal
            Get
                Return _ForPrice
            End Get
            Set(ByVal value As Decimal)
                _ForPrice = value
            End Set
        End Property
        Public Property ForQuantity() As Integer
            Get
                Return _ForQuantity
            End Get
            Set(ByVal value As Integer)
                _ForQuantity = value
            End Set
        End Property
        Public Property ItemKey() As Integer
            Get
                Return _ItemKey
            End Get
            Set(ByVal value As Integer)
                _ItemKey = value
            End Set
        End Property
        Public Property Identifier() As String
            Get
                Return _Identifier
            End Get
            Set(ByVal value As String)
                _Identifier = value
            End Set
        End Property
        Public Property StoreNo() As Integer
            Get
                Return _StoreNo
            End Get
            Set(ByVal value As Integer)
                _StoreNo = value
            End Set
        End Property
        Public Property POSDescription() As String
            Get
                Return _POSDescription
            End Get
            Set(ByVal value As String)
                _POSDescription = value
            End Set
        End Property
        Public Property RetailSale() As Boolean
            Get
                Return _RetailSale
            End Get
            Set(ByVal value As Boolean)
                _RetailSale = value
            End Set
        End Property
        Public Property FoodStamps() As Boolean
            Get
                Return _FoodStamps
            End Get
            Set(ByVal value As Boolean)
                _FoodStamps = value
            End Set
        End Property
        Public Property Discountable() As Boolean
            Get
                Return _Discountable
            End Get
            Set(ByVal value As Boolean)
                _Discountable = value
            End Set
        End Property
        Public Property IBMDiscount() As Boolean
            Get
                Return _IBMDiscount
            End Get
            Set(ByVal value As Boolean)
                _IBMDiscount = value
            End Set
        End Property
        Public Property TaxFlag1() As Boolean
            Get
                Return _TaxFlag1
            End Get
            Set(ByVal value As Boolean)
                _TaxFlag1 = value
            End Set
        End Property
        Public Property TaxFlag2() As Boolean
            Get
                Return _TaxFlag2
            End Get
            Set(ByVal value As Boolean)
                _TaxFlag2 = value
            End Set
        End Property
        Public Property TaxFlag3() As Boolean
            Get
                Return _TaxFlag3
            End Get
            Set(ByVal value As Boolean)
                _TaxFlag3 = value
            End Set
        End Property
        Public Property TaxFlag4() As Boolean
            Get
                Return _TaxFlag4
            End Get
            Set(ByVal value As Boolean)
                _TaxFlag4 = value
            End Set
        End Property
        Public Property PriceRequired() As Boolean
            Get
                Return _PriceRequired
            End Get
            Set(ByVal value As Boolean)
                _PriceRequired = value
            End Set
        End Property
        Public Property QuantityRequired() As Boolean
            Get
                Return _QuantityRequired
            End Get
            Set(ByVal value As Boolean)
                _QuantityRequired = value
            End Set
        End Property
        Public Property RestrictedHours() As Boolean
            Get
                Return _RestrictedHours
            End Get
            Set(ByVal value As Boolean)
                _RestrictedHours = value
            End Set
        End Property
        Public Property ItemTypeID() As Integer
            Get
                Return _ItemTypeID
            End Get
            Set(ByVal value As Integer)
                _ItemTypeID = value
            End Set
        End Property
        Public Property SubTeamNo() As Integer
            Get
                Return _SubTeamNo
            End Get
            Set(ByVal value As Integer)
                _SubTeamNo = value
            End Set
        End Property
        Public Property CasePrice() As Decimal
            Get
                Return _CasePrice
            End Get
            Set(ByVal value As Decimal)
                _CasePrice = value
            End Set
        End Property
        Public Property PricingMethodID() As Integer
            Get
                Return _PricingMethodID
            End Get
            Set(ByVal value As Integer)
                _PricingMethodID = value
            End Set
        End Property
        Public Property UnitPrice() As Decimal
            Get
                Return _UnitPrice
            End Get
            Set(ByVal value As Decimal)
                _UnitPrice = value
            End Set
        End Property
        Public Property DealQuantity() As Integer
            Get
                Return _DealQuantity
            End Get
            Set(ByVal value As Integer)
                _DealQuantity = value
            End Set
        End Property
        Public Property DealPrice() As Decimal
            Get
                Return _DealPrice
            End Get
            Set(ByVal value As Decimal)
                _DealPrice = value
            End Set
        End Property
        Public Property LOADDate() As Date
            Get
                Return _LOADDate
            End Get
            Set(ByVal value As Date)
                _LOADDate = value
            End Set
        End Property
        Public Property SoldByWeight() As Boolean
            Get
                Return _SoldByWeight
            End Get
            Set(ByVal value As Boolean)
                _SoldByWeight = value
            End Set
        End Property

        Public ReadOnly Property ToIBMFileString(ByVal StoreNo As Integer, ByVal FileDate As String) As String
            Get
                ' tax plans are not used and places are held with zeros (0)
                Return Me.ItemKey & vbTab & _
                Me.Identifier & vbTab & _
                StoreNo & vbTab & _
                Me.POSDescription & vbTab & _
                Abs(CInt(Me.RetailSale)) & vbTab & _
                Abs(CInt(Me.FoodStamps)) & vbTab & _
                Abs(CInt(Me.Discountable)) & vbTab & _
                Abs(CInt(Me.IBMDiscount)) & vbTab & _
                Abs(CInt(Me.TaxFlag1)) & vbTab & _
                Abs(CInt(Me.TaxFlag2)) & vbTab & _
                Abs(CInt(Me.TaxFlag3)) & vbTab & _
                Abs(CInt(Me.TaxFlag4)) & vbTab & _
                Abs(CInt(Me.PriceRequired)) & vbTab & _
                Abs(CInt(Me.QuantityRequired)) & vbTab & _
                Abs(CInt(Me.RestrictedHours)) & vbTab & _
                Me.ItemTypeID & vbTab & _
                Me.SubTeamNo & vbTab & _
                Me.CasePrice & vbTab & _
                Me.PricingMethodID & vbTab & _
                Me.UnitPrice & vbTab & _
                Me.DealQuantity & vbTab & _
                Me.DealPrice & vbTab & _
                FileDate & vbTab & _
                Abs(CInt(Me.SoldByWeight))

            End Get
        End Property
        Public ReadOnly Property ToNCRFileString(ByVal StoreNo As Integer, ByVal FileDate As String) As String
            Get
                Select Case Me.PricingMethodID
                    Case 0
                        ' Fixed Price
                        'Me.UnitPrice = Me.UnitPrice
                    Case 1
                        ' Unit Price with Rounding
                        ' majority are this one
                        If Me.ForQuantity = 0 Then Me.ForQuantity = 1
                        Me.UnitPrice = Me.UnitPrice / Me.ForQuantity
                    Case 2
                        ' Split Package Pricing
                        'Me.UnitPrice = Me.UnitPrice
                    Case 5
                        ' Unlimited Quantity Discount
                        'Me.UnitPrice = Me.UnitPrice
                        ' me.DealPrice, me.DealQuantity
                End Select

                Return Me.ItemKey & vbTab & _
                Me.Identifier & vbTab & _
                StoreNo & vbTab & _
                Me.POSDescription & vbTab & _
                Abs(CInt(Me.RetailSale)) & vbTab & _
                Abs(CInt(Me.FoodStamps)) & vbTab & _
                Abs(CInt(Me.Discountable)) & vbTab & _
                Abs(CInt(Me.IBMDiscount)) & vbTab & _
                Abs(CInt(Me.TaxFlag1)) & vbTab & _
                Abs(CInt(Me.TaxFlag2)) & vbTab & _
                Abs(CInt(Me.TaxFlag3)) & vbTab & _
                Abs(CInt(Me.TaxFlag4)) & vbTab & _
                Abs(CInt(Me.PriceRequired)) & vbTab & _
                Abs(CInt(Me.QuantityRequired)) & vbTab & _
                Abs(CInt(Me.RestrictedHours)) & vbTab & _
                Me.ItemTypeID & vbTab & _
                Me.SubTeamNo & vbTab & _
                Me.CasePrice & vbTab & _
                Me.PricingMethodID & vbTab & _
                Me.UnitPrice & vbTab & _
                Me.DealQuantity & vbTab & _
                Me.DealPrice & vbTab & _
                FileDate & vbTab & _
                Abs(CInt(Me.SoldByWeight))

            End Get
        End Property

#End Region

        ''' <summary>
        ''' this method blanks out all of the properties for the current instance of this object
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearPropertyValues()
            _Identifier = Nothing
            _StoreNo = -1
            _POSDescription = Nothing
            _RetailSale = False
            _FoodStamps = False
            _Discountable = False
            _IBMDiscount = False
            _TaxFlag1 = False
            _TaxFlag2 = False
            _TaxFlag3 = False
            _TaxFlag4 = False
            _PriceRequired = False
            _QuantityRequired = False
            _RestrictedHours = False
            _ItemTypeID = 0
            _SubTeamNo = 0
            _CasePrice = 0
            _PricingMethodID = 0
            _UnitPrice = 0
            _DealQuantity = 0
            _DealPrice = 0
            _ForPrice = 0
            _ForQuantity = 0
            _LOADDate = Nothing
            _SoldByWeight = False
        End Sub


    End Class
End Namespace

