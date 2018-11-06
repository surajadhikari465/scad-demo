Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Ordering.DataAccess
Imports WholeFoods.IRMA.Ordering
Imports WholeFoods.IRMA.Common
Imports WholeFoods.IRMA.Replenishment.EInvoicing.DataAccess
Imports WholeFoods.IRMA.Administration.StoreAdmin.DataAccess
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic




Public Class InvoiceMatchingBO

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Private Variables"


    ' DATASET HOLDING ALL INVOICE TOLERANCE DATA
    Dim _isError As Boolean
    Dim _errorMessage As String
    Dim _isValidated As Boolean


#End Region


    Public Property IsError() As Boolean
        Get
            Return _isError
        End Get
        Set(ByVal value As Boolean)
            _isError = value
        End Set
    End Property

    Public Property ErrorMessage() As String
        Get
            Return _errorMessage
        End Get
        Set(ByVal value As String)
            _errorMessage = value
        End Set
    End Property

    Public Property IsValidated() As Boolean
        Get
            Return _isValidated
        End Get
        Set(ByVal value As Boolean)
            _isValidated = value
        End Set
    End Property

#Region "Functions"

    Friend Function GetAllToleranceValues() As DataSet

        Dim reader As SqlDataReader = Nothing
        Dim dal As New InvoiceMatchingDAO
        Dim ds As New DataSet
        Dim dt As New DataTable

        ' Get Reader
        Try
            reader = dal.GetInvoiceTolerances
        Catch ex As Exception
            logger.Error("Could Not Get Data")
        End Try

        ' Get Typed Dataset
        ds = CreateDataset()


        ' ******* Gather data and fill data tables in data set ******

        ' **** Regional Tolerance *****
        If reader.HasRows Then
            ' record exists, load it.
            While reader.Read

                '' Populate the Regional Table.

                Dim dr As DataRow
                dr = ds.Tables("RegionalTolerance").NewRow()
                ds.Tables("RegionalTolerance").Rows.Add(dr)
                dr.Item("Vendor_Tolerance") = reader.GetValue((reader.GetOrdinal("Vendor_Tolerance")))
                dr.Item("Vendor_Tolerance_Amount") = reader.GetValue((reader.GetOrdinal("Vendor_Tolerance_Amount")))
                dr.Item("User_ID") = reader.GetValue((reader.GetOrdinal("User_ID")))
                dr.Item("UpdateDate") = reader.GetValue((reader.GetOrdinal("UpdateDate")))
                dr = Nothing

            End While
        Else
            ' not records exist in this table, and a "NULL" record so new values can be added.
            ' this fix may need to be added for the other recordsets as well.
            Dim dr As DataRow
            dr = ds.Tables("RegionalTolerance").NewRow()
            ds.Tables("RegionalTolerance").Rows.Add(dr)
            dr.Item("Vendor_Tolerance") = DBNull.Value
            dr.Item("Vendor_Tolerance_Amount") = DBNull.Value
            dr.Item("User_ID") = DBNull.Value
            dr.Item("UpdateDate") = DBNull.Value
            dr = Nothing
        End If
        

        ' **** Store Tolerance *****

        reader.NextResult()

        While reader.Read

            '' Populate the Store Override Table.

            Dim dr As DataRow
            dr = ds.Tables("StoreTolerance").NewRow()
            dr.Item("Store_No") = reader.GetValue((reader.GetOrdinal("Store_No")))
            dr.Item("Store_Name") = reader.GetValue((reader.GetOrdinal("Store_Name")))
            dr.Item("Vendor_Tolerance") = reader.GetValue((reader.GetOrdinal("Vendor_Tolerance")))
            dr.Item("Vendor_Tolerance_Amount") = reader.GetValue((reader.GetOrdinal("Vendor_Tolerance_Amount")))
            dr.Item("User_ID") = reader.GetValue((reader.GetOrdinal("User_ID")))
            dr.Item("UpdateDate") = reader.GetValue((reader.GetOrdinal("UpdateDate")))
            ds.Tables("StoreTolerance").Rows.Add(dr)

            dr = Nothing

        End While

        ' **** Vendor Tolerance *****

        reader.NextResult()

        While reader.Read

            '' Populate the Vendor Override Table.
            Dim dr As DataRow
            dr = ds.Tables("VendorTolerance").NewRow()
            dr.Item("Vendor_ID") = reader.GetValue((reader.GetOrdinal("Vendor_ID")))
            dr.Item("Vendor_Name") = reader.GetValue((reader.GetOrdinal("Vendor_Name")))
            dr.Item("Vendor_Tolerance") = reader.GetValue((reader.GetOrdinal("Vendor_Tolerance")))
            dr.Item("Vendor_Tolerance_Amount") = reader.GetValue((reader.GetOrdinal("Vendor_Tolerance_Amount")))
            dr.Item("User_ID") = reader.GetValue((reader.GetOrdinal("User_ID")))
            dr.Item("UpdateDate") = reader.GetValue((reader.GetOrdinal("UpdateDate")))
            ds.Tables("VendorTolerance").Rows.Add(dr)

            dr = Nothing

        End While

        Return ds

    End Function

    Friend Function CreateDataset() As DataSet


        ' Build the DataSet.
        Dim ds As New DataSet("InvoiceTolerance")

        ' Build the tables.
        Dim dt_regional As New DataTable("RegionalTolerance")
        Dim dt_store As New DataTable("StoreTolerance")
        Dim dt_vendor As New DataTable("VendorTolerance")

        ' Add Tables to DataSet
        ds.Tables.Add(dt_regional)
        ds.Tables.Add(dt_store)
        ds.Tables.Add(dt_vendor)

        ' Create Regional Table
        dt_regional.Columns.Add("Vendor_Tolerance", GetType(Double))
        dt_regional.Columns.Add("Vendor_Tolerance_Amount", GetType(Double))
        dt_regional.Columns.Add("User_ID", GetType(Integer))
        dt_regional.Columns.Add("UpdateDate", GetType(Date))

        ' Create Store Override Table
        'Dim cl As New DataColumn("Store_No", GetType(Integer))
        'Dim keyCol(0) As DataColumn
        'dt_store.Columns.Add(cl)
        'keyCol(0) = cl
        'dt_store.PrimaryKey = keyCol
        dt_store.Columns.Add("Store_No", GetType(Integer))
        dt_store.Columns.Add("Store_Name", GetType(String))
        dt_store.Columns.Add("Vendor_Tolerance", GetType(Double))
        dt_store.Columns.Add("Vendor_Tolerance_Amount", GetType(Double))
        dt_store.Columns.Add("User_ID", GetType(Integer))
        dt_store.Columns.Add("UpdateDate", GetType(Date))


        ' Create Vendor Override Table
        'cl = New DataColumn("Vendor_ID", GetType(Integer))
        'keyCol(0) = New DataColumn
        'dt_vendor.Columns.Add(cl)
        'keyCol(0) = cl
        'dt_vendor.PrimaryKey = keyCol
        dt_vendor.Columns.Add("Vendor_ID", GetType(Integer))
        dt_vendor.Columns.Add("Vendor_Name", GetType(String))
        dt_vendor.Columns.Add("Vendor_Tolerance", GetType(Double))
        dt_vendor.Columns.Add("Vendor_Tolerance_Amount", GetType(Double))
        dt_vendor.Columns.Add("User_ID", GetType(Integer))
        dt_vendor.Columns.Add("UpdateDate", GetType(Date))


        ' Return Typed Dataset
        Return ds

    End Function

    Private Function CreateDataSetListChanges(ByVal ds As DataSet) As List(Of DataSet)

        Dim dsList As New List(Of DataSet)

        Return dsList

    End Function

    Friend Function GetStores() As DataTable
        Dim stores As ArrayList
        Dim dt As New DataTable("Stores")
        Dim dr As DataRow
        Dim store As StoreBO

        ' ******** CREATE STORE DATA TABLE *******
        dt.Columns.Add("Store_No", GetType(Integer))
        dt.Columns.Add("Store_Name", GetType(String))
        ' ******** GET ALL STORES FOR DROPDOWN *******
        stores = StoreDAO.GetStores
        If stores.Count > 0 Then
            For Each store In stores
                dr = dt.NewRow()
                dr.Item("Store_No") = store.StoreNo
                dr.Item("Store_Name") = store.StoreName
                dt.Rows.Add(dr)
            Next
        End If
        Return dt
    End Function

    Friend Function GetVendors() As DataTable
        Dim vendors As DataTable
        Dim da As New EInvoicingDAO
        ' ****** GET ALL STORES FOR DROPDOWN *********
        vendors = da.getVendors
        Return vendors
    End Function

    Friend Function GetVendorIDList(ByVal vendorList As DataTable) As List(Of Integer)

        Dim list As New List(Of Integer)
        Dim dr As DataRow

        For Each dr In vendorList.Rows

            list.Add(dr.Item("Vendor_ID"))

        Next

        Return list
    End Function

    Friend Function GetStoreIDList(ByVal storeList As ArrayList) As List(Of Integer)

        Dim list As New List(Of Integer)
        Dim i As Integer

        For i = 0 To storeList.Count - 1
            Dim store As New StoreInfo()
            store = storeList.Item(i)
            list.Add(store.StoreNo)
        Next

        Return list

    End Function

    Friend Function ValidateNewEntryRows(ByVal ds As DataSet) As Boolean
        Dim validated As Boolean = True
        Dim dr As DataRow
        Dim dt As New DataTable
        Dim list As New List(Of Integer)
        Dim storeIDs As IEnumerator = ds.Tables(toleranceType.Store).Rows.GetEnumerator
        Dim vendorIDs As IEnumerator = ds.Tables(toleranceType.Vendor).Rows.GetEnumerator

        storeIDs.Reset()
        While storeIDs.MoveNext()

            dr = CType(storeIDs.Current, DataRow)
            'If dr.Item(0) = dr.Item(1) Then
            'Return False
            'End If

        End While

        list.Clear()

        vendorIDs.Reset()
        While vendorIDs.MoveNext()

            dr = CType(vendorIDs.Current, DataRow)
            If dr.Item(0) = dr.Item(1) Then
                Return False
            End If

        End While

        Return validated
    End Function


