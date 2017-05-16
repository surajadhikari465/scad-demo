
Namespace UniversalHandheldServiceLibrary

    ''' <summary>
    ''' Supported Region Codes
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum Region
        FL
        MA
        MW
        NA
        NC
        NE
        PN
        RM
        SO
        SP
        SW
        EU
        TS
    End Enum

    ''' <summary>
    ''' Configuration Key DataContract Class - Defines the list is key/pair values.
    ''' This contact is returned by the GetConfigurationKeys operation.
    ''' </summary>
    ''' <remarks></remarks>
    <DataContract()> _
    Public Class ConfigurationKey

        ''' <summary>
        ''' appSetting Key name
        ''' </summary>
        ''' <remarks></remarks>
        <DataMember()> _
        Public Key As String

        ''' <summary>
        ''' appSetting Value
        ''' </summary>
        ''' <remarks></remarks>
        <DataMember()> _
        Public Value As String

    End Class

    ''' <summary>
    ''' DeviceUpdate DataContract Class - Defines the DataMembers that represent an update to the
    ''' mobile device applications installed for all regions.
    ''' </summary>
    ''' <remarks></remarks>
    <DataContract()> _
    Public Class DeviceUpdate

        ''' <summary>
        ''' The name of the application being updated.
        ''' </summary>
        <DataMember()> _
        Public UpdateName As String

        ''' <summary>
        ''' The path on the device of the application being updated.
        ''' </summary>
        <DataMember()> _
        Public UpdateDevicePath As String

        ''' <summary>
        ''' Indicates whether the applicaton is updateable using the service.
        ''' </summary>
        <DataMember()> _
        Public UpdateEnabled As Boolean

        ''' <summary>
        ''' The version of the update.
        ''' </summary>
        <DataMember()> _
        Public UpdateVersion As String

        ''' <summary>
        ''' The location of the update file.
        ''' </summary>
        <DataMember()> _
        Public UpdateURI As String

        ''' <summary>
        ''' The filename of the update CAB or DLL assembly.
        ''' </summary>
        ''' <remarks></remarks>
        <DataMember()> _
        Public UpdateFile As String

    End Class

    ''' <summary>
    ''' Application DataContract Class - Defines the DataMembers that represent an application.
    ''' This contract is returned by the GetRegionConfiguration operation.
    ''' </summary>
    ''' <remarks>
    ''' Each application returned represents handheld application plugin.
    ''' Some Plugins have their own application assemblies, others may reference other application executables that already exist on the handheld.
    ''' </remarks>
    <DataContract()> _
    Public Class Plugin

        ''' <summary>
        ''' Indicates whether the application plugin requires domain authentication for use.
        ''' </summary>
        ''' <remarks></remarks>
        <DataMember()> _
        Public AuthenticateUser As Boolean

        ''' <summary>
        ''' The name of the application as it is to appear on the UHA UI screen menu.
        ''' </summary>
        <DataMember()> _
        Public Name As String

        ''' <summary>
        ''' Sets the Enabled/Disabled property of the handheld application's menu item in the UI.
        ''' Regions can configure this value to control what applications the handheld application user is allowed to access in that region.
        ''' </summary>
        <DataMember()> _
        Public IsAuthorized As Boolean

        ''' <summary>
        ''' Distinguishes service-based applications from non-service based.
        ''' </summary>
        <DataMember()> _
        Public HasService As Boolean

        ''' <summary>
        ''' Distinguishes between applications that are accessed via assembly or external executable.
        ''' </summary>
        <DataMember()> _
        Public Type As String

        ''' <summary>
        ''' A long description of the application to appear on UI screens where needed.
        ''' </summary>
        <DataMember()> _
        Public Description As String

        ''' <summary>
        ''' The path to the application executable (value should be null when application is service based).
        ''' </summary>
        <DataMember()> _
        Public ExePath As String

        ''' <summary>
        ''' The version number desination of the external executable.
        ''' </summary>
        <DataMember()> _
        Public ExeVersion As String

        ''' <summary>
        ''' The return value is the address of the WCF services for the application. When IsExecutable is True, this value is null.
        ''' </summary>
        <DataMember()> _
        Public ServicePath As String

        ''' <summary>
        ''' The version number desination of the WCF service.
        ''' </summary>
        <DataMember()> _
        Public ServiceVersion As String

        ''' <summary>
        ''' The description name of the plugin.
        ''' </summary>
        <DataMember()> _
        Public AssemblyName As String

        ''' <summary>
        ''' The file name of the plugin assembly.
        ''' </summary>
        <DataMember()> _
        Public AssemblyFile As String

        ''' <summary>
        ''' The assembly file version.
        ''' </summary>
        <DataMember()> _
        Public AssemblyVersion As String

        ''' <summary>
        ''' The class name of the application's UI main form.
        ''' </summary>
        <DataMember()> _
        Public AssemblyEntryPoint As String

        ''' <summary>
        ''' Indicates whether the plugin is updateable by web service.
        ''' </summary>
        <DataMember()> _
        Public UpdateEnabled As Boolean

        ''' <summary>
        ''' The version of the update.
        ''' </summary>
        <DataMember()> _
        Public UpdateVersion As String

        ''' <summary>
        ''' The location of the update file.
        ''' </summary>
        ''' <remarks>For plugins of type Assembly, this is the path and plugin DLL.
        ''' For updates of type Executable, this is the path CAB deployment package file.</remarks>
        <DataMember()> _
        Public UpdateURI As String

        ''' <summary>
        ''' The filename of the update CAB or DLL assembly.
        ''' </summary>
        ''' <remarks></remarks>
        <DataMember()> _
        Public UpdateFile As String

    End Class

End Namespace