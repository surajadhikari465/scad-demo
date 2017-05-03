Public Enum enuCustReturnItemSortOrder
    ID = 0
    Idenitifier = 1
    Item_Description = 2
End Enum
Public Class CustomerReturn
    Dim m_return_id As Long
    Dim m_customer As ItemCatalog.Customer
    Dim m_store As ItemCatalog.Store
    Dim m_user As ItemCatalog.User
    Dim m_return_date As Date
    Dim m_approver As ItemCatalog.User
    Dim m_return_item_count As Long = -1
    Dim m_return_item_total As Decimal = -1
    Dim m_return_items As ArrayList
    Public Property ReturnID() As Long
        Get
            Return m_return_id
        End Get
        Set(ByVal Value As Long)
            m_return_id = Value
        End Set
    End Property
    Public Property Customer() As ItemCatalog.Customer
        Get
            Return m_customer
        End Get
        Set(ByVal Value As ItemCatalog.Customer)
            m_customer = Value
        End Set
    End Property
    Public Property Store() As ItemCatalog.Store
        Get
            Return m_store
        End Get
        Set(ByVal Value As ItemCatalog.Store)
            m_store = Value
        End Set
    End Property
    Public Property User() As ItemCatalog.User
        Get
            Return m_user
        End Get
        Set(ByVal Value As ItemCatalog.User)
            m_user = Value
        End Set
    End Property
    Public Property ReturnDate() As Date
        Get
            Return m_return_date
        End Get
        Set(ByVal Value As Date)
            m_return_date = Value
        End Set
    End Property
    Public Property Approver() As ItemCatalog.User
        Get
            Return m_approver
        End Get
        Set(ByVal Value As ItemCatalog.User)
            m_approver = Value
        End Set
    End Property
    Public ReadOnly Property ReturnItemCount() As Long
        Get
            If m_return_item_count > -1 Then
                Return m_return_item_count
            Else
                If Not (Me.m_return_items Is Nothing) Then
                    m_return_item_count = m_return_items.Count
                    Return m_return_item_count
                Else
                    m_return_item_count = Me.ReturnItems.Count
                    Return m_return_item_count
                End If
            End If
        End Get
    End Property
    Public ReadOnly Property ReturnItemTotal() As Decimal
        Get
            If m_return_item_total > -1 Then
                Return m_return_item_total
            Else
                Dim i As Long
                Dim item As ItemCatalog.CustomerReturnItem
                If Me.m_return_items Is Nothing Then Me.m_return_item_count = Me.ReturnItems.Count
                For i = 0 To m_return_items.Count
                    item = CType(m_return_items(i), CustomerReturnItem)
                    m_return_item_total += (item.Quantity * item.Amount) + (item.Weight * item.Amount)
                Next
                Return m_return_item_total
            End If
        End Get
    End Property
    Public Sub New()
    End Sub
    Public Sub New(ByVal customer As ItemCatalog.Customer, ByVal lReturnID As Long, ByVal lStore_No As Long, ByVal lUser_ID As Long, ByVal dReturnDate As Date, _
    ByVal lApprover_ID As Long, ByVal lReturnItemCount As Long, ByVal dReturnItemTotal As Decimal)
        Me.ReturnID = lReturnID
        Me.Customer = customer
        Me.Store = New ItemCatalog.Store(lStore_No)
        Me.User = New ItemCatalog.User(lUser_ID)
        Me.Approver = New ItemCatalog.User(lApprover_ID)
        Me.ReturnDate = dReturnDate
        m_return_item_count = lReturnItemCount
        m_return_item_total = dReturnItemTotal
    End Sub
    Public Sub New(ByVal customer As ItemCatalog.Customer, ByVal store As ItemCatalog.Store, ByVal user As ItemCatalog.User, ByVal approver As ItemCatalog.User)
        Me.Customer = customer
        Me.Store = store
        Me.User = user
        Me.Approver = approver
        m_return_date = Now
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "InsertCustomerReturn"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@CustomerID"
            prm.Value = Me.Customer.CustomerID
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Store_No"
            prm.Value = Me.Store.Store_No
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@User_ID"
            prm.Value = Me.User.User_ID
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Date
            prm.ParameterName = "@ReturnDate"
            prm.Value = Me.ReturnDate
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Approver_ID"
            prm.Value = Me.Approver.User_ID
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Output
            prm.DbType = DbType.Int32
            prm.ParameterName = "@ReturnID"
            cmd.Parameters.Add(prm)
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
            Me.ReturnID = cmd.Parameters("@ReturnID").Value
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Sub New(ByVal customer As ItemCatalog.Customer, ByVal lReturnID As Long)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            Me.ReturnID = lReturnID
            Me.Customer = customer
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetCustomerReturn"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@ReturnID"
            prm.Value = Me.ReturnID
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Output
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Store_No"
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Output
            prm.DbType = DbType.Int32
            prm.ParameterName = "@User_ID"
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Output
            prm.DbType = DbType.DateTime
            prm.ParameterName = "@ReturnDate"
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Output
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Approver_ID"
            cmd.Parameters.Add(prm)
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
            Me.Store = New ItemCatalog.Store(cmd.Parameters("@Store_No").Value)
            Me.User = New ItemCatalog.User(cmd.Parameters("@User_ID").Value)
            Me.ReturnDate = cmd.Parameters("@ReturnDate").Value
            Me.Approver = New ItemCatalog.User(cmd.Parameters("@Approver_ID").Value)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Function AddReturnItem(ByVal lItem_Key As Long, ByVal sIdentifier As String, ByVal sItem_Description As String, ByVal dQuantity As Decimal, ByVal dWeight As Decimal, ByVal dAmount As Decimal, ByVal lCustReturnReasonID As Long, ByVal sCustReturnReason As String) As ItemCatalog.CustomerReturnItem
        If m_return_items Is Nothing Then m_return_items = New ArrayList

        Dim return_item As New CustomerReturnItem(m_return_id, 0, lItem_Key, dQuantity, dWeight, dAmount, lCustReturnReasonID, sCustReturnReason)

        m_return_items.Add(return_item)

        If m_return_item_count > -1 Then
            m_return_item_count = m_return_item_count + 1
        Else
            m_return_item_count = 1
        End If

        If m_return_item_total > -1 Then
            m_return_item_total = m_return_item_total + dAmount
        Else
            m_return_item_total = dAmount
        End If

        Return return_item
    End Function
    Public Sub UpdateReturnItem(ByVal custreturnitem As CustomerReturnItem)
        'Must sort before doing BinarySearch method
        m_return_items.Sort(New CustReturnItemSort(enuCustReturnItemSortOrder.ID))
        Dim i As Integer = m_return_items.BinarySearch(custreturnitem, New CustReturnItemSort(enuCustReturnItemSortOrder.ID))
        If i < 0 Then
            Throw New ItemCatalog.Exception.OrderItem.MissingException("Return item not found")
        Else
            custreturnitem.Update()
            Dim old As CustomerReturnItem = m_return_items(i)
            m_return_item_total = m_return_item_total - old.Amount
            m_return_items.RemoveAt(i)
            m_return_items.Add(custreturnitem)
            m_return_item_total = m_return_item_total + custreturnitem.Amount
        End If
    End Sub
    Public Sub DeleteReturnItem(ByVal custreturnitem As CustomerReturnItem)
        'Must sort before doing BinarySearch method
        m_return_items.Sort(New CustReturnItemSort(enuCustReturnItemSortOrder.ID))
        Dim i As Integer = m_return_items.BinarySearch(custreturnitem, New CustReturnItemSort(enuCustReturnItemSortOrder.ID))
        If i < 0 Then
            Throw New System.Exception("Application error:  return item not found")
        Else
            m_return_item_total = m_return_item_total - custreturnitem.Amount
            m_return_item_count = m_return_item_count - 1
            custreturnitem.Delete()
            m_return_items.RemoveAt(i)
        End If
    End Sub
    Public Function GetReturnItem(ByVal lReturnItemID As Long) As CustomerReturnItem
        m_return_items.Sort(New CustReturnItemSort(enuCustReturnItemSortOrder.ID))
        Dim searchItem As New ItemCatalog.CustomerReturnItem
        searchItem.ReturnItemID = lReturnItemID
        Dim i As Integer = m_return_items.BinarySearch(searchItem, New CustReturnItemGet)
        If i >= 0 Then Return CType(m_return_items(i), CustomerReturnItem) Else Return Nothing
    End Function
    Public ReadOnly Property ReturnItems() As ArrayList
        Get
            If Not (m_return_items Is Nothing) Then
                Return m_return_items
            Else
                'Get the array list
                Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
                Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
                Try
                    m_return_items = New ArrayList
                    cmd = New System.Data.SqlClient.SqlCommand
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandText = "GetCustomerReturnItems"
                    Dim prm As New System.Data.SqlClient.SqlParameter
                    prm.Direction = ParameterDirection.Input
                    prm.DbType = DbType.Int32
                    prm.ParameterName = "@ReturnID"
                    prm.Value = m_return_id
                    cmd.Parameters.Add(prm)
                    dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
                    If dr.HasRows Then
                        While dr.Read()
                            m_return_items.Add(New ItemCatalog.CustomerReturnItem(m_return_id, dr.GetInt32(0), dr.GetInt32(1), dr.GetDecimal(2), dr.GetDecimal(3), dr.GetDecimal(4), dr.GetInt32(5), dr.GetString(6)))
                        End While
                    End If

                    Return m_return_items
                Finally
                    ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
                    ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
                End Try
            End If
        End Get
    End Property
    Public Shared Function GetCustReturnReasons() As ArrayList
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            Dim results As New ArrayList
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetCustReturnReasons"
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                While dr.Read
                    results.Add(New ItemCatalog.ReferenceList(dr.GetInt32(0), dr.GetString(1)))
                End While
            End If

            Return results
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
End Class
Public Class CustomerReturnItem
    Inherits ItemCatalog.Item
    Dim m_return_item_id As Long
    Dim m_quantity As Decimal
    Dim m_weight As Decimal
    Dim m_amount As Decimal
    Dim m_cust_return_reason_id As Long
    Dim m_cust_return_reason As String
    Public Property ReturnItemID() As Long
        Get
            Return m_return_item_id
        End Get
        Set(ByVal Value As Long)
            m_return_item_id = Value
        End Set
    End Property
    Public Property Quantity() As Decimal
        Get
            Return m_quantity
        End Get
        Set(ByVal Value As Decimal)
            m_quantity = Value
        End Set
    End Property
    Public Property Weight() As Decimal
        Get
            Return m_weight
        End Get
        Set(ByVal Value As Decimal)
            m_weight = Value
        End Set
    End Property
    Public Property Amount() As Decimal
        Get
            Return m_amount
        End Get
        Set(ByVal Value As Decimal)
            m_amount = Value
        End Set
    End Property
    Public ReadOnly Property Total() As Decimal
        Get
            Return (m_quantity * m_amount) + (m_weight * m_amount)
        End Get
    End Property
    Public Property CustReturnReasonID() As Long
        Get
            Return m_cust_return_reason_id
        End Get
        Set(ByVal Value As Long)
            m_cust_return_reason_id = Value
        End Set
    End Property
    Public Property CustReturnReason() As String
        Get
            Return m_cust_return_reason
        End Get
        Set(ByVal Value As String)
            m_cust_return_reason = Value
        End Set
    End Property
    Protected Friend Sub New()
    End Sub
    Protected Friend Sub New(ByVal lReturnID As Long, ByVal lReturnItemID As Long, ByVal lItem_Key As Long, ByVal dQuantity As Decimal, ByVal dWeight As Decimal, ByVal dAmount As Decimal, ByVal lCustReturnReasonID As Long, ByVal sCustReturnReason As String)
        MyBase.New(lItem_Key)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            Me.Quantity = dQuantity
            Me.Weight = dWeight
            Me.Amount = dAmount
            Me.CustReturnReasonID = lCustReturnReasonID
            Me.CustReturnReason = sCustReturnReason
            Validate()
            If lReturnItemID > 0 Then
                Me.ReturnItemID = lReturnItemID
            Else
                cmd = New System.Data.SqlClient.SqlCommand
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "InsertCustomerReturnItem"
                Dim prm As New System.Data.SqlClient.SqlParameter
                prm.Direction = ParameterDirection.Input
                prm.DbType = DbType.Int32
                prm.ParameterName = "@ReturnID"
                prm.Value = lReturnID
                cmd.Parameters.Add(prm)
                prm = New System.Data.SqlClient.SqlParameter
                prm.Direction = ParameterDirection.Input
                prm.DbType = DbType.Int32
                prm.ParameterName = "@Item_Key"
                prm.Value = Me.Item_Key
                cmd.Parameters.Add(prm)
                prm = New System.Data.SqlClient.SqlParameter
                prm.Direction = ParameterDirection.Input
                prm.DbType = DbType.Decimal
                prm.ParameterName = "@Quantity"
                prm.Precision = 18
                prm.Scale = 4
                prm.Value = Me.Quantity
                cmd.Parameters.Add(prm)
                prm = New System.Data.SqlClient.SqlParameter
                prm.Direction = ParameterDirection.Input
                prm.DbType = DbType.Decimal
                prm.ParameterName = "@Weight"
                prm.Precision = 18
                prm.Scale = 4
                prm.Value = Me.Weight
                cmd.Parameters.Add(prm)
                prm = New System.Data.SqlClient.SqlParameter
                prm.Direction = ParameterDirection.Input
                prm.DbType = DbType.Decimal
                prm.ParameterName = "@Amount"
                prm.Precision = 18
                prm.Scale = 4
                prm.Value = Me.Amount
                cmd.Parameters.Add(prm)
                prm = New System.Data.SqlClient.SqlParameter
                prm.Direction = ParameterDirection.Input
                prm.DbType = DbType.Int32
                prm.ParameterName = "@CustReturnReasonID"
                prm.Value = Me.CustReturnReasonID
                cmd.Parameters.Add(prm)
                prm = New System.Data.SqlClient.SqlParameter
                prm.Direction = ParameterDirection.Output
                prm.DbType = DbType.Int32
                prm.ParameterName = "@ReturnItemID"
                cmd.Parameters.Add(prm)
                ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
                Me.ReturnItemID = cmd.Parameters("@ReturnItemID").Value
            End If
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Protected Friend Sub Update()
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            Validate()
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "UpdateCustReturnItem"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@ReturnItemID"
            prm.Value = Me.ReturnItemID
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Decimal
            prm.ParameterName = "@Quantity"
            prm.Precision = 18
            prm.Scale = 4
            prm.Value = Me.Quantity
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Decimal
            prm.ParameterName = "@Weight"
            prm.Precision = 18
            prm.Scale = 4
            prm.Value = Me.Weight
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Decimal
            prm.ParameterName = "@Amount"
            prm.Precision = 18
            prm.Scale = 4
            prm.Value = Me.Amount
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@CustReturnReasonID"
            prm.Value = Me.CustReturnReasonID
            cmd.Parameters.Add(prm)
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Protected Friend Sub Delete()
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "DeleteCustReturnItem"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@ReturnItemID"
            prm.Value = Me.ReturnItemID
            cmd.Parameters.Add(prm)
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Private Sub Validate()
        If (Me.Quantity = 0) And (Me.Weight = 0) Then
            Throw New System.Exception("Quantity or Weight is required")
        ElseIf (Me.Quantity > 0) And (Me.Weight > 0) Then
            Throw New System.Exception("Quantity or Weight must be blank")
        End If
    End Sub
End Class
Public Class CustReturnItemSort
    Implements IComparer
    Dim CompType As enuCustReturnItemSortOrder
    Public Sub New(ByVal xCompType As enuCustReturnItemSortOrder)
        CompType = xCompType
    End Sub
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Select Case CompType
            Case enuCustReturnItemSortOrder.ID
                Compare = CType(x, CustomerReturnItem).ReturnItemID.CompareTo(CType(y, CustomerReturnItem).ReturnItemID)
            Case enuCustReturnItemSortOrder.Idenitifier
                Compare = String.Compare(CType(x, CustomerReturnItem).Identifier, CType(y, CustomerReturnItem).Identifier, True)
            Case enuCustReturnItemSortOrder.Item_Description
                Compare = String.Compare(CType(x, CustomerReturnItem).Item_Description, CType(y, CustomerReturnItem).Item_Description, True)
        End Select
    End Function
End Class
Public Class CustReturnItemGet
    Implements IComparer
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Compare = CType(x, CustomerReturnItem).ReturnItemID.CompareTo(CType(y, CustomerReturnItem).ReturnItemID)
    End Function
End Class
