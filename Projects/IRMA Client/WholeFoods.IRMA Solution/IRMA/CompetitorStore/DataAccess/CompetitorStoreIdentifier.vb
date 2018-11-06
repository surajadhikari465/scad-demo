Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic

Namespace WholeFoods.IRMA.CompetitorStore.DataAccess
    Public Class CompetitorStoreIdentifier

#Region "Helper Methods"

        Private Shared Function CreateInsertCompetitorStoreIdentifierCommand(ByVal factory As DataFactory, ByVal transaction As SqlTransaction) As SqlCommand
            Dim command As SqlCommand = factory.GetDataCommand("InsertCompetitorStoreIdentifier", transaction, True)

            With command.Parameters
                .Add(New SqlParameter("CompetitorStoreID", SqlDbType.Int, 0, "CompetitorStoreID"))
                .Add(New SqlParameter("Identifier", SqlDbType.VarChar, 50, "Identifier"))
            End With

            Return command
        End Function

#End Region

        Public Shared Sub Save(ByRef dataSet As CompetitorStoreDataSet)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim command As SqlCommand = Nothing
            Dim transaction As SqlTransaction = Nothing

            Using adapter As SqlDataAdapter = factory.GetDataAdapter(command, transaction)
                adapter.InsertCommand = CreateInsertCompetitorStoreIdentifierCommand(factory, transaction)

                ' It's possible (though unlikely) that the competitor store identifier has been created
                ' since the initial import step, so here we'll ignore the unique constraint errors that
                ' might come up and be satisfied that they exist in the database
                adapter.ContinueUpdateOnError = True
                adapter.Update(dataSet.CompetitorStoreIdentifier)
            End Using
        End Sub

    End Class
End Namespace