Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Pricing.DataAccess

    Public Class ItemPriceBatchDetailDAO

        Friend Shared Function GetPriceBatchDetailPrintRequestData(ByVal priceBatchHeaderID As Integer, ByVal itemList As String, ByVal itemListSeparator As Char) As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            currentParam = New DBParam
            currentParam.Name = "PriceBatchHeaderID"
            currentParam.Value = priceBatchHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ItemList"
            currentParam.Value = itemList
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ItemListSeparator"
            currentParam.Value = itemListSeparator
            currentParam.Type = DBParamType.Char
            paramList.Add(currentParam)

            Return factory.GetStoredProcedureDataTable("GetPriceBatchDetailForItemKeys", paramList)
        End Function
    End Class
End Namespace
