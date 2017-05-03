Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.CompetitorStore.DataAccess
    Public Class CompetitivePriceType

        Public Shared Sub List(ByVal table As DataTable)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Using adapter As SqlClient.SqlDataAdapter = factory.GetDataAdapter("GetCompetitivePriceTypes", Nothing)
                adapter.Fill(table)
            End Using
        End Sub

        Public Shared Function List() As DataTable
            Dim table As New DataTable()

            List(table)

            Return table
        End Function

    End Class
End Namespace