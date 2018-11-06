Imports log4net
Imports WholeFoods.IRMA.Mammoth.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Mammoth.DataAccess
    Public Class MammothEventDAO

        Private Const ItemLocaleAddOrUpdateEventType As String = "ItemLocaleAddOrUpdate"
        Private Const ItemDeleteEventType As String = "ItemDelete"

        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Shared Sub CreateItemLocaleAddOrUpdateEvent(ByVal mammothEventBo As MammothEventBO)
            logger.Debug("UpdateItemStoreData Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = mammothEventBo.ItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = mammothEventBo.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "EventType"
            currentParam.Value = ItemLocaleAddOrUpdateEventType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("mammoth.InsertItemLocaleChangeQueue", paramList)

            logger.Debug("UpdateItemStoreData Exit")
        End Sub

        Public Shared Sub CreateItemDeleteEvent(ByVal mammothEventBo As MammothEventBO)
            logger.Debug("UpdateItemStoreData Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = mammothEventBo.ItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = mammothEventBo.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "EventType"
            currentParam.Value = ItemDeleteEventType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("mammoth.InsertItemLocaleChangeQueue", paramList)

            logger.Debug("UpdateItemStoreData Exit")
        End Sub
    End Class
End Namespace
