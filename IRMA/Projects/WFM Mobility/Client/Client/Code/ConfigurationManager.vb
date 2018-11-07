Imports System.IO
Imports System.Xml.Linq
Imports System.Reflection
Imports System.Collections.Specialized

''' <summary>
''' Provides access to configuration files for client applications on the .NET Compact Framework.
''' </summary>
Public Class ConfigurationManager

    Private Shared _appSettings As NameValueCollection = New NameValueCollection
    Private Shared _configFile As String

#Region " Public Properties"

    ''' <summary>
    ''' Gets configuration settings in the appSettings section.
    ''' </summary>
    Public Shared ReadOnly Property AppSettings() As NameValueCollection
        Get
            Return _appSettings
        End Get
    End Property

#End Region

#Region " Constructors"

    ''' <summary>
    ''' Static constructor.
    ''' </summary>
    Shared Sub New()

        _configFile = String.Format("{0}.config", System.Reflection.Assembly.GetExecutingAssembly.GetName.CodeBase)

        If Not File.Exists(ConfigurationManager._configFile) Then

            Throw New FileNotFoundException(String.Format("Configuration file ({0}) could not be found.", ConfigurationManager._configFile))

        End If

        Dim _xDoc = XDocument.Load(ConfigurationManager._configFile)

        Dim nodes = From add In _xDoc...<appSettings>.<add>

        For Each addNode In nodes

            AppSettings.Add(addNode.@key, addNode.@value)

        Next

    End Sub

#End Region

#Region " Public Methods"

    ''' <summary>
    ''' Saves changes made to the configuration settings.
    ''' </summary>
    Public Shared Sub Save()

        Dim _configFileInfo As New System.IO.FileInfo(_configFile)

        _configFileInfo.Attributes = _configFileInfo.Attributes And Not FileAttributes.ReadOnly

        Dim _xDoc = <?xml version="1.0" encoding="utf-8"?>
                    <configuration>
                        <appSettings/>
                    </configuration>

        For Each key As String In AppSettings.AllKeys

            Dim entry = <add key=<%= key %> value=<%= AppSettings(key) %>/>

            Dim config = _xDoc.<configuration>.<appSettings>(0)

            config.Add(entry)

        Next

        _xDoc.Save(_configFile)

    End Sub

#End Region

End Class
