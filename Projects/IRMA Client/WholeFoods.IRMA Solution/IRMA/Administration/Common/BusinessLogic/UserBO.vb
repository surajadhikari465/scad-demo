Imports WholeFoods.Utility
Imports WholeFoods.IRMA.Administration.Common.DataAccess

Namespace WholeFoods.IRMA.Administration.Common.BusinessLogic
    Public Enum UserConfigStatus
        Valid
        Error_Required_UserName
        Error_Required_FullName
    End Enum

    Public Class UserBO

#Region "Property Definitions"
        Private _userId As Integer
        Private _accountEnabled As Boolean
        Private _userName As String
        Private _fullName As String
        Private _email As String
        Private _pagerEmail As String
        Private _phoneNumber As String
        Private _faxNumber As String
        Private _title As Integer
        Private _printer As String
        Private _coverPage As String

        ' Flags that control the Role
        Private _superUser As Boolean
        Private _poAccountant As Boolean
        Private _accountant As Boolean
        Private _distributor As Boolean
        Private _facilitycreditprocessor As Boolean
        Private _buyer As Boolean
        Private _coordinator As Boolean
        Private _itemAdministrator As Boolean
        Private _vendorAdministrator As Boolean
        Private _lockAdministrator As Boolean
        Private _warehouse As Boolean
        Private _cancelAllSales As Boolean
        Private _priceBatchProcessor As Boolean
        Private _inventoryAdministrator As Boolean
        Private _batchBuildOnly As Boolean
        Private _dcAdmin As Boolean
        Private _deletePO As Boolean
        Private _taxAdministrator As Boolean
        Private _costAdmin As Boolean
        Private _VendorCostDiscrepancyAdmin As Boolean
        Private _POApprovalAdmin As Boolean
        Private _POEditor As Boolean
        Private _EInvoicingAdmin As Boolean
        Private _ApplicationConfigAdmin As Boolean
        Private _SystemConfigurationAdministrator As Boolean
        Private _DataAdministrator As Boolean
        Private _JobAdministrator As Boolean
        Private _POSInterfaceAdministrator As Boolean
        Private _StoreAdministrator As Boolean
        Private _SecurityAdministrator As Boolean
        Private _UserMaintenance As Boolean

        ' Promo Planner

        Private _promoAccessLevel As Int16

        ' Data that isn't being updated by the UI currently
        ' Need to add property definitions for any that we're going to use
        Private _assistantID As Integer
        Private _search_1 As Boolean
        Private _search_2 As Boolean
        Private _search_3 As Boolean
        Private _search_4 As Boolean
        Private _search_5 As Boolean
        Private _search_6 As Boolean
        Private _search_7 As Boolean
        Private _search_8 As Integer
        Private _search_9 As Integer
        Private _search_10 As Integer
        Private _telxon_Store_Limit As Integer
        Private _telxon_Enabled As Boolean
        Private _telxon_Waste As Boolean
        Private _telxon_Cycle_Count As Boolean
        Private _telxon_Distribution As Boolean
        Private _telxon_Transfers As Boolean
        Private _telxon_Price_Check As Boolean
        Private _telxon_Superuser As Boolean
        Private _telxon_Orders As Boolean
        Private _telxon_Receiving As Boolean
        Private _telxon_Reserved_2 As Boolean
        Private _telxon_Reserved_3 As Boolean
        Private _support_Password As String
        Private _support_Administrator As Boolean
        Private _support_Worker As Boolean
        Private _maintenance_Password As String
        Private _maintenance_Administrator As Boolean
        Private _maintenance_Worker As Boolean
        Private _recvLog_Store_Limit As Integer
        Private _delete_Access As Boolean

        ' calculated values during login processing
        Private _vendorLimit As Integer

        ' SLIM
        Private _slimUserAdmin As Boolean
        Private _slimItemRequest As Boolean
        Private _slimVendorRequest As Boolean
        Private _slimPushToIRMA As Boolean
        Private _slimStoreSpecials As Boolean
        Private _slimRetailCost As Boolean
        Private _slimAuthorizations As Boolean
        Private _slimScaleInfo As Boolean
        Private _slimSecureQuery As Boolean
        Private _slimECommerce As Boolean

        'Shrink
        Private _shrink As Boolean
        Private _shrinkAdmin As Boolean

#End Region

