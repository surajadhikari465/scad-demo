Imports Microsoft.VisualBasic
Imports System.Data.SqlClient


Public Class Users



    Public Function GetUser_ID(ByVal UserName As String) As Integer
        Dim sqlCon As New SqlConnection(ConfigurationManager.ConnectionStrings(DataFactory.ItemCatalog).ConnectionString)
        Dim UserID As Integer
        Dim comstr As String = "select User_ID from Users where " & _
        " UserName = @UserName "
        Dim sqlCom As New SqlCommand(comstr, sqlCon)
        sqlCom.Parameters.AddWithValue("@UserName", UserName)
        sqlCon.Open()
        UserID = sqlCom.ExecuteScalar()
        sqlCon.Close()
        Return UserID
    End Function

    Public Sub GetSubTeamsByUser()

    End Sub

    Public Sub GetStoresByUser()

    End Sub

    Public Sub GetTeamsByUser()

    End Sub
End Class
