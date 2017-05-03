Imports log4net
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic

Namespace WholeFoods.IRMA.ItemHosting.DataAccess

    ''' <summary>
    ''' This data-access class does not need to be used directly by non-Shipper classes/objects.
    ''' The Shipper object handles the business logic and data-access calls.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ShipperDAO

        Public Const SHIPPER_UOM_NAME = "Shipper"
        Public Const SHIPPER_UOM_SYS_CODE = "shp"

#Region "Private Members"

        ''' <summary>
        ''' Log4Net logger for this class.
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#End Region

#Region "Public Shared Read Methods"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="shipperKey">The shipper key is the item key for the Shipper.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetShipperInfo(ByVal shipperKey As Integer) As SqlDataReader
            Dim shipperInfoReader As SqlDataReader
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Shipper_Key"
            currentParam.Value = shipperKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            shipperInfoReader = factory.GetStoredProcedureDataReader("ShipperGetInfo", paramList)

            logger.Debug("[GetShipperInfo][shipperKey=" & shipperKey & "] Shipper info reader has rows?: " & shipperInfoReader.HasRows)

            Return shipperInfoReader
        End Function

        ''' <summary>
        ''' Gets shipper-key and qty for all shippers that contain the specified item.
        ''' </summary>
        ''' <param name="itemKey">Item key for an existing item.</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Used to help validate that an item can be marked as a shipper.  If this method returns one or more Shippers,
        ''' that means the item exists within another Shipper, so it should not be able to be marked as a Shipper.
        ''' </remarks>
        Public Shared Function GetShippersContainingItem(ByVal itemKey As Integer) As SqlDataReader
            Dim shipperInfoReader As SqlDataReader
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = itemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            shipperInfoReader = factory.GetStoredProcedureDataReader("ShipperGetAllContainingItem", paramList)

            logger.Debug("[GetShippersContainingItem][itemKey=" & itemKey & "] Shippers containing item has rows?: " & shipperInfoReader.HasRows)

            Return shipperInfoReader
        End Function

        ''' <summary>
        ''' Returns TRUE if the specified items exists in one or more Shippers.
        ''' </summary>
        ''' <param name="itemKey">Item key for an existing item.</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' .
        ''' </remarks>
        Public Shared Function ItemExistsInAShipper(ByVal itemKey As Integer) As Boolean
            Dim shipperInfoReader As SqlDataReader
            Dim exists As Boolean
            shipperInfoReader = GetShippersContainingItem(itemKey)
            exists = shipperInfoReader.RecordsAffected > 0
            Try
                shipperInfoReader.Close()
            Catch ex As Exception
                logger.Error(String.Format("Error closing reader during ItemExistsInAShipper check, itemKey={0}.", itemKey), ex)
            End Try
            logger.DebugFormat("Item key {0} exists in a Shipper: {1}", itemKey, CStr(exists))
            Return exists
        End Function

        Public Shared Function IsItemAShipper(ByVal itemKey As Integer) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim outputList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = itemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Output param.
            currentParam = New DBParam
            currentParam.Name = "IsShipper"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            outputList = factory.ExecuteStoredProcedure("ShipperCheck", paramList)

            Dim out1 As Boolean
            If outputList IsNot Nothing Then
                If outputList.Count > 0 Then
                    out1 = CBool(outputList.Item(0))
                End If
            End If
            logger.DebugFormat("[IsItemAShipper] output size = {2}, Item key {0} is a Shipper?: {1}", itemKey, out1 = True, outputList.Count)

            Return CBool(outputList.Item(0)) = True
        End Function

#End Region

