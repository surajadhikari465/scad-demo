Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.TagPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.TagPush.Controller
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.TagPush.DataAccess
    Public Class TagWriterDAO
        ' Set the class type for logging
        Private Shared CLASSTYPE As Type = System.Type.GetType("WholeFoods.IRMA.Replenishment.TagPush.DataAccess.TagWriterDAO")

#Region "Read methods"
        ''' <summary>
        ''' Reads all of the POSWriterFileConfig values for the POS File Writer.
        ''' </summary>
        ''' <param name="fileWriterKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetTagWriterFileConfigData(ByVal fileWriterKey As Integer) As SqlDataReader
            Logger.LogDebug("GetTagWriterFileConfigData entry: fileWriterKey=" + fileWriterKey.ToString, CLASSTYPE)
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

            Logger.LogDebug("GetTagWriterFileConfigData exit", CLASSTYPE)
            Return results
        End Function

        ''' <summary>
        ''' Reads all of the data dictionary values for the POS File Writer.
        ''' </summary>
        ''' <param name="fileWriterKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetTagWriterDictionary(ByVal fileWriterKey As Integer) As ArrayList
            Logger.LogDebug("GetTagWriterDictionary entry: fileWriterKey=" + fileWriterKey.ToString, CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As ArrayList
            Dim currentElement As TagWriterDictionaryBO
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
                results = factory.GetStoredProcedureDataReader("TagPush_GetTagWriterDictionary", paramList)

                ' Parse the results into a collection of POSWriterDictionaryBO objects
                While (results.Read())
                    currentElement = New TagWriterDictionaryBO
                    currentElement.PopulateFromTagWriterDictionary(results)
                    dictionaryCollection.Add(currentElement)
                End While
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try

            Logger.LogDebug("GetTagWriterDictionary exit", CLASSTYPE)
            Return dictionaryCollection
        End Function

        ''' <summary>
        ''' Reads all of the Item Data Change records for the start date.
        ''' </summary>
        ''' <param name="batchId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetPriceBatchTagData(ByVal itemList As String, ByVal itemListSeparator As Char, ByVal batchId As Integer, ByVal startLabelPosition As Integer, ByVal PosFileWriterKey As Integer) As SqlDataReader
            Logger.LogDebug("GetPriceBatchTagData entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            Dim results As SqlDataReader = Nothing

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "ItemList"
            currentParam.Value = itemList
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ItemListSeparator"
            currentParam.Value = itemListSeparator
            currentParam.Type = DBParamType.Char
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PriceBatchHeaderID"
            currentParam.Value = batchId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StartLabelPosition"
            currentParam.Value = startLabelPosition
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PosFileWriterKey"
            currentParam.Value = PosFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to read the price batch changes that are ready to be sent to the stores.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            'results = factory.GetStoredProcedureDataReader("TagPush_GetBatchTagData", paramList)
            results = factory.GetStoredProcedureDataReader("Replenishment_TagPush_GetBatchTagFile", paramList)

            Logger.LogDebug("GetPriceBatchTagData exit", CLASSTYPE)
            Return results
        End Function

        Public Shared Function GetElectronicShelfTagBatchData() As SqlDataReader
            Logger.LogDebug("GetElectronicShelfTagBatchData entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing

            ' Execute the stored procedure to read the price batch changes that are ready to be sent to the stores.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            'results = factory.GetStoredProcedureDataReader("TagPush_GetBatchTagData", paramList)
            results = factory.GetStoredProcedureDataReader("Replenishment_TagPush_GetElectronicShelfTagBatchFile")

            Logger.LogDebug("GetElectronicShelfTagBatchData exit", CLASSTYPE)
            Return results
        End Function

        Public Shared Function GetFullElectronicShelfTagFile(iStoreNo As Integer) As SqlDataReader
            Logger.LogDebug("GetFullElectronicShelfTagFile entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "StoreNo"
            currentParam.Value = iStoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to read the price batch changes that are ready to be sent to the stores.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            'results = factory.GetStoredProcedureDataReader("TagPush_GetBatchTagData", paramList)
            results = factory.GetStoredProcedureDataReader("Replenishment_TagPush_GetFullElectronicShelfTagFile", paramList)

            Logger.LogDebug("GetFullElectronicShelfTagFile exit", CLASSTYPE)
            Return results
        End Function

        ''' <summary>
        ''' Reads all of the Item Data Change records for the start date.
        ''' </summary>
        ''' <param name="itemList"></param>
        ''' <param name="itemListSeparator"></param>
        ''' <param name="startLabelPosition"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetReprintTagData(ByVal itemList As String, ByVal store_No As Integer, ByVal itemListSeparator As Char, ByVal startLabelPosition As Integer, ByVal PosFileWriterKey As Integer, ByVal blnSortTags As Boolean) As SqlDataReader
            Logger.LogDebug("GetReprintTagData entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            Dim results As SqlDataReader = Nothing

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "ItemList"
            currentParam.Value = itemList
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = store_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ItemListSeparator"
            currentParam.Value = itemListSeparator
            currentParam.Type = DBParamType.Char
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StartLabelPosition"
            currentParam.Value = startLabelPosition
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PosFileWriterKey"
            currentParam.Value = PosFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SortTags"
            currentParam.Value = CInt(blnSortTags)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure to read the price batch changes that are ready to be sent to the stores.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            results = factory.GetStoredProcedureDataReader("Replenishment_TagPush_GetReprintTagFile", paramList)

            Logger.LogDebug("GetReprintTagData exit", CLASSTYPE)
            Return results
        End Function

        Public Shared Function GetSetRegTagData(ByVal itemList As String, ByVal itemListSeparator As Char, _
                                ByVal store_No As Integer, ByVal isRegular As Boolean, ByVal startDate As Date, ByVal PosFileWriterKey As Integer) As SqlDataReader
            Logger.LogDebug("GetSetRegTagData entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            Dim results As SqlDataReader = Nothing

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "ItemList"
            currentParam.Value = itemList
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ItemListSeparator"
            currentParam.Value = itemListSeparator
            currentParam.Type = DBParamType.Char
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = store_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            'DaveStacey 20070717 - rolling in shelftag fixes from 2.4.0 launch - this one was problematic 
            'due to the fact that the date picked on the form wasn't actually sent to the query 
            ' so i'm stringing the date value through here
            currentParam = New DBParam
            currentParam.Name = "startDate"
            currentParam.Value = startDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PosFileWriterKey"
            currentParam.Value = PosFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to read the price batch changes that are ready to be sent to the stores.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            If isRegular Then
                results = factory.GetStoredProcedureDataReader("Planogram_GetSetRegTagFile", paramList)
            Else
                results = factory.GetStoredProcedureDataReader("Planogram_GetNonRegTagFile", paramList)
            End If

            Logger.LogDebug("GetSetRegTagData exit", CLASSTYPE)
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
            results = factory.GetStoredProcedureDataReader("TagPush_GetPriceBatchOffers", paramList)

            Logger.LogDebug("GetPromoOfferDataChanges exit", CLASSTYPE)
            Return results
        End Function

        ''' <summary>
        ''' returns hashtable of escape characters and their replacement chars for a given POSFileWriterKey
        ''' </summary>
        ''' <param name="fileWriterKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetTagWriterEscapeChars(ByVal fileWriterKey As Integer) As Hashtable
            Logger.LogDebug("GetTagWriterEscapeChars entry: fileWriterKey=" + fileWriterKey.ToString, CLASSTYPE)
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
                results = factory.GetStoredProcedureDataReader("TagPush_GetTagWriterEscapeChars", paramList)

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
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Logger.LogDebug("GetTagWriterEscapeChars exit", CLASSTYPE)
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
        Public Shared Function GetDefaultTagWriterBatchId(ByVal posFileWriter As Integer, ByVal posChangeType As Integer) As String
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
                Throw ex
            Finally
                If reader IsNot Nothing Then
                    reader.Close()
                End If
            End Try

            Return defaultBatchId
        End Function
        ' Function to return a ShelfTag Writer Type for the stroe number passed in
        Public Shared Function GetShelfTagWriterType(ByVal Store_no As String) As String
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim sql As StringBuilder
            Dim writerType As String
            Dim flagKey As String
            Dim activateFileTagWriter As Boolean
            Dim sqlStr As String

            flagKey = "ActivateFileTagWriter"
            sql = New StringBuilder
            sql.Append("SELECT flagValue from InstanceDataFlags where FlagKey='")
            sql.Append(flagKey)
            sql.Append("'")
            sqlStr = sql.ToString
            activateFileTagWriter = CBool(factory.ExecuteScalar(sql.ToString))
            If (activateFileTagWriter) Then
                writerType = "FILE"
            Else
                writerType = "REPORTS"
            End If

            Return writerType
        End Function

#End Region

#Region "Update methods"
        ''' <summary>
        ''' Updates the Item data contained in Promo Off Cost price batch detail records with the current
        ''' Item data.  
        ''' </summary>
        ''' <param name="itemTagBOs"></param>
        ''' <remarks></remarks>
        Public Shared Sub UpdatePriceBatchDetailWithTagID(ByRef itemTagBOs As Hashtable)
            Logger.LogDebug("UpdatePriceBatchDetailWithTagID entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            Dim itemTagBO As ShelfTagBO
            Dim iter As IDictionaryEnumerator

            iter = itemTagBOs.GetEnumerator
            While (iter.MoveNext)
                If (paramList.Count > 0) Then
                    paramList.Clear()
                End If
                itemTagBO = CType(iter.Value, ShelfTagBO)
                'setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "itemKey"
                currentParam.Value = itemTagBO.ItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "tagID"
                currentParam.Value = itemTagBO.TagID2
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "tagID2"
                currentParam.Value = itemTagBO.TagID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "pbdID"
                currentParam.Value = itemTagBO.pbdID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure to update the Promo Off Cost price batch detail records with
                ' the latest Item data.
                factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
                factory.ExecuteStoredProcedure("Replenishment_TagPush_UpdatePBDWithTagID", paramList)
            End While

            Logger.LogDebug("UpdatePriceBatchDetailWithTagID exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Applies the Item Id Add changes to the ItemCatalog database.
        ''' </summary>
        ''' <param name="itemIdentifier"></param>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Sub ApplyItemIdAdds(ByVal itemIdentifier As Integer)

        End Sub

        ''' <summary>
        ''' Applies the Item Id Delete changes to the ItemCatalog database.
        ''' </summary>
        ''' <param name="itemIdentifier"></param>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Sub ApplyItemIdDeletes(ByVal itemIdentifier As Integer)
 
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

        End Sub
#End Region

    End Class
End Namespace
