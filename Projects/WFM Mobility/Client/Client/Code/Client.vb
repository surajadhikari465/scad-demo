''' <summary>
''' A class for interacting with the Universal Handheld Service.
''' </summary>
''' <remarks></remarks>
Public Class Client

    Shared result As String = ""
    Shared binding As System.ServiceModel.Channels.Binding = UniversalHandheldServiceClient.CreateDefaultBinding
    Shared address As Uri = New Uri(ConfigurationManager.AppSettings("ServiceURI").ToString)
    Shared m_proxy As UniversalHandheldServiceClient = New UniversalHandheldServiceClient(binding, New System.ServiceModel.EndpointAddress(address))

#Region " Constructors"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region " Public Methods"

    ''' <summary>
    ''' Returns a list of plugins available for the specified region from the Universal Handheld Service.
    ''' </summary>
    ''' <param name="RegionCode">2-letter region code identifier</param>
    ''' <returns>Array</returns>
    ''' <remarks></remarks>
    Public Shared Function GetPluginList(ByVal RegionCode As Region) As Plugin()

        Return m_proxy.GetRegionConfiguration(RegionCode)

    End Function

    ''' <summary>
    ''' Returns a list of available 2-letter region codes from the Universal Handheld Service.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetRegionList() As Region()

        Return m_proxy.GetRegions()

    End Function

    ''' <summary>
    ''' Authenticates the username and password provided against Active Directory using the Universal Handheld Service.
    ''' </summary>
    ''' <param name="Username"></param>
    ''' <param name="Password"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AuthenticateUser(ByVal Username As String, ByVal Password As String, ByVal RegionCode As String, ByVal PluginName As String) As String

        Dim eCrypt As New Encryption.Encryptor

        Password = eCrypt.Encrypt(Password)

        Return m_proxy.AuthenticateUser(Username, Password, True, RegionCode, PluginName)

    End Function

    ''' <summary>
    ''' Returns a list of configuration key/value pairs.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetConfigurationKeys() As ConfigurationKey()

        Return m_proxy.GetConfigurationKeys()

    End Function

    ''' <summary>
    ''' Returns the available client updates.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDeviceUpdates() As DeviceUpdate()

        Return m_proxy.GetDeviceUpdates()

    End Function

#End Region

End Class
