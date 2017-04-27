Imports WholeFoods.Utility.DataAccess

''' <summary>
''' Handles read/writes
''' </summary>
''' <remarks></remarks>
Public Class SubTeamDAO

    ''' <summary>
    ''' Returns the list of Retail Subteams.
    ''' </summary>
    ''' <returns>DataTable</returns>
    ''' <remarks>Only SubTeams with a SubTeamtype_ID of 1 (retail) or 3 (retail manufacturing) 
    ''' are included in the return DataTable.</remarks>
    Public Shared Function GetSubteams() As DataTable

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataTable = Nothing
        Dim paramList As New ArrayList

        Try

            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataTable("GetAdminSubTeamList", paramList)

        Catch ex As Exception

            Throw ex

        End Try

        Return results

    End Function

End Class
