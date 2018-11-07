Imports WholeFoods.Utility.DataAccess
Imports system.Data.SqlClient
Imports log4net
Namespace WholeFoods.Utility
    Public Class AppDBLogDAO
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Sub PurgeHistory(ByVal appGuid As String, ByVal daysToKeep As Integer)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim parameters As New ArrayList()
            Dim dbParam As New DBParam
            ' We are purposely not catching errors here, so caller must wrap, handle, and log.
            dbParam = New DBParam
            With dbParam
                .Name = "ApplicationID"
                .Type = DBParamType.String
                .Value = appGuid
            End With
            parameters.Add(dbParam)
            dbParam = New DBParam
            With dbParam
                .Name = "DaysToKeep"
                .Type = DBParamType.Int
                .Value = daysToKeep
            End With
            parameters.Add(dbParam)

            factory.ExecuteStoredProcedure("dbo.AppLogPurgeHistory", parameters)
        End Sub

        Public Shared Function GetAppLogEntries( _
            ByVal appName As String, _
            ByVal startDate As Date, _
            ByVal endDate As Date, _
            ByVal TableSearch As Integer _
        ) As SqlDataReader
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList
            Dim reader As SqlDataReader = Nothing

            Try
                ' setup parameters for stored proc
                ' App name.
                currentParam = New DBParam
                currentParam.Name = "AppName"
                currentParam.Value = appName
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Start date.
                currentParam = New DBParam
                currentParam.Name = "StartDate"
                currentParam.Value = startDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                ' End date.
                currentParam = New DBParam
                currentParam.Name = "EndDate"
                currentParam.Value = endDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "SeachTable"
                currentParam.Value = TableSearch
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                reader = factory.GetStoredProcedureDataReader("AppLogGetEntries", paramList)

            Catch ex As Exception
                Throw ex
            End Try

            Return reader

        End Function
    End Class
End Namespace
