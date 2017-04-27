Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic

Namespace WholeFoods.IRMA.CompetitorStore.DataAccess
    Public Class CompetitorLocation

        Public Shared Sub List(ByRef dataSet As CompetitorStoreDataSet)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Using adapter As SqlClient.SqlDataAdapter = factory.GetDataAdapter("GetCompetitorLocations", Nothing)
                adapter.Fill(dataSet.CompetitorLocation)
            End Using
        End Sub

    End Class
End Namespace