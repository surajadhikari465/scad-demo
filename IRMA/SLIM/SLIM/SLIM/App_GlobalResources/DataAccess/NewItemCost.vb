Imports Microsoft.VisualBasic

Public Class NewItemCost

    Public Shared Function InsertIntoIrmaCost(ByVal dt As NewItemRequest.ItemRequestDataTable)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim al As ArrayList = Nothing
        Dim paramList As New ArrayList
        ' ******* Parameters ************
        currentParam = New DBParam
        currentParam.Name = "StoreList"
        currentParam.Value = dt.Rows(0).Item("User_Store")
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "StoreListSeparator"
        currentParam.Value = "|"
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "Item_Key"
        currentParam.Value = dt.Rows(0).Item(currentParam.Name)
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "Vendor_ID"
        currentParam.Value = dt.Rows(0).Item(currentParam.Name)
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "UnitCost"
        currentParam.Value = DBNull.Value
        currentParam.Type = DBParamType.Decimal
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "UnitFreight"
        currentParam.Value = DBNull.Value
        currentParam.Type = DBParamType.Decimal
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Package_Desc1"
        currentParam.Value = dt.Rows(0).Item("PackSize")
        currentParam.Type = DBParamType.Decimal
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "StartDate"
        currentParam.Value = DBNull.Value
        currentParam.Type = DBParamType.DateTime
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "EndDate"
        currentParam.Value = DBNull.Value
        currentParam.Type = DBParamType.DateTime
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Promotional"
        currentParam.Value = False
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "MSRP"
        currentParam.Value = DBNull.Value
        currentParam.Type = DBParamType.Decimal
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "FromVendor"
        currentParam.Value = True
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)
        ' ************************************
        ' ************************************
        ' Execute the stored procedure 
        Try
            al = factory.ExecuteStoredProcedure("InsertVendorCostHistory", paramList)
        Catch ex As Exception
            Debug.WriteLine("ERROR " & ex.Message)
        End Try
        Return al
    End Function

End Class
