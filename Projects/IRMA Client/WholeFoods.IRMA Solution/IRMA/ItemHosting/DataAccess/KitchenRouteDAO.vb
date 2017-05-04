Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemHosting.DataAccess
    Public Class KitchenRouteDAO

        Public Shared Function GetComboList() As DataTable

            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Try

                Return factory.GetStoredProcedureDataTable("GetKitchenRoutes")

            Finally

            End Try

        End Function

    End Class
End Namespace