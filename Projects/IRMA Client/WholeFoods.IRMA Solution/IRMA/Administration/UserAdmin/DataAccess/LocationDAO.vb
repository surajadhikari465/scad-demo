Imports WholeFoods.Utility.DataAccess

''' <summary>
''' Handles read/writes
''' </summary>
''' <remarks></remarks>
Public Class LocationDAO

    ''' <summary>
    ''' Returns all Store entries in the Stores table.
    ''' </summary>
    ''' <returns>DataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetLocations() As DataTable

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataTable = Nothing

        Try

            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataTable("GetAllStores_ByStoreName")

            ' add an "All Stores" row
            Dim dr As DataRow = results.NewRow()

            dr.Item("Store_Name") = "--ALL--"
            dr.Item("Store_No") = 0

            results.Rows.InsertAt(dr, 0)

        Catch ex As Exception

            Throw ex

        End Try

        Return results

    End Function

    ''' <summary>
    ''' Returns a value indicating whether the specified Store is a retail location.
    ''' </summary>
    ''' <param name="StoreNo">The Store_No to look up.</param>
    ''' <returns>Boolean</returns>
    ''' <remarks>Only entries in Stores with WFM_Store = True are included in the return DataTable.</remarks>
    Public Shared Function IsRetailLocation(ByVal StoreNo As Integer) As Boolean

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As Boolean = False

        Try

            ' Execute the function 
            results = factory.ExecuteScalar("SELECT dbo.fn_IsRetailLocation(" & StoreNo & ")")

        Catch ex As Exception

            Throw ex

        End Try

        Return results

    End Function

End Class
