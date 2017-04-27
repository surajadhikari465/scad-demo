Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic

Namespace WholeFoods.IRMA.CompetitorStore.DataAccess
    Public Class ItemUnit

        Public Shared Sub List(ByVal dataSet As CompetitorStoreDataSet)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim command As SqlCommand = factory.GetDataCommand("GetItemUnits", Nothing, True)
            
            Using adapter As SqlDataAdapter = factory.GetDataAdapter(command, Nothing)
                adapter.Fill(dataSet.ItemUnit)
            End Using
        End Sub
    End Class
End Namespace