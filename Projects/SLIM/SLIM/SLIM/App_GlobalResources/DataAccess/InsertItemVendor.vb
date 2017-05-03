Imports Microsoft.VisualBasic

Public Class InsertItemVendor

    Public Shared Function InsertIntoIrmaItemVendor(ByVal itemKey As Integer, ByVal vendorID As Integer)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim al As ArrayList = Nothing
        Dim paramList As New ArrayList
        ' ******* Parameters ************
        currentParam = New DBParam
        currentParam.Name = "Item_Key"
        currentParam.Value = itemKey
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "Vendor_ID"
        currentParam.Value = vendorID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        ' Execute the stored procedure 
        Try
            al = factory.ExecuteStoredProcedure("InsertItemVendor", paramList)
        Catch ex As Exception
            Debug.WriteLine("ERROR " & ex.Message)
        End Try
        Return al
    End Function

    Public Shared Function UpdateIrmaItemVendor(ByVal itemKey As Integer, ByVal vendorID As Integer, ByVal itemID As String)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim al As ArrayList = Nothing
        Dim paramList As New ArrayList
        ' ******* Parameters ************
        currentParam = New DBParam
        currentParam.Name = "Item_Key"
        currentParam.Value = itemKey
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "Vendor_ID"
        currentParam.Value = vendorID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' ************************************
        currentParam = New DBParam
        currentParam.Name = "Item_ID"
        currentParam.Value = itemID
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)
        ' ************************************
        ' Execute the stored procedure 
        Try
            al = factory.ExecuteStoredProcedure("UpdateItemVendor", paramList)
        Catch ex As Exception
            Debug.WriteLine("ERROR " & ex.Message)
        End Try
        Return al
    End Function

End Class
