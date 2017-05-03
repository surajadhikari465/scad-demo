Namespace IRMA.Security

    <DataContract()> _
    Public Class UserRole
        Private _userID As Integer
        Private _facilityCreditProcessor As Boolean
        Private _accountEnabled As Boolean
        Private _superUser As Boolean
        Private _poAccountant As Boolean
        Private _accountant As Boolean
        Private _distributor As Boolean
        Private _buyer As Boolean
        Private _itemAdministrator As Boolean
        Private _coordinator As Boolean
        Private _vendorAdministrator As Boolean
        Private _lockAdminstrator As Boolean
        Private _telxonStoreLimit As Integer
        Private _warehouse As Boolean
        Private _priceBatchProcessor As Boolean
        Private _inventory_Administrator As Boolean
        Private _dcAdmin As Boolean
        Private _costAdmin As Boolean
        Private _shrinkAdmin As Boolean
        Private _shrink As Boolean
        Private _applicationConfigAdmin As Boolean
        Private _jobAdministrator As Boolean
        Private _pOSInterfaceAdministrator As Boolean
        Private _securityAdministrator As Boolean
        Private _storeAdministrator As Boolean
        Private _systemConfigurationAdministrator As Boolean
        Private _userMaintenance As Boolean
        Private _pOApprovalAdmin As Boolean
        Private _vendorCostDiscrepancyAdmin As Boolean
        Private _eInvoicingAdministrator As Boolean
        Private _dataAdministrator As Boolean


        <DataMember()> _
        Public Property UserID() As Integer
            Get
                Return _userID
            End Get
            Set(ByVal value As Integer)
                _userID = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsAccountEnabled() As Boolean
            Get
                Return _accountEnabled
            End Get
            Set(ByVal value As Boolean)
                _accountEnabled = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsSuperUser() As Boolean
            Get
                Return _superUser
            End Get
            Set(ByVal value As Boolean)
                _superUser = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsPOAccountant() As Boolean
            Get
                Return _poAccountant
            End Get
            Set(ByVal value As Boolean)
                _poAccountant = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsAccountant() As Boolean
            Get
                Return _accountant
            End Get
            Set(ByVal value As Boolean)
                _accountant = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsDistributor() As Boolean
            Get
                Return _distributor
            End Get
            Set(ByVal value As Boolean)
                _distributor = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsFacilityCreditProcessor() As Boolean
            Get
                Return _facilityCreditProcessor
            End Get
            Set(ByVal value As Boolean)
                _facilityCreditProcessor = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsBuyer() As Boolean
            Get
                Return _buyer
            End Get
            Set(ByVal value As Boolean)
                _buyer = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsCoordinator() As Boolean
            Get
                Return _coordinator
            End Get
            Set(ByVal value As Boolean)
                _coordinator = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsItemAdministrator() As Boolean
            Get
                Return _itemAdministrator
            End Get
            Set(ByVal value As Boolean)
                _itemAdministrator = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsVendorAdministrator() As Boolean
            Get
                Return _vendorAdministrator
            End Get
            Set(ByVal value As Boolean)
                _vendorAdministrator = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsLockAdministrator() As Boolean
            Get
                Return _lockAdminstrator
            End Get
            Set(ByVal value As Boolean)
                _lockAdminstrator = value
            End Set
        End Property

        <DataMember()> _
        Public Property TelxonStoreLimit() As Integer
            Get
                Return _telxonStoreLimit
            End Get
            Set(ByVal value As Integer)
                _telxonStoreLimit = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsWarehouse() As Boolean
            Get
                Return _warehouse
            End Get
            Set(ByVal value As Boolean)
                _warehouse = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsPricebatchProcessor() As Boolean
            Get
                Return _priceBatchProcessor
            End Get
            Set(ByVal value As Boolean)
                _priceBatchProcessor = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsInventoryAdministrator() As Boolean
            Get
                Return _inventory_Administrator
            End Get
            Set(ByVal value As Boolean)
                _inventory_Administrator = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsDCAdmin() As Boolean
            Get
                Return _dcAdmin
            End Get
            Set(ByVal value As Boolean)
                If IsDBNull(value) Then
                    _dcAdmin = False
                Else
                    _dcAdmin = value
                End If
            End Set
        End Property

        <DataMember()> _
        Public Property IsCostAdmin() As Boolean
            Get
                Return _costAdmin
            End Get
            Set(ByVal value As Boolean)
                _costAdmin = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsShrink() As Boolean
            Get
                Return _shrink
            End Get
            Set(ByVal value As Boolean)
                _shrink = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsShrinkAdmin() As Boolean
            Get
                Return _shrinkAdmin
            End Get
            Set(ByVal value As Boolean)
                _shrinkAdmin = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsApplicationConfigAdmin() As Boolean
            Get
                Return _applicationConfigAdmin
            End Get
            Set(ByVal value As Boolean)
                _applicationConfigAdmin = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsDataAdministrator() As Boolean
            Get
                Return _dataAdministrator
            End Get
            Set(ByVal value As Boolean)
                _dataAdministrator = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsJobAdministrator() As Boolean
            Get
                Return _jobAdministrator
            End Get
            Set(ByVal value As Boolean)
                _jobAdministrator = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsPOSInterfaceAdministrator() As Boolean
            Get
                Return _pOSInterfaceAdministrator
            End Get
            Set(ByVal value As Boolean)
                _pOSInterfaceAdministrator = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsSecurityAdministrator() As Boolean
            Get
                Return _securityAdministrator
            End Get
            Set(ByVal value As Boolean)
                _securityAdministrator = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsStoreAdministrator() As Boolean
            Get
                Return _storeAdministrator
            End Get
            Set(ByVal value As Boolean)
                _storeAdministrator = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsSystemConfigurationAdministrator() As Boolean
            Get
                Return _systemConfigurationAdministrator
            End Get
            Set(ByVal value As Boolean)
                _systemConfigurationAdministrator = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsUserMaintenance() As Boolean
            Get
                Return _userMaintenance
            End Get
            Set(ByVal value As Boolean)
                _userMaintenance = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsPOApprovalAdmin() As Boolean
            Get
                Return _pOApprovalAdmin
            End Get
            Set(ByVal value As Boolean)
                _pOApprovalAdmin = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsEInvoicingAdministator() As Boolean
            Get
                Return _eInvoicingAdministrator
            End Get
            Set(ByVal value As Boolean)
                _eInvoicingAdministrator = value
            End Set
        End Property

        <DataMember()> _
        Public Property IsVendorCostDiscrepancyAdmin() As Boolean
            Get
                Return _vendorCostDiscrepancyAdmin
            End Get
            Set(ByVal value As Boolean)
                _vendorCostDiscrepancyAdmin = value
            End Set
        End Property

    End Class

End Namespace