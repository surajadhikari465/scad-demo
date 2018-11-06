Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient
Imports log4net

Namespace WholeFoods.IRMA.ItemHosting.DataAccess
    Public Class ItemDAO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


#Region "read methods"

        ''' <summary>
        ''' Get the Item.
        ''' </summary>
        ''' <exception cref="DataFactoryException" />
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItem() As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("GetItem")
        End Function

        Public Function LockItem(ByVal itemID As Integer, ByVal userID As Integer, Optional ByRef transaction As SqlTransaction = Nothing) As DataSet

            logger.Debug("LockItem Entry with itemID = " + itemID.ToString + " userID=" + userID.ToString)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim success As Boolean = True
            Dim ds As DataSet = Nothing


            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = itemID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = userID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute Stored Procedure to Create/Maintain Price Batch Details for the offer
                ds = factory.GetStoredProcedureDataSet("LockItem", paramList, transaction)

            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "ItemDAO:LockItem")
                logger.Error(ex.Message)
                ds = Nothing
            End Try

            logger.Debug("LockItem Exit")
            Return ds
        End Function


        Public Function UnlockItem(ByVal itemID As Integer, Optional ByRef transaction As SqlTransaction = Nothing) As Boolean

            logger.Debug("UnlockItem Entry with itemID = " + itemID.ToString)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim success As Boolean = True

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = itemID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute Stored Procedure to Create/Maintain Price Batch Details for the offer
                factory.ExecuteStoredProcedure("UnlockItem", paramList, transaction)

            Catch ex As Exception
                success = False
                MsgBox(ex.Message, MsgBoxStyle.Critical, "ItemDAO:UnlockItem")
            End Try

            logger.Debug("UnlockItem Exit")

            Return success

        End Function

        ''' <summary>
        ''' This method updates the ItemStoreBO with the current data from the Price table.
        ''' 
        ''' </summary>
        ''' <param name="itemStore"></param>
        ''' <remarks></remarks>
        Public Shared Sub GetItemStoreData(ByRef itemStore As ItemStoreBO)
            logger.Debug("GetItemStoreData Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = itemStore.ItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = itemStore.StoreId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute Stored Procedure to get the data
                results = factory.GetStoredProcedureDataReader("GetItemDataInventory", paramList)

                ' Update the itemStore object with the current data
                While results.Read
                    If (Not results.IsDBNull(results.GetOrdinal("CompFlag"))) Then
                        itemStore.CompetitiveItem = results.GetBoolean(results.GetOrdinal("CompFlag"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("GrillPrint"))) Then
                        itemStore.GrillPrint = results.GetBoolean(results.GetOrdinal("GrillPrint"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("IBM_Discount"))) Then
                        itemStore.LineDiscount = results.GetBoolean(results.GetOrdinal("IBM_Discount"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Restricted_Hours"))) Then
                        itemStore.RestrictedHours = results.GetBoolean(results.GetOrdinal("Restricted_Hours"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("SrCitizenDiscount"))) Then
                        itemStore.SrCitizenDiscount = results.GetBoolean(results.GetOrdinal("SrCitizenDiscount"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("NotAuthorizedForSale"))) Then
                        itemStore.StopSale = results.GetBoolean(results.GetOrdinal("NotAuthorizedForSale"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("VisualVerify"))) Then
                        itemStore.VisualVerify = results.GetBoolean(results.GetOrdinal("VisualVerify"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("AgeCode"))) Then
                        itemStore.AgeCode = results.GetInt32(results.GetOrdinal("AgeCode")).ToString()
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("PosTare"))) Then
                        itemStore.POS_Tare = results.GetInt32(results.GetOrdinal("PosTare")).ToString()
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("POSLinkCode"))) Then
                        itemStore.POSLinkCode = results.GetString(results.GetOrdinal("POSLinkCode"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("LinkedItem"))) Then
                        itemStore.LinkedItemKey = results.GetInt32(results.GetOrdinal("LinkedItem"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("LinkedItemIdentifier"))) Then
                        itemStore.LinkedIdentifier = results.GetString(results.GetOrdinal("LinkedItemIdentifier"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("storeSubTeam_No"))) Then
                        itemStore.StoreSubTeam = results.GetInt32(results.GetOrdinal("storeSubTeam_No"))
                    Else
                        itemStore.StoreSubTeam = -1
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("itemSubteam_No"))) Then
                        itemStore.ItemSubTeam = results.GetInt32(results.GetOrdinal("itemSubteam_No"))
                    Else
                        itemStore.ItemSubTeam = -1
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("KitchenRoute_ID"))) Then
                        itemStore.KitchenRouteID = results.GetInt32(results.GetOrdinal("KitchenRoute_ID"))
                    Else
                        itemStore.KitchenRouteID = -1
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Routing_Priority"))) Then
                        itemStore.RoutingPriority = results.GetByte(results.GetOrdinal("Routing_Priority"))
                    Else
                        itemStore.RoutingPriority = 0
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Consolidate_Price_To_Prev_Item"))) Then
                        itemStore.ConsolidatePrice = results.GetBoolean(results.GetOrdinal("Consolidate_Price_To_Prev_Item"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Print_Condiment_On_Receipt"))) Then
                        itemStore.PrintCondimentOnReceipt = results.GetBoolean(results.GetOrdinal("Print_Condiment_On_Receipt"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Age_Restrict"))) Then
                        itemStore.AgeRestrict = results.GetBoolean(results.GetOrdinal("Age_Restrict"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Authorized"))) Then
                        itemStore.Authorized = results.GetBoolean(results.GetOrdinal("Authorized"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("MixMatch"))) Then
                        itemStore.MixMatch = results.GetInt32(results.GetOrdinal("MixMatch"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Discountable"))) Then
                        itemStore.Discountable = results.GetBoolean(results.GetOrdinal("Discountable"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Refresh"))) Then
                        itemStore.RefreshPOSInfo = results.GetBoolean(results.GetOrdinal("Refresh"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("LocalItem"))) Then
                        itemStore.LocalItem = results.GetBoolean(results.GetOrdinal("LocalItem"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("LastScannedUserName_DTS"))) Then
                        itemStore.LastScannedUserName_DTS = results.GetString(results.GetOrdinal("LastScannedUserName_DTS"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("LastScannedUserName_NonDTS"))) Then
                        itemStore.LastScannedUserName_NonDTS = results.GetString(results.GetOrdinal("LastScannedUserName_NonDTS"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("LastScannedDate_DTS"))) Then
                        itemStore.LastScannedDate_DTS = results.GetDateTime(results.GetOrdinal("LastScannedDate_DTS")).ToString
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("LastScannedDate_NonDTS"))) Then
                        itemStore.LastScannedDate_NonDTS = results.GetDateTime(results.GetOrdinal("LastScannedDate_NonDTS")).ToString
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("ItemSurcharge"))) Then
                        itemStore.ItemSurcharge = results.GetInt32(results.GetOrdinal("ItemSurcharge"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("ElectronicShelfTag"))) Then
                        itemStore.ElectronicShelfTag = results.GetBoolean(results.GetOrdinal("ElectronicShelfTag"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Discontinue"))) Then
                        itemStore.Discontinue = results.GetBoolean(results.GetOrdinal("Discontinue"))
                    Else
                        itemStore.Discontinue = 0
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("ECommerce"))) Then
                        itemStore.ECommerce = results.GetBoolean(results.GetOrdinal("ECommerce"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Retail_Unit_ID"))) Then
                        itemStore.RetailUomId = results.GetInt32(results.GetOrdinal("Retail_Unit_ID"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_ScaleUomUnit_ID"))) Then
                        itemStore.ScaleUomId = results.GetInt32(results.GetOrdinal("Scale_ScaleUomUnit_ID"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_FixedWeight"))) Then
                        itemStore.FixedWeight = results.GetString(results.GetOrdinal("Scale_FixedWeight"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Scale_ByCount"))) Then
                        itemStore.ByCount = results.GetInt32(results.GetOrdinal("Scale_ByCount"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("ItemStatusCode"))) Then
                        itemStore.ItemStatusCode = results.GetInt32(results.GetOrdinal("ItemStatusCode"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("OrderedByInfor"))) Then
                        itemStore.OrderedByInfor = results.GetBoolean(results.GetOrdinal("OrderedByInfor"))
                    End If
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If

                logger.Debug("GetItemStoreData Exit")
            End Try
        End Sub
        ''' <summary>
        ''' Checks to see if a costed by weight item is sold as each in Retail
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function IfSoldAsEachInRetail(ByVal identifier As String) As Boolean

            logger.Debug("IfSoldAsEachInRetail Entry with identifier=" + identifier)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim returnVal As Boolean = False

            ' Execute the function
            returnVal = CType(factory.ExecuteScalar("SELECT dbo.fn_IsSoldAsEachCostedByWeightItem('" & identifier & "')"), Boolean)

            logger.Debug("IfSoldAsEachInRetail Exit")

            Return returnVal
        End Function
        ''' <summary>
        ''' Checks to see if a costed by weight item is sold as each in Retail
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetCurrentNetCost(ByVal item_key As Integer, ByVal storeNo As Integer) As Decimal

            logger.Debug("GetCurrentNetCost Entry with identifier")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim returnVal As Decimal = 0

            Dim casecost As Decimal = 0
            Dim casepack As Integer = 1

            casecost = CType(factory.ExecuteScalar("SELECT dbo.fn_GetCurrentNetCost(" & item_key & "," & storeNo & ")"), Decimal)

            casepack = CType(factory.ExecuteScalar("SELECT dbo.fn_GetCurrentVendorPackage_Desc1(" & item_key & "," & storeNo & ")"), Integer)
            If casepack = 0 Then casepack = 1

            returnVal = casecost / casepack
            logger.Debug("GetCurrentNetCost Exit")

            Return returnVal
        End Function
        ''' <summary>
        ''' Returns average unit cost
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetAverageUnitCost(ByVal identifier As String) As Decimal

            logger.Debug("GetAverageUnitCost Entry with identifier=" + identifier)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim returnVal As Decimal = 0.0

            ' Execute the function
            returnVal = CType(factory.ExecuteScalar("SELECT dbo.fn_GetAverageUnitWeight('" & identifier & "')"), Decimal)

            logger.Debug("GetAverageUnitCost Exit")

            Return returnVal
        End Function


        ''' <summary>
        ''' Checks to see if at least one primary vendor relationship exists for the store-item.
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <param name="storeNo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function HasPrimaryVendor(ByVal itemKey As Integer, ByVal storeNo As Integer) As Boolean

            logger.Debug("HasPrimaryVendor Entry with itemKey=" + itemKey.ToString())

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim returnVal As Boolean = False

            ' Execute the function
            returnVal = CType(factory.ExecuteScalar("SELECT dbo.fn_HasPrimaryVendor(" & itemKey & ", " & storeNo & ")"), Boolean)

            logger.Debug("HasPrimaryVendor Exit")
            Return returnVal
        End Function

        Public Shared Function GetMarginInfo(ByVal itemKey As Integer) As ArrayList

            logger.Debug("GetMarginInfo Entry with itemKey = " + itemKey.ToString)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim itemMargin As ItemMarginBO
            Dim marginData As New ArrayList

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = itemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetMarginInfo", paramList)

                While results.Read
                    itemMargin = New ItemMarginBO()
                    itemMargin.StoreNo = results.GetInt32(results.GetOrdinal("Store_No"))
                    itemMargin.StoreName = results.GetString(results.GetOrdinal("Store_Name"))
                    itemMargin.CompanyName = results.GetString(results.GetOrdinal("CompanyName"))
                    itemMargin.CurrentPrice = results.GetString(results.GetOrdinal("CurrentPrice"))
                    itemMargin.PackageDesc1 = results.GetDecimal(results.GetOrdinal("Package_Desc1"))

                    If (Not results.IsDBNull(results.GetOrdinal("AvgCost"))) Then
                        itemMargin.AvgCost = results.GetDecimal(results.GetOrdinal("AvgCost"))
                    End If

                    If (Not results.IsDBNull(results.GetOrdinal("CurrentMarginCurrentCost"))) Then
                        itemMargin.CurrenMarginCurrentCost = results.GetDecimal(results.GetOrdinal("CurrentMarginCurrentCost"))
                    End If

                    If (Not results.IsDBNull(results.GetOrdinal("CurrentMarginAvgCost"))) Then
                        itemMargin.CurrentMarginAvgCost = results.GetDecimal(results.GetOrdinal("CurrentMarginAvgCost"))
                    End If

                    If (Not results.IsDBNull(results.GetOrdinal("CurrentNetCost"))) Then
                        itemMargin.NetUnitCost = results.GetDecimal(results.GetOrdinal("CurrentNetCost"))
                    End If

                    If (Not results.IsDBNull(results.GetOrdinal("CurrentRegCost"))) Then
                        itemMargin.RegularUnitCost = results.GetDecimal(results.GetOrdinal("CurrentRegCost"))
                    End If

                    If (Not results.IsDBNull(results.GetOrdinal("RegMarginAvgCost"))) Then
                        itemMargin.RegularMarginAvgCost = results.GetDecimal(results.GetOrdinal("RegMarginAvgCost"))
                    End If

                    If (Not results.IsDBNull(results.GetOrdinal("RegMarginCurrentCost"))) Then
                        itemMargin.RegularMarginCurrentCost = results.GetDecimal(results.GetOrdinal("RegMarginCurrentCost"))
                    End If



                    marginData.Add(itemMargin)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetMarginInfo Exit")


            Return marginData
        End Function

        Public Shared Function GetItemName(ByVal ItemKey As Integer) As DataTable

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim dt As DataTable

            Try

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = ItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                dt = factory.GetStoredProcedureDataTable("GetItemName", paramList)

            Catch ex As Exception

                MsgBox(ex.Message, MsgBoxStyle.Critical, "ItemDAO:GetItemName")
                logger.Error(ex.Message)
                dt = Nothing

            End Try

            Return dt

            logger.Debug("LockItem Exit")

        End Function

        Public Shared Function GetBatchesInSentState( _
            ByVal itemKey As Integer, _
            ByRef updateIdList As String, _
            ByRef updateInfoList As String, _
            ByRef rollbackIdList As String, _
            ByRef rollbackInfoList As String _
        ) As Boolean
            ' Tom Lux, 2/9/10, TFS 11978, 3.5.9.
            ' This sub should only be called if a batch-sensitive change has been made to an item screen.
            ' The only place this function (GetBatchesInSentState) should be called is at the top of a save-data function
            ' (like item.savedata()), so we should be here before any changes have been saved.

            Dim rsCurrentBatches As DAO.Recordset = Nothing

            ' New logic that updates or rolls back batches if there is a batch in sent state for this item (TFS Bug #5721)
            SQLOpenRS(rsCurrentBatches, "EXEC GetBatchesInSentState " & itemKey, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            rollbackIdList = ""
            rollbackInfoList = ""
            updateIdList = ""
            updateInfoList = ""

            If Not rsCurrentBatches.EOF Then
                Do While Not rsCurrentBatches.EOF
                    ' Batches get rolled back (or just updated) using the following table
                    '
                    'BypassPrintShelftags   BypassPrintShelfTags_PerformPrintLogic   Rollback
                    '--------------------   --------------------------------------   --------
                    '        0                               0                         YES
                    '        0                               1                         YES
                    '        1                               0                         NO 
                    '        1                               1                         YES
                    Select Case InstanceDataDAO.IsFlagActive("ByPassPrintShelfTags", CStr(rsCurrentBatches.Fields("Store_No").Value))
                        Case False
                            rollbackInfoList = rollbackInfoList & CStr(rsCurrentBatches.Fields("BatchDescription").Value) & _
                                                  ", Start Date = " & Format(rsCurrentBatches.Fields("StartDate").Value, "MM/dd/yyyy") & _
                                                  ", Store = " & CStr(rsCurrentBatches.Fields("Store_No").Value) & _
                                                  ", Batch Header ID = " & CStr(rsCurrentBatches.Fields("PriceBatchHeaderID").Value) & _
                                                  vbCrLf

                            If rollbackIdList.Length > 0 Then
                                rollbackIdList = rollbackIdList & "," & CStr(rsCurrentBatches.Fields("PriceBatchHeaderID").Value)
                            Else
                                rollbackIdList = rollbackIdList & CStr(rsCurrentBatches.Fields("PriceBatchHeaderID").Value)
                            End If
                        Case True
                            If InstanceDataDAO.IsFlagActive("ByPassPrintShelfTags_PerformPrintLogic", CStr(rsCurrentBatches.Fields("Store_No").Value)) Then
                                rollbackInfoList = rollbackInfoList & CStr(rsCurrentBatches.Fields("BatchDescription").Value) & _
                                                      ", Start Date = " & Format(rsCurrentBatches.Fields("StartDate").Value, "MM/dd/yyyy") & _
                                                      ", Store = " & CStr(rsCurrentBatches.Fields("Store_No").Value) & _
                                                      ", Batch Header ID = " & CStr(rsCurrentBatches.Fields("PriceBatchHeaderID").Value) & _
                                                      vbCrLf

                                If rollbackIdList.Length > 0 Then
                                    rollbackIdList = rollbackIdList & "," & CStr(rsCurrentBatches.Fields("PriceBatchHeaderID").Value)
                                Else
                                    rollbackIdList = rollbackIdList & CStr(rsCurrentBatches.Fields("PriceBatchHeaderID").Value)
                                End If
                            Else
                                updateInfoList = updateInfoList & CStr(rsCurrentBatches.Fields("BatchDescription").Value) & _
                                                      ", Start Date = " & Format(rsCurrentBatches.Fields("StartDate").Value, "MM/dd/yyyy") & _
                                                      ", Store = " & CStr(rsCurrentBatches.Fields("Store_No").Value) & _
                                                      ", Batch Header ID = " & CStr(rsCurrentBatches.Fields("PriceBatchHeaderID").Value) & _
                                                      vbCrLf

                                If updateIdList.Length > 0 Then
                                    updateIdList = updateIdList & CStr(rsCurrentBatches.Fields("PriceBatchHeaderID").Value) & ","
                                Else
                                    updateIdList = updateIdList & CStr(rsCurrentBatches.Fields("PriceBatchHeaderID").Value)
                                End If
                            End If
                    End Select

                    rsCurrentBatches.MoveNext()
                Loop

                If rollbackIdList.Length > 0 Or updateIdList.Length > 0 Then
                    Return True
                End If

            End If ' If result set for batches in 'sent' state has any records.
            Return False
        End Function

#End Region

#Region "write methods"

        ''' <summary>
        ''' Execute the PostStoreItemChange stored procedure to update the Price table with the
        ''' input Item/Store data.
        ''' </summary>
        ''' <param name="itemStore"></param>
        ''' <remarks></remarks>
        Public Shared Sub UpdateItemStoreData(ByRef itemStore As ItemStoreBO)

            logger.Debug("UpdateItemStoreData Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = itemStore.ItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = itemStore.StoreId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Restricted_Hours"
            currentParam.Value = itemStore.RestrictedHours
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IBM_Discount"
            currentParam.Value = itemStore.LineDiscount
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "NotAuthorizedForSale"
            currentParam.Value = itemStore.StopSale
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CompetitiveItem"
            currentParam.Value = itemStore.CompetitiveItem
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PosTare"
            If itemStore.POS_Tare = Nothing Or (Not itemStore.POS_Tare = Nothing AndAlso itemStore.POS_Tare.ToString.Trim.Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemStore.POS_Tare
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LinkedItem"
            If itemStore.LinkedItemKey = Nothing Or (Not itemStore.LinkedItemKey = Nothing AndAlso itemStore.LinkedItemKey.ToString.Trim.Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemStore.LinkedItemKey
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "GrillPrint"
            currentParam.Value = itemStore.GrillPrint
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "AgeCode"
            If itemStore.AgeCode = Nothing Or (Not itemStore.AgeCode = Nothing AndAlso itemStore.AgeCode.ToString.Trim.Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemStore.AgeCode
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "VisualVerify"
            currentParam.Value = itemStore.VisualVerify
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SrCitizenDiscount"
            currentParam.Value = itemStore.SrCitizenDiscount
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            If itemStore.StoreSubTeam = -1 Then
                ' this is a NULL subteam exception
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemStore.StoreSubTeam
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POSLinkCode"
            If itemStore.POSLinkCode = Nothing Or (Not itemStore.POSLinkCode = Nothing AndAlso itemStore.POSLinkCode.Trim.Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemStore.POSLinkCode
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "KitchenRoute_ID"
            If itemStore.KitchenRouteID = Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemStore.KitchenRouteID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Routing_Priority"
            currentParam.Value = itemStore.RoutingPriority
            currentParam.Type = DBParamType.SmallInt
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Consolidate_Price_To_Prev_Item"
            currentParam.Value = itemStore.ConsolidatePrice
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Print_Condiment_On_Receipt"
            currentParam.Value = itemStore.PrintCondimentOnReceipt
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Age_Restrict"
            currentParam.Value = itemStore.AgeRestrict
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "AuthorizedItem"
            currentParam.Value = itemStore.Authorized
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MixMatch"
            currentParam.Value = itemStore.MixMatch
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Discountable"
            currentParam.Value = itemStore.Discountable
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Refresh"
            currentParam.Value = itemStore.RefreshPOSInfo
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LocalItem"
            currentParam.Value = itemStore.LocalItem
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ItemSurcharge"
            currentParam.Value = itemStore.ItemSurcharge
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ECommerce"
            currentParam.Value = itemStore.ECommerce
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ItemStatusCode"
            If itemStore.ItemStatusCode Is Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemStore.ItemStatusCode
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OrderedByInfor"
            currentParam.Value = itemStore.OrderedByInfor
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute Stored Procedure to update the data
            ' PostStoreItemChange is used by EIM 
            ' PostStoreItemChangeECom is used by IRMA Client
            ' factory.ExecuteStoredProcedure("PostStoreItemChange", paramList)
            factory.ExecuteStoredProcedure("PostStoreItemChangeECom", paramList)

            logger.Debug("UpdateItemStoreData Exit")
        End Sub

        Public Shared Sub UpdateItemUomOverrideData(ByRef itemStore As ItemStoreBO)

            logger.Debug("UpdateItemUomOverrideData Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = itemStore.ItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = itemStore.StoreId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Retail_Unit_ID"
            If itemStore.RetailUomId = 0 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemStore.RetailUomId
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_ScaleUomUnit_ID"
            If itemStore.ScaleUomId = 0 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemStore.ScaleUomId
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_FixedWeight"
            If itemStore.FixedWeight = Nothing Or (Not itemStore.FixedWeight = Nothing AndAlso itemStore.FixedWeight.Trim.Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemStore.FixedWeight
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Scale_ByCount"
            If itemStore.ByCount = Nothing Or (Not itemStore.ByCount = Nothing AndAlso itemStore.ByCount.ToString.Trim.Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemStore.ByCount
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute Stored Procedure to update the data
            ' PostStoreItemChange is used by EIM 
            ' PostStoreItemChangeECom is used by IRMA Client
            ' factory.ExecuteStoredProcedure("PostStoreItemChange", paramList)
            factory.ExecuteStoredProcedure("UpdateItemUomOverride", paramList)

            logger.Debug("UpdateItemUomOverrideData Exit")
        End Sub
        ''' <summary>
        ''' This sub allows a save to the StoreItem.Authorized flag.
        ''' </summary>
        ''' <param name="itemStore"></param>
        ''' <remarks></remarks>
        Public Shared Sub UpdateItemStoreAuthorization(ByRef itemStore As ItemStoreBO)

            logger.Debug("UpdateItemStoreAuthorization Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = itemStore.ItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = itemStore.StoreId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "AuthorizedItem"
            currentParam.Value = itemStore.Authorized
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Refresh"
            currentParam.Value = itemStore.RefreshPOSInfo
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute Stored Procedure to update the data
            factory.ExecuteStoredProcedure("UpdateStoreItem", paramList)

            logger.Debug("UpdateItemStoreAuthorization Exit")

        End Sub

        ''' <summary>
        ''' Execute the UpdateItemPOSData stored procedure to update the Item table with the
        ''' input POS Item data.
        ''' </summary>
        ''' <param name="posItem"></param>
        ''' <remarks></remarks>
        Public Shared Sub UpdatePOSItemData(ByRef posItem As POSItemBO)

            logger.Debug("UpdatePOSItemData Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = posItem.ItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Food_Stamps"
            currentParam.Value = posItem.FoodStamps
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Price_Required"
            currentParam.Value = posItem.PriceRequired
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Quantity_Required"
            currentParam.Value = posItem.QuantityRequired
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "QtyProhibit"
            currentParam.Value = posItem.QuantityProhibit
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "GroupList"
            If posItem.GroupList = Nothing Or (Not posItem.GroupList = Nothing AndAlso posItem.GroupList.ToString.Trim.Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = posItem.GroupList
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Case_Discount"
            currentParam.Value = posItem.CaseDiscount
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Coupon_Multiplier"
            currentParam.Value = posItem.CouponMultiplier
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FSA_Eligible"
            currentParam.Value = posItem.FSAEligible
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Misc_Transaction_Sale"
            If posItem.MiscTransactionSale = Nothing Or (Not posItem.MiscTransactionSale = Nothing AndAlso posItem.MiscTransactionSale.ToString.Trim.Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = posItem.MiscTransactionSale
            End If
            currentParam.Type = DBParamType.SmallInt
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Misc_Transaction_Refund"
            If posItem.MiscTransactionRefund = Nothing Or (Not posItem.MiscTransactionRefund = Nothing AndAlso posItem.MiscTransactionRefund.ToString.Trim.Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = posItem.MiscTransactionRefund
            End If
            currentParam.Type = DBParamType.SmallInt
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Ice_Tare"
            If posItem.IceTare = Nothing Or (Not posItem.IceTare = Nothing AndAlso posItem.IceTare.ToString.Trim.Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = posItem.IceTare
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Unit_Price_Category"
            If posItem.UnitPriceCategory = Nothing Or (Not posItem.UnitPriceCategory = Nothing AndAlso posItem.UnitPriceCategory.ToString.Trim.Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = posItem.UnitPriceCategory
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Product_Code"
            If posItem.ProductCode = Nothing Or (Not posItem.ProductCode = Nothing AndAlso posItem.ProductCode.ToString.Trim.Equals("")) Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = posItem.ProductCode
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Execute Stored Procedure to get the data
            factory.ExecuteStoredProcedure("UpdateItemPOSData", paramList)

            logger.Debug("UpdatePOSItemData Exit")
        End Sub

        Public Shared Sub UpdateDiscontinue(ByRef itemStore As ItemStoreBO)

            logger.Debug("UpdateItemStoreData Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' Setup Params for Discontinue flag
            currentParam = New DBParam
            currentParam.Name = "Discontinue"
            currentParam.Value = itemStore.Discontinue
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = itemStore.ItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = itemStore.StoreId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute Stored Procedure to update Discontinue flag
            factory.ExecuteStoredProcedure("UpdateStoreItemVendorDiscontinue", paramList)

        End Sub

        Public Shared Sub UpdateOrRollbackBatches( _
            ByVal updateIdList As String, _
            ByVal rollbackIdList As String _
        )
            ' Tom Lux, 2/9/10, TFS 11978, 3.5.9.
            ' To facilitate conditionally rolling-back batches based on a prompt/response from the user,
            ' this sub was split into two parts:
            '  - A function (GetBatchesInSentState) that builds lists of batches in 'sent' state.
            '  - This sub (UpdateOrRollbackBatches) that actually performs the batch update or rollback, if there are any batches in the lists.

            ' We shouldn't be unconditionally calling this SP if the batch lists are empty,
            ' so I added an IF-wrapper.
            If updateIdList.Length > 0 Or rollbackIdList.Length > 0 Then
                SQLExecute("EXEC UpdateBatchesInSentState '" & updateIdList & "','" & rollbackIdList & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)
            End If
        End Sub

        Public Shared Function GetRetailUoms() As DataTable
            logger.Debug("GetRetailUoms Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing
            Dim paramList As New ArrayList

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataTable("GetAllItemUnitsCost", paramList)
            Catch ex As Exception
                Throw ex
            End Try

            logger.Debug("GetRetailUoms Exit")
            Return results
        End Function

        Public Shared Function GetScaleUoms() As DataTable
            logger.Debug("GetScaleUoms Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing
            Dim paramList As New ArrayList

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataTable("Scale_GetScaleUOMs", paramList)
            Catch ex As Exception
                Throw ex
            End Try

            logger.Debug("GetScaleUoms Exit")
            Return results
        End Function
#End Region

    End Class
End Namespace

