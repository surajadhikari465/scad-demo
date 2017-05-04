Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic
Imports WholeFoods.IRMA.ItemChaining.DataAccess

Namespace WholeFoods.IRMA.CompetitorStore.DataAccess
    Public Class Price
        Public SearchHelper As New PriceSearchHelper

#Region "Helper Class"

        Public Class PriceSearchHelper

#Region "Member Variables"

            Public ItemSearchHelper As New Item.ItemSearchHelper
            Public Store_No As New Nullable(Of Integer)
            Public CompetitivePriceTypeID As New Nullable(Of Integer)

#End Region

#Region "Helper Methods"

            Private Function CreateValueParameter(ByVal name As String, ByVal type As SqlDbType, ByVal value As Object) As SqlParameter
                Dim param As New SqlParameter(name, type)

                param.Value = value

                Return param
            End Function

#End Region

            Public Sub AddParameters(ByVal parameters As SqlParameterCollection)
                ItemSearchHelper.AddOnlyNonDefaultParameters(parameters)
                
                With parameters
                    If Store_No.HasValue Then
                        .Add(CreateValueParameter("Store_No", SqlDbType.Int, Store_No.Value))
                        Store_No = Nothing
                    End If

                    If CompetitivePriceTypeID.HasValue Then
                        .Add(CreateValueParameter("CompetitivePriceTypeID", SqlDbType.Int, CompetitivePriceTypeID.Value))
                        CompetitivePriceTypeID = Nothing
                    End If
                End With
            End Sub

        End Class

#End Region

#Region "Helper Methods"

        Private Function CreateUpdateCompetitivePriceInfoCommand(ByVal factory As DataFactory, ByVal transaction As SqlTransaction) As SqlCommand
            Dim command As SqlCommand = factory.GetDataCommand("UpdateCompetitivePriceInfo", transaction, True)

            With command.Parameters
                .Add(New SqlParameter("Item_Key", SqlDbType.Int, 0, "Item_Key"))
                .Add(New SqlParameter("Store_No", SqlDbType.Int, 0, "Store_No"))
                .Add(New SqlParameter("CompetitivePriceTypeID", SqlDbType.Int, 0, "CompetitivePriceTypeId"))
                .Add(New SqlParameter("BandwidthPercentageHigh", SqlDbType.TinyInt, 0, "BandwidthPercentageHigh"))
                .Add(New SqlParameter("BandwidthPercentageLow", SqlDbType.TinyInt, 0, "BandwidthPercentageLow"))
            End With

            Return command
        End Function

#End Region

        Public Sub Save(ByVal dataSet As NPVDataSet)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim command As SqlCommand = Nothing
            Dim transaction As SqlTransaction = Nothing

            Using adapter As SqlDataAdapter = factory.GetDataAdapter(command, transaction)
                With adapter
                    .UpdateCommand = CreateUpdateCompetitivePriceInfoCommand(factory, transaction)
                    .Update(dataSet.CompetitivePriceInfo)
                End With
            End Using
        End Sub

        Public Sub Search(ByVal dataSet As NPVDataSet)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim cmd As SqlCommand = factory.GetDataCommand("GetPriceSearch", Nothing, True)

            Me.SearchHelper.AddParameters(cmd.Parameters)

            dataSet.CompetitivePriceInfo.Rows.Clear()

            Using adapter As SqlDataAdapter = factory.GetDataAdapter(cmd, Nothing)
                adapter.Fill(dataSet.CompetitivePriceInfo)
            End Using
        End Sub

    End Class
End Namespace