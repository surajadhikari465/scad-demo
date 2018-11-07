Imports Microsoft.VisualBasic

Public Class RejectStoreSpecial

    Public Function GetRejectedItemInfo(ByVal sISSRejectId As String) As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim params As New ArrayList()
        Dim currentParam As New DBParam()
        Dim ds As DataSet
        Dim sData As String = ""

        currentParam.Name = "RequestIdList"
        currentParam.Type = DBParamType.String
        currentParam.Value = sISSRejectId
        params.Add(currentParam)

        Try
            ds = factory.GetStoredProcedureDataSet("SLIM_GetItemRejectInfo", params)

            Return ds
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
