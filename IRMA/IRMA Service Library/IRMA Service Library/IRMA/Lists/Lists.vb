Namespace IRMA

    Public Class Lists

        'TODO: Move all the classes below to seperate class files.
        'DO NOT ADD any new classes here.

        <DataContract()> _
        Public Class ShrinkAdjustmentReason

            Private _inventoryAdjustmentCodeID As Integer
            Private _abbreviation As String
            Private _adjustmentDescription As String
            Private _adjustmentID As Integer

            <DataMember()> _
            Public Property InventoryAdjustmentCodeID() As Integer
                Get
                    Return _inventoryAdjustmentCodeID
                End Get
                Set(ByVal value As Integer)
                    _inventoryAdjustmentCodeID = value
                End Set
            End Property

            <DataMember()> _
            Public Property Abbreviation() As String
                Get
                    Return _abbreviation
                End Get
                Set(ByVal value As String)
                    _abbreviation = value
                End Set
            End Property

            <DataMember()> _
            Public Property AdjustmentDescription() As String
                Get
                    Return _adjustmentDescription
                End Get
                Set(ByVal value As String)
                    _adjustmentDescription = value
                End Set
            End Property

            <DataMember()> _
            Public Property AdjustmentID() As Integer
                Get
                    Return _adjustmentID
                End Get
                Set(ByVal value As Integer)
                    _adjustmentID = value
                End Set
            End Property


        End Class

        <DataContract()> _
        Public Class StoreFTPConfig

            Private _storeNo As Integer
            Private _fileWriterType As String
            Private _ipAddress As String
            Private _ftpUser As String
            Private _ftpPassword As String
            Private _changeDirectory As String
            Private _port As String  'port set as String so as not to default to 0
            Private _isSecureTransfer As Boolean
            Private _posSystemType As String


            <DataMember()> _
            Public Property StoreNo() As Integer
                Get
                    Return _storeNo
                End Get
                Set(ByVal value As Integer)
                    _storeNo = value
                End Set
            End Property

            <DataMember()> _
            Public Property FileWriterType() As String
                Get
                    Return _fileWriterType
                End Get
                Set(ByVal value As String)
                    _fileWriterType = value
                End Set
            End Property

            <DataMember()> _
            Public Property IPAddress() As String
                Get
                    Return _ipAddress
                End Get
                Set(ByVal value As String)
                    _ipAddress = value
                End Set
            End Property

            <DataMember()> _
            Public Property FTPUser() As String
                Get
                    Return _ftpUser
                End Get
                Set(ByVal value As String)
                    _ftpUser = value
                End Set
            End Property

            <DataMember()> _
            Public Property FTPPassword() As String
                Get
                    Return _ftpPassword
                End Get
                Set(ByVal value As String)
                    _ftpPassword = value
                End Set
            End Property

            <DataMember()> _
            Public Property ChangeDirectory() As String
                Get
                    Return _changeDirectory
                End Get
                Set(ByVal value As String)
                    _changeDirectory = value
                End Set
            End Property

            <DataMember()> _
            Public Property Port() As String
                Get
                    Return _port
                End Get
                Set(ByVal value As String)
                    _port = value
                End Set
            End Property

            <DataMember()> _
            Public Property IsSecureTransfer() As Boolean
                Get
                    Return _isSecureTransfer
                End Get
                Set(ByVal value As Boolean)
                    _isSecureTransfer = value
                End Set
            End Property

            <DataMember()> _
            Public Property POSSystemType() As String
                Get
                    Return _posSystemType
                End Get
                Set(ByVal value As String)
                    _posSystemType = value
                End Set
            End Property


            Sub New()

            End Sub

            Sub New(ByRef results As SqlClient.SqlDataReader)
                Try
                    If (Not results.IsDBNull(results.GetOrdinal("Store_No"))) Then
                        _storeNo = results.GetInt32(results.GetOrdinal("Store_No"))
                    End If

                    If (Not results.IsDBNull(results.GetOrdinal("FileWriterType"))) Then
                        _fileWriterType = results.GetString(results.GetOrdinal("FileWriterType"))
                    End If

                    If (Not results.IsDBNull(results.GetOrdinal("IP_Address"))) Then
                        _ipAddress = results.GetString(results.GetOrdinal("IP_Address"))
                    End If

                    If (Not results.IsDBNull(results.GetOrdinal("FTP_User"))) Then
                        _ftpUser = results.GetString(results.GetOrdinal("FTP_User"))
                    End If

                    If (Not results.IsDBNull(results.GetOrdinal("FTP_Password"))) Then
                        _ftpPassword = results.GetString(results.GetOrdinal("FTP_Password"))
                    End If

                    If (Not results.IsDBNull(results.GetOrdinal("ChangeDirectory"))) Then
                        _changeDirectory = results.GetString(results.GetOrdinal("ChangeDirectory"))
                    End If

                    If (Not results.IsDBNull(results.GetOrdinal("Port"))) Then
                        _port = results.GetInt32(results.GetOrdinal("Port")).ToString
                    End If

                    If (Not results.IsDBNull(results.GetOrdinal("IsSecureTransfer"))) Then
                        _isSecureTransfer = results.GetBoolean(results.GetOrdinal("IsSecureTransfer"))
                    End If

                    If (Not results.IsDBNull(results.GetOrdinal("POSSystemType"))) Then
                        _posSystemType = results.GetString(results.GetOrdinal("POSSystemType"))
                    End If

                Catch ex As Exception
                    Throw ex
                End Try


            End Sub
        End Class


        <DataContract()> _
        Public Class GetItem

            Private _itemKey As Integer
            Private _itemDescription As String
            Private _itemSubteamNo As Integer
            Private _itemSubteamName As String
            Private _packageDesc1 As Integer
            Private _packageDesc2 As Decimal
            Private _packageUnitAbbr As String
            Private _soldByWeight As Boolean
            Private _CostedByWeight As Boolean
            Private _packageUnitID As Integer

            <DataMember()> _
            Public Property ItemKey() As Integer
                Get
                    Return _itemKey
                End Get
                Set(ByVal value As Integer)
                    _itemKey = value
                End Set
            End Property

            <DataMember()> _
            Public Property ItemDescription() As String
                Get
                    Return _itemDescription
                End Get
                Set(ByVal value As String)
                    _itemDescription = value
                End Set
            End Property

            <DataMember()> _
            Public Property ItemSubteamNo() As Integer
                Get
                    Return _itemSubteamNo
                End Get
                Set(ByVal value As Integer)
                    _itemSubteamNo = value
                End Set
            End Property

            <DataMember()> _
            Public Property ItemSubteamName() As String
                Get
                    Return _itemSubteamName
                End Get
                Set(ByVal value As String)
                    _itemSubteamName = value
                End Set
            End Property

            <DataMember()> _
            Public Property PackageDesc1() As Integer
                Get
                    Return _packageDesc1
                End Get
                Set(ByVal value As Integer)
                    _packageDesc1 = value
                End Set
            End Property

            <DataMember()> _
            Public Property PackageDesc2() As Decimal
                Get
                    Return _packageDesc2
                End Get
                Set(ByVal value As Decimal)
                    _packageDesc2 = value
                End Set
            End Property

            <DataMember()> _
            Public Property PackageUnitAbbr() As String
                Get
                    Return _packageUnitAbbr
                End Get
                Set(ByVal value As String)
                    _packageUnitAbbr = value
                End Set
            End Property

            <DataMember()> _
            Public Property SoldByWeight() As Boolean
                Get
                    Return _soldByWeight
                End Get
                Set(ByVal value As Boolean)
                    _soldByWeight = value
                End Set
            End Property

            <DataMember()>
            Public Property CostedByWeight As Boolean
                Get
                    Return _CostedByWeight
                End Get
                Set(ByVal value As Boolean)
                    _CostedByWeight = value
                End Set
            End Property

            <DataMember()>
            Public Property RetailUnit As String

            'ItemKey*
            'Item_Description*
            'POS_Description
            'Item.Subteam_No*
            'Package_Desc1*
            'Package_Desc2*
            'Package_Unit_Abbr*
            'Not_Available
            'Sold_By_Weight*
            'Retail_Sale
            'Vendor_Unit_ID
            'Vendor_Unit_Name

            <DataMember()> _
            Public Property PackageUnitID() As Integer
                Get
                    Return _packageUnitID
                End Get
                Set(ByVal value As Integer)
                    _packageUnitID = value
                End Set
            End Property

        End Class
        <DataContract()> _
        Public Class ReasonCode

            Private _reasonCodeID As Integer
            Private _reasonCode As String
            Private _description As String

            <DataMember()> _
            Public Property ReasonCodeID() As Integer
                Get
                    Return _reasonCodeID
                End Get
                Set(ByVal value As Integer)
                    _reasonCodeID = value
                End Set
            End Property

            <DataMember()> _
            Public Property ReasonCode() As String
                Get
                    Return _reasonCode
                End Get
                Set(ByVal value As String)
                    _reasonCode = value
                End Set
            End Property

            <DataMember()> _
            Public Property Description() As String
                Get
                    Return _description
                End Get
                Set(ByVal value As String)
                    _description = value
                End Set
            End Property

        End Class

        <DataContract()> _
        Public Class Store

            Private _storeName As String
            Private _storeNo As Integer

            <DataMember()> _
            Public Property StoreName() As String
                Get
                    Return _storeName
                End Get
                Set(ByVal value As String)
                    _storeName = value
                End Set
            End Property

            <DataMember()> _
            Public Property StoreNo() As Integer
                Get
                    Return _storeNo
                End Get
                Set(ByVal value As Integer)
                    _storeNo = value
                End Set
            End Property

        End Class

        <DataContract()> _
        Public Class Subteam

            Private _subteamName As String
            Private _subteamNo As Integer
            Private _subteamIsFixedSpoilage As Boolean
            Private _subteamIsUnrestricted As Integer
            Private _subTeamTypeID As Integer

            <DataMember()> _
            Public Property SubteamName() As String
                Get
                    Return _subteamName
                End Get
                Set(ByVal value As String)
                    _subteamName = value
                End Set
            End Property

            <DataMember()> _
            Public Property SubteamNo() As Integer
                Get
                    Return _subteamNo
                End Get
                Set(ByVal value As Integer)
                    _subteamNo = value
                End Set
            End Property

            <DataMember()> _
            Public Property SubteamType() As Integer
                Get
                    Return _subTeamTypeID
                End Get
                Set(ByVal value As Integer)
                    _subTeamTypeID = value
                End Set
            End Property

            <DataMember()> _
            Public Property SubteamIsFixedSpoilage() As Boolean
                Get
                    Return _subteamIsFixedSpoilage
                End Get
                Set(ByVal value As Boolean)
                    _subteamIsFixedSpoilage = value
                End Set
            End Property

            <DataMember()> _
            Public Property SubteamIsUnrestricted() As Boolean
                Get
                    Return _subteamIsUnrestricted
                End Get
                Set(ByVal value As Boolean)
                    _subteamIsUnrestricted = value
                End Set
            End Property

        End Class

        <DataContract()>
        Public Class ItemUnit
            <DataMember()>
            Public Property Unit_Id As Integer
            <DataMember()>
            Public Property Unit_Name As String
            <DataMember()>
            Public Property Weight_Unit As Boolean
            <DataMember()>
            Public Property User_Id As Integer
            <DataMember()>
            Public Property Unit_Abbreviation As String
            <DataMember()>
            Public Property UnitSysCode As String
            <DataMember()>
            Public Property IsPackageUnit As Boolean
            <DataMember()>
            Public Property PlumUnitAbbr As String
            <DataMember()>
            Public Property EDISysCode As String
        End Class


        <DataContract()>
        Public Class Order
            <DataMember()>
            Public Property OrderHeader_ID As Integer
            <DataMember()>
            Public Property OrderExternalSourceOrderID As Integer
            <DataMember()>
            Public Property OrderExternalOrderSource As String

            <DataMember()>
            Public Property Temperature As Integer
            <DataMember()>
            Public Property Accounting_In_DateStamp As DateTime
            <DataMember()>
            Public Property CompanyName As String
            <DataMember()>
            Public Property CreatedByName As String
            <DataMember()>
            Public Property SubTeam_Name As String
            <DataMember()>
            Public Property Transfer_To_SubTeamName As String
            <DataMember()>
            Public Property OrderDate As DateTime
            <DataMember()>
            Public Property CloseDate As DateTime
            <DataMember()>
            Public Property SentDate As DateTime
            <DataMember()>
            Public Property Expected_Date As DateTime
            <DataMember()>
            Public Property InvoiceDate As DateTime
            <DataMember()>
            Public Property UploadedDate As DateTime
            <DataMember()>
            Public Property ApprovedDate As DateTime
            <DataMember()>
            Public Property OrderType_Id As Integer
            <DataMember()>
            Public Property ProductType_ID As Integer
            <DataMember()>
            Public Property CreatedBy As Integer
            <DataMember()>
            Public Property Transfer_SubTeam As Integer
            <DataMember()>
            Public Property Transfer_To_SubTeam As Integer
            <DataMember()>
            Public Property Vendor_ID As Integer
            <DataMember()>
            Public Property Vendor_Store_No As Integer
            <DataMember()>
            Public Property PurchaseLocation_ID As Integer
            <DataMember()>
            Public Property ReceiveLocation_ID As Integer
            <DataMember()>
            Public Property Fax_Order As Boolean
            <DataMember()>
            Public Property Email_Order As Boolean
            <DataMember()>
            Public Property Electronic_Order As Boolean
            <DataMember()>
            Public Property Sent As Boolean
            <DataMember()>
            Public Property Return_Order As Boolean
            <DataMember()>
            Public Property OriginalCloseDate As DateTime
            <DataMember()>
            Public Property User_ID As Integer
            <DataMember()>
            Public Property InvoiceNumber As String
            <DataMember()>
            Public Property ReturnOrder_ID As Integer
            <DataMember()>
            Public Property OverrideTransmissionMethod As Boolean
            <DataMember()>
            Public Property isDropShipment As Boolean
            <DataMember()>
            Public Property OriginalOrder_ID As Integer
            <DataMember()>
            Public Property Store_No As Integer
            <DataMember()>
            Public Property POTransmissionTypeID As Integer
            <DataMember()>
            Public Property WFM_Store As Boolean
            <DataMember()>
            Public Property HFM_Store As Boolean
            <DataMember()>
            Public Property Store_Vend As Boolean
            <DataMember()>
            Public Property RecvLog_No As Integer
            <DataMember()>
            Public Property WarehouseSent As Boolean
            <DataMember()>
            Public Property WarehouseSentDate As DateTime
            <DataMember()>
            Public Property EXEWarehouse As Integer
            <DataMember()>
            Public Property From_SubTeam_Unrestricted As Integer
            <DataMember()>
            Public Property To_SubTeam_Unrestricted As Integer
            <DataMember()>
            Public Property ItemsReceived As Integer
            <DataMember()>
            Public Property OrderEnd As String
            <DataMember()>
            Public Property CurrSysTime As DateTime
            <DataMember()>
            Public Property Distribution_Center As Boolean
            <DataMember()>
            Public Property ReceivingStore_Distribution_Center As Boolean
            <DataMember()>
            Public Property Manufacturer As Boolean
            <DataMember()>
            Public Property WFM As Boolean
            <DataMember()>
            Public Property IsEXEDistributed As Boolean
            <DataMember()>
            Public Property InvoiceAmount As Decimal
            <DataMember()>
            Public Property ClosedByUserName As String
            <DataMember()>
            Public Property ApprovedByUserName As String
            <DataMember()>
            Public Property Freight3Party_OrderCost As Decimal
            <DataMember()>
            Public Property PSVendorID As String
            <DataMember()>
            Public Property ShipToStoreNo As Integer
            <DataMember()>
            Public Property BuyerName As String
            <DataMember()>
            Public Property BuyerEmail As String
            <DataMember()>
            Public Property Notes As String
            <DataMember()>
            Public Property DiscountAmount As Decimal
            <DataMember()>
            Public Property AllowanceDiscountAmount As Decimal
            <DataMember()>
            Public Property Store_Phone As String
            <DataMember()>
            Public Property QuantityDiscount As Decimal
            <DataMember()>
            Public Property DiscountType As Integer
            <DataMember()>
            Public Property StoreCompanyName As String
            <DataMember()>
            Public Property ShipToStoreCompanyName As String
            <DataMember()>
            Public Property IsVendorExternal As Boolean
            <DataMember()>
            Public Property SupplyType_SubTeamName As String
            <DataMember()>
            Public Property AccountingUploadDate As DateTime
            <DataMember()>
            Public Property OrderedCost As Decimal
            <DataMember()>
            Public Property AdjustedReceivedCost As Decimal
            <DataMember()>
            Public Property UploadedCost As Decimal
            <DataMember()>
            Public Property WarehouseCancelled As DateTime
            <DataMember()>
            Public Property PayByAgreedCost As Boolean
            <DataMember()>
            Public Property TotalHandlingCharge As Decimal
            <DataMember()>
            Public Property POCostDate As DateTime

            <DataMember()>
            Public Property OrderItems As List(Of OrderItem)


        End Class

        <DataContract()>
        Public Class ExternalOrder       ' type so user can decode the list
            <DataMember()> _
            Public Property OrderHeader_ID As Integer
            <DataMember()> _
            Public Property Source As String
            <DataMember()> _
            Public Property CompanyName As String
        End Class

        <DataContract()> _
        Public Class ItemMovement

            Private _movementQty As Decimal
            Private _movementDate As Date

            <DataMember()> _
            Public Property MovementQty() As Decimal
                Get
                    Return _movementQty
                End Get
                Set(ByVal value As Decimal)
                    _movementQty = value
                End Set
            End Property

            <DataMember()> _
            Public Property MovementDate() As Date
                Get
                    Return _movementDate
                End Get
                Set(ByVal value As Date)
                    _movementDate = value
                End Set
            End Property

        End Class

        <DataContract()> _
        Public Class ItemBilledQty

            Private _billedQty As Decimal
            Private _orderQty As Decimal
            Private _orderDate As Date
            ' Private _invoiceExist As Boolean
            Private _invNum As String
            ' Private _invDate As Date

            <DataMember()> _
            Public Property BilledQty() As Decimal
                Get
                    Return _billedQty
                End Get
                Set(ByVal value As Decimal)
                    _billedQty = value
                End Set
            End Property

            <DataMember()> _
            Public Property OrderQty() As Decimal
                Get
                    Return _orderQty
                End Get
                Set(ByVal value As Decimal)
                    _orderQty = value
                End Set
            End Property


            <DataMember()> _
            Public Property OrderDate() As Date
                Get
                    Return _orderDate
                End Get
                Set(ByVal value As Date)
                    _orderDate = value
                End Set
            End Property

            '<DataMember()> _
            'Public Property InvoiceExist() As Boolean
            '    Get
            '        Return _invoiceExist
            '    End Get
            '    Set(ByVal value As Boolean)
            '        _invoiceExist = value
            '    End Set
            'End Property

            <DataMember()> _
            Public Property InvNum() As String
                Get
                    Return _invNum
                End Get
                Set(ByVal value As String)
                    _invNum = value
                End Set
            End Property

            '<DataMember()> _
            'Public Property InvDate() As Date
            '    Get
            '        Return _invDate
            '    End Get
            '    Set(ByVal value As Date)
            '        _invDate = value
            '    End Set
            'End Property
        End Class

      

    End Class  ' Lists


End Namespace

