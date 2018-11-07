Imports Microsoft.VisualBasic

Public Class NewItemVendor

    Public Shared Function InsertIntoIrmaVendor(ByVal dt As DataTable)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim al As ArrayList = Nothing
        Dim paramList As New ArrayList
        ' ******* Parameters ************
        currentParam = New DBParam
        currentParam.Name = "CompanyName"
        currentParam.Value = dt.Rows(0).Item("CompanyName")
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "PS_Vendor_ID"
        currentParam.Value = dt.Rows(0).Item("PS_Vendor_ID")
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "PS_Address_Sequence"
        currentParam.Value = dt.Rows(0).Item("PS_Address_Sequence")
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)
        ' *************************************
        ' Execute the stored procedure 
        Try
            al = factory.ExecuteStoredProcedure("InsertVendor", paramList)
        Catch ex As Exception
            Debug.WriteLine("ERROR " & ex.Message)
        End Try
        Return al
    End Function


End Class
