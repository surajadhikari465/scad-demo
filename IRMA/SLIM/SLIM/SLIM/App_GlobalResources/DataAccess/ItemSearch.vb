Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Collections.Generic

Public Class ItemSearch

#Region "Helper Methods"

    Private Shared Function GetNamesStringArray(ByVal procedureName As String, ByVal params As ArrayList) As String()
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim names As New List(Of String)

        Using reader As SqlDataReader = factory.GetStoredProcedureDataReader(procedureName, params)
            While reader.Read()
                names.Add(reader.GetString(reader.GetOrdinal("Value")))
            End While

            reader.Close()
        End Using

        Return names.ToArray()
    End Function

    Private Shared Function GetIDByName(ByVal name As String, ByVal storedProcedureName As String, ByVal parameterName As String, ByVal idColumnName As String) As Nullable(Of Integer)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim foundId As New Nullable(Of Integer)
        Dim params As New ArrayList()
        Dim currentParam As New DBParam()

        currentParam.Name = parameterName
        currentParam.Type = DBParamType.String
        currentParam.Value = name

        params.Add(currentParam)

        Using reader As SqlDataReader = factory.GetStoredProcedureDataReader(storedProcedureName, params)
            If reader.HasRows AndAlso reader.Read() Then
                foundId = reader.GetInt32(reader.GetOrdinal(idColumnName))
            Else
                foundId = 0
            End If
        End Using

        Return foundId
    End Function

#End Region

#Region "Autocompletion"

    Public Shared Function GetBrandIDByName(ByVal brandName As String) As Nullable(Of Integer)
        Return GetIDByName(brandName, "GetBrand_ByNameExact", "Brand_Name", "Brand_ID")
    End Function

    Public Shared Function GetVendorIDByName(ByVal vendorCompanyName As String) As Nullable(Of Integer)
        Return GetIDByName(vendorCompanyName, "GetVendor_ByCompanyNameExact", "CompanyName", "Vendor_ID")
    End Function

    Public Shared Function GetTaxClassIDByName(ByVal taxClassName As String) As Nullable(Of Integer)
        Return GetIDByName(taxClassName, "GetTaxClass_ByDescExact", "TaxClassDesc", "TaxClassID")
    End Function

    Public Shared Function GetBrandNamesByStartsWith(ByVal startsWith As String, ByVal subTeam_No As Integer) As String()
        Dim params As New ArrayList()
        Dim currentParam As New DBParam()

        currentParam.Name = "Start"
        currentParam.Type = DBParamType.String
        currentParam.Value = startsWith

        params.Add(currentParam)

        If subTeam_No > 0 Then
            currentParam = New DBParam()
            currentParam.Name = "SubTeam_No"
            currentParam.Type = DBParamType.Int
            currentParam.Value = subTeam_No

            params.Add(currentParam)
        End If

        Return GetNamesStringArray("GetBrands_ByNameStartsWith", params)
    End Function

    Public Shared Function GetVendorNamesByStartsWith(ByVal startsWith As String) As String()
        Dim params As New ArrayList()
        Dim currentParam As New DBParam()

        currentParam.Name = "Start"
        currentParam.Type = DBParamType.String
        currentParam.Value = startsWith

        params.Add(currentParam)

        Return GetNamesStringArray("GetVendor_ByCompanyNameStartsWith", params)
    End Function

    Public Shared Function GetTaxClassNamesByStartsWith(ByVal startsWith As String) As String()
        Dim params As New ArrayList()
        Dim currentParam As New DBParam()

        currentParam.Name = "Start"
        currentParam.Type = DBParamType.String
        currentParam.Value = startsWith

        params.Add(currentParam)

        Return GetNamesStringArray("GetTaxClass_ByTaxClassDescStartsWith", params)
    End Function

#End Region

