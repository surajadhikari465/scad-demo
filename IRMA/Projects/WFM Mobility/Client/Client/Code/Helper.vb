Imports System.Reflection

Imports WholeFoods.Mobile.Client.Enumerations

''' <summary>
''' A class for global application needs for the WFM Mobile client.
''' </summary>
''' <remarks></remarks>
Public Class Helper

    Shared assem As Assembly = Assembly.GetExecutingAssembly()
    Shared assemName As AssemblyName = assem.GetName()
    Shared ver As Version = assemName.Version

#Region " Public Properties"

    ''' <summary>
    ''' The relative path to the directory containing plugin assemblies.
    ''' </summary>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property PLUGIN_ASSEMBLY_PATH() As String
        Get
            Return IO.Path.GetDirectoryName(assemName.CodeBase) & My.Resources.Path_Plugins
        End Get
    End Property

    ''' <summary>
    ''' Returns the application installation path on the device.
    ''' </summary>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property ApplicationPath() As String
        Get
            Return IO.Path.GetDirectoryName(assemName.CodeBase)
        End Get
    End Property

    ''' <summary>
    ''' Returns the application assembly version.
    ''' </summary>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property ClientVersion() As String
        Get
            Return ver.ToString
        End Get
    End Property

#End Region

#Region " Public Methods"

    ''' <summary>
    ''' Validates that the specified plugin exist.
    ''' </summary>
    ''' <param name="Type">PluginType</param>
    ''' <param name="AssemblyName">The assembly dll file name.</param>
    ''' <param name="ExecutablePath">The absolute path to the external executable.</param>
    ''' <returns>True or False, indicating whether the plugin executable or assembly exixts.</returns>
    ''' <remarks></remarks>
    Public Shared Function PluginExists(ByVal Type As Enumerations.PluginType, Optional ByVal AssemblyName As String = Nothing, Optional ByVal ExecutablePath As String = Nothing) As Boolean

        If Type = Enumerations.PluginType.Assembly Then

            Return IO.File.Exists(PLUGIN_ASSEMBLY_PATH() & AssemblyName)

        Else

            Return IO.File.Exists(ExecutablePath)

        End If

    End Function

    ''' <summary>
    ''' Loads the plugin assembly into memory and opens the plugin Main Form.
    ''' </summary>
    ''' <param name="Plugin">A Plugin object containing the setting values to pass to the plugin assembly.</param>
    ''' <remarks></remarks>
    Public Shared Sub OpenPlugin(ByVal Plugin As PluginAssembly)

        Dim _path As String = Helper.PLUGIN_ASSEMBLY_PATH & Plugin.AssemblyName

        Dim a As [Assembly] = [Assembly].LoadFrom(_path)
        Dim mytypes As Type() = a.GetTypes()
        Dim flags As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Public Or BindingFlags.Static Or _
            BindingFlags.Instance Or BindingFlags.DeclaredOnly

        Dim t As Type
        For Each t In mytypes

            Dim obj As [Object] = Nothing

            If t.FullName.Equals(Plugin.EntryPoint) Then

                obj = Activator.CreateInstance(t)

                Dim pi As PropertyInfo

                ' pass the plugin its property values
                pi = t.GetProperty("ServiceURI")
                pi.SetValue(obj, Plugin.ServiceURI, Nothing)

                pi = t.GetProperty("RegionCode")
                pi.SetValue(obj, Plugin.RegionCode, Nothing)

                pi = t.GetProperty("UserAuthenticated")
                pi.SetValue(obj, Plugin.UserAuthenticated, Nothing)

                pi = t.GetProperty("UserName")
                pi.SetValue(obj, Plugin.UserName, Nothing)

                pi = t.GetProperty("UserEmail")
                pi.SetValue(obj, Plugin.UserEmail, Nothing)

                pi = t.GetProperty("PluginName")
                pi.SetValue(obj, Plugin.PluginName, Nothing)

                Dim frm As Form = CType(obj, Form)

                frm.ShowDialog()

                frm.Dispose()

                Cursor.Current = Cursors.Default

                Exit For

            End If

        Next t

    End Sub

    ''' <summary>
    ''' Returns the assembly or executable version number.
    ''' </summary>
    ''' <param name="DevicePath">The full path, including executable/assembly name.</param>
    ''' <returns>String containing the version number.</returns>
    ''' <remarks>If the file is not present on the device, a "0" is returned.</remarks>
    Public Shared Function GetComponentVersion(ByVal DevicePath As String) As Version

        If IO.File.Exists(DevicePath) Then

            Dim a As [Assembly] = [Assembly].LoadFrom(DevicePath)

            Return a.GetName().Version

        Else

            Return New Version("0.0.0.0")

        End If

    End Function

    ''' <summary>
    ''' Launches an executable type plugin.
    ''' </summary>
    ''' <param name="ExecutablePath">The absolute path to the executable plugin, including the executable name.</param>
    ''' <remarks></remarks>
    Public Shared Sub OpenExecutable(ByVal ExecutablePath As String)

        If Helper.PluginExists(PluginType.Executable, Nothing, ExecutablePath) Then

            Process.Start(ExecutablePath, Nothing)

        Else

            MessageBox.Show(My.Resources.MessageBoxText_PluginNotFound & Environment.NewLine & My.Resources.Text_General_Assistance, My.Resources.MessageBoxCaption_MissingPlugin, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Nothing)

        End If

    End Sub

    ''' <summary>
    ''' Returns the current environment label, including the environment name and the executable version number.
    ''' </summary>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Shared Function GetEnvironmentLabel() As String

        Dim _result As String = String.Format("{0} {1} - v.{2}", ConfigurationManager.AppSettings("Region"), ConfigurationManager.AppSettings("Environment"), ver)

        Return _result

    End Function

    ''' <summary>
    ''' Returns the current executable version label text, including the application title and the version number.
    ''' </summary>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Shared Function GetVersionLabel() As String

        Dim _result As String = String.Format(My.Resources.Text_Version, My.Resources.Application_Title, ver.ToString())

        Return _result

    End Function

    ''' <summary>
    ''' Calls WCELOAD.exe to install the specified CAB file.
    ''' </summary>
    ''' <param name="CABPath">The device path of the CAB file to install.</param>
    ''' <remarks></remarks>
    Public Shared Sub InstallCAB(ByVal CABPath As String)

        Dim p As Process = New Process

        p.StartInfo.UseShellExecute = True
        p.StartInfo.FileName = "\Windows\wceload.exe"
        p.StartInfo.Arguments = "/silent /noui " & """" & CABPath & """"
        p.Start()
        p.WaitForExit()

    End Sub

#End Region

End Class
