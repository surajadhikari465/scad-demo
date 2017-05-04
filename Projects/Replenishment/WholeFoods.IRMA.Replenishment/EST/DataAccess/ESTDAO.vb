Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility
Imports log4net

Namespace WholeFoods.IRMA.Replenishment.EST.DataAccess
    Public Class ESTDAO
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

        Public Function LoadESTFileUpdate(ByVal sImportFilename As String) As Boolean
            Logger.LogDebug("LoadESTFileUpdate entry", Me.GetType)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "ImportFilename"
            currentParam.Value = sImportFilename
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("LoadESTFileUpdate", paramList)

            Return True

            Logger.LogDebug("LoadESTFileUpdate exit", Me.GetType)
        End Function
    End Class
End Namespace
