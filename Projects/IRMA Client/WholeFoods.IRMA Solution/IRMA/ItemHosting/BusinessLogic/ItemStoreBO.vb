Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Pricing.DataAccess

Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Enum ItemStoreStatus
        Valid
        Error_VendorRequiredForAuth
        Error_PendingPriceChangePreventsDeAuth
    End Enum

    Public Class ItemStoreBO
        Private _itemKey As Integer
        Private _storeId As Integer
        Private _authorized As Boolean
        Private _lineDiscount As Boolean
        Private _restrictedHours As Boolean
        Private _stopSale As Boolean
        Private _competitiveItem As Boolean
        Private _grillPrint As Boolean
        Private _srCitizenDiscount As Boolean
        Private _visualVerify As Boolean
        Private _posTare As String
        Private _ageCode As String
        Private _linkedIdentifier As String
        Private _linkedItemKey As Integer
        Private _itemSubTeam As Integer
        Private _storeSubTeam As Integer
        Private _posLinkCode As String
        Private _printCondimentOnReceipt As Boolean
        Private _consolidatePrice As Boolean
        Private _kitchenRouteID As Integer
        Private _routingPriority As Short
        Private _kitchenRouteDescription As String
        Private _ageRestrict As Boolean
        Private _mixMatch As Integer
        Private _discountable As Boolean
        Private _lastScannedUserName_DTS As String
        Private _lastScannedUserName_NonDTS As String
        Private _lastScannedDate_DTS As String
        Private _lastScannedDate_NonDTS As String
        Private _refreshPOSInfo As Boolean
        Private _localItem As Boolean
        Private _itemSurcharge As Integer
        Private _electronicShelfTag As Boolean
        Private _discontinue As Boolean
        Private _ecommerce As Boolean
        Private _retailUomId As Integer
        Private _scaleUomId As Integer
        Private _fixedWeight As String
        Private _byCount As Integer
        Private _itemStatusCode As Integer?

