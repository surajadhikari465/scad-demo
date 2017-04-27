Imports Microsoft.VisualBasic

Public Class NewItemPrice

    Public Shared Function InsertIntoIrmaPrice(ByVal dt As NewItemRequest.ItemRequestDataTable)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim al As ArrayList = Nothing
        Dim paramList As New ArrayList
        ' ******* Parameters ************
        currentParam = New DBParam
        currentParam.Name = "ItemKey"
        currentParam.Value = dt.Rows(0).Item("Item_Key")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "User_ID"
        currentParam.Value = DBNull.Value
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "User_ID_Date"
        currentParam.Value = DBNull.Value
        currentParam.Type = DBParamType.DateTime
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "Store_No"
        currentParam.Value = dt.Rows(0).Item("Store_No")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "StartDate"
        currentParam.Value = DBNull.Value
        currentParam.Type = DBParamType.Decimal
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "Multiple"
        currentParam.Value = DBNull.Value
        currentParam.Type = DBParamType.Decimal
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "POSPrice"
        currentParam.Value = dt.Rows(0).Item("PackSize")
        currentParam.Type = DBParamType.Decimal
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Price"
        currentParam.Value = DBNull.Value
        currentParam.Type = DBParamType.DateTime
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "OldStartDate"
        currentParam.Value = DBNull.Value
        currentParam.Type = DBParamType.DateTime
        paramList.Add(currentParam)
        ' ************************************
        ' Execute the stored procedure 
        Try
            al = factory.ExecuteStoredProcedure("UpdatePriceBatchDetailReg", paramList)
        Catch ex As Exception
            Debug.WriteLine("ERROR " & ex.Message)
        End Try
        Return al
    End Function


End Class
