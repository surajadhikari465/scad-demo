Imports log4net
Public Class User
    Dim m_accountant As Boolean
    Dim m_account_enabled As Boolean
    Dim m_buyer As Boolean
    Dim m_coordinator As Boolean
    Dim m_facilitycreditprocessor As Boolean
    Dim m_delete_access As Boolean
    Dim m_distributor As Boolean
    Dim m_full_name As String
    Dim m_inventory_administrator As Boolean
    Dim m_item_administrator As Boolean
    Dim m_lock_administrator As Boolean
    Dim m_po_accountant As Boolean
    Dim m_price_batch_processor As Boolean
    Dim m_recv_log_store_limit As Integer
    Dim m_super_user As Boolean
    Dim m_team_leader_in_store As Boolean
    Dim m_store_limit As Integer
    Dim m_store_limit_vendor_id As Integer
    Dim m_user_id As Integer
    Dim m_vendor_admin As Boolean
    Dim m_warehouse As Boolean

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public ReadOnly Property IsAccountant() As Boolean
        Get
            Return m_accountant
        End Get
    End Property
    Public ReadOnly Property IsAccountEnabled() As Boolean
        Get
            Return m_account_enabled
        End Get
    End Property
    Public ReadOnly Property IsBuyer() As Boolean
        Get
            Return m_buyer
        End Get
    End Property
    Public ReadOnly Property IsCoordinator() As Boolean
        Get
            Return m_coordinator
        End Get
    End Property
    Public ReadOnly Property IsFacilityCreditProcessor() As Boolean
        Get
            Return Me.m_facilitycreditprocessor
        End Get
    End Property
    Public ReadOnly Property IsInventoryAdministrator() As Boolean
        Get
            Return Me.m_inventory_administrator
        End Get
    End Property
    Public ReadOnly Property IsItemAdministrator() As Boolean
        Get
            Return Me.m_item_administrator
        End Get
    End Property
    Public ReadOnly Property IsLockAdministrator() As Boolean
        Get
            Return Me.m_lock_administrator
        End Get
    End Property
    Public ReadOnly Property IsPOAccountant() As Boolean
        Get
            Return Me.m_po_accountant
        End Get
    End Property
    Public ReadOnly Property IsPriceBatchProcessor() As Boolean
        Get
            Return Me.m_price_batch_processor
        End Get
    End Property
    Public ReadOnly Property IsReceiver() As Boolean
        Get
            Return Me.m_distributor
        End Get
    End Property

    Public ReadOnly Property IsSuperUser() As Boolean
        Get
            Return Me.m_super_user
        End Get
    End Property
    Public ReadOnly Property HasDeleteAccess() As Boolean
        Get
            Return Me.m_delete_access
        End Get
    End Property
    Public ReadOnly Property IsVendorAdministrator() As Boolean
        Get
            Return Me.m_vendor_admin
        End Get
    End Property
    Public ReadOnly Property IsWarehouse() As Boolean
        Get
            Return Me.m_warehouse
        End Get
    End Property
    Public ReadOnly Property ReceivingLogStoreLimit() As Int32
        Get
            Return Me.m_recv_log_store_limit
        End Get
    End Property
    Public ReadOnly Property StoreLimit() As Int32
        Get
            Return Me.m_store_limit
        End Get
    End Property
    Public ReadOnly Property StoreLimitVendorID() As Int32
        Get
            Return Me.m_store_limit_vendor_id
        End Get
    End Property
    Public ReadOnly Property User_ID() As Int32
        Get
            Return m_user_id
        End Get
    End Property
    Public ReadOnly Property FullName() As String
        Get
            Return m_full_name
        End Get
    End Property
    Public Sub New(ByVal sUserName As String, ByVal sPassword As String)
        '    Dim cmd As System.Data.SqlClient.SqlCommand
        '    Dim dr As System.Data.SqlClient.SqlDataReader
        '    Try
        '        Dim sLogonErrMsg As String = WFM.UserAuthentication.Windows.ValidUser(sUserName, sPassword)
        '        If sLogonErrMsg.Length > 0 Then Throw New ItemCatalog.Exception.InvalidLogonException(sLogonErrMsg)
        '        cmd = New System.Data.SqlClient.SqlCommand
        '        cmd.CommandType = CommandType.StoredProcedure
        '        cmd.CommandText = "ValidateLogin"
        '        Dim prm As New System.Data.SqlClient.SqlParameter
        '        prm.Direction = ParameterDirection.Input
        '        prm.DbType = DbType.String
        '        prm.ParameterName = "@UserName"
        '        prm.Value = sUserName
        '        cmd.Parameters.Add(prm)
        '        dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
        '        If dr.HasRows Then
        '            dr.Read()
        '            ConstructUser(dr)
        '        Else
        '            Throw New ItemCatalog.Exception.InvalidLogonException("Invalid IRMA User Name.  Contact the helpdesk for assistance.")
        '        End If
        '    Finally
        '        ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
        '        ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        '    End Try
    End Sub
    Public Sub New(ByVal lUserID As Long)
        'Dim cmd As System.Data.SqlClient.SqlCommand
        'Dim dr As System.Data.SqlClient.SqlDataReader
        'Try
        '    cmd = New System.Data.SqlClient.SqlCommand
        '    cmd.CommandType = CommandType.StoredProcedure
        '    cmd.CommandText = "GetUser"
        '    Dim prm As New System.Data.SqlClient.SqlParameter
        '    prm.Direction = ParameterDirection.Input
        '    prm.DbType = DbType.Int32
        '    prm.ParameterName = "@User_ID"
        '    prm.Value = lUserID
        '    cmd.Parameters.Add(prm)
        '    dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
        '    If dr.HasRows Then
        '        dr.Read()
        '        ConstructUser(dr)
        '    Else
        '        Throw New System.Exception("User_ID supplied does not exist")
        '    End If
        'Finally
        '    ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
        '    ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        'End Try
    End Sub
    Private Sub ConstructUser(ByVal dr As System.Data.SqlClient.SqlDataReader)

        logger.Debug("ConstructUser Entry")

        m_super_user = CBool(dr!SuperUser)
        m_po_accountant = CBool(dr!PO_Accountant) Or m_super_user
        m_accountant = CBool(dr!Accountant) Or CBool(dr!PO_Accountant)
        m_account_enabled = CBool(dr!AccountEnabled)
        m_coordinator = CBool(dr!Coordinator) Or m_po_accountant Or m_super_user
        m_buyer = CBool(dr!Buyer) Or m_coordinator Or m_super_user
        m_facilitycreditprocessor = CBool(dr!FacilityCreditProcessor) Or m_super_user
        m_delete_access = CBool(dr!Delete_Access) Or m_super_user
        m_distributor = CBool(dr!Distributor) Or m_po_accountant Or m_super_user
        m_full_name = CStr(dr!FullName)
        m_inventory_administrator = CBool(dr!Inventory_Administrator) Or m_super_user
        m_lock_administrator = CBool(dr!Lock_Administrator) Or m_super_user
        m_price_batch_processor = CBool(dr!PriceBatchProcessor) Or m_super_user
        If Not IsDBNull(dr!RecvLog_Store_Limit) Then m_recv_log_store_limit = CInt(dr!RecvLog_Store_Limit)
        m_item_administrator = CBool(dr!Item_Administrator)
        If Not IsDBNull(dr!Telxon_Store_Limit) Then m_store_limit = CInt(dr!Telxon_Store_Limit)
        If Not IsDBNull(dr!Vendor_Limit) Then m_store_limit_vendor_id = CInt(dr!Vendor_Limit)
        m_user_id = CInt(dr!User_ID)
        m_vendor_admin = CBool(dr!Vendor_Administrator) Or m_super_user
        m_warehouse = CBool(dr!Warehouse) Or m_super_user

        logger.Debug("ConstructUser Exit")

    End Sub
End Class
