Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.Pricing.DataAccess

Public Enum PriceChangeStatus
    Valid
    Error_RegMultipleGreaterZero
    Error_RegPriceGreaterEqualZero
    Error_RegPriceGreaterZero
    Error_RegStartDateInPast
    Error_RegPriceChgTypeIDRequired
    Error_MissingPrimaryVendor
    Error_SaleMultipleGreaterZero
    Error_SalePriceGreaterEqualZero
    Error_SalePriceGreaterZero
    Error_SalePriceMustEqualZero
    Error_SaleStartAndEndDatesRequired
    Error_SaleStartDateInPast
    Error_SaleStartDateGreaterMaxDBDate
    Error_SaleEndDateAfterSaleStartDate
    Error_SaleEndDateGreaterMaxDBDate
    Error_MSRPMultipleGreaterZero
    Error_MSRPPriceGreaterZero
    Error_PriceQuantityGreaterZero
    Error_SalePriceLimitGreaterZero
    Error_SaleWithPriceChangeInBatch
    Error_ExistingPromoWithSameStartDate
    Error_SalePriceChgTypeIDRequired
    Error_SalePricingMethodRequired
    Error_Unknown
    Warning_RegConflictsWithRegPriceChange
    Warning_RegConflictsWithSalePriceChange
    Warning_RegWithSaleCurrentlyOngoing
    Warning_RegWithPriceChangeInBatch
    Warning_SaleConflictsWithRegPriceChange
    Warning_SaleConflictsWithSalePriceChange
    Warning_SaleCurrentlyOngoing
End Enum

Namespace WholeFoods.IRMA.Pricing.BusinessLogic
    Public Class PriceChangeBO

        Private _itemKey As Integer
        Private _storeNo As Integer
        Private _storeName As String
        Private _userId As Integer
        Private _userIdDate As Date
        Private _priceChgType As PriceChgTypeBO
        Private _startDate As Date
        Private _regMultiple As Integer
        Private _regPrice As Decimal
        Private _regPOSPrice As Decimal
        Private _oldStartDate As Date
        Private _MSRPPrice As Decimal
        Private _MSRPMultiple As Integer
        Private _pricingMethodId As Integer
        Private _saleMultiple As Integer
        Private _salePrice As Decimal
        Private _salePOSPrice As Decimal
        Private _saleEndDate As Date
        Private _sale_EarnedDisc1 As Integer
        Private _sale_EarnedDisc2 As Integer
        Private _sale_EarnedDisc3 As Integer
        Private _priceBatchDetailId As Integer
        Private _lineDrive As Boolean
        Private _insertApplication As String
        Private _errorDescription As String

