Imports WholeFoods.IRMA.Common.DataAccess

Public Enum AdjustSaleDataStatus
    Valid
    Error_Invalid_RegularMultiple 'must be greater than zero
    Error_Invalid_RegularPrice 'must be greater than zero
    Error_Required_SelectSale
    Error_Required_StoreList
    Error_StartDate_PastDate
    Error_StartDate_AfterSaleEndDate 'new effective date must be before previous sale_end_date
End Enum

Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    ''' <summary>
    ''' used to track business rules for 'End Sale Early' and 'Cancel All Sales" functionality
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AdjustSaleDataBO

        Private _itemKey As Integer
        Private _startDate As Date
        Private _previousSaleEndDate As Date  'Sale_End_Date value for sale being ended early
        Private _storeList As String
        Private _storeListSeparator As Char
        Private _userID As Integer  'ID of user making the change to the sale
        Private _userIDDate As String  'preserves time in format also in database down to milliseconds
        Private _posPrice As Decimal  'regular price
        Private _price As Decimal
        Private _regMultiple As Integer
        Private _priceBatchDetailIdToEndEarly As Integer = -1

#Region "properties"

        Public Property ItemKey() As Integer
            Get
                Return _itemKey
            End Get
            Set(ByVal value As Integer)
                _itemKey = value
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

        Public Property PreviousSaleEndDate() As Date
            Get
                Return _previousSaleEndDate
            End Get
            Set(ByVal value As Date)
                _previousSaleEndDate = value
            End Set
        End Property

        Public Property StoreList() As String
            Get
                Return _storeList
            End Get
            Set(ByVal value As String)
                _storeList = value
            End Set
        End Property

        Public Property StoreListSeparator() As Char
            Get
                Return _storeListSeparator
            End Get
            Set(ByVal value As Char)
                _storeListSeparator = value
            End Set
        End Property

        Public Property UserID() As Integer
            Get
                Return _userID
            End Get
            Set(ByVal value As Integer)
                _userID = value
            End Set
        End Property

        Public Property UserIDDate() As String
            Get
                Return _userIDDate
            End Get
            Set(ByVal value As String)
                _userIDDate = value
            End Set
        End Property

        Public Property POSPrice() As Decimal
            Get
                Return _posPrice
            End Get
            Set(ByVal value As Decimal)
                _posPrice = value
            End Set
        End Property

        Public Property Price() As Decimal
            Get
                Return _price
            End Get
            Set(ByVal value As Decimal)
                _price = value
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

        Public Property PriceBatchDetailIdToEndEarly() As Integer
            Get
                Return _priceBatchDetailIdToEndEarly
            End Get
            Set(ByVal value As Integer)
                _priceBatchDetailIdToEndEarly = value
            End Set
        End Property

#End Region

#Region "business rules"

        ''' <summary>
        ''' sets the Price field (not the POSPrice) field based on regional rules;  
        ''' UK region sets the price less VAT with the POSPrice inclusive of VAT
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetPrice(ByVal posPrice As Decimal)
            If InstanceDataDAO.IsFlagActive("UseVAT") Then
                'TODO: GET TAX RATE FOR THIS STORE/ITEM
                Dim taxRate As Decimal

                Dim rs As DAO.Recordset = Nothing
                SQLOpenRS(rs, "EXEC GetTaxRateForStore " & Me.ItemKey & ", " & Me.StoreList, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

                taxRate = rs.Fields(0).Value

                Me.Price = GetPriceWithoutVAT(posPrice, taxRate)
            Else
                Me.Price = posPrice
            End If
        End Sub

        Public Function ValidateEndSaleEarly() As ArrayList
            Dim statusList As New ArrayList

            'user must select a sale to end early
            If _priceBatchDetailIdToEndEarly < 0 Then
                statusList.Add(AdjustSaleDataStatus.Error_Required_SelectSale)
            End If

            'effective date must be greater than or equal to today
            If _startDate < System.DateTime.Today Then
                statusList.Add(AdjustSaleDataStatus.Error_StartDate_PastDate)
            End If

            'effective date must be less than (or equal to) the previous Sale_End_Date (ie: end the sale early)
            If _startDate > _previousSaleEndDate Then
                statusList.Add(AdjustSaleDataStatus.Error_StartDate_AfterSaleEndDate)
            End If

            'regular price must be greater than zero
            If _posPrice <= 0 Then
                statusList.Add(AdjustSaleDataStatus.Error_Invalid_RegularPrice)
            End If

            'regular multiple must be greater than zero
            If _regMultiple <= 0 Then
                statusList.Add(AdjustSaleDataStatus.Error_Invalid_RegularMultiple)
            End If

            If statusList.Count = 0 Then
                statusList.Add(AdjustSaleDataStatus.Valid)
            End If

            Return statusList
        End Function

        Public Function ValidateCancelAllSales() As ArrayList
            Dim statusList As New ArrayList

            'effective date must be greater than or equal to today
            If _startDate < System.DateTime.Today Then
                statusList.Add(AdjustSaleDataStatus.Error_StartDate_PastDate)
            End If

            'at least one store must be selected
            If _storeList Is Nothing OrElse _storeList.Equals("") Then
                statusList.Add(AdjustSaleDataStatus.Error_Required_StoreList)
            End If

            If statusList.Count = 0 Then
                statusList.Add(AdjustSaleDataStatus.Valid)
            End If

            Return statusList
        End Function

#End Region

#Region "validation"

        Public Shared Function IsIntegerValue(ByVal inValue As String) As Boolean
            Dim isInt As Boolean = False
            Dim tempInt As Integer

            Try
                tempInt = CType(inValue, Integer)
                isInt = True
            Catch ex As Exception
                isInt = False
            End Try

            Return isInt
        End Function

#End Region

    End Class

End Namespace