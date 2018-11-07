Imports Microsoft.VisualBasic
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient

Public Class DAOPONumbers

    Private con As String = ConfigurationManager.ConnectionStrings("MultiPOTool").ConnectionString()

    Public Sub AssignPONumbers(ByVal RegionID As Integer, ByVal POTypeID As Integer, ByVal UserID As Integer, ByVal POCount As Integer)

        Dim factory As New DataFactory(con, True)
        Dim results As ArrayList
        'Dim results As Integer
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "RegionID"
            currentParam.Value = RegionID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POTypeID"
            currentParam.Value = POTypeID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UserID"
            currentParam.Value = UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POCount"
            currentParam.Value = POCount
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.ExecuteStoredProcedure("AssignPONumbers", paramList, False)
            'results = factory.ExecuteNonQuery("AssignPONumber", paramList)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function GetUnusedPONumbersByUser(ByVal UserID As Integer) As DataSet

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

            results = factory.GetStoredProcedureDataSet("GetUnusedPONumbersByUser", paramList)
        Catch ex As Exception
            Throw ex
        End Try

        Return results

    End Function
    Public Function GetReasonCodesForUser(ByVal UserID As Integer) As DataSet

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

            results = factory.GetStoredProcedureDataSet("GetReasonCodes", paramList)
        Catch ex As Exception
            Throw ex
        End Try

        Return results

    End Function

    Public Function GetPOTypes() As DataSet

        Dim factory As New DataFactory(con, True)
        Dim results As DataSet = Nothing

        Try
            results = factory.GetStoredProcedureDataSet("GetPOTypes")

        Catch ex As Exception
            Throw ex
        End Try

        Return results

    End Function

    Public Function GetVersion() As DataSet

        Dim factory As New DataFactory(con, True)
        Dim results As DataSet = Nothing

        Try
            results = factory.GetStoredProcedureDataSet("GetVersion")

        Catch ex As Exception
            Throw ex
        End Try

        Return results

    End Function

    Public Function DeletePONumber(ByVal iPONumber As Integer) As Boolean
        Dim factory As New DataFactory(con, True)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "PONumber"
            currentParam.Value = iPONumber
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("DeletePONumber", paramList)
            Return True
        Catch ex As Exception
            Return False
            Throw ex
        End Try

    End Function
End Class