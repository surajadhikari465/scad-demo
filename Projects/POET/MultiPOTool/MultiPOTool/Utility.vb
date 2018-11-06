Imports Microsoft.VisualBasic
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient

Public Class Utility

    Public Shared Function GetRegionsByUser(ByVal UserId As Integer) As DataSet

        Dim con As String = ConfigurationManager.ConnectionStrings("MultiPOTool").ConnectionString()
        Dim factory As New DataFactory(con, True)
        Dim results As DataSet
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "UserID"
            currentParam.Value = UserId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataSet("GetRegionsByUser", paramList)

        Catch ex As Exception
            Throw ex
        End Try

        Return results

    End Function

    Public Shared Sub InsertErrorLog(ByVal ErrorMessage As String)

        Dim con As String = ConfigurationManager.ConnectionStrings("MultiPOTool").ConnectionString()
        Dim factory As New DataFactory(con, True)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "ErrorMessage"
            currentParam.Value = ErrorMessage
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("InsertErrorLog", paramList)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
End Class
