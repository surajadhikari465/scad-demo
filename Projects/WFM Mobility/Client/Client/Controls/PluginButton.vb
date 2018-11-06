''' <summary>
''' A custom button control extended with plugin specific properties.
''' </summary>
''' <remarks></remarks>
Public Class PluginButton
    Inherits Windows.Forms.Button

    Private _pluginName As String
    Private _pluginAssemblyEntryPoint As String
    Private _pluginAssemblyName As String
    Private _pluginType As Enumerations.PluginType
    Private _pluginExecutablePath As String
    Private _pluginServiceURI As String
    Private _authenticateUser As Boolean

#Region " Public Properties"

    ''' <summary>
    ''' Determines whether the specified plugin requires domain authentication against Active Directory before initiating
    ''' the plugin assembly.
    ''' </summary>
    ''' <value>True if authentication is required, otherwise False.</value>
    ''' <returns>True if authentication is required, otherwise False.</returns>
    ''' <remarks><see cref="Enumerations.PluginType.Executable"/> type plugins should set this property to False.</remarks>
    Public Property AuthenticateUser() As Boolean
        Get
            Return _authenticateUser
        End Get
        Set(ByVal value As Boolean)
            _authenticateUser = value
        End Set
    End Property

    ''' <summary>
    ''' The type of plugin this button represents.
    ''' </summary>
    ''' <value><see cref="Enumerations.PluginType"/></value>
    ''' <returns><see cref="Enumerations.PluginType"/></returns>
    Public Property PluginType() As Enumerations.PluginType
        Get
            Return _pluginType
        End Get
        Set(ByVal value As Enumerations.PluginType)
            _pluginType = value
        End Set
    End Property

    ''' <summary>
    ''' The string value for the Button.Text property of the plugin's button.
    ''' </summary>
    Public Property PluginName() As String
        Get
            Return _pluginName
        End Get
        Set(ByVal value As String)
            _pluginName = value
        End Set
    End Property

    ''' <summary>
    ''' The file name of the plugin assembly, including file extension.
    ''' </summary>
    Public Property PluginAssemblyName() As String
        Get
            Return _pluginAssemblyName
        End Get
        Set(ByVal value As String)
            _pluginAssemblyName = value
        End Set
    End Property

    ''' <summary>
    ''' The classname of the Windows.Forms.Form to open when loading the plugin, including namespace.
    ''' </summary>
    Public Property PluginEntryPoint() As String
        Get
            Return _pluginAssemblyEntryPoint
        End Get
        Set(ByVal value As String)
            _pluginAssemblyEntryPoint = value
        End Set
    End Property

    ''' <summary>
    ''' The full device path to the external executable application, including the executable name and extension.
    ''' </summary>
    Public Property PluginExecutablePath() As String
        Get
            Return _pluginExecutablePath
        End Get
        Set(ByVal value As String)
            _pluginExecutablePath = value
        End Set
    End Property

    ''' <summary>
    ''' The WCF service URL used by the plugin.
    ''' </summary>
    Public Property PluginServiceURI() As String
        Get
            Return _pluginServiceURI
        End Get
        Set(ByVal value As String)
            _pluginServiceURI = value
        End Set
    End Property

#End Region

End Class
