Imports WFM.DataAccess.Setup
Public Enum enuCycleCountSortOrder
    SubTeam_No = 0
End Enum
Public Enum enuCycleCountHeaderSortOrder
    InventoryLocationID = 0
End Enum
Public Class CycleCount
    Dim m_master_count_id As Long
    Dim m_store_no As Long
    Dim m_subteam_no As Long
    Dim m_end_scan As Date
    Dim m_closed As Date
    Dim m_end_period As Boolean
    Dim m_internal_cycle_headers As ArrayList
    Dim m_external_cycle_header As ItemCatalog.CycleCount.ExternalCycleCountHeader
    Public Property ID() As Long
        Get
            Return Me.m_master_count_id
        End Get
        Set(ByVal Value As Long)
            Me.m_master_count_id = Value
        End Set
    End Property
    Public Property Closed() As Date
        Get
            Return Me.m_closed
        End Get
        Set(ByVal Value As Date)
            Me.m_closed = Value
        End Set
    End Property
    Public Property EndScan() As Date
        Get
            Return Me.m_end_scan
        End Get
        Set(ByVal Value As Date)
            Me.m_end_scan = Value
        End Set
    End Property
    Public Property IsEndOfPeriod() As Boolean
        Get
            Return Me.m_end_period
        End Get
        Set(ByVal Value As Boolean)
            Me.m_end_period = Value
        End Set
    End Property
    Public Property Store_No() As Long
        Get
            Return Me.m_store_no
        End Get
        Set(ByVal Value As Long)
            Me.m_store_no = Value
        End Set
    End Property
    Public Property SubTeam_No() As Long
        Get
            Return Me.m_subteam_no
        End Get
        Set(ByVal Value As Long)
            Me.m_subteam_no = Value
        End Set
    End Property
    Private Sub New()
    End Sub
    Protected Friend Sub New(ByVal Store_No As Long, ByVal SubTeam_No As Long)
        Me.m_store_no = Store_No
        Me.m_subteam_no = SubTeam_No
    End Sub
    Public Shared Function GetCycleCount(ByVal Store_No As Long, ByVal SubTeam_No As Long) As ItemCatalog.CycleCount
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            Dim cc As ItemCatalog.CycleCount = Nothing
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetCycleCountMasterList"
            cmd.Parameters.Add(CreateParam("@Store_No", SqlDbType.Int, ParameterDirection.Input, CObj(Store_No)))
            cmd.Parameters.Add(CreateParam("@SubTeam_No", SqlDbType.Int, ParameterDirection.Input, CObj(SubTeam_No)))
            cmd.Parameters.Add(CreateParam("@Status", SqlDbType.VarChar, ParameterDirection.Input, CObj("OPEN"), 10))
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                cc = New ItemCatalog.CycleCount
                dr.Read()
                cc.m_store_no = Store_No
                cc.m_subteam_no = SubTeam_No
                cc.m_master_count_id = dr!MasterCountID
                cc.m_end_scan = dr!EndScan
                cc.m_end_period = dr!EndofPeriod
                cc.m_closed = IIf(Not IsDBNull(dr!ClosedDate), dr!ClosedDate, Nothing)
                ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
                cmd.Parameters.Clear()
                cmd.CommandText = "GetCycleCountList"
                cmd.Parameters.Add(CreateParam("@MasterCountID", SqlDbType.Int, ParameterDirection.Input, CObj(cc.m_master_count_id)))
                cmd.Parameters.Add(CreateParam("@Name", SqlDbType.VarChar, ParameterDirection.Input, CObj(System.DBNull.Value), 50))
                cmd.Parameters.Add(CreateParam("@StartScan", SqlDbType.VarChar, ParameterDirection.Input, CObj(System.DBNull.Value), 25))
                cmd.Parameters.Add(CreateParam("@Status", SqlDbType.VarChar, ParameterDirection.Input, CObj("OPEN"), 10))
                dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
                If dr.HasRows Then
                    While dr.Read
                        If Not CBool(dr!External) Then
                            If cc.m_internal_cycle_headers Is Nothing Then cc.m_internal_cycle_headers = New ArrayList
                            cc.m_internal_cycle_headers.Add(New ItemCatalog.CycleCount.InternalCycleCountHeader(dr!CycleCountID, IIf(IsDBNull(dr!InvLoc_ID), 0, dr!InvLoc_ID), dr!StartScan))
                        Else
                            cc.m_external_cycle_header = New ItemCatalog.CycleCount.ExternalCycleCountHeader(dr!CycleCountID)
                        End If
                    End While
                End If
            End If
            Return cc
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
    Private Function CreateHeader(ByVal StartScan As Date, ByVal InventoryLocationID As Long, ByVal External As Boolean) As Object
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            If StartScan > IIf(Me.m_end_period, Me.m_end_scan.AddHours(36), Me.m_end_scan) Then
                Throw New ItemCatalog.Exception.CycleCount.ExceedsEndScanException(IIf(Me.m_end_period, Me.m_end_scan.AddHours(36), Me.m_end_scan))
            Else
                cmd = New System.Data.SqlClient.SqlCommand
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "InsertCycleCountHeader"
                cmd.Parameters.Add(CreateParam("@MasterCountID", SqlDbType.Int, ParameterDirection.Input, CObj(Me.m_master_count_id)))
                cmd.Parameters.Add(CreateParam("@InvLocID", SqlDbType.Int, ParameterDirection.Input, CObj(IIf(InventoryLocationID <> 0, InventoryLocationID, System.DBNull.Value))))
                cmd.Parameters.Add(CreateParam("@StartScan", SqlDbType.DateTime, ParameterDirection.Input, CObj(StartScan)))
                cmd.Parameters.Add(CreateParam("@External", SqlDbType.Bit, ParameterDirection.Input, CObj(External)))
                dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
                If dr.HasRows Then
                    dr.Read()
                    If External Then
                        Me.m_external_cycle_header = New ItemCatalog.CycleCount.ExternalCycleCountHeader(dr!Added)
                        Return Me.m_external_cycle_header
                    Else
                        Dim ch As New ItemCatalog.CycleCount.InternalCycleCountHeader(dr!Added, InventoryLocationID, StartScan)
                        If Me.m_internal_cycle_headers Is Nothing Then Me.m_internal_cycle_headers = New ArrayList
                        Me.m_internal_cycle_headers.Add(ch)
                        Return ch
                    End If
                Else
                    Throw New System.ApplicationException("CycleCountHeader creation failed - no CycleCountID returned")
                End If
            End If
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
    Public Sub AddItemExternal(ByVal StartScan As Date, ByVal SI As ItemCatalog.StoreItem, ByVal Quantity As Decimal, ByVal Weight As Decimal, ByVal PackSize As Decimal)

        If SI.StoreNo <> Me.Store_No Then Throw New System.ApplicationException("Cycle count add item must be for cycle count store")
        If SI.SubTeam_No <> Me.SubTeam_No Then Throw New System.ApplicationException("Cycle count add item must be for cycle count subteam")

        If Me.m_external_cycle_header Is Nothing Then CreateHeader(StartScan, 0, True)
        Me.m_external_cycle_header.AddItem(SI.Item_Key, Quantity, Weight, PackSize, False)

    End Sub
    Public Sub AddItemInternal(ByVal StartScan As Date, ByVal InventoryLocationID As Long, ByVal SI As ItemCatalog.StoreItem, ByVal Quantity As Decimal, ByVal Weight As Decimal, ByVal PackSize As Decimal, ByVal IsCaseCnt As Boolean)

        If SI.StoreNo <> Me.Store_No Then Throw New System.ApplicationException("Cycle count add item must be for cycle count store")
        If SI.SubTeam_No <> Me.SubTeam_No Then Throw New System.ApplicationException("Cycle count add item must be for cycle count subteam")

        Dim ch As ItemCatalog.CycleCount.InternalCycleCountHeader = GetInternalCycleCountHeader(InventoryLocationID)
        If ch Is Nothing Then ch = CreateHeader(StartScan, InventoryLocationID, False)
        If StartScan < ch.StartScan Then
            Throw New ItemCatalog.Exception.CycleCount.BeforeStartScanException(ch.StartScan)
        Else
            If StartScan > IIf(Me.m_end_period, Me.m_end_scan.AddHours(36), Me.m_end_scan) Then
                Throw New ItemCatalog.Exception.CycleCount.ExceedsEndScanException(IIf(Me.m_end_period, Me.m_end_scan.AddHours(36), Me.m_end_scan))
            Else
                ch.AddItem(SI.Item_Key, Quantity, Weight, PackSize, IsCaseCnt)
            End If
        End If

    End Sub
    Private Function GetInternalCycleCountHeader(ByVal InventoryLocationID As Long) As ItemCatalog.CycleCount.InternalCycleCountHeader

        If Not (Me.m_internal_cycle_headers Is Nothing) Then
            Me.m_internal_cycle_headers.Sort(New InternalCycleCountHeaderSort(enuCycleCountHeaderSortOrder.InventoryLocationID))
            Dim searchCCH As New ItemCatalog.CycleCount.InternalCycleCountHeader(InventoryLocationID)
            Dim i As Integer = m_internal_cycle_headers.BinarySearch(searchCCH, New InternalCycleCountHeaderGetInvLocID)
            If i >= 0 Then Return CType(m_internal_cycle_headers(i), CycleCount.InternalCycleCountHeader) Else Return Nothing
        Else
            Return Nothing
        End If

    End Function
    Public Class InternalCycleCountHeader
        Dim m_cycle_count_id As Long
        Dim m_inv_loc_id As Long
        Dim m_start_scan As Date
        Public Property ID() As Long
            Get
                Return Me.m_cycle_count_id
            End Get
            Set(ByVal Value As Long)
                Me.m_cycle_count_id = Value
            End Set
        End Property
        Public Property InventoryLocationID() As Long
            Get
                Return Me.m_inv_loc_id
            End Get
            Set(ByVal Value As Long)
                Me.m_inv_loc_id = Value
            End Set
        End Property
        Public Property StartScan() As Date
            Get
                Return Me.m_start_scan
            End Get
            Set(ByVal Value As Date)
                Me.m_start_scan = Value
            End Set
        End Property
        Protected Friend Sub New(ByVal InventoryLocationID As Long)
            Me.m_inv_loc_id = InventoryLocationID
        End Sub
        Protected Friend Sub New(ByVal ID As Long, ByVal InventoryLocationID As Long, ByVal StartScan As Date)
            Me.m_cycle_count_id = ID
            Me.m_inv_loc_id = InventoryLocationID
            Me.m_start_scan = StartScan
        End Sub
        Protected Friend Sub AddItem(ByVal Item_Key As Long, ByVal Quantity As Decimal, ByVal Weight As Decimal, ByVal PackSize As Decimal, ByVal IsCaseCnt As Boolean)
            Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
            Try
                cmd = New System.Data.SqlClient.SqlCommand
                cmd.CommandText = "InsertCycleCountItem2"
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add(CreateParam("@CycleCountID", SqlDbType.Int, ParameterDirection.Input, CObj(Me.m_cycle_count_id)))
                cmd.Parameters.Add(CreateParam("@Item_Key", SqlDbType.Int, ParameterDirection.Input, CObj(Item_Key)))
                cmd.Parameters.Add(CreateParam("@InvLocID", SqlDbType.Int, ParameterDirection.Input, CObj(IIf(Me.m_inv_loc_id <> 0, Me.m_inv_loc_id, System.DBNull.Value))))
                cmd.Parameters.Add(CreateParam("@ScanDateTime", SqlDbType.DateTime, ParameterDirection.Input, CObj(System.DBNull.Value)))
                cmd.Parameters.Add(CreateParam("@Count", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(Quantity)))
                cmd.Parameters.Add(CreateParam("@Weight", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(Weight)))
                cmd.Parameters.Add(CreateParam("@PackSize", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(PackSize)))
                cmd.Parameters.Add(CreateParam("@IsCaseCnt", SqlDbType.Bit, ParameterDirection.Input, CObj(IsCaseCnt)))
                ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
            Finally
                ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
            End Try
        End Sub
    End Class
    Public Class ExternalCycleCountHeader
        Dim m_cycle_count_id As Long
        Public Property ID() As Long
            Get
                Return Me.m_cycle_count_id
            End Get
            Set(ByVal Value As Long)
                Me.m_cycle_count_id = Value
            End Set
        End Property
        Protected Friend Sub New(ByVal ID As Long)
            Me.m_cycle_count_id = ID
        End Sub
        Protected Friend Sub AddItem(ByVal Item_Key As Long, ByVal Quantity As Decimal, ByVal Weight As Decimal, ByVal PackSize As Decimal, ByVal IsCaseCnt As Boolean)
            Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
            Try
                cmd = New System.Data.SqlClient.SqlCommand
                cmd.CommandText = "InsertCycleCountItem2"
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add(CreateParam("@CycleCountID", SqlDbType.Int, ParameterDirection.Input, CObj(Me.m_cycle_count_id)))
                cmd.Parameters.Add(CreateParam("@Item_Key", SqlDbType.Int, ParameterDirection.Input, CObj(Item_Key)))
                cmd.Parameters.Add(CreateParam("@InvLocID", SqlDbType.Int, ParameterDirection.Input, CObj(System.DBNull.Value)))
                cmd.Parameters.Add(CreateParam("@ScanDateTime", SqlDbType.DateTime, ParameterDirection.Input, CObj(System.DBNull.Value)))
                cmd.Parameters.Add(CreateParam("@Count", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(Quantity)))
                cmd.Parameters.Add(CreateParam("@Weight", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(Weight)))
                cmd.Parameters.Add(CreateParam("@PackSize", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(PackSize)))
                cmd.Parameters.Add(CreateParam("@IsCaseCnt", SqlDbType.Bit, ParameterDirection.Input, CObj(IsCaseCnt)))
                ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
            Finally
                ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
            End Try
        End Sub
    End Class
