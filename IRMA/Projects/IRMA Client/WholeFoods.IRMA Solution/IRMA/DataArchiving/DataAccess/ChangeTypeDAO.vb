Imports WholeFoods.Utility.DataAccess

''' <summary>
''' Handles read/writes
''' </summary>
''' <remarks></remarks>
Public Class ChangeTypeDAO

    ''' <summary>
    ''' Retruns a list of all data archive change types.
    ''' </summary>
    ''' <returns>DataTable</returns>
    ''' <remarks>New Stored Procedure in IRMA v3.5.</remarks>
    Public Shared Function GetChangeTypes() As DataTable

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataTable = Nothing

        Try

            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataTable("Administration_DataArchive_GetChangeTypes")

        Catch ex As Exception
            Throw ex
        End Try

        Return results

    End Function

End Class
