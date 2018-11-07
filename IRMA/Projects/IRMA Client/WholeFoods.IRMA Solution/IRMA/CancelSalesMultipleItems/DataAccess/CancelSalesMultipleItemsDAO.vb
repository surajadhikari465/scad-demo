Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient

Public Class CancelSalesMultipleItemsDAO
    Public Function ValidateCancelAllSalesData(ByVal storeItemInfoDataTable As DataTable) As DataSet

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim sqlConnection As SqlConnection = New SqlConnection()
        sqlConnection.ConnectionString = factory.ConnectString

        Dim storeItemInfoCommand As New SqlCommand("ValidateCancelAllSalesData", sqlConnection)
        Dim sqlParamter As SqlParameter = storeItemInfoCommand.Parameters.Add("@StoreNoItemIdentiferData", SqlDbType.Structured)

        storeItemInfoCommand.CommandType = CommandType.StoredProcedure
        sqlParamter.Value = storeItemInfoDataTable

        Dim sqlAdapter As SqlDataAdapter = New SqlDataAdapter(storeItemInfoCommand)
        Dim returnedDataset As New DataSet

        sqlAdapter.Fill(returnedDataset)

        Return returnedDataset
    End Function
End Class