#Region "constructors and helper methods to initialize the data"
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Create a new instance of the object from the database using the UserID.
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal UserID As Integer)

            Dim dt As DataTable = Nothing

            Try

                dt = New DataTable
                dt = UserDAO.GetUser(UserID)

                If dt.Rows.Count > 0 Then

                    _userId = dt.Rows(0).Item("User_ID")

                    If dt.Rows(0).Item("UserName") IsNot DBNull.Value Then
                        _userName = dt.Rows(0).Item("UserName")
                    End If

                    If dt.Rows(0).Item("FullName") IsNot DBNull.Value Then
                        _fullName = dt.Rows(0).Item("FullName")
                    End If

                    If dt.Rows(0).Item("Printer") IsNot DBNull.Value Then
                        _printer = dt.Rows(0).Item("Printer")
                    End If

                    If dt.Rows(0).Item("CoverPage") IsNot DBNull.Value Then
                        _coverPage = dt.Rows(0).Item("CoverPage")
                    End If

                    If dt.Rows(0).Item("Email") IsNot DBNull.Value Then
                        _email = dt.Rows(0).Item("Email")
                    End If

                    If dt.Rows(0).Item("Pager_Email") IsNot DBNull.Value Then
                        _pagerEmail = dt.Rows(0).Item("Pager_Email")
                    End If

                    If dt.Rows(0).Item("Fax_Number") IsNot DBNull.Value Then
                        _faxNumber = dt.Rows(0).Item("Fax_Number")
                    End If

                    If dt.Rows(0).Item("AccountEnabled") IsNot DBNull.Value Then
                        _accountEnabled = dt.Rows(0).Item("AccountEnabled")
                    End If

                    If dt.Rows(0).Item("Telxon_Store_Limit") IsNot DBNull.Value Then
                        _telxon_Store_Limit = dt.Rows(0).Item("Telxon_Store_Limit")
                    End If

                    If dt.Rows(0).Item("Phone_Number") IsNot DBNull.Value Then
                        _phoneNumber = dt.Rows(0).Item("Phone_Number")
                    End If

                    If dt.Rows(0).Item("Title") IsNot DBNull.Value Then
                        _title = dt.Rows(0).Item("Title")
                    End If

                    If dt.Rows(0).Item("RecvLog_Store_Limit") IsNot DBNull.Value Then
                        _recvLog_Store_Limit = dt.Rows(0).Item("RecvLog_Store_Limit")
                    End If

                    ' Roles
                    _superUser = dt.Rows(0).Item("SuperUser")
                    _poAccountant = dt.Rows(0).Item("PO_Accountant")
                    _accountant = dt.Rows(0).Item("Accountant")
                    _distributor = dt.Rows(0).Item("Distributor")
                    _facilitycreditprocessor = dt.Rows(0).Item("FacilityCreditProcessor")
                    _buyer = dt.Rows(0).Item("Buyer")
                    _coordinator = dt.Rows(0).Item("Coordinator")
                    _itemAdministrator = dt.Rows(0).Item("Item_Administrator")
                    _vendorAdministrator = dt.Rows(0).Item("Vendor_Administrator")
                    _lockAdministrator = dt.Rows(0).Item("Lock_Administrator")
                    _EInvoicingAdmin = dt.Rows(0).Item("Einvoicing_Administrator")
                    _warehouse = dt.Rows(0).Item("Warehouse")
                    _priceBatchProcessor = dt.Rows(0).Item("PriceBatchProcessor")
                    _inventoryAdministrator = dt.Rows(0).Item("Inventory_Administrator")
                    _batchBuildOnly = dt.Rows(0).Item("BatchBuildOnly")
                    _costAdmin = dt.Rows(0).Item("CostAdmin")
                    _shrink = dt.Rows(0).Item("Shrink")
                    _shrinkAdmin = dt.Rows(0).Item("ShrinkAdmin")
                    _cancelAllSales = dt.Rows(0).Item("CancelAllSales")

                    If dt.Rows(0).Item("DCAdmin") IsNot DBNull.Value Then
                        _dcAdmin = dt.Rows(0).Item("DCAdmin")
                    End If
                    If dt.Rows(0).Item("DeletePO") IsNot DBNull.Value Then
                        _deletePO = dt.Rows(0).Item("DeletePO")
                    End If
                    If dt.Rows(0).Item("VendorCostDiscrepancyAdmin") IsNot DBNull.Value Then
                        _VendorCostDiscrepancyAdmin = dt.Rows(0).Item("VendorCostDiscrepancyAdmin")
                    End If

                    If dt.Rows(0).Item("PromoAccessLevel") IsNot DBNull.Value Then
                        _promoAccessLevel = dt.Rows(0).Item("PromoAccessLevel")
                    End If

                    If dt.Rows(0).Item("POApprovalAdmin") IsNot DBNull.Value Then
                        _POApprovalAdmin = dt.Rows(0).Item("POApprovalAdmin")
                    End If

                    If dt.Rows(0).Item("POEditor") IsNot DBNull.Value Then
                        _POEditor = dt.Rows(0).Item("POEditor")
                    End If

                    If dt.Rows(0).Item("TaxAdministrator") IsNot DBNull.Value Then
                        _taxAdministrator = dt.Rows(0).Item("TaxAdministrator")
                    End If

                    ' Admin Roles
                    Me._ApplicationConfigAdmin = dt.Rows(0).Item("ApplicationConfigAdmin")
                    Me._SystemConfigurationAdministrator = dt.Rows(0).Item("SystemConfigurationAdministrator")
                    Me._DataAdministrator = dt.Rows(0).Item("DataAdministrator")
                    Me._JobAdministrator = dt.Rows(0).Item("JobAdministrator")
                    Me._POSInterfaceAdministrator = dt.Rows(0).Item("POSInterfaceAdministrator")
                    Me._StoreAdministrator = dt.Rows(0).Item("StoreAdministrator")
                    Me._SecurityAdministrator = dt.Rows(0).Item("SecurityAdministrator")
                    Me._UserMaintenance = dt.Rows(0).Item("UserMaintenance")

                    ' SLIM access attributes

                    Me._slimUserAdmin = dt.Rows(0).Item("UserAdmin")
                    Me._slimItemRequest = dt.Rows(0).Item("ItemRequest")
                    Me._slimVendorRequest = dt.Rows(0).Item("VendorRequest")
                    Me._slimPushToIRMA = dt.Rows(0).Item("IRMAPush")
                    Me._slimStoreSpecials = dt.Rows(0).Item("StoreSpecials")
                    Me._slimRetailCost = dt.Rows(0).Item("RetailCost")
                    Me._slimAuthorizations = dt.Rows(0).Item("Authorizations")
                    Me._slimSecureQuery = dt.Rows(0).Item("WebQuery")
                    Me._slimScaleInfo = dt.Rows(0).Item("ScaleInfo")
                    Me._slimECommerce = dt.Rows(0).Item("ECommerce")
                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

        End Sub

        ''' <summary>
        ''' Create a new instance of the object, populated from the DataGridViewRow
        ''' for a selected row on the UI.
        ''' </summary>
        ''' <param name="selectedRow"></param>
        ''' <remarks></remarks>
        Public Sub New(ByRef selectedRow As DataGridViewRow)
            If selectedRow.Cells("User_ID").Value IsNot DBNull.Value Then
                _userId = CType(selectedRow.Cells("User_ID").Value, Integer)
            End If
            If selectedRow.Cells("AccountEnabled").Value IsNot DBNull.Value Then
                _accountEnabled = CType(selectedRow.Cells("AccountEnabled").Value, Boolean)
            End If
            If selectedRow.Cells("UserName").Value IsNot DBNull.Value Then
                _userName = CType(selectedRow.Cells("UserName").Value, String)
            End If
            If selectedRow.Cells("FullName").Value IsNot DBNull.Value Then
                _fullName = CType(selectedRow.Cells("FullName").Value, String)
            End If
            If selectedRow.Cells("EMail").Value IsNot DBNull.Value Then
                _email = CType(selectedRow.Cells("EMail").Value, String)
            End If
            If selectedRow.Cells("Pager_Email").Value IsNot DBNull.Value Then
                _pagerEmail = CType(selectedRow.Cells("Pager_Email").Value, String)
            End If
            If selectedRow.Cells("Phone_Number").Value IsNot DBNull.Value Then
                _phoneNumber = CType(selectedRow.Cells("Phone_Number").Value, String)
            End If
            If selectedRow.Cells("Fax_Number").Value IsNot DBNull.Value Then
                _faxNumber = CType(selectedRow.Cells("Fax_Number").Value, String)
            End If
            If selectedRow.Cells("Title").Value IsNot DBNull.Value Then
                _title = CType(selectedRow.Cells("Title").Value, Integer)
            End If
            If selectedRow.Cells("Printer").Value IsNot DBNull.Value Then
                _printer = CType(selectedRow.Cells("Printer").Value, String)
            End If
            If selectedRow.Cells("CoverPage").Value IsNot DBNull.Value Then
                _coverPage = CType(selectedRow.Cells("CoverPage").Value, String)
            End If
            If selectedRow.Cells("SuperUser").Value IsNot DBNull.Value Then
                _superUser = CType(selectedRow.Cells("SuperUser").Value, Boolean)
            End If
            If selectedRow.Cells("PO_Accountant").Value IsNot DBNull.Value Then
                _poAccountant = CType(selectedRow.Cells("PO_Accountant").Value, Boolean)
            End If
            If selectedRow.Cells("Accountant").Value IsNot DBNull.Value Then
                _accountant = CType(selectedRow.Cells("Accountant").Value, Boolean)
            End If
            If selectedRow.Cells("Distributor").Value IsNot DBNull.Value Then
                _distributor = CType(selectedRow.Cells("Distributor").Value, Boolean)
            End If
            If selectedRow.Cells("FacilityCreditProcessor").Value IsNot DBNull.Value Then
                _facilitycreditprocessor = CType(selectedRow.Cells("FacilityCreditProcessor").Value, Boolean)
            End If
            If selectedRow.Cells("Buyer").Value IsNot DBNull.Value Then
                _buyer = CType(selectedRow.Cells("Buyer").Value, Boolean)
            End If
            If selectedRow.Cells("Coordinator").Value IsNot DBNull.Value Then
                _coordinator = CType(selectedRow.Cells("Coordinator").Value, Boolean)
            End If
            If selectedRow.Cells("Item_Administrator").Value IsNot DBNull.Value Then
                _itemAdministrator = CType(selectedRow.Cells("Item_Administrator").Value, Boolean)
            End If
            If selectedRow.Cells("Vendor_Administrator").Value IsNot DBNull.Value Then
                _vendorAdministrator = CType(selectedRow.Cells("Vendor_Administrator").Value, Boolean)
            End If
            If selectedRow.Cells("Lock_Administrator").Value IsNot DBNull.Value Then
                _lockAdministrator = CType(selectedRow.Cells("Lock_Administrator").Value, Boolean)
            End If
            If selectedRow.Cells("Warehouse").Value IsNot DBNull.Value Then
                _warehouse = CType(selectedRow.Cells("Warehouse").Value, Boolean)
            End If
            If selectedRow.Cells("CancelAllSales").Value IsNot DBNull.Value Then
                _cancelAllSales = CType(selectedRow.Cells("CancelAllSales").Value, Boolean)
            End If
            If selectedRow.Cells("PriceBatchProcessor").Value IsNot DBNull.Value Then
                _priceBatchProcessor = CType(selectedRow.Cells("PriceBatchProcessor").Value, Boolean)
            End If
            If selectedRow.Cells("Inventory_Administrator").Value IsNot DBNull.Value Then
                _inventoryAdministrator = CType(selectedRow.Cells("Inventory_Administrator").Value, Boolean)
            End If
            If selectedRow.Cells("BatchBuildOnly").Value IsNot DBNull.Value Then
                _batchBuildOnly = CType(selectedRow.Cells("BatchBuildOnly").Value, Boolean)
            End If
            If selectedRow.Cells("DCAdmin").Value IsNot DBNull.Value Then
                _dcAdmin = CType(selectedRow.Cells("DCAdmin").Value, Boolean)
            End If
            If selectedRow.Cells("DeletePO").Value IsNot DBNull.Value Then
                _deletePO = CType(selectedRow.Cells("DeletePO").Value, Boolean)
            End If
            If selectedRow.Cells("CostAdmin").Value IsNot DBNull.Value Then
                _costAdmin = CType(selectedRow.Cells("CostAdmin").Value, Boolean)
            End If
            If selectedRow.Cells("TaxAdministrator").Value IsNot DBNull.Value Then
                _costAdmin = CType(selectedRow.Cells("TaxAdministrator").Value, Boolean)
            End If
            If selectedRow.Cells("Telxon_Store_Limit").Value IsNot DBNull.Value Then
                _telxon_Store_Limit = CType(selectedRow.Cells("Telxon_Store_Limit").Value, Integer)
            End If
            If selectedRow.Cells("VendorCostDiscrepancyAdmin").Value IsNot DBNull.Value Then
                _VendorCostDiscrepancyAdmin = CType(selectedRow.Cells("VendorCostDiscrepancyAdmin").Value, Integer)
            End If
            If selectedRow.Cells("RecvLog_Store_Limit").Value IsNot DBNull.Value Then
                _recvLog_Store_Limit = CType(selectedRow.Cells("RecvLog_Store_Limit").Value, Integer)
            End If
            If selectedRow.Cells("PromoAccessLevel").Value IsNot DBNull.Value Then
                _promoAccessLevel = CType(selectedRow.Cells("PromoAccessLevel").Value, Int16)
            End If
            If selectedRow.Cells("POApprovalAdmin").Value IsNot DBNull.Value Then
                _VendorCostDiscrepancyAdmin = CType(selectedRow.Cells("POApprovalAdmin").Value, Integer)
            End If
            If selectedRow.Cells("POEditor").Value IsNot DBNull.Value Then
                _POEditor = CType(selectedRow.Cells("POEditor").Value, Integer)
            End If
            If selectedRow.Cells("Shrink").Value IsNot DBNull.Value Then
                _shrink = CType(selectedRow.Cells("Shrink").Value, Integer)
            End If
            If selectedRow.Cells("ShrinkAdmin").Value IsNot DBNull.Value Then
                _shrinkAdmin = CType(selectedRow.Cells("ShrinkAdmin").Value, Integer)
            End If
            If selectedRow.Cells("ApplicationConfigAdmin").Value IsNot DBNull.Value Then
                _ApplicationConfigAdmin = CType(selectedRow.Cells("ApplicationConfigAdmin").Value, Integer)
            End If
            If selectedRow.Cells("SystemConfigurationAdministrator").Value IsNot DBNull.Value Then
                _SystemConfigurationAdministrator = CType(selectedRow.Cells("SystemConfigurationAdministrator").Value, Integer)
            End If
            If selectedRow.Cells("DataAdministrator").Value IsNot DBNull.Value Then
                _DataAdministrator = CType(selectedRow.Cells("DataAdministrator").Value, Integer)
            End If
            If selectedRow.Cells("JobAdministrator").Value IsNot DBNull.Value Then
                _JobAdministrator = CType(selectedRow.Cells("JobAdministrator").Value, Integer)
            End If
            If selectedRow.Cells("POSInterfaceAdministrator").Value IsNot DBNull.Value Then
                _POSInterfaceAdministrator = CType(selectedRow.Cells("POSInterfaceAdministrator").Value, Integer)
            End If
            If selectedRow.Cells("StoreAdministrator").Value IsNot DBNull.Value Then
                _StoreAdministrator = CType(selectedRow.Cells("StoreAdministrator").Value, Integer)
            End If
            If selectedRow.Cells("SecurityAdministrator").Value IsNot DBNull.Value Then
                _SecurityAdministrator = CType(selectedRow.Cells("SecurityAdministrator").Value, Integer)
            End If
            If selectedRow.Cells("UserMaintenance").Value IsNot DBNull.Value Then
                _UserMaintenance = CType(selectedRow.Cells("UserMaintenance").Value, Integer)
            End If

            ' SLIM access attributes

            If selectedRow.Cells("UserAdmin").Value IsNot DBNull.Value Then
                Me._slimUserAdmin = CType(selectedRow.Cells("UserAdmin").Value, Boolean)
            End If
            If selectedRow.Cells("ItemRequest").Value IsNot DBNull.Value Then
                Me._slimItemRequest = CType(selectedRow.Cells("ItemRequest").Value, Boolean)
            End If
            If selectedRow.Cells("VendorRequest").Value IsNot DBNull.Value Then
                Me._slimVendorRequest = CType(selectedRow.Cells("VendorRequest").Value, Boolean)
            End If
            If selectedRow.Cells("IRMAPush").Value IsNot DBNull.Value Then
                Me._slimPushToIRMA = CType(selectedRow.Cells("IRMAPush").Value, Boolean)
            End If
            If selectedRow.Cells("StoreSpecials").Value IsNot DBNull.Value Then
                Me._slimStoreSpecials = CType(selectedRow.Cells("StoreSpecials").Value, Boolean)
            End If
            If selectedRow.Cells("RetailCost").Value IsNot DBNull.Value Then
                Me._slimRetailCost = CType(selectedRow.Cells("RetailCost").Value, Boolean)
            End If
            If selectedRow.Cells("Authorizations").Value IsNot DBNull.Value Then
                Me._slimAuthorizations = CType(selectedRow.Cells("Authorizations").Value, Boolean)
            End If
            If selectedRow.Cells("WebQuery").Value IsNot DBNull.Value Then
                Me._slimSecureQuery = CType(selectedRow.Cells("WebQuery").Value, Boolean)
            End If
            If selectedRow.Cells("ScaleInfo").Value IsNot DBNull.Value Then
                Me._slimScaleInfo = CType(selectedRow.Cells("ScaleInfo").Value, Boolean)
            End If
            If selectedRow.Cells("ECommerce").Value IsNot DBNull.Value Then
                Me._slimAuthorizations = CType(selectedRow.Cells("ECommerce").Value, Boolean)
            End If

        End Sub
