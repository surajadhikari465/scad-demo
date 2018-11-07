Imports Microsoft.VisualBasic
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient
Imports System.Text

Public Class DAOValidateByRegion

    Private con As String = ConfigurationManager.ConnectionStrings("MultiPOTool").ConnectionString()

    Public Function ListRegions() As ArrayList

        Dim factory As New DataFactory(con, True)
        Dim results As SqlDataReader = Nothing
        Dim regionList As New ArrayList

        Try
            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataReader("GetRegionsWithOrdersInQueue")
            While results.Read
                regionList.Add(CInt(results.Item(0)))
            End While
        Catch ex As Exception
            Debug.WriteLine(ex.InnerException)
            Debug.WriteLine(ex.Message)
        Finally
            If results IsNot Nothing Then
                results.Close()
            End If
        End Try

        Return regionList

    End Function

    Public Sub ValidateByRegion(ByVal RegionID As Integer)

        Dim factory As New DataFactory(con, True)
        Dim results As New ArrayList
        Dim paramList As New ArrayList
        Dim currentParam As DBParam
        Dim cmdTimeout As Integer
        cmdTimeout = CInt(ConfigurationManager.AppSettings("ValidationTimeout"))

        Try
            currentParam = New DBParam
            currentParam.Name = "RegionID"
            currentParam.Value = RegionID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            factory.CommandTimeout = cmdTimeout

            results = factory.ExecuteStoredProcedure("ValidatePODataInIRMA", paramList)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GetEmailInfo() As DataTable

        Dim OrdersToValidate As DataTable

        Dim factory As New DataFactory(con, True)
        Dim paramList As New ArrayList

        Try
            OrdersToValidate = factory.GetStoredProcedureDataTable("GetValidationEmailInfo")
        Catch ex As Exception
            Throw ex
        End Try

        Return OrdersToValidate

    End Function

    Public Function GetValidationSuccess(ByVal SessionID As Integer) As Boolean

        Dim factory As New DataFactory(con, True)

        Dim sql As New StringBuilder
        Dim result As Object = Nothing

        sql.Append("SELECT ValidationSuccessful FROM UploadSessionHistory ")
        sql.AppendFormat("WHERE UploadSessionHistoryID = {0}", SessionID)
        Try
            result = factory.ExecuteScalar(sql.ToString)
        Catch ex As Exception
            Debug.WriteLine(ex.InnerException)
            Debug.WriteLine(ex.Message)
        End Try

        Return CBool(result)

    End Function


End Class