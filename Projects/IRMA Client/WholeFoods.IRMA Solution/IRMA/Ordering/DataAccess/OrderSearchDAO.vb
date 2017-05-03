Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility
Imports log4net

Namespace WholeFoods.IRMA.Ordering.DataAccess
    Public Class OrderSearchDAO

        Public Shared Function GetAllOrderExternalSource() As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Return factory.GetStoredProcedureDataTable("GetAllOrderExternalSource")
        End Function

        Public Shared Function IsMultipleJurisdiction() As Boolean

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As Boolean = False

            Try
                results = factory.ExecuteScalar("SELECT dbo.fn_IsRegionWithMutipleJurisdiction()")

            Catch ex As Exception
                Throw ex
            End Try

            Return results

        End Function
    End Class
End Namespace