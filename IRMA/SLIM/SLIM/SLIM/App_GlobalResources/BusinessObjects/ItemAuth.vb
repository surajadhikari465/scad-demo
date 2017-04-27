Imports Microsoft.VisualBasic
Imports System.Data.SqlClient

Public Class ItemAuth

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

    Public Sub SetPrimaryVendor(ByVal store As String, _
    ByVal Itemkey As Integer, ByVal VendorID As Integer)
        paramList.Clear()
        ' ***** Parameters ********
        currentParam = New DBParam
        currentParam.Name = "Store_No"
        currentParam.Value = store
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)
        currentParam = New DBParam
        currentParam.Name = "Item_Key"
        currentParam.Value = Itemkey
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        currentParam = New DBParam
        currentParam.Name = "Vendor_ID"
        currentParam.Value = VendorID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *********************************
        df.ExecuteStoredProcedure("SetPrimaryVendor", paramList)
    End Sub

    Public Sub SwapPrimaryVendors(ByVal sVendorID As Integer, _
    ByVal tVendorID As Integer, ByVal ItemKey As Integer, ByVal Store_No As Integer)
        
        paramList.Clear()
        ' ***** Parameters ********
        currentParam = New DBParam
        currentParam.Name = "Store_No"
        currentParam.Value = Store_No
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        currentParam = New DBParam
        currentParam.Name = "Item_Key"
        currentParam.Value = ItemKey
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        currentParam = New DBParam
        currentParam.Name = "SourceVendorID"
        currentParam.Value = sVendorID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        currentParam = New DBParam
        currentParam.Name = "TargetVendorID"
        currentParam.Value = tVendorID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *********************************
        df.ExecuteStoredProcedure("SwitchPrimaryVendor", paramList)
    End Sub

    Public Sub DeleteStoreItemVendor(ByVal VendorID As Integer, _
    ByVal Store_No As Integer, ByVal ItemKey As Integer, _
    ByVal DeleteDate As DateTime)

        paramList.Clear()
        ' ***** Parameters ********
        currentParam = New DBParam
        currentParam.Name = "Store_No"
        currentParam.Value = Store_No
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        currentParam = New DBParam
        currentParam.Name = "Item_Key"
        currentParam.Value = ItemKey
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        currentParam = New DBParam
        currentParam.Name = "Vendor_ID"
        currentParam.Value = VendorID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        currentParam = New DBParam
        currentParam.Name = "DeleteDate"
        currentParam.Value = DeleteDate
        currentParam.Type = DBParamType.DateTime
        paramList.Add(currentParam)
        ' *********************************
        df.ExecuteStoredProcedure("DeleteStoreItemVendor", paramList)
    End Sub

    Public Sub InsertStoreItemVendor(ByVal VendorID As Integer, _
    ByVal Store_No As Integer, ByVal ItemKey As Integer)
        paramList.Clear()
        ' ***** Parameters ********
        currentParam = New DBParam
        currentParam.Name = "Store_NO"
        currentParam.Value = Store_No
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        currentParam = New DBParam
        currentParam.Name = "Item_Key"
        currentParam.Value = ItemKey
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        currentParam = New DBParam
        currentParam.Name = "Vendor_ID"
        currentParam.Value = VendorID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)
        ' *********************************
        df.ExecuteStoredProcedure("InsertStoreItemVendor", paramList)
    End Sub
    Public Sub UpdateStoreItem(ByVal Item_Key As Integer, _
        ByVal Store_No As Integer, ByVal AuthorizedItem As Boolean)
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
        currentParam.Name = "AuthorizedItem"
        currentParam.Value = AuthorizedItem
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Refresh"
        currentParam.Value = False
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)
        ' *********************************
        df.ExecuteStoredProcedure("UpdateStoreItem", paramList)
    End Sub
    Public Sub UpdateStoreItemECommerce(ByVal Item_Key As Integer, _
        ByVal Store_No As Integer, ByVal ECommerce As Boolean)
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
        currentParam.Name = "ECommerce"
        currentParam.Value = ECommerce
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        df.ExecuteStoredProcedure("UpdateStoreItemECommerce", paramList)
    End Sub

    Public Sub InsertItemVendor(ByVal item_Key As Integer, _
    ByVal vendor_ID As Integer, ByVal warehouse As String)
        Dim da As New IrmaItemTableAdapters.ItemTableAdapter
        da.InsertItemVendor(vendor_ID, item_Key)
        da.UpdateItemVendor(item_Key, vendor_ID, warehouse)
    End Sub
    Public Function GetStoreItemAuths(ByVal ItemKey As Integer, ByVal Store_No As Integer) As DataTable
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
            results = df.GetStoredProcedureDataReader("GetStoreItemAuths", paramList)


            'add columns to table
            table.Columns.Add(New DataColumn("Store_No", GetType(Int32)))
            table.Columns.Add(New DataColumn("Store_Name", GetType(String)))
            table.Columns.Add(New DataColumn("Authorized", GetType(Boolean)))
            table.Columns.Add(New DataColumn("StoreItemVendorID", GetType(Int32)))
            table.Columns.Add(New DataColumn("StoreJurisdictionID", GetType(Int32)))

            While results.Read
                'This filters down the dataset based on whether or not all stores or one store is selected.
                If Store_No = 0 Or Store_No = results.GetInt32(results.GetOrdinal("Store_No")) Then
                    row = table.NewRow
                    row("Store_No") = results.GetInt32(results.GetOrdinal("Store_No"))
                    row("Store_Name") = results.GetString(results.GetOrdinal("Store_Name"))
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

    Public Function GetStoreItemECommerce(ByVal ItemKey As Integer, ByVal Store_No As Integer) As DataTable
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
            results = df.GetStoredProcedureDataReader("GetStoreItemECommerce", paramList)


            'add columns to table
            table.Columns.Add(New DataColumn("Store_No", GetType(Int32)))
            table.Columns.Add(New DataColumn("Store_Name", GetType(String)))
            table.Columns.Add(New DataColumn("ECommerce", GetType(Boolean)))
            table.Columns.Add(New DataColumn("StoreItemVendorID", GetType(Int32)))
            table.Columns.Add(New DataColumn("StoreJurisdictionID", GetType(Int32)))

            While results.Read
                'This filters down the dataset based on whether or not all stores or one store is selected.
                If Store_No = 0 Or Store_No = results.GetInt32(results.GetOrdinal("Store_No")) Then
                    row = table.NewRow
                    row("Store_No") = results.GetInt32(results.GetOrdinal("Store_No"))
                    row("Store_Name") = results.GetString(results.GetOrdinal("Store_Name"))
                    row("ECommerce") = results.GetBoolean(results.GetOrdinal("ECommerce"))

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
