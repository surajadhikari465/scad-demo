Imports System.Reflection

Imports WholeFoods.Mobile.Client.Helper
Imports WholeFoods.Mobile.Client.Enumerations
Imports System.Threading
Imports System.IO
Imports Microsoft.Win32

Public Class MainForm

    Private sleepTime As DateTime = Nothing
    Private frmSplash As SplashForm
    Private currentUser As String
    Private currentUserEmail As String

#Region " Private Properties"

    Public WriteOnly Property UpdateSplashStatus()
        Set(ByVal value)

            Try
                frmSplash.Label1.Text = value
                frmSplash.Refresh()
            Catch ex As Exception
            End Try

        End Set
    End Property

#End Region

#Region " Constructors"

    Public Sub New()
        MyBase.New()

        ' initialize and show the plash screen
        Me.frmSplash = New SplashForm
        Me.frmSplash.Owner = Me
        Me.frmSplash.WindowState = FormWindowState.Maximized
        Me.frmSplash.Show()

        Application.DoEvents()

        Me.Enabled = False

        InitializeComponent()

    End Sub

#End Region

#Region " Control Events"

    Private Sub MainForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            ' only do this when the app starts
            If sender Is Me Then
                InstallNetCFMessages()
                LoadConfigurationSettings()
            End If

            ' this handler records the current time the unit went to sleep in the _sleepTime variable
            Dim _powerDown As New OpenNETCF.WindowsCE.DeviceNotification(AddressOf PowerDown)
            AddHandler OpenNETCF.WindowsCE.PowerManagement.PowerDown, _powerDown

            ' this handler records the current time the unit went to sleep in the _sleepTime variable
            Dim _powerIdle As New OpenNETCF.WindowsCE.DeviceNotification(AddressOf PowerIdle)
            AddHandler OpenNETCF.WindowsCE.PowerManagement.PowerIdle, _powerIdle

            ' this handler compares the sleep time to the current time and shuts down the app
            ' if the diff in seconds exceeds the IdleShutdownThreshold app config setting
            Dim _powerUp As New OpenNETCF.WindowsCE.DeviceNotification(AddressOf PowerUp)
            AddHandler OpenNETCF.WindowsCE.PowerManagement.PowerUp, _powerUp

            ' make sure a region has been set
            If ConfigurationManager.AppSettings("Region") = String.Empty Then

                ' the settings form will not close without a setting being set
                SettingsForm.ShowDialog()
                SettingsForm.Hide()

            End If

            lblEnvironment.Text = Helper.GetEnvironmentLabel()

            LoadPlugins(ConfigurationManager.AppSettings("Region"))

            Try
                DoUpdateCheck()
            Catch ex As Exception
                ' something went wrong
                MessageBox.Show(String.Format(My.Resources.MessageBoxText_UnhandledException, ex.Message), My.Resources.MessageBoxCaption_UnhandledException, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Nothing)
                Application.Exit()
            End Try

            If ucContainer.Controls.Count = 0 Then

                ' no plugins were found for the selected region - notify the user and tell them what to do
                Dim _label As New Label
                _label.Text = String.Format(My.Resources.LabelText_Error_NoPlugin, ConfigurationManager.AppSettings("Region").ToString) & _
                            Environment.NewLine & _
                            Environment.NewLine & _
                            My.Resources.Text_General_Assistance

                _label.Size = New Size(180, 180)

                ucContainer.Controls.Add(_label)

                ' plugins are not available, set the title appropriately
                lblTitle.Text = My.Resources.LableText_Title_MainForm_NoPlugin

            Else

                ' plugins are available, set the title appropriately
                lblTitle.Text = My.Resources.LabelText_Title_MainForm_Default

            End If

        Catch ex As Exception

            ' something went wrong trying to contact the service - notify the user and shut down
            MessageBox.Show(String.Format(My.Resources.MessageBoxText_UnhandledException, ex.Message), My.Resources.MessageBoxCaption_UnhandledException, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Nothing)
            Application.Exit()

        End Try

        Me.Text = My.Resources.Application_Title

        Me.Enabled = True
        Me.Visible = True

        ' close the splash screen
        Me.frmSplash.Close()

        Cursor.Current = Cursors.Default

    End Sub

    ''' <summary>
    ''' A custom handler for the plugin button Click event.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PluginButton_Click(ByVal sender As PluginButton, ByVal e As EventArgs)

        Dim _authenticated As Boolean = False

        ' make sure the plugin exists
        If Not Helper.PluginExists(sender.PluginType, sender.PluginAssemblyName, sender.PluginExecutablePath) Then

            MessageBox.Show(My.Resources.MessageBoxText_PluginNotFound & Environment.NewLine & Environment.NewLine & My.Resources.Text_General_Assistance, My.Resources.MessageBoxCaption_MissingPlugin, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Nothing)

            Exit Sub

        End If

        ' if the plugin requires AD authentication, do it now
        If sender.AuthenticateUser Then

            Dim _authForm As New AuthenticateForm
            _authForm.PluginCaller = sender

            If Not String.IsNullOrEmpty(currentUser) Then
                _authForm.txtUserName.Text = currentUser
            End If

            Dim response As Windows.Forms.DialogResult = _authForm.ShowDialog()

            ' anything but a Yes response back from the authentication form means authentication failed
            If response = Windows.Forms.DialogResult.Yes Then

                _authenticated = True
                currentUser = _authForm.txtUserName.Text
                currentUserEmail = _authForm.UserEmail

            Else

                _authenticated = False

            End If

            _authForm.Dispose()

        Else

            ' authentication is not required
            _authenticated = True

        End If

        If _authenticated Then

            If sender.PluginType = Enumerations.PluginType.Assembly Then

                ' open the selected plugin assembly

                Dim _plugin As New PluginAssembly
                _plugin.RegionCode = ConfigurationManager.AppSettings("Region")
                _plugin.ServiceURI = sender.PluginServiceURI
                _plugin.UserAuthenticated = _authenticated
                _plugin.UserEmail = currentUserEmail
                _plugin.UserName = currentUser
                _plugin.EntryPoint = sender.PluginEntryPoint
                _plugin.AssemblyName = sender.PluginAssemblyName
                _plugin.PluginName = sender.PluginName

                Try

                    OpenPlugin(_plugin)

                Catch ex As Exception

                    ' error opening the plugin assembly - don't crash WFM Mobile!
                    MessageBox.Show(String.Format(My.Resources.MessageBoxText_UnhandledException, ex.Message), My.Resources.MessageBoxCaption_UnhandledException, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Nothing)

                End Try

            ElseIf sender.PluginType = Enumerations.PluginType.Executable Then

                ' open the selectedplugin executale
                OpenExecutable(sender.PluginExecutablePath)

            End If

        End If

    End Sub

    Private Sub mnuAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAbout.Click

        MessageBox.Show(Helper.GetVersionLabel(), My.Resources.MessageBoxCaption_VersionCaption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, Nothing)

    End Sub

    Private Sub mnuSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSettings.Click

        Dim frmLocation As New SettingsForm

        frmLocation.ShowDialog()

        frmLocation.Dispose()

        ' reload the plugins in case the user changed regions
        MainForm_Load(sender, e)

    End Sub

    Private Sub mnuExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuExit.Click

        Application.Exit()

    End Sub

