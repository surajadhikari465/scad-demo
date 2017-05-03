Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Enum POSItemStatus
        Valid
        Error_GroupListNotIntegerFormat
        Error_QtyRequiredAndProhibitBothTrue
        Error_IceTareNotIntegerFormat
        Error_MiscTransSaleNotIntegerFormat
        Error_MiscTransRefundNotIntegerFormat
        Error_UnitPriceCategoryNotIntegerFormat
    End Enum

    Public Class POSItemBO

        ' Some integer vales will be stored as strings so that null values can be handled more easily.
        Private _itemKey As Integer
        Private _storeJurisdictionID As String
        Private _storeJurisdictionDesc As String
        Private _priceRequired As Boolean
        Private _quantityRequired As Boolean
        Private _foodStamps As Boolean
        Private _quantityProhibit As Boolean
        Private _groupList As String
        Private _caseDiscount As Boolean
        Private _couponMultiplier As Boolean
        Private _fsaEligible As Boolean
        Private _miscTransSale As String
        Private _miscTransRefund As String
        Private _ageRestrict As Boolean
        Private _iceTare As String
        Private _productCode As String
        Private _unitPricecategory As String
        Private _isValidated As String
        Private _hasIngredientIdentifier As String

#Region "Property Access Methods"

        Public Property ItemKey() As Integer
            Get
                Return _itemKey
            End Get
            Set(ByVal value As Integer)
                _itemKey = value
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

        Public Property PriceRequired() As Boolean
            Get
                Return _priceRequired
            End Get
            Set(ByVal value As Boolean)
                _priceRequired = value
            End Set
        End Property

        Public Property QuantityRequired() As Boolean
            Get
                Return _quantityRequired
            End Get
            Set(ByVal value As Boolean)
                _quantityRequired = value
            End Set
        End Property

        Public Property FoodStamps() As Boolean
            Get
                Return _foodStamps
            End Get
            Set(ByVal value As Boolean)
                _foodStamps = value
            End Set
        End Property

        Public Property QuantityProhibit() As Boolean
            Get
                Return _quantityProhibit
            End Get
            Set(ByVal value As Boolean)
                _quantityProhibit = value
            End Set
        End Property

        Public Property GroupList() As String
            Get
                Return _groupList
            End Get
            Set(ByVal value As String)
                _groupList = value
            End Set
        End Property


        Public Property CaseDiscount() As Boolean
            Get
                Return _caseDiscount
            End Get
            Set(ByVal value As Boolean)
                _caseDiscount = value
            End Set
        End Property

        Public Property AgeRestrict() As Boolean
            Get
                Return _ageRestrict
            End Get
            Set(ByVal value As Boolean)
                _ageRestrict = value
            End Set
        End Property

        Public Property CouponMultiplier() As Boolean
            Get
                Return _couponMultiplier
            End Get
            Set(ByVal value As Boolean)
                _couponMultiplier = value
            End Set
        End Property

        Public Property FSAEligible() As Boolean
            Get
                Return _fsaEligible
            End Get
            Set(ByVal value As Boolean)
                _fsaEligible = value
            End Set
        End Property

        Public Property MiscTransactionSale() As String
            Get
                Return _miscTransSale
            End Get
            Set(ByVal value As String)
                _miscTransSale = value
            End Set
        End Property

        Public Property MiscTransactionRefund() As String
            Get
                Return _miscTransRefund
            End Get
            Set(ByVal value As String)
                _miscTransRefund = value
            End Set
        End Property

        Public Property IceTare() As String
            Get
                Return _iceTare
            End Get
            Set(ByVal value As String)
                _iceTare = value
            End Set
        End Property

        Public Property ProductCode() As String
            Get
                Return _productCode
            End Get
            Set(ByVal value As String)
                _productCode = value
            End Set
        End Property

        Public Property UnitPriceCategory() As String
            Get
                Return _unitPricecategory
            End Get
            Set(ByVal value As String)
                _unitPricecategory = value
            End Set
        End Property

        Public Property IsValidated() As Boolean
            Get
                Return _isValidated
            End Get
            Set(ByVal value As Boolean)
                _isValidated = value
            End Set
        End Property

        Public Property HasIngredientIdentifier() As Boolean
            Get
                Return _hasIngredientIdentifier
            End Get
            Set(ByVal value As Boolean)
                _hasIngredientIdentifier = value
            End Set
        End Property