#End Region

#Region "Subs"

    Friend Sub UpdateDataSource(ByVal ds As DataSet)

        Dim dsList As New List(Of DataSet)
        Dim dt As New DataTable
        dsList.Insert(changeType.Modified, ds.GetChanges(DataRowState.Modified))
        dsList.Insert(changeType.Added, ds.GetChanges(DataRowState.Added))
        dsList.Insert(changeType.Deleted, ds.GetChanges(DataRowState.Deleted))
        Dim ds1 As New DataSet
        Dim dr As DataRow
        Dim da As New InvoiceMatchingDAO
        Dim dsListLoop As Integer = 0

        'RESET ERROR VALUE
        IsError = False

        ' LOOP THROUGH DATA TABLES IN DATASET OF CHANGES
        For Each ds1 In dsList


            If Not ds1 Is Nothing Then

                For Each dt In ds1.Tables

                    If Not dt.Rows.Count = 0 Then

                        Select Case dt.TableName

                            Case "RegionalTolerance"
                                'Update Regional Tolerance
                                Try
                                    dr = dt.Rows(0)
                                    logger.InfoFormat("UpdateInvoiceTolerance {0} {1} {2}", dr.Item("Vendor_Tolerance"), dr.Item("Vendor_Tolerance_Amount"), giUserID)
                                    da.UpdateInvoiceTolerance(dr.Item("Vendor_Tolerance"), dr.Item("Vendor_Tolerance_Amount"), giUserID)
                                Catch ex As Exception
                                    IsError = True
                                    ErrorMessage = ex.Message.ToString
                                    Exit Sub
                                End Try

                            Case "StoreTolerance"

                                'Update Store Tolerance
                                If dsListLoop = changeType.Modified Then
                                    Try
                                        For Each dr In dt.Rows
                                            logger.InfoFormat("UpdateInvoiceToleranceStore {0} {1} {2} {3}", dr.Item("Store_No"), dr.Item("Vendor_Tolerance"), dr.Item("Vendor_Tolerance_Amount"), giUserID)
                                            da.UpdateInvoiceToleranceStore(dr.Item("Store_No"), dr.Item("Vendor_Tolerance"), dr.Item("Vendor_Tolerance_Amount"), giUserID)
                                        Next
                                    Catch ex As Exception
                                        IsError = True
                                        ErrorMessage = ex.Message.ToString
                                        Exit Sub
                                    End Try
                                End If


                                'Delete Store Tolerance

                                If dsListLoop = changeType.Deleted Then
                                    Try
                                        For Each dr In dt.Rows
                                            logger.InfoFormat("DeleteInvoiceToleranceStore {0} {1}", dr.Item("Store_No", DataRowVersion.Original), giUserID)
                                            da.DeleteInvoiceToleranceStore(dr.Item("Store_No", DataRowVersion.Original))
                                        Next
                                    Catch ex As Exception
                                        IsError = True
                                        ErrorMessage = ex.Message.ToString
                                        Exit Sub
                                    End Try

                                End If

                                'Insert Store Tolerance

                                If dsListLoop = changeType.Added Then
                                    Try
                                        For Each dr In dt.Rows
                                            logger.InfoFormat("InsertInvoiceToleranceStore {0} {1} {2} {3}", dr.Item(1), dr.Item("Vendor_Tolerance"), dr.Item("Vendor_Tolerance_Amount"), giUserID)
                                            da.InsertInvoiceToleranceStore(dr.Item(1), dr.Item("Vendor_Tolerance"), dr.Item("Vendor_Tolerance_Amount"), giUserID)
                                        Next
                                    Catch ex As Exception
                                        IsError = True
                                        ErrorMessage = ex.Message.ToString
                                        Exit Sub
                                    End Try

                                End If

                            Case "VendorTolerance"

                                'Update Vendor Tolerance
                                If dsListLoop = changeType.Modified Then
                                    For Each dr In dt.Rows
                                        Try
                                            logger.InfoFormat("UpdateInvoiceToleranceVendor {0} {1} {2} {3}", dr.Item("Vendor_ID"), dr.Item("Vendor_Tolerance"), dr.Item("Vendor_Tolerance_Amount"), giUserID)
                                            da.UpdateInvoiceToleranceVendor(dr.Item("Vendor_ID"), dr.Item("Vendor_Tolerance"), dr.Item("Vendor_Tolerance_Amount"), giUserID)
                                        Catch ex As Exception
                                            IsError = True
                                            ErrorMessage = ex.Message.ToString
                                            Exit Sub
                                        End Try
                                    Next
                                End If

                                'Delete Vendor Tolerance

                                If dsListLoop = changeType.Deleted Then
                                    Try
                                        For Each dr In dt.Rows
                                            logger.InfoFormat("DeleteInvoiceToleranceVendor {0} {1}", dr.Item("Vendor_ID", DataRowVersion.Original), giUserID)
                                            da.DeleteInvoiceToleranceVendor(dr.Item("Vendor_ID", DataRowVersion.Original))
                                        Next
                                    Catch ex As Exception
                                        IsError = True
                                        ErrorMessage = ex.Message.ToString
                                        Exit Sub
                                    End Try
                                End If

                                'Insert Vendor Tolerance

                                If dsListLoop = changeType.Added Then
                                    Try
                                        For Each dr In dt.Rows
                                            logger.InfoFormat("InsertInvoiceToleranceVendor {0} {1} {2} {3}", dr.Item(1), dr.Item("Vendor_Tolerance"), dr.Item("Vendor_Tolerance_Amount"), giUserID)
                                            da.InsertInvoiceToleranceVendor(dr.Item(1), dr.Item("Vendor_Tolerance"), dr.Item("Vendor_Tolerance_Amount"), giUserID)
                                        Next
                                    Catch ex As Exception
                                        IsError = True
                                        ErrorMessage = ex.Message.ToString
                                        Exit Sub
                                    End Try

                                End If

                        End Select
                    End If

                Next

            End If
            dsListLoop += 1
        Next

    End Sub

    Friend Sub PrintValues(ByVal dataSet As DataSet, ByVal label As String)
        Debug.WriteLine(label + ControlChars.Cr)
        Dim table As DataTable
        For Each table In dataSet.Tables
            Debug.WriteLine("TableName: " + table.TableName)
            Dim row As DataRow
            For Each row In table.Rows
                Dim column As DataColumn
                For Each column In table.Columns
                    Debug.Write(ControlChars.Tab & " " _
                       & row(column).ToString())
                Next column
                Debug.WriteLine("END")
            Next row
        Next table
    End Sub

#End Region


#Region "Enumeration"

    Friend Enum changeType
        Modified
        Added
        Deleted
    End Enum

    Friend Enum toleranceType
        Regional
        Store
        Vendor
    End Enum



#End Region

End Class






