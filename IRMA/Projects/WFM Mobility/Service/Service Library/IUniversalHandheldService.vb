
Namespace UniversalHandheldServiceLibrary

    <ServiceContract()> _
    Public Interface IUniversalHandheldService

        ''' <summary>
        ''' Authenticates a username and password against Active Directory.
        ''' </summary>
        ''' <param name="Username">Domain Username</param>
        ''' <param name="Password">Domain Password</param>
        ''' <param name="EncryptedPassword">Indicates if the password is encrypted, requiring decryption prior to attempting to authenticate against AD.</param>
        ''' <param name="RegionCode">The Region Code set on the hand held device.</param>
        ''' <param name="PluginName">The name of the calling plugin.</param>
        ''' <returns>String value with user's email address (successful authentication) or an error message indicating the reason authentication failed.</returns>
        ''' <remarks></remarks>
        <OperationContract()> _
        Function AuthenticateUser(ByVal Username As String, ByVal Password As String, ByVal EncryptedPassword As Boolean, ByVal RegionCode As String, ByVal PluginName As String) As String

        ''' <summary>
        ''' Returns the regional plugin configuration to the handheld application.
        ''' </summary>
        ''' <param name="Region">Handheld User's Region code</param>
        ''' <returns>An array of plugin applications configured for the specified region.</returns>
        ''' <remarks></remarks>
        <OperationContract()> _
        Function GetRegionConfiguration(ByVal Region As Region) As List(Of Plugin)

        ''' <summary>
        ''' Returns an enumerated list of region codes.
        ''' </summary>
        ''' <remarks></remarks>
        <OperationContract()> _
        Function GetRegions() As List(Of Region)

        ''' <summary>
        ''' Returns the configuration key/value pairs.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <OperationContract()> _
        Function GetConfigurationKeys() As List(Of ConfigurationKey)

        ''' <summary>
        ''' Returns the available updates for any aplications that should be installed on all devices
        ''' regardless of region. This includes the WFM Mobile Client and AutoUpdate applications.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>If not update is available, the return list is empty.</remarks>
        <OperationContract()> _
        Function GetDeviceUpdates() As List(Of DeviceUpdate)

    End Interface

End Namespace

