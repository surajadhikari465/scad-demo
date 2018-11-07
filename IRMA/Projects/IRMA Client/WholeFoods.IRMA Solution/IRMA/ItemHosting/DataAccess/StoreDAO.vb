Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess
Imports log4net


Namespace WholeFoods.IRMA.ItemHosting.DataAccess
    Public Class StoreDAO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Shared Function CreateStoreDataTable() As DataTable

            logger.Debug("CreateStoreDataTable Entry")

            Dim table As New DataTable

            'add columns to table
            table.Columns.Add(New DataColumn("Store_No", GetType(Int32)))
            table.Columns.Add(New DataColumn("Store_Name", GetType(String)))
            table.Columns.Add(New DataColumn("Zone_ID", GetType(Int16)))
            table.Columns.Add(New DataColumn("State", GetType(String)))
            table.Columns.Add(New DataColumn("WFM_Store", GetType(Boolean)))
            table.Columns.Add(New DataColumn("Mega_Store", GetType(Boolean)))
            table.Columns.Add(New DataColumn("CustomerType", GetType(Byte)))

            logger.Debug("CreateStoreDataTable Exit")

            Return table
        End Function

        Public Shared Function GetStoreList() As DataTable
            Return GetStoreList(CreateStoreDataTable())
        End Function

        Public Shared Function GetStoreAndDCList() As DataTable
            Return GetStoreAndDCList(CreateStoreDataTable())
        End Function


        Public Shared Function GetStoreAndDCList(ByVal table As DataTable) As DataTable
            logger.Debug("GetStoreAndDCList Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim row As DataRow

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetStoresAndDist")

                While results.Read
                    row = table.NewRow

                    row("Store_No") = results.GetInt32(results.GetOrdinal("Store_No"))
                    row("Store_Name") = results.GetString(results.GetOrdinal("Store_Name"))
                    If (Not results.IsDBNull(results.GetOrdinal("Zone_ID"))) Then
                        row("Zone_ID") = results.GetInt32(results.GetOrdinal("Zone_ID"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("State"))) Then
                        row("State") = results.GetString(results.GetOrdinal("State"))
                    End If
                    row("WFM_Store") = results.GetBoolean(results.GetOrdinal("WFM_Store"))
                    row("Mega_Store") = results.GetBoolean(results.GetOrdinal("Mega_Store"))
                    If (Not results.IsDBNull(results.GetOrdinal("CustomerType"))) Then
                        row("CustomerType") = results.GetByte(results.GetOrdinal("CustomerType"))
                    End If

                    table.Rows.Add(row)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetStoreAndDCList Exit")

            Return table
        End Function



        Public Shared Function GetStoreList(ByVal table As DataTable) As DataTable

            logger.Debug("GetStoreList")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim row As DataRow

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetStores")

                While results.Read
                    row = table.NewRow

                    row("Store_No") = results.GetInt32(results.GetOrdinal("Store_No"))
                    row("Store_Name") = results.GetString(results.GetOrdinal("Store_Name"))
                    If (Not results.IsDBNull(results.GetOrdinal("Zone_ID"))) Then
                        row("Zone_ID") = results.GetInt32(results.GetOrdinal("Zone_ID"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("State"))) Then
                        row("State") = results.GetString(results.GetOrdinal("State"))
                    End If
                    row("WFM_Store") = results.GetBoolean(results.GetOrdinal("WFM_Store"))
                    row("Mega_Store") = results.GetBoolean(results.GetOrdinal("Mega_Store"))
                    If (Not results.IsDBNull(results.GetOrdinal("CustomerType"))) Then
                        row("CustomerType") = results.GetByte(results.GetOrdinal("CustomerType"))
                    End If

                    table.Rows.Add(row)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetStoreList Exit")

            Return table
        End Function

        Public Shared Function GetRetailStoreList() As DataTable

            logger.Debug("GetRetailStoreList Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim row As DataRow
            Dim dtStores As New DataTable("StoreList")

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetRetailStores")

                'add columns to table
                dtStores.Columns.Add(New DataColumn("Store_No", GetType(Integer)))
                dtStores.Columns.Add(New DataColumn("Store_Name", GetType(String)))
                dtStores.Columns.Add(New DataColumn("Zone_ID", GetType(Integer)))
                dtStores.Columns.Add(New DataColumn("State", GetType(String)))
                dtStores.Columns.Add(New DataColumn("WFM_Store", GetType(Boolean)))
                dtStores.Columns.Add(New DataColumn("Mega_Store", GetType(Boolean)))
                dtStores.Columns.Add(New DataColumn("CustomerType", GetType(Integer)))
                dtStores.Columns.Add(New DataColumn("IsGPMStore", GetType(Boolean)))

                While results.Read
                    row = dtStores.NewRow

                    row("Store_No") = results.GetInt32(results.GetOrdinal("Store_No"))
                    row("Store_Name") = results.GetString(results.GetOrdinal("Store_Name"))
                    If (Not results.IsDBNull(results.GetOrdinal("Zone_ID"))) Then
                        row("Zone_ID") = results.GetInt32(results.GetOrdinal("Zone_ID"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("State"))) Then
                        row("State") = results.GetString(results.GetOrdinal("State"))
                    End If
                    row("WFM_Store") = results.GetBoolean(results.GetOrdinal("WFM_Store"))
                    row("Mega_Store") = results.GetBoolean(results.GetOrdinal("Mega_Store"))
                    If (Not results.IsDBNull(results.GetOrdinal("CustomerType"))) Then
                        row("CustomerType") = results.GetByte(results.GetOrdinal("CustomerType"))
                    End If
                    row("IsGPMStore") = InstanceDataDAO.IsFlagActive("GlobalPriceManagement", row("Store_No"))

                    dtStores.Rows.Add(row)
                End While

                dtStores.AcceptChanges()
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetRetailStoreList Exit")

            Return dtStores
        End Function

        ''' <summary>
        ''' Cheks to see if an item is authorized for sale and ordering at a store.
        ''' </summary>
        ''' <param name="storeNo"></param>
        ''' <param name="itemKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function IsItemAuthorized(ByVal storeNo As Integer, ByVal itemKey As Integer) As Boolean

            logger.Debug("IsItemAuthorized Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim authorized As Boolean = False

            ' Execute the function
            authorized = CType(factory.ExecuteScalar("SELECT dbo.fn_IsItemAuthorizedForStore(" & itemKey & ", " & storeNo & ")"), Boolean)

            logger.Debug("IsItemAuthorized Exit")

            Return authorized
        End Function

        Public Shared Function GetStoreItemVendorList(ByVal itemKey As Integer, ByVal vendorID As Integer) As DataTable

            logger.Debug("GetStoreItemVendorList Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim row As DataRow
            Dim dtStores As New DataTable("StoreList")
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = itemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Vendor_ID"
                currentParam.Value = vendorID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetStoreItemVendorStores", paramList)

                'add columns to table
                dtStores.Columns.Add(New DataColumn("Store_No", GetType(Integer)))
                dtStores.Columns.Add(New DataColumn("Store_Name", GetType(String)))
                dtStores.Columns.Add(New DataColumn("Zone_ID", GetType(Integer)))
                dtStores.Columns.Add(New DataColumn("State", GetType(String)))
                dtStores.Columns.Add(New DataColumn("WFM_Store", GetType(Boolean)))
                dtStores.Columns.Add(New DataColumn("Mega_Store", GetType(Boolean)))
                dtStores.Columns.Add(New DataColumn("CustomerType", GetType(Integer)))
                dtStores.Columns.Add(New DataColumn("StoreJurisdictionID", GetType(Integer)))

                While results.Read
                    row = dtStores.NewRow

                    row("Store_No") = results.GetInt32(results.GetOrdinal("Store_No"))
                    row("Store_Name") = results.GetString(results.GetOrdinal("Store_Name"))
                    If (Not results.IsDBNull(results.GetOrdinal("Zone_ID"))) Then
                        row("Zone_ID") = results.GetInt32(results.GetOrdinal("Zone_ID"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("State"))) Then
                        row("State") = results.GetString(results.GetOrdinal("State"))
                    End If
                    row("WFM_Store") = results.GetBoolean(results.GetOrdinal("WFM_Store"))
                    row("Mega_Store") = results.GetBoolean(results.GetOrdinal("Mega_Store"))
                    If (Not results.IsDBNull(results.GetOrdinal("CustomerType"))) Then
                        row("CustomerType") = results.GetByte(results.GetOrdinal("CustomerType"))
                    End If
                    row("StoreJurisdictionID") = results.GetInt32(results.GetOrdinal("StoreJurisdictionID"))

                    dtStores.Rows.Add(row)
                End While

                dtStores.AcceptChanges()
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetStoreItemVendorList Exit")

            Return dtStores
        End Function

        Public Sub UpdateStore(ByVal storeData As StoreBO)

            logger.Debug("UpdateStore Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = storeData.StoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Store_Name"
                currentParam.Value = storeData.StoreName
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreAbbr"
                currentParam.Value = storeData.StoreAbbr
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Phone_Number"
                currentParam.Value = storeData.PhoneNumber
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Zone_ID"
                currentParam.Value = storeData.ZoneID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "BusinessUnit_ID"
                currentParam.Value = storeData.BusinessUnitID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "UNFI_Store"
                currentParam.Value = storeData.UNFIStore
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "EXEWarehouse"
                If storeData.EXEWarehouse IsNot Nothing Then
                    currentParam.Value = CType(storeData.EXEWarehouse, Int16)
                Else
                    currentParam.Value = DBNull.Value
                End If
                currentParam.Type = DBParamType.SmallInt
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Internal"
                currentParam.Value = storeData.IsInternal
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Regional"
                currentParam.Value = storeData.IsRegional
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Mega_Store"
                currentParam.Value = storeData.IsMegaStore
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "WFM_Store"
                currentParam.Value = storeData.IsWFMStore
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Distribution_Center"
                currentParam.Value = storeData.IsDistributionCenter
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Manufacturer"
                currentParam.Value = storeData.IsManufacturer
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxJurisdictionID"
                currentParam.Value = storeData.TaxJurisdictionID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PSI_Store_No"
                currentParam.Value = IIf(storeData.PSIStoreNo = 0, DBNull.Value, storeData.PSIStoreNo)
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreJurisdictionID"
                currentParam.Value = storeData.UpdatedStoreJurisdictionID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "GeoCode"
                If storeData.GeoCode.Length > 0 Then
                    currentParam.Value = storeData.GeoCode
                Else
                    currentParam.Value = DBNull.Value
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PLUMStoreNo"
                currentParam.Value = storeData.PLUMStoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("ItemHosting_UpdateStore", paramList)
            Catch ex As Exception
                'TODO handle exception
            End Try

            logger.Debug("UpdateStore Exit")
        End Sub

        Public Function CheckPLUMStoreNoExists(ByRef StoreNo As Integer, ByRef PLUMStoreNo As Integer) As Boolean
            logger.Debug("CheckUniquePLUMStoreNo Entry")
            Dim PLUMStoreNoFound As Boolean = False

            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Try
                Dim sql As String = "select dbo.fn_CheckUniquePLUMStoreNo(" & StoreNo.ToString() & ", " & PLUMStoreNo.ToString() & ")"

                ' Execute the stored procedure
                Dim results As Integer = CType(factory.ExecuteScalar(sql), Integer)
                If results = 1 Then
                    PLUMStoreNoFound = True
                End If
            Catch ex As Exception
                'TODO handle exception
            End Try
            logger.Debug("CheckUniquePLUMStoreNo Exit")

            Return PLUMStoreNoFound
        End Function
    End Class
End Namespace
