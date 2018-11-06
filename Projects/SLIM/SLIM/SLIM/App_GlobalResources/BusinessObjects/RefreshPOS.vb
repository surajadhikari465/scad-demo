Imports Microsoft.VisualBasic
Imports System.Data.SqlClient

Public Class RefreshPOS

#Region "Private Members"
    Private paramList As New ArrayList
    Private df As New DataFactory(DataFactory.ItemCatalog)
    Dim currentParam As New DBParam
#End Region

#Region "Constructor"
    Sub New()
    End Sub
#End Region

#Region "Public Methods"

    Public Sub UpdateStoreItemRefresh(ByVal Item_Key As Integer, ByVal Store_No As Integer, ByVal Refresh As Boolean, ByVal Reason As String, ByVal UserID As Integer)
        paramList.Clear()
        ' ***** Parameters ********
        currentParam = New DBParam
        currentParam.Name = "Item_Key"
        currentParam.Value = Item_Key
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Store_No"
        currentParam.Value = Store_No
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Refresh"
        currentParam.Value = Refresh
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Reason"
        currentParam.Value = Reason
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "UserID"
        currentParam.Value = UserID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *********************************
        df.ExecuteStoredProcedure("UpdateStoreItemRefresh", paramList)
    End Sub

    Public Function GetStoreItemRefresh(ByVal ItemKey As Integer, ByVal Store_No As Integer) As DataTable
        Dim results As SqlDataReader = Nothing
        Dim row As DataRow
        Dim table As New DataTable
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = ItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            results = df.GetStoredProcedureDataReader("GetStoreItemRefresh", paramList)

            'add columns to table
            table.Columns.Add(New DataColumn("Store_No", GetType(Int32)))
            table.Columns.Add(New DataColumn("Store_Name", GetType(String)))
            table.Columns.Add(New DataColumn("Refresh", GetType(Boolean)))
            table.Columns.Add(New DataColumn("StoreItemVendorID", GetType(Int32)))
            table.Columns.Add(New DataColumn("Authorized", GetType(Boolean)))
            table.Columns.Add(New DataColumn("StoreJurisdictionID", GetType(Int32)))

            While results.Read
                'This filters down the dataset based on whether or not all stores or one store is selected.
                If Store_No = 0 Or Store_No = results.GetInt32(results.GetOrdinal("Store_No")) Then
                    row = table.NewRow
                    row("Store_No") = results.GetInt32(results.GetOrdinal("Store_No"))
                    row("Store_Name") = results.GetString(results.GetOrdinal("Store_Name"))
                    row("Refresh") = results.GetBoolean(results.GetOrdinal("Refresh"))
                    row("Authorized") = results.GetBoolean(results.GetOrdinal("Authorized"))

                    If Not results.IsDBNull(results.GetOrdinal("StoreItemVendorID")) Then
                        row("StoreItemVendorID") = results.GetInt32(results.GetOrdinal("StoreItemVendorID"))
                    End If

                    row("StoreJurisdictionID") = results.GetInt32(results.GetOrdinal("StoreJurisdictionID"))
                    table.Rows.Add(row)
                End If

            End While
        Finally
            If results IsNot Nothing Then
                results.Close()
            End If
        End Try

        Return table
    End Function
#End Region

End Class