#Region "Property Access Methods"
        Public Property ItemKey() As Integer
            Get
                Return _itemKey
            End Get
            Set(ByVal value As Integer)
                _itemKey = value
            End Set
        End Property

        Public Property StoreSubTeam() As Integer
            Get
                Return _storeSubTeam
            End Get
            Set(ByVal value As Integer)
                _storeSubTeam = value
            End Set
        End Property

        Public Property Authorized() As Boolean
            Get
                Return _authorized
            End Get
            Set(ByVal value As Boolean)
                _authorized = value
            End Set
        End Property

        Public Property ItemSubTeam() As Integer
            Get
                Return _itemSubTeam
            End Get
            Set(ByVal value As Integer)
                _itemSubTeam = value
            End Set
        End Property

        Public Property StoreId() As Integer
            Get
                Return _storeId
            End Get
            Set(ByVal value As Integer)
                _storeId = value
            End Set
        End Property

        Public Property LineDiscount() As Boolean
            Get
                Return _lineDiscount
            End Get
            Set(ByVal value As Boolean)
                _lineDiscount = value
            End Set
        End Property

        Public Property RestrictedHours() As Boolean
            Get
                Return _restrictedHours
            End Get
            Set(ByVal value As Boolean)
                _restrictedHours = value
            End Set
        End Property

        Public Property StopSale() As Boolean
            Get
                Return _stopSale
            End Get
            Set(ByVal value As Boolean)
                _stopSale = value
            End Set
        End Property

        Public Property CompetitiveItem() As Boolean
            Get
                Return _competitiveItem
            End Get
            Set(ByVal value As Boolean)
                _competitiveItem = value
            End Set
        End Property

        Public Property GrillPrint() As Boolean
            Get
                Return _grillPrint
            End Get
            Set(ByVal value As Boolean)
                _grillPrint = value
            End Set
        End Property

        Public Property SrCitizenDiscount() As Boolean
            Get
                Return _srCitizenDiscount
            End Get
            Set(ByVal value As Boolean)
                _srCitizenDiscount = value
            End Set
        End Property

        Public Property VisualVerify() As Boolean
            Get
                Return _visualVerify
            End Get
            Set(ByVal value As Boolean)
                _visualVerify = value
            End Set
        End Property

        Public Property POS_Tare() As String
            Get
                Return _posTare
            End Get
            Set(ByVal value As String)
                _posTare = value
            End Set
        End Property

        Public Property AgeCode() As String
            Get
                Return _ageCode
            End Get
            Set(ByVal value As String)
                _ageCode = value
            End Set
        End Property

        Public Property LinkedIdentifier() As String
            Get
                Return _linkedIdentifier
            End Get
            Set(ByVal value As String)
                _linkedIdentifier = value
            End Set
        End Property

        Public Property LinkedItemKey() As Integer
            Get
                Return _linkedItemKey
            End Get
            Set(ByVal value As Integer)
                _linkedItemKey = value
            End Set
        End Property

        Public Property POSLinkCode() As String
            Get
                Return _posLinkCode
            End Get
            Set(ByVal value As String)
                _posLinkCode = value
            End Set
        End Property

        Public Property MixMatch() As Integer
            Get
                Return _mixMatch
            End Get
            Set(ByVal value As Integer)
                _mixMatch = value
            End Set
        End Property

        ''' <summary>
        ''' Freedom System Setting Print Condiment on Receipt
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PrintCondimentOnReceipt() As Boolean
            Get
                Return _printCondimentOnReceipt
            End Get
            Set(ByVal value As Boolean)
                _printCondimentOnReceipt = value
            End Set
        End Property

        ''' <summary>
        ''' Freedom System Setting Consolidate Price to Previous Item
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConsolidatePrice() As Boolean
            Get
                Return _consolidatePrice
            End Get
            Set(ByVal value As Boolean)
                _consolidatePrice = value
            End Set
        End Property

        ''' <summary>
        ''' Freedom System Setting Kitchen Route
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property KitchenRouteID() As Integer
            Get
                Return _kitchenRouteID
            End Get
            Set(ByVal value As Integer)
                _kitchenRouteID = value
            End Set
        End Property

        ''' <summary>
        ''' Freedom System Setting Kitchen Route Description
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Sent to POS</remarks>
        Public Property KitchenRouteDescription() As String
            Get
                Return _kitchenRouteDescription
            End Get
            Set(ByVal value As String)
                _kitchenRouteDescription = value
            End Set
        End Property

        ''' <summary>
        ''' Freedom System Setting Routing Priority
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RoutingPriority() As Int16
            Get
                Return _routingPriority
            End Get
            Set(ByVal value As Int16)
                _routingPriority = value
            End Set
        End Property

        ''' <summary>
        ''' Item Sale Ages Restriction
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AgeRestrict() As Boolean
            Get
                Return _ageRestrict
            End Get
            Set(ByVal value As Boolean)
                _ageRestrict = value
            End Set
        End Property

        ''' <summary>
        ''' Refresh POS Info
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RefreshPOSInfo() As Boolean
            Get
                Return _refreshPOSInfo
            End Get
            Set(ByVal value As Boolean)
                _refreshPOSInfo = value
            End Set
        End Property

        ''' <summary>
        ''' Allow team member discount to apply to this item at this store
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Discountable() As Boolean
            Get
                Return _discountable
            End Get
            Set(ByVal value As Boolean)
                _discountable = value
            End Set
        End Property

        ''' <summary>
        ''' Name of the last DTS that scanned an item
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LastScannedUserName_DTS() As String
            Get
                Return _lastScannedUserName_DTS
            End Get
            Set(ByVal value As String)
                _lastScannedUserName_DTS = value
            End Set
        End Property

        ''' <summary>
        ''' Name of the last Non DTS that scanned an item
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LastScannedUserName_NonDTS() As String
            Get
                Return _lastScannedUserName_NonDTS
            End Get
            Set(ByVal value As String)
                _lastScannedUserName_NonDTS = value
            End Set
        End Property

        ''' <summary>
        ''' Date of the last DTS that scanned an item
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LastScannedDate_DTS() As String
            Get
                Return _lastScannedDate_DTS
            End Get
            Set(ByVal value As String)
                _lastScannedDate_DTS = value
            End Set
        End Property

        ''' <summary>
        ''' Date of the last Non DTS that scanned an item
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LastScannedDate_NonDTS() As String
            Get
                Return _lastScannedDate_NonDTS
            End Get
            Set(ByVal value As String)
                _lastScannedDate_NonDTS = value
            End Set
        End Property

        ''' <summary>
        ''' Determines whether or not an item is Local
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LocalItem() As Boolean
            Get
                Return _localItem
            End Get
            Set(ByVal value As Boolean)
                _localItem = value
            End Set
        End Property

        Public Property ItemSurcharge() As Integer
            Get
                Return _itemSurcharge
            End Get
            Set(ByVal value As Integer)
                _itemSurcharge = value
            End Set
        End Property

        Public Property ElectronicShelfTag() As Boolean
            Get
                Return _electronicShelfTag
            End Get
            Set(ByVal value As Boolean)
                _electronicShelfTag = value
            End Set
        End Property

        Public Property Discontinue() As Boolean
            Get
                Return _discontinue
            End Get
            Set(ByVal value As Boolean)
                _discontinue = value
            End Set
        End Property

        Public Property ECommerce() As Boolean
            Get
                Return _ecommerce
            End Get
            Set(value As Boolean)
                _ecommerce = value
            End Set
        End Property

        Public Property RetailUomId() As Integer
            Get
                Return _retailUomId
            End Get
            Set(ByVal value As Integer)
                _retailUomId = value
            End Set
        End Property

        Public Property ScaleUomId() As Integer
            Get
                Return _scaleUomId
            End Get
            Set(ByVal value As Integer)
                _scaleUomId = value
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

        Public Property FixedWeight() As String
            Get
                Return _fixedWeight
            End Get
            Set(ByVal value As String)
                _fixedWeight = value
            End Set
        End Property

        Public Property ItemStatusCode() As Integer?
            Get
                Return _itemStatusCode
            End Get
            Set(ByVal value As Integer?)
                _itemStatusCode = value
            End Set
        End Property
#End Region

#Region "business rules"

        Public Function ValidateData() As ArrayList
            Dim statusList As New ArrayList

            ' If the user set the Authorized flag to true, verify there is at least one primary vendor 
            ' relationship defined.
            If _authorized AndAlso Not ItemDAO.HasPrimaryVendor(_itemKey, _storeId) Then
                statusList.Add(ItemStoreStatus.Error_VendorRequiredForAuth)
            End If

            ' If the user set the Authorized flag to false, verify there are not pending price changes
            ' that have not been processed.
            If Not _authorized AndAlso PriceBatchDetailDAO.CheckForPendingPriceBatches(_itemKey, _storeId.ToString, ","c, BatchStatus.AllButProcessed) Then
                statusList.Add(ItemStoreStatus.Error_PendingPriceChangePreventsDeAuth)
            End If

            ' TODO: If the user set the Discontinue flag to true, verify there is an active StoreItemVendor record

            If statusList.Count <= 0 Then
                statusList.Add(ItemStoreStatus.Valid)
            End If

            Return statusList
        End Function

#End Region

    End Class

End Namespace