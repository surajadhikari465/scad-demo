Imports WholeFoods.Utility.DataAccess
Imports System.Configuration

Public Class Configuration

    Private Shared factory As DataFactory

    Public Shared Sub CreateAppSettings()

        Try

            ' get the app.config GUIDs that identify the application and environment to connect to
            Dim _appID As New Guid(ConfigurationManager.AppSettings("ApplicationGUID").ToString)
            Dim _envID As New Guid(ConfigurationManager.AppSettings("EnvironmentGUID").ToString)

            ' initiate the factory
            If factory Is Nothing Then
                factory = New DataFactory(DataFactory.ItemCatalog)
            End If

            ' write the configuration for the application and environment
            factory.WriteConfiguration(_appID, _envID)

        Catch ex As Exception

            Throw New Exception("The application is unable to retrieve configuration information " & _
                        "from the database. Contact your Regional IT Support Team for further assistance.")

        End Try

    End Sub

End Class
