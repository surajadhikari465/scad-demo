Imports System.ComponentModel
Imports System.Data.SqlClient

Imports WholeFoods.IRMA.Ordering.BusinessLogic
Imports WholeFoods.IRMA.Ordering.BusinessLogic.FairShareAllocationBO
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility

Imports log4net

Public Class FairShareAllocationDAO

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    ''' <summary>
    ''' Gets the list of orders that the substitution item does not appear on.
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetOrdersMissingSubstitutionItem(ByVal Identifier As String, ByVal SubIdentifier As String, ByVal NonRetail As Boolean) As DataTable

        logger.Debug("GetOrdersMissingSubstitutionItem entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        ' setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "Identifier"
        currentParam.Value = Identifier
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "SubIdentifier"
        currentParam.Value = SubIdentifier
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "NonRetail"
        currentParam.Value = IIf(NonRetail, 1, 0)
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        ' Execute Stored Procedure to retrieve the data
        Return factory.GetStoredProcedureDataTable("dbo.GetOrdersNoSubstitutionItem", paramList)

        logger.Debug("GetOrdersMissingSubstitutionItem exit")

    End Function

    Public Shared Function DoFSASubstitution(ByVal Identifier As String, ByVal SubIdentifier As String, ByVal NonRetail As Boolean) As Boolean

        logger.Debug("DoFSASubstitution entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        ' setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "Identifier"
        currentParam.Value = Identifier
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "SubIdentifier"
        currentParam.Value = SubIdentifier
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "NonRetail"
        currentParam.Value = IIf(NonRetail, 1, 0)
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        ' Execute Stored Procedure to retrieve the data
        factory.ExecuteStoredProcedure("dbo.DoFSASubstitution", paramList)

        Return True

        logger.Debug("DoFSASubstitution exit")

    End Function

    Public Shared Function DoFSAAutoAllocate(ByVal WarehouseNo As Integer, ByVal SubteamNo As Integer, ByVal UserName As String, ByVal PreOrderOption As PreOrder, ByVal DoCasePackMoves As Boolean) As Boolean
        logger.Debug("DoFSAAutoAllocate entry")
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        ' setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "WarehouseNo"
        currentParam.Value = WarehouseNo
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "SubTeam_No"
        currentParam.Value = SubteamNo
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "UserName"
        currentParam.Value = UserName
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "PreOrder"
        currentParam.Value = CInt(PreOrderOption)
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "PerformCasePackMoves"
        currentParam.Value = CInt(DoCasePackMoves)
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        ' Execute the stored procedure 
        factory.ExecuteStoredProcedure("dbo.DoFSAAutoAllocate", paramList)

        Return True

        logger.Debug("DoFSAAutoAllocate exit")
    End Function

    Public Shared Sub UpdateFSAStoreOnOrder(ByVal WarehouseNo As Integer, ByVal SubteamNo As Integer, ByVal UserName As String, ByVal PreOrderOption As PreOrder)

        logger.Debug("UpdateFSAStoreOnOrder entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        ' setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "WarehouseNo"
        currentParam.Value = WarehouseNo
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "SubTeam_No"
        currentParam.Value = SubteamNo
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "UserName"
        currentParam.Value = UserName
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "PreOrder"
        currentParam.Value = CInt(PreOrderOption)
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        ' Execute Stored Procedure to retrieve the data
        factory.ExecuteStoredProcedure("dbo.UpdateFSAStoreOnOrder", paramList)

        logger.Debug("UpdateFSAStoreOnOrder exit")

    End Sub

    Public Shared Function GetSubTeamUserName(ByVal sStoreNo As String, ByVal sSubTeamNo As String, ByVal PreOrderOption As PreOrder, ByVal Username As String) As String
        logger.Debug("GetSubTeamUserName Entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        Return CType(factory.ExecuteScalar("SELECT dbo.fn_GetSubTeamUserName(" & sStoreNo & ", " & sSubTeamNo & ", " & PreOrderOption & ", '" & Username & "')"), String)

        logger.Debug("GetSubTeamUserName Exit")
    End Function

    Public Shared Sub DeleteTempFSARecords(ByVal iStoreNo As Integer, ByVal iSubTeamNo As Integer, ByVal sUserName As String, ByVal blnDeleteItem As Boolean, ByVal blnDeleteBOH As Boolean, ByVal PreOrderOption As PreOrder)
        logger.Debug("DeleteTempFSARecords entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        ' setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "StoreNo"
        currentParam.Value = iStoreNo
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "SubTeamNo"
        currentParam.Value = iSubTeamNo
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "UserName"
        currentParam.Value = sUserName
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "DeleteItem"
        currentParam.Value = blnDeleteItem
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "DeleteBOH"
        currentParam.Value = blnDeleteBOH
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "PreOrder"
        currentParam.Value = PreOrderOption
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        ' Execute Stored Procedure to retrieve the data
        factory.ExecuteStoredProcedure("dbo.DeleteTempFSARecords", paramList)

        logger.Debug("DeleteTempFSARecords exit")
    End Sub

    Public Shared Function GetOrderAllocateItemsQty(ByVal iItemKey As Integer) As DataTable
        logger.Debug("GetOrderAllocateItemsQty entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "ItemKey"
            currentParam.Value = iItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute Stored Procedure to retrieve the data
            Return factory.GetStoredProcedureDataTable("dbo.GetOrderAllocateItemsQty", paramList)
        Catch ex As Exception
            Throw New Exception("GetOrderAllocateItemsQty() failed: " & ex.Message)
        End Try

        logger.Debug("GetOrderAllocateItemsQty exit")
    End Function

    Public Shared Function GetAllocationItems(ByVal iItemKey As Integer) As DataTable
        logger.Debug("GetAllocationItems entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "ItemKey"
            currentParam.Value = iItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute Stored Procedure to retrieve the data
            Return factory.GetStoredProcedureDataTable("dbo.GetAllocationItems", paramList)
        Catch ex As Exception
            Throw New Exception("GetAllocationItems() failed: " & ex.Message)
        End Try

        logger.Debug("GetAllocationItems exit")
    End Function

    Public Shared Function GetAllocationItemPackSizes(ByVal iItemKey As Integer)
        logger.Debug("GetAllocationItemPackSizes entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "ItemKey"
            currentParam.Value = iItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Return factory.GetStoredProcedureDataTable("dbo.GetAllocationItemPackSizes", paramList)
        Catch ex As Exception
            Throw New Exception("GetAllocationItemPackSizes() failed: " & ex.Message)
        End Try

        logger.Debug("GetAllocationItemPackSizes exit")
    End Function

    Public Shared Sub RefreshBOH(ByVal iStoreNo As Integer, ByVal iSubTeamNo As Integer, ByVal blnNonRetail As OrderSubteamType, ByVal blnAdjustBOH As Boolean, ByVal blnInclWOO As Boolean, ByVal iPreOrder As PreOrder, Optional ByVal dtWOOStart As Date = Nothing, Optional ByVal dtWOOEnd As Date = Nothing)
        logger.Debug("RefreshBOH Entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As ArrayList = New ArrayList
        Dim currentParam As DBParam

        Try
            paramList.Clear()

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = iStoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UserName"
            currentParam.Value = gsUserName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            currentParam.Value = iSubTeamNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "NonRetail"
            currentParam.Value = blnNonRetail
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "AdjustBOH"
            currentParam.Value = blnAdjustBOH
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IncludeInboundQty"
            currentParam.Value = blnInclWOO
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ExpectedDateStart"
            If dtWOOStart = Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = dtWOOStart
            End If
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ExpectedDateEnd"
            If dtWOOEnd = Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = dtWOOEnd
            End If
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PreOrderOption"
            currentParam.Value = iPreOrder
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("GetOrderAllocItems", paramList)
        Catch ex As Exception
            Throw New Exception("GetOrderAllocItems() failed: " & ex.Message)
        End Try

        logger.Debug("RefreshBOH Exit")
    End Sub

    Public Shared Function CountAllocationItems() As Integer
        logger.Debug("CountAllocationItems Entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        Try
            Return CType(factory.ExecuteScalar("SELECT dbo.fn_CountAllocationItems()"), Integer)
        Catch ex As Exception
            Throw New Exception("fn_CountAllocationItems() failed: " & ex.Message)
        End Try

        logger.Debug("CountAllocationItems Exit")
    End Function

    Public Shared Function GetOrderAllocationOrderItems(ByVal iStoreNo As Integer, ByVal iSubTeamNo As Integer, ByVal blnNonRetail As OrderSubteamType) As DataTable
        logger.Debug("GetOrderAllocationOrderItems entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = iStoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            currentParam.Value = iSubTeamNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "NonRetail"
            currentParam.Value = blnNonRetail
            currentParam.Type = CInt(DBParamType.Int)
            paramList.Add(currentParam)

            Return factory.GetStoredProcedureDataTable("dbo.GetOrderAllocOrderItems", paramList)
        Catch ex As Exception
            Throw New Exception("GetOrderAllocOrderItems() failed: " & ex.Message)
        End Try

        logger.Debug("GetOrderAllocationOrderItems exit")
    End Function

    Public Shared Sub UpdateAllocationItemPackSize()
        logger.Debug("UpdateAllocationItemPackSize entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        Try
            factory.ExecuteStoredProcedure("dbo.UpdateAllocationItemPackSize")
        Catch ex As Exception
            Throw New Exception("UpdateAllocationItemPackSize() failed: " & ex.Message)
        End Try

        logger.Debug("UpdateAllocationItemPackSize exit")
    End Sub

    Public Shared Function GetOrderAllocationItems(ByVal iStoreNo As Integer, ByVal iSubTeamNo As Integer, ByVal iPreOrder As Integer, ByVal iGroupById As Integer, ByVal bMultiPackOnly As Boolean) As DataTable
        logger.Debug("GetOrderAllocationItems entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "StoreNo"
            currentParam.Value = iStoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeamNo"
            currentParam.Value = iSubTeamNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PreOrder"
            currentParam.Value = iPreOrder
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "GroupById"
            currentParam.Value = iGroupById
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MultiPackOnly"
            currentParam.Value = bMultiPackOnly
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            Return factory.GetStoredProcedureDataTable("dbo.GetOrderAllocationItems", paramList)
        Catch ex As Exception
            Throw New Exception("GetOrderAllocationItems() failed: " & ex.Message)
        End Try

        logger.Debug("GetOrderAllocationItems exit")
    End Function

    Public Shared Function DoFSAWarehouseSend(ByVal Session As FairShareAllocationBO.AllocationSession) As Boolean

        logger.Debug("DoFSAWarehouseSend entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "PreOrder"
            currentParam.Value = CInt(Session.PreOrderOption)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Username"
            currentParam.Value = Session.Username
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Subteam"
            currentParam.Value = Session.SubteamNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Warehouse"
            currentParam.Value = Session.Warehouse
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("dbo.DoFSAWarehouseSend", paramList)

            Return True

        Catch ex As Exception
            Throw New Exception("DoFSAWarehouseSend() failed: " & ex.Message)
        End Try

        logger.Debug("DoFSAWarehouseSend exit")

    End Function
End Class
