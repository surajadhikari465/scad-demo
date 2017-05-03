Imports Infragistics.Win.UltraWinGrid
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports log4net

Public Enum VendorDealStatus
    Valid
    Error_CaseQty_MustBeWhole
    Error_Conflict_NotStackable
    Error_DataRange_CaseAmt 'must be within smallmoney parameters
    Error_EndDate_PastDate
    Error_Required_CaseQty 'must be greater than zero
    Error_Required_CaseAmt 'must be greater than zero
    Error_Required_CostPromoCode
    Error_Required_StoreSelection
    Error_Required_VendorDealType
    Error_StartDate_PastDate
End Enum

Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Class VendorDealBO

        Private _vendorDealHistoryID As Integer
        Private _storeItemVendorID As Integer
        Private _storeList As String
        Private _storeListSeparator As Char
        Private _itemKey As Integer
        Private _vendorID As Integer
        Private _caseQty As Integer
        Private _packageDesc1 As Decimal
        Private _caseAmt As Decimal
        Private _caseAmtType As String
        Private _startDate As Date
        Private _endDate As Date
        Private _dealTypeBO As VendorDealTypeBO
        Private _fromVendor As Boolean
        Private _costPromoBO As CostPromoCodeTypeBO
        Private _notStackable As Boolean

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


#Region "properties"

        Public Property VendorDealHistoryID() As Integer
            Get
                Return _vendorDealHistoryID
            End Get
            Set(ByVal value As Integer)
                _vendorDealHistoryID = value
            End Set
        End Property

        Public Property StoreItemVendorID() As Integer
            Get
                Return _storeItemVendorID
            End Get
            Set(ByVal value As Integer)
                _storeItemVendorID = value
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

        Public Property ItemKey() As Integer
            Get
                Return _itemKey
            End Get
            Set(ByVal value As Integer)
                _itemKey = value
            End Set
        End Property

        Public Property VendorID() As Integer
            Get
                Return _vendorID
            End Get
            Set(ByVal value As Integer)
                _vendorID = value
            End Set
        End Property

        Public Property CaseQty() As Integer
            Get
                Return _caseQty
            End Get
            Set(ByVal value As Integer)
                _caseQty = value
            End Set
        End Property

        Public Property PackageDesc1() As Decimal
            Get
                Return _packageDesc1
            End Get
            Set(ByVal value As Decimal)
                _packageDesc1 = value
            End Set
        End Property

        Public Property CaseAmt() As Decimal
            Get
                Return _caseAmt
            End Get
            Set(ByVal value As Decimal)
                _caseAmt = value
            End Set
        End Property

        Public Property CaseAmtType() As String
            Get
                Return _caseAmtType
            End Get
            Set(ByVal value As String)
                _caseAmtType = value
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

        Public Property EndDate() As Date
            Get
                Return _endDate
            End Get
            Set(ByVal value As Date)
                _endDate = value
            End Set
        End Property

        Public Property DealTypeBO() As VendorDealTypeBO
            Get
                Return _dealTypeBO
            End Get
            Set(ByVal value As VendorDealTypeBO)
                _dealTypeBO = value
            End Set
        End Property

        Public Property IsFromVendor() As Boolean
            Get
                Return _fromVendor
            End Get
            Set(ByVal value As Boolean)
                _fromVendor = value
            End Set
        End Property

        Public Property CostPromoBO() As CostPromoCodeTypeBO
            Get
                Return _costPromoBO
            End Get
            Set(ByVal value As CostPromoCodeTypeBO)
                _costPromoBO = value
            End Set
        End Property

        Public Property NotStackable() As Boolean
            Get
                Return _notStackable
            End Get
            Set(ByVal value As Boolean)
                _notStackable = value
            End Set
        End Property

#End Region

#Region "constructors"

        Public Sub New()

        End Sub

        Public Sub New(ByVal row As UltraGridRow)
            _vendorDealHistoryID = CType(row.Cells("VendorDealHistoryID").Value, Integer)
            _itemKey = CType(row.Cells("ItemKey").Value, Integer)
            _vendorID = CType(row.Cells("VendorID").Value, Integer)
            _caseQty = CType(row.Cells("CaseQty").Value, Integer)
            _packageDesc1 = CType(row.Cells("PackageDesc1").Value, Decimal)
            _caseAmt = CType(row.Cells("CaseAmt").Value, Decimal)
            _caseAmtType = row.Cells("CaseAmtType").Value.ToString
            _startDate = CType(row.Cells("StartDate").Value, Date)
            _endDate = CType(row.Cells("EndDate").Value, Date)
            _notStackable = CType(row.Cells("NotStackable").Value, Boolean)

            Dim vendorDealType As New VendorDealTypeBO
            vendorDealType.VendorDealTypeID = CType(row.Cells("VendorDealTypeID").Value, Integer)
            vendorDealType.VendorDealTypeCode = row.Cells("VendorDealTypeCode").Value.ToString
            vendorDealType.VendorDealTypeDesc = row.Cells("VendorDealTypeDesc").Value.ToString
            Me.DealTypeBO = vendorDealType

            Dim costPromoCodeType As New CostPromoCodeTypeBO
            costPromoCodeType.CostPromoCodeTypeID = CType(row.Cells("CostPromoCodeTypeID").Value, Integer)
            costPromoCodeType.CostPromoCode = CType(row.Cells("CostPromoCode").Value, Integer)
            costPromoCodeType.CostPromoDesc = row.Cells("CostPromoDesc").Value.ToString
            Me.CostPromoBO = costPromoCodeType
        End Sub

