Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.POSPush.DataAccess
    Public Class POSWriterDAO
        ' Set the class type for logging
        Private Shared CLASSTYPE As Type = System.Type.GetType("WholeFoods.IRMA.Replenishment.POSPush.DataAccess.POSWriterDAO")

#Region "Read methods"
        ''' <summary>
        ''' Reads all of the POSWriterFileConfig values for the POS File Writer.
        ''' </summary>
        ''' <param name="fileWriterKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetPOSWriterFileConfigData(ByVal fileWriterKey As Integer) As SqlDataReader
            Logger.LogDebug("GetPOSWriterFileConfigData entry: fileWriterKey=" + fileWriterKey.ToString, CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As ArrayList
            ' setup parameters for stored proc
            paramList = New ArrayList

            currentParam = New DBParam
            currentParam.Name = "FileWriterKey"
            currentParam.Value = fileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataReader("Replenishment_POSPush_GetPOSWriterFileConfig", paramList)

            Logger.LogDebug("GetPOSWriterFileConfigData exit", CLASSTYPE)
            Return results
        End Function

        ''' <summary>
        ''' Reads all of the data dictionary values for the POS File Writer.
        ''' </summary>
        ''' <param name="fileWriterKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetPOSWriterDictionary(ByVal fileWriterKey As Integer) As ArrayList
            Logger.LogDebug("GetPOSWriterDictionary entry: fileWriterKey=" + fileWriterKey.ToString, CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As ArrayList
            Dim currentElement As POSWriterDictionaryBO
            Dim dictionaryCollection As New ArrayList()

            ' setup parameters for stored proc
            paramList = New ArrayList

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = fileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Administration_POSPush_GetPOSWriterDictionary", paramList)

                ' Parse the results into a collection of POSWriterDictionaryBO objects
                While (results.Read())
                    currentElement = New POSWriterDictionaryBO
                    currentElement.PopulateFromPOSWriterDictionary(results)
                    dictionaryCollection.Add(currentElement)
                End While
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try

            Logger.LogDebug("GetPOSWriterDictionary exit", CLASSTYPE)
            Return dictionaryCollection
        End Function

        ''' <summary>
        ''' Reads all of the Item Id Add records for the start date.
        ''' </summary>
        ''' <param name="dStart"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetItemIdAdds(ByVal dStart As Date) As SqlDataReader
            Return GetItemIdChanges(dStart, False, False, Nothing)
        End Function

        ''' <summary>
        ''' Reads all of the Item Id Delete records for the start date.
        ''' </summary>
        ''' <param name="dStart"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetItemIdDeletes(ByVal dStart As Date) As SqlDataReader
            Return GetItemIdChanges(dStart, True, False, Nothing)
        End Function

        ''' <summary>
        ''' Reads all of the Item Refresh records for the start date.
        ''' </summary>
        ''' <param name="dStart"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetItemRefreshes(ByVal dStart As Date) As SqlDataReader
            Return GetItemRefreshChanges(dStart, Nothing)
        End Function

        ''' <summary>
        ''' Reads all of the item records for the start date for all stores.  Used for audit reporting.
        ''' </summary>
        ''' <param name="dStart"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetAuditReportData(ByVal dStart As Date, ByVal storeNo As Integer) As SqlDataReader
            Return GetItemIdChanges(dStart, False, True, storeNo)
        End Function

        ''' <summary>
        ''' gets all Item ID Adds OR Deletes based on passed in flag
        ''' </summary>
        ''' <param name="dStart"></param>
        ''' <param name="storeNo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function GetItemRefreshChanges(ByVal dStart As Date, ByVal storeNo As Integer) As SqlDataReader
            Logger.LogDebug("GetItemIdChanges entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            Dim results As SqlDataReader = Nothing

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Date"
            currentParam.Value = dStart
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "AuditReport"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to read the item id adds 
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            results = factory.GetStoredProcedureDataReader("Replenishment_POSPush_GetIdentifierRefreshes", paramList)

            Logger.LogDebug("GetItemIdChanges exit", CLASSTYPE)
            Return results
        End Function

        ''' <summary>
        ''' gets all Item ID Adds OR Deletes based on passed in flag
        ''' </summary>
        ''' <param name="dStart"></param>
        ''' <param name="isDelete"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function GetItemIdChanges(ByVal dStart As Date, ByVal isDelete As Boolean, ByVal isAuditReport As Boolean, ByVal storeNo As Integer) As SqlDataReader
            Logger.LogDebug("GetItemIdChanges entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            Dim results As SqlDataReader = Nothing

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Date"
            currentParam.Value = dStart
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            If Not isDelete Then
                'add AuditReport param only for ADDS
                currentParam = New DBParam
                currentParam.Name = "AuditReport"
                currentParam.Value = isAuditReport
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                'add StoreNo param only for ADDS
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                If isAuditReport Then
                    currentParam.Value = storeNo
                Else
                    currentParam.Value = DBNull.Value
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
            Else
                currentParam = New DBParam
                currentParam.Name = "IsScaleZoneData"
                currentParam.Value = 0
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)
            End If

            If isDelete Then
                ' Execute the stored procedure to read the item id deletes 
                factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
                results = factory.GetStoredProcedureDataReader("Replenishment_POSPush_GetIdentifierDeletes", paramList)
            Else
                ' Execute the stored procedure to read the item id adds 
                factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
                results = factory.GetStoredProcedureDataReader("Replenishment_POSPush_GetIdentifierAdds", paramList)
            End If

            Logger.LogDebug("GetItemIdChanges exit", CLASSTYPE)
            Return results
        End Function

        Friend Shared Sub DeleteItemNonBatchableChanges(dStart As Date)
            Logger.LogDebug("DeleteItemNonBatchableChanges entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            currentParam = New DBParam
            currentParam.Name = "Date"
            currentParam.Value = dStart
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            factory.GetStoredProcedureDataReader("Replenishment_POSPush_DeleteItemNonBatchableChanges", paramList)

            Logger.LogDebug("GetItemIdChanges exit", CLASSTYPE)

        End Sub

        ''' <summary>
        ''' Reads all of the Item Data Change records for the start date.
        ''' </summary>
        ''' <param name="dStart"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetItemDataChanges(ByVal dStart As Date, ByVal getIconNonBatchableChanges As Boolean) As SqlDataReader
            Logger.LogDebug("GetItemDataChanges entry", CLASSTYPE)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            Dim results As SqlDataReader = Nothing

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Date"
            currentParam.Value = dStart
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Deletes"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MaxBatchItems"
            currentParam.Value = ConfigurationServices.AppSettings("MaxPOSBatchRecords")
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IsScaleZoneData"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IsItemNonBatchableChanges"
            currentParam.Value = getIconNonBatchableChanges
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure to read the price batch changes that are ready to be sent to the stores.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            results = factory.GetStoredProcedureDataReader("Replenishment_POSPush_GetPriceBatchSent", paramList)

            Logger.LogDebug("GetItemDataChanges exit", CLASSTYPE)

            Return results
        End Function

        ''' <summary>
        ''' Reads all of the Item Delete records for the start date.
        ''' </summary>
        ''' <param name="dStart"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetItemDeletes(ByVal dStart As Date) As SqlDataReader
            Logger.LogDebug("GetItemDeletes entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            Dim results As SqlDataReader = Nothing

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Date"
            currentParam.Value = dStart
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Deletes"
            currentParam.Value = 1
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MaxBatchItems"
            currentParam.Value = ConfigurationServices.AppSettings("MaxPOSBatchRecords")
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@IsScaleZoneData"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure to read the price batch deletes that are ready to be sent to the stores.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            results = factory.GetStoredProcedureDataReader("Replenishment_POSPush_GetPriceBatchSent", paramList)

            Logger.LogDebug("GetItemDeletes exit", CLASSTYPE)
            Return results
        End Function

        ''' <summary>
        ''' Reads all of the PromotionalOffer Change records for the start date.
        ''' </summary>
        ''' <param name="dStart"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetPromoOfferDataChanges(ByVal dStart As Date) As SqlDataReader
            Logger.LogDebug("GetPromoOfferDataChanges entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            Dim results As SqlDataReader = Nothing

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Date"
            currentParam.Value = dStart
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MaxBatchItems"
            currentParam.Value = ConfigurationServices.AppSettings("MaxPOSBatchRecords")
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to read the price batch changes that are ready to be sent to the stores.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            results = factory.GetStoredProcedureDataReader("Replenishment_POSPush_GetPriceBatchOffers", paramList)

            Logger.LogDebug("GetPromoOfferDataChanges exit", CLASSTYPE)
            Return results
        End Function

        Public Shared Function GetVendorAdds(ByVal isAuditReport As Boolean, Optional ByVal storeNo As String = Nothing) As SqlDataReader
            Logger.LogDebug("GetVendorAdds entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "AuditReport"
            currentParam.Value = isAuditReport
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            If storeNo IsNot Nothing Then
                currentParam.Value = storeNo
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to read the price batch changes that are ready to be sent to the stores.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            results = factory.GetStoredProcedureDataReader("Replenishment_POSPush_GetVendorAdds", paramList)

            Logger.LogDebug("GetVendorAdds exit", CLASSTYPE)
            Return results
        End Function

        ''' <summary>
        ''' returns hashtable of escape characters and their replacement chars for a given POSFileWriterKey
        ''' </summary>
        ''' <param name="fileWriterKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetPOSWriterEscapeChars(ByVal fileWriterKey As Integer) As Hashtable
            Logger.LogDebug("GetPOSWriterEscapeChars entry: fileWriterKey=" + fileWriterKey.ToString, CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            Dim escapeChars As New Hashtable
            Dim key As String = Nothing
            Dim value As String = Nothing

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = fileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Administration_POSPush_GetPOSWriterEscapeChars", paramList)

                While results.Read
                    key = Nothing
                    value = Nothing

                    If Not results.IsDBNull(results.GetOrdinal("EscapeCharValue")) Then
                        key = results.GetString(results.GetOrdinal("EscapeCharValue"))
                    End If

                    If Not results.IsDBNull(results.GetOrdinal("EscapeCharReplacement")) Then
                        value = results.GetString(results.GetOrdinal("EscapeCharReplacement"))
                    End If

                    If key IsNot Nothing AndAlso value IsNot Nothing Then
                        escapeChars.Add(key, value)
                    End If
                End While
            Catch ex As Exception
                'throw exception up to calling method
                Throw
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Logger.LogDebug("GetPOSWriterEscapeChars exit", CLASSTYPE)
            Return escapeChars
        End Function

        ''' <summary>
        ''' The Admin application provides the ability to define default batch ids to be set when a batch
        ''' is created.  This stored procedure will query the database to determine which default batch id,
        ''' if any, should be set.
        ''' </summary>
        ''' <param name="posFileWriter"></param>
        ''' <param name="posChangeType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetDefaultPOSWriterBatchId(ByVal posFileWriter As Integer, ByVal posChangeType As Integer) As String
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim reader As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim defaultBatchId As String = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "POSFileWriterKey"
                currentParam.Value = posFileWriter
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "POSChangeTypeKey"
                currentParam.Value = posChangeType
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                reader = factory.GetStoredProcedureDataReader("GetDefaultPOSWriterBatchId", paramList)

                While reader.Read
                    If reader.GetValue(reader.GetOrdinal("POSBatchIdDefault")).GetType IsNot GetType(DBNull) Then
                        defaultBatchId = reader.GetInt32(reader.GetOrdinal("POSBatchIdDefault")).ToString
                    End If
                End While
            Catch ex As Exception
                Throw
            Finally
                If reader IsNot Nothing Then
                    reader.Close()
                End If
            End Try

            Return defaultBatchId
        End Function

        ''' <summary>
        ''' Gets the data for the all the stores associated with the batch
        ''' </summary>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetBatchStores(dStart As Date)  As SqlDataReader
            Logger.LogDebug("GetBatchStores entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim result As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Date"
            currentParam.Value = dStart
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MaxBatchItems"
            currentParam.Value = ConfigurationServices.AppSettings("MaxPOSBatchRecords")
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to read the price batch changes that are ready to be sent to the stores.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            result = factory.GetStoredProcedureDataReader("Replenishment_POSPush_GetBatchDataForStores", paramList)

            Logger.LogDebug("GetBatchStores exit", CLASSTYPE)
            Return result

        End Function

                ''' <summary>
        ''' Gets the data for the all the stores associated with the batch
        ''' </summary>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetTaxFlags(dStart As Date)  As SqlDataReader
            Logger.LogDebug("GetTaxFlags entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim result As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Date"
            currentParam.Value = dStart
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MaxBatchItems"
            currentParam.Value = ConfigurationServices.AppSettings("MaxPOSBatchRecords")
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IsScaleZoneData"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to read the price batch changes that are ready to be sent to the stores.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            result = factory.GetStoredProcedureDataReader("Replenishment_POSPush_GetAllTaxflagData", paramList)

            Logger.LogDebug("GetTaxFlags exit", CLASSTYPE)
            Return result

        End Function
#End Region

#Region "Update methods"
        ''' <summary>
        ''' Updates the Item data contained in Promo Off Cost price batch detail records with the current
        ''' Item data.  
        ''' </summary>
        ''' <param name="dStart"></param>
        ''' <remarks></remarks>
        Public Shared Sub UpdatePromoOffCostDetails(ByVal dStart As Date)
            Logger.LogDebug("UpdatePromoOffCostDetails entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Date"
            currentParam.Value = dStart
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            ' Execute the stored procedure to update the Promo Off Cost price batch detail records with
            ' the latest Item data.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            factory.ExecuteStoredProcedure("Replenishment_POSPush_UpdateOffPromoCostRecords", paramList)

            Logger.LogDebug("UpdatePromoOffCostDetails exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Applies the Item Id Add changes to the ItemCatalog database.
        ''' </summary>
        ''' <param name="itemIdentifiers">A <see cref="DataTable"/> object that contains the Item Identifier values that need to be updated</param>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Sub ApplyItemIdAdds(ByVal itemIdentifiers As DataTable)
            Logger.LogDebug("ApplyItemIdAdds entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' Execute the stored procedure to update the item identifier record
            ' for the Identifier_ID, setting Add_Identifier = 0 (false)
            Dim cmdUpdateItems As SqlCommand = factory.GetDataCommand("Replenishment_POSPush_AddIdentifier", Nothing, True)
            Dim itemIdentifierParm As SqlParameter = cmdUpdateItems.Parameters.Add("@IdentifierIds", SqlDbType.Structured)
            
            itemIdentifierParm.Value = itemIdentifiers
            cmdUpdateItems.ExecuteNonQuery()

            Logger.LogDebug("ApplyItemIdAdds exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Applies the Item Refreshes to the ItemCatalog database.
        ''' </summary>
        ''' <param name="itemIdentifiers">A <see cref="DataTable"/> object that contains the list of the identifier ids that need to be updated</param>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Sub ApplyItemRefresh(ByVal itemIdentifiers As DataTable)
            Logger.LogDebug("ApplyItemRefresh entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Dim cmdUpdateItems As SqlCommand = factory.GetDataCommand("Replenishment_POSPush_UpdateRefreshSent", Nothing, True)
            Dim itemIdentifierParm As SqlParameter = cmdUpdateItems.Parameters.Add("@IdentifierIds", SqlDbType.Structured)
            
            itemIdentifierParm.Value = itemIdentifiers
            cmdUpdateItems.ExecuteNonQuery()
            
            Logger.LogDebug("ApplyItemRefresh exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Applies the Item Id Delete changes to the ItemCatalog database.
        ''' </summary>
        ''' <param name="itemIdentifiers">A <see cref="DataTable"/> object that contains the list of the identifier ids that need to be updated</param>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Sub ApplyItemIdDeletes(ByVal itemIdentifiers As DataTable)
            Logger.LogDebug("ApplyItemIdDeletes entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Dim cmdUpdateItems As SqlCommand = factory.GetDataCommand("Replenishment_POSPush_DeleteIdentifier", Nothing, True)
            Dim itemIdentifierParm As SqlParameter = cmdUpdateItems.Parameters.Add("@IdentifierIds", SqlDbType.Structured)
            
            itemIdentifierParm.Value = itemIdentifiers
            cmdUpdateItems.ExecuteNonQuery()

            Logger.LogDebug("ApplyItemIdDeletes exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Applies the Item Data changes to the ItemCatalog database.
        ''' </summary>
        ''' <param name="PriceBatchHeaderIds"></param>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Sub ApplyItemDataChanges(ByVal PriceBatchHeaderIds As DataTable)
            Logger.LogDebug("ApplyItemDataChanges entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Dim cmdUpdatePriceBatchHeaders As SqlCommand = factory.GetDataCommand("Replenishment_POSPush_UpdatePriceBatchProcessedChg", Nothing, True) ' As New SqlCommand("Replenishment_POSPush_UpdateRefreshSent")
            Dim itemIdentifierParm As SqlParameter = cmdUpdatePriceBatchHeaders.Parameters.Add("@PriceBatchHeaderIds", SqlDbType.Structured)
            
            itemIdentifierParm.Value = PriceBatchHeaderIds
            cmdUpdatePriceBatchHeaders.ExecuteNonQuery()

            Logger.LogDebug("ApplyItemDataChanges exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Applies the Item Delete changes to the ItemCatalog database.
        ''' </summary>
        ''' <param name="PriceBatchHeaderIds">A <see cref="DataTable"/> the contains the PriceBatchHeaderId's that need to be processed</param>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Sub ApplyItemDeletes(ByVal PriceBatchHeaderIds As DataTable)
            Logger.LogDebug("ApplyItemDeletes entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Dim cmdUpdatePriceBatchHeaders As SqlCommand = factory.GetDataCommand("Replenishment_POSPush_UpdatePriceBatchProcessedDel", Nothing, True) ' As New SqlCommand("Replenishment_POSPush_UpdateRefreshSent")
            Dim itemIdentifierParm As SqlParameter = cmdUpdatePriceBatchHeaders.Parameters.Add("@PriceBatchHeaderIds", SqlDbType.Structured)
            
            itemIdentifierParm.Value = PriceBatchHeaderIds
            cmdUpdatePriceBatchHeaders.ExecuteNonQuery()

            Logger.LogDebug("ApplyItemDeletes exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Applies the Promo Offer changes to the ItemCatalog database.
        ''' </summary>
        ''' <param name="headerData"></param>
        ''' <param name="currentStore"></param>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Sub ApplyPromoOffers(ByRef headerData As POSBatchHeaderBO, ByRef currentStore As StoreUpdatesBO)
            Logger.LogDebug("ApplyPromoOffers entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            Dim results As SqlDataReader = Nothing

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "PriceBatchHeaderID"
            currentParam.Value = headerData.BatchID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            'this batchID is at the STORE record level (not used for all POS systems)
            currentParam = New DBParam
            currentParam.Name = "POSBatchID"
            currentParam.Value = currentStore.BatchID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to update the PriceBatchHeader and update PromotionalOfferStore.Active flag
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            factory.ExecuteStoredProcedure("Replenishment_POSPush_UpdatePromoOffersProcessed", paramList)

            Logger.LogDebug("ApplyPromoOffers exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Applies the Vendor Id Add changes to the ItemCatalog database.
        ''' </summary>
        ''' <param name="vendorID"></param>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Sub ApplyVendorIdAdds(ByVal vendorID As Integer)
            Logger.LogDebug("ApplyVendorIdAdds entry: vendorKey=" + vendorID.ToString, CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Vendor_ID"
            currentParam.Value = vendorID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to update the vendor record
            ' for the Vendor_ID, setting AddVendor = 0 (false)
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            factory.ExecuteStoredProcedure("Replenishment_POSPush_UpdateVendorAddsProcessed", paramList)

            Logger.LogDebug("ApplyVendorIdAdds exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Applies the Scale DeAuthorization changes to the ItemCatalog database.
        ''' </summary>
        ''' <param name="StoreItemAuthorizationIds">A <see cref="DataTable"/> that contains the StoreItemAuthorizationId's that need to be processed</param>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Sub UpdateDeAuthorizedPOSChanges(ByVal StoreItemAuthorizationIds As DataTable)
            Logger.LogDebug("UpdateDeAuthorizedPOSChanges entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' Execute the stored procedure to update the item identifier record
            ' for the Identifier_ID, setting Add_Identifier = 0 (false)
            Dim cmdUpdatePriceBatchHeaders As SqlCommand = factory.GetDataCommand("Replenishment_POSPush_DeAuthorizeItem", Nothing, True) ' As New SqlCommand("Replenishment_POSPush_UpdateRefreshSent")
            Dim itemIdentifierParm As SqlParameter = cmdUpdatePriceBatchHeaders.Parameters.Add("@StoreItemAuthorizationIds", SqlDbType.Structured)
            Dim DeauthValue As SqlParameter = cmdUpdatePriceBatchHeaders.Parameters.Add("@POSDeAuth", SqlDbType.Bit)

            itemIdentifierParm.Value = StoreItemAuthorizationIds
            DeauthValue.Value = 0 'False
            cmdUpdatePriceBatchHeaders.ExecuteNonQuery()

            Logger.LogDebug("UpdateDeAuthorizedPOSChanges exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Applies the Vendor Id Add changes to the ItemCatalog database.
        ''' </summary>
        ''' <param name="POSResultSet"></param>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Sub PushPOSDataToDenormTable(ByVal POSResultSet As DataTable)
            Logger.LogDebug("PushPOSDataToDenormTable entry:", CLASSTYPE)

            Dim ConnectionString As String = Environment.GetCommandLineArgs.GetValue(2).ToString

            Dim DenormConnection As SqlConnection = New SqlConnection()
            DenormConnection.ConnectionString = ConnectionString

            Try
                DenormConnection.Open()
                Dim DenormCommand As New SqlCommand("Replenishment_POSPush_PopulatePriceBatchDenorm", DenormConnection)
                Dim DenormParam As SqlParameter = DenormCommand.Parameters.Add("@PriceBatchDetailData", SqlDbType.Structured)
                DenormCommand.CommandType = CommandType.StoredProcedure
                DenormParam.Value = POSResultSet

                Dim DenormAdapter As SqlDataAdapter = New SqlDataAdapter(DenormCommand)
                Dim FinalResult As New DataSet
                DenormAdapter.Fill(FinalResult, "FinalResult")

            Catch ex As Exception
                Logger.LogError("PushPOSDataToDenormTable Error: " & ex.Message, CLASSTYPE)
                Throw
            Finally
                DenormConnection.Close()
            End Try

            Logger.LogDebug("PushPOSDataToDenormTable exit", CLASSTYPE)
        End Sub

        Public Shared Sub PublishDenormTable()
            Logger.LogDebug("PublishDenormTable entry: ", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Try
                factory.ExecuteStoredProcedure("Replenishment_POSPush_PublishPriceBatchDenorm")
            Catch ex As Exception
                Logger.LogError("PublishDenormTable Error: " & ex.Message, CLASSTYPE)
                Throw
            End Try


            Logger.LogDebug("PublishDenormTable exit", CLASSTYPE)
        End Sub

        Public Shared Sub PopulateIconPOSPushStagingTable(ByVal IconPOSPushStaging As DataTable)
            Logger.LogDebug("PopulateIconDenormTable entry:", CLASSTYPE)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Dim StagingConnection As SqlConnection = New SqlConnection()
            StagingConnection.ConnectionString = factory.ConnectString

            Try
                StagingConnection.Open()
                Dim StagingCommand As New SqlCommand("Replenishment_POSPush_PopulateIconPOSPushStaging", StagingConnection)
                Dim StagingParam As SqlParameter = StagingCommand.Parameters.Add("@IconPOSPushStaging", SqlDbType.Structured)
                StagingCommand.CommandType = CommandType.StoredProcedure
                StagingParam.Value = IconPOSPushStaging

                Dim DenormAdapter As SqlDataAdapter = New SqlDataAdapter(StagingCommand)
                Dim FinalResult As New DataSet
                DenormAdapter.Fill(FinalResult, "FinalResult")

            Catch ex As Exception
                Logger.LogError("PopulateIconPOSPushStaging Error: " & ex.Message, CLASSTYPE)
                Throw
            Finally
                StagingConnection.Close()
            End Try

            Logger.LogDebug("PopulateIconPOSPushStaging exit", CLASSTYPE)
        End Sub

                ''' <summary>
        ''' Reads all of the Item Data Change records for the start date.
        ''' </summary>
        ''' <param name="dStart"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function PopulateIConPOSPushStagingWithBatchData(ByVal dStart As Date) As SqlDataReader
            Logger.LogDebug("PopulateIConPOSPushStagingWithBatchData entry", CLASSTYPE)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            Dim results As SqlDataReader = Nothing

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Date"
            currentParam.Value = dStart
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Deletes"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MaxBatchItems"
            currentParam.Value = ConfigurationServices.AppSettings("MaxPOSBatchRecords")
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IsScaleZoneData"
            currentParam.Value = 0
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure to read the price batch changes that are ready to be sent to the stores.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            results = factory.GetStoredProcedureDataReader("Replenishment_POSPush_PopulateBatchIconPOSPushStaging", paramList)

            Logger.LogDebug("PopulateIConPOSPushStagingWithBatchData exit", CLASSTYPE)

            Return results
        End Function

        Public Shared Sub PopulateIConPOSPushStagingWithNonBatchData()
            Logger.LogDebug("PopulateIConPOSPushStagingWithNonBatchData entry: ", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Try
                factory.ExecuteStoredProcedure("Replenishment_POSPush_PopulateNonBatchIconPOSPushStaging")
            Catch ex As Exception
                Logger.LogError("PopulateIConPOSPushStagingWithNonBatchData Error: " & ex.Message, CLASSTYPE)
                Throw
            End Try


            Logger.LogDebug("PopulateIConPOSPushStagingWithNonBatchData exit", CLASSTYPE)
        End Sub

        Public Shared Sub PopulateIConPOSPushPublish(ByRef ApplyChanges As Integer)
            Logger.LogDebug("PopulateIConPOSPushPublish entry: ", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "ApplyChanges"
            currentParam.Value = ApplyChanges
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)


            Try
                factory.ExecuteStoredProcedure("Replenishment_POSPush_PopulateIConPOSPushPublish", paramList)
            Catch ex As Exception
                Logger.LogError("PopulateIConPOSPushPublish Error: " & ex.Message, CLASSTYPE)
                Throw
            End Try


            Logger.LogDebug("PopulateIConPOSPushPublish exit", CLASSTYPE)
        End Sub

        Public Shared Sub DeleteIConPOSPushStaging()
            Logger.LogDebug("DeleteIConPOSPushStaging entry: ", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Try
                factory.ExecuteNonQuery("DELETE FROM IConPOSPushStaging")
            Catch ex As Exception
                Logger.LogError("DeleteIConPOSPushStaging Error: " & ex.Message, CLASSTYPE)
                Throw
            End Try


            Logger.LogDebug("DeleteIConPOSPushStaging exit", CLASSTYPE)
        End Sub

        Public Shared Sub BuildStorePosFileForR10(ByRef StoreNumber As Integer)
            Logger.LogDebug("BuildStorePosFileForR10 entry: ", CLASSTYPE)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            currentParam = New DBParam
            currentParam.Name = "StoreNumber"
            currentParam.Value = StoreNumber
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Try
                factory.ExecuteStoredProcedure("BuildStorePosFileForR10", paramList)
            Catch ex As Exception
                Logger.LogError("BuildStorePosFileForR10 Error: " & ex.Message, CLASSTYPE)
                Throw
            End Try

            Logger.LogDebug("BuildStorePosFileForR10 exit:", CLASSTYPE)
        End Sub

        Public Shared Function GetPOSFileWriterClass() As DataTable
            Logger.LogDebug("GetPOSFileWriteClass entry: ", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Try

                ' Execute the stored procedure 
                Return factory.GetStoredProcedureDataTable("Replenishment_POSPush_GetPOSFileWriterClass")

            Catch ex As Exception
                Throw
            End Try

        End Function

#End Region

    End Class
End Namespace