#End Region

#Region "Property access methods"

        Public Property UserId() As Integer
            Get
                Return _userId
            End Get
            Set(ByVal value As Integer)
                _userId = value
            End Set
        End Property

        Public Property AccountEnabled() As Boolean
            Get
                Return _accountEnabled
            End Get
            Set(ByVal value As Boolean)
                _accountEnabled = value
            End Set
        End Property

        Public Property UserName() As String
            Get
                Return _userName
            End Get
            Set(ByVal value As String)
                _userName = value
            End Set
        End Property

        Public Property FullName() As String
            Get
                Return _fullName
            End Get
            Set(ByVal value As String)
                _fullName = value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return _email
            End Get
            Set(ByVal value As String)
                _email = value
            End Set
        End Property

        Public Property PagerEmail() As String
            Get
                Return _pagerEmail
            End Get
            Set(ByVal value As String)
                _pagerEmail = value
            End Set
        End Property

        Public Property PhoneNumber() As String
            Get
                Return _phoneNumber
            End Get
            Set(ByVal value As String)
                _phoneNumber = value
            End Set
        End Property

        Public Property FaxNumber() As String
            Get
                Return _faxNumber
            End Get
            Set(ByVal value As String)
                _faxNumber = value
            End Set
        End Property

        Public Property Title() As Integer
            Get
                Return _title
            End Get
            Set(ByVal value As Integer)
                _title = value
            End Set
        End Property

        Public Property Printer() As String
            Get
                Return _printer
            End Get
            Set(ByVal value As String)
                _printer = value
            End Set
        End Property

        Public Property CoverPage() As String
            Get
                Return _coverPage
            End Get
            Set(ByVal value As String)
                _coverPage = value
            End Set
        End Property

        Public Property SuperUser() As Boolean
            Get
                Return _superUser
            End Get
            Set(ByVal value As Boolean)
                _superUser = value
            End Set
        End Property

        Public Property POAccountant() As Boolean
            Get
                Return _poAccountant
            End Get
            Set(ByVal value As Boolean)
                _poAccountant = value
            End Set
        End Property

        Public Property Accountant() As Boolean
            Get
                Return _accountant
            End Get
            Set(ByVal value As Boolean)
                _accountant = value
            End Set
        End Property

        Public Property Distributor() As Boolean
            Get
                Return _distributor
            End Get
            Set(ByVal value As Boolean)
                _distributor = value
            End Set
        End Property

        Public Property FacilityCreditProcessor() As Boolean
            Get
                Return _facilitycreditprocessor
            End Get
            Set(ByVal value As Boolean)
                _facilitycreditprocessor = value
            End Set
        End Property

        Public Property Buyer() As Boolean
            Get
                Return _buyer
            End Get
            Set(ByVal value As Boolean)
                _buyer = value
            End Set
        End Property

        Public Property Coordinator() As Boolean
            Get
                Return _coordinator
            End Get
            Set(ByVal value As Boolean)
                _coordinator = value
            End Set
        End Property

        Public Property ItemAdministrator() As Boolean
            Get
                Return _itemAdministrator
            End Get
            Set(ByVal value As Boolean)
                _itemAdministrator = value
            End Set
        End Property

        Public Property VendorAdministrator() As Boolean
            Get
                Return _vendorAdministrator
            End Get
            Set(ByVal value As Boolean)
                _vendorAdministrator = value
            End Set
        End Property

        Public Property LockAdministrator() As Boolean
            Get
                Return _lockAdministrator
            End Get
            Set(ByVal value As Boolean)
                _lockAdministrator = value
            End Set
        End Property

        Public Property Warehouse() As Boolean
            Get
                Return _warehouse
            End Get
            Set(ByVal value As Boolean)
                _warehouse = value
            End Set
        End Property

        Public Property CancellAllSales() As Boolean
            Get
                Return _cancelAllSales
            End Get
            Set(ByVal value As Boolean)
                _cancelAllSales = value
            End Set
        End Property
        Public Property PriceBatchProcessor() As Boolean
            Get
                Return _priceBatchProcessor
            End Get
            Set(ByVal value As Boolean)
                _priceBatchProcessor = value
            End Set
        End Property

        Public Property EInvoicingAdministrator() As Boolean
            Get
                Return _EInvoicingAdmin
            End Get
            Set(ByVal value As Boolean)
                _EInvoicingAdmin = value
            End Set
        End Property
        Public Property InventoryAdministrator() As Boolean
            Get
                Return _inventoryAdministrator
            End Get
            Set(ByVal value As Boolean)
                _inventoryAdministrator = value
            End Set
        End Property

        Public Property BatchBuildOnly() As Boolean
            Get
                Return _batchBuildOnly
            End Get
            Set(ByVal value As Boolean)
                _batchBuildOnly = value
            End Set
        End Property

        Public Property DCAdmin() As Boolean
            Get
                Return _dcAdmin
            End Get
            Set(ByVal value As Boolean)
                _dcAdmin = value
            End Set
        End Property

        Public Property DeletePO() As Boolean
            Get
                Return _deletePO
            End Get
            Set(ByVal value As Boolean)
                _deletePO = value
            End Set
        End Property
        Public Property CostAdmin() As Boolean
            Get
                Return _costAdmin
            End Get
            Set(ByVal value As Boolean)
                _costAdmin = value
            End Set
        End Property
        Public Property VendorCostDiscrepancyAdmin() As Boolean
            Get
                Return _VendorCostDiscrepancyAdmin
            End Get
            Set(ByVal value As Boolean)
                _VendorCostDiscrepancyAdmin = value
            End Set
        End Property
        Public Property DeleteAccess() As Boolean
            Get
                Return _delete_Access
            End Get
            Set(ByVal value As Boolean)
                _delete_Access = value
            End Set
        End Property

        Public Property RecvLogStoreLimit() As Integer
            Get
                Return _recvLog_Store_Limit
            End Get
            Set(ByVal value As Integer)
                _recvLog_Store_Limit = value
            End Set
        End Property

        Public Property TaxAdministrator() As Boolean
            Get
                Return _taxAdministrator
            End Get
            Set(ByVal value As Boolean)
                _taxAdministrator = value
            End Set
        End Property

        Public Property TelxonStoreLimit() As Integer
            Get
                Return _telxon_Store_Limit
            End Get
            Set(ByVal value As Integer)
                _telxon_Store_Limit = value
            End Set
        End Property

        Public Property TelxonEnabled() As Boolean
            Get
                Return _telxon_Enabled
            End Get
            Set(ByVal value As Boolean)
                _telxon_Enabled = value
            End Set
        End Property

        Public Property TelxonWaste() As Boolean
            Get
                Return _telxon_Waste
            End Get
            Set(ByVal value As Boolean)
                _telxon_Waste = value
            End Set
        End Property

        Public Property TelxonCycleCount() As Boolean
            Get
                Return _telxon_Cycle_Count
            End Get
            Set(ByVal value As Boolean)
                _telxon_Cycle_Count = value
            End Set
        End Property

        Public Property TelxonDistribution() As Boolean
            Get
                Return _telxon_Distribution
            End Get
            Set(ByVal value As Boolean)
                _telxon_Distribution = value
            End Set
        End Property

        Public Property TelxonTransfers() As Boolean
            Get
                Return _telxon_Transfers
            End Get
            Set(ByVal value As Boolean)
                _telxon_Transfers = value
            End Set
        End Property

        Public Property TelxonPriceCheck() As Boolean
            Get
                Return _telxon_Price_Check
            End Get
            Set(ByVal value As Boolean)
                _telxon_Price_Check = value
            End Set
        End Property

        Public Property TelxonSuperuser() As Boolean
            Get
                Return _telxon_Superuser
            End Get
            Set(ByVal value As Boolean)
                _telxon_Superuser = value
            End Set
        End Property

        Public Property TelxonOrders() As Boolean
            Get
                Return _telxon_Orders
            End Get
            Set(ByVal value As Boolean)
                _telxon_Orders = value
            End Set
        End Property

        Public Property VendorLimit() As Integer
            Get
                Return _vendorLimit
            End Get
            Set(ByVal value As Integer)
                _vendorLimit = value
            End Set
        End Property

        ' used for PromoPlanner access
        Public Property PromoAccessLevel() As Int16
            Get
                Return _promoAccessLevel
            End Get
            Set(ByVal value As Int16)
                _promoAccessLevel = value
            End Set
        End Property

        Public Property POApprovalAdmin() As Boolean
            Get
                Return _POApprovalAdmin
            End Get
            Set(ByVal value As Boolean)
                _POApprovalAdmin = value
            End Set
        End Property

        Public Property POEditor() As Boolean
            Get
                Return _POEditor
            End Get
            Set(ByVal value As Boolean)
                _POEditor = value
            End Set
        End Property

        Public Property ApplicationConfigAdmin() As Boolean
            Get
                Return _ApplicationConfigAdmin
            End Get
            Set(ByVal value As Boolean)
                _ApplicationConfigAdmin = value
            End Set
        End Property

        Public Property SystemConfigurationAdministrator() As Boolean
            Get
                Return _SystemConfigurationAdministrator
            End Get
            Set(ByVal value As Boolean)
                _SystemConfigurationAdministrator = value
            End Set
        End Property

        Public Property DataAdministrator() As Boolean
            Get
                Return _DataAdministrator
            End Get
            Set(ByVal value As Boolean)
                _DataAdministrator = value
            End Set
        End Property

        Public Property JobAdministrator() As Boolean
            Get
                Return _JobAdministrator
            End Get
            Set(ByVal value As Boolean)
                _JobAdministrator = value
            End Set
        End Property

        Public Property POSInterfaceAdministrator() As Boolean
            Get
                Return _POSInterfaceAdministrator
            End Get
            Set(ByVal value As Boolean)
                _POSInterfaceAdministrator = value
            End Set
        End Property

        Public Property StoreAdministrator() As Boolean
            Get
                Return _StoreAdministrator
            End Get
            Set(ByVal value As Boolean)
                _StoreAdministrator = value
            End Set
        End Property

        Public Property SecurityAdministrator() As Boolean
            Get
                Return _SecurityAdministrator
            End Get
            Set(ByVal value As Boolean)
                _SecurityAdministrator = value
            End Set
        End Property

        Public Property UserMaintenance() As Boolean
            Get
                Return _UserMaintenance
            End Get
            Set(ByVal value As Boolean)
                _UserMaintenance = value
            End Set
        End Property

        ' access permissions specific to the SLIM web app
        Public Property SLIMUserAdmin() As Boolean
            Get
                Return Me._slimUserAdmin
            End Get
            Set(ByVal value As Boolean)
                Me._slimUserAdmin = value
            End Set
        End Property

        Public Property SLIMItemRequest() As Boolean
            Get
                Return Me._slimItemRequest
            End Get
            Set(ByVal value As Boolean)
                Me._slimItemRequest = value
            End Set
        End Property

        Public Property SLIMVendorRequest() As Boolean
            Get
                Return Me._slimVendorRequest
            End Get
            Set(ByVal value As Boolean)
                Me._slimVendorRequest = value
            End Set
        End Property

        Public Property SLIMPushToIRMA() As Boolean
            Get
                Return Me._slimPushToIRMA
            End Get
            Set(ByVal value As Boolean)
                Me._slimPushToIRMA = value
            End Set
        End Property

        Public Property SLIMStoreSpecials() As Boolean
            Get
                Return Me._slimStoreSpecials
            End Get
            Set(ByVal value As Boolean)
                Me._slimStoreSpecials = value
            End Set
        End Property

        Public Property SLIMRetailCost() As Boolean
            Get
                Return Me._slimRetailCost
            End Get
            Set(ByVal value As Boolean)
                Me._slimRetailCost = value
            End Set
        End Property

        Public Property SLIMAuthorizations() As Boolean
            Get
                Return Me._slimAuthorizations
            End Get
            Set(ByVal value As Boolean)
                Me._slimAuthorizations = value
            End Set
        End Property

        Public Property SLIMECommerce() As Boolean
            Get
                Return Me._slimECommerce
            End Get
            Set(ByVal value As Boolean)
                Me._slimECommerce = value
            End Set
        End Property

        Public Property SLIMSecureQuery() As Boolean
            Get
                Return Me._slimSecureQuery
            End Get
            Set(ByVal value As Boolean)
                Me._slimSecureQuery = value
            End Set
        End Property

        Public Property SLIMScaleInfo() As Boolean
            Get
                Return Me._slimScaleInfo
            End Get
            Set(ByVal value As Boolean)
                Me._slimScaleInfo = value
            End Set
        End Property

        Public Property Shrink() As Boolean
            Get
                Return Me._shrink
            End Get
            Set(ByVal value As Boolean)
                Me._shrink = value
            End Set
        End Property

        Public Property ShrinkAdmin() As Boolean
            Get
                Return Me._shrinkAdmin
            End Get
            Set(ByVal value As Boolean)
                Me._shrinkAdmin = value
            End Set
        End Property
#End Region

#Region "Business rules"
        ''' <summary>
        ''' validates data elements of current instance of UserBO object
        ''' </summary>
        ''' <returns>ArrayList of UserConfigStatus values</returns>
        ''' <remarks></remarks>
        Public Function ValidateUserConfigData() As ArrayList
            Dim errorList As New ArrayList

            ' required fields
            If _userName.Equals("") Then
                errorList.Add(UserConfigStatus.Error_Required_UserName)
            End If
            If _fullName.Equals("") Then
                errorList.Add(UserConfigStatus.Error_Required_FullName)
            End If

            'no errors - return Valid status
            If errorList.Count = 0 Then
                errorList.Add(UserConfigStatus.Valid)
            End If

            Return errorList
        End Function
#End Region

    End Class
End Namespace
