Imports Microsoft.VisualBasic

Public Class LockAuthCheck

    Public Shared Function IsAuthLockedByJurisdiction(ByVal item_Key As Integer) As DataTable
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim dataTable As New DataTable
        Dim row As DataRow
        Dim dataReader As SqlClient.SqlDataReader = Nothing
        Dim currentParam As DBParam
        Dim paramList As New ArrayList

        currentParam = New DBParam
        currentParam.Name = "Item_Key"
        currentParam.Value = item_Key
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        dataReader = factory.GetStoredProcedureDataReader("CheckItemAuthLock", paramList)

        dataTable.Columns.Add(New DataColumn("JurisdictionId", GetType(Int32)))
        dataTable.Columns.Add(New DataColumn("JurisdictionDescription", GetType(String)))
        dataTable.Columns.Add(New DataColumn("LockAuth", GetType(Boolean)))

        While dataReader.Read
            row = dataTable.NewRow
            row("JurisdictionId") = dataReader.GetInt32(dataReader.GetOrdinal("StoreJurisdictionID"))
            row("JurisdictionDescription") = dataReader.GetString(dataReader.GetOrdinal("StoreJurisdictionDesc"))
            row("LockAuth") = dataReader.GetBoolean(dataReader.GetOrdinal("LockAuth"))

            dataTable.Rows.Add(row)
        End While

        Return dataTable
        'Return CBool(factory.ExecuteScalar(String.Format("CheckItemAuthLock @Item_Key = {0}", item_Key)))
    End Function

    Public Shared Function IsAuthLocked(ByVal item_Key As Integer) As Boolean
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Return CBool(factory.ExecuteScalar(String.Format("CheckItemAuthLock @Item_Key = {0}", item_Key)))
    End Function

End Class