#Region "Property Access Methods"
        Public Property ItemKey() As Integer
            Get
                Return _itemKey
            End Get
            Set(ByVal value As Integer)
                _itemKey = value
            End Set
        End Property

        Public Property StoreNo() As Integer
            Get
                Return _storeNo
            End Get
            Set(ByVal value As Integer)
                _storeNo = value
            End Set
        End Property

        Public Property StoreName() As String
            Get
                Return _storeName
            End Get
            Set(ByVal value As String)
                _storeName = value
            End Set
        End Property

        Public Property UserId() As Integer
            Get
                Return _userId
            End Get
            Set(ByVal value As Integer)
                _userId = value
            End Set
        End Property

        Public Property UserIdDate() As Date
            Get
                Return _userIdDate
            End Get
            Set(ByVal value As Date)
                _userIdDate = value
            End Set
        End Property

        Public Property PriceChgType() As PriceChgTypeBO
            Get
                Return _priceChgType
            End Get
            Set(ByVal value As PriceChgTypeBO)
                _priceChgType = value
            End Set
        End Property

        Public Property StartDate() As Date
            Get
                Return _startDate
            End Get
            Set(ByVal value As Date)
                _startDate = value
            End Set
        End Property

        Public Property RegMultiple() As Integer
            Get
                Return _regMultiple
            End Get
            Set(ByVal value As Integer)
                _regMultiple = value
            End Set
        End Property

        Public Property RegPrice() As Decimal
            Get
                Return _regPrice
            End Get
            Set(ByVal value As Decimal)
                _regPrice = value
            End Set
        End Property

        Public Property RegPOSPrice() As Decimal
            Get
                Return _regPOSPrice
            End Get
            Set(ByVal value As Decimal)
                _regPOSPrice = value
            End Set
        End Property

        Public Property OldStartDate() As Date
            Get
                Return _oldStartDate
            End Get
            Set(ByVal value As Date)
                _oldStartDate = value
            End Set
        End Property

        Public Property MSRPPrice() As Decimal
            Get
                Return _MSRPPrice
            End Get
            Set(ByVal value As Decimal)
                _MSRPPrice = value
            End Set
        End Property

        Public Property MSRPMultiple() As Integer
            Get
                Return _MSRPMultiple
            End Get
            Set(ByVal value As Integer)
                _MSRPMultiple = value
            End Set
        End Property

        Public Property PricingMethodId() As Integer
            Get
                Return _pricingMethodId
            End Get
            Set(ByVal value As Integer)
                _pricingMethodId = value
            End Set
        End Property

        Public Property SaleMultiple() As Integer
            Get
                Return _saleMultiple
            End Get
            Set(ByVal value As Integer)
                _saleMultiple = value
            End Set
        End Property

        Public Property SalePrice() As Decimal
            Get
                Return _salePrice
            End Get
            Set(ByVal value As Decimal)
                _salePrice = value
            End Set
        End Property

        Public Property SalePOSPrice() As Decimal
            Get
                Return _salePOSPrice
            End Get
            Set(ByVal value As Decimal)
                _salePOSPrice = value
            End Set
        End Property

        Public Property SaleEndDate() As Date
            Get
                Return _saleEndDate
            End Get
            Set(ByVal value As Date)
                _saleEndDate = value
            End Set
        End Property

        Public Property Sale_EarnedDisc1() As Integer
            Get
                Return _sale_EarnedDisc1
            End Get
            Set(ByVal value As Integer)
                _sale_EarnedDisc1 = value
            End Set
        End Property

        Public Property Sale_EarnedDisc2() As Integer
            Get
                Return _sale_EarnedDisc2
            End Get
            Set(ByVal value As Integer)
                _sale_EarnedDisc2 = value
            End Set
        End Property

        Public Property Sale_EarnedDisc3() As Integer
            Get
                Return _sale_EarnedDisc3
            End Get
            Set(ByVal value As Integer)
                _sale_EarnedDisc3 = value
            End Set
        End Property

        Public Property PriceBatchDetailId() As Integer
            Get
                Return _priceBatchDetailId
            End Get
            Set(ByVal value As Integer)
                _priceBatchDetailId = value
            End Set
        End Property

        Public Property LineDrive() As Boolean
            Get
                Return _lineDrive
            End Get
            Set(ByVal value As Boolean)
                _lineDrive = value
            End Set
        End Property

        Public Property InsertApplication() As String
            Get
                Return _insertApplication
            End Get
            Set(ByVal value As String)
                _insertApplication = value
            End Set
        End Property

        Public Property ErrorDescription() As String
            Get
                Return _errorDescription
            End Get
            Set(ByVal value As String)
                _errorDescription = value
            End Set
        End Property

#End Region

