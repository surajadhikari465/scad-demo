Imports Microsoft.VisualBasic
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient


Public Class DAOExceptions

    Private con As String = ConfigurationManager.ConnectionStrings("MultiPOTool").ConnectionString()


    Public Function GetExceptionsByUserID(ByVal UserID As Integer) As DataSet

        Dim factory As New DataFactory(con, True)
        Dim results As New DataSet
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "UserID"
            currentParam.Value = UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataSet("GetExceptionsByUserID", paramList)
        Catch ex As Exception
            Throw ex
        End Try

        Return results

    End Function

    Public Function GetSessionsWithExceptionsByUserID(ByVal UserID As Integer) As DataSet

        Dim factory As New DataFactory(con, True)
        Dim results As New DataSet
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "UserID"
            currentParam.Value = UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataSet("GetSessionsWithExceptionsByUserID", paramList)
        Catch ex As Exception
            Throw ex
        End Try

        Return results

    End Function

    Public Function GetExceptionsByUploadSession(ByVal UploadSessionHistoryID As Integer) As DataSet

        Dim factory As New DataFactory(con, True)
        Dim results As New DataSet
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "UploadSessionHistoryID"
            currentParam.Value = UploadSessionHistoryID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataSet("GetExceptionsByUploadSession", paramList)
        Catch ex As Exception
            Throw ex
        End Try

        Return results

    End Function
    Public Function GetExceptionHeadersByUploadSession(ByVal UploadSessionHistoryID As Integer) As DataTable

        Dim factory As New DataFactory(con, True)
        Dim results As New DataTable
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "UploadSessionHistoryID"
            currentParam.Value = UploadSessionHistoryID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataTable("GetExceptionHeadersByUploadSession", paramList)
        Catch ex As Exception
            Throw ex
        End Try

        Return results

    End Function
    Public Function GetExceptionItemsByUploadSession(ByVal UploadSessionHistoryID As Integer) As DataTable

        Dim factory As New DataFactory(con, True)
        Dim results As New DataTable
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "UploadSessionHistoryID"
            currentParam.Value = UploadSessionHistoryID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataTable("GetExceptionItemsByUploadSession", paramList)
        Catch ex As Exception
            Throw ex
        End Try

        Return results

    End Function

End Class