End Class
Public Class CycleCountSort
    Implements IComparer
    Dim CompType As enuCycleCountSortOrder
    Public Sub New(ByVal xCompType As enuCycleCountSortOrder)
        CompType = xCompType
    End Sub
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Select Case CompType
            Case enuCycleCountSortOrder.SubTeam_No
                Compare = CType(x, CycleCount).SubTeam_No.CompareTo(CType(y, CycleCount).SubTeam_No)
        End Select
    End Function
End Class
Public Class CycleCountGetSubTeam_No
    Implements IComparer

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Compare = CType(x, CycleCount).SubTeam_No.CompareTo(CType(y, CycleCount).SubTeam_No)
    End Function
End Class
Public Class InternalCycleCountHeaderSort
    Implements IComparer
    Dim CompType As enuCycleCountHeaderSortOrder
    Public Sub New(ByVal xCompType As enuCycleCountHeaderSortOrder)
        CompType = xCompType
    End Sub
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Select Case CompType
            Case enuCycleCountHeaderSortOrder.InventoryLocationID
                Compare = CType(x, CycleCount.InternalCycleCountHeader).InventoryLocationID.CompareTo(CType(y, CycleCount.InternalCycleCountHeader).InventoryLocationID)
        End Select
    End Function
End Class
Public Class InternalCycleCountHeaderGetInvLocID
    Implements IComparer

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Compare = CType(x, CycleCount.InternalCycleCountHeader).InventoryLocationID.CompareTo(CType(y, CycleCount.InternalCycleCountHeader).InventoryLocationID)
    End Function
End Class