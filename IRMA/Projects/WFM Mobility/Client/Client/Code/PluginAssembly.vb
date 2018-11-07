Public Class PluginAssembly

    Private _entryPoint As String
    Private _assemblyName As String
    Private _serviceURI As String
    Private _regionCode As String
    Private _userName As String
    Private _userEmail As String
    Private _userAuthenticated As Boolean
    Private _pluginName As String

    Public Property EntryPoint() As String
        Get
            Return _entryPoint
        End Get
        Set(ByVal value As String)
            _entryPoint = value
        End Set
    End Property

    Public Property AssemblyName() As String
        Get
            Return _assemblyName
        End Get
        Set(ByVal value As String)
            _assemblyName = value
        End Set
    End Property

    Public Property ServiceURI() As String
        Get
            Return _serviceURI
        End Get
        Set(ByVal value As String)
            _serviceURI = value
        End Set
    End Property

    Public Property RegionCode() As String
        Get
            Return _regionCode
        End Get
        Set(ByVal value As String)
            _regionCode = value
        End Set
    End Property

    Public Property UserName() As String
        Get
            Return _userName
        End Get
        Set(ByVal value As String)
            _userName = value
        End Set
    End Property

    Public Property UserEmail() As String
        Get
            Return _userEmail
        End Get
        Set(ByVal value As String)
            _userEmail = value
        End Set
    End Property

    Public Property UserAuthenticated() As Boolean
        Get
            Return _userAuthenticated
        End Get
        Set(ByVal value As Boolean)
            _userAuthenticated = value
        End Set
    End Property

    Public Property PluginName() As String
        Get
            Return _PluginName
        End Get
        Set(ByVal value As String)
            _PluginName = value
        End Set
    End Property

    Sub New()

    End Sub

End Class

