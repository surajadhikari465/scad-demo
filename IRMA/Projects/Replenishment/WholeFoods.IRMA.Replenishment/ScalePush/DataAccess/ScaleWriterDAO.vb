Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.Controller
Imports WholeFoods.IRMA.Replenishment.POSPush.POSException
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.ScalePush.DataAccess

    Public Class ScaleWriterDAO

        ' Set the class type for logging
        Private Shared CLASSTYPE As Type = System.Type.GetType("WholeFoods.IRMA.Replenishment.ScalePush.DataAccess.ScaleWriterDAO")

        ''' <summary>
        ''' Reads all of the Item Data Change records for the start date.
        ''' </summary>
        ''' <param name="dStart"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetItemDataChanges(ByVal dStart As Date) As SqlDataReader
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
            currentParam.Value = 1 'LIMIT OUTPUT TO ONLY PRICE CHANGES & SCALE ITEMS
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure to read the price batch changes that are ready to be sent to the stores.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            results = factory.GetStoredProcedureDataReader("Replenishment_ScalePush_GetPriceBatchSent", paramList)

            Logger.LogDebug("GetItemDataChanges exit", CLASSTYPE)
            Return results
        End Function

        ''' <summary>
        ''' Reads all of the SmartX Price Change records for the start date.
        ''' </summary>
        ''' <param name="dStart"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetSmartXPriceChanges(ByVal dStart As Date) As SqlDataReader
            Logger.LogDebug("GetSmartXPriceChanges entry", CLASSTYPE)
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
            currentParam.Value = 1 'LIMIT OUTPUT TO ONLY PRICE CHANGES & SCALE ITEMS
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure to read the price batch changes that are ready to be sent to the stores.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            results = factory.GetStoredProcedureDataReader("Replenishment_ScalePush_GetSmartXPrices", paramList)

            Logger.LogDebug("GetSmartXPriceChanges exit", CLASSTYPE)
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
            currentParam.Value = 1
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure to read the price batch deletes that are ready to be sent to the stores.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            results = factory.GetStoredProcedureDataReader("Replenishment_ScalePush_GetPriceBatchSent", paramList)

            Logger.LogDebug("GetItemDeletes exit", CLASSTYPE)
            Return results
        End Function

        ''' <summary>
        ''' Reads all of the NutriFact changes, ordering the results by the action code.
        ''' Action Codes: A=Add; D=Delete; C=Change
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetNutriFactChanges() As SqlDataReader
            Logger.LogDebug("GetNutriFactChanges entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing

            ' Execute the stored procedure to read the changes.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            results = factory.GetStoredProcedureDataReader("Replenishment_ScalePush_GetNutriFactChanges")

            Logger.LogDebug("GetNutriFactChanges exit", CLASSTYPE)
            Return results
        End Function

        ''' <summary>
        ''' Reads all of the ExtraText changes, ordering the results by the action code.
        ''' Action Codes: A=Add; D=Delete; C=Change
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetExtraTextChanges() As SqlDataReader
            Logger.LogDebug("GetExtraTextChanges entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing

            ' Execute the stored procedure to read the changes.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            results = factory.GetStoredProcedureDataReader("Replenishment_ScalePush_GetExtraTextChanges")

            Logger.LogDebug("GetExtraTextChanges exit", CLASSTYPE)
            Return results
        End Function

        ''' <summary>
        ''' Clear all of the NutriFact change records that are in the Temp queue.  This should happen
        ''' after the changes have been successfully communicated to all scale systems.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub DeleteTempNutriFactChanges()
            Logger.LogDebug("DeleteTempNutriFactChanges entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' Execute the stored procedure to delete all data from NutriFactsChgQueueTmp.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            factory.ExecuteStoredProcedure("Replenishment_ScalePush_DeleteNutriFactsChgQueueTmp")

            Logger.LogDebug("DeleteTempNutriFactChanges exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Clear all of the ExtraText change records that are in the Temp queue.  This should happen
        ''' after the changes have been successfully communicated to all scale systems.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub DeleteTempExtraTextChanges()
            Logger.LogDebug("DeleteTempExtraTextChanges entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' Execute the stored procedure to delete all data from Scale_ExtraTextChgQueueTmp.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            factory.ExecuteStoredProcedure("Replenishment_ScalePush_DeleteExtraTextChgQueueTmp")

            Logger.LogDebug("DeleteTempExtraTextChanges exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Applies the Scale Authorization changes to the ItemCatalog database.
        ''' </summary>
        ''' <param name="storeItemAuthId"></param>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Sub UpdateAuthorizedScaleChanges(ByVal storeItemAuthId As Integer)
            Logger.LogDebug("UpdateAuthorizedScaleChanges entry: storeItemAuthId=" + storeItemAuthId.ToString, CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "StoreItemAuthorizationId"
            currentParam.Value = storeItemAuthId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to update the item identifier record
            ' for the Identifier_ID, setting Add_Identifier = 0 (false)
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            factory.ExecuteStoredProcedure("Replenishment_ScalePush_AuthorizeItem", paramList)

            Logger.LogDebug("UpdateAuthorizedScaleChanges exit", CLASSTYPE)
        End Sub

        ''' <summary>
        ''' Applies the Scale DeAuthorization changes to the ItemCatalog database.
        ''' </summary>
        ''' <param name="storeItemAuthId"></param>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Sub UpdateDeAuthorizedScaleChanges(ByVal storeItemAuthId As Integer)
            Logger.LogDebug("UpdateDeAuthorizedScaleChanges entry: storeItemAuthId=" + storeItemAuthId.ToString, CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "StoreItemAuthorizationId"
            currentParam.Value = storeItemAuthId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to update the item identifier record
            ' for the Identifier_ID, setting Add_Identifier = 0 (false)
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            factory.ExecuteStoredProcedure("Replenishment_ScalePush_DeAuthorizeItem", paramList)

            Logger.LogDebug("UpdateDeAuthorizedScaleChanges exit", CLASSTYPE)
        End Sub


    End Class

End Namespace