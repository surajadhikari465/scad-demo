Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic

Namespace WholeFoods.IRMA.CompetitorStore.DataAccess
    Public Class CompetitorStore

#Region "Helper Methods"

        Private Shared Function CreateValueParameter(ByVal name As String, ByVal value As Object, ByVal type As SqlDbType) As SqlParameter
            Dim param As New SqlParameter(name, type)

            param.Value = value

            Return param
        End Function

        Private Shared Function GetNullableIntParam(ByVal name As String, ByVal value As Nullable(Of Integer)) As SqlParameter
            Dim objectValue As Object

            If value.HasValue Then
                objectValue = value.Value
            Else
                objectValue = DBNull.Value
            End If

            Return CreateValueParameter(name, objectValue, SqlDbType.Int)
        End Function

#End Region

        Public Sub Search(ByVal dataSet As CompetitorStoreDataSet, ByVal competitorID As Nullable(Of Integer), ByVal competitorLocationID As Nullable(Of Integer))
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim command As SqlCommand = factory.GetDataCommand("GetCompetitorStoreSearch", Nothing, True)

            command.Parameters.Add(GetNullableIntParam("CompetitorID", competitorID))
            command.Parameters.Add(GetNullableIntParam("CompetitorLocationID", competitorLocationID))

            Using adapter As SqlClient.SqlDataAdapter = factory.GetDataAdapter(command, Nothing)
                adapter.Fill(dataSet.CompetitorStore)
            End Using
        End Sub

        Public Shared Function GetByName(ByVal dataSet As CompetitorStoreDataSet, ByVal competitorName As String, ByVal competitorLocationName As String, ByVal competitorStoreName As String) As CompetitorStoreDataSet.CompetitorStoreRow
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim command As SqlCommand = factory.GetDataCommand("GetCompetitorStoreByName", Nothing, True)
            Dim store As CompetitorStoreDataSet.CompetitorStoreRow = Nothing
            Dim view As DataView

            command.Parameters.Add(CreateValueParameter("CompetitorName", competitorName, SqlDbType.VarChar))
            command.Parameters.Add(CreateValueParameter("CompetitorLocationName", competitorLocationName, SqlDbType.VarChar))
            command.Parameters.Add(CreateValueParameter("CompetitorStoreName", competitorStoreName, SqlDbType.VarChar))

            Using adapter As SqlClient.SqlDataAdapter = factory.GetDataAdapter(command, Nothing)
                adapter.TableMappings.Add("Table", "Competitor")
                adapter.TableMappings.Add("Table1", "CompetitorLocation")
                adapter.TableMappings.Add("Table2", "CompetitorStore")

                adapter.Fill(dataSet)
            End Using

            view = New DataView(dataSet.CompetitorStore)

            view.RowFilter = String.Format("Parent({0}).Name = '{0}' AND Parent({1}).Name = '{1}' AND Name = '{2}'", _
                dataSet.CompetitorStore.ParentRelations(0).RelationName, dataSet.CompetitorStore.ParentRelations(1).RelationName, competitorName, competitorLocationName, competitorStoreName)

            If view.Count = 1 Then
                store = CType(view(0).Row, CompetitorStoreDataSet.CompetitorStoreRow)
            End If

            Return store
        End Function

    End Class
End Namespace