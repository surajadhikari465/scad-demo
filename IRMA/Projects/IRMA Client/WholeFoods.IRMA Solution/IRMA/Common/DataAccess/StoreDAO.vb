Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Imports log4net


Namespace WholeFoods.IRMA.Common.DataAccess

    Public Class StoreListDAO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        ''' <summary>
        ''' Gets list of facilities by VendorName and Vendor_ID
        ''' </summary>
        ''' <returns>ArrayList of StoreListBO objects</returns>
        ''' <remarks></remarks>
        Public Shared Function GetFacilitiesListByVendorName() As ArrayList

            logger.Debug("GetFacilitiesListByVendorName Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim facility As StoreListBO
            Dim FacilityList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetAllFacilities")

                While results.Read
                    facility = New StoreListBO()
                    facility.VendorID = results.GetInt32(results.GetOrdinal("Vendor_ID"))
                    facility.VendorName = results.GetString(results.GetOrdinal("CompanyName"))
                    FacilityList.Add(facility)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetFacilitiesListByVendorName Exit")


            Return FacilityList
        End Function

        Friend Shared Function GetStoreNumberToBusinessUnitCollection() As Dictionary(Of Integer, Integer)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim connection As SqlConnection = New SqlConnection()
            Dim results As SqlDataReader
            Dim storeNumberToBusinessUnit As Dictionary(Of Integer, Integer) = New Dictionary(Of Integer, Integer)

            Try
                connection.ConnectionString = factory.ConnectString
                connection.Open()

                Dim command As New SqlCommand("GetStoreNumberToBusinessUnitCollection", connection) With {.CommandType = CommandType.StoredProcedure, .CommandTimeout = factory.CommandTimeout}

                results = command.ExecuteReader()

                While results.Read()
                    storeNumberToBusinessUnit.Add(results.GetInt32(results.GetOrdinal("StoreNumber")), results.GetInt32(results.GetOrdinal("BusinessUnit")))
                End While
            Finally
                connection.Close()
            End Try

            Return storeNumberToBusinessUnit
        End Function

        ''' <summary>
        ''' Gets list of facilities by Store_Name and Store_No 
        ''' </summary>
        ''' <returns>ArrayList of StoreListBO objects</returns>
        ''' <remarks></remarks>
        '''  This function is calling from DailyItem AVerage Cost Change report.
        ''' 
        Public Shared Function GetFacilitiesListByStoreName() As ArrayList

            logger.Debug("GetFacilitiesListByStoreName Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim facility As StoreListBO
            Dim FacilityList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetAvgCostAllFacilities")

                While results.Read
                    facility = New StoreListBO()
                    facility.StoreNo = results.GetInt32(results.GetOrdinal("Store_No"))
                    facility.StoreName = results.GetString(results.GetOrdinal("Store_Name"))
                    FacilityList.Add(facility)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetFacilitiesListByStoreName Exit")

            Return FacilityList
        End Function

        ''' <summary>
        ''' Gets list of stores by Store_Name and Store_No 
        ''' </summary>
        ''' <returns>ArrayList of StoreListBO objects</returns>
        ''' <remarks></remarks>
        '''  This function is calling from DailyItem AVerage Cost Change report.
        ''' 
        Public Shared Function GetStoresListByStoreName() As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim store As StoreListBO
            Dim StoreList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetStoresAndDist")

                While results.Read
                    store = New StoreListBO()
                    store.StoreNo = results.GetInt32(results.GetOrdinal("Store_No"))
                    store.StoreName = results.GetString(results.GetOrdinal("Store_Name"))
                    StoreList.Add(store)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return StoreList
        End Function

        Public Shared Function GetStoresAndDistAdjustments() As DataTable

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataSet = Nothing
            Dim paramList As New ArrayList

            Try
                ' Execute the stored procedure 
                logger.Debug("GetAdjustmentReasons entry")

                results = New DataSet

                results = factory.GetStoredProcedureDataSet("dbo.GetStoresAndDistAdjustments")

            Finally
                If results IsNot Nothing Then
                    results.Dispose()
                End If
            End Try

            logger.Debug("GetAdjustmentReasons Exit")

            Return results.Tables(0)

        End Function

        Public Shared Function GetRetailStores() As DataTable
            logger.Debug("GetRetailStores entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Dim results As DataTable = New DataTable()

            Try
                results = factory.GetStoredProcedureDataTable("GetRetailStores")
            Catch ex As Exception
                logger.Error("GetRetailStores error occurred when calling procedure GetRetailStores", ex)
                Throw
            Finally
                logger.Debug("GetRetailStores exit")
            End Try

            Return results
        End Function

    End Class

End Namespace