#Region "Public Shared Write Methods"

        ''' <summary>
        ''' Inserts an item into a Shipper.
        ''' </summary>
        ''' <param name="shipperKey">Parent Shipper item key.</param>
        ''' <param name="newItemKey">Item key of item being added to the Shipper.</param>
        ''' <remarks></remarks>
        Public Shared Function AddItemToShipper(ByVal shipperKey As Integer, ByVal newItemKey As Integer, ByVal initQty As Integer) As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim outputList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Shipper_Key"
            currentParam.Value = shipperKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = newItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Qty"
            currentParam.Value = initQty
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Output param.
            currentParam = New DBParam
            currentParam.Name = "Identifier"
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Output param.
            currentParam = New DBParam
            currentParam.Name = "Desc"
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            outputList = factory.ExecuteStoredProcedure("ShipperInsertItem", paramList)

            Dim out1 As String = ""
            Dim out2 As String = ""
            If outputList IsNot Nothing Then
                If outputList.Count > 0 Then
                    out1 = CStr(outputList.Item(0))
                End If
                If outputList.Count > 1 Then
                    out2 = CStr(outputList.Item(1))
                End If
            End If
            logger.DebugFormat("[AddItemToShipper, SP=ShipperInsertItem] output size = {2}, output items: identifier={0}, desc={1}", out1, out2, outputList.Count)

            Return outputList
        End Function

        ''' <summary>
        ''' Removes an item from a Shipper.
        ''' </summary>
        ''' <param name="shipperKey">Shipper key for the Shipper from which item is to be removed.</param>
        ''' <param name="deleteItemKey">Item key for the item to be removed.</param>
        ''' <remarks>Exceptions should be handled by caller.</remarks>
        Public Shared Sub DeleteItemFromShipper(ByVal shipperKey As Integer, ByVal deleteItemKey As Integer)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Shipper_Key"
            currentParam.Value = shipperKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = deleteItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("ShipperDeleteItem", paramList)

            logger.DebugFormat("[DeleteItemFromShipper, SP=ShipperDeleteItem] Item deleted: ShipperKey={0}, ItemKey={1}", shipperKey, deleteItemKey)
        End Sub

        ''' <summary>
        ''' Updates the pack qty of a Shipper based on the items it contains.
        ''' </summary>
        ''' <param name="itemKey">Item key of the Shipper item.</param>
        ''' <remarks></remarks>
        Public Shared Sub UpdatePackInfo(ByVal itemKey As Integer)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = itemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            Try
                factory.ExecuteStoredProcedure("dbo.ShipperUpdatePackInfo", paramList)
            Catch ex As Exception
                logger.Error(String.Format(ShipperMessages.ERROR_SHIPPER_COULD_NOT_UPDATE_PACK_INFO & "  [ItemKey={0}, ]", itemKey), ex)
                Throw New Exception(ShipperMessages.ERROR_SHIPPER_COULD_NOT_UPDATE_PACK_INFO, ex)
            End Try
        End Sub

        ''' <summary>
        ''' Updates the unit qty of an item in a Shipper.
        ''' </summary>
        ''' <param name="shipperKey">Item key for the parent Shipper.</param>
        ''' <param name="itemKey">Item key for the item to be updated.</param>
        ''' <param name="newQty">New quantity.</param>
        ''' <remarks>Exceptions will be thrown up to caller and should be handled appropriately.</remarks>
        Public Shared Sub UpdateShipperItemQty(ByVal shipperKey As Integer, ByVal itemKey As Integer, ByVal newQty As Decimal)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Shipper_Key"
            currentParam.Value = shipperKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = itemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Qty"
            currentParam.Value = newQty
            currentParam.Type = DBParamType.Decimal
            paramList.Add(currentParam)

            Try
                factory.ExecuteStoredProcedure("dbo.ShipperItemUpdateInfo", paramList)
            Catch ex As Exception
                logger.Error(String.Format(ShipperMessages.ERROR_SHIPPERITEM_DURING_UPDATE_INFO & "  [ShipperKey={0}, ItemKey={1}, NewQty={2}]", shipperKey, itemKey, newQty), ex)
                Throw New Exception(ShipperMessages.ERROR_SHIPPERITEM_DURING_UPDATE_INFO, ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace
