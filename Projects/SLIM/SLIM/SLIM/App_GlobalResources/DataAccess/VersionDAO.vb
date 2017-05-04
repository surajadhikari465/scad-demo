Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Text
Imports SLIM.WholeFoods.IRMA.Common.BusinessLogic
Imports SLIM.WholeFoods.Utility.DataAccess

Public Class VersionDAO
    Public Shared Function GetVersionInfo(ByVal sApplicationName As String) As String
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim reader As SqlDataReader = Nothing
        Dim sVersionInfo As String = "(version not found)"
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "ApplicationName"
            currentParam.Value = UCase(sApplicationName)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            reader = factory.GetStoredProcedureDataReader("GetVersion", paramList)

            While reader.Read
                sVersionInfo = reader("Version")
            End While
        Catch e As DataFactoryException
            Logger.LogError("Exception: ", Nothing, e)
            'send message about exception
            ErrorHandler.ProcessError(ErrorType.DataFactoryException, SeverityLevel.Warning, e)
        Finally
            ' Close the result set and the connection
            If (reader IsNot Nothing) Then
                reader.Close()
            End If
        End Try

        Return sVersionInfo
    End Function
End Class
