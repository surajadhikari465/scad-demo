Imports WholeFoods.Utility.DataAccess
Imports System.Configuration

Public Class Configuration

    Private Shared factory As DataFactory

    Public Shared Sub CreateAppSettings()

        Dim _appID As New Guid(ConfigurationManager.AppSettings("ApplicationGUID").ToString)
        Dim _envID As New Guid(ConfigurationManager.AppSettings("EnvironmentGUID").ToString)

        ' initiate the factory
        If factory Is Nothing Then
            factory = New DataFactory(DataFactory.ItemCatalog)
        End If

        ' write the configuration for the application and environment
        factory.WriteConfiguration(_appID, _envID)

    End Sub

End Class
