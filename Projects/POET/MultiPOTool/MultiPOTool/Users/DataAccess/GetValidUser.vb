Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient

Public Class DALGetValidUser


    Private con As String = ConfigurationManager.ConnectionStrings("MultiPOTool").ConnectionString


    Public Function IsValidUser(ByVal username As String) As Boolean

        Dim factory As New DataFactory(con, True)

        Dim sql As New StringBuilder
        Dim result As Object = Nothing

        sql.Append("select count(userid) from users where username = ")
        sql.AppendFormat("'{0}' and Active = 1", username)


        Try
            result = factory.ExecuteScalar(sql.ToString)
        Catch ex As Exception
            Debug.WriteLine(ex.InnerException)
            Debug.WriteLine(ex.Message)
        End Try

        Return CBool(result)

    End Function

    Public Function GetValidUserID(ByVal username As String) As Integer

        Dim factory As New DataFactory(con, True)

        Dim sql As New StringBuilder
        Dim result As Object = Nothing

        sql.Append("select userid from users where username = ")
        sql.AppendFormat("'{0}' and Active = 1", username)


        Try
            result = factory.ExecuteScalar(sql.ToString)
        Catch ex As Exception
            Debug.WriteLine(ex.InnerException)
            Debug.WriteLine(ex.Message)
        End Try

        Return CInt(result)

    End Function


    Public Function GetValidUserCredentials(ByVal userID As Integer) As DataTable

        Dim factory As New DataFactory(con, True)

        Dim sql As New StringBuilder
        Dim result As DataTable = Nothing

        sql.Append("select users.*, regions.regionName from users inner join regions on users.regionID = regions.regionID where userID = ")
        sql.Append(userID)

        Try
            result = factory.GetDataTable(sql.ToString)
        Catch ex As Exception
            Debug.WriteLine(ex.InnerException)
            Debug.WriteLine(ex.Message)
        End Try

        Return result

    End Function


End Class
