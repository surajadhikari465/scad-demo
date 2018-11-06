Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Replenishment.SendOrders.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility
Imports log4net

Namespace WholeFoods.IRMA.Replenishment.HART.DataAccess
    Public Class HARTDAO

        Public Function LoadInventoryServiceExport() As Boolean
            Logger.LogDebug("LoadInventoryServiceExport entry", Me.GetType)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            factory.CommandTimeout = CInt(ConfigurationServices.AppSettings("defaultCommandTimeout"))
            factory.ExecuteStoredProcedure("LoadInventoryServiceExport")

            Return True

            Logger.LogDebug("LoadInventoryServiceExport exit", Me.GetType)
        End Function

        Public Function GetInventoryServiceExport(ByVal iICVID As Integer, ByVal iBusinessUnitID As Integer) As DataTable
            Logger.LogDebug("GetInventoryServiceExport entry", Me.GetType)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim dt As DataTable
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "ICVID"
            currentParam.Value = 3
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "BusinessUnit_ID"
            currentParam.Value = iBusinessUnitID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            dt = factory.GetStoredProcedureDataTable("GetInventoryServiceExport", paramList)

            Return dt

            Logger.LogDebug("GetInventoryServiceExport exit", Me.GetType)
        End Function

        Public Function GetStoreBusinessUnits() As DataTable
            Logger.LogDebug("GetStoreBusinessUnits entry", Me.GetType)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim dt As DataTable

            dt = factory.GetStoredProcedureDataTable("GetRetailStores")

            Return dt

            Logger.LogDebug("GetStoreBusinessUnits exit", Me.GetType)
        End Function

        Public Function GetRegionCode() As String
            Logger.LogDebug("GetRegionCode entry", Me.GetType)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim dt As DataTable
            Dim sRegionCode As String = ""

            dt = factory.GetStoredProcedureDataTable("GetRegions")

            For Each row As DataRow In dt.Rows
                sRegionCode = row("RegionCode").ToString
            Next

            Return sRegionCode

            Logger.LogDebug("GetRegionCode exit", Me.GetType)
        End Function

        Public Function DeleteInventortServiceImportLoad(ByVal iBusinessUnitID As Integer) As DataTable
            Logger.LogDebug("DeleteInventortServiceImportLoad entry", Me.GetType)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim dt As DataTable
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "BusinessUnit_ID"
            currentParam.Value = iBusinessUnitID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            dt = factory.GetStoredProcedureDataTable("DeleteInventoryServiceImportLoad", paramList)

            Return dt

            Logger.LogDebug("DeleteInventortServiceImportLoad exit", Me.GetType)
        End Function

        Public Function LoadInventoryServiceImport(ByVal sImportFilename As String, ByVal bUsePSSubTeamNoForImport As Boolean) As Boolean
            Logger.LogDebug("LoadInventoryServiceImport entry", Me.GetType)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "ImportFilename"
            currentParam.Value = sImportFilename
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UsePSSubTeamNoForImport"
            currentParam.Value = bUsePSSubTeamNoForImport
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("LoadInventoryServiceImport", paramList)

            Return True

            Logger.LogDebug("LoadInventoryServiceImport exit", Me.GetType)
        End Function

        Public Function LoadCycleCountExternal(ByVal dtDate As DateTime, ByVal bUsePSSubTeamNoForImport As Boolean) As Boolean
            Logger.LogDebug("LoadCycleCountExternal entry", Me.GetType)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "EndScan"
            currentParam.Value = dtDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UsePSSubTeamNoForImport"
            currentParam.Value = bUsePSSubTeamNoForImport
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("LoadCycleCountExternal", paramList)

            Return True

            Logger.LogDebug("LoadCycleCountExternal exit", Me.GetType)
        End Function

        Public Function GetInventoryStoreOpsExport(ByVal dtDate As DateTime) As DataTable
            Logger.LogDebug("GetInventoryStoreOpsExport entry", Me.GetType)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim dt As DataTable
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "EndScan"
            currentParam.Value = dtDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            dt = factory.GetStoredProcedureDataTable("HARTGetInventoryStoreOpsExport", paramList)

            Return dt

            Logger.LogDebug("GetInventoryStoreOpsExport exit", Me.GetType)
        End Function


        Public Function GetFiscalCalendarInfo(ByVal dateValue As Date) As DataTable

            Logger.LogDebug("GetFiscalCalendarInfo entry", Me.GetType)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As DataTable = Nothing

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "@myDate"
            currentParam.Value = dateValue
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataTable("dbo.GetFiscalCalendarInfo", paramList)

            Return results

            Logger.LogDebug("GetFiscalCalendarInfo exit", Me.GetType)

        End Function
    End Class
End Namespace