#End Region

#Region " Private Methods"

    ''' <summary>
    ''' Fires when the unit wakes up. If the sleep time + the idle allowance in seconds is less than or = the current time
    ''' the application automatically exits to kill any open authenticated plugin sessions.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PowerUp()

        ' WinMo6 weirdness - going to sleep fires PowerUp? so we have to catch it and make sure it doesn't exit the
        ' program every time the thing goes to sleep or the power saver turns off the screen.
        If sleepTime = Nothing Then

            sleepTime = TimeOfDay

        Else

            If sleepTime.AddSeconds(ConfigurationManager.AppSettings("IdleShutdownThreshold")) <= TimeOfDay Then

                Application.Exit()

            Else

                sleepTime = Nothing

            End If

        End If

    End Sub

    ''' <summary>
    ''' Fires when the unit goes to sleep or is turned off. It record the sleep/off for the PowerUp event.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PowerDown()

        sleepTime = TimeOfDay

    End Sub

    ''' <summary>
    ''' Fires when the unit goes idle. It record the sleep/off for the PowerUp event.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PowerIdle()

        sleepTime = TimeOfDay

    End Sub

    ''' <summary>
    ''' Loads the plugin buttons on the Main Form.
    ''' </summary>
    ''' <param name="RegionCode">The two-character identifier for the region.</param>
    ''' <remarks></remarks>
    Private Sub LoadPlugins(ByVal RegionCode As String)

        Me.UpdateSplashStatus = "Getting plugin list..."

        PluginManifest.Clear()

        ucContainer.Controls.Clear()

        ' get a list of plugins from the universal handheld service
        Dim _appList() As Plugin = Client.GetPluginList(CType([Enum].Parse(GetType(Region), RegionCode, True), Region))

        Dim _count As Integer = _appList.Count

        ' setup the start position for the first button
        Dim x As Int16 = 3
        Dim y As Int16 = 3
        Dim dp As New Drawing.Point(x, y)

        Dim _button As PluginButton = Nothing

        ' if there are plugins to add, start adding them
        If _count > 0 Then

            Me.UpdateSplashStatus = "Loading plugins..."

            Dim i As Integer = 1

            For Each _app In _appList

                _button = New PluginButton

                _button.AuthenticateUser = _app.AuthenticateUser
                _button.Text = _app.Name
                _button.Name = "btn" & _count
                _button.Size = New System.Drawing.Size(75, 25)
                _button.PluginName = _app.Name
                _button.PluginType = CType([Enum].Parse(GetType(PluginType), _app.Type, True), PluginType)
                _button.PluginAssemblyName = _app.AssemblyFile
                _button.PluginEntryPoint = _app.AssemblyEntryPoint
                _button.PluginExecutablePath = _app.ExePath
                _button.PluginServiceURI = _app.ServicePath

                dp.X = x
                dp.Y = y

                _button.Location = dp

                ' create a handler for this button
                AddHandler _button.Click, AddressOf PluginButton_Click

                ' add the button to the panel
                ucContainer.Controls.Add(_button)

                If i = 6 Then
                    ' when we hit the 6th plugin, start a new column
                    x = 80
                    y = 3

                ElseIf i = 12 Then

                    x = 157
                    y = 3

                ElseIf i > 17 Then

                    ' that's more entries than we can accomodate right now

                Else
                    ' setup the next button position in the first column
                    y = y + 30

                End If

                ' increment the counter
                i = i + 1

                ' add the plugin to the manifest
                Dim fPath As String = String.Empty

                If CType([Enum].Parse(GetType(PluginType), _app.Type, True), PluginType) = PluginType.Assembly Then
                    fPath = Helper.PLUGIN_ASSEMBLY_PATH
                Else
                    fPath = _app.ExePath
                End If

                Dim entry = <Plugin name=<%= _app.Name %> type=<%= _app.Type %> updateTo=<%= _app.UpdateVersion %> updateEnabled=<%= _app.UpdateEnabled %>>
                                <DevicePath><%= fPath %></DevicePath>
                                <UpdateURI><%= _app.UpdateURI %></UpdateURI>
                                <UpdateFileName><%= _app.UpdateFile %></UpdateFileName>
                            </Plugin>

                PluginManifest.Add(entry)

            Next

        End If

        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi

    End Sub

    ''' <summary>
    ''' Loads the app config settings hosted on the remote server and replaces the local settings values is specified to do so.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadConfigurationSettings()

        Me.UpdateSplashStatus = "Loading configuration..."

        ' setup our values to be retained in the registry
        Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\WFM Mobile", True)

        If regKey Is Nothing Then

            regKey = Registry.CurrentUser.CreateSubKey("Software\WFM Mobile")
            regKey.SetValue("Region", "")
            regKey.SetValue("Version", Helper.ClientVersion.ToString)

        Else

            regKey = Registry.CurrentUser.CreateSubKey("Software\WFM Mobile")
            regKey.SetValue("Version", Helper.ClientVersion.ToString)

        End If

        Registry.CurrentUser.Close()

        regKey = Registry.CurrentUser.OpenSubKey("Software\WFM Mobile", True)
        Dim reInstallUpdater As String = regKey.GetValue("ReinstallUpdater")

        If Registry.LocalMachine.OpenSubKey("Software\Apps\Whole Foods Market AutoUpdater") Is Nothing Or reInstallUpdater = "1" Then

            regKey.SetValue("ReinstallUpdater", InstallAutoUpdater().ToString)

        End If

        regKey.Close()

        Me.frmSplash.Label1.Text = "Loading..."

        ' get all the available configuration keys from the service
        Dim _keyList() As ConfigurationKey = Client.GetConfigurationKeys()

        ' we only want to overwrite the existing values if the service tells us to
        Dim _overWriteValues = From ck In _keyList _
                                Where ck.Key = "OverwriteConfigOnLoad" _
                                Select CBool(ck.Value)

        For Each _key In _keyList

            ' does the key exist? if it does, then do the overwrite logic
            If ConfigurationManager.AppSettings.AllKeys.Contains(_key.Key) Then

                ' if OverwriteConfigOnLoad = True... 
                If _overWriteValues.First Then

                    'overwrite the value with new values only
                    If Not ConfigurationManager.AppSettings(_key.Key).Equals(_key.Value) Then
                        ConfigurationManager.AppSettings(_key.Key) = _key.Value
                    End If

                End If

            Else

                ' if the key doesn't exist, add it
                ConfigurationManager.AppSettings.Add(_key.Key, _key.Value)

            End If

        Next

        ' pull in the last one so we don't ask again after an app update
        regKey = Registry.CurrentUser.OpenSubKey("Software\WFM Mobile", True)
        Dim region As String = regKey.GetValue("Region")

        If region = "TS" Then region = ""

        regKey.Close()

        ConfigurationManager.AppSettings("Region") = region

        ' save the configuration locally
        ConfigurationManager.Save()

    End Sub

    ''' <summary>
    ''' Checks for available updates and creates the update manifest for the AutoUpdater aplication.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DoUpdateCheck()

        Dim updateRequired As Boolean = False
        Dim currVersion As Version

        ' first clear the update manifest of any previous update information
        UpdateManifest.Clear()

        Me.UpdateSplashStatus = "Checking for updates..."

        ' first let's go through our plugin list and see if there are any updates to be done
        If File.Exists(PluginManifest.ManifestFile) Then

            Dim xDoc As XDocument = XDocument.Load(PluginManifest.ManifestFile)

            Dim list = From Plugin In xDoc.<Plugins>.<Plugin> _
                       Where Plugin.@updateEnabled = True

            For Each item In list

                If item.@type = "Assembly" Then
                    currVersion = Helper.GetComponentVersion(item.<DevicePath>.Value & "\" & item.<UpdateFileName>.Value)
                Else
                    currVersion = Helper.GetComponentVersion(item.<DevicePath>.Value)
                End If

                If New Version(item.@updateTo).CompareTo(currVersion) > 0 Then

                    Dim entry = <Update name=<%= item.@name %> type=<%= item.@type %> downloaded="0">
                                    <Location><%= item.<UpdateURI>.Value %></Location>
                                    <File><%= item.<UpdateFileName>.Value %></File>
                                </Update>

                    UpdateManifest.Add(entry)

                    updateRequired = True

                End If

            Next

        End If

        ' now let's check the device updates
        Dim updates As DeviceUpdate() = Client.GetDeviceUpdates

        If updates IsNot Nothing Then

            For Each deviceUpdate In updates

                currVersion = Helper.GetComponentVersion(deviceUpdate.UpdateDevicePath)

                If New Version(deviceUpdate.UpdateVersion).CompareTo(currVersion) > 0 Then

                    ' an update is needed so add it to the manifest
                    Dim entry = <Update name=<%= deviceUpdate.UpdateName %> type="Executable" downloaded="0">
                                    <Location><%= deviceUpdate.UpdateURI %></Location>
                                    <File><%= deviceUpdate.UpdateFile %></File>
                                </Update>

                    UpdateManifest.Add(entry)

                    updateRequired = True

                End If

            Next

        End If

        ' if any components need updating, inform the user
        If updateRequired Then

            Dim resp As DialogResult = MessageBox.Show(My.Resources.MessageBoxText_UpdateNeed, My.Resources.MessageBoxCaption_UpdateNeeded, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)

            If resp = Windows.Forms.DialogResult.OK Then

                Me.UpdateSplashStatus = "Launching AutoUpdate..."

                ' launch the updater
                Helper.OpenExecutable(ApplicationPath & "\Update.exe")

                Application.Exit()

            Else

                ' the updates are required and the user didn't tap OK so shut down until they do
                Application.Exit()

            End If

        End If

    End Sub

    ''' <summary>
    ''' Installs the auto-updater application and any subsequent updates to it.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InstallAutoUpdater() As Integer

        ' create the temp dirs for the plugin manifest and the autoupdate manifest
        If Not IO.Directory.Exists(UpdateManifest.UpdatesPath) Then
            Directory.CreateDirectory(UpdateManifest.UpdatesPath)
        End If

        Cursor.Current = Cursors.WaitCursor

        Dim p As Process = New Process

        Try

            Me.UpdateSplashStatus = "Configuring AutoUpdater..."

            Dim _fileInfo As FileInfo

            If File.Exists(UpdateManifest.UpdatesPath & My.Resources.Path_AutoUpdateCAB) Then
                _fileInfo = New System.IO.FileInfo(UpdateManifest.UpdatesPath & My.Resources.Path_AutoUpdateCAB)
                _fileInfo.Attributes = _fileInfo.Attributes And Not FileAttributes.ReadOnly
            End If

            File.Copy(ApplicationPath & My.Resources.Path_AutoUpdateCAB, UpdateManifest.UpdatesPath & My.Resources.Path_AutoUpdateCAB, True)

            _fileInfo = New System.IO.FileInfo(UpdateManifest.UpdatesPath & My.Resources.Path_AutoUpdateCAB)
            _fileInfo.Attributes = _fileInfo.Attributes And Not FileAttributes.ReadOnly

            Application.DoEvents()

            InstallCAB(UpdateManifest.UpdatesPath & My.Resources.Path_AutoUpdateCAB)

            Cursor.Current = Cursors.Default

            Return 0

        Catch w As System.ComponentModel.Win32Exception

            Dim e As New Exception()
            e = w.GetBaseException()
            MessageBox.Show(e.Message)

            Application.Exit()

        End Try

    End Function

    ''' <summary>
    ''' Installs additional .NETCF 3.5 resources for more descriptive exception messages.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InstallNetCFMessages()

        Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey("Software\Apps\Microsoft .NET CF 3.5 EN-String Resource")

        If regKey Is Nothing Then

            Dim netCfUpdateCab As String = String.Empty

            Dim p As Process = New Process

            Try

                Me.UpdateSplashStatus = "Installing Diagnostics..."

                Application.DoEvents()

                If Environment.OSVersion.Version.Major >= 5 Then

                    InstallCAB(Helper.ApplicationPath & My.Resources.Path_WinMobileNetCFMsgs)

                Else

                    InstallCAB(Helper.ApplicationPath & My.Resources.Path_PPCNetCFMsgs)

                End If

            Catch w As System.ComponentModel.Win32Exception

                Dim e As New Exception()
                e = w.GetBaseException()
                MessageBox.Show(e.Message)

                Application.Exit()

            End Try

            Cursor.Current = Cursors.Default

            Me.frmSplash.Label1.Text = "Loading..."

        End If

    End Sub

#End Region

End Class
