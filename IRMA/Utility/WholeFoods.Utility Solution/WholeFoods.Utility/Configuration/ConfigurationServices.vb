Imports System.Drawing
Imports System.Reflection
Imports System.Resources
Imports System.Configuration

Imports log4net

Namespace WholeFoods.Utility

    Public Class ConfigurationServices

        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Shared Function AppSettings(ByVal Key As String) As String

            ' 1.05.10   Project Jeannie Modifications
            '           Robert Shurbet
            ' Add new optional parameters so the job knows where to access the configuration file.
            ' For Scheduled Jobs, each reigon has a work folder under executable directory. This is where we need to access the settings file.
            ' For Windows Applications, we can access from the CurrentUserApplicationData directory as normal.

            Dim _config As Configuration
            Dim _value As String = String.Empty
            Dim fileMap As New ExeConfigurationFileMap()

            Try

                If Environment.GetCommandLineArgs.Length > 1 Then

                    fileMap.ExeConfigFilename = My.Computer.FileSystem.CurrentDirectory & "\" & Environment.GetCommandLineArgs.GetValue(1).ToString & "\appSettings.config"

                Else

                    fileMap.ExeConfigFilename = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\appSettings.config"

                End If

                _config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None)

                _value = _config.AppSettings.Settings.Item(Key).Value.ToString

                Return _value

            Catch ex As Exception

                logger.Info("AppSettings - Get FAILS for key: " & Key & ". Configure the key for the '" & My.Application.Info.ProductName & "' using the System Configuration tools in the IRMA Client.")

                Throw New Exception("The configuration keys document is missing key: " & Key & ". Restart IRMA to fix the problem." & vbCrLf & "If the problem persists, contact your Regional IT Support Team.")

            End Try

        End Function

    End Class

End Namespace
