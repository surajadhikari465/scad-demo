Imports WholeFoods.Utility.DataAccess

''' <summary>
''' Handles read/writes
''' </summary>
''' <remarks></remarks>
Public Class TitleDAO

    ''' <summary>
    ''' Retruns a list of all entries in the Titles table.
    ''' </summary>
    ''' <returns>DataTable</returns>
    ''' <remarks>New Stored Procedure in IRMA v3.2.</remarks>
    Public Shared Function GetTitles() As DataTable
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataTable = Nothing

        Try
            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataTable("SecurityGetTitles")
        Catch ex As Exception
            Throw ex
        End Try

        Return results
    End Function

    Public Shared Function GetTitleConflicts(ByVal iTitleId As Integer) As DataTable
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataTable = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "@TitleID"
        currentParam.Value = iTitleId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        Try
            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataTable("GetTitleConflicts", paramList)
        Catch ex As Exception
            Throw ex
        End Try

        Return results
    End Function

    Public Shared Function InsertRoleConflictReason(ByVal sConflictType As String, ByVal iUserId As Integer, ByVal iTitleId As Integer, ByVal sRole1 As String, ByVal sRole2 As String, ByVal sReason As String, ByVal iInsertUserId As Integer) As Boolean
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "@ConflictType"
        currentParam.Value = sConflictType
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@UserID"
        currentParam.Value = iUserId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@TitleID"
        currentParam.Value = iTitleId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Role1"
        currentParam.Value = sRole1
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Role2"
        currentParam.Value = sRole2
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Reason"
        currentParam.Value = sReason
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@InsertUserId"
        currentParam.Value = iInsertUserId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        Try
            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("InsertRoleConflictReason", paramList)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetTitlePermissions(ByVal iTitleId As Integer) As DataTable
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataTable = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "@TitleID"
        currentParam.Value = iTitleId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        Try
            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataTable("GetTitlePermissions", paramList)
        Catch ex As Exception
            Throw ex
        End Try

        Return results
    End Function

    Public Shared Function DeleteTitle(ByVal iTitleId As Integer) As Boolean
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "@TitleID"
        currentParam.Value = iTitleId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        Try
            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("DeleteTitle", paramList)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function UpdateTitleConflicts(ByVal iTitleId As Integer, ByVal iUserId As Integer) As Boolean
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "@TitleId"
        currentParam.Value = iTitleId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@UserId"
        currentParam.Value = iUserId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        Try
            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("UpdateTitleConflicts", paramList)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetUsersWithTitle(ByVal iTitleId As Integer) As DataTable
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataTable = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "@TitleID"
        currentParam.Value = iTitleId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        Try
            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataTable("GetUsersWithTitle", paramList)
        Catch ex As Exception
            Throw ex
        End Try

        Return results
    End Function

    Public Shared Function AddTitle(ByVal sTitleDesc As String) As Boolean
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "@TitleDesc"
        currentParam.Value = sTitleDesc
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        Try
            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("AddTitle", paramList)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function UpdateTitle(ByVal iTitleId As Integer, ByVal sTitleDesc As String) As Boolean
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "@TitleID"
        currentParam.Value = iTitleId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@TitleDesc"
        currentParam.Value = sTitleDesc
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        Try
            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("UpdateTitle", paramList)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function SaveTitlePermissions(ByVal iTitleId As Integer, ByVal blnAccountant As Boolean, ByVal blnBatchBuildOnly As Boolean, ByVal blnBuyer As Boolean,
                                                ByVal blnCoordinator As Boolean, ByVal blnCostAdmin As Boolean, ByVal blnFacilityCreditProcessor As Boolean, ByVal blnDCAdmin As Boolean,
                                                ByVal blnDeletePO As Boolean, ByVal blnEInvoicingAdministrator As Boolean, ByVal blnInventoryAdministrator As Boolean, ByVal blnItemAdministrator As Boolean,
                                                ByVal blnLockAdministrator As Boolean, ByVal blnPOAccountant As Boolean, ByVal blnPOApprovalAdmin As Boolean,
                                                ByVal blnPOEditor As Boolean, ByVal blnPriceBatchProcessor As Boolean, ByVal blnDistributor As Boolean, ByVal blnVendorAdministrator As Boolean,
                                                ByVal blnVendorCostDiscrepancyAdmin As Boolean, ByVal blnWarehouse As Boolean, ByVal blnCancelAllSales As Boolean, ByVal blnApplicationConfigAdmin As Boolean,
                                                ByVal blnDataAdministrator As Boolean, ByVal blnPOSInterfaceAdministrator As Boolean, ByVal blnJobAdministrator As Boolean,
                                                ByVal blnStoreAdministrator As Boolean, ByVal blnUserMaintenance As Boolean, ByVal blnShrink As Boolean, ByVal blnShrinkAdmin As Boolean,
                                                ByVal blnTaxAdministrator As Boolean) As Boolean

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "@TitleId"
        currentParam.Value = iTitleId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Accountant"
        currentParam.Value = blnAccountant
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@BatchBuildOnly"
        currentParam.Value = blnBatchBuildOnly
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Buyer"
        currentParam.Value = blnBuyer
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Coordinator"
        currentParam.Value = blnCoordinator
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@CostAdministrator"
        currentParam.Value = blnCostAdmin
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@DCAdmin"
        currentParam.Value = blnDCAdmin
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@DeletePO"
        currentParam.Value = blnDeletePO
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@EInvoicing"
        currentParam.Value = blnEInvoicingAdministrator
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@FacilityCreditProcessor"
        currentParam.Value = blnFacilityCreditProcessor
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@InventoryAdministrator"
        currentParam.Value = blnInventoryAdministrator
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@ItemAdministrator"
        currentParam.Value = blnItemAdministrator
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@LockAdministrator"
        currentParam.Value = blnLockAdministrator
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@POAccountant"
        currentParam.Value = blnPOAccountant
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@POApprovalAdministrator"
        currentParam.Value = blnPOApprovalAdmin
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@POEditor"
        currentParam.Value = blnPOEditor
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@PriceBatchProcessor"
        currentParam.Value = blnPriceBatchProcessor
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Distributor"
        currentParam.Value = blnDistributor
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@TaxAdministrator"
        currentParam.Value = blnTaxAdministrator
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@VendorAdministrator"
        currentParam.Value = blnVendorAdministrator
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@VendorCostDiscrepancyAdmin"
        currentParam.Value = blnVendorCostDiscrepancyAdmin
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Warehouse"
        currentParam.Value = blnWarehouse
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@CancelAllSales"
        currentParam.Value = blnCancelAllSales
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@ApplicationConfigAdmin"
        currentParam.Value = blnApplicationConfigAdmin
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@DataAdministrator"
        currentParam.Value = blnDataAdministrator
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@JobAdministrator"
        currentParam.Value = blnJobAdministrator
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@POSInterfaceAdministrator"
        currentParam.Value = blnPOSInterfaceAdministrator
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@StoreAdministrator"
        currentParam.Value = blnStoreAdministrator
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@UserMaintenance"
        currentParam.Value = blnUserMaintenance
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Shrink"
        currentParam.Value = blnShrink
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@ShrinkAdmin"
        currentParam.Value = blnShrinkAdmin
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        Try
            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("SaveTitlePermissions", paramList)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function AddRoleConflict(ByVal sRole1 As String, ByVal sRole2 As String) As Boolean
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "@Role1"
        currentParam.Value = sRole1
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Role2"
        currentParam.Value = sRole2
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        Try
            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("AddRoleConflict", paramList)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DeleteRoleConflict(ByVal iConflictId As Integer) As Boolean
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "@ConflictId"
        currentParam.Value = iConflictId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        Try
            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("DeleteRoleConflict", paramList)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function GetRoleConflicts() As DataTable
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataTable = Nothing

        Try
            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataTable("GetRoleConflicts")
        Catch ex As Exception
            Throw ex
        End Try

        Return results
    End Function

    Public Shared Function GetRoleConflictReason(ByVal sConflictType As String, ByVal iUserId As Integer, ByVal iTitleId As Integer) As DataTable
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam
        Dim results As DataTable

        currentParam = New DBParam
        currentParam.Name = "@ConflictType"
        currentParam.Value = sConflictType
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@UserId"
        currentParam.Value = iTitleId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@TitleId"
        currentParam.Value = iTitleId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        Try
            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataTable("GetRoleConflictReason", paramList)
        Catch ex As Exception
            Throw ex
        End Try

        Return results
    End Function

    Public Shared Function GetToolTipText(ByVal sRole As String) As String
        Dim sText As String = ""

        Select Case UCase(Trim(sRole))
            Case "ACCOUNTANT"
                sText = "Accountant can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Edit -> Orders -> Add/Edit" & vbCrLf & _
                         "   Edit -> Orders -> Invoice Entry" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools -> Vendor List"

            Case "BATCH BUILD ONLY"
                sText = "Batch Build Only can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Edit -> Pricing -> Batches (this role toggles the visibility of the ""Package"" and ""Send To Store"" buttons)" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools -> Vendor List"

            Case "BUYER"
                sText = "Buyer can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Edit -> Item -> Edit Existing Item" & vbCrLf & _
                         "   Edit -> Orders -> Add/Edit" & vbCrLf & _
                         "   Edit -> Orders -> Item Queue" & vbCrLf & _
                         "   Edit -> Pricing -> Reprint Signs" & vbCrLf & _
                         "   Edit -> Vendors -> Add/Edit" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools -> Vendor List"

            Case "COORDINATOR"
                sText = "Coordinator can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   File -> Import -> RIPE Order" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Edit -> Item -> Edit Existing Item" & vbCrLf & _
                         "   Edit -> Orders -> Add/Edit" & vbCrLf & _
                         "   Edit -> Orders -> Item Queue" & vbCrLf & _
                         "   Edit -> Pricing -> Reprint Signs" & vbCrLf & _
                         "   Edit -> Vendors -> Add/Edit" & vbCrLf & _
                         "   Inventory -> Shrink Corrections" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools -> Vendor List"

            Case "COST ADMINISTRATOR"
                sText = "Cost Administrator can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Inventory -> Inventory Costing -> Average Cost Adjustment" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools -> Vendor List"

            Case "FACILITY CREDIT PROCESSOR"
                sText = "Facility Credit Processor can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Edit -> Orders -> Add/Edit" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools -> Vendor List"

            Case "DC ADMIN"
                sText = "DC Admin can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Edit -> Item -> Edit Existing Item" & vbCrLf & _
                         "   Edit -> Vendors -> Add/Edit" & vbCrLf & _
                         "   Inventory -> Adjustments" & vbCrLf & _
                         "   Inventory -> Cycle Counts" & vbCrLf & _
                         "   Inventory -> Locations" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools -> Vendor List"

            Case "E-INVOICING"
                sText = "E-Invoicing can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Edit -> Orders -> E-Invoicing" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools   -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools   -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools   -> Vendor List"

            Case "INVENTORY ADMINISTRATOR"
                sText = "Accountant can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Inventory -> Adjustments" & vbCrLf & _
                         "   Inventory -> Cycle Counts" & vbCrLf & _
                         "   Inventory -> Locations" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools   -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools   -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools   -> Vendor List"

            Case "ITEM ADMINISTRATOR"
                sText = "Item Administrator can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   File -> Export -> Planogram" & vbCrLf & _
                         "   File -> Export -> RGIS" & vbCrLf & _
                         "   File -> Import -> EIM" & vbCrLf & _
                         "   File -> Import -> Import Order" & vbCrLf & _
                         "   File -> Import -> Item Maintenance Bulk Load" & vbCrLf & _
                         "   File -> Import -> Item Store Maintenance Bulk Load" & vbCrLf & _
                         "   File -> Import -> Item Vendor Maintenance Bulk Load" & vbCrLf & _
                         "   File -> Import -> Planogram" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Edit -> Item -> Add New Item" & vbCrLf & _
                         "   Edit -> Item -> Edit Existing Item" & vbCrLf & _
                         "   Edit -> Item -> Item Chain" & vbCrLf & _
                         "   Edit -> Item -> Bulk Load Audit History" & vbCrLf & _
                         "   Edit -> Pricing -> Batches" & vbCrLf & _
                         "   Edit -> Pricing -> Reprint Signs" & vbCrLf & _
                         "   Edit -> Pricing -> Promotional Offers" & vbCrLf & _
                         "   Edit -> Pricing -> Price Change Wizard" & vbCrLf & _
                         "   Edit -> Tax Hosting -> Tax Classification" & vbCrLf & _
                         "   Edit -> Tax Hosting -> Tax Flags" & vbCrLf & _
                         "   Edit -> Tax Hosting -> Tax Jurisdiction" & vbCrLf & _
                         "   Edit -> Vendors -> Cost Import Exceptions" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools   -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools   -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools   -> Vendor List"

            Case "LOCK ADMINISTRATOR"
                sText = "Lock Administrator can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools   -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools   -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools   -> Vendor List"

            Case "PO ACCOUNTANT"
                sText = "PO Accountant can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Edit -> Item -> Edit Existing Item" & vbCrLf & _
                         "   Edit -> Orders -> Add/Edit" & vbCrLf & _
                         "   Edit -> Orders -> Item Queue" & vbCrLf & _
                         "   Edit -> Orders -> Invoice Entry" & vbCrLf & _
                         "   Edit -> Orders -> Invoice Matching" & vbCrLf & _
                         "   Edit -> Orders -> Batch Receive Close" & vbCrLf & _
                         "   Edit -> Vendors -> Add/Edit" & vbCrLf & _
                         "   Inventory -> Shrink Corrections" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools   -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools   -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools   -> Vendor List"

            Case "PO APPROVAL ADMINISTRATOR"
                sText = "PO Approval Administrator can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   File -> Import -> RIPE" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Edit -> Item -> Edit Existing Item" & vbCrLf & _
                         "   Edit -> Orders -> Add/Edit" & vbCrLf & _
                         "   Edit -> Orders -> Item Queue" & vbCrLf & _
                         "   Edit -> Orders -> Invoice Entry" & vbCrLf & _
                         "   Edit -> Orders -> Invoice Matching" & vbCrLf & _
                         "   Edit -> Pricing -> Reprint Signs" & vbCrLf & _
                         "   Edit -> Vendors -> Add/Edit" & vbCrLf & _
                         "   Inventory -> Shrink Corrections" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools   -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools   -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools   -> Vendor List"

            Case "PO EDITOR"
                sText = "PO Editor can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Edit -> Orders -> Add/Edit" & vbCrLf & _
                         "   (Note: This role can only edit the Qty, UOM, and Pack on an order item)" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools   -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools   -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools   -> Vendor List"

            Case "PRICE BATCH PROCESSOR"
                sText = "Price Batch Processor can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   File -> Import -> Planogram" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Edit -> Item -> Edit Existing Item" & vbCrLf & _
                         "   Edit -> Pricing -> Batches" & vbCrLf & _
                         "   Edit -> Pricing -> Reprint Signs" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools   -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools   -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools   -> Vendor List"

            Case "RECEIVER"
                sText = "Receiver can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Edit -> Item -> Edit Existing Item" & vbCrLf & _
                         "   Edit -> Orders -> Add/Edit" & vbCrLf & _
                         "   Edit -> Orders -> Invoice Entry" & vbCrLf & _
                         "   Edit -> Vendors -> Add/Edit" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools   -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools   -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools   -> Vendor List"

            Case "SHRINK"
                sText = "Shrink can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Inventory -> Shrink Corrections" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools   -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools   -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools   -> Vendor List"

            Case "SHRINK ADMINISTRATOR"
                sText = "Shrink Admin can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Inventory -> Shrink Corrections" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools   -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools   -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools   -> Vendor List"

            Case "TAX ADMINISTRATOR"
                sText = "Tax Administrator can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Edit -> Tax Hosting -> Tax Classification" & vbCrLf & _
                         "   Edit -> Tax Hosting -> Tax Flags" & vbCrLf & _
                         "   Edit -> Tax Hosting -> Tax Jurisdiction" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Store"

            Case "VENDOR ADMINISTRATOR"
                sText = "Vendor Administrator can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Edit -> Vendors -> Add/Edit" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools   -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools   -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools   -> Vendor List"

            Case "VENDOR COST DISCREPANCY ADMIN"
                sText = "Vendor Cost Discrepancy Admin can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Edit -> Orders -> Invoice Matching" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools   -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools   -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools   -> Vendor List"

            Case "WAREHOUSE"
                sText = "Warehouse can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Edit -> Orders -> Add/Edit" & vbCrLf & _
                         "   Edit -> Orders -> Allocate" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools   -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools   -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools   -> Vendor List"
            Case "CANCEL ALL SALES"
                sText = "Cancel All Sales can access the following screens:" & vbCrLf &
                        " Data -> Cancel All Sales"
            Case "APPLICATION CONFIGURATION ADMIN"
                sText = "Application Configuration Admin can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Attribute Defaults" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Average Cost Adjustment Reasons" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Brand Name" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Class Name" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Conversion" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Currency" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> E-Invoicing Element Configuration" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Inventory Adjustment Code" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Invoice Matching Tolerance" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Item Attributes" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Item Units" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Order Window" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Origin" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Price Types" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Role Conflicts" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Shelf Life" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Store" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Store/SubTeam Relationships" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> SubTeams" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Tax Jurisdictions" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Team" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Zone Distribution" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Zone Markup" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Zones" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Scale Maintenance -> Eat By" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Scale Maintenance -> Extra Text" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Scale Maintenance -> Grade" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Scale Maintenance -> Label Format" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Scale Maintenance -> Label Style" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Scale Maintenance -> Label Type" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Scale Maintenance -> Nutrifact" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Scale Maintenance -> Random Weight Type" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> Application Configuration -> Scale Maintenance -> Tare"

            Case "DATA ADMINISTRATOR"
                sText = "Data Administrator can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Data -> Batch Rollback" & vbCrLf & _
                         "   Data -> Restore Deleted Item" & vbCrLf & _
                         "   Data -> Scale/POS Push"

            Case "JOB ADMINISTRATOR"
                sText = "Job Administrator can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Administration -> Scheduled Jobs -> AP Upload" & vbCrLf & _
                         "   Administration -> Scheduled Jobs -> Audit Exceptions Report" & vbCrLf & _
                         "   Administration -> Scheduled Jobs -> Average Cost Update" & vbCrLf & _
                         "   Administration -> Scheduled Jobs -> Close Receiving" & vbCrLf & _
                         "   Administration -> Scheduled Jobs -> PLUM Host" & vbCrLf & _
                         "   Administration -> Scheduled Jobs -> POS Pull" & vbCrLf & _
                         "   Administration -> Scheduled Jobs -> Send Orders" & vbCrLf & _
                         "   Administration -> Scheduled Jobs -> TLog Processing" & vbCrLf & _
                         "   Administration -> Scheduled Jobs -> View App Logs" & vbCrLf & _
                         "   Administration -> Scheduled Jobs -> Weekly Sales Rollup"

            Case "POS INTERFACE ADMINISTRATOR"
                sText = "POS Interface Administrator can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Administration -> POS Interface -> File Writers" & vbCrLf & _
                         "   Administration -> POS Interface -> Store FTP Configuration" & vbCrLf & _
                         "   Administration -> POS Interface -> Pricing Methods"

            Case "SECURITY ADMINISTRATOR"
                sText = "Security Administrator can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Administration -> Users -> Manage Titles" & _
                         "   Administration -> Users -> Manage Users"

            Case "STORE ADMINISTRATOR"
                sText = "Store Administrator can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Administration -> Stores -> Build POS File" & vbCrLf & _
                         "   Administration -> Stores -> Build Scale File" & vbCrLf & _
                         "   Administration -> Stores -> Create Store"
            Case "DELETE PO"
                sText = "Delete PO can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Competitor Trend Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> Import Competitor Prices" & vbCrLf & _
                         "   Edit -> Competitor Store -> Margin Impact Report" & vbCrLf & _
                         "   Edit -> Competitor Store -> National Purchasing Values" & vbCrLf & _
                         "   Edit -> Orders -> Add/Edit" & vbCrLf & _
                         "   Reports -> All Reports Available" & vbCrLf & _
                         "   Tools   -> TGM -> Load TGM View" & vbCrLf & _
                         "   Tools   -> TGM -> New TGM View" & vbCrLf & _
                         "   Tools   -> Vendor List"

            Case "SYSTEM CONFIGURATION ADMINISTRATOR"
                sText = "System Configuration Administrator can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> System Configuration -> Build Configuration" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> System Configuration -> Instance Data" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> System Configuration -> Instance Data Flags" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> System Configuration -> Manage Configuration" & vbCrLf & _
                         "   Administration -> IRMA Configuration -> System Configuration -> Menu Access"

            Case "SUPER USER"
                sText = "Super User has can access all screens and functionality."

            Case "USER MAINTENANCE"
                sText = "User Maintenance can access the following screens:" & vbCrLf & _
                         "   File -> IRMA Process Monitor" & vbCrLf & _
                         "   Administration -> Users -> Manage Users"
        End Select

        Return sText
    End Function
End Class