#Region "Business Rules"
        ''' <summary>
        ''' This function maps the validation codes returned by the database to PriceChangeStatus values that are
        ''' used by the IRMA UI screens.
        ''' </summary>
        ''' <param name="validationCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function MapPriceChangeValidationCode(ByVal validationCode As Integer) As PriceChangeStatus
            Dim status As PriceChangeStatus
            Select Case validationCode
                Case 0
                    status = PriceChangeStatus.Valid
                Case 100
                    status = PriceChangeStatus.Error_RegMultipleGreaterZero
                Case 101
                    status = PriceChangeStatus.Error_RegPriceGreaterZero
                Case 102
                    status = PriceChangeStatus.Error_RegStartDateInPast
                Case 103
                    status = PriceChangeStatus.Error_MissingPrimaryVendor
                Case 104
                    status = PriceChangeStatus.Error_RegPriceChgTypeIDRequired
                Case 105
                    status = PriceChangeStatus.Warning_RegConflictsWithRegPriceChange
                Case 106
                    status = PriceChangeStatus.Warning_RegConflictsWithSalePriceChange
                Case 107
                    status = PriceChangeStatus.Warning_RegWithSaleCurrentlyOngoing
                Case 108
                    status = PriceChangeStatus.Warning_RegWithPriceChangeInBatch
                Case 200
                    status = PriceChangeStatus.Error_RegMultipleGreaterZero
                Case 201
                    status = PriceChangeStatus.Error_RegPriceGreaterZero
                Case 202
                    status = PriceChangeStatus.Error_SaleMultipleGreaterZero
                Case 203
                    status = PriceChangeStatus.Error_SalePriceGreaterZero
                Case 204
                    status = PriceChangeStatus.Error_SaleStartDateInPast
                Case 205
                    status = PriceChangeStatus.Error_SaleEndDateAfterSaleStartDate
                Case 206
                    status = PriceChangeStatus.Error_MSRPMultipleGreaterZero
                Case 207
                    status = PriceChangeStatus.Error_MSRPPriceGreaterZero
                Case 208
                    status = PriceChangeStatus.Error_PriceQuantityGreaterZero
                Case 209
                    status = PriceChangeStatus.Error_SalePriceLimitGreaterZero
                Case 210
                    status = PriceChangeStatus.Error_SaleWithPriceChangeInBatch
                Case 211
                    status = PriceChangeStatus.Error_ExistingPromoWithSameStartDate
                Case 212
                    status = PriceChangeStatus.Error_MissingPrimaryVendor
                Case 213
                    status = PriceChangeStatus.Error_SalePriceChgTypeIDRequired
                Case 214
                    status = PriceChangeStatus.Warning_SaleConflictsWithRegPriceChange
                Case 215
                    status = PriceChangeStatus.Warning_SaleConflictsWithSalePriceChange
                Case 216
                    status = PriceChangeStatus.Warning_SaleCurrentlyOngoing
                Case Else
                    status = PriceChangeStatus.Error_Unknown
            End Select
            Return status
        End Function

        ''' <summary>
        ''' This function duplicates some of the logic that is performed in the database function,
        ''' called by fn_ValidatePromoPriceChange.
        ''' It also appears in it's own function for performance reasons.  The client should not have
        ''' to communicate with the database to perform validation for required fields and data formatting
        ''' checks.
        ''' These are data validation messages that are common to all stores, such as missing a 
        ''' required field or impropertly formatted data.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidatePromoPriceChangeDataFormatting() As PriceChangeStatus
            Dim status As PriceChangeStatus

            ' Make sure that all the data necessary for a promotion was entered.  This is performing all of the
            ' validation that can happen without querying the database.
            If _regMultiple < 1 Then
                status = PriceChangeStatus.Error_RegMultipleGreaterZero
            ElseIf InstanceDataDAO.IsFlagActive("AllowZeroRegPrice") AndAlso _regPOSPrice < 0 Then
                status = PriceChangeStatus.Error_RegPriceGreaterEqualZero
            ElseIf Not InstanceDataDAO.IsFlagActive("AllowZeroRegPrice") AndAlso _regPOSPrice <= 0 Then
                status = PriceChangeStatus.Error_RegPriceGreaterZero
            ElseIf _saleMultiple < 1 Then
                status = PriceChangeStatus.Error_SaleMultipleGreaterZero
            ElseIf InstanceDataDAO.IsFlagActive("AllowZeroSalePrice") AndAlso _salePOSPrice < 0 Then
                status = PriceChangeStatus.Error_SalePriceGreaterEqualZero
            ElseIf Not InstanceDataDAO.IsFlagActive("AllowZeroSalePrice") AndAlso _salePOSPrice <= 0 Then
                status = PriceChangeStatus.Error_SalePriceGreaterZero
            ElseIf (_startDate = System.DateTime.MinValue) Or (_saleEndDate = System.DateTime.MinValue) Then
                status = PriceChangeStatus.Error_SaleStartAndEndDatesRequired
            ElseIf _startDate < System.DateTime.Today Then
                status = PriceChangeStatus.Error_SaleStartDateInPast
            ElseIf _startDate > ResourcesPricing.MaxSmalldatetimeDate Then
                'START DATE max value: the underlying db datatype for the date column is smallDateTime which has a maximum date value of 06/06/2079
                status = PriceChangeStatus.Error_SaleStartDateGreaterMaxDBDate
            ElseIf _saleEndDate < _startDate Then
                status = PriceChangeStatus.Error_SaleEndDateAfterSaleStartDate
            ElseIf _saleEndDate > ResourcesPricing.MaxSmalldatetimeDate Then
                'END DATE max value: the underlying db datatype for the date column is smallDateTime which has a maximum date value of 06/06/2079
                status = PriceChangeStatus.Error_SaleEndDateGreaterMaxDBDate
            ElseIf _pricingMethodId = -1 Then
                status = PriceChangeStatus.Error_SalePricingMethodRequired
            ElseIf _priceChgType Is Nothing Then
                status = PriceChangeStatus.Error_SalePriceChgTypeIDRequired
            ElseIf _priceChgType.IsMSRPRequired AndAlso _MSRPMultiple < 1 Then
                status = PriceChangeStatus.Error_MSRPMultipleGreaterZero
            ElseIf _priceChgType.IsMSRPRequired AndAlso _MSRPPrice <= 0 Then
                status = PriceChangeStatus.Error_MSRPPriceGreaterZero
            End If

            ' IMPLEMENT THESE ***************************************
            'If CDbl(cmbPricingMethod.SelectedValue) = 2 And CShort(IIf(Len(txtField(iPriceEarnedDiscount1).Text) > 0, txtField(iPriceEarnedDiscount1).Text, 0)) < 1 Then
            '    MsgBox(ResourcesItemHosting.GetString("QuantityRegularPriceNotZero"), MsgBoxStyle.Critical, Me.Text)
            '    txtField(iPriceEarnedDiscount1).Focus()
            '    Exit Function
            'End If

            'If CShort(IIf(Len(Trim(txtField(iPriceEarnedDiscount3).Text)) = 0, ResourcesIRMA.GetString("Zero"), txtField(iPriceEarnedDiscount3).Text)) < 1 Then
            '    MsgBox(ResourcesItemHosting.GetString("EarnedSaleLimitNotZero"), MsgBoxStyle.Critical, Me.Text)
            '    txtField(iPriceEarnedDiscount3).Focus()
            '    Exit Function
            'End If

            Return status
        End Function

         ''' <summary>
        ''' This function is used to validate the price change data for a promotion price.
        ''' It queries a common database function that is shared by all of the IRMA applications
        ''' to ensure the price change logic is consistent.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidatePromoPriceChangeData() As PriceChangeStatus
            Dim status As PriceChangeStatus
            ' Call the common price validation function for promo price changes
            Dim validationCode As Integer = PriceBatchDetailDAO.ValidatePromoPriceChange(Me)
            status = MapPriceChangeValidationCode(validationCode)
            Return status
        End Function

        ''' <summary>
        ''' This function will save the updates for the promotional price change to the database.
        ''' It returns the validation code based on the promotional price change validation logic.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SavePromoPriceChange() As Integer
            Dim validationCode As Integer = PriceBatchDetailDAO.SavePromoPriceChange(Me)
            Return validationCode
        End Function

        ''' <summary>
        ''' This function duplicates some of the logic that is performed in the database function,
        ''' called by fn_ValidateRegularPriceChange.
        ''' It also appears in it's own function for performance reasons.  The client should not have
        ''' to communicate with the database to perform validation for required fields and data formatting
        ''' checks.
        ''' These are data validation messages that are common to all stores, such as missing a 
        ''' required field or impropertly formatted data.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateRegularPriceChangeDataFormatting() As PriceChangeStatus
            Dim status As PriceChangeStatus

            ' Make sure that all the data necessary for a promotion was entered.  This is performing all of the
            ' validation that can happen without querying the database.
            If _regMultiple < 1 Then
                status = PriceChangeStatus.Error_RegMultipleGreaterZero
            ElseIf InstanceDataDAO.IsFlagActive("AllowZeroRegPrice") AndAlso _regPOSPrice < 0 Then
                status = PriceChangeStatus.Error_RegPriceGreaterEqualZero
            ElseIf Not InstanceDataDAO.IsFlagActive("AllowZeroRegPrice") AndAlso _regPOSPrice <= 0 Then
                status = PriceChangeStatus.Error_RegPriceGreaterZero
            ElseIf (_startDate = System.DateTime.MinValue) Then
                status = PriceChangeStatus.Error_RegStartDateInPast
            ElseIf _startDate < System.DateTime.Today Then
                status = PriceChangeStatus.Error_RegStartDateInPast
            End If
            Return status
        End Function

        ''' <summary>
        ''' This function is used to validate the price change data for a regular price.
        ''' It queries a common database function that is shared by all of the IRMA applications
        ''' to ensure the price change logic is consistent.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateRegularPriceChangeData() As PriceChangeStatus
            Dim status As PriceChangeStatus
            ' Call the common price validation function for promo price changes
            Dim validationCode As Integer = PriceBatchDetailDAO.ValidateRegularPriceChange(Me)
            status = MapPriceChangeValidationCode(validationCode)
            Return status
        End Function

        ''' <summary>
        ''' This function will save the updates for the regular price change to the database.
        ''' It returns the validation code based on the regular price change validation logic.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SaveRegularPriceChange() As Integer
            Dim validationCode As Integer = PriceBatchDetailDAO.SaveRegularPriceChange(Me)
            Return validationCode
        End Function

#End Region

    End Class
End Namespace

