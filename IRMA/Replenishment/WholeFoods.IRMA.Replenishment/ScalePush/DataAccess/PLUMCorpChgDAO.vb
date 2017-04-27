Imports System.Configuration
Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.ScalePush.ScaleException
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.ScalePush.DataAccess

    Public Class PLUMCorpChgDAO

        ' Set the class type for logging
        Private Shared CLASSTYPE As Type = System.Type.GetType("WholeFoods.IRMA.Replenishment.ScalePush.DataAccess.PLUMCorpChgDAO")

        ''' <summary>
        ''' Reads all of the Item Id Delete records for the start date.
        ''' </summary>
        ''' <param name="dStart"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetCorporateItemIdDeletes(ByVal dStart As Date) As SqlDataReader
            Logger.LogDebug("GetCorporateItemIdDeletes entry", CLASSTYPE)
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
            currentParam.Name = "IsScaleZoneData"
            currentParam.Value = 1
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure to read the item id deletes 
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            results = factory.GetStoredProcedureDataReader("Replenishment_ScalePush_GetIdentifierDeletes", paramList)

            Logger.LogDebug("GetCorporateItemIdDeletes exit", CLASSTYPE)
            Return results
        End Function

        ''' <summary>
        ''' Reads all of the corporate data changes for the given change type action code.
        ''' Action Codes: A=ItemIdAdd; D=ItemIdDelete; C=ItemDataChange; F=FullScaleLoad
        ''' </summary>
        ''' <param name="actionCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <exception cref="DataFactoryException" />
        Public Shared Function GetCorporateDataChanges(ByVal actionCode As Char, ByVal dStart As Date, ByVal storeNo As String) As SqlDataReader
            Logger.LogDebug("GetCorporateDataChanges entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            Dim results As SqlDataReader = Nothing

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "ActionCode"
            currentParam.Value = actionCode
            currentParam.Type = DBParamType.Char
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Date"
            currentParam.Value = dStart
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            If storeNo Is Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = storeNo
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to read the price batch changes that are ready to be sent to the stores.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            results = factory.GetStoredProcedureDataReader("GetPLUMCorpChg", paramList)

            Logger.LogDebug("GetCorporateDataChanges exit", CLASSTYPE)
            Return results
        End Function

        ''' <summary>
        ''' Clear all of the records from the PLUMCorpChgQueueTmp that were successfully processed by this run of
        ''' scale push.
        ''' The storeList is a pipe delimited (|) list of the stores that successfully received the file via FTP.
        ''' </summary>
        ''' <param name="storeList"></param>
        ''' <remarks></remarks>
        Public Shared Sub DeleteTempCorporateDataChanges(ByVal storeList As String)
            Logger.LogDebug("DeletePLUMCorpChgQueueTmp entry", CLASSTYPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "StoreList"
            currentParam.Value = storeList
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StoreListSeparator"
            currentParam.Value = "|"
            currentParam.Type = DBParamType.Char
            paramList.Add(currentParam)

            ' Execute the stored procedure to delete all data from PLUMCorpChgQueueTmp.
            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("DBPushTimeout"))
            factory.ExecuteStoredProcedure("DeletePLUMCorpChgQueueTmp", paramList)

            Logger.LogDebug("DeletePLUMCorpChgQueueTmp exit", CLASSTYPE)
        End Sub

    End Class

End Namespace
