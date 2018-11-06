Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.Writers.Constants

Public Class StoreInfoDAO
    Private factory As DataAccess.DataFactory

    Private Sub CreateDatabaseConnection()
        If factory Is Nothing Then
            factory = New DataAccess.DataFactory(DataAccess.DataFactory.ItemCatalog)
        End If
    End Sub

    Public Sub New()
        CreateDatabaseConnection()
    End Sub

    Public Function GetStorePOSPullInfo() As List(Of StoreInfo)
        Dim storeInformation As List(Of StoreInfo) = New List(Of StoreInfo)
        'setup parameters for stored proc
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "FileWriterType"
        currentParam.Value = FileWriterType_POSPULL
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        Dim ds As DataSet = factory.GetStoredProcedureDataSet("Replenishment_POSPush_GetFTPConfigForWriterType", paramList)

        For Each dr As DataRow In ds.Tables(0).Rows
            Dim store As StoreInfo = New StoreInfo
            store.StoreName = dr("Store_Name").ToString
            store.StoreNo = CInt(dr("Store_No"))
            store.StoreAbbr = dr("Storeabbr").ToString
            storeInformation.Add(store)
        Next

        Return storeInformation


    End Function
End Class
