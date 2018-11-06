Imports WholeFoods.Utility.DataAccess
Imports System.Configuration

Namespace My

    Partial Friend Class MyApplication

        Private factory As DataFactory

        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
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

        Private Sub MyApplication_UnhandledException(ByVal sender As Object, _
                ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException

            MessageBox.Show(e.Exception.Message.ToString & vbCrLf & e.Exception.InnerException.StackTrace, _
                            "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.ExitApplication = True

        End Sub


    End Class

End Namespace

