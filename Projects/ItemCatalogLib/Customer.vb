Public Class Customer
    Dim m_customer_id As Long
    Dim m_first_name As String
    Dim m_last_name As String
    Dim m_phone As String
    Dim m_address1 As String
    Dim m_address2 As String
    Dim m_city As String
    Dim m_state As String
    Dim m_zip_code As String
    Public Property CustomerID() As Long
        Get
            Return m_customer_id
        End Get
        Set(ByVal Value As Long)
            m_customer_id = Value
        End Set
    End Property
    Public Property FirstName() As String
        Get
            Return m_first_name
        End Get
        Set(ByVal Value As String)
            m_first_name = Value
        End Set
    End Property
    Public Property LastName() As String
        Get
            Return m_last_name
        End Get
        Set(ByVal Value As String)
            m_last_name = Value
        End Set
    End Property
    Public Property Phone() As String
        Get
            Return m_phone
        End Get
        Set(ByVal Value As String)
            m_phone = FormatPhoneNumber(Value)
        End Set
    End Property
    Public Property Address1() As String
        Get
            Return m_address1
        End Get
        Set(ByVal Value As String)
            m_address1 = Value
        End Set
    End Property
    Public Property Address2() As String
        Get
            Return m_address2
        End Get
        Set(ByVal Value As String)
            m_address2 = Value
        End Set
    End Property
    Public Property City() As String
        Get
            Return m_city
        End Get
        Set(ByVal Value As String)
            m_city = Value
        End Set
    End Property
    Public Property State() As String
        Get
            Return m_state
        End Get
        Set(ByVal Value As String)
            m_state = Value
        End Set
    End Property
    Public Property ZipCode() As String
        Get
            Return m_zip_code
        End Get
        Set(ByVal Value As String)
            m_zip_code = Value
        End Set
    End Property
    Public Sub New(ByVal sFirstName As String, ByVal sLastName As String, ByVal sPhone As String, _
    ByVal sAddress1 As String, ByVal sAddress2 As String, ByVal sCity As String, ByVal sState As String, ByVal sZipCode As String)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            Me.FirstName = sFirstName
            Me.LastName = sLastName
            Me.Phone = sPhone
            Me.Address1 = sAddress1
            Me.Address2 = sAddress2
            Me.City = sCity
            Me.State = sState
            Me.ZipCode = sZipCode
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "InsertCustomer"
            CreateAddUpdateParms(cmd, ParameterDirection.Output)
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
            m_customer_id = cmd.Parameters("@CustomerID").Value
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Sub New(ByVal lCustomerID As Long)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetCustomer"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@CustomerID"
            prm.DbType = DbType.Int32
            prm.Value = lCustomerID
            cmd.Parameters.Add(prm)
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                dr.Read()
                Me.CustomerID = lCustomerID
                Me.FirstName = dr.GetSqlString(0).Value
                Me.LastName = dr.GetSqlString(1).Value
                Me.Phone = dr.GetSqlString(2).Value
                Me.Address1 = dr.GetSqlString(3).Value
                Me.Address2 = dr.GetSqlString(4).Value
                Me.City = dr.GetSqlString(5).Value
                Me.State = dr.GetSqlString(6).Value
                Me.ZipCode = dr.GetSqlString(7).Value
            Else
                Throw New System.Exception("CustomerID does not exist")
            End If
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Sub New(ByVal lCustomerID As Long, ByVal sFirstName As String, ByVal sLastName As String, ByVal sPhone As String, _
    ByVal sAddress1 As String, ByVal sAddress2 As String, ByVal sCity As String, ByVal sState As String, ByVal sZipCode As String)
        Me.CustomerID = lCustomerID
        Me.FirstName = sFirstName
        Me.LastName = sLastName
        Me.Phone = sPhone
        Me.Address1 = sAddress1
        Me.Address2 = sAddress2
        Me.City = sCity
        Me.State = sState
        Me.ZipCode = sZipCode
    End Sub
    Public Sub Update()
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "UpdateCustomer"
            CreateAddUpdateParms(cmd, ParameterDirection.Input)
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Private Sub CreateAddUpdateParms(ByVal cmd As System.Data.SqlClient.SqlCommand, ByVal iMode As ParameterDirection)
        Dim prm As New System.Data.SqlClient.SqlParameter
        prm.Direction = ParameterDirection.Input
        prm.DbType = DbType.String
        prm.ParameterName = "@FirstName"
        prm.Value = IIf(m_first_name.Trim = String.Empty, System.DBNull.Value, m_first_name.Trim)
        cmd.Parameters.Add(prm)
        prm = New System.Data.SqlClient.SqlParameter
        prm.Direction = ParameterDirection.Input
        prm.DbType = DbType.String
        prm.ParameterName = "@LastName"
        prm.Value = IIf(m_last_name.Trim = String.Empty, System.DBNull.Value, m_last_name.Trim)
        cmd.Parameters.Add(prm)
        prm = New System.Data.SqlClient.SqlParameter
        prm.Direction = ParameterDirection.Input
        prm.DbType = DbType.String
        prm.ParameterName = "@Phone"
        prm.Value = IIf(m_phone.Trim = String.Empty, System.DBNull.Value, m_phone.Trim)
        cmd.Parameters.Add(prm)
        prm = New System.Data.SqlClient.SqlParameter
        prm.Direction = ParameterDirection.Input
        prm.DbType = DbType.String
        prm.ParameterName = "@Address1"
        prm.Value = IIf(m_address1.Trim = String.Empty, System.DBNull.Value, m_address1.Trim)
        cmd.Parameters.Add(prm)
        prm = New System.Data.SqlClient.SqlParameter
        prm.Direction = ParameterDirection.Input
        prm.DbType = DbType.String
        prm.ParameterName = "@Address2"
        prm.Value = IIf(m_address2.Trim = String.Empty, System.DBNull.Value, m_address2.Trim)
        cmd.Parameters.Add(prm)
        prm = New System.Data.SqlClient.SqlParameter
        prm.Direction = ParameterDirection.Input
        prm.DbType = DbType.String
        prm.ParameterName = "@City"
        prm.Value = IIf(m_city.Trim = String.Empty, System.DBNull.Value, m_city.Trim)
        cmd.Parameters.Add(prm)
        prm = New System.Data.SqlClient.SqlParameter
        prm.Direction = ParameterDirection.Input
        prm.DbType = DbType.String
        prm.ParameterName = "@State"
        prm.Value = IIf(m_state.Trim = String.Empty, System.DBNull.Value, m_state.Trim.ToUpper)
        cmd.Parameters.Add(prm)
        prm = New System.Data.SqlClient.SqlParameter
        prm.Direction = ParameterDirection.Input
        prm.DbType = DbType.String
        prm.ParameterName = "@ZipCode"
        prm.Value = IIf(m_zip_code.Trim = String.Empty, System.DBNull.Value, m_zip_code.Trim)
        cmd.Parameters.Add(prm)
        prm = New System.Data.SqlClient.SqlParameter
        prm.Direction = iMode
        prm.DbType = DbType.Int32
        prm.ParameterName = "@CustomerID"
        If iMode = ParameterDirection.Input Then prm.Value = m_customer_id
        cmd.Parameters.Add(prm)
    End Sub
    Public Overrides Function ToString() As String
        Return m_last_name & ", " & m_first_name
    End Function
    Public Function GetReturns() As ArrayList
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            Dim results As New ArrayList
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetCustomerReturnHistory"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.ParameterName = "@CustomerID"
            prm.DbType = DbType.Int32
            prm.Value = Me.CustomerID
            cmd.Parameters.Add(prm)
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                While dr.Read
                    results.Add(New ItemCatalog.CustomerReturn(Me, CType(dr!ReturnID, Long), CType(dr!Store_No, Long), CType(dr!User_ID, Long), CType(dr!ReturnDate, DateTime), CType(dr!Approver_ID, Long), CType(dr!ReturnItemCount, Long), CType(dr!ReturnItemTotal, Decimal)))
                End While
            End If
            Return results
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
    Private Function FormatPhoneNumber(ByVal sIn As String) As String
        If sIn.Length = 0 Then
            Return sIn
        Else
            Dim sOut As String = String.Empty
            Dim chr As String = String.Empty
            Dim i As Integer
            For i = 0 To sIn.Length - 1
                chr = sIn.Substring(i, 1)
                If (chr >= "0") And (chr <= "9") Then sOut = sOut & chr
            Next
            Return sOut.Substring(0, 3) & "-" & sOut.Substring(3, 3) & "-" & sOut.Substring(6, 4)
        End If
    End Function
End Class
