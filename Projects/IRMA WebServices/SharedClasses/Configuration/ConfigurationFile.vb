Imports System.Configuration
Public Class ConfigurationFile

    Public Shared Function GetAppSetting(ByVal Key As String, ByVal FilePath As String) As String
        Dim _Config As Configuration
        Dim fileMap As New ExeConfigurationFileMap()
        Try
            fileMap.ExeConfigFilename = FilePath
            _Config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None)
            Return _Config.AppSettings.Settings.Item(Key).Value.ToString
        Catch ex As Exception
            Throw ex
        End Try

    End Function

End Class
