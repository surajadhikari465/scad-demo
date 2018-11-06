Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic

Namespace WholeFoods.IRMA.CompetitorStore.DataAccess
    Public Class CompetitorImportSession
        Public Function Delete(ByVal competitorImportSession As CompetitorStoreDataSet.CompetitorImportSessionRow) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim params As New ArrayList
            Dim success As Boolean = True

            params.Add(New DBParam("CompetitorImportSessionID", DBParamType.Int, competitorImportSession.CompetitorImportSessionID))

            Try
                factory.ExecuteStoredProcedure("DeleteCompetitorImportSession", params)
            Catch ex As Exception
                success = False
            End Try

            Return success
        End Function


        Public Function InsertCompetitorPriceFromImportSession(ByVal competitorImportSession As CompetitorStoreDataSet.CompetitorImportSessionRow, ByVal overwriteExistingPrices As Boolean) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim params As New ArrayList
            Dim success As Boolean = True

            params.Add(New DBParam("CompetitorImportSessionID", DBParamType.Int, competitorImportSession.CompetitorImportSessionID))
            params.Add(New DBParam("OverwriteExistingPrices", DBParamType.Bit, overwriteExistingPrices))

            Try
                factory.ExecuteStoredProcedure("InsertCompetitorPriceFromImportSession", params)
            Catch ex As Exception
                success = False
            End Try

            Return success
        End Function
    End Class
End Namespace