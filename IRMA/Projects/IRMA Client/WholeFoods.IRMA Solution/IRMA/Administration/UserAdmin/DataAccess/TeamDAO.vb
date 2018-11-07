Imports WholeFoods.Utility.DataAccess

''' <summary>
''' Handles read/writes
''' </summary>
''' <remarks></remarks>
Public Class TeamDAO

    ''' <summary>
    ''' Returns a list of all Retail Teams.
    ''' </summary>
    ''' <returns>DataTable</returns>
    ''' <remarks>Only Teams that are associated with SubTeams that have a SubTeamtype_ID of 1 (retail) or 3 (retail manufacturing) 
    ''' are included in the return DataTable.</remarks>
    Public Shared Function GetTeams() As DataTable

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataTable = Nothing

        Try

            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataTable("GetRetailTeams")

        Catch ex As Exception
            Throw ex
        End Try

        Return results

    End Function

End Class
