Imports Microsoft.VisualBasic
Imports SLIM.WholeFoods.Utility
Imports SLIM.WholeFoods.Utility.DataAccess
Imports SLIM.WholeFoods.Utility.SMTP


Public Class NewItem

    Public Shared Function InsertItemIntoIrma(ByVal dt As NewItemRequest.ItemRequestDataTable)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim al As ArrayList = Nothing
        Dim paramList As New ArrayList
        ' ******* Parameters ************
        currentParam = New DBParam
        currentParam.Name = "POS_Description"
        currentParam.Value = dt.Rows(0).Item(currentParam.Name)
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "Item_Description"
        currentParam.Value = dt.Rows(0).Item(currentParam.Name)
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "SubTeam_No"
        currentParam.Value = dt.Rows(0).Item(currentParam.Name)
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "Category_ID"
        currentParam.Value = dt.Rows(0).Item(currentParam.Name)
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "Retail_Unit_ID"
        currentParam.Value = dt.Rows(0).Item("ItemUnit")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "Package_Unit_ID"
        currentParam.Value = dt.Rows(0).Item("ItemUnit")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Package_Desc1"
        currentParam.Value = dt.Rows(0).Item("PackSize")
        currentParam.Type = DBParamType.Decimal
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Package_Desc2"
        currentParam.Value = dt.Rows(0).Item("ItemSize")
        currentParam.Type = DBParamType.Decimal
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "IdentifierType"
        currentParam.Value = "B"
        currentParam.Type = DBParamType.Char
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Identifier"
        currentParam.Value = dt.Rows(0).Item(currentParam.Name)
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "CheckDigit"
        currentParam.Value = DBNull.Value
        currentParam.Type = DBParamType.Char
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "User_ID"
        currentParam.Value = dt.Rows(0).Item(currentParam.Name)
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Retail_Sale"
        currentParam.Value = True
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "ClassID"
        currentParam.Value = dt.Rows(0).Item(currentParam.Name)
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "CostedByWeight"
        currentParam.Value = False
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Vendor_Unit_ID"
        currentParam.Value = dt.Rows(0).Item("ItemUnit")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Distribution_Unit_ID"
        currentParam.Value = dt.Rows(0).Item("ItemUnit")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Cost_Unit_ID"
        currentParam.Value = dt.Rows(0).Item("ItemUnit")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Freight_Unit_ID"
        currentParam.Value = dt.Rows(0).Item("ItemUnit")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "TaxClassID"
        currentParam.Value = dt.Rows(0).Item("TaxClass_ID")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "LabelType_ID"
        currentParam.Value = DBNull.Value
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        ' Execute the stored procedure 
        Try
            al = factory.ExecuteStoredProcedure("InsertItem", paramList)
        Catch ex As Exception
            Debug.WriteLine("ERROR " & ex.Message)
        End Try
        Return al
    End Function

    Public Shared Function UpdateItemIrma(ByVal dt As NewItemRequest.ItemRequestDataTable)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim al As ArrayList = Nothing
        Dim paramList As New ArrayList
        ' ******* Parameters ************
        currentParam = New DBParam
        currentParam.Name = "POS_Description"
        currentParam.Value = dt.Rows(0).Item(currentParam.Name)
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "Item_Description"
        currentParam.Value = dt.Rows(0).Item(currentParam.Name)
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "SubTeam_No"
        currentParam.Value = dt.Rows(0).Item(currentParam.Name)
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "Category_ID"
        currentParam.Value = dt.Rows(0).Item(currentParam.Name)
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "Retail_Unit_ID"
        currentParam.Value = dt.Rows(0).Item("ItemUnit")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "Package_Unit_ID"
        currentParam.Value = dt.Rows(0).Item("ItemUnit")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Package_Desc1"
        currentParam.Value = dt.Rows(0).Item("PackSize")
        currentParam.Type = DBParamType.Decimal
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Package_Desc2"
        currentParam.Value = dt.Rows(0).Item("ItemSize")
        currentParam.Type = DBParamType.Decimal
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "IdentifierType"
        currentParam.Value = "B"
        currentParam.Type = DBParamType.Char
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Identifier"
        currentParam.Value = dt.Rows(0).Item(currentParam.Name)
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "CheckDigit"
        currentParam.Value = DBNull.Value
        currentParam.Type = DBParamType.Char
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "User_ID"
        currentParam.Value = dt.Rows(0).Item(currentParam.Name)
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Retail_Sale"
        currentParam.Value = True
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "ClassID"
        currentParam.Value = dt.Rows(0).Item(currentParam.Name)
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "CostedByWeight"
        currentParam.Value = False
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Vendor_Unit_ID"
        currentParam.Value = dt.Rows(0).Item("ItemUnit")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Distribution_Unit_ID"
        currentParam.Value = dt.Rows(0).Item("ItemUnit")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Cost_Unit_ID"
        currentParam.Value = dt.Rows(0).Item("ItemUnit")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Freight_Unit_ID"
        currentParam.Value = dt.Rows(0).Item("ItemUnit")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "TaxClassID"
        currentParam.Value = dt.Rows(0).Item("TaxClass_ID")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "LabelType_ID"
        currentParam.Value = DBNull.Value
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        ' Execute the stored procedure 
        Try
            al = factory.ExecuteStoredProcedure("UpdateItemInfo", paramList)
        Catch ex As Exception
            Debug.WriteLine("ERROR " & ex.Message)
        End Try
        Return al
    End Function

End Class