#Region "Hierarchy"

    Public Shared Function GetAllSubTeamsReader() As SqlDataReader
        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        Return factory.GetStoredProcedureDataReader("GetAllSubTeams")
    End Function

    Public Shared Function GetUserSubTeamsReader(ByVal userID As Integer) As SqlDataReader
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim params As New ArrayList()
        Dim currentParam As New DBParam()

        currentParam.Name = "User_ID"
        currentParam.Type = DBParamType.Int
        currentParam.Value = userID

        params.Add(currentParam)

        Return factory.GetStoredProcedureDataReader("GetUserStoreTeamTitles_ByUser_ID", params)
    End Function

    Public Shared Function GetCategoriesBySubTeamReader(ByVal subTeam_No As Integer) As SqlDataReader
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim params As New ArrayList()
        Dim currentParam As New DBParam()

        currentParam.Name = "SubTeam_No"
        currentParam.Type = DBParamType.Int
        currentParam.Value = subTeam_No

        params.Add(currentParam)

        Return factory.GetStoredProcedureDataReader("GetCategoriesBySubTeam", params)
    End Function

    Public Shared Function GetProdHierarchyLevel3sByCategoryReader(ByVal categoryID As Integer) As SqlDataReader
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim params As New ArrayList()
        Dim currentParam As New DBParam()

        currentParam.Name = "Category_ID"
        currentParam.Type = DBParamType.Int
        currentParam.Value = categoryID

        params.Add(currentParam)

        Return factory.GetStoredProcedureDataReader("GetProdHierarchyLevel3sByCategory", params)
    End Function

    Public Shared Function GetProdHierarchyLevel4sByLevel3Reader(ByVal level3ID As Integer) As SqlDataReader
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim params As New ArrayList()
        Dim currentParam As New DBParam()

        currentParam.Name = "ProdHierarchyLevel3_ID"
        currentParam.Type = DBParamType.Int
        currentParam.Value = level3ID

        params.Add(currentParam)

        Return factory.GetStoredProcedureDataReader("GetProdHierarchyLevel4sByLevel3", params)
    End Function

#End Region

    Public Shared Function GetItemWebQuery(ByVal ht As Hashtable) As DataSet
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim paramList As New ArrayList
        Dim ds As New DataSet
        Dim dt As New DataTable
        ds.Tables.Add(dt)
        ' ******* Parameters ************
        currentParam = New DBParam
        currentParam.Name = "Identifier"
        currentParam.Value = ht.Item("Identifier")
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "Item_Description"
        currentParam.Value = ht.Item("Item_Description")
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "SubTeam_No"
        currentParam.Value = ht.Item("SubTeam_No")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *************************************
        currentParam = New DBParam
        currentParam.Name = "Brand_ID"
        currentParam.Value = ht.Item("Brand_ID")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *************************************
        ' Execute the stored procedure 
        Try
            ds = factory.GetStoredProcedureDataSet("GetItemWebQuery", paramList)
        Catch ex As Exception
            Debug.WriteLine("ERROR " & ex.Message)
        End Try
        Return ds
    End Function

    Public Shared Function GetItemDetails(ByVal item_key As Integer, ByVal storeJurisdictionId As Integer) As DataRow
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As New DBParam()
        Dim paramList As New ArrayList
        Dim results As DataSet
        Dim details As DataRow = Nothing

        currentParam.Name = "Item_Key"
        currentParam.Type = DBParamType.Int
        currentParam.Value = item_key

        paramList.Add(currentParam)

        currentParam = New DBParam()
        currentParam.Name = "StoreJurisdictionId"
        currentParam.Type = DBParamType.Int
        currentParam.Value = storeJurisdictionId

        paramList.Add(currentParam)

        results = factory.GetStoredProcedureDataSet("ItemWebQueryDetailShort", paramList)

        If results.Tables.Count > 0 AndAlso results.Tables(0).Rows.Count > 0 Then
            details = results.Tables(0).Rows(0)
        End If

        Return details
    End Function

End Class
