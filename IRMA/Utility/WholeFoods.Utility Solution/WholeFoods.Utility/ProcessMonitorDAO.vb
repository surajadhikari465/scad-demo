Imports WholeFoods.Utility.DataAccess

Public Class ProcessMonitorDAO

    Public Sub UpdateProcessMonitor(ByVal Classname As String, ByVal Status As String, ByVal StatusDescription As String, ByVal Details As String, ByVal UpdateLastRun As Boolean)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim parameters As New ArrayList()
        Dim dbParam As New DBParam
        Try
            dbParam = New DBParam
            With dbParam
                .Name = "Classname"
                .Type = DBParamType.String
                .Value = Classname
            End With
            parameters.Add(dbParam)

            dbParam = New DBParam
            With dbParam
                .Name = "Status"
                .Type = DBParamType.String
                .Value = Status
            End With
            parameters.Add(dbParam)

            dbParam = New DBParam
            With dbParam
                .Name = "StatusDescription"
                .Type = DBParamType.String
                .Value = StatusDescription
            End With
            parameters.Add(dbParam)

            dbParam = New DBParam
            With dbParam
                .Name = "Details"
                .Type = DBParamType.String
                .Value = Details
            End With
            parameters.Add(dbParam)

            dbParam = New DBParam
            With dbParam
                .Name = "UpdateLastRun"
                .Type = DBParamType.Bit
                .Value = UpdateLastRun
            End With
            parameters.Add(dbParam)

            factory.ExecuteStoredProcedure("dbo.UpdateJobStatusList", parameters)
        Catch ex As Exception
        End Try
    End Sub

End Class
