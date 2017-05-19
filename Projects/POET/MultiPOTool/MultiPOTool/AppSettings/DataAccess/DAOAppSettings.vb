Imports Microsoft.VisualBasic
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient

Public Class DAOAppSettings

    Private con As String = ConfigurationManager.ConnectionStrings("MultiPOTool").ConnectionString()

    Public Function GetAppSetting(ByVal stKeyName As String) As String
        Dim factory As New DataFactory(con, True)
        'Dim results As New String

        Try
            Return factory.ExecuteScalar("EXEC GetAppSetting '" & stKeyName & "'")
        Catch ex As Exception
            Throw ex
        End Try

        'Return results
    End Function
End Class
