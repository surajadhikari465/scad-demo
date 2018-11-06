Imports WholeFoods.Utility

Public Class DataAccess
    Private Shared ReadOnly mcolConnect As Collection
    Public Enum enuDBList
        ItemCatalog = 1
    End Enum
    Shared Sub New()
        mcolConnect = New Collection
        Try
            Dim config As New Configuration.AppSettingsReader
            mcolConnect.Add(New WFM.SQLServer.Connect(ConfigurationServices.AppSettings("ItemCatalogConnectionString"), ConfigurationServices.AppSettings("ItemCatalogCommandTimeout")), enuDBList.ItemCatalog.ToString)
            config = Nothing
        Catch ex As System.Exception
            Throw
        End Try
    End Sub
    Public Shared Sub AddConnectionString(ByVal ConnectionString As String, ByVal CommandTimeout As Integer)
        mcolConnect.Add(New WFM.SQLServer.Connect(ConnectionString, CommandTimeout))
    End Sub
    Public Shared Sub ConnectSqlCommand(ByRef cmd As SqlClient.SqlCommand, ByVal DB As enuDBList)
        Dim con As WFM.SQLServer.Connect = mcolConnect(DB)
        con.ConnectSqlCommand(cmd)
        con = Nothing
    End Sub
    Public Shared Sub ExecuteSqlCommand(ByRef cmd As System.Data.SqlClient.SqlCommand, ByVal DB As enuDBList)
        Dim con As WFM.SQLServer.Connect = mcolConnect(DB)

        Try
            con.ExecuteSqlCommand(cmd)
        Catch ex As System.Exception
            Throw
        End Try
        con = Nothing
    End Sub
    Public Shared Function GetSqlDataReader(ByRef cmd As System.Data.SqlClient.SqlCommand, ByVal DB As enuDBList) As System.Data.SqlClient.SqlDataReader
        Dim con As WFM.SQLServer.Connect = mcolConnect(DB)
        Return con.GetSqlDataReader(cmd)
        con = Nothing
    End Function
    Public Shared Function GetSqlDataSet(ByRef cmd As System.Data.SqlClient.SqlCommand, ByVal DB As enuDBList) As System.Data.DataSet
        Dim con As WFM.SQLServer.Connect = mcolConnect(DB)
        Return con.GetSqlDataSet(cmd)
        con = Nothing
    End Function
    Public Shared Sub ReleaseDataObject(ByRef obj As System.Object, ByVal DB As enuDBList)
        Dim con As WFM.SQLServer.Connect = mcolConnect(DB)
        con.ReleaseDataObject(obj)
        con = Nothing
    End Sub
End Class

