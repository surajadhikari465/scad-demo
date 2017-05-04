Imports WFM.DataAccess.Setup
Public Class Store
    Inherits Facility
    Dim m_mega_store As Boolean
    Dim m_wfm_store As Boolean
    Dim m_shelfTagStockType As String = String.Empty
    Dim m_subteams As ArrayList
    Dim m_wireless_printers As ArrayList
    Public Property Mega_Store() As Boolean
        Get
            Return m_mega_store
        End Get
        Set(ByVal Value As Boolean)
            m_mega_store = Value
        End Set
    End Property
    Public Property ShelfTagStockType() As String
        Get
            Return m_shelfTagStockType
        End Get
        Set(ByVal Value As String)
            m_shelfTagStockType = Value
        End Set
    End Property
    Public Property WFM_Store() As Boolean
        Get
            Return m_wfm_store
        End Get
        Set(ByVal Value As Boolean)
            m_wfm_store = Value
        End Set
    End Property
    Public Sub New()
    End Sub
    Public Sub New(ByVal lStore_No As Long)
        MyBase.Store_No = lStore_No
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetStore"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Store_No"
            prm.Value = lStore_No
            cmd.Parameters.Add(prm)
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                dr.Read()

                Me.Store_Name = dr.Item("Store_Name")
                Me.m_mega_store = dr.Item("Mega_Store")
                Me.m_wfm_store = dr.Item("WFM_Store")
                'Me.m_shelfTagStockType = dr.Item("ShelfTagStockType")
            Else
                Throw New System.Exception("Invalid Store Number")
            End If
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Sub New(ByVal lStore_No As Long, ByVal sStore_Name As String, ByVal bMega_Store As Boolean, ByVal bWFM_Store As Boolean)
        Me.Store_No = lStore_No
        Me.Store_Name = sStore_Name
        Me.Mega_Store = bMega_Store
        Me.WFM_Store = bWFM_Store
    End Sub
    Public Function CustomerSearch(ByVal sFirstName As String, ByVal sLastName As String, ByVal sPhone As String, _
    ByVal sCity As String, ByVal sState As String, ByVal sZipCode As String) As ArrayList
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            Dim results As New ArrayList
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetCustomerSearch"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@FirstName"
            prm.DbType = DbType.String
            prm.Value = IIf(sFirstName.Trim = String.Empty, System.DBNull.Value, sFirstName.Trim)
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@LastName"
            prm.DbType = DbType.String
            prm.Value = IIf(sLastName.Trim = String.Empty, System.DBNull.Value, sLastName.Trim)
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@Phone"
            prm.DbType = DbType.String
            prm.Value = IIf(sPhone.Trim = String.Empty, System.DBNull.Value, sPhone.Trim)
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@City"
            prm.DbType = DbType.String
            prm.Value = IIf(sCity.Trim = String.Empty, System.DBNull.Value, sCity.Trim)
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@State"
            prm.DbType = DbType.String
            prm.Value = IIf(sState.Trim = String.Empty, System.DBNull.Value, sState.Trim)
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@ZipCode"
            prm.DbType = DbType.String
            prm.Value = IIf(sZipCode.Trim = String.Empty, System.DBNull.Value, sZipCode.Trim)
            cmd.Parameters.Add(prm)
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                While dr.Read
                    results.Add(New ItemCatalog.Customer(dr.GetInt32(0), dr.GetString(1), dr.GetString(2), dr.GetString(3), dr.GetString(4), dr.GetString(5), dr.GetString(6), dr.GetString(7), dr.GetString(8)))
                End While
            End If
            Return results
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
    Public Function ItemSearch(ByVal sDescription As String, ByVal sIdentifier As String) As ArrayList
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            Dim results As New ArrayList
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetStoreItemSearch"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@Store_No"
            prm.DbType = DbType.Int32
            prm.Value = Me.Store_No
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@SubTeam_No"
            prm.DbType = DbType.Int32
            prm.Value = 0
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@Category_ID"
            prm.DbType = DbType.Int32
            prm.Value = 0
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@Vendor"
            prm.DbType = DbType.String
            prm.Value = String.Empty
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@Vendor_ID"
            prm.DbType = DbType.String
            prm.Value = String.Empty
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@Item_Description"
            prm.DbType = DbType.String
            prm.Value = sDescription
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@Identifier"
            prm.DbType = DbType.String
            prm.Value = sIdentifier
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@Discontinue_Item"
            prm.DbType = DbType.Int32
            prm.Value = -1
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@WFM_Item"
            prm.DbType = DbType.Int32
            prm.Value = (m_wfm_store = 1)
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@Not_Available"
            prm.DbType = DbType.Int32
            prm.Value = 0
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@HFM_Item"
            prm.DbType = DbType.Int16
            prm.Value = (m_mega_store = 1)
            cmd.Parameters.Add(prm)
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                While dr.Read
                    Dim sItemIdentifier As String
                    If Not dr.IsDBNull(2) Then sItemIdentifier = dr.GetString(2)
                    results.Add(New ItemCatalog.StoreItem(Me.Store_No, dr.GetInt32(0), dr.GetString(1), sItemIdentifier, dr.GetInt32(3), dr.GetDecimal(4), dr.GetByte(5)))
                End While
            End If
            Return results
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
    Public Sub DeleteCustomerReturn(ByVal lReturnID As Long)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "DeleteCustomerReturn"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@ReturnID"
            prm.Value = lReturnID
            cmd.Parameters.Add(prm)
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Function GetAllUsers() As ArrayList
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            Dim results As New ArrayList
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetAllStoreUsers"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Store_No"
            prm.Value = Me.Store_No
            cmd.Parameters.Add(prm)
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                Dim sName As String
                While dr.Read
                    If IsDBNull(dr!FullName) Then
                        sName = CType(dr!UserName, String)
                    Else
                        sName = CType(dr!FullName, String)
                    End If
                    results.Add(New ItemCatalog.ReferenceList(CType(dr!User_ID, Long), sName))
                End While
            End If
            Return results
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
    Public Function GetSubTeams() As ArrayList
        If m_subteams Is Nothing Then
            Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
            Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
            Try
                cmd = New System.Data.SqlClient.SqlCommand
                m_subteams = New ArrayList
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "GetStoreSubTeam"
                Dim prm As New System.Data.SqlClient.SqlParameter
                prm.Direction = ParameterDirection.Input
                prm.DbType = DbType.Int32
                prm.ParameterName = "@Store_No"
                prm.Value = Me.Store_No
                cmd.Parameters.Add(prm)
                dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
                If dr.HasRows Then
                    While dr.Read
                        m_subteams.Add(New ItemCatalog.SubTeam(dr!SubTeam_No, dr!SubTeam_Name, IIf(IsDBNull(dr!SubTeam_Abbreviation), String.Empty, dr!SubTeam_Abbreviation), Not (dr!SubTeam_Unrestricted), dr!IsExpense))
                    End While
                End If
            Finally
                ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
                ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
            End Try
        End If

        Return m_subteams
    End Function
    Public Function GetWirelessPrinters() As ArrayList
        If Me.m_wireless_printers Is Nothing Then
            Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
            Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
            Try
                cmd = New System.Data.SqlClient.SqlCommand
                m_wireless_printers = New ArrayList
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "GetStoreMobilePrinter"
                cmd.Parameters.Add(CreateParam("@Store_No", SqlDbType.Int, ParameterDirection.Input, CObj(Me.Store_No)))
                dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
                If dr.HasRows Then
                    While dr.Read
                        m_wireless_printers.Add(New ItemCatalog.ReferenceList(dr!MobilePrinterID, dr!NetworkName))
                    End While
                End If
            Finally
                ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
                ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
            End Try
        End If

        Return Me.m_wireless_printers
    End Function
    Public Shared Function GetRetailStores() As ArrayList
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            Dim results As New ArrayList
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetStores"
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                While dr.Read
                    results.Add(New ItemCatalog.Store(dr!Store_No, dr!Store_Name, dr!Mega_Store, dr!WFM_Store))
                End While
            End If

            Return results
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
End Class

