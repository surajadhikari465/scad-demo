Imports Microsoft.VisualBasic
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient

Public Class DAOPushToIRMA

    Private con As String = ConfigurationManager.ConnectionStrings("MultiPOTool").ConnectionString()

    Public Function ListRegions() As ArrayList

        Dim factory As New DataFactory(con, True)
        Dim results As SqlDataReader = Nothing
        Dim regionList As New ArrayList

        Try
            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataReader("GetRegionsToPush")
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

    Public Sub PushByRegion(ByVal RegionID As Integer)

        Dim factory As New DataFactory(con, True)
        Dim results As New ArrayList
        Dim paramList As New ArrayList
        Dim currentParam As DBParam
        Dim cmdTimeout As Integer
        cmdTimeout = CInt(ConfigurationManager.AppSettings("PushTimeout"))

        Try
            currentParam = New DBParam
            currentParam.Name = "RegionID"
            currentParam.Value = RegionID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            factory.CommandTimeout = cmdTimeout

            results = factory.ExecuteStoredProcedure("PushPOsToIRMA", paramList)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class