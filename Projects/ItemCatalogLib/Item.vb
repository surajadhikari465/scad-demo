Imports WFM.DataAccess.Setup
Imports log4net
Imports log4net.Config
Imports System.Reflection



Public Enum enuItemUnitSortOrder
    ID = 0
    Name = 1
    Abbr = 2
    SystemCode = 3
End Enum
Public Class Item
    Dim m_item_key As Long
    Dim m_identifier As String
    Dim m_item_description As String
    Dim m_pos_description As String
    Dim m_sign_description As String
    Dim m_subteam_no As Long
    Dim m_subteam_name As String
    Dim m_package_desc1 As Decimal
    Dim m_package_desc2 As Decimal
    Dim m_package_unit_abbr As String
    Dim m_not_available As Boolean
    Dim m_discontinue_item As Boolean
    Dim m_sold_by_weight As Boolean
    Dim m_wfm As Boolean
    Dim m_hfm As Boolean
    Dim m_retail_sale As Boolean
    Dim m_vendor_unit_id As Integer
    Dim m_vendor_unit_name As String
    Dim m_brand_name As String
    Dim m_package_unit_name As String
    Dim m_catchweight_requried As Boolean
    Dim m_COOL As Boolean
    Dim m_BIO As Boolean
    Dim m_pricechgtype As String
    Dim m_vendorname As String
    'Dim m_vendorpack As Decimal
    Dim m_identifier_list As ArrayList

    Private Shared ReadOnly logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)



    Public Property Item_Key() As Long
        Get
            Return m_item_key
        End Get
        Set(ByVal Value As Long)
            m_item_key = Value
        End Set
    End Property
    Public ReadOnly Property Identifiers() As ArrayList
        Get
            Return Me.m_identifier_list
        End Get
    End Property
    Public Property Identifier() As String
        Get
            Return m_identifier
        End Get
        Set(ByVal Value As String)
            m_identifier = Value
        End Set
    End Property
    Public Property IsForRetailSale() As Boolean
        Get
            Return m_retail_sale
        End Get
        Set(ByVal Value As Boolean)
            m_retail_sale = Value
        End Set
    End Property
    Public ReadOnly Property IsScaleItem() As Boolean
        Get
            Return (Identifier.Length = 11 And Left(Identifier, 1) = "2" And Right(Identifier, 5) = "00000")
        End Get
    End Property
    Public Property IsSoldByWeight() As Boolean
        Get
            Return m_sold_by_weight
        End Get
        Set(ByVal Value As Boolean)
            m_sold_by_weight = Value
        End Set
    End Property
    Public Property IsSoldHFM() As Boolean
        Get
            Return m_hfm
        End Get
        Set(ByVal Value As Boolean)
            m_hfm = Value
        End Set
    End Property
    Public Property IsSoldWFM() As Boolean
        Get
            Return m_wfm
        End Get
        Set(ByVal Value As Boolean)
            m_wfm = Value
        End Set
    End Property
    Public Property Item_Description() As String
        Get
            Return m_item_description
        End Get
        Set(ByVal Value As String)
            m_item_description = Value
        End Set
    End Property
    Public Property POS_Description() As String
        Get
            Return m_pos_description
        End Get
        Set(ByVal Value As String)
            m_pos_description = Value
        End Set
    End Property
    Public Property Sign_Description() As String
        Get
            Return Me.m_sign_description
        End Get
        Set(ByVal Value As String)
            m_sign_description = Value
        End Set
    End Property
    Public Property SubTeam_No() As Long
        Get
            Return m_subteam_no
        End Get
        Set(ByVal Value As Long)
            m_subteam_no = Value
        End Set
    End Property
    Public Property SubTeamName() As String
        Get
            Return m_subteam_name
        End Get
        Set(ByVal Value As String)
            m_subteam_name = Value
        End Set
    End Property
    Public Property PriceChgType() As String
        Get
            Return m_pricechgtype
        End Get
        Set(ByVal Value As String)
            m_pricechgtype = Value
        End Set
    End Property
    Public Property Package_Desc1() As Decimal
        Get
            Return Me.m_package_desc1
        End Get
        Set(ByVal Value As Decimal)
            Me.m_package_desc1 = Value
        End Set
    End Property
    Public Property Package_Desc2() As Decimal
        Get
            Return Me.m_package_desc2
        End Get
        Set(ByVal Value As Decimal)
            Me.m_package_desc2 = Value
        End Set
    End Property
    Public Property Package_Unit_Abbr() As String
        Get
            Return Me.m_package_unit_abbr
        End Get
        Set(ByVal Value As String)
            Me.m_package_unit_abbr = Value
        End Set
    End Property
    Public Property IsNotAvailable() As Boolean
        Get
            Return m_not_available
        End Get
        Set(ByVal Value As Boolean)
            m_not_available = Value
        End Set
    End Property
    Public Property IsDiscontinued() As Boolean
        Get
            Return Me.m_discontinue_item
        End Get
        Set(ByVal Value As Boolean)
            m_discontinue_item = Value
        End Set
    End Property
    Public Property VendorOrderUnitID() As Integer
        Get
            Return Me.m_vendor_unit_id
        End Get
        Set(ByVal Value As Integer)
            Me.m_vendor_unit_id = Value
        End Set
    End Property
    Public Property VendorOrderUnitName() As String
        Get
            Return Me.m_vendor_unit_name
        End Get
        Set(ByVal Value As String)
            Me.m_vendor_unit_name = Value
        End Set
    End Property
    Public Property VendorName() As String
        Get
            Return Me.m_vendorname
        End Get
        Set(ByVal Value As String)
            Me.m_vendorname = Value
        End Set
    End Property
    'Public Property VendorPack() As Decimal
    '    Get
    '        Return Me.m_vendorpack
    '    End Get
    '    Set(ByVal Value As Decimal)
    '        Me.m_vendorpack = Value
    '    End Set
    'End Property
    Public Property COOL() As Boolean
        Get
            Return m_COOL
        End Get
        Set(ByVal Value As Boolean)
            m_COOL = Value
        End Set
    End Property
    Public Property BIO() As Boolean
        Get
            Return m_BIO
        End Get
        Set(ByVal Value As Boolean)
            m_BIO = Value
        End Set
    End Property

    Public Property CatchWeightRequired() As Boolean
        Get
            Return m_catchweight_requried
        End Get
        Set(ByVal value As Boolean)
            m_catchweight_requried = value
        End Set
    End Property

    Public Property BrandName() As String
        Get
            Return Me.m_brand_name
        End Get
        Set(ByVal Value As String)
            Me.m_brand_name = Value
        End Set
    End Property
    Public Property PackageUnitName() As String
        Get
            Return Me.m_package_unit_name
        End Get
        Set(ByVal Value As String)
            Me.m_package_unit_name = Value
        End Set
    End Property
    Public Sub New()
    End Sub
    Public Sub New(ByVal lItem_Key As Long, ByVal sItem_Description As String, ByVal sIdentifier As String, ByVal lSubTeam_No As Long)
        Me.Item_Key = lItem_Key
        Me.Identifier = sIdentifier
        Me.Item_Description = sItem_Description
        Me.SubTeam_No = lSubTeam_No
    End Sub
    Public Sub New(ByVal lItem_Key As Long, ByVal sIdentifier As String, ByVal sItem_Description As String, ByVal sPOS_Description As String, ByVal lSubTeam_No As Long, ByVal sSubTeamName As String, ByVal dPackageDesc1 As Decimal, ByVal dPackageDesc2 As Decimal, ByVal bNotAvailable As Boolean, ByVal bCOOL As Boolean, ByVal bBIO As Boolean, ByVal bCatchWeightRequired As Boolean, Optional ByVal sPackageUnitAbbr As String = "")
        Me.Item_Key = lItem_Key
        Me.Identifier = sIdentifier
        Me.Item_Description = sItem_Description
        Me.POS_Description = sPOS_Description
        Me.SubTeam_No = lSubTeam_No
        Me.SubTeamName = sSubTeamName
        Me.Package_Desc1 = dPackageDesc1
        Me.Package_Desc2 = dPackageDesc2
        Me.IsNotAvailable = bNotAvailable
        Me.COOL = bCOOL
        Me.BIO = bBIO
        Me.CatchWeightRequired = bCatchWeightRequired
        Me.Package_Unit_Abbr = sPackageUnitAbbr
    End Sub
    Public Sub New(ByVal oItem As ItemCatalog.Item)
        Me.IsDiscontinued = oItem.IsDiscontinued
        Me.IsForRetailSale = oItem.IsForRetailSale
        Me.IsNotAvailable = oItem.IsNotAvailable
        Me.IsSoldByWeight = oItem.IsSoldByWeight
        Me.IsSoldHFM = oItem.IsSoldHFM
        Me.IsSoldWFM = oItem.IsSoldWFM
        Me.Item_Key = oItem.Item_Key
        Me.Identifier = oItem.Identifier
        Me.Item_Description = oItem.Item_Description
        Me.Package_Unit_Abbr = oItem.Package_Unit_Abbr
        Me.Package_Desc1 = oItem.Package_Desc1
        Me.Package_Desc2 = oItem.Package_Desc2
        Me.POS_Description = oItem.POS_Description
        Me.Sign_Description = oItem.Sign_Description
        Me.SubTeam_No = oItem.SubTeam_No
        Me.SubTeamName = oItem.SubTeamName
        Me.VendorOrderUnitID = oItem.VendorOrderUnitID
        Me.VendorOrderUnitName = oItem.VendorOrderUnitName
    End Sub
    Public Sub New(ByVal lItem_Key As Long)
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Dim cmd As New System.Data.SqlClient.SqlCommand
        Try
            Me.Item_Key = lItem_Key
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetItem"
            With cmd.Parameters
                .Add(CreateParam("@Item_Key", SqlDbType.Int, ParameterDirection.Input, CObj(lItem_Key)))
                .Add(CreateParam("@Identifier", SqlDbType.VarChar, ParameterDirection.Input, System.DBNull.Value, 255))
            End With

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                dr.Read()
                Construct(dr)
            Else
                Throw New System.Exception("Invalid Item Key")
            End If
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Sub New(ByVal sIdentifier As String)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetItem"

            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Item_Key"
            prm.Value = System.DBNull.Value
            cmd.Parameters.Add(prm)

            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.String
            prm.ParameterName = "@Identifier"
            prm.Value = sIdentifier.Trim
            cmd.Parameters.Add(prm)

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                dr.Read()
                Construct(dr)
            Else
                Throw New ItemCatalog.Exception.InvalidIdentifierException
            End If
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Private Sub Construct(ByVal dr As System.Data.SqlClient.SqlDataReader)
        Me.m_item_key = CType(dr!Item_Key, Long)
        Me.m_identifier = CType(dr!Identifier, String)
        Me.m_item_description = CType(dr!Item_Description, String)
        Me.m_pos_description = CType(dr!POS_Description, String)
        Me.m_sign_description = dr!Sign_Description
        Me.m_subteam_no = CType(dr!SubTeam_No, Long)
        Me.m_subteam_name = CType(dr!SubTeam_Name, String)
        Me.m_package_desc1 = CType(dr!Package_Desc1, Decimal)
        Me.m_package_desc2 = CType(dr!Package_Desc2, Decimal)
        Me.m_package_unit_abbr = dr!Package_Unit_Abbr
        Me.m_not_available = CType(dr!Not_Available, Boolean)
        Me.m_discontinue_item = dr!Discontinue_Item
        Me.m_sold_by_weight = dr!Sold_By_Weight
        Me.m_hfm = dr!HFM_Item
        Me.m_wfm = dr!WFM_Item
        Me.m_retail_sale = dr!Retail_Sale
        Me.m_vendor_unit_id = IIf(IsDBNull(dr!Vendor_Unit_ID), 0, dr!Vendor_Unit_ID)
        Me.m_vendor_unit_name = IIf(IsDBNull(dr!Vendor_Unit_Name), String.Empty, dr!Vendor_Unit_Name)
        Me.m_catchweight_requried = IIf(IsDBNull(dr!CatchweightRequired), False, CType(dr!CatchweightRequired, Boolean))
    End Sub
    Friend Sub LoadIdentifier(ByVal sIdentifier As String)
        If Not IsMyIdentifier(sIdentifier) Then
            If Me.m_identifier_list Is Nothing Then Me.m_identifier_list = New ArrayList
            Me.m_identifier_list.Add(sIdentifier)
        End If
    End Sub
    Public Function IsMyIdentifier(ByVal sIdentifier As String) As Boolean
        If Me.m_identifier_list Is Nothing Then
            Return False
        Else
            Return (Me.m_identifier_list.BinarySearch(sIdentifier) >= 0)
        End If
    End Function
    Public Shared Function ConvertCost(ByVal Amount As Decimal, ByVal FromUnit As Long, ByVal ToUnit As Long, ByVal PD1 As Decimal, ByVal PD2 As Decimal, ByVal PDU As Long, ByVal Total_Weight As Decimal, ByVal Received As Decimal) As Decimal
        Dim iuFrom As ItemCatalog.ItemUnit
        Dim iuTo As ItemCatalog.ItemUnit
        Dim iuPDU As ItemCatalog.ItemUnit
        Dim result As Decimal

        iuFrom = ItemCatalog.Item.GetItemUnit(FromUnit)
        iuTo = ItemCatalog.Item.GetItemUnit(ToUnit)
        iuPDU = ItemCatalog.Item.GetItemUnit(PDU)

        '-- Adjust PD1 based on the weight received if dealing with boxes
        If Total_Weight <> 0 And Received <> 0 And (iuFrom.IsPackageUnit Or iuTo.IsPackageUnit) Then PD1 = Total_Weight / (Received * PD2)

        If FromUnit <> ToUnit Then
            If iuFrom.IsWeightUnit Then
                If iuTo.IsPackageUnit Then
                    result = Amount * PD1 * PD2
                Else
                    result = Amount
                End If
            Else
                If Not iuFrom.IsPackageUnit Then
                    If iuTo.IsPackageUnit Then
                        result = Amount * PD1
                    Else
                        result = Amount
                    End If
                Else 'FromUnit is a package
                    result = Amount / (PD1 * IIf(iuTo.IsWeightUnit, PD2, 1))
                End If
            End If
        Else
            result = Amount
        End If

        Return result

    End Function
    Public Shared Function ConvertItemUnit(ByVal Amount As Decimal, ByVal FromUnitID As Long, ByVal ToUnitID As Long) As Decimal
        ' !!! these paths are not Project Jeannie friendly.
        Dim result As Decimal

        result = Amount

        Dim ds As New System.Data.DataSet
        Dim results As New ArrayList

        If Dir(My.Application.Info.DirectoryPath & "\ItemUnitConversion.xml") = "" Then
            Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
            Try
                cmd = New System.Data.SqlClient.SqlCommand
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "GetItemConversionAll"
                ds = ItemCatalog.DataAccess.GetSqlDataSet(cmd, DataAccess.enuDBList.ItemCatalog)
                ds.WriteXml(My.Application.Info.DirectoryPath & "\ItemUnitConversion.xml", XmlWriteMode.WriteSchema)
            Finally
                ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
            End Try
        Else
            ds.ReadXml(My.Application.Info.DirectoryPath & "\ItemUnitConversion.xml", XmlReadMode.ReadSchema)
        End If

        Dim dr As System.Data.DataRow() = ds.Tables(0).Select("FromUnit_ID = " & FromUnitID & " AND ToUnit_ID = " & ToUnitID)

        If dr.GetLength(0) = 1 Then
            Select Case dr(0)!ConversionSymbol
                Case "*" : result = result * dr(0)!ConversionFactor
                Case "/" : result = result / dr(0)!ConversionFactor
                Case "+" : result = result + dr(0)!ConversionFactor
                Case "-" : result = result - dr(0)!ConversionFactor
            End Select
        End If

        dr = Nothing
        ds.Clear()
        ds.Dispose()

        ConvertItemUnit = result

    End Function
    Public Shared Function GetItemUnit(ByVal UnitID As Long) As ItemCatalog.ItemUnit
        Dim units As ArrayList = ItemCatalog.Item.GetItemUnits

        units.Sort(New ItemUnitSort(enuItemUnitSortOrder.ID))
        Dim iuSearch As New ItemCatalog.ItemUnit
        iuSearch.UnitID = UnitID
        Dim i As Integer = units.BinarySearch(iuSearch, New ItemUnitIDGet)
        If i >= 0 Then Return CType(units(i), ItemUnit)
    End Function
    Public Shared Function GetItemUnit(ByVal SystemCode As String) As ItemCatalog.ItemUnit
        Dim units As ArrayList = ItemCatalog.Item.GetItemUnits

        units.Sort(New ItemUnitSort(enuItemUnitSortOrder.SystemCode))
        Dim iuSearch As New ItemCatalog.ItemUnit
        iuSearch.SystemCode = SystemCode
        Dim i As Integer = units.BinarySearch(iuSearch, New ItemUnitSystemCodeGet)
        If i >= 0 Then Return CType(units(i), ItemUnit)
    End Function
    Public Shared Function GetItemUnits() As ArrayList
        Dim ds As New System.Data.DataSet
        Dim results As New ArrayList
        Dim sItemUnitsFilePath As String = String.Format("{0}\{1}\ItemUnit.xml", My.Application.Info.DirectoryPath, Environment.GetCommandLineArgs.GetValue(1).ToString)

        logger.InfoFormat("Getting Item Units: {0}:", sItemUnitsFilePath)
        If Dir(sItemUnitsFilePath) = "" Then
            Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
            Try
                cmd = New System.Data.SqlClient.SqlCommand
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "GetItemUnits"
                ds = ItemCatalog.DataAccess.GetSqlDataSet(cmd, DataAccess.enuDBList.ItemCatalog)
                ds.WriteXml(sItemUnitsFilePath, XmlWriteMode.WriteSchema)
                logger.InfoFormat("Got Item Units from DB. Caching: {0}", sItemUnitsFilePath)
            Finally
                ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
            End Try
        Else
            ds.ReadXml(sItemUnitsFilePath, XmlReadMode.ReadSchema)
            logger.InfoFormat("Got Item Units from disk cache: {0}", sItemUnitsFilePath)
        End If

        Dim i As Long
        Dim r As System.Data.DataRow
        For i = 0 To ds.Tables(0).Rows.Count - 1
            r = ds.Tables(0).Rows.Item(i)
            results.Add(New ItemCatalog.ItemUnit(r!Unit_ID, r!Unit_Name, r!Unit_Abbreviation, r!UnitSysCode, r!Weight_Unit, r!IsPackageUnit))
        Next

        Return results
    End Function

    Public Shared Function GetP2PItemUnits() As ArrayList
        Dim ds As New System.Data.DataSet
        Dim results As New ArrayList
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing

        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetItemUnits"
            ds = ItemCatalog.DataAccess.GetSqlDataSet(cmd, DataAccess.enuDBList.ItemCatalog)

            Dim i As Long
            Dim r As System.Data.DataRow
            For i = 0 To ds.Tables(0).Rows.Count - 1
                r = ds.Tables(0).Rows.Item(i)
                results.Add(New ItemCatalog.ItemUnit(r!Unit_ID, r!Unit_Name, r!Unit_Abbreviation, r!UnitSysCode, r!Weight_Unit, r!IsPackageUnit, r!EDISysCode))
            Next
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try

        Return results
    End Function
    Friend Sub GetItemIdentifersForItem(ByVal sItem_Key As Integer)

        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing

        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetItemIdentifersForItem"

            cmd.Parameters.Add(CreateParam("@Item_Key", SqlDbType.Int, ParameterDirection.Input, CObj(sItem_Key)))

            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                While dr.Read
                    Me.LoadIdentifier(dr!Identifier)
                End While
            End If

        Finally

            ItemCatalog.DataAccess.ReleaseDataObject(CObj(dr), DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(CObj(cmd), DataAccess.enuDBList.ItemCatalog)

        End Try

    End Sub
End Class
'End Class
Public Class ItemUnit
    Dim m_unit_id As Long
    Dim m_unit_name As String
    Dim m_unit_abbr As String
    Dim m_system_code As String
    'bubba - needed?  Moved to Item.
    Dim m_edi_system_code As String
    Dim m_weight_unit As Boolean
    Dim m_package_unit As Boolean

    Public Property UnitID() As Long
        Get
            Return Me.m_unit_id
        End Get
        Set(ByVal Value As Long)
            Me.m_unit_id = Value
        End Set
    End Property
    Public Property UnitName() As String
        Get
            Return Me.m_unit_name
        End Get
        Set(ByVal Value As String)
            Me.m_unit_name = Value
        End Set
    End Property
    Public Property UnitAbbreviation() As String
        Get
            Return Me.m_unit_abbr
        End Get
        Set(ByVal Value As String)
            Me.m_unit_abbr = Value
        End Set
    End Property
    Public Property SystemCode() As String
        Get
            Return Me.m_system_code
        End Get
        Set(ByVal Value As String)
            Me.m_system_code = Value
        End Set
    End Property
    Public Property EDISystemCode() As String
        Get
            Return Me.m_edi_system_code
        End Get
        Set(ByVal Value As String)
            Me.m_edi_system_code = Value
        End Set
    End Property
    Public Property IsPackageUnit() As Boolean
        Get
            Return Me.m_package_unit
        End Get
        Set(ByVal Value As Boolean)
            Me.m_package_unit = Value
        End Set
    End Property
    Public ReadOnly Property IsUnit() As Boolean
        Get
            Return (Not IsPackageUnit) And (Not IsWeightUnit)
        End Get
    End Property
    Public Property IsWeightUnit() As Boolean
        Get
            Return Me.m_weight_unit
        End Get
        Set(ByVal Value As Boolean)
            Me.m_weight_unit = Value
        End Set
    End Property

    Public Sub New()
    End Sub

    Public Sub New(ByVal ID As Long, ByVal Name As String, ByVal Abbreviation As String, ByVal SystemCode As String, ByVal IsWeightUnit As Boolean, ByVal IsPackageUnit As Boolean)
        Me.m_unit_id = ID
        Me.m_unit_name = Name
        Me.m_unit_abbr = Abbreviation
        Me.m_system_code = SystemCode
        Me.m_weight_unit = IsWeightUnit
        Me.m_package_unit = IsPackageUnit
    End Sub

    Public Sub New(ByVal ID As Long, ByVal Name As String, ByVal Abbreviation As String, ByVal SystemCode As String, ByVal IsWeightUnit As Boolean, ByVal IsPackageUnit As Boolean, ByVal EDISystemCode As String)
        Me.m_unit_id = ID
        Me.m_unit_name = Name
        Me.m_unit_abbr = Abbreviation
        Me.m_system_code = SystemCode
        Me.m_weight_unit = IsWeightUnit
        Me.m_package_unit = IsPackageUnit
        Me.m_edi_system_code = EDISystemCode
    End Sub

End Class
Public Class ItemUnitIDGet
    Implements IComparer
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Compare = CType(x, ItemUnit).UnitID.CompareTo(CType(y, ItemUnit).UnitID)
    End Function
End Class
Public Class ItemUnitNameGet
    Implements IComparer
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Compare = CType(x, ItemUnit).UnitName.CompareTo(CType(y, ItemUnit).UnitName)
    End Function
End Class
Public Class ItemUnitSystemCodeGet
    Implements IComparer
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Compare = CType(x, ItemUnit).SystemCode.CompareTo(CType(y, ItemUnit).SystemCode)
    End Function
End Class
Public Class ItemUnitSort
    Implements IComparer
    Dim CompType As enuItemUnitSortOrder
    Public Sub New(ByVal xCompType As enuItemUnitSortOrder)
        CompType = xCompType
    End Sub
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Select Case CompType
            Case enuItemUnitSortOrder.ID
                Compare = CType(x, ItemUnit).UnitID.CompareTo(CType(y, ItemUnit).UnitID)
            Case enuItemUnitSortOrder.Name
                Compare = CType(x, ItemUnit).UnitName.CompareTo(CType(y, ItemUnit).UnitName)
            Case enuItemUnitSortOrder.Abbr
                Compare = CType(x, ItemUnit).UnitAbbreviation.CompareTo(CType(y, ItemUnit).UnitAbbreviation)
            Case enuItemUnitSortOrder.SystemCode
                Compare = CType(x, ItemUnit).SystemCode.CompareTo(CType(y, ItemUnit).SystemCode)
        End Select
    End Function
End Class

