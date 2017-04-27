Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic

Namespace WholeFoods.IRMA.CompetitorStore.DataAccess
    Public Class StoreCompetitorStore

#Region "Helper Methods"

        Private Shared Function CreateInsertStoreCompetitorStoreCommand(ByVal factory As DataFactory, ByVal transaction As SqlTransaction) As SqlCommand
            Dim command As SqlCommand = factory.GetDataCommand("InsertStoreCompetitorStore", transaction, True)

            With command.Parameters
                .Add(New SqlParameter("Store_No", SqlDbType.Int, 0, "Store_No"))
                .Add(New SqlParameter("CompetitorStoreID", SqlDbType.Int, 0, "CompetitorStoreID"))
                .Add(New SqlParameter("Priority", SqlDbType.TinyInt, 0, "Priority"))
            End With

            Return command
        End Function

        Private Shared Function CreateUpdateStoreCompetitorStoreCommand(ByVal factory As DataFactory, ByVal transaction As SqlTransaction) As SqlCommand
            Dim command As SqlCommand = factory.GetDataCommand("UpdateStoreCompetitorStore", transaction, True)

            With command.Parameters
                .Add(New SqlParameter("Store_No", SqlDbType.Int, 0, "Store_No"))
                .Add(New SqlParameter("CompetitorStoreID", SqlDbType.Int, 0, "CompetitorStoreID"))
                .Add(New SqlParameter("Priority", SqlDbType.TinyInt, 0, "Priority"))
            End With

            Return command
        End Function

        Private Shared Function CreateDeleteStoreCompetitorStoreCommand(ByVal factory As DataFactory, ByVal transaction As SqlTransaction) As SqlCommand
            Dim command As SqlCommand = factory.GetDataCommand("DeleteStoreCompetitorStore", transaction, True)

            With command.Parameters
                .Add(New SqlParameter("Store_No", SqlDbType.Int, 0, "Store_No"))
                .Add(New SqlParameter("CompetitorStoreID", SqlDbType.Int, 0, "CompetitorStoreID"))
            End With

            Return command
        End Function

#End Region

        Public Shared Sub GetByStore_No(ByVal dataSet As CompetitorStoreDataSet, ByVal store_no As Integer)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim command As SqlCommand = factory.GetDataCommand("GetStoreCompetitorStoresByStore_No", Nothing, True)
            Dim param As New SqlParameter("Store_No", SqlDbType.Int)

            param.Value = store_no

            command.Parameters.Add(param)

            Using adapter As SqlClient.SqlDataAdapter = factory.GetDataAdapter(command, Nothing)
                adapter.Fill(dataSet.StoreCompetitorStore)
            End Using
        End Sub

        Public Shared Sub Save(ByVal dataSet As CompetitorStoreDataSet)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim command As SqlCommand = Nothing
            Dim transaction As SqlTransaction = Nothing

            Using adapter As SqlDataAdapter = factory.GetDataAdapter(command, transaction)
                adapter.InsertCommand = CreateInsertStoreCompetitorStoreCommand(factory, transaction)
                adapter.UpdateCommand = CreateUpdateStoreCompetitorStoreCommand(factory, transaction)
                adapter.DeleteCommand = CreateDeleteStoreCompetitorStoreCommand(factory, transaction)

                adapter.Update(dataSet.StoreCompetitorStore)
            End Using
        End Sub

    End Class
End Namespace