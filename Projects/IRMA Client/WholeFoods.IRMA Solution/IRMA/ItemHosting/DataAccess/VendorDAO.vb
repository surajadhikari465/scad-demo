Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Imports log4net

Namespace WholeFoods.IRMA.ItemHosting.DataAccess

    Public Class VendorDAO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        ''' <summary>
        ''' returns a list of stores that DO NOT have a primary vendor assigned to them for the given item key
        ''' </summary>
        ''' <param name="itemKey"></param> 
        ''' <returns>ArrayList of store names</returns>
        ''' <remarks></remarks>
        Public Shared Function GetStoresWithNoVendorForItem(ByVal itemKey As Integer) As ArrayList

            logger.Debug("GetStoresWithNoVendorForItem Entry with itemKey =" & itemKey.ToString)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim storeList As New ArrayList

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = itemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetStoresWithNoVendorForItem", paramList)

                While results.Read
                    storeList.Add(results.GetString(results.GetOrdinal("Store_Name")))
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetStoresWithNoVendorForItem Exit")

            Return storeList
        End Function
        Public Shared Sub UpdateVendorItemStatus(ByVal itemkey As Integer, ByVal vendorid As Integer, ByVal VendorItemStatus As Integer)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Dim sql As String = String.Format("update ItemVendor Set VendorItemStatus = {0} where Vendor_Id = {1} and Item_key = {2}", VendorItemStatus, vendorid, itemkey)
            factory.ExecuteNonQuery(sql)



            logger.Debug("UpdateVendorItemStatus Exit")
        End Sub

        Public Shared Function GetStoresWithPrimaryVendorThatCanSwap(ByVal itemKey As Integer, ByVal primaryVendorId As Integer) As ArrayList

            logger.Debug("GetStoresWithPrimaryVendorThatCanSwap Entry with itemKey = " & itemKey.ToString & " primaryVendorId =" & primaryVendorId.ToString)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim storeList As New ArrayList

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "VendorID"
                currentParam.Value = primaryVendorId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = itemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetStoresWithPrimaryVendorThatCanSwap", paramList)

                While results.Read
                    storeList.Add(results.GetInt32(results.GetOrdinal("Store_No")))
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetStoresWithPrimaryVendorThatCanSwap Exit")

            Return storeList
        End Function

        Public Shared Function IsStoreDistributionCenter(ByVal storeno As Integer) As Boolean

            logger.Debug("IsStoreDistributionCenter Entry with storeno =" & storeno.ToString)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = storeno
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetStoreIsDistribution", paramList)

                While results.Read
                    If results.GetBoolean(results.GetOrdinal("Distribution_Center")) Then
                        Return True
                    Else
                        Return False
                    End If
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("IsStoreDistributionCenter Exit")

            Return True
        End Function

        Public Shared Function VendorKeyExists(ByVal Key As String) As Boolean

            logger.Debug("VendorKeyExists Entry with Vendor Key =" & Key)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim retVal As Boolean = False

            ' Execute the stored procedure 
            retVal = CBool(factory.ExecuteScalar("SELECT dbo.fn_VendorKeyExists('" & Key & "')"))

            Return retVal

            logger.Debug("VendorKeyExists Exit")

        End Function


        Public Shared Function GetCurrentHandlingCharge(ByVal itemkey As Integer, ByVal vendorid As Integer) As Double

            logger.Debug("GetCurrentHandlingCharge Entry with itemkey = " & itemkey.ToString & " vendorid = " & vendorid.ToString)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim returnVal As Double = 0

            ' Execute the function
            returnVal = CType(factory.ExecuteScalar("SELECT dbo.fn_GetCurrentHandlingCharge(" & itemkey & ", " & vendorid & ")"), Double)

            Return returnVal
        End Function

        Public Shared Function GetVendors() As List(Of VendorBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As New DataTable
            Dim vendor As VendorBO
            Dim vendorList As List(Of VendorBO) = New List(Of VendorBO)


            results = factory.GetDataTable("GetVendors")

            For Each dr As DataRow In results.Rows
                vendor = New VendorBO(dr("Vendor_Id"), dr("CompanyName"), dr("PS_Vendor_Id").ToString)

                vendorList.Add(vendor)
            Next
            results.Dispose()

            Return vendorList

        End Function

        Public Shared Function GetInventoryCountVendors() As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As New DataTable

            results = factory.GetDataTable("GetInventoryCountVendors")

            Return results

        End Function
    End Class
End Namespace
