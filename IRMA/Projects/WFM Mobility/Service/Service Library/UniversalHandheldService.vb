Imports System.Reflection

Imports UniversalHandheldServiceLibrary.Enumerations

Namespace UniversalHandheldServiceLibrary

    <ServiceBehavior(InstanceContextMode:=InstanceContextMode.PerSession)> _
    Public Class UniversalHandheldService
        Implements IUniversalHandheldService

#Region " Service Members"

        Private regions As New List(Of Region)

        ''' <summary>
        ''' Authenticates a username and password against Active Directory.
        ''' </summary>
        ''' <param name="Username">Domain Username</param>
        ''' <param name="UserPassword">Domain Password</param>
        ''' <param name="EncryptedPassword">Indicates if the password is encrypted, requiring decryption prior to attempting to authenticate against AD.</param>
        ''' <param name="RegionCode">The two character identifer for the region.</param>
        ''' <param name="PluginName">The name of the plugin that is calling for authentication.</param>
        ''' <returns>String value with user's email address (successful authentication) or an error message indicating the reason authentication failed.</returns>
        ''' <remarks>This method should be called by</remarks>
        Public Function AuthenticateUser(ByVal Username As String, ByVal UserPassword As String, ByVal EncryptedPassword As Boolean, ByVal RegionCode As String, ByVal PluginName As String) As String Implements IUniversalHandheldService.AuthenticateUser

            ' we're just giving back a response here, no special message, just a boolean value
            Dim _ad As New ActiveDirectory(Username, UserPassword, EncryptedPassword)

            _ad.RegionCode = RegionCode
            _ad.PluginName = PluginName

            If RegionCode = String.Empty Then
                Return AuthenticationResponse.RegionNotSpecified.ToString
            ElseIf PluginName = String.Empty Then
                Return AuthenticationResponse.PluginNotSpecified.ToString
            ElseIf Not ConfigurationIO.IsRegionConfigured(RegionCode) Then
                Return AuthenticationResponse.RegionNotConfigured.ToString
            ElseIf ConfigurationIO.PluginCount(RegionCode) = 0 Then
                Return AuthenticationResponse.NoPluginsAvailable.ToString
            Else
                _ad.Authenticate()
            End If

            Return _ad.AuthenticationMessage

        End Function

        ''' <summary>
        ''' Returns the regional plugin configuration to the handheld application.
        ''' </summary>
        ''' <param name="UserRegion">Handheld User's Region code</param>
        ''' <returns>An array of plugin applications configured for the specified region.</returns>
        ''' <remarks></remarks>
        Public Function GetRegionConfiguration(ByVal UserRegion As Region) As List(Of Plugin) Implements IUniversalHandheldService.GetRegionConfiguration

            Dim _plugins As New List(Of Plugin)
            Dim _plugin As Plugin

            ' now let's assemble our list of applications to return
            Dim _xDoc As XDocument = Nothing

            ' load up the configuraiton file
            _xDoc = XDocument.Load(ConfigurationIO.ConfigurationDocument)

            ' pull the regional configuration
            Dim _regionalList = From region In _xDoc...<Region> _
                                Where region.@id = UserRegion.ToString

            ' pull the apps for the region
            Dim _appList = From apps In _regionalList...<Application>

            ' loop through the apps and create our return list
            For Each _app In _appList

                _plugin = New Plugin

                _plugin.AuthenticateUser = _app.@authenticateUser
                _plugin.Name = _app.@name
                _plugin.Description = _app.<Description>.Value
                _plugin.IsAuthorized = _app.@isAuthorized
                _plugin.HasService = _app.@hasService
                _plugin.Type = _app.@type

                _plugin.ExePath = _app.<ExePath>.Value
                _plugin.ExeVersion = _app.<ExePath>.@version
                _plugin.ServicePath = _app.<Service>.Value
                _plugin.ServiceVersion = _app.<Service>.@version

                _plugin.AssemblyName = _app.<Assembly>.@name
                _plugin.AssemblyFile = _app.<Assembly>.@file
                _plugin.AssemblyVersion = _app.<Assembly>.@version
                _plugin.AssemblyEntryPoint = _app.<Assembly>.@entryPoint
                _plugin.UpdateEnabled = _app.<Update>.@enabled
                _plugin.UpdateVersion = _app.<Update>.@version
                _plugin.UpdateURI = _app.<Update>.<Location>.Value
                _plugin.UpdateFile = _app.<Update>.<File>.Value

                _plugins.Add(_plugin)

            Next

            Return _plugins

        End Function

        ''' <summary>
        ''' Returns an enumerated list of region codes.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function GetRegions() As System.Collections.Generic.List(Of Region) Implements IUniversalHandheldService.GetRegions

            ' run through the list if enumerated region values and pass them back to the caller
            Dim enumType As Type = GetType(Region)
            Dim enumRegions() As FieldInfo = enumType.GetFields()

            Dim results() As Object = Nothing
            Dim max_result As Integer = -1

            For Each enumRegion In enumRegions

                If enumRegion.IsLiteral Then
                    regions.Add(enumRegion.GetValue(Nothing))
                End If

            Next

            Return regions

        End Function

        ''' <summary>
        ''' Returns a list of configuration keys for the WFM Mobile client and plugins.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetConfigurationKeys() As List(Of ConfigurationKey) Implements IUniversalHandheldService.GetConfigurationKeys

            Dim _keys As New List(Of ConfigurationKey)
            Dim _key As ConfigurationKey

            Dim _xDoc As XDocument = Nothing

            ' load up the configuraiton file
            _xDoc = XDocument.Load(ConfigurationIO.ConfigurationDocument)

            ' pull the keys out of the document
            Dim _keyList = From AppSettings In _xDoc...<AppSettings>.<add>

            ' loop through the apps and create our return list
            For Each key In _keyList

                _key = New ConfigurationKey

                _key.Key = key.@key
                _key.Value = key.@value

                _keys.Add(_key)

            Next

            Return _keys

        End Function

        ''' <summary>
        ''' Returns the available updates for any aplications that should be installed on all devices
        ''' regardless of region. This includes the WFM Mobile Client and AutoUpdate applications.
        ''' </summary>
        ''' <returns>A DeviceUpdate object when an update is available, otherwise Nothing</returns>
        ''' <remarks></remarks>
        Public Function GetDeviceUpdates() As List(Of DeviceUpdate) Implements IUniversalHandheldService.GetDeviceUpdates

            Dim _entries As New List(Of DeviceUpdate)
            Dim _entry As DeviceUpdate

            Dim _xDoc As XDocument = Nothing

            ' load up the configuration file
            _xDoc = XDocument.Load(ConfigurationIO.ConfigurationDocument)

            Dim _entryList = From Update In _xDoc...<WFMUHS>.<Update> _
                             Where Update.@enabled = True

            If _entryList.Count > 0 Then

                For Each entry In _entryList

                    _entry = New DeviceUpdate

                    _entry.UpdateName = entry.@name
                    _entry.UpdateDevicePath = entry.<DevicePath>.Value
                    _entry.UpdateEnabled = entry.@enabled
                    _entry.UpdateVersion = entry.@version
                    _entry.UpdateURI = entry.<Location>.Value
                    _entry.UpdateFile = entry.<File>.Value

                    _entries.Add(_entry)

                Next

                Return _entries

            Else

                Return Nothing

            End If

        End Function

#End Region

    End Class

End Namespace
