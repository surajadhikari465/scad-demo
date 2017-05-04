Imports WFM.DataAccess.Setup
Public Enum enuReprintSignSource
    PriceBatch = 1
    ScanGun = 2
    Manual = 3
End Enum
Public Class StoreItem
    Inherits ItemCatalog.Item
    Dim m_store_no As Integer
    Dim m_subteam_no As Integer
    Dim m_subteam_name As String
    Dim m_price As Decimal
    Dim m_multiple As Byte
    Dim m_on_sale As Boolean
    Dim m_sale_edlp As Boolean
    Dim m_sale_start_date As Date
    Dim m_sale_end_date As Date
    Dim m_sale_multiple As Byte
    Dim m_sale_price As Decimal
    Dim m_sale_earned_disc1 As Byte
    Dim m_sale_earned_disc2 As Byte
    Dim m_sale_earned_disc3 As Byte
    Dim m_avg_cost As Decimal
    Dim m_sellable As Boolean
    Dim m_user_id As Integer
    Dim m_pricechgtypedesc As String

    Public Structure CycleCountInfo
        Public Quantity As Decimal
        Public Weight As Decimal
    End Structure
    Public Structure OrderInfo
        Public PrimaryVendorKey As String
        Public PrimaryVendorName As String
        Public OnOrder As Decimal
        Public InQueue As Decimal
        Public InQueueCredit As Decimal
        Public InQueueTransfer As Decimal
        Public LastReceivedDate As Date
        Public LastReceived As Decimal
    End Structure
    Public ReadOnly Property AvgCost() As Decimal
        Get
            Return Me.m_avg_cost
        End Get
    End Property
    Public ReadOnly Property IsSellable() As Boolean
        Get
            Return Me.m_sellable
        End Get
    End Property
    Public ReadOnly Property IsSaleEDLP() As Boolean
        Get
            Return Me.m_sale_edlp
        End Get
    End Property
    Public ReadOnly Property IsOnSale() As Boolean
        Get
            Return Me.m_on_sale
        End Get
    End Property
    Public ReadOnly Property Multiple() As Byte
        Get
            If m_multiple > 0 Then
                Return m_multiple
            Else
                Return 1
            End If
        End Get
    End Property
    Public ReadOnly Property Price() As Decimal
        Get
            Return m_price
        End Get
    End Property
    Public ReadOnly Property PriceChgTypeDesc() As String
        Get
            Return m_pricechgtypedesc
        End Get
    End Property
    Public ReadOnly Property SaleMultiple() As Byte
        Get
            Return Me.m_sale_multiple
        End Get
    End Property
    Public ReadOnly Property SalePrice() As Decimal
        Get
            Return m_sale_price
        End Get
    End Property
    Public ReadOnly Property SaleStartDate() As Date
        Get
            Return Me.m_sale_start_date
        End Get
    End Property
    Public ReadOnly Property SaleEndDate() As Date
        Get
            Return Me.m_sale_end_date
        End Get
    End Property
    Public ReadOnly Property SaleEarnedDiscount1() As Byte
        Get
            Return Me.m_sale_earned_disc1
        End Get
    End Property
    Public ReadOnly Property SaleEarnedDiscount2() As Byte
        Get
            Return Me.m_sale_earned_disc2
        End Get
    End Property
    Public ReadOnly Property SaleEarnedDiscount3() As Byte
        Get
            Return Me.m_sale_earned_disc3
        End Get
    End Property
    Public ReadOnly Property StoreNo() As Integer
        Get
            Return Me.m_store_no
        End Get
    End Property
    Public ReadOnly Property UserId() As Integer
        Get
            Return Me.m_user_id
        End Get
    End Property
    Public Shadows ReadOnly Property SubTeam_No() As Integer
        Get
            If Me.m_subteam_no = 0 Then
                Return MyBase.SubTeam_No
            Else
                Return Me.m_subteam_no
            End If
        End Get
    End Property
    Public Shadows ReadOnly Property SubTeamName() As String
        Get
            If Me.m_subteam_name Is Nothing Then
                Return MyBase.SubTeamName
            Else
                If Me.m_subteam_name.Length = 0 Then
                    Return MyBase.SubTeamName
                Else
                    Return Me.m_subteam_name
                End If
            End If
        End Get
    End Property
    Public ReadOnly Property UnitPrice() As Decimal
        Get
            If m_multiple > 0 Then
                Return m_price / m_multiple
            Else
                Return m_price
            End If
        End Get
    End Property

    Public Sub New(ByVal Store_No As Integer, ByVal User_Id As Integer, ByVal Identifier As String, ByVal DTSAudit As Boolean)
        MyBase.New()
        Me.m_store_no = Store_No
        Me.m_user_id = User_Id
        Construct(0, Identifier)
    End Sub

    Public Sub New(ByVal Store_No As Integer, ByVal Identifier As String)
        MyBase.New()
        Me.m_store_no = Store_No
        Construct(0, Identifier)
    End Sub
    Public Sub New(ByVal Store_No As Integer, ByVal SubTeam_No As Integer, ByVal Identifier As String)
        MyBase.New()
        Me.m_store_no = Store_No
        Me.m_subteam_no = SubTeam_No
        Construct(0, Identifier)
    End Sub
    Public Sub New(ByVal Store_No As Integer, ByVal SubTeam_No As Integer, ByVal User_Id As Integer, ByVal Identifier As String)
        MyBase.New()
        Me.m_store_no = Store_No
        Me.m_subteam_no = SubTeam_No
        Me.m_user_id = User_Id
        Construct(0, Identifier)
    End Sub
    Public Sub New(ByVal Store_No As Integer, ByVal Item_Key As Integer)
        MyBase.New()
        Me.m_store_no = Store_No
        Construct(Item_Key, String.Empty)
    End Sub
    Public Sub New(ByVal Store_No As Integer, ByVal SubTeam_No As Integer, ByVal Item_Key As Integer)
        MyBase.New()
        Me.m_store_no = Store_No
        Me.m_subteam_no = SubTeam_No
        Construct(Item_Key, String.Empty)
    End Sub
    Protected Friend Sub New(ByVal StoreNo As Integer, ByVal lItem_Key As Long, ByVal sItem_Description As String, ByVal sIdentifier As String, ByVal lSubTeam_No As Long, ByVal dPrice As Decimal, ByVal iMultiple As Int16)
        MyBase.New(lItem_Key, sItem_Description, sIdentifier, lSubTeam_No)
        Me.m_store_no = StoreNo
        Me.m_price = dPrice
        Me.m_multiple = iMultiple
    End Sub
    Private Sub Construct(ByVal Item_Key As Long, ByVal Identifier As String)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetStoreItem"
            cmd.Parameters.Add(CreateParam("@Store_No", SqlDbType.Int, ParameterDirection.Input, CObj(Me.StoreNo)))
            cmd.Parameters.Add(CreateParam("@TransferToSubTeam_No", SqlDbType.Int, ParameterDirection.Input, CObj(IIf(Me.SubTeam_No = 0, System.DBNull.Value, Me.SubTeam_No))))
            cmd.Parameters.Add(CreateParam("@User_ID", SqlDbType.Int, ParameterDirection.Input, CObj(Me.UserId)))
            cmd.Parameters.Add(CreateParam("@Item_Key", SqlDbType.Int, ParameterDirection.InputOutput, CObj(IIf(Item_Key = 0, System.DBNull.Value, Item_Key))))
            cmd.Parameters.Add(CreateParam("@Identifier", SqlDbType.VarChar, ParameterDirection.InputOutput, CObj(IIf(Identifier.Length = 0, System.DBNull.Value, Identifier)), 13))
            cmd.Parameters.Add(CreateParam("@Price", SqlDbType.SmallMoney, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@Multiple", SqlDbType.TinyInt, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@On_Sale", SqlDbType.Bit, ParameterDirection.Output))
            'cmd.Parameters.Add(CreateParam("@Sale_EDLP", SqlDbType.Bit, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@Sale_Start_Date", SqlDbType.SmallDateTime, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@Sale_End_Date", SqlDbType.SmallDateTime, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@Sale_Multiple", SqlDbType.TinyInt, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@Sale_Price", SqlDbType.SmallMoney, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@Sale_Earned_Disc1", SqlDbType.TinyInt, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@Sale_Earned_Disc2", SqlDbType.TinyInt, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@Sale_Earned_Disc3", SqlDbType.TinyInt, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@AvgCost", SqlDbType.SmallMoney, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@CanInventory", SqlDbType.Bit, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@IsSellable", SqlDbType.Bit, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@Item_Description", SqlDbType.VarChar, ParameterDirection.Output, 255))
            cmd.Parameters.Add(CreateParam("@POS_Description", SqlDbType.VarChar, ParameterDirection.Output, 255))
            cmd.Parameters.Add(CreateParam("@RetailSubTeam_No", SqlDbType.Int, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@RetailSubTeam_Name", SqlDbType.VarChar, ParameterDirection.Output, 255))
            cmd.Parameters.Add(CreateParam("@TransferToSubTeam_Name", SqlDbType.VarChar, ParameterDirection.Output, 255))
            cmd.Parameters.Add(CreateParam("@Package_Desc1", SqlDbType.Int, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@Package_Desc2", SqlDbType.Decimal, ParameterDirection.Output, 9, 4))
            cmd.Parameters.Add(CreateParam("@Package_Unit_Abbr", SqlDbType.VarChar, ParameterDirection.Output, 255))
            cmd.Parameters.Add(CreateParam("@Not_Available", SqlDbType.Bit, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@Discontinue_Item", SqlDbType.Bit, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@Sign_Description", SqlDbType.VarChar, ParameterDirection.Output, 255))
            cmd.Parameters.Add(CreateParam("@Sold_By_Weight", SqlDbType.Bit, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@WFM_Item", SqlDbType.Bit, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@HFM_Item", SqlDbType.Bit, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@Retail_Sale", SqlDbType.Bit, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@Vendor_Unit_ID", SqlDbType.Int, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@Vendor_Unit_Name", SqlDbType.VarChar, ParameterDirection.Output, 255))
            cmd.Parameters.Add(CreateParam("@PriceChgTypeDesc", SqlDbType.VarChar, ParameterDirection.Output, 5))
            cmd.Parameters.Add(CreateParam("@VendorName", SqlDbType.VarChar, ParameterDirection.Output, 50))
            'cmd.Parameters.Add(CreateParam("@VendorPack", SqlDbType.Decimal, ParameterDirection.Output, 9, 4))

            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)

            If IsDBNull(cmd.Parameters("@Item_Key").Value) Then Throw New ItemCatalog.Exception.InvalidIdentifierException
            If Not CBool(cmd.Parameters("@CanInventory").Value) Then Throw New ItemCatalog.Exception.ItemInventorySubTeamException
            Me.m_price = cmd.Parameters("@Price").Value
            Me.m_pricechgtypedesc = cmd.Parameters("@PriceChgTypeDesc").Value
            Me.m_multiple = cmd.Parameters("@Multiple").Value
            Me.m_on_sale = cmd.Parameters("@On_Sale").Value
            Me.m_sale_start_date = IIf(IsDBNull(cmd.Parameters("@Sale_Start_Date").Value), Date.MinValue, cmd.Parameters("@Sale_Start_Date").Value)
            Me.m_sale_end_date = IIf(IsDBNull(cmd.Parameters("@Sale_End_Date").Value), Date.MinValue, cmd.Parameters("@Sale_End_Date").Value)
            Me.m_sale_multiple = cmd.Parameters("@Sale_Multiple").Value
            Me.m_sale_price = cmd.Parameters("@Sale_Price").Value
            Me.m_sale_earned_disc1 = cmd.Parameters("@Sale_Earned_Disc1").Value
            Me.m_sale_earned_disc2 = cmd.Parameters("@Sale_Earned_Disc2").Value
            Me.m_sale_earned_disc3 = cmd.Parameters("@Sale_Earned_Disc3").Value
            Me.m_avg_cost = IIf(IsDBNull(cmd.Parameters("@AvgCost").Value), 0, cmd.Parameters("@AvgCost").Value)
            Me.m_sellable = cmd.Parameters("@IsSellable").Value
            Me.Item_Key = cmd.Parameters("@Item_Key").Value
            Me.Identifier = cmd.Parameters("@Identifier").Value
            Me.Item_Description = cmd.Parameters("@Item_Description").Value
            Me.POS_Description = cmd.Parameters("@POS_Description").Value
            Me.Sign_Description = cmd.Parameters("@Sign_Description").Value
            MyBase.SubTeam_No = cmd.Parameters("@RetailSubTeam_No").Value
            MyBase.SubTeamName = cmd.Parameters("@RetailSubTeam_Name").Value
            If Not IsDBNull(cmd.Parameters("@TransferToSubTeam_Name").Value) Then Me.m_subteam_name = cmd.Parameters("@TransferToSubTeam_Name").Value
            Me.Package_Desc1 = cmd.Parameters("@Package_Desc1").Value
            Me.Package_Desc2 = cmd.Parameters("@Package_Desc2").Value
            Me.Package_Unit_Abbr = cmd.Parameters("@Package_Unit_Abbr").Value
            Me.IsNotAvailable = cmd.Parameters("@Not_Available").Value
            Me.IsDiscontinued = cmd.Parameters("@Discontinue_Item").Value
            Me.IsSoldByWeight = cmd.Parameters("@Sold_By_Weight").Value
            Me.IsSoldHFM = cmd.Parameters("@HFM_Item").Value
            Me.IsSoldWFM = cmd.Parameters("@WFM_Item").Value
            Me.IsForRetailSale = cmd.Parameters("@Retail_Sale").Value
            Me.VendorOrderUnitID = IIf(IsDBNull(cmd.Parameters("@Vendor_Unit_ID").Value), 0, cmd.Parameters("@Vendor_Unit_ID").Value)
            Me.VendorOrderUnitName = IIf(IsDBNull(cmd.Parameters("@Vendor_Unit_Name").Value), String.Empty, cmd.Parameters("@Vendor_Unit_Name").Value)
            Me.VendorName = IIf(IsDBNull(cmd.Parameters("@VendorName").Value), "", cmd.Parameters("@VendorName").Value)
            'Me.VendorPack = IIf(IsDBNull(cmd.Parameters("@VendorPack").Value), 0, cmd.Parameters("@VendorPack").Value)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Sub AddToOrderQueue(ByVal IsTransfer As Boolean, ByVal IsCredit As Boolean, ByVal Quantity As Decimal, ByVal UnitID As Integer, ByVal User_ID As Integer)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        IsOrderable(True)
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "InsertOrderItemQueue"
            cmd.Parameters.Add(CreateParam("@Store_No", SqlDbType.Int, ParameterDirection.Input, CObj(Me.StoreNo)))
            cmd.Parameters.Add(CreateParam("@TransferToSubTeam_No", SqlDbType.Int, ParameterDirection.Input, CObj(Me.SubTeam_No)))
            cmd.Parameters.Add(CreateParam("@Item_Key", SqlDbType.Int, ParameterDirection.Input, CObj(Me.Item_Key)))
            cmd.Parameters.Add(CreateParam("@Transfer", SqlDbType.Bit, ParameterDirection.Input, CObj(IsTransfer)))
            cmd.Parameters.Add(CreateParam("@User_ID", SqlDbType.Int, ParameterDirection.Input, CObj(User_ID)))
            cmd.Parameters.Add(CreateParam("@Quantity", SqlDbType.Decimal, ParameterDirection.Input, 18, 4, CObj(Quantity)))
            cmd.Parameters.Add(CreateParam("@Unit_ID", SqlDbType.Int, ParameterDirection.Input, CObj(UnitID)))
            cmd.Parameters.Add(CreateParam("@Credit", SqlDbType.Bit, ParameterDirection.Input, CObj(IsCredit)))
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Sub AddToReprintSignQueue(ByVal User_ID As Long, ByVal SourceType As enuReprintSignSource)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "UpdateSignQueuePrinted"
            cmd.Parameters.Add(CreateParam("@ItemList", SqlDbType.VarChar, ParameterDirection.Input, CObj(Me.Item_Key.ToString), 8000))
            cmd.Parameters.Add(CreateParam("@ItemListSeparator", SqlDbType.Char, ParameterDirection.Input, CObj("|")))
            cmd.Parameters.Add(CreateParam("@Store_No", SqlDbType.Int, ParameterDirection.Input, CObj(Me.StoreNo)))
            cmd.Parameters.Add(CreateParam("@Printed", SqlDbType.Bit, ParameterDirection.Input, CObj(0)))
            cmd.Parameters.Add(CreateParam("@User_ID", SqlDbType.Int, ParameterDirection.Input, CObj(User_ID)))
            cmd.Parameters.Add(CreateParam("@Type", SqlDbType.TinyInt, ParameterDirection.Input, CObj(SourceType)))
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Function GetCycleCountInfo(Optional ByVal InventoryLocationID As Long = 0) As CycleCountInfo
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetStoreItemCycleCountInfo"
            cmd.Parameters.Add(CreateParam("@Store_No", SqlDbType.Int, ParameterDirection.Input, CObj(Me.StoreNo)))
            cmd.Parameters.Add(CreateParam("@SubTeam_No", SqlDbType.Int, ParameterDirection.Input, CObj(Me.SubTeam_No)))
            cmd.Parameters.Add(CreateParam("@InvLocID", SqlDbType.Int, ParameterDirection.Input, CObj(InventoryLocationID)))
            cmd.Parameters.Add(CreateParam("@Item_Key", SqlDbType.Int, ParameterDirection.Input, CObj(Me.Item_Key)))
            cmd.Parameters.Add(CreateParam("@Count", SqlDbType.Decimal, ParameterDirection.Output, 18, 4))
            cmd.Parameters.Add(CreateParam("@Weight", SqlDbType.Decimal, ParameterDirection.Output, 18, 4))
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
            Dim result As CycleCountInfo
            result.Quantity = cmd.Parameters("@Count").Value
            result.Weight = cmd.Parameters("@Weight").Value
            Return result
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
    Public Function GetOrderInfo() As OrderInfo
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetStoreItemOrderInfo"
            cmd.Parameters.Add(CreateParam("@Store_No", SqlDbType.Int, ParameterDirection.Input, CObj(Me.StoreNo)))
            cmd.Parameters.Add(CreateParam("@TransferToSubTeam_No", SqlDbType.Int, ParameterDirection.Input, CObj(Me.SubTeam_No)))
            cmd.Parameters.Add(CreateParam("@Item_Key", SqlDbType.Int, ParameterDirection.Input, CObj(Me.Item_Key)))
            cmd.Parameters.Add(CreateParam("@PrimaryVendorKey", SqlDbType.VarChar, ParameterDirection.Output, 255))
            cmd.Parameters.Add(CreateParam("@PrimaryVendorName", SqlDbType.VarChar, ParameterDirection.Output, 255))
            cmd.Parameters.Add(CreateParam("@QtyOnOrder", SqlDbType.Decimal, ParameterDirection.Output, 18, 4))
            cmd.Parameters.Add(CreateParam("@QtyOnQueue", SqlDbType.Decimal, ParameterDirection.Output, 18, 4))
            cmd.Parameters.Add(CreateParam("@QtyOnQueueCredit", SqlDbType.Decimal, ParameterDirection.Output, 18, 4))
            cmd.Parameters.Add(CreateParam("@QtyOnQueueTransfer", SqlDbType.Decimal, ParameterDirection.Output, 18, 4))
            cmd.Parameters.Add(CreateParam("@LastReceivedDate", SqlDbType.DateTime, ParameterDirection.Output))
            cmd.Parameters.Add(CreateParam("@LastReceived", SqlDbType.Decimal, ParameterDirection.Output, 18, 4))
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
            Dim result As OrderInfo
            result.PrimaryVendorKey = cmd.Parameters("@PrimaryVendorKey").Value
            result.PrimaryVendorName = cmd.Parameters("@PrimaryVendorName").Value
            result.OnOrder = cmd.Parameters("@QtyOnOrder").Value
            result.InQueue = cmd.Parameters("@QtyOnQueue").Value
            result.InQueueCredit = cmd.Parameters("@QtyOnQueueCredit").Value
            result.InQueueTransfer = cmd.Parameters("@QtyOnQueueTransfer").Value
            result.LastReceivedDate = cmd.Parameters("@LastReceivedDate").Value
            result.LastReceived = cmd.Parameters("@LastReceived").Value
            Return result
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
    Public Function GetPrimaryVendor() As ItemCatalog.StoreItem.Vendor
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            Dim vend As ItemCatalog.StoreItem.Vendor
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetItemStoreVendorsCost"
            cmd.Parameters.Add(CreateParam("@Store_No", SqlDbType.Int, ParameterDirection.Input, CObj(Me.StoreNo)))
            cmd.Parameters.Add(CreateParam("@Item_Key", SqlDbType.Int, ParameterDirection.Input, CObj(Me.Item_Key)))
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                While dr.Read()
                    If CBool(dr!PrimaryVendor) Then
                        vend = New ItemCatalog.StoreItem.Vendor(dr!Vendor_ID, dr!Vendor_Key, dr!CompanyName, dr!Address_Line_1, dr!Address_Line_2, dr!City, dr!State, dr!Zip_Code, dr!Country, dr!Phone, dr!UnitCost, dr!UnitFreight, dr!Package_Desc1, dr!MSRP, dr!PrimaryVendor)
                        Exit While
                    End If
                End While
            End If

            Return vend
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
    Public Function GetVendors() As ArrayList
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            Dim results As New ArrayList
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetItemStoreVendorsCost"
            cmd.Parameters.Add(CreateParam("@Store_No", SqlDbType.Int, ParameterDirection.Input, CObj(Me.StoreNo)))
            cmd.Parameters.Add(CreateParam("@Item_Key", SqlDbType.Int, ParameterDirection.Input, CObj(Me.Item_Key)))
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                While dr.Read
                    results.Add(New ItemCatalog.StoreItem.Vendor(dr!Vendor_ID, dr!Vendor_Key, dr!CompanyName, dr!Address_Line_1, dr!Address_Line_2, dr!City, dr!State, dr!Zip_Code, dr!Country, dr!Phone, dr!UnitCost, dr!UnitFreight, dr!Package_Desc1, dr!MSRP, dr!PrimaryVendor))
                End While
            End If

            Return results
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
    Public Function IsOrderable(ByVal ThrowExceptions As Boolean) As Boolean
        If Me.IsForRetailSale And (MyBase.SubTeam_No = Me.SubTeam_No) And Not Me.IsSellable Then
            If ThrowExceptions Then
                Throw New ItemCatalog.Exception.OrderItemQueue.NotSoldException
            Else
                Return False
            End If
        End If
        Return True
    End Function
    Public Class Vendor
        Inherits ItemCatalog.Vendor
        Private m_unit_cost As Decimal
        Private m_unit_freight As Decimal
        Private m_pack_size As Decimal
        Private m_msrp As Decimal
        Private m_primary As Boolean
        Public Property UnitCost() As Decimal
            Get
                Return Me.m_unit_cost
            End Get
            Set(ByVal Value As Decimal)
                Me.m_unit_cost = Value
            End Set
        End Property
        Public Property UnitFreight() As Decimal
            Get
                Return Me.m_unit_freight
            End Get
            Set(ByVal Value As Decimal)
                Me.m_unit_freight = Value
            End Set
        End Property
        Public Property PackSize() As Decimal
            Get
                Return Me.m_pack_size
            End Get
            Set(ByVal Value As Decimal)
                Me.m_pack_size = Value
            End Set
        End Property
        Public Property MSRP() As Decimal
            Get
                Return Me.m_msrp
            End Get
            Set(ByVal Value As Decimal)
                Me.m_msrp = Value
            End Set
        End Property
        Public Property IsPrimary() As Boolean
            Get
                Return Me.m_primary
            End Get
            Set(ByVal Value As Boolean)
                Me.m_primary = Value
            End Set
        End Property
        Protected Friend Sub New(ByVal VendorID As Long, ByVal Key As String, ByVal CompanyName As String, ByVal AddressLine1 As String, ByVal AddressLine2 As String, ByVal City As String, ByVal State As String, ByVal ZipCode As String, ByVal Country As String, ByVal Phone As String, ByVal UnitCost As Decimal, ByVal UnitFreight As Decimal, ByVal PackSize As Decimal, ByVal MSRP As Decimal, ByVal IsPrimary As Boolean)
            MyBase.VendorID = VendorID
            MyBase.Key = Key
            MyBase.CompanyName = CompanyName
            MyBase.AddressLine1 = AddressLine1
            MyBase.AddressLine2 = AddressLine2
            MyBase.City = City
            MyBase.State = State
            MyBase.ZipCode = ZipCode
            MyBase.Country = Country
            MyBase.Phone = Phone
            Me.m_msrp = MSRP
            Me.m_pack_size = PackSize
            Me.m_primary = IsPrimary
            Me.m_unit_cost = UnitCost
            Me.m_unit_freight = UnitFreight
        End Sub
    End Class
End Class