#End Region

        Public Function ValidateData() As ArrayList
            Dim statusList As New ArrayList

            '_quantityRequired and _quantityProhibit can not both be TRUE
            If _quantityRequired AndAlso _quantityProhibit Then
                statusList.Add(POSItemStatus.Error_QtyRequiredAndProhibitBothTrue)
            End If

            ' verify the group list is an integer, if it is entered
            If _groupList IsNot Nothing AndAlso _groupList <> "" Then
                Try
                    Dim tempInt As Integer = CInt(_groupList)
                    'validate that value is >= 0
                    If ScaleBO.ValidatePositiveNumber(tempInt, True) = False Then
                        statusList.Add(POSItemStatus.Error_GroupListNotIntegerFormat)
                    Else
                        'validate the user did not enter decimal values
                        If _groupList.IndexOf(".") >= 0 Then
                            statusList.Add(POSItemStatus.Error_GroupListNotIntegerFormat)
                        End If
                    End If
                Catch ex As Exception
                    ' error - the conversion to int failed
                    statusList.Add(POSItemStatus.Error_GroupListNotIntegerFormat)
                End Try
            End If

            ' verify the ice tare is an integer, if it is entered
            If _iceTare IsNot Nothing AndAlso _iceTare <> "" Then
                Try
                    Dim tempInt As Integer = CInt(_iceTare)
                    'validate that value is >= 0
                    If ScaleBO.ValidatePositiveNumber(tempInt, True) = False Then
                        statusList.Add(POSItemStatus.Error_IceTareNotIntegerFormat)
                    Else
                        'validate the user did not enter decimal values
                        If _iceTare.IndexOf(".") >= 0 Then
                            statusList.Add(POSItemStatus.Error_IceTareNotIntegerFormat)
                        End If
                    End If
                Catch ex As Exception
                    ' error - the conversion to int failed
                    statusList.Add(POSItemStatus.Error_IceTareNotIntegerFormat)
                End Try
            End If

            ' ensure MiscTransSale is a numeric value
            If _miscTransSale IsNot Nothing AndAlso _miscTransSale <> "" Then
                If Not ScaleBO.ValidateNumericValue(_miscTransSale) Then
                    statusList.Add(POSItemStatus.Error_MiscTransSaleNotIntegerFormat)
                End If
            End If

            ' verify the Unit Price category is an integer, if entered
            If _unitPricecategory IsNot Nothing AndAlso _unitPricecategory <> "" Then
                Try
                    Dim tempInt As Integer = CInt(_unitPricecategory)
                    'validate that value is >= 0
                    If ScaleBO.ValidatePositiveNumber(tempInt, True) = False Then
                        statusList.Add(POSItemStatus.Error_UnitPriceCategoryNotIntegerFormat)
                    Else
                        'validate the user did not enter decimal values
                        If _unitPricecategory.IndexOf(".") >= 0 Then
                            statusList.Add(POSItemStatus.Error_UnitPriceCategoryNotIntegerFormat)
                        End If
                    End If
                Catch ex As Exception
                    ' error - the conversion to int failed
                    statusList.Add(POSItemStatus.Error_UnitPriceCategoryNotIntegerFormat)
                End Try
            End If

            ' ensure MiscTransRefund is a numeric value
            If _miscTransRefund IsNot Nothing AndAlso _miscTransRefund <> "" Then
                If Not ScaleBO.ValidateNumericValue(_miscTransRefund) Then
                    statusList.Add(POSItemStatus.Error_MiscTransRefundNotIntegerFormat)
                End If
            End If

            If statusList.Count <= 0 Then
                statusList.Add(POSItemStatus.Valid)
            End If

            Return statusList
        End Function

    End Class

End Namespace
