Imports WFM.DataAccess.Setup
Public Class InventoryLocation
    Dim m_id As Long
    Dim m_name As String
    Dim m_desc As String
    Dim m_store_no As Long
    Dim m_subteam_no As Long
    Dim m_notes As String
    Dim m_items As Long()
    Public Property ID() As Long
        Get
            Return Me.m_id
        End Get
        Set(ByVal Value As Long)
            Me.m_id = Value
        End Set
    End Property
    Public Property Name() As String
        Get
            Return Me.m_name
        End Get
        Set(ByVal Value As String)
            Me.m_name = Value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return Me.m_desc
        End Get
        Set(ByVal Value As String)
            Me.m_desc = Value
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
    Public Property Notes() As String
        Get
            Return Me.m_notes
        End Get
        Set(ByVal Value As String)
            Me.m_notes = Value
        End Set
    End Property
    Protected Friend Sub New(ByVal ID As Long, ByVal Store_No As Long, ByVal SubTeam_No As Long, ByVal Name As String, ByVal Description As String)
        Me.m_id = ID
        Me.m_desc = Description
        Me.m_name = Name
        Me.m_store_no = Store_No
        Me.m_subteam_no = SubTeam_No
    End Sub
    Protected Friend Sub New(ByVal ID As Long, ByVal Store_No As Long, ByVal SubTeam_No As Long, ByVal Name As String, ByVal Description As String, ByVal Notes As String)
        Me.m_id = ID
        Me.m_desc = Description
        Me.m_name = Name
        Me.m_store_no = Store_No
        Me.m_subteam_no = SubTeam_No
        Me.m_notes = Notes
    End Sub
    Public Sub AddItem(ByVal Item As ItemCatalog.StoreItem)
        If Item.StoreNo <> Me.m_store_no Then Throw New System.ApplicationException("Input item store must match inventory location store")
        If Item.SubTeam_No <> Me.m_subteam_no Then Throw New System.ApplicationException("Input item subteam must match inventory location subteam")
        If Not Me.m_items Is Nothing Then
            If Array.BinarySearch(Me.m_items, Item.Item_Key) >= 0 Then
                Throw New ItemCatalog.Exception.InventoryLocationItemExistsException
            End If
        End If
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "InsertInventoryLocationItem"
            cmd.Parameters.Add(CreateParam("@InvLocID", SqlDbType.Int, ParameterDirection.Input, CObj(Me.m_id)))
            cmd.Parameters.Add(CreateParam("@Item_Key", SqlDbType.Int, ParameterDirection.Input, CObj(Item.Item_Key)))
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
            If Me.m_items Is Nothing Then
                ReDim Me.m_items(0)
            Else
                ReDim Preserve Me.m_items(Me.m_items.GetUpperBound(0) + 1)
            End If
            Me.m_items(Me.m_items.GetUpperBound(0)) = Item.Item_Key
            Array.Sort(Me.m_items)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Shared Function GetInventoryLocation(ByVal ID As Long) As ItemCatalog.InventoryLocation
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Dim loc As ItemCatalog.InventoryLocation
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetInventoryLocation"
            cmd.Parameters.Add(CreateParam("@InvLoc_ID", SqlDbType.Int, ParameterDirection.Input, CObj(ID)))
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                dr.Read()
                loc = New ItemCatalog.InventoryLocation(ID, dr!Store_No, dr!SubTeam_No, dr!InvLoc_Name, IIf(IsDBNull(dr!InvLoc_Desc), String.Empty, dr!InvLoc_Desc), IIf(IsDBNull(dr!Notes), String.Empty, dr!Notes))
                ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
                cmd.Parameters.Clear()
                cmd.CommandText = "GetInventoryLocationItems"
                cmd.Parameters.Add(CreateParam("@InvLocID", SqlDbType.Int, ParameterDirection.Input, CObj(ID)))
                cmd.Parameters.Add(CreateParam("@Identifier", SqlDbType.VarChar, ParameterDirection.Input, CObj(System.DBNull.Value), 15))
                cmd.Parameters.Add(CreateParam("@ItemName", SqlDbType.VarChar, ParameterDirection.Input, CObj(System.DBNull.Value), 60))
                dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
                If dr.HasRows Then
                    While dr.Read
                        If loc.m_items Is Nothing Then
                            ReDim loc.m_items(0)
                        Else
                            ReDim Preserve loc.m_items(loc.m_items.GetUpperBound(0) + 1)
                        End If
                        loc.m_items(loc.m_items.GetUpperBound(0)) = dr!Item_Key
                    End While
                    Array.Sort(loc.m_items)
                End If
            End If
            Return loc
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
    Public Shared Function GetInventoryLocations(ByVal Store_No As Long, ByVal SubTeam_No As Long) As ArrayList
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            Dim results As New ArrayList
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetInventoryLocations"
            cmd.Parameters.Add(CreateParam("@StoreID", SqlDbType.Int, ParameterDirection.Input, CObj(Store_No)))
            cmd.Parameters.Add(CreateParam("@SubTeamID", SqlDbType.Int, ParameterDirection.Input, CObj(SubTeam_No)))
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                While dr.Read
                    results.Add(New ItemCatalog.InventoryLocation(dr!InvLoc_ID, Store_No, SubTeam_No, dr!InvLoc_Name, IIf(IsDBNull(dr!InvLoc_Desc), String.Empty, dr!InvLoc_Desc)))
                End While
            End If

            Return results
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
End Class
