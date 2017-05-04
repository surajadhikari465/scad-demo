Option Strict Off

Imports WholeFoods.Utility.DataAccess
Imports log4net
Imports System.Reflection

Namespace My
    Partial Friend Class MyApplication

        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
        Private factory As DataFactory
        Private environment As Guid
        Private _userID As Integer

        Private Shadows Enum ManifestType
            Admin
            Client
        End Enum

        Public Property CurrentUserID() As Integer
            Get
                Return Me._userID
            End Get
            Set(ByVal value As Integer)
                Me._userID = value
            End Set
        End Property

        Public ReadOnly Property IsProduction() As Boolean
            Get
                If environment.ToString.ToUpper.Equals("8A5FF21E-9ACE-403E-8BFB-7338C71151F5") Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Private _envName As String
        Public ReadOnly Property EnvironmentName() As String
            Get
                Return _envName
            End Get
        End Property

        Private _region As String
        Public ReadOnly Property Region() As String
            Get
                Return _region
            End Get
        End Property

        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            Try

                ' initiate log4net
                log4net.Config.XmlConfigurator.Configure()

                logger.Info("MyApplication_Startup - Enter")

                Dim _appID As Guid
                Dim _envID As Guid

                Try

                    ' get the app.config GUIDs that identify the application and environment to connect to
                    _appID = New Guid(ConfigurationManager.AppSettings("ApplicationGUID").ToString)
                    _envID = New Guid(ConfigurationManager.AppSettings("EnvironmentGUID").ToString)

                    environment = _envID

                    logger.Info("MyApplication_Startup - AppSettings - Get ApplicationGUID = " & _appID.ToString.ToUpper)
                    logger.Info("MyApplication_Startup - AppSettings - Get EnvironmentGUID = " & _envID.ToString.ToUpper)

                Catch ex As Exception

                    logger.Info("MyApplication_Startup - AppSettings ERROR - Unable to retrieve configuration keys from IRMA.exe.config.")

                    Throw New Exception("Unable to retrieve application and environment keys from IRMA.exe.config.")

                End Try

                ' initiate the factory
                If factory Is Nothing Then
                    factory = New DataFactory(DataFactory.ItemCatalog)
                End If

                ' write the configuration for the application and environment
                factory.WriteConfiguration(_appID, _envID)

                _envName = WholeFoods.Utility.ConfigurationServices.AppSettings("Environment").ToString
                _region = WholeFoods.Utility.ConfigurationServices.AppSettings("Region").ToString

                Try
                    Dim sCreateDesktopShortcut As String

                    Try
                        sCreateDesktopShortcut = WholeFoods.Utility.ConfigurationServices.AppSettings("CreateDesktopShortcut")
                    Catch ex As Exception
                        sCreateDesktopShortcut = ""
                    End Try

                    If sCreateDesktopShortcut = "" Then sCreateDesktopShortcut = "False"

                    If CBool(sCreateDesktopShortcut) Then
                        ' put ClickOnce shortcuts on desktop
                        CheckForShortcut()
                    End If

                Catch ex As Exception
                    ' quietly log and ignore the error
                    logger.Info("MyApplication_Startup - Shortcut Creation ERROR: " & ex.Message)
                End Try
            Catch ex As Exception

                logger.Info("MyApplication_Startup - ERROR: " & ex.Message)

                Throw ex

            Finally

                logger.Info("MyApplication_Startup - Exit")

            End Try

        End Sub

        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            Try
                ' use the Error Dialog with email capability
                ErrorDialog.HandleError(e.Exception, ErrorDialog.NotificationTypes.DialogAndEmail)
            Catch ex As Exception
                ' use the Error Dialog with email capability
                ErrorDialog.HandleError(Nothing, e.Exception.Message, e.Exception.StackTrace, ErrorDialog.NotificationTypes.DialogAndEmail, Nothing)
            Finally
                ' don't shut down the application
                e.ExitApplication = False
                ' iterate through all open forms and close them
                Dim frm As Form
                For Each frm In Windows.Forms.Application.OpenForms
                    If Not frm.Name = "frmMain" Then ' don't close the main form
                        frm.Close()
                    End If
                Next
            End Try
        End Sub

        Private Sub CheckForShortcut()

            Dim shortcutName As String = String.Empty
            Dim desktopPath As String = String.Empty
            Dim appName As String = String.Empty

            'manifest URL has been repurposed to point to the Start Menu\Programs location on the local machine where the IRMA Client ClickOnce shortcut exists 
            ' (e.g. “Whole Foods Market\MW IRMA.appref-ms”)  
            shortcutName = String.Concat(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Programs), "\", WholeFoods.Utility.ConfigurationServices.AppSettings("ManifestURL").ToString)

            ' now get just the app name
            appName = System.IO.Path.GetFileName(shortcutName)

            ' reconstruct the desktop location for the shorcut using the app name
            desktopPath = String.Concat(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "\", appName)

            ' since we removed any dependencies on the network manifest, we'll now want to make sure the source shortcut exists before we do this
            If Not System.IO.File.Exists(desktopPath) And System.IO.File.Exists(shortcutName) Then
                System.IO.File.Delete(System.IO.File.Exists(desktopPath))
                System.IO.File.Copy(shortcutName, desktopPath, True)
            End If

        End Sub

    End Class

End Namespace