#End Region

#Region "business rules"

        Public Function ValidateNumericValue(ByVal stringIn As String) As Boolean

            logger.Debug("ValidateNumericValue Entry")


            Dim valid As Boolean = False
            Try
                'if value present then validate that it's a number
                If stringIn IsNot Nothing AndAlso Not stringIn.Trim.Equals("") AndAlso IsNumeric(stringIn) Then
                    valid = True
                End If
            Catch ex As Exception
                valid = False
            End Try

            logger.Debug("ValidateNumericValue Exit - " & valid)

            Return valid
        End Function

        Public Function ValidateData(ByVal vendorDeal As VendorDealBO) As ArrayList

            logger.Debug("ValidateData Entry")

            Dim statusList As New ArrayList

            'promo type is required
            If vendorDeal.CostPromoBO Is Nothing OrElse vendorDeal.CostPromoBO.CostPromoCode <= 0 Then
                statusList.Add(VendorDealStatus.Error_Required_CostPromoCode)
            End If

            'vendor deal type is required
            If vendorDeal.DealTypeBO Is Nothing OrElse vendorDeal.DealTypeBO.VendorDealTypeCode.Equals("") Then
                statusList.Add(VendorDealStatus.Error_Required_VendorDealType)
            End If

            'case qty must be > 0
            If vendorDeal.CaseQty <= 0 Then
                statusList.Add(VendorDealStatus.Error_Required_CaseQty)
            ElseIf vendorDeal.CaseQty.ToString.IndexOf(".") > -1 Then
                'case qty must be a whole number
                statusList.Add(VendorDealStatus.Error_CaseQty_MustBeWhole)
            End If

            'case amt must be > 0
            If vendorDeal.CaseAmt <= 0 Then
                statusList.Add(VendorDealStatus.Error_Required_CaseAmt)
            Else
                'case amt must be within the valid range for a smallmoney value; smallmoney data range = +214,748.3647 to -214,748.3647;
                'amt must already be greater than zero - no need to check for negative range
                Dim smallMoneyMaxValue As Decimal = CType(214748.3647, Decimal)

                If vendorDeal.CaseAmt > smallMoneyMaxValue Then
                    statusList.Add(VendorDealStatus.Error_DataRange_CaseAmt)
                End If
            End If

            'start date must be >= today
            If vendorDeal.StartDate < System.DateTime.Today Then
                statusList.Add(VendorDealStatus.Error_StartDate_PastDate)
            End If

            'end date must be > start date
            If vendorDeal.EndDate <= vendorDeal.StartDate Then
                statusList.Add(VendorDealStatus.Error_EndDate_PastDate)
            End If

            'at least one store must be selected
            If vendorDeal.StoreList Is Nothing OrElse vendorDeal.StoreList.Trim.Equals("") Then
                statusList.Add(VendorDealStatus.Error_Required_StoreSelection)
                ' if this validation fails, return status list early, because otherwise next validation will throw an exception.
                Return statusList
            End If

            'deals can ONLY have one "Not Stackable" item for a given time frame (start-end dates)
            'this is because during that time frame, it will override all other deals setup
            If vendorDeal.NotStackable AndAlso CostPromotionDAO.GetStackableConflicts(vendorDeal) Then
                statusList.Add(VendorDealStatus.Error_Conflict_NotStackable)
            End If



            If statusList.Count = 0 Then
                statusList.Add(VendorDealStatus.Valid)
            End If

            logger.Debug("ValidateData Exit")


            Return statusList
        End Function

        ''' <summary>
        ''' user is entering a discount --> checks the cost being entered by the user against existing net cost of item and 
        ''' returns true if the new net cost is less than or equal to $0
        ''' </summary>
        ''' <param name="vendorDeal"></param>
        ''' <returns>ArrayList of store conflicts</returns>
        ''' <remarks></remarks>
        Public Function GetNegativeCostStoreConflicts(ByVal vendorDeal As VendorDealBO) As ArrayList
            Return CostPromotionDAO.GetNetCostConflicts(vendorDeal)
        End Function

        ''' <summary>
        ''' user is entering a cost change --> checks the cost being entered by the user against existing net cost of item and 
        ''' returns true if the new net cost is less than or equal to $0
        ''' </summary>
        ''' <param name="vendorDeal"></param>
        ''' <returns>ArrayList of store conflicts</returns>
        ''' <remarks></remarks>
        Public Function GetNegativeCostStoreConflicts_CostChange(ByVal vendorDeal As VendorDealBO) As ArrayList
            Return CostPromotionDAO.GetNetCostConflicts_CostChange(vendorDeal)
        End Function

#End Region

    End Class

End Namespace